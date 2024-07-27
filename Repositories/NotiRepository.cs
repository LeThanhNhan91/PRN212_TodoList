using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
	public class NotiRepository
	{
		private TodoDbContext _db;

		public void Add(InvitationNotification notification)
		{
			_db = new TodoDbContext();
			_db.InvitationNotifications.Add(notification);
			_db.SaveChanges();
		}

		public void Update(InvitationNotification notification)
		{
			_db = new TodoDbContext();
			_db.InvitationNotifications.Update(notification);
			_db.SaveChanges();
		}

		public void Delete(InvitationNotification notification)
		{
			_db = new TodoDbContext();
			_db.InvitationNotifications.Remove(notification);
			_db.SaveChanges();
		}

		public List<InvitationNotification> GetAll()
		{
			_db = new TodoDbContext();
			return _db.InvitationNotifications.ToList();
		}

	}
}
