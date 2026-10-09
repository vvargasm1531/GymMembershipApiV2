using System;
using System.Collections.Generic;

namespace GymMembershipApiV2.Models;

public partial class Pago
{
    public int PagoId { get; set; }

    public int MembresiaId { get; set; }

    public int MetodoPagoId { get; set; }

    public decimal Monto { get; set; }

    public DateTime FechaPago { get; set; }

    public string? Referencia { get; set; }

    public int? RegistradoPor { get; set; }

    public virtual Membresia Membresia { get; set; } = null!;

    public virtual MetodosPago MetodoPago { get; set; } = null!;

    public virtual Empleado? RegistradoPorNavigation { get; set; }
}
