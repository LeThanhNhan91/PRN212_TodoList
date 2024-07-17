using Microsoft.IdentityModel.Tokens;
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
			if (email.IsNullOrEmpty() || password.IsNullOrEmpty())
			{
				MessageBox.Show("Email or password is empty!", "Field required", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			var user = _service.Login(email, password);
			if (user == null)
			{
				MessageBox.Show("Wrong email or password!", "Incorrect credential", MessageBoxButton.OK);
				return;
			}
			this.Hide();
			switch (user.Role)
			{
				case 0: (new AdminWindow()).ShowDialog(); break;
				case 1: (new MainWindow()).ShowDialog(); break;
				default: 
					MessageBox.Show("You do not have the permission for the system", "Not allowed", MessageBoxButton.OK, MessageBoxImage.Error); 
					break;
			}
		}
	}
}
