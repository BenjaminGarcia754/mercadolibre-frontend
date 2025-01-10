namespace frontendnet.Models
{
    public class Usuario
    {
        public string? id { get; set; }
        public string? email { get; set; }
        public string? passwordhash { get; set; }
        public string? nombre { get; set; }
        public bool? protegido { get; set; }
        public string? rolid { get; set; }
    }
}
