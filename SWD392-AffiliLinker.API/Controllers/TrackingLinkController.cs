using Microsoft.AspNetCore.Mvc;
using SWD392_AffiliLinker.Services.Interfaces;

namespace SWD392_AffiliLinker.API.Controllers
{
	public class TrackingLinkController : Controller
	{
		private readonly IAffiliateLinkService _affiliateLinkService;

		public TrackingLinkController(IAffiliateLinkService affiliateLinkService)
		{
			_affiliateLinkService = affiliateLinkService;
		}

		[HttpGet("o/{slug}")]
		public async Task<IActionResult> RedirectOptimizeUrl(string? slug)
		{
			var url = await _affiliateLinkService.RedirectOptimizeUrl(slug);
			return Redirect(url);
		}

		[HttpGet("s/{shortenCode}")]
		public async Task<IActionResult> RedirectShortenUrl(string? shortenCode)
		{
			var url = await _affiliateLinkService.RedirectShortenUrl(shortenCode);
			return Redirect(url);
		}
	}
}
