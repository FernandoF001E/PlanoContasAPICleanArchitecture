using Microsoft.EntityFrameworkCore;
using PlanoContasApi.Application.Interfaces;
using PlanoContasApi.Application.Service;
using PlanoContasApi.Domain.Interfaces;
using PlanoContasApi.Infrastructure.Persistence;
using PlanoContasApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	if (File.Exists(xmlPath))
	{
		c.IncludeXmlComments(xmlPath);
	}
});

builder.Services.AddScoped<IContaRepository, ContaRepository>();
builder.Services.AddScoped<IContaService, ContaService>();

var app = builder.Build();

// Aplicar migrations automaticamente
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.Use(async (context, next) =>
{
	if (context.Request.Path == "/")
	{
		context.Response.Redirect("/swagger");
		return;
	}
	await next();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
