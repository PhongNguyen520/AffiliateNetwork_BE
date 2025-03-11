﻿using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AffiliateLinkDTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Interfaces
{
	public interface IAffiliateLinkService
	{
		Task<CreateLinkResponse> CreateLink(CreateLinkRequest request);
		//Task<string> GetByShortCodeAsync(string shortCode);
		Task<BasePaginatedList<GetLinksResponse>> GetPublisherLinkList(int pageIndex);
	}
}
