using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ColorvisionPaintsERP.Models;
using System.Linq;

namespace ColorvisionPaintsERP.Services
{
    public class ManufacturingService : IManufacturingService
    {
        private readonly HttpClient _http;
        private readonly string _projectId;

        // Private nested types (not file-scoped, so no CS9051)
        private class FsDoc
        {
            public string Name { get; set; } = "";
            public Dictionary<string, FsVal> Fields { get; set; } = new();
        }
        private class FsQueryResp { public List<FsDoc>? Documents { get; set; } }
        private class FsVal
        {
            public string?  StringValue    { get; set; }
            public bool?    BooleanValue   { get; set; }
            public long?    IntegerValue   { get; set; }
            public double?  DoubleValue    { get; set; }
            public string?  TimestampValue { get; set; }
        }

        // Field reader helpers
        private static string  S(Dictionary<string,FsVal> f, string k, string d="")  => f.TryGetValue(k,out var v) ? v.StringValue??d : d;
        private static long    I(Dictionary<string,FsVal> f, string k, long d=0)     => f.TryGetValue(k,out var v) ? v.IntegerValue??d : d;
        private static double  D(Dictionary<string,FsVal> f, string k, double d=0)   => f.TryGetValue(k,out var v) ? v.DoubleValue??d : d;
        private static DateTime T(Dictionary<string,FsVal> f, string k)              => f.TryGetValue(k,out var v) && DateTime.TryParse(v.TimestampValue, out var dt) ? dt : DateTime.UtcNow;

        public ManufacturingService(HttpClient http, IOptions<FirebaseConfig> cfg)
        {
            _http = http;
            _projectId = cfg.Value.ProjectId;
        }

        private string Base => $"https://firestore.googleapis.com/v1/projects/{_projectId}/databases/(default)/documents";

        private async Task<HttpResponseMessage> Req(HttpMethod method, string url, string token, object? body = null)
        {
            var req = new HttpRequestMessage(method, url);
            if (!string.IsNullOrEmpty(token))
                req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            if (body != null) req.Content = JsonContent.Create(body);
            return await _http.SendAsync(req);
        }

        private async Task<List<FsDoc>> GetDocs(string collection, string token, int pageSize = 200)
        {
            var r = await Req(HttpMethod.Get, $"{Base}/{collection}?pageSize={pageSize}", token);
            if (!r.IsSuccessStatusCode) return new();
            var c = await r.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FsQueryResp>(c, new JsonSerializerOptions{PropertyNameCaseInsensitive=true})?.Documents ?? new();
        }

        /* ── Sequential ID ── */
        public async Task<string> GetNextSequenceIdAsync(string collection, string prefix, string token)
        {
            var docs = await GetDocs(collection, token, 1000);
            return $"{prefix}-{(docs.Count+1):D4}";
        }

        /* ════════════════ FORMULATIONS ════════════════ */
        public async Task<List<Formulation>> GetFormulationsAsync(string t) =>
            (await GetDocs("formulations", t)).Select(MapForm).ToList();

        public async Task<Formulation?> GetFormulationAsync(string id, string t)
        {
            var r = await Req(HttpMethod.Get, $"{Base}/formulations/{id}", t);
            if (!r.IsSuccessStatusCode) return null;
            return MapForm(JsonSerializer.Deserialize<FsDoc>(await r.Content.ReadAsStringAsync(),
                new JsonSerializerOptions{PropertyNameCaseInsensitive=true})!);
        }

        public async Task<string> CreateFormulationAsync(Formulation f, string t)
        {
            var id = Guid.NewGuid().ToString();
            await Req(HttpMethod.Post, $"{Base}/formulations?documentId={id}", t, new { fields = FormFields(f) });
            return id;
        }

        public async Task UpdateFormulationAsync(string id, Formulation f, string t) =>
            await Req(new HttpMethod("PATCH"), $"{Base}/formulations/{id}", t, new { fields = FormFields(f) });

