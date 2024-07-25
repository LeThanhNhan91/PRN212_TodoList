//using Microsoft.IdentityModel.Tokens;
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
using TodoList;

namespace GUI
{
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		private UserService _service = new();
		public LoginWindow()
		{
			InitializeComponent();
		}

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			string email = EmailTextBox.Text;
			string password = UserPasswordBox.Password;
			if (email == "" || password == "")
			{
				MessageBox.Show("Email or password is empty!", "Field required", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			try
			{
				var user = _service.Login(email, password);
				if (user == null)
				{
					MessageBox.Show("Wrong email or password!", "Incorrect credential", MessageBoxButton.OK);
					return;
				}
				this.Hide();
				switch (user.Role)
				{
					case 0:
						AdminWindow admin = new AdminWindow();
						admin.User = user;
						admin.ShowDialog();
						break;
					case 1:
                        CalendarWindow customer = new CalendarWindow();
						customer.User = user;
						customer.ShowDialog();
						break;
					default:
						MessageBox.Show("You do not have the permission for the system", "Not allowed", MessageBoxButton.OK, MessageBoxImage.Error);
						break;
				}
				this.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void RegisterButton_Click(object sender, RoutedEventArgs e)
		{
			RegisterWindow registerWindow = new RegisterWindow();
			registerWindow.Show();
			this.Close();
		}
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                LoginButton_Click(LoginButton, new RoutedEventArgs());
            }
        }
    }
}
