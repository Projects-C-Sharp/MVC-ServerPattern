using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreSistem.Models;

public class Prestamo
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int LibroId { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime? FechaDevolucion { get; set; }

    [ForeignKey("UsuarioId")]
    public Usuario? Usuario { get; set; }

    [ForeignKey("LibroId")]
    public Libro? Libro { get; set; }
}