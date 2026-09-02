using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPShipmentPackageDetailModel : ERPBaseModel, IERPShipmentPackageDetailModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllShipmentPackageDetails(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPShipmentPackageDetailRepository iERPShipmentPackageDetailRepository = (base.ERPShipmentPackageDetailRepository = new ERPShipmentPackageDetailRepository(base.ApiClientContext));
		using (iERPShipmentPackageDetailRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPShipmentPackageDetailRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPShipmentPackageDetailRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPShipmentPackageDetailRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPShipmentPackageDetailRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetShipmentPackageDetail(Guid shipmentPackageDetailId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentPackageDetailRepository iERPShipmentPackageDetailRepository = (base.ERPShipmentPackageDetailRepository = new ERPShipmentPackageDetailRepository(base.ApiClientContext));
		using (iERPShipmentPackageDetailRepository)
		{
			if (!(await base.ERPShipmentPackageDetailRepository.DoesShipmentPackageDetailExist(shipmentPackageDetailId)))
			{
				errorsList.Add($"ShipmentPackageDetail [{shipmentPackageDetailId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutShipmentPackageDetail(ERPShipmentPackageDetailDto shipmentPackageDetail)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentPackageDetailRepository iERPShipmentPackageDetailRepository = (base.ERPShipmentPackageDetailRepository = new ERPShipmentPackageDetailRepository(base.ApiClientContext));
		using (iERPShipmentPackageDetailRepository)
		{
			if (!string.IsNullOrWhiteSpace(shipmentPackageDetail.spdShipmentID) && !(await base.ERPShipmentPackageDetailRepository.DoesRecordExistInTableUsingKeys("SHIPMENTS", new object[1] { "SMPSHIPMENTID" }, new object[1] { shipmentPackageDetail.spdShipmentID })))
			{
				errorsList.Add("spdShipmentID [" + shipmentPackageDetail.spdShipmentID + "] not found.");
			}
			if (shipmentPackageDetail.spdShipmentLineID > 0 && !(await base.ERPShipmentPackageDetailRepository.DoesRecordExistInTableUsingKeys("SHIPMENTLINES", new object[2] { "SMLSHIPMENTID", "SMLSHIPMENTLINEID" }, new object[2] { shipmentPackageDetail.spdShipmentID, shipmentPackageDetail.spdShipmentLineID })))
			{
				errorsList.Add($"spdShipmentLineID [{shipmentPackageDetail.spdShipmentLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentPackageDetail.spdPartID) && !(await base.ERPShipmentPackageDetailRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { shipmentPackageDetail.spdPartID })))
			{
				errorsList.Add("spdPartID [" + shipmentPackageDetail.spdPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentPackageDetail.spdPartRevisionID) && !(await base.ERPShipmentPackageDetailRepository.DoesRecordExistInTableUsingKeys("PARTREVISIONS", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { shipmentPackageDetail.spdPartID, shipmentPackageDetail.spdPartRevisionID })))
			{
				errorsList.Add("spdPartRevisionID [" + shipmentPackageDetail.spdPartRevisionID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPShipmentPackageDetailDto>>> Process_GetAllShipmentPackageDetails(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPShipmentPackageDetailDto> allShipmentPackageDetailsDto = new List<ERPShipmentPackageDetailDto>();
		ERPResponseMessageDto<IList<ERPShipmentPackageDetailDto>> result;
		try
		{
			IERPShipmentPackageDetailRepository iERPShipmentPackageDetailRepository = (base.ERPShipmentPackageDetailRepository = new ERPShipmentPackageDetailRepository(base.ApiClientContext));
			using (iERPShipmentPackageDetailRepository)
			{
				foreach (ERPShipmentPackageDetailInformationDto item2 in await base.ERPShipmentPackageDetailRepository.GetAllShipmentPackageDetails(pageSize, pageNumber, filter, orderBy))
				{
					ERPShipmentPackageDetailDto item = new ERPShipmentPackageDetailDto
					{
						spdCommodityDescription = item2.spdCommodityDescription,
						spdCountryOfManufacture = item2.spdCountryOfManufacture,
						spdCreatedBy = item2.spdCreatedBy,
						spdCreatedDate = item2.spdCreatedDate,
						spdUniqueID = item2.spdUniqueID,
						spdPartID = item2.spdPartID,
						spdPartRevisionID = item2.spdPartRevisionID,
						spdQuantity = item2.spdQuantity,
						spdRowVersion = item2.SPDRowVersion,
						spdShipmentID = item2.spdShipmentID,
						spdShipmentIDNumber = item2.spdShipmentIDNumber,
						spdShipmentLineID = item2.spdShipmentLineID,
						spdShipmentPackageID = item2.spdShipmentPackageID,
						spdShipmentPackageLineID = item2.spdShipmentPackageLineID,
						spdTotalPriceBase = item2.spdTotalPriceBase,
						spdTotalPriceForeign = item2.spdTotalPriceForeign,
						spdWeight = item2.spdWeight,
						spdWeightUnitOfMeasure = item2.spdWeightUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allShipmentPackageDetailsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ShipmentPackageDetails]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPShipmentPackageDetailDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allShipmentPackageDetailsDto,
				RecordCount = allShipmentPackageDetailsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShipmentPackageDetailDto>> Process_GetShipmentPackageDetail(Guid shipmentPackageDetailId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPShipmentPackageDetailDto shipmentPackageDetailDto = null;
		ERPResponseMessageDto<ERPShipmentPackageDetailDto> result;
		try
		{
			IERPShipmentPackageDetailRepository iERPShipmentPackageDetailRepository = (base.ERPShipmentPackageDetailRepository = new ERPShipmentPackageDetailRepository(base.ApiClientContext));
			using (iERPShipmentPackageDetailRepository)
			{
				ERPShipmentPackageDetailInformationDto eRPShipmentPackageDetailInformationDto = await base.ERPShipmentPackageDetailRepository.GetShipmentPackageDetail(shipmentPackageDetailId);
				shipmentPackageDetailDto = new ERPShipmentPackageDetailDto
				{
					spdCommodityDescription = eRPShipmentPackageDetailInformationDto.spdCommodityDescription,
					spdCountryOfManufacture = eRPShipmentPackageDetailInformationDto.spdCountryOfManufacture,
					spdCreatedBy = eRPShipmentPackageDetailInformationDto.spdCreatedBy,
					spdCreatedDate = eRPShipmentPackageDetailInformationDto.spdCreatedDate,
					spdUniqueID = eRPShipmentPackageDetailInformationDto.spdUniqueID,
					spdPartID = eRPShipmentPackageDetailInformationDto.spdPartID,
					spdPartRevisionID = eRPShipmentPackageDetailInformationDto.spdPartRevisionID,
					spdQuantity = eRPShipmentPackageDetailInformationDto.spdQuantity,
					spdRowVersion = eRPShipmentPackageDetailInformationDto.SPDRowVersion,
					spdShipmentID = eRPShipmentPackageDetailInformationDto.spdShipmentID,
					spdShipmentIDNumber = eRPShipmentPackageDetailInformationDto.spdShipmentIDNumber,
					spdShipmentLineID = eRPShipmentPackageDetailInformationDto.spdShipmentLineID,
					spdShipmentPackageID = eRPShipmentPackageDetailInformationDto.spdShipmentPackageID,
					spdShipmentPackageLineID = eRPShipmentPackageDetailInformationDto.spdShipmentPackageLineID,
					spdTotalPriceBase = eRPShipmentPackageDetailInformationDto.spdTotalPriceBase,
					spdTotalPriceForeign = eRPShipmentPackageDetailInformationDto.spdTotalPriceForeign,
					spdWeight = eRPShipmentPackageDetailInformationDto.spdWeight,
					spdWeightUnitOfMeasure = eRPShipmentPackageDetailInformationDto.spdWeightUnitOfMeasure,
					CustomFields = eRPShipmentPackageDetailInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ShipmentPackageDetails []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentPackageDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = shipmentPackageDetailDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShipmentPackageDetailDto>> Process_PutShipmentPackageDetail(ERPShipmentPackageDetailDto shipmentPackageDetail)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPShipmentPackageDetailDto createdObject = null;
		ERPResponseMessageDto<ERPShipmentPackageDetailDto> result;
		try
		{
			IERPShipmentPackageDetailRepository iERPShipmentPackageDetailRepository = (base.ERPShipmentPackageDetailRepository = new ERPShipmentPackageDetailRepository(base.ApiClientContext));
			using (iERPShipmentPackageDetailRepository)
			{
				APIValidationInfoDto postResult = await base.ERPShipmentPackageDetailRepository.SaveShipmentPackageDetail(shipmentPackageDetail);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPShipmentPackageDetailInformationDto eRPShipmentPackageDetailInformationDto = await base.ERPShipmentPackageDetailRepository.GetShipmentPackageDetail(shipmentPackageDetail.spdUniqueID);
					createdObject = new ERPShipmentPackageDetailDto
					{
						spdCommodityDescription = eRPShipmentPackageDetailInformationDto.spdCommodityDescription,
						spdCountryOfManufacture = eRPShipmentPackageDetailInformationDto.spdCountryOfManufacture,
						spdCreatedBy = eRPShipmentPackageDetailInformationDto.spdCreatedBy,
						spdCreatedDate = eRPShipmentPackageDetailInformationDto.spdCreatedDate,
						spdUniqueID = eRPShipmentPackageDetailInformationDto.spdUniqueID,
						spdPartID = eRPShipmentPackageDetailInformationDto.spdPartID,
						spdPartRevisionID = eRPShipmentPackageDetailInformationDto.spdPartRevisionID,
						spdQuantity = eRPShipmentPackageDetailInformationDto.spdQuantity,
						spdRowVersion = eRPShipmentPackageDetailInformationDto.SPDRowVersion,
						spdShipmentID = eRPShipmentPackageDetailInformationDto.spdShipmentID,
						spdShipmentIDNumber = eRPShipmentPackageDetailInformationDto.spdShipmentIDNumber,
						spdShipmentLineID = eRPShipmentPackageDetailInformationDto.spdShipmentLineID,
						spdShipmentPackageID = eRPShipmentPackageDetailInformationDto.spdShipmentPackageID,
						spdShipmentPackageLineID = eRPShipmentPackageDetailInformationDto.spdShipmentPackageLineID,
						spdTotalPriceBase = eRPShipmentPackageDetailInformationDto.spdTotalPriceBase,
						spdTotalPriceForeign = eRPShipmentPackageDetailInformationDto.spdTotalPriceForeign,
						spdWeight = eRPShipmentPackageDetailInformationDto.spdWeight,
						spdWeightUnitOfMeasure = eRPShipmentPackageDetailInformationDto.spdWeightUnitOfMeasure,
						CustomFields = eRPShipmentPackageDetailInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ShipmentPackageDetail [{shipmentPackageDetail.spdUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentPackageDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteShipmentPackageDetail(Guid shipmentPackageDetailId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentPackageDetailRepository iERPShipmentPackageDetailRepository = (base.ERPShipmentPackageDetailRepository = new ERPShipmentPackageDetailRepository(base.ApiClientContext));
		using (iERPShipmentPackageDetailRepository)
		{
			if (!(await base.ERPShipmentPackageDetailRepository.DoesShipmentPackageDetailExist(shipmentPackageDetailId)))
			{
				base.ErrorsList.Add($"ShipmentPackageDetail [{shipmentPackageDetailId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPShipmentPackageDetailInformationDto eRPShipmentPackageDetailInformationDto = await base.ERPShipmentPackageDetailRepository.GetShipmentPackageDetail(shipmentPackageDetailId);
				string text = await base.ERPShipmentPackageDetailRepository.WhereUsed("ShipmentPackageDetails", new object[3] { eRPShipmentPackageDetailInformationDto.spdShipmentID, eRPShipmentPackageDetailInformationDto.spdShipmentLineID, eRPShipmentPackageDetailInformationDto.spdShipmentPackageLineID }, new object[3] { "spdShipmentID", "spdShipmentLineID", "spdShipmentPackageLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ShipmentPackageDetail cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPShipmentPackageDetailDto>> Process_DeleteShipmentPackageDetail(Guid shipmentPackageDetailId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPShipmentPackageDetailDto> result;
		try
		{
			IERPShipmentPackageDetailRepository iERPShipmentPackageDetailRepository = (base.ERPShipmentPackageDetailRepository = new ERPShipmentPackageDetailRepository(base.ApiClientContext));
			using (iERPShipmentPackageDetailRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPShipmentPackageDetailRepository.DeleteRowFromTable("ShipmentPackageDetails", "spd", shipmentPackageDetailId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ShipmentPackageDetail [{shipmentPackageDetailId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentPackageDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPShipmentPackageDetailDto()
			};
		}
		return result;
	}
}
