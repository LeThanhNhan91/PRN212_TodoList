using Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ITodoService
    {
        ObservableCollection<Todo> AllTodos { get; set; }
        event PropertyChangedEventHandler? PropertyChanged;

        void LoadToDo();
        void SaveFileToDo();
        List<Todo> GetAllTasks();
        void AddTask(Todo todo);
        void UpdateTask(Todo todo);
        void RemoveTask(Todo todo);
        List<Todo> GetTasksByUserAndTime(int userId, DateOnly date);
        List<Todo> SearchTaskByTitle(string name);
        void ShareTask(int todoId, int userId);
    }
}
