using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;

namespace SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Request
{
	public class FilterLinkRequest
	{
		public string id { get; set; }
		public int? PageIndex { get; set; }
		public int? PageSize { get; set; }
	}
}
