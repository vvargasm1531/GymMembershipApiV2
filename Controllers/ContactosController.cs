using GymMembershipApiV2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace GymMembershipApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactosController : ControllerBase
    {
        private readonly GymMembresiasContext _db;

        public ContactosController(GymMembresiasContext db)
        {
            _db = db;
        }

        public class NuevoContacto
        {
            public string Nombre { get; set; } = "";
            public string Correo { get; set; } = "";
            public string? Mensaje { get; set; }
        }

        private static void Param(DbCommand cmd, string name, object? value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var lista = new List<object>();

            await _db.Database.OpenConnectionAsync();
            try
            {
                using var cmd = _db.Database.GetDbConnection().CreateCommand();
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
                await _db.Database.CloseConnectionAsync();
            }

            return Ok(lista);
        }

        [HttpPost]
        public async Task<IActionResult> Post(NuevoContacto contacto)
        {
            if (string.IsNullOrWhiteSpace(contacto.Nombre) || string.IsNullOrWhiteSpace(contacto.Correo))
                return BadRequest("Nombre y correo son obligatorios");

            await _db.Database.OpenConnectionAsync();
            try
            {
                using var cmd = _db.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = "INSERT INTO Contactos (Nombre, Correo, Mensaje) VALUES (@n, @c, @m)";
                Param(cmd, "@n", contacto.Nombre.Trim());
                Param(cmd, "@c", contacto.Correo.Trim());
                Param(cmd, "@m", string.IsNullOrWhiteSpace(contacto.Mensaje) ? null : contacto.Mensaje.Trim());
                await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                await _db.Database.CloseConnectionAsync();
            }

            return Ok(new { mensaje = "Contacto guardado" });
        }
    }
}
