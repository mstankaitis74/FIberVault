using FiberVault.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var cs = builder.Configuration.GetConnectionString("Db")
         ?? throw new InvalidOperationException("Missing ConnectionStrings:Db");

builder.Services.AddDbContext<FiberVaultDbContext>(opt =>
    opt.UseNpgsql(cs, o => o.UseNetTopologySuite()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/health", () => Results.Ok("OK"));
app.MapControllers();

app.Run();