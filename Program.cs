var builder = WebApplication.CreateBuilder(args);

// Registrar servicios requeridos
builder.Services.AddOpenApi();

var app = builder.Build();

// Configuración de middlewares
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ================================================================
// MTS ENDPOINTS DE PRUEBA
// ================================================================

// 1. Endpoint simple (Retorna texto plano)
app.MapGet("/", () => "API en .NET 10 ejecutándose correctamente.");

// 2. Endpoint con objeto anónimo (Retorna JSON automático)
app.MapGet("/api/info", () => new
{
    Curso = "Backend .NET 10",
    Estado = "Activo",
    Fecha = DateTime.Now
});

// 3. Endpoint con parámetros por ruta (Route Parameter)
app.MapGet("/api/saludar/{nombre}", (string nombre) =>
{
    return $"¡Hola, {nombre}! Hola desde Hot Reload.";

});

app.Run();