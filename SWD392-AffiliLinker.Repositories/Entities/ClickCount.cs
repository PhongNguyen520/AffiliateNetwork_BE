using SWD392_AffiliLinker.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Entities
{
    public class ClickCount : BaseEntity
    {
        public double Count { get; set; }

        public string AffiliateLinkId { get; set; }
        public virtual AffiliateLink AffiliateLink { get; set;}
    }
}
