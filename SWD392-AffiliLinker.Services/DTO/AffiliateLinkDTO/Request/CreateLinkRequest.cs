using static SWD392_AffiliLinker.Core.Store.EnumStatus;

namespace SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Request
{
	public class CreateLinkRequest
	{
		public string? Url { get; set; }
		public string? UtmSource { get; set; }
		public string? UtmMedium { get; set; }
		public string? UtmCampaign { get; set; }
		public string? UtmContent { get; set; }
		public string? OptimizeUrl { get; set; }
		public LinkStatus Status { get; set; }
		public string CampaignId { get; set; }
	}
}
