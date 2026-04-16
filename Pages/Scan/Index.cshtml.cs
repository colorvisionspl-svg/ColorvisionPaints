using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ColorvisionPaintsERP.Models;
using ColorvisionPaintsERP.Services;
using System.Threading.Tasks;

namespace ColorvisionPaintsERP.Pages.Scan
{
    public class IndexModel : PageModel
    {
        private readonly IRewardsService _svc;
        public IndexModel(IRewardsService svc) => _svc = svc;

        [BindProperty(SupportsGet = true)]
        public string SerialNumber { get; set; } = "";

        public QrCode? QrCode { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!string.IsNullOrEmpty(SerialNumber))
                QrCode = await _svc.GetQrCodeAsync(SerialNumber, "");

            // If not found in Firestore, create a demo QR for preview purposes
            if (QrCode == null && SerialNumber.StartsWith("CV-QR-"))
            {
                QrCode = new QrCode
                {
                    SerialNumber = SerialNumber,
                    ProductName  = "InteriorEmu Pro — Arctic White",
                    PointValue   = 15,
                    Status       = QRStatus.Unscanned,
                    BatchNumber  = "PRD-BATCH-0001",
                    ExpiryDate   = System.DateTime.Now.AddDays(350),
                };
            }
            return Page();
        }
    }
}
