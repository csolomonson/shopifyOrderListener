using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAPInvoiceExpenseAccountModel : ERPBaseModel, IERPAPInvoiceExpenseAccountModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAPInvoiceExpenseAccounts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAPInvoiceExpenseAccountRepository iERPAPInvoiceExpenseAccountRepository = (base.ERPAPInvoiceExpenseAccountRepository = new ERPAPInvoiceExpenseAccountRepository(base.ApiClientContext));
		using (iERPAPInvoiceExpenseAccountRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAPInvoiceExpenseAccountRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAPInvoiceExpenseAccountRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAPInvoiceExpenseAccountRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAPInvoiceExpenseAccountRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAPInvoiceExpenseAccount(Guid aPInvoiceExpenseAccountId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPInvoiceExpenseAccountRepository iERPAPInvoiceExpenseAccountRepository = (base.ERPAPInvoiceExpenseAccountRepository = new ERPAPInvoiceExpenseAccountRepository(base.ApiClientContext));
		using (iERPAPInvoiceExpenseAccountRepository)
		{
			if (!(await base.ERPAPInvoiceExpenseAccountRepository.DoesAPInvoiceExpenseAccountExist(aPInvoiceExpenseAccountId)))
			{
				errorsList.Add($"APInvoiceExpenseAccount [{aPInvoiceExpenseAccountId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAPInvoiceExpenseAccount(ERPAPInvoiceExpenseAccountDto aPInvoiceExpenseAccount)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPInvoiceExpenseAccountRepository iERPAPInvoiceExpenseAccountRepository = (base.ERPAPInvoiceExpenseAccountRepository = new ERPAPInvoiceExpenseAccountRepository(base.ApiClientContext));
		using (iERPAPInvoiceExpenseAccountRepository)
		{
			if (!string.IsNullOrWhiteSpace(aPInvoiceExpenseAccount.apxApInvoiceID) && !(await base.ERPAPInvoiceExpenseAccountRepository.DoesRecordExistInTableUsingKeys("APInvoices", new object[1] { "APPAPINVOICEID" }, new object[1] { aPInvoiceExpenseAccount.apxApInvoiceID })))
			{
				errorsList.Add("apxApInvoiceID [" + aPInvoiceExpenseAccount.apxApInvoiceID + "] not found.");
			}
			if (aPInvoiceExpenseAccount.apxApInvoiceLineID > 0 && !(await base.ERPAPInvoiceExpenseAccountRepository.DoesRecordExistInTableUsingKeys("APInvoiceLines", new object[2] { "APLAPINVOICEID", "APLAPINVOICELINEID" }, new object[2] { aPInvoiceExpenseAccount.apxApInvoiceID, aPInvoiceExpenseAccount.apxApInvoiceLineID })))
			{
				errorsList.Add($"apxApInvoiceLineID [{aPInvoiceExpenseAccount.apxApInvoiceLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceExpenseAccount.apxExpenseGlAccountID) && !(await base.ERPAPInvoiceExpenseAccountRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aPInvoiceExpenseAccount.apxExpenseGlAccountID })))
			{
				errorsList.Add("apxExpenseGlAccountID [" + aPInvoiceExpenseAccount.apxExpenseGlAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAPInvoiceExpenseAccountDto>>> Process_GetAllAPInvoiceExpenseAccounts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAPInvoiceExpenseAccountDto> allAPInvoiceExpenseAccountsDto = new List<ERPAPInvoiceExpenseAccountDto>();
		ERPResponseMessageDto<IList<ERPAPInvoiceExpenseAccountDto>> result;
		try
		{
			IERPAPInvoiceExpenseAccountRepository iERPAPInvoiceExpenseAccountRepository = (base.ERPAPInvoiceExpenseAccountRepository = new ERPAPInvoiceExpenseAccountRepository(base.ApiClientContext));
			using (iERPAPInvoiceExpenseAccountRepository)
			{
				foreach (ERPAPInvoiceExpenseAccountInformationDto item2 in await base.ERPAPInvoiceExpenseAccountRepository.GetAllAPInvoiceExpenseAccounts(pageSize, pageNumber, filter, orderBy))
				{
					ERPAPInvoiceExpenseAccountDto item = new ERPAPInvoiceExpenseAccountDto
					{
						apxAmount = item2.apxAmount,
						apxApInvoiceID = item2.apxApInvoiceID,
						apxApInvoiceLineID = item2.apxApInvoiceLineID,
						apxCreatedBy = item2.apxCreatedBy,
						apxCreatedDate = item2.apxCreatedDate,
						apxUniqueID = item2.apxUniqueID,
						apxExpenseGlAccountID = item2.apxExpenseGlAccountID,
						apxPostedToGl = item2.apxPostedToGl,
						apxPercent = item2.apxPercent,
						apxRowVersion = item2.apxRowVersion,
						apxApInvoiceExpenseAccountID = item2.apxApInvoiceExpenseAccountID,
						apxSourceTableName = item2.apxSourceTableName,
						apxSourceTableUniqueID = item2.apxSourceTableUniqueID,
						CustomFields = item2.CustomFields
					};
					allAPInvoiceExpenseAccountsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all APInvoiceExpenseAccounts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAPInvoiceExpenseAccountDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAPInvoiceExpenseAccountsDto,
				RecordCount = allAPInvoiceExpenseAccountsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAPInvoiceExpenseAccountDto>> Process_GetAPInvoiceExpenseAccount(Guid aPInvoiceExpenseAccountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAPInvoiceExpenseAccountDto aPInvoiceExpenseAccountDto = null;
		ERPResponseMessageDto<ERPAPInvoiceExpenseAccountDto> result;
		try
		{
			IERPAPInvoiceExpenseAccountRepository iERPAPInvoiceExpenseAccountRepository = (base.ERPAPInvoiceExpenseAccountRepository = new ERPAPInvoiceExpenseAccountRepository(base.ApiClientContext));
			using (iERPAPInvoiceExpenseAccountRepository)
			{
				ERPAPInvoiceExpenseAccountInformationDto eRPAPInvoiceExpenseAccountInformationDto = await base.ERPAPInvoiceExpenseAccountRepository.GetAPInvoiceExpenseAccount(aPInvoiceExpenseAccountId);
				aPInvoiceExpenseAccountDto = new ERPAPInvoiceExpenseAccountDto
				{
					apxAmount = eRPAPInvoiceExpenseAccountInformationDto.apxAmount,
					apxApInvoiceID = eRPAPInvoiceExpenseAccountInformationDto.apxApInvoiceID,
					apxApInvoiceLineID = eRPAPInvoiceExpenseAccountInformationDto.apxApInvoiceLineID,
					apxCreatedBy = eRPAPInvoiceExpenseAccountInformationDto.apxCreatedBy,
					apxCreatedDate = eRPAPInvoiceExpenseAccountInformationDto.apxCreatedDate,
					apxUniqueID = eRPAPInvoiceExpenseAccountInformationDto.apxUniqueID,
					apxExpenseGlAccountID = eRPAPInvoiceExpenseAccountInformationDto.apxExpenseGlAccountID,
					apxPostedToGl = eRPAPInvoiceExpenseAccountInformationDto.apxPostedToGl,
					apxPercent = eRPAPInvoiceExpenseAccountInformationDto.apxPercent,
					apxRowVersion = eRPAPInvoiceExpenseAccountInformationDto.apxRowVersion,
					apxApInvoiceExpenseAccountID = eRPAPInvoiceExpenseAccountInformationDto.apxApInvoiceExpenseAccountID,
					apxSourceTableName = eRPAPInvoiceExpenseAccountInformationDto.apxSourceTableName,
					apxSourceTableUniqueID = eRPAPInvoiceExpenseAccountInformationDto.apxSourceTableUniqueID,
					CustomFields = eRPAPInvoiceExpenseAccountInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the APInvoiceExpenseAccounts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPInvoiceExpenseAccountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = aPInvoiceExpenseAccountDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAPInvoiceExpenseAccountDto>> Process_PutAPInvoiceExpenseAccount(ERPAPInvoiceExpenseAccountDto aPInvoiceExpenseAccount)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAPInvoiceExpenseAccountDto createdObject = null;
		ERPResponseMessageDto<ERPAPInvoiceExpenseAccountDto> result;
		try
		{
			IERPAPInvoiceExpenseAccountRepository iERPAPInvoiceExpenseAccountRepository = (base.ERPAPInvoiceExpenseAccountRepository = new ERPAPInvoiceExpenseAccountRepository(base.ApiClientContext));
			using (iERPAPInvoiceExpenseAccountRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAPInvoiceExpenseAccountRepository.SaveAPInvoiceExpenseAccount(aPInvoiceExpenseAccount);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAPInvoiceExpenseAccountInformationDto eRPAPInvoiceExpenseAccountInformationDto = await base.ERPAPInvoiceExpenseAccountRepository.GetAPInvoiceExpenseAccount(aPInvoiceExpenseAccount.apxUniqueID);
					createdObject = new ERPAPInvoiceExpenseAccountDto
					{
						apxAmount = eRPAPInvoiceExpenseAccountInformationDto.apxAmount,
						apxApInvoiceID = eRPAPInvoiceExpenseAccountInformationDto.apxApInvoiceID,
						apxApInvoiceLineID = eRPAPInvoiceExpenseAccountInformationDto.apxApInvoiceLineID,
						apxCreatedBy = eRPAPInvoiceExpenseAccountInformationDto.apxCreatedBy,
						apxCreatedDate = eRPAPInvoiceExpenseAccountInformationDto.apxCreatedDate,
						apxUniqueID = eRPAPInvoiceExpenseAccountInformationDto.apxUniqueID,
						apxExpenseGlAccountID = eRPAPInvoiceExpenseAccountInformationDto.apxExpenseGlAccountID,
						apxPostedToGl = eRPAPInvoiceExpenseAccountInformationDto.apxPostedToGl,
						apxPercent = eRPAPInvoiceExpenseAccountInformationDto.apxPercent,
						apxRowVersion = eRPAPInvoiceExpenseAccountInformationDto.apxRowVersion,
						apxApInvoiceExpenseAccountID = eRPAPInvoiceExpenseAccountInformationDto.apxApInvoiceExpenseAccountID,
						apxSourceTableName = eRPAPInvoiceExpenseAccountInformationDto.apxSourceTableName,
						apxSourceTableUniqueID = eRPAPInvoiceExpenseAccountInformationDto.apxSourceTableUniqueID,
						CustomFields = eRPAPInvoiceExpenseAccountInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing APInvoiceExpenseAccount [{aPInvoiceExpenseAccount.apxUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPInvoiceExpenseAccountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAPInvoiceExpenseAccount(Guid aPInvoiceExpenseAccountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPInvoiceExpenseAccountRepository iERPAPInvoiceExpenseAccountRepository = (base.ERPAPInvoiceExpenseAccountRepository = new ERPAPInvoiceExpenseAccountRepository(base.ApiClientContext));
		using (iERPAPInvoiceExpenseAccountRepository)
		{
			if (!(await base.ERPAPInvoiceExpenseAccountRepository.DoesAPInvoiceExpenseAccountExist(aPInvoiceExpenseAccountId)))
			{
				base.ErrorsList.Add($"APInvoiceExpenseAccount [{aPInvoiceExpenseAccountId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAPInvoiceExpenseAccountInformationDto eRPAPInvoiceExpenseAccountInformationDto = await base.ERPAPInvoiceExpenseAccountRepository.GetAPInvoiceExpenseAccount(aPInvoiceExpenseAccountId);
				string text = await base.ERPAPInvoiceExpenseAccountRepository.WhereUsed("APInvoiceExpenseAccounts", new object[3] { eRPAPInvoiceExpenseAccountInformationDto.apxApInvoiceID, eRPAPInvoiceExpenseAccountInformationDto.apxApInvoiceLineID, eRPAPInvoiceExpenseAccountInformationDto.apxApInvoiceExpenseAccountID }, new object[3] { "apxApInvoiceID", "apxApInvoiceLineID", "apxApInvoiceExpenseAccountID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("APInvoiceExpenseAccount cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAPInvoiceExpenseAccountDto>> Process_DeleteAPInvoiceExpenseAccount(Guid aPInvoiceExpenseAccountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAPInvoiceExpenseAccountDto> result;
		try
		{
			IERPAPInvoiceExpenseAccountRepository iERPAPInvoiceExpenseAccountRepository = (base.ERPAPInvoiceExpenseAccountRepository = new ERPAPInvoiceExpenseAccountRepository(base.ApiClientContext));
			using (iERPAPInvoiceExpenseAccountRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAPInvoiceExpenseAccountRepository.DeleteRowFromTable("APInvoiceExpenseAccounts", "apx", aPInvoiceExpenseAccountId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of APInvoiceExpenseAccount [{aPInvoiceExpenseAccountId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPInvoiceExpenseAccountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAPInvoiceExpenseAccountDto()
			};
		}
		return result;
	}
}
