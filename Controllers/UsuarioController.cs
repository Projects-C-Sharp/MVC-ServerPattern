using Microsoft.AspNetCore.Mvc;
using BookstoreSistem.Models;
using BookstoreSistem.Servicios;

namespace BookstoreSistem.Controllers;

public class UsuarioController : Controller
{
    private readonly UsuarioService _usuarioService;

    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    public IActionResult Index()
    {
        var response = _usuarioService.GetAllUsuarios();
        return View(response.Data);
    }

    public IActionResult Details(int id)
    {
        var response = _usuarioService.GetUsuarioById(id);
        return View(response.Data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Usuario usuario)
    {
        var newUsuario = _usuarioService.CreateUsuario(usuario);
        if (newUsuario.Success.HasValue && newUsuario.Success.Value)
        {
            TempData["Message"] = newUsuario.Message;
            return RedirectToAction("Index");
        }
        TempData["Message"] = newUsuario.Message;
        return RedirectToAction("Create");
    }

    public IActionResult Edit(int id)
    {
        var response = _usuarioService.GetUsuarioById(id);
        if (response.Success != true)
        {
            TempData["Message"] = response.Message;
            return RedirectToAction("Index");
        }
        return View(response.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Usuario usuario)
    {
        var updatedUsuario = _usuarioService.UpdateUsuario(usuario);
        if (updatedUsuario.Success ?? false)
        {
            TempData["Message"] = updatedUsuario.Message;
            return RedirectToAction("Index");
        }
        TempData["Message"] = updatedUsuario.Message;
        return View(usuario);
    }

    public IActionResult Destroy(int id)
    {
        var response = _usuarioService.GetUsuarioById(id);
        if (!(response.Success ?? false))
        {
            TempData["Message"] = response.Message;
            return RedirectToAction("Index");
        }
        return View(response.Data);
    }

    [HttpPost, ActionName("Destroy")]
    [ValidateAntiForgeryToken]
    public IActionResult DestroyConfirmed(int id)
    {
        var deletedUsuario = _usuarioService.DeleteUsuario(id);
        TempData["Message"] = deletedUsuario.Message;
        return RedirectToAction("Index");
    }
}