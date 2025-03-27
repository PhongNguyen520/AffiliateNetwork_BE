using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.DTO.CampaginDTO.Request
{
	public class CreateCampaignRequest
	{
		public string CampaignName { get; set; }
		public string? Description { get; set; }
		public string? Introduction { get; set; }
		public string? Policy { get; set; }
		public string? WebsiteLink { get; set; }
		public string? TargetCustomer { get; set; }
		public string? Zone { get; set; }
		public decimal Budget { get; set; }
		public decimal ConversionRate { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string? CategoryId { get; set; }
		public List<string> PayoutModelsId { get; set; } 
		public IFormFile Image {  get; set; }

    }
}
