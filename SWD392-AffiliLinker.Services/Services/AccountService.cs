using AutoMapper;
using Microsoft.AspNetCore.Http;
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
using StatusCodes = SWD392_AffiliLinker.Core.Store.StatusCodes;

namespace SWD392_AffiliLinker.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IHepperUploadImage _hepperUploadImage;

        public AccountService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<User> userManager, IHepperUploadImage hepperUploadImage)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _hepperUploadImage = hepperUploadImage;
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

        public async Task<string> UpdateAvatar(string userId, IFormFile file)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var userDb = await _unitOfWork.GetRepository<User>().FirstOrDefaultAsync(_ => _.Id.ToString() == userId);
                if (userDb is null)
                {
                    throw new BaseException.ErrorException(StatusCodes.NotFound, StatusCodes.NotFound.Name(), "User not exist!!!");
                }
                using var stream = file.OpenReadStream();
                var url = await _hepperUploadImage.UploadImageAsync(stream, file.FileName);

                userDb.Avatar = url;
                await _unitOfWork.SaveAsync();
                _unitOfWork.CommitTransaction();
                return userDb.Avatar;
            }
            catch
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }
}
