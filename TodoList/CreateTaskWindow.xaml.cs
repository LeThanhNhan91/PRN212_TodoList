using Microsoft.IdentityModel.Tokens;
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
    /// Interaction logic for CreateTaskWindow.xaml
    /// </summary>
    public partial class CreateTaskWindow : Window
    {

		public User User { get; set; }
		public TodoService _service = new TodoService();
        public CreateTaskWindow()
        {
            InitializeComponent();
        }


		private void TitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

        }

		private void DescriptionTextbox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void LoadData()
		{
			HourComboBox.Items.Clear();
			MinuteComboBox.Items.Clear();
			List<int> hours = new List<int>();
			List<int> minutes = new List<int>();
			for (int i = 1; i <= 60; i++)
			{
				if (i <= 24)
					hours.Add(i);
				minutes.Add(i);
			}
			HourComboBox.ItemsSource = hours;
			MinuteComboBox.ItemsSource = minutes;
		}

		private void Grid_Loaded(object sender, RoutedEventArgs e)
		{
			LoadData();
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			if (TitleTextBox.Text.IsNullOrEmpty())
			{
				MessageBox.Show("Title can't be empty", "Error Notification", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			int hour = HourComboBox.SelectedItem.ToString().IsNullOrEmpty() ? 0 : int.Parse(HourComboBox.SelectedItem.ToString());
			int minute = MinuteComboBox.SelectedItem.ToString().IsNullOrEmpty() ? 0 : int.Parse(MinuteComboBox.SelectedItem.ToString());
			DateTime rawDate = PeriodDatePicker.SelectedDate == null ? DateTime.Now : PeriodDatePicker.SelectedDate.Value;

			DateTime date = new DateTime(rawDate.Year, rawDate.Month, rawDate.Day, hour, minute, minute, 0);

			if (date < DateTime.Now)
			{

				MessageBox.Show("Invalid date", "Error Notification", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
