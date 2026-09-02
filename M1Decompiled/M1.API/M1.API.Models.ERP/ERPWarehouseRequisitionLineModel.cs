using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWarehouseRequisitionLineModel : ERPBaseModel, IERPWarehouseRequisitionLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseRequisitionLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWarehouseRequisitionLineRepository iERPWarehouseRequisitionLineRepository = (base.ERPWarehouseRequisitionLineRepository = new ERPWarehouseRequisitionLineRepository(base.ApiClientContext));
		using (iERPWarehouseRequisitionLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWarehouseRequisitionLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWarehouseRequisitionLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWarehouseRequisitionLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWarehouseRequisitionLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWarehouseRequisitionLine(Guid warehouseRequisitionLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseRequisitionLineRepository iERPWarehouseRequisitionLineRepository = (base.ERPWarehouseRequisitionLineRepository = new ERPWarehouseRequisitionLineRepository(base.ApiClientContext));
		using (iERPWarehouseRequisitionLineRepository)
		{
			if (!(await base.ERPWarehouseRequisitionLineRepository.DoesWarehouseRequisitionLineExist(warehouseRequisitionLineId)))
			{
				errorsList.Add($"WarehouseRequisitionLine [{warehouseRequisitionLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutWarehouseRequisitionLine(ERPWarehouseRequisitionLineDto warehouseRequisitionLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseRequisitionLineRepository iERPWarehouseRequisitionLineRepository = (base.ERPWarehouseRequisitionLineRepository = new ERPWarehouseRequisitionLineRepository(base.ApiClientContext));
		using (iERPWarehouseRequisitionLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(warehouseRequisitionLine.wqlWarehouseRequisitionID) && !(await base.ERPWarehouseRequisitionLineRepository.DoesRecordExistInTableUsingKeys("WarehouseRequisitions", new object[1] { "WQPWAREHOUSEREQUISITIONID" }, new object[1] { warehouseRequisitionLine.wqlWarehouseRequisitionID })))
			{
				errorsList.Add("wqlWarehouseRequisitionID [" + warehouseRequisitionLine.wqlWarehouseRequisitionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseRequisitionLine.wqlPartID) && !(await base.ERPWarehouseRequisitionLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { warehouseRequisitionLine.wqlPartID })))
			{
				errorsList.Add("wqlPartID [" + warehouseRequisitionLine.wqlPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseRequisitionLine.wqlPartRevisionID) && !(await base.ERPWarehouseRequisitionLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { warehouseRequisitionLine.wqlPartID, warehouseRequisitionLine.wqlPartRevisionID })))
			{
				errorsList.Add("wqlPartRevisionID [" + warehouseRequisitionLine.wqlPartRevisionID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWarehouseRequisitionLineDto>>> Process_GetAllWarehouseRequisitionLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWarehouseRequisitionLineDto> allWarehouseRequisitionLinesDto = new List<ERPWarehouseRequisitionLineDto>();
		ERPResponseMessageDto<IList<ERPWarehouseRequisitionLineDto>> result;
		try
		{
			IERPWarehouseRequisitionLineRepository iERPWarehouseRequisitionLineRepository = (base.ERPWarehouseRequisitionLineRepository = new ERPWarehouseRequisitionLineRepository(base.ApiClientContext));
			using (iERPWarehouseRequisitionLineRepository)
			{
				foreach (ERPWarehouseRequisitionLineInformationDto item2 in await base.ERPWarehouseRequisitionLineRepository.GetAllWarehouseRequisitionLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPWarehouseRequisitionLineDto item = new ERPWarehouseRequisitionLineDto
					{
						wqlCreatedBy = item2.wqlCreatedBy,
						wqlCreatedDate = item2.wqlCreatedDate,
						wqlUniqueID = item2.wqlUniqueID,
						wqlClosed = item2.wqlClosed,
						wqlKitPart = item2.wqlKitPart,
						wqlTransferredComplete = item2.wqlTransferredComplete,
						wqlPartDescription = item2.wqlPartDescription,
						wqlPartID = item2.wqlPartID,
						wqlPartRevisionID = item2.wqlPartRevisionID,
						wqlQuantityTransferred = item2.wqlQuantityTransferred,
						wqlRequestedQuantity = item2.wqlRequestedQuantity,
						wqlRowVersion = item2.wqlRowVersion,
						wqlWarehouseRequisitionLineID = item2.wqlWarehouseRequisitionLineID,
						wqlSourceWarehouseID = item2.wqlSourceWarehouseID,
						wqlUnitOfMeasure = item2.wqlUnitOfMeasure,
						wqlWarehouseRequisitionID = item2.wqlWarehouseRequisitionID,
						CustomFields = item2.CustomFields
					};
					allWarehouseRequisitionLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all WarehouseRequisitionLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWarehouseRequisitionLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWarehouseRequisitionLinesDto,
				RecordCount = allWarehouseRequisitionLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseRequisitionLineDto>> Process_GetWarehouseRequisitionLine(Guid warehouseRequisitionLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWarehouseRequisitionLineDto warehouseRequisitionLineDto = null;
		ERPResponseMessageDto<ERPWarehouseRequisitionLineDto> result;
		try
		{
			IERPWarehouseRequisitionLineRepository iERPWarehouseRequisitionLineRepository = (base.ERPWarehouseRequisitionLineRepository = new ERPWarehouseRequisitionLineRepository(base.ApiClientContext));
			using (iERPWarehouseRequisitionLineRepository)
			{
				ERPWarehouseRequisitionLineInformationDto eRPWarehouseRequisitionLineInformationDto = await base.ERPWarehouseRequisitionLineRepository.GetWarehouseRequisitionLine(warehouseRequisitionLineId);
				warehouseRequisitionLineDto = new ERPWarehouseRequisitionLineDto
				{
					wqlCreatedBy = eRPWarehouseRequisitionLineInformationDto.wqlCreatedBy,
					wqlCreatedDate = eRPWarehouseRequisitionLineInformationDto.wqlCreatedDate,
					wqlUniqueID = eRPWarehouseRequisitionLineInformationDto.wqlUniqueID,
					wqlClosed = eRPWarehouseRequisitionLineInformationDto.wqlClosed,
					wqlKitPart = eRPWarehouseRequisitionLineInformationDto.wqlKitPart,
					wqlTransferredComplete = eRPWarehouseRequisitionLineInformationDto.wqlTransferredComplete,
					wqlPartDescription = eRPWarehouseRequisitionLineInformationDto.wqlPartDescription,
					wqlPartID = eRPWarehouseRequisitionLineInformationDto.wqlPartID,
					wqlPartRevisionID = eRPWarehouseRequisitionLineInformationDto.wqlPartRevisionID,
					wqlQuantityTransferred = eRPWarehouseRequisitionLineInformationDto.wqlQuantityTransferred,
					wqlRequestedQuantity = eRPWarehouseRequisitionLineInformationDto.wqlRequestedQuantity,
					wqlRowVersion = eRPWarehouseRequisitionLineInformationDto.wqlRowVersion,
					wqlWarehouseRequisitionLineID = eRPWarehouseRequisitionLineInformationDto.wqlWarehouseRequisitionLineID,
					wqlSourceWarehouseID = eRPWarehouseRequisitionLineInformationDto.wqlSourceWarehouseID,
					wqlUnitOfMeasure = eRPWarehouseRequisitionLineInformationDto.wqlUnitOfMeasure,
					wqlWarehouseRequisitionID = eRPWarehouseRequisitionLineInformationDto.wqlWarehouseRequisitionID,
					CustomFields = eRPWarehouseRequisitionLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the WarehouseRequisitionLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseRequisitionLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = warehouseRequisitionLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseRequisitionLineDto>> Process_PutWarehouseRequisitionLine(ERPWarehouseRequisitionLineDto warehouseRequisitionLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPWarehouseRequisitionLineDto createdObject = null;
		ERPResponseMessageDto<ERPWarehouseRequisitionLineDto> result;
		try
		{
			IERPWarehouseRequisitionLineRepository iERPWarehouseRequisitionLineRepository = (base.ERPWarehouseRequisitionLineRepository = new ERPWarehouseRequisitionLineRepository(base.ApiClientContext));
			using (iERPWarehouseRequisitionLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPWarehouseRequisitionLineRepository.SaveWarehouseRequisitionLine(warehouseRequisitionLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPWarehouseRequisitionLineInformationDto eRPWarehouseRequisitionLineInformationDto = await base.ERPWarehouseRequisitionLineRepository.GetWarehouseRequisitionLine(warehouseRequisitionLine.wqlUniqueID);
					createdObject = new ERPWarehouseRequisitionLineDto
					{
						wqlCreatedBy = eRPWarehouseRequisitionLineInformationDto.wqlCreatedBy,
						wqlCreatedDate = eRPWarehouseRequisitionLineInformationDto.wqlCreatedDate,
						wqlUniqueID = eRPWarehouseRequisitionLineInformationDto.wqlUniqueID,
						wqlClosed = eRPWarehouseRequisitionLineInformationDto.wqlClosed,
						wqlKitPart = eRPWarehouseRequisitionLineInformationDto.wqlKitPart,
						wqlTransferredComplete = eRPWarehouseRequisitionLineInformationDto.wqlTransferredComplete,
						wqlPartDescription = eRPWarehouseRequisitionLineInformationDto.wqlPartDescription,
						wqlPartID = eRPWarehouseRequisitionLineInformationDto.wqlPartID,
						wqlPartRevisionID = eRPWarehouseRequisitionLineInformationDto.wqlPartRevisionID,
						wqlQuantityTransferred = eRPWarehouseRequisitionLineInformationDto.wqlQuantityTransferred,
						wqlRequestedQuantity = eRPWarehouseRequisitionLineInformationDto.wqlRequestedQuantity,
						wqlRowVersion = eRPWarehouseRequisitionLineInformationDto.wqlRowVersion,
						wqlWarehouseRequisitionLineID = eRPWarehouseRequisitionLineInformationDto.wqlWarehouseRequisitionLineID,
						wqlSourceWarehouseID = eRPWarehouseRequisitionLineInformationDto.wqlSourceWarehouseID,
						wqlUnitOfMeasure = eRPWarehouseRequisitionLineInformationDto.wqlUnitOfMeasure,
						wqlWarehouseRequisitionID = eRPWarehouseRequisitionLineInformationDto.wqlWarehouseRequisitionID,
						CustomFields = eRPWarehouseRequisitionLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing WarehouseRequisitionLine [{warehouseRequisitionLine.wqlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseRequisitionLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseRequisitionLine(Guid warehouseRequisitionLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseRequisitionLineRepository iERPWarehouseRequisitionLineRepository = (base.ERPWarehouseRequisitionLineRepository = new ERPWarehouseRequisitionLineRepository(base.ApiClientContext));
		using (iERPWarehouseRequisitionLineRepository)
		{
			if (!(await base.ERPWarehouseRequisitionLineRepository.DoesWarehouseRequisitionLineExist(warehouseRequisitionLineId)))
			{
				base.ErrorsList.Add($"WarehouseRequisitionLine [{warehouseRequisitionLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPWarehouseRequisitionLineInformationDto eRPWarehouseRequisitionLineInformationDto = await base.ERPWarehouseRequisitionLineRepository.GetWarehouseRequisitionLine(warehouseRequisitionLineId);
				string text = await base.ERPWarehouseRequisitionLineRepository.WhereUsed("WarehouseRequisitionLines", new object[2] { eRPWarehouseRequisitionLineInformationDto.wqlWarehouseRequisitionID, eRPWarehouseRequisitionLineInformationDto.wqlWarehouseRequisitionLineID }, new object[2] { "wqlWarehouseRequisitionID", "wqlWarehouseRequisitionLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("WarehouseRequisitionLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseRequisitionLineDto>> Process_DeleteWarehouseRequisitionLine(Guid warehouseRequisitionLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPWarehouseRequisitionLineDto> result;
		try
		{
			IERPWarehouseRequisitionLineRepository iERPWarehouseRequisitionLineRepository = (base.ERPWarehouseRequisitionLineRepository = new ERPWarehouseRequisitionLineRepository(base.ApiClientContext));
			using (iERPWarehouseRequisitionLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPWarehouseRequisitionLineRepository.DeleteRowFromTable("WarehouseRequisitionLines", "wql", warehouseRequisitionLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of WarehouseRequisitionLine [{warehouseRequisitionLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseRequisitionLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPWarehouseRequisitionLineDto()
			};
		}
		return result;
	}
}
