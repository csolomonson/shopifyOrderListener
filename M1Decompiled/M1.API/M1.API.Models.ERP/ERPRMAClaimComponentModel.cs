using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPRMAClaimComponentModel : ERPBaseModel, IERPRMAClaimComponentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllRMAClaimComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPRMAClaimComponentRepository iERPRMAClaimComponentRepository = (base.ERPRMAClaimComponentRepository = new ERPRMAClaimComponentRepository(base.ApiClientContext));
		using (iERPRMAClaimComponentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPRMAClaimComponentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPRMAClaimComponentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPRMAClaimComponentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPRMAClaimComponentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetRMAClaimComponent(Guid rMAClaimComponentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAClaimComponentRepository iERPRMAClaimComponentRepository = (base.ERPRMAClaimComponentRepository = new ERPRMAClaimComponentRepository(base.ApiClientContext));
		using (iERPRMAClaimComponentRepository)
		{
			if (!(await base.ERPRMAClaimComponentRepository.DoesRMAClaimComponentExist(rMAClaimComponentId)))
			{
				errorsList.Add($"RMAClaimComponent [{rMAClaimComponentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutRMAClaimComponent(ERPRMAClaimComponentDto rMAClaimComponent)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAClaimComponentRepository iERPRMAClaimComponentRepository = (base.ERPRMAClaimComponentRepository = new ERPRMAClaimComponentRepository(base.ApiClientContext));
		using (iERPRMAClaimComponentRepository)
		{
			if (!string.IsNullOrWhiteSpace(rMAClaimComponent.raoRmaClaimID) && !(await base.ERPRMAClaimComponentRepository.DoesRecordExistInTableUsingKeys("RMAClaims", new object[1] { "RAPRMACLAIMID" }, new object[1] { rMAClaimComponent.raoRmaClaimID })))
			{
				errorsList.Add("raoRmaClaimID [" + rMAClaimComponent.raoRmaClaimID + "] not found.");
			}
			if (rMAClaimComponent.raoRmaClaimLineID > 0 && !(await base.ERPRMAClaimComponentRepository.DoesRecordExistInTableUsingKeys("RMAClaimLines", new object[2] { "RALRMACLAIMID", "RALRMACLAIMLINEID" }, new object[2] { rMAClaimComponent.raoRmaClaimID, rMAClaimComponent.raoRmaClaimLineID })))
			{
				errorsList.Add($"raoRmaClaimLineID [{rMAClaimComponent.raoRmaClaimLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimComponent.raoPartID) && !(await base.ERPRMAClaimComponentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { rMAClaimComponent.raoPartID })))
			{
				errorsList.Add("raoPartID [" + rMAClaimComponent.raoPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimComponent.raoPartRevisionID) && !(await base.ERPRMAClaimComponentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { rMAClaimComponent.raoPartID, rMAClaimComponent.raoPartRevisionID })))
			{
				errorsList.Add("raoPartRevisionID [" + rMAClaimComponent.raoPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimComponent.raoPartWarehouseLocationID) && !(await base.ERPRMAClaimComponentRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { rMAClaimComponent.raoPartID, rMAClaimComponent.raoPartRevisionID, rMAClaimComponent.raoPartWarehouseLocationID })))
			{
				errorsList.Add("raoPartWarehouseLocationID [" + rMAClaimComponent.raoPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimComponent.raoPartBinID) && !(await base.ERPRMAClaimComponentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { rMAClaimComponent.raoPartID, rMAClaimComponent.raoPartRevisionID, rMAClaimComponent.raoPartWarehouseLocationID, rMAClaimComponent.raoPartBinID })))
			{
				errorsList.Add("raoPartBinID [" + rMAClaimComponent.raoPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimComponent.raoShipmentID) && !(await base.ERPRMAClaimComponentRepository.DoesRecordExistInTableUsingKeys("Shipments", new object[1] { "SMPSHIPMENTID" }, new object[1] { rMAClaimComponent.raoShipmentID })))
			{
				errorsList.Add("raoShipmentID [" + rMAClaimComponent.raoShipmentID + "] not found.");
			}
			if (rMAClaimComponent.raoShipmentLineID > 0 && !(await base.ERPRMAClaimComponentRepository.DoesRecordExistInTableUsingKeys("ShipmentLines", new object[2] { "SMLSHIPMENTID", "SMLSHIPMENTLINEID" }, new object[2] { rMAClaimComponent.raoShipmentID, rMAClaimComponent.raoShipmentLineID })))
			{
				errorsList.Add($"raoShipmentLineID [{rMAClaimComponent.raoShipmentLineID}] not found.");
			}
			if (rMAClaimComponent.raoShipmentComponentID > 0 && !(await base.ERPRMAClaimComponentRepository.DoesRecordExistInTableUsingKeys("ShipmentComponents", new object[3] { "SMOSHIPMENTID", "SMOSHIPMENTLINEID", "SMOSHIPMENTCOMPONENTID" }, new object[3] { rMAClaimComponent.raoShipmentID, rMAClaimComponent.raoShipmentLineID, rMAClaimComponent.raoShipmentComponentID })))
			{
				errorsList.Add($"raoShipmentComponentID [{rMAClaimComponent.raoShipmentComponentID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPRMAClaimComponentDto>>> Process_GetAllRMAClaimComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPRMAClaimComponentDto> allRMAClaimComponentsDto = new List<ERPRMAClaimComponentDto>();
		ERPResponseMessageDto<IList<ERPRMAClaimComponentDto>> result;
		try
		{
			IERPRMAClaimComponentRepository iERPRMAClaimComponentRepository = (base.ERPRMAClaimComponentRepository = new ERPRMAClaimComponentRepository(base.ApiClientContext));
			using (iERPRMAClaimComponentRepository)
			{
				foreach (ERPRMAClaimComponentInformationDto item2 in await base.ERPRMAClaimComponentRepository.GetAllRMAClaimComponents(pageSize, pageNumber, filter, orderBy))
				{
					ERPRMAClaimComponentDto item = new ERPRMAClaimComponentDto
					{
						raoAdditionalQuantity = item2.raoAdditionalQuantity,
						raoCreatedBy = item2.raoCreatedBy,
						raoCreatedDate = item2.raoCreatedDate,
						raoDescription = item2.raoDescription,
						raoUniqueID = item2.raoUniqueID,
						raoReceivedComplete = item2.raoReceivedComplete,
						raoParentQuantity = item2.raoParentQuantity,
						raoPartBinID = item2.raoPartBinID,
						raoPartID = item2.raoPartID,
						raoPartRevisionID = item2.raoPartRevisionID,
						raoPartWarehouseLocationID = item2.raoPartWarehouseLocationID,
						raoQuantity = item2.raoQuantity,
						raoQuantityPerParent = item2.raoQuantityPerParent,
						raoQuantityReceived = item2.raoQuantityReceived,
						raoRmaClaimID = item2.raoRmaClaimID,
						raoRmaClaimLineID = item2.raoRmaClaimLineID,
						raoRowVersion = item2.raoRowVersion,
						raoRmaClaimComponentID = item2.raoRmaClaimComponentID,
						raoShipmentComponentID = item2.raoShipmentComponentID,
						raoShipmentID = item2.raoShipmentID,
						raoShipmentLineID = item2.raoShipmentLineID,
						raoUnitOfMeasure = item2.raoUnitOfMeasure,
						raoWeight = item2.raoWeight,
						CustomFields = item2.CustomFields
					};
					allRMAClaimComponentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all RMAClaimComponents]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPRMAClaimComponentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allRMAClaimComponentsDto,
				RecordCount = allRMAClaimComponentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRMAClaimComponentDto>> Process_GetRMAClaimComponent(Guid rMAClaimComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPRMAClaimComponentDto rMAClaimComponentDto = null;
		ERPResponseMessageDto<ERPRMAClaimComponentDto> result;
		try
		{
			IERPRMAClaimComponentRepository iERPRMAClaimComponentRepository = (base.ERPRMAClaimComponentRepository = new ERPRMAClaimComponentRepository(base.ApiClientContext));
			using (iERPRMAClaimComponentRepository)
			{
				ERPRMAClaimComponentInformationDto eRPRMAClaimComponentInformationDto = await base.ERPRMAClaimComponentRepository.GetRMAClaimComponent(rMAClaimComponentId);
				rMAClaimComponentDto = new ERPRMAClaimComponentDto
				{
					raoAdditionalQuantity = eRPRMAClaimComponentInformationDto.raoAdditionalQuantity,
					raoCreatedBy = eRPRMAClaimComponentInformationDto.raoCreatedBy,
					raoCreatedDate = eRPRMAClaimComponentInformationDto.raoCreatedDate,
					raoDescription = eRPRMAClaimComponentInformationDto.raoDescription,
					raoUniqueID = eRPRMAClaimComponentInformationDto.raoUniqueID,
					raoReceivedComplete = eRPRMAClaimComponentInformationDto.raoReceivedComplete,
					raoParentQuantity = eRPRMAClaimComponentInformationDto.raoParentQuantity,
					raoPartBinID = eRPRMAClaimComponentInformationDto.raoPartBinID,
					raoPartID = eRPRMAClaimComponentInformationDto.raoPartID,
					raoPartRevisionID = eRPRMAClaimComponentInformationDto.raoPartRevisionID,
					raoPartWarehouseLocationID = eRPRMAClaimComponentInformationDto.raoPartWarehouseLocationID,
					raoQuantity = eRPRMAClaimComponentInformationDto.raoQuantity,
					raoQuantityPerParent = eRPRMAClaimComponentInformationDto.raoQuantityPerParent,
					raoQuantityReceived = eRPRMAClaimComponentInformationDto.raoQuantityReceived,
					raoRmaClaimID = eRPRMAClaimComponentInformationDto.raoRmaClaimID,
					raoRmaClaimLineID = eRPRMAClaimComponentInformationDto.raoRmaClaimLineID,
					raoRowVersion = eRPRMAClaimComponentInformationDto.raoRowVersion,
					raoRmaClaimComponentID = eRPRMAClaimComponentInformationDto.raoRmaClaimComponentID,
					raoShipmentComponentID = eRPRMAClaimComponentInformationDto.raoShipmentComponentID,
					raoShipmentID = eRPRMAClaimComponentInformationDto.raoShipmentID,
					raoShipmentLineID = eRPRMAClaimComponentInformationDto.raoShipmentLineID,
					raoUnitOfMeasure = eRPRMAClaimComponentInformationDto.raoUnitOfMeasure,
					raoWeight = eRPRMAClaimComponentInformationDto.raoWeight,
					CustomFields = eRPRMAClaimComponentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the RMAClaimComponents []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAClaimComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = rMAClaimComponentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRMAClaimComponentDto>> Process_PutRMAClaimComponent(ERPRMAClaimComponentDto rMAClaimComponent)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPRMAClaimComponentDto createdObject = null;
		ERPResponseMessageDto<ERPRMAClaimComponentDto> result;
		try
		{
			IERPRMAClaimComponentRepository iERPRMAClaimComponentRepository = (base.ERPRMAClaimComponentRepository = new ERPRMAClaimComponentRepository(base.ApiClientContext));
			using (iERPRMAClaimComponentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPRMAClaimComponentRepository.SaveRMAClaimComponent(rMAClaimComponent);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPRMAClaimComponentInformationDto eRPRMAClaimComponentInformationDto = await base.ERPRMAClaimComponentRepository.GetRMAClaimComponent(rMAClaimComponent.raoUniqueID);
					createdObject = new ERPRMAClaimComponentDto
					{
						raoAdditionalQuantity = eRPRMAClaimComponentInformationDto.raoAdditionalQuantity,
						raoCreatedBy = eRPRMAClaimComponentInformationDto.raoCreatedBy,
						raoCreatedDate = eRPRMAClaimComponentInformationDto.raoCreatedDate,
						raoDescription = eRPRMAClaimComponentInformationDto.raoDescription,
						raoUniqueID = eRPRMAClaimComponentInformationDto.raoUniqueID,
						raoReceivedComplete = eRPRMAClaimComponentInformationDto.raoReceivedComplete,
						raoParentQuantity = eRPRMAClaimComponentInformationDto.raoParentQuantity,
						raoPartBinID = eRPRMAClaimComponentInformationDto.raoPartBinID,
						raoPartID = eRPRMAClaimComponentInformationDto.raoPartID,
						raoPartRevisionID = eRPRMAClaimComponentInformationDto.raoPartRevisionID,
						raoPartWarehouseLocationID = eRPRMAClaimComponentInformationDto.raoPartWarehouseLocationID,
						raoQuantity = eRPRMAClaimComponentInformationDto.raoQuantity,
						raoQuantityPerParent = eRPRMAClaimComponentInformationDto.raoQuantityPerParent,
						raoQuantityReceived = eRPRMAClaimComponentInformationDto.raoQuantityReceived,
						raoRmaClaimID = eRPRMAClaimComponentInformationDto.raoRmaClaimID,
						raoRmaClaimLineID = eRPRMAClaimComponentInformationDto.raoRmaClaimLineID,
						raoRowVersion = eRPRMAClaimComponentInformationDto.raoRowVersion,
						raoRmaClaimComponentID = eRPRMAClaimComponentInformationDto.raoRmaClaimComponentID,
						raoShipmentComponentID = eRPRMAClaimComponentInformationDto.raoShipmentComponentID,
						raoShipmentID = eRPRMAClaimComponentInformationDto.raoShipmentID,
						raoShipmentLineID = eRPRMAClaimComponentInformationDto.raoShipmentLineID,
						raoUnitOfMeasure = eRPRMAClaimComponentInformationDto.raoUnitOfMeasure,
						raoWeight = eRPRMAClaimComponentInformationDto.raoWeight,
						CustomFields = eRPRMAClaimComponentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing RMAClaimComponent [{rMAClaimComponent.raoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAClaimComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteRMAClaimComponent(Guid rMAClaimComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAClaimComponentRepository iERPRMAClaimComponentRepository = (base.ERPRMAClaimComponentRepository = new ERPRMAClaimComponentRepository(base.ApiClientContext));
		using (iERPRMAClaimComponentRepository)
		{
			if (!(await base.ERPRMAClaimComponentRepository.DoesRMAClaimComponentExist(rMAClaimComponentId)))
			{
				base.ErrorsList.Add($"RMAClaimComponent [{rMAClaimComponentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPRMAClaimComponentInformationDto eRPRMAClaimComponentInformationDto = await base.ERPRMAClaimComponentRepository.GetRMAClaimComponent(rMAClaimComponentId);
				string text = await base.ERPRMAClaimComponentRepository.WhereUsed("RMAClaimComponents", new object[3] { eRPRMAClaimComponentInformationDto.raoRmaClaimID, eRPRMAClaimComponentInformationDto.raoRmaClaimLineID, eRPRMAClaimComponentInformationDto.raoRmaClaimComponentID }, new object[3] { "raoRmaClaimID", "raoRmaClaimLineID", "raoRmaClaimComponentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("RMAClaimComponent cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPRMAClaimComponentDto>> Process_DeleteRMAClaimComponent(Guid rMAClaimComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPRMAClaimComponentDto> result;
		try
		{
			IERPRMAClaimComponentRepository iERPRMAClaimComponentRepository = (base.ERPRMAClaimComponentRepository = new ERPRMAClaimComponentRepository(base.ApiClientContext));
			using (iERPRMAClaimComponentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPRMAClaimComponentRepository.DeleteRowFromTable("RMAClaimComponents", "rao", rMAClaimComponentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of RMAClaimComponent [{rMAClaimComponentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAClaimComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPRMAClaimComponentDto()
			};
		}
		return result;
	}
}
