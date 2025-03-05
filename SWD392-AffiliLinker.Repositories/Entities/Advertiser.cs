using SWD392_AffiliLinker.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Entities
{
	public class Advertiser : BaseEntity
	{
		public string CampanyName { get; set; }
		public string CompanyAddress { get; set; }
		public string Website { get; set; }
		public DateTime Since { get; set; }
		public string BussinessLicense {  get; set; }
		public Guid UserId {  get; set; }
		public virtual User User { get; set; }
	}
}
