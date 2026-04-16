using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ColorvisionPaintsERP.Models;

namespace ColorvisionPaintsERP.Services
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public FirebaseAuthService(HttpClient httpClient, IOptions<FirebaseConfig> config)
        {
            _httpClient = httpClient;
            _apiKey = config.Value.ApiKey;
        }

        public async Task<FirebaseAuthResponse> LoginWithEmailAsync(string email, string password)
        {
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_apiKey}";
            var payload = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var response = await _httpClient.PostAsJsonAsync(url, payload);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<FirebaseErrorResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                throw new HttpRequestException(error?.Error?.Message ?? "Authentication failed.");
            }

            return JsonSerializer.Deserialize<FirebaseAuthResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<FirebaseOtpResponse> SendOtpAsync(string phoneNumber)
        {
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:sendVerificationCode?key={_apiKey}";
            // Ensure phone number starts with +91 if not provided
            if (!phoneNumber.StartsWith("+"))
            {
                phoneNumber = "+91" + phoneNumber;
            }

            var payload = new
            {
                phoneNumber = phoneNumber
            };

            var response = await _httpClient.PostAsJsonAsync(url, payload);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<FirebaseErrorResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                throw new HttpRequestException(error?.Error?.Message ?? "Failed to send OTP.");
            }

            return JsonSerializer.Deserialize<FirebaseOtpResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<FirebaseAuthResponse> VerifyOtpAsync(string sessionInfo, string code)
        {
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPhoneNumber?key={_apiKey}";
            var payload = new
            {
                sessionInfo = sessionInfo,
                code = code
            };

            var response = await _httpClient.PostAsJsonAsync(url, payload);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<FirebaseErrorResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                throw new HttpRequestException(error?.Error?.Message ?? "Invalid OTP.");
            }

            return JsonSerializer.Deserialize<FirebaseAuthResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }
    }
}
