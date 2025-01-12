using frontendnet.Models;

namespace frontendnet.Services;

public class ProductosClientService(HttpClient client)
{
    public async Task <List<ProductoA>?> GetAsync(string? search)
    {
        return await client.GetFromJsonAsync<List<ProductoA>>($"api/productos?s={search}");
    }

    public async Task<ProductoA?> GetAsync(int id)
    {
        return await client.GetFromJsonAsync<ProductoA>($"api/productos/{id}");
    }

    public async Task PostAsync(ProductoA producto)
    {
        var response = await client.PostAsJsonAsync($"api/productos", producto);
        response.EnsureSuccessStatusCode();
    }

    public async Task PutAsync(ProductoA producto)
    {
        var response = await client.PutAsJsonAsync($"api/productos/{producto.ProductoId}", producto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await client.DeleteAsync($"api/productos/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task PostAsync(int id, int categoriaid)
    {
        var response = await client.PostAsJsonAsync($"api/productos/{id}/categoria", new { categoriaid });
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id, int categoriaid)
    {
        var response = await client.DeleteAsync($"api/productos/{id}/categoria/{categoriaid}");
        response.EnsureSuccessStatusCode();
    }
}
