using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSerialNumberModel : ERPBaseModel, IERPSerialNumberModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSerialNumbers(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSerialNumberRepository iERPSerialNumberRepository = (base.ERPSerialNumberRepository = new ERPSerialNumberRepository(base.ApiClientContext));
		using (iERPSerialNumberRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSerialNumberRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSerialNumberRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSerialNumberRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSerialNumberRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSerialNumber(Guid serialNumberId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSerialNumberRepository iERPSerialNumberRepository = (base.ERPSerialNumberRepository = new ERPSerialNumberRepository(base.ApiClientContext));
		using (iERPSerialNumberRepository)
		{
			if (!(await base.ERPSerialNumberRepository.DoesSerialNumberExist(serialNumberId)))
			{
				errorsList.Add($"SerialNumber [{serialNumberId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutSerialNumber(ERPSerialNumberDto serialNumber)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSerialNumberRepository iERPSerialNumberRepository = (base.ERPSerialNumberRepository = new ERPSerialNumberRepository(base.ApiClientContext));
		using (iERPSerialNumberRepository)
		{
			if (!string.IsNullOrWhiteSpace(serialNumber.imsPartID) && !(await base.ERPSerialNumberRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { serialNumber.imsPartID })))
			{
				errorsList.Add("imsPartID [" + serialNumber.imsPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumber.imsPartRevisionID) && !(await base.ERPSerialNumberRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { serialNumber.imsPartID, serialNumber.imsPartRevisionID })))
			{
				errorsList.Add("imsPartRevisionID [" + serialNumber.imsPartRevisionID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSerialNumberDto>>> Process_GetAllSerialNumbers(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSerialNumberDto> allSerialNumbersDto = new List<ERPSerialNumberDto>();
		ERPResponseMessageDto<IList<ERPSerialNumberDto>> result;
		try
		{
			IERPSerialNumberRepository iERPSerialNumberRepository = (base.ERPSerialNumberRepository = new ERPSerialNumberRepository(base.ApiClientContext));
			using (iERPSerialNumberRepository)
			{
				foreach (ERPSerialNumberInformationDto item2 in await base.ERPSerialNumberRepository.GetAllSerialNumbers(pageSize, pageNumber, filter, orderBy))
				{
					ERPSerialNumberDto item = new ERPSerialNumberDto
					{
						imsAddedByUserID = item2.imsAddedByUserID,
						imsAddedDate = item2.imsAddedDate,
						imsSerialNumberID = item2.imsSerialNumberID,
						imsCreatedBy = item2.imsCreatedBy,
						imsCreatedDate = item2.imsCreatedDate,
						imsUniqueID = item2.imsUniqueID,
						imsExpirationDate = item2.imsExpirationDate,
						imsInactiveDate = item2.imsInactiveDate,
						imsInactive = item2.imsInactive,
						imsPartID = item2.imsPartID,
						imsPartRevisionID = item2.imsPartRevisionID,
						imsRowVersion = item2.imsRowVersion,
						CustomFields = item2.CustomFields
					};
					allSerialNumbersDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SerialNumbers]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSerialNumberDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSerialNumbersDto,
				RecordCount = allSerialNumbersDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSerialNumberDto>> Process_GetSerialNumber(Guid serialNumberId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSerialNumberDto serialNumberDto = null;
		ERPResponseMessageDto<ERPSerialNumberDto> result;
		try
		{
			IERPSerialNumberRepository iERPSerialNumberRepository = (base.ERPSerialNumberRepository = new ERPSerialNumberRepository(base.ApiClientContext));
			using (iERPSerialNumberRepository)
			{
				ERPSerialNumberInformationDto eRPSerialNumberInformationDto = await base.ERPSerialNumberRepository.GetSerialNumber(serialNumberId);
				serialNumberDto = new ERPSerialNumberDto
				{
					imsAddedByUserID = eRPSerialNumberInformationDto.imsAddedByUserID,
					imsAddedDate = eRPSerialNumberInformationDto.imsAddedDate,
					imsSerialNumberID = eRPSerialNumberInformationDto.imsSerialNumberID,
					imsCreatedBy = eRPSerialNumberInformationDto.imsCreatedBy,
					imsCreatedDate = eRPSerialNumberInformationDto.imsCreatedDate,
					imsUniqueID = eRPSerialNumberInformationDto.imsUniqueID,
					imsExpirationDate = eRPSerialNumberInformationDto.imsExpirationDate,
					imsInactiveDate = eRPSerialNumberInformationDto.imsInactiveDate,
					imsInactive = eRPSerialNumberInformationDto.imsInactive,
					imsPartID = eRPSerialNumberInformationDto.imsPartID,
					imsPartRevisionID = eRPSerialNumberInformationDto.imsPartRevisionID,
					imsRowVersion = eRPSerialNumberInformationDto.imsRowVersion,
					CustomFields = eRPSerialNumberInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SerialNumbers []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSerialNumberDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = serialNumberDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSerialNumberDto>> Process_PutSerialNumber(ERPSerialNumberDto serialNumber)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPSerialNumberDto createdObject = null;
		ERPResponseMessageDto<ERPSerialNumberDto> result;
		try
		{
			IERPSerialNumberRepository iERPSerialNumberRepository = (base.ERPSerialNumberRepository = new ERPSerialNumberRepository(base.ApiClientContext));
			using (iERPSerialNumberRepository)
			{
				APIValidationInfoDto postResult = await base.ERPSerialNumberRepository.SaveSerialNumber(serialNumber);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPSerialNumberInformationDto eRPSerialNumberInformationDto = await base.ERPSerialNumberRepository.GetSerialNumber(serialNumber.imsUniqueID);
					createdObject = new ERPSerialNumberDto
					{
						imsAddedByUserID = eRPSerialNumberInformationDto.imsAddedByUserID,
						imsAddedDate = eRPSerialNumberInformationDto.imsAddedDate,
						imsSerialNumberID = eRPSerialNumberInformationDto.imsSerialNumberID,
						imsCreatedBy = eRPSerialNumberInformationDto.imsCreatedBy,
						imsCreatedDate = eRPSerialNumberInformationDto.imsCreatedDate,
						imsUniqueID = eRPSerialNumberInformationDto.imsUniqueID,
						imsExpirationDate = eRPSerialNumberInformationDto.imsExpirationDate,
						imsInactiveDate = eRPSerialNumberInformationDto.imsInactiveDate,
						imsInactive = eRPSerialNumberInformationDto.imsInactive,
						imsPartID = eRPSerialNumberInformationDto.imsPartID,
						imsPartRevisionID = eRPSerialNumberInformationDto.imsPartRevisionID,
						imsRowVersion = eRPSerialNumberInformationDto.imsRowVersion,
						CustomFields = eRPSerialNumberInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing SerialNumber [{serialNumber.imsUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSerialNumberDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteSerialNumber(Guid serialNumberId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSerialNumberRepository iERPSerialNumberRepository = (base.ERPSerialNumberRepository = new ERPSerialNumberRepository(base.ApiClientContext));
		using (iERPSerialNumberRepository)
		{
			if (!(await base.ERPSerialNumberRepository.DoesSerialNumberExist(serialNumberId)))
			{
				base.ErrorsList.Add($"SerialNumber [{serialNumberId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPSerialNumberInformationDto eRPSerialNumberInformationDto = await base.ERPSerialNumberRepository.GetSerialNumber(serialNumberId);
				string text = await base.ERPSerialNumberRepository.WhereUsed("SerialNumbers", new object[3] { eRPSerialNumberInformationDto.imsPartID, eRPSerialNumberInformationDto.imsPartRevisionID, eRPSerialNumberInformationDto.imsSerialNumberID }, new object[3] { "imsPartID", "imsPartRevisionID", "imsSerialNumberID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("SerialNumber cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPSerialNumberDto>> Process_DeleteSerialNumber(Guid serialNumberId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPSerialNumberDto> result;
		try
		{
			IERPSerialNumberRepository iERPSerialNumberRepository = (base.ERPSerialNumberRepository = new ERPSerialNumberRepository(base.ApiClientContext));
			using (iERPSerialNumberRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPSerialNumberRepository.DeleteRowFromTable("SerialNumbers", "ims", serialNumberId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of SerialNumber [{serialNumberId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSerialNumberDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPSerialNumberDto()
			};
		}
		return result;
	}
}
