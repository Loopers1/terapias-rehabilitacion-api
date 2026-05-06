using Microsoft.EntityFrameworkCore;
using Terapias_rehabilitaciones.Models;

namespace Terapias_rehabilitaciones.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Terapeuta> Terapeutas { get; set; }
        public DbSet<TipoTerapia> TiposTerapia { get; set; }
        public DbSet<Sesion> Sesiones { get; set; }
        public DbSet<PlanTratamiento> Planes { get; set; }
        public DbSet<Evaluacion> Evaluaciones { get; set; }
        public DbSet<Insumo> Insumos { get; set; }
        public DbSet<UsoInsumo> UsoInsumos { get; set; }
    }
}
