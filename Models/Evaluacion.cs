using System.ComponentModel.DataAnnotations.Schema;

namespace Terapias_rehabilitaciones.Models
{
    public class Evaluacion
    {
        public int Id { get; set; }
        public string Codigo { get; set; }

        [ForeignKey("Plan")]
        public int IdPlan { get; set; }
        public DateTime Fecha { get; set; }
        public int PuntajeProgreso { get; set; }
        public string Comentario { get; set; }
        public string Estado { get; set; } = "Activo";

        public PlanTratamiento? Plan { get; set; }
    }
}
