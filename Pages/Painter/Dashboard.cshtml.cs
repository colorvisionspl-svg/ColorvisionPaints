using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace ColorvisionPaintsERP.Pages.Painter
{
    public class DashboardModel : PageModel
    {
        // Painter dashboard is purely client-side (reads from localStorage + Firestore JS SDK)
        // No server auth needed — painter auth is Firebase Phone Auth (client-side)
        public IActionResult OnGet() => Page();
    }
}
