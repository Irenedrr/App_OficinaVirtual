using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using App_OficinaVirtual.DTO;

namespace App_OficinaVirtual.Services;

public class EventoService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;
    private readonly string _baseUrl = "http://localhost:8000/eventos";

    public EventoService(HttpClient httpClient, JsonSerializerOptions options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task<List<EventoResponseDto>> LeerTodosAsync()
    {
        var respuesta = await _httpClient.GetAsync(_baseUrl);
        if (!respuesta.IsSuccessStatusCode) return new List<EventoResponseDto>();

        var contenido = await respuesta.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<EventoResponseDto>>(contenido, _options);
    }

    public async Task<EventoResponseDto> LeerPorIdAsync(int id)
    {
        var respuesta = await _httpClient.GetAsync($"{_baseUrl}/{id}");
        if (!respuesta.IsSuccessStatusCode) return null;

        var contenido = await respuesta.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EventoResponseDto>(contenido, _options);
    }

    public async Task<EventoResponseDto> CrearAsync(EventoCreateDto dto)
    {
        var respuesta = await _httpClient.PostAsJsonAsync(_baseUrl, dto);
        if (!respuesta.IsSuccessStatusCode) return null;

        var contenido = await respuesta.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EventoResponseDto>(contenido, _options);
    }

    public async Task<EventoResponseDto> ActualizarAsync(int id, EventoUpdateDto dto)
    {
        var respuesta = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", dto);
        if (!respuesta.IsSuccessStatusCode) return null;

        var contenido = await respuesta.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EventoResponseDto>(contenido, _options);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var respuesta = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
        return respuesta.IsSuccessStatusCode;
    }
}
