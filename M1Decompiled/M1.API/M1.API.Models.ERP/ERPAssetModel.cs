using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAssetModel : ERPBaseModel, IERPAssetModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAssets(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAssetRepository iERPAssetRepository = (base.ERPAssetRepository = new ERPAssetRepository(base.ApiClientContext));
		using (iERPAssetRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAssetRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAssetRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAssetRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAssetRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAsset(Guid assetId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetRepository iERPAssetRepository = (base.ERPAssetRepository = new ERPAssetRepository(base.ApiClientContext));
		using (iERPAssetRepository)
		{
			if (!(await base.ERPAssetRepository.DoesAssetExist(assetId)))
			{
				errorsList.Add($"Asset [{assetId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAsset(ERPAssetDto asset)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetRepository iERPAssetRepository = (base.ERPAssetRepository = new ERPAssetRepository(base.ApiClientContext));
		using (iERPAssetRepository)
		{
			if (!string.IsNullOrWhiteSpace(asset.fapAssetTypeID) && !(await base.ERPAssetRepository.DoesRecordExistInTableUsingKeys("AssetTypes", new object[1] { "FATASSETTYPEID" }, new object[1] { asset.fapAssetTypeID })))
			{
				errorsList.Add("fapAssetTypeID [" + asset.fapAssetTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(asset.fapPlantID) && !(await base.ERPAssetRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { asset.fapPlantID })))
			{
				errorsList.Add("fapPlantID [" + asset.fapPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(asset.fapWorkCenterID) && !(await base.ERPAssetRepository.DoesRecordExistInTableUsingKeys("WorkCenters", new object[1] { "XAWWORKCENTERID" }, new object[1] { asset.fapWorkCenterID })))
			{
				errorsList.Add("fapWorkCenterID [" + asset.fapWorkCenterID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(asset.fapPurchaseOrderID) && !(await base.ERPAssetRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { asset.fapPurchaseOrderID })))
			{
				errorsList.Add("fapPurchaseOrderID [" + asset.fapPurchaseOrderID + "] not found.");
			}
			if (asset.fapPurchaseOrderLineID > 0 && !(await base.ERPAssetRepository.DoesRecordExistInTableUsingKeys("PurchaseOrderLines", new object[2] { "PMLPURCHASEORDERID", "PMLPURCHASEORDERLINEID" }, new object[2] { asset.fapPurchaseOrderID, asset.fapPurchaseOrderLineID })))
			{
				errorsList.Add($"fapPurchaseOrderLineID [{asset.fapPurchaseOrderLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(asset.fapSupplierOrganizationID) && !(await base.ERPAssetRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { asset.fapSupplierOrganizationID })))
			{
				errorsList.Add("fapSupplierOrganizationID [" + asset.fapSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(asset.fapReceiptID) && !(await base.ERPAssetRepository.DoesRecordExistInTableUsingKeys("Receipts", new object[1] { "RMPRECEIPTID" }, new object[1] { asset.fapReceiptID })))
			{
				errorsList.Add("fapReceiptID [" + asset.fapReceiptID + "] not found.");
			}
			if (asset.fapReceiptLineID > 0 && !(await base.ERPAssetRepository.DoesRecordExistInTableUsingKeys("ReceiptLines", new object[2] { "RMLRECEIPTID", "RMLRECEIPTLINEID" }, new object[2] { asset.fapReceiptID, asset.fapReceiptLineID })))
			{
				errorsList.Add($"fapReceiptLineID [{asset.fapReceiptLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(asset.fapApInvoiceID) && !(await base.ERPAssetRepository.DoesRecordExistInTableUsingKeys("APInvoices", new object[1] { "APPAPINVOICEID" }, new object[1] { asset.fapApInvoiceID })))
			{
				errorsList.Add("fapApInvoiceID [" + asset.fapApInvoiceID + "] not found.");
			}
			if (asset.fapApInvoiceLineID > 0 && !(await base.ERPAssetRepository.DoesRecordExistInTableUsingKeys("APInvoiceLines", new object[2] { "APLAPINVOICEID", "APLAPINVOICELINEID" }, new object[2] { asset.fapApInvoiceID, asset.fapApInvoiceLineID })))
			{
				errorsList.Add($"fapApInvoiceLineID [{asset.fapApInvoiceLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(asset.fapFinanceOrganizationID) && !(await base.ERPAssetRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { asset.fapFinanceOrganizationID })))
			{
				errorsList.Add("fapFinanceOrganizationID [" + asset.fapFinanceOrganizationID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAssetDto>>> Process_GetAllAssets(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAssetDto> allAssetsDto = new List<ERPAssetDto>();
		ERPResponseMessageDto<IList<ERPAssetDto>> result;
		try
		{
			IERPAssetRepository iERPAssetRepository = (base.ERPAssetRepository = new ERPAssetRepository(base.ApiClientContext));
			using (iERPAssetRepository)
			{
				foreach (ERPAssetInformationDto item2 in await base.ERPAssetRepository.GetAllAssets(pageSize, pageNumber, filter, orderBy))
				{
					ERPAssetDto item = new ERPAssetDto
					{
						fapApInvoiceID = item2.fapApInvoiceID,
						fapApInvoiceLineID = item2.fapApInvoiceLineID,
						fapAssetTypeID = item2.fapAssetTypeID,
						fapBookDepreciationEndDate = item2.fapBookDepreciationEndDate,
						fapBookDepreciationRate = item2.fapBookDepreciationRate,
						fapBookEffectiveLife = item2.fapBookEffectiveLife,
						fapBookStartValue = item2.fapBookStartValue,
						fapAssetID = item2.fapAssetID,
						fapCreatedBy = item2.fapCreatedBy,
						fapCreatedDate = item2.fapCreatedDate,
						fapDeemedValue = item2.fapDeemedValue,
						fapDepreciationLimit = item2.fapDepreciationLimit,
						fapDepreciationStartDate = item2.fapDepreciationStartDate,
						fapDescription = item2.fapDescription,
						fapDisposalDate = item2.fapDisposalDate,
						fapDisposalValue = item2.fapDisposalValue,
						fapUniqueID = item2.fapUniqueID,
						fapEstimatedProductionUnits = item2.fapEstimatedProductionUnits,
						fapFinanceOrganizationID = item2.fapFinanceOrganizationID,
						fapInServiceDate = item2.fapInServiceDate,
						fapLowCostAsset = item2.fapLowCostAsset,
						fapLowValueAssetInPool = item2.fapLowValueAssetInPool,
						fapItemType = item2.fapItemType,
						fapLeaseExpiryDate = item2.fapLeaseExpiryDate,
						fapLeaseMonths = item2.fapLeaseMonths,
						fapLocation = item2.fapLocation,
						fapLongDescriptionRtf = item2.fapLongDescriptionRtf,
						fapLongDescriptionText = item2.fapLongDescriptionText,
						fapPaymentAmount = item2.fapPaymentAmount,
						fapPlantID = item2.fapPlantID,
						fapPurchaseDate = item2.fapPurchaseDate,
						fapPurchaseOrderID = item2.fapPurchaseOrderID,
						fapPurchaseOrderLineID = item2.fapPurchaseOrderLineID,
						fapPurchaseType = item2.fapPurchaseType,
						fapPurchaseValue = item2.fapPurchaseValue,
						fapQuantity = item2.fapQuantity,
						fapReceiptDate = item2.fapReceiptDate,
						fapReceiptID = item2.fapReceiptID,
						fapReceiptLineID = item2.fapReceiptLineID,
						fapResidualAmount = item2.fapResidualAmount,
						fapRowVersion = item2.fapRowVersion,
						fapSerialNumber = item2.fapSerialNumber,
						fapStartYearInPool = item2.fapStartYearInPool,
						fapStatus = item2.fapStatus,
						fapSupplierOrganizationID = item2.fapSupplierOrganizationID,
						fapTaxableUsePercentage = item2.fapTaxableUsePercentage,
						fapTaxDepreciationEndDate = item2.fapTaxDepreciationEndDate,
						fapTaxDepreciationRate = item2.fapTaxDepreciationRate,
						fapTaxEffectiveLife = item2.fapTaxEffectiveLife,
						fapTaxStartValue = item2.fapTaxStartValue,
						fapWorkCenterID = item2.fapWorkCenterID,
						CustomFields = item2.CustomFields
					};
					allAssetsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Assets]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAssetDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAssetsDto,
				RecordCount = allAssetsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAssetDto>> Process_GetAsset(Guid assetId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAssetDto assetDto = null;
		ERPResponseMessageDto<ERPAssetDto> result;
		try
		{
			IERPAssetRepository iERPAssetRepository = (base.ERPAssetRepository = new ERPAssetRepository(base.ApiClientContext));
			using (iERPAssetRepository)
			{
				ERPAssetInformationDto eRPAssetInformationDto = await base.ERPAssetRepository.GetAsset(assetId);
				assetDto = new ERPAssetDto
				{
					fapApInvoiceID = eRPAssetInformationDto.fapApInvoiceID,
					fapApInvoiceLineID = eRPAssetInformationDto.fapApInvoiceLineID,
					fapAssetTypeID = eRPAssetInformationDto.fapAssetTypeID,
					fapBookDepreciationEndDate = eRPAssetInformationDto.fapBookDepreciationEndDate,
					fapBookDepreciationRate = eRPAssetInformationDto.fapBookDepreciationRate,
					fapBookEffectiveLife = eRPAssetInformationDto.fapBookEffectiveLife,
					fapBookStartValue = eRPAssetInformationDto.fapBookStartValue,
					fapAssetID = eRPAssetInformationDto.fapAssetID,
					fapCreatedBy = eRPAssetInformationDto.fapCreatedBy,
					fapCreatedDate = eRPAssetInformationDto.fapCreatedDate,
					fapDeemedValue = eRPAssetInformationDto.fapDeemedValue,
					fapDepreciationLimit = eRPAssetInformationDto.fapDepreciationLimit,
					fapDepreciationStartDate = eRPAssetInformationDto.fapDepreciationStartDate,
					fapDescription = eRPAssetInformationDto.fapDescription,
					fapDisposalDate = eRPAssetInformationDto.fapDisposalDate,
					fapDisposalValue = eRPAssetInformationDto.fapDisposalValue,
					fapUniqueID = eRPAssetInformationDto.fapUniqueID,
					fapEstimatedProductionUnits = eRPAssetInformationDto.fapEstimatedProductionUnits,
					fapFinanceOrganizationID = eRPAssetInformationDto.fapFinanceOrganizationID,
					fapInServiceDate = eRPAssetInformationDto.fapInServiceDate,
					fapLowCostAsset = eRPAssetInformationDto.fapLowCostAsset,
					fapLowValueAssetInPool = eRPAssetInformationDto.fapLowValueAssetInPool,
					fapItemType = eRPAssetInformationDto.fapItemType,
					fapLeaseExpiryDate = eRPAssetInformationDto.fapLeaseExpiryDate,
					fapLeaseMonths = eRPAssetInformationDto.fapLeaseMonths,
					fapLocation = eRPAssetInformationDto.fapLocation,
					fapLongDescriptionRtf = eRPAssetInformationDto.fapLongDescriptionRtf,
					fapLongDescriptionText = eRPAssetInformationDto.fapLongDescriptionText,
					fapPaymentAmount = eRPAssetInformationDto.fapPaymentAmount,
					fapPlantID = eRPAssetInformationDto.fapPlantID,
					fapPurchaseDate = eRPAssetInformationDto.fapPurchaseDate,
					fapPurchaseOrderID = eRPAssetInformationDto.fapPurchaseOrderID,
					fapPurchaseOrderLineID = eRPAssetInformationDto.fapPurchaseOrderLineID,
					fapPurchaseType = eRPAssetInformationDto.fapPurchaseType,
					fapPurchaseValue = eRPAssetInformationDto.fapPurchaseValue,
					fapQuantity = eRPAssetInformationDto.fapQuantity,
					fapReceiptDate = eRPAssetInformationDto.fapReceiptDate,
					fapReceiptID = eRPAssetInformationDto.fapReceiptID,
					fapReceiptLineID = eRPAssetInformationDto.fapReceiptLineID,
					fapResidualAmount = eRPAssetInformationDto.fapResidualAmount,
					fapRowVersion = eRPAssetInformationDto.fapRowVersion,
					fapSerialNumber = eRPAssetInformationDto.fapSerialNumber,
					fapStartYearInPool = eRPAssetInformationDto.fapStartYearInPool,
					fapStatus = eRPAssetInformationDto.fapStatus,
					fapSupplierOrganizationID = eRPAssetInformationDto.fapSupplierOrganizationID,
					fapTaxableUsePercentage = eRPAssetInformationDto.fapTaxableUsePercentage,
					fapTaxDepreciationEndDate = eRPAssetInformationDto.fapTaxDepreciationEndDate,
					fapTaxDepreciationRate = eRPAssetInformationDto.fapTaxDepreciationRate,
					fapTaxEffectiveLife = eRPAssetInformationDto.fapTaxEffectiveLife,
					fapTaxStartValue = eRPAssetInformationDto.fapTaxStartValue,
					fapWorkCenterID = eRPAssetInformationDto.fapWorkCenterID,
					CustomFields = eRPAssetInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Assets []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = assetDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAssetDto>> Process_PutAsset(ERPAssetDto asset)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAssetDto createdObject = null;
		ERPResponseMessageDto<ERPAssetDto> result;
		try
		{
			IERPAssetRepository iERPAssetRepository = (base.ERPAssetRepository = new ERPAssetRepository(base.ApiClientContext));
			using (iERPAssetRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAssetRepository.SaveAsset(asset);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAssetInformationDto eRPAssetInformationDto = await base.ERPAssetRepository.GetAsset(asset.fapUniqueID);
					createdObject = new ERPAssetDto
					{
						fapApInvoiceID = eRPAssetInformationDto.fapApInvoiceID,
						fapApInvoiceLineID = eRPAssetInformationDto.fapApInvoiceLineID,
						fapAssetTypeID = eRPAssetInformationDto.fapAssetTypeID,
						fapBookDepreciationEndDate = eRPAssetInformationDto.fapBookDepreciationEndDate,
						fapBookDepreciationRate = eRPAssetInformationDto.fapBookDepreciationRate,
						fapBookEffectiveLife = eRPAssetInformationDto.fapBookEffectiveLife,
						fapBookStartValue = eRPAssetInformationDto.fapBookStartValue,
						fapAssetID = eRPAssetInformationDto.fapAssetID,
						fapCreatedBy = eRPAssetInformationDto.fapCreatedBy,
						fapCreatedDate = eRPAssetInformationDto.fapCreatedDate,
						fapDeemedValue = eRPAssetInformationDto.fapDeemedValue,
						fapDepreciationLimit = eRPAssetInformationDto.fapDepreciationLimit,
						fapDepreciationStartDate = eRPAssetInformationDto.fapDepreciationStartDate,
						fapDescription = eRPAssetInformationDto.fapDescription,
						fapDisposalDate = eRPAssetInformationDto.fapDisposalDate,
						fapDisposalValue = eRPAssetInformationDto.fapDisposalValue,
						fapUniqueID = eRPAssetInformationDto.fapUniqueID,
						fapEstimatedProductionUnits = eRPAssetInformationDto.fapEstimatedProductionUnits,
						fapFinanceOrganizationID = eRPAssetInformationDto.fapFinanceOrganizationID,
						fapInServiceDate = eRPAssetInformationDto.fapInServiceDate,
						fapLowCostAsset = eRPAssetInformationDto.fapLowCostAsset,
						fapLowValueAssetInPool = eRPAssetInformationDto.fapLowValueAssetInPool,
						fapItemType = eRPAssetInformationDto.fapItemType,
						fapLeaseExpiryDate = eRPAssetInformationDto.fapLeaseExpiryDate,
						fapLeaseMonths = eRPAssetInformationDto.fapLeaseMonths,
						fapLocation = eRPAssetInformationDto.fapLocation,
						fapLongDescriptionRtf = eRPAssetInformationDto.fapLongDescriptionRtf,
						fapLongDescriptionText = eRPAssetInformationDto.fapLongDescriptionText,
						fapPaymentAmount = eRPAssetInformationDto.fapPaymentAmount,
						fapPlantID = eRPAssetInformationDto.fapPlantID,
						fapPurchaseDate = eRPAssetInformationDto.fapPurchaseDate,
						fapPurchaseOrderID = eRPAssetInformationDto.fapPurchaseOrderID,
						fapPurchaseOrderLineID = eRPAssetInformationDto.fapPurchaseOrderLineID,
						fapPurchaseType = eRPAssetInformationDto.fapPurchaseType,
						fapPurchaseValue = eRPAssetInformationDto.fapPurchaseValue,
						fapQuantity = eRPAssetInformationDto.fapQuantity,
						fapReceiptDate = eRPAssetInformationDto.fapReceiptDate,
						fapReceiptID = eRPAssetInformationDto.fapReceiptID,
						fapReceiptLineID = eRPAssetInformationDto.fapReceiptLineID,
						fapResidualAmount = eRPAssetInformationDto.fapResidualAmount,
						fapRowVersion = eRPAssetInformationDto.fapRowVersion,
						fapSerialNumber = eRPAssetInformationDto.fapSerialNumber,
						fapStartYearInPool = eRPAssetInformationDto.fapStartYearInPool,
						fapStatus = eRPAssetInformationDto.fapStatus,
						fapSupplierOrganizationID = eRPAssetInformationDto.fapSupplierOrganizationID,
						fapTaxableUsePercentage = eRPAssetInformationDto.fapTaxableUsePercentage,
						fapTaxDepreciationEndDate = eRPAssetInformationDto.fapTaxDepreciationEndDate,
						fapTaxDepreciationRate = eRPAssetInformationDto.fapTaxDepreciationRate,
						fapTaxEffectiveLife = eRPAssetInformationDto.fapTaxEffectiveLife,
						fapTaxStartValue = eRPAssetInformationDto.fapTaxStartValue,
						fapWorkCenterID = eRPAssetInformationDto.fapWorkCenterID,
						CustomFields = eRPAssetInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Asset [{asset.fapUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAsset(Guid assetId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetRepository iERPAssetRepository = (base.ERPAssetRepository = new ERPAssetRepository(base.ApiClientContext));
		using (iERPAssetRepository)
		{
			if (!(await base.ERPAssetRepository.DoesAssetExist(assetId)))
			{
				base.ErrorsList.Add($"Asset [{assetId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAssetInformationDto eRPAssetInformationDto = await base.ERPAssetRepository.GetAsset(assetId);
				string text = await base.ERPAssetRepository.WhereUsed("Assets", new object[1] { eRPAssetInformationDto.fapAssetID }, new object[1] { "fapAssetID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Asset cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAssetDto>> Process_DeleteAsset(Guid assetId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAssetDto> result;
		try
		{
			IERPAssetRepository iERPAssetRepository = (base.ERPAssetRepository = new ERPAssetRepository(base.ApiClientContext));
			using (iERPAssetRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAssetRepository.DeleteRowFromTable("Assets", "fap", assetId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Asset [{assetId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAssetDto()
			};
		}
		return result;
	}
}
