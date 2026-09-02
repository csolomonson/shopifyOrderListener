using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPRMAReceiptComponentModel : ERPBaseModel, IERPRMAReceiptComponentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllRMAReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPRMAReceiptComponentRepository iERPRMAReceiptComponentRepository = (base.ERPRMAReceiptComponentRepository = new ERPRMAReceiptComponentRepository(base.ApiClientContext));
		using (iERPRMAReceiptComponentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPRMAReceiptComponentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPRMAReceiptComponentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPRMAReceiptComponentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPRMAReceiptComponentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetRMAReceiptComponent(Guid rMAReceiptComponentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAReceiptComponentRepository iERPRMAReceiptComponentRepository = (base.ERPRMAReceiptComponentRepository = new ERPRMAReceiptComponentRepository(base.ApiClientContext));
		using (iERPRMAReceiptComponentRepository)
		{
			if (!(await base.ERPRMAReceiptComponentRepository.DoesRMAReceiptComponentExist(rMAReceiptComponentId)))
			{
				errorsList.Add($"RMAReceiptComponent [{rMAReceiptComponentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutRMAReceiptComponent(ERPRMAReceiptComponentDto rMAReceiptComponent)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAReceiptComponentRepository iERPRMAReceiptComponentRepository = (base.ERPRMAReceiptComponentRepository = new ERPRMAReceiptComponentRepository(base.ApiClientContext));
		using (iERPRMAReceiptComponentRepository)
		{
			if (!string.IsNullOrWhiteSpace(rMAReceiptComponent.rroRmaReceiptID) && !(await base.ERPRMAReceiptComponentRepository.DoesRecordExistInTableUsingKeys("RMAReceipts", new object[1] { "RRPRMARECEIPTID" }, new object[1] { rMAReceiptComponent.rroRmaReceiptID })))
			{
				errorsList.Add("rroRmaReceiptID [" + rMAReceiptComponent.rroRmaReceiptID + "] not found.");
			}
			if (rMAReceiptComponent.rroRmaReceiptLineID > 0 && !(await base.ERPRMAReceiptComponentRepository.DoesRecordExistInTableUsingKeys("RMAReceiptLines", new object[2] { "RRLRMARECEIPTID", "RRLRMARECEIPTLINEID" }, new object[2] { rMAReceiptComponent.rroRmaReceiptID, rMAReceiptComponent.rroRmaReceiptLineID })))
			{
				errorsList.Add($"rroRmaReceiptLineID [{rMAReceiptComponent.rroRmaReceiptLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceiptComponent.rroPartID) && !(await base.ERPRMAReceiptComponentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { rMAReceiptComponent.rroPartID })))
			{
				errorsList.Add("rroPartID [" + rMAReceiptComponent.rroPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceiptComponent.rroPartRevisionID) && !(await base.ERPRMAReceiptComponentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { rMAReceiptComponent.rroPartID, rMAReceiptComponent.rroPartRevisionID })))
			{
				errorsList.Add("rroPartRevisionID [" + rMAReceiptComponent.rroPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceiptComponent.rroPartWarehouseLocationID) && !(await base.ERPRMAReceiptComponentRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { rMAReceiptComponent.rroPartID, rMAReceiptComponent.rroPartRevisionID, rMAReceiptComponent.rroPartWarehouseLocationID })))
			{
				errorsList.Add("rroPartWarehouseLocationID [" + rMAReceiptComponent.rroPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceiptComponent.rroPartBinID) && !(await base.ERPRMAReceiptComponentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { rMAReceiptComponent.rroPartID, rMAReceiptComponent.rroPartRevisionID, rMAReceiptComponent.rroPartWarehouseLocationID, rMAReceiptComponent.rroPartBinID })))
			{
				errorsList.Add("rroPartBinID [" + rMAReceiptComponent.rroPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceiptComponent.rroRmaClaimID) && !(await base.ERPRMAReceiptComponentRepository.DoesRecordExistInTableUsingKeys("RMAClaims", new object[1] { "RAPRMACLAIMID" }, new object[1] { rMAReceiptComponent.rroRmaClaimID })))
			{
				errorsList.Add("rroRmaClaimID [" + rMAReceiptComponent.rroRmaClaimID + "] not found.");
			}
			if (rMAReceiptComponent.rroRmaClaimLineID > 0 && !(await base.ERPRMAReceiptComponentRepository.DoesRecordExistInTableUsingKeys("RMAClaimLines", new object[2] { "RALRMACLAIMID", "RALRMACLAIMLINEID" }, new object[2] { rMAReceiptComponent.rroRmaClaimID, rMAReceiptComponent.rroRmaClaimLineID })))
			{
				errorsList.Add($"rroRmaClaimLineID [{rMAReceiptComponent.rroRmaClaimLineID}] not found.");
			}
			if (rMAReceiptComponent.rroRmaClaimComponentID > 0 && !(await base.ERPRMAReceiptComponentRepository.DoesRecordExistInTableUsingKeys("RMAClaimComponents", new object[3] { "raoRMAClaimID", "raoRMAClaimLineID", "raoRMAClaimComponentID" }, new object[3] { rMAReceiptComponent.rroRmaClaimID, rMAReceiptComponent.rroRmaClaimLineID, rMAReceiptComponent.rroRmaClaimComponentID })))
			{
				errorsList.Add($"rroRmaClaimComponentID [{rMAReceiptComponent.rroRmaClaimComponentID}] not found.");
			}
			if (rMAReceiptComponent.rroReverseRmaReceiptLineID > 0 && !(await base.ERPRMAReceiptComponentRepository.DoesRecordExistInTableUsingKeys("RMAReceiptLines", new object[2] { "RRLRMARECEIPTID", "RRLRMARECEIPTLINEID" }, new object[2] { rMAReceiptComponent.rroReverseRmaReceiptID, rMAReceiptComponent.rroReverseRmaReceiptLineID })))
			{
				errorsList.Add($"rroReverseRmaReceiptLineID [{rMAReceiptComponent.rroReverseRmaReceiptLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceiptComponent.rroReverseRmaReceiptID) && !(await base.ERPRMAReceiptComponentRepository.DoesRecordExistInTableUsingKeys("RMAReceipts", new object[1] { "RRPRMARECEIPTID" }, new object[1] { rMAReceiptComponent.rroReverseRmaReceiptID })))
			{
				errorsList.Add("rroReverseRmaReceiptID [" + rMAReceiptComponent.rroReverseRmaReceiptID + "] not found.");
			}
			if (rMAReceiptComponent.rroReverseRmaReceiptCompID > 0 && !(await base.ERPRMAReceiptComponentRepository.DoesRecordExistInTableUsingKeys("RMAReceiptComponents", new object[3] { "rroRMAReceiptID", "rroRMAReceiptLineID", "rroRMAReceiptComponentID" }, new object[3] { rMAReceiptComponent.rroReverseRmaReceiptID, rMAReceiptComponent.rroReverseRmaReceiptLineID, rMAReceiptComponent.rroReverseRmaReceiptCompID })))
			{
				errorsList.Add($"rroReverseRmaReceiptCompID [{rMAReceiptComponent.rroReverseRmaReceiptCompID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPRMAReceiptComponentDto>>> Process_GetAllRMAReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPRMAReceiptComponentDto> allRMAReceiptComponentsDto = new List<ERPRMAReceiptComponentDto>();
		ERPResponseMessageDto<IList<ERPRMAReceiptComponentDto>> result;
		try
		{
			IERPRMAReceiptComponentRepository iERPRMAReceiptComponentRepository = (base.ERPRMAReceiptComponentRepository = new ERPRMAReceiptComponentRepository(base.ApiClientContext));
			using (iERPRMAReceiptComponentRepository)
			{
				foreach (ERPRMAReceiptComponentInformationDto item2 in await base.ERPRMAReceiptComponentRepository.GetAllRMAReceiptComponents(pageSize, pageNumber, filter, orderBy))
				{
					ERPRMAReceiptComponentDto item = new ERPRMAReceiptComponentDto
					{
						rroAdditionalQuantity = item2.rroAdditionalQuantity,
						rroCreatedBy = item2.rroCreatedBy,
						rroCreatedDate = item2.rroCreatedDate,
						rroDescription = item2.rroDescription,
						rroUniqueID = item2.rroUniqueID,
						rroExtendedCost = item2.rroExtendedCost,
						rroExtendedCostForeign = item2.rroExtendedCostForeign,
						rroInspParentQuantity = item2.rroInspParentQuantity,
						rroClosed = item2.rroClosed,
						rroInspectionComplete = item2.rroInspectionComplete,
						rroPosted = item2.rroPosted,
						rroReceivedComplete = item2.rroReceivedComplete,
						rroReversed = item2.rroReversed,
						rroParentQuantity = item2.rroParentQuantity,
						rroPartBinID = item2.rroPartBinID,
						rroPartID = item2.rroPartID,
						rroPartRevisionID = item2.rroPartRevisionID,
						rroPartWarehouseLocationID = item2.rroPartWarehouseLocationID,
						rroQuantityPerParent = item2.rroQuantityPerParent,
						rroQuantityReceived = item2.rroQuantityReceived,
						rroQuantityToInspect = item2.rroQuantityToInspect,
						rroReverseRmaReceiptCompID = item2.rroReverseRmaReceiptCompID,
						rroReverseRmaReceiptID = item2.rroReverseRmaReceiptID,
						rroReverseRmaReceiptLineID = item2.rroReverseRmaReceiptLineID,
						rroRmaClaimComponentID = item2.rroRmaClaimComponentID,
						rroRmaClaimID = item2.rroRmaClaimID,
						rroRmaClaimLineID = item2.rroRmaClaimLineID,
						rroRmaReceiptID = item2.rroRmaReceiptID,
						rroRmaReceiptLineID = item2.rroRmaReceiptLineID,
						rroRowVersion = item2.rroRowVersion,
						rroRmaReceiptComponentID = item2.rroRmaReceiptComponentID,
						rroUnitCost = item2.rroUnitCost,
						rroUnitCostForeign = item2.rroUnitCostForeign,
						rroUnitOfMeasure = item2.rroUnitOfMeasure,
						rroWeight = item2.rroWeight,
						CustomFields = item2.CustomFields
					};
					allRMAReceiptComponentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all RMAReceiptComponents]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPRMAReceiptComponentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allRMAReceiptComponentsDto,
				RecordCount = allRMAReceiptComponentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRMAReceiptComponentDto>> Process_GetRMAReceiptComponent(Guid rMAReceiptComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPRMAReceiptComponentDto rMAReceiptComponentDto = null;
		ERPResponseMessageDto<ERPRMAReceiptComponentDto> result;
		try
		{
			IERPRMAReceiptComponentRepository iERPRMAReceiptComponentRepository = (base.ERPRMAReceiptComponentRepository = new ERPRMAReceiptComponentRepository(base.ApiClientContext));
			using (iERPRMAReceiptComponentRepository)
			{
				ERPRMAReceiptComponentInformationDto eRPRMAReceiptComponentInformationDto = await base.ERPRMAReceiptComponentRepository.GetRMAReceiptComponent(rMAReceiptComponentId);
				rMAReceiptComponentDto = new ERPRMAReceiptComponentDto
				{
					rroAdditionalQuantity = eRPRMAReceiptComponentInformationDto.rroAdditionalQuantity,
					rroCreatedBy = eRPRMAReceiptComponentInformationDto.rroCreatedBy,
					rroCreatedDate = eRPRMAReceiptComponentInformationDto.rroCreatedDate,
					rroDescription = eRPRMAReceiptComponentInformationDto.rroDescription,
					rroUniqueID = eRPRMAReceiptComponentInformationDto.rroUniqueID,
					rroExtendedCost = eRPRMAReceiptComponentInformationDto.rroExtendedCost,
					rroExtendedCostForeign = eRPRMAReceiptComponentInformationDto.rroExtendedCostForeign,
					rroInspParentQuantity = eRPRMAReceiptComponentInformationDto.rroInspParentQuantity,
					rroClosed = eRPRMAReceiptComponentInformationDto.rroClosed,
					rroInspectionComplete = eRPRMAReceiptComponentInformationDto.rroInspectionComplete,
					rroPosted = eRPRMAReceiptComponentInformationDto.rroPosted,
					rroReceivedComplete = eRPRMAReceiptComponentInformationDto.rroReceivedComplete,
					rroReversed = eRPRMAReceiptComponentInformationDto.rroReversed,
					rroParentQuantity = eRPRMAReceiptComponentInformationDto.rroParentQuantity,
					rroPartBinID = eRPRMAReceiptComponentInformationDto.rroPartBinID,
					rroPartID = eRPRMAReceiptComponentInformationDto.rroPartID,
					rroPartRevisionID = eRPRMAReceiptComponentInformationDto.rroPartRevisionID,
					rroPartWarehouseLocationID = eRPRMAReceiptComponentInformationDto.rroPartWarehouseLocationID,
					rroQuantityPerParent = eRPRMAReceiptComponentInformationDto.rroQuantityPerParent,
					rroQuantityReceived = eRPRMAReceiptComponentInformationDto.rroQuantityReceived,
					rroQuantityToInspect = eRPRMAReceiptComponentInformationDto.rroQuantityToInspect,
					rroReverseRmaReceiptCompID = eRPRMAReceiptComponentInformationDto.rroReverseRmaReceiptCompID,
					rroReverseRmaReceiptID = eRPRMAReceiptComponentInformationDto.rroReverseRmaReceiptID,
					rroReverseRmaReceiptLineID = eRPRMAReceiptComponentInformationDto.rroReverseRmaReceiptLineID,
					rroRmaClaimComponentID = eRPRMAReceiptComponentInformationDto.rroRmaClaimComponentID,
					rroRmaClaimID = eRPRMAReceiptComponentInformationDto.rroRmaClaimID,
					rroRmaClaimLineID = eRPRMAReceiptComponentInformationDto.rroRmaClaimLineID,
					rroRmaReceiptID = eRPRMAReceiptComponentInformationDto.rroRmaReceiptID,
					rroRmaReceiptLineID = eRPRMAReceiptComponentInformationDto.rroRmaReceiptLineID,
					rroRowVersion = eRPRMAReceiptComponentInformationDto.rroRowVersion,
					rroRmaReceiptComponentID = eRPRMAReceiptComponentInformationDto.rroRmaReceiptComponentID,
					rroUnitCost = eRPRMAReceiptComponentInformationDto.rroUnitCost,
					rroUnitCostForeign = eRPRMAReceiptComponentInformationDto.rroUnitCostForeign,
					rroUnitOfMeasure = eRPRMAReceiptComponentInformationDto.rroUnitOfMeasure,
					rroWeight = eRPRMAReceiptComponentInformationDto.rroWeight,
					CustomFields = eRPRMAReceiptComponentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the RMAReceiptComponents []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAReceiptComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = rMAReceiptComponentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRMAReceiptComponentDto>> Process_PutRMAReceiptComponent(ERPRMAReceiptComponentDto rMAReceiptComponent)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPRMAReceiptComponentDto createdObject = null;
		ERPResponseMessageDto<ERPRMAReceiptComponentDto> result;
		try
		{
			IERPRMAReceiptComponentRepository iERPRMAReceiptComponentRepository = (base.ERPRMAReceiptComponentRepository = new ERPRMAReceiptComponentRepository(base.ApiClientContext));
			using (iERPRMAReceiptComponentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPRMAReceiptComponentRepository.SaveRMAReceiptComponent(rMAReceiptComponent);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPRMAReceiptComponentInformationDto eRPRMAReceiptComponentInformationDto = await base.ERPRMAReceiptComponentRepository.GetRMAReceiptComponent(rMAReceiptComponent.rroUniqueID);
					createdObject = new ERPRMAReceiptComponentDto
					{
						rroAdditionalQuantity = eRPRMAReceiptComponentInformationDto.rroAdditionalQuantity,
						rroCreatedBy = eRPRMAReceiptComponentInformationDto.rroCreatedBy,
						rroCreatedDate = eRPRMAReceiptComponentInformationDto.rroCreatedDate,
						rroDescription = eRPRMAReceiptComponentInformationDto.rroDescription,
						rroUniqueID = eRPRMAReceiptComponentInformationDto.rroUniqueID,
						rroExtendedCost = eRPRMAReceiptComponentInformationDto.rroExtendedCost,
						rroExtendedCostForeign = eRPRMAReceiptComponentInformationDto.rroExtendedCostForeign,
						rroInspParentQuantity = eRPRMAReceiptComponentInformationDto.rroInspParentQuantity,
						rroClosed = eRPRMAReceiptComponentInformationDto.rroClosed,
						rroInspectionComplete = eRPRMAReceiptComponentInformationDto.rroInspectionComplete,
						rroPosted = eRPRMAReceiptComponentInformationDto.rroPosted,
						rroReceivedComplete = eRPRMAReceiptComponentInformationDto.rroReceivedComplete,
						rroReversed = eRPRMAReceiptComponentInformationDto.rroReversed,
						rroParentQuantity = eRPRMAReceiptComponentInformationDto.rroParentQuantity,
						rroPartBinID = eRPRMAReceiptComponentInformationDto.rroPartBinID,
						rroPartID = eRPRMAReceiptComponentInformationDto.rroPartID,
						rroPartRevisionID = eRPRMAReceiptComponentInformationDto.rroPartRevisionID,
						rroPartWarehouseLocationID = eRPRMAReceiptComponentInformationDto.rroPartWarehouseLocationID,
						rroQuantityPerParent = eRPRMAReceiptComponentInformationDto.rroQuantityPerParent,
						rroQuantityReceived = eRPRMAReceiptComponentInformationDto.rroQuantityReceived,
						rroQuantityToInspect = eRPRMAReceiptComponentInformationDto.rroQuantityToInspect,
						rroReverseRmaReceiptCompID = eRPRMAReceiptComponentInformationDto.rroReverseRmaReceiptCompID,
						rroReverseRmaReceiptID = eRPRMAReceiptComponentInformationDto.rroReverseRmaReceiptID,
						rroReverseRmaReceiptLineID = eRPRMAReceiptComponentInformationDto.rroReverseRmaReceiptLineID,
						rroRmaClaimComponentID = eRPRMAReceiptComponentInformationDto.rroRmaClaimComponentID,
						rroRmaClaimID = eRPRMAReceiptComponentInformationDto.rroRmaClaimID,
						rroRmaClaimLineID = eRPRMAReceiptComponentInformationDto.rroRmaClaimLineID,
						rroRmaReceiptID = eRPRMAReceiptComponentInformationDto.rroRmaReceiptID,
						rroRmaReceiptLineID = eRPRMAReceiptComponentInformationDto.rroRmaReceiptLineID,
						rroRowVersion = eRPRMAReceiptComponentInformationDto.rroRowVersion,
						rroRmaReceiptComponentID = eRPRMAReceiptComponentInformationDto.rroRmaReceiptComponentID,
						rroUnitCost = eRPRMAReceiptComponentInformationDto.rroUnitCost,
						rroUnitCostForeign = eRPRMAReceiptComponentInformationDto.rroUnitCostForeign,
						rroUnitOfMeasure = eRPRMAReceiptComponentInformationDto.rroUnitOfMeasure,
						rroWeight = eRPRMAReceiptComponentInformationDto.rroWeight,
						CustomFields = eRPRMAReceiptComponentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing RMAReceiptComponent [{rMAReceiptComponent.rroUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAReceiptComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteRMAReceiptComponent(Guid rMAReceiptComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAReceiptComponentRepository iERPRMAReceiptComponentRepository = (base.ERPRMAReceiptComponentRepository = new ERPRMAReceiptComponentRepository(base.ApiClientContext));
		using (iERPRMAReceiptComponentRepository)
		{
			if (!(await base.ERPRMAReceiptComponentRepository.DoesRMAReceiptComponentExist(rMAReceiptComponentId)))
			{
				base.ErrorsList.Add($"RMAReceiptComponent [{rMAReceiptComponentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPRMAReceiptComponentInformationDto eRPRMAReceiptComponentInformationDto = await base.ERPRMAReceiptComponentRepository.GetRMAReceiptComponent(rMAReceiptComponentId);
				string text = await base.ERPRMAReceiptComponentRepository.WhereUsed("RMAReceiptComponents", new object[3] { eRPRMAReceiptComponentInformationDto.rroRmaReceiptID, eRPRMAReceiptComponentInformationDto.rroRmaReceiptLineID, eRPRMAReceiptComponentInformationDto.rroRmaReceiptComponentID }, new object[3] { "rroRmaReceiptID", "rroRmaReceiptLineID", "rroRmaReceiptComponentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("RMAReceiptComponent cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPRMAReceiptComponentDto>> Process_DeleteRMAReceiptComponent(Guid rMAReceiptComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPRMAReceiptComponentDto> result;
		try
		{
			IERPRMAReceiptComponentRepository iERPRMAReceiptComponentRepository = (base.ERPRMAReceiptComponentRepository = new ERPRMAReceiptComponentRepository(base.ApiClientContext));
			using (iERPRMAReceiptComponentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPRMAReceiptComponentRepository.DeleteRowFromTable("RMAReceiptComponents", "rro", rMAReceiptComponentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of RMAReceiptComponent [{rMAReceiptComponentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAReceiptComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPRMAReceiptComponentDto()
			};
		}
		return result;
	}
}
