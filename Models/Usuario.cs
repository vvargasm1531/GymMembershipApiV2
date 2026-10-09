using System;
using System.Collections.Generic;

namespace GymMembershipApiV2.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string ContrasenaHash { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public int? SocioId { get; set; }

    public int? EmpleadoId { get; set; }

    public bool Activo { get; set; }

    public DateTime? UltimoAcceso { get; set; }

    public virtual Empleado? Empleado { get; set; }

    public virtual Socio? Socio { get; set; }
}
