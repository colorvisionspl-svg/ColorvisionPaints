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
    public class ProcurementService : IProcurementService
    {
        private readonly HttpClient _httpClient;
        private readonly string _projectId;

        public ProcurementService(HttpClient httpClient, IOptions<FirebaseConfig> config)
        {
            _httpClient = httpClient;
            _projectId = config.Value.ProjectId;
        }

        private string GetBaseUrl() => $"https://firestore.googleapis.com/v1/projects/{_projectId}/databases/(default)/documents";

        private async Task<HttpResponseMessage> SendWithAuthAsync(HttpMethod method, string url, string idToken, object? payload = null)
        {
            var request = new HttpRequestMessage(method, url);
            if (!string.IsNullOrEmpty(idToken))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", idToken);
            }
            if (payload != null)
            {
                request.Content = JsonContent.Create(payload);
            }
            return await _httpClient.SendAsync(request);
        }

        /* ═══════════════════════════════════════════════════════════
           VENDORS
           ═══════════════════════════════════════════════════════════ */
        public async Task<List<Vendor>> GetVendorsAsync(string idToken)
        {
            var url = $"{GetBaseUrl()}/vendors";
            var response = await SendWithAuthAsync(HttpMethod.Get, url, idToken);
            if (!response.IsSuccessStatusCode) return new List<Vendor>();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<FirestoreQueryResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            return result?.Documents?.Select(d => MapToVendor(d)).ToList() ?? new List<Vendor>();
        }

        public async Task<Vendor?> GetVendorAsync(string id, string idToken)
        {
            var url = $"{GetBaseUrl()}/vendors/{id}";
            var response = await SendWithAuthAsync(HttpMethod.Get, url, idToken);
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var doc = JsonSerializer.Deserialize<FirestoreDocument>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return MapToVendor(doc!);
        }

        public async Task CreateVendorAsync(Vendor vendor, string idToken)
        {
            var documentId = Guid.NewGuid().ToString();
            var url = $"{GetBaseUrl()}/vendors?documentId={documentId}";
            var payload = new { fields = MapVendorToFields(vendor) };
            await SendWithAuthAsync(HttpMethod.Post, url, idToken, payload);
        }

        public async Task UpdateVendorAsync(string id, Vendor vendor, string idToken)
        {
            var url = $"{GetBaseUrl()}/vendors/{id}";
            var payload = new { fields = MapVendorToFields(vendor) };
            await SendWithAuthAsync(new HttpMethod("PATCH"), url, idToken, payload);
        }

        /* ═══════════════════════════════════════════════════════════
           MAPPING HELPERS (Simplified for brevity in MVP)
           ═══════════════════════════════════════════════════════════ */
        private Vendor MapToVendor(FirestoreDocument doc)
        {
            var f = doc.Fields;
            var id = doc.Name.Split('/').Last();
            return new Vendor
            {
                Id = id,
                VendorCode = f.GetValueOrDefault("vendorCode")?.StringValue ?? "",
                CompanyName = f.GetValueOrDefault("companyName")?.StringValue ?? "",
                ContactPerson = f.GetValueOrDefault("contactPerson")?.StringValue ?? "",
                Mobile = f.GetValueOrDefault("mobile")?.StringValue ?? "",
                Email = f.GetValueOrDefault("email")?.StringValue ?? "",
                City = f.GetValueOrDefault("city")?.StringValue ?? "",
                QualityRating = (int)(f.GetValueOrDefault("qualityRating")?.IntegerValue ?? 0),
                IsActive = f.GetValueOrDefault("isActive")?.BooleanValue ?? true
            };
        }

        private object MapVendorToFields(Vendor v) => new
        {
            vendorCode = new { stringValue = v.VendorCode },
            companyName = new { stringValue = v.CompanyName },
            contactPerson = new { stringValue = v.ContactPerson },
            mobile = new { stringValue = v.Mobile },
            email = new { stringValue = v.Email },
            gstNumber = new { stringValue = v.GstNumber },
            address = new { stringValue = v.Address },
            city = new { stringValue = v.City },
            state = new { stringValue = v.State },
            paymentTermsDays = new { integerValue = v.PaymentTermsDays },
            qualityRating = new { integerValue = v.QualityRating },
            isActive = new { booleanValue = v.IsActive },
            createdAt = new { timestampValue = v.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ") }
        };

        /* ═══════════════════════════════════════════════════════════
           SEQUENTIAL ID GENERATOR
           ═══════════════════════════════════════════════════════════ */
        public async Task<string> GetNextSequenceIdAsync(string collection, string prefix, string idToken)
        {
            // Simple logic: count existing docs and add 1
            // In production, use a dedicated counter document with atomic increments
            var url = $"{GetBaseUrl()}/{collection}?pageSize=1000";
            var response = await SendWithAuthAsync(HttpMethod.Get, url, idToken);
            var count = 1;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<FirestoreQueryResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                count = (result?.Documents?.Count ?? 0) + 1;
            }
            return $"{prefix}-{count:D4}";
        }

        /* ═══════════════════════════════════════════════════════════
           STUBS FOR OTHER COLLECTIONS (To be implemented as needed)
           ═══════════════════════════════════════════════════════════ */
        public async Task<List<RawMaterial>> GetRawMaterialsAsync(string idToken) => new();
        public async Task CreateRawMaterialAsync(RawMaterial m, string t) => await Task.CompletedTask;
        public async Task<List<PurchaseOrder>> GetPurchaseOrdersAsync(string t) => new();
        public async Task<PurchaseOrder?> GetPurchaseOrderAsync(string i, string t) => null;
        public async Task CreatePurchaseOrderAsync(PurchaseOrder p, string t) => await Task.CompletedTask;
        public async Task UpdatePurchaseOrderStatusAsync(string i, string s, string t) => await Task.CompletedTask;
        public async Task<List<GRN>> GetGRNsAsync(string t) => new();
        public async Task CreateGRNAsync(GRN g, string t) => await Task.CompletedTask;
        public async Task UpdateGRNStatusAsync(string i, string s, string t) => await Task.CompletedTask;
        public async Task<List<PurchaseBill>> GetBillsAsync(string t) => new();
        public async Task CreateBillAsync(PurchaseBill b, string t) => await Task.CompletedTask;
        public async Task<List<RawMaterialStock>> GetStockAsync(string t) => new();
        public async Task UpdateStockStatusAsync(string i, string s, string t) => await Task.CompletedTask;

        private class FirestoreDocument { 
            public string Name { get; set; } = "";
            public Dictionary<string, FirestoreValue> Fields { get; set; } = new(); 
        }
        private class FirestoreQueryResponse { public List<FirestoreDocument>? Documents { get; set; } }
        private class FirestoreValue {
            public string? StringValue { get; set; }
            public bool? BooleanValue { get; set; }
            public long? IntegerValue { get; set; }
            public string? TimestampValue { get; set; }
        }
    }
}