        private Formulation MapForm(FsDoc d)
        {
            var f = d.Fields;
            return new Formulation {
                Id=d.Name.Split('/').Last(), FormulationCode=S(f,"formulationCode"),
                ProductId=S(f,"productId"), ProductName=S(f,"productName"),
                ShadeId=S(f,"shadeId"), ShadeName=S(f,"shadeName"),
                Version=(int)I(f,"version",1), StandardBatchSizeKg=D(f,"standardBatchSizeKg",1000),
                ExpectedYieldPercent=D(f,"expectedYieldPercent",96), ProcessingTimeHours=D(f,"processingTimeHours"),
                Status=S(f,"status",FormulationStatus.Draft), ApprovedBy=S(f,"approvedBy"),
                CreatedBy=S(f,"createdBy"), CreatedAt=T(f,"createdAt"), UpdatedAt=T(f,"updatedAt"),
                Ingredients=new(), ProcessSteps=new() };
        }

        private object FormFields(Formulation f) => new {
            formulationCode      = new{stringValue=f.FormulationCode},
            productId            = new{stringValue=f.ProductId},
            productName          = new{stringValue=f.ProductName},
            shadeId              = new{stringValue=f.ShadeId},
            shadeName            = new{stringValue=f.ShadeName},
            version              = new{integerValue=f.Version},
            standardBatchSizeKg  = new{doubleValue=f.StandardBatchSizeKg},
            expectedYieldPercent = new{doubleValue=f.ExpectedYieldPercent},
            processingTimeHours  = new{doubleValue=f.ProcessingTimeHours},
            status               = new{stringValue=f.Status},
            createdBy            = new{stringValue=f.CreatedBy},
            createdAt            = new{timestampValue=f.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ")},
            updatedAt            = new{timestampValue=DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")},
        };

        /* ════════════════ PRODUCTION ORDERS ════════════════ */
        public async Task<List<ProductionOrder>> GetProductionOrdersAsync(string t) =>
            (await GetDocs("production_orders", t)).Select(MapOrder).ToList();

        public async Task<ProductionOrder?> GetProductionOrderAsync(string id, string t)
        {
            var r = await Req(HttpMethod.Get, $"{Base}/production_orders/{id}", t);
            if (!r.IsSuccessStatusCode) return null;
            return MapOrder(JsonSerializer.Deserialize<FsDoc>(await r.Content.ReadAsStringAsync(),
                new JsonSerializerOptions{PropertyNameCaseInsensitive=true})!);
        }

        public async Task<string> CreateProductionOrderAsync(ProductionOrder o, string t)
        {
            var id = Guid.NewGuid().ToString();
            await Req(HttpMethod.Post, $"{Base}/production_orders?documentId={id}", t, new { fields = OrderFields(o) });
            return id;
        }

        public async Task UpdateOrderStatusAsync(string id, string status, string t)
        {
            var url = $"{Base}/production_orders/{id}?updateMask.fieldPaths=status";
            await Req(new HttpMethod("PATCH"), url, t, new { fields = new { status = new{stringValue=status} } });
        }

        private ProductionOrder MapOrder(FsDoc d)
        {
            var f = d.Fields;
            return new ProductionOrder {
                Id=d.Name.Split('/').Last(), OrderNumber=S(f,"orderNumber"),
                ProductVariantId=S(f,"productVariantId"), ProductName=S(f,"productName"),
                ShadeName=S(f,"shadeName"), FormulationId=S(f,"formulationId"),
                FormulationVersion=(int)I(f,"formulationVersion",1),
                PlannedQuantityKg=D(f,"plannedQuantityKg"), PlannedBatches=(int)I(f,"plannedBatches"),
                ProductionLineId=S(f,"productionLineId"), ProductionLineName=S(f,"productionLineName"),
                Status=S(f,"status",ProductionOrderStatus.Planned),
                PlannedStartDate=T(f,"plannedStartDate"), PlannedEndDate=T(f,"plannedEndDate"),
                CreatedBy=S(f,"createdBy"), CreatedAt=T(f,"createdAt") };
        }

        private object OrderFields(ProductionOrder o) => new {
            orderNumber        = new{stringValue=o.OrderNumber},
            productVariantId   = new{stringValue=o.ProductVariantId},
            productName        = new{stringValue=o.ProductName},
            shadeName          = new{stringValue=o.ShadeName},
            formulationId      = new{stringValue=o.FormulationId},
            formulationVersion = new{integerValue=o.FormulationVersion},
            plannedQuantityKg  = new{doubleValue=o.PlannedQuantityKg},
            plannedBatches     = new{integerValue=o.PlannedBatches},
            productionLineId   = new{stringValue=o.ProductionLineId},
            productionLineName = new{stringValue=o.ProductionLineName},
            status             = new{stringValue=o.Status},
            plannedStartDate   = new{timestampValue=o.PlannedStartDate.ToString("yyyy-MM-ddTHH:mm:ssZ")},
            plannedEndDate     = new{timestampValue=o.PlannedEndDate.ToString("yyyy-MM-ddTHH:mm:ssZ")},
            createdBy          = new{stringValue=o.CreatedBy},
            createdAt          = new{timestampValue=o.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ")},
        };

        /* ════════════════ BATCHES ════════════════ */
        public async Task<List<ProductionBatch>> GetBatchesForOrderAsync(string orderId, string t)
        {
            var docs = await GetDocs("production_batches", t, 500);
            return docs.Select(MapBatch).Where(b=>b.ProductionOrderId==orderId).ToList();
        }

        public async Task<string> CreateBatchAsync(ProductionBatch b, string t)
        {
            var id = Guid.NewGuid().ToString();
            await Req(HttpMethod.Post, $"{Base}/production_batches?documentId={id}", t, new { fields = BatchFields(b) });
            return id;
        }

        public async Task UpdateBatchAsync(string id, ProductionBatch b, string t) =>
            await Req(new HttpMethod("PATCH"), $"{Base}/production_batches/{id}", t, new { fields = BatchFields(b) });

        private ProductionBatch MapBatch(FsDoc d)
        {
            var f = d.Fields;
            return new ProductionBatch {
                Id=d.Name.Split('/').Last(), ProductionOrderId=S(f,"productionOrderId"),
                BatchNumber=S(f,"batchNumber"), BatchSizeKg=D(f,"batchSizeKg"),
                ProductionLineId=S(f,"productionLineId"), OperatorId=S(f,"operatorId"),
                Status=S(f,"status",BatchStatus.Pending), StartTime=T(f,"startTime"),
                YieldKg=D(f,"yieldKg"), WasteKg=D(f,"wasteKg"),
                ProductName=S(f,"productName"), CreatedAt=T(f,"createdAt") };
        }

        private object BatchFields(ProductionBatch b) => new {
            productionOrderId=new{stringValue=b.ProductionOrderId},
            batchNumber      =new{stringValue=b.BatchNumber},
            batchSizeKg      =new{doubleValue=b.BatchSizeKg},
            productionLineId =new{stringValue=b.ProductionLineId},
            operatorId       =new{stringValue=b.OperatorId},
            status           =new{stringValue=b.Status},
            startTime        =new{timestampValue=b.StartTime.ToString("yyyy-MM-ddTHH:mm:ssZ")},
            yieldKg          =new{doubleValue=b.YieldKg},
            wasteKg          =new{doubleValue=b.WasteKg},
            productName      =new{stringValue=b.ProductName},
            createdAt        =new{timestampValue=b.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ")},
        };

        /* ════════════════ QC INSPECTIONS ════════════════ */
        public async Task<List<QCInspection>> GetQCInspectionsAsync(string t) =>
            (await GetDocs("qc_inspections", t)).Select(MapQC).ToList();

        public async Task<QCInspection?> GetQCInspectionAsync(string id, string t)
        {
            var r = await Req(HttpMethod.Get, $"{Base}/qc_inspections/{id}", t);
            if (!r.IsSuccessStatusCode) return null;
            return MapQC(JsonSerializer.Deserialize<FsDoc>(await r.Content.ReadAsStringAsync(),
                new JsonSerializerOptions{PropertyNameCaseInsensitive=true})!);
        }

        public async Task<string> CreateQCInspectionAsync(QCInspection i, string t)
        {
            var id = Guid.NewGuid().ToString();
            await Req(HttpMethod.Post, $"{Base}/qc_inspections?documentId={id}", t,
                new { fields = new {
                    batchId=new{stringValue=i.BatchId}, batchNumber=new{stringValue=i.BatchNumber},
                    productName=new{stringValue=i.ProductName}, shadeName=new{stringValue=i.ShadeName},
                    yieldKg=new{doubleValue=i.YieldKg}, overallResult=new{stringValue=""},
                    productionOrderId=new{stringValue=i.ProductionOrderId},
                    createdAt=new{timestampValue=DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")} }});
            return id;
        }

        public async Task UpdateQCInspectionAsync(string id, QCInspection i, string t) =>
            await Req(new HttpMethod("PATCH"), $"{Base}/qc_inspections/{id}", t,
                new { fields = new {
                    overallResult=new{stringValue=i.OverallResult},
                    remarks=new{stringValue=i.Remarks},
                    inspectorName=new{stringValue=i.InspectorName},
                    coaUrl=new{stringValue=i.CoaUrl} }});

        private QCInspection MapQC(FsDoc d)
        {
            var f = d.Fields;
            return new QCInspection {
                Id=d.Name.Split('/').Last(), BatchId=S(f,"batchId"),
                ProductionOrderId=S(f,"productionOrderId"), BatchNumber=S(f,"batchNumber"),
                ProductName=S(f,"productName"), ShadeName=S(f,"shadeName"),
                YieldKg=D(f,"yieldKg"), InspectorId=S(f,"inspectorId"),
                InspectorName=S(f,"inspectorName"), OverallResult=S(f,"overallResult"),
                Remarks=S(f,"remarks"), CoaUrl=S(f,"coaUrl"), CreatedAt=T(f,"createdAt"),
                TestResults=new() };
        }

        /* ════════════════ PRODUCTION LINES ════════════════ */
        public async Task<List<ProductionLine>> GetProductionLinesAsync(string t) =>
            (await GetDocs("production_lines", t, 50)).Select(MapLine).ToList();

        public async Task<string> CreateProductionLineAsync(ProductionLine l, string t)
        {
            var id = Guid.NewGuid().ToString();
            await Req(HttpMethod.Post, $"{Base}/production_lines?documentId={id}", t, new { fields = LineFields(l) });
            return id;
        }

        public async Task UpdateProductionLineAsync(string id, ProductionLine l, string t) =>
            await Req(new HttpMethod("PATCH"), $"{Base}/production_lines/{id}", t, new { fields = LineFields(l) });

        private ProductionLine MapLine(FsDoc d)
        {
            var f = d.Fields;
            return new ProductionLine {
                Id=d.Name.Split('/').Last(), Name=S(f,"name"),
                CapacityKgPerDay=D(f,"capacityKgPerDay",5000),
                Status=S(f,"status",LineStatus.Idle),
                CurrentBatchId=S(f,"currentBatchId"), Notes=S(f,"notes") };
        }

        private object LineFields(ProductionLine l) => new {
            name            =new{stringValue=l.Name},
            capacityKgPerDay=new{doubleValue=l.CapacityKgPerDay},
            status          =new{stringValue=l.Status},
            notes           =new{stringValue=l.Notes},
        };
    }
}
