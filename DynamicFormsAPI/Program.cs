// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast");

// app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
using DynamicFormsAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using DynamicFormsAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Agregar Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar el repositorio
builder.Services.AddScoped<FormularioRepository>();

// Habilitar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", builder =>
        builder.WithOrigins("http://localhost:3000") // Permitir solicitudes solo desde localhost:3000
               .AllowAnyHeader() // Permitir cualquier encabezado
               .AllowAnyMethod()); // Permitir cualquier método (GET, POST, PUT, DELETE, etc.)
});

// Agregar servicios para los controladores
builder.Services.AddControllers();

var app = builder.Build();

// Usar CORS antes de las rutas y la autorización
app.UseCors("AllowLocalhost3000"); // Usar la política de CORS configurada

app.UseRouting();
app.UseAuthorization();

// Mapear los controladores
app.MapControllers();

// Iniciar la aplicación
app.Run();
