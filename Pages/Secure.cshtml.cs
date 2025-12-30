using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace sso_authentication.Pages
{
  public class SecureModel : PageModel
  {
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost() {
      await HttpContext.SignOutAsync();

      return Redirect(Url.Page("login") ?? "");
    }
  }
}
