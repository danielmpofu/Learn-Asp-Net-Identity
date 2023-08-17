using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn_Asp_Net_Identity.Pages
{
    [Authorize(policy: "MustBelongToHRDepartment")]
    public class HumanResourcePageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
