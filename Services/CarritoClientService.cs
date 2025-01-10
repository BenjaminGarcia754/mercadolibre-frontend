using System.Text.Json;
using frontendnet.Models;

namespace frontendnet.Services;

public class CarritoClientService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string SessionKeyCarrito = "Carrito";

    public CarritoClientService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<List<Producto>> ObtenerCarrito()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        var carritoJson = session?.GetString(SessionKeyCarrito);
        return Task.FromResult(carritoJson == null ? new List<Producto>() : JsonSerializer.Deserialize<List<Producto>>(carritoJson)!);
    }

    public Task GuardarCarrito(List<Producto> carrito)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        var carritoJson = JsonSerializer.Serialize(carrito);
        session?.SetString(SessionKeyCarrito, carritoJson);
        return Task.CompletedTask;
    }

    public async Task AgregarAlCarrito(Producto producto)
    {
        var carrito = await ObtenerCarrito() ?? new List<Producto>();
        carrito.Add(producto);
        await GuardarCarrito(carrito);
    }

    public Task LimpiarCarrito()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        session?.Remove(SessionKeyCarrito);
        return Task.CompletedTask;
    }

    public async Task EliminarDelCarrito(int id)
    {
        var carrito = await ObtenerCarrito() ?? new List<Producto>();
        carrito.RemoveAll(p => p.ProductoId == id);
        await GuardarCarrito(carrito);
    }
}