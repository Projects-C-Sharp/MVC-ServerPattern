using BookstoreSistem.Data;
using BookstoreSistem.Models;
using BookstoreSistem.Response;
using Microsoft.EntityFrameworkCore;

namespace BookstoreSistem.Servicios;

public class PrestamoService
{
    private readonly BookstoreDbContext _context;

    public PrestamoService(BookstoreDbContext context)
    {
        _context = context;
    }

    public ServiceResponse<IEnumerable<Prestamo>> GetAllPrestamos()
    {
        var prestamos = _context.Prestamos.Include(p => p.Usuario).Include(p => p.Libro).ToList();
        return new ServiceResponse<IEnumerable<Prestamo>>()
        {
            Success = true,
            Data = prestamos
        };
    }

    public ServiceResponse<Prestamo> GetPrestamoById(int id)
    {
        var prestamo = _context.Prestamos.Include(p => p.Usuario).Include(p => p.Libro).FirstOrDefault(x => x.Id == id);
        if (prestamo == null)
        {
            return new ServiceResponse<Prestamo>()
            {
                Success = false,
                Message = "Préstamo no encontrado"
            };
        }
        return new ServiceResponse<Prestamo>()
        {
            Success = true,
            Data = prestamo
        };
    }

    public ServiceResponse<Prestamo> CreatePrestamo(Prestamo prestamo)
    {
        // Check if user exists
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == prestamo.UsuarioId);
        if (usuario == null)
        {
            return new ServiceResponse<Prestamo>()
            {
                Success = false,
                Message = "Usuario no encontrado"
            };
        }

        // Check if book exists
        var libro = _context.Libros.FirstOrDefault(l => l.Id == prestamo.LibroId);
        if (libro == null)
        {
            return new ServiceResponse<Prestamo>()
            {
                Success = false,
                Message = "Libro no encontrado"
            };
        }

        // Check if book is already loaned and not returned
        var existingPrestamo = _context.Prestamos.FirstOrDefault(p => p.LibroId == prestamo.LibroId && p.FechaDevolucion == null);
        if (existingPrestamo != null)
        {
            return new ServiceResponse<Prestamo>()
            {
                Success = false,
                Message = "Libro ya prestado"
            };
        }

        prestamo.FechaPrestamo = DateTime.Now;
        _context.Prestamos.Add(prestamo);
        var response = _context.SaveChanges();

        if (response > 0)
        {
            return new ServiceResponse<Prestamo>()
            {
                Success = true,
                Data = prestamo,
                Message = "Préstamo creado"
            };
        }

        return new ServiceResponse<Prestamo>()
        {
            Success = false,
            Message = "Creación de préstamo fallida"
        };
    }

    public ServiceResponse<Prestamo> UpdatePrestamo(Prestamo prestamo)
    {
        var existingPrestamo = _context.Prestamos.FirstOrDefault(x => x.Id == prestamo.Id);
        if (existingPrestamo == null)
        {
            return new ServiceResponse<Prestamo>()
            {
                Success = false,
                Message = "Préstamo no encontrado"
            };
        }
        existingPrestamo.UsuarioId = prestamo.UsuarioId;
        existingPrestamo.LibroId = prestamo.LibroId;
        existingPrestamo.FechaPrestamo = prestamo.FechaPrestamo;
        existingPrestamo.FechaDevolucion = prestamo.FechaDevolucion;
        var response = _context.SaveChanges();
        if (response > 0)
        {
            return new ServiceResponse<Prestamo>()
            {
                Success = true,
                Data = existingPrestamo,
                Message = "Préstamo actualizado"
            };
        }
        return new ServiceResponse<Prestamo>()
        {
            Success = false,
            Message = "Actualización de préstamo fallida"
        };
    }

    public ServiceResponse<Prestamo> DeletePrestamo(int id)
    {
        var prestamo = _context.Prestamos.FirstOrDefault(x => x.Id == id);
        if (prestamo == null)
        {
            return new ServiceResponse<Prestamo>()
            {
                Success = false,
                Message = "Préstamo no encontrado"
            };
        }
        _context.Prestamos.Remove(prestamo);
        var response = _context.SaveChanges();
        if (response > 0)
        {
            return new ServiceResponse<Prestamo>()
            {
                Success = true,
                Data = prestamo,
                Message = "Préstamo eliminado"
            };
        }
        return new ServiceResponse<Prestamo>()
        {
            Success = false,
            Message = "Eliminación de préstamo fallida"
        };
    }

    public ServiceResponse<Prestamo> DevolverLibro(int id)
    {
        var prestamo = _context.Prestamos.FirstOrDefault(x => x.Id == id);
        if (prestamo == null)
        {
            return new ServiceResponse<Prestamo>()
            {
                Success = false,
                Message = "Préstamo no encontrado"
            };
        }
        if (prestamo.FechaDevolucion != null)
        {
            return new ServiceResponse<Prestamo>()
            {
                Success = false,
                Message = "Libro ya devuelto"
            };
        }
        prestamo.FechaDevolucion = DateTime.Now;
        var response = _context.SaveChanges();
        if (response > 0)
        {
            return new ServiceResponse<Prestamo>()
            {
                Success = true,
                Data = prestamo,
                Message = "Libro devuelto"
            };
        }
        return new ServiceResponse<Prestamo>()
        {
            Success = false,
            Message = "Devolución fallida"
        };
    }
}