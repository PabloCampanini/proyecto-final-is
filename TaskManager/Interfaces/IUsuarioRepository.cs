public interface IUsuarioRepository
{
    void CreateUsuario(Usuarios nuevoUsuario);
    void UpdateUsuario(int idBuscado, Usuarios usuario);
    List<Usuarios> GetAllUsuarios();
    Usuarios GetUsuarioById(int idBuscado);
    void DeleteUsuario(int idUsuario);

    //Metodo de dominio especifico
    void ChangePassword(int idBuscado, string newPassword);
}