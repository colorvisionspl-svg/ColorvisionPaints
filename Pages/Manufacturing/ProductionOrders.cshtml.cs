using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ColorvisionPaintsERP.Models;
using ColorvisionPaintsERP.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

namespace ColorvisionPaintsERP.Pages.Manufacturing
{
    [Authorize]
    public class ProductionOrdersModel : PageModel
    {
        private readonly IManufacturingService _mfg;
        public ProductionOrdersModel(IManufacturingService mfg) => _mfg = mfg;

        public List<ProductionOrder> Orders { get; set; } = new();
        public List<Formulation> Formulations { get; set; } = new();
        public List<ProductionLine> Lines { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Orders       = await _mfg.GetProductionOrdersAsync("");
            Formulations = await _mfg.GetFormulationsAsync("");
            Lines        = await _mfg.GetProductionLinesAsync("");

            if (Orders.Count == 0)
            {
                Orders = new List<ProductionOrder>
                {
                    new ProductionOrder {
                        OrderNumber="CV/PRD/2025-26/0001", ProductName="InteriorEmu Pro", ShadeName="Arctic White",
                        FormulationId="FORM-0001", PlannedQuantityKg=5000, PlannedBatches=5,
                        ProductionLineName="Line A — Emulsion", Status=ProductionOrderStatus.InProgress,
                        PlannedStartDate=DateTime.Now.AddDays(-2), PlannedEndDate=DateTime.Now.AddDays(1), CreatedAt=DateTime.Now.AddDays(-3)
                    },
                    new ProductionOrder {
                        OrderNumber="CV/PRD/2025-26/0002", ProductName="Exterior Shield XT", ShadeName="Ocean Blue",
                        FormulationId="FORM-0002", PlannedQuantityKg=2000, PlannedBatches=4,
                        ProductionLineName="Line B — Enamel", Status=ProductionOrderStatus.Planned,
                        PlannedStartDate=DateTime.Now.AddDays(1), PlannedEndDate=DateTime.Now.AddDays(4), CreatedAt=DateTime.Now.AddDays(-1)
                    },
                    new ProductionOrder {
                        OrderNumber="CV/PRD/2025-26/0003", ProductName="Steel Coat Pro", ShadeName="Silver Grey",
                        FormulationId="FORM-0001", PlannedQuantityKg=1000, PlannedBatches=2,
                        ProductionLineName="Line A — Emulsion", Status=ProductionOrderStatus.QCPending,
                        PlannedStartDate=DateTime.Now.AddDays(-5), PlannedEndDate=DateTime.Now.AddDays(-1), CreatedAt=DateTime.Now.AddDays(-6)
                    },
                };
            }

            if (Formulations.Count == 0)
                Formulations = new() { new Formulation { Id="FORM-0001", FormulationCode="FORM-0001", ProductName="InteriorEmu Pro", ShadeName="Arctic White", StandardBatchSizeKg=1000, Status=FormulationStatus.Active } };

            if (Lines.Count == 0)
                Lines = new() {
                    new ProductionLine { Id="L1", Name="Line A — Emulsion", Status=LineStatus.Active, CapacityKgPerDay=5000 },
                    new ProductionLine { Id="L2", Name="Line B — Enamel", Status=LineStatus.Active, CapacityKgPerDay=3000 }
                };

            return Page();
        }
    }
}
