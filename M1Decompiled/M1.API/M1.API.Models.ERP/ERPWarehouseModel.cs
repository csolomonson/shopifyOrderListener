using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWarehouseModel : ERPBaseModel, IERPWarehouseModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouses(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWarehouseRepository iERPWarehouseRepository = (base.ERPWarehouseRepository = new ERPWarehouseRepository(base.ApiClientContext));
		using (iERPWarehouseRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWarehouseRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWarehouseRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWarehouseRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWarehouseRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWarehouse(Guid warehouseId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseRepository iERPWarehouseRepository = (base.ERPWarehouseRepository = new ERPWarehouseRepository(base.ApiClientContext));
		using (iERPWarehouseRepository)
		{
			if (!(await base.ERPWarehouseRepository.DoesWarehouseExist(warehouseId)))
			{
				errorsList.Add($"Warehouse [{warehouseId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutWarehouse(ERPWarehouseDto warehouse)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseRepository iERPWarehouseRepository = (base.ERPWarehouseRepository = new ERPWarehouseRepository(base.ApiClientContext));
		using (iERPWarehouseRepository)
		{
			if (!string.IsNullOrWhiteSpace(warehouse.imwPlantDepartmentID) && !(await base.ERPWarehouseRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { warehouse.imwPlantID, warehouse.imwPlantDepartmentID })))
			{
				errorsList.Add("imwPlantDepartmentID [" + warehouse.imwPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouse.imwPlantID) && !(await base.ERPWarehouseRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { warehouse.imwPlantID })))
			{
				errorsList.Add("imwPlantID [" + warehouse.imwPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouse.imwShippingMethodID) && !(await base.ERPWarehouseRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { warehouse.imwShippingMethodID })))
			{
				errorsList.Add("imwShippingMethodID [" + warehouse.imwShippingMethodID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWarehouseDto>>> Process_GetAllWarehouses(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWarehouseDto> allWarehousesDto = new List<ERPWarehouseDto>();
		ERPResponseMessageDto<IList<ERPWarehouseDto>> result;
		try
		{
			IERPWarehouseRepository iERPWarehouseRepository = (base.ERPWarehouseRepository = new ERPWarehouseRepository(base.ApiClientContext));
			using (iERPWarehouseRepository)
			{
				foreach (ERPWarehouseInformationDto item2 in await base.ERPWarehouseRepository.GetAllWarehouses(pageSize, pageNumber, filter, orderBy))
				{
					ERPWarehouseDto item = new ERPWarehouseDto
					{
						imwAddressLine1 = item2.imwAddressLine1,
						imwAddressLine2 = item2.imwAddressLine2,
						imwAddressLine3 = item2.imwAddressLine3,
						imwCity = item2.imwCity,
						imwWarehouseID = item2.imwWarehouseID,
						imwCountry = item2.imwCountry,
						imwCreatedBy = item2.imwCreatedBy,
						imwCreatedDate = item2.imwCreatedDate,
						imwDefaultBinCount = item2.imwDefaultBinCount,
						imwEmailAddress = item2.imwEmailAddress,
						imwUniqueID = item2.imwUniqueID,
						imwEstablishedDate = item2.imwEstablishedDate,
						imwFaxNumber = item2.imwFaxNumber,
						imwInactiveDate = item2.imwInactiveDate,
						imwInactive = item2.imwInactive,
						imwAvalaraAddressValidated = item2.imwAvalaraAddressValidated,
						imwDefaultWarehouse = item2.imwDefaultWarehouse,
						imwDoNotIncludeInJobCosts = item2.imwDoNotIncludeInJobCosts,
						imwNonNettable = item2.imwNonNettable,
						imwName = item2.imwName,
						imwNonNettableType = item2.imwNonNettableType,
						imwPhoneNumber = item2.imwPhoneNumber,
						imwPlantDepartmentID = item2.imwPlantDepartmentID,
						imwPlantID = item2.imwPlantID,
						imwPostCode = item2.imwPostCode,
						imwRowVersion = item2.imwRowVersion,
						imwShippingMethodID = item2.imwShippingMethodID,
						imwState = item2.imwState,
						CustomFields = item2.CustomFields
					};
					allWarehousesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Warehouses]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWarehouseDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWarehousesDto,
				RecordCount = allWarehousesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseDto>> Process_GetWarehouse(Guid warehouseId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWarehouseDto warehouseDto = null;
		ERPResponseMessageDto<ERPWarehouseDto> result;
		try
		{
			IERPWarehouseRepository iERPWarehouseRepository = (base.ERPWarehouseRepository = new ERPWarehouseRepository(base.ApiClientContext));
			using (iERPWarehouseRepository)
			{
				ERPWarehouseInformationDto eRPWarehouseInformationDto = await base.ERPWarehouseRepository.GetWarehouse(warehouseId);
				warehouseDto = new ERPWarehouseDto
				{
					imwAddressLine1 = eRPWarehouseInformationDto.imwAddressLine1,
					imwAddressLine2 = eRPWarehouseInformationDto.imwAddressLine2,
					imwAddressLine3 = eRPWarehouseInformationDto.imwAddressLine3,
					imwCity = eRPWarehouseInformationDto.imwCity,
					imwWarehouseID = eRPWarehouseInformationDto.imwWarehouseID,
					imwCountry = eRPWarehouseInformationDto.imwCountry,
					imwCreatedBy = eRPWarehouseInformationDto.imwCreatedBy,
					imwCreatedDate = eRPWarehouseInformationDto.imwCreatedDate,
					imwDefaultBinCount = eRPWarehouseInformationDto.imwDefaultBinCount,
					imwEmailAddress = eRPWarehouseInformationDto.imwEmailAddress,
					imwUniqueID = eRPWarehouseInformationDto.imwUniqueID,
					imwEstablishedDate = eRPWarehouseInformationDto.imwEstablishedDate,
					imwFaxNumber = eRPWarehouseInformationDto.imwFaxNumber,
					imwInactiveDate = eRPWarehouseInformationDto.imwInactiveDate,
					imwInactive = eRPWarehouseInformationDto.imwInactive,
					imwAvalaraAddressValidated = eRPWarehouseInformationDto.imwAvalaraAddressValidated,
					imwDefaultWarehouse = eRPWarehouseInformationDto.imwDefaultWarehouse,
					imwDoNotIncludeInJobCosts = eRPWarehouseInformationDto.imwDoNotIncludeInJobCosts,
					imwNonNettable = eRPWarehouseInformationDto.imwNonNettable,
					imwName = eRPWarehouseInformationDto.imwName,
					imwNonNettableType = eRPWarehouseInformationDto.imwNonNettableType,
					imwPhoneNumber = eRPWarehouseInformationDto.imwPhoneNumber,
					imwPlantDepartmentID = eRPWarehouseInformationDto.imwPlantDepartmentID,
					imwPlantID = eRPWarehouseInformationDto.imwPlantID,
					imwPostCode = eRPWarehouseInformationDto.imwPostCode,
					imwRowVersion = eRPWarehouseInformationDto.imwRowVersion,
					imwShippingMethodID = eRPWarehouseInformationDto.imwShippingMethodID,
					imwState = eRPWarehouseInformationDto.imwState,
					CustomFields = eRPWarehouseInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Warehouses []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = warehouseDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseDto>> Process_PutWarehouse(ERPWarehouseDto warehouse)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPWarehouseDto createdObject = null;
		ERPResponseMessageDto<ERPWarehouseDto> result;
		try
		{
			IERPWarehouseRepository iERPWarehouseRepository = (base.ERPWarehouseRepository = new ERPWarehouseRepository(base.ApiClientContext));
			using (iERPWarehouseRepository)
			{
				APIValidationInfoDto postResult = await base.ERPWarehouseRepository.SaveWarehouse(warehouse);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPWarehouseInformationDto eRPWarehouseInformationDto = await base.ERPWarehouseRepository.GetWarehouse(warehouse.imwUniqueID);
					createdObject = new ERPWarehouseDto
					{
						imwAddressLine1 = eRPWarehouseInformationDto.imwAddressLine1,
						imwAddressLine2 = eRPWarehouseInformationDto.imwAddressLine2,
						imwAddressLine3 = eRPWarehouseInformationDto.imwAddressLine3,
						imwCity = eRPWarehouseInformationDto.imwCity,
						imwWarehouseID = eRPWarehouseInformationDto.imwWarehouseID,
						imwCountry = eRPWarehouseInformationDto.imwCountry,
						imwCreatedBy = eRPWarehouseInformationDto.imwCreatedBy,
						imwCreatedDate = eRPWarehouseInformationDto.imwCreatedDate,
						imwDefaultBinCount = eRPWarehouseInformationDto.imwDefaultBinCount,
						imwEmailAddress = eRPWarehouseInformationDto.imwEmailAddress,
						imwUniqueID = eRPWarehouseInformationDto.imwUniqueID,
						imwEstablishedDate = eRPWarehouseInformationDto.imwEstablishedDate,
						imwFaxNumber = eRPWarehouseInformationDto.imwFaxNumber,
						imwInactiveDate = eRPWarehouseInformationDto.imwInactiveDate,
						imwInactive = eRPWarehouseInformationDto.imwInactive,
						imwAvalaraAddressValidated = eRPWarehouseInformationDto.imwAvalaraAddressValidated,
						imwDefaultWarehouse = eRPWarehouseInformationDto.imwDefaultWarehouse,
						imwDoNotIncludeInJobCosts = eRPWarehouseInformationDto.imwDoNotIncludeInJobCosts,
						imwNonNettable = eRPWarehouseInformationDto.imwNonNettable,
						imwName = eRPWarehouseInformationDto.imwName,
						imwNonNettableType = eRPWarehouseInformationDto.imwNonNettableType,
						imwPhoneNumber = eRPWarehouseInformationDto.imwPhoneNumber,
						imwPlantDepartmentID = eRPWarehouseInformationDto.imwPlantDepartmentID,
						imwPlantID = eRPWarehouseInformationDto.imwPlantID,
						imwPostCode = eRPWarehouseInformationDto.imwPostCode,
						imwRowVersion = eRPWarehouseInformationDto.imwRowVersion,
						imwShippingMethodID = eRPWarehouseInformationDto.imwShippingMethodID,
						imwState = eRPWarehouseInformationDto.imwState,
						CustomFields = eRPWarehouseInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Warehouse [{warehouse.imwUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouse(Guid warehouseId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseRepository iERPWarehouseRepository = (base.ERPWarehouseRepository = new ERPWarehouseRepository(base.ApiClientContext));
		using (iERPWarehouseRepository)
		{
			if (!(await base.ERPWarehouseRepository.DoesWarehouseExist(warehouseId)))
			{
				base.ErrorsList.Add($"Warehouse [{warehouseId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPWarehouseInformationDto eRPWarehouseInformationDto = await base.ERPWarehouseRepository.GetWarehouse(warehouseId);
				string text = await base.ERPWarehouseRepository.WhereUsed("Warehouses", new object[1] { eRPWarehouseInformationDto.imwWarehouseID }, new object[1] { "imwWarehouseID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Warehouse cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseDto>> Process_DeleteWarehouse(Guid warehouseId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPWarehouseDto> result;
		try
		{
			IERPWarehouseRepository iERPWarehouseRepository = (base.ERPWarehouseRepository = new ERPWarehouseRepository(base.ApiClientContext));
			using (iERPWarehouseRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPWarehouseRepository.DeleteRowFromTable("Warehouses", "imw", warehouseId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Warehouse [{warehouseId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPWarehouseDto()
			};
		}
		return result;
	}
}
