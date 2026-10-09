using System;
using System.Collections.Generic;

namespace GymMembershipApiV2.Models;

public partial class MetodosPago
{
    public int MetodoPagoId { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
