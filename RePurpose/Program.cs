using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using RePurpose.Configurations;
using RePurpose_Models.Entities;
using RePurpose_Models.Mapping;
using RePurpose_Utility.Setting;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddDbContext<RePurposeContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDependenceInjection();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddAutoMapper(typeof(GeneralProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{

}

app.UseSwagger(c =>
{
        c.RouteTemplate = "/api/swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI(c =>
{
        c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "production");
        c.RoutePrefix = "api/swagger";
});
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
