using System;
using System.Collections.Generic;
using GymMembershipApiV2.Models;

namespace GymMembershipApiV2.Models;

public partial class Socio
{
    public int SocioId { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Documento { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public DateTime FechaRegistro { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Asistencia> Asistencia { get; set; } = new List<Asistencia>();

    public virtual ICollection<Membresia> Membresia { get; set; } = new List<Membresia>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
