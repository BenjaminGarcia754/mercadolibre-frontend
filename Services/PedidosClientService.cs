using frontendnet.Models;

namespace frontendnet.Services;

public class PedidosClientService(CarritoClientService carrito, HttpClient client)
{
    public async Task PostAsync(string email)
    {
        //var carritoService = HttpContext.RequestServices.GetService<CarritoClientService>();
        var productos = await carrito.ObtenerCarrito();

        if (productos == null)
        {
            throw new InvalidOperationException("No products found in the cart.");
        }

        var pedido = productos.Select(producto => new Pedido
        {
            Email = email,
            ProductoId = producto.ProductoId,
        }).ToList();

        PedidosCarrito pedidos = new PedidosCarrito(pedido);

        var response = await client.PostAsJsonAsync($"api/pedidos", pedidos);

        if (response.IsSuccessStatusCode)
        {
            await carrito.LimpiarCarrito();
        }
        
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<Pedido>?> GetAsync()
    {
        return await client.GetFromJsonAsync<List<Pedido>>("api/pedidos");
    }
}