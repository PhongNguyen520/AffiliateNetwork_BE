using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.DTO.CampaginDTO.Response
{
	public class CampaignDetailResponse
	{
		public string Id { get; set; }
		public string CampaignName { get; set; }
		public string? Description { get; set; }
		public string? Introduction { get; set; }
		public string? Policy { get; set; }
		public string? Image { get; set; }
		public string? WebsiteLink { get; set; }
		public int EnrollCount { get; set; }
		public decimal ConversionRate { get; set; }
		public string? TargetCustomer { get; set; }
		public string? Zone { get; set; }
		public string? CategoryName { get; set; }
		public decimal Budget {  get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public List<string> PayoutModelName { get; set; }
		public string Status { get; set; }
		public bool? IsJoin { get; set; }
	}
}
