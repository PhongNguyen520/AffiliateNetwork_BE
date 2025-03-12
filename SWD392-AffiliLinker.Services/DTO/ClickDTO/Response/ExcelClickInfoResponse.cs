using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.DTO.ClickDTO.Response
{
    public class ExcelClickInfoResponse
    {
        public string Id { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Status { get; set; }
    }
}
