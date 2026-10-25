using System.Net.Http.Json;
using System.Text.Json;
using ClientesBlazor.Models;

namespace ClientesBlazor.Services;

public record ResultadoOperacion(bool Exito, string Mensaje);

/// <summary>
/// Toda la comunicación con la API REST pasa por aquí.
/// Blazor -> (este servicio / HttpClient) -> API REST -> Azure SQL
/// </summary>
public class ClienteService(HttpClient http)
{
    private const string Ruta = "api/clientes";

    public string UrlApi => http.BaseAddress?.ToString() ?? "(sin configurar)";

    public async Task<List<Cliente>> ObtenerTodosAsync()
        => await http.GetFromJsonAsync<List<Cliente>>(Ruta) ?? [];

    public Task<ResultadoOperacion> CrearAsync(Cliente cliente)
        => EjecutarAsync(() => http.PostAsJsonAsync(Ruta, cliente),
                         $"Cliente {cliente.NombreCompleto} registrado correctamente.");

    public Task<ResultadoOperacion> ActualizarAsync(Cliente cliente)
        => EjecutarAsync(() => http.PutAsJsonAsync($"{Ruta}/{cliente.IdCliente}", cliente),
                         $"Cliente {cliente.NombreCompleto} actualizado correctamente.");

    public Task<ResultadoOperacion> EliminarAsync(Cliente cliente)
        => EjecutarAsync(() => http.DeleteAsync($"{Ruta}/{cliente.IdCliente}"),
                         $"Cliente {cliente.NombreCompleto} eliminado correctamente.");

    private static async Task<ResultadoOperacion> EjecutarAsync(
        Func<Task<HttpResponseMessage>> peticion, string mensajeExito)
    {
        try
        {
            using var respuesta = await peticion();
            return respuesta.IsSuccessStatusCode
                ? new ResultadoOperacion(true, mensajeExito)
                : new ResultadoOperacion(false, await LeerErrorAsync(respuesta));
        }
        catch (Exception ex)
        {
            return new ResultadoOperacion(false, $"No se pudo conectar con la API: {ex.Message}");
        }
    }

    /// <summary>Extrae un mensaje legible de la respuesta de error de la API.</summary>
    private static async Task<string> LeerErrorAsync(HttpResponseMessage respuesta)
    {
        var cuerpo = await respuesta.Content.ReadAsStringAsync();
        try
        {
            using var doc = JsonDocument.Parse(cuerpo);
            var raiz = doc.RootElement;
            if (raiz.ValueKind == JsonValueKind.Object)
            {
                if (raiz.TryGetProperty("mensaje", out var m))
                    return m.GetString() ?? "Error desconocido.";

                if (raiz.TryGetProperty("errors", out var errores) && errores.ValueKind == JsonValueKind.Object)
                    return string.Join(" ", errores.EnumerateObject()
                        .SelectMany(p => p.Value.EnumerateArray().Select(v => v.GetString())));

                if (raiz.TryGetProperty("title", out var t))
                    return t.GetString() ?? "Error desconocido.";
            }
        }
        catch (JsonException) { /* la respuesta no era JSON */ }

        return $"Error {(int)respuesta.StatusCode} ({respuesta.ReasonPhrase}).";
    }
}
