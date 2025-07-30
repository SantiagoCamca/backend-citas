using BackEndAdministradorCitas.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndAdministradorCitas.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {

    }
    public DbSet<Negocio> Negocios { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

  }
}
