using SWD392_AffiliLinker.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Entities
{
    public class Publisher : BaseEntity
    {
        public string? TaxCode { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Commission> Commissions { get; set; } = new List<Commission>();
    }
}
