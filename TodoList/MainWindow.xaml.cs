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

namespace TodoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Todos todos;
        public MainWindow()
        {
            InitializeComponent();
            todos = new Todos();
            DataContext = todos;
        }

        private void AddTodoButton_clicked(object sender, RoutedEventArgs e)
        {
            if (NewTodoTextBox.Text.Length == 0)
            {
                MessageBox.Show("Please text to the box", "Error input", MessageBoxButton.OK, MessageBoxImage.None);
                return;
            }
            Todo todo = new Todo()
            {
                Desc = NewTodoTextBox.Text,
            };
            todos.AllTodos.Add(todo);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NewTodoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}