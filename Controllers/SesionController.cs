using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Terapias_rehabilitaciones.Data;
using Terapias_rehabilitaciones.Models;

namespace Terapias_rehabilitaciones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SesionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SesionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("por-paciente/{codigoPaciente}")]
        public async Task<IActionResult> GetByPaciente(string codigoPaciente)
        {
            var resultado = await (
                from s in _context.Sesiones
                join p in _context.Pacientes on s.IdPaciente equals p.Id
                where s.Estado == "Activo" && p.Codigo == codigoPaciente
                select new
                {
                    s.Codigo,
                    s.FechaHora,
                    s.DuracionMinutos,
                    s.Observaciones,
                    Paciente = p.Nombre + " " + p.Apellido
                }
            ).ToListAsync();
            return Ok(resultado);
        }

        [HttpGet("detalle-completo")]
        public async Task<IActionResult> GetDetalleCompleto()
        {
            var resultado = await (
                from s in _context.Sesiones
                join p in _context.Pacientes on s.IdPaciente equals p.Id
                join t in _context.Terapeutas on s.IdTerapeuta equals t.Id
                join tt in _context.TiposTerapia on s.IdTipoTerapia equals tt.Id
                where s.Estado == "Activo"
                select new
                {
                    s.Codigo,
                    Paciente = p.Nombre + " " + p.Apellido,
                    Terapeuta = t.Nombre + " " + t.Apellido,
                    TipoTerapia = tt.Nombre,
                    s.FechaHora,
                    s.DuracionMinutos,
                    s.Observaciones
                }
            ).ToListAsync();
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SesionDto dto)
        {
            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Codigo == dto.CodigoPaciente && p.Estado == "Activo");
            if (paciente == null) return BadRequest("Paciente no encontrado o inactivo");

            var terapeuta = await _context.Terapeutas
                .FirstOrDefaultAsync(t => t.Codigo == dto.CodigoTerapeuta && t.Estado == "Activo");
            if (terapeuta == null) return BadRequest("Terapeuta no encontrado o inactivo");

            var tipoTerapia = await _context.TiposTerapia
                .FirstOrDefaultAsync(t => t.Codigo == dto.CodigoTipoTerapia && t.Estado == "Activo");
            if (tipoTerapia == null) return BadRequest("Tipo de terapia no encontrado o inactivo");

            var sesion = new Sesion
            {
                Codigo = dto.Codigo,
                IdPaciente = paciente.Id,
                IdTerapeuta = terapeuta.Id,
                IdTipoTerapia = tipoTerapia.Id,
                FechaHora = dto.FechaHora,
                DuracionMinutos = dto.DuracionMinutos,
                Observaciones = dto.Observaciones,
                Estado = "Activo"
            };

            _context.Sesiones.Add(sesion);
            await _context.SaveChangesAsync();
            return Ok(new { sesion.Codigo, sesion.FechaHora });
        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Delete(string codigo)
        {
            var item = await _context.Sesiones
                .FirstOrDefaultAsync(s => s.Codigo == codigo && s.Estado == "Activo");
            if (item == null) return NotFound();
            item.Estado = "Inactivo";
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Sesión desactivada correctamente" });
        }
    }

    public class SesionDto
    {
        public string Codigo { get; set; }
        public string CodigoPaciente { get; set; }
        public string CodigoTerapeuta { get; set; }
        public string CodigoTipoTerapia { get; set; }
        public DateTime FechaHora { get; set; }
        public int DuracionMinutos { get; set; }
        public string Observaciones { get; set; }
    }
}