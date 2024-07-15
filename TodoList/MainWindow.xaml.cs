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
using Hardcodet.Wpf.TaskbarNotification;
using Repositories;

namespace TodoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskbarIcon _notifyIcon;
        private TodoService todos;
        public MainWindow()
        {
            InitializeComponent();
            todos = new TodoService();
            DataContext = todos;
            Closing += Window_Closing;
          
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
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
           todos.SaveFileToDo();
            e.Cancel = true;
            this.Hide();
        }
        public void ShowWindow()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NewTodoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}