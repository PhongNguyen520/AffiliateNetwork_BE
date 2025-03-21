using SWD392_AffiliLinker.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.DTO.ConversionDTO.Response
{
    public class ConversionResponse
    {
        public string Id { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public string? OrderId { get; set; } = string.Empty;
        public string? ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal Commission { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool? IsPayment { get; set; }
        public string AffiliateLinkId { get; set; }
    }
}
