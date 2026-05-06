using Terapias_rehabilitaciones.Models;

namespace Terapias_rehabilitaciones.Models
{
    public class Terapeuta
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Estado { get; set; } = "Activo";

        public ICollection<Sesion>? Sesiones { get; set; }
    }
}