using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ColorvisionPaintsERP.Models;
using ColorvisionPaintsERP.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;

namespace ColorvisionPaintsERP.Pages.Manufacturing
{
    [Authorize]
    public class ScheduleModel : PageModel
    {
        private readonly IManufacturingService _mfg;
        public ScheduleModel(IManufacturingService mfg) => _mfg = mfg;

        public List<ProductionLine> Lines { get; set; } = new();
        public List<ProductionBatch> Batches { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Lines   = await _mfg.GetProductionLinesAsync("");
            // Batches come from all orders for simplicity
            Batches = new();

            if (Lines.Count == 0)
            {
                Lines = new List<ProductionLine>
                {
                    new ProductionLine { Id="L1", Name="Line A — Emulsion", Status=LineStatus.Active, CapacityKgPerDay=5000 },
                    new ProductionLine { Id="L2", Name="Line B — Enamel",   Status=LineStatus.Active, CapacityKgPerDay=3000 },
                    new ProductionLine { Id="L3", Name="Line C — Primer",   Status=LineStatus.Maintenance, CapacityKgPerDay=2000 },
                };
            }

            if (Batches.Count == 0)
            {
                Batches = new List<ProductionBatch>
                {
                    new ProductionBatch { Id="B1", BatchNumber="PRD-BATCH-0001", ProductionLineId="L1", ProductName="InteriorEmu Pro — Arctic White", Status=BatchStatus.InProgress, StartTime=DateTime.Now.AddDays(-1), EndTime=DateTime.Now.AddDays(0.5) },
                    new ProductionBatch { Id="B2", BatchNumber="PRD-BATCH-0002", ProductionLineId="L1", ProductName="InteriorEmu Pro — Arctic White", Status=BatchStatus.Pending,    StartTime=DateTime.Now.AddDays(0.5), EndTime=DateTime.Now.AddDays(1.5) },
                    new ProductionBatch { Id="B3", BatchNumber="PRD-BATCH-0003", ProductionLineId="L2", ProductName="Exterior Shield XT — Ocean Blue",Status=BatchStatus.Pending,    StartTime=DateTime.Now.AddDays(1), EndTime=DateTime.Now.AddDays(2) },
                    new ProductionBatch { Id="B4", BatchNumber="PRD-BATCH-0004", ProductionLineId="L2", ProductName="Steel Coat Pro — Silver Grey",  Status=BatchStatus.Passed,      StartTime=DateTime.Now.AddDays(-3), EndTime=DateTime.Now.AddDays(-2) },
                };
            }

            return Page();
        }
    }
}
