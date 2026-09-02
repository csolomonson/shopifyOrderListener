using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWarehouseTransferComponentModel : ERPBaseModel, IERPWarehouseTransferComponentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseTransferComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWarehouseTransferComponentRepository iERPWarehouseTransferComponentRepository = (base.ERPWarehouseTransferComponentRepository = new ERPWarehouseTransferComponentRepository(base.ApiClientContext));
		using (iERPWarehouseTransferComponentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWarehouseTransferComponentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWarehouseTransferComponentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWarehouseTransferComponentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWarehouseTransferComponentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWarehouseTransferComponent(Guid warehouseTransferComponentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseTransferComponentRepository iERPWarehouseTransferComponentRepository = (base.ERPWarehouseTransferComponentRepository = new ERPWarehouseTransferComponentRepository(base.ApiClientContext));
		using (iERPWarehouseTransferComponentRepository)
		{
			if (!(await base.ERPWarehouseTransferComponentRepository.DoesWarehouseTransferComponentExist(warehouseTransferComponentId)))
			{
				errorsList.Add($"WarehouseTransferComponent [{warehouseTransferComponentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutWarehouseTransferComponent(ERPWarehouseTransferComponentDto warehouseTransferComponent)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseTransferComponentRepository iERPWarehouseTransferComponentRepository = (base.ERPWarehouseTransferComponentRepository = new ERPWarehouseTransferComponentRepository(base.ApiClientContext));
		using (iERPWarehouseTransferComponentRepository)
		{
			if (!string.IsNullOrWhiteSpace(warehouseTransferComponent.mwoWarehouseTransferID) && !(await base.ERPWarehouseTransferComponentRepository.DoesRecordExistInTableUsingKeys("WarehouseTransfers", new object[1] { "MWPWAREHOUSETRANSFERID" }, new object[1] { warehouseTransferComponent.mwoWarehouseTransferID })))
			{
				errorsList.Add("mwoWarehouseTransferID [" + warehouseTransferComponent.mwoWarehouseTransferID + "] not found.");
			}
			if (warehouseTransferComponent.mwoWarehouseTransferLineID > 0 && !(await base.ERPWarehouseTransferComponentRepository.DoesRecordExistInTableUsingKeys("WarehouseTransferLines", new object[2] { "MWLWAREHOUSETRANSFERID", "MWLWAREHOUSETRANSFERLINEID" }, new object[2] { warehouseTransferComponent.mwoWarehouseTransferID, warehouseTransferComponent.mwoWarehouseTransferLineID })))
			{
				errorsList.Add($"mwoWarehouseTransferLineID [{warehouseTransferComponent.mwoWarehouseTransferLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseTransferComponent.mwoPartID) && !(await base.ERPWarehouseTransferComponentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { warehouseTransferComponent.mwoPartID })))
			{
				errorsList.Add("mwoPartID [" + warehouseTransferComponent.mwoPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseTransferComponent.mwoPartRevisionID) && !(await base.ERPWarehouseTransferComponentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { warehouseTransferComponent.mwoPartID, warehouseTransferComponent.mwoPartRevisionID })))
			{
				errorsList.Add("mwoPartRevisionID [" + warehouseTransferComponent.mwoPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseTransferComponent.mwoSourcePartBinID) && !(await base.ERPWarehouseTransferComponentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { warehouseTransferComponent.mwoPartID, warehouseTransferComponent.mwoPartRevisionID, warehouseTransferComponent.mwoSourceWarehouseID, warehouseTransferComponent.mwoSourcePartBinID })))
			{
				errorsList.Add("mwoSourcePartBinID [" + warehouseTransferComponent.mwoSourcePartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseTransferComponent.mwoWarehouseRequisitionID) && !(await base.ERPWarehouseTransferComponentRepository.DoesRecordExistInTableUsingKeys("WarehouseRequisitions", new object[1] { "WQPWAREHOUSEREQUISITIONID" }, new object[1] { warehouseTransferComponent.mwoWarehouseRequisitionID })))
			{
				errorsList.Add("mwoWarehouseRequisitionID [" + warehouseTransferComponent.mwoWarehouseRequisitionID + "] not found.");
			}
			if (warehouseTransferComponent.mwoWarehouseRequisitionLineID > 0 && !(await base.ERPWarehouseTransferComponentRepository.DoesRecordExistInTableUsingKeys("WarehouseRequisitionLines", new object[2] { "WQLWAREHOUSEREQUISITIONID", "WQLWAREHOUSEREQUISITIONLINEID" }, new object[2] { warehouseTransferComponent.mwoWarehouseRequisitionID, warehouseTransferComponent.mwoWarehouseRequisitionLineID })))
			{
				errorsList.Add($"mwoWarehouseRequisitionLineID [{warehouseTransferComponent.mwoWarehouseRequisitionLineID}] not found.");
			}
			if (warehouseTransferComponent.mwoWarehouseReqComponentID > 0 && !(await base.ERPWarehouseTransferComponentRepository.DoesRecordExistInTableUsingKeys("WarehouseRequisitionComponents", new object[3] { "WQOWAREHOUSEREQUISITIONID", "WQOWAREHOUSEREQUISITIONLINEID", "WQOWAREHOUSEREQCOMPONENTID" }, new object[3] { warehouseTransferComponent.mwoWarehouseRequisitionID, warehouseTransferComponent.mwoWarehouseRequisitionLineID, warehouseTransferComponent.mwoWarehouseReqComponentID })))
			{
				errorsList.Add($"mwoWarehouseReqComponentID [{warehouseTransferComponent.mwoWarehouseReqComponentID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWarehouseTransferComponentDto>>> Process_GetAllWarehouseTransferComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWarehouseTransferComponentDto> allWarehouseTransferComponentsDto = new List<ERPWarehouseTransferComponentDto>();
		ERPResponseMessageDto<IList<ERPWarehouseTransferComponentDto>> result;
		try
		{
			IERPWarehouseTransferComponentRepository iERPWarehouseTransferComponentRepository = (base.ERPWarehouseTransferComponentRepository = new ERPWarehouseTransferComponentRepository(base.ApiClientContext));
			using (iERPWarehouseTransferComponentRepository)
			{
				foreach (ERPWarehouseTransferComponentInformationDto item2 in await base.ERPWarehouseTransferComponentRepository.GetAllWarehouseTransferComponents(pageSize, pageNumber, filter, orderBy))
				{
					ERPWarehouseTransferComponentDto item = new ERPWarehouseTransferComponentDto
					{
						mwoAdditionalQuantity = item2.mwoAdditionalQuantity,
						mwoCreatedBy = item2.mwoCreatedBy,
						mwoCreatedDate = item2.mwoCreatedDate,
						mwoDescription = item2.mwoDescription,
						mwoDestinationWarehouseID = item2.mwoDestinationWarehouseID,
						mwoUniqueID = item2.mwoUniqueID,
						mwoClosed = item2.mwoClosed,
						mwoPosted = item2.mwoPosted,
						mwoReceivedComplete = item2.mwoReceivedComplete,
						mwoReversed = item2.mwoReversed,
						mwoShippedComplete = item2.mwoShippedComplete,
						mwoParentQuantity = item2.mwoParentQuantity,
						mwoPartID = item2.mwoPartID,
						mwoPartRevisionID = item2.mwoPartRevisionID,
						mwoQuantityInTransit = item2.mwoQuantityInTransit,
						mwoQuantityPerParent = item2.mwoQuantityPerParent,
						mwoReceivedQuantity = item2.mwoReceivedQuantity,
						mwoReverseWHTransComponentID = item2.mwoReverseWHTransComponentID,
						mwoReverseWHTransferID = item2.mwoReverseWHTransferID,
						mwoReverseWHTransferLineID = item2.mwoReverseWHTransferLineID,
						mwoRowVersion = item2.mwoRowVersion,
						mwoShipQuantity = item2.mwoShipQuantity,
						mwoSourcePartBinID = item2.mwoSourcePartBinID,
						mwoSourceWarehouseID = item2.mwoSourceWarehouseID,
						mwoUnitOfMeasure = item2.mwoUnitOfMeasure,
						mwoWarehouseReqComponentID = item2.mwoWarehouseReqComponentID,
						mwoWarehouseRequisitionID = item2.mwoWarehouseRequisitionID,
						mwoWarehouseRequisitionLineID = item2.mwoWarehouseRequisitionLineID,
						mwoWarehouseTransComponentID = item2.mwoWarehouseTransComponentID,
						mwoWarehouseTransferID = item2.mwoWarehouseTransferID,
						mwoWarehouseTransferLineID = item2.mwoWarehouseTransferLineID,
						mwoWeight = item2.mwoWeight,
						CustomFields = item2.CustomFields
					};
					allWarehouseTransferComponentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all WarehouseTransferComponents]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWarehouseTransferComponentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWarehouseTransferComponentsDto,
				RecordCount = allWarehouseTransferComponentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseTransferComponentDto>> Process_GetWarehouseTransferComponent(Guid warehouseTransferComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWarehouseTransferComponentDto warehouseTransferComponentDto = null;
		ERPResponseMessageDto<ERPWarehouseTransferComponentDto> result;
		try
		{
			IERPWarehouseTransferComponentRepository iERPWarehouseTransferComponentRepository = (base.ERPWarehouseTransferComponentRepository = new ERPWarehouseTransferComponentRepository(base.ApiClientContext));
			using (iERPWarehouseTransferComponentRepository)
			{
				ERPWarehouseTransferComponentInformationDto eRPWarehouseTransferComponentInformationDto = await base.ERPWarehouseTransferComponentRepository.GetWarehouseTransferComponent(warehouseTransferComponentId);
				warehouseTransferComponentDto = new ERPWarehouseTransferComponentDto
				{
					mwoAdditionalQuantity = eRPWarehouseTransferComponentInformationDto.mwoAdditionalQuantity,
					mwoCreatedBy = eRPWarehouseTransferComponentInformationDto.mwoCreatedBy,
					mwoCreatedDate = eRPWarehouseTransferComponentInformationDto.mwoCreatedDate,
					mwoDescription = eRPWarehouseTransferComponentInformationDto.mwoDescription,
					mwoDestinationWarehouseID = eRPWarehouseTransferComponentInformationDto.mwoDestinationWarehouseID,
					mwoUniqueID = eRPWarehouseTransferComponentInformationDto.mwoUniqueID,
					mwoClosed = eRPWarehouseTransferComponentInformationDto.mwoClosed,
					mwoPosted = eRPWarehouseTransferComponentInformationDto.mwoPosted,
					mwoReceivedComplete = eRPWarehouseTransferComponentInformationDto.mwoReceivedComplete,
					mwoReversed = eRPWarehouseTransferComponentInformationDto.mwoReversed,
					mwoShippedComplete = eRPWarehouseTransferComponentInformationDto.mwoShippedComplete,
					mwoParentQuantity = eRPWarehouseTransferComponentInformationDto.mwoParentQuantity,
					mwoPartID = eRPWarehouseTransferComponentInformationDto.mwoPartID,
					mwoPartRevisionID = eRPWarehouseTransferComponentInformationDto.mwoPartRevisionID,
					mwoQuantityInTransit = eRPWarehouseTransferComponentInformationDto.mwoQuantityInTransit,
					mwoQuantityPerParent = eRPWarehouseTransferComponentInformationDto.mwoQuantityPerParent,
					mwoReceivedQuantity = eRPWarehouseTransferComponentInformationDto.mwoReceivedQuantity,
					mwoReverseWHTransComponentID = eRPWarehouseTransferComponentInformationDto.mwoReverseWHTransComponentID,
					mwoReverseWHTransferID = eRPWarehouseTransferComponentInformationDto.mwoReverseWHTransferID,
					mwoReverseWHTransferLineID = eRPWarehouseTransferComponentInformationDto.mwoReverseWHTransferLineID,
					mwoRowVersion = eRPWarehouseTransferComponentInformationDto.mwoRowVersion,
					mwoShipQuantity = eRPWarehouseTransferComponentInformationDto.mwoShipQuantity,
					mwoSourcePartBinID = eRPWarehouseTransferComponentInformationDto.mwoSourcePartBinID,
					mwoSourceWarehouseID = eRPWarehouseTransferComponentInformationDto.mwoSourceWarehouseID,
					mwoUnitOfMeasure = eRPWarehouseTransferComponentInformationDto.mwoUnitOfMeasure,
					mwoWarehouseReqComponentID = eRPWarehouseTransferComponentInformationDto.mwoWarehouseReqComponentID,
					mwoWarehouseRequisitionID = eRPWarehouseTransferComponentInformationDto.mwoWarehouseRequisitionID,
					mwoWarehouseRequisitionLineID = eRPWarehouseTransferComponentInformationDto.mwoWarehouseRequisitionLineID,
					mwoWarehouseTransComponentID = eRPWarehouseTransferComponentInformationDto.mwoWarehouseTransComponentID,
					mwoWarehouseTransferID = eRPWarehouseTransferComponentInformationDto.mwoWarehouseTransferID,
					mwoWarehouseTransferLineID = eRPWarehouseTransferComponentInformationDto.mwoWarehouseTransferLineID,
					mwoWeight = eRPWarehouseTransferComponentInformationDto.mwoWeight,
					CustomFields = eRPWarehouseTransferComponentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the WarehouseTransferComponents []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseTransferComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = warehouseTransferComponentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseTransferComponentDto>> Process_PutWarehouseTransferComponent(ERPWarehouseTransferComponentDto warehouseTransferComponent)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPWarehouseTransferComponentDto createdObject = null;
		ERPResponseMessageDto<ERPWarehouseTransferComponentDto> result;
		try
		{
			IERPWarehouseTransferComponentRepository iERPWarehouseTransferComponentRepository = (base.ERPWarehouseTransferComponentRepository = new ERPWarehouseTransferComponentRepository(base.ApiClientContext));
			using (iERPWarehouseTransferComponentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPWarehouseTransferComponentRepository.SaveWarehouseTransferComponent(warehouseTransferComponent);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPWarehouseTransferComponentInformationDto eRPWarehouseTransferComponentInformationDto = await base.ERPWarehouseTransferComponentRepository.GetWarehouseTransferComponent(warehouseTransferComponent.mwoUniqueID);
					createdObject = new ERPWarehouseTransferComponentDto
					{
						mwoAdditionalQuantity = eRPWarehouseTransferComponentInformationDto.mwoAdditionalQuantity,
						mwoCreatedBy = eRPWarehouseTransferComponentInformationDto.mwoCreatedBy,
						mwoCreatedDate = eRPWarehouseTransferComponentInformationDto.mwoCreatedDate,
						mwoDescription = eRPWarehouseTransferComponentInformationDto.mwoDescription,
						mwoDestinationWarehouseID = eRPWarehouseTransferComponentInformationDto.mwoDestinationWarehouseID,
						mwoUniqueID = eRPWarehouseTransferComponentInformationDto.mwoUniqueID,
						mwoClosed = eRPWarehouseTransferComponentInformationDto.mwoClosed,
						mwoPosted = eRPWarehouseTransferComponentInformationDto.mwoPosted,
						mwoReceivedComplete = eRPWarehouseTransferComponentInformationDto.mwoReceivedComplete,
						mwoReversed = eRPWarehouseTransferComponentInformationDto.mwoReversed,
						mwoShippedComplete = eRPWarehouseTransferComponentInformationDto.mwoShippedComplete,
						mwoParentQuantity = eRPWarehouseTransferComponentInformationDto.mwoParentQuantity,
						mwoPartID = eRPWarehouseTransferComponentInformationDto.mwoPartID,
						mwoPartRevisionID = eRPWarehouseTransferComponentInformationDto.mwoPartRevisionID,
						mwoQuantityInTransit = eRPWarehouseTransferComponentInformationDto.mwoQuantityInTransit,
						mwoQuantityPerParent = eRPWarehouseTransferComponentInformationDto.mwoQuantityPerParent,
						mwoReceivedQuantity = eRPWarehouseTransferComponentInformationDto.mwoReceivedQuantity,
						mwoReverseWHTransComponentID = eRPWarehouseTransferComponentInformationDto.mwoReverseWHTransComponentID,
						mwoReverseWHTransferID = eRPWarehouseTransferComponentInformationDto.mwoReverseWHTransferID,
						mwoReverseWHTransferLineID = eRPWarehouseTransferComponentInformationDto.mwoReverseWHTransferLineID,
						mwoRowVersion = eRPWarehouseTransferComponentInformationDto.mwoRowVersion,
						mwoShipQuantity = eRPWarehouseTransferComponentInformationDto.mwoShipQuantity,
						mwoSourcePartBinID = eRPWarehouseTransferComponentInformationDto.mwoSourcePartBinID,
						mwoSourceWarehouseID = eRPWarehouseTransferComponentInformationDto.mwoSourceWarehouseID,
						mwoUnitOfMeasure = eRPWarehouseTransferComponentInformationDto.mwoUnitOfMeasure,
						mwoWarehouseReqComponentID = eRPWarehouseTransferComponentInformationDto.mwoWarehouseReqComponentID,
						mwoWarehouseRequisitionID = eRPWarehouseTransferComponentInformationDto.mwoWarehouseRequisitionID,
						mwoWarehouseRequisitionLineID = eRPWarehouseTransferComponentInformationDto.mwoWarehouseRequisitionLineID,
						mwoWarehouseTransComponentID = eRPWarehouseTransferComponentInformationDto.mwoWarehouseTransComponentID,
						mwoWarehouseTransferID = eRPWarehouseTransferComponentInformationDto.mwoWarehouseTransferID,
						mwoWarehouseTransferLineID = eRPWarehouseTransferComponentInformationDto.mwoWarehouseTransferLineID,
						mwoWeight = eRPWarehouseTransferComponentInformationDto.mwoWeight,
						CustomFields = eRPWarehouseTransferComponentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing WarehouseTransferComponent [{warehouseTransferComponent.mwoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseTransferComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseTransferComponent(Guid warehouseTransferComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseTransferComponentRepository iERPWarehouseTransferComponentRepository = (base.ERPWarehouseTransferComponentRepository = new ERPWarehouseTransferComponentRepository(base.ApiClientContext));
		using (iERPWarehouseTransferComponentRepository)
		{
			if (!(await base.ERPWarehouseTransferComponentRepository.DoesWarehouseTransferComponentExist(warehouseTransferComponentId)))
			{
				base.ErrorsList.Add($"WarehouseTransferComponent [{warehouseTransferComponentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPWarehouseTransferComponentInformationDto eRPWarehouseTransferComponentInformationDto = await base.ERPWarehouseTransferComponentRepository.GetWarehouseTransferComponent(warehouseTransferComponentId);
				string text = await base.ERPWarehouseTransferComponentRepository.WhereUsed("WarehouseTransferComponents", new object[3] { eRPWarehouseTransferComponentInformationDto.mwoWarehouseTransferID, eRPWarehouseTransferComponentInformationDto.mwoWarehouseTransferLineID, eRPWarehouseTransferComponentInformationDto.mwoWarehouseTransComponentID }, new object[3] { "mwoWarehouseTransferID", "mwoWarehouseTransferLineID", "mwoWarehouseTransComponentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("WarehouseTransferComponent cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseTransferComponentDto>> Process_DeleteWarehouseTransferComponent(Guid warehouseTransferComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPWarehouseTransferComponentDto> result;
		try
		{
			IERPWarehouseTransferComponentRepository iERPWarehouseTransferComponentRepository = (base.ERPWarehouseTransferComponentRepository = new ERPWarehouseTransferComponentRepository(base.ApiClientContext));
			using (iERPWarehouseTransferComponentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPWarehouseTransferComponentRepository.DeleteRowFromTable("WarehouseTransferComponents", "mwo", warehouseTransferComponentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of WarehouseTransferComponent [{warehouseTransferComponentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseTransferComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPWarehouseTransferComponentDto()
			};
		}
		return result;
	}
}
