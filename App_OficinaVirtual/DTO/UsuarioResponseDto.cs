using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App_OficinaVirtual.DTO;

public class UsuarioResponseDto : UsuarioCreateDto
{
    public int Id { get; set; }

    [JsonPropertyName("eventos")]
    public List<EventoResponseDto> Eventos { get; set; }

    [JsonPropertyName("mensajes_enviados")]
    public List<MensajeResponseDto> MensajesEnviados { get; set; }

    [JsonPropertyName("mensajes_recibidos")]
    public List<MensajeResponseDto> MensajesRecibidos { get; set; }

    public bool IsSeleccionado { get; set; }


}
