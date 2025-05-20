using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App_OficinaVirtual.DTO;

public class EventoCreateDto
{
    [JsonPropertyName("titulo")]
    public string Titulo { get; set; }

    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; }

    [JsonPropertyName("creador_id")]
    public int CreadorId { get; set; }

    [JsonPropertyName("fecha_inicio")]
    public DateTime FechaInicio { get; set; }

    [JsonPropertyName("fecha_fin")]
    public DateTime FechaFin { get; set; }

    [JsonPropertyName("tipo")]
    public string Tipo { get; set; } = "reunion"; // valor por defecto como en Python

    [JsonPropertyName("participantes")]
    public List<int> Participantes { get; set; } = new();
}

