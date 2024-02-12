using DatingApp.Application;
using DatingApp.Infrastructure.Common;
using DatingApp.Infrastructure.DbContexts;
using DatingApp.Web.Extensions;
using DatingApp.Web.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Service
builder.Services.AddApplicationServices();
builder.Services.AddWebAppServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
#endregion




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Dating App",
        Version = "v1"
    });

    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    //options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();
}

//Middleware

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("CustomPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();

    await Seed.SeedUsers(context);
}
catch (Exception ex)
{

   var logger  = services.GetService<ILogger<Program>>();

    logger.LogError(ex,"An error occurred during migration");
}

app.Run();
