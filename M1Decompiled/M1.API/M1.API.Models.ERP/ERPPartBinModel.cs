using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartBinModel : ERPBaseModel, IERPPartBinModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartBins(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartBinRepository iERPPartBinRepository = (base.ERPPartBinRepository = new ERPPartBinRepository(base.ApiClientContext));
		using (iERPPartBinRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartBinRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartBinRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartBinRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartBinRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartBin(Guid partBinId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartBinRepository iERPPartBinRepository = (base.ERPPartBinRepository = new ERPPartBinRepository(base.ApiClientContext));
		using (iERPPartBinRepository)
		{
			if (!(await base.ERPPartBinRepository.DoesPartBinExist(partBinId)))
			{
				errorsList.Add($"PartBin [{partBinId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartBin(ERPPartBinDto partBin)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartBinRepository iERPPartBinRepository = (base.ERPPartBinRepository = new ERPPartBinRepository(base.ApiClientContext));
		using (iERPPartBinRepository)
		{
			if (!string.IsNullOrWhiteSpace(partBin.imbPartID) && !(await base.ERPPartBinRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partBin.imbPartID })))
			{
				errorsList.Add("imbPartID [" + partBin.imbPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partBin.imbPartRevisionID) && !(await base.ERPPartBinRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partBin.imbPartID, partBin.imbPartRevisionID })))
			{
				errorsList.Add("imbPartRevisionID [" + partBin.imbPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partBin.imbWarehouseID) && !(await base.ERPPartBinRepository.DoesRecordExistInTableUsingKeys("Warehouses", new object[1] { "IMWWAREHOUSEID" }, new object[1] { partBin.imbWarehouseID })))
			{
				errorsList.Add("imbWarehouseID [" + partBin.imbWarehouseID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartBinDto>>> Process_GetAllPartBins(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartBinDto> allPartBinsDto = new List<ERPPartBinDto>();
		ERPResponseMessageDto<IList<ERPPartBinDto>> result;
		try
		{
			IERPPartBinRepository iERPPartBinRepository = (base.ERPPartBinRepository = new ERPPartBinRepository(base.ApiClientContext));
			using (iERPPartBinRepository)
			{
				foreach (ERPPartBinInformationDto item2 in await base.ERPPartBinRepository.GetAllPartBins(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartBinDto item = new ERPPartBinDto
					{
						imbBinQuantityOnHand = item2.imbBinQuantityOnHand,
						imbPartBinID = item2.imbPartBinID,
						imbConversionFactor = item2.imbConversionFactor,
						imbCreatedBy = item2.imbCreatedBy,
						imbCreatedDate = item2.imbCreatedDate,
						imbDescription = item2.imbDescription,
						imbUniqueID = item2.imbUniqueID,
						imbInactiveBinDate = item2.imbInactiveBinDate,
						imbInactiveBin = item2.imbInactiveBin,
						imbDefaultBin = item2.imbDefaultBin,
						imbPartID = item2.imbPartID,
						imbPartRevisionID = item2.imbPartRevisionID,
						imbQuantityAllocated = item2.imbQuantityAllocated,
						imbQuantityOnHand = item2.imbQuantityOnHand,
						imbQuantityOnOrderPurchases = item2.imbQuantityOnOrderPurchases,
						imbQuantityOnOrderSales = item2.imbQuantityOnOrderSales,
						imbQuantityToInspect = item2.imbQuantityToInspect,
						imbQuantityToReturn = item2.imbQuantityToReturn,
						imbQuantityToReturnJob = item2.imbQuantityToReturnJob,
						imbRowVersion = item2.imbRowVersion,
						imbWarehouseID = item2.imbWarehouseID,
						CustomFields = item2.CustomFields
					};
					allPartBinsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartBins]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartBinDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartBinsDto,
				RecordCount = allPartBinsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartBinDto>> Process_GetPartBin(Guid partBinId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartBinDto partBinDto = null;
		ERPResponseMessageDto<ERPPartBinDto> result;
		try
		{
			IERPPartBinRepository iERPPartBinRepository = (base.ERPPartBinRepository = new ERPPartBinRepository(base.ApiClientContext));
			using (iERPPartBinRepository)
			{
				ERPPartBinInformationDto eRPPartBinInformationDto = await base.ERPPartBinRepository.GetPartBin(partBinId);
				partBinDto = new ERPPartBinDto
				{
					imbBinQuantityOnHand = eRPPartBinInformationDto.imbBinQuantityOnHand,
					imbPartBinID = eRPPartBinInformationDto.imbPartBinID,
					imbConversionFactor = eRPPartBinInformationDto.imbConversionFactor,
					imbCreatedBy = eRPPartBinInformationDto.imbCreatedBy,
					imbCreatedDate = eRPPartBinInformationDto.imbCreatedDate,
					imbDescription = eRPPartBinInformationDto.imbDescription,
					imbUniqueID = eRPPartBinInformationDto.imbUniqueID,
					imbInactiveBinDate = eRPPartBinInformationDto.imbInactiveBinDate,
					imbInactiveBin = eRPPartBinInformationDto.imbInactiveBin,
					imbDefaultBin = eRPPartBinInformationDto.imbDefaultBin,
					imbPartID = eRPPartBinInformationDto.imbPartID,
					imbPartRevisionID = eRPPartBinInformationDto.imbPartRevisionID,
					imbQuantityAllocated = eRPPartBinInformationDto.imbQuantityAllocated,
					imbQuantityOnHand = eRPPartBinInformationDto.imbQuantityOnHand,
					imbQuantityOnOrderPurchases = eRPPartBinInformationDto.imbQuantityOnOrderPurchases,
					imbQuantityOnOrderSales = eRPPartBinInformationDto.imbQuantityOnOrderSales,
					imbQuantityToInspect = eRPPartBinInformationDto.imbQuantityToInspect,
					imbQuantityToReturn = eRPPartBinInformationDto.imbQuantityToReturn,
					imbQuantityToReturnJob = eRPPartBinInformationDto.imbQuantityToReturnJob,
					imbRowVersion = eRPPartBinInformationDto.imbRowVersion,
					imbWarehouseID = eRPPartBinInformationDto.imbWarehouseID,
					CustomFields = eRPPartBinInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartBins []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartBinDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partBinDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartBinDto>> Process_PutPartBin(ERPPartBinDto partBin)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartBinDto createdObject = null;
		ERPResponseMessageDto<ERPPartBinDto> result;
		try
		{
			IERPPartBinRepository iERPPartBinRepository = (base.ERPPartBinRepository = new ERPPartBinRepository(base.ApiClientContext));
			using (iERPPartBinRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartBinRepository.SavePartBin(partBin);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartBinInformationDto eRPPartBinInformationDto = await base.ERPPartBinRepository.GetPartBin(partBin.imbUniqueID);
					createdObject = new ERPPartBinDto
					{
						imbBinQuantityOnHand = eRPPartBinInformationDto.imbBinQuantityOnHand,
						imbPartBinID = eRPPartBinInformationDto.imbPartBinID,
						imbConversionFactor = eRPPartBinInformationDto.imbConversionFactor,
						imbCreatedBy = eRPPartBinInformationDto.imbCreatedBy,
						imbCreatedDate = eRPPartBinInformationDto.imbCreatedDate,
						imbDescription = eRPPartBinInformationDto.imbDescription,
						imbUniqueID = eRPPartBinInformationDto.imbUniqueID,
						imbInactiveBinDate = eRPPartBinInformationDto.imbInactiveBinDate,
						imbInactiveBin = eRPPartBinInformationDto.imbInactiveBin,
						imbDefaultBin = eRPPartBinInformationDto.imbDefaultBin,
						imbPartID = eRPPartBinInformationDto.imbPartID,
						imbPartRevisionID = eRPPartBinInformationDto.imbPartRevisionID,
						imbQuantityAllocated = eRPPartBinInformationDto.imbQuantityAllocated,
						imbQuantityOnHand = eRPPartBinInformationDto.imbQuantityOnHand,
						imbQuantityOnOrderPurchases = eRPPartBinInformationDto.imbQuantityOnOrderPurchases,
						imbQuantityOnOrderSales = eRPPartBinInformationDto.imbQuantityOnOrderSales,
						imbQuantityToInspect = eRPPartBinInformationDto.imbQuantityToInspect,
						imbQuantityToReturn = eRPPartBinInformationDto.imbQuantityToReturn,
						imbQuantityToReturnJob = eRPPartBinInformationDto.imbQuantityToReturnJob,
						imbRowVersion = eRPPartBinInformationDto.imbRowVersion,
						imbWarehouseID = eRPPartBinInformationDto.imbWarehouseID,
						CustomFields = eRPPartBinInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartBin [{partBin.imbUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartBinDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartBin(Guid partBinId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartBinRepository iERPPartBinRepository = (base.ERPPartBinRepository = new ERPPartBinRepository(base.ApiClientContext));
		using (iERPPartBinRepository)
		{
			if (!(await base.ERPPartBinRepository.DoesPartBinExist(partBinId)))
			{
				base.ErrorsList.Add($"PartBin [{partBinId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartBinInformationDto eRPPartBinInformationDto = await base.ERPPartBinRepository.GetPartBin(partBinId);
				string text = await base.ERPPartBinRepository.WhereUsed("PartBins", new object[4] { eRPPartBinInformationDto.imbPartID, eRPPartBinInformationDto.imbPartRevisionID, eRPPartBinInformationDto.imbWarehouseID, eRPPartBinInformationDto.imbPartBinID }, new object[4] { "imbPartID", "imbPartRevisionID", "imbWarehouseID", "imbPartBinID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartBin cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartBinDto>> Process_DeletePartBin(Guid partBinId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartBinDto> result;
		try
		{
			IERPPartBinRepository iERPPartBinRepository = (base.ERPPartBinRepository = new ERPPartBinRepository(base.ApiClientContext));
			using (iERPPartBinRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartBinRepository.DeleteRowFromTable("PartBins", "imb", partBinId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartBin [{partBinId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartBinDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartBinDto()
			};
		}
		return result;
	}
}
