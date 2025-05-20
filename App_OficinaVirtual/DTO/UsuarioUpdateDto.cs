using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App_OficinaVirtual.DTO;

public class UsuarioUpdateDto
{
    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("contrasena")]
    public string? Contrasena { get; set; }

    [JsonPropertyName("rol_id")]
    public int? RolId { get; set; }

    [JsonPropertyName("imagen_url")]
    public string? ImagenUrl { get; set; }

    [JsonPropertyName("personaje")]
    public string? Personaje { get; set; }

    [JsonPropertyName("oficina")]
    public string? Oficina { get; set; }

    [JsonPropertyName("estado")]
    public string? Estado { get; set; }
}

