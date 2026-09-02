using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPurchaseOrderAccountModel : ERPBaseModel, IERPPurchaseOrderAccountModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPurchaseOrderAccounts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPurchaseOrderAccountRepository iERPPurchaseOrderAccountRepository = (base.ERPPurchaseOrderAccountRepository = new ERPPurchaseOrderAccountRepository(base.ApiClientContext));
		using (iERPPurchaseOrderAccountRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPurchaseOrderAccountRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPurchaseOrderAccountRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPurchaseOrderAccountRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPurchaseOrderAccountRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPurchaseOrderAccount(Guid purchaseOrderAccountId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderAccountRepository iERPPurchaseOrderAccountRepository = (base.ERPPurchaseOrderAccountRepository = new ERPPurchaseOrderAccountRepository(base.ApiClientContext));
		using (iERPPurchaseOrderAccountRepository)
		{
			if (!(await base.ERPPurchaseOrderAccountRepository.DoesPurchaseOrderAccountExist(purchaseOrderAccountId)))
			{
				errorsList.Add($"PurchaseOrderAccount [{purchaseOrderAccountId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPurchaseOrderAccount(ERPPurchaseOrderAccountDto purchaseOrderAccount)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderAccountRepository iERPPurchaseOrderAccountRepository = (base.ERPPurchaseOrderAccountRepository = new ERPPurchaseOrderAccountRepository(base.ApiClientContext));
		using (iERPPurchaseOrderAccountRepository)
		{
			if (!string.IsNullOrWhiteSpace(purchaseOrderAccount.pmxPurchaseOrderID) && !(await base.ERPPurchaseOrderAccountRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { purchaseOrderAccount.pmxPurchaseOrderID })))
			{
				errorsList.Add("pmxPurchaseOrderID [" + purchaseOrderAccount.pmxPurchaseOrderID + "] not found.");
			}
			if (purchaseOrderAccount.pmxPurchaseOrderLineID > 0 && !(await base.ERPPurchaseOrderAccountRepository.DoesRecordExistInTableUsingKeys("PurchaseOrderLines", new object[2] { "PMLPURCHASEORDERID", "PMLPURCHASEORDERLINEID" }, new object[2] { purchaseOrderAccount.pmxPurchaseOrderID, purchaseOrderAccount.pmxPurchaseOrderLineID })))
			{
				errorsList.Add($"pmxPurchaseOrderLineID [{purchaseOrderAccount.pmxPurchaseOrderLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderAccount.pmxExpenseGlAccountID) && !(await base.ERPPurchaseOrderAccountRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { purchaseOrderAccount.pmxExpenseGlAccountID })))
			{
				errorsList.Add("pmxExpenseGlAccountID [" + purchaseOrderAccount.pmxExpenseGlAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPurchaseOrderAccountDto>>> Process_GetAllPurchaseOrderAccounts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPurchaseOrderAccountDto> allPurchaseOrderAccountsDto = new List<ERPPurchaseOrderAccountDto>();
		ERPResponseMessageDto<IList<ERPPurchaseOrderAccountDto>> result;
		try
		{
			IERPPurchaseOrderAccountRepository iERPPurchaseOrderAccountRepository = (base.ERPPurchaseOrderAccountRepository = new ERPPurchaseOrderAccountRepository(base.ApiClientContext));
			using (iERPPurchaseOrderAccountRepository)
			{
				foreach (ERPPurchaseOrderAccountInformationDto item2 in await base.ERPPurchaseOrderAccountRepository.GetAllPurchaseOrderAccounts(pageSize, pageNumber, filter, orderBy))
				{
					ERPPurchaseOrderAccountDto item = new ERPPurchaseOrderAccountDto
					{
						pmxAmount = item2.pmxAmount,
						pmxCreatedBy = item2.pmxCreatedBy,
						pmxCreatedDate = item2.pmxCreatedDate,
						pmxUniqueID = item2.pmxUniqueID,
						pmxExpenseGlAccountID = item2.pmxExpenseGlAccountID,
						pmxClosed = item2.pmxClosed,
						pmxPercent = item2.pmxPercent,
						pmxPurchaseOrderID = item2.pmxPurchaseOrderID,
						pmxPurchaseOrderLineID = item2.pmxPurchaseOrderLineID,
						pmxRowVersion = item2.pmxRowVersion,
						pmxPurchaseOrderAccountID = item2.pmxPurchaseOrderAccountID,
						CustomFields = item2.CustomFields
					};
					allPurchaseOrderAccountsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PurchaseOrderAccounts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPurchaseOrderAccountDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPurchaseOrderAccountsDto,
				RecordCount = allPurchaseOrderAccountsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderAccountDto>> Process_GetPurchaseOrderAccount(Guid purchaseOrderAccountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPurchaseOrderAccountDto purchaseOrderAccountDto = null;
		ERPResponseMessageDto<ERPPurchaseOrderAccountDto> result;
		try
		{
			IERPPurchaseOrderAccountRepository iERPPurchaseOrderAccountRepository = (base.ERPPurchaseOrderAccountRepository = new ERPPurchaseOrderAccountRepository(base.ApiClientContext));
			using (iERPPurchaseOrderAccountRepository)
			{
				ERPPurchaseOrderAccountInformationDto eRPPurchaseOrderAccountInformationDto = await base.ERPPurchaseOrderAccountRepository.GetPurchaseOrderAccount(purchaseOrderAccountId);
				purchaseOrderAccountDto = new ERPPurchaseOrderAccountDto
				{
					pmxAmount = eRPPurchaseOrderAccountInformationDto.pmxAmount,
					pmxCreatedBy = eRPPurchaseOrderAccountInformationDto.pmxCreatedBy,
					pmxCreatedDate = eRPPurchaseOrderAccountInformationDto.pmxCreatedDate,
					pmxUniqueID = eRPPurchaseOrderAccountInformationDto.pmxUniqueID,
					pmxExpenseGlAccountID = eRPPurchaseOrderAccountInformationDto.pmxExpenseGlAccountID,
					pmxClosed = eRPPurchaseOrderAccountInformationDto.pmxClosed,
					pmxPercent = eRPPurchaseOrderAccountInformationDto.pmxPercent,
					pmxPurchaseOrderID = eRPPurchaseOrderAccountInformationDto.pmxPurchaseOrderID,
					pmxPurchaseOrderLineID = eRPPurchaseOrderAccountInformationDto.pmxPurchaseOrderLineID,
					pmxRowVersion = eRPPurchaseOrderAccountInformationDto.pmxRowVersion,
					pmxPurchaseOrderAccountID = eRPPurchaseOrderAccountInformationDto.pmxPurchaseOrderAccountID,
					CustomFields = eRPPurchaseOrderAccountInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PurchaseOrderAccounts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderAccountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = purchaseOrderAccountDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderAccountDto>> Process_PutPurchaseOrderAccount(ERPPurchaseOrderAccountDto purchaseOrderAccount)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPurchaseOrderAccountDto createdObject = null;
		ERPResponseMessageDto<ERPPurchaseOrderAccountDto> result;
		try
		{
			IERPPurchaseOrderAccountRepository iERPPurchaseOrderAccountRepository = (base.ERPPurchaseOrderAccountRepository = new ERPPurchaseOrderAccountRepository(base.ApiClientContext));
			using (iERPPurchaseOrderAccountRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPurchaseOrderAccountRepository.SavePurchaseOrderAccount(purchaseOrderAccount);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPurchaseOrderAccountInformationDto eRPPurchaseOrderAccountInformationDto = await base.ERPPurchaseOrderAccountRepository.GetPurchaseOrderAccount(purchaseOrderAccount.pmxUniqueID);
					createdObject = new ERPPurchaseOrderAccountDto
					{
						pmxAmount = eRPPurchaseOrderAccountInformationDto.pmxAmount,
						pmxCreatedBy = eRPPurchaseOrderAccountInformationDto.pmxCreatedBy,
						pmxCreatedDate = eRPPurchaseOrderAccountInformationDto.pmxCreatedDate,
						pmxUniqueID = eRPPurchaseOrderAccountInformationDto.pmxUniqueID,
						pmxExpenseGlAccountID = eRPPurchaseOrderAccountInformationDto.pmxExpenseGlAccountID,
						pmxClosed = eRPPurchaseOrderAccountInformationDto.pmxClosed,
						pmxPercent = eRPPurchaseOrderAccountInformationDto.pmxPercent,
						pmxPurchaseOrderID = eRPPurchaseOrderAccountInformationDto.pmxPurchaseOrderID,
						pmxPurchaseOrderLineID = eRPPurchaseOrderAccountInformationDto.pmxPurchaseOrderLineID,
						pmxRowVersion = eRPPurchaseOrderAccountInformationDto.pmxRowVersion,
						pmxPurchaseOrderAccountID = eRPPurchaseOrderAccountInformationDto.pmxPurchaseOrderAccountID,
						CustomFields = eRPPurchaseOrderAccountInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PurchaseOrderAccount [{purchaseOrderAccount.pmxUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderAccountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePurchaseOrderAccount(Guid purchaseOrderAccountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderAccountRepository iERPPurchaseOrderAccountRepository = (base.ERPPurchaseOrderAccountRepository = new ERPPurchaseOrderAccountRepository(base.ApiClientContext));
		using (iERPPurchaseOrderAccountRepository)
		{
			if (!(await base.ERPPurchaseOrderAccountRepository.DoesPurchaseOrderAccountExist(purchaseOrderAccountId)))
			{
				base.ErrorsList.Add($"PurchaseOrderAccount [{purchaseOrderAccountId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPurchaseOrderAccountInformationDto eRPPurchaseOrderAccountInformationDto = await base.ERPPurchaseOrderAccountRepository.GetPurchaseOrderAccount(purchaseOrderAccountId);
				string text = await base.ERPPurchaseOrderAccountRepository.WhereUsed("PurchaseOrderAccounts", new object[3] { eRPPurchaseOrderAccountInformationDto.pmxPurchaseOrderID, eRPPurchaseOrderAccountInformationDto.pmxPurchaseOrderLineID, eRPPurchaseOrderAccountInformationDto.pmxPurchaseOrderAccountID }, new object[3] { "pmxPurchaseOrderID", "pmxPurchaseOrderLineID", "pmxPurchaseOrderAccountID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PurchaseOrderAccount cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderAccountDto>> Process_DeletePurchaseOrderAccount(Guid purchaseOrderAccountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPurchaseOrderAccountDto> result;
		try
		{
			IERPPurchaseOrderAccountRepository iERPPurchaseOrderAccountRepository = (base.ERPPurchaseOrderAccountRepository = new ERPPurchaseOrderAccountRepository(base.ApiClientContext));
			using (iERPPurchaseOrderAccountRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPurchaseOrderAccountRepository.DeleteRowFromTable("PurchaseOrderAccounts", "pmx", purchaseOrderAccountId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PurchaseOrderAccount [{purchaseOrderAccountId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderAccountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPurchaseOrderAccountDto()
			};
		}
		return result;
	}
}
