using Microsoft.AspNetCore.Http;
using SWD392_AffiliLinker.Services.DTO.PaymentDTO.Request;
using SWD392_AffiliLinker.Services.DTO.PaymentDTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_AffiliLinker.Services.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);

        VNPaymentResponseModel PaymentExecute(IQueryCollection collections);

        Task SaveTransactionAsync(VNPaymentResponseModel response);


        Task<IEnumerable<TransactionResponseModel>> GetTransactionsAsync(string userId);

    }
}
