using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPContactTitleModel : ERPBaseModel, IERPContactTitleModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllContactTitles(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPContactTitleRepository iERPContactTitleRepository = (base.ERPContactTitleRepository = new ERPContactTitleRepository(base.ApiClientContext));
		using (iERPContactTitleRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPContactTitleRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPContactTitleRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPContactTitleRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPContactTitleRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetContactTitle(Guid contactTitleId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPContactTitleRepository iERPContactTitleRepository = (base.ERPContactTitleRepository = new ERPContactTitleRepository(base.ApiClientContext));
		using (iERPContactTitleRepository)
		{
			if (!(await base.ERPContactTitleRepository.DoesContactTitleExist(contactTitleId)))
			{
				errorsList.Add($"ContactTitle [{contactTitleId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutContactTitle(ERPContactTitleDto contactTitle)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPContactTitleRepository iERPContactTitleRepository = (base.ERPContactTitleRepository = new ERPContactTitleRepository(base.ApiClientContext));
		using (iERPContactTitleRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPContactTitleDto>>> Process_GetAllContactTitles(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPContactTitleDto> allContactTitlesDto = new List<ERPContactTitleDto>();
		ERPResponseMessageDto<IList<ERPContactTitleDto>> result;
		try
		{
			IERPContactTitleRepository iERPContactTitleRepository = (base.ERPContactTitleRepository = new ERPContactTitleRepository(base.ApiClientContext));
			using (iERPContactTitleRepository)
			{
				foreach (ERPContactTitleInformationDto item2 in await base.ERPContactTitleRepository.GetAllContactTitles(pageSize, pageNumber, filter, orderBy))
				{
					ERPContactTitleDto item = new ERPContactTitleDto
					{
						cmeContactTitleID = item2.cmeContactTitleID,
						cmeCreatedBy = item2.cmeCreatedBy,
						cmeCreatedDate = item2.cmeCreatedDate,
						cmeDescription = item2.cmeDescription,
						cmeUniqueID = item2.cmeUniqueID,
						cmeRowVersion = item2.cmeRowVersion,
						CustomFields = item2.CustomFields
					};
					allContactTitlesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ContactTitles]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPContactTitleDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allContactTitlesDto,
				RecordCount = allContactTitlesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPContactTitleDto>> Process_GetContactTitle(Guid contactTitleId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPContactTitleDto contactTitleDto = null;
		ERPResponseMessageDto<ERPContactTitleDto> result;
		try
		{
			IERPContactTitleRepository iERPContactTitleRepository = (base.ERPContactTitleRepository = new ERPContactTitleRepository(base.ApiClientContext));
			using (iERPContactTitleRepository)
			{
				ERPContactTitleInformationDto eRPContactTitleInformationDto = await base.ERPContactTitleRepository.GetContactTitle(contactTitleId);
				contactTitleDto = new ERPContactTitleDto
				{
					cmeContactTitleID = eRPContactTitleInformationDto.cmeContactTitleID,
					cmeCreatedBy = eRPContactTitleInformationDto.cmeCreatedBy,
					cmeCreatedDate = eRPContactTitleInformationDto.cmeCreatedDate,
					cmeDescription = eRPContactTitleInformationDto.cmeDescription,
					cmeUniqueID = eRPContactTitleInformationDto.cmeUniqueID,
					cmeRowVersion = eRPContactTitleInformationDto.cmeRowVersion,
					CustomFields = eRPContactTitleInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ContactTitles []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPContactTitleDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = contactTitleDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPContactTitleDto>> Process_PutContactTitle(ERPContactTitleDto contactTitle)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPContactTitleDto createdObject = null;
		ERPResponseMessageDto<ERPContactTitleDto> result;
		try
		{
			IERPContactTitleRepository iERPContactTitleRepository = (base.ERPContactTitleRepository = new ERPContactTitleRepository(base.ApiClientContext));
			using (iERPContactTitleRepository)
			{
				APIValidationInfoDto postResult = await base.ERPContactTitleRepository.SaveContactTitle(contactTitle);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPContactTitleInformationDto eRPContactTitleInformationDto = await base.ERPContactTitleRepository.GetContactTitle(contactTitle.cmeUniqueID);
					createdObject = new ERPContactTitleDto
					{
						cmeContactTitleID = eRPContactTitleInformationDto.cmeContactTitleID,
						cmeCreatedBy = eRPContactTitleInformationDto.cmeCreatedBy,
						cmeCreatedDate = eRPContactTitleInformationDto.cmeCreatedDate,
						cmeDescription = eRPContactTitleInformationDto.cmeDescription,
						cmeUniqueID = eRPContactTitleInformationDto.cmeUniqueID,
						cmeRowVersion = eRPContactTitleInformationDto.cmeRowVersion,
						CustomFields = eRPContactTitleInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ContactTitle [{contactTitle.cmeUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPContactTitleDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteContactTitle(Guid contactTitleId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPContactTitleRepository iERPContactTitleRepository = (base.ERPContactTitleRepository = new ERPContactTitleRepository(base.ApiClientContext));
		using (iERPContactTitleRepository)
		{
			if (!(await base.ERPContactTitleRepository.DoesContactTitleExist(contactTitleId)))
			{
				base.ErrorsList.Add($"ContactTitle [{contactTitleId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPContactTitleInformationDto eRPContactTitleInformationDto = await base.ERPContactTitleRepository.GetContactTitle(contactTitleId);
				string text = await base.ERPContactTitleRepository.WhereUsed("ContactTitles", new object[1] { eRPContactTitleInformationDto.cmeContactTitleID }, new object[1] { "cmeContactTitleID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ContactTitle cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPContactTitleDto>> Process_DeleteContactTitle(Guid contactTitleId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPContactTitleDto> result;
		try
		{
			IERPContactTitleRepository iERPContactTitleRepository = (base.ERPContactTitleRepository = new ERPContactTitleRepository(base.ApiClientContext));
			using (iERPContactTitleRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPContactTitleRepository.DeleteRowFromTable("ContactTitles", "cme", contactTitleId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ContactTitle [{contactTitleId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPContactTitleDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPContactTitleDto()
			};
		}
		return result;
	}
}
