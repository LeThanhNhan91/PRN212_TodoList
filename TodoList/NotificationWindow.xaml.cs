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
	/// Interaction logic for NotificationWindow.xaml
	/// </summary>
	public partial class NotificationWindow : Window
	{

		public User User { get; set; }
		private NotiService _notiService = new NotiService();
		public NotificationWindow()
		{
			InitializeComponent();
		}

		public void LoadData()
		{
			NotiDataGrid.ItemsSource = null;
			List<InvitationNotification> tmpStorage = _notiService.GetAllNotis().Where(x => x.InviteIdUserId == User.UserId && !x.IsAccepted).ToList();
			NotiDataGrid.ItemsSource = _notiService.FormatNotifications(tmpStorage);
		}

		private void NotiDataGrid_Loaded(object sender, RoutedEventArgs e)
		{
			LoadData();
		}

		private void ViewNotiDetailButton_Click(object sender, RoutedEventArgs e)
		{
			NotificationDetailWindow notificationDetailWindow = new NotificationDetailWindow();
			int id = (NotiDataGrid.SelectedItem as NotiDisplayModel).NotiId;
			notificationDetailWindow.NotiId = id;
			notificationDetailWindow.Show();
		}

		private void NotiWindowBackButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void NotiDataGrid_LayoutUpdated(object sender, EventArgs e)
		{

        }
    }
}
