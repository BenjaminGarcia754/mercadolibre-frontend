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
            prod.titulo = item.Titulo;
            prod.stock = 0;
            prod.precio = item.Precio;
            prod.archivoid = (int)item.ProductoId;
            prod.descripcion = item.Descripcion;
            carritoDTO.Producto = prod;
            carritoDTO.Cantidad = 1;
            carritoDTO.correousuario = email;

            listaCarritoDTO.Add(carritoDTO);
        }
        if (productos == null)
        {
            throw new InvalidOperationException("Carrito Vacio.");
        }

        var response = await client.PostAsJsonAsync($"api/compra", listaCarritoDTO);

        if (response.IsSuccessStatusCode)
        {
            await carrito.LimpiarCarrito();
        }
        else
        {
            throw new InvalidOperationException("Error al realizar la compra.");
        }
        
        response.EnsureSuccessStatusCode();
    }



    public async Task<List<Pedido>?> GetAsync()
    {
        return await client.GetFromJsonAsync<List<Pedido>>("api/pedidos");
    }
}