using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPLeadLineModel : ERPBaseModel, IERPLeadLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllLeadLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPLeadLineRepository iERPLeadLineRepository = (base.ERPLeadLineRepository = new ERPLeadLineRepository(base.ApiClientContext));
		using (iERPLeadLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPLeadLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPLeadLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPLeadLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPLeadLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetLeadLine(Guid leadLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLeadLineRepository iERPLeadLineRepository = (base.ERPLeadLineRepository = new ERPLeadLineRepository(base.ApiClientContext));
		using (iERPLeadLineRepository)
		{
			if (!(await base.ERPLeadLineRepository.DoesLeadLineExist(leadLineId)))
			{
				errorsList.Add($"LeadLine [{leadLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutLeadLine(ERPLeadLineDto leadLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLeadLineRepository iERPLeadLineRepository = (base.ERPLeadLineRepository = new ERPLeadLineRepository(base.ApiClientContext));
		using (iERPLeadLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(leadLine.lolLeadID) && !(await base.ERPLeadLineRepository.DoesRecordExistInTableUsingKeys("Leads", new object[1] { "LOPLEADID" }, new object[1] { leadLine.lolLeadID })))
			{
				errorsList.Add("lolLeadID [" + leadLine.lolLeadID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(leadLine.lolPartID) && !(await base.ERPLeadLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { leadLine.lolPartID })))
			{
				errorsList.Add("lolPartID [" + leadLine.lolPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(leadLine.lolPartRevisionID) && !(await base.ERPLeadLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { leadLine.lolPartID, leadLine.lolPartRevisionID })))
			{
				errorsList.Add("lolPartRevisionID [" + leadLine.lolPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(leadLine.lolPartGroupID) && !(await base.ERPLeadLineRepository.DoesRecordExistInTableUsingKeys("PartGroups", new object[1] { "IMUPARTGROUPID" }, new object[1] { leadLine.lolPartGroupID })))
			{
				errorsList.Add("lolPartGroupID [" + leadLine.lolPartGroupID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(leadLine.lolResolutionReasonID) && !(await base.ERPLeadLineRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { leadLine.lolResolutionReasonID })))
			{
				errorsList.Add("lolResolutionReasonID [" + leadLine.lolResolutionReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(leadLine.lolCurrencyRateID) && !(await base.ERPLeadLineRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { leadLine.lolCurrencyRateID })))
			{
				errorsList.Add("lolCurrencyRateID [" + leadLine.lolCurrencyRateID + "] not found.");
			}
			if (leadLine.lolPartPriceID > 0 && !(await base.ERPLeadLineRepository.DoesRecordExistInTableUsingKeys("PartPrices", new object[1] { "IMIPARTPRICEID" }, new object[1] { leadLine.lolPartPriceID })))
			{
				errorsList.Add($"lolPartPriceID [{leadLine.lolPartPriceID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPLeadLineDto>>> Process_GetAllLeadLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPLeadLineDto> allLeadLinesDto = new List<ERPLeadLineDto>();
		ERPResponseMessageDto<IList<ERPLeadLineDto>> result;
		try
		{
			IERPLeadLineRepository iERPLeadLineRepository = (base.ERPLeadLineRepository = new ERPLeadLineRepository(base.ApiClientContext));
			using (iERPLeadLineRepository)
			{
				foreach (ERPLeadLineInformationDto item2 in await base.ERPLeadLineRepository.GetAllLeadLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPLeadLineDto item = new ERPLeadLineDto
					{
						lolCreatedBy = item2.lolCreatedBy,
						lolCreatedDate = item2.lolCreatedDate,
						lolCurrencyRateID = item2.lolCurrencyRateID,
						lolDescription = item2.lolDescription,
						lolDiscountAmount = item2.lolDiscountAmount,
						lolDiscountAmountForeign = item2.lolDiscountAmountForeign,
						lolDiscountPercent = item2.lolDiscountPercent,
						lolUniqueID = item2.lolUniqueID,
						lolExchangeRate = item2.lolExchangeRate,
						lolForecastDate = item2.lolForecastDate,
						lolGrossAmount = item2.lolGrossAmount,
						lolGrossAmountForeign = item2.lolGrossAmountForeign,
						lolCreatedFromMobile = item2.lolCreatedFromMobile,
						lolCustomRate = item2.lolCustomRate,
						lolTransferredToQuote = item2.lolTransferredToQuote,
						lolLeadDate = item2.lolLeadDate,
						lolLeadID = item2.lolLeadID,
						lolOrgPartID = item2.lolOrgPartID,
						lolOrgPartShortDescription = item2.lolOrgPartShortDescription,
						lolPartGroupID = item2.lolPartGroupID,
						lolPartID = item2.lolPartID,
						lolPartPriceID = item2.lolPartPriceID,
						lolPartRevisionID = item2.lolPartRevisionID,
						lolQuantity = item2.lolQuantity,
						lolResolutionReasonID = item2.lolResolutionReasonID,
						lolRevenueForecast = item2.lolRevenueForecast,
						lolRevenueForecastForeign = item2.lolRevenueForecastForeign,
						lolRowVersion = item2.lolRowVersion,
						lolLeadLineID = item2.lolLeadLineID,
						lolUnitOfMeasure = item2.lolUnitOfMeasure,
						lolUnitSalePriceBase = item2.lolUnitSalePriceBase,
						lolUnitSalePriceForeign = item2.lolUnitSalePriceForeign,
						CustomFields = item2.CustomFields
					};
					allLeadLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all LeadLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPLeadLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allLeadLinesDto,
				RecordCount = allLeadLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLeadLineDto>> Process_GetLeadLine(Guid leadLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPLeadLineDto leadLineDto = null;
		ERPResponseMessageDto<ERPLeadLineDto> result;
		try
		{
			IERPLeadLineRepository iERPLeadLineRepository = (base.ERPLeadLineRepository = new ERPLeadLineRepository(base.ApiClientContext));
			using (iERPLeadLineRepository)
			{
				ERPLeadLineInformationDto eRPLeadLineInformationDto = await base.ERPLeadLineRepository.GetLeadLine(leadLineId);
				leadLineDto = new ERPLeadLineDto
				{
					lolCreatedBy = eRPLeadLineInformationDto.lolCreatedBy,
					lolCreatedDate = eRPLeadLineInformationDto.lolCreatedDate,
					lolCurrencyRateID = eRPLeadLineInformationDto.lolCurrencyRateID,
					lolDescription = eRPLeadLineInformationDto.lolDescription,
					lolDiscountAmount = eRPLeadLineInformationDto.lolDiscountAmount,
					lolDiscountAmountForeign = eRPLeadLineInformationDto.lolDiscountAmountForeign,
					lolDiscountPercent = eRPLeadLineInformationDto.lolDiscountPercent,
					lolUniqueID = eRPLeadLineInformationDto.lolUniqueID,
					lolExchangeRate = eRPLeadLineInformationDto.lolExchangeRate,
					lolForecastDate = eRPLeadLineInformationDto.lolForecastDate,
					lolGrossAmount = eRPLeadLineInformationDto.lolGrossAmount,
					lolGrossAmountForeign = eRPLeadLineInformationDto.lolGrossAmountForeign,
					lolCreatedFromMobile = eRPLeadLineInformationDto.lolCreatedFromMobile,
					lolCustomRate = eRPLeadLineInformationDto.lolCustomRate,
					lolTransferredToQuote = eRPLeadLineInformationDto.lolTransferredToQuote,
					lolLeadDate = eRPLeadLineInformationDto.lolLeadDate,
					lolLeadID = eRPLeadLineInformationDto.lolLeadID,
					lolOrgPartID = eRPLeadLineInformationDto.lolOrgPartID,
					lolOrgPartShortDescription = eRPLeadLineInformationDto.lolOrgPartShortDescription,
					lolPartGroupID = eRPLeadLineInformationDto.lolPartGroupID,
					lolPartID = eRPLeadLineInformationDto.lolPartID,
					lolPartPriceID = eRPLeadLineInformationDto.lolPartPriceID,
					lolPartRevisionID = eRPLeadLineInformationDto.lolPartRevisionID,
					lolQuantity = eRPLeadLineInformationDto.lolQuantity,
					lolResolutionReasonID = eRPLeadLineInformationDto.lolResolutionReasonID,
					lolRevenueForecast = eRPLeadLineInformationDto.lolRevenueForecast,
					lolRevenueForecastForeign = eRPLeadLineInformationDto.lolRevenueForecastForeign,
					lolRowVersion = eRPLeadLineInformationDto.lolRowVersion,
					lolLeadLineID = eRPLeadLineInformationDto.lolLeadLineID,
					lolUnitOfMeasure = eRPLeadLineInformationDto.lolUnitOfMeasure,
					lolUnitSalePriceBase = eRPLeadLineInformationDto.lolUnitSalePriceBase,
					lolUnitSalePriceForeign = eRPLeadLineInformationDto.lolUnitSalePriceForeign,
					CustomFields = eRPLeadLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the LeadLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLeadLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = leadLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLeadLineDto>> Process_PutLeadLine(ERPLeadLineDto leadLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPLeadLineDto createdObject = null;
		ERPResponseMessageDto<ERPLeadLineDto> result;
		try
		{
			IERPLeadLineRepository iERPLeadLineRepository = (base.ERPLeadLineRepository = new ERPLeadLineRepository(base.ApiClientContext));
			using (iERPLeadLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPLeadLineRepository.SaveLeadLine(leadLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPLeadLineInformationDto eRPLeadLineInformationDto = await base.ERPLeadLineRepository.GetLeadLine(leadLine.lolUniqueID);
					createdObject = new ERPLeadLineDto
					{
						lolCreatedBy = eRPLeadLineInformationDto.lolCreatedBy,
						lolCreatedDate = eRPLeadLineInformationDto.lolCreatedDate,
						lolCurrencyRateID = eRPLeadLineInformationDto.lolCurrencyRateID,
						lolDescription = eRPLeadLineInformationDto.lolDescription,
						lolDiscountAmount = eRPLeadLineInformationDto.lolDiscountAmount,
						lolDiscountAmountForeign = eRPLeadLineInformationDto.lolDiscountAmountForeign,
						lolDiscountPercent = eRPLeadLineInformationDto.lolDiscountPercent,
						lolUniqueID = eRPLeadLineInformationDto.lolUniqueID,
						lolExchangeRate = eRPLeadLineInformationDto.lolExchangeRate,
						lolForecastDate = eRPLeadLineInformationDto.lolForecastDate,
						lolGrossAmount = eRPLeadLineInformationDto.lolGrossAmount,
						lolGrossAmountForeign = eRPLeadLineInformationDto.lolGrossAmountForeign,
						lolCreatedFromMobile = eRPLeadLineInformationDto.lolCreatedFromMobile,
						lolCustomRate = eRPLeadLineInformationDto.lolCustomRate,
						lolTransferredToQuote = eRPLeadLineInformationDto.lolTransferredToQuote,
						lolLeadDate = eRPLeadLineInformationDto.lolLeadDate,
						lolLeadID = eRPLeadLineInformationDto.lolLeadID,
						lolOrgPartID = eRPLeadLineInformationDto.lolOrgPartID,
						lolOrgPartShortDescription = eRPLeadLineInformationDto.lolOrgPartShortDescription,
						lolPartGroupID = eRPLeadLineInformationDto.lolPartGroupID,
						lolPartID = eRPLeadLineInformationDto.lolPartID,
						lolPartPriceID = eRPLeadLineInformationDto.lolPartPriceID,
						lolPartRevisionID = eRPLeadLineInformationDto.lolPartRevisionID,
						lolQuantity = eRPLeadLineInformationDto.lolQuantity,
						lolResolutionReasonID = eRPLeadLineInformationDto.lolResolutionReasonID,
						lolRevenueForecast = eRPLeadLineInformationDto.lolRevenueForecast,
						lolRevenueForecastForeign = eRPLeadLineInformationDto.lolRevenueForecastForeign,
						lolRowVersion = eRPLeadLineInformationDto.lolRowVersion,
						lolLeadLineID = eRPLeadLineInformationDto.lolLeadLineID,
						lolUnitOfMeasure = eRPLeadLineInformationDto.lolUnitOfMeasure,
						lolUnitSalePriceBase = eRPLeadLineInformationDto.lolUnitSalePriceBase,
						lolUnitSalePriceForeign = eRPLeadLineInformationDto.lolUnitSalePriceForeign,
						CustomFields = eRPLeadLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing LeadLine [{leadLine.lolUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLeadLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteLeadLine(Guid leadLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLeadLineRepository iERPLeadLineRepository = (base.ERPLeadLineRepository = new ERPLeadLineRepository(base.ApiClientContext));
		using (iERPLeadLineRepository)
		{
			if (!(await base.ERPLeadLineRepository.DoesLeadLineExist(leadLineId)))
			{
				base.ErrorsList.Add($"LeadLine [{leadLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPLeadLineInformationDto eRPLeadLineInformationDto = await base.ERPLeadLineRepository.GetLeadLine(leadLineId);
				string text = await base.ERPLeadLineRepository.WhereUsed("LeadLines", new object[2] { eRPLeadLineInformationDto.lolLeadID, eRPLeadLineInformationDto.lolLeadLineID }, new object[2] { "lolLeadID", "lolLeadLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("LeadLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPLeadLineDto>> Process_DeleteLeadLine(Guid leadLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPLeadLineDto> result;
		try
		{
			IERPLeadLineRepository iERPLeadLineRepository = (base.ERPLeadLineRepository = new ERPLeadLineRepository(base.ApiClientContext));
			using (iERPLeadLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPLeadLineRepository.DeleteRowFromTable("LeadLines", "lol", leadLineId);
				((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
				IList<string> errorsList = base.ErrorsList;
				if (errorsList != null && errorsList.Count > 0)
				{
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of LeadLine [{leadLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLeadLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPLeadLineDto()
			};
		}
		return result;
	}
}
