using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPShipmentFreightReferenceModel : ERPBaseModel, IERPShipmentFreightReferenceModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllShipmentFreightReferences(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPShipmentFreightReferenceRepository iERPShipmentFreightReferenceRepository = (base.ERPShipmentFreightReferenceRepository = new ERPShipmentFreightReferenceRepository(base.ApiClientContext));
		using (iERPShipmentFreightReferenceRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPShipmentFreightReferenceRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPShipmentFreightReferenceRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPShipmentFreightReferenceRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPShipmentFreightReferenceRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetShipmentFreightReference(Guid shipmentFreightReferenceId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentFreightReferenceRepository iERPShipmentFreightReferenceRepository = (base.ERPShipmentFreightReferenceRepository = new ERPShipmentFreightReferenceRepository(base.ApiClientContext));
		using (iERPShipmentFreightReferenceRepository)
		{
			if (!(await base.ERPShipmentFreightReferenceRepository.DoesShipmentFreightReferenceExist(shipmentFreightReferenceId)))
			{
				errorsList.Add($"ShipmentFreightReference [{shipmentFreightReferenceId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutShipmentFreightReference(ERPShipmentFreightReferenceDto shipmentFreightReference)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentFreightReferenceRepository iERPShipmentFreightReferenceRepository = (base.ERPShipmentFreightReferenceRepository = new ERPShipmentFreightReferenceRepository(base.ApiClientContext));
		using (iERPShipmentFreightReferenceRepository)
		{
			if (!string.IsNullOrWhiteSpace(shipmentFreightReference.smrShipmentID) && !(await base.ERPShipmentFreightReferenceRepository.DoesRecordExistInTableUsingKeys("Shipments", new object[1] { "SMPSHIPMENTID" }, new object[1] { shipmentFreightReference.smrShipmentID })))
			{
				errorsList.Add("smrShipmentID [" + shipmentFreightReference.smrShipmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentFreightReference.smrFreightShipmentID) && !(await base.ERPShipmentFreightReferenceRepository.DoesRecordExistInTableUsingKeys("FreightShipments", new object[1] { "FSPFREIGHTSHIPMENTID" }, new object[1] { shipmentFreightReference.smrFreightShipmentID })))
			{
				errorsList.Add("smrFreightShipmentID [" + shipmentFreightReference.smrFreightShipmentID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPShipmentFreightReferenceDto>>> Process_GetAllShipmentFreightReferences(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPShipmentFreightReferenceDto> allShipmentFreightReferencesDto = new List<ERPShipmentFreightReferenceDto>();
		ERPResponseMessageDto<IList<ERPShipmentFreightReferenceDto>> result;
		try
		{
			IERPShipmentFreightReferenceRepository iERPShipmentFreightReferenceRepository = (base.ERPShipmentFreightReferenceRepository = new ERPShipmentFreightReferenceRepository(base.ApiClientContext));
			using (iERPShipmentFreightReferenceRepository)
			{
				foreach (ERPShipmentFreightReferenceInformationDto item2 in await base.ERPShipmentFreightReferenceRepository.GetAllShipmentFreightReferences(pageSize, pageNumber, filter, orderBy))
				{
					ERPShipmentFreightReferenceDto item = new ERPShipmentFreightReferenceDto
					{
						smrCreatedBy = item2.smrCreatedBy,
						smrCreatedDate = item2.smrCreatedDate,
						smrUniqueID = item2.smrUniqueID,
						smrFreightShipmentID = item2.smrFreightShipmentID,
						smrRowVersion = item2.smrRowVersion,
						smrShipmentFreightReferenceID = item2.smrShipmentFreightReferenceID,
						smrShipmentID = item2.smrShipmentID,
						CustomFields = item2.CustomFields
					};
					allShipmentFreightReferencesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ShipmentFreightReferences]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPShipmentFreightReferenceDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allShipmentFreightReferencesDto,
				RecordCount = allShipmentFreightReferencesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShipmentFreightReferenceDto>> Process_GetShipmentFreightReference(Guid shipmentFreightReferenceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPShipmentFreightReferenceDto shipmentFreightReferenceDto = null;
		ERPResponseMessageDto<ERPShipmentFreightReferenceDto> result;
		try
		{
			IERPShipmentFreightReferenceRepository iERPShipmentFreightReferenceRepository = (base.ERPShipmentFreightReferenceRepository = new ERPShipmentFreightReferenceRepository(base.ApiClientContext));
			using (iERPShipmentFreightReferenceRepository)
			{
				ERPShipmentFreightReferenceInformationDto eRPShipmentFreightReferenceInformationDto = await base.ERPShipmentFreightReferenceRepository.GetShipmentFreightReference(shipmentFreightReferenceId);
				shipmentFreightReferenceDto = new ERPShipmentFreightReferenceDto
				{
					smrCreatedBy = eRPShipmentFreightReferenceInformationDto.smrCreatedBy,
					smrCreatedDate = eRPShipmentFreightReferenceInformationDto.smrCreatedDate,
					smrUniqueID = eRPShipmentFreightReferenceInformationDto.smrUniqueID,
					smrFreightShipmentID = eRPShipmentFreightReferenceInformationDto.smrFreightShipmentID,
					smrRowVersion = eRPShipmentFreightReferenceInformationDto.smrRowVersion,
					smrShipmentFreightReferenceID = eRPShipmentFreightReferenceInformationDto.smrShipmentFreightReferenceID,
					smrShipmentID = eRPShipmentFreightReferenceInformationDto.smrShipmentID,
					CustomFields = eRPShipmentFreightReferenceInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ShipmentFreightReferences []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentFreightReferenceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = shipmentFreightReferenceDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShipmentFreightReferenceDto>> Process_PutShipmentFreightReference(ERPShipmentFreightReferenceDto shipmentFreightReference)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPShipmentFreightReferenceDto createdObject = null;
		ERPResponseMessageDto<ERPShipmentFreightReferenceDto> result;
		try
		{
			IERPShipmentFreightReferenceRepository iERPShipmentFreightReferenceRepository = (base.ERPShipmentFreightReferenceRepository = new ERPShipmentFreightReferenceRepository(base.ApiClientContext));
			using (iERPShipmentFreightReferenceRepository)
			{
				APIValidationInfoDto postResult = await base.ERPShipmentFreightReferenceRepository.SaveShipmentFreightReference(shipmentFreightReference);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPShipmentFreightReferenceInformationDto eRPShipmentFreightReferenceInformationDto = await base.ERPShipmentFreightReferenceRepository.GetShipmentFreightReference(shipmentFreightReference.smrUniqueID);
					createdObject = new ERPShipmentFreightReferenceDto
					{
						smrCreatedBy = eRPShipmentFreightReferenceInformationDto.smrCreatedBy,
						smrCreatedDate = eRPShipmentFreightReferenceInformationDto.smrCreatedDate,
						smrUniqueID = eRPShipmentFreightReferenceInformationDto.smrUniqueID,
						smrFreightShipmentID = eRPShipmentFreightReferenceInformationDto.smrFreightShipmentID,
						smrRowVersion = eRPShipmentFreightReferenceInformationDto.smrRowVersion,
						smrShipmentFreightReferenceID = eRPShipmentFreightReferenceInformationDto.smrShipmentFreightReferenceID,
						smrShipmentID = eRPShipmentFreightReferenceInformationDto.smrShipmentID,
						CustomFields = eRPShipmentFreightReferenceInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ShipmentFreightReference [{shipmentFreightReference.smrUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentFreightReferenceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteShipmentFreightReference(Guid shipmentFreightReferenceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentFreightReferenceRepository iERPShipmentFreightReferenceRepository = (base.ERPShipmentFreightReferenceRepository = new ERPShipmentFreightReferenceRepository(base.ApiClientContext));
		using (iERPShipmentFreightReferenceRepository)
		{
			if (!(await base.ERPShipmentFreightReferenceRepository.DoesShipmentFreightReferenceExist(shipmentFreightReferenceId)))
			{
				base.ErrorsList.Add($"ShipmentFreightReference [{shipmentFreightReferenceId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPShipmentFreightReferenceInformationDto eRPShipmentFreightReferenceInformationDto = await base.ERPShipmentFreightReferenceRepository.GetShipmentFreightReference(shipmentFreightReferenceId);
				string text = await base.ERPShipmentFreightReferenceRepository.WhereUsed("ShipmentFreightReferences", new object[2] { eRPShipmentFreightReferenceInformationDto.smrShipmentID, eRPShipmentFreightReferenceInformationDto.smrShipmentFreightReferenceID }, new object[2] { "smrShipmentID", "smrShipmentFreightReferenceID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ShipmentFreightReference cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPShipmentFreightReferenceDto>> Process_DeleteShipmentFreightReference(Guid shipmentFreightReferenceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPShipmentFreightReferenceDto> result;
		try
		{
			IERPShipmentFreightReferenceRepository iERPShipmentFreightReferenceRepository = (base.ERPShipmentFreightReferenceRepository = new ERPShipmentFreightReferenceRepository(base.ApiClientContext));
			using (iERPShipmentFreightReferenceRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPShipmentFreightReferenceRepository.DeleteRowFromTable("ShipmentFreightReferences", "smr", shipmentFreightReferenceId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ShipmentFreightReference [{shipmentFreightReferenceId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentFreightReferenceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPShipmentFreightReferenceDto()
			};
		}
		return result;
	}
}
