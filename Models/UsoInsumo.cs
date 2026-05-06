using System.ComponentModel.DataAnnotations.Schema;

namespace Terapias_rehabilitaciones.Models
{
    public class UsoInsumo
    {
        public int Id { get; set; }

        [ForeignKey("Sesion")]
        public int IdSesion { get; set; }

        [ForeignKey("Insumo")]
        public int IdInsumo { get; set; }
        public int CantidadUsada { get; set; }
        public string Estado { get; set; } = "Activo";


        
        public Sesion? Sesion { get; set; }
        
        public Insumo? Insumo { get; set; }
    }
}
