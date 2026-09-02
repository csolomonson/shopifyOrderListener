using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPExpenseAccountSplitModel : ERPBaseModel, IERPExpenseAccountSplitModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllExpenseAccountSplits(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPExpenseAccountSplitRepository iERPExpenseAccountSplitRepository = (base.ERPExpenseAccountSplitRepository = new ERPExpenseAccountSplitRepository(base.ApiClientContext));
		using (iERPExpenseAccountSplitRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPExpenseAccountSplitRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPExpenseAccountSplitRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPExpenseAccountSplitRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPExpenseAccountSplitRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetExpenseAccountSplit(Guid expenseAccountSplitId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPExpenseAccountSplitRepository iERPExpenseAccountSplitRepository = (base.ERPExpenseAccountSplitRepository = new ERPExpenseAccountSplitRepository(base.ApiClientContext));
		using (iERPExpenseAccountSplitRepository)
		{
			if (!(await base.ERPExpenseAccountSplitRepository.DoesExpenseAccountSplitExist(expenseAccountSplitId)))
			{
				errorsList.Add($"ExpenseAccountSplit [{expenseAccountSplitId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutExpenseAccountSplit(ERPExpenseAccountSplitDto expenseAccountSplit)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPExpenseAccountSplitRepository iERPExpenseAccountSplitRepository = (base.ERPExpenseAccountSplitRepository = new ERPExpenseAccountSplitRepository(base.ApiClientContext));
		using (iERPExpenseAccountSplitRepository)
		{
			if (!string.IsNullOrWhiteSpace(expenseAccountSplit.xazSupplierOrganizationID) && !(await base.ERPExpenseAccountSplitRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { expenseAccountSplit.xazSupplierOrganizationID })))
			{
				errorsList.Add("xazSupplierOrganizationID [" + expenseAccountSplit.xazSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(expenseAccountSplit.xazPartID) && !(await base.ERPExpenseAccountSplitRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { expenseAccountSplit.xazPartID })))
			{
				errorsList.Add("xazPartID [" + expenseAccountSplit.xazPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(expenseAccountSplit.xazPartRevisionID) && !(await base.ERPExpenseAccountSplitRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { expenseAccountSplit.xazPartID, expenseAccountSplit.xazPartRevisionID })))
			{
				errorsList.Add("xazPartRevisionID [" + expenseAccountSplit.xazPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(expenseAccountSplit.xazExpenseGlAccountID) && !(await base.ERPExpenseAccountSplitRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { expenseAccountSplit.xazExpenseGlAccountID })))
			{
				errorsList.Add("xazExpenseGlAccountID [" + expenseAccountSplit.xazExpenseGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(expenseAccountSplit.xazLandedCostCategoryID) && !(await base.ERPExpenseAccountSplitRepository.DoesRecordExistInTableUsingKeys("LandedCostCategories", new object[1] { "RMALANDEDCOSTCATEGORYID" }, new object[1] { expenseAccountSplit.xazLandedCostCategoryID })))
			{
				errorsList.Add("xazLandedCostCategoryID [" + expenseAccountSplit.xazLandedCostCategoryID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPExpenseAccountSplitDto>>> Process_GetAllExpenseAccountSplits(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPExpenseAccountSplitDto> allExpenseAccountSplitsDto = new List<ERPExpenseAccountSplitDto>();
		ERPResponseMessageDto<IList<ERPExpenseAccountSplitDto>> result;
		try
		{
			IERPExpenseAccountSplitRepository iERPExpenseAccountSplitRepository = (base.ERPExpenseAccountSplitRepository = new ERPExpenseAccountSplitRepository(base.ApiClientContext));
			using (iERPExpenseAccountSplitRepository)
			{
				foreach (ERPExpenseAccountSplitInformationDto item2 in await base.ERPExpenseAccountSplitRepository.GetAllExpenseAccountSplits(pageSize, pageNumber, filter, orderBy))
				{
					ERPExpenseAccountSplitDto item = new ERPExpenseAccountSplitDto
					{
						xazExpenseAccountSplitID = item2.xazExpenseAccountSplitID,
						xazCreatedBy = item2.xazCreatedBy,
						xazCreatedDate = item2.xazCreatedDate,
						xazExpenseGlAccountID = item2.xazExpenseGlAccountID,
						xazLandedCostCategoryID = item2.xazLandedCostCategoryID,
						xazPartID = item2.xazPartID,
						xazPartRevisionID = item2.xazPartRevisionID,
						xazPercent = item2.xazPercent,
						xazRowVersion = item2.xazRowVersion,
						xazSequence = item2.xazSequence,
						xazSupplierOrganizationID = item2.xazSupplierOrganizationID,
						CustomFields = item2.CustomFields
					};
					allExpenseAccountSplitsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ExpenseAccountSplits]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPExpenseAccountSplitDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allExpenseAccountSplitsDto,
				RecordCount = allExpenseAccountSplitsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPExpenseAccountSplitDto>> Process_GetExpenseAccountSplit(Guid expenseAccountSplitId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPExpenseAccountSplitDto expenseAccountSplitDto = null;
		ERPResponseMessageDto<ERPExpenseAccountSplitDto> result;
		try
		{
			IERPExpenseAccountSplitRepository iERPExpenseAccountSplitRepository = (base.ERPExpenseAccountSplitRepository = new ERPExpenseAccountSplitRepository(base.ApiClientContext));
			using (iERPExpenseAccountSplitRepository)
			{
				ERPExpenseAccountSplitInformationDto eRPExpenseAccountSplitInformationDto = await base.ERPExpenseAccountSplitRepository.GetExpenseAccountSplit(expenseAccountSplitId);
				expenseAccountSplitDto = new ERPExpenseAccountSplitDto
				{
					xazExpenseAccountSplitID = eRPExpenseAccountSplitInformationDto.xazExpenseAccountSplitID,
					xazCreatedBy = eRPExpenseAccountSplitInformationDto.xazCreatedBy,
					xazCreatedDate = eRPExpenseAccountSplitInformationDto.xazCreatedDate,
					xazExpenseGlAccountID = eRPExpenseAccountSplitInformationDto.xazExpenseGlAccountID,
					xazLandedCostCategoryID = eRPExpenseAccountSplitInformationDto.xazLandedCostCategoryID,
					xazPartID = eRPExpenseAccountSplitInformationDto.xazPartID,
					xazPartRevisionID = eRPExpenseAccountSplitInformationDto.xazPartRevisionID,
					xazPercent = eRPExpenseAccountSplitInformationDto.xazPercent,
					xazRowVersion = eRPExpenseAccountSplitInformationDto.xazRowVersion,
					xazSequence = eRPExpenseAccountSplitInformationDto.xazSequence,
					xazSupplierOrganizationID = eRPExpenseAccountSplitInformationDto.xazSupplierOrganizationID,
					CustomFields = eRPExpenseAccountSplitInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ExpenseAccountSplits []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPExpenseAccountSplitDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = expenseAccountSplitDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPExpenseAccountSplitDto>> Process_PutExpenseAccountSplit(ERPExpenseAccountSplitDto expenseAccountSplit)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPExpenseAccountSplitDto createdObject = null;
		ERPResponseMessageDto<ERPExpenseAccountSplitDto> result;
		try
		{
			IERPExpenseAccountSplitRepository iERPExpenseAccountSplitRepository = (base.ERPExpenseAccountSplitRepository = new ERPExpenseAccountSplitRepository(base.ApiClientContext));
			using (iERPExpenseAccountSplitRepository)
			{
				APIValidationInfoDto postResult = await base.ERPExpenseAccountSplitRepository.SaveExpenseAccountSplit(expenseAccountSplit);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPExpenseAccountSplitInformationDto eRPExpenseAccountSplitInformationDto = await base.ERPExpenseAccountSplitRepository.GetExpenseAccountSplit(expenseAccountSplit.xazExpenseAccountSplitID);
					createdObject = new ERPExpenseAccountSplitDto
					{
						xazExpenseAccountSplitID = eRPExpenseAccountSplitInformationDto.xazExpenseAccountSplitID,
						xazCreatedBy = eRPExpenseAccountSplitInformationDto.xazCreatedBy,
						xazCreatedDate = eRPExpenseAccountSplitInformationDto.xazCreatedDate,
						xazExpenseGlAccountID = eRPExpenseAccountSplitInformationDto.xazExpenseGlAccountID,
						xazLandedCostCategoryID = eRPExpenseAccountSplitInformationDto.xazLandedCostCategoryID,
						xazPartID = eRPExpenseAccountSplitInformationDto.xazPartID,
						xazPartRevisionID = eRPExpenseAccountSplitInformationDto.xazPartRevisionID,
						xazPercent = eRPExpenseAccountSplitInformationDto.xazPercent,
						xazRowVersion = eRPExpenseAccountSplitInformationDto.xazRowVersion,
						xazSequence = eRPExpenseAccountSplitInformationDto.xazSequence,
						xazSupplierOrganizationID = eRPExpenseAccountSplitInformationDto.xazSupplierOrganizationID,
						CustomFields = eRPExpenseAccountSplitInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ExpenseAccountSplit [{expenseAccountSplit.xazExpenseAccountSplitID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPExpenseAccountSplitDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteExpenseAccountSplit(Guid expenseAccountSplitId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPExpenseAccountSplitRepository iERPExpenseAccountSplitRepository = (base.ERPExpenseAccountSplitRepository = new ERPExpenseAccountSplitRepository(base.ApiClientContext));
		using (iERPExpenseAccountSplitRepository)
		{
			if (!(await base.ERPExpenseAccountSplitRepository.DoesExpenseAccountSplitExist(expenseAccountSplitId)))
			{
				base.ErrorsList.Add($"ExpenseAccountSplit [{expenseAccountSplitId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPExpenseAccountSplitInformationDto eRPExpenseAccountSplitInformationDto = await base.ERPExpenseAccountSplitRepository.GetExpenseAccountSplit(expenseAccountSplitId);
				string text = await base.ERPExpenseAccountSplitRepository.WhereUsed("ExpenseAccountSplits", new object[1] { eRPExpenseAccountSplitInformationDto.xazExpenseAccountSplitID }, new object[1] { "xazExpenseAccountSplitID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ExpenseAccountSplit cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPExpenseAccountSplitDto>> Process_DeleteExpenseAccountSplit(Guid expenseAccountSplitId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPExpenseAccountSplitDto> result;
		try
		{
			IERPExpenseAccountSplitRepository iERPExpenseAccountSplitRepository = (base.ERPExpenseAccountSplitRepository = new ERPExpenseAccountSplitRepository(base.ApiClientContext));
			using (iERPExpenseAccountSplitRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPExpenseAccountSplitRepository.DeleteRowFromTable("ExpenseAccountSplits", "xaz", expenseAccountSplitId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ExpenseAccountSplit [{expenseAccountSplitId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPExpenseAccountSplitDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPExpenseAccountSplitDto()
			};
		}
		return result;
	}
}
