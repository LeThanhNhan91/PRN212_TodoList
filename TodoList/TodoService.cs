using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;



namespace TodoList
{
    public class TodoService : INotifyPropertyChanged
    {
        private ObservableCollection<Todo> _allTodos = new();
        private string FileToDo
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TodoList", "todos.json");
            }
        }

        public TodoService()
        {
            LoadToDo();
        }

       
        
        public void LoadToDo ()
        {
            if (!File.Exists(FileToDo))
            {
                return;
            }
            var json = File.ReadAllText(FileToDo);
                var todos = JsonSerializer.Deserialize<List<Todo>>(json);
                _allTodos = new ObservableCollection<Todo>(todos);
        }

        /// <summary>
        /// Save file vào local/roaming, lần đầu (chưa có file) thì sẽ tạo ở roaming 1 file json
        /// </summary>
        public void SaveFileToDo()
        {
            
            var pathOfFile = Path.GetDirectoryName(FileToDo);
            if (!Directory.Exists(pathOfFile))
            {
                Directory.CreateDirectory(pathOfFile);
            }
            var todos = new List<Todo>(_allTodos);
            //new JsonSerializerOptions { WriteIndented = true } là thụt lề
            var json = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileToDo, json);

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged (string propertyName)
        {
            //method to invoke the event
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       
        public ObservableCollection<Todo> AllTodos
        {
            get { return _allTodos; }
            set 
            { 
                _allTodos = value;
                OnPropertyChanged(nameof(AllTodos));
            }
        }
    }
}
