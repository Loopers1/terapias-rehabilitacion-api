namespace Terapias_rehabilitaciones.Models
{
    public class Sesion
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public int IdPaciente { get; set; }
        public int IdTerapeuta { get; set; }
        public int IdTipoTerapia { get; set; }
        public DateTime FechaHora { get; set; }
        public int DuracionMinutos { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; } = "Activo";

        public Paciente? Paciente { get; set; }
        public Terapeuta? Terapeuta { get; set; }
        public TipoTerapia? TipoTerapia { get; set; }
        public ICollection<UsoInsumo>? UsoInsumos { get; set; }
    }
}
