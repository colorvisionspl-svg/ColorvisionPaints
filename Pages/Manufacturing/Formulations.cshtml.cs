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
    public class FormulationsModel : PageModel
    {
        private readonly IManufacturingService _mfg;
        public FormulationsModel(IManufacturingService mfg) => _mfg = mfg;

        public List<Formulation> Formulations { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Formulations = await _mfg.GetFormulationsAsync("");

            if (Formulations.Count == 0)
            {
                Formulations = new List<Formulation>
                {
                    new Formulation {
                        FormulationCode = "FORM-0001", ProductName = "InteriorEmu Pro", ShadeName = "Arctic White",
                        Version = 2, StandardBatchSizeKg = 1000, ExpectedYieldPercent = 96.5,
                        ProcessingTimeHours = 4, Status = FormulationStatus.Active,
                        Ingredients = new() {
                            new FormulationIngredient { MaterialName="Titanium Dioxide", QuantityKg=200, PercentByWeight=20, Sequence=1 },
                            new FormulationIngredient { MaterialName="Calcium Carbonate", QuantityKg=300, PercentByWeight=30, Sequence=2 },
                            new FormulationIngredient { MaterialName="Acrylic Emulsion", QuantityKg=400, PercentByWeight=40, Sequence=3 },
                            new FormulationIngredient { MaterialName="Water", QuantityKg=100, PercentByWeight=10, Sequence=4 }
                        }
                    },
                    new Formulation {
                        FormulationCode = "FORM-0002", ProductName = "Exterior Shield XT", ShadeName = "Ocean Blue",
                        Version = 1, StandardBatchSizeKg = 500, ExpectedYieldPercent = 94,
                        ProcessingTimeHours = 6, Status = FormulationStatus.Draft,
                        Ingredients = new()
                    },
                };
            }

            return Page();
        }
    }
}
