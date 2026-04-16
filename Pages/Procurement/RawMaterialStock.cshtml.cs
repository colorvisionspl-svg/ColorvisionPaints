using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ColorvisionPaintsERP.Models;
using ColorvisionPaintsERP.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;

namespace ColorvisionPaintsERP.Pages.Procurement
{
    [Authorize]
    public class RawMaterialStockModel : PageModel
    {
        private readonly IProcurementService _procurementService;

        public RawMaterialStockModel(IProcurementService procurementService)
        {
            _procurementService = procurementService;
        }

        public List<RawMaterialStock> StockItems { get; set; } = new();
        public List<RawMaterial> Materials { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            string idToken = ""; 

            StockItems = await _procurementService.GetStockAsync(idToken);
            Materials = await _procurementService.GetRawMaterialsAsync(idToken);
            
            if (StockItems.Count == 0)
            {
                StockItems = new List<RawMaterialStock>
                {
                    new RawMaterialStock { 
                        MaterialName = "Blue Pigment", 
                        Quantity = 1200, 
                        Available = 1050, // Added in-memory helper
                        Unit = "kg", 
                        BatchNumber = "B-992", 
                        ExpiryDate = DateTime.Now.AddDays(15), 
                        WarehouseZone = "Raw Material",
                        Status = StockStatus.Approved
                    },
                    new RawMaterialStock { 
                        MaterialName = "Titanium Dioxide", 
                        Quantity = 50, 
                        Available = 50, 
                        Unit = "kg", 
                        BatchNumber = "B-102", 
                        ExpiryDate = DateTime.Now.AddDays(200), 
                        WarehouseZone = "Raw Material",
                        Status = StockStatus.Approved
                    },
                    new RawMaterialStock { 
                        MaterialName = "Epoxy Resin", 
                        Quantity = 800, 
                        Available = 800, 
                        Unit = "L", 
                        BatchNumber = "B-551", 
                        ExpiryDate = DateTime.Now.AddDays(45), 
                        WarehouseZone = "Hazardous",
                        Status = StockStatus.Quarantine
                    }
                };
            }

            if (Materials.Count == 0)
            {
                Materials = new List<RawMaterial> { 
                    new RawMaterial { Name = "Blue Pigment", Category = "Pigment", ReorderLevel = 1500, SafetyStock = 500 },
                    new RawMaterial { Name = "Titanium Dioxide", Category = "Pigment", ReorderLevel = 100, SafetyStock = 40 },
                    new RawMaterial { Name = "Epoxy Resin", Category = "Resin", ReorderLevel = 1000, SafetyStock = 300 }
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAdjustAsync([FromBody] StockAdjustment adjustment)
        {
            // Update logic here
            return new JsonResult(new { success = true });
        }
    }

    public class StockAdjustment {
        public string MaterialId { get; set; } = "";
        public string BatchNumber { get; set; } = "";
        public double Quantity { get; set; }
        public string Type { get; set; } = "Reduce"; // Add / Reduce
        public string Reason { get; set; } = "";
    }
}
