namespace Terapias_rehabilitaciones.Models
{
    public class Insumo
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string UnidadMedida { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public string Estado { get; set; } = "Activo";

        public ICollection<UsoInsumo>? UsoInsumos { get; set; }
    }
}
