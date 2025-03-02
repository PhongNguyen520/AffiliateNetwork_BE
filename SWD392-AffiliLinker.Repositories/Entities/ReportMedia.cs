using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Repositories.Entities
{
    public class ReportMedia
    {
        public string FileName { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string Status { get; set; }
        public string ReportId { get; set; }
        public virtual Report Report { get; set; }
    }
}
