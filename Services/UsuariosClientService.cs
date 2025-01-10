using frontendnet.Models;

namespace frontendnet.Services;

public class UsuariosClientService(HttpClient client)
{
    public async Task<List<UsuarioA>?> GetAsync()
    {
        return await client.GetFromJsonAsync<List<UsuarioA>>("api/usuarios");
    }

    public async Task<UsuarioA?> GetAsync(string email)
    {
        return await client.GetFromJsonAsync<UsuarioA>($"api/usuarios/{email}");
    }

    public async Task PostAsync(UsuarioPwd usuario)
    {
        var response = await client.PostAsJsonAsync("api/usuarios", usuario);
        response.EnsureSuccessStatusCode();
    }

    public async Task PutAsync(UsuarioA usuario)
    {
        var response = await client.PutAsJsonAsync($"api/usuarios/{usuario.Email}", usuario);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string email)
    {
        var response = await client.DeleteAsync($"api/usuarios/{email}");
        response.EnsureSuccessStatusCode();
    }
}
