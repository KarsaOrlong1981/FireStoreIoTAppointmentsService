using IoTAppointmentsService.Database;
using IoTAppointmentsService.Models;
using IoTAppointmentsService.Services;
using Microsoft.EntityFrameworkCore;

namespace FireStoreIoTAppointmentsService.Services
{
    public class TodoService : ITodoService
    {
        private readonly IFirebaseService _firebaseService;

        private readonly AppointmentsDbContext _dbContext;

        public TodoService(IFirebaseService firebaseService)
        {
            _dbContext = new AppointmentsDbContext();
            _firebaseService = firebaseService;
            _dbContext.Database.EnsureCreated();
        }

        public async Task<List<TodoList>> GetAllTodoListsAsync()
        {
            return await _dbContext.TodoLists.Include(t => t.Todos).ToListAsync();
        }

        public async Task<TodoList> GetTodoListByIdAsync(string id)
        {
            return await _dbContext.TodoLists.Include(t => t.Todos).FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task AddOrUpdateTodoListAsync(TodoList todoList)
        {
            var existingList = await _dbContext.TodoLists.FindAsync(todoList.Id);
            bool isNewList = existingList == null;

            if (!isNewList)
            {
                // Liste existiert bereits -> Werte aktualisieren
                _dbContext.Entry(existingList).CurrentValues.SetValues(todoList);
                foreach (var task in todoList.Todos)
                {
                    var existingTask = existingList.Todos.FirstOrDefault(t => t.Id == task.Id);
                    if (existingTask != null)
                    {
                        _dbContext.Entry(existingTask).CurrentValues.SetValues(task);
                    }
                    else
                    {
                        existingList.Todos.Add(task);
                    }
                }
            }
            else
            {
                // Neue Liste erstellen
                await _dbContext.TodoLists.AddAsync(todoList);
            }

            await _dbContext.SaveChangesAsync();

            // Nachricht und Titel erstellen
            string title = isNewList
                ? $"{todoList.Name} wurde erstellt."
                : $"{existingList?.Name} wurde aktualisiert.";
            string message = isNewList
                ? "Eine neue Liste wurde hinzugefügt und steht jetzt zur Verfügung."
                : "Die bestehende Liste wurde angepasst.";

            // Notification senden
            await _firebaseService.SendNotificationToAllClients(title, message);
        }

        public async Task DeleteTodoListAsync(string id)
        {
            var list = await _dbContext.TodoLists.FindAsync(id);
            if (list != null)
            {
                var listName = list.Name;
                _dbContext.TodoLists.Remove(list);
                await _dbContext.SaveChangesAsync();
                var title = $"{listName} wurde gelöscht.";
                var message = $"Die liste {listName} wurde entgültig gelöscht.";
                await _firebaseService.SendNotificationToAllClients(title, message);
            }
        }

        public async Task<List<TodoTask>> GetTodoTasksByListIdAsync(string todoListId)
        {
            return await _dbContext.TodoTasks
                .Where(t => t.TodoListId == todoListId)
                .ToListAsync();
        }

        public async Task CreateOrUpdateTodoTaskAsync(TodoTask todoTask)
        {
            var existingTask = await _dbContext.TodoTasks.FindAsync(todoTask.Id);
            bool isNewTask = existingTask == null;

            if (!isNewTask)
            {
                _dbContext.Entry(existingTask).CurrentValues.SetValues(todoTask);
            }
            else
            {
                await _dbContext.TodoTasks.AddAsync(todoTask);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTodoTaskAsync(string todoTaskId)
        {
            try
            {
                // 1. Finde die zugehörige TodoList
                var todoList = await _dbContext.TodoLists
                    .Include(tl => tl.Todos)
                    .FirstOrDefaultAsync(tl => tl.Todos.Any(t => t.Id == todoTaskId));

                if (todoList == null)
                {
                    throw new ApplicationException($"No TodoList contains a TodoTask with ID {todoTaskId}.");
                }

                // 2. Entferne die Aufgabe aus der TodoList
                var taskToRemove = todoList.Todos.FirstOrDefault(t => t.Id == todoTaskId);
                if (taskToRemove != null)
                {
                    todoList.Todos.Remove(taskToRemove);
                    await _dbContext.SaveChangesAsync(); // Speichere Änderungen an der TodoList
                }
            }
            catch (HttpRequestException ex)
            {
                // Logge spezifische HttpRequestException
                //logger.Error(ex, $"An error occurred while removing TodoTask with ID {todoTaskId} from the list.");
                throw new ApplicationException($"Error removing TodoTask with ID {todoTaskId} from the list.", ex);
            }
            catch (Exception ex)
            {
                // Logge allgemeine Fehler
                //logger.Error(ex, "An unexpected error occurred while removing TodoTask from the list.");
                throw;
            }
        }
    }

}
