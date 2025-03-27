namespace SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Response
{
	public class GetLinksResponse
	{
		public string Id { get; set; }
		public string Status { get; set; }
		public string Url { get; set; }
		public string ShortenUrl { get; set; }
		public string OptimizeUrl { get; set; }
		public string? UtmSource { get; set; }
		public string? UtmMedium { get; set; }
		public string? UtmCampaign { get; set; }
		public string? UtmContent { get; set; }
	}
}
