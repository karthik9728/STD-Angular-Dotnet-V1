using DatingApp.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using DatingApp.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DatingApp.Web.Extensions;

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
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CustomPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
