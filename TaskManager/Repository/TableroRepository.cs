using Microsoft.Data.Sqlite;

public class TableroRepository : ITableroRepository
{
    private readonly string _ConnectionString;

    public TableroRepository(string ConnectionString)
    {
        _ConnectionString = ConnectionString;
    }

    public Tablero CreateTablero(int idCreador, string nombreTabla, string descripcionTabla)
    {
        Tablero nuevoTablero = new Tablero
        {
            IdUsuarioPropietario = idCreador,
            Nombre = nombreTabla,
            Descripcion = descripcionTabla
        };

        string queryStringInsert = @"INSERT INTO Tablero (id_usuario_propietario, nombre, descripcion)
                                     VALUES(@idCreador, @nombreTabla, @descripcionTabla);";

        string queryStringSelect = @"SELECT MAX(id_tablero) AS id_tablero FROM Tablero;";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            SqliteCommand commandInsert = new SqliteCommand(queryStringInsert, connection);

            connection.Open();

            commandInsert.Parameters.Add(new SqliteParameter("@idCreador", idCreador));
            commandInsert.Parameters.Add(new SqliteParameter("@nombreTabla", nombreTabla));
            commandInsert.Parameters.Add(new SqliteParameter("@descripcionTabla", string.IsNullOrEmpty(descripcionTabla) ? DBNull.Value : descripcionTabla));

            int filas = commandInsert.ExecuteNonQuery();

            if (filas == 0)
            {
                throw new Exception("No se pudo crear el tablero. Verifique los datos proporcionados.");
            }

            SqliteCommand commandSelect = new SqliteCommand(queryStringSelect, connection);

            using (SqliteDataReader reader = commandSelect.ExecuteReader())
            {
                if (reader.Read())
                {
                    nuevoTablero.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                }
            }

            connection.Close();
        }

        return nuevoTablero;
    }

    public void UpdateTablero(int idBuscado, Tablero tableroNuevo)
    {
        string queryString = @"UPDATE Tablero SET id_usuario_propietario = @idUsuarioPropietario,
                                                  nombre = @NombreTabla,
                                                  descripcion = @DescripcionTabla
                               WHERE id_tablero = @idBuscado;";

        using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@idUsuarioPropietario", tableroNuevo.IdUsuarioPropietario));
            command.Parameters.Add(new SqliteParameter("@NombreTabla", tableroNuevo.Nombre));
            command.Parameters.Add(new SqliteParameter("@DescripcionTabla", string.IsNullOrEmpty(tableroNuevo.Descripcion) ? DBNull.Value : tableroNuevo.Descripcion));
            command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

            int filas = command.ExecuteNonQuery();

            if (filas == 0)
            {
                throw new Exception($"No se encontró un tablero con el id {idBuscado} para actualizar.");
            }

            connection.Close();
        }
    }
}