using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPInventoryCountModel : ERPBaseModel, IERPInventoryCountModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllInventoryCounts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPInventoryCountRepository iERPInventoryCountRepository = (base.ERPInventoryCountRepository = new ERPInventoryCountRepository(base.ApiClientContext));
		using (iERPInventoryCountRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPInventoryCountRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPInventoryCountRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPInventoryCountRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPInventoryCountRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetInventoryCount(Guid inventoryCountId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInventoryCountRepository iERPInventoryCountRepository = (base.ERPInventoryCountRepository = new ERPInventoryCountRepository(base.ApiClientContext));
		using (iERPInventoryCountRepository)
		{
			if (!(await base.ERPInventoryCountRepository.DoesInventoryCountExist(inventoryCountId)))
			{
				errorsList.Add($"InventoryCount [{inventoryCountId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutInventoryCount(ERPInventoryCountDto inventoryCount)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInventoryCountRepository iERPInventoryCountRepository = (base.ERPInventoryCountRepository = new ERPInventoryCountRepository(base.ApiClientContext));
		using (iERPInventoryCountRepository)
		{
			if (!string.IsNullOrWhiteSpace(inventoryCount.imnCycleCodeID) && !(await base.ERPInventoryCountRepository.DoesRecordExistInTableUsingKeys("CycleCodes", new object[1] { "IMDCYCLECODEID" }, new object[1] { inventoryCount.imnCycleCodeID })))
			{
				errorsList.Add("imnCycleCodeID [" + inventoryCount.imnCycleCodeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPInventoryCountDto>>> Process_GetAllInventoryCounts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPInventoryCountDto> allInventoryCountsDto = new List<ERPInventoryCountDto>();
		ERPResponseMessageDto<IList<ERPInventoryCountDto>> result;
		try
		{
			IERPInventoryCountRepository iERPInventoryCountRepository = (base.ERPInventoryCountRepository = new ERPInventoryCountRepository(base.ApiClientContext));
			using (iERPInventoryCountRepository)
			{
				foreach (ERPInventoryCountInformationDto item2 in await base.ERPInventoryCountRepository.GetAllInventoryCounts(pageSize, pageNumber, filter, orderBy))
				{
					ERPInventoryCountDto item = new ERPInventoryCountDto
					{
						imnCreatedBy = item2.imnCreatedBy,
						imnCreatedDate = item2.imnCreatedDate,
						imnCycleCodeID = item2.imnCycleCodeID,
						imnUniqueID = item2.imnUniqueID,
						imnGeneratedDate = item2.imnGeneratedDate,
						imnExcludeInactivePartBins = item2.imnExcludeInactivePartBins,
						imnIncludeBlankPartClass = item2.imnIncludeBlankPartClass,
						imnIncludeBlankPartGroup = item2.imnIncludeBlankPartGroup,
						imnPostedToInventory = item2.imnPostedToInventory,
						imnRecordsGenerated = item2.imnRecordsGenerated,
						imnNumberofRecordsGenerated = item2.imnNumberofRecordsGenerated,
						imnPartBinIDs = item2.imnPartBinIDs,
						imnPartClassIDs = item2.imnPartClassIDs,
						imnPartGroupIDs = item2.imnPartGroupIDs,
						imnPartWarehouseIDs = item2.imnPartWarehouseIDs,
						imnPostedDate = item2.imnPostedDate,
						imnRowVersion = item2.imnRowVersion,
						imnInventoryCountID = item2.imnInventoryCountID,
						imnStatus = item2.imnStatus,
						imnSupplierOrganizationIDs = item2.imnSupplierOrganizationIDs,
						CustomFields = item2.CustomFields
					};
					allInventoryCountsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all InventoryCounts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPInventoryCountDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allInventoryCountsDto,
				RecordCount = allInventoryCountsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPInventoryCountDto>> Process_GetInventoryCount(Guid inventoryCountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPInventoryCountDto inventoryCountDto = null;
		ERPResponseMessageDto<ERPInventoryCountDto> result;
		try
		{
			IERPInventoryCountRepository iERPInventoryCountRepository = (base.ERPInventoryCountRepository = new ERPInventoryCountRepository(base.ApiClientContext));
			using (iERPInventoryCountRepository)
			{
				ERPInventoryCountInformationDto eRPInventoryCountInformationDto = await base.ERPInventoryCountRepository.GetInventoryCount(inventoryCountId);
				inventoryCountDto = new ERPInventoryCountDto
				{
					imnCreatedBy = eRPInventoryCountInformationDto.imnCreatedBy,
					imnCreatedDate = eRPInventoryCountInformationDto.imnCreatedDate,
					imnCycleCodeID = eRPInventoryCountInformationDto.imnCycleCodeID,
					imnUniqueID = eRPInventoryCountInformationDto.imnUniqueID,
					imnGeneratedDate = eRPInventoryCountInformationDto.imnGeneratedDate,
					imnExcludeInactivePartBins = eRPInventoryCountInformationDto.imnExcludeInactivePartBins,
					imnIncludeBlankPartClass = eRPInventoryCountInformationDto.imnIncludeBlankPartClass,
					imnIncludeBlankPartGroup = eRPInventoryCountInformationDto.imnIncludeBlankPartGroup,
					imnPostedToInventory = eRPInventoryCountInformationDto.imnPostedToInventory,
					imnRecordsGenerated = eRPInventoryCountInformationDto.imnRecordsGenerated,
					imnNumberofRecordsGenerated = eRPInventoryCountInformationDto.imnNumberofRecordsGenerated,
					imnPartBinIDs = eRPInventoryCountInformationDto.imnPartBinIDs,
					imnPartClassIDs = eRPInventoryCountInformationDto.imnPartClassIDs,
					imnPartGroupIDs = eRPInventoryCountInformationDto.imnPartGroupIDs,
					imnPartWarehouseIDs = eRPInventoryCountInformationDto.imnPartWarehouseIDs,
					imnPostedDate = eRPInventoryCountInformationDto.imnPostedDate,
					imnRowVersion = eRPInventoryCountInformationDto.imnRowVersion,
					imnInventoryCountID = eRPInventoryCountInformationDto.imnInventoryCountID,
					imnStatus = eRPInventoryCountInformationDto.imnStatus,
					imnSupplierOrganizationIDs = eRPInventoryCountInformationDto.imnSupplierOrganizationIDs,
					CustomFields = eRPInventoryCountInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the InventoryCounts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInventoryCountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = inventoryCountDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPInventoryCountDto>> Process_PutInventoryCount(ERPInventoryCountDto inventoryCount)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPInventoryCountDto createdObject = null;
		ERPResponseMessageDto<ERPInventoryCountDto> result;
		try
		{
			IERPInventoryCountRepository iERPInventoryCountRepository = (base.ERPInventoryCountRepository = new ERPInventoryCountRepository(base.ApiClientContext));
			using (iERPInventoryCountRepository)
			{
				APIValidationInfoDto postResult = await base.ERPInventoryCountRepository.SaveInventoryCount(inventoryCount);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPInventoryCountInformationDto eRPInventoryCountInformationDto = await base.ERPInventoryCountRepository.GetInventoryCount(inventoryCount.imnUniqueID);
					createdObject = new ERPInventoryCountDto
					{
						imnCreatedBy = eRPInventoryCountInformationDto.imnCreatedBy,
						imnCreatedDate = eRPInventoryCountInformationDto.imnCreatedDate,
						imnCycleCodeID = eRPInventoryCountInformationDto.imnCycleCodeID,
						imnUniqueID = eRPInventoryCountInformationDto.imnUniqueID,
						imnGeneratedDate = eRPInventoryCountInformationDto.imnGeneratedDate,
						imnExcludeInactivePartBins = eRPInventoryCountInformationDto.imnExcludeInactivePartBins,
						imnIncludeBlankPartClass = eRPInventoryCountInformationDto.imnIncludeBlankPartClass,
						imnIncludeBlankPartGroup = eRPInventoryCountInformationDto.imnIncludeBlankPartGroup,
						imnPostedToInventory = eRPInventoryCountInformationDto.imnPostedToInventory,
						imnRecordsGenerated = eRPInventoryCountInformationDto.imnRecordsGenerated,
						imnNumberofRecordsGenerated = eRPInventoryCountInformationDto.imnNumberofRecordsGenerated,
						imnPartBinIDs = eRPInventoryCountInformationDto.imnPartBinIDs,
						imnPartClassIDs = eRPInventoryCountInformationDto.imnPartClassIDs,
						imnPartGroupIDs = eRPInventoryCountInformationDto.imnPartGroupIDs,
						imnPartWarehouseIDs = eRPInventoryCountInformationDto.imnPartWarehouseIDs,
						imnPostedDate = eRPInventoryCountInformationDto.imnPostedDate,
						imnRowVersion = eRPInventoryCountInformationDto.imnRowVersion,
						imnInventoryCountID = eRPInventoryCountInformationDto.imnInventoryCountID,
						imnStatus = eRPInventoryCountInformationDto.imnStatus,
						imnSupplierOrganizationIDs = eRPInventoryCountInformationDto.imnSupplierOrganizationIDs,
						CustomFields = eRPInventoryCountInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing InventoryCount [{inventoryCount.imnUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInventoryCountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteInventoryCount(Guid inventoryCountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInventoryCountRepository iERPInventoryCountRepository = (base.ERPInventoryCountRepository = new ERPInventoryCountRepository(base.ApiClientContext));
		using (iERPInventoryCountRepository)
		{
			if (!(await base.ERPInventoryCountRepository.DoesInventoryCountExist(inventoryCountId)))
			{
				base.ErrorsList.Add($"InventoryCount [{inventoryCountId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPInventoryCountInformationDto eRPInventoryCountInformationDto = await base.ERPInventoryCountRepository.GetInventoryCount(inventoryCountId);
				string text = await base.ERPInventoryCountRepository.WhereUsed("InventoryCounts", new object[1] { eRPInventoryCountInformationDto.imnInventoryCountID }, new object[1] { "imnInventoryCountID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("InventoryCount cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPInventoryCountDto>> Process_DeleteInventoryCount(Guid inventoryCountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPInventoryCountDto> result;
		try
		{
			IERPInventoryCountRepository iERPInventoryCountRepository = (base.ERPInventoryCountRepository = new ERPInventoryCountRepository(base.ApiClientContext));
			using (iERPInventoryCountRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPInventoryCountRepository.DeleteRowFromTable("InventoryCounts", "imn", inventoryCountId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of InventoryCount [{inventoryCountId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInventoryCountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPInventoryCountDto()
			};
		}
		return result;
	}
}
