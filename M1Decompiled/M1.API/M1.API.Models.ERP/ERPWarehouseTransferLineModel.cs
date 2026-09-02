using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWarehouseTransferLineModel : ERPBaseModel, IERPWarehouseTransferLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseTransferLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWarehouseTransferLineRepository iERPWarehouseTransferLineRepository = (base.ERPWarehouseTransferLineRepository = new ERPWarehouseTransferLineRepository(base.ApiClientContext));
		using (iERPWarehouseTransferLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWarehouseTransferLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWarehouseTransferLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWarehouseTransferLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWarehouseTransferLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWarehouseTransferLine(Guid warehouseTransferLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseTransferLineRepository iERPWarehouseTransferLineRepository = (base.ERPWarehouseTransferLineRepository = new ERPWarehouseTransferLineRepository(base.ApiClientContext));
		using (iERPWarehouseTransferLineRepository)
		{
			if (!(await base.ERPWarehouseTransferLineRepository.DoesWarehouseTransferLineExist(warehouseTransferLineId)))
			{
				errorsList.Add($"WarehouseTransferLine [{warehouseTransferLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutWarehouseTransferLine(ERPWarehouseTransferLineDto warehouseTransferLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseTransferLineRepository iERPWarehouseTransferLineRepository = (base.ERPWarehouseTransferLineRepository = new ERPWarehouseTransferLineRepository(base.ApiClientContext));
		using (iERPWarehouseTransferLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(warehouseTransferLine.mwlWarehouseTransferID) && !(await base.ERPWarehouseTransferLineRepository.DoesRecordExistInTableUsingKeys("WarehouseTransfers", new object[1] { "MWPWAREHOUSETRANSFERID" }, new object[1] { warehouseTransferLine.mwlWarehouseTransferID })))
			{
				errorsList.Add("mwlWarehouseTransferID [" + warehouseTransferLine.mwlWarehouseTransferID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseTransferLine.mwlPartID) && !(await base.ERPWarehouseTransferLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { warehouseTransferLine.mwlPartID })))
			{
				errorsList.Add("mwlPartID [" + warehouseTransferLine.mwlPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseTransferLine.mwlPartRevisionID) && !(await base.ERPWarehouseTransferLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { warehouseTransferLine.mwlPartID, warehouseTransferLine.mwlPartRevisionID })))
			{
				errorsList.Add("mwlPartRevisionID [" + warehouseTransferLine.mwlPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseTransferLine.mwlSourcePartBinID) && !(await base.ERPWarehouseTransferLineRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { warehouseTransferLine.mwlPartID, warehouseTransferLine.mwlPartRevisionID, warehouseTransferLine.mwlSourceWarehouseID, warehouseTransferLine.mwlSourcePartBinID })))
			{
				errorsList.Add("mwlSourcePartBinID [" + warehouseTransferLine.mwlSourcePartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseTransferLine.mwlWarehouseRequisitionID) && !(await base.ERPWarehouseTransferLineRepository.DoesRecordExistInTableUsingKeys("WarehouseRequisitions", new object[1] { "WQPWAREHOUSEREQUISITIONID" }, new object[1] { warehouseTransferLine.mwlWarehouseRequisitionID })))
			{
				errorsList.Add("mwlWarehouseRequisitionID [" + warehouseTransferLine.mwlWarehouseRequisitionID + "] not found.");
			}
			if (warehouseTransferLine.mwlWarehouseRequisitionLineID > 0 && !(await base.ERPWarehouseTransferLineRepository.DoesRecordExistInTableUsingKeys("WarehouseRequisitionLines", new object[2] { "WQLWAREHOUSEREQUISITIONID", "WQLWAREHOUSEREQUISITIONLINEID" }, new object[2] { warehouseTransferLine.mwlWarehouseRequisitionID, warehouseTransferLine.mwlWarehouseRequisitionLineID })))
			{
				errorsList.Add($"mwlWarehouseRequisitionLineID [{warehouseTransferLine.mwlWarehouseRequisitionLineID}] not found.");
			}
			if (warehouseTransferLine.mwlReverseWHTransferLineID > 0 && !(await base.ERPWarehouseTransferLineRepository.DoesRecordExistInTableUsingKeys("WarehouseTransferLines", new object[2] { "MWLWAREHOUSETRANSFERID", "MWLWAREHOUSETRANSFERLINEID" }, new object[2] { warehouseTransferLine.mwlReverseWHTransferID, warehouseTransferLine.mwlReverseWHTransferLineID })))
			{
				errorsList.Add($"mwlReverseWHTransferLineID [{warehouseTransferLine.mwlReverseWHTransferLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseTransferLine.mwlReverseWHTransferID) && !(await base.ERPWarehouseTransferLineRepository.DoesRecordExistInTableUsingKeys("WarehouseTransfers", new object[1] { "MWPWAREHOUSETRANSFERID" }, new object[1] { warehouseTransferLine.mwlReverseWHTransferID })))
			{
				errorsList.Add("mwlReverseWHTransferID [" + warehouseTransferLine.mwlReverseWHTransferID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWarehouseTransferLineDto>>> Process_GetAllWarehouseTransferLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWarehouseTransferLineDto> allWarehouseTransferLinesDto = new List<ERPWarehouseTransferLineDto>();
		ERPResponseMessageDto<IList<ERPWarehouseTransferLineDto>> result;
		try
		{
			IERPWarehouseTransferLineRepository iERPWarehouseTransferLineRepository = (base.ERPWarehouseTransferLineRepository = new ERPWarehouseTransferLineRepository(base.ApiClientContext));
			using (iERPWarehouseTransferLineRepository)
			{
				foreach (ERPWarehouseTransferLineInformationDto item2 in await base.ERPWarehouseTransferLineRepository.GetAllWarehouseTransferLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPWarehouseTransferLineDto item = new ERPWarehouseTransferLineDto
					{
						mwlCreatedBy = item2.mwlCreatedBy,
						mwlCreatedDate = item2.mwlCreatedDate,
						mwlDestinationWarehouseID = item2.mwlDestinationWarehouseID,
						mwlUniqueID = item2.mwlUniqueID,
						mwlClosed = item2.mwlClosed,
						mwlKitPart = item2.mwlKitPart,
						mwlPosted = item2.mwlPosted,
						mwlReceivedComplete = item2.mwlReceivedComplete,
						mwlReversed = item2.mwlReversed,
						mwlShippedComplete = item2.mwlShippedComplete,
						mwlPartDescription = item2.mwlPartDescription,
						mwlPartID = item2.mwlPartID,
						mwlPartRevisionID = item2.mwlPartRevisionID,
						mwlQuantityInTransit = item2.mwlQuantityInTransit,
						mwlReceivedDate = item2.mwlReceivedDate,
						mwlReceivedQuantity = item2.mwlReceivedQuantity,
						mwlReverseWHTransferID = item2.mwlReverseWHTransferID,
						mwlReverseWHTransferLineID = item2.mwlReverseWHTransferLineID,
						mwlRowVersion = item2.mwlRowVersion,
						mwlWarehouseTransferLineID = item2.mwlWarehouseTransferLineID,
						mwlShipQuantity = item2.mwlShipQuantity,
						mwlSourcePartBinID = item2.mwlSourcePartBinID,
						mwlSourceWarehouseID = item2.mwlSourceWarehouseID,
						mwlUnitOfMeasure = item2.mwlUnitOfMeasure,
						mwlWarehouseRequisitionID = item2.mwlWarehouseRequisitionID,
						mwlWarehouseRequisitionLineID = item2.mwlWarehouseRequisitionLineID,
						mwlWarehouseTransferID = item2.mwlWarehouseTransferID,
						mwlWROpenQuantity = item2.mwlWROpenQuantity,
						mwlWRRequestedQuantity = item2.mwlWRRequestedQuantity,
						CustomFields = item2.CustomFields
					};
					allWarehouseTransferLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all WarehouseTransferLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWarehouseTransferLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWarehouseTransferLinesDto,
				RecordCount = allWarehouseTransferLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseTransferLineDto>> Process_GetWarehouseTransferLine(Guid warehouseTransferLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWarehouseTransferLineDto warehouseTransferLineDto = null;
		ERPResponseMessageDto<ERPWarehouseTransferLineDto> result;
		try
		{
			IERPWarehouseTransferLineRepository iERPWarehouseTransferLineRepository = (base.ERPWarehouseTransferLineRepository = new ERPWarehouseTransferLineRepository(base.ApiClientContext));
			using (iERPWarehouseTransferLineRepository)
			{
				ERPWarehouseTransferLineInformationDto eRPWarehouseTransferLineInformationDto = await base.ERPWarehouseTransferLineRepository.GetWarehouseTransferLine(warehouseTransferLineId);
				warehouseTransferLineDto = new ERPWarehouseTransferLineDto
				{
					mwlCreatedBy = eRPWarehouseTransferLineInformationDto.mwlCreatedBy,
					mwlCreatedDate = eRPWarehouseTransferLineInformationDto.mwlCreatedDate,
					mwlDestinationWarehouseID = eRPWarehouseTransferLineInformationDto.mwlDestinationWarehouseID,
					mwlUniqueID = eRPWarehouseTransferLineInformationDto.mwlUniqueID,
					mwlClosed = eRPWarehouseTransferLineInformationDto.mwlClosed,
					mwlKitPart = eRPWarehouseTransferLineInformationDto.mwlKitPart,
					mwlPosted = eRPWarehouseTransferLineInformationDto.mwlPosted,
					mwlReceivedComplete = eRPWarehouseTransferLineInformationDto.mwlReceivedComplete,
					mwlReversed = eRPWarehouseTransferLineInformationDto.mwlReversed,
					mwlShippedComplete = eRPWarehouseTransferLineInformationDto.mwlShippedComplete,
					mwlPartDescription = eRPWarehouseTransferLineInformationDto.mwlPartDescription,
					mwlPartID = eRPWarehouseTransferLineInformationDto.mwlPartID,
					mwlPartRevisionID = eRPWarehouseTransferLineInformationDto.mwlPartRevisionID,
					mwlQuantityInTransit = eRPWarehouseTransferLineInformationDto.mwlQuantityInTransit,
					mwlReceivedDate = eRPWarehouseTransferLineInformationDto.mwlReceivedDate,
					mwlReceivedQuantity = eRPWarehouseTransferLineInformationDto.mwlReceivedQuantity,
					mwlReverseWHTransferID = eRPWarehouseTransferLineInformationDto.mwlReverseWHTransferID,
					mwlReverseWHTransferLineID = eRPWarehouseTransferLineInformationDto.mwlReverseWHTransferLineID,
					mwlRowVersion = eRPWarehouseTransferLineInformationDto.mwlRowVersion,
					mwlWarehouseTransferLineID = eRPWarehouseTransferLineInformationDto.mwlWarehouseTransferLineID,
					mwlShipQuantity = eRPWarehouseTransferLineInformationDto.mwlShipQuantity,
					mwlSourcePartBinID = eRPWarehouseTransferLineInformationDto.mwlSourcePartBinID,
					mwlSourceWarehouseID = eRPWarehouseTransferLineInformationDto.mwlSourceWarehouseID,
					mwlUnitOfMeasure = eRPWarehouseTransferLineInformationDto.mwlUnitOfMeasure,
					mwlWarehouseRequisitionID = eRPWarehouseTransferLineInformationDto.mwlWarehouseRequisitionID,
					mwlWarehouseRequisitionLineID = eRPWarehouseTransferLineInformationDto.mwlWarehouseRequisitionLineID,
					mwlWarehouseTransferID = eRPWarehouseTransferLineInformationDto.mwlWarehouseTransferID,
					mwlWROpenQuantity = eRPWarehouseTransferLineInformationDto.mwlWROpenQuantity,
					mwlWRRequestedQuantity = eRPWarehouseTransferLineInformationDto.mwlWRRequestedQuantity,
					CustomFields = eRPWarehouseTransferLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the WarehouseTransferLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseTransferLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = warehouseTransferLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseTransferLineDto>> Process_PutWarehouseTransferLine(ERPWarehouseTransferLineDto warehouseTransferLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPWarehouseTransferLineDto createdObject = null;
		ERPResponseMessageDto<ERPWarehouseTransferLineDto> result;
		try
		{
			IERPWarehouseTransferLineRepository iERPWarehouseTransferLineRepository = (base.ERPWarehouseTransferLineRepository = new ERPWarehouseTransferLineRepository(base.ApiClientContext));
			using (iERPWarehouseTransferLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPWarehouseTransferLineRepository.SaveWarehouseTransferLine(warehouseTransferLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPWarehouseTransferLineInformationDto eRPWarehouseTransferLineInformationDto = await base.ERPWarehouseTransferLineRepository.GetWarehouseTransferLine(warehouseTransferLine.mwlUniqueID);
					createdObject = new ERPWarehouseTransferLineDto
					{
						mwlCreatedBy = eRPWarehouseTransferLineInformationDto.mwlCreatedBy,
						mwlCreatedDate = eRPWarehouseTransferLineInformationDto.mwlCreatedDate,
						mwlDestinationWarehouseID = eRPWarehouseTransferLineInformationDto.mwlDestinationWarehouseID,
						mwlUniqueID = eRPWarehouseTransferLineInformationDto.mwlUniqueID,
						mwlClosed = eRPWarehouseTransferLineInformationDto.mwlClosed,
						mwlKitPart = eRPWarehouseTransferLineInformationDto.mwlKitPart,
						mwlPosted = eRPWarehouseTransferLineInformationDto.mwlPosted,
						mwlReceivedComplete = eRPWarehouseTransferLineInformationDto.mwlReceivedComplete,
						mwlReversed = eRPWarehouseTransferLineInformationDto.mwlReversed,
						mwlShippedComplete = eRPWarehouseTransferLineInformationDto.mwlShippedComplete,
						mwlPartDescription = eRPWarehouseTransferLineInformationDto.mwlPartDescription,
						mwlPartID = eRPWarehouseTransferLineInformationDto.mwlPartID,
						mwlPartRevisionID = eRPWarehouseTransferLineInformationDto.mwlPartRevisionID,
						mwlQuantityInTransit = eRPWarehouseTransferLineInformationDto.mwlQuantityInTransit,
						mwlReceivedDate = eRPWarehouseTransferLineInformationDto.mwlReceivedDate,
						mwlReceivedQuantity = eRPWarehouseTransferLineInformationDto.mwlReceivedQuantity,
						mwlReverseWHTransferID = eRPWarehouseTransferLineInformationDto.mwlReverseWHTransferID,
						mwlReverseWHTransferLineID = eRPWarehouseTransferLineInformationDto.mwlReverseWHTransferLineID,
						mwlRowVersion = eRPWarehouseTransferLineInformationDto.mwlRowVersion,
						mwlWarehouseTransferLineID = eRPWarehouseTransferLineInformationDto.mwlWarehouseTransferLineID,
						mwlShipQuantity = eRPWarehouseTransferLineInformationDto.mwlShipQuantity,
						mwlSourcePartBinID = eRPWarehouseTransferLineInformationDto.mwlSourcePartBinID,
						mwlSourceWarehouseID = eRPWarehouseTransferLineInformationDto.mwlSourceWarehouseID,
						mwlUnitOfMeasure = eRPWarehouseTransferLineInformationDto.mwlUnitOfMeasure,
						mwlWarehouseRequisitionID = eRPWarehouseTransferLineInformationDto.mwlWarehouseRequisitionID,
						mwlWarehouseRequisitionLineID = eRPWarehouseTransferLineInformationDto.mwlWarehouseRequisitionLineID,
						mwlWarehouseTransferID = eRPWarehouseTransferLineInformationDto.mwlWarehouseTransferID,
						mwlWROpenQuantity = eRPWarehouseTransferLineInformationDto.mwlWROpenQuantity,
						mwlWRRequestedQuantity = eRPWarehouseTransferLineInformationDto.mwlWRRequestedQuantity,
						CustomFields = eRPWarehouseTransferLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing WarehouseTransferLine [{warehouseTransferLine.mwlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseTransferLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseTransferLine(Guid warehouseTransferLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseTransferLineRepository iERPWarehouseTransferLineRepository = (base.ERPWarehouseTransferLineRepository = new ERPWarehouseTransferLineRepository(base.ApiClientContext));
		using (iERPWarehouseTransferLineRepository)
		{
			if (!(await base.ERPWarehouseTransferLineRepository.DoesWarehouseTransferLineExist(warehouseTransferLineId)))
			{
				base.ErrorsList.Add($"WarehouseTransferLine [{warehouseTransferLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPWarehouseTransferLineInformationDto eRPWarehouseTransferLineInformationDto = await base.ERPWarehouseTransferLineRepository.GetWarehouseTransferLine(warehouseTransferLineId);
				string text = await base.ERPWarehouseTransferLineRepository.WhereUsed("WarehouseTransferLines", new object[2] { eRPWarehouseTransferLineInformationDto.mwlWarehouseTransferID, eRPWarehouseTransferLineInformationDto.mwlWarehouseTransferLineID }, new object[2] { "mwlWarehouseTransferID", "mwlWarehouseTransferLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("WarehouseTransferLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseTransferLineDto>> Process_DeleteWarehouseTransferLine(Guid warehouseTransferLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPWarehouseTransferLineDto> result;
		try
		{
			IERPWarehouseTransferLineRepository iERPWarehouseTransferLineRepository = (base.ERPWarehouseTransferLineRepository = new ERPWarehouseTransferLineRepository(base.ApiClientContext));
			using (iERPWarehouseTransferLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPWarehouseTransferLineRepository.DeleteRowFromTable("WarehouseTransferLines", "mwl", warehouseTransferLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of WarehouseTransferLine [{warehouseTransferLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseTransferLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPWarehouseTransferLineDto()
			};
		}
		return result;
	}
}
