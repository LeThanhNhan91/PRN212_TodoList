﻿using Repositories;
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
    /// Interaction logic for TaskDetailWindow.xaml
    /// </summary>
    public partial class TaskDetailWindow : Window
    {
		public User User { get; set; }
		public Todo Task { get; set; }
		public DateTime Time = DateTime.Now;

		public TaskDetailWindow()
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
			StatusComboBox.Items.Clear();
			HourComboBox.Items.Clear();
			MinuteComboBox.Items.Clear();
			PeriodDatePicker.SelectedDate = null;
			NoteIdLabel.Content = "";

			List<int> hours = new List<int>();
			List<int> minutes = new List<int>();
			for (int i = 0; i <= 60; i++)
			{
				if (i <= 23)
					hours.Add(i);
				minutes.Add(i);
			}
			HourComboBox.ItemsSource = hours;
			MinuteComboBox.ItemsSource = minutes;
			NoteIdLabel.Visibility = Visibility.Collapsed;
			UserIdLabel.Content = User.UserId;
			UserIdLabel.Visibility = Visibility.Collapsed;
			StatusComboBox.ItemsSource = new List<string>() { "Done", "Not yet" };
		}

		private void Grid_Loaded(object sender, RoutedEventArgs e)
		{
			string title = "";
			DateTime date = DateTime.Now;
			//----------------------------------
			LoadData();
			// Update
			if (Task != null)
			{
				title = "Update task";
				NoteIdLabel.Content = Task.NoteId;
				TitleTextBox.Text = Task.Title;
				DescriptionTextbox.Text = Task.Description;
				PeriodDatePicker.Text = Task.Time.ToString();
				date = Task.Time;
				StatusComboBox.SelectedValue = Task.IsDone == true ? "Done" : "Not yet";
				NoteIdLabel.Content = Task.NoteId.ToString();
			}
			else
			{
				title = "Create task";
				StatusComboBox.IsEnabled = false;
			}
			PeriodDatePicker.SelectedDate = date;
			HourComboBox.SelectedValue = date.Hour;
			MinuteComboBox.SelectedValue = date.Minute;
			TaskDetailTitleLabel.Content = title;
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			Todo task = new Todo();
			int hour = string.IsNullOrEmpty(HourComboBox.SelectedItem.ToString()) ? 0 : int.Parse(HourComboBox.SelectedItem.ToString());
			int minute = string.IsNullOrEmpty(MinuteComboBox.SelectedItem.ToString()) ? 0 : int.Parse(MinuteComboBox.SelectedItem.ToString());
			DateTime rawDate = PeriodDatePicker.SelectedDate == null ? DateTime.Now : PeriodDatePicker.SelectedDate.Value;
			//---------------------------------------------------
			DateTime date = new DateTime(rawDate.Year, rawDate.Month, rawDate.Day, hour, minute, minute, 0);
			bool status = false;
			// Create
			if (string.IsNullOrEmpty(NoteIdLabel.Content.ToString()))
			{
				if (string.IsNullOrEmpty(TitleTextBox.Text))
				{
					MessageBox.Show("Title can't be empty", "Error Notification", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				task.Title = TitleTextBox.Text;
				task.Description = DescriptionTextbox.Text;
			}
			// Update
			else
			{
				if (!string.IsNullOrEmpty(TitleTextBox.Text))
					task.Title = TitleTextBox.Text;
				if (!string.IsNullOrEmpty(DescriptionTextbox.Text))
					task.Description = DescriptionTextbox.Text;
				if (StatusComboBox.SelectedValue == "Done")
					status = true;
				task.NoteId = int.Parse(NoteIdLabel.Content.ToString());
			}
			//----------------------------------------------------------
			task.Time = date;
			task.IsDone = status;
			task.ModifiedDate = DateTime.Now;
			task.UserId = int.Parse(UserIdLabel.Content.ToString());
			if (Task != null)
			{
				// Gọi service update
			}
			else
			{
				// Gọi service add
			}
			this.Close();
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}