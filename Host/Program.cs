using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Filters;
using OggettoCase.Configuration;
using OggettoCase.DataAccess.Context;
using OggettoCase.DataAccess.Interfaces;
using OggettoCase.DataAccess.Models.Calendars;
using OggettoCase.DataAccess.Repositories;
using OggettoCase.DataContracts.Interfaces;
using OggettoCase.Hubs;
using OggettoCase.Interfaces;
using OggettoCase.Middleware;
using OggettoCase.Services;
using BaseConfiguration = OggettoCase.Configuration.BaseConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
    configuration.Filter.ByExcluding(Matching.WithProperty<string>("RequestPath", p => p == "/health"));
});

const string baseSectionConfig = "BaseConfiguration";

var swaggerConfig = builder.Configuration.GetSection(baseSectionConfig).Get<BaseConfiguration>()?.SwaggerConfig;
var baseConfiguration = builder.Configuration.GetSection(baseSectionConfig).Get<BaseConfiguration>();
if (baseConfiguration is null)
{
    throw new InvalidDataException("Base configuration was not provided");
}

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
    );
builder.Services.AddTransient<ProblemDetailsFactory, BaseProblemDetailsFactory>();

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

builder.Services.AddHttpClient();

builder.Services.AddSignalR();

var databaseConfig = baseConfiguration.DatabaseConfig;
builder.Services.AddDbContext<DatabaseContext>(options => DatabaseContextFactory.CreateDbContext(options, databaseConfig.FullConnectionString));
builder.Services.AddScoped<IDatabaseContextFactory>(_ => new DatabaseContextFactory(databaseConfig.FullConnectionString));

var jwtOptions  = builder.Configuration.GetSection(JwtConfiguration.OptionsKey);
builder.Services.Configure<JwtConfiguration>(jwtOptions);

var jwtOptionsSettings = jwtOptions.Get<JwtConfiguration>();
if (jwtOptionsSettings is null)
{
    throw new ArgumentNullException(nameof(jwtOptionsSettings));
}
        
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        SaveSigninToken = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptionsSettings?.Issuer,
        ValidAudience = jwtOptionsSettings?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptionsSettings?.Key ?? string.Empty))
    };
});

builder.Services.AddAuthorization(options => {
    options.AddPolicy("ExcludeRoles", policy => policy.AddRequirements(new CustomRoleRequirement()));
});


builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ICalendarEventService, CalendarEventService>();
builder.Services.AddScoped<ICalendarRepository, CalendarRepository>();

builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();

builder.Services.AddTransient<IGoogleService, GoogleService>();

const string customPolicyName = "CustomCors";
var origins = (builder.Configuration.GetSection($"CorsConfiguration:Origins")
        .Get<string[]>() ?? Array.Empty<string>())
    .Where(s => !string.IsNullOrEmpty(s))
    .ToArray();

builder.Services.AddCors(options => options.AddPolicy(name: customPolicyName,
    corsPolicyBuilder =>
    {
        
        corsPolicyBuilder.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
        
        /*
        if (!origins.Any() || origins.Any(o => o.ToLower().Equals("all")))
        {
            corsPolicyBuilder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials();
        }
        else
        {
            corsPolicyBuilder.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins(origins.ToArray());
        }*/
    }));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();;

if (swaggerConfig is { IsEnabled: true })
{
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = $"Swagger of Template service",
            Description = "Swagger of Template service build with .NET CORE 8.0"
        });
        
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
            In = ParameterLocation.Header, 
            Description = "Please insert JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey 
        });
        
        options.EnableAnnotations();
        options.SupportNonNullableReferenceTypes();
        var xmlFilename = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
        if (File.Exists(xmlFilename))
        {
            options.IncludeXmlComments(xmlFilename, true);
        }
    });
}


var app = builder.Build();
app.UseDeveloperExceptionPage();
if (swaggerConfig is { IsEnabled: true })
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthChecks("/health");

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.UseCors(customPolicyName);
app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapControllers();
app.MapHub<CommentsHub>("/comments-hub");
app.Run();