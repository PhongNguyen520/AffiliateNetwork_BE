using SWD392_AffiliLinker.Services.DTO.CategoryDTO.Request;
using SWD392_AffiliLinker.Services.DTO.CategoryDTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Interfaces
{
	public interface ICategoryService
	{
		Task<IEnumerable<GetCategoriesResponse>> GetCategories();
		Task CreateCategory(CreateCategoryRequest request);
	}
}
