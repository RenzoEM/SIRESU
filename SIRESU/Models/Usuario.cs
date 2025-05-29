using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string Nombre { get; set; }

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string Correo { get; set; }

    [Required]
    [Column(TypeName = "varchar(255)")]
    public string Contraseña { get; set; }

    [Required]
    [Column(TypeName = "varchar(50)")]
    public string Rol { get; set; }
}
