using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ColorvisionPaintsERP.Models;
using ColorvisionPaintsERP.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;

namespace ColorvisionPaintsERP.Pages.Rewards
{
    [Authorize]
    public class QRGenerationModel : PageModel
    {
        private readonly IRewardsService _svc;
        public QRGenerationModel(IRewardsService svc) => _svc = svc;

        public List<QrCode> QrCodes { get; set; } = new();
        public QRGenerationSummary Summary { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            QrCodes = await _svc.GetQrCodesAsync("");
            if (QrCodes.Count == 0) QrCodes = MockQrCodes();

            var now = DateTime.UtcNow;
            var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var scannedMonth = QrCodes.Count(q => q.Status == QRStatus.Scanned && q.ScannedAt >= monthStart);
            var total = QrCodes.Count;

            Summary = new QRGenerationSummary
            {
                TotalGenerated   = total,
                ScannedThisMonth = scannedMonth,
                ScanRatePercent  = total > 0 ? Math.Round(QrCodes.Count(q => q.Status == QRStatus.Scanned) * 100.0 / total, 1) : 0,
                PointsIssuedMtd  = QrCodes.Where(q => q.Status == QRStatus.Scanned && q.ScannedAt >= monthStart).Sum(q => q.PointValue),
            };
            return Page();
        }

        private List<QrCode> MockQrCodes() => new()
        {
            new QrCode { SerialNumber="CV-QR-00000001", ProductName="InteriorEmu Pro", BatchNumber="PRD-BATCH-0001", PointValue=15, Status=QRStatus.Scanned, ScanCity="Mumbai", CreatedAt=DateTime.Now.AddDays(-10), ExpiryDate=DateTime.Now.AddDays(355), ScannedAt=DateTime.Now.AddDays(-2) },
            new QrCode { SerialNumber="CV-QR-00000002", ProductName="InteriorEmu Pro", BatchNumber="PRD-BATCH-0001", PointValue=15, Status=QRStatus.Unscanned, CreatedAt=DateTime.Now.AddDays(-10), ExpiryDate=DateTime.Now.AddDays(355) },
            new QrCode { SerialNumber="CV-QR-00000003", ProductName="Exterior Shield XT", BatchNumber="PRD-BATCH-0002", PointValue=25, Status=QRStatus.Unscanned, CreatedAt=DateTime.Now.AddDays(-5), ExpiryDate=DateTime.Now.AddDays(360) },
            new QrCode { SerialNumber="CV-QR-00000004", ProductName="Steel Coat Pro",     BatchNumber="PRD-BATCH-0003", PointValue=40, Status=QRStatus.Voided,    CreatedAt=DateTime.Now.AddDays(-30), ExpiryDate=DateTime.Now.AddDays(335) },
            new QrCode { SerialNumber="CV-QR-00000005", ProductName="Primer Plus",         BatchNumber="PRD-BATCH-0001", PointValue=10, Status=QRStatus.Scanned, ScanCity="Pune", CreatedAt=DateTime.Now.AddDays(-15), ExpiryDate=DateTime.Now.AddDays(350), ScannedAt=DateTime.Now.AddDays(-1) },
        };
    }
}
