using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
	public class InvitationNotification
	{
		public int NotiId { get; set; }
		public int InviteIdUserId { get; set; }
		public int OriginalTaskId { get; set; }
		public bool IsAccepted { get; set; }

		public virtual Todo Todo { get; set; }

		public virtual User User { get; set; }
	}
}
