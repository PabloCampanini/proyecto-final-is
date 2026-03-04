using Microsoft.Data.Sqlite;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly string _ConnectionString;

    public UsuarioRepository(string ConnectionString)
    {
        _ConnectionString = ConnectionString;
    }

    public void CreateUsuario(Usuarios nuevoUsuario)
    {
        string queryString = @"INSERT INTO Usuario (nombre_de_usuario, email, password, id_rol_usuario)
                                VALUES (@NombreDeUsuario, @Email, @Password, @Rol);";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@NombreDeUsuario", nuevoUsuario.NombreDeUsuario));
            command.Parameters.Add(new SqliteParameter("@Email", nuevoUsuario.Email));
            command.Parameters.Add(new SqliteParameter("@Password", nuevoUsuario.Password));
            command.Parameters.Add(new SqliteParameter("@Rol", (int)nuevoUsuario.Rol));

            int filas = command.ExecuteNonQuery();

            if (filas == 0) throw new Exception("No se pudo cargar el usuario. Verifique los datos mandados");

            connection.Close();
        }
    }

    public void UpdateUsuario(int idBuscado, Usuarios usuario)
    {
        string queryString = @"UPDATE Usuario SET nombre_de_usuario = @NombreDeUsuario,
                                                  email = @Email,
                                                  password = @Password,
                                                  id_rol_usuario = @Rol
                               WHERE id_usuario = @idBuscado;";
        
        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter(@"NombreDeUsuario", usuario.NombreDeUsuario));
            command.Parameters.Add(new SqliteParameter(@"Email", usuario.Email));
            command.Parameters.Add(new SqliteParameter(@"Rol", (int)usuario.Rol));
            command.Parameters.Add(new SqliteParameter(@"idBuscado", idBuscado));

            int filas = command.ExecuteNonQuery();

            if (filas == 0) throw new Exception($"No se pudo modificar el usuario id {idBuscado}. Verifique los datos mandados");

            connection.Close();
        }
    }

    public List<Usuarios> GetAllUsuarios()
    {
        List<Usuarios> ListaUsuarios = new List<Usuarios>();

        string queryString = @"SELECT id_usuario, 
                                      nombre_de_usuario, 
                                      email, 
                                      password,
                                      id_rol_usuario 
                               FROM Usuario;";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Usuarios usuarioDb = new Usuarios
                    {
                        IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                        NombreDeUsuario = Convert.ToString(reader["nombre_de_usuario"])!,
                        Email = Convert.ToString(reader["email"])!,
                        Password = Convert.ToString(reader["password"])!,
                        Rol = (RolUsuario)Convert.ToInt32(reader["id_rol_usuario"])
                    };

                    ListaUsuarios.Add(usuarioDb);
                }
            }

            connection.Close();
        }

        if (ListaUsuarios.Count == 0)
        {
            throw new Exception("Error al obtener la lista de usuarios.");
        }

        return ListaUsuarios;
    }

    public Usuarios GetUsuarioById(int idBuscado)
    {
        Usuarios? usuarioDb = null;

        string queryString = @"SELECT id_usuario, nombre_de_usuario,
                                      email, password, id_rol_usuario
                               FROM Usuario
                               WHERE id_usuario = @idBuscado;";
        
        using (SqliteConnection  connection = new  SqliteConnection(_ConnectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if(reader.Read())
                {
                    usuarioDb = new Usuarios
                    {
                      IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                      NombreDeUsuario = Convert.ToString(reader["nombre_de_usuario"])!,
                      Email = Convert.ToString(reader["email"])!,
                      Password = Convert.ToString(reader["password"])!,
                      Rol = (RolUsuario)Convert.ToInt32(reader["id_rol_usuario"])  
                    };
                }
            }

            connection.Close();
        }

        if (usuarioDb == null)
        {
            throw new Exception($"No se encontro un usuario con id {idBuscado}");
        }

        return usuarioDb;
    }

    public void DeleteUsuario(int IdUsuario)
    {
        string queryString = @"DELETE FROM Usuario WHERE id_usuario = @idUsuario;";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@idUsuario", IdUsuario));

            int filas = command.ExecuteNonQuery();

            if (filas == 0) throw new Exception($"Error al borrar. No se encontro un usuario con id {IdUsuario}.");

            connection.Close();
        }
    }

    public void ChangePassword(int idBuscado, string newPassword)
    {
        string queryString = @"UPDATE Usuario SET password = @newPassword WHERE id_usuario = @idBuscado;";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@newPassword", newPassword));
            command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

            int filas = command.ExecuteNonQuery();

            if (filas == 0) throw new Exception("Error al modificar la contraseña. Verifique los datos mandados");

            connection.Close();
        }
    }
}
