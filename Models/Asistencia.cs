using System;
using System.Collections.Generic;

namespace GymMembershipApiV2.Models;

public partial class Asistencia
{
    public int AsistenciaId { get; set; }

    public int SocioId { get; set; }

    public DateTime FechaHoraEntrada { get; set; }

    public DateTime? FechaHoraSalida { get; set; }

    public virtual Socio Socio { get; set; } = null!;
}
