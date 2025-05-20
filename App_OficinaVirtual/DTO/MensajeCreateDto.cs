using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App_OficinaVirtual.DTO;

public class MensajeCreateDto
{
    [JsonPropertyName("contenido")]
    public string Contenido { get; set; }

    [JsonPropertyName("emisor_id")]
    public int EmisorId { get; set; }

    [JsonPropertyName("receptor_id")]
    public int ReceptorId { get; set; }

    [JsonPropertyName("fecha")]
    public DateTime? Fecha { get; set; }
}
