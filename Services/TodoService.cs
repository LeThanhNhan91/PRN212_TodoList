using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Repositories;
using Services.Interface;
using StackExchange.Redis;

namespace Services
{
	public class TodoService : ITodoService
    {

		private ObservableCollection<Todo> _allTodos = new();
		private TodoRepository _repo = new TodoRepository();
      
        private readonly RedisService _redisPublisher;
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
			_redisPublisher = InitializeRedisService();

        }
        private RedisService InitializeRedisService()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection"));
            return new RedisService(redis);
        }



        public void LoadToDo()
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

		private void OnPropertyChanged(string propertyName)
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

		public List<Todo> GetAllTasks() => _repo.GetAll();
		public void AddTask(Todo todo)
		{
			_repo.Add(todo);
            
		 }
		public void UpdateTask(Todo todo) => _repo.Update(todo);
		public void RemoveTask(Todo todo) => _repo.Remove(todo);

        public List<Todo> GetTasksByUserAndTime(int userId, DateOnly date)
        {
            List<Todo> tasks = GetAllTasks().FindAll(t => t.UserId == userId && DateOnly.FromDateTime(t.Time) == date);

            return tasks;
        }
        public List<Todo> SearchTaskByTitle(string name)
        {
            name = name.ToLower();
           
            
            return _repo.GetAll().Where(x => x.Title.ToLower().Contains(name)).ToList();
        }

		public void ShareTask(int todoId, int userId) {
			_repo.ShareTask(todoId, userId);

			var todo = _repo.GetById(todoId);
			var todoNew = new Todo()
			{
				UserId = userId,
				Title = todo.Title,
				Description = todo.Description,
				ModifiedDate = todo.ModifiedDate,
				Time = todo.Time,
				IsDone = todo.IsDone
			};
			
			_repo.Add(todoNew);
            _redisPublisher.PublishMessageAsync("task_channel", $"New todo added: {todo.Title}").Wait();
        }

    }
}
