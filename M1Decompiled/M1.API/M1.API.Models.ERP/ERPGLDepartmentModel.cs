using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPGLDepartmentModel : ERPBaseModel, IERPGLDepartmentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllGLDepartments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLDepartmentRepository iERPGLDepartmentRepository = (base.ERPGLDepartmentRepository = new ERPGLDepartmentRepository(base.ApiClientContext));
		using (iERPGLDepartmentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPGLDepartmentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPGLDepartmentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPGLDepartmentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPGLDepartmentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetGLDepartment(Guid gLDepartmentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLDepartmentRepository iERPGLDepartmentRepository = (base.ERPGLDepartmentRepository = new ERPGLDepartmentRepository(base.ApiClientContext));
		using (iERPGLDepartmentRepository)
		{
			if (!(await base.ERPGLDepartmentRepository.DoesGLDepartmentExist(gLDepartmentId)))
			{
				errorsList.Add($"GLDepartment [{gLDepartmentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutGLDepartment(ERPGLDepartmentDto gLDepartment)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLDepartmentRepository iERPGLDepartmentRepository = (base.ERPGLDepartmentRepository = new ERPGLDepartmentRepository(base.ApiClientContext));
		using (iERPGLDepartmentRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPGLDepartmentDto>>> Process_GetAllGLDepartments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPGLDepartmentDto> allGLDepartmentsDto = new List<ERPGLDepartmentDto>();
		ERPResponseMessageDto<IList<ERPGLDepartmentDto>> result;
		try
		{
			IERPGLDepartmentRepository iERPGLDepartmentRepository = (base.ERPGLDepartmentRepository = new ERPGLDepartmentRepository(base.ApiClientContext));
			using (iERPGLDepartmentRepository)
			{
				foreach (ERPGLDepartmentInformationDto item2 in await base.ERPGLDepartmentRepository.GetAllGLDepartments(pageSize, pageNumber, filter, orderBy))
				{
					ERPGLDepartmentDto item = new ERPGLDepartmentDto
					{
						gldGlDepartmentID = item2.gldGlDepartmentID,
						gldCreatedBy = item2.gldCreatedBy,
						gldCreatedDate = item2.gldCreatedDate,
						gldDescription = item2.gldDescription,
						gldUniqueID = item2.gldUniqueID,
						gldRowVersion = item2.gldRowVersion,
						CustomFields = item2.CustomFields
					};
					allGLDepartmentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all GLDepartments]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPGLDepartmentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allGLDepartmentsDto,
				RecordCount = allGLDepartmentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLDepartmentDto>> Process_GetGLDepartment(Guid gLDepartmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPGLDepartmentDto gLDepartmentDto = null;
		ERPResponseMessageDto<ERPGLDepartmentDto> result;
		try
		{
			IERPGLDepartmentRepository iERPGLDepartmentRepository = (base.ERPGLDepartmentRepository = new ERPGLDepartmentRepository(base.ApiClientContext));
			using (iERPGLDepartmentRepository)
			{
				ERPGLDepartmentInformationDto eRPGLDepartmentInformationDto = await base.ERPGLDepartmentRepository.GetGLDepartment(gLDepartmentId);
				gLDepartmentDto = new ERPGLDepartmentDto
				{
					gldGlDepartmentID = eRPGLDepartmentInformationDto.gldGlDepartmentID,
					gldCreatedBy = eRPGLDepartmentInformationDto.gldCreatedBy,
					gldCreatedDate = eRPGLDepartmentInformationDto.gldCreatedDate,
					gldDescription = eRPGLDepartmentInformationDto.gldDescription,
					gldUniqueID = eRPGLDepartmentInformationDto.gldUniqueID,
					gldRowVersion = eRPGLDepartmentInformationDto.gldRowVersion,
					CustomFields = eRPGLDepartmentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the GLDepartments []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLDepartmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = gLDepartmentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLDepartmentDto>> Process_PutGLDepartment(ERPGLDepartmentDto gLDepartment)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPGLDepartmentDto createdObject = null;
		ERPResponseMessageDto<ERPGLDepartmentDto> result;
		try
		{
			IERPGLDepartmentRepository iERPGLDepartmentRepository = (base.ERPGLDepartmentRepository = new ERPGLDepartmentRepository(base.ApiClientContext));
			using (iERPGLDepartmentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPGLDepartmentRepository.SaveGLDepartment(gLDepartment);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPGLDepartmentInformationDto eRPGLDepartmentInformationDto = await base.ERPGLDepartmentRepository.GetGLDepartment(gLDepartment.gldUniqueID);
					createdObject = new ERPGLDepartmentDto
					{
						gldGlDepartmentID = eRPGLDepartmentInformationDto.gldGlDepartmentID,
						gldCreatedBy = eRPGLDepartmentInformationDto.gldCreatedBy,
						gldCreatedDate = eRPGLDepartmentInformationDto.gldCreatedDate,
						gldDescription = eRPGLDepartmentInformationDto.gldDescription,
						gldUniqueID = eRPGLDepartmentInformationDto.gldUniqueID,
						gldRowVersion = eRPGLDepartmentInformationDto.gldRowVersion,
						CustomFields = eRPGLDepartmentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing GLDepartment [{gLDepartment.gldUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLDepartmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteGLDepartment(Guid gLDepartmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLDepartmentRepository iERPGLDepartmentRepository = (base.ERPGLDepartmentRepository = new ERPGLDepartmentRepository(base.ApiClientContext));
		using (iERPGLDepartmentRepository)
		{
			if (!(await base.ERPGLDepartmentRepository.DoesGLDepartmentExist(gLDepartmentId)))
			{
				base.ErrorsList.Add($"GLDepartment [{gLDepartmentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPGLDepartmentInformationDto eRPGLDepartmentInformationDto = await base.ERPGLDepartmentRepository.GetGLDepartment(gLDepartmentId);
				string text = await base.ERPGLDepartmentRepository.WhereUsed("GLDepartments", new object[1] { eRPGLDepartmentInformationDto.gldGlDepartmentID }, new object[1] { "gldGlDepartmentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("GLDepartment cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPGLDepartmentDto>> Process_DeleteGLDepartment(Guid gLDepartmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPGLDepartmentDto> result;
		try
		{
			IERPGLDepartmentRepository iERPGLDepartmentRepository = (base.ERPGLDepartmentRepository = new ERPGLDepartmentRepository(base.ApiClientContext));
			using (iERPGLDepartmentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPGLDepartmentRepository.DeleteRowFromTable("GLDepartments", "gld", gLDepartmentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of GLDepartment [{gLDepartmentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLDepartmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPGLDepartmentDto()
			};
		}
		return result;
	}
}
