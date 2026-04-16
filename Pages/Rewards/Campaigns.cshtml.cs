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
    public class CampaignsModel : PageModel
    {
        private readonly IRewardsService _svc;
        public CampaignsModel(IRewardsService svc) => _svc = svc;

        public List<RewardCampaign> Campaigns { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Campaigns = await _svc.GetCampaignsAsync("");
            if (Campaigns.Count == 0)
                Campaigns = new List<RewardCampaign>
                {
                    new RewardCampaign { Id="C1", Name="Summer Splash 2026",      Description="Double points on all exterior products during summer.",       StartDate=DateTime.Now.AddDays(-5), EndDate=DateTime.Now.AddDays(25), BonusMultiplier=2, BonusPoints=0, IsActive=true,  TargetProductIds=new(){"P2"}, TargetCities=new() },
                    new RewardCampaign { Id="C2", Name="Mumbai Metro Boost",       Description="Flat 50 bonus points for every scan in Mumbai.",             StartDate=DateTime.Now.AddDays(-2), EndDate=DateTime.Now.AddDays(28), BonusMultiplier=1, BonusPoints=50, IsActive=true, TargetProductIds=new(), TargetCities=new(){"Mumbai"} },
                    new RewardCampaign { Id="C3", Name="Waterproofing Drive Q1",   Description="3x points on waterproofing products for premium retention.", StartDate=DateTime.Now.AddDays(-30),EndDate=DateTime.Now.AddDays(-1),BonusMultiplier=3, BonusPoints=0, IsActive=false, TargetProductIds=new(){"P3"}, TargetCities=new() },
                };
            return Page();
        }
    }
}
