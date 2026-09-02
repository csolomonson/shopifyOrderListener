using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSalesOrderPickListLineModel : ERPBaseModel, IERPSalesOrderPickListLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderPickListLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSalesOrderPickListLineRepository iERPSalesOrderPickListLineRepository = (base.ERPSalesOrderPickListLineRepository = new ERPSalesOrderPickListLineRepository(base.ApiClientContext));
		using (iERPSalesOrderPickListLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSalesOrderPickListLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSalesOrderPickListLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSalesOrderPickListLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSalesOrderPickListLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderPickListLine(Guid salesOrderPickListLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderPickListLineRepository iERPSalesOrderPickListLineRepository = (base.ERPSalesOrderPickListLineRepository = new ERPSalesOrderPickListLineRepository(base.ApiClientContext));
		using (iERPSalesOrderPickListLineRepository)
		{
			if (!(await base.ERPSalesOrderPickListLineRepository.DoesSalesOrderPickListLineExist(salesOrderPickListLineId)))
			{
				errorsList.Add($"SalesOrderPickListLine [{salesOrderPickListLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderPickListLine(ERPSalesOrderPickListLineDto salesOrderPickListLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderPickListLineRepository iERPSalesOrderPickListLineRepository = (base.ERPSalesOrderPickListLineRepository = new ERPSalesOrderPickListLineRepository(base.ApiClientContext));
		using (iERPSalesOrderPickListLineRepository)
		{
			if (salesOrderPickListLine.omyPickListSessionID > 0 && !(await base.ERPSalesOrderPickListLineRepository.DoesRecordExistInTableUsingKeys("SalesOrderPickListSessions", new object[1] { "OMSPICKLISTSESSIONID" }, new object[1] { salesOrderPickListLine.omyPickListSessionID })))
			{
				errorsList.Add($"omyPickListSessionID [{salesOrderPickListLine.omyPickListSessionID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderPickListLine.omySalesOrderID) && !(await base.ERPSalesOrderPickListLineRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { salesOrderPickListLine.omySalesOrderID })))
			{
				errorsList.Add("omySalesOrderID [" + salesOrderPickListLine.omySalesOrderID + "] not found.");
			}
			if (salesOrderPickListLine.omySalesOrderLineID > 0 && !(await base.ERPSalesOrderPickListLineRepository.DoesRecordExistInTableUsingKeys("SalesOrderLines", new object[2] { "OMLSALESORDERID", "OMLSALESORDERLINEID" }, new object[2] { salesOrderPickListLine.omySalesOrderID, salesOrderPickListLine.omySalesOrderLineID })))
			{
				errorsList.Add($"omySalesOrderLineID [{salesOrderPickListLine.omySalesOrderLineID}] not found.");
			}
			if (salesOrderPickListLine.omySalesOrderDeliveryID > 0 && !(await base.ERPSalesOrderPickListLineRepository.DoesRecordExistInTableUsingKeys("SalesOrderDeliveries", new object[3] { "OMDSALESORDERID", "OMDSALESORDERLINEID", "OMDSALESORDERDELIVERYID" }, new object[3] { salesOrderPickListLine.omySalesOrderID, salesOrderPickListLine.omySalesOrderLineID, salesOrderPickListLine.omySalesOrderDeliveryID })))
			{
				errorsList.Add($"omySalesOrderDeliveryID [{salesOrderPickListLine.omySalesOrderDeliveryID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderPickListLine.omyPartID) && !(await base.ERPSalesOrderPickListLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { salesOrderPickListLine.omyPartID })))
			{
				errorsList.Add("omyPartID [" + salesOrderPickListLine.omyPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderPickListLine.omyPartRevisionID) && !(await base.ERPSalesOrderPickListLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { salesOrderPickListLine.omyPartID, salesOrderPickListLine.omyPartRevisionID })))
			{
				errorsList.Add("omyPartRevisionID [" + salesOrderPickListLine.omyPartRevisionID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSalesOrderPickListLineDto>>> Process_GetAllSalesOrderPickListLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSalesOrderPickListLineDto> allSalesOrderPickListLinesDto = new List<ERPSalesOrderPickListLineDto>();
		ERPResponseMessageDto<IList<ERPSalesOrderPickListLineDto>> result;
		try
		{
			IERPSalesOrderPickListLineRepository iERPSalesOrderPickListLineRepository = (base.ERPSalesOrderPickListLineRepository = new ERPSalesOrderPickListLineRepository(base.ApiClientContext));
			using (iERPSalesOrderPickListLineRepository)
			{
				foreach (ERPSalesOrderPickListLineInformationDto item2 in await base.ERPSalesOrderPickListLineRepository.GetAllSalesOrderPickListLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPSalesOrderPickListLineDto item = new ERPSalesOrderPickListLineDto
					{
						omyCreatedBy = item2.omyCreatedBy,
						omyCreatedDate = item2.omyCreatedDate,
						omyDeliveryDate = item2.omyDeliveryDate,
						omyUniqueID = item2.omyUniqueID,
						omyOpenQuantity = item2.omyOpenQuantity,
						omyPartBinID = item2.omyPartBinID,
						omyPartID = item2.omyPartID,
						omyPartRevisionID = item2.omyPartRevisionID,
						omyPartWareHouseLocationID = item2.omyPartWareHouseLocationID,
						omyPickDate = item2.omyPickDate,
						omyPickListLineID = item2.omyPickListLineID,
						omyPickListSessionID = item2.omyPickListSessionID,
						omyPickQuantity = item2.omyPickQuantity,
						omyRowVersion = item2.omyRowVersion,
						omySalesOrderDeliveryID = item2.omySalesOrderDeliveryID,
						omySalesOrderID = item2.omySalesOrderID,
						omySalesOrderLineID = item2.omySalesOrderLineID,
						omyStatus = item2.omyStatus,
						CustomFields = item2.CustomFields
					};
					allSalesOrderPickListLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SalesOrderPickListLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSalesOrderPickListLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSalesOrderPickListLinesDto,
				RecordCount = allSalesOrderPickListLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderPickListLineDto>> Process_GetSalesOrderPickListLine(Guid salesOrderPickListLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSalesOrderPickListLineDto salesOrderPickListLineDto = null;
		ERPResponseMessageDto<ERPSalesOrderPickListLineDto> result;
		try
		{
			IERPSalesOrderPickListLineRepository iERPSalesOrderPickListLineRepository = (base.ERPSalesOrderPickListLineRepository = new ERPSalesOrderPickListLineRepository(base.ApiClientContext));
			using (iERPSalesOrderPickListLineRepository)
			{
				ERPSalesOrderPickListLineInformationDto eRPSalesOrderPickListLineInformationDto = await base.ERPSalesOrderPickListLineRepository.GetSalesOrderPickListLine(salesOrderPickListLineId);
				salesOrderPickListLineDto = new ERPSalesOrderPickListLineDto
				{
					omyCreatedBy = eRPSalesOrderPickListLineInformationDto.omyCreatedBy,
					omyCreatedDate = eRPSalesOrderPickListLineInformationDto.omyCreatedDate,
					omyDeliveryDate = eRPSalesOrderPickListLineInformationDto.omyDeliveryDate,
					omyUniqueID = eRPSalesOrderPickListLineInformationDto.omyUniqueID,
					omyOpenQuantity = eRPSalesOrderPickListLineInformationDto.omyOpenQuantity,
					omyPartBinID = eRPSalesOrderPickListLineInformationDto.omyPartBinID,
					omyPartID = eRPSalesOrderPickListLineInformationDto.omyPartID,
					omyPartRevisionID = eRPSalesOrderPickListLineInformationDto.omyPartRevisionID,
					omyPartWareHouseLocationID = eRPSalesOrderPickListLineInformationDto.omyPartWareHouseLocationID,
					omyPickDate = eRPSalesOrderPickListLineInformationDto.omyPickDate,
					omyPickListLineID = eRPSalesOrderPickListLineInformationDto.omyPickListLineID,
					omyPickListSessionID = eRPSalesOrderPickListLineInformationDto.omyPickListSessionID,
					omyPickQuantity = eRPSalesOrderPickListLineInformationDto.omyPickQuantity,
					omyRowVersion = eRPSalesOrderPickListLineInformationDto.omyRowVersion,
					omySalesOrderDeliveryID = eRPSalesOrderPickListLineInformationDto.omySalesOrderDeliveryID,
					omySalesOrderID = eRPSalesOrderPickListLineInformationDto.omySalesOrderID,
					omySalesOrderLineID = eRPSalesOrderPickListLineInformationDto.omySalesOrderLineID,
					omyStatus = eRPSalesOrderPickListLineInformationDto.omyStatus,
					CustomFields = eRPSalesOrderPickListLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SalesOrderPickListLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderPickListLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = salesOrderPickListLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderPickListLineDto>> Process_PutSalesOrderPickListLine(ERPSalesOrderPickListLineDto salesOrderPickListLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPSalesOrderPickListLineDto createdObject = null;
		ERPResponseMessageDto<ERPSalesOrderPickListLineDto> result;
		try
		{
			IERPSalesOrderPickListLineRepository iERPSalesOrderPickListLineRepository = (base.ERPSalesOrderPickListLineRepository = new ERPSalesOrderPickListLineRepository(base.ApiClientContext));
			using (iERPSalesOrderPickListLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPSalesOrderPickListLineRepository.SaveSalesOrderPickListLine(salesOrderPickListLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPSalesOrderPickListLineInformationDto eRPSalesOrderPickListLineInformationDto = await base.ERPSalesOrderPickListLineRepository.GetSalesOrderPickListLine(salesOrderPickListLine.omyUniqueID);
					createdObject = new ERPSalesOrderPickListLineDto
					{
						omyCreatedBy = eRPSalesOrderPickListLineInformationDto.omyCreatedBy,
						omyCreatedDate = eRPSalesOrderPickListLineInformationDto.omyCreatedDate,
						omyDeliveryDate = eRPSalesOrderPickListLineInformationDto.omyDeliveryDate,
						omyUniqueID = eRPSalesOrderPickListLineInformationDto.omyUniqueID,
						omyOpenQuantity = eRPSalesOrderPickListLineInformationDto.omyOpenQuantity,
						omyPartBinID = eRPSalesOrderPickListLineInformationDto.omyPartBinID,
						omyPartID = eRPSalesOrderPickListLineInformationDto.omyPartID,
						omyPartRevisionID = eRPSalesOrderPickListLineInformationDto.omyPartRevisionID,
						omyPartWareHouseLocationID = eRPSalesOrderPickListLineInformationDto.omyPartWareHouseLocationID,
						omyPickDate = eRPSalesOrderPickListLineInformationDto.omyPickDate,
						omyPickListLineID = eRPSalesOrderPickListLineInformationDto.omyPickListLineID,
						omyPickListSessionID = eRPSalesOrderPickListLineInformationDto.omyPickListSessionID,
						omyPickQuantity = eRPSalesOrderPickListLineInformationDto.omyPickQuantity,
						omyRowVersion = eRPSalesOrderPickListLineInformationDto.omyRowVersion,
						omySalesOrderDeliveryID = eRPSalesOrderPickListLineInformationDto.omySalesOrderDeliveryID,
						omySalesOrderID = eRPSalesOrderPickListLineInformationDto.omySalesOrderID,
						omySalesOrderLineID = eRPSalesOrderPickListLineInformationDto.omySalesOrderLineID,
						omyStatus = eRPSalesOrderPickListLineInformationDto.omyStatus,
						CustomFields = eRPSalesOrderPickListLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing SalesOrderPickListLine [{salesOrderPickListLine.omyUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderPickListLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderPickListLine(Guid salesOrderPickListLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderPickListLineRepository iERPSalesOrderPickListLineRepository = (base.ERPSalesOrderPickListLineRepository = new ERPSalesOrderPickListLineRepository(base.ApiClientContext));
		using (iERPSalesOrderPickListLineRepository)
		{
			if (!(await base.ERPSalesOrderPickListLineRepository.DoesSalesOrderPickListLineExist(salesOrderPickListLineId)))
			{
				base.ErrorsList.Add($"SalesOrderPickListLine [{salesOrderPickListLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPSalesOrderPickListLineInformationDto eRPSalesOrderPickListLineInformationDto = await base.ERPSalesOrderPickListLineRepository.GetSalesOrderPickListLine(salesOrderPickListLineId);
				string text = await base.ERPSalesOrderPickListLineRepository.WhereUsed("SalesOrderPickListLines", new object[2] { eRPSalesOrderPickListLineInformationDto.omyPickListSessionID, eRPSalesOrderPickListLineInformationDto.omyPickListLineID }, new object[2] { "omyPickListSessionID", "omyPickListLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("SalesOrderPickListLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderPickListLineDto>> Process_DeleteSalesOrderPickListLine(Guid salesOrderPickListLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPSalesOrderPickListLineDto> result;
		try
		{
			IERPSalesOrderPickListLineRepository iERPSalesOrderPickListLineRepository = (base.ERPSalesOrderPickListLineRepository = new ERPSalesOrderPickListLineRepository(base.ApiClientContext));
			using (iERPSalesOrderPickListLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPSalesOrderPickListLineRepository.DeleteRowFromTable("SalesOrderPickListLines", "omy", salesOrderPickListLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of SalesOrderPickListLine [{salesOrderPickListLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderPickListLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPSalesOrderPickListLineDto()
			};
		}
		return result;
	}
}
