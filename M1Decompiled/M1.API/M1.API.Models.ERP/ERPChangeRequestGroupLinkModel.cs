using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPChangeRequestGroupLinkModel : ERPBaseModel, IERPChangeRequestGroupLinkModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllChangeRequestGroupLinks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPChangeRequestGroupLinkRepository iERPChangeRequestGroupLinkRepository = (base.ERPChangeRequestGroupLinkRepository = new ERPChangeRequestGroupLinkRepository(base.ApiClientContext));
		using (iERPChangeRequestGroupLinkRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPChangeRequestGroupLinkRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPChangeRequestGroupLinkRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPChangeRequestGroupLinkRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPChangeRequestGroupLinkRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetChangeRequestGroupLink(Guid changeRequestGroupLinkId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPChangeRequestGroupLinkRepository iERPChangeRequestGroupLinkRepository = (base.ERPChangeRequestGroupLinkRepository = new ERPChangeRequestGroupLinkRepository(base.ApiClientContext));
		using (iERPChangeRequestGroupLinkRepository)
		{
			if (!(await base.ERPChangeRequestGroupLinkRepository.DoesChangeRequestGroupLinkExist(changeRequestGroupLinkId)))
			{
				errorsList.Add($"ChangeRequestGroupLink [{changeRequestGroupLinkId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutChangeRequestGroupLink(ERPChangeRequestGroupLinkDto changeRequestGroupLink)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPChangeRequestGroupLinkRepository iERPChangeRequestGroupLinkRepository = (base.ERPChangeRequestGroupLinkRepository = new ERPChangeRequestGroupLinkRepository(base.ApiClientContext));
		using (iERPChangeRequestGroupLinkRepository)
		{
			if (!string.IsNullOrWhiteSpace(changeRequestGroupLink.chrChangeRequestID) && !(await base.ERPChangeRequestGroupLinkRepository.DoesRecordExistInTableUsingKeys("ChangeRequests", new object[1] { "CHPCHANGEREQUESTID" }, new object[1] { changeRequestGroupLink.chrChangeRequestID })))
			{
				errorsList.Add("chrChangeRequestID [" + changeRequestGroupLink.chrChangeRequestID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(changeRequestGroupLink.chrChangeRequestGroupID) && !(await base.ERPChangeRequestGroupLinkRepository.DoesRecordExistInTableUsingKeys("ChangeRequestGroups", new object[1] { "CHGCHANGEREQUESTGROUPID" }, new object[1] { changeRequestGroupLink.chrChangeRequestGroupID })))
			{
				errorsList.Add("chrChangeRequestGroupID [" + changeRequestGroupLink.chrChangeRequestGroupID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPChangeRequestGroupLinkDto>>> Process_GetAllChangeRequestGroupLinks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPChangeRequestGroupLinkDto> allChangeRequestGroupLinksDto = new List<ERPChangeRequestGroupLinkDto>();
		ERPResponseMessageDto<IList<ERPChangeRequestGroupLinkDto>> result;
		try
		{
			IERPChangeRequestGroupLinkRepository iERPChangeRequestGroupLinkRepository = (base.ERPChangeRequestGroupLinkRepository = new ERPChangeRequestGroupLinkRepository(base.ApiClientContext));
			using (iERPChangeRequestGroupLinkRepository)
			{
				foreach (ERPChangeRequestGroupLinkInformationDto item2 in await base.ERPChangeRequestGroupLinkRepository.GetAllChangeRequestGroupLinks(pageSize, pageNumber, filter, orderBy))
				{
					ERPChangeRequestGroupLinkDto item = new ERPChangeRequestGroupLinkDto
					{
						chrChangeRequestGroupID = item2.chrChangeRequestGroupID,
						chrChangeRequestID = item2.chrChangeRequestID,
						chrCreatedBy = item2.chrCreatedBy,
						chrCreatedDate = item2.chrCreatedDate,
						chrUniqueID = item2.chrUniqueID,
						chrRowVersion = item2.chrRowVersion,
						chrChangeRequestGroupLinkID = item2.chrChangeRequestGroupLinkID,
						CustomFields = item2.CustomFields
					};
					allChangeRequestGroupLinksDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ChangeRequestGroupLinks]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPChangeRequestGroupLinkDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allChangeRequestGroupLinksDto,
				RecordCount = allChangeRequestGroupLinksDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPChangeRequestGroupLinkDto>> Process_GetChangeRequestGroupLink(Guid changeRequestGroupLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPChangeRequestGroupLinkDto changeRequestGroupLinkDto = null;
		ERPResponseMessageDto<ERPChangeRequestGroupLinkDto> result;
		try
		{
			IERPChangeRequestGroupLinkRepository iERPChangeRequestGroupLinkRepository = (base.ERPChangeRequestGroupLinkRepository = new ERPChangeRequestGroupLinkRepository(base.ApiClientContext));
			using (iERPChangeRequestGroupLinkRepository)
			{
				ERPChangeRequestGroupLinkInformationDto eRPChangeRequestGroupLinkInformationDto = await base.ERPChangeRequestGroupLinkRepository.GetChangeRequestGroupLink(changeRequestGroupLinkId);
				changeRequestGroupLinkDto = new ERPChangeRequestGroupLinkDto
				{
					chrChangeRequestGroupID = eRPChangeRequestGroupLinkInformationDto.chrChangeRequestGroupID,
					chrChangeRequestID = eRPChangeRequestGroupLinkInformationDto.chrChangeRequestID,
					chrCreatedBy = eRPChangeRequestGroupLinkInformationDto.chrCreatedBy,
					chrCreatedDate = eRPChangeRequestGroupLinkInformationDto.chrCreatedDate,
					chrUniqueID = eRPChangeRequestGroupLinkInformationDto.chrUniqueID,
					chrRowVersion = eRPChangeRequestGroupLinkInformationDto.chrRowVersion,
					chrChangeRequestGroupLinkID = eRPChangeRequestGroupLinkInformationDto.chrChangeRequestGroupLinkID,
					CustomFields = eRPChangeRequestGroupLinkInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ChangeRequestGroupLinks []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPChangeRequestGroupLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = changeRequestGroupLinkDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPChangeRequestGroupLinkDto>> Process_PutChangeRequestGroupLink(ERPChangeRequestGroupLinkDto changeRequestGroupLink)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPChangeRequestGroupLinkDto createdObject = null;
		ERPResponseMessageDto<ERPChangeRequestGroupLinkDto> result;
		try
		{
			IERPChangeRequestGroupLinkRepository iERPChangeRequestGroupLinkRepository = (base.ERPChangeRequestGroupLinkRepository = new ERPChangeRequestGroupLinkRepository(base.ApiClientContext));
			using (iERPChangeRequestGroupLinkRepository)
			{
				APIValidationInfoDto postResult = await base.ERPChangeRequestGroupLinkRepository.SaveChangeRequestGroupLink(changeRequestGroupLink);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPChangeRequestGroupLinkInformationDto eRPChangeRequestGroupLinkInformationDto = await base.ERPChangeRequestGroupLinkRepository.GetChangeRequestGroupLink(changeRequestGroupLink.chrUniqueID);
					createdObject = new ERPChangeRequestGroupLinkDto
					{
						chrChangeRequestGroupID = eRPChangeRequestGroupLinkInformationDto.chrChangeRequestGroupID,
						chrChangeRequestID = eRPChangeRequestGroupLinkInformationDto.chrChangeRequestID,
						chrCreatedBy = eRPChangeRequestGroupLinkInformationDto.chrCreatedBy,
						chrCreatedDate = eRPChangeRequestGroupLinkInformationDto.chrCreatedDate,
						chrUniqueID = eRPChangeRequestGroupLinkInformationDto.chrUniqueID,
						chrRowVersion = eRPChangeRequestGroupLinkInformationDto.chrRowVersion,
						chrChangeRequestGroupLinkID = eRPChangeRequestGroupLinkInformationDto.chrChangeRequestGroupLinkID,
						CustomFields = eRPChangeRequestGroupLinkInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ChangeRequestGroupLink [{changeRequestGroupLink.chrUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPChangeRequestGroupLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteChangeRequestGroupLink(Guid changeRequestGroupLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPChangeRequestGroupLinkRepository iERPChangeRequestGroupLinkRepository = (base.ERPChangeRequestGroupLinkRepository = new ERPChangeRequestGroupLinkRepository(base.ApiClientContext));
		using (iERPChangeRequestGroupLinkRepository)
		{
			if (!(await base.ERPChangeRequestGroupLinkRepository.DoesChangeRequestGroupLinkExist(changeRequestGroupLinkId)))
			{
				base.ErrorsList.Add($"ChangeRequestGroupLink [{changeRequestGroupLinkId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPChangeRequestGroupLinkInformationDto eRPChangeRequestGroupLinkInformationDto = await base.ERPChangeRequestGroupLinkRepository.GetChangeRequestGroupLink(changeRequestGroupLinkId);
				string text = await base.ERPChangeRequestGroupLinkRepository.WhereUsed("ChangeRequestGroupLinks", new object[2] { eRPChangeRequestGroupLinkInformationDto.chrChangeRequestID, eRPChangeRequestGroupLinkInformationDto.chrChangeRequestGroupLinkID }, new object[2] { "chrChangeRequestID", "chrChangeRequestGroupLinkID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ChangeRequestGroupLink cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPChangeRequestGroupLinkDto>> Process_DeleteChangeRequestGroupLink(Guid changeRequestGroupLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPChangeRequestGroupLinkDto> result;
		try
		{
			IERPChangeRequestGroupLinkRepository iERPChangeRequestGroupLinkRepository = (base.ERPChangeRequestGroupLinkRepository = new ERPChangeRequestGroupLinkRepository(base.ApiClientContext));
			using (iERPChangeRequestGroupLinkRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPChangeRequestGroupLinkRepository.DeleteRowFromTable("ChangeRequestGroupLinks", "chr", changeRequestGroupLinkId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ChangeRequestGroupLink [{changeRequestGroupLinkId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPChangeRequestGroupLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPChangeRequestGroupLinkDto()
			};
		}
		return result;
	}
}
