using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
	public interface INotiService
	{
		public void CreateNoti(InvitationNotification notification);

		public void UpdateNoti(InvitationNotification notification);

		public void RemoveNoti(InvitationNotification notification);

		public List<InvitationNotification> GetAllNotis();

		public List<InvitationNotification> GetNotAcceptedNotis(int userId);

		public List<NotiDisplayModel> FormatNotifications(List<InvitationNotification> list);
	}
}
