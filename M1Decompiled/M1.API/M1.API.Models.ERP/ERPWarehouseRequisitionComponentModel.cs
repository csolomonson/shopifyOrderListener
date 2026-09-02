using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWarehouseRequisitionComponentModel : ERPBaseModel, IERPWarehouseRequisitionComponentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseRequisitionComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWarehouseRequisitionComponentRepository iERPWarehouseRequisitionComponentRepository = (base.ERPWarehouseRequisitionComponentRepository = new ERPWarehouseRequisitionComponentRepository(base.ApiClientContext));
		using (iERPWarehouseRequisitionComponentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWarehouseRequisitionComponentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWarehouseRequisitionComponentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWarehouseRequisitionComponentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWarehouseRequisitionComponentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWarehouseRequisitionComponent(Guid warehouseRequisitionComponentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseRequisitionComponentRepository iERPWarehouseRequisitionComponentRepository = (base.ERPWarehouseRequisitionComponentRepository = new ERPWarehouseRequisitionComponentRepository(base.ApiClientContext));
		using (iERPWarehouseRequisitionComponentRepository)
		{
			if (!(await base.ERPWarehouseRequisitionComponentRepository.DoesWarehouseRequisitionComponentExist(warehouseRequisitionComponentId)))
			{
				errorsList.Add($"WarehouseRequisitionComponent [{warehouseRequisitionComponentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutWarehouseRequisitionComponent(ERPWarehouseRequisitionComponentDto warehouseRequisitionComponent)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseRequisitionComponentRepository iERPWarehouseRequisitionComponentRepository = (base.ERPWarehouseRequisitionComponentRepository = new ERPWarehouseRequisitionComponentRepository(base.ApiClientContext));
		using (iERPWarehouseRequisitionComponentRepository)
		{
			if (!string.IsNullOrWhiteSpace(warehouseRequisitionComponent.wqoWarehouseRequisitionID) && !(await base.ERPWarehouseRequisitionComponentRepository.DoesRecordExistInTableUsingKeys("WarehouseRequisitions", new object[1] { "WQPWAREHOUSEREQUISITIONID" }, new object[1] { warehouseRequisitionComponent.wqoWarehouseRequisitionID })))
			{
				errorsList.Add("wqoWarehouseRequisitionID [" + warehouseRequisitionComponent.wqoWarehouseRequisitionID + "] not found.");
			}
			if (warehouseRequisitionComponent.wqoWarehouseRequisitionLineID > 0 && !(await base.ERPWarehouseRequisitionComponentRepository.DoesRecordExistInTableUsingKeys("WarehouseRequisitionLines", new object[2] { "WQLWAREHOUSEREQUISITIONID", "WQLWAREHOUSEREQUISITIONLINEID" }, new object[2] { warehouseRequisitionComponent.wqoWarehouseRequisitionID, warehouseRequisitionComponent.wqoWarehouseRequisitionLineID })))
			{
				errorsList.Add($"wqoWarehouseRequisitionLineID [{warehouseRequisitionComponent.wqoWarehouseRequisitionLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseRequisitionComponent.wqoPartID) && !(await base.ERPWarehouseRequisitionComponentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { warehouseRequisitionComponent.wqoPartID })))
			{
				errorsList.Add("wqoPartID [" + warehouseRequisitionComponent.wqoPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseRequisitionComponent.wqoPartRevisionID) && !(await base.ERPWarehouseRequisitionComponentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { warehouseRequisitionComponent.wqoPartID, warehouseRequisitionComponent.wqoPartRevisionID })))
			{
				errorsList.Add("wqoPartRevisionID [" + warehouseRequisitionComponent.wqoPartRevisionID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWarehouseRequisitionComponentDto>>> Process_GetAllWarehouseRequisitionComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWarehouseRequisitionComponentDto> allWarehouseRequisitionComponentsDto = new List<ERPWarehouseRequisitionComponentDto>();
		ERPResponseMessageDto<IList<ERPWarehouseRequisitionComponentDto>> result;
		try
		{
			IERPWarehouseRequisitionComponentRepository iERPWarehouseRequisitionComponentRepository = (base.ERPWarehouseRequisitionComponentRepository = new ERPWarehouseRequisitionComponentRepository(base.ApiClientContext));
			using (iERPWarehouseRequisitionComponentRepository)
			{
				foreach (ERPWarehouseRequisitionComponentInformationDto item2 in await base.ERPWarehouseRequisitionComponentRepository.GetAllWarehouseRequisitionComponents(pageSize, pageNumber, filter, orderBy))
				{
					ERPWarehouseRequisitionComponentDto item = new ERPWarehouseRequisitionComponentDto
					{
						wqoAdditionalQuantity = item2.wqoAdditionalQuantity,
						wqoCreatedBy = item2.wqoCreatedBy,
						wqoCreatedDate = item2.wqoCreatedDate,
						wqoDescription = item2.wqoDescription,
						wqoUniqueID = item2.wqoUniqueID,
						wqoClosed = item2.wqoClosed,
						wqoTransferredComplete = item2.wqoTransferredComplete,
						wqoParentQuantity = item2.wqoParentQuantity,
						wqoPartID = item2.wqoPartID,
						wqoPartRevisionID = item2.wqoPartRevisionID,
						wqoQuantityPerParent = item2.wqoQuantityPerParent,
						wqoQuantityRequested = item2.wqoQuantityRequested,
						wqoQuantityTransferred = item2.wqoQuantityTransferred,
						wqoRowVersion = item2.wqoRowVersion,
						wqoSourceWarehouseID = item2.wqoSourceWarehouseID,
						wqoUnitOfMeasure = item2.wqoUnitOfMeasure,
						wqoWarehouseReqComponentID = item2.wqoWarehouseReqComponentID,
						wqoWarehouseRequisitionID = item2.wqoWarehouseRequisitionID,
						wqoWarehouseRequisitionLineID = item2.wqoWarehouseRequisitionLineID,
						wqoWeight = item2.wqoWeight,
						CustomFields = item2.CustomFields
					};
					allWarehouseRequisitionComponentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all WarehouseRequisitionComponents]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWarehouseRequisitionComponentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWarehouseRequisitionComponentsDto,
				RecordCount = allWarehouseRequisitionComponentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseRequisitionComponentDto>> Process_GetWarehouseRequisitionComponent(Guid warehouseRequisitionComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWarehouseRequisitionComponentDto warehouseRequisitionComponentDto = null;
		ERPResponseMessageDto<ERPWarehouseRequisitionComponentDto> result;
		try
		{
			IERPWarehouseRequisitionComponentRepository iERPWarehouseRequisitionComponentRepository = (base.ERPWarehouseRequisitionComponentRepository = new ERPWarehouseRequisitionComponentRepository(base.ApiClientContext));
			using (iERPWarehouseRequisitionComponentRepository)
			{
				ERPWarehouseRequisitionComponentInformationDto eRPWarehouseRequisitionComponentInformationDto = await base.ERPWarehouseRequisitionComponentRepository.GetWarehouseRequisitionComponent(warehouseRequisitionComponentId);
				warehouseRequisitionComponentDto = new ERPWarehouseRequisitionComponentDto
				{
					wqoAdditionalQuantity = eRPWarehouseRequisitionComponentInformationDto.wqoAdditionalQuantity,
					wqoCreatedBy = eRPWarehouseRequisitionComponentInformationDto.wqoCreatedBy,
					wqoCreatedDate = eRPWarehouseRequisitionComponentInformationDto.wqoCreatedDate,
					wqoDescription = eRPWarehouseRequisitionComponentInformationDto.wqoDescription,
					wqoUniqueID = eRPWarehouseRequisitionComponentInformationDto.wqoUniqueID,
					wqoClosed = eRPWarehouseRequisitionComponentInformationDto.wqoClosed,
					wqoTransferredComplete = eRPWarehouseRequisitionComponentInformationDto.wqoTransferredComplete,
					wqoParentQuantity = eRPWarehouseRequisitionComponentInformationDto.wqoParentQuantity,
					wqoPartID = eRPWarehouseRequisitionComponentInformationDto.wqoPartID,
					wqoPartRevisionID = eRPWarehouseRequisitionComponentInformationDto.wqoPartRevisionID,
					wqoQuantityPerParent = eRPWarehouseRequisitionComponentInformationDto.wqoQuantityPerParent,
					wqoQuantityRequested = eRPWarehouseRequisitionComponentInformationDto.wqoQuantityRequested,
					wqoQuantityTransferred = eRPWarehouseRequisitionComponentInformationDto.wqoQuantityTransferred,
					wqoRowVersion = eRPWarehouseRequisitionComponentInformationDto.wqoRowVersion,
					wqoSourceWarehouseID = eRPWarehouseRequisitionComponentInformationDto.wqoSourceWarehouseID,
					wqoUnitOfMeasure = eRPWarehouseRequisitionComponentInformationDto.wqoUnitOfMeasure,
					wqoWarehouseReqComponentID = eRPWarehouseRequisitionComponentInformationDto.wqoWarehouseReqComponentID,
					wqoWarehouseRequisitionID = eRPWarehouseRequisitionComponentInformationDto.wqoWarehouseRequisitionID,
					wqoWarehouseRequisitionLineID = eRPWarehouseRequisitionComponentInformationDto.wqoWarehouseRequisitionLineID,
					wqoWeight = eRPWarehouseRequisitionComponentInformationDto.wqoWeight,
					CustomFields = eRPWarehouseRequisitionComponentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the WarehouseRequisitionComponents []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseRequisitionComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = warehouseRequisitionComponentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseRequisitionComponentDto>> Process_PutWarehouseRequisitionComponent(ERPWarehouseRequisitionComponentDto warehouseRequisitionComponent)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPWarehouseRequisitionComponentDto createdObject = null;
		ERPResponseMessageDto<ERPWarehouseRequisitionComponentDto> result;
		try
		{
			IERPWarehouseRequisitionComponentRepository iERPWarehouseRequisitionComponentRepository = (base.ERPWarehouseRequisitionComponentRepository = new ERPWarehouseRequisitionComponentRepository(base.ApiClientContext));
			using (iERPWarehouseRequisitionComponentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPWarehouseRequisitionComponentRepository.SaveWarehouseRequisitionComponent(warehouseRequisitionComponent);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPWarehouseRequisitionComponentInformationDto eRPWarehouseRequisitionComponentInformationDto = await base.ERPWarehouseRequisitionComponentRepository.GetWarehouseRequisitionComponent(warehouseRequisitionComponent.wqoUniqueID);
					createdObject = new ERPWarehouseRequisitionComponentDto
					{
						wqoAdditionalQuantity = eRPWarehouseRequisitionComponentInformationDto.wqoAdditionalQuantity,
						wqoCreatedBy = eRPWarehouseRequisitionComponentInformationDto.wqoCreatedBy,
						wqoCreatedDate = eRPWarehouseRequisitionComponentInformationDto.wqoCreatedDate,
						wqoDescription = eRPWarehouseRequisitionComponentInformationDto.wqoDescription,
						wqoUniqueID = eRPWarehouseRequisitionComponentInformationDto.wqoUniqueID,
						wqoClosed = eRPWarehouseRequisitionComponentInformationDto.wqoClosed,
						wqoTransferredComplete = eRPWarehouseRequisitionComponentInformationDto.wqoTransferredComplete,
						wqoParentQuantity = eRPWarehouseRequisitionComponentInformationDto.wqoParentQuantity,
						wqoPartID = eRPWarehouseRequisitionComponentInformationDto.wqoPartID,
						wqoPartRevisionID = eRPWarehouseRequisitionComponentInformationDto.wqoPartRevisionID,
						wqoQuantityPerParent = eRPWarehouseRequisitionComponentInformationDto.wqoQuantityPerParent,
						wqoQuantityRequested = eRPWarehouseRequisitionComponentInformationDto.wqoQuantityRequested,
						wqoQuantityTransferred = eRPWarehouseRequisitionComponentInformationDto.wqoQuantityTransferred,
						wqoRowVersion = eRPWarehouseRequisitionComponentInformationDto.wqoRowVersion,
						wqoSourceWarehouseID = eRPWarehouseRequisitionComponentInformationDto.wqoSourceWarehouseID,
						wqoUnitOfMeasure = eRPWarehouseRequisitionComponentInformationDto.wqoUnitOfMeasure,
						wqoWarehouseReqComponentID = eRPWarehouseRequisitionComponentInformationDto.wqoWarehouseReqComponentID,
						wqoWarehouseRequisitionID = eRPWarehouseRequisitionComponentInformationDto.wqoWarehouseRequisitionID,
						wqoWarehouseRequisitionLineID = eRPWarehouseRequisitionComponentInformationDto.wqoWarehouseRequisitionLineID,
						wqoWeight = eRPWarehouseRequisitionComponentInformationDto.wqoWeight,
						CustomFields = eRPWarehouseRequisitionComponentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing WarehouseRequisitionComponent [{warehouseRequisitionComponent.wqoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseRequisitionComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseRequisitionComponent(Guid warehouseRequisitionComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseRequisitionComponentRepository iERPWarehouseRequisitionComponentRepository = (base.ERPWarehouseRequisitionComponentRepository = new ERPWarehouseRequisitionComponentRepository(base.ApiClientContext));
		using (iERPWarehouseRequisitionComponentRepository)
		{
			if (!(await base.ERPWarehouseRequisitionComponentRepository.DoesWarehouseRequisitionComponentExist(warehouseRequisitionComponentId)))
			{
				base.ErrorsList.Add($"WarehouseRequisitionComponent [{warehouseRequisitionComponentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPWarehouseRequisitionComponentInformationDto eRPWarehouseRequisitionComponentInformationDto = await base.ERPWarehouseRequisitionComponentRepository.GetWarehouseRequisitionComponent(warehouseRequisitionComponentId);
				string text = await base.ERPWarehouseRequisitionComponentRepository.WhereUsed("WarehouseRequisitionComponents", new object[3] { eRPWarehouseRequisitionComponentInformationDto.wqoWarehouseRequisitionID, eRPWarehouseRequisitionComponentInformationDto.wqoWarehouseRequisitionLineID, eRPWarehouseRequisitionComponentInformationDto.wqoWarehouseReqComponentID }, new object[3] { "wqoWarehouseRequisitionID", "wqoWarehouseRequisitionLineID", "wqoWarehouseReqComponentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("WarehouseRequisitionComponent cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseRequisitionComponentDto>> Process_DeleteWarehouseRequisitionComponent(Guid warehouseRequisitionComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPWarehouseRequisitionComponentDto> result;
		try
		{
			IERPWarehouseRequisitionComponentRepository iERPWarehouseRequisitionComponentRepository = (base.ERPWarehouseRequisitionComponentRepository = new ERPWarehouseRequisitionComponentRepository(base.ApiClientContext));
			using (iERPWarehouseRequisitionComponentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPWarehouseRequisitionComponentRepository.DeleteRowFromTable("WarehouseRequisitionComponents", "wqo", warehouseRequisitionComponentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of WarehouseRequisitionComponent [{warehouseRequisitionComponentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseRequisitionComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPWarehouseRequisitionComponentDto()
			};
		}
		return result;
	}
}
