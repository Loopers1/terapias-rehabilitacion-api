using System.ComponentModel.DataAnnotations.Schema;

namespace Terapias_rehabilitaciones.Models
{
    public class PlanTratamiento
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public int IdPaciente { get; set; }
        public string Objetivo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinEstimada { get; set; }
        public string Estado { get; set; } = "Activo";

        [ForeignKey("IdPaciente")]
        public Paciente Paciente { get; set; }
        public ICollection<Evaluacion>? Evaluaciones { get; set; }
    }
}
