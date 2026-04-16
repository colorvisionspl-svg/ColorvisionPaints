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
    public class QualityControlModel : PageModel
    {
        private readonly IManufacturingService _mfg;
        public QualityControlModel(IManufacturingService mfg) => _mfg = mfg;

        public List<QCInspection> Inspections { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Inspections = await _mfg.GetQCInspectionsAsync("");

            if (Inspections.Count == 0)
            {
                Inspections = new List<QCInspection>
                {
                    new QCInspection {
                        BatchNumber="PRD-BATCH-0001", ProductName="InteriorEmu Pro", ShadeName="Arctic White",
                        ProductionOrderId="CV/PRD/2025-26/0001", InspectorName="QC Team",
                        YieldKg=965, OverallResult=QCResult.Pass, InspectionDate=DateTime.Now.AddDays(-1),
                        CoaUrl="#", CreatedAt=DateTime.Now.AddDays(-1),
                        TestResults = new() {
                            new QCTestResult { TestName="Viscosity", TargetValue="3000–4000 cP", MinAcceptable=3000, MaxAcceptable=4000, ActualValue=3400, Unit="cP", Result=QCResult.Pass },
                            new QCTestResult { TestName="Color ΔE",  TargetValue="<1.0",          MinAcceptable=0,    MaxAcceptable=1.0,  ActualValue=0.6,  Unit="ΔE", Result=QCResult.Pass },
                            new QCTestResult { TestName="Gloss",     TargetValue="85–95 GU",      MinAcceptable=85,   MaxAcceptable=95,   ActualValue=88,   Unit="GU", Result=QCResult.Pass },
                        }
                    },
                    new QCInspection {
                        BatchNumber="PRD-BATCH-0005", ProductName="Steel Coat Pro", ShadeName="Silver Grey",
                        ProductionOrderId="CV/PRD/2025-26/0003", InspectorName="",
                        YieldKg=480, OverallResult="", InspectionDate=DateTime.Now,
                        CoaUrl="", CreatedAt=DateTime.Now,
                        TestResults=new()
                    },
                };
            }

            return Page();
        }
    }
}
