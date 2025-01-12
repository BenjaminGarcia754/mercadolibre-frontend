using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace frontendnet;

public class ProductosController(ProductosClientService productos, CategoriasClientService categorias, ArchivosClientService archivos, IConfiguration configuration) : Controller
{
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Index(string s)
    {
        List<ProductoA>? lista = [];
        try
        {
            lista = await productos.GetAsync(s);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
        }
        if (User.FindFirstValue(ClaimTypes.Role) == "Administrador")
        {
            ViewBag.SoloAdmin = true;
        }
        ViewBag.Url = configuration["UrlWebAPI"];
        ViewBag.search = s;
        return View(lista);
    }

    [Authorize(Roles = "Administrador, Usuario")]
    public async Task<IActionResult> Detalle(int id)
    {
        ProductoA? item = null;
        ViewBag.Url = configuration["UrlWebAPI"];
        try
        {
            item = await productos.GetAsync(id);
            if (item == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
        }
        return View(item);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear()
    {
        await ProductosDropDownListAsync();
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CrearAsync(ProductoA itemToCreate)
    {
        ViewBag.Url = configuration["UrlWebAPI"];
        if (ModelState.IsValid)
        {
            try
            {
                await productos.PostAsync(itemToCreate);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Salir", "Auth");
                }
            }
        }
        await ProductosDropDownListAsync();
        ModelState.AddModelError("Titulo", "No ha sido posible realizar la acción. Inténtelo nuevamente.");
        return View(itemToCreate);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> EditarAsync(int id)
    {
        ProductoA? itemToEdit = null;
        ViewBag.Url = configuration["UrlWebAPI"];
        try
        {
            itemToEdit = await productos.GetAsync(id);
            if (itemToEdit == null)
            {
                return NotFound();
            }
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
        }
        await ProductosDropDownListAsync();
        return View(itemToEdit);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> EditarAsync(int id, ProductoA itemToEdit)
    {
        if (id != itemToEdit.ProductoId) return NotFound();

        ViewBag.Url = configuration["UrlWebAPI"];
        if (ModelState.IsValid)
        {
            try
            {
                await productos.PutAsync(itemToEdit);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Salir", "Auth");
                }
            }
        }
        await ProductosDropDownListAsync();
        ModelState.AddModelError("Titulo", "No ha sido posible realizar la acción. Inténtelo nuevamente.");
        return View(itemToEdit);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id, bool? showError = false)
    {
        ViewBag.Url = configuration["UrlWebAPI"];
        ProductoA? itemToDelete = null;
        try
        {
            itemToDelete = await productos.GetAsync(id);
            if (itemToDelete == null)
            {
                return NotFound();
            }
            if (showError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "No ha sido posible realizar la acción. Inténtelo nuevamente.";
            }
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
        }
        return View(itemToDelete);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> EliminarAsync(int id)
    {
        ViewBag.Url = configuration["UrlWebAPI"];
        if (ModelState.IsValid)
        {
            try
            {
                await productos.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Salir", "Auth");
                }
            }
        }
        return RedirectToAction(nameof(Eliminar), new { id, showError = true });
    }

    [AcceptVerbs("GET", "POST")]
    [Authorize(Roles = "Administrador")]
    public IActionResult ValidaPoster(string Poster)
    {
        if (Uri.IsWellFormedUriString(Poster, UriKind.Absolute) || Poster.Equals("N/A"))
        {
            return Json(true);
        }
        return Json(false);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Categorias(int id)
    {
        ProductoA? itemToView = null;
        try
        {
            itemToView = await productos.GetAsync(id);
            if (itemToView == null)
            {
                return NotFound();
            }
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
        }
        ViewBag.Url = configuration["UrlWebAPI"];
        ViewData["ProductoId"] = itemToView?.ProductoId;
        return View(itemToView);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CategoriasAgregar(int id)
    {
        ProductoCategoria? itemToView = null;
        try
        {
            ProductoA? producto = await productos.GetAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            await CategoriasDropDownListAsync();
            itemToView = new ProductoCategoria { Producto = producto };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
        }
        ViewBag.Url = configuration["UrlWebAPI"];
        return View(itemToView);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CategoriasAgregar(int id, int categoriaid)
    {
        ProductoA? producto = null;
        if (ModelState.IsValid)
        {
            try
            {
                producto = await productos.GetAsync(id);
                if (producto == null)
                {
                    return NotFound();
                }

                Categoria? categoria = await categorias.GetAsync(categoriaid);
                if (categoria == null)
                {
                    return NotFound();
                }

                await productos.PostAsync(id, categoriaid);
                return RedirectToAction(nameof(Categorias), new { id });
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Salir", "Auth");
                }
            }
        }
        ViewBag.Url = configuration["UrlWebAPI"];
        ModelState.AddModelError("id", "No ha sido posible realizar la acción. Inténtelo nuevamente.");
        await CategoriasDropDownListAsync();
        return View(new ProductoCategoria { Producto = producto });
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CategoriasRemover(int id, int categoriaid, bool? showError = false)
    {
        ProductoCategoria? itemToView = null;
        try
        {
            ProductoA? producto = await productos.GetAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            Categoria? categoria = await categorias.GetAsync(categoriaid);
            if (categoria == null)
            {
                return NotFound();
            }

            itemToView = new ProductoCategoria { Producto = producto, CategoriaId = categoriaid, Nombre = categoria.Nombre };

            if (showError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "No ha sido posible realizar la acción. Inténtelo nuevamente.";
            }
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
        }
        ViewBag.Url = configuration["UrlWebAPI"];
        return View(itemToView);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CategoriasRemover(int id, int categoriaid)
    {
        ViewBag.Url = configuration["UrlWebAPI"];
        if (ModelState.IsValid)
        {
            try
            {
                await productos.DeleteAsync(id, categoriaid);
                return RedirectToAction(nameof(Categorias), new { id });
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Salir", "Auth");
                }
            }
        }
        return RedirectToAction(nameof(CategoriasRemover), new { id, categoriaid, showError = true });
    }

    [Authorize(Roles = "Administrador")]
    private async Task CategoriasDropDownListAsync(object? itemSeleccionado = null)
    {
        var listado = await categorias.GetAsync();
        ViewBag.Categoria = new SelectList(listado, "CategoriaId", "Nombre", itemSeleccionado);
    }

    [Authorize(Roles = "Administrador")]
    private async Task ProductosDropDownListAsync(object? itemSeleccionado = null)
    {
        var listado = await archivos.GetAsync();
        ViewBag.Archivo = new SelectList(listado, "ArchivoId", "Nombre", itemSeleccionado);
    }

}