using System.Threading.Tasks;
using ColorvisionPaintsERP.Models;

namespace ColorvisionPaintsERP.Services
{
    public interface IFirestoreService
    {
        Task<UserProfile?> GetUserProfileAsync(string uid, string idToken);
        Task CreateUserProfileAsync(string uid, UserProfile profile, string idToken);
        Task<RolePermissions> GetRolePermissionsAsync(string role, string idToken);
        Task SeedRolePermissionsAsync(string idToken);
        Task<bool> IsFirstUserAsync(string idToken);
    }
}
