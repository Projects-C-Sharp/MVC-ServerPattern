using BookstoreSistem.Data;
using BookstoreSistem.Models;
using BookstoreSistem.Response;

namespace BookstoreSistem.Servicios;

public class UsuarioService
{
    private readonly BookstoreDbContext _context;

    public UsuarioService(BookstoreDbContext context)
    {
        _context = context;
    }

    public ServiceResponse<IEnumerable<Usuario>> GetAllUsuarios()
    {
        var usuarios = _context.Usuarios.ToList();
        return new ServiceResponse<IEnumerable<Usuario>>()
        {
            Success = true,
            Data = usuarios
        };
    }

    public ServiceResponse<Usuario> GetUsuarioById(int id)
    {
        var usuario = _context.Usuarios.FirstOrDefault(x => x.Id == id);
        if (usuario == null)
        {
            return new ServiceResponse<Usuario>()
            {
                Success = false,
                Message = "Usuario no encontrado"
            };
        }
        return new ServiceResponse<Usuario>()
        {
            Success = true,
            Data = usuario
        };
    }

    public ServiceResponse<Usuario> CreateUsuario(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        var response = _context.SaveChanges();

        if (response > 0)
        {
            return new ServiceResponse<Usuario>()
            {
                Success = true,
                Data = usuario,
                Message = "Usuario creado"
            };
        }

        return new ServiceResponse<Usuario>()
        {
            Success = false,
            Message = "Creación de usuario fallida"
        };
    }

    public ServiceResponse<Usuario> UpdateUsuario(Usuario usuario)
    {
        var existingUsuario = _context.Usuarios.FirstOrDefault(x => x.Id == usuario.Id);
        if (existingUsuario == null)
        {
            return new ServiceResponse<Usuario>()
            {
                Success = false,
                Message = "Usuario no encontrado"
            };
        }
        existingUsuario.Nombre = usuario.Nombre;
        existingUsuario.Apellido = usuario.Apellido;
        existingUsuario.Edad = usuario.Edad;
        var response = _context.SaveChanges();
        if (response > 0)
        {
            return new ServiceResponse<Usuario>()
            {
                Success = true,
                Data = existingUsuario,
                Message = "Usuario actualizado"
            };
        }
        return new ServiceResponse<Usuario>()
        {
            Success = false,
            Message = "Actualización de usuario fallida"
        };
    }

    public ServiceResponse<Usuario> DeleteUsuario(int id)
    {
        var usuario = _context.Usuarios.FirstOrDefault(x => x.Id == id);
        if (usuario == null)
        {
            return new ServiceResponse<Usuario>()
            {
                Success = false,
                Message = "Usuario no encontrado"
            };
        }
        _context.Usuarios.Remove(usuario);
        var response = _context.SaveChanges();
        if (response > 0)
        {
            return new ServiceResponse<Usuario>()
            {
                Success = true,
                Data = usuario,
                Message = "Usuario eliminado"
            };
        }
        return new ServiceResponse<Usuario>()
        {
            Success = false,
            Message = "Eliminación de usuario fallida"
        };
    }
}