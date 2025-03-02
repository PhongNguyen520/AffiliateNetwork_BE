using SWD392_AffiliLinker.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Entities
{
	public class Commission : BaseEntity
	{
		public decimal TotalCommission { get; set; }
		public decimal? ApprovalCommission { get; set; }
		public string PublisherId { get; set; }

		public virtual Publisher Publisher { get; set; } = null!;
	}
}
