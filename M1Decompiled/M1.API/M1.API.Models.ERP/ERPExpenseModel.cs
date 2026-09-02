using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPExpenseModel : ERPBaseModel, IERPExpenseModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllExpenses(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPExpenseRepository iERPExpenseRepository = (base.ERPExpenseRepository = new ERPExpenseRepository(base.ApiClientContext));
		using (iERPExpenseRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPExpenseRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPExpenseRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPExpenseRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPExpenseRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetExpense(Guid expenseId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPExpenseRepository iERPExpenseRepository = (base.ERPExpenseRepository = new ERPExpenseRepository(base.ApiClientContext));
		using (iERPExpenseRepository)
		{
			if (!(await base.ERPExpenseRepository.DoesExpenseExist(expenseId)))
			{
				errorsList.Add($"Expense [{expenseId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutExpense(ERPExpenseDto expense)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPExpenseRepository iERPExpenseRepository = (base.ERPExpenseRepository = new ERPExpenseRepository(base.ApiClientContext));
		using (iERPExpenseRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPExpenseDto>>> Process_GetAllExpenses(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPExpenseDto> allExpensesDto = new List<ERPExpenseDto>();
		ERPResponseMessageDto<IList<ERPExpenseDto>> result;
		try
		{
			IERPExpenseRepository iERPExpenseRepository = (base.ERPExpenseRepository = new ERPExpenseRepository(base.ApiClientContext));
			using (iERPExpenseRepository)
			{
				foreach (ERPExpenseInformationDto item2 in await base.ERPExpenseRepository.GetAllExpenses(pageSize, pageNumber, filter, orderBy))
				{
					ERPExpenseDto item = new ERPExpenseDto
					{
						lmxExpenseID = item2.lmxExpenseID,
						lmxCreatedBy = item2.lmxCreatedBy,
						lmxCreatedDate = item2.lmxCreatedDate,
						lmxDescription = item2.lmxDescription,
						lmxUniqueID = item2.lmxUniqueID,
						lmxRowVersion = item2.lmxRowVersion,
						CustomFields = item2.CustomFields
					};
					allExpensesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Expenses]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPExpenseDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allExpensesDto,
				RecordCount = allExpensesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPExpenseDto>> Process_GetExpense(Guid expenseId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPExpenseDto expenseDto = null;
		ERPResponseMessageDto<ERPExpenseDto> result;
		try
		{
			IERPExpenseRepository iERPExpenseRepository = (base.ERPExpenseRepository = new ERPExpenseRepository(base.ApiClientContext));
			using (iERPExpenseRepository)
			{
				ERPExpenseInformationDto eRPExpenseInformationDto = await base.ERPExpenseRepository.GetExpense(expenseId);
				expenseDto = new ERPExpenseDto
				{
					lmxExpenseID = eRPExpenseInformationDto.lmxExpenseID,
					lmxCreatedBy = eRPExpenseInformationDto.lmxCreatedBy,
					lmxCreatedDate = eRPExpenseInformationDto.lmxCreatedDate,
					lmxDescription = eRPExpenseInformationDto.lmxDescription,
					lmxUniqueID = eRPExpenseInformationDto.lmxUniqueID,
					lmxRowVersion = eRPExpenseInformationDto.lmxRowVersion,
					CustomFields = eRPExpenseInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Expenses []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPExpenseDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = expenseDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPExpenseDto>> Process_PutExpense(ERPExpenseDto expense)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPExpenseDto createdObject = null;
		ERPResponseMessageDto<ERPExpenseDto> result;
		try
		{
			IERPExpenseRepository iERPExpenseRepository = (base.ERPExpenseRepository = new ERPExpenseRepository(base.ApiClientContext));
			using (iERPExpenseRepository)
			{
				APIValidationInfoDto postResult = await base.ERPExpenseRepository.SaveExpense(expense);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPExpenseInformationDto eRPExpenseInformationDto = await base.ERPExpenseRepository.GetExpense(expense.lmxUniqueID);
					createdObject = new ERPExpenseDto
					{
						lmxExpenseID = eRPExpenseInformationDto.lmxExpenseID,
						lmxCreatedBy = eRPExpenseInformationDto.lmxCreatedBy,
						lmxCreatedDate = eRPExpenseInformationDto.lmxCreatedDate,
						lmxDescription = eRPExpenseInformationDto.lmxDescription,
						lmxUniqueID = eRPExpenseInformationDto.lmxUniqueID,
						lmxRowVersion = eRPExpenseInformationDto.lmxRowVersion,
						CustomFields = eRPExpenseInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Expense [{expense.lmxUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPExpenseDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteExpense(Guid expenseId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPExpenseRepository iERPExpenseRepository = (base.ERPExpenseRepository = new ERPExpenseRepository(base.ApiClientContext));
		using (iERPExpenseRepository)
		{
			if (!(await base.ERPExpenseRepository.DoesExpenseExist(expenseId)))
			{
				base.ErrorsList.Add($"Expense [{expenseId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPExpenseInformationDto eRPExpenseInformationDto = await base.ERPExpenseRepository.GetExpense(expenseId);
				string text = await base.ERPExpenseRepository.WhereUsed("Expenses", new object[1] { eRPExpenseInformationDto.lmxExpenseID }, new object[1] { "lmxExpenseID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Expense cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPExpenseDto>> Process_DeleteExpense(Guid expenseId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPExpenseDto> result;
		try
		{
			IERPExpenseRepository iERPExpenseRepository = (base.ERPExpenseRepository = new ERPExpenseRepository(base.ApiClientContext));
			using (iERPExpenseRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPExpenseRepository.DeleteRowFromTable("Expenses", "lmx", expenseId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Expense [{expenseId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPExpenseDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPExpenseDto()
			};
		}
		return result;
	}
}
