using SWD392_AffiliLinker.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Entities
{
    public class Bank : BaseEntity
    {
        public string AccountName { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string BankBranch { get; set; }
        public string Status { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
