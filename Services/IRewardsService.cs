using ColorvisionPaintsERP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColorvisionPaintsERP.Services
{
    public interface IRewardsService
    {
        Task<List<QrCode>> GetQrCodesAsync(string idToken, int pageSize = 200);
        Task<QrCode?> GetQrCodeAsync(string serialNumber, string idToken);
        Task CreateQrCodesAsync(string productVariantId, string productName, string batchNumber, System.DateTime mfgDate, int quantity, int pointValue, string idToken);
        Task VoidQrCodeAsync(string serialNumber, string idToken);

        Task<List<Painter>> GetPaintersAsync(string idToken);
        Task<Painter?> GetPainterAsync(string id, string idToken);
        Task BlockPainterAsync(string id, string reason, string idToken);
        Task UnblockPainterAsync(string id, string idToken);

        Task<List<PainterTransaction>> GetTransactionsAsync(string idToken);
        Task<List<PainterTransaction>> GetTransactionsByPainterAsync(string painterId, string idToken);
        Task ApproveRedemptionAsync(string id, string idToken);
        Task FailRedemptionAsync(string id, string reason, string idToken);

        Task<List<RewardCampaign>> GetCampaignsAsync(string idToken);
        Task<string> CreateCampaignAsync(RewardCampaign campaign, string idToken);
        Task ToggleCampaignAsync(string id, bool isActive, string idToken);

        Task<List<FraudFlag>> GetFraudFlagsAsync(string idToken);
        Task UpdateFlagStatusAsync(string id, string status, string reviewedBy, string idToken);

        Task<string> GetNextQrSerialAsync(string idToken);
    }
}
