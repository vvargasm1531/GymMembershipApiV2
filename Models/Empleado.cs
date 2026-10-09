using System;
using System.Collections.Generic;

namespace GymMembershipApiV2.Models;

public partial class Empleado
{
    public int EmpleadoId { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Documento { get; set; } = null!;

    public string Cargo { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public DateOnly FechaContrato { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
