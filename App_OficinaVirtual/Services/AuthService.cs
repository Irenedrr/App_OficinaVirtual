using System.Net.Http.Headers;
using System.Text.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;


namespace App_OficinaVirtual.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    private const string BaseUrl = "http://localhost:8000";
    private string _accessToken = string.Empty;


    private int ObtenerUsuarioIdDesdeToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var claim = jwt.Claims.FirstOrDefault(c =>
            c.Type == "sub" || c.Type == "id" || c.Type == "user_id");

        return claim != null ? int.Parse(claim.Value) : -1;
    }

    public AuthService(HttpClient httpClient, JsonSerializerOptions serializerOptions)
    {
        _httpClient = httpClient;
        _serializerOptions = serializerOptions;
        InicializarTokenDesdePreferencias();
    }

    public string AccessToken => _accessToken;

    public async Task<bool> LoginAsync(string email, string password)
    {
        var parametros = new Dictionary<string, string>
        {
            { "username", email },
            { "password", password }
        };

        var content = new FormUrlEncodedContent(parametros);



        try
        {
            Debug.WriteLine($"URL de la petición: {BaseUrl}/usuarios/login");
            HttpResponseMessage response = await _httpClient.PostAsync($"{BaseUrl}/usuarios/login", content);

            Debug.WriteLine($"Código de estado: {response.StatusCode}");
            string responseContent = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Contenido de la respuesta: {responseContent}");

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent, _serializerOptions);

                if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    _accessToken = tokenResponse.AccessToken;
                    Preferences.Set("access_token", _accessToken);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    int userId = ObtenerUsuarioIdDesdeToken(_accessToken);
                    Preferences.Set("usuario_id", userId);

                    return true;
                }
            }
            else
            {
                Debug.WriteLine($"Error en la respuesta: {response.StatusCode}");
                Debug.WriteLine($"Contenido del error: {responseContent}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Excepción durante el login: {ex.Message}");
            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
        }

        return false;
    }

    public void Logout()
    {
        _accessToken = null;
        Preferences.Remove("access_token");
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public void InicializarTokenDesdePreferencias()
    {
        _accessToken = Preferences.Get("access_token", null);
        if (!string.IsNullOrEmpty(_accessToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }
    }
}






