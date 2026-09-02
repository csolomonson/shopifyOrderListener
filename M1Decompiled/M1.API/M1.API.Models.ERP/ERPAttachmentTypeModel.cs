using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAttachmentTypeModel : ERPBaseModel, IERPAttachmentTypeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAttachmentTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAttachmentTypeRepository iERPAttachmentTypeRepository = (base.ERPAttachmentTypeRepository = new ERPAttachmentTypeRepository(base.ApiClientContext));
		using (iERPAttachmentTypeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAttachmentTypeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAttachmentTypeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAttachmentTypeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAttachmentTypeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAttachmentType(Guid attachmentTypeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAttachmentTypeRepository iERPAttachmentTypeRepository = (base.ERPAttachmentTypeRepository = new ERPAttachmentTypeRepository(base.ApiClientContext));
		using (iERPAttachmentTypeRepository)
		{
			if (!(await base.ERPAttachmentTypeRepository.DoesAttachmentTypeExist(attachmentTypeId)))
			{
				errorsList.Add($"AttachmentType [{attachmentTypeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAttachmentType(ERPAttachmentTypeDto attachmentType)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAttachmentTypeRepository iERPAttachmentTypeRepository = (base.ERPAttachmentTypeRepository = new ERPAttachmentTypeRepository(base.ApiClientContext));
		using (iERPAttachmentTypeRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAttachmentTypeDto>>> Process_GetAllAttachmentTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAttachmentTypeDto> allAttachmentTypesDto = new List<ERPAttachmentTypeDto>();
		ERPResponseMessageDto<IList<ERPAttachmentTypeDto>> result;
		try
		{
			IERPAttachmentTypeRepository iERPAttachmentTypeRepository = (base.ERPAttachmentTypeRepository = new ERPAttachmentTypeRepository(base.ApiClientContext));
			using (iERPAttachmentTypeRepository)
			{
				foreach (ERPAttachmentTypeInformationDto item2 in await base.ERPAttachmentTypeRepository.GetAllAttachmentTypes(pageSize, pageNumber, filter, orderBy))
				{
					ERPAttachmentTypeDto item = new ERPAttachmentTypeDto
					{
						cmtAttachmentTypeID = item2.cmtAttachmentTypeID,
						cmtCreatedBy = item2.cmtCreatedBy,
						cmtCreatedDate = item2.cmtCreatedDate,
						cmtDescription = item2.cmtDescription,
						cmtUniqueID = item2.cmtUniqueID,
						cmtRequiresLogin = item2.cmtRequiresLogin,
						cmtRequiresServiceContract = item2.cmtRequiresServiceContract,
						cmtRowVersion = item2.cmtRowVersion,
						CustomFields = item2.CustomFields
					};
					allAttachmentTypesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all AttachmentTypes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAttachmentTypeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAttachmentTypesDto,
				RecordCount = allAttachmentTypesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAttachmentTypeDto>> Process_GetAttachmentType(Guid attachmentTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAttachmentTypeDto attachmentTypeDto = null;
		ERPResponseMessageDto<ERPAttachmentTypeDto> result;
		try
		{
			IERPAttachmentTypeRepository iERPAttachmentTypeRepository = (base.ERPAttachmentTypeRepository = new ERPAttachmentTypeRepository(base.ApiClientContext));
			using (iERPAttachmentTypeRepository)
			{
				ERPAttachmentTypeInformationDto eRPAttachmentTypeInformationDto = await base.ERPAttachmentTypeRepository.GetAttachmentType(attachmentTypeId);
				attachmentTypeDto = new ERPAttachmentTypeDto
				{
					cmtAttachmentTypeID = eRPAttachmentTypeInformationDto.cmtAttachmentTypeID,
					cmtCreatedBy = eRPAttachmentTypeInformationDto.cmtCreatedBy,
					cmtCreatedDate = eRPAttachmentTypeInformationDto.cmtCreatedDate,
					cmtDescription = eRPAttachmentTypeInformationDto.cmtDescription,
					cmtUniqueID = eRPAttachmentTypeInformationDto.cmtUniqueID,
					cmtRequiresLogin = eRPAttachmentTypeInformationDto.cmtRequiresLogin,
					cmtRequiresServiceContract = eRPAttachmentTypeInformationDto.cmtRequiresServiceContract,
					cmtRowVersion = eRPAttachmentTypeInformationDto.cmtRowVersion,
					CustomFields = eRPAttachmentTypeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the AttachmentTypes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAttachmentTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = attachmentTypeDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAttachmentTypeDto>> Process_PutAttachmentType(ERPAttachmentTypeDto attachmentType)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAttachmentTypeDto createdObject = null;
		ERPResponseMessageDto<ERPAttachmentTypeDto> result;
		try
		{
			IERPAttachmentTypeRepository iERPAttachmentTypeRepository = (base.ERPAttachmentTypeRepository = new ERPAttachmentTypeRepository(base.ApiClientContext));
			using (iERPAttachmentTypeRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAttachmentTypeRepository.SaveAttachmentType(attachmentType);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAttachmentTypeInformationDto eRPAttachmentTypeInformationDto = await base.ERPAttachmentTypeRepository.GetAttachmentType(attachmentType.cmtUniqueID);
					createdObject = new ERPAttachmentTypeDto
					{
						cmtAttachmentTypeID = eRPAttachmentTypeInformationDto.cmtAttachmentTypeID,
						cmtCreatedBy = eRPAttachmentTypeInformationDto.cmtCreatedBy,
						cmtCreatedDate = eRPAttachmentTypeInformationDto.cmtCreatedDate,
						cmtDescription = eRPAttachmentTypeInformationDto.cmtDescription,
						cmtUniqueID = eRPAttachmentTypeInformationDto.cmtUniqueID,
						cmtRequiresLogin = eRPAttachmentTypeInformationDto.cmtRequiresLogin,
						cmtRequiresServiceContract = eRPAttachmentTypeInformationDto.cmtRequiresServiceContract,
						cmtRowVersion = eRPAttachmentTypeInformationDto.cmtRowVersion,
						CustomFields = eRPAttachmentTypeInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing AttachmentType [{attachmentType.cmtUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAttachmentTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAttachmentType(Guid attachmentTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAttachmentTypeRepository iERPAttachmentTypeRepository = (base.ERPAttachmentTypeRepository = new ERPAttachmentTypeRepository(base.ApiClientContext));
		using (iERPAttachmentTypeRepository)
		{
			if (!(await base.ERPAttachmentTypeRepository.DoesAttachmentTypeExist(attachmentTypeId)))
			{
				base.ErrorsList.Add($"AttachmentType [{attachmentTypeId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAttachmentTypeInformationDto eRPAttachmentTypeInformationDto = await base.ERPAttachmentTypeRepository.GetAttachmentType(attachmentTypeId);
				string text = await base.ERPAttachmentTypeRepository.WhereUsed("AttachmentTypes", new object[1] { eRPAttachmentTypeInformationDto.cmtAttachmentTypeID }, new object[1] { "cmtAttachmentTypeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("AttachmentType cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAttachmentTypeDto>> Process_DeleteAttachmentType(Guid attachmentTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAttachmentTypeDto> result;
		try
		{
			IERPAttachmentTypeRepository iERPAttachmentTypeRepository = (base.ERPAttachmentTypeRepository = new ERPAttachmentTypeRepository(base.ApiClientContext));
			using (iERPAttachmentTypeRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAttachmentTypeRepository.DeleteRowFromTable("AttachmentTypes", "cmt", attachmentTypeId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of AttachmentType [{attachmentTypeId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAttachmentTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAttachmentTypeDto()
			};
		}
		return result;
	}
}
