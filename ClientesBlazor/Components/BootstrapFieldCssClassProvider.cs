using Microsoft.AspNetCore.Components.Forms;

namespace ClientesBlazor.Components;

/// <summary>
/// Hace que la validación de Blazor use las clases de Bootstrap 5:
///   campo inválido -> "is-invalid" (borde rojo)
///   campo válido y modificado -> "is-valid" (borde verde)
/// </summary>
public class BootstrapFieldCssClassProvider : FieldCssClassProvider
{
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        var esValido = !editContext.GetValidationMessages(fieldIdentifier).Any();
        var modificado = editContext.IsModified(fieldIdentifier);

        if (!esValido) return "is-invalid";
        return modificado ? "is-valid" : string.Empty;
    }
}
