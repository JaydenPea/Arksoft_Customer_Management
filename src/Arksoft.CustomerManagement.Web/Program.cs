using Arksoft.CustomerManagement.Application;
using Arksoft.CustomerManagement.Infrastructure;
using Arksoft.CustomerManagement.Infrastructure.Data;
using Arksoft.CustomerManagement.Web.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("Logs/arksoft-customer-management-.log", 
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        shared: true,
        flushToDiskInterval: TimeSpan.FromSeconds(1))
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "Arksoft.CustomerManagement")
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllersWithViews();

// Add API services
builder.Services.AddControllers();

// Add Swagger/OpenAPI in development
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Arksoft Customer Management API",
            Version = "v1",
            Description = "A professional customer management API built with Clean Architecture",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact
            {
                Name = "Arksoft Development Team",
                Email = "dev@arksoft.com"
            }
        });

        // Add API Key authentication to Swagger
        c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Name = "X-API-KEY",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "API Key needed to access the endpoints"
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    }
                },
                new string[] {}
            }
        });
    });
}

// Add Clean Architecture layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Seed the database in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Customer/Error");
    app.UseHsts();
}
else
{
    // Enable Swagger in development only
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Arksoft Customer Management API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Arksoft Customer Management API";
        c.DefaultModelsExpandDepth(-1); // Hide models section by default
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add API key authentication middleware
app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();

// Map MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customer}/{action=Index}/{id?}");

// Map API routes
app.MapControllers();

app.Run();