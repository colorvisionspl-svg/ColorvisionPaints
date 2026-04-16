using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ColorvisionPaintsERP.Pages.Dashboard
{
    [Authorize]
    public class SalesmanModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
