using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPurchaseOrderApprovalModel : ERPBaseModel, IERPPurchaseOrderApprovalModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPurchaseOrderApprovals(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPurchaseOrderApprovalRepository iERPPurchaseOrderApprovalRepository = (base.ERPPurchaseOrderApprovalRepository = new ERPPurchaseOrderApprovalRepository(base.ApiClientContext));
		using (iERPPurchaseOrderApprovalRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPurchaseOrderApprovalRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPurchaseOrderApprovalRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPurchaseOrderApprovalRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPurchaseOrderApprovalRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPurchaseOrderApproval(Guid purchaseOrderApprovalId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderApprovalRepository iERPPurchaseOrderApprovalRepository = (base.ERPPurchaseOrderApprovalRepository = new ERPPurchaseOrderApprovalRepository(base.ApiClientContext));
		using (iERPPurchaseOrderApprovalRepository)
		{
			if (!(await base.ERPPurchaseOrderApprovalRepository.DoesPurchaseOrderApprovalExist(purchaseOrderApprovalId)))
			{
				errorsList.Add($"PurchaseOrderApproval [{purchaseOrderApprovalId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPurchaseOrderApproval(ERPPurchaseOrderApprovalDto purchaseOrderApproval)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderApprovalRepository iERPPurchaseOrderApprovalRepository = (base.ERPPurchaseOrderApprovalRepository = new ERPPurchaseOrderApprovalRepository(base.ApiClientContext));
		using (iERPPurchaseOrderApprovalRepository)
		{
			if (!string.IsNullOrWhiteSpace(purchaseOrderApproval.pmaPurchaseOrderID) && !(await base.ERPPurchaseOrderApprovalRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { purchaseOrderApproval.pmaPurchaseOrderID })))
			{
				errorsList.Add("pmaPurchaseOrderID [" + purchaseOrderApproval.pmaPurchaseOrderID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderApproval.pmaApprovalEmployeeID) && !(await base.ERPPurchaseOrderApprovalRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { purchaseOrderApproval.pmaApprovalEmployeeID })))
			{
				errorsList.Add("pmaApprovalEmployeeID [" + purchaseOrderApproval.pmaApprovalEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPurchaseOrderApprovalDto>>> Process_GetAllPurchaseOrderApprovals(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPurchaseOrderApprovalDto> allPurchaseOrderApprovalsDto = new List<ERPPurchaseOrderApprovalDto>();
		ERPResponseMessageDto<IList<ERPPurchaseOrderApprovalDto>> result;
		try
		{
			IERPPurchaseOrderApprovalRepository iERPPurchaseOrderApprovalRepository = (base.ERPPurchaseOrderApprovalRepository = new ERPPurchaseOrderApprovalRepository(base.ApiClientContext));
			using (iERPPurchaseOrderApprovalRepository)
			{
				foreach (ERPPurchaseOrderApprovalInformationDto item2 in await base.ERPPurchaseOrderApprovalRepository.GetAllPurchaseOrderApprovals(pageSize, pageNumber, filter, orderBy))
				{
					ERPPurchaseOrderApprovalDto item = new ERPPurchaseOrderApprovalDto
					{
						pmaApprovalEmployeeID = item2.pmaApprovalEmployeeID,
						pmaCreatedBy = item2.pmaCreatedBy,
						pmaCreatedDate = item2.pmaCreatedDate,
						pmaDescription = item2.pmaDescription,
						pmaUniqueID = item2.pmaUniqueID,
						pmaPurchaseOrderID = item2.pmaPurchaseOrderID,
						pmaRowVersion = item2.pmaRowVersion,
						pmaPurchaseOrderApprovalID = item2.pmaPurchaseOrderApprovalID,
						pmaStatus = item2.pmaStatus,
						pmaStatusDate = item2.pmaStatusDate,
						CustomFields = item2.CustomFields
					};
					allPurchaseOrderApprovalsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PurchaseOrderApprovals]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPurchaseOrderApprovalDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPurchaseOrderApprovalsDto,
				RecordCount = allPurchaseOrderApprovalsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderApprovalDto>> Process_GetPurchaseOrderApproval(Guid purchaseOrderApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPurchaseOrderApprovalDto purchaseOrderApprovalDto = null;
		ERPResponseMessageDto<ERPPurchaseOrderApprovalDto> result;
		try
		{
			IERPPurchaseOrderApprovalRepository iERPPurchaseOrderApprovalRepository = (base.ERPPurchaseOrderApprovalRepository = new ERPPurchaseOrderApprovalRepository(base.ApiClientContext));
			using (iERPPurchaseOrderApprovalRepository)
			{
				ERPPurchaseOrderApprovalInformationDto eRPPurchaseOrderApprovalInformationDto = await base.ERPPurchaseOrderApprovalRepository.GetPurchaseOrderApproval(purchaseOrderApprovalId);
				purchaseOrderApprovalDto = new ERPPurchaseOrderApprovalDto
				{
					pmaApprovalEmployeeID = eRPPurchaseOrderApprovalInformationDto.pmaApprovalEmployeeID,
					pmaCreatedBy = eRPPurchaseOrderApprovalInformationDto.pmaCreatedBy,
					pmaCreatedDate = eRPPurchaseOrderApprovalInformationDto.pmaCreatedDate,
					pmaDescription = eRPPurchaseOrderApprovalInformationDto.pmaDescription,
					pmaUniqueID = eRPPurchaseOrderApprovalInformationDto.pmaUniqueID,
					pmaPurchaseOrderID = eRPPurchaseOrderApprovalInformationDto.pmaPurchaseOrderID,
					pmaRowVersion = eRPPurchaseOrderApprovalInformationDto.pmaRowVersion,
					pmaPurchaseOrderApprovalID = eRPPurchaseOrderApprovalInformationDto.pmaPurchaseOrderApprovalID,
					pmaStatus = eRPPurchaseOrderApprovalInformationDto.pmaStatus,
					pmaStatusDate = eRPPurchaseOrderApprovalInformationDto.pmaStatusDate,
					CustomFields = eRPPurchaseOrderApprovalInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PurchaseOrderApprovals []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = purchaseOrderApprovalDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderApprovalDto>> Process_PutPurchaseOrderApproval(ERPPurchaseOrderApprovalDto purchaseOrderApproval)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPurchaseOrderApprovalDto createdObject = null;
		ERPResponseMessageDto<ERPPurchaseOrderApprovalDto> result;
		try
		{
			IERPPurchaseOrderApprovalRepository iERPPurchaseOrderApprovalRepository = (base.ERPPurchaseOrderApprovalRepository = new ERPPurchaseOrderApprovalRepository(base.ApiClientContext));
			using (iERPPurchaseOrderApprovalRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPurchaseOrderApprovalRepository.SavePurchaseOrderApproval(purchaseOrderApproval);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPurchaseOrderApprovalInformationDto eRPPurchaseOrderApprovalInformationDto = await base.ERPPurchaseOrderApprovalRepository.GetPurchaseOrderApproval(purchaseOrderApproval.pmaUniqueID);
					createdObject = new ERPPurchaseOrderApprovalDto
					{
						pmaApprovalEmployeeID = eRPPurchaseOrderApprovalInformationDto.pmaApprovalEmployeeID,
						pmaCreatedBy = eRPPurchaseOrderApprovalInformationDto.pmaCreatedBy,
						pmaCreatedDate = eRPPurchaseOrderApprovalInformationDto.pmaCreatedDate,
						pmaDescription = eRPPurchaseOrderApprovalInformationDto.pmaDescription,
						pmaUniqueID = eRPPurchaseOrderApprovalInformationDto.pmaUniqueID,
						pmaPurchaseOrderID = eRPPurchaseOrderApprovalInformationDto.pmaPurchaseOrderID,
						pmaRowVersion = eRPPurchaseOrderApprovalInformationDto.pmaRowVersion,
						pmaPurchaseOrderApprovalID = eRPPurchaseOrderApprovalInformationDto.pmaPurchaseOrderApprovalID,
						pmaStatus = eRPPurchaseOrderApprovalInformationDto.pmaStatus,
						pmaStatusDate = eRPPurchaseOrderApprovalInformationDto.pmaStatusDate,
						CustomFields = eRPPurchaseOrderApprovalInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PurchaseOrderApproval [{purchaseOrderApproval.pmaUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePurchaseOrderApproval(Guid purchaseOrderApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderApprovalRepository iERPPurchaseOrderApprovalRepository = (base.ERPPurchaseOrderApprovalRepository = new ERPPurchaseOrderApprovalRepository(base.ApiClientContext));
		using (iERPPurchaseOrderApprovalRepository)
		{
			if (!(await base.ERPPurchaseOrderApprovalRepository.DoesPurchaseOrderApprovalExist(purchaseOrderApprovalId)))
			{
				base.ErrorsList.Add($"PurchaseOrderApproval [{purchaseOrderApprovalId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPurchaseOrderApprovalInformationDto eRPPurchaseOrderApprovalInformationDto = await base.ERPPurchaseOrderApprovalRepository.GetPurchaseOrderApproval(purchaseOrderApprovalId);
				string text = await base.ERPPurchaseOrderApprovalRepository.WhereUsed("PurchaseOrderApprovals", new object[2] { eRPPurchaseOrderApprovalInformationDto.pmaPurchaseOrderID, eRPPurchaseOrderApprovalInformationDto.pmaApprovalEmployeeID }, new object[2] { "pmaPurchaseOrderID", "pmaApprovalEmployeeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PurchaseOrderApproval cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderApprovalDto>> Process_DeletePurchaseOrderApproval(Guid purchaseOrderApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPurchaseOrderApprovalDto> result;
		try
		{
			IERPPurchaseOrderApprovalRepository iERPPurchaseOrderApprovalRepository = (base.ERPPurchaseOrderApprovalRepository = new ERPPurchaseOrderApprovalRepository(base.ApiClientContext));
			using (iERPPurchaseOrderApprovalRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPurchaseOrderApprovalRepository.DeleteRowFromTable("PurchaseOrderApprovals", "pma", purchaseOrderApprovalId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PurchaseOrderApproval [{purchaseOrderApprovalId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPurchaseOrderApprovalDto()
			};
		}
		return result;
	}
}
