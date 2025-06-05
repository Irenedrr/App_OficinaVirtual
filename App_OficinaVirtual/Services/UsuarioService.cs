using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using App_OficinaVirtual.DTO;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace App_OficinaVirtual.Services;

public class UsuarioService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    private readonly string _baseUrl = "http://localhost:8000/usuarios";

    public UsuarioService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    private void ConfigurarToken()
    {
        var token = Preferences.Get("access_token", string.Empty);
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<UsuarioResponseDto>> LeerTodosAsync()
    {
        try
        {
            ConfigurarToken();
            var respuesta = await _httpClient.GetAsync(_baseUrl);
            
            if (!respuesta.IsSuccessStatusCode)
            {
                var errorContent = await respuesta.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error al obtener usuarios: {errorContent}");
                throw new Exception($"Error al obtener usuarios: {respuesta.StatusCode}");
            }

            var contenido = await respuesta.Content.ReadAsStringAsync();
            Debug.WriteLine($"Respuesta de usuarios: {contenido}");
            return JsonSerializer.Deserialize<List<UsuarioResponseDto>>(contenido, _options);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error en LeerTodosAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<UsuarioResponseDto> LeerPorIdAsync(int id)
    {
        try
        {
            ConfigurarToken();
            var respuesta = await _httpClient.GetAsync($"{_baseUrl}/{id}");
            
            if (!respuesta.IsSuccessStatusCode)
            {
                var errorContent = await respuesta.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error al obtener usuario {id}: {errorContent}");
                return null;
            }

            var contenido = await respuesta.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UsuarioResponseDto>(contenido, _options);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error en LeerPorIdAsync: {ex.Message}");
            return null;
        }
    }

    public async Task<UsuarioResponseDto> CrearAsync(UsuarioCreateDto dto)
    {
        try
        {
            var json = JsonSerializer.Serialize(dto, _options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var respuesta = await _httpClient.PostAsync(_baseUrl, content);
            if (!respuesta.IsSuccessStatusCode)
            {
                var errorContent = await respuesta.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error al crear usuario: {errorContent}");
                return null;
            }

            var contenido = await respuesta.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UsuarioResponseDto>(contenido, _options);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Excepción al crear usuario: {ex.Message}");
            return null;
        }
    }

    public async Task<UsuarioResponseDto> ActualizarAsync(int id, UsuarioUpdateDto dto)
    {
        try
        {
            var json = JsonSerializer.Serialize(dto, _options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Console.WriteLine($"➡️ POST URL: {_baseUrl}/actualizar/{id}");
            Console.WriteLine($"➡️ Body: {json}");

            var response = await _httpClient.PostAsync($"{_baseUrl}/actualizar/{id}", content);

            Console.WriteLine($"⬅️ Código: {(int)response.StatusCode} {response.ReasonPhrase}");

            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"⬅️ Respuesta: {body}");

            if (!response.IsSuccessStatusCode)
                return null;

            return JsonSerializer.Deserialize<UsuarioResponseDto>(body, _options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ ERROR: {ex.Message}");
            return null;
        }
    }

    public async Task<int> ObtenerIdPorCorreoAsync(string correo)
    {
        var usuarios = await LeerTodosAsync(); // o un endpoint específico por correo
        var usuario = usuarios.FirstOrDefault(u => u.Email.Equals(correo, StringComparison.OrdinalIgnoreCase));

        if (usuario != null)
            return usuario.Id;

        return -1;
    }






    public async Task<bool> EliminarAsync(int id)
    {
        var respuesta = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
        return respuesta.IsSuccessStatusCode;
    }
}
