using System.Data.SqlClient;
using Arca.Api.Repositories;   
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1) Controllers (Web API)
builder.Services.AddControllers();

// 2) Swagger (UI para probar el API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Arca.Api",
        Version = "v1",
        Description = "API REST para Arca del Día Cero"
    });
});

// 3) CORS (para que tu UI MVC 5 pueda llamar al API desde otro puerto)
builder.Services.AddCors(o => o.AddPolicy("default", p =>
    p.AllowAnyOrigin()
     .AllowAnyHeader()
     .AllowAnyMethod()));

// 4) Factory para conexiones ADO.NET (lee la ConnectionString de appsettings.json)
builder.Services.AddSingleton(new SqlConnectionFactory(
    builder.Configuration.GetConnectionString("DefaultConnection")!));

// 5) Repositorios (registra aquí los que vayas usando)
builder.Services.AddScoped<SemillaRepository>();
builder.Services.AddScoped<EspecieRepository>();
builder.Services.AddScoped<UbicacionRepository>();
// builder.Services.AddScoped<UsuarioRepository>();
// builder.Services.AddScoped<ReporteRepository>();

var app = builder.Build();

// --- Pipeline ---

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Arca.Api v1");
    // Deja la UI en /swagger (por defecto)
    // c.RoutePrefix = string.Empty; // descomenta si quieres Swagger en "/"
});

// CORS
app.UseCors("default");

// (Auth/Authorization irán después cuando agreguemos JWT)
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();

public record SqlConnectionFactory(string ConnectionString)
{
    public SqlConnection Create() => new SqlConnection(ConnectionString);
}