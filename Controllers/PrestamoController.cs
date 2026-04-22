using Microsoft.AspNetCore.Mvc;
using BookstoreSistem.Models;
using BookstoreSistem.Servicios;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookstoreSistem.Controllers;

public class PrestamoController : Controller
{
    private readonly PrestamoService _prestamoService;
    private readonly UsuarioService _usuarioService;
    private readonly LibroService _libroService;

    public PrestamoController(PrestamoService prestamoService, UsuarioService usuarioService, LibroService libroService)
    {
        _prestamoService = prestamoService;
        _usuarioService = usuarioService;
        _libroService = libroService;
    }

    public IActionResult Index()
    {
        var response = _prestamoService.GetAllPrestamos();
        return View(response.Data);
    }

    public IActionResult Details(int id)
    {
        var response = _prestamoService.GetPrestamoById(id);
        return View(response.Data);
    }

    public IActionResult Create()
    {
        ViewBag.Usuarios = new SelectList(_usuarioService.GetAllUsuarios().Data, "Id", "Nombre");
        ViewBag.Libros = new SelectList(_libroService.GetAllLibros().Data, "Id", "Titulo");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Prestamo prestamo)
    {
        var newPrestamo = _prestamoService.CreatePrestamo(prestamo);
        if (newPrestamo.Success.HasValue && newPrestamo.Success.Value)
        {
            TempData["Message"] = newPrestamo.Message;
            return RedirectToAction("Index");
        }
        TempData["Message"] = newPrestamo.Message;
        ViewBag.Usuarios = new SelectList(_usuarioService.GetAllUsuarios().Data, "Id", "Nombre");
        ViewBag.Libros = new SelectList(_libroService.GetAllLibros().Data, "Id", "Titulo");
        return View(prestamo);
    }

    public IActionResult Edit(int id)
    {
        var response = _prestamoService.GetPrestamoById(id);
        if (response.Success != true)
        {
            TempData["Message"] = response.Message;
            return RedirectToAction("Index");
        }
        ViewBag.Usuarios = new SelectList(_usuarioService.GetAllUsuarios().Data, "Id", "Nombre");
        ViewBag.Libros = new SelectList(_libroService.GetAllLibros().Data, "Id", "Titulo");
        return View(response.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Prestamo prestamo)
    {
        var updatedPrestamo = _prestamoService.UpdatePrestamo(prestamo);
        if (updatedPrestamo.Success ?? false)
        {
            TempData["Message"] = updatedPrestamo.Message;
            return RedirectToAction("Index");
        }
        TempData["Message"] = updatedPrestamo.Message;
        ViewBag.Usuarios = new SelectList(_usuarioService.GetAllUsuarios().Data, "Id", "Nombre");
        ViewBag.Libros = new SelectList(_libroService.GetAllLibros().Data, "Id", "Titulo");
        return View(prestamo);
    }

    public IActionResult Destroy(int id)
    {
        var response = _prestamoService.GetPrestamoById(id);
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
        var deletedPrestamo = _prestamoService.DeletePrestamo(id);
        TempData["Message"] = deletedPrestamo.Message;
        return RedirectToAction("Index");
    }

    public IActionResult Devolver(int id)
    {
        var devolver = _prestamoService.DevolverLibro(id);
        TempData["Message"] = devolver.Message;
        return RedirectToAction("Index");
    }
}