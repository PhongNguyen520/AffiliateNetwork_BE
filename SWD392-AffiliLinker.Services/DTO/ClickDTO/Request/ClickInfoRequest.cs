using SWD392_AffiliLinker.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.DTO.ClickDTO.Request
{
    public class ClickInfoRequest
    {
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Status { get; set; }
        public string AffiliateLinkId { get; set; }
    }
}

