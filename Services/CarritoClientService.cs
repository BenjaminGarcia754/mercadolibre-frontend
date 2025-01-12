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

    public Task<List<ProductoA>> ObtenerCarrito()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        var carritoJson = session?.GetString(SessionKeyCarrito);
        return Task.FromResult(carritoJson == null ? new List<ProductoA>() : JsonSerializer.Deserialize<List<ProductoA>>(carritoJson)!);
    }

    public Task GuardarCarrito(List<ProductoA> carrito)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        var carritoJson = JsonSerializer.Serialize(carrito);
        session?.SetString(SessionKeyCarrito, carritoJson);
        return Task.CompletedTask;
    }

    public async Task AgregarAlCarrito(ProductoA producto)
    {
        var carrito = await ObtenerCarrito() ?? new List<ProductoA>();
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
        var carrito = await ObtenerCarrito() ?? new List<ProductoA>();
        carrito.RemoveAll(p => p.ProductoId == id);
        await GuardarCarrito(carrito);
    }
}