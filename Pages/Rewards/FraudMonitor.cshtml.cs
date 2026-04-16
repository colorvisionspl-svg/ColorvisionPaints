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
    public class FraudMonitorModel : PageModel
    {
        private readonly IRewardsService _svc;
        public FraudMonitorModel(IRewardsService svc) => _svc = svc;

        public List<FraudFlag> Flags { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Flags = await _svc.GetFraudFlagsAsync("");
            if (Flags.Count == 0)
                Flags = new List<FraudFlag>
                {
                    new FraudFlag { Id="FF1", Type=FraudType.HighVelocity,    PainterName="Prakash B.",  ScanCount=28, TimeWindow="24 hours",  GpsCoordinates="18.9257, 72.8242", FlaggedAt=DateTime.Now.AddHours(-3),  Status=FlagStatus.Pending },
                    new FraudFlag { Id="FF2", Type=FraudType.GeoAnomaly,      PainterName="Unknown #5",  ScanCount=2,  TimeWindow="90 minutes", GpsCoordinates="19.0760, 72.8777 → 28.6139, 77.2090", FlaggedAt=DateTime.Now.AddHours(-6), Status=FlagStatus.Pending },
                    new FraudFlag { Id="FF3", Type=FraudType.BulkScan,        PainterName="Anon Device", ScanCount=8,  TimeWindow="45 minutes", GpsCoordinates="18.5204, 73.8567", FlaggedAt=DateTime.Now.AddDays(-1),  Status=FlagStatus.Reviewed },
                    new FraudFlag { Id="FF4", Type=FraudType.DuplicateDevice, PainterName="3 accounts",  ScanCount=3,  TimeWindow="—",          GpsCoordinates="22.5726, 88.3639", FlaggedAt=DateTime.Now.AddDays(-2),  Status=FlagStatus.Dismissed },
                };
            return Page();
        }
    }
}
