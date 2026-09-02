using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPContactMethodModel : ERPBaseModel, IERPContactMethodModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllContactMethods(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPContactMethodRepository iERPContactMethodRepository = (base.ERPContactMethodRepository = new ERPContactMethodRepository(base.ApiClientContext));
		using (iERPContactMethodRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPContactMethodRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPContactMethodRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPContactMethodRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPContactMethodRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetContactMethod(Guid contactMethodId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPContactMethodRepository iERPContactMethodRepository = (base.ERPContactMethodRepository = new ERPContactMethodRepository(base.ApiClientContext));
		using (iERPContactMethodRepository)
		{
			if (!(await base.ERPContactMethodRepository.DoesContactMethodExist(contactMethodId)))
			{
				errorsList.Add($"ContactMethod [{contactMethodId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutContactMethod(ERPContactMethodDto contactMethod)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPContactMethodRepository iERPContactMethodRepository = (base.ERPContactMethodRepository = new ERPContactMethodRepository(base.ApiClientContext));
		using (iERPContactMethodRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPContactMethodDto>>> Process_GetAllContactMethods(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPContactMethodDto> allContactMethodsDto = new List<ERPContactMethodDto>();
		ERPResponseMessageDto<IList<ERPContactMethodDto>> result;
		try
		{
			IERPContactMethodRepository iERPContactMethodRepository = (base.ERPContactMethodRepository = new ERPContactMethodRepository(base.ApiClientContext));
			using (iERPContactMethodRepository)
			{
				foreach (ERPContactMethodInformationDto item2 in await base.ERPContactMethodRepository.GetAllContactMethods(pageSize, pageNumber, filter, orderBy))
				{
					ERPContactMethodDto item = new ERPContactMethodDto
					{
						kbcContactMethodID = item2.kbcContactMethodID,
						kbcCreatedBy = item2.kbcCreatedBy,
						kbcCreatedDate = item2.kbcCreatedDate,
						kbcDescription = item2.kbcDescription,
						kbcUniqueID = item2.kbcUniqueID,
						kbcRowVersion = item2.kbcRowVersion,
						CustomFields = item2.CustomFields
					};
					allContactMethodsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ContactMethods]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPContactMethodDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allContactMethodsDto,
				RecordCount = allContactMethodsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPContactMethodDto>> Process_GetContactMethod(Guid contactMethodId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPContactMethodDto contactMethodDto = null;
		ERPResponseMessageDto<ERPContactMethodDto> result;
		try
		{
			IERPContactMethodRepository iERPContactMethodRepository = (base.ERPContactMethodRepository = new ERPContactMethodRepository(base.ApiClientContext));
			using (iERPContactMethodRepository)
			{
				ERPContactMethodInformationDto eRPContactMethodInformationDto = await base.ERPContactMethodRepository.GetContactMethod(contactMethodId);
				contactMethodDto = new ERPContactMethodDto
				{
					kbcContactMethodID = eRPContactMethodInformationDto.kbcContactMethodID,
					kbcCreatedBy = eRPContactMethodInformationDto.kbcCreatedBy,
					kbcCreatedDate = eRPContactMethodInformationDto.kbcCreatedDate,
					kbcDescription = eRPContactMethodInformationDto.kbcDescription,
					kbcUniqueID = eRPContactMethodInformationDto.kbcUniqueID,
					kbcRowVersion = eRPContactMethodInformationDto.kbcRowVersion,
					CustomFields = eRPContactMethodInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ContactMethods []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPContactMethodDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = contactMethodDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPContactMethodDto>> Process_PutContactMethod(ERPContactMethodDto contactMethod)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPContactMethodDto createdObject = null;
		ERPResponseMessageDto<ERPContactMethodDto> result;
		try
		{
			IERPContactMethodRepository iERPContactMethodRepository = (base.ERPContactMethodRepository = new ERPContactMethodRepository(base.ApiClientContext));
			using (iERPContactMethodRepository)
			{
				APIValidationInfoDto postResult = await base.ERPContactMethodRepository.SaveContactMethod(contactMethod);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPContactMethodInformationDto eRPContactMethodInformationDto = await base.ERPContactMethodRepository.GetContactMethod(contactMethod.kbcUniqueID);
					createdObject = new ERPContactMethodDto
					{
						kbcContactMethodID = eRPContactMethodInformationDto.kbcContactMethodID,
						kbcCreatedBy = eRPContactMethodInformationDto.kbcCreatedBy,
						kbcCreatedDate = eRPContactMethodInformationDto.kbcCreatedDate,
						kbcDescription = eRPContactMethodInformationDto.kbcDescription,
						kbcUniqueID = eRPContactMethodInformationDto.kbcUniqueID,
						kbcRowVersion = eRPContactMethodInformationDto.kbcRowVersion,
						CustomFields = eRPContactMethodInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ContactMethod [{contactMethod.kbcUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPContactMethodDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteContactMethod(Guid contactMethodId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPContactMethodRepository iERPContactMethodRepository = (base.ERPContactMethodRepository = new ERPContactMethodRepository(base.ApiClientContext));
		using (iERPContactMethodRepository)
		{
			if (!(await base.ERPContactMethodRepository.DoesContactMethodExist(contactMethodId)))
			{
				base.ErrorsList.Add($"ContactMethod [{contactMethodId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPContactMethodInformationDto eRPContactMethodInformationDto = await base.ERPContactMethodRepository.GetContactMethod(contactMethodId);
				string text = await base.ERPContactMethodRepository.WhereUsed("ContactMethods", new object[1] { eRPContactMethodInformationDto.kbcContactMethodID }, new object[1] { "kbcContactMethodID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ContactMethod cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPContactMethodDto>> Process_DeleteContactMethod(Guid contactMethodId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPContactMethodDto> result;
		try
		{
			IERPContactMethodRepository iERPContactMethodRepository = (base.ERPContactMethodRepository = new ERPContactMethodRepository(base.ApiClientContext));
			using (iERPContactMethodRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPContactMethodRepository.DeleteRowFromTable("ContactMethods", "kbc", contactMethodId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ContactMethod [{contactMethodId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPContactMethodDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPContactMethodDto()
			};
		}
		return result;
	}
}
