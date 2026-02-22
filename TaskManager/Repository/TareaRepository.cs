using System.Data;
using Microsoft.Data.Sqlite;

public class TareaRepository : ITareaRepository
{
    private readonly string _ConnectionString;

    public TareaRepository(string ConnectionString)
    {
        _ConnectionString = ConnectionString;
    }

}