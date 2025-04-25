using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApplication1.Data;
/// <summary>
///  <Authorship>I, Ali Abdulsameea, 000857347 certify that this material is my original work.
/// No other person's work has been used without due acknowledgement.</Authorship>
/// This is main program to run and connect to data context
/// <author>Ali Abubaker - 000857347</author>
/// </summary>
var builder = WebApplication.CreateBuilder(args);// method sets up Kestrel as the default web server. 
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Healthcare Management API", 
        Version = "v1",
        Description = "API for managing health data."
    });
});
builder.Services.AddDbContext<HealthDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HealthDataContext") ?? throw new InvalidOperationException("Connection string 'HealthDataContext' not found.")));
// Add services to the container.
builder.Services.AddControllers()
    .AddXmlSerializerFormatters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();