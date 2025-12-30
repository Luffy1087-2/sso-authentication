using DotEnv.Core;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
new EnvLoader().Load();
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(defaultScheme: "Cookie")
    .AddCookie(authenticationScheme: "Cookie", (opt) =>
    {
      opt.Cookie.Name = "sso-auth";
      opt.ExpireTimeSpan = TimeSpan.FromHours(8);
      opt.LoginPath = "/login";
    })
    .AddCookie("TempSessionHandler")
    .AddGoogle("Google", (opt) =>
    {
      opt.ClientId = Environment.GetEnvironmentVariable("SSO_CLIENT_ID") ?? "";
      opt.ClientSecret = Environment.GetEnvironmentVariable("SSO_CLIENT_SECRET") ?? "";
      opt.CallbackPath = "/sso-return-url"; // Set this url to google ( defaault: /signin-google )
      opt.SignInScheme = "TempSessionHandler"; // Store google data into a temporary TempSessionHandler cookie ( default: defaultScheme, "Cookie" )
    });

WebApplication app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages()
    .RequireAuthorization();
app.MapGet("/", () => Results.Redirect("/login"));
app.Run();