using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Repositories.IUOW;
using SWD392_AffiliLinker.Services.DTO.ClickDTO.Response;
using SWD392_AffiliLinker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using SWD392_AffiliLinker.Core.Base;
using SWD392_AffiliLinker.Core.Store;
using SWD392_AffiliLinker.Core.Utils;
using SWD392_AffiliLinker.Services.DTO.ConversionDTO.Response;
using SWD392_AffiliLinker.Services.DTO.ClickDTO.Request;
using SWD392_AffiliLinker.Services.DTO.ConversionDTO.Request;

namespace SWD392_AffiliLinker.Services.Services
{
    public class ExportTrafficService : IExportTrafficService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExportTrafficService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<byte[]> ExportClickInfoExcel(FillterExportClickInfo request)
        {
            try
            {
                var listClickInfo = _unitOfWork.GetRepository<ClickInfo>()
                                                     .Entities
                                                     .AsNoTracking()
                                                     .Where(_ => _.AffiliateLinkId == request.AffiliateId);
                if (request.BeginDate.HasValue)
                {
                    listClickInfo = listClickInfo.Where(_ => _.CreatedTime.Date >= request.BeginDate.Value.Date);
                }
                if (request.EndDate.HasValue)
                {
                    listClickInfo = listClickInfo.Where(_ => _.CreatedTime.Date <= request.EndDate.Value.Date);
                }

                var newList = listClickInfo.ToList();

                if (newList.Count == 0)
                {
                    throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), $"No ClickInfo Data of {request.AffiliateId}");
                }

                var listExcel = _mapper.Map<List<ExcelClickInfoResponse>>(listClickInfo);

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add($"Click_Information_Of_{request.AffiliateId}");
                    var properties = typeof(ExcelClickInfoResponse).GetProperties();

                    int row = 1;

                    for (int col = 0; col < properties.Length; col++)
                    {
                        worksheet.Cells[row, col + 1].Value = properties[col].Name;
                    }
                    row++;
                    foreach (var i in listExcel)
                    {
                        for (int col = 0; col < properties.Length; col++)
                        {
                            worksheet.Cells[row, col + 1].Value = properties[col].GetValue(i).ToString();
                        }
                        row++;
                    }

                    worksheet.Cells.AutoFitColumns();
                    return package.GetAsByteArray();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<byte[]> ExportConversionExcel(FillterExportConversion request)
        {
            try
            {
                var listConversionDb = _unitOfWork.GetRepository<Conversion>()
                                                     .Entities
                                                     .AsNoTracking()
                                                     .Where(_ => _.AffiliateLinkId == request.AffiliateId);

                if (request.BeginDate.HasValue)
                {
                    listConversionDb = listConversionDb.Where(_ => _.CreatedTime.Date >= request.BeginDate.Value.Date);
                }
                if (request.EndDate.HasValue)
                {
                    listConversionDb = listConversionDb.Where(_ => _.CreatedTime.Date <= request.EndDate.Value.Date);
                }

                var newList = listConversionDb.ToList();

                if (newList.Count == 0)
                {
                    throw new BaseException.ErrorException(StatusCodes.BadRequest, StatusCodes.BadRequest.Name(), $"No Conversion Data of {request.AffiliateId}");
                }

                var listExcel = _mapper.Map<List<ConversionResponse>>(listConversionDb);

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add($"Conversion_Information_Of_{request.AffiliateId}");
                    var properties = typeof(ConversionResponse).GetProperties();

                    int row = 1;

                    for (int col = 0; col < properties.Length; col++)
                    {
                        worksheet.Cells[row, col + 1].Value = properties[col].Name;
                    }
                    row++;
                    foreach (var i in listExcel)
                    {
                        for (int col = 0; col < properties.Length; col++)
                        {
                            worksheet.Cells[row, col + 1].Value = properties[col].GetValue(i).ToString();
                        }
                        row++;
                    }

                    worksheet.Cells.AutoFitColumns();
                    return package.GetAsByteArray();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
