using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GUI;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Models;
using Services;

namespace TodoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskbarIcon _notifyIcon;
        private TodoHelper todos;
        private TodoService _todoService;
        private DateOnly _currentWeekStart = DateOnly.FromDateTime(DateTime.Now);
        public User user { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            todos = new TodoHelper();
            UpdateWeekLabel();
            DataContext = todos;
            Closing += Window_Closing;

            _notifyIcon = new TaskbarIcon();
            _notifyIcon.Icon = new System.Drawing.Icon("favicon.ico");
            _notifyIcon.ToolTipText = "TodoList Application";
            _notifyIcon.TrayLeftMouseUp += NotifyIcon_TrayLeftMouseUp;
        }


        private void NotifyIcon_TrayLeftMouseUp(object sender, RoutedEventArgs e)
        {
            ShowWindow();
        }


        private void AddTodoButton_clicked(object sender, RoutedEventArgs e)
        {
            if (NewTodoTextBox.Text.Length == 0)
            {
                System.Windows.MessageBox.Show("Please text to the box", "Error input", MessageBoxButton.OK, MessageBoxImage.None);
                return;
            }
            Todo todo = new Todo()
            {
                Description = NewTodoTextBox.Text,
            };
            todos.AllTodos.Add(todo);
            NewTodoTextBox.Clear();
        }

        private void GetCurrentWeek()
        {
            if (_currentWeekStart.DayOfWeek.ToString().Equals("Monday"))
            {
                return;
            }
            while (_currentWeekStart.DayOfWeek.ToString() != "Monday")
            {
                _currentWeekStart = _currentWeekStart.AddDays(-1);
            }
        }

        private void UpdateWeekLabel()
        {

            var weekEnd = _currentWeekStart.AddDays(6);
            CurrentWeekLabel.Content = $"{_currentWeekStart:dd/MM/yyyy} - {weekEnd:dd/MM/yyyy}";
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            todos.SaveFileToDo();
            e.Cancel = true;
            this.Hide();
            if (_notifyIcon == null)
            {
                _notifyIcon = new TaskbarIcon();
                _notifyIcon.Icon = new System.Drawing.Icon("favicon.ico");


            }
            _notifyIcon.ShowBalloonTip("TodoList", "The application has been minimized to the system tray.", BalloonIcon.Info);

        }
        private void ShowWindow()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
        }
        private void LoadTasks()
        {
            var tasks = _todoService.GetAllTodos()
                        .Where(t => t.UserId == user.UserId
                            && t.Time >= _currentWeekStart.ToDateTime(TimeOnly.MinValue)
                            && t.Time < _currentWeekStart.AddDays(7).ToDateTime(TimeOnly.MinValue))
                        .ToList();


            var currentWeekTasks = tasks.GroupBy(t => t.Time.DayOfWeek)
                                        .ToDictionary(g => g.Key, g => g.Select(t => t.Title).FirstOrDefault());

            var viewModel = new TodoViewModel
            {
                Monday = currentWeekTasks.ContainsKey(DayOfWeek.Monday) ? currentWeekTasks[DayOfWeek.Monday] : string.Empty,
                Tuesday = currentWeekTasks.ContainsKey(DayOfWeek.Tuesday) ? currentWeekTasks[DayOfWeek.Tuesday] : string.Empty,
                Wednesday = currentWeekTasks.ContainsKey(DayOfWeek.Wednesday) ? currentWeekTasks[DayOfWeek.Wednesday] : string.Empty,
                Thursday = currentWeekTasks.ContainsKey(DayOfWeek.Thursday) ? currentWeekTasks[DayOfWeek.Thursday] : string.Empty,
                Friday = currentWeekTasks.ContainsKey(DayOfWeek.Friday) ? currentWeekTasks[DayOfWeek.Friday] : string.Empty,
                Saturday = currentWeekTasks.ContainsKey(DayOfWeek.Saturday) ? currentWeekTasks[DayOfWeek.Saturday] : string.Empty,
                Sunday = currentWeekTasks.ContainsKey(DayOfWeek.Sunday) ? currentWeekTasks[DayOfWeek.Sunday] : string.Empty
            };

            TasksDataGrid.ItemsSource = new List<TodoViewModel> { viewModel };
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NewTodoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TasksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrevWeekButton_Click(object sender, RoutedEventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(-7);
            UpdateWeekLabel();
            LoadTasks();
        }

        private void NextWeekButton_Click(object sender, RoutedEventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(7);
            UpdateWeekLabel();
            LoadTasks();
        }
    }
}