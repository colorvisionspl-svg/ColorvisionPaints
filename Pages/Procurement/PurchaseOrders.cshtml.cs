using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ColorvisionPaintsERP.Models;
using ColorvisionPaintsERP.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ColorvisionPaintsERP.Pages.Procurement
{
    [Authorize]
    public class PurchaseOrdersModel : PageModel
    {
        private readonly IProcurementService _procurementService;

        public PurchaseOrdersModel(IProcurementService procurementService)
        {
            _procurementService = procurementService;
        }

        public List<PurchaseOrder> Orders { get; set; } = new();
        public List<Vendor> Vendors { get; set; } = new();
        public List<RawMaterial> Materials { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            string idToken = ""; 

            // Fetch data (with fallback mocks)
            Orders = await _procurementService.GetPurchaseOrdersAsync(idToken);
            Vendors = await _procurementService.GetVendorsAsync(idToken);
            Materials = await _procurementService.GetRawMaterialsAsync(idToken);

            if (Orders.Count == 0)
            {
                Orders = new List<PurchaseOrder>
                {
                    new PurchaseOrder { 
                        PoNumber = "CV/PO/2025-26/0001", 
                        VendorName = "ChemPrime Solutions", 
                        OrderDate = DateTime.Now.AddDays(-5), 
                        ExpectedDeliveryDate = DateTime.Now.AddDays(2),
                        Items = new List<OrderItem>{ new OrderItem { MaterialName = "Blue Pigment", Quantity = 500 } },
                        TotalAmount = 45000, 
                        Status = POStatus.Sent 
                    },
                    new PurchaseOrder { 
                        PoNumber = "CV/PO/2025-26/0002", 
                        VendorName = "Pigment Hub Ltd", 
                        OrderDate = DateTime.Now.AddDays(-2), 
                        ExpectedDeliveryDate = DateTime.Now.AddDays(5),
                        Items = new List<OrderItem>{ new OrderItem { MaterialName = "Epoxy Resin", Quantity = 200 } },
                        TotalAmount = 120000, 
                        Status = POStatus.Draft 
                    }
                };
            }

            if (Vendors.Count == 0)
            {
                Vendors = new List<Vendor> { new Vendor { VendorCode = "VND-0001", CompanyName = "ChemPrime Solutions", QualityRating = 5 } };
            }

            if (Materials.Count == 0)
            {
                Materials = new List<RawMaterial> { 
                    new RawMaterial { MaterialCode = "RM-0001", Name = "Blue Pigment", Unit = "kg" },
                    new RawMaterial { MaterialCode = "RM-0002", Name = "Epoxy Resin", Unit = "L" }
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCreateOrderAsync([FromBody] PurchaseOrder po)
        {
            string idToken = "";
            po.PoNumber = await _procurementService.GetNextSequenceIdAsync("purchase_orders", "CV/PO/2025-26", idToken);
            await _procurementService.CreatePurchaseOrderAsync(po, idToken);
            return new JsonResult(new { success = true });
        }
    }
}
