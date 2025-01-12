namespace frontendnet.Models
{
    public class Producto
    {
        public int? id { get; set; }
        public string? titulo { get; set; }
        public string? descripcion { get; set; }
        public decimal precio { get; set;}
        public int archivoid { get; set; }
        public int stock { get; set; }
    }
}
