using SWD392_AffiliLinker.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Entities
{
	public class Notification : BaseEntity
	{
		public string Type { get; set; }
		public string Message { get; set; }
		public string Status { get; set; }

		public Guid UserId { get; set; }
		public virtual User User { get; set; }
	}
}
