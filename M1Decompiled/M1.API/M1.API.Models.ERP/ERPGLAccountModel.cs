using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPGLAccountModel : ERPBaseModel, IERPGLAccountModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllGLAccounts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLAccountRepository iERPGLAccountRepository = (base.ERPGLAccountRepository = new ERPGLAccountRepository(base.ApiClientContext));
		using (iERPGLAccountRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPGLAccountRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPGLAccountRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPGLAccountRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPGLAccountRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetGLAccount(Guid gLAccountId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLAccountRepository iERPGLAccountRepository = (base.ERPGLAccountRepository = new ERPGLAccountRepository(base.ApiClientContext));
		using (iERPGLAccountRepository)
		{
			if (!(await base.ERPGLAccountRepository.DoesGLAccountExist(gLAccountId)))
			{
				errorsList.Add($"GLAccount [{gLAccountId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutGLAccount(ERPGLAccountDto gLAccount)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLAccountRepository iERPGLAccountRepository = (base.ERPGLAccountRepository = new ERPGLAccountRepository(base.ApiClientContext));
		using (iERPGLAccountRepository)
		{
			if (!string.IsNullOrWhiteSpace(gLAccount.glaGlDivisionID) && !(await base.ERPGLAccountRepository.DoesRecordExistInTableUsingKeys("GLDivisions", new object[1] { "GLVGLDIVISIONID" }, new object[1] { gLAccount.glaGlDivisionID })))
			{
				errorsList.Add("glaGlDivisionID [" + gLAccount.glaGlDivisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLAccount.glaGlChartID) && !(await base.ERPGLAccountRepository.DoesRecordExistInTableUsingKeys("GLCharts", new object[1] { "GLCGLCHARTID" }, new object[1] { gLAccount.glaGlChartID })))
			{
				errorsList.Add("glaGlChartID [" + gLAccount.glaGlChartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLAccount.glaGlDepartmentID) && !(await base.ERPGLAccountRepository.DoesRecordExistInTableUsingKeys("GLDepartments", new object[1] { "GLDGLDEPARTMENTID" }, new object[1] { gLAccount.glaGlDepartmentID })))
			{
				errorsList.Add("glaGlDepartmentID [" + gLAccount.glaGlDepartmentID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPGLAccountDto>>> Process_GetAllGLAccounts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPGLAccountDto> allGLAccountsDto = new List<ERPGLAccountDto>();
		ERPResponseMessageDto<IList<ERPGLAccountDto>> result;
		try
		{
			IERPGLAccountRepository iERPGLAccountRepository = (base.ERPGLAccountRepository = new ERPGLAccountRepository(base.ApiClientContext));
			using (iERPGLAccountRepository)
			{
				foreach (ERPGLAccountInformationDto item2 in await base.ERPGLAccountRepository.GetAllGLAccounts(pageSize, pageNumber, filter, orderBy))
				{
					ERPGLAccountDto item = new ERPGLAccountDto
					{
						glaGlAccountID = item2.glaGlAccountID,
						glaCreatedBy = item2.glaCreatedBy,
						glaCreatedDate = item2.glaCreatedDate,
						glaUniqueID = item2.glaUniqueID,
						glaExternalGlCode = item2.glaExternalGlCode,
						glaGlChartID = item2.glaGlChartID,
						glaGlDepartmentID = item2.glaGlDepartmentID,
						glaGlDivisionID = item2.glaGlDivisionID,
						glaInactiveDate = item2.glaInactiveDate,
						glaInactive = item2.glaInactive,
						glaRowVersion = item2.glaRowVersion,
						CustomFields = item2.CustomFields
					};
					allGLAccountsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all GLAccounts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPGLAccountDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allGLAccountsDto,
				RecordCount = allGLAccountsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLAccountDto>> Process_GetGLAccount(Guid gLAccountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPGLAccountDto gLAccountDto = null;
		ERPResponseMessageDto<ERPGLAccountDto> result;
		try
		{
			IERPGLAccountRepository iERPGLAccountRepository = (base.ERPGLAccountRepository = new ERPGLAccountRepository(base.ApiClientContext));
			using (iERPGLAccountRepository)
			{
				ERPGLAccountInformationDto eRPGLAccountInformationDto = await base.ERPGLAccountRepository.GetGLAccount(gLAccountId);
				gLAccountDto = new ERPGLAccountDto
				{
					glaGlAccountID = eRPGLAccountInformationDto.glaGlAccountID,
					glaCreatedBy = eRPGLAccountInformationDto.glaCreatedBy,
					glaCreatedDate = eRPGLAccountInformationDto.glaCreatedDate,
					glaUniqueID = eRPGLAccountInformationDto.glaUniqueID,
					glaExternalGlCode = eRPGLAccountInformationDto.glaExternalGlCode,
					glaGlChartID = eRPGLAccountInformationDto.glaGlChartID,
					glaGlDepartmentID = eRPGLAccountInformationDto.glaGlDepartmentID,
					glaGlDivisionID = eRPGLAccountInformationDto.glaGlDivisionID,
					glaInactiveDate = eRPGLAccountInformationDto.glaInactiveDate,
					glaInactive = eRPGLAccountInformationDto.glaInactive,
					glaRowVersion = eRPGLAccountInformationDto.glaRowVersion,
					CustomFields = eRPGLAccountInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the GLAccounts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLAccountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = gLAccountDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLAccountDto>> Process_PutGLAccount(ERPGLAccountDto gLAccount)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPGLAccountDto createdObject = null;
		ERPResponseMessageDto<ERPGLAccountDto> result;
		try
		{
			IERPGLAccountRepository iERPGLAccountRepository = (base.ERPGLAccountRepository = new ERPGLAccountRepository(base.ApiClientContext));
			using (iERPGLAccountRepository)
			{
				APIValidationInfoDto postResult = await base.ERPGLAccountRepository.SaveGLAccount(gLAccount);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPGLAccountInformationDto eRPGLAccountInformationDto = await base.ERPGLAccountRepository.GetGLAccount(gLAccount.glaUniqueID);
					createdObject = new ERPGLAccountDto
					{
						glaGlAccountID = eRPGLAccountInformationDto.glaGlAccountID,
						glaCreatedBy = eRPGLAccountInformationDto.glaCreatedBy,
						glaCreatedDate = eRPGLAccountInformationDto.glaCreatedDate,
						glaUniqueID = eRPGLAccountInformationDto.glaUniqueID,
						glaExternalGlCode = eRPGLAccountInformationDto.glaExternalGlCode,
						glaGlChartID = eRPGLAccountInformationDto.glaGlChartID,
						glaGlDepartmentID = eRPGLAccountInformationDto.glaGlDepartmentID,
						glaGlDivisionID = eRPGLAccountInformationDto.glaGlDivisionID,
						glaInactiveDate = eRPGLAccountInformationDto.glaInactiveDate,
						glaInactive = eRPGLAccountInformationDto.glaInactive,
						glaRowVersion = eRPGLAccountInformationDto.glaRowVersion,
						CustomFields = eRPGLAccountInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing GLAccount [{gLAccount.glaUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLAccountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteGLAccount(Guid gLAccountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLAccountRepository iERPGLAccountRepository = (base.ERPGLAccountRepository = new ERPGLAccountRepository(base.ApiClientContext));
		using (iERPGLAccountRepository)
		{
			if (!(await base.ERPGLAccountRepository.DoesGLAccountExist(gLAccountId)))
			{
				base.ErrorsList.Add($"GLAccount [{gLAccountId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPGLAccountInformationDto eRPGLAccountInformationDto = await base.ERPGLAccountRepository.GetGLAccount(gLAccountId);
				string text = await base.ERPGLAccountRepository.WhereUsed("GLAccounts", new object[1] { eRPGLAccountInformationDto.glaGlAccountID }, new object[1] { "glaGlAccountID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("GLAccount cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPGLAccountDto>> Process_DeleteGLAccount(Guid gLAccountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPGLAccountDto> result;
		try
		{
			IERPGLAccountRepository iERPGLAccountRepository = (base.ERPGLAccountRepository = new ERPGLAccountRepository(base.ApiClientContext));
			using (iERPGLAccountRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPGLAccountRepository.DeleteRowFromTable("GLAccounts", "gla", gLAccountId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of GLAccount [{gLAccountId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLAccountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPGLAccountDto()
			};
		}
		return result;
	}
}
