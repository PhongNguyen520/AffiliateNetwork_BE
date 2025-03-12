using Microsoft.AspNetCore.Mvc;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Services.DTO.CategoryDTO.Request;
using SWD392_AffiliLinker.Services.DTO.CategoryDTO.Response;
using SWD392_AffiliLinker.Services.DTO.PayoutModelDTO.Request;
using SWD392_AffiliLinker.Services.DTO.PayoutModelDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;

namespace SWD392_AffiliLinker.API.Controllers
{
	[Route("api/category")]
	[ApiController]
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;
		private readonly ILogger<PayoutModelController> _logger;

		public CategoryController(ICategoryService categoryService, ILogger<PayoutModelController> logger)
		{
			_categoryService = categoryService;
			_logger = logger;
		}

		[HttpPost("create")]
		public async Task<ActionResult<BaseResponse<string>>> CreateCategory(CreateCategoryRequest request)
		{
			try
			{
				await _categoryService.CreateCategory(request);
				return Ok(BaseResponse<string>.OkResponse("Sucessful"));
			}
			catch (BaseException.ErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, new BaseResponse<string>
				{
					StatusCode = ex.StatusCode,
					Message = ex.ErrorDetail.ErrorMessage.ToString(),
					Code = (int)ex.StatusCode
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error when create category");
				return StatusCode(500, new BaseResponse<string>
				{
					StatusCode = Core.Store.StatusCodes.ServerError,
					Message = "Có lỗi xảy ra, vui lòng thử lại sau.",
					Code = (int)Core.Store.StatusCodes.ServerError
				});
			}
		}

		[HttpGet]
		public async Task<ActionResult<BaseResponse<IEnumerable<GetCategoriesResponse>>>> GetAllCategories()
		{
			try
			{
				var result = await _categoryService.GetCategories();
				return Ok(BaseResponse<IEnumerable<GetCategoriesResponse>>.OkResponse(result, "Sucessful"));
			}
			catch (BaseException.ErrorException ex)
			{
				_logger.LogError(ex, "Error when create payout model");
				return StatusCode((int)ex.StatusCode, new BaseResponse<string>
				{
					StatusCode = ex.StatusCode,
					Message = ex.ErrorDetail.ErrorMessage.ToString(),
					Code = (int)ex.StatusCode
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error when create payout model");
				return StatusCode(500, new BaseResponse<string>
				{
					StatusCode = Core.Store.StatusCodes.ServerError,
					Message = "Có lỗi xảy ra, vui lòng thử lại sau.",
					Code = (int)Core.Store.StatusCodes.ServerError
				});
			}
		}
	}
}
