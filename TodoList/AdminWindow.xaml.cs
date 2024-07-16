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
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private UserService _service = new();


        public AdminWindow()
        {
            InitializeComponent();
        }

       private void LoadDataGrid()
        {
            UserListDataGrid.ItemsSource = null;
            UserListDataGrid.ItemsSource = _service.GetAllUsers();
        }

        private void AdminMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            //trang detail
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            //trang detail
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
    }
}
