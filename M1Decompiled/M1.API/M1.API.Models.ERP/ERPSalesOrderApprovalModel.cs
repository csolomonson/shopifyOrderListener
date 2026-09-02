using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSalesOrderApprovalModel : ERPBaseModel, IERPSalesOrderApprovalModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderApprovals(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSalesOrderApprovalRepository iERPSalesOrderApprovalRepository = (base.ERPSalesOrderApprovalRepository = new ERPSalesOrderApprovalRepository(base.ApiClientContext));
		using (iERPSalesOrderApprovalRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSalesOrderApprovalRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSalesOrderApprovalRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSalesOrderApprovalRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSalesOrderApprovalRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderApproval(Guid salesOrderApprovalId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderApprovalRepository iERPSalesOrderApprovalRepository = (base.ERPSalesOrderApprovalRepository = new ERPSalesOrderApprovalRepository(base.ApiClientContext));
		using (iERPSalesOrderApprovalRepository)
		{
			if (!(await base.ERPSalesOrderApprovalRepository.DoesSalesOrderApprovalExist(salesOrderApprovalId)))
			{
				errorsList.Add($"SalesOrderApproval [{salesOrderApprovalId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderApproval(ERPSalesOrderApprovalDto salesOrderApproval)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderApprovalRepository iERPSalesOrderApprovalRepository = (base.ERPSalesOrderApprovalRepository = new ERPSalesOrderApprovalRepository(base.ApiClientContext));
		using (iERPSalesOrderApprovalRepository)
		{
			if (!string.IsNullOrWhiteSpace(salesOrderApproval.omaSalesOrderID) && !(await base.ERPSalesOrderApprovalRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { salesOrderApproval.omaSalesOrderID })))
			{
				errorsList.Add("omaSalesOrderID [" + salesOrderApproval.omaSalesOrderID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderApproval.omaApprovalEmployeeID) && !(await base.ERPSalesOrderApprovalRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { salesOrderApproval.omaApprovalEmployeeID })))
			{
				errorsList.Add("omaApprovalEmployeeID [" + salesOrderApproval.omaApprovalEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSalesOrderApprovalDto>>> Process_GetAllSalesOrderApprovals(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSalesOrderApprovalDto> allSalesOrderApprovalsDto = new List<ERPSalesOrderApprovalDto>();
		ERPResponseMessageDto<IList<ERPSalesOrderApprovalDto>> result;
		try
		{
			IERPSalesOrderApprovalRepository iERPSalesOrderApprovalRepository = (base.ERPSalesOrderApprovalRepository = new ERPSalesOrderApprovalRepository(base.ApiClientContext));
			using (iERPSalesOrderApprovalRepository)
			{
				foreach (ERPSalesOrderApprovalInformationDto item2 in await base.ERPSalesOrderApprovalRepository.GetAllSalesOrderApprovals(pageSize, pageNumber, filter, orderBy))
				{
					ERPSalesOrderApprovalDto item = new ERPSalesOrderApprovalDto
					{
						omaApprovalEmployeeID = item2.omaApprovalEmployeeID,
						omaCreatedBy = item2.omaCreatedBy,
						omaCreatedDate = item2.omaCreatedDate,
						omaDescription = item2.omaDescription,
						omaUniqueID = item2.omaUniqueID,
						omaRowVersion = item2.omaRowVersion,
						omaSalesOrderID = item2.omaSalesOrderID,
						omaSalesOrderApprovalID = item2.omaSalesOrderApprovalID,
						omaStatus = item2.omaStatus,
						omaStatusDate = item2.omaStatusDate,
						CustomFields = item2.CustomFields
					};
					allSalesOrderApprovalsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SalesOrderApprovals]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSalesOrderApprovalDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSalesOrderApprovalsDto,
				RecordCount = allSalesOrderApprovalsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderApprovalDto>> Process_GetSalesOrderApproval(Guid salesOrderApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSalesOrderApprovalDto salesOrderApprovalDto = null;
		ERPResponseMessageDto<ERPSalesOrderApprovalDto> result;
		try
		{
			IERPSalesOrderApprovalRepository iERPSalesOrderApprovalRepository = (base.ERPSalesOrderApprovalRepository = new ERPSalesOrderApprovalRepository(base.ApiClientContext));
			using (iERPSalesOrderApprovalRepository)
			{
				ERPSalesOrderApprovalInformationDto eRPSalesOrderApprovalInformationDto = await base.ERPSalesOrderApprovalRepository.GetSalesOrderApproval(salesOrderApprovalId);
				salesOrderApprovalDto = new ERPSalesOrderApprovalDto
				{
					omaApprovalEmployeeID = eRPSalesOrderApprovalInformationDto.omaApprovalEmployeeID,
					omaCreatedBy = eRPSalesOrderApprovalInformationDto.omaCreatedBy,
					omaCreatedDate = eRPSalesOrderApprovalInformationDto.omaCreatedDate,
					omaDescription = eRPSalesOrderApprovalInformationDto.omaDescription,
					omaUniqueID = eRPSalesOrderApprovalInformationDto.omaUniqueID,
					omaRowVersion = eRPSalesOrderApprovalInformationDto.omaRowVersion,
					omaSalesOrderID = eRPSalesOrderApprovalInformationDto.omaSalesOrderID,
					omaSalesOrderApprovalID = eRPSalesOrderApprovalInformationDto.omaSalesOrderApprovalID,
					omaStatus = eRPSalesOrderApprovalInformationDto.omaStatus,
					omaStatusDate = eRPSalesOrderApprovalInformationDto.omaStatusDate,
					CustomFields = eRPSalesOrderApprovalInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SalesOrderApprovals []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = salesOrderApprovalDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderApprovalDto>> Process_PutSalesOrderApproval(ERPSalesOrderApprovalDto salesOrderApproval)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPSalesOrderApprovalDto createdObject = null;
		ERPResponseMessageDto<ERPSalesOrderApprovalDto> result;
		try
		{
			IERPSalesOrderApprovalRepository iERPSalesOrderApprovalRepository = (base.ERPSalesOrderApprovalRepository = new ERPSalesOrderApprovalRepository(base.ApiClientContext));
			using (iERPSalesOrderApprovalRepository)
			{
				APIValidationInfoDto postResult = await base.ERPSalesOrderApprovalRepository.SaveSalesOrderApproval(salesOrderApproval);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPSalesOrderApprovalInformationDto eRPSalesOrderApprovalInformationDto = await base.ERPSalesOrderApprovalRepository.GetSalesOrderApproval(salesOrderApproval.omaUniqueID);
					createdObject = new ERPSalesOrderApprovalDto
					{
						omaApprovalEmployeeID = eRPSalesOrderApprovalInformationDto.omaApprovalEmployeeID,
						omaCreatedBy = eRPSalesOrderApprovalInformationDto.omaCreatedBy,
						omaCreatedDate = eRPSalesOrderApprovalInformationDto.omaCreatedDate,
						omaDescription = eRPSalesOrderApprovalInformationDto.omaDescription,
						omaUniqueID = eRPSalesOrderApprovalInformationDto.omaUniqueID,
						omaRowVersion = eRPSalesOrderApprovalInformationDto.omaRowVersion,
						omaSalesOrderID = eRPSalesOrderApprovalInformationDto.omaSalesOrderID,
						omaSalesOrderApprovalID = eRPSalesOrderApprovalInformationDto.omaSalesOrderApprovalID,
						omaStatus = eRPSalesOrderApprovalInformationDto.omaStatus,
						omaStatusDate = eRPSalesOrderApprovalInformationDto.omaStatusDate,
						CustomFields = eRPSalesOrderApprovalInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing SalesOrderApproval [{salesOrderApproval.omaUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderApproval(Guid salesOrderApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderApprovalRepository iERPSalesOrderApprovalRepository = (base.ERPSalesOrderApprovalRepository = new ERPSalesOrderApprovalRepository(base.ApiClientContext));
		using (iERPSalesOrderApprovalRepository)
		{
			if (!(await base.ERPSalesOrderApprovalRepository.DoesSalesOrderApprovalExist(salesOrderApprovalId)))
			{
				base.ErrorsList.Add($"SalesOrderApproval [{salesOrderApprovalId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPSalesOrderApprovalInformationDto eRPSalesOrderApprovalInformationDto = await base.ERPSalesOrderApprovalRepository.GetSalesOrderApproval(salesOrderApprovalId);
				string text = await base.ERPSalesOrderApprovalRepository.WhereUsed("SalesOrderApprovals", new object[2] { eRPSalesOrderApprovalInformationDto.omaSalesOrderID, eRPSalesOrderApprovalInformationDto.omaApprovalEmployeeID }, new object[2] { "omaSalesOrderID", "omaApprovalEmployeeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("SalesOrderApproval cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderApprovalDto>> Process_DeleteSalesOrderApproval(Guid salesOrderApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPSalesOrderApprovalDto> result;
		try
		{
			IERPSalesOrderApprovalRepository iERPSalesOrderApprovalRepository = (base.ERPSalesOrderApprovalRepository = new ERPSalesOrderApprovalRepository(base.ApiClientContext));
			using (iERPSalesOrderApprovalRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPSalesOrderApprovalRepository.DeleteRowFromTable("SalesOrderApprovals", "oma", salesOrderApprovalId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of SalesOrderApproval [{salesOrderApprovalId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPSalesOrderApprovalDto()
			};
		}
		return result;
	}
}
