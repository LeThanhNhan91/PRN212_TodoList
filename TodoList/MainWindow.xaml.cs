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
                

            }
            _notifyIcon.ShowBalloonTip("TodoList", "The application has been minimized to the system tray.", BalloonIcon.Info);

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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NewTodoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddTodoButton_clicked(object sender, RoutedEventArgs e)
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
                }
            }
        }

    }
}