using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace sso_authentication.Pages
{
  [AllowAnonymous]
  public class SessionHandlerModel : PageModel
  {
    private const string TemporaryCookie = "TempSessionHandler";

    public async Task<IActionResult> OnGet()
    {
      AuthenticateResult sessionData = await HttpContext.AuthenticateAsync(TemporaryCookie);

      if (!sessionData.Succeeded)
      {
        return RedirectToPage("Login?sessionFault=true");
      }

      ClaimsPrincipal extClaimsData = sessionData.Principal;
      string sub = extClaimsData.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
      string name = extClaimsData.FindFirst(ClaimTypes.Name)?.Value ?? "";
      string email = extClaimsData.FindFirst(ClaimTypes.Email)?.Value ?? "";
      string issuer = sessionData.Properties.Items["schema"] ?? "";

      var claims = new List<Claim>
      {
        new("sub", sub),
        new("name", name),
        new("email", email),
        new("issuer", issuer)
      };
      var ci = new ClaimsIdentity(claims, issuer, "name", "user");
      var cp = new ClaimsPrincipal(ci);

      await HttpContext.SignInAsync(cp);
      await HttpContext.SignOutAsync(TemporaryCookie);

      return RedirectToPage(Url.Page("Secure"));
    }
  }
}
