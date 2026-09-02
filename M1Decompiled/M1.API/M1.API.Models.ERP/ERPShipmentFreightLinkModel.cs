using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPShipmentFreightLinkModel : ERPBaseModel, IERPShipmentFreightLinkModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllShipmentFreightLinks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPShipmentFreightLinkRepository iERPShipmentFreightLinkRepository = (base.ERPShipmentFreightLinkRepository = new ERPShipmentFreightLinkRepository(base.ApiClientContext));
		using (iERPShipmentFreightLinkRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPShipmentFreightLinkRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPShipmentFreightLinkRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPShipmentFreightLinkRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPShipmentFreightLinkRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetShipmentFreightLink(Guid shipmentFreightLinkId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentFreightLinkRepository iERPShipmentFreightLinkRepository = (base.ERPShipmentFreightLinkRepository = new ERPShipmentFreightLinkRepository(base.ApiClientContext));
		using (iERPShipmentFreightLinkRepository)
		{
			if (!(await base.ERPShipmentFreightLinkRepository.DoesShipmentFreightLinkExist(shipmentFreightLinkId)))
			{
				errorsList.Add($"ShipmentFreightLink [{shipmentFreightLinkId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutShipmentFreightLink(ERPShipmentFreightLinkDto shipmentFreightLink)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentFreightLinkRepository iERPShipmentFreightLinkRepository = (base.ERPShipmentFreightLinkRepository = new ERPShipmentFreightLinkRepository(base.ApiClientContext));
		using (iERPShipmentFreightLinkRepository)
		{
			if (!string.IsNullOrWhiteSpace(shipmentFreightLink.smxFreightShipmentID) && !(await base.ERPShipmentFreightLinkRepository.DoesRecordExistInTableUsingKeys("FreightShipments", new object[1] { "FSPFREIGHTSHIPMENTID" }, new object[1] { shipmentFreightLink.smxFreightShipmentID })))
			{
				errorsList.Add("smxFreightShipmentID [" + shipmentFreightLink.smxFreightShipmentID + "] not found.");
			}
			if (shipmentFreightLink.smxFreightPackageID > 0 && !(await base.ERPShipmentFreightLinkRepository.DoesRecordExistInTableUsingKeys("FreightPackages", new object[2] { "FSLFREIGHTSHIPMENTID", "FSLFREIGHTPACKAGEID" }, new object[2] { shipmentFreightLink.smxFreightShipmentID, shipmentFreightLink.smxFreightPackageID })))
			{
				errorsList.Add($"smxFreightPackageID [{shipmentFreightLink.smxFreightPackageID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentFreightLink.smxShipmentID) && !(await base.ERPShipmentFreightLinkRepository.DoesRecordExistInTableUsingKeys("Shipments", new object[1] { "SMPSHIPMENTID" }, new object[1] { shipmentFreightLink.smxShipmentID })))
			{
				errorsList.Add("smxShipmentID [" + shipmentFreightLink.smxShipmentID + "] not found.");
			}
			if (shipmentFreightLink.smxShipmentLineID > 0 && !(await base.ERPShipmentFreightLinkRepository.DoesRecordExistInTableUsingKeys("ShipmentLines", new object[2] { "SMLSHIPMENTID", "SMLSHIPMENTLINEID" }, new object[2] { shipmentFreightLink.smxShipmentID, shipmentFreightLink.smxShipmentLineID })))
			{
				errorsList.Add($"smxShipmentLineID [{shipmentFreightLink.smxShipmentLineID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPShipmentFreightLinkDto>>> Process_GetAllShipmentFreightLinks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPShipmentFreightLinkDto> allShipmentFreightLinksDto = new List<ERPShipmentFreightLinkDto>();
		ERPResponseMessageDto<IList<ERPShipmentFreightLinkDto>> result;
		try
		{
			IERPShipmentFreightLinkRepository iERPShipmentFreightLinkRepository = (base.ERPShipmentFreightLinkRepository = new ERPShipmentFreightLinkRepository(base.ApiClientContext));
			using (iERPShipmentFreightLinkRepository)
			{
				foreach (ERPShipmentFreightLinkInformationDto item2 in await base.ERPShipmentFreightLinkRepository.GetAllShipmentFreightLinks(pageSize, pageNumber, filter, orderBy))
				{
					ERPShipmentFreightLinkDto item = new ERPShipmentFreightLinkDto
					{
						smxCreatedBy = item2.smxCreatedBy,
						smxCreatedDate = item2.smxCreatedDate,
						smxUniqueID = item2.smxUniqueID,
						smxFreightCharges = item2.smxFreightCharges,
						smxFreightPackageID = item2.smxFreightPackageID,
						smxFreightShipmentID = item2.smxFreightShipmentID,
						smxClosed = item2.smxClosed,
						smxLinkPctCharge = item2.smxLinkPctCharge,
						smxPackagePartialCount = item2.smxPackagePartialCount,
						smxPackagePartialWeight = item2.smxPackagePartialWeight,
						smxRowVersion = item2.smxRowVersion,
						smxShipmentFreightLinkID = item2.smxShipmentFreightLinkID,
						smxShipmentID = item2.smxShipmentID,
						smxShipmentLineID = item2.smxShipmentLineID,
						CustomFields = item2.CustomFields
					};
					allShipmentFreightLinksDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ShipmentFreightLinks]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPShipmentFreightLinkDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allShipmentFreightLinksDto,
				RecordCount = allShipmentFreightLinksDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShipmentFreightLinkDto>> Process_GetShipmentFreightLink(Guid shipmentFreightLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPShipmentFreightLinkDto shipmentFreightLinkDto = null;
		ERPResponseMessageDto<ERPShipmentFreightLinkDto> result;
		try
		{
			IERPShipmentFreightLinkRepository iERPShipmentFreightLinkRepository = (base.ERPShipmentFreightLinkRepository = new ERPShipmentFreightLinkRepository(base.ApiClientContext));
			using (iERPShipmentFreightLinkRepository)
			{
				ERPShipmentFreightLinkInformationDto eRPShipmentFreightLinkInformationDto = await base.ERPShipmentFreightLinkRepository.GetShipmentFreightLink(shipmentFreightLinkId);
				shipmentFreightLinkDto = new ERPShipmentFreightLinkDto
				{
					smxCreatedBy = eRPShipmentFreightLinkInformationDto.smxCreatedBy,
					smxCreatedDate = eRPShipmentFreightLinkInformationDto.smxCreatedDate,
					smxUniqueID = eRPShipmentFreightLinkInformationDto.smxUniqueID,
					smxFreightCharges = eRPShipmentFreightLinkInformationDto.smxFreightCharges,
					smxFreightPackageID = eRPShipmentFreightLinkInformationDto.smxFreightPackageID,
					smxFreightShipmentID = eRPShipmentFreightLinkInformationDto.smxFreightShipmentID,
					smxClosed = eRPShipmentFreightLinkInformationDto.smxClosed,
					smxLinkPctCharge = eRPShipmentFreightLinkInformationDto.smxLinkPctCharge,
					smxPackagePartialCount = eRPShipmentFreightLinkInformationDto.smxPackagePartialCount,
					smxPackagePartialWeight = eRPShipmentFreightLinkInformationDto.smxPackagePartialWeight,
					smxRowVersion = eRPShipmentFreightLinkInformationDto.smxRowVersion,
					smxShipmentFreightLinkID = eRPShipmentFreightLinkInformationDto.smxShipmentFreightLinkID,
					smxShipmentID = eRPShipmentFreightLinkInformationDto.smxShipmentID,
					smxShipmentLineID = eRPShipmentFreightLinkInformationDto.smxShipmentLineID,
					CustomFields = eRPShipmentFreightLinkInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ShipmentFreightLinks []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentFreightLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = shipmentFreightLinkDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShipmentFreightLinkDto>> Process_PutShipmentFreightLink(ERPShipmentFreightLinkDto shipmentFreightLink)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPShipmentFreightLinkDto createdObject = null;
		ERPResponseMessageDto<ERPShipmentFreightLinkDto> result;
		try
		{
			IERPShipmentFreightLinkRepository iERPShipmentFreightLinkRepository = (base.ERPShipmentFreightLinkRepository = new ERPShipmentFreightLinkRepository(base.ApiClientContext));
			using (iERPShipmentFreightLinkRepository)
			{
				APIValidationInfoDto postResult = await base.ERPShipmentFreightLinkRepository.SaveShipmentFreightLink(shipmentFreightLink);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPShipmentFreightLinkInformationDto eRPShipmentFreightLinkInformationDto = await base.ERPShipmentFreightLinkRepository.GetShipmentFreightLink(shipmentFreightLink.smxUniqueID);
					createdObject = new ERPShipmentFreightLinkDto
					{
						smxCreatedBy = eRPShipmentFreightLinkInformationDto.smxCreatedBy,
						smxCreatedDate = eRPShipmentFreightLinkInformationDto.smxCreatedDate,
						smxUniqueID = eRPShipmentFreightLinkInformationDto.smxUniqueID,
						smxFreightCharges = eRPShipmentFreightLinkInformationDto.smxFreightCharges,
						smxFreightPackageID = eRPShipmentFreightLinkInformationDto.smxFreightPackageID,
						smxFreightShipmentID = eRPShipmentFreightLinkInformationDto.smxFreightShipmentID,
						smxClosed = eRPShipmentFreightLinkInformationDto.smxClosed,
						smxLinkPctCharge = eRPShipmentFreightLinkInformationDto.smxLinkPctCharge,
						smxPackagePartialCount = eRPShipmentFreightLinkInformationDto.smxPackagePartialCount,
						smxPackagePartialWeight = eRPShipmentFreightLinkInformationDto.smxPackagePartialWeight,
						smxRowVersion = eRPShipmentFreightLinkInformationDto.smxRowVersion,
						smxShipmentFreightLinkID = eRPShipmentFreightLinkInformationDto.smxShipmentFreightLinkID,
						smxShipmentID = eRPShipmentFreightLinkInformationDto.smxShipmentID,
						smxShipmentLineID = eRPShipmentFreightLinkInformationDto.smxShipmentLineID,
						CustomFields = eRPShipmentFreightLinkInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ShipmentFreightLink [{shipmentFreightLink.smxUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentFreightLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteShipmentFreightLink(Guid shipmentFreightLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentFreightLinkRepository iERPShipmentFreightLinkRepository = (base.ERPShipmentFreightLinkRepository = new ERPShipmentFreightLinkRepository(base.ApiClientContext));
		using (iERPShipmentFreightLinkRepository)
		{
			if (!(await base.ERPShipmentFreightLinkRepository.DoesShipmentFreightLinkExist(shipmentFreightLinkId)))
			{
				base.ErrorsList.Add($"ShipmentFreightLink [{shipmentFreightLinkId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPShipmentFreightLinkInformationDto eRPShipmentFreightLinkInformationDto = await base.ERPShipmentFreightLinkRepository.GetShipmentFreightLink(shipmentFreightLinkId);
				string text = await base.ERPShipmentFreightLinkRepository.WhereUsed("ShipmentFreightLinks", new object[3] { eRPShipmentFreightLinkInformationDto.smxFreightShipmentID, eRPShipmentFreightLinkInformationDto.smxFreightPackageID, eRPShipmentFreightLinkInformationDto.smxShipmentFreightLinkID }, new object[3] { "smxFreightShipmentID", "smxFreightPackageID", "smxShipmentFreightLinkID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ShipmentFreightLink cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPShipmentFreightLinkDto>> Process_DeleteShipmentFreightLink(Guid shipmentFreightLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPShipmentFreightLinkDto> result;
		try
		{
			IERPShipmentFreightLinkRepository iERPShipmentFreightLinkRepository = (base.ERPShipmentFreightLinkRepository = new ERPShipmentFreightLinkRepository(base.ApiClientContext));
			using (iERPShipmentFreightLinkRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPShipmentFreightLinkRepository.DeleteRowFromTable("ShipmentFreightLinks", "smx", shipmentFreightLinkId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ShipmentFreightLink [{shipmentFreightLinkId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentFreightLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPShipmentFreightLinkDto()
			};
		}
		return result;
	}
}
