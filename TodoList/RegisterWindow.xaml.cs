using Repositories;
using Services;
using Services.Validation;
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
	/// Interaction logic for RegisterWindow.xaml
	/// </summary>
	public partial class RegisterWindow : Window
	{
		private UserValidationWrapper _validationWrapper;
		private UserService _userService = new();
		public RegisterWindow()
		{
			InitializeComponent();
		}
		private bool ValidateUser(string email, string fullname, string password, string confirmPassword)
		{
			if (!AnyEmpty(email, fullname, password, confirmPassword))
			{
				MessageBox.Show("All fields are required!", "Field required", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}
			_validationWrapper = new UserValidationWrapper();
			_validationWrapper.EmailValidation.Email = email;
			_validationWrapper.FullNameValidation.FullName = fullname;
			_validationWrapper.PasswordValidation.Password = password;
			if (_validationWrapper.EmailValidation.HasError)
			{
				MessageBox.Show("Email is not in correct format!", "Bad format", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}
			if (_validationWrapper.FullNameValidation.HasError)
			{
				MessageBox.Show("Full name is not in correct format!\nFull name is: LastName FirstName, and at least 6 characters", "Bad format", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}
			if (_validationWrapper.PasswordValidation.HasError)
			{
				MessageBox.Show("Password must be at least 6 characters and must have: at least a lower case character, an upper case character, a number and a special characer", "Bad format", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}
			if (password != confirmPassword)
			{
				MessageBox.Show("Password is not the same as confirm password!", "Input mismatch", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}
			bool duplicatedEmail = _userService.GetAllUsers().FirstOrDefault(x => x.Email == email) != null;
			if (duplicatedEmail)
			{
				MessageBox.Show("Duplicated email!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}
			return true;
		}
		private void RegisterButton_Click(object sender, RoutedEventArgs e)
		{
			string email = EmailTextBox.Text;
			string fullname = FullNameTextBox.Text;
			string password = UserPasswordBox.Password;
			string passwordConfirm = PasswordConfirmPasswordBox.Password;
			try
			{
				if (!ValidateUser(email, fullname, password, passwordConfirm)) return;
				User u = new()
				{
					Email = email,
					FullName = fullname,
					Password = password,
					Role = 1,
				};
				_userService.AddUser(u);
				MessageBox.Show("Register success. You'll be redirected to login window.", "Success");
				LoginWindow loginWindow = new();
				loginWindow.Show();
				this.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		private static bool AnyEmpty(params string[] strings) => (strings.FirstOrDefault(x => x == null || x.Length == 0) == null);

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			LoginWindow loginWindow = new LoginWindow();
			loginWindow.Show();
			this.Close();
		}
	}
}
