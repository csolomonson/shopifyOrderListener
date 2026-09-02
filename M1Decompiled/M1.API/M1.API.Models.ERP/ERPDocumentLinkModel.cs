using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPDocumentLinkModel : ERPBaseModel, IERPDocumentLinkModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllDocumentLinks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPDocumentLinkRepository iERPDocumentLinkRepository = (base.ERPDocumentLinkRepository = new ERPDocumentLinkRepository(base.ApiClientContext));
		using (iERPDocumentLinkRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPDocumentLinkRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPDocumentLinkRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPDocumentLinkRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPDocumentLinkRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetDocumentLink(Guid documentLinkId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDocumentLinkRepository iERPDocumentLinkRepository = (base.ERPDocumentLinkRepository = new ERPDocumentLinkRepository(base.ApiClientContext));
		using (iERPDocumentLinkRepository)
		{
			if (!(await base.ERPDocumentLinkRepository.DoesDocumentLinkExist(documentLinkId)))
			{
				errorsList.Add($"DocumentLink [{documentLinkId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutDocumentLink(ERPDocumentLinkDto documentLink)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDocumentLinkRepository iERPDocumentLinkRepository = (base.ERPDocumentLinkRepository = new ERPDocumentLinkRepository(base.ApiClientContext));
		using (iERPDocumentLinkRepository)
		{
			if (!string.IsNullOrWhiteSpace(documentLink.xalType) && !(await base.ERPDocumentLinkRepository.DoesRecordExistInTableUsingKeys("AttachmentTypes", new object[1] { "CMTATTACHMENTTYPEID" }, new object[1] { documentLink.xalType })))
			{
				errorsList.Add("xalType [" + documentLink.xalType + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPDocumentLinkDto>>> Process_GetAllDocumentLinks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPDocumentLinkDto> allDocumentLinksDto = new List<ERPDocumentLinkDto>();
		ERPResponseMessageDto<IList<ERPDocumentLinkDto>> result;
		try
		{
			IERPDocumentLinkRepository iERPDocumentLinkRepository = (base.ERPDocumentLinkRepository = new ERPDocumentLinkRepository(base.ApiClientContext));
			using (iERPDocumentLinkRepository)
			{
				foreach (ERPDocumentLinkInformationDto item2 in await base.ERPDocumentLinkRepository.GetAllDocumentLinks(pageSize, pageNumber, filter, orderBy))
				{
					ERPDocumentLinkDto item = new ERPDocumentLinkDto
					{
						xalAddedByUserID = item2.xalAddedByUserID,
						xalAddedDate = item2.xalAddedDate,
						xalCloudFileId = item2.xalCloudFileId,
						xalCreatedBy = item2.xalCreatedBy,
						xalCreatedDate = item2.xalCreatedDate,
						xalDescription = item2.xalDescription,
						xalUniqueID = item2.xalUniqueID,
						xalFileLastModifiedDate = item2.xalFileLastModifiedDate,
						xalFileName = item2.xalFileName,
						xalFileNameWhenUploaded = item2.xalFileNameWhenUploaded,
						xalEmailDefault = item2.xalEmailDefault,
						xalPrintDefault = item2.xalPrintDefault,
						xalReference = item2.xalReference,
						xalRowVersion = item2.xalRowVersion,
						xalDocumentLinkID = item2.xalDocumentLinkID,
						xalType = item2.xalType,
						CustomFields = item2.CustomFields
					};
					allDocumentLinksDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all DocumentLinks]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPDocumentLinkDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allDocumentLinksDto,
				RecordCount = allDocumentLinksDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPDocumentLinkDto>> Process_GetDocumentLink(Guid documentLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPDocumentLinkDto documentLinkDto = null;
		ERPResponseMessageDto<ERPDocumentLinkDto> result;
		try
		{
			IERPDocumentLinkRepository iERPDocumentLinkRepository = (base.ERPDocumentLinkRepository = new ERPDocumentLinkRepository(base.ApiClientContext));
			using (iERPDocumentLinkRepository)
			{
				ERPDocumentLinkInformationDto eRPDocumentLinkInformationDto = await base.ERPDocumentLinkRepository.GetDocumentLink(documentLinkId);
				documentLinkDto = new ERPDocumentLinkDto
				{
					xalAddedByUserID = eRPDocumentLinkInformationDto.xalAddedByUserID,
					xalAddedDate = eRPDocumentLinkInformationDto.xalAddedDate,
					xalCloudFileId = eRPDocumentLinkInformationDto.xalCloudFileId,
					xalCreatedBy = eRPDocumentLinkInformationDto.xalCreatedBy,
					xalCreatedDate = eRPDocumentLinkInformationDto.xalCreatedDate,
					xalDescription = eRPDocumentLinkInformationDto.xalDescription,
					xalUniqueID = eRPDocumentLinkInformationDto.xalUniqueID,
					xalFileLastModifiedDate = eRPDocumentLinkInformationDto.xalFileLastModifiedDate,
					xalFileName = eRPDocumentLinkInformationDto.xalFileName,
					xalFileNameWhenUploaded = eRPDocumentLinkInformationDto.xalFileNameWhenUploaded,
					xalEmailDefault = eRPDocumentLinkInformationDto.xalEmailDefault,
					xalPrintDefault = eRPDocumentLinkInformationDto.xalPrintDefault,
					xalReference = eRPDocumentLinkInformationDto.xalReference,
					xalRowVersion = eRPDocumentLinkInformationDto.xalRowVersion,
					xalDocumentLinkID = eRPDocumentLinkInformationDto.xalDocumentLinkID,
					xalType = eRPDocumentLinkInformationDto.xalType,
					CustomFields = eRPDocumentLinkInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the DocumentLinks []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDocumentLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = documentLinkDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPDocumentLinkDto>> Process_PutDocumentLink(ERPDocumentLinkDto documentLink)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPDocumentLinkDto createdObject = null;
		ERPResponseMessageDto<ERPDocumentLinkDto> result;
		try
		{
			IERPDocumentLinkRepository iERPDocumentLinkRepository = (base.ERPDocumentLinkRepository = new ERPDocumentLinkRepository(base.ApiClientContext));
			using (iERPDocumentLinkRepository)
			{
				APIValidationInfoDto postResult = await base.ERPDocumentLinkRepository.SaveDocumentLink(documentLink);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPDocumentLinkInformationDto eRPDocumentLinkInformationDto = await base.ERPDocumentLinkRepository.GetDocumentLink(documentLink.xalUniqueID);
					createdObject = new ERPDocumentLinkDto
					{
						xalAddedByUserID = eRPDocumentLinkInformationDto.xalAddedByUserID,
						xalAddedDate = eRPDocumentLinkInformationDto.xalAddedDate,
						xalCloudFileId = eRPDocumentLinkInformationDto.xalCloudFileId,
						xalCreatedBy = eRPDocumentLinkInformationDto.xalCreatedBy,
						xalCreatedDate = eRPDocumentLinkInformationDto.xalCreatedDate,
						xalDescription = eRPDocumentLinkInformationDto.xalDescription,
						xalUniqueID = eRPDocumentLinkInformationDto.xalUniqueID,
						xalFileLastModifiedDate = eRPDocumentLinkInformationDto.xalFileLastModifiedDate,
						xalFileName = eRPDocumentLinkInformationDto.xalFileName,
						xalFileNameWhenUploaded = eRPDocumentLinkInformationDto.xalFileNameWhenUploaded,
						xalEmailDefault = eRPDocumentLinkInformationDto.xalEmailDefault,
						xalPrintDefault = eRPDocumentLinkInformationDto.xalPrintDefault,
						xalReference = eRPDocumentLinkInformationDto.xalReference,
						xalRowVersion = eRPDocumentLinkInformationDto.xalRowVersion,
						xalDocumentLinkID = eRPDocumentLinkInformationDto.xalDocumentLinkID,
						xalType = eRPDocumentLinkInformationDto.xalType,
						CustomFields = eRPDocumentLinkInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing DocumentLink [{documentLink.xalUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDocumentLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteDocumentLink(Guid documentLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDocumentLinkRepository iERPDocumentLinkRepository = (base.ERPDocumentLinkRepository = new ERPDocumentLinkRepository(base.ApiClientContext));
		using (iERPDocumentLinkRepository)
		{
			if (!(await base.ERPDocumentLinkRepository.DoesDocumentLinkExist(documentLinkId)))
			{
				base.ErrorsList.Add($"DocumentLink [{documentLinkId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPDocumentLinkInformationDto eRPDocumentLinkInformationDto = await base.ERPDocumentLinkRepository.GetDocumentLink(documentLinkId);
				string text = await base.ERPDocumentLinkRepository.WhereUsed("DocumentLinks", new object[1] { eRPDocumentLinkInformationDto.xalDocumentLinkID }, new object[1] { "xalDocumentLinkID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("DocumentLink cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPDocumentLinkDto>> Process_DeleteDocumentLink(Guid documentLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPDocumentLinkDto> result;
		try
		{
			IERPDocumentLinkRepository iERPDocumentLinkRepository = (base.ERPDocumentLinkRepository = new ERPDocumentLinkRepository(base.ApiClientContext));
			using (iERPDocumentLinkRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPDocumentLinkRepository.DeleteRowFromTable("DocumentLinks", "xal", documentLinkId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of DocumentLink [{documentLinkId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDocumentLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPDocumentLinkDto()
			};
		}
		return result;
	}
}
