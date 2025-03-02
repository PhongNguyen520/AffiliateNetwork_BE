using SWD392_AffiliLinker.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Entities
{
	public class Transaction : BaseEntity
	{
		public decimal Amount { get; set; }
		public string Transaction_Type { get; set; }
		public string Description { get; set; }
		public string Status { get; set; }
		public Guid UserId { get; set; }
		public virtual User User { get; set; }
	}
}
