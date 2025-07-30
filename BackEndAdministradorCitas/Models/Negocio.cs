using System.ComponentModel.DataAnnotations;

namespace BackEndAdministradorCitas.Models
{
  public class Negocio
  {
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Categoria { get; set; } // Ej: Barbería, Consultorio

    public string? Descripcion { get; set; }

    public string? ImagenUrl { get; set; } // Si luego queremos mostrar logo o foto
  }
}
