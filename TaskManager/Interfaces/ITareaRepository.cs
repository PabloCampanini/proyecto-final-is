public interface ITareaRepository
{
    Tareas CreateTarea(int idTablero);
    void CreateTarea(Tareas nuevaTarea);
    //void UpdateTarea(int idTarea, Tareas tarea);
    //Tareas GetTareaByIdTarea(int idTareaBuscada);
    //List<Tareas> GetAllTareasByIdUsuario(int idUsuario);
    //List<Tareas> GetAllTareasByIdTablero(int idTablero);
    //void DeleteTarea(int idTareaB);
    //void AsignarTarea(int idUsuario, int idTarea);
}