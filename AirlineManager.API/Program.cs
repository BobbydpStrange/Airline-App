using AirlineManager.API;
using AirlineManager.API.Data;
using AirlineManager.API.Maintenance;
using AirlineManager.API.Repository;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Web.Http;
using AirlineManager.API;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;
using Serilog;
using Serilog.Exceptions;

public partial class Program {
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        /***********************************/
        builder.Logging.ClearProviders();
        //builder.Logging.AddJsonConsole();
        //builder.Logging.AddDebug();
        builder.Host.UseSerilog((context, loggerConfig) =>
        {
            loggerConfig.WriteTo
            .Console()
            .Enrich.WithExceptionDetails()
            .Enrich.With<ActivityEnricher>()
            .WriteTo.Seq("http://localhost:5341");
        });
        //builder.Services.Addapplicationinsightstelemetry();
        /***************************************/

        //______________________________________________
        builder.Services.AddHealthChecks();
          // .AddDbContextCheck<context>();
        //_______________________________________________
        builder.Services.AddProblemDetails(opts =>
        {
            opts.IncludeExceptionDetails = (ctx, ex) => false;
            opts.OnBeforeWriteDetails = (ctx, dtls) =>
            {
                if (dtls.Status == 500)
                {
                    dtls.Detail = "An error occurred in our API. Use the trace id when contacting us.";
                }
            };
            opts.Rethrow<SqliteException>();
            opts.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        });
        //******************************************************************
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://demo.duendesoftware.com";
                options.Audience = "api";
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    NameClaimType = "email"
                };
            });
        //*******************************************************************

        //builder.Logging.AddFilter("MaintenanceController", Loglevel.Debug);

        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var tracePath = Path.Join(path, $"Log_Airline_{DateTime.Now.ToString("yyyyMMdd-HHmm")}.txt");
        Trace.Listeners.Add(new TextWriterTraceListener(System.IO.File.CreateText(tracePath)));
        Trace.AutoFlush= true;

       

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
            {
                c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
                c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidAudience = builder.Configuration["Auth0:Audience"],
                    ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}"
                };
            });

        // Add services to the container.

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerOptions>();
        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
            });
        });

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        builder.Services.AddDbContext<AirmanDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration["pg"]);
        });

        builder.Services.AddScoped<AirmanDbContext>();
        builder.Services.AddScoped<IDataRepository, AirmanRepo>();
        builder.Services.AddScoped<MaintenanceApp>();
        builder.Services.AddScoped<AirmanEmailService>(x=> new AirmanEmailService(builder.Configuration["EmailApi"]));

        var app = builder.Build();

        app.UseMiddleware<CriticalExceptionMiddleware>();
        app.UseProblemDetails();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.OAuthClientId("interactive.public.short");
                options.OAuthAppName("Airline API");
                options.OAuthUsePkce();
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseMiddleware<UserScopeMiddleware>();
        app.UseAuthorization();

        app.MapControllers().RequireAuthorization();

        //______________________________________-
        app.MapHealthChecks("health").AllowAnonymous();
        //________________________________________-

        app.Run();
    }
}

public partial class Program { }
