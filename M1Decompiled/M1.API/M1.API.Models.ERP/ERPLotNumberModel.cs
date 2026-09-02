using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPLotNumberModel : ERPBaseModel, IERPLotNumberModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllLotNumbers(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPLotNumberRepository iERPLotNumberRepository = (base.ERPLotNumberRepository = new ERPLotNumberRepository(base.ApiClientContext));
		using (iERPLotNumberRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPLotNumberRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPLotNumberRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPLotNumberRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPLotNumberRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetLotNumber(Guid lotNumberId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLotNumberRepository iERPLotNumberRepository = (base.ERPLotNumberRepository = new ERPLotNumberRepository(base.ApiClientContext));
		using (iERPLotNumberRepository)
		{
			if (!(await base.ERPLotNumberRepository.DoesLotNumberExist(lotNumberId)))
			{
				errorsList.Add($"LotNumber [{lotNumberId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutLotNumber(ERPLotNumberDto lotNumber)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLotNumberRepository iERPLotNumberRepository = (base.ERPLotNumberRepository = new ERPLotNumberRepository(base.ApiClientContext));
		using (iERPLotNumberRepository)
		{
			if (!string.IsNullOrWhiteSpace(lotNumber.ablPartID) && !(await base.ERPLotNumberRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { lotNumber.ablPartID })))
			{
				errorsList.Add("ablPartID [" + lotNumber.ablPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumber.ablPartRevisionID) && !(await base.ERPLotNumberRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { lotNumber.ablPartID, lotNumber.ablPartRevisionID })))
			{
				errorsList.Add("ablPartRevisionID [" + lotNumber.ablPartRevisionID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPLotNumberDto>>> Process_GetAllLotNumbers(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPLotNumberDto> allLotNumbersDto = new List<ERPLotNumberDto>();
		ERPResponseMessageDto<IList<ERPLotNumberDto>> result;
		try
		{
			IERPLotNumberRepository iERPLotNumberRepository = (base.ERPLotNumberRepository = new ERPLotNumberRepository(base.ApiClientContext));
			using (iERPLotNumberRepository)
			{
				foreach (ERPLotNumberInformationDto item2 in await base.ERPLotNumberRepository.GetAllLotNumbers(pageSize, pageNumber, filter, orderBy))
				{
					ERPLotNumberDto item = new ERPLotNumberDto
					{
						ablAddedByUserID = item2.ablAddedByUserID,
						ablAddedDate = item2.ablAddedDate,
						ablLotNumberID = item2.ablLotNumberID,
						ablCreatedBy = item2.ablCreatedBy,
						ablCreatedDate = item2.ablCreatedDate,
						ablUniqueID = item2.ablUniqueID,
						ablExpirationDate = item2.ablExpirationDate,
						ablInactiveDate = item2.ablInactiveDate,
						ablInactive = item2.ablInactive,
						ablPartID = item2.ablPartID,
						ablPartRevisionID = item2.ablPartRevisionID,
						ablRowVersion = item2.ablRowVersion,
						CustomFields = item2.CustomFields
					};
					allLotNumbersDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all LotNumbers]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPLotNumberDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allLotNumbersDto,
				RecordCount = allLotNumbersDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLotNumberDto>> Process_GetLotNumber(Guid lotNumberId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPLotNumberDto lotNumberDto = null;
		ERPResponseMessageDto<ERPLotNumberDto> result;
		try
		{
			IERPLotNumberRepository iERPLotNumberRepository = (base.ERPLotNumberRepository = new ERPLotNumberRepository(base.ApiClientContext));
			using (iERPLotNumberRepository)
			{
				ERPLotNumberInformationDto eRPLotNumberInformationDto = await base.ERPLotNumberRepository.GetLotNumber(lotNumberId);
				lotNumberDto = new ERPLotNumberDto
				{
					ablAddedByUserID = eRPLotNumberInformationDto.ablAddedByUserID,
					ablAddedDate = eRPLotNumberInformationDto.ablAddedDate,
					ablLotNumberID = eRPLotNumberInformationDto.ablLotNumberID,
					ablCreatedBy = eRPLotNumberInformationDto.ablCreatedBy,
					ablCreatedDate = eRPLotNumberInformationDto.ablCreatedDate,
					ablUniqueID = eRPLotNumberInformationDto.ablUniqueID,
					ablExpirationDate = eRPLotNumberInformationDto.ablExpirationDate,
					ablInactiveDate = eRPLotNumberInformationDto.ablInactiveDate,
					ablInactive = eRPLotNumberInformationDto.ablInactive,
					ablPartID = eRPLotNumberInformationDto.ablPartID,
					ablPartRevisionID = eRPLotNumberInformationDto.ablPartRevisionID,
					ablRowVersion = eRPLotNumberInformationDto.ablRowVersion,
					CustomFields = eRPLotNumberInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the LotNumbers []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLotNumberDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = lotNumberDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLotNumberDto>> Process_PutLotNumber(ERPLotNumberDto lotNumber)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPLotNumberDto createdObject = null;
		ERPResponseMessageDto<ERPLotNumberDto> result;
		try
		{
			IERPLotNumberRepository iERPLotNumberRepository = (base.ERPLotNumberRepository = new ERPLotNumberRepository(base.ApiClientContext));
			using (iERPLotNumberRepository)
			{
				APIValidationInfoDto postResult = await base.ERPLotNumberRepository.SaveLotNumber(lotNumber);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPLotNumberInformationDto eRPLotNumberInformationDto = await base.ERPLotNumberRepository.GetLotNumber(lotNumber.ablUniqueID);
					createdObject = new ERPLotNumberDto
					{
						ablAddedByUserID = eRPLotNumberInformationDto.ablAddedByUserID,
						ablAddedDate = eRPLotNumberInformationDto.ablAddedDate,
						ablLotNumberID = eRPLotNumberInformationDto.ablLotNumberID,
						ablCreatedBy = eRPLotNumberInformationDto.ablCreatedBy,
						ablCreatedDate = eRPLotNumberInformationDto.ablCreatedDate,
						ablUniqueID = eRPLotNumberInformationDto.ablUniqueID,
						ablExpirationDate = eRPLotNumberInformationDto.ablExpirationDate,
						ablInactiveDate = eRPLotNumberInformationDto.ablInactiveDate,
						ablInactive = eRPLotNumberInformationDto.ablInactive,
						ablPartID = eRPLotNumberInformationDto.ablPartID,
						ablPartRevisionID = eRPLotNumberInformationDto.ablPartRevisionID,
						ablRowVersion = eRPLotNumberInformationDto.ablRowVersion,
						CustomFields = eRPLotNumberInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing LotNumber [{lotNumber.ablUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLotNumberDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteLotNumber(Guid lotNumberId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLotNumberRepository iERPLotNumberRepository = (base.ERPLotNumberRepository = new ERPLotNumberRepository(base.ApiClientContext));
		using (iERPLotNumberRepository)
		{
			if (!(await base.ERPLotNumberRepository.DoesLotNumberExist(lotNumberId)))
			{
				base.ErrorsList.Add($"LotNumber [{lotNumberId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPLotNumberInformationDto eRPLotNumberInformationDto = await base.ERPLotNumberRepository.GetLotNumber(lotNumberId);
				string text = await base.ERPLotNumberRepository.WhereUsed("LotNumbers", new object[3] { eRPLotNumberInformationDto.ablPartID, eRPLotNumberInformationDto.ablPartRevisionID, eRPLotNumberInformationDto.ablLotNumberID }, new object[3] { "ablPartID", "ablPartRevisionID", "ablLotNumberID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("LotNumber cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPLotNumberDto>> Process_DeleteLotNumber(Guid lotNumberId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPLotNumberDto> result;
		try
		{
			IERPLotNumberRepository iERPLotNumberRepository = (base.ERPLotNumberRepository = new ERPLotNumberRepository(base.ApiClientContext));
			using (iERPLotNumberRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPLotNumberRepository.DeleteRowFromTable("LotNumbers", "abl", lotNumberId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of LotNumber [{lotNumberId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLotNumberDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPLotNumberDto()
			};
		}
		return result;
	}
}
