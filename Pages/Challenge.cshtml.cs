using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace sso_authentication.Pages
{
  [AllowAnonymous]
  public class ChallengeModel : PageModel
  {
    public IActionResult OnGet()
    {
      var props = new AuthenticationProperties
      {
        RedirectUri = "/secure"
      };

      return Challenge(props, "Google");
    }
  }
}
