using Microsoft.IdentityModel.Tokens;
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
	/// Interaction logic for NotificationDetailWindow.xaml
	/// </summary>
	public partial class NotificationDetailWindow : Window
	{

		public int NotiId { get; set; }
		private NotiService _notiService = new NotiService();
		private TodoService _todoService = new TodoService();
		public NotificationDetailWindow()
		{
			InitializeComponent();
		}


		private void LoadData()
		{
			InvitationNotification notification = _notiService.GetAllNotis().FirstOrDefault(x => x.NotiId == NotiId);
			DecisionComboBox.ItemsSource = new List<string>() { "Deny", "Accept" };
			DecisionComboBox.SelectedIndex = 0;
			//--------------------------------------------------
			Todo assignedTask = _todoService.GetAllTasks().Where(x => x.NoteId == notification.OriginalTaskId).FirstOrDefault();
			OriginTaskTitleTextBox.Text = assignedTask.Title;
			OriginTaskStatusTextBox.Text = assignedTask.IsDone ? "Done" : "Not yet";
		}

		private void NotiDetailSaveButton_Click(object sender, RoutedEventArgs e)
		{
			string decision = DecisionComboBox.SelectedItem as string;
			InvitationNotification notification = _notiService.GetAllNotis().FirstOrDefault(x => x.NotiId == NotiId);
			if (decision == "Deny")
			{
				_notiService.RemoveNoti(notification);
				MessageBox.Show("Successfully");
				return;
			}

			notification.IsAccepted = true;

			Todo assignedTask = _todoService.GetAllTasks().Where(x => x.NoteId == notification.OriginalTaskId).FirstOrDefault();

			_todoService.AddTask(new Todo() { Description = assignedTask.Description, IsDone = assignedTask.IsDone, IsHost = false, UserId = notification.InviteIdUserId, Time = assignedTask.Time, Title = assignedTask.Title, ModifiedDate = DateTime.Now });
			this.Close();
		}

		private void NotiDetailBackButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
