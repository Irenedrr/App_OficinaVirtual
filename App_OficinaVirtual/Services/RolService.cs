using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using App_OficinaVirtual.DTO;

namespace App_OficinaVirtual.Services;

public class RolService
{
    private readonly JsonSerializerOptions _options;
    private readonly string _baseUrl = "http://localhost:8000/roles";

    public RolService(JsonSerializerOptions options)
    {
        _options = options;
    }

    public async Task<List<RolResponseDto>> LeerTodosAsync()
    {
        var _httpClient = Helpers.HttpClientHelper.GetClient();
        var respuesta = await _httpClient.GetAsync($"{_baseUrl}/");
        if (!respuesta.IsSuccessStatusCode) return new List<RolResponseDto>();

        var contenido = await respuesta.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<RolResponseDto>>(contenido, _options);
    }

    public async Task<RolResponseDto> LeerPorIdAsync(int id)
    {
        var _httpClient = Helpers.HttpClientHelper.GetClient();
        var respuesta = await _httpClient.GetAsync($"{_baseUrl}/{id}");
        if (!respuesta.IsSuccessStatusCode) return null;

        var contenido = await respuesta.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<RolResponseDto>(contenido, _options);
    }

    public async Task<RolResponseDto> CrearAsync(RolCreateDto dto)
    {
        var _httpClient = Helpers.HttpClientHelper.GetClient();
        var respuesta = await _httpClient.PostAsJsonAsync($"{_baseUrl}/", dto);
        if (!respuesta.IsSuccessStatusCode) return null;

        var contenido = await respuesta.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<RolResponseDto>(contenido, _options);
    }

    public async Task<RolResponseDto> ActualizarAsync(int id, RolUpdateDto dto)
    {
        var _httpClient = Helpers.HttpClientHelper.GetClient();
        var respuesta = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", dto);
        if (!respuesta.IsSuccessStatusCode) return null;

        var contenido = await respuesta.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<RolResponseDto>(contenido, _options);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var _httpClient = Helpers.HttpClientHelper.GetClient();
        var respuesta = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
        return respuesta.IsSuccessStatusCode;
    }
}