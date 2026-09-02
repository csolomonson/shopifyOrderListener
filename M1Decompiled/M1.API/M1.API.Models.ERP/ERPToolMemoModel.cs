using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPToolMemoModel : ERPBaseModel, IERPToolMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllToolMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPToolMemoRepository iERPToolMemoRepository = (base.ERPToolMemoRepository = new ERPToolMemoRepository(base.ApiClientContext));
		using (iERPToolMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPToolMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPToolMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPToolMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPToolMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetToolMemo(Guid toolMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPToolMemoRepository iERPToolMemoRepository = (base.ERPToolMemoRepository = new ERPToolMemoRepository(base.ApiClientContext));
		using (iERPToolMemoRepository)
		{
			if (!(await base.ERPToolMemoRepository.DoesToolMemoExist(toolMemoId)))
			{
				errorsList.Add($"ToolMemo [{toolMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutToolMemo(ERPToolMemoDto toolMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPToolMemoRepository iERPToolMemoRepository = (base.ERPToolMemoRepository = new ERPToolMemoRepository(base.ApiClientContext));
		using (iERPToolMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(toolMemo.xtmToolID) && !(await base.ERPToolMemoRepository.DoesRecordExistInTableUsingKeys("Tools", new object[1] { "xttToolID" }, new object[1] { toolMemo.xtmToolID })))
			{
				errorsList.Add("xtmToolID [" + toolMemo.xtmToolID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPToolMemoDto>>> Process_GetAllToolMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPToolMemoDto> allToolMemosDto = new List<ERPToolMemoDto>();
		ERPResponseMessageDto<IList<ERPToolMemoDto>> result;
		try
		{
			IERPToolMemoRepository iERPToolMemoRepository = (base.ERPToolMemoRepository = new ERPToolMemoRepository(base.ApiClientContext));
			using (iERPToolMemoRepository)
			{
				foreach (ERPToolMemoInformationDto item2 in await base.ERPToolMemoRepository.GetAllToolMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPToolMemoDto item = new ERPToolMemoDto
					{
						xtmCreatedBy = item2.xtmCreatedBy,
						xtmCreatedDate = item2.xtmCreatedDate,
						xtmUniqueID = item2.xtmUniqueID,
						xtmLongDescriptionRtf = item2.xtmLongDescriptionRtf,
						xtmLongDescriptionText = item2.xtmLongDescriptionText,
						xtmMemoDate = item2.xtmMemoDate,
						xtmRowVersion = item2.xtmRowVersion,
						xtmToolMemoID = item2.xtmToolMemoID,
						xtmShortDescription = item2.xtmShortDescription,
						xtmToolID = item2.xtmToolID,
						CustomFields = item2.CustomFields
					};
					allToolMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ToolMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPToolMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allToolMemosDto,
				RecordCount = allToolMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPToolMemoDto>> Process_GetToolMemo(Guid toolMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPToolMemoDto toolMemoDto = null;
		ERPResponseMessageDto<ERPToolMemoDto> result;
		try
		{
			IERPToolMemoRepository iERPToolMemoRepository = (base.ERPToolMemoRepository = new ERPToolMemoRepository(base.ApiClientContext));
			using (iERPToolMemoRepository)
			{
				ERPToolMemoInformationDto eRPToolMemoInformationDto = await base.ERPToolMemoRepository.GetToolMemo(toolMemoId);
				toolMemoDto = new ERPToolMemoDto
				{
					xtmCreatedBy = eRPToolMemoInformationDto.xtmCreatedBy,
					xtmCreatedDate = eRPToolMemoInformationDto.xtmCreatedDate,
					xtmUniqueID = eRPToolMemoInformationDto.xtmUniqueID,
					xtmLongDescriptionRtf = eRPToolMemoInformationDto.xtmLongDescriptionRtf,
					xtmLongDescriptionText = eRPToolMemoInformationDto.xtmLongDescriptionText,
					xtmMemoDate = eRPToolMemoInformationDto.xtmMemoDate,
					xtmRowVersion = eRPToolMemoInformationDto.xtmRowVersion,
					xtmToolMemoID = eRPToolMemoInformationDto.xtmToolMemoID,
					xtmShortDescription = eRPToolMemoInformationDto.xtmShortDescription,
					xtmToolID = eRPToolMemoInformationDto.xtmToolID,
					CustomFields = eRPToolMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ToolMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPToolMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = toolMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPToolMemoDto>> Process_PutToolMemo(ERPToolMemoDto toolMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPToolMemoDto createdObject = null;
		ERPResponseMessageDto<ERPToolMemoDto> result;
		try
		{
			IERPToolMemoRepository iERPToolMemoRepository = (base.ERPToolMemoRepository = new ERPToolMemoRepository(base.ApiClientContext));
			using (iERPToolMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPToolMemoRepository.SaveToolMemo(toolMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPToolMemoInformationDto eRPToolMemoInformationDto = await base.ERPToolMemoRepository.GetToolMemo(toolMemo.xtmUniqueID);
					createdObject = new ERPToolMemoDto
					{
						xtmCreatedBy = eRPToolMemoInformationDto.xtmCreatedBy,
						xtmCreatedDate = eRPToolMemoInformationDto.xtmCreatedDate,
						xtmUniqueID = eRPToolMemoInformationDto.xtmUniqueID,
						xtmLongDescriptionRtf = eRPToolMemoInformationDto.xtmLongDescriptionRtf,
						xtmLongDescriptionText = eRPToolMemoInformationDto.xtmLongDescriptionText,
						xtmMemoDate = eRPToolMemoInformationDto.xtmMemoDate,
						xtmRowVersion = eRPToolMemoInformationDto.xtmRowVersion,
						xtmToolMemoID = eRPToolMemoInformationDto.xtmToolMemoID,
						xtmShortDescription = eRPToolMemoInformationDto.xtmShortDescription,
						xtmToolID = eRPToolMemoInformationDto.xtmToolID,
						CustomFields = eRPToolMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ToolMemo [{toolMemo.xtmUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPToolMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteToolMemo(Guid toolMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPToolMemoRepository iERPToolMemoRepository = (base.ERPToolMemoRepository = new ERPToolMemoRepository(base.ApiClientContext));
		using (iERPToolMemoRepository)
		{
			if (!(await base.ERPToolMemoRepository.DoesToolMemoExist(toolMemoId)))
			{
				base.ErrorsList.Add($"ToolMemo [{toolMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPToolMemoInformationDto eRPToolMemoInformationDto = await base.ERPToolMemoRepository.GetToolMemo(toolMemoId);
				string text = await base.ERPToolMemoRepository.WhereUsed("ToolMemos", new object[2] { eRPToolMemoInformationDto.xtmToolID, eRPToolMemoInformationDto.xtmToolMemoID }, new object[2] { "xtmToolID", "xtmToolMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ToolMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPToolMemoDto>> Process_DeleteToolMemo(Guid toolMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPToolMemoDto> result;
		try
		{
			IERPToolMemoRepository iERPToolMemoRepository = (base.ERPToolMemoRepository = new ERPToolMemoRepository(base.ApiClientContext));
			using (iERPToolMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPToolMemoRepository.DeleteRowFromTable("ToolMemos", "xtm", toolMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ToolMemo [{toolMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPToolMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPToolMemoDto()
			};
		}
		return result;
	}
}
