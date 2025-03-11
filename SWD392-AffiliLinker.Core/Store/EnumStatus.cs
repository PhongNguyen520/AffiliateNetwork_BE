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
	}
}
