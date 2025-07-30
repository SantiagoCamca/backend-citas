using BackEndAdministradorCitas.Data;
using BackEndAdministradorCitas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEndAdministradorCitas.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize]
  public class NegocioController : ControllerBase
  {
    private readonly AppDbContext _context;

    public NegocioController(AppDbContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Negocio>>> GetNegocios()
    {
      return await _context.Negocios.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Negocio>> GetNegocio(int id)
    {
      var negocio = await _context.Negocios.FindAsync(id);
      if (negocio == null)
        return NotFound();
      return negocio;
    }

    [HttpPost]
    public async Task<ActionResult<Negocio>> CrearNegocio(Negocio negocio)
    {
      _context.Negocios.Add(negocio);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetNegocio), new { id = negocio.Id }, negocio);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarNegocio(int id, Negocio negocio)
    {
      if (id != negocio.Id)
        return BadRequest();

      _context.Entry(negocio).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!NegocioExists(id))
          return NotFound();
        else
          throw;
      }

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarNegocio(int id)
    {
      var negocio = await _context.Negocios.FindAsync(id);
      if (negocio == null)
        return NotFound();

      _context.Negocios.Remove(negocio);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool NegocioExists(int id)
    {
      return _context.Negocios.Any(e => e.Id == id);
    }
  }
}
