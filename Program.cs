using GymMembershipApiV2.Data;
using GymMembershipApiV2.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =========================
// SERVICIOS
// =========================

// CORS (una sola politica)
builder.Services.AddCors(options =>
{
    options.AddPolicy("dev", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// OpenAPI
builder.Services.AddOpenApi();

// Base de datos (SQL Server)
builder.Services.AddDbContext<GymMembresiasContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// =========================
// CREAR APP
// =========================

var app = builder.Build();

// CORS: una sola vez, antes de los endpoints
app.UseCors("dev");

// =========================
// CREAR TABLA CONTACTOS SI NO EXISTE
// =========================

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<GymMembresiasContext>();
        db.Database.ExecuteSqlRaw(@"
            IF OBJECT_ID('dbo.Contactos', 'U') IS NULL
            CREATE TABLE dbo.Contactos (
                ContactoID INT IDENTITY(1,1) PRIMARY KEY,
                Nombre NVARCHAR(150) NOT NULL,
                Correo NVARCHAR(150) NOT NULL,
                Mensaje NVARCHAR(1000) NULL,
                FechaEnvio DATETIME2 NOT NULL DEFAULT SYSDATETIME()
            );");
        Console.WriteLine(">>> Tabla Contactos lista");
    }
    catch (Exception ex)
    {
        Console.WriteLine(">>> No se pudo crear la tabla Contactos: " + ex.Message);
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// =========================
// PRUEBA
// =========================

app.MapGet("/", () => "API funcionando");
app.MapGet("/ping", () => "pong");

// =========================
// SOCIOS
// =========================

app.MapGet("/api/socios", async (GymMembresiasContext db) =>
{
    var socios = await db.Socios.ToListAsync();
    return Results.Ok(socios);
});

app.MapPost("/api/socios", async (GymMembresiasContext db, Socio socio) =>
{
    socio.SocioId = 0;
    socio.FechaRegistro = DateTime.Now;
    socio.Activo = true;

    try
    {
        db.Socios.Add(socio);
        await db.SaveChangesAsync();
        return Results.Ok(socio);
    }
    catch (DbUpdateException ex)
    {
        // El mensaje real de SQL Server (duplicados, nulos, etc.) llega al navegador
        return Results.BadRequest(ex.InnerException?.Message ?? ex.Message);
    }
});

// =========================
// CONTACTOS
// =========================

app.MapGet("/api/contactos", async (GymMembresiasContext db) =>
{
    var lista = new List<object>();

    await db.Database.OpenConnectionAsync();
    try
    {
        using var cmd = db.Database.GetDbConnection().CreateCommand();
        cmd.CommandText = "SELECT ContactoID, Nombre, Correo, Mensaje, FechaEnvio FROM Contactos ORDER BY FechaEnvio DESC";

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            lista.Add(new
            {
                contactoID = reader.GetInt32(0),
                nombre = reader.GetString(1),
                correo = reader.GetString(2),
                mensaje = reader.IsDBNull(3) ? null : reader.GetString(3),
                fechaEnvio = reader.GetDateTime(4)
            });
        }
    }
    finally
    {
        await db.Database.CloseConnectionAsync();
    }

    return Results.Ok(lista);
});

app.MapPost("/api/contactos", async (GymMembresiasContext db, NuevoContacto c) =>
{
    if (string.IsNullOrWhiteSpace(c.Nombre) || string.IsNullOrWhiteSpace(c.Correo))
        return Results.BadRequest("Nombre y correo son obligatorios");

    var nombre = c.Nombre.Trim();
    var correo = c.Correo.Trim();
    object mensaje = string.IsNullOrWhiteSpace(c.Mensaje) ? DBNull.Value : c.Mensaje.Trim();

    try
    {
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"INSERT INTO Contactos (Nombre, Correo, Mensaje) VALUES ({nombre}, {correo}, {mensaje})");
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.InnerException?.Message ?? ex.Message);
    }

    return Results.Ok(new { mensaje = "Contacto guardado" });
});

// =========================
// EJECUTAR API
// =========================

app.Run();

record NuevoContacto(string? Nombre, string? Correo, string? Mensaje);