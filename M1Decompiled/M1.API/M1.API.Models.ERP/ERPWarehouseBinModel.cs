using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWarehouseBinModel : ERPBaseModel, IERPWarehouseBinModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseBins(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWarehouseBinRepository iERPWarehouseBinRepository = (base.ERPWarehouseBinRepository = new ERPWarehouseBinRepository(base.ApiClientContext));
		using (iERPWarehouseBinRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWarehouseBinRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWarehouseBinRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWarehouseBinRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWarehouseBinRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWarehouseBin(Guid warehouseBinId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseBinRepository iERPWarehouseBinRepository = (base.ERPWarehouseBinRepository = new ERPWarehouseBinRepository(base.ApiClientContext));
		using (iERPWarehouseBinRepository)
		{
			if (!(await base.ERPWarehouseBinRepository.DoesWarehouseBinExist(warehouseBinId)))
			{
				errorsList.Add($"WarehouseBin [{warehouseBinId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutWarehouseBin(ERPWarehouseBinDto warehouseBin)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseBinRepository iERPWarehouseBinRepository = (base.ERPWarehouseBinRepository = new ERPWarehouseBinRepository(base.ApiClientContext));
		using (iERPWarehouseBinRepository)
		{
			if (!string.IsNullOrWhiteSpace(warehouseBin.inbWarehouseID) && !(await base.ERPWarehouseBinRepository.DoesRecordExistInTableUsingKeys("Warehouses", new object[1] { "IMWWAREHOUSEID" }, new object[1] { warehouseBin.inbWarehouseID })))
			{
				errorsList.Add("inbWarehouseID [" + warehouseBin.inbWarehouseID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWarehouseBinDto>>> Process_GetAllWarehouseBins(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWarehouseBinDto> allWarehouseBinsDto = new List<ERPWarehouseBinDto>();
		ERPResponseMessageDto<IList<ERPWarehouseBinDto>> result;
		try
		{
			IERPWarehouseBinRepository iERPWarehouseBinRepository = (base.ERPWarehouseBinRepository = new ERPWarehouseBinRepository(base.ApiClientContext));
			using (iERPWarehouseBinRepository)
			{
				foreach (ERPWarehouseBinInformationDto item2 in await base.ERPWarehouseBinRepository.GetAllWarehouseBins(pageSize, pageNumber, filter, orderBy))
				{
					ERPWarehouseBinDto item = new ERPWarehouseBinDto
					{
						inbWarehouseBinID = item2.inbWarehouseBinID,
						inbCreatedBy = item2.inbCreatedBy,
						inbCreatedDate = item2.inbCreatedDate,
						inbDescription = item2.inbDescription,
						inbUniqueID = item2.inbUniqueID,
						inbInactiveDate = item2.inbInactiveDate,
						inbInactive = item2.inbInactive,
						inbDefaultBin = item2.inbDefaultBin,
						inbHasQOHQTI = item2.inbHasQOHQTI,
						inbLongDescriptionRtf = item2.inbLongDescriptionRtf,
						inbLongDescriptionText = item2.inbLongDescriptionText,
						inbRowVersion = item2.inbRowVersion,
						inbWarehouseID = item2.inbWarehouseID,
						CustomFields = item2.CustomFields
					};
					allWarehouseBinsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all WarehouseBins]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWarehouseBinDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWarehouseBinsDto,
				RecordCount = allWarehouseBinsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseBinDto>> Process_GetWarehouseBin(Guid warehouseBinId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWarehouseBinDto warehouseBinDto = null;
		ERPResponseMessageDto<ERPWarehouseBinDto> result;
		try
		{
			IERPWarehouseBinRepository iERPWarehouseBinRepository = (base.ERPWarehouseBinRepository = new ERPWarehouseBinRepository(base.ApiClientContext));
			using (iERPWarehouseBinRepository)
			{
				ERPWarehouseBinInformationDto eRPWarehouseBinInformationDto = await base.ERPWarehouseBinRepository.GetWarehouseBin(warehouseBinId);
				warehouseBinDto = new ERPWarehouseBinDto
				{
					inbWarehouseBinID = eRPWarehouseBinInformationDto.inbWarehouseBinID,
					inbCreatedBy = eRPWarehouseBinInformationDto.inbCreatedBy,
					inbCreatedDate = eRPWarehouseBinInformationDto.inbCreatedDate,
					inbDescription = eRPWarehouseBinInformationDto.inbDescription,
					inbUniqueID = eRPWarehouseBinInformationDto.inbUniqueID,
					inbInactiveDate = eRPWarehouseBinInformationDto.inbInactiveDate,
					inbInactive = eRPWarehouseBinInformationDto.inbInactive,
					inbDefaultBin = eRPWarehouseBinInformationDto.inbDefaultBin,
					inbHasQOHQTI = eRPWarehouseBinInformationDto.inbHasQOHQTI,
					inbLongDescriptionRtf = eRPWarehouseBinInformationDto.inbLongDescriptionRtf,
					inbLongDescriptionText = eRPWarehouseBinInformationDto.inbLongDescriptionText,
					inbRowVersion = eRPWarehouseBinInformationDto.inbRowVersion,
					inbWarehouseID = eRPWarehouseBinInformationDto.inbWarehouseID,
					CustomFields = eRPWarehouseBinInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the WarehouseBins []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseBinDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = warehouseBinDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseBinDto>> Process_PutWarehouseBin(ERPWarehouseBinDto warehouseBin)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPWarehouseBinDto createdObject = null;
		ERPResponseMessageDto<ERPWarehouseBinDto> result;
		try
		{
			IERPWarehouseBinRepository iERPWarehouseBinRepository = (base.ERPWarehouseBinRepository = new ERPWarehouseBinRepository(base.ApiClientContext));
			using (iERPWarehouseBinRepository)
			{
				APIValidationInfoDto postResult = await base.ERPWarehouseBinRepository.SaveWarehouseBin(warehouseBin);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPWarehouseBinInformationDto eRPWarehouseBinInformationDto = await base.ERPWarehouseBinRepository.GetWarehouseBin(warehouseBin.inbUniqueID);
					createdObject = new ERPWarehouseBinDto
					{
						inbWarehouseBinID = eRPWarehouseBinInformationDto.inbWarehouseBinID,
						inbCreatedBy = eRPWarehouseBinInformationDto.inbCreatedBy,
						inbCreatedDate = eRPWarehouseBinInformationDto.inbCreatedDate,
						inbDescription = eRPWarehouseBinInformationDto.inbDescription,
						inbUniqueID = eRPWarehouseBinInformationDto.inbUniqueID,
						inbInactiveDate = eRPWarehouseBinInformationDto.inbInactiveDate,
						inbInactive = eRPWarehouseBinInformationDto.inbInactive,
						inbDefaultBin = eRPWarehouseBinInformationDto.inbDefaultBin,
						inbHasQOHQTI = eRPWarehouseBinInformationDto.inbHasQOHQTI,
						inbLongDescriptionRtf = eRPWarehouseBinInformationDto.inbLongDescriptionRtf,
						inbLongDescriptionText = eRPWarehouseBinInformationDto.inbLongDescriptionText,
						inbRowVersion = eRPWarehouseBinInformationDto.inbRowVersion,
						inbWarehouseID = eRPWarehouseBinInformationDto.inbWarehouseID,
						CustomFields = eRPWarehouseBinInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing WarehouseBin [{warehouseBin.inbUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseBinDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseBin(Guid warehouseBinId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseBinRepository iERPWarehouseBinRepository = (base.ERPWarehouseBinRepository = new ERPWarehouseBinRepository(base.ApiClientContext));
		using (iERPWarehouseBinRepository)
		{
			if (!(await base.ERPWarehouseBinRepository.DoesWarehouseBinExist(warehouseBinId)))
			{
				base.ErrorsList.Add($"WarehouseBin [{warehouseBinId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPWarehouseBinInformationDto eRPWarehouseBinInformationDto = await base.ERPWarehouseBinRepository.GetWarehouseBin(warehouseBinId);
				string text = await base.ERPWarehouseBinRepository.WhereUsed("WarehouseBins", new object[2] { eRPWarehouseBinInformationDto.inbWarehouseID, eRPWarehouseBinInformationDto.inbWarehouseBinID }, new object[2] { "inbWarehouseID", "inbWarehouseBinID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("WarehouseBin cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseBinDto>> Process_DeleteWarehouseBin(Guid warehouseBinId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPWarehouseBinDto> result;
		try
		{
			IERPWarehouseBinRepository iERPWarehouseBinRepository = (base.ERPWarehouseBinRepository = new ERPWarehouseBinRepository(base.ApiClientContext));
			using (iERPWarehouseBinRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPWarehouseBinRepository.DeleteRowFromTable("WarehouseBins", "inb", warehouseBinId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of WarehouseBin [{warehouseBinId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseBinDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPWarehouseBinDto()
			};
		}
		return result;
	}
}
