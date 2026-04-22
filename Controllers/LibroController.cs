using Microsoft.AspNetCore.Mvc;
using BookstoreSistem.Models;
using BookstoreSistem.Servicios;

namespace BookstoreSistem.Controllers;

public class LibroController : Controller
{
    private readonly LibroService _libroService;

    public LibroController(LibroService libroService)
    {
        _libroService = libroService;
    }

    public IActionResult Index()
    {
        var response = _libroService.GetAllLibros();
        return View(response.Data);
    }

    public IActionResult Details(int id)
    {
        var response = _libroService.GetLibroById(id);
        return View(response.Data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Libro libro)
    {
        var newLibro = _libroService.CreateLibro(libro);
        if (newLibro.Success.HasValue && newLibro.Success.Value)
        {
            TempData["Message"] = newLibro.Message;
            return RedirectToAction("Index");
        }
        TempData["Message"] = newLibro.Message;
        return RedirectToAction("Create");
    }

    public IActionResult Edit(int id)
    {
        var response = _libroService.GetLibroById(id);
        if (response.Success != true)
        {
            TempData["Message"] = response.Message;
            return RedirectToAction("Index");
        }
        return View(response.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Libro libro)
    {
        var updatedLibro = _libroService.UpdateLibro(libro);
        if (updatedLibro.Success ?? false)
        {
            TempData["Message"] = updatedLibro.Message;
            return RedirectToAction("Index");
        }
        TempData["Message"] = updatedLibro.Message;
        return View(libro);
    }

    public IActionResult Destroy(int id)
    {
        var response = _libroService.GetLibroById(id);
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
        var deletedLibro = _libroService.DeleteLibro(id);
        TempData["Message"] = deletedLibro.Message;
        return RedirectToAction("Index");
    }
}