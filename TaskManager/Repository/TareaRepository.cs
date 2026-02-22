using System.Data;
using Microsoft.Data.Sqlite;

public class TareaRepository : ITareaRepository
{
    private readonly string _ConnectionString;

    public TareaRepository(string ConnectionString)
    {
        _ConnectionString = ConnectionString;
    }

    public Tareas CreateTarea(int idTablero)
    {
        Tareas nuevaTarea = new Tareas
        {
            IdTablero = idTablero,
        };

        return nuevaTarea;
    }

    public void CreateTarea(Tareas nuevaTarea)
    {
        string queryString = @"INSERT INTO Tarea (id_tablero, nombre, id_estado, descripcion, 
                                                  id_color, id_usuario_asignado)
                               VALUES (@idTablero, @Nombre, @idEstado, @Descripcion, @idColor, @idUsuario);";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@idTablero", nuevaTarea.IdTablero));
            command.Parameters.Add(new SqliteParameter("@Nombre", nuevaTarea.Nombre));
            command.Parameters.Add(new SqliteParameter("@idEstado", (int)nuevaTarea.Estado));
            command.Parameters.Add(new SqliteParameter("@Descripcion", string.IsNullOrEmpty(nuevaTarea.Descripcion) ? DBNull.Value : nuevaTarea.Descripcion));
            command.Parameters.Add(new SqliteParameter("@idColor", (int)nuevaTarea.Color));
            command.Parameters.Add(new SqliteParameter("@idUsuario", nuevaTarea.IdUsuarioAsignado ?? (object)DBNull.Value));

            int filas = command.ExecuteNonQuery();

            if (filas == 0)
            {
                throw new Exception("Error al crear una tarea. Verifique los datos enviados");
            }

            connection.Close();
        }
    }
}