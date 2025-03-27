using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.DTO.CampaginDTO.Response
{
	public class CampaignFilterResponse
	{
		public string Id { get; set; }
		public string CampaignName { get; set; }
		public string? Image { get; set; }
		public int EnrollCount { get; set; }
		public decimal ConversionRate { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Status { get; set; }
	}
}
