using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Interfaces
{
	public interface ICampaignMemberService
	{
		Task JoinCampaign(string campaignId);
	}
}
