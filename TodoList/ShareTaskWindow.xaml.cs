using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for ShareTaskWindow.xaml
    /// </summary>
    public partial class ShareTaskWindow : Window
    {
        private int _todoId;
        private TodoService _todoService;
        private UserService _userService;
        public ShareTaskWindow(int todoId)
        {
            InitializeComponent();
            _todoId = todoId;
            _todoService = new TodoService();
            LoadUsers();
        }
        private void LoadUsers()
        {
            _userService = new UserService();
            var users = _userService.GetAllUsers();
            UsersComboBox.ItemsSource = users;
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersComboBox.SelectedItem as Repositories.User;
            if (selectedUser == null)
            {
                MessageBox.Show("Please Select a User to Share With", "Select One", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _todoService.ShareTask(_todoId, selectedUser.UserId);
            MessageBox.Show("Task Shared Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
