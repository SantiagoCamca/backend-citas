using System.ComponentModel.DataAnnotations;

namespace BackEndAdministradorCitas.Models
{
  public class Usuario
  {
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Correo { get; set; } = string.Empty;

    [Required]
    public string ContrasenaHash { get; set; } = string.Empty;
  }
}
