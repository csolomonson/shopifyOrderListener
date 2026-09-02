using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPQuoteMaterialModel : ERPBaseModel, IERPQuoteMaterialModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllQuoteMaterials(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPQuoteMaterialRepository iERPQuoteMaterialRepository = (base.ERPQuoteMaterialRepository = new ERPQuoteMaterialRepository(base.ApiClientContext));
		using (iERPQuoteMaterialRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPQuoteMaterialRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPQuoteMaterialRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPQuoteMaterialRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPQuoteMaterialRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetQuoteMaterial(Guid quoteMaterialId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteMaterialRepository iERPQuoteMaterialRepository = (base.ERPQuoteMaterialRepository = new ERPQuoteMaterialRepository(base.ApiClientContext));
		using (iERPQuoteMaterialRepository)
		{
			if (!(await base.ERPQuoteMaterialRepository.DoesQuoteMaterialExist(quoteMaterialId)))
			{
				errorsList.Add($"QuoteMaterial [{quoteMaterialId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutQuoteMaterial(ERPQuoteMaterialDto quoteMaterial)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteMaterialRepository iERPQuoteMaterialRepository = (base.ERPQuoteMaterialRepository = new ERPQuoteMaterialRepository(base.ApiClientContext));
		using (iERPQuoteMaterialRepository)
		{
			if (!string.IsNullOrWhiteSpace(quoteMaterial.qmmQuoteID) && !(await base.ERPQuoteMaterialRepository.DoesRecordExistInTableUsingKeys("Quotes", new object[1] { "QMPQUOTEID" }, new object[1] { quoteMaterial.qmmQuoteID })))
			{
				errorsList.Add("qmmQuoteID [" + quoteMaterial.qmmQuoteID + "] not found.");
			}
			if (quoteMaterial.qmmQuoteLineID > 0 && !(await base.ERPQuoteMaterialRepository.DoesRecordExistInTableUsingKeys("QuoteLines", new object[2] { "QMLQUOTEID", "QMLQUOTELINEID" }, new object[2] { quoteMaterial.qmmQuoteID, quoteMaterial.qmmQuoteLineID })))
			{
				errorsList.Add($"qmmQuoteLineID [{quoteMaterial.qmmQuoteLineID}] not found.");
			}
			if (quoteMaterial.qmmQuoteAssemblyID > 0 && !(await base.ERPQuoteMaterialRepository.DoesRecordExistInTableUsingKeys("QuoteAssemblies", new object[3] { "QMAQUOTEID", "QMAQUOTELINEID", "QMAQUOTEASSEMBLYID" }, new object[3] { quoteMaterial.qmmQuoteID, quoteMaterial.qmmQuoteLineID, quoteMaterial.qmmQuoteAssemblyID })))
			{
				errorsList.Add($"qmmQuoteAssemblyID [{quoteMaterial.qmmQuoteAssemblyID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteMaterial.qmmPartID) && !(await base.ERPQuoteMaterialRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { quoteMaterial.qmmPartID })))
			{
				errorsList.Add("qmmPartID [" + quoteMaterial.qmmPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteMaterial.qmmPartRevisionID) && !(await base.ERPQuoteMaterialRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { quoteMaterial.qmmPartID, quoteMaterial.qmmPartRevisionID })))
			{
				errorsList.Add("qmmPartRevisionID [" + quoteMaterial.qmmPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteMaterial.qmmPartWarehouseLocationID) && !(await base.ERPQuoteMaterialRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { quoteMaterial.qmmPartID, quoteMaterial.qmmPartRevisionID, quoteMaterial.qmmPartWarehouseLocationID })))
			{
				errorsList.Add("qmmPartWarehouseLocationID [" + quoteMaterial.qmmPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteMaterial.qmmPartBinID) && !(await base.ERPQuoteMaterialRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { quoteMaterial.qmmPartID, quoteMaterial.qmmPartRevisionID, quoteMaterial.qmmPartWarehouseLocationID, quoteMaterial.qmmPartBinID })))
			{
				errorsList.Add("qmmPartBinID [" + quoteMaterial.qmmPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteMaterial.qmmSupplierOrganizationID) && !(await base.ERPQuoteMaterialRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { quoteMaterial.qmmSupplierOrganizationID })))
			{
				errorsList.Add("qmmSupplierOrganizationID [" + quoteMaterial.qmmSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteMaterial.qmmPurchaseLocationID) && !(await base.ERPQuoteMaterialRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { quoteMaterial.qmmSupplierOrganizationID, quoteMaterial.qmmPurchaseLocationID })))
			{
				errorsList.Add("qmmPurchaseLocationID [" + quoteMaterial.qmmPurchaseLocationID + "] not found.");
			}
			if (quoteMaterial.qmmRelatedQuoteOperationID > 0 && !(await base.ERPQuoteMaterialRepository.DoesRecordExistInTableUsingKeys("QuoteOperations", new object[4] { "QMOQUOTEID", "QMOQUOTELINEID", "QMOQUOTEASSEMBLYID", "QMOQUOTEOPERATIONID" }, new object[4] { quoteMaterial.qmmQuoteID, quoteMaterial.qmmQuoteLineID, quoteMaterial.qmmQuoteAssemblyID, quoteMaterial.qmmRelatedQuoteOperationID })))
			{
				errorsList.Add($"qmmRelatedQuoteOperationID [{quoteMaterial.qmmRelatedQuoteOperationID}] not found.");
			}
			if (quoteMaterial.qmmSourcePriceID > 0 && !(await base.ERPQuoteMaterialRepository.DoesRecordExistInTableUsingKeys("PartPrices", new object[1] { "IMIPARTPRICEID" }, new object[1] { quoteMaterial.qmmSourcePriceID })))
			{
				errorsList.Add($"qmmSourcePriceID [{quoteMaterial.qmmSourcePriceID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteMaterial.qmmSourceRfqID) && !(await base.ERPQuoteMaterialRepository.DoesRecordExistInTableUsingKeys("RFQs", new object[1] { "RQPRFQID" }, new object[1] { quoteMaterial.qmmSourceRfqID })))
			{
				errorsList.Add("qmmSourceRfqID [" + quoteMaterial.qmmSourceRfqID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPQuoteMaterialDto>>> Process_GetAllQuoteMaterials(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPQuoteMaterialDto> allQuoteMaterialsDto = new List<ERPQuoteMaterialDto>();
		ERPResponseMessageDto<IList<ERPQuoteMaterialDto>> result;
		try
		{
			IERPQuoteMaterialRepository iERPQuoteMaterialRepository = (base.ERPQuoteMaterialRepository = new ERPQuoteMaterialRepository(base.ApiClientContext));
			using (iERPQuoteMaterialRepository)
			{
				foreach (ERPQuoteMaterialInformationDto item2 in await base.ERPQuoteMaterialRepository.GetAllQuoteMaterials(pageSize, pageNumber, filter, orderBy))
				{
					ERPQuoteMaterialDto item = new ERPQuoteMaterialDto
					{
						qmmCreatedBy = item2.qmmCreatedBy,
						qmmCreatedDate = item2.qmmCreatedDate,
						qmmDocuments = item2.qmmDocuments,
						qmmUniqueID = item2.qmmUniqueID,
						qmmEstimatedUnitCost = item2.qmmEstimatedUnitCost,
						qmmBackflush = item2.qmmBackflush,
						qmmClosed = item2.qmmClosed,
						qmmCostOverride = item2.qmmCostOverride,
						qmmLeadTime = item2.qmmLeadTime,
						qmmLeadTime1 = item2.qmmLeadTime1,
						qmmLeadTime2 = item2.qmmLeadTime2,
						qmmLeadTime3 = item2.qmmLeadTime3,
						qmmLeadTime4 = item2.qmmLeadTime4,
						qmmLeadTime5 = item2.qmmLeadTime5,
						qmmLeadTime6 = item2.qmmLeadTime6,
						qmmLeadTime7 = item2.qmmLeadTime7,
						qmmLeadTime8 = item2.qmmLeadTime8,
						qmmLeadTime9 = item2.qmmLeadTime9,
						qmmMinimumCharge = item2.qmmMinimumCharge,
						qmmPartBinID = item2.qmmPartBinID,
						qmmPartID = item2.qmmPartID,
						qmmPartLongDescriptionRtf = item2.qmmPartLongDescriptionRtf,
						qmmPartLongDescriptionText = item2.qmmPartLongDescriptionText,
						qmmPartRevisionID = item2.qmmPartRevisionID,
						qmmPartShortDescription = item2.qmmPartShortDescription,
						qmmPartWarehouseLocationID = item2.qmmPartWarehouseLocationID,
						qmmPurchaseLocationID = item2.qmmPurchaseLocationID,
						qmmQuantityBreak1 = item2.qmmQuantityBreak1,
						qmmQuantityBreak2 = item2.qmmQuantityBreak2,
						qmmQuantityBreak3 = item2.qmmQuantityBreak3,
						qmmQuantityBreak4 = item2.qmmQuantityBreak4,
						qmmQuantityBreak5 = item2.qmmQuantityBreak5,
						qmmQuantityBreak6 = item2.qmmQuantityBreak6,
						qmmQuantityBreak7 = item2.qmmQuantityBreak7,
						qmmQuantityBreak8 = item2.qmmQuantityBreak8,
						qmmQuantityBreak9 = item2.qmmQuantityBreak9,
						qmmQuantityPerAssembly = item2.qmmQuantityPerAssembly,
						qmmQuoteAssemblyID = item2.qmmQuoteAssemblyID,
						qmmQuoteID = item2.qmmQuoteID,
						qmmQuoteLineID = item2.qmmQuoteLineID,
						qmmRelatedQuoteOperationID = item2.qmmRelatedQuoteOperationID,
						qmmRowVersion = item2.qmmRowVersion,
						qmmScrapPercent = item2.qmmScrapPercent,
						qmmScrapQuantity = item2.qmmScrapQuantity,
						qmmQuoteMaterialID = item2.qmmQuoteMaterialID,
						qmmSourcePriceID = item2.qmmSourcePriceID,
						qmmSourceRfqID = item2.qmmSourceRfqID,
						qmmSupplierOrganizationID = item2.qmmSupplierOrganizationID,
						qmmUnitCost1 = item2.qmmUnitCost1,
						qmmUnitCost2 = item2.qmmUnitCost2,
						qmmUnitCost3 = item2.qmmUnitCost3,
						qmmUnitCost4 = item2.qmmUnitCost4,
						qmmUnitCost5 = item2.qmmUnitCost5,
						qmmUnitCost6 = item2.qmmUnitCost6,
						qmmUnitCost7 = item2.qmmUnitCost7,
						qmmUnitCost8 = item2.qmmUnitCost8,
						qmmUnitCost9 = item2.qmmUnitCost9,
						qmmUnitOfMeasure = item2.qmmUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allQuoteMaterialsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all QuoteMaterials]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPQuoteMaterialDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allQuoteMaterialsDto,
				RecordCount = allQuoteMaterialsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteMaterialDto>> Process_GetQuoteMaterial(Guid quoteMaterialId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPQuoteMaterialDto quoteMaterialDto = null;
		ERPResponseMessageDto<ERPQuoteMaterialDto> result;
		try
		{
			IERPQuoteMaterialRepository iERPQuoteMaterialRepository = (base.ERPQuoteMaterialRepository = new ERPQuoteMaterialRepository(base.ApiClientContext));
			using (iERPQuoteMaterialRepository)
			{
				ERPQuoteMaterialInformationDto eRPQuoteMaterialInformationDto = await base.ERPQuoteMaterialRepository.GetQuoteMaterial(quoteMaterialId);
				quoteMaterialDto = new ERPQuoteMaterialDto
				{
					qmmCreatedBy = eRPQuoteMaterialInformationDto.qmmCreatedBy,
					qmmCreatedDate = eRPQuoteMaterialInformationDto.qmmCreatedDate,
					qmmDocuments = eRPQuoteMaterialInformationDto.qmmDocuments,
					qmmUniqueID = eRPQuoteMaterialInformationDto.qmmUniqueID,
					qmmEstimatedUnitCost = eRPQuoteMaterialInformationDto.qmmEstimatedUnitCost,
					qmmBackflush = eRPQuoteMaterialInformationDto.qmmBackflush,
					qmmClosed = eRPQuoteMaterialInformationDto.qmmClosed,
					qmmCostOverride = eRPQuoteMaterialInformationDto.qmmCostOverride,
					qmmLeadTime = eRPQuoteMaterialInformationDto.qmmLeadTime,
					qmmLeadTime1 = eRPQuoteMaterialInformationDto.qmmLeadTime1,
					qmmLeadTime2 = eRPQuoteMaterialInformationDto.qmmLeadTime2,
					qmmLeadTime3 = eRPQuoteMaterialInformationDto.qmmLeadTime3,
					qmmLeadTime4 = eRPQuoteMaterialInformationDto.qmmLeadTime4,
					qmmLeadTime5 = eRPQuoteMaterialInformationDto.qmmLeadTime5,
					qmmLeadTime6 = eRPQuoteMaterialInformationDto.qmmLeadTime6,
					qmmLeadTime7 = eRPQuoteMaterialInformationDto.qmmLeadTime7,
					qmmLeadTime8 = eRPQuoteMaterialInformationDto.qmmLeadTime8,
					qmmLeadTime9 = eRPQuoteMaterialInformationDto.qmmLeadTime9,
					qmmMinimumCharge = eRPQuoteMaterialInformationDto.qmmMinimumCharge,
					qmmPartBinID = eRPQuoteMaterialInformationDto.qmmPartBinID,
					qmmPartID = eRPQuoteMaterialInformationDto.qmmPartID,
					qmmPartLongDescriptionRtf = eRPQuoteMaterialInformationDto.qmmPartLongDescriptionRtf,
					qmmPartLongDescriptionText = eRPQuoteMaterialInformationDto.qmmPartLongDescriptionText,
					qmmPartRevisionID = eRPQuoteMaterialInformationDto.qmmPartRevisionID,
					qmmPartShortDescription = eRPQuoteMaterialInformationDto.qmmPartShortDescription,
					qmmPartWarehouseLocationID = eRPQuoteMaterialInformationDto.qmmPartWarehouseLocationID,
					qmmPurchaseLocationID = eRPQuoteMaterialInformationDto.qmmPurchaseLocationID,
					qmmQuantityBreak1 = eRPQuoteMaterialInformationDto.qmmQuantityBreak1,
					qmmQuantityBreak2 = eRPQuoteMaterialInformationDto.qmmQuantityBreak2,
					qmmQuantityBreak3 = eRPQuoteMaterialInformationDto.qmmQuantityBreak3,
					qmmQuantityBreak4 = eRPQuoteMaterialInformationDto.qmmQuantityBreak4,
					qmmQuantityBreak5 = eRPQuoteMaterialInformationDto.qmmQuantityBreak5,
					qmmQuantityBreak6 = eRPQuoteMaterialInformationDto.qmmQuantityBreak6,
					qmmQuantityBreak7 = eRPQuoteMaterialInformationDto.qmmQuantityBreak7,
					qmmQuantityBreak8 = eRPQuoteMaterialInformationDto.qmmQuantityBreak8,
					qmmQuantityBreak9 = eRPQuoteMaterialInformationDto.qmmQuantityBreak9,
					qmmQuantityPerAssembly = eRPQuoteMaterialInformationDto.qmmQuantityPerAssembly,
					qmmQuoteAssemblyID = eRPQuoteMaterialInformationDto.qmmQuoteAssemblyID,
					qmmQuoteID = eRPQuoteMaterialInformationDto.qmmQuoteID,
					qmmQuoteLineID = eRPQuoteMaterialInformationDto.qmmQuoteLineID,
					qmmRelatedQuoteOperationID = eRPQuoteMaterialInformationDto.qmmRelatedQuoteOperationID,
					qmmRowVersion = eRPQuoteMaterialInformationDto.qmmRowVersion,
					qmmScrapPercent = eRPQuoteMaterialInformationDto.qmmScrapPercent,
					qmmScrapQuantity = eRPQuoteMaterialInformationDto.qmmScrapQuantity,
					qmmQuoteMaterialID = eRPQuoteMaterialInformationDto.qmmQuoteMaterialID,
					qmmSourcePriceID = eRPQuoteMaterialInformationDto.qmmSourcePriceID,
					qmmSourceRfqID = eRPQuoteMaterialInformationDto.qmmSourceRfqID,
					qmmSupplierOrganizationID = eRPQuoteMaterialInformationDto.qmmSupplierOrganizationID,
					qmmUnitCost1 = eRPQuoteMaterialInformationDto.qmmUnitCost1,
					qmmUnitCost2 = eRPQuoteMaterialInformationDto.qmmUnitCost2,
					qmmUnitCost3 = eRPQuoteMaterialInformationDto.qmmUnitCost3,
					qmmUnitCost4 = eRPQuoteMaterialInformationDto.qmmUnitCost4,
					qmmUnitCost5 = eRPQuoteMaterialInformationDto.qmmUnitCost5,
					qmmUnitCost6 = eRPQuoteMaterialInformationDto.qmmUnitCost6,
					qmmUnitCost7 = eRPQuoteMaterialInformationDto.qmmUnitCost7,
					qmmUnitCost8 = eRPQuoteMaterialInformationDto.qmmUnitCost8,
					qmmUnitCost9 = eRPQuoteMaterialInformationDto.qmmUnitCost9,
					qmmUnitOfMeasure = eRPQuoteMaterialInformationDto.qmmUnitOfMeasure,
					CustomFields = eRPQuoteMaterialInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the QuoteMaterials []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteMaterialDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteMaterialDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteMaterialDto>> Process_PutQuoteMaterial(ERPQuoteMaterialDto quoteMaterial)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPQuoteMaterialDto createdObject = null;
		ERPResponseMessageDto<ERPQuoteMaterialDto> result;
		try
		{
			IERPQuoteMaterialRepository iERPQuoteMaterialRepository = (base.ERPQuoteMaterialRepository = new ERPQuoteMaterialRepository(base.ApiClientContext));
			using (iERPQuoteMaterialRepository)
			{
				APIValidationInfoDto postResult = await base.ERPQuoteMaterialRepository.SaveQuoteMaterial(quoteMaterial);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPQuoteMaterialInformationDto eRPQuoteMaterialInformationDto = await base.ERPQuoteMaterialRepository.GetQuoteMaterial(quoteMaterial.qmmUniqueID);
					createdObject = new ERPQuoteMaterialDto
					{
						qmmCreatedBy = eRPQuoteMaterialInformationDto.qmmCreatedBy,
						qmmCreatedDate = eRPQuoteMaterialInformationDto.qmmCreatedDate,
						qmmDocuments = eRPQuoteMaterialInformationDto.qmmDocuments,
						qmmUniqueID = eRPQuoteMaterialInformationDto.qmmUniqueID,
						qmmEstimatedUnitCost = eRPQuoteMaterialInformationDto.qmmEstimatedUnitCost,
						qmmBackflush = eRPQuoteMaterialInformationDto.qmmBackflush,
						qmmClosed = eRPQuoteMaterialInformationDto.qmmClosed,
						qmmCostOverride = eRPQuoteMaterialInformationDto.qmmCostOverride,
						qmmLeadTime = eRPQuoteMaterialInformationDto.qmmLeadTime,
						qmmLeadTime1 = eRPQuoteMaterialInformationDto.qmmLeadTime1,
						qmmLeadTime2 = eRPQuoteMaterialInformationDto.qmmLeadTime2,
						qmmLeadTime3 = eRPQuoteMaterialInformationDto.qmmLeadTime3,
						qmmLeadTime4 = eRPQuoteMaterialInformationDto.qmmLeadTime4,
						qmmLeadTime5 = eRPQuoteMaterialInformationDto.qmmLeadTime5,
						qmmLeadTime6 = eRPQuoteMaterialInformationDto.qmmLeadTime6,
						qmmLeadTime7 = eRPQuoteMaterialInformationDto.qmmLeadTime7,
						qmmLeadTime8 = eRPQuoteMaterialInformationDto.qmmLeadTime8,
						qmmLeadTime9 = eRPQuoteMaterialInformationDto.qmmLeadTime9,
						qmmMinimumCharge = eRPQuoteMaterialInformationDto.qmmMinimumCharge,
						qmmPartBinID = eRPQuoteMaterialInformationDto.qmmPartBinID,
						qmmPartID = eRPQuoteMaterialInformationDto.qmmPartID,
						qmmPartLongDescriptionRtf = eRPQuoteMaterialInformationDto.qmmPartLongDescriptionRtf,
						qmmPartLongDescriptionText = eRPQuoteMaterialInformationDto.qmmPartLongDescriptionText,
						qmmPartRevisionID = eRPQuoteMaterialInformationDto.qmmPartRevisionID,
						qmmPartShortDescription = eRPQuoteMaterialInformationDto.qmmPartShortDescription,
						qmmPartWarehouseLocationID = eRPQuoteMaterialInformationDto.qmmPartWarehouseLocationID,
						qmmPurchaseLocationID = eRPQuoteMaterialInformationDto.qmmPurchaseLocationID,
						qmmQuantityBreak1 = eRPQuoteMaterialInformationDto.qmmQuantityBreak1,
						qmmQuantityBreak2 = eRPQuoteMaterialInformationDto.qmmQuantityBreak2,
						qmmQuantityBreak3 = eRPQuoteMaterialInformationDto.qmmQuantityBreak3,
						qmmQuantityBreak4 = eRPQuoteMaterialInformationDto.qmmQuantityBreak4,
						qmmQuantityBreak5 = eRPQuoteMaterialInformationDto.qmmQuantityBreak5,
						qmmQuantityBreak6 = eRPQuoteMaterialInformationDto.qmmQuantityBreak6,
						qmmQuantityBreak7 = eRPQuoteMaterialInformationDto.qmmQuantityBreak7,
						qmmQuantityBreak8 = eRPQuoteMaterialInformationDto.qmmQuantityBreak8,
						qmmQuantityBreak9 = eRPQuoteMaterialInformationDto.qmmQuantityBreak9,
						qmmQuantityPerAssembly = eRPQuoteMaterialInformationDto.qmmQuantityPerAssembly,
						qmmQuoteAssemblyID = eRPQuoteMaterialInformationDto.qmmQuoteAssemblyID,
						qmmQuoteID = eRPQuoteMaterialInformationDto.qmmQuoteID,
						qmmQuoteLineID = eRPQuoteMaterialInformationDto.qmmQuoteLineID,
						qmmRelatedQuoteOperationID = eRPQuoteMaterialInformationDto.qmmRelatedQuoteOperationID,
						qmmRowVersion = eRPQuoteMaterialInformationDto.qmmRowVersion,
						qmmScrapPercent = eRPQuoteMaterialInformationDto.qmmScrapPercent,
						qmmScrapQuantity = eRPQuoteMaterialInformationDto.qmmScrapQuantity,
						qmmQuoteMaterialID = eRPQuoteMaterialInformationDto.qmmQuoteMaterialID,
						qmmSourcePriceID = eRPQuoteMaterialInformationDto.qmmSourcePriceID,
						qmmSourceRfqID = eRPQuoteMaterialInformationDto.qmmSourceRfqID,
						qmmSupplierOrganizationID = eRPQuoteMaterialInformationDto.qmmSupplierOrganizationID,
						qmmUnitCost1 = eRPQuoteMaterialInformationDto.qmmUnitCost1,
						qmmUnitCost2 = eRPQuoteMaterialInformationDto.qmmUnitCost2,
						qmmUnitCost3 = eRPQuoteMaterialInformationDto.qmmUnitCost3,
						qmmUnitCost4 = eRPQuoteMaterialInformationDto.qmmUnitCost4,
						qmmUnitCost5 = eRPQuoteMaterialInformationDto.qmmUnitCost5,
						qmmUnitCost6 = eRPQuoteMaterialInformationDto.qmmUnitCost6,
						qmmUnitCost7 = eRPQuoteMaterialInformationDto.qmmUnitCost7,
						qmmUnitCost8 = eRPQuoteMaterialInformationDto.qmmUnitCost8,
						qmmUnitCost9 = eRPQuoteMaterialInformationDto.qmmUnitCost9,
						qmmUnitOfMeasure = eRPQuoteMaterialInformationDto.qmmUnitOfMeasure,
						CustomFields = eRPQuoteMaterialInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing QuoteMaterial [{quoteMaterial.qmmUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteMaterialDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteQuoteMaterial(Guid quoteMaterialId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteMaterialRepository iERPQuoteMaterialRepository = (base.ERPQuoteMaterialRepository = new ERPQuoteMaterialRepository(base.ApiClientContext));
		using (iERPQuoteMaterialRepository)
		{
			if (!(await base.ERPQuoteMaterialRepository.DoesQuoteMaterialExist(quoteMaterialId)))
			{
				base.ErrorsList.Add($"QuoteMaterial [{quoteMaterialId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPQuoteMaterialInformationDto eRPQuoteMaterialInformationDto = await base.ERPQuoteMaterialRepository.GetQuoteMaterial(quoteMaterialId);
				string text = await base.ERPQuoteMaterialRepository.WhereUsed("QuoteMaterials", new object[4] { eRPQuoteMaterialInformationDto.qmmQuoteID, eRPQuoteMaterialInformationDto.qmmQuoteLineID, eRPQuoteMaterialInformationDto.qmmQuoteAssemblyID, eRPQuoteMaterialInformationDto.qmmQuoteMaterialID }, new object[4] { "qmmQuoteID", "qmmQuoteLineID", "qmmQuoteAssemblyID", "qmmQuoteMaterialID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("QuoteMaterial cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPQuoteMaterialDto>> Process_DeleteQuoteMaterial(Guid quoteMaterialId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPQuoteMaterialDto> result;
		try
		{
			IERPQuoteMaterialRepository iERPQuoteMaterialRepository = (base.ERPQuoteMaterialRepository = new ERPQuoteMaterialRepository(base.ApiClientContext));
			using (iERPQuoteMaterialRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPQuoteMaterialRepository.DeleteRowFromTable("QuoteMaterials", "qmm", quoteMaterialId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of QuoteMaterial [{quoteMaterialId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteMaterialDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPQuoteMaterialDto()
			};
		}
		return result;
	}
}
