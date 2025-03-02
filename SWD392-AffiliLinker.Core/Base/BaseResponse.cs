using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Core.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SWD392_AffiliLinker.Core.Base
{
	public class BaseResponse<T>
	{
		public T? Data { get; set; }
		public string? Message { get; set; }
		public StatusCodes StatusCode { get; set; }
		public string? Code { get; set; }

		public BaseResponse()
		{

		}
		public BaseResponse(StatusCodes statusCode, string code, T? data, string? message)
		{
			Data = data;
			Message = message;
			StatusCode = statusCode;
			Code = code;
		}

		public BaseResponse(StatusCodes statusCode, string code, T? data)
		{
			Data = data;
			StatusCode = statusCode;
			Code = code;
		}

		public BaseResponse(StatusCodes statusCode, string code, string? message)
		{
			Message = message;
			StatusCode = statusCode;
			Code = code;
		}

		public static BaseResponse<T> OkResponse(T? data, string? mess)
		{
			return new BaseResponse<T>(StatusCodes.OK, StatusCodes.OK.Name(), data, mess);
		}
		public static BaseResponse<T> OkResponse(string? mess)
		{
			return new BaseResponse<T>(StatusCodes.OK, StatusCodes.OK.Name(), mess);
		}
		public static BaseResponse<T> FailResponse(string? message, StatusCodes statusCode = StatusCodes.BadRequest)
		{
			return new BaseResponse<T>(statusCode, statusCode.Name(), message);
		}

		public static BaseResponse<T> FailResponse(string? message, string code, StatusCodes statusCode)
		{
			return new BaseResponse<T>(statusCode, code, message);
		}

		public static implicit operator BaseResponse<T>(BaseResponse<bool> v)
		{
			throw new NotImplementedException();
		}
	}
}
