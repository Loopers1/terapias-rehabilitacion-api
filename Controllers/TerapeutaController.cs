using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Terapias_rehabilitaciones.Data;
using Terapias_rehabilitaciones.Models;


namespace Terapias_rehabilitaciones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TerapeutaController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TerapeutaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lista = await (
                from t in _context.Terapeutas
                where t.Estado == "Activo"
                select new { t.Codigo, t.Nombre, t.Apellido }
            ).ToListAsync();
            return Ok(lista);
        }

        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetByCodigo(string codigo)
        {
            var item = await (
                from t in _context.Terapeutas
                where t.Codigo == codigo && t.Estado == "Activo"
                select new { t.Codigo, t.Nombre, t.Apellido }
            ).FirstOrDefaultAsync();
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Terapeuta nuevo)
        {
            nuevo.Estado = "Activo";
            _context.Terapeutas.Add(nuevo);
            await _context.SaveChangesAsync();
            return Ok(new { nuevo.Codigo, nuevo.Nombre, nuevo.Apellido });
        }

        [HttpPut("{codigo}")]
        public async Task<IActionResult> Update(string codigo, [FromBody] Terapeuta actualizado)
        {
            var item = await _context.Terapeutas
                .FirstOrDefaultAsync(t => t.Codigo == codigo && t.Estado == "Activo");
            if (item == null) return NotFound();
            item.Nombre = actualizado.Nombre;
            item.Apellido = actualizado.Apellido;
            await _context.SaveChangesAsync();
            return Ok(new { item.Codigo, item.Nombre, item.Apellido });
        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Delete(string codigo)
        {
            var item = await _context.Terapeutas
                .FirstOrDefaultAsync(t => t.Codigo == codigo && t.Estado == "Activo");
            if (item == null) return NotFound();
            item.Estado = "Inactivo";
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Terapeuta marcado como inactivo" });
        }
    }
}