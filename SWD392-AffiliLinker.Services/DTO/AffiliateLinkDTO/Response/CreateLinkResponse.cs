using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Response
{
	public class CreateLinkResponse
	{

		public string UrlShorten { get; set; }
		public string? UrlOptimize { get; set; }
		public string CampaignName { get; set; }
		public string Introduction { get; set; }
		public string Description { get; set; }
	}
}
