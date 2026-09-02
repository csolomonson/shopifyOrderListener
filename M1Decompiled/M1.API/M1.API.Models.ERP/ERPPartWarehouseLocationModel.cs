using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartWarehouseLocationModel : ERPBaseModel, IERPPartWarehouseLocationModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartWarehouseLocations(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartWarehouseLocationRepository iERPPartWarehouseLocationRepository = (base.ERPPartWarehouseLocationRepository = new ERPPartWarehouseLocationRepository(base.ApiClientContext));
		using (iERPPartWarehouseLocationRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartWarehouseLocationRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartWarehouseLocationRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartWarehouseLocationRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartWarehouseLocationRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartWarehouseLocation(Guid partWarehouseLocationId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartWarehouseLocationRepository iERPPartWarehouseLocationRepository = (base.ERPPartWarehouseLocationRepository = new ERPPartWarehouseLocationRepository(base.ApiClientContext));
		using (iERPPartWarehouseLocationRepository)
		{
			if (!(await base.ERPPartWarehouseLocationRepository.DoesPartWarehouseLocationExist(partWarehouseLocationId)))
			{
				errorsList.Add($"PartWarehouseLocation [{partWarehouseLocationId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartWarehouseLocation(ERPPartWarehouseLocationDto partWarehouseLocation)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartWarehouseLocationRepository iERPPartWarehouseLocationRepository = (base.ERPPartWarehouseLocationRepository = new ERPPartWarehouseLocationRepository(base.ApiClientContext));
		using (iERPPartWarehouseLocationRepository)
		{
			if (!string.IsNullOrWhiteSpace(partWarehouseLocation.imlPartID) && !(await base.ERPPartWarehouseLocationRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partWarehouseLocation.imlPartID })))
			{
				errorsList.Add("imlPartID [" + partWarehouseLocation.imlPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partWarehouseLocation.imlPartRevisionID) && !(await base.ERPPartWarehouseLocationRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partWarehouseLocation.imlPartID, partWarehouseLocation.imlPartRevisionID })))
			{
				errorsList.Add("imlPartRevisionID [" + partWarehouseLocation.imlPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partWarehouseLocation.imlPartWarehouseID) && !(await base.ERPPartWarehouseLocationRepository.DoesRecordExistInTableUsingKeys("Warehouses", new object[1] { "IMWWAREHOUSEID" }, new object[1] { partWarehouseLocation.imlPartWarehouseID })))
			{
				errorsList.Add("imlPartWarehouseID [" + partWarehouseLocation.imlPartWarehouseID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartWarehouseLocationDto>>> Process_GetAllPartWarehouseLocations(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartWarehouseLocationDto> allPartWarehouseLocationsDto = new List<ERPPartWarehouseLocationDto>();
		ERPResponseMessageDto<IList<ERPPartWarehouseLocationDto>> result;
		try
		{
			IERPPartWarehouseLocationRepository iERPPartWarehouseLocationRepository = (base.ERPPartWarehouseLocationRepository = new ERPPartWarehouseLocationRepository(base.ApiClientContext));
			using (iERPPartWarehouseLocationRepository)
			{
				foreach (ERPPartWarehouseLocationInformationDto item2 in await base.ERPPartWarehouseLocationRepository.GetAllPartWarehouseLocations(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartWarehouseLocationDto item = new ERPPartWarehouseLocationDto
					{
						imlCreatedBy = item2.imlCreatedBy,
						imlCreatedDate = item2.imlCreatedDate,
						imlUniqueID = item2.imlUniqueID,
						imlNonNettable = item2.imlNonNettable,
						imLLastRunDatePurchasePlanner = item2.imLLastRunDatePurchasePlanner,
						imlMaximumQuantity = item2.imlMaximumQuantity,
						imlMinimumQuantity = item2.imlMinimumQuantity,
						imlPartID = item2.imlPartID,
						imlPartRevisionID = item2.imlPartRevisionID,
						imlPartWarehouseID = item2.imlPartWarehouseID,
						imlQuantityInTransit = item2.imlQuantityInTransit,
						imlRowVersion = item2.imlRowVersion,
						CustomFields = item2.CustomFields
					};
					allPartWarehouseLocationsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartWarehouseLocations]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartWarehouseLocationDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartWarehouseLocationsDto,
				RecordCount = allPartWarehouseLocationsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartWarehouseLocationDto>> Process_GetPartWarehouseLocation(Guid partWarehouseLocationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartWarehouseLocationDto partWarehouseLocationDto = null;
		ERPResponseMessageDto<ERPPartWarehouseLocationDto> result;
		try
		{
			IERPPartWarehouseLocationRepository iERPPartWarehouseLocationRepository = (base.ERPPartWarehouseLocationRepository = new ERPPartWarehouseLocationRepository(base.ApiClientContext));
			using (iERPPartWarehouseLocationRepository)
			{
				ERPPartWarehouseLocationInformationDto eRPPartWarehouseLocationInformationDto = await base.ERPPartWarehouseLocationRepository.GetPartWarehouseLocation(partWarehouseLocationId);
				partWarehouseLocationDto = new ERPPartWarehouseLocationDto
				{
					imlCreatedBy = eRPPartWarehouseLocationInformationDto.imlCreatedBy,
					imlCreatedDate = eRPPartWarehouseLocationInformationDto.imlCreatedDate,
					imlUniqueID = eRPPartWarehouseLocationInformationDto.imlUniqueID,
					imlNonNettable = eRPPartWarehouseLocationInformationDto.imlNonNettable,
					imLLastRunDatePurchasePlanner = eRPPartWarehouseLocationInformationDto.imLLastRunDatePurchasePlanner,
					imlMaximumQuantity = eRPPartWarehouseLocationInformationDto.imlMaximumQuantity,
					imlMinimumQuantity = eRPPartWarehouseLocationInformationDto.imlMinimumQuantity,
					imlPartID = eRPPartWarehouseLocationInformationDto.imlPartID,
					imlPartRevisionID = eRPPartWarehouseLocationInformationDto.imlPartRevisionID,
					imlPartWarehouseID = eRPPartWarehouseLocationInformationDto.imlPartWarehouseID,
					imlQuantityInTransit = eRPPartWarehouseLocationInformationDto.imlQuantityInTransit,
					imlRowVersion = eRPPartWarehouseLocationInformationDto.imlRowVersion,
					CustomFields = eRPPartWarehouseLocationInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartWarehouseLocations []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartWarehouseLocationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partWarehouseLocationDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartWarehouseLocationDto>> Process_PutPartWarehouseLocation(ERPPartWarehouseLocationDto partWarehouseLocation)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartWarehouseLocationDto createdObject = null;
		ERPResponseMessageDto<ERPPartWarehouseLocationDto> result;
		try
		{
			IERPPartWarehouseLocationRepository iERPPartWarehouseLocationRepository = (base.ERPPartWarehouseLocationRepository = new ERPPartWarehouseLocationRepository(base.ApiClientContext));
			using (iERPPartWarehouseLocationRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartWarehouseLocationRepository.SavePartWarehouseLocation(partWarehouseLocation);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartWarehouseLocationInformationDto eRPPartWarehouseLocationInformationDto = await base.ERPPartWarehouseLocationRepository.GetPartWarehouseLocation(partWarehouseLocation.imlUniqueID);
					createdObject = new ERPPartWarehouseLocationDto
					{
						imlCreatedBy = eRPPartWarehouseLocationInformationDto.imlCreatedBy,
						imlCreatedDate = eRPPartWarehouseLocationInformationDto.imlCreatedDate,
						imlUniqueID = eRPPartWarehouseLocationInformationDto.imlUniqueID,
						imlNonNettable = eRPPartWarehouseLocationInformationDto.imlNonNettable,
						imLLastRunDatePurchasePlanner = eRPPartWarehouseLocationInformationDto.imLLastRunDatePurchasePlanner,
						imlMaximumQuantity = eRPPartWarehouseLocationInformationDto.imlMaximumQuantity,
						imlMinimumQuantity = eRPPartWarehouseLocationInformationDto.imlMinimumQuantity,
						imlPartID = eRPPartWarehouseLocationInformationDto.imlPartID,
						imlPartRevisionID = eRPPartWarehouseLocationInformationDto.imlPartRevisionID,
						imlPartWarehouseID = eRPPartWarehouseLocationInformationDto.imlPartWarehouseID,
						imlQuantityInTransit = eRPPartWarehouseLocationInformationDto.imlQuantityInTransit,
						imlRowVersion = eRPPartWarehouseLocationInformationDto.imlRowVersion,
						CustomFields = eRPPartWarehouseLocationInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartWarehouseLocation [{partWarehouseLocation.imlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartWarehouseLocationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartWarehouseLocation(Guid partWarehouseLocationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartWarehouseLocationRepository iERPPartWarehouseLocationRepository = (base.ERPPartWarehouseLocationRepository = new ERPPartWarehouseLocationRepository(base.ApiClientContext));
		using (iERPPartWarehouseLocationRepository)
		{
			if (!(await base.ERPPartWarehouseLocationRepository.DoesPartWarehouseLocationExist(partWarehouseLocationId)))
			{
				base.ErrorsList.Add($"PartWarehouseLocation [{partWarehouseLocationId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartWarehouseLocationInformationDto eRPPartWarehouseLocationInformationDto = await base.ERPPartWarehouseLocationRepository.GetPartWarehouseLocation(partWarehouseLocationId);
				string text = await base.ERPPartWarehouseLocationRepository.WhereUsed("PartWarehouseLocations", new object[3] { eRPPartWarehouseLocationInformationDto.imlPartID, eRPPartWarehouseLocationInformationDto.imlPartRevisionID, eRPPartWarehouseLocationInformationDto.imlPartWarehouseID }, new object[3] { "imlPartID", "imlPartRevisionID", "imlPartWarehouseID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartWarehouseLocation cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartWarehouseLocationDto>> Process_DeletePartWarehouseLocation(Guid partWarehouseLocationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartWarehouseLocationDto> result;
		try
		{
			IERPPartWarehouseLocationRepository iERPPartWarehouseLocationRepository = (base.ERPPartWarehouseLocationRepository = new ERPPartWarehouseLocationRepository(base.ApiClientContext));
			using (iERPPartWarehouseLocationRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartWarehouseLocationRepository.DeleteRowFromTable("PartWarehouseLocations", "iml", partWarehouseLocationId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartWarehouseLocation [{partWarehouseLocationId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartWarehouseLocationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartWarehouseLocationDto()
			};
		}
		return result;
	}
}
