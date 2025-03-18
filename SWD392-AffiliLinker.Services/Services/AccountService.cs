using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.DTO.AccountDTO.Request;
using SWD392_AffiliLinker.Services.DTO.AccountDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AccountService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<AdvertiserAccountResponse> GetAdvertiserUserById(string id)
        {
            try
            {
                var advertiserDb = await _unitOfWork.GetRepository<Advertiser>()
                                                  .Entities
                                                  .Include(_ => _.User)
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync(_ => _.UserId.ToString() == id);
                if (advertiserDb is null)
                {
                    return null;
                }
                var account = _mapper.Map<AdvertiserAccountResponse>(advertiserDb);
                return account;
            }
            catch
            {
                throw;
            }
        }

        public async Task<PublisherAccountResponse> GetPublisherUserById(string id)
        {
            try
            {
                var publisher = await _unitOfWork.GetRepository<Publisher>()
                                                    .Entities
                                                    .Include(_ => _.User)
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync(_ => _.UserId.ToString() == id);
                if (publisher is null)
                {
                    return null;
                }
                var account = _mapper.Map<PublisherAccountResponse>(publisher);
                return account;
            }
            catch
            {
                throw;
            }
        }

        public async Task<BasePaginatedList<AccountResponse>> GetAll(FillterAccountRequest request)
        {
            try
            {
                var listDb = _unitOfWork.GetRepository<User>().Entities.AsNoTracking();

                if (listDb.Count() == 0)
                {
                    throw new BaseException.ErrorException(StatusCodes.NotFound, StatusCodes.NotFound.Name(), "List Account is empty!!!");
                }
                if (request.Status.HasValue)
                {
                    listDb = listDb.Where(_ => _.Status == request.Status.ToString());
                }

                var paging = await _unitOfWork.GetRepository<User>().GetPagging(listDb, request.PageIndex, request.PageSize);

                var result = _mapper.Map<BasePaginatedList<AccountResponse>>(paging);
                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}
