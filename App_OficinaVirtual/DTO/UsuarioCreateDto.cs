using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace App_OficinaVirtual.DTO;

public class UsuarioCreateDto
{
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("contrasena")]
    public string Contrasena { get; set; }

    [JsonPropertyName("rol_id")]
    public int RolId { get; set; }

    [JsonPropertyName("imagen_url")]
    public string ImagenUrl { get; set; }

    [JsonPropertyName("personaje")]
    public string Personaje { get; set; }

    [JsonPropertyName("oficina")]
    public string Oficina { get; set; }

    [JsonPropertyName("estado")]
    public string Estado { get; set; } = "conectado";
}
