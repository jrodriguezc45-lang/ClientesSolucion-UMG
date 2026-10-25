using ClientesApi.Data;
using ClientesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesApi.Controllers;

/// <summary>
/// CRUD de clientes.  Ruta base:  /api/clientes
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientesController(AppDbContext db) : ControllerBase
{
    // GET: api/clientes  -> listado completo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
    {
        var clientes = await db.Clientes
            .AsNoTracking()
            .OrderBy(c => c.IdCliente)
            .ToListAsync();

        return Ok(clientes);
    }

    // GET: api/clientes/5  -> un cliente por ID
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Cliente>> GetCliente(int id)
    {
        var cliente = await db.Clientes.FindAsync(id);

        if (cliente is null)
            return NotFound(new { mensaje = $"No existe un cliente con ID {id}." });

        return Ok(cliente);
    }

    // POST: api/clientes  -> registrar nuevo cliente
    [HttpPost]
    public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
    {
        if (await db.Clientes.AnyAsync(c => c.CUI == cliente.CUI))
            return Conflict(new { mensaje = $"Ya existe un cliente con el CUI {cliente.CUI}." });

        cliente.IdCliente = 0; // lo genera la base de datos (IDENTITY)
        db.Clientes.Add(cliente);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCliente), new { id = cliente.IdCliente }, cliente);
    }

    // PUT: api/clientes/5  -> actualizar cliente existente
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutCliente(int id, Cliente cliente)
    {
        if (id != cliente.IdCliente)
            return BadRequest(new { mensaje = "El ID de la URL no coincide con el del cliente." });

        var existente = await db.Clientes.FindAsync(id);
        if (existente is null)
            return NotFound(new { mensaje = $"No existe un cliente con ID {id}." });

        if (await db.Clientes.AnyAsync(c => c.CUI == cliente.CUI && c.IdCliente != id))
            return Conflict(new { mensaje = $"Otro cliente ya tiene el CUI {cliente.CUI}." });

        existente.CUI = cliente.CUI;
        existente.NIT = cliente.NIT;
        existente.Nombres = cliente.Nombres;
        existente.Apellidos = cliente.Apellidos;
        existente.Direccion = cliente.Direccion;
        existente.Telefono = cliente.Telefono;
        existente.FechaNacimiento = cliente.FechaNacimiento;

        await db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/clientes/5  -> eliminar cliente
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCliente(int id)
    {
        var cliente = await db.Clientes.FindAsync(id);
        if (cliente is null)
            return NotFound(new { mensaje = $"No existe un cliente con ID {id}." });

        db.Clientes.Remove(cliente);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
