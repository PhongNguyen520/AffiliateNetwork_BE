using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Core.Store
{
    public class EnumStatus
    {
        public enum UserStatus
        {
            Active,
            Pending,
            Banned,
            Deleted
        }

		public enum LinkStatus
		{
			Active,
			Stop
		}

        public enum MemberStatus
        {
			Active,
			Stop
		}

		public enum CampaignStatus
		{
			Wait,
			Active,
			Reject,
			UnPaid,
			Stop,
			End
		}

		public enum CommonStatus
		{
			Active,
			Stop
		}

		public enum ClickInfoStatus
		{
			Valid,
			Invalid
		}

        public enum ConversionStatus
        {
            Approved,
            Pending,
            Provisionally,
			Rejected
        }

    }
}
