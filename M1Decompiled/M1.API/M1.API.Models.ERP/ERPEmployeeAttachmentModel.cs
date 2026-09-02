using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPEmployeeAttachmentModel : ERPBaseModel, IERPEmployeeAttachmentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeeAttachments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPEmployeeAttachmentRepository iERPEmployeeAttachmentRepository = (base.ERPEmployeeAttachmentRepository = new ERPEmployeeAttachmentRepository(base.ApiClientContext));
		using (iERPEmployeeAttachmentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPEmployeeAttachmentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPEmployeeAttachmentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPEmployeeAttachmentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPEmployeeAttachmentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetEmployeeAttachment(Guid employeeAttachmentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeAttachmentRepository iERPEmployeeAttachmentRepository = (base.ERPEmployeeAttachmentRepository = new ERPEmployeeAttachmentRepository(base.ApiClientContext));
		using (iERPEmployeeAttachmentRepository)
		{
			if (!(await base.ERPEmployeeAttachmentRepository.DoesEmployeeAttachmentExist(employeeAttachmentId)))
			{
				errorsList.Add($"EmployeeAttachment [{employeeAttachmentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutEmployeeAttachment(ERPEmployeeAttachmentDto employeeAttachment)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeAttachmentRepository iERPEmployeeAttachmentRepository = (base.ERPEmployeeAttachmentRepository = new ERPEmployeeAttachmentRepository(base.ApiClientContext));
		using (iERPEmployeeAttachmentRepository)
		{
			if (!string.IsNullOrWhiteSpace(employeeAttachment.lmaEmployeeID) && !(await base.ERPEmployeeAttachmentRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { employeeAttachment.lmaEmployeeID })))
			{
				errorsList.Add("lmaEmployeeID [" + employeeAttachment.lmaEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(employeeAttachment.lmaAttachmentTypeID) && !(await base.ERPEmployeeAttachmentRepository.DoesRecordExistInTableUsingKeys("AttachmentTypes", new object[1] { "CMTATTACHMENTTYPEID" }, new object[1] { employeeAttachment.lmaAttachmentTypeID })))
			{
				errorsList.Add("lmaAttachmentTypeID [" + employeeAttachment.lmaAttachmentTypeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPEmployeeAttachmentDto>>> Process_GetAllEmployeeAttachments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPEmployeeAttachmentDto> allEmployeeAttachmentsDto = new List<ERPEmployeeAttachmentDto>();
		ERPResponseMessageDto<IList<ERPEmployeeAttachmentDto>> result;
		try
		{
			IERPEmployeeAttachmentRepository iERPEmployeeAttachmentRepository = (base.ERPEmployeeAttachmentRepository = new ERPEmployeeAttachmentRepository(base.ApiClientContext));
			using (iERPEmployeeAttachmentRepository)
			{
				foreach (ERPEmployeeAttachmentInformationDto item2 in await base.ERPEmployeeAttachmentRepository.GetAllEmployeeAttachments(pageSize, pageNumber, filter, orderBy))
				{
					ERPEmployeeAttachmentDto item = new ERPEmployeeAttachmentDto
					{
						lmaAttachmentTypeID = item2.lmaAttachmentTypeID,
						lmaEmployeeAttachmentID = item2.lmaEmployeeAttachmentID,
						lmaCreatedBy = item2.lmaCreatedBy,
						lmaCreatedDate = item2.lmaCreatedDate,
						lmaDate = item2.lmaDate,
						lmaEmployeeID = item2.lmaEmployeeID,
						lmaUniqueID = item2.lmaUniqueID,
						lmaFileLocation = item2.lmaFileLocation,
						lmaFileName = item2.lmaFileName,
						lmaLongDescriptionRtf = item2.lmaLongDescriptionRtf,
						lmaLongDescriptionText = item2.lmaLongDescriptionText,
						lmaRowVersion = item2.lmaRowVersion,
						lmaShortDescription = item2.lmaShortDescription,
						CustomFields = item2.CustomFields
					};
					allEmployeeAttachmentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all EmployeeAttachments]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPEmployeeAttachmentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allEmployeeAttachmentsDto,
				RecordCount = allEmployeeAttachmentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeAttachmentDto>> Process_GetEmployeeAttachment(Guid employeeAttachmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPEmployeeAttachmentDto employeeAttachmentDto = null;
		ERPResponseMessageDto<ERPEmployeeAttachmentDto> result;
		try
		{
			IERPEmployeeAttachmentRepository iERPEmployeeAttachmentRepository = (base.ERPEmployeeAttachmentRepository = new ERPEmployeeAttachmentRepository(base.ApiClientContext));
			using (iERPEmployeeAttachmentRepository)
			{
				ERPEmployeeAttachmentInformationDto eRPEmployeeAttachmentInformationDto = await base.ERPEmployeeAttachmentRepository.GetEmployeeAttachment(employeeAttachmentId);
				employeeAttachmentDto = new ERPEmployeeAttachmentDto
				{
					lmaAttachmentTypeID = eRPEmployeeAttachmentInformationDto.lmaAttachmentTypeID,
					lmaEmployeeAttachmentID = eRPEmployeeAttachmentInformationDto.lmaEmployeeAttachmentID,
					lmaCreatedBy = eRPEmployeeAttachmentInformationDto.lmaCreatedBy,
					lmaCreatedDate = eRPEmployeeAttachmentInformationDto.lmaCreatedDate,
					lmaDate = eRPEmployeeAttachmentInformationDto.lmaDate,
					lmaEmployeeID = eRPEmployeeAttachmentInformationDto.lmaEmployeeID,
					lmaUniqueID = eRPEmployeeAttachmentInformationDto.lmaUniqueID,
					lmaFileLocation = eRPEmployeeAttachmentInformationDto.lmaFileLocation,
					lmaFileName = eRPEmployeeAttachmentInformationDto.lmaFileName,
					lmaLongDescriptionRtf = eRPEmployeeAttachmentInformationDto.lmaLongDescriptionRtf,
					lmaLongDescriptionText = eRPEmployeeAttachmentInformationDto.lmaLongDescriptionText,
					lmaRowVersion = eRPEmployeeAttachmentInformationDto.lmaRowVersion,
					lmaShortDescription = eRPEmployeeAttachmentInformationDto.lmaShortDescription,
					CustomFields = eRPEmployeeAttachmentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the EmployeeAttachments []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeAttachmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = employeeAttachmentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeAttachmentDto>> Process_PutEmployeeAttachment(ERPEmployeeAttachmentDto employeeAttachment)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPEmployeeAttachmentDto createdObject = null;
		ERPResponseMessageDto<ERPEmployeeAttachmentDto> result;
		try
		{
			IERPEmployeeAttachmentRepository iERPEmployeeAttachmentRepository = (base.ERPEmployeeAttachmentRepository = new ERPEmployeeAttachmentRepository(base.ApiClientContext));
			using (iERPEmployeeAttachmentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPEmployeeAttachmentRepository.SaveEmployeeAttachment(employeeAttachment);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPEmployeeAttachmentInformationDto eRPEmployeeAttachmentInformationDto = await base.ERPEmployeeAttachmentRepository.GetEmployeeAttachment(employeeAttachment.lmaUniqueID);
					createdObject = new ERPEmployeeAttachmentDto
					{
						lmaAttachmentTypeID = eRPEmployeeAttachmentInformationDto.lmaAttachmentTypeID,
						lmaEmployeeAttachmentID = eRPEmployeeAttachmentInformationDto.lmaEmployeeAttachmentID,
						lmaCreatedBy = eRPEmployeeAttachmentInformationDto.lmaCreatedBy,
						lmaCreatedDate = eRPEmployeeAttachmentInformationDto.lmaCreatedDate,
						lmaDate = eRPEmployeeAttachmentInformationDto.lmaDate,
						lmaEmployeeID = eRPEmployeeAttachmentInformationDto.lmaEmployeeID,
						lmaUniqueID = eRPEmployeeAttachmentInformationDto.lmaUniqueID,
						lmaFileLocation = eRPEmployeeAttachmentInformationDto.lmaFileLocation,
						lmaFileName = eRPEmployeeAttachmentInformationDto.lmaFileName,
						lmaLongDescriptionRtf = eRPEmployeeAttachmentInformationDto.lmaLongDescriptionRtf,
						lmaLongDescriptionText = eRPEmployeeAttachmentInformationDto.lmaLongDescriptionText,
						lmaRowVersion = eRPEmployeeAttachmentInformationDto.lmaRowVersion,
						lmaShortDescription = eRPEmployeeAttachmentInformationDto.lmaShortDescription,
						CustomFields = eRPEmployeeAttachmentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing EmployeeAttachment [{employeeAttachment.lmaUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeAttachmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteEmployeeAttachment(Guid employeeAttachmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeAttachmentRepository iERPEmployeeAttachmentRepository = (base.ERPEmployeeAttachmentRepository = new ERPEmployeeAttachmentRepository(base.ApiClientContext));
		using (iERPEmployeeAttachmentRepository)
		{
			if (!(await base.ERPEmployeeAttachmentRepository.DoesEmployeeAttachmentExist(employeeAttachmentId)))
			{
				base.ErrorsList.Add($"EmployeeAttachment [{employeeAttachmentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPEmployeeAttachmentInformationDto eRPEmployeeAttachmentInformationDto = await base.ERPEmployeeAttachmentRepository.GetEmployeeAttachment(employeeAttachmentId);
				string text = await base.ERPEmployeeAttachmentRepository.WhereUsed("EmployeeAttachments", new object[1] { eRPEmployeeAttachmentInformationDto.lmaEmployeeAttachmentID }, new object[1] { "lmaEmployeeAttachmentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("EmployeeAttachment cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeAttachmentDto>> Process_DeleteEmployeeAttachment(Guid employeeAttachmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPEmployeeAttachmentDto> result;
		try
		{
			IERPEmployeeAttachmentRepository iERPEmployeeAttachmentRepository = (base.ERPEmployeeAttachmentRepository = new ERPEmployeeAttachmentRepository(base.ApiClientContext));
			using (iERPEmployeeAttachmentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPEmployeeAttachmentRepository.DeleteRowFromTable("EmployeeAttachments", "lma", employeeAttachmentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of EmployeeAttachment [{employeeAttachmentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeAttachmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPEmployeeAttachmentDto()
			};
		}
		return result;
	}
}
