using SWD392_AffiliLinker.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Entities
{
    public class Conversion : BaseEntity
    {
		public string? OrderId { get; set; } = string.Empty;
		public string? ProductId { get; set; } = string.Empty;
		public int Quantity { get; set; }
		public decimal? Subtotal { get; set; }
		public decimal Commission { get; set; }
		public string Status { get; set; } = string.Empty;

		public bool? IsPayment { get; set; }

		public string AffiliateLinkId { get; set; }
		public virtual AffiliateLink AffiliateLink { get; set; } = null!;
	}
}
