using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPurchasePlannerSessionModel : ERPBaseModel, IERPPurchasePlannerSessionModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPurchasePlannerSessions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPurchasePlannerSessionRepository iERPPurchasePlannerSessionRepository = (base.ERPPurchasePlannerSessionRepository = new ERPPurchasePlannerSessionRepository(base.ApiClientContext));
		using (iERPPurchasePlannerSessionRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPurchasePlannerSessionRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPurchasePlannerSessionRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPurchasePlannerSessionRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPurchasePlannerSessionRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPurchasePlannerSession(Guid purchasePlannerSessionId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchasePlannerSessionRepository iERPPurchasePlannerSessionRepository = (base.ERPPurchasePlannerSessionRepository = new ERPPurchasePlannerSessionRepository(base.ApiClientContext));
		using (iERPPurchasePlannerSessionRepository)
		{
			if (!(await base.ERPPurchasePlannerSessionRepository.DoesPurchasePlannerSessionExist(purchasePlannerSessionId)))
			{
				errorsList.Add($"PurchasePlannerSession [{purchasePlannerSessionId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPurchasePlannerSession(ERPPurchasePlannerSessionDto purchasePlannerSession)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchasePlannerSessionRepository iERPPurchasePlannerSessionRepository = (base.ERPPurchasePlannerSessionRepository = new ERPPurchasePlannerSessionRepository(base.ApiClientContext));
		using (iERPPurchasePlannerSessionRepository)
		{
			if (!string.IsNullOrWhiteSpace(purchasePlannerSession.ppsPlantID) && !(await base.ERPPurchasePlannerSessionRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { purchasePlannerSession.ppsPlantID })))
			{
				errorsList.Add("ppsPlantID [" + purchasePlannerSession.ppsPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerSession.ppsWarehouseID) && !(await base.ERPPurchasePlannerSessionRepository.DoesRecordExistInTableUsingKeys("Warehouses", new object[1] { "IMWWAREHOUSEID" }, new object[1] { purchasePlannerSession.ppsWarehouseID })))
			{
				errorsList.Add("ppsWarehouseID [" + purchasePlannerSession.ppsWarehouseID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerSession.ppsBuyerEmployeeID) && !(await base.ERPPurchasePlannerSessionRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { purchasePlannerSession.ppsBuyerEmployeeID })))
			{
				errorsList.Add("ppsBuyerEmployeeID [" + purchasePlannerSession.ppsBuyerEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPurchasePlannerSessionDto>>> Process_GetAllPurchasePlannerSessions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPurchasePlannerSessionDto> allPurchasePlannerSessionsDto = new List<ERPPurchasePlannerSessionDto>();
		ERPResponseMessageDto<IList<ERPPurchasePlannerSessionDto>> result;
		try
		{
			IERPPurchasePlannerSessionRepository iERPPurchasePlannerSessionRepository = (base.ERPPurchasePlannerSessionRepository = new ERPPurchasePlannerSessionRepository(base.ApiClientContext));
			using (iERPPurchasePlannerSessionRepository)
			{
				foreach (ERPPurchasePlannerSessionInformationDto item2 in await base.ERPPurchasePlannerSessionRepository.GetAllPurchasePlannerSessions(pageSize, pageNumber, filter, orderBy))
				{
					ERPPurchasePlannerSessionDto item = new ERPPurchasePlannerSessionDto
					{
						ppsBuyerEmployeeID = item2.ppsBuyerEmployeeID,
						ppsCompletedDate = item2.ppsCompletedDate,
						ppsCreatedBy = item2.ppsCreatedBy,
						ppsCreatedDate = item2.ppsCreatedDate,
						ppsCutoffDate = item2.ppsCutoffDate,
						ppsCutoffDatePosupply = item2.ppsCutoffDatePosupply,
						ppsUniqueID = item2.ppsUniqueID,
						ppsCalculateForAllParts = item2.ppsCalculateForAllParts,
						ppsCompleted = item2.ppsCompleted,
						ppsFirmOnly = item2.ppsFirmOnly,
						ppsGenerated = item2.ppsGenerated,
						ppsJobIDs = item2.ppsJobIDs,
						ppsPartClassIDs = item2.ppsPartClassIDs,
						ppsPartIDs = item2.ppsPartIDs,
						ppsPlantID = item2.ppsPlantID,
						ppsRowVersion = item2.ppsRowVersion,
						ppsSalesOrderIDs = item2.ppsSalesOrderIDs,
						ppsSessionID = item2.ppsSessionID,
						ppsSessionSubtotalBase = item2.ppsSessionSubtotalBase,
						ppsShowAllDemandForPartsOnJobs = item2.ppsShowAllDemandForPartsOnJobs,
						ppsSupplierIDs = item2.ppsSupplierIDs,
						ppsWarehouseID = item2.ppsWarehouseID,
						CustomFields = item2.CustomFields
					};
					allPurchasePlannerSessionsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PurchasePlannerSessions]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPurchasePlannerSessionDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPurchasePlannerSessionsDto,
				RecordCount = allPurchasePlannerSessionsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchasePlannerSessionDto>> Process_GetPurchasePlannerSession(Guid purchasePlannerSessionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPurchasePlannerSessionDto purchasePlannerSessionDto = null;
		ERPResponseMessageDto<ERPPurchasePlannerSessionDto> result;
		try
		{
			IERPPurchasePlannerSessionRepository iERPPurchasePlannerSessionRepository = (base.ERPPurchasePlannerSessionRepository = new ERPPurchasePlannerSessionRepository(base.ApiClientContext));
			using (iERPPurchasePlannerSessionRepository)
			{
				ERPPurchasePlannerSessionInformationDto eRPPurchasePlannerSessionInformationDto = await base.ERPPurchasePlannerSessionRepository.GetPurchasePlannerSession(purchasePlannerSessionId);
				purchasePlannerSessionDto = new ERPPurchasePlannerSessionDto
				{
					ppsBuyerEmployeeID = eRPPurchasePlannerSessionInformationDto.ppsBuyerEmployeeID,
					ppsCompletedDate = eRPPurchasePlannerSessionInformationDto.ppsCompletedDate,
					ppsCreatedBy = eRPPurchasePlannerSessionInformationDto.ppsCreatedBy,
					ppsCreatedDate = eRPPurchasePlannerSessionInformationDto.ppsCreatedDate,
					ppsCutoffDate = eRPPurchasePlannerSessionInformationDto.ppsCutoffDate,
					ppsCutoffDatePosupply = eRPPurchasePlannerSessionInformationDto.ppsCutoffDatePosupply,
					ppsUniqueID = eRPPurchasePlannerSessionInformationDto.ppsUniqueID,
					ppsCalculateForAllParts = eRPPurchasePlannerSessionInformationDto.ppsCalculateForAllParts,
					ppsCompleted = eRPPurchasePlannerSessionInformationDto.ppsCompleted,
					ppsFirmOnly = eRPPurchasePlannerSessionInformationDto.ppsFirmOnly,
					ppsGenerated = eRPPurchasePlannerSessionInformationDto.ppsGenerated,
					ppsJobIDs = eRPPurchasePlannerSessionInformationDto.ppsJobIDs,
					ppsPartClassIDs = eRPPurchasePlannerSessionInformationDto.ppsPartClassIDs,
					ppsPartIDs = eRPPurchasePlannerSessionInformationDto.ppsPartIDs,
					ppsPlantID = eRPPurchasePlannerSessionInformationDto.ppsPlantID,
					ppsRowVersion = eRPPurchasePlannerSessionInformationDto.ppsRowVersion,
					ppsSalesOrderIDs = eRPPurchasePlannerSessionInformationDto.ppsSalesOrderIDs,
					ppsSessionID = eRPPurchasePlannerSessionInformationDto.ppsSessionID,
					ppsSessionSubtotalBase = eRPPurchasePlannerSessionInformationDto.ppsSessionSubtotalBase,
					ppsShowAllDemandForPartsOnJobs = eRPPurchasePlannerSessionInformationDto.ppsShowAllDemandForPartsOnJobs,
					ppsSupplierIDs = eRPPurchasePlannerSessionInformationDto.ppsSupplierIDs,
					ppsWarehouseID = eRPPurchasePlannerSessionInformationDto.ppsWarehouseID,
					CustomFields = eRPPurchasePlannerSessionInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PurchasePlannerSessions []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchasePlannerSessionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = purchasePlannerSessionDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchasePlannerSessionDto>> Process_PutPurchasePlannerSession(ERPPurchasePlannerSessionDto purchasePlannerSession)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPurchasePlannerSessionDto createdObject = null;
		ERPResponseMessageDto<ERPPurchasePlannerSessionDto> result;
		try
		{
			IERPPurchasePlannerSessionRepository iERPPurchasePlannerSessionRepository = (base.ERPPurchasePlannerSessionRepository = new ERPPurchasePlannerSessionRepository(base.ApiClientContext));
			using (iERPPurchasePlannerSessionRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPurchasePlannerSessionRepository.SavePurchasePlannerSession(purchasePlannerSession);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPurchasePlannerSessionInformationDto eRPPurchasePlannerSessionInformationDto = await base.ERPPurchasePlannerSessionRepository.GetPurchasePlannerSession(purchasePlannerSession.ppsUniqueID);
					createdObject = new ERPPurchasePlannerSessionDto
					{
						ppsBuyerEmployeeID = eRPPurchasePlannerSessionInformationDto.ppsBuyerEmployeeID,
						ppsCompletedDate = eRPPurchasePlannerSessionInformationDto.ppsCompletedDate,
						ppsCreatedBy = eRPPurchasePlannerSessionInformationDto.ppsCreatedBy,
						ppsCreatedDate = eRPPurchasePlannerSessionInformationDto.ppsCreatedDate,
						ppsCutoffDate = eRPPurchasePlannerSessionInformationDto.ppsCutoffDate,
						ppsCutoffDatePosupply = eRPPurchasePlannerSessionInformationDto.ppsCutoffDatePosupply,
						ppsUniqueID = eRPPurchasePlannerSessionInformationDto.ppsUniqueID,
						ppsCalculateForAllParts = eRPPurchasePlannerSessionInformationDto.ppsCalculateForAllParts,
						ppsCompleted = eRPPurchasePlannerSessionInformationDto.ppsCompleted,
						ppsFirmOnly = eRPPurchasePlannerSessionInformationDto.ppsFirmOnly,
						ppsGenerated = eRPPurchasePlannerSessionInformationDto.ppsGenerated,
						ppsJobIDs = eRPPurchasePlannerSessionInformationDto.ppsJobIDs,
						ppsPartClassIDs = eRPPurchasePlannerSessionInformationDto.ppsPartClassIDs,
						ppsPartIDs = eRPPurchasePlannerSessionInformationDto.ppsPartIDs,
						ppsPlantID = eRPPurchasePlannerSessionInformationDto.ppsPlantID,
						ppsRowVersion = eRPPurchasePlannerSessionInformationDto.ppsRowVersion,
						ppsSalesOrderIDs = eRPPurchasePlannerSessionInformationDto.ppsSalesOrderIDs,
						ppsSessionID = eRPPurchasePlannerSessionInformationDto.ppsSessionID,
						ppsSessionSubtotalBase = eRPPurchasePlannerSessionInformationDto.ppsSessionSubtotalBase,
						ppsShowAllDemandForPartsOnJobs = eRPPurchasePlannerSessionInformationDto.ppsShowAllDemandForPartsOnJobs,
						ppsSupplierIDs = eRPPurchasePlannerSessionInformationDto.ppsSupplierIDs,
						ppsWarehouseID = eRPPurchasePlannerSessionInformationDto.ppsWarehouseID,
						CustomFields = eRPPurchasePlannerSessionInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PurchasePlannerSession [{purchasePlannerSession.ppsUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchasePlannerSessionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePurchasePlannerSession(Guid purchasePlannerSessionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchasePlannerSessionRepository iERPPurchasePlannerSessionRepository = (base.ERPPurchasePlannerSessionRepository = new ERPPurchasePlannerSessionRepository(base.ApiClientContext));
		using (iERPPurchasePlannerSessionRepository)
		{
			if (!(await base.ERPPurchasePlannerSessionRepository.DoesPurchasePlannerSessionExist(purchasePlannerSessionId)))
			{
				base.ErrorsList.Add($"PurchasePlannerSession [{purchasePlannerSessionId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPurchasePlannerSessionInformationDto eRPPurchasePlannerSessionInformationDto = await base.ERPPurchasePlannerSessionRepository.GetPurchasePlannerSession(purchasePlannerSessionId);
				string text = await base.ERPPurchasePlannerSessionRepository.WhereUsed("PurchasePlannerSessions", new object[1] { eRPPurchasePlannerSessionInformationDto.ppsSessionID }, new object[1] { "ppsSessionID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PurchasePlannerSession cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPurchasePlannerSessionDto>> Process_DeletePurchasePlannerSession(Guid purchasePlannerSessionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPurchasePlannerSessionDto> result;
		try
		{
			IERPPurchasePlannerSessionRepository iERPPurchasePlannerSessionRepository = (base.ERPPurchasePlannerSessionRepository = new ERPPurchasePlannerSessionRepository(base.ApiClientContext));
			using (iERPPurchasePlannerSessionRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPurchasePlannerSessionRepository.DeleteRowFromTable("PurchasePlannerSessions", "pps", purchasePlannerSessionId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PurchasePlannerSession [{purchasePlannerSessionId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchasePlannerSessionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPurchasePlannerSessionDto()
			};
		}
		return result;
	}
}
