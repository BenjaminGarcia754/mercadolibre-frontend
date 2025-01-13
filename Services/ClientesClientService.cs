using frontendnet.Models;

namespace frontendnet.Services;

public class ClientesClientService(HttpClient client)
{
    public async Task PostAsync(Client cliente)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        Usuario usuario = new Usuario
        {
            email = cliente.Email,
            nombre = cliente.Nombre,
            rolid = builder.Configuration["UsuarioRolID"],
            passwordhash = cliente.Password,
            protegido = false,
            id = "null"
        };

        var response = await client.PostAsJsonAsync($"api/utilidades", usuario);
        response.EnsureSuccessStatusCode();
    }
}
