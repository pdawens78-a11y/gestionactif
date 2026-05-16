using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionInventaire.Web.Areas.Identity.Pages.Account
{
    public class UserInvitationSentModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        public void OnGet()
        {
        }
    }
}