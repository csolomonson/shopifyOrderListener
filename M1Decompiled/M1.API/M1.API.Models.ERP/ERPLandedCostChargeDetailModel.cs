using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPLandedCostChargeDetailModel : ERPBaseModel, IERPLandedCostChargeDetailModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllLandedCostChargeDetails(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPLandedCostChargeDetailRepository iERPLandedCostChargeDetailRepository = (base.ERPLandedCostChargeDetailRepository = new ERPLandedCostChargeDetailRepository(base.ApiClientContext));
		using (iERPLandedCostChargeDetailRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPLandedCostChargeDetailRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPLandedCostChargeDetailRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPLandedCostChargeDetailRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPLandedCostChargeDetailRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetLandedCostChargeDetail(Guid landedCostChargeDetailId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLandedCostChargeDetailRepository iERPLandedCostChargeDetailRepository = (base.ERPLandedCostChargeDetailRepository = new ERPLandedCostChargeDetailRepository(base.ApiClientContext));
		using (iERPLandedCostChargeDetailRepository)
		{
			if (!(await base.ERPLandedCostChargeDetailRepository.DoesLandedCostChargeDetailExist(landedCostChargeDetailId)))
			{
				errorsList.Add($"LandedCostChargeDetail [{landedCostChargeDetailId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutLandedCostChargeDetail(ERPLandedCostChargeDetailDto landedCostChargeDetail)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLandedCostChargeDetailRepository iERPLandedCostChargeDetailRepository = (base.ERPLandedCostChargeDetailRepository = new ERPLandedCostChargeDetailRepository(base.ApiClientContext));
		using (iERPLandedCostChargeDetailRepository)
		{
			if (!string.IsNullOrWhiteSpace(landedCostChargeDetail.rmiLandedCostID) && !(await base.ERPLandedCostChargeDetailRepository.DoesRecordExistInTableUsingKeys("LandedCosts", new object[1] { "RMCLANDEDCOSTID" }, new object[1] { landedCostChargeDetail.rmiLandedCostID })))
			{
				errorsList.Add("rmiLandedCostID [" + landedCostChargeDetail.rmiLandedCostID + "] not found.");
			}
			if (landedCostChargeDetail.rmiLandedCostChargeID > 0 && !(await base.ERPLandedCostChargeDetailRepository.DoesRecordExistInTableUsingKeys("LandedCostCharges", new object[2] { "RMHLANDEDCOSTID", "RMHLANDEDCOSTCHARGEID" }, new object[2] { landedCostChargeDetail.rmiLandedCostID, landedCostChargeDetail.rmiLandedCostChargeID })))
			{
				errorsList.Add($"rmiLandedCostChargeID [{landedCostChargeDetail.rmiLandedCostChargeID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCostChargeDetail.rmiPurchaseOrderID) && !(await base.ERPLandedCostChargeDetailRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { landedCostChargeDetail.rmiPurchaseOrderID })))
			{
				errorsList.Add("rmiPurchaseOrderID [" + landedCostChargeDetail.rmiPurchaseOrderID + "] not found.");
			}
			if (landedCostChargeDetail.rmiPurchaseOrderLineID > 0 && !(await base.ERPLandedCostChargeDetailRepository.DoesRecordExistInTableUsingKeys("PurchaseOrderLines", new object[2] { "PMLPURCHASEORDERID", "PMLPURCHASEORDERLINEID" }, new object[2] { landedCostChargeDetail.rmiPurchaseOrderID, landedCostChargeDetail.rmiPurchaseOrderLineID })))
			{
				errorsList.Add($"rmiPurchaseOrderLineID [{landedCostChargeDetail.rmiPurchaseOrderLineID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPLandedCostChargeDetailDto>>> Process_GetAllLandedCostChargeDetails(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPLandedCostChargeDetailDto> allLandedCostChargeDetailsDto = new List<ERPLandedCostChargeDetailDto>();
		ERPResponseMessageDto<IList<ERPLandedCostChargeDetailDto>> result;
		try
		{
			IERPLandedCostChargeDetailRepository iERPLandedCostChargeDetailRepository = (base.ERPLandedCostChargeDetailRepository = new ERPLandedCostChargeDetailRepository(base.ApiClientContext));
			using (iERPLandedCostChargeDetailRepository)
			{
				foreach (ERPLandedCostChargeDetailInformationDto item2 in await base.ERPLandedCostChargeDetailRepository.GetAllLandedCostChargeDetails(pageSize, pageNumber, filter, orderBy))
				{
					ERPLandedCostChargeDetailDto item = new ERPLandedCostChargeDetailDto
					{
						rmiCreatedBy = item2.rmiCreatedBy,
						rmiCreatedDate = item2.rmiCreatedDate,
						rmiUniqueID = item2.rmiUniqueID,
						rmiEstTotalCost = item2.rmiEstTotalCost,
						rmiEstTotalCostForeign = item2.rmiEstTotalCostForeign,
						rmiLandedCostChargeID = item2.rmiLandedCostChargeID,
						rmiLandedCostID = item2.rmiLandedCostID,
						rmiPurchaseOrderID = item2.rmiPurchaseOrderID,
						rmiPurchaseOrderLineID = item2.rmiPurchaseOrderLineID,
						rmiRowVersion = item2.rmiRowVersion,
						rmiLandedCostChargeDetailID = item2.rmiLandedCostChargeDetailID,
						rmiTotalCost = item2.rmiTotalCost,
						rmiTotalCostForeign = item2.rmiTotalCostForeign,
						CustomFields = item2.CustomFields
					};
					allLandedCostChargeDetailsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all LandedCostChargeDetails]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPLandedCostChargeDetailDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allLandedCostChargeDetailsDto,
				RecordCount = allLandedCostChargeDetailsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLandedCostChargeDetailDto>> Process_GetLandedCostChargeDetail(Guid landedCostChargeDetailId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPLandedCostChargeDetailDto landedCostChargeDetailDto = null;
		ERPResponseMessageDto<ERPLandedCostChargeDetailDto> result;
		try
		{
			IERPLandedCostChargeDetailRepository iERPLandedCostChargeDetailRepository = (base.ERPLandedCostChargeDetailRepository = new ERPLandedCostChargeDetailRepository(base.ApiClientContext));
			using (iERPLandedCostChargeDetailRepository)
			{
				ERPLandedCostChargeDetailInformationDto eRPLandedCostChargeDetailInformationDto = await base.ERPLandedCostChargeDetailRepository.GetLandedCostChargeDetail(landedCostChargeDetailId);
				landedCostChargeDetailDto = new ERPLandedCostChargeDetailDto
				{
					rmiCreatedBy = eRPLandedCostChargeDetailInformationDto.rmiCreatedBy,
					rmiCreatedDate = eRPLandedCostChargeDetailInformationDto.rmiCreatedDate,
					rmiUniqueID = eRPLandedCostChargeDetailInformationDto.rmiUniqueID,
					rmiEstTotalCost = eRPLandedCostChargeDetailInformationDto.rmiEstTotalCost,
					rmiEstTotalCostForeign = eRPLandedCostChargeDetailInformationDto.rmiEstTotalCostForeign,
					rmiLandedCostChargeID = eRPLandedCostChargeDetailInformationDto.rmiLandedCostChargeID,
					rmiLandedCostID = eRPLandedCostChargeDetailInformationDto.rmiLandedCostID,
					rmiPurchaseOrderID = eRPLandedCostChargeDetailInformationDto.rmiPurchaseOrderID,
					rmiPurchaseOrderLineID = eRPLandedCostChargeDetailInformationDto.rmiPurchaseOrderLineID,
					rmiRowVersion = eRPLandedCostChargeDetailInformationDto.rmiRowVersion,
					rmiLandedCostChargeDetailID = eRPLandedCostChargeDetailInformationDto.rmiLandedCostChargeDetailID,
					rmiTotalCost = eRPLandedCostChargeDetailInformationDto.rmiTotalCost,
					rmiTotalCostForeign = eRPLandedCostChargeDetailInformationDto.rmiTotalCostForeign,
					CustomFields = eRPLandedCostChargeDetailInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the LandedCostChargeDetails []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLandedCostChargeDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = landedCostChargeDetailDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLandedCostChargeDetailDto>> Process_PutLandedCostChargeDetail(ERPLandedCostChargeDetailDto landedCostChargeDetail)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPLandedCostChargeDetailDto createdObject = null;
		ERPResponseMessageDto<ERPLandedCostChargeDetailDto> result;
		try
		{
			IERPLandedCostChargeDetailRepository iERPLandedCostChargeDetailRepository = (base.ERPLandedCostChargeDetailRepository = new ERPLandedCostChargeDetailRepository(base.ApiClientContext));
			using (iERPLandedCostChargeDetailRepository)
			{
				APIValidationInfoDto postResult = await base.ERPLandedCostChargeDetailRepository.SaveLandedCostChargeDetail(landedCostChargeDetail);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPLandedCostChargeDetailInformationDto eRPLandedCostChargeDetailInformationDto = await base.ERPLandedCostChargeDetailRepository.GetLandedCostChargeDetail(landedCostChargeDetail.rmiUniqueID);
					createdObject = new ERPLandedCostChargeDetailDto
					{
						rmiCreatedBy = eRPLandedCostChargeDetailInformationDto.rmiCreatedBy,
						rmiCreatedDate = eRPLandedCostChargeDetailInformationDto.rmiCreatedDate,
						rmiUniqueID = eRPLandedCostChargeDetailInformationDto.rmiUniqueID,
						rmiEstTotalCost = eRPLandedCostChargeDetailInformationDto.rmiEstTotalCost,
						rmiEstTotalCostForeign = eRPLandedCostChargeDetailInformationDto.rmiEstTotalCostForeign,
						rmiLandedCostChargeID = eRPLandedCostChargeDetailInformationDto.rmiLandedCostChargeID,
						rmiLandedCostID = eRPLandedCostChargeDetailInformationDto.rmiLandedCostID,
						rmiPurchaseOrderID = eRPLandedCostChargeDetailInformationDto.rmiPurchaseOrderID,
						rmiPurchaseOrderLineID = eRPLandedCostChargeDetailInformationDto.rmiPurchaseOrderLineID,
						rmiRowVersion = eRPLandedCostChargeDetailInformationDto.rmiRowVersion,
						rmiLandedCostChargeDetailID = eRPLandedCostChargeDetailInformationDto.rmiLandedCostChargeDetailID,
						rmiTotalCost = eRPLandedCostChargeDetailInformationDto.rmiTotalCost,
						rmiTotalCostForeign = eRPLandedCostChargeDetailInformationDto.rmiTotalCostForeign,
						CustomFields = eRPLandedCostChargeDetailInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing LandedCostChargeDetail [{landedCostChargeDetail.rmiUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLandedCostChargeDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteLandedCostChargeDetail(Guid landedCostChargeDetailId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLandedCostChargeDetailRepository iERPLandedCostChargeDetailRepository = (base.ERPLandedCostChargeDetailRepository = new ERPLandedCostChargeDetailRepository(base.ApiClientContext));
		using (iERPLandedCostChargeDetailRepository)
		{
			if (!(await base.ERPLandedCostChargeDetailRepository.DoesLandedCostChargeDetailExist(landedCostChargeDetailId)))
			{
				base.ErrorsList.Add($"LandedCostChargeDetail [{landedCostChargeDetailId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPLandedCostChargeDetailInformationDto eRPLandedCostChargeDetailInformationDto = await base.ERPLandedCostChargeDetailRepository.GetLandedCostChargeDetail(landedCostChargeDetailId);
				string text = await base.ERPLandedCostChargeDetailRepository.WhereUsed("LandedCostChargeDetails", new object[3] { eRPLandedCostChargeDetailInformationDto.rmiLandedCostID, eRPLandedCostChargeDetailInformationDto.rmiLandedCostChargeID, eRPLandedCostChargeDetailInformationDto.rmiLandedCostChargeDetailID }, new object[3] { "rmiLandedCostID", "rmiLandedCostChargeID", "rmiLandedCostChargeDetailID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("LandedCostChargeDetail cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPLandedCostChargeDetailDto>> Process_DeleteLandedCostChargeDetail(Guid landedCostChargeDetailId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPLandedCostChargeDetailDto> result;
		try
		{
			IERPLandedCostChargeDetailRepository iERPLandedCostChargeDetailRepository = (base.ERPLandedCostChargeDetailRepository = new ERPLandedCostChargeDetailRepository(base.ApiClientContext));
			using (iERPLandedCostChargeDetailRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPLandedCostChargeDetailRepository.DeleteRowFromTable("LandedCostChargeDetails", "rmi", landedCostChargeDetailId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of LandedCostChargeDetail [{landedCostChargeDetailId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLandedCostChargeDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPLandedCostChargeDetailDto()
			};
		}
		return result;
	}
}
