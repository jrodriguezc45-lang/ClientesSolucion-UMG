using System.ComponentModel.DataAnnotations;

namespace ClientesBlazor.Models;

/// <summary>
/// Modelo de cliente utilizado en la aplicación Blazor para el formulario y la tabla.
/// Utiliza DateTime? para sincronizarse correctamente con los datos enviados por la API.
/// </summary>
public class Cliente
{
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "El CUI es obligatorio.")]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "El CUI debe tener exactamente 13 dígitos.")]
    public string CUI { get; set; } = string.Empty;

    [Required(ErrorMessage = "El NIT es obligatorio.")]
    [StringLength(15, ErrorMessage = "El NIT no puede exceder los 15 caracteres.")]
    public string NIT { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(100, ErrorMessage = "Los nombres no pueden exceder los 100 caracteres.")]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(100, ErrorMessage = "Los apellidos no pueden exceder los 100 caracteres.")]
    public string Apellidos { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres.")]
    public string? Direccion { get; set; }

    [StringLength(15, ErrorMessage = "El teléfono no puede exceder los 15 caracteres.")]
    public string? Telefono { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    /// <summary>
    /// Propiedad calculada que concatena Nombres y Apellidos.
    /// </summary>
    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();

    /// <summary>
    /// Crea una copia exacta del cliente para edición en ventanas modales.
    /// </summary>
    public Cliente Clonar()
    {
        return new Cliente
        {
            IdCliente = this.IdCliente,
            CUI = this.CUI,
            NIT = this.NIT,
            Nombres = this.Nombres,
            Apellidos = this.Apellidos,
            Direccion = this.Direccion,
            Telefono = this.Telefono,
            FechaNacimiento = this.FechaNacimiento
        };
    }
}
