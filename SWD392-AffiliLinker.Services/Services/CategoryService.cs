﻿using AutoMapper;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.DTO.CategoryDTO.Request;
using SWD392_AffiliLinker.Services.DTO.CategoryDTO.Response;
using SWD392_AffiliLinker.Services.DTO.PayoutModelDTO.Request;
using SWD392_AffiliLinker.Services.DTO.PayoutModelDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWD392_AffiliLinker.Core.Store.EnumStatus;

namespace SWD392_AffiliLinker.Services.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<GetCategoriesResponse>> GetCategories()
		{
			var result = _unitOfWork.GetRepository<CampaignCategory>().Entities.Where(s => s.Status == CommonStatus.Active.ToString());
			return _mapper.Map<IEnumerable<GetCategoriesResponse>>(result);
		}

		public async Task CreateCategory(CreateCategoryRequest request)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				var model = _unitOfWork.GetRepository<CampaignCategory>().Entities.FirstOrDefault(s => s.Name == request.Name);
				if (model != null)
				{
					throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), "This category is exist");
				}

				var result = _mapper.Map<CampaignCategory>(request);
				result.CreatedTime = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7));
				result.LastUpdatedTime = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7));
				result.Status = CommonStatus.Active.ToString();
				await _unitOfWork.GetRepository<CampaignCategory>().InsertAsync(result);
				await _unitOfWork.SaveAsync();
				_unitOfWork.CommitTransaction();
			}
			catch (Exception ex)
			{
				_unitOfWork.RollBack();
				throw;
			}
		}
	}
}
