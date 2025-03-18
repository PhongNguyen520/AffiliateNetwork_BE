using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.DTO.AccountDTO.Response
{
    public class PublisherAccountResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; }
        public string PublisherId { get; set; }
        public string? TaxCode { get; set; }
    }
}
