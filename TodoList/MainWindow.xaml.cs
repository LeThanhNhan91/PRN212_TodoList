using System.Configuration;
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
using System.Windows.Documents;
using System.Windows.Controls;
using System.Printing;
using StackExchange.Redis;


using Microsoft.Extensions.Configuration;


namespace TodoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DateOnly _selectedDate;

        private TaskbarIcon _notifyIcon;
        private TodoService todos = new TodoService();
        private DateOnly _current = DateOnly.FromDateTime(DateTime.Now);
        private Services.TodoService todoService = new Services.TodoService();
        private DispatcherTimer _timer;
        private readonly IConnectionMultiplexer _redis;

        public Repositories.User User { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            todos = new TodoService();
            
            var configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .Build();

            _redis = ConnectionMultiplexer.Connect(configuration. GetConnectionString("RedisConnection"));
            DataContext = todos;
            Closing += Window_Closing;
            InitializeNotification();
            SubscribeToRedis();

        }

        private void InitializeNotification()
        {
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

        private void SubscribeToRedis()
        {
            var subscriber = _redis.GetSubscriber();
            subscriber.Subscribe("task_channel", (channel, message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(message, "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadTasks();
                });
            });
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
            if (User == null)
            {
                MessageBox.Show("User is not logged in!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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
           _selectedDate = DateOnly.FromDateTime(TimeCalendar.SelectedDate.Value);
            ToDoDataGrid.ItemsSource = null;
            ToDoDataGrid.ItemsSource = todoService.GetTasksByUserAndTime(User.UserId, _selectedDate);
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

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = ToDoDataGrid.SelectedItem as Todo;
            if (selected == null)
            {
                MessageBox.Show("Please Select a Task to Share", "Select One", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Open a new window to select the user to share the task with
            ShareTaskWindow shareTaskWindow = new ShareTaskWindow( selected.NoteId);
            shareTaskWindow.ShowDialog();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintTasksOfSelectedDay();
        }
        private void PrintTasksOfSelectedDay()
        {
            if (User == null)
            {
                MessageBox.Show("User is not logged in!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var tasks = todoService.GetTasksByUserAndTime(User.UserId, _selectedDate);
            if (tasks == null || !tasks.Any())
            {
                MessageBox.Show("No tasks for the selected day!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Create a FlowDocument for printing
            FlowDocument doc = new FlowDocument();
            doc.PagePadding = new Thickness(50);
            doc.ColumnWidth = double.PositiveInfinity;

            // Add a title
            Paragraph title = new Paragraph(new Run($"Tasks for {_selectedDate.ToString("D")}"));
            title.FontSize = 20;
            title.TextAlignment = TextAlignment.Center;
            doc.Blocks.Add(title);

            // Add tasks
            foreach (var task in tasks)
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Bold(new Run(task.Title)));
                paragraph.Inlines.Add(new LineBreak());
                paragraph.Inlines.Add(new Run(task.Description));
                paragraph.Inlines.Add(new LineBreak());
                paragraph.Inlines.Add(new Run(task.Time.ToString("g")));
                paragraph.Margin = new Thickness(0, 0, 0, 20);
                doc.Blocks.Add(paragraph);
            }

            // Create a PrintDialog and print the document
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                try
                {
                    IDocumentPaginatorSource idpSource = doc;
                    printDialog.PrintDocument(idpSource.DocumentPaginator, "Tasks of the Selected Day");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Printing error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


    }
}
