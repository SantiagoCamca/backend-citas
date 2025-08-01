﻿namespace BackEndAdministradorCitas.Helpers
{
  public class JwtSettings
  {
    public string SecretKey { get; set; } = string.Empty;
    public int ExpirationInMinutes { get; set; }
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
  }
}
