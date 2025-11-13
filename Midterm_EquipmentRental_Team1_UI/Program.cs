using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Cookies";
    options.DefaultSignInScheme = "Cookies";
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
})
.AddOpenIdConnect("oidc", options =>
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

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "role"

    };

    options.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents
    {
        OnRedirectToIdentityProviderForSignOut = context =>
        {
            var logoutUri = "https://accounts.google.com/Logout";
            context.HandleResponse();
            context.Response.Redirect("/");
            return Task.CompletedTask;
        }
    };
});
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
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
