using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientesApi.Models;

/// <summary>
/// Representa un registro de la tabla dbo.clientes en Azure SQL.
/// Los atributos [Column] hacen que el nombre de la propiedad en C#
/// coincida con el nombre real de la columna en la base de datos.
/// </summary>
[Table("clientes")]
public class Cliente
{
    [Key]
    [Column("Id_cliente")]
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "El CUI es obligatorio.")]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "El CUI debe tener exactamente 13 dígitos.")]
    [Column("CUI", TypeName = "varchar(13)")]
    public string CUI { get; set; } = string.Empty;

    [Required(ErrorMessage = "El NIT es obligatorio.")]
    [StringLength(15)]
    [Column("NIT", TypeName = "varchar(15)")]
    public string NIT { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(100)]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(100)]
    public string Apellidos { get; set; } = string.Empty;

    [StringLength(200)]
    [Column("Direccion")]
    public string? Direccion { get; set; }

    [StringLength(15)]
    [Column("Telefono", TypeName = "varchar(15)")]
    public string? Telefono { get; set; }

    [Column("Fecha_Nacimiento", TypeName = "date")]
    public DateTime? FechaNacimiento { get; set; }
}
