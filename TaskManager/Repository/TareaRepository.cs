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

            command.ExecuteNonQuery();
        }
    }

    public void UpdateTarea(int idTarea, Tareas tarea)
    {
        string queryString = @"UPDATE Tarea SET id_tablero = @idTablero,
                                                nombre = @Nombre,
                                                id_estado = @idEstado,
                                                descripcion = @Descripcion,
                                                id_color = @idColor,
                                                id_usuario_asignado = @idUsuario
                               WHERE id_tarea = @idTarea;";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@idTablero", tarea.IdTablero));
            command.Parameters.Add(new SqliteParameter("@Nombre", tarea.Nombre));
            command.Parameters.Add(new SqliteParameter("@idEstado", (int)tarea.Estado));
            command.Parameters.Add(new SqliteParameter("@Descripcion", string.IsNullOrEmpty(tarea.Descripcion) ? DBNull.Value : tarea.Descripcion));
            command.Parameters.Add(new SqliteParameter("@idColor", (int)tarea.Color));

            if (tarea.IdUsuarioAsignado > 0)
            {
                command.Parameters.Add(new SqliteParameter("@idUsuario", tarea.IdUsuarioAsignado));
            }
            else
            {
                command.Parameters.Add(new SqliteParameter("@idUsuario", DBNull.Value));
            }

            command.Parameters.Add(new SqliteParameter("@idTarea", idTarea));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public Tareas GetTareaByIdTarea(int idTareaBuscada)
    {
        Tareas tareaDb = null;

        string queryString = @"SELECT id_tarea,
                                      id_tablero,
                                      nombre,
                                      id_estado,
                                      descripcion,
                                      id_color,
                                      id_usuario_asignado
                               FROM Tarea
                               WHERE id_tarea = @idTareaBuscada;";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@idTareaBuscada", idTareaBuscada));

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    tareaDb = new Tareas
                    {
                        IdTarea = Convert.ToInt32(reader["id_tarea"]),
                        IdTablero = Convert.ToInt32(reader["id_tablero"]),
                        Nombre = Convert.ToString(reader["nombre"]),
                        Estado = (EstadoTarea)Convert.ToInt32(reader["id_estado"]),
                        Descripcion = Convert.ToString(reader["descripcion"]),
                        Color = (Color)Convert.ToInt32(reader["id_color"]),
                        IdUsuarioAsignado = Convert.ToInt32(reader["id_usuario_asignado"] == DBNull.Value ? null : reader["id_usuario_asignado"])
                    };
                }
            }

            connection.Close();
        }

        return tareaDb;
    }

    public List<Tareas> GetAllTareasByIdUsuario(int idUsuario)
    {
        List<Tareas> ListaTareas = new List<Tareas>();

        string queryString = @"SELECT id_tarea,
                                    id_tablero,
                                    nombre,
                                    id_estado,
                                    descripcion,
                                    id_color,
                                    id_usuario_asignado
                            FROM Tarea
                            WHERE id_usuario_asignado = @idUsuario;";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@idUsuario", idUsuario));

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tareas tareaDb = new Tareas
                    {
                        IdTarea = Convert.ToInt32(reader["id_tarea"]),
                        IdTablero = Convert.ToInt32(reader["id_tablero"]),
                        Nombre = Convert.ToString(reader["nombre"]),
                        Estado = (EstadoTarea)Convert.ToInt32(reader["id_estado"]),
                        Descripcion = Convert.ToString(reader["descripcion"]),
                        Color = (Color)Convert.ToInt32(reader["id_color"]),
                        IdUsuarioAsignado = reader["id_usuario_asignado"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(reader["id_usuario_asignado"])
                    };

                    ListaTareas.Add(tareaDb);
                }
            }

            connection.Close();
        }

        return ListaTareas;
    }   

    public List<Tareas> GetAllTareasByIdTablero(int idTablero)
    {
        List<Tareas> ListaTareas = new List<Tareas>();

        string queryString = @"SELECT id_tarea,
                                    id_tablero,
                                    nombre,
                                    id_estado,
                                    descripcion,
                                    id_color,
                                    id_usuario_asignado
                            FROM Tarea
                            WHERE id_tablero = @idTablero;";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@idTablero", idTablero));

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tareas tareaDb = new Tareas
                    {
                        IdTarea = Convert.ToInt32(reader["id_tarea"]),
                        IdTablero = Convert.ToInt32(reader["id_tablero"]),
                        Nombre = Convert.ToString(reader["nombre"]),
                        Estado = (EstadoTarea)Convert.ToInt32(reader["id_estado"]),
                        Descripcion = Convert.ToString(reader["descripcion"]),
                        Color = (Color)Convert.ToInt32(reader["id_color"]),
                        IdUsuarioAsignado = reader["id_usuario_asignado"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(reader["id_usuario_asignado"])
                    };

                    ListaTareas.Add(tareaDb);
                }
            }

            connection.Close();
        }

        return ListaTareas;
    }

    public void DeleteTarea(int idTareaB)
    {
        string queryString = @"DELETE FROM Tarea WHERE id_tarea = @idTareaB;";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@idTareaB", idTareaB));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void AsignarTarea(int idUsuario, int idTarea)
    {
        string queryString = @"UPDATE Tarea SET id_usuario_asignado = @idUsuario
                               WHERE id_tarea = @idTarea;";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@idTarea", idTarea));

            if (idUsuario > 0)
            {
                command.Parameters.Add(new SqliteParameter("@idUsuario", idUsuario));
            }
            else
            {
                command.Parameters.Add(new SqliteParameter("@idUsuario", DBNull.Value));
            }


            command.ExecuteNonQuery();

            connection.Close();
        }
    }

}