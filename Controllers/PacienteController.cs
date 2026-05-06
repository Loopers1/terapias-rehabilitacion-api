using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Terapias_rehabilitaciones.Data;
using Terapias_rehabilitaciones.Models;

namespace Terapias_rehabilitaciones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PacienteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lista = await _context.Pacientes
                .Where(p => p.Estado == "Activo")
                .Select(p => new { p.Codigo, p.Nombre, p.Apellido, p.CI, p.Telefono })
                .ToListAsync();
            return Ok(lista);
        }

        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetByCodigo(string codigo)
        {
            var item = await _context.Pacientes
                .Where(p => p.Codigo == codigo && p.Estado == "Activo")
                .Select(p => new { p.Codigo, p.Nombre, p.Apellido, p.CI, p.Telefono })
                .FirstOrDefaultAsync();
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Paciente nuevo)
        {
            nuevo.Estado = "Activo";
            _context.Pacientes.Add(nuevo);
            await _context.SaveChangesAsync();
            return Ok(new { nuevo.Codigo, nuevo.Nombre });
        }

        [HttpPut("{codigo}")]
        public async Task<IActionResult> Update(string codigo, [FromBody] Paciente actualizado)
        {
            var item = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Codigo == codigo && p.Estado == "Activo");
            if (item == null) return NotFound();
            item.Nombre = actualizado.Nombre;
            item.Apellido = actualizado.Apellido;
            item.Telefono = actualizado.Telefono;
            await _context.SaveChangesAsync();
            return Ok(new { item.Codigo, item.Nombre });
        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Delete(string codigo)
        {
            var item = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Codigo == codigo && p.Estado == "Activo");
            if (item == null) return NotFound();
            item.Estado = "Inactivo";
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Paciente desactivado correctamente" });
        }
    }
}
