public class Usuarios
{
    private int idUsuario;
    private string nombreDeUsuario;
    private string email;
    private string password;
    private RolUsuario rol;
    
    public Usuarios()
    {
    }

    public int IdUsuario { get => idUsuario; set => idUsuario = value; }
    public string NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }
    public string Email { get => email; set => email = value; }
    public string Password { get => password; set => password = value; }
    public RolUsuario Rol { get => rol; set => rol = value; }
}