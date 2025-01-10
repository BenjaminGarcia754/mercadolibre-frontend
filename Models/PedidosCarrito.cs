namespace frontendnet.Models;

public class PedidosCarrito
{
    public List<Pedido> Pedidos { get; set; }

    public PedidosCarrito(List<Pedido> pedidos)
    {
        Pedidos = pedidos;
    }
}