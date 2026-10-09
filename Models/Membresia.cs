using System;
using System.Collections.Generic;

namespace GymMembershipApiV2.Models;

public partial class Membresia
{
    public int MembresiaId { get; set; }

    public int SocioId { get; set; }

    public int PlanId { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual PlanesMembresium Plan { get; set; } = null!;

    public virtual Socio Socio { get; set; } = null!;
}
