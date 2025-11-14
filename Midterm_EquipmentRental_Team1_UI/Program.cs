using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Midterm_EquipmentRental_Team1_Models;
using Midterm_EquipmentRental_Team1_UI.Global;
using System.Security.Claims;
using Midterm_EquipmentRental_Team1_UI.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";
})
.AddOpenIdConnect(options =>
{
    options.Authority = "https://accounts.google.com";
    options.ClientId = "794486917877-27k138kbs06ku89n2urrv24l9poti16u.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.ResponseType = "code";
    options.CallbackPath = "/signin-oidc";
    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.SaveTokens = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };

    options.Events = new OpenIdConnectEvents
    {
        OnTokenValidated = async context =>
        {
            var httpClientFactory = context.HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
            var client = httpClientFactory.CreateClient();

            var email = context.Principal?.FindFirstValue(ClaimTypes.Email);
            var externalId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (email == null || externalId == null) return;

            var appUser = new AppUser
            {
                Email = email,
                ExternalId = externalId,
                ExternalProvider = "Google"
            };

            var response = await client.PostAsJsonAsync($"{GlobalUsings.API_BASE_URL}/Auth/Sync", appUser);

            if (!response.IsSuccessStatusCode) return;

            var syncedUser = await response.Content.ReadFromJsonAsync<AppUser>();

            var identity = context.Principal!.Identity as ClaimsIdentity;
            var existingRoleClaim = identity!.FindFirst(ClaimTypes.Role);

            if (existingRoleClaim != null)
            {
                identity.RemoveClaim(existingRoleClaim);
            }

            identity.AddClaim(new Claim(ClaimTypes.Name, syncedUser!.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Role, syncedUser!.Role));

            await context.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity!),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                });

            var jwt = JwtTokenGenerator.GenerateToken(syncedUser!, context.HttpContext.RequestServices.GetRequiredService<IConfiguration>());
            context.HttpContext.Session.SetString("JWToken", jwt);
        },
        OnRedirectToIdentityProviderForSignOut = context =>
        {
            context.HandleResponse();
            context.Response.Redirect("/Auth/Login");
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddSession();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Auth/AccessDenied");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customer}/{action=Dashboard}/{id?}");

app.Run();
