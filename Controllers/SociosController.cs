using GymMembershipApiV2.Data;
using GymMembershipApiV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace GymMembershipApiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SociosController : ControllerBase
    {
        private readonly GymMembresiasContext _db;

        public SociosController(GymMembresiasContext db)
        {
            _db = db;
        }

        // ---------- SOCIOS ----------

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Socio>>> GetSocios()
        {
            return await _db.Socios.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Socio>> GetSocio(int id)
        {
            var socio = await _db.Socios.FindAsync(id);
            if (socio == null) return NotFound();
            return socio;
        }

        [HttpPost]
        public async Task<IActionResult> PostSocio(Socio socio)
        {
            _db.Socios.Add(socio);
            await _db.SaveChangesAsync();
            return Ok(socio);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSocio(int id, Socio socio)
        {
            if (id != socio.SocioId)
                return BadRequest("El id no coincide");

            _db.Entry(socio).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Socios.AnyAsync(s => s.SocioId == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSocio(int id)
        {
            var socio = await _db.Socios.FindAsync(id);
            if (socio == null) return NotFound();

            _db.Socios.Remove(socio);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // ---------- CONTACTOS (ruta: /api/Contactos) ----------

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

        [HttpGet("~/api/Contactos")]
        public async Task<IActionResult> GetContactos()
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

        [HttpPost("~/api/Contactos")]
        public async Task<IActionResult> PostContacto(NuevoContacto contacto)
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