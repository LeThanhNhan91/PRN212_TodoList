using Repositories;
using Services;
using System;
using System.Windows;
using Services.Validation;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interaction logic for UserDetailWindow.xaml
    /// </summary>
    public partial class UserDetailWindow : Window
    {
        private UserService _service = new();

        public User SelectedUser { get; set; }

        private UserValidationWrapper _validationWrapper;

        public UserDetailWindow(User user = null)
        {
            InitializeComponent();
            SelectedUser = user;
            _validationWrapper = new UserValidationWrapper();
            DataContext = _validationWrapper;

            if (SelectedUser != null)
            {
                _validationWrapper.EmailValidation.Email = SelectedUser.Email;
                _validationWrapper.FullNameValidation.FullName = SelectedUser.FullName;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_validationWrapper.EmailValidation.HasError || _validationWrapper.EmailValidation.Email is null || _validationWrapper.FullNameValidation.HasError)
            {
                return;
            }
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
