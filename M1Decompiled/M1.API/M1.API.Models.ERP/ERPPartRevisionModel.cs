using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartRevisionModel : ERPBaseModel, IERPPartRevisionModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartRevisions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartRevisionRepository iERPPartRevisionRepository = (base.ERPPartRevisionRepository = new ERPPartRevisionRepository(base.ApiClientContext));
		using (iERPPartRevisionRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartRevisionRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartRevisionRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartRevisionRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartRevisionRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartRevision(Guid partRevisionId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartRevisionRepository iERPPartRevisionRepository = (base.ERPPartRevisionRepository = new ERPPartRevisionRepository(base.ApiClientContext));
		using (iERPPartRevisionRepository)
		{
			if (!(await base.ERPPartRevisionRepository.DoesPartRevisionExist(partRevisionId)))
			{
				errorsList.Add($"PartRevision [{partRevisionId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartRevision(ERPPartRevisionDto partRevision)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartRevisionRepository iERPPartRevisionRepository = (base.ERPPartRevisionRepository = new ERPPartRevisionRepository(base.ApiClientContext));
		using (iERPPartRevisionRepository)
		{
			if (!string.IsNullOrWhiteSpace(partRevision.imrPartID) && !(await base.ERPPartRevisionRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partRevision.imrPartID })))
			{
				errorsList.Add("imrPartID [" + partRevision.imrPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partRevision.imrSupplierOrganizationID) && !(await base.ERPPartRevisionRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { partRevision.imrSupplierOrganizationID })))
			{
				errorsList.Add("imrSupplierOrganizationID [" + partRevision.imrSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partRevision.imrPurchaseLocationID) && !(await base.ERPPartRevisionRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { partRevision.imrSupplierOrganizationID, partRevision.imrPurchaseLocationID })))
			{
				errorsList.Add("imrPurchaseLocationID [" + partRevision.imrPurchaseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partRevision.imrSourceMethodID) && !(await base.ERPPartRevisionRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partRevision.imrSourceMethodID })))
			{
				errorsList.Add("imrSourceMethodID [" + partRevision.imrSourceMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partRevision.imrSourceRevisionID) && !(await base.ERPPartRevisionRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partRevision.imrSourceMethodID, partRevision.imrSourceRevisionID })))
			{
				errorsList.Add("imrSourceRevisionID [" + partRevision.imrSourceRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partRevision.imrProductCategoryID) && !(await base.ERPPartRevisionRepository.DoesRecordExistInTableUsingKeys("PRODUCTCATEGORIES", new object[1] { "INCPRODUCTCATEGORYID" }, new object[1] { partRevision.imrProductCategoryID })))
			{
				errorsList.Add("imrProductCategoryID [" + partRevision.imrProductCategoryID + "] not found.");
			}
			if (partRevision.imrProductCategoryLineID > 0 && !(await base.ERPPartRevisionRepository.DoesRecordExistInTableUsingKeys("PRODUCTCATEGORYLINES", new object[2] { "INSPRODUCTCATEGORYID", "INSPRODUCTCATEGORYLINEID" }, new object[2] { partRevision.imrProductCategoryID, partRevision.imrProductCategoryLineID })))
			{
				errorsList.Add($"imrProductCategoryLineID [{partRevision.imrProductCategoryLineID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartRevisionDto>>> Process_GetAllPartRevisions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartRevisionDto> allPartRevisionsDto = new List<ERPPartRevisionDto>();
		ERPResponseMessageDto<IList<ERPPartRevisionDto>> result;
		try
		{
			IERPPartRevisionRepository iERPPartRevisionRepository = (base.ERPPartRevisionRepository = new ERPPartRevisionRepository(base.ApiClientContext));
			using (iERPPartRevisionRepository)
			{
				foreach (ERPPartRevisionInformationDto item2 in await base.ERPPartRevisionRepository.GetAllPartRevisions(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartRevisionDto item = new ERPPartRevisionDto
					{
						imrAverageDutyCost = item2.imrAverageDutyCost,
						imrAverageFreightCost = item2.imrAverageFreightCost,
						imrAverageLaborCost = item2.imrAverageLaborCost,
						imrAverageMaterialCost = item2.imrAverageMaterialCost,
						imrAverageMiscCost = item2.imrAverageMiscCost,
						imrAverageOverheadCost = item2.imrAverageOverheadCost,
						imrAverageSubcontractCost = item2.imrAverageSubcontractCost,
						imrBarLength = item2.imrBarLength,
						imrBlanketPeriodBegin = item2.imrBlanketPeriodBegin,
						imrBlanketPeriodEnd = item2.imrBlanketPeriodEnd,
						imrPartRevisionID = item2.imrPartRevisionID,
						imrCommodityCode = item2.imrCommodityCode,
						imrCommodityDescription = item2.imrCommodityDescription,
						imrConversionFactor = item2.imrConversionFactor,
						imrCountryOfManufacture = item2.imrCountryOfManufacture,
						imrCreatedBy = item2.imrCreatedBy,
						imrCreatedDate = item2.imrCreatedDate,
						imrDocuments = item2.imrDocuments,
						imrEffectiveEndDate = item2.imrEffectiveEndDate,
						imrEffectiveStartDate = item2.imrEffectiveStartDate,
						imrUniqueID = item2.imrUniqueID,
						imrExpenseSplitPercentTotal = item2.imrExpenseSplitPercentTotal,
						imrFdxHandlingCost = item2.imrFdxHandlingCost,
						imrFdxPackageHeight = item2.imrFdxPackageHeight,
						imrFdxPackageLength = item2.imrFdxPackageLength,
						imrFdxPackageWidth = item2.imrFdxPackageWidth,
						imrFdxPackaging = item2.imrFdxPackaging,
						imrFdxPackagingCost = item2.imrFdxPackagingCost,
						imrFdxShipCostMarkupPct = item2.imrFdxShipCostMarkupPct,
						imrFormID = item2.imrFormID,
						imrInspectionNotesRTF = item2.imrInspectionNotesRTF,
						imrInspectionNotesText = item2.imrInspectionNotesText,
						imrInventoryUnitOfMeasure = item2.imrInventoryUnitOfMeasure,
						imrInactive = item2.imrInactive,
						imrConfigured = item2.imrConfigured,
						imrFdxNonstandardContainer = item2.imrFdxNonstandardContainer,
						imrFdxOneItemPerShipment = item2.imrFdxOneItemPerShipment,
						imrPreferredRefExists = item2.imrPreferredRefExists,
						imrPurchasableItem = item2.imrPurchasableItem,
						imrSuppressShortDescription = item2.imrSuppressShortDescription,
						imrUseQuotePrice = item2.imrUseQuotePrice,
						imrLastDutyCost = item2.imrLastDutyCost,
						imrLastFreightCost = item2.imrLastFreightCost,
						imrLastLaborCost = item2.imrLastLaborCost,
						imrLastMaterialCost = item2.imrLastMaterialCost,
						imrLastMiscCost = item2.imrLastMiscCost,
						imrLastOverheadCost = item2.imrLastOverheadCost,
						imrLastReceiptDate = item2.imrLastReceiptDate,
						imrLastRunDatePurchasePlanner = item2.imrLastRunDatePurchasePlanner,
						imrLastSubcontractCost = item2.imrLastSubcontractCost,
						imrLastTransactionDate = item2.imrLastTransactionDate,
						imrLeadTime = item2.imrLeadTime,
						imrLongDescriptionHtml = item2.imrLongDescriptionHtml,
						imrLongDescriptionRtf = item2.imrLongDescriptionRtf,
						imrLongDescriptionText = item2.imrLongDescriptionText,
						imrManufacturingLotSize = item2.imrManufacturingLotSize,
						imrMaximumQuantity = item2.imrMaximumQuantity,
						imrMinimumQuantity = item2.imrMinimumQuantity,
						imrNetCostBeginDate = item2.imrNetCostBeginDate,
						imrNetCostCode = item2.imrNetCostCode,
						imrNetCostEndDate = item2.imrNetCostEndDate,
						imrPartID = item2.imrPartID,
						imrPartImageFileName = item2.imrPartImageFileName,
						imrPreferenceCriteria = item2.imrPreferenceCriteria,
						imrProducerDetermination = item2.imrProducerDetermination,
						imrProductCategoryID = item2.imrProductCategoryID,
						imrProductCategoryLineID = item2.imrProductCategoryLineID,
						imrProductionNotesRTF = item2.imrProductionNotesRTF,
						imrProductionNotesText = item2.imrProductionNotesText,
						imrPurchaseLocationID = item2.imrPurchaseLocationID,
						imrPurchaseUnitOfMeasure = item2.imrPurchaseUnitOfMeasure,
						imrQuantityAllocated = item2.imrQuantityAllocated,
						imrQuantityOnHand = item2.imrQuantityOnHand,
						imrQuantityOnOrderPurchases = item2.imrQuantityOnOrderPurchases,
						imrQuantityOnOrderSales = item2.imrQuantityOnOrderSales,
						imrQuantityToInspect = item2.imrQuantityToInspect,
						imrQuantityToReturn = item2.imrQuantityToReturn,
						imrQuantityToReturnJob = item2.imrQuantityToReturnJob,
						imrRequiresInspection = item2.imrRequiresInspection,
						imrRowVersion = item2.imrRowVersion,
						imrSheetSizeX = item2.imrSheetSizeX,
						imrSheetSizeY = item2.imrSheetSizeY,
						imrShortDescription = item2.imrShortDescription,
						imrSourceMethodID = item2.imrSourceMethodID,
						imrSourceRevisionID = item2.imrSourceRevisionID,
						imrStandardDutyCost = item2.imrStandardDutyCost,
						imrStandardFreightCost = item2.imrStandardFreightCost,
						imrStandardLaborCost = item2.imrStandardLaborCost,
						imrStandardMaterialCost = item2.imrStandardMaterialCost,
						imrStandardMiscCost = item2.imrStandardMiscCost,
						imrStandardOverheadCost = item2.imrStandardOverheadCost,
						imrStandardSubcontractCost = item2.imrStandardSubcontractCost,
						imrSupplierOrganizationID = item2.imrSupplierOrganizationID,
						imrThickness = item2.imrThickness,
						imrUniversalProductCode = item2.imrUniversalProductCode,
						imrVolume = item2.imrVolume,
						imrWeight = item2.imrWeight,
						imrWeightUnitOfMeasure = item2.imrWeightUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allPartRevisionsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartRevisions]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartRevisionDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartRevisionsDto,
				RecordCount = allPartRevisionsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartRevisionDto>> Process_GetPartRevision(Guid partRevisionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartRevisionDto partRevisionDto = null;
		ERPResponseMessageDto<ERPPartRevisionDto> result;
		try
		{
			IERPPartRevisionRepository iERPPartRevisionRepository = (base.ERPPartRevisionRepository = new ERPPartRevisionRepository(base.ApiClientContext));
			using (iERPPartRevisionRepository)
			{
				ERPPartRevisionInformationDto eRPPartRevisionInformationDto = await base.ERPPartRevisionRepository.GetPartRevision(partRevisionId);
				partRevisionDto = new ERPPartRevisionDto
				{
					imrAverageDutyCost = eRPPartRevisionInformationDto.imrAverageDutyCost,
					imrAverageFreightCost = eRPPartRevisionInformationDto.imrAverageFreightCost,
					imrAverageLaborCost = eRPPartRevisionInformationDto.imrAverageLaborCost,
					imrAverageMaterialCost = eRPPartRevisionInformationDto.imrAverageMaterialCost,
					imrAverageMiscCost = eRPPartRevisionInformationDto.imrAverageMiscCost,
					imrAverageOverheadCost = eRPPartRevisionInformationDto.imrAverageOverheadCost,
					imrAverageSubcontractCost = eRPPartRevisionInformationDto.imrAverageSubcontractCost,
					imrBarLength = eRPPartRevisionInformationDto.imrBarLength,
					imrBlanketPeriodBegin = eRPPartRevisionInformationDto.imrBlanketPeriodBegin,
					imrBlanketPeriodEnd = eRPPartRevisionInformationDto.imrBlanketPeriodEnd,
					imrPartRevisionID = eRPPartRevisionInformationDto.imrPartRevisionID,
					imrCommodityCode = eRPPartRevisionInformationDto.imrCommodityCode,
					imrCommodityDescription = eRPPartRevisionInformationDto.imrCommodityDescription,
					imrConversionFactor = eRPPartRevisionInformationDto.imrConversionFactor,
					imrCountryOfManufacture = eRPPartRevisionInformationDto.imrCountryOfManufacture,
					imrCreatedBy = eRPPartRevisionInformationDto.imrCreatedBy,
					imrCreatedDate = eRPPartRevisionInformationDto.imrCreatedDate,
					imrDocuments = eRPPartRevisionInformationDto.imrDocuments,
					imrEffectiveEndDate = eRPPartRevisionInformationDto.imrEffectiveEndDate,
					imrEffectiveStartDate = eRPPartRevisionInformationDto.imrEffectiveStartDate,
					imrUniqueID = eRPPartRevisionInformationDto.imrUniqueID,
					imrExpenseSplitPercentTotal = eRPPartRevisionInformationDto.imrExpenseSplitPercentTotal,
					imrFdxHandlingCost = eRPPartRevisionInformationDto.imrFdxHandlingCost,
					imrFdxPackageHeight = eRPPartRevisionInformationDto.imrFdxPackageHeight,
					imrFdxPackageLength = eRPPartRevisionInformationDto.imrFdxPackageLength,
					imrFdxPackageWidth = eRPPartRevisionInformationDto.imrFdxPackageWidth,
					imrFdxPackaging = eRPPartRevisionInformationDto.imrFdxPackaging,
					imrFdxPackagingCost = eRPPartRevisionInformationDto.imrFdxPackagingCost,
					imrFdxShipCostMarkupPct = eRPPartRevisionInformationDto.imrFdxShipCostMarkupPct,
					imrFormID = eRPPartRevisionInformationDto.imrFormID,
					imrInspectionNotesRTF = eRPPartRevisionInformationDto.imrInspectionNotesRTF,
					imrInspectionNotesText = eRPPartRevisionInformationDto.imrInspectionNotesText,
					imrInventoryUnitOfMeasure = eRPPartRevisionInformationDto.imrInventoryUnitOfMeasure,
					imrInactive = eRPPartRevisionInformationDto.imrInactive,
					imrConfigured = eRPPartRevisionInformationDto.imrConfigured,
					imrFdxNonstandardContainer = eRPPartRevisionInformationDto.imrFdxNonstandardContainer,
					imrFdxOneItemPerShipment = eRPPartRevisionInformationDto.imrFdxOneItemPerShipment,
					imrPreferredRefExists = eRPPartRevisionInformationDto.imrPreferredRefExists,
					imrPurchasableItem = eRPPartRevisionInformationDto.imrPurchasableItem,
					imrSuppressShortDescription = eRPPartRevisionInformationDto.imrSuppressShortDescription,
					imrUseQuotePrice = eRPPartRevisionInformationDto.imrUseQuotePrice,
					imrLastDutyCost = eRPPartRevisionInformationDto.imrLastDutyCost,
					imrLastFreightCost = eRPPartRevisionInformationDto.imrLastFreightCost,
					imrLastLaborCost = eRPPartRevisionInformationDto.imrLastLaborCost,
					imrLastMaterialCost = eRPPartRevisionInformationDto.imrLastMaterialCost,
					imrLastMiscCost = eRPPartRevisionInformationDto.imrLastMiscCost,
					imrLastOverheadCost = eRPPartRevisionInformationDto.imrLastOverheadCost,
					imrLastReceiptDate = eRPPartRevisionInformationDto.imrLastReceiptDate,
					imrLastRunDatePurchasePlanner = eRPPartRevisionInformationDto.imrLastRunDatePurchasePlanner,
					imrLastSubcontractCost = eRPPartRevisionInformationDto.imrLastSubcontractCost,
					imrLastTransactionDate = eRPPartRevisionInformationDto.imrLastTransactionDate,
					imrLeadTime = eRPPartRevisionInformationDto.imrLeadTime,
					imrLongDescriptionHtml = eRPPartRevisionInformationDto.imrLongDescriptionHtml,
					imrLongDescriptionRtf = eRPPartRevisionInformationDto.imrLongDescriptionRtf,
					imrLongDescriptionText = eRPPartRevisionInformationDto.imrLongDescriptionText,
					imrManufacturingLotSize = eRPPartRevisionInformationDto.imrManufacturingLotSize,
					imrMaximumQuantity = eRPPartRevisionInformationDto.imrMaximumQuantity,
					imrMinimumQuantity = eRPPartRevisionInformationDto.imrMinimumQuantity,
					imrNetCostBeginDate = eRPPartRevisionInformationDto.imrNetCostBeginDate,
					imrNetCostCode = eRPPartRevisionInformationDto.imrNetCostCode,
					imrNetCostEndDate = eRPPartRevisionInformationDto.imrNetCostEndDate,
					imrPartID = eRPPartRevisionInformationDto.imrPartID,
					imrPartImageFileName = eRPPartRevisionInformationDto.imrPartImageFileName,
					imrPreferenceCriteria = eRPPartRevisionInformationDto.imrPreferenceCriteria,
					imrProducerDetermination = eRPPartRevisionInformationDto.imrProducerDetermination,
					imrProductCategoryID = eRPPartRevisionInformationDto.imrProductCategoryID,
					imrProductCategoryLineID = eRPPartRevisionInformationDto.imrProductCategoryLineID,
					imrProductionNotesRTF = eRPPartRevisionInformationDto.imrProductionNotesRTF,
					imrProductionNotesText = eRPPartRevisionInformationDto.imrProductionNotesText,
					imrPurchaseLocationID = eRPPartRevisionInformationDto.imrPurchaseLocationID,
					imrPurchaseUnitOfMeasure = eRPPartRevisionInformationDto.imrPurchaseUnitOfMeasure,
					imrQuantityAllocated = eRPPartRevisionInformationDto.imrQuantityAllocated,
					imrQuantityOnHand = eRPPartRevisionInformationDto.imrQuantityOnHand,
					imrQuantityOnOrderPurchases = eRPPartRevisionInformationDto.imrQuantityOnOrderPurchases,
					imrQuantityOnOrderSales = eRPPartRevisionInformationDto.imrQuantityOnOrderSales,
					imrQuantityToInspect = eRPPartRevisionInformationDto.imrQuantityToInspect,
					imrQuantityToReturn = eRPPartRevisionInformationDto.imrQuantityToReturn,
					imrQuantityToReturnJob = eRPPartRevisionInformationDto.imrQuantityToReturnJob,
					imrRequiresInspection = eRPPartRevisionInformationDto.imrRequiresInspection,
					imrRowVersion = eRPPartRevisionInformationDto.imrRowVersion,
					imrSheetSizeX = eRPPartRevisionInformationDto.imrSheetSizeX,
					imrSheetSizeY = eRPPartRevisionInformationDto.imrSheetSizeY,
					imrShortDescription = eRPPartRevisionInformationDto.imrShortDescription,
					imrSourceMethodID = eRPPartRevisionInformationDto.imrSourceMethodID,
					imrSourceRevisionID = eRPPartRevisionInformationDto.imrSourceRevisionID,
					imrStandardDutyCost = eRPPartRevisionInformationDto.imrStandardDutyCost,
					imrStandardFreightCost = eRPPartRevisionInformationDto.imrStandardFreightCost,
					imrStandardLaborCost = eRPPartRevisionInformationDto.imrStandardLaborCost,
					imrStandardMaterialCost = eRPPartRevisionInformationDto.imrStandardMaterialCost,
					imrStandardMiscCost = eRPPartRevisionInformationDto.imrStandardMiscCost,
					imrStandardOverheadCost = eRPPartRevisionInformationDto.imrStandardOverheadCost,
					imrStandardSubcontractCost = eRPPartRevisionInformationDto.imrStandardSubcontractCost,
					imrSupplierOrganizationID = eRPPartRevisionInformationDto.imrSupplierOrganizationID,
					imrThickness = eRPPartRevisionInformationDto.imrThickness,
					imrUniversalProductCode = eRPPartRevisionInformationDto.imrUniversalProductCode,
					imrVolume = eRPPartRevisionInformationDto.imrVolume,
					imrWeight = eRPPartRevisionInformationDto.imrWeight,
					imrWeightUnitOfMeasure = eRPPartRevisionInformationDto.imrWeightUnitOfMeasure,
					CustomFields = eRPPartRevisionInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartRevisions []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartRevisionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partRevisionDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartRevisionDto>> Process_PutPartRevision(ERPPartRevisionDto partRevision)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartRevisionDto createdObject = null;
		ERPResponseMessageDto<ERPPartRevisionDto> result;
		try
		{
			IERPPartRevisionRepository iERPPartRevisionRepository = (base.ERPPartRevisionRepository = new ERPPartRevisionRepository(base.ApiClientContext));
			using (iERPPartRevisionRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartRevisionRepository.SavePartRevision(partRevision);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartRevisionInformationDto eRPPartRevisionInformationDto = await base.ERPPartRevisionRepository.GetPartRevision(partRevision.imrUniqueID);
					createdObject = new ERPPartRevisionDto
					{
						imrAverageDutyCost = eRPPartRevisionInformationDto.imrAverageDutyCost,
						imrAverageFreightCost = eRPPartRevisionInformationDto.imrAverageFreightCost,
						imrAverageLaborCost = eRPPartRevisionInformationDto.imrAverageLaborCost,
						imrAverageMaterialCost = eRPPartRevisionInformationDto.imrAverageMaterialCost,
						imrAverageMiscCost = eRPPartRevisionInformationDto.imrAverageMiscCost,
						imrAverageOverheadCost = eRPPartRevisionInformationDto.imrAverageOverheadCost,
						imrAverageSubcontractCost = eRPPartRevisionInformationDto.imrAverageSubcontractCost,
						imrBarLength = eRPPartRevisionInformationDto.imrBarLength,
						imrBlanketPeriodBegin = eRPPartRevisionInformationDto.imrBlanketPeriodBegin,
						imrBlanketPeriodEnd = eRPPartRevisionInformationDto.imrBlanketPeriodEnd,
						imrPartRevisionID = eRPPartRevisionInformationDto.imrPartRevisionID,
						imrCommodityCode = eRPPartRevisionInformationDto.imrCommodityCode,
						imrCommodityDescription = eRPPartRevisionInformationDto.imrCommodityDescription,
						imrConversionFactor = eRPPartRevisionInformationDto.imrConversionFactor,
						imrCountryOfManufacture = eRPPartRevisionInformationDto.imrCountryOfManufacture,
						imrCreatedBy = eRPPartRevisionInformationDto.imrCreatedBy,
						imrCreatedDate = eRPPartRevisionInformationDto.imrCreatedDate,
						imrDocuments = eRPPartRevisionInformationDto.imrDocuments,
						imrEffectiveEndDate = eRPPartRevisionInformationDto.imrEffectiveEndDate,
						imrEffectiveStartDate = eRPPartRevisionInformationDto.imrEffectiveStartDate,
						imrUniqueID = eRPPartRevisionInformationDto.imrUniqueID,
						imrExpenseSplitPercentTotal = eRPPartRevisionInformationDto.imrExpenseSplitPercentTotal,
						imrFdxHandlingCost = eRPPartRevisionInformationDto.imrFdxHandlingCost,
						imrFdxPackageHeight = eRPPartRevisionInformationDto.imrFdxPackageHeight,
						imrFdxPackageLength = eRPPartRevisionInformationDto.imrFdxPackageLength,
						imrFdxPackageWidth = eRPPartRevisionInformationDto.imrFdxPackageWidth,
						imrFdxPackaging = eRPPartRevisionInformationDto.imrFdxPackaging,
						imrFdxPackagingCost = eRPPartRevisionInformationDto.imrFdxPackagingCost,
						imrFdxShipCostMarkupPct = eRPPartRevisionInformationDto.imrFdxShipCostMarkupPct,
						imrFormID = eRPPartRevisionInformationDto.imrFormID,
						imrInspectionNotesRTF = eRPPartRevisionInformationDto.imrInspectionNotesRTF,
						imrInspectionNotesText = eRPPartRevisionInformationDto.imrInspectionNotesText,
						imrInventoryUnitOfMeasure = eRPPartRevisionInformationDto.imrInventoryUnitOfMeasure,
						imrInactive = eRPPartRevisionInformationDto.imrInactive,
						imrConfigured = eRPPartRevisionInformationDto.imrConfigured,
						imrFdxNonstandardContainer = eRPPartRevisionInformationDto.imrFdxNonstandardContainer,
						imrFdxOneItemPerShipment = eRPPartRevisionInformationDto.imrFdxOneItemPerShipment,
						imrPreferredRefExists = eRPPartRevisionInformationDto.imrPreferredRefExists,
						imrPurchasableItem = eRPPartRevisionInformationDto.imrPurchasableItem,
						imrSuppressShortDescription = eRPPartRevisionInformationDto.imrSuppressShortDescription,
						imrUseQuotePrice = eRPPartRevisionInformationDto.imrUseQuotePrice,
						imrLastDutyCost = eRPPartRevisionInformationDto.imrLastDutyCost,
						imrLastFreightCost = eRPPartRevisionInformationDto.imrLastFreightCost,
						imrLastLaborCost = eRPPartRevisionInformationDto.imrLastLaborCost,
						imrLastMaterialCost = eRPPartRevisionInformationDto.imrLastMaterialCost,
						imrLastMiscCost = eRPPartRevisionInformationDto.imrLastMiscCost,
						imrLastOverheadCost = eRPPartRevisionInformationDto.imrLastOverheadCost,
						imrLastReceiptDate = eRPPartRevisionInformationDto.imrLastReceiptDate,
						imrLastRunDatePurchasePlanner = eRPPartRevisionInformationDto.imrLastRunDatePurchasePlanner,
						imrLastSubcontractCost = eRPPartRevisionInformationDto.imrLastSubcontractCost,
						imrLastTransactionDate = eRPPartRevisionInformationDto.imrLastTransactionDate,
						imrLeadTime = eRPPartRevisionInformationDto.imrLeadTime,
						imrLongDescriptionHtml = eRPPartRevisionInformationDto.imrLongDescriptionHtml,
						imrLongDescriptionRtf = eRPPartRevisionInformationDto.imrLongDescriptionRtf,
						imrLongDescriptionText = eRPPartRevisionInformationDto.imrLongDescriptionText,
						imrManufacturingLotSize = eRPPartRevisionInformationDto.imrManufacturingLotSize,
						imrMaximumQuantity = eRPPartRevisionInformationDto.imrMaximumQuantity,
						imrMinimumQuantity = eRPPartRevisionInformationDto.imrMinimumQuantity,
						imrNetCostBeginDate = eRPPartRevisionInformationDto.imrNetCostBeginDate,
						imrNetCostCode = eRPPartRevisionInformationDto.imrNetCostCode,
						imrNetCostEndDate = eRPPartRevisionInformationDto.imrNetCostEndDate,
						imrPartID = eRPPartRevisionInformationDto.imrPartID,
						imrPartImageFileName = eRPPartRevisionInformationDto.imrPartImageFileName,
						imrPreferenceCriteria = eRPPartRevisionInformationDto.imrPreferenceCriteria,
						imrProducerDetermination = eRPPartRevisionInformationDto.imrProducerDetermination,
						imrProductCategoryID = eRPPartRevisionInformationDto.imrProductCategoryID,
						imrProductCategoryLineID = eRPPartRevisionInformationDto.imrProductCategoryLineID,
						imrProductionNotesRTF = eRPPartRevisionInformationDto.imrProductionNotesRTF,
						imrProductionNotesText = eRPPartRevisionInformationDto.imrProductionNotesText,
						imrPurchaseLocationID = eRPPartRevisionInformationDto.imrPurchaseLocationID,
						imrPurchaseUnitOfMeasure = eRPPartRevisionInformationDto.imrPurchaseUnitOfMeasure,
						imrQuantityAllocated = eRPPartRevisionInformationDto.imrQuantityAllocated,
						imrQuantityOnHand = eRPPartRevisionInformationDto.imrQuantityOnHand,
						imrQuantityOnOrderPurchases = eRPPartRevisionInformationDto.imrQuantityOnOrderPurchases,
						imrQuantityOnOrderSales = eRPPartRevisionInformationDto.imrQuantityOnOrderSales,
						imrQuantityToInspect = eRPPartRevisionInformationDto.imrQuantityToInspect,
						imrQuantityToReturn = eRPPartRevisionInformationDto.imrQuantityToReturn,
						imrQuantityToReturnJob = eRPPartRevisionInformationDto.imrQuantityToReturnJob,
						imrRequiresInspection = eRPPartRevisionInformationDto.imrRequiresInspection,
						imrRowVersion = eRPPartRevisionInformationDto.imrRowVersion,
						imrSheetSizeX = eRPPartRevisionInformationDto.imrSheetSizeX,
						imrSheetSizeY = eRPPartRevisionInformationDto.imrSheetSizeY,
						imrShortDescription = eRPPartRevisionInformationDto.imrShortDescription,
						imrSourceMethodID = eRPPartRevisionInformationDto.imrSourceMethodID,
						imrSourceRevisionID = eRPPartRevisionInformationDto.imrSourceRevisionID,
						imrStandardDutyCost = eRPPartRevisionInformationDto.imrStandardDutyCost,
						imrStandardFreightCost = eRPPartRevisionInformationDto.imrStandardFreightCost,
						imrStandardLaborCost = eRPPartRevisionInformationDto.imrStandardLaborCost,
						imrStandardMaterialCost = eRPPartRevisionInformationDto.imrStandardMaterialCost,
						imrStandardMiscCost = eRPPartRevisionInformationDto.imrStandardMiscCost,
						imrStandardOverheadCost = eRPPartRevisionInformationDto.imrStandardOverheadCost,
						imrStandardSubcontractCost = eRPPartRevisionInformationDto.imrStandardSubcontractCost,
						imrSupplierOrganizationID = eRPPartRevisionInformationDto.imrSupplierOrganizationID,
						imrThickness = eRPPartRevisionInformationDto.imrThickness,
						imrUniversalProductCode = eRPPartRevisionInformationDto.imrUniversalProductCode,
						imrVolume = eRPPartRevisionInformationDto.imrVolume,
						imrWeight = eRPPartRevisionInformationDto.imrWeight,
						imrWeightUnitOfMeasure = eRPPartRevisionInformationDto.imrWeightUnitOfMeasure,
						CustomFields = eRPPartRevisionInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartRevision [{partRevision.imrUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartRevisionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartRevision(Guid partRevisionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartRevisionRepository iERPPartRevisionRepository = (base.ERPPartRevisionRepository = new ERPPartRevisionRepository(base.ApiClientContext));
		using (iERPPartRevisionRepository)
		{
			if (!(await base.ERPPartRevisionRepository.DoesPartRevisionExist(partRevisionId)))
			{
				base.ErrorsList.Add($"PartRevision [{partRevisionId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartRevisionInformationDto eRPPartRevisionInformationDto = await base.ERPPartRevisionRepository.GetPartRevision(partRevisionId);
				string text = await base.ERPPartRevisionRepository.WhereUsed("PartRevisions", new object[2] { eRPPartRevisionInformationDto.imrPartID, eRPPartRevisionInformationDto.imrPartRevisionID }, new object[2] { "imrPartID", "imrPartRevisionID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartRevision cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartRevisionDto>> Process_DeletePartRevision(Guid partRevisionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartRevisionDto> result;
		try
		{
			IERPPartRevisionRepository iERPPartRevisionRepository = (base.ERPPartRevisionRepository = new ERPPartRevisionRepository(base.ApiClientContext));
			using (iERPPartRevisionRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartRevisionRepository.DeleteRowFromTable("PartRevisions", "imr", partRevisionId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartRevision [{partRevisionId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartRevisionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartRevisionDto()
			};
		}
		return result;
	}
}
