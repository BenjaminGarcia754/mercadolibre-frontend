using frontendnet.Models;

namespace frontendnet.Services
{
    public class UsuarioClientService(HttpClient client)
    {
        public async Task PostAsync (Client cliente)
        {
            Usuario usuario = new Usuario
            {
                email = cliente.Email,
                nombre = cliente.Nombre,
                rolid = "6db6772f-1730-4021-8dc1-9954d341fd55",
                passwordhash = cliente.Password,
                protegido = false,
                id = null
            };

            var response = await client.PostAsJsonAsync("api/utilidades", usuario);
        }
    }
}
