using WPLBlazor.Components;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using WPLBlazor.AuthenticationStateSyncer.PersistingRevalidatingAuthenticationStateProvider;
using Blazorise.LoadingIndicator;
using WPLBlazor.Data;
using WPLBlazor.Data.Services;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;


WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add Data Services
builder.Services.AddTransient<DataFactory>();
builder.Services.AddTransient<WeekHelper>();
builder.Services.AddTransient<PDataService>();
builder.Services.AddTransient<TeamHelper>();
builder.Services.AddTransient<PlayerHelpers>();
builder.Services.AddTransient<RosterHelper>();
builder.Services.AddTransient<PlayerViewService>();
builder.Services.AddDbContextFactory<WPLStatsDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WPLStatsDB"));
});



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Add Authentication Services
builder.Services.AddAuth0WebAppAuthentication(options =>
    {
        options.Domain = builder.Configuration["Auth0:Domain"];
        options.ClientId = builder.Configuration["Auth0:ClientId"];

    });

//AntiForgery
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AntiForgery>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddControllersWithViews();
// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddBlazorise(options =>
    {
        options.ProductToken = builder.Configuration["ProductToken"];
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons()
    .AddLoadingIndicator();

//Syncfusion
builder.Services.AddSyncfusionBlazor();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration.GetSection("SFLicense").Value);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

//Authentication


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapGet("/Account/Login", async (HttpContext httpContext, string returnUrl = "/") =>
{
    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .Build();

    await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
});

app.MapGet("/Account/Logout", async (HttpContext httpContext) =>
{
    var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri("/")
            .Build();

    await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
