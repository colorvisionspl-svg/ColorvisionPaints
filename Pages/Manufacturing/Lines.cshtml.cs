using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ColorvisionPaintsERP.Models;
using ColorvisionPaintsERP.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ColorvisionPaintsERP.Pages.Manufacturing
{
    [Authorize]
    public class LinesModel : PageModel
    {
        private readonly IManufacturingService _mfg;
        public LinesModel(IManufacturingService mfg) => _mfg = mfg;

        public List<ProductionLine> Lines { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Lines = await _mfg.GetProductionLinesAsync("");

            if (Lines.Count == 0)
            {
                Lines = new List<ProductionLine>
                {
                    new ProductionLine { Id="L1", Name="Line A — Emulsion", CapacityKgPerDay=5000, Status=LineStatus.Active, CurrentBatchId="PRD-BATCH-0001", ProgressPercent=65 },
                    new ProductionLine { Id="L2", Name="Line B — Enamel",   CapacityKgPerDay=3000, Status=LineStatus.Active, CurrentBatchId="", ProgressPercent=0 },
                    new ProductionLine { Id="L3", Name="Line C — Primer",   CapacityKgPerDay=2000, Status=LineStatus.Maintenance, CurrentBatchId="", ProgressPercent=0 },
                };
            }

            return Page();
        }
    }
}
