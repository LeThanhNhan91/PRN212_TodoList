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
using System.Windows.Threading;
using GUI;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using Repositories;
using Services;

namespace TodoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskbarIcon _notifyIcon;
        private TodoService todos = new TodoService();
        private DateOnly _current = DateOnly.FromDateTime(DateTime.Now);
        private Services.TodoService todoService = new Services.TodoService();
        private DispatcherTimer _timer;

        public Repositories.User User { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            todos = new TodoService();
            DataContext = todos;
            Closing += Window_Closing;

            _notifyIcon = new TaskbarIcon();
            _notifyIcon.Icon = new System.Drawing.Icon("favicon.ico");
            _notifyIcon.ToolTipText = "TodoList Application";
            _notifyIcon.TrayLeftMouseUp += NotifyIcon_TrayLeftMouseUp;
            _notifyIcon.ContextMenu = (ContextMenu)FindResource("NotifyIconContextMenu");

            //------------- khai bao thông báo giờ gần đến
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5); 
            _timer.Tick += Timer_Tick;
            _timer.Start();

        }

       

        private void Timer_Tick(object sender, EventArgs e)
        {
            CheckUpcomingTasks();
        }

        private void CheckUpcomingTasks()
        {
            var tasks = todoService.GetTasksByUserAndTime(User.UserId, _current);
            DateTime now = DateTime.Now;

            foreach (var task in tasks)
            {
                DateTime taskTime = task.Time;
                var minutesUntilTask = (taskTime - now).TotalMinutes;

                if (minutesUntilTask <= 5 && minutesUntilTask > 4)
                {
                    _notifyIcon.ShowBalloonTip("Upcoming Task", $"5 minutes left until {task.Title} starts", BalloonIcon.Info);
                }
                else if (minutesUntilTask <= 0 && minutesUntilTask >= -1) 
                {
                    _notifyIcon.ShowBalloonTip("Task Starting", $"Task {task.Title} is starting now!", BalloonIcon.Info);
                }

            }
        }

        private void NotifyIcon_TrayLeftMouseUp(object sender, RoutedEventArgs e)
        {
            ShowWindow();
        }


        //private void AddTodoButton_clicked(object sender, RoutedEventArgs e)
        //{
        //    if (NewTodoTextBox.Text.Length == 0)
        //    {
        //        System.Windows.MessageBox.Show("Please text to the box", "Error input", MessageBoxButton.OK, MessageBoxImage.None);
        //        return;
        //    }
        //    Todo todo = new Todo()
        //    {
        //        Description = NewTodoTextBox.Text,
        //    };
        //    todos.AllTodos.Add(todo);
        //    NewTodoTextBox.Clear();
        //}
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            todos.SaveFileToDo();
            e.Cancel = true;
            this.Hide();
            if (_notifyIcon == null)
            {
                _notifyIcon = new TaskbarIcon();
                _notifyIcon.Icon = new System.Drawing.Icon("favicon.ico");
                _notifyIcon.ShowBalloonTip("TodoList", "The application has been minimized to the system tray.", BalloonIcon.Info);

            }
           

        }
        public void ShowWindow()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTasks();
        }
        private void LoadTasks()
        {
            _current = DateOnly.FromDateTime(DateTime.Now);
            ToDoDataGrid.ItemsSource = todoService.GetTasksByUserAndTime(User.UserId, _current);
        }
        //private void ToDoDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        //{
        //    if (e.Column is DataGridCheckBoxColumn && e.EditAction == DataGridEditAction.Commit)
        //    {
        //        var item = e.Row.Item as Todo; 
        //        if (item != null)
        //        {

        //            item.IsDone = !item.IsDone;
        //            todoService.UpdateTask(item);
        //        }
        //    }
        //}

        private void QuitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _notifyIcon.Dispose();
            Application.Current.Shutdown(); 
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NewTodoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        

        private void TimeCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateOnly date = DateOnly.FromDateTime(TimeCalendar.SelectedDate.Value);
            ToDoDataGrid.ItemsSource = null;
            ToDoDataGrid.ItemsSource = todoService.GetTasksByUserAndTime(User.UserId, date);
        }

        

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {

            TaskDetailWindow taskDetailWindow = new TaskDetailWindow();
            taskDetailWindow.User = User;
            taskDetailWindow.ShowDialog();
            LoadTasks();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var selected  = ToDoDataGrid.SelectedItem as Todo;
            
            if (selected == null)
            {
                MessageBox.Show("Please Select Before Updating", "Selected One", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            TaskDetailWindow taskDetailWindow = new();
            taskDetailWindow.Task = selected;
            taskDetailWindow.User = User;
            taskDetailWindow.ShowDialog();
            LoadTasks();

        }

        private void ToDoDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ToDoDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
           
            if (ToDoDataGrid.CurrentColumn is DataGridCheckBoxColumn)
            {
                
                var item = ToDoDataGrid.CurrentItem as Todo;
                if (item != null)
                {
                    
                    item.IsDone = !item.IsDone;
                   
                    todoService.UpdateTask(item);
                    _current = DateOnly.FromDateTime(item.Time);
                    ToDoDataGrid.ItemsSource = todoService.GetTasksByUserAndTime(User.UserId, _current);
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = ToDoDataGrid.SelectedItem as Todo;
            if (selected == null)
            {
                MessageBox.Show("Please Select Before Deleting", "Select One", MessageBoxButton.OK, MessageBoxImage.Stop); return;
            }
            MessageBoxResult answer = MessageBox.Show("Do you really want to delete ? ", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.No)
            {
                return;
            }

            //nếu cus bấm yes thì :
            todoService.RemoveTask(selected);
            LoadTasks();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string task = BookNameTextBox.Text.ToLower();

            ToDoDataGrid.ItemsSource = null;
            ToDoDataGrid.ItemsSource = todoService.SearchTaskByTitle(task);
        }
    }
}