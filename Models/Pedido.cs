using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace frontendnet.Models;

public class Pedido
{
    [Display(Name = "Id")]
    [JsonPropertyName("pedidoid")]
    public int? PedidoId { get; set; }

    [Display(Name = "Email")]
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [Display(Name = "Producto")]
    [JsonPropertyName("productoid")]
    public required int? ProductoId { get; set; }

    [Display(Name = "Producto")]
    public Producto? Producto { get; set; }

    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    [Display(Name = "Total a pagar")]
    public decimal Total { get; set; }

    [Display(Name = "Usuario")]
    public UsuarioPedido? Usuario { get; set; }

    [Display(Name = "Fecha")]
    public DateTime? Fecha { get; set; }
}
