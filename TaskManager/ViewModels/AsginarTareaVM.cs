using System.ComponentModel.DataAnnotations;

public class AsignarTareaVM
{
    public int IdTablero { get; set; }
    public int IdTarea { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un usuario para asignar la tarea.")]
    public int IdUsuarioAsignado { get; set; }

    public List<Usuarios> Usuarios { get; set; } = new List<Usuarios>();

    public AsignarTareaVM() { }

    public AsignarTareaVM(int idTareaB, int idTablero)
    {
        IdTablero = idTablero;
        IdTarea = idTareaB;
    }
}