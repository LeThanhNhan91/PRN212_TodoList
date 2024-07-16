using Repositories;
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
        private User user = new User();
        public UserDetailWindow()
        {
            InitializeComponent();
        }

        

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserRoleCombobox.SelectedItem is ComboBoxItem selectedItem)
            {
                string displayText = selectedItem.Content.ToString();
                int valueToSave = int.Parse(selectedItem.Tag.ToString());
                user.Role = valueToSave;
                

                // Bạn có thể lưu giá trị này vào cơ sở dữ liệu hoặc xử lý theo yêu cầu
            }
            user.FullName = FullNameTextBox.Text;
            user.Email = EmailTextBox.Text;
            user.Password = PasswordTextBox.Text;
            
            _service.AddUser(user);
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
