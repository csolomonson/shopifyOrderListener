using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPInventoryCountLineModel : ERPBaseModel, IERPInventoryCountLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllInventoryCountLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPInventoryCountLineRepository iERPInventoryCountLineRepository = (base.ERPInventoryCountLineRepository = new ERPInventoryCountLineRepository(base.ApiClientContext));
		using (iERPInventoryCountLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPInventoryCountLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPInventoryCountLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPInventoryCountLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPInventoryCountLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetInventoryCountLine(Guid inventoryCountLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInventoryCountLineRepository iERPInventoryCountLineRepository = (base.ERPInventoryCountLineRepository = new ERPInventoryCountLineRepository(base.ApiClientContext));
		using (iERPInventoryCountLineRepository)
		{
			if (!(await base.ERPInventoryCountLineRepository.DoesInventoryCountLineExist(inventoryCountLineId)))
			{
				errorsList.Add($"InventoryCountLine [{inventoryCountLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutInventoryCountLine(ERPInventoryCountLineDto inventoryCountLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInventoryCountLineRepository iERPInventoryCountLineRepository = (base.ERPInventoryCountLineRepository = new ERPInventoryCountLineRepository(base.ApiClientContext));
		using (iERPInventoryCountLineRepository)
		{
			if (inventoryCountLine.imqInventoryCountID > 0 && !(await base.ERPInventoryCountLineRepository.DoesRecordExistInTableUsingKeys("InventoryCounts", new object[1] { "IMNINVENTORYCOUNTID" }, new object[1] { inventoryCountLine.imqInventoryCountID })))
			{
				errorsList.Add($"imqInventoryCountID [{inventoryCountLine.imqInventoryCountID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inventoryCountLine.imqPartID) && !(await base.ERPInventoryCountLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { inventoryCountLine.imqPartID })))
			{
				errorsList.Add("imqPartID [" + inventoryCountLine.imqPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inventoryCountLine.imqPartRevisionID) && !(await base.ERPInventoryCountLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { inventoryCountLine.imqPartID, inventoryCountLine.imqPartRevisionID })))
			{
				errorsList.Add("imqPartRevisionID [" + inventoryCountLine.imqPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inventoryCountLine.imqPartWarehouseLocationID) && !(await base.ERPInventoryCountLineRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { inventoryCountLine.imqPartID, inventoryCountLine.imqPartRevisionID, inventoryCountLine.imqPartWarehouseLocationID })))
			{
				errorsList.Add("imqPartWarehouseLocationID [" + inventoryCountLine.imqPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inventoryCountLine.imqPartBinID) && !(await base.ERPInventoryCountLineRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { inventoryCountLine.imqPartID, inventoryCountLine.imqPartRevisionID, inventoryCountLine.imqPartWarehouseLocationID, inventoryCountLine.imqPartBinID })))
			{
				errorsList.Add("imqPartBinID [" + inventoryCountLine.imqPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inventoryCountLine.imqPartClassID) && !(await base.ERPInventoryCountLineRepository.DoesRecordExistInTableUsingKeys("PartClasses", new object[1] { "IMCPARTCLASSID" }, new object[1] { inventoryCountLine.imqPartClassID })))
			{
				errorsList.Add("imqPartClassID [" + inventoryCountLine.imqPartClassID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPInventoryCountLineDto>>> Process_GetAllInventoryCountLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPInventoryCountLineDto> allInventoryCountLinesDto = new List<ERPInventoryCountLineDto>();
		ERPResponseMessageDto<IList<ERPInventoryCountLineDto>> result;
		try
		{
			IERPInventoryCountLineRepository iERPInventoryCountLineRepository = (base.ERPInventoryCountLineRepository = new ERPInventoryCountLineRepository(base.ApiClientContext));
			using (iERPInventoryCountLineRepository)
			{
				foreach (ERPInventoryCountLineInformationDto item2 in await base.ERPInventoryCountLineRepository.GetAllInventoryCountLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPInventoryCountLineDto item = new ERPInventoryCountLineDto
					{
						imqBinDescription = item2.imqBinDescription,
						imqCountedBy = item2.imqCountedBy,
						imqCountedDate = item2.imqCountedDate,
						imqCreatedBy = item2.imqCreatedBy,
						imqCreatedDate = item2.imqCreatedDate,
						imqUniqueID = item2.imqUniqueID,
						imqFinalCount = item2.imqFinalCount,
						imqInventoryCountID = item2.imqInventoryCountID,
						imqPartBinID = item2.imqPartBinID,
						imqPartClassID = item2.imqPartClassID,
						imqPartID = item2.imqPartID,
						imqPartRevisionID = item2.imqPartRevisionID,
						imqPartShortDescription = item2.imqPartShortDescription,
						imqPartWarehouseLocationID = item2.imqPartWarehouseLocationID,
						imqQuantityOnHand = item2.imqQuantityOnHand,
						imqRowVersion = item2.imqRowVersion,
						imqInventoryCountLineID = item2.imqInventoryCountLineID,
						CustomFields = item2.CustomFields
					};
					allInventoryCountLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all InventoryCountLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPInventoryCountLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allInventoryCountLinesDto,
				RecordCount = allInventoryCountLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPInventoryCountLineDto>> Process_GetInventoryCountLine(Guid inventoryCountLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPInventoryCountLineDto inventoryCountLineDto = null;
		ERPResponseMessageDto<ERPInventoryCountLineDto> result;
		try
		{
			IERPInventoryCountLineRepository iERPInventoryCountLineRepository = (base.ERPInventoryCountLineRepository = new ERPInventoryCountLineRepository(base.ApiClientContext));
			using (iERPInventoryCountLineRepository)
			{
				ERPInventoryCountLineInformationDto eRPInventoryCountLineInformationDto = await base.ERPInventoryCountLineRepository.GetInventoryCountLine(inventoryCountLineId);
				inventoryCountLineDto = new ERPInventoryCountLineDto
				{
					imqBinDescription = eRPInventoryCountLineInformationDto.imqBinDescription,
					imqCountedBy = eRPInventoryCountLineInformationDto.imqCountedBy,
					imqCountedDate = eRPInventoryCountLineInformationDto.imqCountedDate,
					imqCreatedBy = eRPInventoryCountLineInformationDto.imqCreatedBy,
					imqCreatedDate = eRPInventoryCountLineInformationDto.imqCreatedDate,
					imqUniqueID = eRPInventoryCountLineInformationDto.imqUniqueID,
					imqFinalCount = eRPInventoryCountLineInformationDto.imqFinalCount,
					imqInventoryCountID = eRPInventoryCountLineInformationDto.imqInventoryCountID,
					imqPartBinID = eRPInventoryCountLineInformationDto.imqPartBinID,
					imqPartClassID = eRPInventoryCountLineInformationDto.imqPartClassID,
					imqPartID = eRPInventoryCountLineInformationDto.imqPartID,
					imqPartRevisionID = eRPInventoryCountLineInformationDto.imqPartRevisionID,
					imqPartShortDescription = eRPInventoryCountLineInformationDto.imqPartShortDescription,
					imqPartWarehouseLocationID = eRPInventoryCountLineInformationDto.imqPartWarehouseLocationID,
					imqQuantityOnHand = eRPInventoryCountLineInformationDto.imqQuantityOnHand,
					imqRowVersion = eRPInventoryCountLineInformationDto.imqRowVersion,
					imqInventoryCountLineID = eRPInventoryCountLineInformationDto.imqInventoryCountLineID,
					CustomFields = eRPInventoryCountLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the InventoryCountLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInventoryCountLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = inventoryCountLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPInventoryCountLineDto>> Process_PutInventoryCountLine(ERPInventoryCountLineDto inventoryCountLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPInventoryCountLineDto createdObject = null;
		ERPResponseMessageDto<ERPInventoryCountLineDto> result;
		try
		{
			IERPInventoryCountLineRepository iERPInventoryCountLineRepository = (base.ERPInventoryCountLineRepository = new ERPInventoryCountLineRepository(base.ApiClientContext));
			using (iERPInventoryCountLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPInventoryCountLineRepository.SaveInventoryCountLine(inventoryCountLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPInventoryCountLineInformationDto eRPInventoryCountLineInformationDto = await base.ERPInventoryCountLineRepository.GetInventoryCountLine(inventoryCountLine.imqUniqueID);
					createdObject = new ERPInventoryCountLineDto
					{
						imqBinDescription = eRPInventoryCountLineInformationDto.imqBinDescription,
						imqCountedBy = eRPInventoryCountLineInformationDto.imqCountedBy,
						imqCountedDate = eRPInventoryCountLineInformationDto.imqCountedDate,
						imqCreatedBy = eRPInventoryCountLineInformationDto.imqCreatedBy,
						imqCreatedDate = eRPInventoryCountLineInformationDto.imqCreatedDate,
						imqUniqueID = eRPInventoryCountLineInformationDto.imqUniqueID,
						imqFinalCount = eRPInventoryCountLineInformationDto.imqFinalCount,
						imqInventoryCountID = eRPInventoryCountLineInformationDto.imqInventoryCountID,
						imqPartBinID = eRPInventoryCountLineInformationDto.imqPartBinID,
						imqPartClassID = eRPInventoryCountLineInformationDto.imqPartClassID,
						imqPartID = eRPInventoryCountLineInformationDto.imqPartID,
						imqPartRevisionID = eRPInventoryCountLineInformationDto.imqPartRevisionID,
						imqPartShortDescription = eRPInventoryCountLineInformationDto.imqPartShortDescription,
						imqPartWarehouseLocationID = eRPInventoryCountLineInformationDto.imqPartWarehouseLocationID,
						imqQuantityOnHand = eRPInventoryCountLineInformationDto.imqQuantityOnHand,
						imqRowVersion = eRPInventoryCountLineInformationDto.imqRowVersion,
						imqInventoryCountLineID = eRPInventoryCountLineInformationDto.imqInventoryCountLineID,
						CustomFields = eRPInventoryCountLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing InventoryCountLine [{inventoryCountLine.imqUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInventoryCountLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteInventoryCountLine(Guid inventoryCountLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInventoryCountLineRepository iERPInventoryCountLineRepository = (base.ERPInventoryCountLineRepository = new ERPInventoryCountLineRepository(base.ApiClientContext));
		using (iERPInventoryCountLineRepository)
		{
			if (!(await base.ERPInventoryCountLineRepository.DoesInventoryCountLineExist(inventoryCountLineId)))
			{
				base.ErrorsList.Add($"InventoryCountLine [{inventoryCountLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPInventoryCountLineInformationDto eRPInventoryCountLineInformationDto = await base.ERPInventoryCountLineRepository.GetInventoryCountLine(inventoryCountLineId);
				string text = await base.ERPInventoryCountLineRepository.WhereUsed("InventoryCountLines", new object[2] { eRPInventoryCountLineInformationDto.imqInventoryCountID, eRPInventoryCountLineInformationDto.imqInventoryCountLineID }, new object[2] { "imqInventoryCountID", "imqInventoryCountLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("InventoryCountLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPInventoryCountLineDto>> Process_DeleteInventoryCountLine(Guid inventoryCountLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPInventoryCountLineDto> result;
		try
		{
			IERPInventoryCountLineRepository iERPInventoryCountLineRepository = (base.ERPInventoryCountLineRepository = new ERPInventoryCountLineRepository(base.ApiClientContext));
			using (iERPInventoryCountLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPInventoryCountLineRepository.DeleteRowFromTable("InventoryCountLines", "imq", inventoryCountLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of InventoryCountLine [{inventoryCountLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInventoryCountLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPInventoryCountLineDto()
			};
		}
		return result;
	}
}
