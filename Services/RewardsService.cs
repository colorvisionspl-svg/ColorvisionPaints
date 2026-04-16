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
    public class RewardsService : IRewardsService
    {
        private readonly HttpClient _http;
        private readonly string _projectId;

        private class FsDoc { public string Name{get;set;}=""; public Dictionary<string,FsVal> Fields{get;set;}=new(); }
        private class FsQ   { public List<FsDoc>? Documents{get;set;} }
        private class FsVal { public string? StringValue{get;set;} public bool? BooleanValue{get;set;} public long? IntegerValue{get;set;} public double? DoubleValue{get;set;} public string? TimestampValue{get;set;} }

        private static string  Str(Dictionary<string,FsVal> f,string k,string d="") => f.TryGetValue(k,out var v)?v.StringValue??d:d;
        private static long    Int(Dictionary<string,FsVal> f,string k,long d=0)    => f.TryGetValue(k,out var v)?v.IntegerValue??d:d;
        private static double  Dbl(Dictionary<string,FsVal> f,string k,double d=0)  => f.TryGetValue(k,out var v)?v.DoubleValue??d:d;
        private static bool    Boo(Dictionary<string,FsVal> f,string k)             => f.TryGetValue(k,out var v)&&(v.BooleanValue??false);
        private static DateTime Ts(Dictionary<string,FsVal> f,string k)             => f.TryGetValue(k,out var v)&&DateTime.TryParse(v.TimestampValue,out var d)?d:DateTime.UtcNow;

        public RewardsService(HttpClient http, IOptions<FirebaseConfig> cfg) { _http=http; _projectId=cfg.Value.ProjectId; }

        private string Base => $"https://firestore.googleapis.com/v1/projects/{_projectId}/databases/(default)/documents";

        private async Task<HttpResponseMessage> Req(HttpMethod m, string url, string token, object? body=null)
        {
            var req = new HttpRequestMessage(m, url);
            if (!string.IsNullOrEmpty(token)) req.Headers.Authorization=new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
            if (body!=null) req.Content=JsonContent.Create(body);
            return await _http.SendAsync(req);
        }

        private async Task<List<FsDoc>> GetDocs(string col, string token, int ps=200)
        {
            var r=await Req(HttpMethod.Get,$"{Base}/{col}?pageSize={ps}",token);
            if(!r.IsSuccessStatusCode) return new();
            return JsonSerializer.Deserialize<FsQ>(await r.Content.ReadAsStringAsync(),new JsonSerializerOptions{PropertyNameCaseInsensitive=true})?.Documents??new();
        }

        /* ── QR CODES ── */
        public async Task<List<QrCode>> GetQrCodesAsync(string t, int ps=200) =>
            (await GetDocs("qr_codes",t,ps)).Select(MapQr).ToList();

        public async Task<QrCode?> GetQrCodeAsync(string serial, string t)
        {
            var r=await Req(HttpMethod.Get,$"{Base}/qr_codes/{serial}",t);
            if(!r.IsSuccessStatusCode) return null;
            return MapQr(JsonSerializer.Deserialize<FsDoc>(await r.Content.ReadAsStringAsync(),new JsonSerializerOptions{PropertyNameCaseInsensitive=true})!);
        }

        public async Task CreateQrCodesAsync(string pvId, string pName, string batch, DateTime mfg, int qty, int pts, string t)
        {
            var docs=(await GetDocs("qr_codes",t,10000));
            var next=docs.Count+1;
            var expiry=mfg.AddDays(365);
            for(int i=0;i<qty;i++)
            {
                var serial=$"CV-QR-{(next+i):D8}";
                var payload=new{fields=new{
                    serialNumber=new{stringValue=serial},
                    productVariantId=new{stringValue=pvId},
                    productName=new{stringValue=pName},
                    batchNumber=new{stringValue=batch},
                    manufacturingDate=new{timestampValue=mfg.ToString("yyyy-MM-ddTHH:mm:ssZ")},
                    pointValue=new{integerValue=pts},
                    status=new{stringValue=QRStatus.Unscanned},
                    createdAt=new{timestampValue=DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")},
                    expiryDate=new{timestampValue=expiry.ToString("yyyy-MM-ddTHH:mm:ssZ")}
                }};
                await Req(HttpMethod.Post,$"{Base}/qr_codes?documentId={serial}",t,payload);
            }
        }

        public async Task VoidQrCodeAsync(string serial, string t) =>
            await Req(new HttpMethod("PATCH"),$"{Base}/qr_codes/{serial}?updateMask.fieldPaths=status",t,
                new{fields=new{status=new{stringValue=QRStatus.Voided}}});

        private QrCode MapQr(FsDoc d){var f=d.Fields; return new QrCode{
            SerialNumber=Str(f,"serialNumber"), ProductVariantId=Str(f,"productVariantId"),
            ProductName=Str(f,"productName"), BatchNumber=Str(f,"batchNumber"),
            ManufacturingDate=Ts(f,"manufacturingDate"), PointValue=(int)Int(f,"pointValue"),
            Status=Str(f,"status",QRStatus.Unscanned), ScannedByPainterId=Str(f,"scannedByPainterId"),
            ScanCity=Str(f,"scanCity"), ScanState=Str(f,"scanState"),
            CreatedAt=Ts(f,"createdAt"), ExpiryDate=Ts(f,"expiryDate") };}

        public async Task<string> GetNextQrSerialAsync(string t)
        {
            var docs=await GetDocs("qr_codes",t,10000);
            return $"CV-QR-{(docs.Count+1):D8}";
        }

        /* ── PAINTERS ── */
        public async Task<List<Painter>> GetPaintersAsync(string t) =>
            (await GetDocs("painters",t)).Select(MapPainter).ToList();

        public async Task<Painter?> GetPainterAsync(string id, string t)
        {
            var r=await Req(HttpMethod.Get,$"{Base}/painters/{id}",t);
            if(!r.IsSuccessStatusCode) return null;
            return MapPainter(JsonSerializer.Deserialize<FsDoc>(await r.Content.ReadAsStringAsync(),new JsonSerializerOptions{PropertyNameCaseInsensitive=true})!);
        }

        public async Task BlockPainterAsync(string id, string reason, string t) =>
            await Req(new HttpMethod("PATCH"),$"{Base}/painters/{id}?updateMask.fieldPaths=isBlocked&updateMask.fieldPaths=blockReason",t,
                new{fields=new{isBlocked=new{booleanValue=true},blockReason=new{stringValue=reason}}});

        public async Task UnblockPainterAsync(string id, string t) =>
            await Req(new HttpMethod("PATCH"),$"{Base}/painters/{id}?updateMask.fieldPaths=isBlocked&updateMask.fieldPaths=blockReason",t,
                new{fields=new{isBlocked=new{booleanValue=false},blockReason=new{stringValue=""}}});

        private Painter MapPainter(FsDoc d){var f=d.Fields; return new Painter{
            Id=d.Name.Split('/').Last(), Mobile=Str(f,"mobile"), Name=Str(f,"name"),
            City=Str(f,"city"), State=Str(f,"state"), AadhaarLast4=Str(f,"aadhaarLast4"),
            UpiId=Str(f,"upiId"), Tier=Str(f,"tier",PainterTier.Bronze),
            TotalPointsEarned=Dbl(f,"totalPointsEarned"), TotalPointsRedeemed=Dbl(f,"totalPointsRedeemed"),
            CurrentPoints=Dbl(f,"currentPoints"), RegisteredAt=Ts(f,"registeredAt"),
            IsVerified=Boo(f,"isVerified"), IsBlocked=Boo(f,"isBlocked"), BlockReason=Str(f,"blockReason") };}

        /* ── TRANSACTIONS ── */
        public async Task<List<PainterTransaction>> GetTransactionsAsync(string t) =>
            (await GetDocs("painter_transactions",t)).Select(MapTxn).ToList();

        public async Task<List<PainterTransaction>> GetTransactionsByPainterAsync(string pid, string t) =>
            (await GetTransactionsAsync(t)).Where(x=>x.PainterId==pid).ToList();

        public async Task ApproveRedemptionAsync(string id, string t) =>
            await Req(new HttpMethod("PATCH"),$"{Base}/painter_transactions/{id}?updateMask.fieldPaths=status",t,
                new{fields=new{status=new{stringValue=TxnStatus.Processing}}});

        public async Task FailRedemptionAsync(string id, string reason, string t) =>
            await Req(new HttpMethod("PATCH"),$"{Base}/painter_transactions/{id}?updateMask.fieldPaths=status",t,
                new{fields=new{status=new{stringValue=TxnStatus.Failed}}});

        private PainterTransaction MapTxn(FsDoc d){var f=d.Fields; return new PainterTransaction{
            Id=d.Name.Split('/').Last(), PainterId=Str(f,"painterId"), PainterName=Str(f,"painterName"),
            Type=Str(f,"type",TxnType.Earn), Points=Dbl(f,"points"), Amount=Dbl(f,"amount"),
            QrCodeSerial=Str(f,"qrCodeSerial"), ProductName=Str(f,"productName"),
            RedemptionType=Str(f,"redemptionType"), UpiId=Str(f,"upiId"),
            UpiTransactionId=Str(f,"upiTransactionId"), TdsDeducted=Dbl(f,"tdsDeducted"),
            Status=Str(f,"status",TxnStatus.Pending), CreatedAt=Ts(f,"createdAt")};}

        /* ── CAMPAIGNS ── */
        public async Task<List<RewardCampaign>> GetCampaignsAsync(string t) =>
            (await GetDocs("reward_campaigns",t)).Select(MapCampaign).ToList();

        public async Task<string> CreateCampaignAsync(RewardCampaign c, string t)
        {
            var id=Guid.NewGuid().ToString();
            await Req(HttpMethod.Post,$"{Base}/reward_campaigns?documentId={id}",t,new{fields=new{
                name=new{stringValue=c.Name}, description=new{stringValue=c.Description},
                startDate=new{timestampValue=c.StartDate.ToString("yyyy-MM-ddTHH:mm:ssZ")},
                endDate=new{timestampValue=c.EndDate.ToString("yyyy-MM-ddTHH:mm:ssZ")},
                bonusMultiplier=new{doubleValue=c.BonusMultiplier},
                bonusPoints=new{integerValue=c.BonusPoints},
                isActive=new{booleanValue=c.IsActive},
                createdAt=new{timestampValue=DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")}
            }});
            return id;
        }

        public async Task ToggleCampaignAsync(string id, bool active, string t) =>
            await Req(new HttpMethod("PATCH"),$"{Base}/reward_campaigns/{id}?updateMask.fieldPaths=isActive",t,
                new{fields=new{isActive=new{booleanValue=active}}});

        private RewardCampaign MapCampaign(FsDoc d){var f=d.Fields; return new RewardCampaign{
            Id=d.Name.Split('/').Last(), Name=Str(f,"name"), Description=Str(f,"description"),
            StartDate=Ts(f,"startDate"), EndDate=Ts(f,"endDate"),
            BonusMultiplier=Dbl(f,"bonusMultiplier",1), BonusPoints=(int)Int(f,"bonusPoints"),
            IsActive=Boo(f,"isActive"), CreatedAt=Ts(f,"createdAt")};}

        /* ── FRAUD FLAGS ── */
        public async Task<List<FraudFlag>> GetFraudFlagsAsync(string t) =>
            (await GetDocs("fraud_flags",t)).Select(MapFlag).ToList();

        public async Task UpdateFlagStatusAsync(string id, string status, string reviewer, string t) =>
            await Req(new HttpMethod("PATCH"),$"{Base}/fraud_flags/{id}?updateMask.fieldPaths=status&updateMask.fieldPaths=reviewedBy",t,
                new{fields=new{status=new{stringValue=status},reviewedBy=new{stringValue=reviewer}}});

        private FraudFlag MapFlag(FsDoc d){var f=d.Fields; return new FraudFlag{
            Id=d.Name.Split('/').Last(), Type=Str(f,"type"), PainterId=Str(f,"painterId"),
            PainterName=Str(f,"painterName"), DeviceFingerprint=Str(f,"deviceFingerprint"),
            ScanCount=(int)Int(f,"scanCount"), TimeWindow=Str(f,"timeWindow"),
            GpsCoordinates=Str(f,"gpsCoordinates"), FlaggedAt=Ts(f,"flaggedAt"),
            Status=Str(f,"status",FlagStatus.Pending), ReviewedBy=Str(f,"reviewedBy")};}
    }
}
