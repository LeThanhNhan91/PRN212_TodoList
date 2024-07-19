using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private UserService _service = new();
        private readonly Window _loginWindow;

        public User User { get; set; }

        public AdminWindow()
        {
            InitializeComponent();
        }

        private void LoadDataGrid()
        {
            HelloMsgLabel.Content = "Hello, " + User.FullName;
            UserListDataGrid.ItemsSource = null;
            UserListDataGrid.ItemsSource = _service.GetAllUsers();
        }

        private void AdminMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            UserDetailWindow detail = new();
            detail.ShowDialog();
            LoadDataGrid();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = UserListDataGrid.SelectedItem as User;
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user before updating!", "Select one", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            UserDetailWindow detail = new UserDetailWindow(selectedUser);
            detail.ShowDialog();
            LoadDataGrid();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            User selected = UserListDataGrid.SelectedItem as User;
            if (selected == null)
            {
                MessageBox.Show("Please select a row before delete", "Select one", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            MessageBoxResult answer = MessageBox.Show("Do you really want to delete this user", "Confirm?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.No)
                return;

            _service.DeleteUser(selected);
            LoadDataGrid();
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.ToLower();
            string fullName = FullNameTextBox.Text.ToLower();

            List<User> result = _service.SearchUserByEmailAndFullName(email, fullName);
            UserListDataGrid.ItemsSource = null;
            UserListDataGrid.ItemsSource = result;
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            LoginWindow login = new LoginWindow();           
            login.ShowDialog();
        }
    }
}

