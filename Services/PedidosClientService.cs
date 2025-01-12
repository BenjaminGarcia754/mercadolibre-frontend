using frontendnet.Models;

namespace frontendnet.Services;

public class PedidosClientService(CarritoClientService carrito, HttpClient client)
{
    public async Task PostAsync(string email)
    {
        var productos = await carrito.ObtenerCarrito();
        List<CarritoDTO> listaCarritoDTO = new List<CarritoDTO>();
        CarritoDTO carritoDTO = new CarritoDTO();
        
        foreach (var item in productos)
        {
            var prod = new Producto();
            prod.id = (int)item.ProductoId;
            prod.titulo = item.ProductoId.ToString();
            prod.precio = item.Precio;
            prod.stock = 0;
            prod.archivoid = (int)item.ProductoId;
            prod.descripcion = item.Descripcion;
            carritoDTO.Producto = prod;
            carritoDTO.Cantidad = 1;
            carritoDTO.correousuario = email;

            listaCarritoDTO.Add(carritoDTO);
        }
        //var prod = new Producto();
        if (productos == null)
        {
            throw new InvalidOperationException("No products found in the cart.");
        }
        /* 
        var pedido = productos.Select(producto => new Pedido
        {
            Email = email,
            ProductoId = producto.ProductoId,
        }).ToList();
        */

        //PedidosCarrito pedidos = new PedidosCarrito(pedido);

        var response = await client.PostAsJsonAsync($"api/compra", listaCarritoDTO);

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