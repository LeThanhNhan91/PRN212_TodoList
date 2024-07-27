using Repositories;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class NotiService : INotiService
	{
		private NotiRepository _repo = new NotiRepository();

		public void CreateNoti(InvitationNotification notification) => _repo.Add(notification);

		public void UpdateNoti(InvitationNotification notification) => _repo.Update(notification);

		public void RemoveNoti(InvitationNotification notification) => _repo.Delete(notification);

		public List<InvitationNotification> GetAllNotis() => _repo.GetAll();

		public List<InvitationNotification> GetNotAcceptedNotis(int userId) => GetAllNotis().Where(x => x.IsAccepted == false && x.InviteIdUserId == userId).ToList();

		public List<NotiDisplayModel> FormatNotifications(List<InvitationNotification> list)
		{
			List<NotiDisplayModel> result = new List<NotiDisplayModel>();
			TodoService todoService = new TodoService();
			UserService userService = new UserService();
			foreach (InvitationNotification notification in list)
			{
				Todo assignedTask = todoService.GetAllTasks().FirstOrDefault(x => x.NoteId == notification.OriginalTaskId);
				User user = userService.GetAllUsers().FirstOrDefault(x => x.UserId == assignedTask.UserId);
				result.Add(new NotiDisplayModel { Title = assignedTask.Title, NotiId = notification.NotiId, FromEmail = user.Email });
			}
			return result;
		}

	}
}
