using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartBinDetailModel : ERPBaseModel, IERPPartBinDetailModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartBinDetails(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartBinDetailRepository iERPPartBinDetailRepository = (base.ERPPartBinDetailRepository = new ERPPartBinDetailRepository(base.ApiClientContext));
		using (iERPPartBinDetailRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartBinDetailRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartBinDetailRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartBinDetailRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartBinDetailRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartBinDetail(Guid partBinDetailId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartBinDetailRepository iERPPartBinDetailRepository = (base.ERPPartBinDetailRepository = new ERPPartBinDetailRepository(base.ApiClientContext));
		using (iERPPartBinDetailRepository)
		{
			if (!(await base.ERPPartBinDetailRepository.DoesPartBinDetailExist(partBinDetailId)))
			{
				errorsList.Add($"PartBinDetail [{partBinDetailId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartBinDetail(ERPPartBinDetailDto partBinDetail)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartBinDetailRepository iERPPartBinDetailRepository = (base.ERPPartBinDetailRepository = new ERPPartBinDetailRepository(base.ApiClientContext));
		using (iERPPartBinDetailRepository)
		{
			if (!string.IsNullOrWhiteSpace(partBinDetail.imgPartID) && !(await base.ERPPartBinDetailRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partBinDetail.imgPartID })))
			{
				errorsList.Add("imgPartID [" + partBinDetail.imgPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partBinDetail.imgPartRevisionID) && !(await base.ERPPartBinDetailRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partBinDetail.imgPartID, partBinDetail.imgPartRevisionID })))
			{
				errorsList.Add("imgPartRevisionID [" + partBinDetail.imgPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partBinDetail.imgWarehouseID) && !(await base.ERPPartBinDetailRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { partBinDetail.imgPartID, partBinDetail.imgPartRevisionID, partBinDetail.imgWarehouseID })))
			{
				errorsList.Add("imgWarehouseID [" + partBinDetail.imgWarehouseID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partBinDetail.imgPartBinID) && !(await base.ERPPartBinDetailRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { partBinDetail.imgPartID, partBinDetail.imgPartRevisionID, partBinDetail.imgWarehouseID, partBinDetail.imgPartBinID })))
			{
				errorsList.Add("imgPartBinID [" + partBinDetail.imgPartBinID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartBinDetailDto>>> Process_GetAllPartBinDetails(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartBinDetailDto> allPartBinDetailsDto = new List<ERPPartBinDetailDto>();
		ERPResponseMessageDto<IList<ERPPartBinDetailDto>> result;
		try
		{
			IERPPartBinDetailRepository iERPPartBinDetailRepository = (base.ERPPartBinDetailRepository = new ERPPartBinDetailRepository(base.ApiClientContext));
			using (iERPPartBinDetailRepository)
			{
				foreach (ERPPartBinDetailInformationDto item2 in await base.ERPPartBinDetailRepository.GetAllPartBinDetails(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartBinDetailDto item = new ERPPartBinDetailDto
					{
						imgCreatedBy = item2.imgCreatedBy,
						imgCreatedDate = item2.imgCreatedDate,
						imgUniqueID = item2.imgUniqueID,
						imgOriginalQuantity = item2.imgOriginalQuantity,
						imgPartBinID = item2.imgPartBinID,
						imgPartID = item2.imgPartID,
						imgPartRevisionID = item2.imgPartRevisionID,
						imgQuantityType = item2.imgQuantityType,
						imgRemainingQuantity = item2.imgRemainingQuantity,
						imgRowVersion = item2.imgRowVersion,
						imgPartBinDetailID = item2.imgPartBinDetailID,
						imgSourceTableName = item2.imgSourceTableName,
						imgSourceTableUniqueID = item2.imgSourceTableUniqueID,
						imgTransactionDate = item2.imgTransactionDate,
						imgUnitDutyCost = item2.imgUnitDutyCost,
						imgUnitFreightCost = item2.imgUnitFreightCost,
						imgUnitLaborCost = item2.imgUnitLaborCost,
						imgUnitMaterialCost = item2.imgUnitMaterialCost,
						imgUnitMiscCost = item2.imgUnitMiscCost,
						imgUnitOverheadCost = item2.imgUnitOverheadCost,
						imgUnitSubcontractCost = item2.imgUnitSubcontractCost,
						imgWarehouseID = item2.imgWarehouseID,
						CustomFields = item2.CustomFields
					};
					allPartBinDetailsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartBinDetails]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartBinDetailDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartBinDetailsDto,
				RecordCount = allPartBinDetailsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartBinDetailDto>> Process_GetPartBinDetail(Guid partBinDetailId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartBinDetailDto partBinDetailDto = null;
		ERPResponseMessageDto<ERPPartBinDetailDto> result;
		try
		{
			IERPPartBinDetailRepository iERPPartBinDetailRepository = (base.ERPPartBinDetailRepository = new ERPPartBinDetailRepository(base.ApiClientContext));
			using (iERPPartBinDetailRepository)
			{
				ERPPartBinDetailInformationDto eRPPartBinDetailInformationDto = await base.ERPPartBinDetailRepository.GetPartBinDetail(partBinDetailId);
				partBinDetailDto = new ERPPartBinDetailDto
				{
					imgCreatedBy = eRPPartBinDetailInformationDto.imgCreatedBy,
					imgCreatedDate = eRPPartBinDetailInformationDto.imgCreatedDate,
					imgUniqueID = eRPPartBinDetailInformationDto.imgUniqueID,
					imgOriginalQuantity = eRPPartBinDetailInformationDto.imgOriginalQuantity,
					imgPartBinID = eRPPartBinDetailInformationDto.imgPartBinID,
					imgPartID = eRPPartBinDetailInformationDto.imgPartID,
					imgPartRevisionID = eRPPartBinDetailInformationDto.imgPartRevisionID,
					imgQuantityType = eRPPartBinDetailInformationDto.imgQuantityType,
					imgRemainingQuantity = eRPPartBinDetailInformationDto.imgRemainingQuantity,
					imgRowVersion = eRPPartBinDetailInformationDto.imgRowVersion,
					imgPartBinDetailID = eRPPartBinDetailInformationDto.imgPartBinDetailID,
					imgSourceTableName = eRPPartBinDetailInformationDto.imgSourceTableName,
					imgSourceTableUniqueID = eRPPartBinDetailInformationDto.imgSourceTableUniqueID,
					imgTransactionDate = eRPPartBinDetailInformationDto.imgTransactionDate,
					imgUnitDutyCost = eRPPartBinDetailInformationDto.imgUnitDutyCost,
					imgUnitFreightCost = eRPPartBinDetailInformationDto.imgUnitFreightCost,
					imgUnitLaborCost = eRPPartBinDetailInformationDto.imgUnitLaborCost,
					imgUnitMaterialCost = eRPPartBinDetailInformationDto.imgUnitMaterialCost,
					imgUnitMiscCost = eRPPartBinDetailInformationDto.imgUnitMiscCost,
					imgUnitOverheadCost = eRPPartBinDetailInformationDto.imgUnitOverheadCost,
					imgUnitSubcontractCost = eRPPartBinDetailInformationDto.imgUnitSubcontractCost,
					imgWarehouseID = eRPPartBinDetailInformationDto.imgWarehouseID,
					CustomFields = eRPPartBinDetailInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartBinDetails []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartBinDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partBinDetailDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartBinDetailDto>> Process_PutPartBinDetail(ERPPartBinDetailDto partBinDetail)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartBinDetailDto createdObject = null;
		ERPResponseMessageDto<ERPPartBinDetailDto> result;
		try
		{
			IERPPartBinDetailRepository iERPPartBinDetailRepository = (base.ERPPartBinDetailRepository = new ERPPartBinDetailRepository(base.ApiClientContext));
			using (iERPPartBinDetailRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartBinDetailRepository.SavePartBinDetail(partBinDetail);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartBinDetailInformationDto eRPPartBinDetailInformationDto = await base.ERPPartBinDetailRepository.GetPartBinDetail(partBinDetail.imgUniqueID);
					createdObject = new ERPPartBinDetailDto
					{
						imgCreatedBy = eRPPartBinDetailInformationDto.imgCreatedBy,
						imgCreatedDate = eRPPartBinDetailInformationDto.imgCreatedDate,
						imgUniqueID = eRPPartBinDetailInformationDto.imgUniqueID,
						imgOriginalQuantity = eRPPartBinDetailInformationDto.imgOriginalQuantity,
						imgPartBinID = eRPPartBinDetailInformationDto.imgPartBinID,
						imgPartID = eRPPartBinDetailInformationDto.imgPartID,
						imgPartRevisionID = eRPPartBinDetailInformationDto.imgPartRevisionID,
						imgQuantityType = eRPPartBinDetailInformationDto.imgQuantityType,
						imgRemainingQuantity = eRPPartBinDetailInformationDto.imgRemainingQuantity,
						imgRowVersion = eRPPartBinDetailInformationDto.imgRowVersion,
						imgPartBinDetailID = eRPPartBinDetailInformationDto.imgPartBinDetailID,
						imgSourceTableName = eRPPartBinDetailInformationDto.imgSourceTableName,
						imgSourceTableUniqueID = eRPPartBinDetailInformationDto.imgSourceTableUniqueID,
						imgTransactionDate = eRPPartBinDetailInformationDto.imgTransactionDate,
						imgUnitDutyCost = eRPPartBinDetailInformationDto.imgUnitDutyCost,
						imgUnitFreightCost = eRPPartBinDetailInformationDto.imgUnitFreightCost,
						imgUnitLaborCost = eRPPartBinDetailInformationDto.imgUnitLaborCost,
						imgUnitMaterialCost = eRPPartBinDetailInformationDto.imgUnitMaterialCost,
						imgUnitMiscCost = eRPPartBinDetailInformationDto.imgUnitMiscCost,
						imgUnitOverheadCost = eRPPartBinDetailInformationDto.imgUnitOverheadCost,
						imgUnitSubcontractCost = eRPPartBinDetailInformationDto.imgUnitSubcontractCost,
						imgWarehouseID = eRPPartBinDetailInformationDto.imgWarehouseID,
						CustomFields = eRPPartBinDetailInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartBinDetail [{partBinDetail.imgUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartBinDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartBinDetail(Guid partBinDetailId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartBinDetailRepository iERPPartBinDetailRepository = (base.ERPPartBinDetailRepository = new ERPPartBinDetailRepository(base.ApiClientContext));
		using (iERPPartBinDetailRepository)
		{
			if (!(await base.ERPPartBinDetailRepository.DoesPartBinDetailExist(partBinDetailId)))
			{
				base.ErrorsList.Add($"PartBinDetail [{partBinDetailId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartBinDetailInformationDto eRPPartBinDetailInformationDto = await base.ERPPartBinDetailRepository.GetPartBinDetail(partBinDetailId);
				string text = await base.ERPPartBinDetailRepository.WhereUsed("PartBinDetails", new object[5] { eRPPartBinDetailInformationDto.imgPartID, eRPPartBinDetailInformationDto.imgPartRevisionID, eRPPartBinDetailInformationDto.imgWarehouseID, eRPPartBinDetailInformationDto.imgPartBinID, eRPPartBinDetailInformationDto.imgPartBinDetailID }, new object[5] { "imgPartID", "imgPartRevisionID", "imgWarehouseID", "imgPartBinID", "imgPartBinDetailID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartBinDetail cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartBinDetailDto>> Process_DeletePartBinDetail(Guid partBinDetailId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartBinDetailDto> result;
		try
		{
			IERPPartBinDetailRepository iERPPartBinDetailRepository = (base.ERPPartBinDetailRepository = new ERPPartBinDetailRepository(base.ApiClientContext));
			using (iERPPartBinDetailRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartBinDetailRepository.DeleteRowFromTable("PartBinDetails", "img", partBinDetailId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartBinDetail [{partBinDetailId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartBinDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartBinDetailDto()
			};
		}
		return result;
	}
}
