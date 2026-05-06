using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Terapias_rehabilitaciones.Data;
using Terapias_rehabilitaciones.Models;


namespace Terapias_rehabilitaciones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoTerapiaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoTerapiaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: obtener todos los tipos activos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lista = await _context.TiposTerapia
                .Where(t => t.Estado == "Activo")
                .Select(t => new { t.Codigo, t.Nombre, t.Descripcion })
                .ToListAsync();
            return Ok(lista);
        }

        // GET por codigo
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetByCodigo(string codigo)
        {
            var item = await _context.TiposTerapia
                .Where(t => t.Codigo == codigo && t.Estado == "Activo")
                .Select(t => new { t.Codigo, t.Nombre, t.Descripcion })
                .FirstOrDefaultAsync();
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: crear nuevo tipo de terapia
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TipoTerapia nuevo)
        {
            nuevo.Estado = "Activo";
            _context.TiposTerapia.Add(nuevo);
            await _context.SaveChangesAsync();
            return Ok(new { nuevo.Codigo, nuevo.Nombre });
        }

        // PUT: actualizar por codigo
        [HttpPut("{codigo}")]
        public async Task<IActionResult> Update(string codigo, [FromBody] TipoTerapia actualizado)
        {
            var item = await _context.TiposTerapia
                .FirstOrDefaultAsync(t => t.Codigo == codigo && t.Estado == "Activo");
            if (item == null) return NotFound();
            item.Nombre = actualizado.Nombre;
            item.Descripcion = actualizado.Descripcion;
            await _context.SaveChangesAsync();
            return Ok(new { item.Codigo, item.Nombre });
        }

        // DELETE: soft delete por codigo
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Delete(string codigo)
        {
            var item = await _context.TiposTerapia
                .FirstOrDefaultAsync(t => t.Codigo == codigo && t.Estado == "Activo");
            if (item == null) return NotFound();
            item.Estado = "Inactivo";
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Registro desactivado correctamente" });
        }
    }
}
