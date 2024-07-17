using Repositories.Entities;
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
    /// Interaction logic for UserDetailWindow.xaml
    /// </summary>
    public partial class UserDetailWindow : Window
    {
        private UserService _service = new();

        public User SelectedUser { get; set; }

        public UserDetailWindow(User user = null)
        {
            InitializeComponent();
            SelectedUser = user;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            User user = SelectedUser ?? new User();

            if (UserRoleCombobox.SelectedItem is ComboBoxItem selectedItem)
            {
                string displayText = selectedItem.Content.ToString();
                int valueToSave = int.Parse(selectedItem.Tag.ToString());
                user.Role = valueToSave;
            }

            user.FullName = FullNameTextBox.Text;
            user.Email = EmailTextBox.Text;
            user.Password = PasswordTextBox.Text;

            if (SelectedUser == null)
                _service.AddUser(user);
            else
                _service.UpdateUser(user);

            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (SelectedUser != null)
            {
                UsersModelLabel.Content = "Update User Account";
                UserIdTextBox.Text = SelectedUser.UserId.ToString();
                UserIdTextBox.IsEnabled = false;
                FullNameTextBox.Text = SelectedUser.FullName;
                EmailTextBox.Text = SelectedUser.Email;
                PasswordTextBox.Text = SelectedUser.Password;
                UserRoleCombobox.SelectedValue = SelectedUser.Role;
            }
            else
            {
                UsersModelLabel.Content = "Create User Account";
                UserIdTextBox.Text = "Cannot input the userId";
                UserIdTextBox.IsEnabled = false;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

