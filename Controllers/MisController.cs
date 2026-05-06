using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Terapias_rehabilitaciones.Data;
using Terapias_rehabilitaciones.Models;


namespace TerapiasRehabilitacion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MisController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MisController(AppDbContext context)
        {
            _context = context;
        }

        // CONSULTA 1 - JOIN 2 tablas: sesiones con paciente
        [HttpGet("sesiones-con-paciente")]
        public async Task<IActionResult> SesionesConPaciente()
        {
            var resultado = await (
                from s in _context.Sesiones
                join p in _context.Pacientes on s.IdPaciente equals p.Id
                where s.Estado == "Activo" && p.Estado == "Activo"
                select new
                {
                    p.Nombre,
                    p.Apellido,
                    s.FechaHora,
                    s.DuracionMinutos,
                    s.Observaciones
                }
            ).ToListAsync();
            return Ok(resultado);
        }

        // CONSULTA 2 - GROUP BY + COUNT: sesiones por paciente
        [HttpGet("sesiones-por-paciente")]
        public async Task<IActionResult> SesionesPorPaciente()
        {
            var resultado = await (
                from s in _context.Sesiones
                join p in _context.Pacientes on s.IdPaciente equals p.Id
                where s.Estado == "Activo" && p.Estado == "Activo"
                group s by new { p.Codigo, p.Nombre, p.Apellido } into g
                select new
                {
                    g.Key.Codigo,
                    g.Key.Nombre,
                    g.Key.Apellido,
                    TotalSesiones = g.Count()
                }
            ).ToListAsync();
            return Ok(resultado);
        }

        // CONSULTA 3 - GROUP BY + SUM: costo total por paciente
        [HttpGet("costo-por-paciente")]
        public async Task<IActionResult> CostoPorPaciente()
        {
            var resultado = await (
                from s in _context.Sesiones
                join p in _context.Pacientes on s.IdPaciente equals p.Id
                join tt in _context.TiposTerapia on s.IdTipoTerapia equals tt.Id
                where s.Estado == "Activo" && p.Estado == "Activo"
                group tt by new { p.Codigo, p.Nombre, p.Apellido } into g
                select new
                {
                    g.Key.Codigo,
                    g.Key.Nombre,
                    g.Key.Apellido,
                    CostoTotal = g.Sum(x => x.Costo)
                }
            ).ToListAsync();
            return Ok(resultado);
        }

        //crear planes de tratamiento
        [HttpPost("plan-tratamiento/crear")]
        public async Task<IActionResult> CrearPlan([FromBody] PlanDto dto)
        {
            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Codigo == dto.CodigoPaciente && p.Estado == "Activo");
            if (paciente == null) return BadRequest("Paciente no encontrado");

            var plan = new PlanTratamiento
            {
                Codigo = dto.Codigo,
                IdPaciente = paciente.Id,
                Objetivo = dto.Objetivo,
                FechaInicio = dto.FechaInicio,
                FechaFinEstimada = dto.FechaFinEstimada,
                Estado = "Activo"
            };

            _context.Planes.Add(plan);
            await _context.SaveChangesAsync();
            return Ok(new { plan.Codigo, plan.Objetivo });
        }

        [HttpPut("plan-tratamiento/editar/{codigo}")]
        public async Task<IActionResult> EditarPlan(string codigo, [FromBody] PlanDto dto)
        {
            var plan = await _context.Planes
                .FirstOrDefaultAsync(p => p.Codigo == codigo && p.Estado == "Activo");
            if (plan == null) return NotFound();

            plan.Objetivo = dto.Objetivo;
            plan.FechaInicio = dto.FechaInicio;
            plan.FechaFinEstimada = dto.FechaFinEstimada;
            await _context.SaveChangesAsync();
            return Ok(new { plan.Codigo, plan.Objetivo });
        }

        [HttpDelete("plan-tratamiento/eliminar/{codigo}")]
        public async Task<IActionResult> EliminarPlan(string codigo)
        {
            var plan = await _context.Planes
                .FirstOrDefaultAsync(p => p.Codigo == codigo && p.Estado == "Activo");
            if (plan == null) return NotFound();
            plan.Estado = "Inactivo";
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Plan desactivado correctamente" });
        }

        //MOSTRAR PLANES DE TRATAMIENTO
        [HttpGet("plan-tratamiento/todos")]
        public async Task<IActionResult> GetTodosLosPlanes()
        {
            var resultado = await (
                from pl in _context.Planes
                join p in _context.Pacientes on pl.IdPaciente equals p.Id
                where pl.Estado == "Activo"
                select new
                {
                    pl.Codigo,
                    Nombre = p.Nombre + " " + p.Apellido,
                    pl.Objetivo,
                    pl.FechaInicio,
                    pl.FechaFinEstimada
                }
            ).ToListAsync();
            return Ok(resultado);
        }
        public class PlanDto
        {
            public string Codigo { get; set; }
            public string CodigoPaciente { get; set; }
            public string Objetivo { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFinEstimada { get; set; }
        }

        // CONSULTA 4 - Búsqueda por código de paciente
        [HttpGet("paciente/{codigo}")]
        public async Task<IActionResult> BuscarPorCodigo(string codigo)
        {
            var resultado = await (
                from p in _context.Pacientes
                where p.Codigo == codigo && p.Estado == "Activo"
                select new
                {
                    p.Codigo,
                    p.Nombre,
                    p.Apellido,
                    p.CI,
                    p.Telefono
                }
            ).FirstOrDefaultAsync();
            if (resultado == null) return NotFound();
            return Ok(resultado);
        }

        // CONSULTA 5 - NOT EXISTS: pacientes sin sesiones
        [HttpGet("pacientes-sin-sesiones")]
        public async Task<IActionResult> PacientesSinSesiones()
        {
            var resultado = await (
                from p in _context.Pacientes
                where p.Estado == "Activo" &&
                      !_context.Sesiones.Any(s => s.IdPaciente == p.Id && s.Estado == "Activo")
                select new
                {
                    p.Codigo,
                    p.Nombre,
                    p.Apellido,
                    p.Telefono
                }
            ).ToListAsync();
            return Ok(resultado);
        }

        // CONSULTA 6 - UC1: sesiones activas de un paciente
        [HttpGet("sesiones-activas/{codigoPaciente}")]
        public async Task<IActionResult> SesionesActivasPorPaciente(string codigoPaciente)
        {
            var resultado = await (
                from s in _context.Sesiones
                join p in _context.Pacientes on s.IdPaciente equals p.Id
                join tt in _context.TiposTerapia on s.IdTipoTerapia equals tt.Id
                where s.Estado == "Activo" && p.Codigo == codigoPaciente
                select new
                {
                    s.FechaHora,
                    s.DuracionMinutos,
                    s.Observaciones,
                    TipoTerapia = tt.Nombre
                }
            ).ToListAsync();
            return Ok(resultado);
        }

        // CONSULTA 7 - UC2: plan de tratamiento de un paciente
        [HttpGet("plan-tratamiento/{codigoPaciente}")]
        public async Task<IActionResult> PlanTratamientoPorPaciente(string codigoPaciente)
        {
            var resultado = await (
                from pl in _context.Planes
                join p in _context.Pacientes on pl.IdPaciente equals p.Id
                where p.Codigo == codigoPaciente && pl.Estado == "Activo"
                select new
                {
                    p.Nombre,
                    p.Apellido,
                    pl.Objetivo,
                    pl.FechaInicio,
                    pl.FechaFinEstimada
                }
            ).ToListAsync();
            return Ok(resultado);
        }

        // CONSULTA 8 - UC3: historial de evaluaciones de progreso
        [HttpGet("evaluaciones/{codigoPaciente}")]
        public async Task<IActionResult> EvaluacionesPorPaciente(string codigoPaciente)
        {
            var resultado = await (
                from e in _context.Evaluaciones
                join pl in _context.Planes on e.IdPlan equals pl.Id
                join p in _context.Pacientes on pl.IdPaciente equals p.Id
                where p.Codigo == codigoPaciente && e.Estado == "Activo"
                orderby e.Fecha
                select new
                {
                    e.Fecha,
                    e.PuntajeProgreso,
                    e.Comentario
                }
            ).ToListAsync();
            return Ok(resultado);
        }

        // CONSULTA 9 - UC4: insumos utilizados en una sesión
        [HttpGet("insumos-sesion/{codigoSesion}")]
        public async Task<IActionResult> InsumosPorSesion(string codigoSesion)
        {
            var resultado = await (
                from ui in _context.UsoInsumos
                join s in _context.Sesiones on ui.IdSesion equals s.Id
                join i in _context.Insumos on ui.IdInsumo equals i.Id
                where s.Codigo == codigoSesion && s.Estado == "Activo"
                select new
                {
                    Insumo = i.Nombre,
                    ui.CantidadUsada,
                    i.UnidadMedida
                }
            ).ToListAsync();
            return Ok(resultado);
        }

        // CONSULTA 10 - UC5: pacientes atendidos por mes
        [HttpGet("pacientes-por-mes")]
        public async Task<IActionResult> PacientesPorMes()
        {
            var resultado = await (
                from s in _context.Sesiones
                where s.Estado == "Activo"
                group s by new
                {
                    Anio = s.FechaHora.Year,
                    Mes = s.FechaHora.Month
                } into g
                orderby g.Key.Anio, g.Key.Mes
                select new
                {
                    g.Key.Anio,
                    g.Key.Mes,
                    TotalAtenciones = g.Count()
                }
            ).ToListAsync();
            return Ok(resultado);
        }

        // CONSULTA 11 - UC6: citas programadas por terapeuta
        [HttpGet("sesiones-por-terapeuta")]
        public async Task<IActionResult> SesionesPorTerapeuta()
        {
            var resultado = await (
                from s in _context.Sesiones
                join t in _context.Terapeutas on s.IdTerapeuta equals t.Id
                where s.Estado == "Activo" && t.Estado == "Activo"
                group s by new { t.Codigo, t.Nombre, t.Apellido } into g
                select new
                {
                    g.Key.Codigo,
                    g.Key.Nombre,
                    g.Key.Apellido,
                    TotalSesiones = g.Count()
                }
            ).ToListAsync();
            return Ok(resultado);
        }

        // CONSULTA 12 - UC7: pacientes sin sesiones registradas (ya en consulta 5)
        // Se reutiliza el endpoint pacientes-sin-sesiones

        // CONSULTA 13 - UC8: insumos bajo stock mínimo
        [HttpGet("insumos-bajo-stock")]
        public async Task<IActionResult> InsumosBajoStock()
        {
            var resultado = await (
                from i in _context.Insumos
                where i.Estado == "Activo" && i.StockActual <= i.StockMinimo
                select new
                {
                    i.Codigo,
                    i.Nombre,
                    i.StockActual,
                    i.StockMinimo,
                    Diferencia = i.StockMinimo - i.StockActual
                }
            ).ToListAsync();
            return Ok(resultado);
        }

        // CONSULTA 14 - UC9: cantidad de sesiones por tipo de terapia
        [HttpGet("sesiones-por-tipo-terapia")]
        public async Task<IActionResult> SesionesPorTipoTerapia()
        {
            var resultado = await (
                from s in _context.Sesiones
                join tt in _context.TiposTerapia on s.IdTipoTerapia equals tt.Id
                where s.Estado == "Activo" && tt.Estado == "Activo"
                group s by new { tt.Codigo, tt.Nombre } into g
                select new
                {
                    g.Key.Codigo,
                    g.Key.Nombre,
                    TotalSesiones = g.Count()
                }
            ).ToListAsync();
            return Ok(resultado);
        }

        // CONSULTA 15 - UC10: costo total de tratamientos por paciente (JOIN 3 tablas)
        [HttpGet("costo-total-por-paciente")]
        public async Task<IActionResult> CostoTotalPorPaciente()
        {
            var resultado = await (
                from s in _context.Sesiones
                join p in _context.Pacientes on s.IdPaciente equals p.Id
                join tt in _context.TiposTerapia on s.IdTipoTerapia equals tt.Id
                where s.Estado == "Activo" && p.Estado == "Activo" && tt.Estado == "Activo"
                group new { tt } by new { p.Codigo, p.Nombre, p.Apellido } into g
                select new
                {
                    g.Key.Codigo,
                    g.Key.Nombre,
                    g.Key.Apellido,
                    CostoTotal = g.Sum(x => x.tt.Costo),
                    TotalSesiones = g.Count()
                }
            ).ToListAsync();
            return Ok(resultado);
        }
    }
}