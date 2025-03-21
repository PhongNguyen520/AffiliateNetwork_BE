using Microsoft.AspNetCore.Http;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Services.DTO.AccountDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AccountDTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Interfaces
{
    public interface IAccountService
    {
        Task<PublisherAccountResponse> GetPublisherUserById (string id);
        Task<AdvertiserAccountResponse> GetAdvertiserUserById (string id);
        Task<BasePaginatedList<AccountResponse>> GetAll (FillterAccountRequest request);
        Task<string> UpdateAvatar(string userId, IFormFile file);
    }
}
