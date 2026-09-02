using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSalesOrderComponentModel : ERPBaseModel, IERPSalesOrderComponentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSalesOrderComponentRepository iERPSalesOrderComponentRepository = (base.ERPSalesOrderComponentRepository = new ERPSalesOrderComponentRepository(base.ApiClientContext));
		using (iERPSalesOrderComponentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSalesOrderComponentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSalesOrderComponentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSalesOrderComponentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSalesOrderComponentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderComponent(Guid salesOrderComponentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderComponentRepository iERPSalesOrderComponentRepository = (base.ERPSalesOrderComponentRepository = new ERPSalesOrderComponentRepository(base.ApiClientContext));
		using (iERPSalesOrderComponentRepository)
		{
			if (!(await base.ERPSalesOrderComponentRepository.DoesSalesOrderComponentExist(salesOrderComponentId)))
			{
				errorsList.Add($"SalesOrderComponent [{salesOrderComponentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderComponent(ERPSalesOrderComponentDto salesOrderComponent)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderComponentRepository iERPSalesOrderComponentRepository = (base.ERPSalesOrderComponentRepository = new ERPSalesOrderComponentRepository(base.ApiClientContext));
		using (iERPSalesOrderComponentRepository)
		{
			if (!string.IsNullOrWhiteSpace(salesOrderComponent.omoSalesOrderID) && !(await base.ERPSalesOrderComponentRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { salesOrderComponent.omoSalesOrderID })))
			{
				errorsList.Add("omoSalesOrderID [" + salesOrderComponent.omoSalesOrderID + "] not found.");
			}
			if (salesOrderComponent.omoSalesOrderLineID > 0 && !(await base.ERPSalesOrderComponentRepository.DoesRecordExistInTableUsingKeys("SalesOrderLines", new object[2] { "OMLSALESORDERID", "OMLSALESORDERLINEID" }, new object[2] { salesOrderComponent.omoSalesOrderID, salesOrderComponent.omoSalesOrderLineID })))
			{
				errorsList.Add($"omoSalesOrderLineID [{salesOrderComponent.omoSalesOrderLineID}] not found.");
			}
			if (salesOrderComponent.omoSalesOrderDeliveryID > 0 && !(await base.ERPSalesOrderComponentRepository.DoesRecordExistInTableUsingKeys("SalesOrderDeliveries", new object[3] { "OMDSALESORDERID", "OMDSALESORDERLINEID", "OMDSALESORDERDELIVERYID" }, new object[3] { salesOrderComponent.omoSalesOrderID, salesOrderComponent.omoSalesOrderLineID, salesOrderComponent.omoSalesOrderDeliveryID })))
			{
				errorsList.Add($"omoSalesOrderDeliveryID [{salesOrderComponent.omoSalesOrderDeliveryID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderComponent.omoPartID) && !(await base.ERPSalesOrderComponentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { salesOrderComponent.omoPartID })))
			{
				errorsList.Add("omoPartID [" + salesOrderComponent.omoPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderComponent.omoPartRevisionID) && !(await base.ERPSalesOrderComponentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { salesOrderComponent.omoPartID, salesOrderComponent.omoPartRevisionID })))
			{
				errorsList.Add("omoPartRevisionID [" + salesOrderComponent.omoPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderComponent.omoPartWarehouseLocationID) && !(await base.ERPSalesOrderComponentRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { salesOrderComponent.omoPartID, salesOrderComponent.omoPartRevisionID, salesOrderComponent.omoPartWarehouseLocationID })))
			{
				errorsList.Add("omoPartWarehouseLocationID [" + salesOrderComponent.omoPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderComponent.omoPartBinID) && !(await base.ERPSalesOrderComponentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { salesOrderComponent.omoPartID, salesOrderComponent.omoPartRevisionID, salesOrderComponent.omoPartWarehouseLocationID, salesOrderComponent.omoPartBinID })))
			{
				errorsList.Add("omoPartBinID [" + salesOrderComponent.omoPartBinID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSalesOrderComponentDto>>> Process_GetAllSalesOrderComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSalesOrderComponentDto> allSalesOrderComponentsDto = new List<ERPSalesOrderComponentDto>();
		ERPResponseMessageDto<IList<ERPSalesOrderComponentDto>> result;
		try
		{
			IERPSalesOrderComponentRepository iERPSalesOrderComponentRepository = (base.ERPSalesOrderComponentRepository = new ERPSalesOrderComponentRepository(base.ApiClientContext));
			using (iERPSalesOrderComponentRepository)
			{
				foreach (ERPSalesOrderComponentInformationDto item2 in await base.ERPSalesOrderComponentRepository.GetAllSalesOrderComponents(pageSize, pageNumber, filter, orderBy))
				{
					ERPSalesOrderComponentDto item = new ERPSalesOrderComponentDto
					{
						omoAdditionalQuantity = item2.omoAdditionalQuantity,
						omoCreatedBy = item2.omoCreatedBy,
						omoCreatedDate = item2.omoCreatedDate,
						omoDeliveryQuantity = item2.omoDeliveryQuantity,
						omoDescription = item2.omoDescription,
						omoUniqueID = item2.omoUniqueID,
						omoClosed = item2.omoClosed,
						omoShippedComplete = item2.omoShippedComplete,
						omoParentQuantity = item2.omoParentQuantity,
						omoPartBinID = item2.omoPartBinID,
						omoPartID = item2.omoPartID,
						omoPartRevisionID = item2.omoPartRevisionID,
						omoPartWarehouseLocationID = item2.omoPartWarehouseLocationID,
						omoQuantityAllocated = item2.omoQuantityAllocated,
						omoQuantityPerParent = item2.omoQuantityPerParent,
						omoQuantityShipped = item2.omoQuantityShipped,
						omoRowVersion = item2.omoRowVersion,
						omoSalesOrderDeliveryID = item2.omoSalesOrderDeliveryID,
						omoSalesOrderID = item2.omoSalesOrderID,
						omoSalesOrderLineID = item2.omoSalesOrderLineID,
						omoSalesOrderComponentID = item2.omoSalesOrderComponentID,
						omoUnitOfMeasure = item2.omoUnitOfMeasure,
						omoWeight = item2.omoWeight,
						CustomFields = item2.CustomFields
					};
					allSalesOrderComponentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SalesOrderComponents]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSalesOrderComponentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSalesOrderComponentsDto,
				RecordCount = allSalesOrderComponentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderComponentDto>> Process_GetSalesOrderComponent(Guid salesOrderComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSalesOrderComponentDto salesOrderComponentDto = null;
		ERPResponseMessageDto<ERPSalesOrderComponentDto> result;
		try
		{
			IERPSalesOrderComponentRepository iERPSalesOrderComponentRepository = (base.ERPSalesOrderComponentRepository = new ERPSalesOrderComponentRepository(base.ApiClientContext));
			using (iERPSalesOrderComponentRepository)
			{
				ERPSalesOrderComponentInformationDto eRPSalesOrderComponentInformationDto = await base.ERPSalesOrderComponentRepository.GetSalesOrderComponent(salesOrderComponentId);
				salesOrderComponentDto = new ERPSalesOrderComponentDto
				{
					omoAdditionalQuantity = eRPSalesOrderComponentInformationDto.omoAdditionalQuantity,
					omoCreatedBy = eRPSalesOrderComponentInformationDto.omoCreatedBy,
					omoCreatedDate = eRPSalesOrderComponentInformationDto.omoCreatedDate,
					omoDeliveryQuantity = eRPSalesOrderComponentInformationDto.omoDeliveryQuantity,
					omoDescription = eRPSalesOrderComponentInformationDto.omoDescription,
					omoUniqueID = eRPSalesOrderComponentInformationDto.omoUniqueID,
					omoClosed = eRPSalesOrderComponentInformationDto.omoClosed,
					omoShippedComplete = eRPSalesOrderComponentInformationDto.omoShippedComplete,
					omoParentQuantity = eRPSalesOrderComponentInformationDto.omoParentQuantity,
					omoPartBinID = eRPSalesOrderComponentInformationDto.omoPartBinID,
					omoPartID = eRPSalesOrderComponentInformationDto.omoPartID,
					omoPartRevisionID = eRPSalesOrderComponentInformationDto.omoPartRevisionID,
					omoPartWarehouseLocationID = eRPSalesOrderComponentInformationDto.omoPartWarehouseLocationID,
					omoQuantityAllocated = eRPSalesOrderComponentInformationDto.omoQuantityAllocated,
					omoQuantityPerParent = eRPSalesOrderComponentInformationDto.omoQuantityPerParent,
					omoQuantityShipped = eRPSalesOrderComponentInformationDto.omoQuantityShipped,
					omoRowVersion = eRPSalesOrderComponentInformationDto.omoRowVersion,
					omoSalesOrderDeliveryID = eRPSalesOrderComponentInformationDto.omoSalesOrderDeliveryID,
					omoSalesOrderID = eRPSalesOrderComponentInformationDto.omoSalesOrderID,
					omoSalesOrderLineID = eRPSalesOrderComponentInformationDto.omoSalesOrderLineID,
					omoSalesOrderComponentID = eRPSalesOrderComponentInformationDto.omoSalesOrderComponentID,
					omoUnitOfMeasure = eRPSalesOrderComponentInformationDto.omoUnitOfMeasure,
					omoWeight = eRPSalesOrderComponentInformationDto.omoWeight,
					CustomFields = eRPSalesOrderComponentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SalesOrderComponents []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = salesOrderComponentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderComponentDto>> Process_PutSalesOrderComponent(ERPSalesOrderComponentDto salesOrderComponent)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPSalesOrderComponentDto createdObject = null;
		ERPResponseMessageDto<ERPSalesOrderComponentDto> result;
		try
		{
			IERPSalesOrderComponentRepository iERPSalesOrderComponentRepository = (base.ERPSalesOrderComponentRepository = new ERPSalesOrderComponentRepository(base.ApiClientContext));
			using (iERPSalesOrderComponentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPSalesOrderComponentRepository.SaveSalesOrderComponent(salesOrderComponent);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPSalesOrderComponentInformationDto eRPSalesOrderComponentInformationDto = await base.ERPSalesOrderComponentRepository.GetSalesOrderComponent(salesOrderComponent.omoUniqueID);
					createdObject = new ERPSalesOrderComponentDto
					{
						omoAdditionalQuantity = eRPSalesOrderComponentInformationDto.omoAdditionalQuantity,
						omoCreatedBy = eRPSalesOrderComponentInformationDto.omoCreatedBy,
						omoCreatedDate = eRPSalesOrderComponentInformationDto.omoCreatedDate,
						omoDeliveryQuantity = eRPSalesOrderComponentInformationDto.omoDeliveryQuantity,
						omoDescription = eRPSalesOrderComponentInformationDto.omoDescription,
						omoUniqueID = eRPSalesOrderComponentInformationDto.omoUniqueID,
						omoClosed = eRPSalesOrderComponentInformationDto.omoClosed,
						omoShippedComplete = eRPSalesOrderComponentInformationDto.omoShippedComplete,
						omoParentQuantity = eRPSalesOrderComponentInformationDto.omoParentQuantity,
						omoPartBinID = eRPSalesOrderComponentInformationDto.omoPartBinID,
						omoPartID = eRPSalesOrderComponentInformationDto.omoPartID,
						omoPartRevisionID = eRPSalesOrderComponentInformationDto.omoPartRevisionID,
						omoPartWarehouseLocationID = eRPSalesOrderComponentInformationDto.omoPartWarehouseLocationID,
						omoQuantityAllocated = eRPSalesOrderComponentInformationDto.omoQuantityAllocated,
						omoQuantityPerParent = eRPSalesOrderComponentInformationDto.omoQuantityPerParent,
						omoQuantityShipped = eRPSalesOrderComponentInformationDto.omoQuantityShipped,
						omoRowVersion = eRPSalesOrderComponentInformationDto.omoRowVersion,
						omoSalesOrderDeliveryID = eRPSalesOrderComponentInformationDto.omoSalesOrderDeliveryID,
						omoSalesOrderID = eRPSalesOrderComponentInformationDto.omoSalesOrderID,
						omoSalesOrderLineID = eRPSalesOrderComponentInformationDto.omoSalesOrderLineID,
						omoSalesOrderComponentID = eRPSalesOrderComponentInformationDto.omoSalesOrderComponentID,
						omoUnitOfMeasure = eRPSalesOrderComponentInformationDto.omoUnitOfMeasure,
						omoWeight = eRPSalesOrderComponentInformationDto.omoWeight,
						CustomFields = eRPSalesOrderComponentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing SalesOrderComponent [{salesOrderComponent.omoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderComponent(Guid salesOrderComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderComponentRepository iERPSalesOrderComponentRepository = (base.ERPSalesOrderComponentRepository = new ERPSalesOrderComponentRepository(base.ApiClientContext));
		using (iERPSalesOrderComponentRepository)
		{
			if (!(await base.ERPSalesOrderComponentRepository.DoesSalesOrderComponentExist(salesOrderComponentId)))
			{
				base.ErrorsList.Add($"SalesOrderComponent [{salesOrderComponentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPSalesOrderComponentInformationDto eRPSalesOrderComponentInformationDto = await base.ERPSalesOrderComponentRepository.GetSalesOrderComponent(salesOrderComponentId);
				string text = await base.ERPSalesOrderComponentRepository.WhereUsed("SalesOrderComponents", new object[4] { eRPSalesOrderComponentInformationDto.omoSalesOrderID, eRPSalesOrderComponentInformationDto.omoSalesOrderLineID, eRPSalesOrderComponentInformationDto.omoSalesOrderDeliveryID, eRPSalesOrderComponentInformationDto.omoSalesOrderComponentID }, new object[4] { "omoSalesOrderID", "omoSalesOrderLineID", "omoSalesOrderDeliveryID", "omoSalesOrderComponentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("SalesOrderComponent cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderComponentDto>> Process_DeleteSalesOrderComponent(Guid salesOrderComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPSalesOrderComponentDto> result;
		try
		{
			IERPSalesOrderComponentRepository iERPSalesOrderComponentRepository = (base.ERPSalesOrderComponentRepository = new ERPSalesOrderComponentRepository(base.ApiClientContext));
			using (iERPSalesOrderComponentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPSalesOrderComponentRepository.DeleteRowFromTable("SalesOrderComponents", "omo", salesOrderComponentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of SalesOrderComponent [{salesOrderComponentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPSalesOrderComponentDto()
			};
		}
		return result;
	}
}
