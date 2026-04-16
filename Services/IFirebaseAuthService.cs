using ColorvisionPaintsERP.Models;
using System.Threading.Tasks;

namespace ColorvisionPaintsERP.Services
{
    public interface IFirebaseAuthService
    {
        Task<FirebaseAuthResponse> LoginWithEmailAsync(string email, string password);
        Task<FirebaseOtpResponse> SendOtpAsync(string phoneNumber);
        Task<FirebaseAuthResponse> VerifyOtpAsync(string sessionInfo, string code);
    }
}
