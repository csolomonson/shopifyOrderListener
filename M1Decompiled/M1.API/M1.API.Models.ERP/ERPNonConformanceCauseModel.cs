using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPNonConformanceCauseModel : ERPBaseModel, IERPNonConformanceCauseModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllNonConformanceCauses(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPNonConformanceCauseRepository iERPNonConformanceCauseRepository = (base.ERPNonConformanceCauseRepository = new ERPNonConformanceCauseRepository(base.ApiClientContext));
		using (iERPNonConformanceCauseRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPNonConformanceCauseRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPNonConformanceCauseRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPNonConformanceCauseRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPNonConformanceCauseRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetNonConformanceCause(Guid nonConformanceCauseId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPNonConformanceCauseRepository iERPNonConformanceCauseRepository = (base.ERPNonConformanceCauseRepository = new ERPNonConformanceCauseRepository(base.ApiClientContext));
		using (iERPNonConformanceCauseRepository)
		{
			if (!(await base.ERPNonConformanceCauseRepository.DoesNonConformanceCauseExist(nonConformanceCauseId)))
			{
				errorsList.Add($"NonConformanceCause [{nonConformanceCauseId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutNonConformanceCause(ERPNonConformanceCauseDto nonConformanceCause)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPNonConformanceCauseRepository iERPNonConformanceCauseRepository = (base.ERPNonConformanceCauseRepository = new ERPNonConformanceCauseRepository(base.ApiClientContext));
		using (iERPNonConformanceCauseRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPNonConformanceCauseDto>>> Process_GetAllNonConformanceCauses(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPNonConformanceCauseDto> allNonConformanceCausesDto = new List<ERPNonConformanceCauseDto>();
		ERPResponseMessageDto<IList<ERPNonConformanceCauseDto>> result;
		try
		{
			IERPNonConformanceCauseRepository iERPNonConformanceCauseRepository = (base.ERPNonConformanceCauseRepository = new ERPNonConformanceCauseRepository(base.ApiClientContext));
			using (iERPNonConformanceCauseRepository)
			{
				foreach (ERPNonConformanceCauseInformationDto item2 in await base.ERPNonConformanceCauseRepository.GetAllNonConformanceCauses(pageSize, pageNumber, filter, orderBy))
				{
					ERPNonConformanceCauseDto item = new ERPNonConformanceCauseDto
					{
						qauNonConformanceCauseID = item2.qauNonConformanceCauseID,
						qauCreatedBy = item2.qauCreatedBy,
						qauCreatedDate = item2.qauCreatedDate,
						qauDescription = item2.qauDescription,
						qauUniqueID = item2.qauUniqueID,
						qauRowVersion = item2.qauRowVersion,
						CustomFields = item2.CustomFields
					};
					allNonConformanceCausesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all NonConformanceCauses]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPNonConformanceCauseDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allNonConformanceCausesDto,
				RecordCount = allNonConformanceCausesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPNonConformanceCauseDto>> Process_GetNonConformanceCause(Guid nonConformanceCauseId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPNonConformanceCauseDto nonConformanceCauseDto = null;
		ERPResponseMessageDto<ERPNonConformanceCauseDto> result;
		try
		{
			IERPNonConformanceCauseRepository iERPNonConformanceCauseRepository = (base.ERPNonConformanceCauseRepository = new ERPNonConformanceCauseRepository(base.ApiClientContext));
			using (iERPNonConformanceCauseRepository)
			{
				ERPNonConformanceCauseInformationDto eRPNonConformanceCauseInformationDto = await base.ERPNonConformanceCauseRepository.GetNonConformanceCause(nonConformanceCauseId);
				nonConformanceCauseDto = new ERPNonConformanceCauseDto
				{
					qauNonConformanceCauseID = eRPNonConformanceCauseInformationDto.qauNonConformanceCauseID,
					qauCreatedBy = eRPNonConformanceCauseInformationDto.qauCreatedBy,
					qauCreatedDate = eRPNonConformanceCauseInformationDto.qauCreatedDate,
					qauDescription = eRPNonConformanceCauseInformationDto.qauDescription,
					qauUniqueID = eRPNonConformanceCauseInformationDto.qauUniqueID,
					qauRowVersion = eRPNonConformanceCauseInformationDto.qauRowVersion,
					CustomFields = eRPNonConformanceCauseInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the NonConformanceCauses []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPNonConformanceCauseDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = nonConformanceCauseDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPNonConformanceCauseDto>> Process_PutNonConformanceCause(ERPNonConformanceCauseDto nonConformanceCause)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPNonConformanceCauseDto createdObject = null;
		ERPResponseMessageDto<ERPNonConformanceCauseDto> result;
		try
		{
			IERPNonConformanceCauseRepository iERPNonConformanceCauseRepository = (base.ERPNonConformanceCauseRepository = new ERPNonConformanceCauseRepository(base.ApiClientContext));
			using (iERPNonConformanceCauseRepository)
			{
				APIValidationInfoDto postResult = await base.ERPNonConformanceCauseRepository.SaveNonConformanceCause(nonConformanceCause);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPNonConformanceCauseInformationDto eRPNonConformanceCauseInformationDto = await base.ERPNonConformanceCauseRepository.GetNonConformanceCause(nonConformanceCause.qauUniqueID);
					createdObject = new ERPNonConformanceCauseDto
					{
						qauNonConformanceCauseID = eRPNonConformanceCauseInformationDto.qauNonConformanceCauseID,
						qauCreatedBy = eRPNonConformanceCauseInformationDto.qauCreatedBy,
						qauCreatedDate = eRPNonConformanceCauseInformationDto.qauCreatedDate,
						qauDescription = eRPNonConformanceCauseInformationDto.qauDescription,
						qauUniqueID = eRPNonConformanceCauseInformationDto.qauUniqueID,
						qauRowVersion = eRPNonConformanceCauseInformationDto.qauRowVersion,
						CustomFields = eRPNonConformanceCauseInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing NonConformanceCause [{nonConformanceCause.qauUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPNonConformanceCauseDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteNonConformanceCause(Guid nonConformanceCauseId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPNonConformanceCauseRepository iERPNonConformanceCauseRepository = (base.ERPNonConformanceCauseRepository = new ERPNonConformanceCauseRepository(base.ApiClientContext));
		using (iERPNonConformanceCauseRepository)
		{
			if (!(await base.ERPNonConformanceCauseRepository.DoesNonConformanceCauseExist(nonConformanceCauseId)))
			{
				base.ErrorsList.Add($"NonConformanceCause [{nonConformanceCauseId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPNonConformanceCauseInformationDto eRPNonConformanceCauseInformationDto = await base.ERPNonConformanceCauseRepository.GetNonConformanceCause(nonConformanceCauseId);
				string text = await base.ERPNonConformanceCauseRepository.WhereUsed("NonConformanceCauses", new object[1] { eRPNonConformanceCauseInformationDto.qauNonConformanceCauseID }, new object[1] { "qauNonConformanceCauseID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("NonConformanceCause cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPNonConformanceCauseDto>> Process_DeleteNonConformanceCause(Guid nonConformanceCauseId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPNonConformanceCauseDto> result;
		try
		{
			IERPNonConformanceCauseRepository iERPNonConformanceCauseRepository = (base.ERPNonConformanceCauseRepository = new ERPNonConformanceCauseRepository(base.ApiClientContext));
			using (iERPNonConformanceCauseRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPNonConformanceCauseRepository.DeleteRowFromTable("NonConformanceCauses", "qau", nonConformanceCauseId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of NonConformanceCause [{nonConformanceCauseId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPNonConformanceCauseDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPNonConformanceCauseDto()
			};
		}
		return result;
	}
}
