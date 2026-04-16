using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ColorvisionPaintsERP.Models;
using ColorvisionPaintsERP.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;

// Alias to avoid clash with Pages.Painter namespace
using PainterModel = ColorvisionPaintsERP.Models.Painter;

namespace ColorvisionPaintsERP.Pages.Rewards
{
    [Authorize]
    public class PainterProfilesModel : PageModel
    {
        private readonly IRewardsService _svc;
        public PainterProfilesModel(IRewardsService svc) => _svc = svc;

        public List<PainterModel> Painters { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var raw = await _svc.GetPaintersAsync("");
            Painters = raw.Count > 0 ? raw : new List<PainterModel>
            {
                new PainterModel { Id="P1", Name="Ravi Shankar", Mobile="+919876543210", City="Mumbai", State="MH", Tier=PainterTier.Gold,     CurrentPoints=18450, TotalPointsEarned=22000, RegisteredAt=DateTime.Now.AddMonths(-6), LastScanAt=DateTime.Now.AddDays(-1), IsVerified=true },
                new PainterModel { Id="P2", Name="Suresh Patil",  Mobile="+919812345678", City="Pune",   State="MH", Tier=PainterTier.Silver,   CurrentPoints=7200,  TotalPointsEarned=9000,  RegisteredAt=DateTime.Now.AddMonths(-3), LastScanAt=DateTime.Now.AddDays(-5), IsVerified=true },
                new PainterModel { Id="P3", Name="Anil Kumar",    Mobile="+917654321098", City="Delhi",  State="DL", Tier=PainterTier.Bronze,   CurrentPoints=1800,  TotalPointsEarned=2100,  RegisteredAt=DateTime.Now.AddMonths(-1), LastScanAt=DateTime.Now.AddDays(-3), IsVerified=false },
                new PainterModel { Id="P4", Name="Mohan Das",     Mobile="+918765432109", City="Chennai",State="TN", Tier=PainterTier.Platinum, CurrentPoints=41000, TotalPointsEarned=48000, RegisteredAt=DateTime.Now.AddMonths(-9), LastScanAt=DateTime.Now.AddDays(-0), IsVerified=true },
                new PainterModel { Id="P5", Name="Prakash B.",    Mobile="+916543210987", City="Kolkata",State="WB", Tier=PainterTier.Bronze,   CurrentPoints=500,   TotalPointsEarned=500,   RegisteredAt=DateTime.Now.AddDays(-10), LastScanAt=DateTime.Now.AddDays(-2), IsVerified=false, IsBlocked=true, BlockReason="Suspicious scan pattern" },
            };
            return Page();
        }
    }
}
