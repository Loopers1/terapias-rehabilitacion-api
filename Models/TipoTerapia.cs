namespace Terapias_rehabilitaciones.Models
{
    public class TipoTerapia
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Costo { get; set; }
        public string Estado { get; set; } = "Activo";

        public ICollection<Sesion>? Sesiones { get; set; }
    }
}
