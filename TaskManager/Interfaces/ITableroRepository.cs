public interface ITableroRepository
{
    Tablero CreateTablero(int idCreador, string nombreTabla, string descripcionTabla);
    void UpdateTablero(int idBuscado, Tablero tableroNuevo);
    Tablero GetTableroByIdTablero(int idTableroBuscado);
    List<Tablero> GetAllTableros();
    List<Tablero> GetAllTablerosByIdCreador(int idCreador);
    List<Tablero> GetAllTablerosByIdUsAsignado(int idUsuarioAs);
    //void DeleteTablero(int idTableroB);
}