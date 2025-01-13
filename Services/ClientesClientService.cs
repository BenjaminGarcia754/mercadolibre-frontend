using frontendnet.Models;

namespace frontendnet.Services;

public class ClientesClientService(HttpClient client)
{
    public async Task PostAsync(Client cliente)
    {
        Usuario usuario = new Usuario
        {
            email = cliente.Email,
            nombre = cliente.Nombre,
            rolid = "51d11c7c-54aa-452f-8178-be26d96e1dba",
            passwordhash = cliente.Password,
            protegido = false,
            id = "null"
        };

        //Hello World
        var response = await client.PostAsJsonAsync($"api/utilidades", usuario);
        response.EnsureSuccessStatusCode();
    }
}
