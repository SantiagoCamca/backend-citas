using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackEndAdministradorCitas.Data;
using BackEndAdministradorCitas.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using BackEndAdministradorCitas.Models.DTOs;

namespace BackEndAdministradorCitas.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
      _context = context;
      _config = config;
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registro(UsuarioDto request)
    {
      if (await _context.Usuarios.AnyAsync(u => u.Correo == request.Correo))
        return BadRequest("El usuario ya existe");

      var nuevoUsuario = new Usuario
      {
        Nombre = request.Correo, // O puedes pedir nombre aparte en el DTO
        Correo = request.Correo,
        ContrasenaHash = HashPassword(request.Contrasena)
      };

      _context.Usuarios.Add(nuevoUsuario);
      await _context.SaveChangesAsync();

      return Ok("Usuario registrado");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UsuarioDto request)
    {
      var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == request.Correo);
      if (usuario == null || !VerifyPassword(request.Contrasena, usuario.ContrasenaHash))
        return Unauthorized("Credenciales incorrectas");

      var token = GenerateJwtToken(usuario);
      return Ok(new { token });
    }

    private string GenerateJwtToken(Usuario usuario)
    {
      var claims = new[]
      {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Correo)
            };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["JwtSettings:ExpirationInMinutes"]));

      var token = new JwtSecurityToken(
          issuer: _config["JwtSettings:Issuer"],
          audience: _config["JwtSettings:Audience"],
          claims: claims,
          expires: expiration,
          signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
      byte[] salt = Encoding.UTF8.GetBytes("salt_fijo_en_produccion_usar_aleatorio"); // En producción usar salt único
      return Convert.ToBase64String(KeyDerivation.Pbkdf2(
          password: password,
          salt: salt,
          prf: KeyDerivationPrf.HMACSHA256,
          iterationCount: 10000,
          numBytesRequested: 32));
    }

    private bool VerifyPassword(string password, string storedHash)
    {
      return HashPassword(password) == storedHash;
    }
  }
}
