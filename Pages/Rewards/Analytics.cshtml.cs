using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ColorvisionPaintsERP.Models;
using ColorvisionPaintsERP.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

// Alias to avoid clash with Pages.Painter namespace
using PainterModel = ColorvisionPaintsERP.Models.Painter;

namespace ColorvisionPaintsERP.Pages.Rewards
{
    [Authorize]
    public class AnalyticsModel : PageModel
    {
        private readonly IRewardsService _svc;
        public AnalyticsModel(IRewardsService svc) => _svc = svc;

        public List<PainterModel> TopPainters { get; set; } = new();
        public List<QrCode>       QrCodes     { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var painters = await _svc.GetPaintersAsync("");
            QrCodes      = await _svc.GetQrCodesAsync("");

            if (painters.Count == 0) painters = new List<PainterModel> {
                new PainterModel { Name="Mohan Das",    City="Chennai", TotalPointsEarned=48000, Tier=PainterTier.Platinum },
                new PainterModel { Name="Ravi Shankar", City="Mumbai",  TotalPointsEarned=22000, Tier=PainterTier.Gold },
                new PainterModel { Name="Suresh Patil", City="Pune",    TotalPointsEarned=9000,  Tier=PainterTier.Silver },
                new PainterModel { Name="Anil Kumar",   City="Delhi",   TotalPointsEarned=2100,  Tier=PainterTier.Bronze },
                new PainterModel { Name="Prakash B.",   City="Kolkata", TotalPointsEarned=500,   Tier=PainterTier.Bronze },
            };

            TopPainters = painters.OrderByDescending(p => p.TotalPointsEarned).Take(10).ToList();
            return Page();
        }
    }
}
