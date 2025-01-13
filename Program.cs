using frontendnet.Middlewares;
using frontendnet.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var UrlWebAPI = builder.Configuration["URLWebAPI"];
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<EnviaBearerDelegatingHandler>();
builder.Services.AddTransient<RefrescaTokenDelegatingHandler>();
builder.Services.AddHttpClient<AuthClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); });
builder.Services.AddHttpClient<CategoriasClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); })
    .AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
    .AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();
builder.Services.AddHttpClient<UsuariosClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); })
    .AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
    .AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();
builder.Services.AddHttpClient<PedidosClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); })
    .AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
    .AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();
builder.Services.AddHttpClient<RolesClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); })
    .AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
    .AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();
builder.Services.AddHttpClient<ProductosClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); })
    .AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
    .AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();
builder.Services.AddHttpClient<PerfilClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); })
    .AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
    .AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();
builder.Services.AddHttpClient<ArchivosClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); })
    .AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
    .AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();
builder.Services.AddHttpClient<BitacoraClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); })
    .AddHttpMessageHandler<EnviaBearerDelegatingHandler>()
    .AddHttpMessageHandler<RefrescaTokenDelegatingHandler>();
builder.Services.AddHttpClient<ClientesClientService>(httpClient => { httpClient.BaseAddress = new Uri(UrlWebAPI!); } );

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".frontendnet";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.LoginPath = "/Auth";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

builder.Services.AddSingleton<CarritoClientService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
