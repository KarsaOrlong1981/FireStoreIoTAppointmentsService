using IoTAppointmentsService.Models;

namespace FireStoreIoTAppointmentsService.Services
{
    public interface ITodoService
    {
        Task<List<TodoList>> GetAllTodoListsAsync();
        Task<TodoList> GetTodoListByIdAsync(string id);
        Task AddOrUpdateTodoListAsync(TodoList todoList);
        Task DeleteTodoListAsync(string id);
        Task<List<TodoTask>> GetTodoTasksByListIdAsync(string todoListId);
        Task CreateOrUpdateTodoTaskAsync(TodoTask todoTask);
        Task DeleteTodoTaskAsync(string todoTaskId);
    }

}
