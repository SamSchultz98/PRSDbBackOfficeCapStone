using PRSDbBackOfficeCapStone.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//Getting the connection string
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("Main"));
});

builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline.

//Opens up the security
app.UseCors(x=>
           x.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
