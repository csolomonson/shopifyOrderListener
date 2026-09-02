using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPShipmentPackageModel : ERPBaseModel, IERPShipmentPackageModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllShipmentPackages(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPShipmentPackageRepository iERPShipmentPackageRepository = (base.ERPShipmentPackageRepository = new ERPShipmentPackageRepository(base.ApiClientContext));
		using (iERPShipmentPackageRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPShipmentPackageRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPShipmentPackageRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPShipmentPackageRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPShipmentPackageRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetShipmentPackage(Guid shipmentPackageId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentPackageRepository iERPShipmentPackageRepository = (base.ERPShipmentPackageRepository = new ERPShipmentPackageRepository(base.ApiClientContext));
		using (iERPShipmentPackageRepository)
		{
			if (!(await base.ERPShipmentPackageRepository.DoesShipmentPackageExist(shipmentPackageId)))
			{
				errorsList.Add($"ShipmentPackage [{shipmentPackageId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutShipmentPackage(ERPShipmentPackageDto shipmentPackage)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentPackageRepository iERPShipmentPackageRepository = (base.ERPShipmentPackageRepository = new ERPShipmentPackageRepository(base.ApiClientContext));
		using (iERPShipmentPackageRepository)
		{
			if (!string.IsNullOrWhiteSpace(shipmentPackage.spaShipmentID) && !(await base.ERPShipmentPackageRepository.DoesRecordExistInTableUsingKeys("SHIPMENTS", new object[1] { "SMPSHIPMENTID" }, new object[1] { shipmentPackage.spaShipmentID })))
			{
				errorsList.Add("spaShipmentID [" + shipmentPackage.spaShipmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentPackage.spaShippingMethodID) && !(await base.ERPShipmentPackageRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { shipmentPackage.spaShippingMethodID })))
			{
				errorsList.Add("spaShippingMethodID [" + shipmentPackage.spaShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentPackage.spaCustomerPackageID) && !(await base.ERPShipmentPackageRepository.DoesRecordExistInTableUsingKeys("CustomerPackages", new object[1] { "cpaCustomerPackageID" }, new object[1] { shipmentPackage.spaCustomerPackageID })))
			{
				errorsList.Add("spaCustomerPackageID [" + shipmentPackage.spaCustomerPackageID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPShipmentPackageDto>>> Process_GetAllShipmentPackages(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPShipmentPackageDto> allShipmentPackagesDto = new List<ERPShipmentPackageDto>();
		ERPResponseMessageDto<IList<ERPShipmentPackageDto>> result;
		try
		{
			IERPShipmentPackageRepository iERPShipmentPackageRepository = (base.ERPShipmentPackageRepository = new ERPShipmentPackageRepository(base.ApiClientContext));
			using (iERPShipmentPackageRepository)
			{
				foreach (ERPShipmentPackageInformationDto item2 in await base.ERPShipmentPackageRepository.GetAllShipmentPackages(pageSize, pageNumber, filter, orderBy))
				{
					ERPShipmentPackageDto item = new ERPShipmentPackageDto
					{
						spaCarrier = item2.spaCarrier,
						spaCreatedBy = item2.spaCreatedBy,
						spaCreatedDate = item2.spaCreatedDate,
						spaCustomerPackageID = item2.spaCustomerPackageID,
						spaEdi856CustomLabel = item2.spaEdi856CustomLabel,
						spaUniqueID = item2.spaUniqueID,
						spaFedExPackageTypes = item2.spaFedExPackageTypes,
						spaAdditionalHandlingRequired = item2.spaAdditionalHandlingRequired,
						spaLargePackage = item2.spaLargePackage,
						spaVerbalConfirmationRequired = item2.spaVerbalConfirmationRequired,
						spaLabelFilePath = item2.spaLabelFilePath,
						spaPackageDimensionsUom = item2.spaPackageDimensionsUom,
						spaPackageHeight = item2.spaPackageHeight,
						spaPackageLength = item2.spaPackageLength,
						spaPackageRate = item2.spaPackageRate,
						spaPackageRateForeign = item2.spaPackageRateForeign,
						spaPackageValue = item2.spaPackageValue,
						spaPackageValueForeign = item2.spaPackageValueForeign,
						spaPackageWeight = item2.spaPackageWeight,
						spaPackageWeightUom = item2.spaPackageWeightUom,
						spaPackageWidth = item2.spaPackageWidth,
						spaReference1 = item2.spaReference1,
						spaReference2 = item2.spaReference2,
						spaRowVersion = item2.SPArowVersion,
						spaShipmentPackageID = item2.spaShipmentPackageID,
						spaShipmentID = item2.spaShipmentID,
						spaShipmentIDNumber = item2.spaShipmentIDNumber,
						spaShippingMethodID = item2.spaShippingMethodID,
						spaTrackingNo = item2.spaTrackingNo,
						spaUpsPackageTypes = item2.spaUpsPackageTypes,
						CustomFields = item2.CustomFields
					};
					allShipmentPackagesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ShipmentPackages]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPShipmentPackageDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allShipmentPackagesDto,
				RecordCount = allShipmentPackagesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShipmentPackageDto>> Process_GetShipmentPackage(Guid shipmentPackageId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPShipmentPackageDto shipmentPackageDto = null;
		ERPResponseMessageDto<ERPShipmentPackageDto> result;
		try
		{
			IERPShipmentPackageRepository iERPShipmentPackageRepository = (base.ERPShipmentPackageRepository = new ERPShipmentPackageRepository(base.ApiClientContext));
			using (iERPShipmentPackageRepository)
			{
				ERPShipmentPackageInformationDto eRPShipmentPackageInformationDto = await base.ERPShipmentPackageRepository.GetShipmentPackage(shipmentPackageId);
				shipmentPackageDto = new ERPShipmentPackageDto
				{
					spaCarrier = eRPShipmentPackageInformationDto.spaCarrier,
					spaCreatedBy = eRPShipmentPackageInformationDto.spaCreatedBy,
					spaCreatedDate = eRPShipmentPackageInformationDto.spaCreatedDate,
					spaCustomerPackageID = eRPShipmentPackageInformationDto.spaCustomerPackageID,
					spaEdi856CustomLabel = eRPShipmentPackageInformationDto.spaEdi856CustomLabel,
					spaUniqueID = eRPShipmentPackageInformationDto.spaUniqueID,
					spaFedExPackageTypes = eRPShipmentPackageInformationDto.spaFedExPackageTypes,
					spaAdditionalHandlingRequired = eRPShipmentPackageInformationDto.spaAdditionalHandlingRequired,
					spaLargePackage = eRPShipmentPackageInformationDto.spaLargePackage,
					spaVerbalConfirmationRequired = eRPShipmentPackageInformationDto.spaVerbalConfirmationRequired,
					spaLabelFilePath = eRPShipmentPackageInformationDto.spaLabelFilePath,
					spaPackageDimensionsUom = eRPShipmentPackageInformationDto.spaPackageDimensionsUom,
					spaPackageHeight = eRPShipmentPackageInformationDto.spaPackageHeight,
					spaPackageLength = eRPShipmentPackageInformationDto.spaPackageLength,
					spaPackageRate = eRPShipmentPackageInformationDto.spaPackageRate,
					spaPackageRateForeign = eRPShipmentPackageInformationDto.spaPackageRateForeign,
					spaPackageValue = eRPShipmentPackageInformationDto.spaPackageValue,
					spaPackageValueForeign = eRPShipmentPackageInformationDto.spaPackageValueForeign,
					spaPackageWeight = eRPShipmentPackageInformationDto.spaPackageWeight,
					spaPackageWeightUom = eRPShipmentPackageInformationDto.spaPackageWeightUom,
					spaPackageWidth = eRPShipmentPackageInformationDto.spaPackageWidth,
					spaReference1 = eRPShipmentPackageInformationDto.spaReference1,
					spaReference2 = eRPShipmentPackageInformationDto.spaReference2,
					spaRowVersion = eRPShipmentPackageInformationDto.SPArowVersion,
					spaShipmentPackageID = eRPShipmentPackageInformationDto.spaShipmentPackageID,
					spaShipmentID = eRPShipmentPackageInformationDto.spaShipmentID,
					spaShipmentIDNumber = eRPShipmentPackageInformationDto.spaShipmentIDNumber,
					spaShippingMethodID = eRPShipmentPackageInformationDto.spaShippingMethodID,
					spaTrackingNo = eRPShipmentPackageInformationDto.spaTrackingNo,
					spaUpsPackageTypes = eRPShipmentPackageInformationDto.spaUpsPackageTypes,
					CustomFields = eRPShipmentPackageInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ShipmentPackages []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentPackageDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = shipmentPackageDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShipmentPackageDto>> Process_PutShipmentPackage(ERPShipmentPackageDto shipmentPackage)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPShipmentPackageDto createdObject = null;
		ERPResponseMessageDto<ERPShipmentPackageDto> result;
		try
		{
			IERPShipmentPackageRepository iERPShipmentPackageRepository = (base.ERPShipmentPackageRepository = new ERPShipmentPackageRepository(base.ApiClientContext));
			using (iERPShipmentPackageRepository)
			{
				APIValidationInfoDto postResult = await base.ERPShipmentPackageRepository.SaveShipmentPackage(shipmentPackage);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPShipmentPackageInformationDto eRPShipmentPackageInformationDto = await base.ERPShipmentPackageRepository.GetShipmentPackage(shipmentPackage.spaUniqueID);
					createdObject = new ERPShipmentPackageDto
					{
						spaCarrier = eRPShipmentPackageInformationDto.spaCarrier,
						spaCreatedBy = eRPShipmentPackageInformationDto.spaCreatedBy,
						spaCreatedDate = eRPShipmentPackageInformationDto.spaCreatedDate,
						spaCustomerPackageID = eRPShipmentPackageInformationDto.spaCustomerPackageID,
						spaEdi856CustomLabel = eRPShipmentPackageInformationDto.spaEdi856CustomLabel,
						spaUniqueID = eRPShipmentPackageInformationDto.spaUniqueID,
						spaFedExPackageTypes = eRPShipmentPackageInformationDto.spaFedExPackageTypes,
						spaAdditionalHandlingRequired = eRPShipmentPackageInformationDto.spaAdditionalHandlingRequired,
						spaLargePackage = eRPShipmentPackageInformationDto.spaLargePackage,
						spaVerbalConfirmationRequired = eRPShipmentPackageInformationDto.spaVerbalConfirmationRequired,
						spaLabelFilePath = eRPShipmentPackageInformationDto.spaLabelFilePath,
						spaPackageDimensionsUom = eRPShipmentPackageInformationDto.spaPackageDimensionsUom,
						spaPackageHeight = eRPShipmentPackageInformationDto.spaPackageHeight,
						spaPackageLength = eRPShipmentPackageInformationDto.spaPackageLength,
						spaPackageRate = eRPShipmentPackageInformationDto.spaPackageRate,
						spaPackageRateForeign = eRPShipmentPackageInformationDto.spaPackageRateForeign,
						spaPackageValue = eRPShipmentPackageInformationDto.spaPackageValue,
						spaPackageValueForeign = eRPShipmentPackageInformationDto.spaPackageValueForeign,
						spaPackageWeight = eRPShipmentPackageInformationDto.spaPackageWeight,
						spaPackageWeightUom = eRPShipmentPackageInformationDto.spaPackageWeightUom,
						spaPackageWidth = eRPShipmentPackageInformationDto.spaPackageWidth,
						spaReference1 = eRPShipmentPackageInformationDto.spaReference1,
						spaReference2 = eRPShipmentPackageInformationDto.spaReference2,
						spaRowVersion = eRPShipmentPackageInformationDto.SPArowVersion,
						spaShipmentPackageID = eRPShipmentPackageInformationDto.spaShipmentPackageID,
						spaShipmentID = eRPShipmentPackageInformationDto.spaShipmentID,
						spaShipmentIDNumber = eRPShipmentPackageInformationDto.spaShipmentIDNumber,
						spaShippingMethodID = eRPShipmentPackageInformationDto.spaShippingMethodID,
						spaTrackingNo = eRPShipmentPackageInformationDto.spaTrackingNo,
						spaUpsPackageTypes = eRPShipmentPackageInformationDto.spaUpsPackageTypes,
						CustomFields = eRPShipmentPackageInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ShipmentPackage [{shipmentPackage.spaUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentPackageDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteShipmentPackage(Guid shipmentPackageId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentPackageRepository iERPShipmentPackageRepository = (base.ERPShipmentPackageRepository = new ERPShipmentPackageRepository(base.ApiClientContext));
		using (iERPShipmentPackageRepository)
		{
			if (!(await base.ERPShipmentPackageRepository.DoesShipmentPackageExist(shipmentPackageId)))
			{
				base.ErrorsList.Add($"ShipmentPackage [{shipmentPackageId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPShipmentPackageInformationDto eRPShipmentPackageInformationDto = await base.ERPShipmentPackageRepository.GetShipmentPackage(shipmentPackageId);
				string text = await base.ERPShipmentPackageRepository.WhereUsed("ShipmentPackages", new object[2] { eRPShipmentPackageInformationDto.spaShipmentID, eRPShipmentPackageInformationDto.spaShipmentPackageID }, new object[2] { "spaShipmentID", "spaShipmentPackageID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ShipmentPackage cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPShipmentPackageDto>> Process_DeleteShipmentPackage(Guid shipmentPackageId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPShipmentPackageDto> result;
		try
		{
			IERPShipmentPackageRepository iERPShipmentPackageRepository = (base.ERPShipmentPackageRepository = new ERPShipmentPackageRepository(base.ApiClientContext));
			using (iERPShipmentPackageRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPShipmentPackageRepository.DeleteRowFromTable("ShipmentPackages", "spa", shipmentPackageId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ShipmentPackage [{shipmentPackageId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentPackageDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPShipmentPackageDto()
			};
		}
		return result;
	}
}
