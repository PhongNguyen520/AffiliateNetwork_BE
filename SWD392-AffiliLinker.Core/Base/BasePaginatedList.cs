using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Core.Base
{
	public class BasePaginatedList<T>
	{
		public List<T> Items { get; set; }

		// Thuộc tính để lưu trữ tổng số phần tử
		public int TotalItems { get; private set; }

		// Thuộc tính để lưu trữ số trang hiện tại
		public int? CurrentPage { get; private set; }

		// Thuộc tính để lưu trữ tổng số trang
		public int TotalPages { get; private set; }

		// Thuộc tính để lưu trữ số phần tử trên mỗi trang
		public int? PageSize { get; private set; }

        // Constructor để khởi tạo danh sách phân trang
        public BasePaginatedList() { }
        public BasePaginatedList(List<T> items, int count, int? pageNumber, int? pageSize)
		{
			TotalItems = count;
			CurrentPage = (pageNumber > 0 ? pageNumber : 1) ?? 1;
			PageSize = (pageSize > 0 ? pageSize : 6) ?? 6;
			TotalPages = PageSize > 0 ? (int)Math.Ceiling(count / (double)PageSize) : 1;
			Items = items;
		}

		// Phương thức để kiểm tra nếu có trang trước đó
		public bool HasPreviousPage => CurrentPage > 1;

		// Phương thức để kiểm tra nếu có trang kế tiếp
		public bool HasNextPage => CurrentPage < TotalPages;

	}
}
