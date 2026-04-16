using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ColorvisionPaintsERP.Models;
using ColorvisionPaintsERP.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

namespace ColorvisionPaintsERP.Pages.Procurement
{
    [Authorize]
    public class GRNModel : PageModel
    {
        private readonly IProcurementService _procurementService;

        public GRNModel(IProcurementService procurementService)
        {
            _procurementService = procurementService;
        }

        public List<GRN> GRNs { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            string idToken = ""; 

            GRNs = await _procurementService.GetGRNsAsync(idToken);
            
            if (GRNs.Count == 0)
            {
                GRNs = new List<GRN>
                {
                    new GRN { 
                        GrnNumber = "CV/GRN/2025-26/0001", 
                        PoNumber = "CV/PO/2025-26/0001", 
                        VendorId = "VND-0001", 
                        ReceivedDate = DateTime.Now.AddDays(-1),
                        Items = new List<GRNItem>{ new GRNItem { MaterialName = "Blue Pigment", ReceivedQty = 500, BatchNumber = "BATCH-A101" } },
                        Status = GRNStatus.QCApproved 
                    },
                    new GRN { 
                        GrnNumber = "CV/GRN/2025-26/0002", 
                        PoNumber = "CV/PO/2025-26/0005", 
                        VendorId = "VND-0002", 
                        ReceivedDate = DateTime.Now,
                        Items = new List<GRNItem>{ new GRNItem { MaterialName = "Epoxy Resin", ReceivedQty = 200, BatchNumber = "BATCH-B202" } },
                        Status = GRNStatus.QCPending 
                    }
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostApproveQCAsync(string grnId)
        {
            string idToken = "";
            await _procurementService.UpdateGRNStatusAsync(grnId, GRNStatus.QCApproved, idToken);
            return new JsonResult(new { success = true });
        }
    }
}
