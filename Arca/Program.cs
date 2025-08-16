using Arca.Api.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// CORS (si pruebas API desde navegador)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMvc", policy => policy
        .WithOrigins("https://localhost:44384", "http://localhost:44384")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Arca.Api", Version = "v1" });
});

// DI: fábrica + repos
builder.Services.AddSingleton<SqlConnectionFactory>();
builder.Services.AddScoped<UbicacionRepository>();
builder.Services.AddScoped<SemillaRepository>();
builder.Services.AddScoped<EspecieRepository>();
builder.Services.AddScoped<UsuarioRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Arca.Api v1"));

app.UseHttpsRedirection();
app.UseCors("AllowMvc");
app.MapControllers();
app.Run();