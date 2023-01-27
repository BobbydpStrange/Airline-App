using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using AirlineManager.Blazor.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using AirlineManager.Blazor.Handlers;
using Microsoft.IdentityModel.Tokens;
using TokenHandler = AirlineManager.Blazor.Handlers.TokenHandler;
using System.IdentityModel.Tokens.Jwt;
using AirlineManager.Blazor.Services;
using Blazored.Modal;
using Serilog;
using Serilog.Exceptions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
//builder.Logging.AddJsonConsole();
//builder.services.addapplicationinsightstelemetry();
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.WriteTo
    .Console()
    .Enrich.WithExceptionDetails()
    .WriteTo.Seq("http://localhost:5341");
});
//**************************************************
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://demo.duendesoftware.com";
        options.ClientId = "interactive.confidential";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("api");
        options.Scope.Add("offline_access");
        options.GetClaimsFromUserInfoEndpoint = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "email"
        };
        options.SaveTokens = true;
    });
builder.Services.AddHttpContextAccessor();
//************************************************

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    logging.MediaTypeOptions.AddText("applocation/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});
// Add services to the container.
//______________________________________________
builder.Services.AddHealthChecks()
    .AddIdentityServer(new Uri("https://demo.duendesoftware.com"), failureStatus: HealthStatus.Degraded);
//_______________________________________________
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<WeatherForecastService>();
builder.Services.AddBlazoredModal();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddOpenIdConnect("Auth0", options =>
    {
        // Set the authority to your Auth0 domain
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";

        // Configure the Auth0 Client ID and Client Secret
        options.ClientId = builder.Configuration["Auth0:ClientId"];
        options.ClientSecret = builder.Configuration["ClientSecret"];

        // Set response type to code
        options.ResponseType = OpenIdConnectResponseType.Code;

        // Configure the scope
        options.Scope.Clear();
        options.Scope.Add("openid profile email");

        //This is needed to save a token for the api
        options.SaveTokens = true;

        // Set the callback path, so Auth0 will call back to http://localhost:3000/callback
        // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard
        options.CallbackPath = new PathString("/callback");

        // Configure the Claims Issuer to be Auth0
        options.ClaimsIssuer = "Auth0";

        options.Events = new OpenIdConnectEvents
        {
            // handle the logout redirection 
            OnRedirectToIdentityProviderForSignOut = (context) =>
            {
                var logoutUri = $"https://{builder.Configuration["Auth0:Domain"]}/v2/logout?client_id={builder.Configuration["Auth0:ClientId"]}";

                var postLogoutUri = context.Properties.RedirectUri;
                if (!string.IsNullOrEmpty(postLogoutUri))
                {
                    if (postLogoutUri.StartsWith("/"))
                    {
                        // transform to absolute
                        var request = context.Request;
                        postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                    }
                    logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                }

                context.Response.Redirect(logoutUri);
                context.HandleResponse();

                return Task.CompletedTask;
            },
            OnRedirectToIdentityProvider = context =>
            {
                // The context's ProtocolMessage can be used to pass along additional query parameters
                // to Auth0's /authorize endpoint.
                // 
                // Set the audience query parameter to the API identifier to ensure the returned Access Tokens can be used
                // to call protected endpoints on the corresponding API.
                context.ProtocolMessage.SetParameter("audience", builder.Configuration["Auth0:Audience"]);

                return Task.FromResult(0);
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // map the claimsPrincipal's roles to the roles claim
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/roles",
            NameClaimType = "name"
        };
    }
);

var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
builder.Services.AddW3CLogging(opts =>
{
    opts.LoggingFields = Microsoft.AspNetCore.HttpLogging.W3CLoggingFields.All;
    opts.FileSizeLimit = 5 * 1024 * 1024;
    opts.RetainedFileCountLimit = 2;
    opts.FileName = "Airline-W3C-UI";
    opts.LogDirectory = Path.Combine(path, "logs");
    opts.FlushInterval = TimeSpan.FromSeconds(2);

});
//Authenticated http client for the api

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenHandler>();
builder.Services.AddHttpClient("AirmanAPI",
      client => client.BaseAddress = new Uri(builder.Configuration["AirmanApiBaseUrl"]))
    .AddHttpMessageHandler<TokenHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
  .CreateClient("AirmanAPI"));

builder.Services.AddScoped<IMaintenanceDataService, MaintenanceService>();

var app = builder.Build();
app.UseHttpLogging();
app.UseW3CLogging();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
//}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages().RequireAuthorization();
//______________________________________-
app.MapHealthChecks("health").AllowAnonymous();
//________________________________________-
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
