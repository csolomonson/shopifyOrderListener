using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSalesOrderLineModel : ERPBaseModel, IERPSalesOrderLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSalesOrderLineRepository iERPSalesOrderLineRepository = (base.ERPSalesOrderLineRepository = new ERPSalesOrderLineRepository(base.ApiClientContext));
		using (iERPSalesOrderLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSalesOrderLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSalesOrderLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSalesOrderLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSalesOrderLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderLine(Guid salesOrderLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderLineRepository iERPSalesOrderLineRepository = (base.ERPSalesOrderLineRepository = new ERPSalesOrderLineRepository(base.ApiClientContext));
		using (iERPSalesOrderLineRepository)
		{
			if (!(await base.ERPSalesOrderLineRepository.DoesSalesOrderLineExist(salesOrderLineId)))
			{
				errorsList.Add($"SalesOrderLine [{salesOrderLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderLine(ERPSalesOrderLineDto salesOrderLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderLineRepository iERPSalesOrderLineRepository = (base.ERPSalesOrderLineRepository = new ERPSalesOrderLineRepository(base.ApiClientContext));
		using (iERPSalesOrderLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(salesOrderLine.omlSalesOrderID) && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { salesOrderLine.omlSalesOrderID })))
			{
				errorsList.Add("omlSalesOrderID [" + salesOrderLine.omlSalesOrderID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderLine.omlPartID) && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { salesOrderLine.omlPartID })))
			{
				errorsList.Add("omlPartID [" + salesOrderLine.omlPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderLine.omlPartRevisionID) && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { salesOrderLine.omlPartID, salesOrderLine.omlPartRevisionID })))
			{
				errorsList.Add("omlPartRevisionID [" + salesOrderLine.omlPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderLine.omlPartGroupID) && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("PartGroups", new object[1] { "IMUPARTGROUPID" }, new object[1] { salesOrderLine.omlPartGroupID })))
			{
				errorsList.Add("omlPartGroupID [" + salesOrderLine.omlPartGroupID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderLine.omlTaxCodeID) && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { salesOrderLine.omlTaxCodeID })))
			{
				errorsList.Add("omlTaxCodeID [" + salesOrderLine.omlTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderLine.omlNonTaxReasonID) && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { salesOrderLine.omlNonTaxReasonID })))
			{
				errorsList.Add("omlNonTaxReasonID [" + salesOrderLine.omlNonTaxReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderLine.omlSecondTaxCodeID) && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { salesOrderLine.omlSecondTaxCodeID })))
			{
				errorsList.Add("omlSecondTaxCodeID [" + salesOrderLine.omlSecondTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderLine.omlQuoteID) && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("Quotes", new object[1] { "QMPQUOTEID" }, new object[1] { salesOrderLine.omlQuoteID })))
			{
				errorsList.Add("omlQuoteID [" + salesOrderLine.omlQuoteID + "] not found.");
			}
			if (salesOrderLine.omlQuoteLineID > 0 && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("QuoteLines", new object[2] { "QMLQUOTEID", "QMLQUOTELINEID" }, new object[2] { salesOrderLine.omlQuoteID, salesOrderLine.omlQuoteLineID })))
			{
				errorsList.Add($"omlQuoteLineID [{salesOrderLine.omlQuoteLineID}] not found.");
			}
			if (salesOrderLine.omlQuoteQuantityID > 0 && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("QuoteQuantities", new object[3] { "QMQQUOTEID", "QMQQUOTELINEID", "QMQQUOTEQUANTITYID" }, new object[3] { salesOrderLine.omlQuoteID, salesOrderLine.omlQuoteLineID, salesOrderLine.omlQuoteQuantityID })))
			{
				errorsList.Add($"omlQuoteQuantityID [{salesOrderLine.omlQuoteQuantityID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderLine.omlLeadID) && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("Leads", new object[1] { "LOPLEADID" }, new object[1] { salesOrderLine.omlLeadID })))
			{
				errorsList.Add("omlLeadID [" + salesOrderLine.omlLeadID + "] not found.");
			}
			if (salesOrderLine.omlLeadLineID > 0 && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("LeadLines", new object[2] { "LOLLEADID", "LOLLEADLINEID" }, new object[2] { salesOrderLine.omlLeadID, salesOrderLine.omlLeadLineID })))
			{
				errorsList.Add($"omlLeadLineID [{salesOrderLine.omlLeadLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderLine.omlRmaClaimID) && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("RMAClaims", new object[1] { "RAPRMACLAIMID" }, new object[1] { salesOrderLine.omlRmaClaimID })))
			{
				errorsList.Add("omlRmaClaimID [" + salesOrderLine.omlRmaClaimID + "] not found.");
			}
			if (salesOrderLine.omlRmaClaimLineID > 0 && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("RMAClaimLines", new object[2] { "RALRMACLAIMID", "RALRMACLAIMLINEID" }, new object[2] { salesOrderLine.omlRmaClaimID, salesOrderLine.omlRmaClaimLineID })))
			{
				errorsList.Add($"omlRmaClaimLineID [{salesOrderLine.omlRmaClaimLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderLine.omlProjectID) && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { salesOrderLine.omlProjectID })))
			{
				errorsList.Add("omlProjectID [" + salesOrderLine.omlProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderLine.omlProjectAreaID) && !(await base.ERPSalesOrderLineRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { salesOrderLine.omlProjectID, salesOrderLine.omlProjectAreaID })))
			{
				errorsList.Add("omlProjectAreaID [" + salesOrderLine.omlProjectAreaID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSalesOrderLineDto>>> Process_GetAllSalesOrderLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSalesOrderLineDto> allSalesOrderLinesDto = new List<ERPSalesOrderLineDto>();
		ERPResponseMessageDto<IList<ERPSalesOrderLineDto>> result;
		try
		{
			IERPSalesOrderLineRepository iERPSalesOrderLineRepository = (base.ERPSalesOrderLineRepository = new ERPSalesOrderLineRepository(base.ApiClientContext));
			using (iERPSalesOrderLineRepository)
			{
				foreach (ERPSalesOrderLineInformationDto item2 in await base.ERPSalesOrderLineRepository.GetAllSalesOrderLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPSalesOrderLineDto item = new ERPSalesOrderLineDto
					{
						omlCreatedBy = item2.omlCreatedBy,
						omlCreatedDate = item2.omlCreatedDate,
						omlDeliveryQuantityTotal = item2.omlDeliveryQuantityTotal,
						omlDepositAmountBase = item2.omlDepositAmountBase,
						omlDepositAmountForeign = item2.omlDepositAmountForeign,
						omlDepositPercent = item2.omlDepositPercent,
						omlDiscountPercent = item2.omlDiscountPercent,
						omlDocuments = item2.omlDocuments,
						omlUniqueID = item2.omlUniqueID,
						omlExtendedDiscountBase = item2.omlExtendedDiscountBase,
						omlExtendedDiscountForeign = item2.omlExtendedDiscountForeign,
						omlExtendedPriceBase = item2.omlExtendedPriceBase,
						omlExtendedPriceForeign = item2.omlExtendedPriceForeign,
						omlExtendedWeight = item2.omlExtendedWeight,
						omlFreightAmountBase = item2.omlFreightAmountBase,
						omlFreightAmountForeign = item2.omlFreightAmountForeign,
						omlFullExtendedPriceBase = item2.omlFullExtendedPriceBase,
						omlFullExtendedPriceForeign = item2.omlFullExtendedPriceForeign,
						omlFullUnitPriceBase = item2.omlFullUnitPriceBase,
						omlFullUnitPriceForeign = item2.omlFullUnitPriceForeign,
						omlAvalaraIgnoreLine = item2.omlAvalaraIgnoreLine,
						omlClosed = item2.omlClosed,
						omlConfigured = item2.omlConfigured,
						omlDeposit = item2.omlDeposit,
						omlDepositCreated = item2.omlDepositCreated,
						omlDepositCredited = item2.omlDepositCredited,
						omlPayCommission = item2.omlPayCommission,
						omlPriceOverride = item2.omlPriceOverride,
						omlTimeAndMaterial = item2.omlTimeAndMaterial,
						omlLeadID = item2.omlLeadID,
						omlLeadLineID = item2.omlLeadLineID,
						omlNonTaxReasonID = item2.omlNonTaxReasonID,
						omlOrderQuantity = item2.omlOrderQuantity,
						omlOrgPartID = item2.omlOrgPartID,
						omlOrgPartShortDescription = item2.omlOrgPartShortDescription,
						omlPartGroupID = item2.omlPartGroupID,
						omlPartID = item2.omlPartID,
						omlPartLongDescriptionRtf = item2.omlPartLongDescriptionRtf,
						omlPartLongDescriptionText = item2.omlPartLongDescriptionText,
						omlPartRevisionID = item2.omlPartRevisionID,
						omlPartShortDescription = item2.omlPartShortDescription,
						omlProjectAreaID = item2.omlProjectAreaID,
						omlProjectID = item2.omlProjectID,
						omlQuantityShipped = item2.omlQuantityShipped,
						omlQuoteID = item2.omlQuoteID,
						omlQuoteLineID = item2.omlQuoteLineID,
						omlQuoteQuantityID = item2.omlQuoteQuantityID,
						omlReleaseNumber = item2.omlReleaseNumber,
						omlRmaClaimID = item2.omlRmaClaimID,
						omlRmaClaimLineID = item2.omlRmaClaimLineID,
						omlRowVersion = item2.omlRowVersion,
						omlSalesOrderID = item2.omlSalesOrderID,
						omlSecondTaxAmountBase = item2.omlSecondTaxAmountBase,
						omlSecondTaxAmountForeign = item2.omlSecondTaxAmountForeign,
						omlSecondTaxCodeID = item2.omlSecondTaxCodeID,
						omlSalesOrderLineID = item2.omlSalesOrderLineID,
						omlTaxAmountBase = item2.omlTaxAmountBase,
						omlTaxAmountForeign = item2.omlTaxAmountForeign,
						omlTaxCodeID = item2.omlTaxCodeID,
						omlUnitDiscountBase = item2.omlUnitDiscountBase,
						omlUnitDiscountForeign = item2.omlUnitDiscountForeign,
						omlUnitOfMeasure = item2.omlUnitOfMeasure,
						omlUnitPriceBase = item2.omlUnitPriceBase,
						omlUnitPriceForeign = item2.omlUnitPriceForeign,
						omlWeight = item2.omlWeight,
						CustomFields = item2.CustomFields
					};
					allSalesOrderLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SalesOrderLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSalesOrderLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSalesOrderLinesDto,
				RecordCount = allSalesOrderLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderLineDto>> Process_GetSalesOrderLine(Guid salesOrderLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSalesOrderLineDto salesOrderLineDto = null;
		ERPResponseMessageDto<ERPSalesOrderLineDto> result;
		try
		{
			IERPSalesOrderLineRepository iERPSalesOrderLineRepository = (base.ERPSalesOrderLineRepository = new ERPSalesOrderLineRepository(base.ApiClientContext));
			using (iERPSalesOrderLineRepository)
			{
				ERPSalesOrderLineInformationDto eRPSalesOrderLineInformationDto = await base.ERPSalesOrderLineRepository.GetSalesOrderLine(salesOrderLineId);
				salesOrderLineDto = new ERPSalesOrderLineDto
				{
					omlCreatedBy = eRPSalesOrderLineInformationDto.omlCreatedBy,
					omlCreatedDate = eRPSalesOrderLineInformationDto.omlCreatedDate,
					omlDeliveryQuantityTotal = eRPSalesOrderLineInformationDto.omlDeliveryQuantityTotal,
					omlDepositAmountBase = eRPSalesOrderLineInformationDto.omlDepositAmountBase,
					omlDepositAmountForeign = eRPSalesOrderLineInformationDto.omlDepositAmountForeign,
					omlDepositPercent = eRPSalesOrderLineInformationDto.omlDepositPercent,
					omlDiscountPercent = eRPSalesOrderLineInformationDto.omlDiscountPercent,
					omlDocuments = eRPSalesOrderLineInformationDto.omlDocuments,
					omlUniqueID = eRPSalesOrderLineInformationDto.omlUniqueID,
					omlExtendedDiscountBase = eRPSalesOrderLineInformationDto.omlExtendedDiscountBase,
					omlExtendedDiscountForeign = eRPSalesOrderLineInformationDto.omlExtendedDiscountForeign,
					omlExtendedPriceBase = eRPSalesOrderLineInformationDto.omlExtendedPriceBase,
					omlExtendedPriceForeign = eRPSalesOrderLineInformationDto.omlExtendedPriceForeign,
					omlExtendedWeight = eRPSalesOrderLineInformationDto.omlExtendedWeight,
					omlFreightAmountBase = eRPSalesOrderLineInformationDto.omlFreightAmountBase,
					omlFreightAmountForeign = eRPSalesOrderLineInformationDto.omlFreightAmountForeign,
					omlFullExtendedPriceBase = eRPSalesOrderLineInformationDto.omlFullExtendedPriceBase,
					omlFullExtendedPriceForeign = eRPSalesOrderLineInformationDto.omlFullExtendedPriceForeign,
					omlFullUnitPriceBase = eRPSalesOrderLineInformationDto.omlFullUnitPriceBase,
					omlFullUnitPriceForeign = eRPSalesOrderLineInformationDto.omlFullUnitPriceForeign,
					omlAvalaraIgnoreLine = eRPSalesOrderLineInformationDto.omlAvalaraIgnoreLine,
					omlClosed = eRPSalesOrderLineInformationDto.omlClosed,
					omlConfigured = eRPSalesOrderLineInformationDto.omlConfigured,
					omlDeposit = eRPSalesOrderLineInformationDto.omlDeposit,
					omlDepositCreated = eRPSalesOrderLineInformationDto.omlDepositCreated,
					omlDepositCredited = eRPSalesOrderLineInformationDto.omlDepositCredited,
					omlPayCommission = eRPSalesOrderLineInformationDto.omlPayCommission,
					omlPriceOverride = eRPSalesOrderLineInformationDto.omlPriceOverride,
					omlTimeAndMaterial = eRPSalesOrderLineInformationDto.omlTimeAndMaterial,
					omlLeadID = eRPSalesOrderLineInformationDto.omlLeadID,
					omlLeadLineID = eRPSalesOrderLineInformationDto.omlLeadLineID,
					omlNonTaxReasonID = eRPSalesOrderLineInformationDto.omlNonTaxReasonID,
					omlOrderQuantity = eRPSalesOrderLineInformationDto.omlOrderQuantity,
					omlOrgPartID = eRPSalesOrderLineInformationDto.omlOrgPartID,
					omlOrgPartShortDescription = eRPSalesOrderLineInformationDto.omlOrgPartShortDescription,
					omlPartGroupID = eRPSalesOrderLineInformationDto.omlPartGroupID,
					omlPartID = eRPSalesOrderLineInformationDto.omlPartID,
					omlPartLongDescriptionRtf = eRPSalesOrderLineInformationDto.omlPartLongDescriptionRtf,
					omlPartLongDescriptionText = eRPSalesOrderLineInformationDto.omlPartLongDescriptionText,
					omlPartRevisionID = eRPSalesOrderLineInformationDto.omlPartRevisionID,
					omlPartShortDescription = eRPSalesOrderLineInformationDto.omlPartShortDescription,
					omlProjectAreaID = eRPSalesOrderLineInformationDto.omlProjectAreaID,
					omlProjectID = eRPSalesOrderLineInformationDto.omlProjectID,
					omlQuantityShipped = eRPSalesOrderLineInformationDto.omlQuantityShipped,
					omlQuoteID = eRPSalesOrderLineInformationDto.omlQuoteID,
					omlQuoteLineID = eRPSalesOrderLineInformationDto.omlQuoteLineID,
					omlQuoteQuantityID = eRPSalesOrderLineInformationDto.omlQuoteQuantityID,
					omlReleaseNumber = eRPSalesOrderLineInformationDto.omlReleaseNumber,
					omlRmaClaimID = eRPSalesOrderLineInformationDto.omlRmaClaimID,
					omlRmaClaimLineID = eRPSalesOrderLineInformationDto.omlRmaClaimLineID,
					omlRowVersion = eRPSalesOrderLineInformationDto.omlRowVersion,
					omlSalesOrderID = eRPSalesOrderLineInformationDto.omlSalesOrderID,
					omlSecondTaxAmountBase = eRPSalesOrderLineInformationDto.omlSecondTaxAmountBase,
					omlSecondTaxAmountForeign = eRPSalesOrderLineInformationDto.omlSecondTaxAmountForeign,
					omlSecondTaxCodeID = eRPSalesOrderLineInformationDto.omlSecondTaxCodeID,
					omlSalesOrderLineID = eRPSalesOrderLineInformationDto.omlSalesOrderLineID,
					omlTaxAmountBase = eRPSalesOrderLineInformationDto.omlTaxAmountBase,
					omlTaxAmountForeign = eRPSalesOrderLineInformationDto.omlTaxAmountForeign,
					omlTaxCodeID = eRPSalesOrderLineInformationDto.omlTaxCodeID,
					omlUnitDiscountBase = eRPSalesOrderLineInformationDto.omlUnitDiscountBase,
					omlUnitDiscountForeign = eRPSalesOrderLineInformationDto.omlUnitDiscountForeign,
					omlUnitOfMeasure = eRPSalesOrderLineInformationDto.omlUnitOfMeasure,
					omlUnitPriceBase = eRPSalesOrderLineInformationDto.omlUnitPriceBase,
					omlUnitPriceForeign = eRPSalesOrderLineInformationDto.omlUnitPriceForeign,
					omlWeight = eRPSalesOrderLineInformationDto.omlWeight,
					CustomFields = eRPSalesOrderLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SalesOrderLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = salesOrderLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderLineDto>> Process_PutSalesOrderLine(ERPSalesOrderLineDto salesOrderLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPSalesOrderLineDto createdObject = null;
		ERPResponseMessageDto<ERPSalesOrderLineDto> result;
		try
		{
			IERPSalesOrderLineRepository iERPSalesOrderLineRepository = (base.ERPSalesOrderLineRepository = new ERPSalesOrderLineRepository(base.ApiClientContext));
			using (iERPSalesOrderLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPSalesOrderLineRepository.SaveSalesOrderLine(salesOrderLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPSalesOrderLineInformationDto eRPSalesOrderLineInformationDto = await base.ERPSalesOrderLineRepository.GetSalesOrderLine(salesOrderLine.omlUniqueID);
					createdObject = new ERPSalesOrderLineDto
					{
						omlCreatedBy = eRPSalesOrderLineInformationDto.omlCreatedBy,
						omlCreatedDate = eRPSalesOrderLineInformationDto.omlCreatedDate,
						omlDeliveryQuantityTotal = eRPSalesOrderLineInformationDto.omlDeliveryQuantityTotal,
						omlDepositAmountBase = eRPSalesOrderLineInformationDto.omlDepositAmountBase,
						omlDepositAmountForeign = eRPSalesOrderLineInformationDto.omlDepositAmountForeign,
						omlDepositPercent = eRPSalesOrderLineInformationDto.omlDepositPercent,
						omlDiscountPercent = eRPSalesOrderLineInformationDto.omlDiscountPercent,
						omlDocuments = eRPSalesOrderLineInformationDto.omlDocuments,
						omlUniqueID = eRPSalesOrderLineInformationDto.omlUniqueID,
						omlExtendedDiscountBase = eRPSalesOrderLineInformationDto.omlExtendedDiscountBase,
						omlExtendedDiscountForeign = eRPSalesOrderLineInformationDto.omlExtendedDiscountForeign,
						omlExtendedPriceBase = eRPSalesOrderLineInformationDto.omlExtendedPriceBase,
						omlExtendedPriceForeign = eRPSalesOrderLineInformationDto.omlExtendedPriceForeign,
						omlExtendedWeight = eRPSalesOrderLineInformationDto.omlExtendedWeight,
						omlFreightAmountBase = eRPSalesOrderLineInformationDto.omlFreightAmountBase,
						omlFreightAmountForeign = eRPSalesOrderLineInformationDto.omlFreightAmountForeign,
						omlFullExtendedPriceBase = eRPSalesOrderLineInformationDto.omlFullExtendedPriceBase,
						omlFullExtendedPriceForeign = eRPSalesOrderLineInformationDto.omlFullExtendedPriceForeign,
						omlFullUnitPriceBase = eRPSalesOrderLineInformationDto.omlFullUnitPriceBase,
						omlFullUnitPriceForeign = eRPSalesOrderLineInformationDto.omlFullUnitPriceForeign,
						omlAvalaraIgnoreLine = eRPSalesOrderLineInformationDto.omlAvalaraIgnoreLine,
						omlClosed = eRPSalesOrderLineInformationDto.omlClosed,
						omlConfigured = eRPSalesOrderLineInformationDto.omlConfigured,
						omlDeposit = eRPSalesOrderLineInformationDto.omlDeposit,
						omlDepositCreated = eRPSalesOrderLineInformationDto.omlDepositCreated,
						omlDepositCredited = eRPSalesOrderLineInformationDto.omlDepositCredited,
						omlPayCommission = eRPSalesOrderLineInformationDto.omlPayCommission,
						omlPriceOverride = eRPSalesOrderLineInformationDto.omlPriceOverride,
						omlTimeAndMaterial = eRPSalesOrderLineInformationDto.omlTimeAndMaterial,
						omlLeadID = eRPSalesOrderLineInformationDto.omlLeadID,
						omlLeadLineID = eRPSalesOrderLineInformationDto.omlLeadLineID,
						omlNonTaxReasonID = eRPSalesOrderLineInformationDto.omlNonTaxReasonID,
						omlOrderQuantity = eRPSalesOrderLineInformationDto.omlOrderQuantity,
						omlOrgPartID = eRPSalesOrderLineInformationDto.omlOrgPartID,
						omlOrgPartShortDescription = eRPSalesOrderLineInformationDto.omlOrgPartShortDescription,
						omlPartGroupID = eRPSalesOrderLineInformationDto.omlPartGroupID,
						omlPartID = eRPSalesOrderLineInformationDto.omlPartID,
						omlPartLongDescriptionRtf = eRPSalesOrderLineInformationDto.omlPartLongDescriptionRtf,
						omlPartLongDescriptionText = eRPSalesOrderLineInformationDto.omlPartLongDescriptionText,
						omlPartRevisionID = eRPSalesOrderLineInformationDto.omlPartRevisionID,
						omlPartShortDescription = eRPSalesOrderLineInformationDto.omlPartShortDescription,
						omlProjectAreaID = eRPSalesOrderLineInformationDto.omlProjectAreaID,
						omlProjectID = eRPSalesOrderLineInformationDto.omlProjectID,
						omlQuantityShipped = eRPSalesOrderLineInformationDto.omlQuantityShipped,
						omlQuoteID = eRPSalesOrderLineInformationDto.omlQuoteID,
						omlQuoteLineID = eRPSalesOrderLineInformationDto.omlQuoteLineID,
						omlQuoteQuantityID = eRPSalesOrderLineInformationDto.omlQuoteQuantityID,
						omlReleaseNumber = eRPSalesOrderLineInformationDto.omlReleaseNumber,
						omlRmaClaimID = eRPSalesOrderLineInformationDto.omlRmaClaimID,
						omlRmaClaimLineID = eRPSalesOrderLineInformationDto.omlRmaClaimLineID,
						omlRowVersion = eRPSalesOrderLineInformationDto.omlRowVersion,
						omlSalesOrderID = eRPSalesOrderLineInformationDto.omlSalesOrderID,
						omlSecondTaxAmountBase = eRPSalesOrderLineInformationDto.omlSecondTaxAmountBase,
						omlSecondTaxAmountForeign = eRPSalesOrderLineInformationDto.omlSecondTaxAmountForeign,
						omlSecondTaxCodeID = eRPSalesOrderLineInformationDto.omlSecondTaxCodeID,
						omlSalesOrderLineID = eRPSalesOrderLineInformationDto.omlSalesOrderLineID,
						omlTaxAmountBase = eRPSalesOrderLineInformationDto.omlTaxAmountBase,
						omlTaxAmountForeign = eRPSalesOrderLineInformationDto.omlTaxAmountForeign,
						omlTaxCodeID = eRPSalesOrderLineInformationDto.omlTaxCodeID,
						omlUnitDiscountBase = eRPSalesOrderLineInformationDto.omlUnitDiscountBase,
						omlUnitDiscountForeign = eRPSalesOrderLineInformationDto.omlUnitDiscountForeign,
						omlUnitOfMeasure = eRPSalesOrderLineInformationDto.omlUnitOfMeasure,
						omlUnitPriceBase = eRPSalesOrderLineInformationDto.omlUnitPriceBase,
						omlUnitPriceForeign = eRPSalesOrderLineInformationDto.omlUnitPriceForeign,
						omlWeight = eRPSalesOrderLineInformationDto.omlWeight,
						CustomFields = eRPSalesOrderLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing SalesOrderLine [{salesOrderLine.omlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderLine(Guid salesOrderLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderLineRepository iERPSalesOrderLineRepository = (base.ERPSalesOrderLineRepository = new ERPSalesOrderLineRepository(base.ApiClientContext));
		using (iERPSalesOrderLineRepository)
		{
			if (!(await base.ERPSalesOrderLineRepository.DoesSalesOrderLineExist(salesOrderLineId)))
			{
				base.ErrorsList.Add($"SalesOrderLine [{salesOrderLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPSalesOrderLineInformationDto eRPSalesOrderLineInformationDto = await base.ERPSalesOrderLineRepository.GetSalesOrderLine(salesOrderLineId);
				string text = await base.ERPSalesOrderLineRepository.WhereUsed("SalesOrderLines", new object[2] { eRPSalesOrderLineInformationDto.omlSalesOrderID, eRPSalesOrderLineInformationDto.omlSalesOrderLineID }, new object[2] { "omlSalesOrderID", "omlSalesOrderLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("SalesOrderLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderLineDto>> Process_DeleteSalesOrderLine(Guid salesOrderLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPSalesOrderLineDto> result;
		try
		{
			IERPSalesOrderLineRepository iERPSalesOrderLineRepository = (base.ERPSalesOrderLineRepository = new ERPSalesOrderLineRepository(base.ApiClientContext));
			using (iERPSalesOrderLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPSalesOrderLineRepository.DeleteRowFromTable("SalesOrderLines", "oml", salesOrderLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of SalesOrderLine [{salesOrderLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPSalesOrderLineDto()
			};
		}
		return result;
	}
}
