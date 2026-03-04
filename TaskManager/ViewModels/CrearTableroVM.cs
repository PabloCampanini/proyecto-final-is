using System.ComponentModel.DataAnnotations;

public class CrearTableroVM
{
    public int IdCreador { get; set; }

    [Required(ErrorMessage = "El campo 'Nombre del tablero' es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre del tablero no puede tener más de 100 caracteres.")]
    public string NombreTablero { get; set; }
    public string DescripcionTablero { get; set; }
}