using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ColorvisionPaintsERP.Models;
using System;

namespace ColorvisionPaintsERP.Services
{
    public class FirestoreService : IFirestoreService
    {
        private readonly HttpClient _httpClient;
        private readonly string _projectId;

        public FirestoreService(HttpClient httpClient, IOptions<FirebaseConfig> config)
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

        public async Task<UserProfile?> GetUserProfileAsync(string uid, string idToken)
        {
            var url = $"{GetBaseUrl()}/users/{uid}";
            var response = await SendWithAuthAsync(HttpMethod.Get, url, idToken);
            
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var doc = JsonSerializer.Deserialize<FirestoreDocument>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            return MapToUserProfile(uid, doc?.Fields);
        }

        public async Task CreateUserProfileAsync(string uid, UserProfile profile, string idToken)
        {
            // First check if this is the first user overall. If it is, force them to SuperAdmin.
            var isFirstUser = await IsFirstUserAsync(idToken);
            if (isFirstUser)
            {
                profile.Role = UserRoles.SuperAdmin;
                await SeedRolePermissionsAsync(idToken); // Initialize roles on first SuperAdmin signup
            }

            var url = $"{GetBaseUrl()}/users?documentId={uid}";
            var payload = new
            {
                fields = new
                {
                    uid = new { stringValue = uid },
                    name = new { stringValue = profile.Name },
                    email = new { stringValue = profile.Email },
                    mobile = new { stringValue = profile.Mobile },
                    role = new { stringValue = profile.Role },
                    territoryId = new { stringValue = profile.TerritoryId },
                    reportingTo = new { stringValue = profile.ReportingTo },
                    isActive = new { booleanValue = profile.IsActive }
                }
            };

            await SendWithAuthAsync(HttpMethod.Post, url, idToken, payload);
        }

        public async Task<RolePermissions> GetRolePermissionsAsync(string role, string idToken)
        {
            var url = $"{GetBaseUrl()}/roles_permissions/{role}";
            var response = await SendWithAuthAsync(HttpMethod.Get, url, idToken);
            
            if (!response.IsSuccessStatusCode)
                return new RolePermissions(); // Default empty if not found

            var content = await response.Content.ReadAsStringAsync();
            var doc = JsonSerializer.Deserialize<FirestoreDocument>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            return MapToRolePermissions(doc?.Fields);
        }

        public async Task<bool> IsFirstUserAsync(string idToken)
        {
            var url = $"{GetBaseUrl()}/users?pageSize=1";
            var response = await SendWithAuthAsync(HttpMethod.Get, url, idToken);
            if (!response.IsSuccessStatusCode)
                return true; // Default to true if collection read fails/doesn't exist
                
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<FirestoreQueryResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            return result?.Documents == null || result.Documents.Count == 0;
        }

        public async Task SeedRolePermissionsAsync(string idToken)
        {
            var roles = UserRoles.All;
            foreach(var role in roles)
            {
                // Simple default generation based on role logic
                var permissions = new RolePermissions();
                
                if (role == UserRoles.SuperAdmin)
                {
                    permissions.Dealers = new ModulePermission { View = true, Create = true, Edit = true, Delete = true, Approve = true };
                    permissions.Sales = new ModulePermission { View = true, Create = true, Edit = true, Delete = true, Approve = true };
                    permissions.Inventory = new ModulePermission { View = true, Create = true, Edit = true, Approve = true };
                    // Set everything else to true for SuperAdmin
                }

                var url = $"{GetBaseUrl()}/roles_permissions?documentId={role}";
                var payload = new
                {
                    fields = new
                    {
                        seeded = new { booleanValue = true }
                    }
                };

                await SendWithAuthAsync(HttpMethod.Post, url, idToken, payload);
            }
        }

        // Helpers to map Firestore complex document structure
        private UserProfile MapToUserProfile(string uid, Dictionary<string, FirestoreValue>? fields)
        {
            if (fields == null) return new UserProfile { Uid = uid };

            return new UserProfile
            {
                Uid = uid,
                Name = fields.GetValueOrDefault("name")?.StringValue ?? "New User",
                Email = fields.GetValueOrDefault("email")?.StringValue ?? "",
                Mobile = fields.GetValueOrDefault("mobile")?.StringValue ?? "",
                Role = fields.GetValueOrDefault("role")?.StringValue ?? "DealerPortalUser",
                TerritoryId = fields.GetValueOrDefault("territoryId")?.StringValue ?? "",
                ReportingTo = fields.GetValueOrDefault("reportingTo")?.StringValue ?? "",
                IsActive = fields.GetValueOrDefault("isActive")?.BooleanValue ?? true
            };
        }

        private RolePermissions MapToRolePermissions(Dictionary<string, FirestoreValue>? fields)
        {
            return new RolePermissions();
        }

        // Subclasses for deserializing Google Firestore REST responses
        private class FirestoreDocument
        {
            public Dictionary<string, FirestoreValue> Fields { get; set; } = new();
        }

        private class FirestoreQueryResponse
        {
            public List<FirestoreDocument> Documents { get; set; } = new();
        }

        private class FirestoreValue
        {
            public string? StringValue { get; set; }
            public bool? BooleanValue { get; set; }
            public Dictionary<string, FirestoreValue>? MapValue { get; set; }
        }
    }
}
