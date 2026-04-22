using BookstoreSistem.Data;
using BookstoreSistem.Models;
using BookstoreSistem.Response;

namespace BookstoreSistem.Servicios;

public class LibroService
{
    private readonly BookstoreDbContext _context;

    public LibroService(BookstoreDbContext context)
    {
        _context = context;
    }

    public ServiceResponse<IEnumerable<Libro>> GetAllLibros()
    {
        var libros = _context.Libros.ToList();
        return new ServiceResponse<IEnumerable<Libro>>()
        {
            Success = true,
            Data = libros
        };
    }

    public ServiceResponse<Libro> GetLibroById(int id)
    {
        var libro = _context.Libros.FirstOrDefault(x => x.Id == id);
        if (libro == null)
        {
            return new ServiceResponse<Libro>()
            {
                Success = false,
                Message = "Libro no encontrado"
            };
        }
        return new ServiceResponse<Libro>()
        {
            Success = true,
            Data = libro
        };
    }

    public ServiceResponse<Libro> CreateLibro(Libro libro)
    {
        _context.Libros.Add(libro);
        var response = _context.SaveChanges();

        if (response > 0)
        {
            return new ServiceResponse<Libro>()
            {
                Success = true,
                Data = libro,
                Message = "Libro creado"
            };
        }

        return new ServiceResponse<Libro>()
        {
            Success = false,
            Message = "Creación de libro fallida"
        };
    }

    public ServiceResponse<Libro> UpdateLibro(Libro libro)
    {
        var existingLibro = _context.Libros.FirstOrDefault(x => x.Id == libro.Id);
        if (existingLibro == null)
        {
            return new ServiceResponse<Libro>()
            {
                Success = false,
                Message = "Libro no encontrado"
            };
        }
        existingLibro.Titulo = libro.Titulo;
        existingLibro.Autor = libro.Autor;
        existingLibro.Genero = libro.Genero;
        existingLibro.AnioPublicacion = libro.AnioPublicacion;
        var response = _context.SaveChanges();
        if (response > 0)
        {
            return new ServiceResponse<Libro>()
            {
                Success = true,
                Data = existingLibro,
                Message = "Libro actualizado"
            };
        }
        return new ServiceResponse<Libro>()
        {
            Success = false,
            Message = "Actualización de libro fallida"
        };
    }

    public ServiceResponse<Libro> DeleteLibro(int id)
    {
        var libro = _context.Libros.FirstOrDefault(x => x.Id == id);
        if (libro == null)
        {
            return new ServiceResponse<Libro>()
            {
                Success = false,
                Message = "Libro no encontrado"
            };
        }
        _context.Libros.Remove(libro);
        var response = _context.SaveChanges();
        if (response > 0)
        {
            return new ServiceResponse<Libro>()
            {
                Success = true,
                Data = libro,
                Message = "Libro eliminado"
            };
        }
        return new ServiceResponse<Libro>()
        {
            Success = false,
            Message = "Eliminación de libro fallida"
        };
    }
}