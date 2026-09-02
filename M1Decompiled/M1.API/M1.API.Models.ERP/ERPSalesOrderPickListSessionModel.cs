using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSalesOrderPickListSessionModel : ERPBaseModel, IERPSalesOrderPickListSessionModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderPickListSessions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSalesOrderPickListSessionRepository iERPSalesOrderPickListSessionRepository = (base.ERPSalesOrderPickListSessionRepository = new ERPSalesOrderPickListSessionRepository(base.ApiClientContext));
		using (iERPSalesOrderPickListSessionRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSalesOrderPickListSessionRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSalesOrderPickListSessionRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSalesOrderPickListSessionRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSalesOrderPickListSessionRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderPickListSession(Guid salesOrderPickListSessionId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderPickListSessionRepository iERPSalesOrderPickListSessionRepository = (base.ERPSalesOrderPickListSessionRepository = new ERPSalesOrderPickListSessionRepository(base.ApiClientContext));
		using (iERPSalesOrderPickListSessionRepository)
		{
			if (!(await base.ERPSalesOrderPickListSessionRepository.DoesSalesOrderPickListSessionExist(salesOrderPickListSessionId)))
			{
				errorsList.Add($"SalesOrderPickListSession [{salesOrderPickListSessionId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderPickListSession(ERPSalesOrderPickListSessionDto salesOrderPickListSession)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderPickListSessionRepository iERPSalesOrderPickListSessionRepository = (base.ERPSalesOrderPickListSessionRepository = new ERPSalesOrderPickListSessionRepository(base.ApiClientContext));
		using (iERPSalesOrderPickListSessionRepository)
		{
			if (!string.IsNullOrWhiteSpace(salesOrderPickListSession.omsPlantDepartmentID) && !(await base.ERPSalesOrderPickListSessionRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { salesOrderPickListSession.omsPlantID, salesOrderPickListSession.omsPlantDepartmentID })))
			{
				errorsList.Add("omsPlantDepartmentID [" + salesOrderPickListSession.omsPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderPickListSession.omsPlantID) && !(await base.ERPSalesOrderPickListSessionRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { salesOrderPickListSession.omsPlantID })))
			{
				errorsList.Add("omsPlantID [" + salesOrderPickListSession.omsPlantID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSalesOrderPickListSessionDto>>> Process_GetAllSalesOrderPickListSessions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSalesOrderPickListSessionDto> allSalesOrderPickListSessionsDto = new List<ERPSalesOrderPickListSessionDto>();
		ERPResponseMessageDto<IList<ERPSalesOrderPickListSessionDto>> result;
		try
		{
			IERPSalesOrderPickListSessionRepository iERPSalesOrderPickListSessionRepository = (base.ERPSalesOrderPickListSessionRepository = new ERPSalesOrderPickListSessionRepository(base.ApiClientContext));
			using (iERPSalesOrderPickListSessionRepository)
			{
				foreach (ERPSalesOrderPickListSessionInformationDto item2 in await base.ERPSalesOrderPickListSessionRepository.GetAllSalesOrderPickListSessions(pageSize, pageNumber, filter, orderBy))
				{
					ERPSalesOrderPickListSessionDto item = new ERPSalesOrderPickListSessionDto
					{
						omsCreatedBy = item2.omsCreatedBy,
						omsCreatedDate = item2.omsCreatedDate,
						omsDevice = item2.omsDevice,
						omsUniqueID = item2.omsUniqueID,
						omsPullFromStockOnly = item2.omsPullFromStockOnly,
						omsPickListSessionID = item2.omsPickListSessionID,
						omsPlantDepartmentID = item2.omsPlantDepartmentID,
						omsPlantID = item2.omsPlantID,
						omsPostedDate = item2.omsPostedDate,
						omsRowVersion = item2.omsRowVersion,
						omsSessionDate = item2.omsSessionDate,
						omsStatus = item2.omsStatus,
						CustomFields = item2.CustomFields
					};
					allSalesOrderPickListSessionsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SalesOrderPickListSessions]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSalesOrderPickListSessionDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSalesOrderPickListSessionsDto,
				RecordCount = allSalesOrderPickListSessionsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderPickListSessionDto>> Process_GetSalesOrderPickListSession(Guid salesOrderPickListSessionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSalesOrderPickListSessionDto salesOrderPickListSessionDto = null;
		ERPResponseMessageDto<ERPSalesOrderPickListSessionDto> result;
		try
		{
			IERPSalesOrderPickListSessionRepository iERPSalesOrderPickListSessionRepository = (base.ERPSalesOrderPickListSessionRepository = new ERPSalesOrderPickListSessionRepository(base.ApiClientContext));
			using (iERPSalesOrderPickListSessionRepository)
			{
				ERPSalesOrderPickListSessionInformationDto eRPSalesOrderPickListSessionInformationDto = await base.ERPSalesOrderPickListSessionRepository.GetSalesOrderPickListSession(salesOrderPickListSessionId);
				salesOrderPickListSessionDto = new ERPSalesOrderPickListSessionDto
				{
					omsCreatedBy = eRPSalesOrderPickListSessionInformationDto.omsCreatedBy,
					omsCreatedDate = eRPSalesOrderPickListSessionInformationDto.omsCreatedDate,
					omsDevice = eRPSalesOrderPickListSessionInformationDto.omsDevice,
					omsUniqueID = eRPSalesOrderPickListSessionInformationDto.omsUniqueID,
					omsPullFromStockOnly = eRPSalesOrderPickListSessionInformationDto.omsPullFromStockOnly,
					omsPickListSessionID = eRPSalesOrderPickListSessionInformationDto.omsPickListSessionID,
					omsPlantDepartmentID = eRPSalesOrderPickListSessionInformationDto.omsPlantDepartmentID,
					omsPlantID = eRPSalesOrderPickListSessionInformationDto.omsPlantID,
					omsPostedDate = eRPSalesOrderPickListSessionInformationDto.omsPostedDate,
					omsRowVersion = eRPSalesOrderPickListSessionInformationDto.omsRowVersion,
					omsSessionDate = eRPSalesOrderPickListSessionInformationDto.omsSessionDate,
					omsStatus = eRPSalesOrderPickListSessionInformationDto.omsStatus,
					CustomFields = eRPSalesOrderPickListSessionInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SalesOrderPickListSessions []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderPickListSessionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = salesOrderPickListSessionDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderPickListSessionDto>> Process_PutSalesOrderPickListSession(ERPSalesOrderPickListSessionDto salesOrderPickListSession)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPSalesOrderPickListSessionDto createdObject = null;
		ERPResponseMessageDto<ERPSalesOrderPickListSessionDto> result;
		try
		{
			IERPSalesOrderPickListSessionRepository iERPSalesOrderPickListSessionRepository = (base.ERPSalesOrderPickListSessionRepository = new ERPSalesOrderPickListSessionRepository(base.ApiClientContext));
			using (iERPSalesOrderPickListSessionRepository)
			{
				APIValidationInfoDto postResult = await base.ERPSalesOrderPickListSessionRepository.SaveSalesOrderPickListSession(salesOrderPickListSession);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPSalesOrderPickListSessionInformationDto eRPSalesOrderPickListSessionInformationDto = await base.ERPSalesOrderPickListSessionRepository.GetSalesOrderPickListSession(salesOrderPickListSession.omsUniqueID);
					createdObject = new ERPSalesOrderPickListSessionDto
					{
						omsCreatedBy = eRPSalesOrderPickListSessionInformationDto.omsCreatedBy,
						omsCreatedDate = eRPSalesOrderPickListSessionInformationDto.omsCreatedDate,
						omsDevice = eRPSalesOrderPickListSessionInformationDto.omsDevice,
						omsUniqueID = eRPSalesOrderPickListSessionInformationDto.omsUniqueID,
						omsPullFromStockOnly = eRPSalesOrderPickListSessionInformationDto.omsPullFromStockOnly,
						omsPickListSessionID = eRPSalesOrderPickListSessionInformationDto.omsPickListSessionID,
						omsPlantDepartmentID = eRPSalesOrderPickListSessionInformationDto.omsPlantDepartmentID,
						omsPlantID = eRPSalesOrderPickListSessionInformationDto.omsPlantID,
						omsPostedDate = eRPSalesOrderPickListSessionInformationDto.omsPostedDate,
						omsRowVersion = eRPSalesOrderPickListSessionInformationDto.omsRowVersion,
						omsSessionDate = eRPSalesOrderPickListSessionInformationDto.omsSessionDate,
						omsStatus = eRPSalesOrderPickListSessionInformationDto.omsStatus,
						CustomFields = eRPSalesOrderPickListSessionInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing SalesOrderPickListSession [{salesOrderPickListSession.omsUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderPickListSessionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderPickListSession(Guid salesOrderPickListSessionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderPickListSessionRepository iERPSalesOrderPickListSessionRepository = (base.ERPSalesOrderPickListSessionRepository = new ERPSalesOrderPickListSessionRepository(base.ApiClientContext));
		using (iERPSalesOrderPickListSessionRepository)
		{
			if (!(await base.ERPSalesOrderPickListSessionRepository.DoesSalesOrderPickListSessionExist(salesOrderPickListSessionId)))
			{
				base.ErrorsList.Add($"SalesOrderPickListSession [{salesOrderPickListSessionId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPSalesOrderPickListSessionInformationDto eRPSalesOrderPickListSessionInformationDto = await base.ERPSalesOrderPickListSessionRepository.GetSalesOrderPickListSession(salesOrderPickListSessionId);
				string text = await base.ERPSalesOrderPickListSessionRepository.WhereUsed("SalesOrderPickListSessions", new object[1] { eRPSalesOrderPickListSessionInformationDto.omsPickListSessionID }, new object[1] { "omsPickListSessionID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("SalesOrderPickListSession cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderPickListSessionDto>> Process_DeleteSalesOrderPickListSession(Guid salesOrderPickListSessionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPSalesOrderPickListSessionDto> result;
		try
		{
			IERPSalesOrderPickListSessionRepository iERPSalesOrderPickListSessionRepository = (base.ERPSalesOrderPickListSessionRepository = new ERPSalesOrderPickListSessionRepository(base.ApiClientContext));
			using (iERPSalesOrderPickListSessionRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPSalesOrderPickListSessionRepository.DeleteRowFromTable("SalesOrderPickListSessions", "oms", salesOrderPickListSessionId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of SalesOrderPickListSession [{salesOrderPickListSessionId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderPickListSessionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPSalesOrderPickListSessionDto()
			};
		}
		return result;
	}
}
