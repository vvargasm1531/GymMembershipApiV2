using System;
using System.ComponentModel.DataAnnotations;

public class Contacto
{
    [Key]
    public int ContactoID { get; set; }

    [Required]
    public string Nombre { get; set; } = null!;

    [Required]
    public string Correo { get; set; } = null!;

    public string? Mensaje { get; set; }

    public DateTime FechaEnvio { get; set; }
}