using System;
using System.Collections.Generic;

namespace GymMembershipApiV2.Models;

public partial class PlanesMembresium
{
    public int PlanId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int DuracionDias { get; set; }

    public decimal Precio { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Membresia> Membresia { get; set; } = new List<Membresia>();
}
