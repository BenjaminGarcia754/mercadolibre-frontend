using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class UsuarioPedido
{
    [Display(Name = "Id")]
    public string? Id { get; set; }

    public string? Email
    { get; set; }

    public string? Nombre { get; set; }

}
