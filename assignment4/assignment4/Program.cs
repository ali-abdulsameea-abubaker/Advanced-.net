using assignment4.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
/// <summary>
/// I, Ali Abubaker, 000857347, certify that this material is my original work. No other person's work has been used without due acknowledgement and I have not made my work available to anyone else.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();




builder.Services.AddDbContext<NorthwindContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("northwindDB")));


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
