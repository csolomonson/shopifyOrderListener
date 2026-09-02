using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPShipmentComponentModel : ERPBaseModel, IERPShipmentComponentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllShipmentComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPShipmentComponentRepository iERPShipmentComponentRepository = (base.ERPShipmentComponentRepository = new ERPShipmentComponentRepository(base.ApiClientContext));
		using (iERPShipmentComponentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPShipmentComponentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPShipmentComponentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPShipmentComponentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPShipmentComponentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetShipmentComponent(Guid shipmentComponentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentComponentRepository iERPShipmentComponentRepository = (base.ERPShipmentComponentRepository = new ERPShipmentComponentRepository(base.ApiClientContext));
		using (iERPShipmentComponentRepository)
		{
			if (!(await base.ERPShipmentComponentRepository.DoesShipmentComponentExist(shipmentComponentId)))
			{
				errorsList.Add($"ShipmentComponent [{shipmentComponentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutShipmentComponent(ERPShipmentComponentDto shipmentComponent)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentComponentRepository iERPShipmentComponentRepository = (base.ERPShipmentComponentRepository = new ERPShipmentComponentRepository(base.ApiClientContext));
		using (iERPShipmentComponentRepository)
		{
			if (!string.IsNullOrWhiteSpace(shipmentComponent.smoShipmentID) && !(await base.ERPShipmentComponentRepository.DoesRecordExistInTableUsingKeys("Shipments", new object[1] { "SMPSHIPMENTID" }, new object[1] { shipmentComponent.smoShipmentID })))
			{
				errorsList.Add("smoShipmentID [" + shipmentComponent.smoShipmentID + "] not found.");
			}
			if (shipmentComponent.smoShipmentLineID > 0 && !(await base.ERPShipmentComponentRepository.DoesRecordExistInTableUsingKeys("ShipmentLines", new object[2] { "SMLSHIPMENTID", "SMLSHIPMENTLINEID" }, new object[2] { shipmentComponent.smoShipmentID, shipmentComponent.smoShipmentLineID })))
			{
				errorsList.Add($"smoShipmentLineID [{shipmentComponent.smoShipmentLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentComponent.smoPartID) && !(await base.ERPShipmentComponentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { shipmentComponent.smoPartID })))
			{
				errorsList.Add("smoPartID [" + shipmentComponent.smoPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentComponent.smoPartRevisionID) && !(await base.ERPShipmentComponentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { shipmentComponent.smoPartID, shipmentComponent.smoPartRevisionID })))
			{
				errorsList.Add("smoPartRevisionID [" + shipmentComponent.smoPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentComponent.smoPartWarehouseLocationID) && !(await base.ERPShipmentComponentRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { shipmentComponent.smoPartID, shipmentComponent.smoPartRevisionID, shipmentComponent.smoPartWarehouseLocationID })))
			{
				errorsList.Add("smoPartWarehouseLocationID [" + shipmentComponent.smoPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentComponent.smoPartBinID) && !(await base.ERPShipmentComponentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { shipmentComponent.smoPartID, shipmentComponent.smoPartRevisionID, shipmentComponent.smoPartWarehouseLocationID, shipmentComponent.smoPartBinID })))
			{
				errorsList.Add("smoPartBinID [" + shipmentComponent.smoPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentComponent.smoSalesOrderID) && !(await base.ERPShipmentComponentRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { shipmentComponent.smoSalesOrderID })))
			{
				errorsList.Add("smoSalesOrderID [" + shipmentComponent.smoSalesOrderID + "] not found.");
			}
			if (shipmentComponent.smoSalesOrderLineID > 0 && !(await base.ERPShipmentComponentRepository.DoesRecordExistInTableUsingKeys("SalesOrderLines", new object[2] { "OMLSALESORDERID", "OMLSALESORDERLINEID" }, new object[2] { shipmentComponent.smoSalesOrderID, shipmentComponent.smoSalesOrderLineID })))
			{
				errorsList.Add($"smoSalesOrderLineID [{shipmentComponent.smoSalesOrderLineID}] not found.");
			}
			if (shipmentComponent.smoSalesOrderDeliveryID > 0 && !(await base.ERPShipmentComponentRepository.DoesRecordExistInTableUsingKeys("SalesOrderDeliveries", new object[3] { "OMDSALESORDERID", "OMDSALESORDERLINEID", "OMDSALESORDERDELIVERYID" }, new object[3] { shipmentComponent.smoSalesOrderID, shipmentComponent.smoSalesOrderLineID, shipmentComponent.smoSalesOrderDeliveryID })))
			{
				errorsList.Add($"smoSalesOrderDeliveryID [{shipmentComponent.smoSalesOrderDeliveryID}] not found.");
			}
			if (shipmentComponent.smoSalesOrderComponentID > 0 && !(await base.ERPShipmentComponentRepository.DoesRecordExistInTableUsingKeys("SalesOrderComponents", new object[4] { "OMOSALESORDERID", "OMOSALESORDERLINEID", "OMOSALESORDERDELIVERYID", "OMOSALESORDERCOMPONENTID" }, new object[4] { shipmentComponent.smoSalesOrderID, shipmentComponent.smoSalesOrderLineID, shipmentComponent.smoSalesOrderDeliveryID, shipmentComponent.smoSalesOrderComponentID })))
			{
				errorsList.Add($"smoSalesOrderComponentID [{shipmentComponent.smoSalesOrderComponentID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentComponent.smoJobID) && !(await base.ERPShipmentComponentRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { shipmentComponent.smoJobID })))
			{
				errorsList.Add("smoJobID [" + shipmentComponent.smoJobID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPShipmentComponentDto>>> Process_GetAllShipmentComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPShipmentComponentDto> allShipmentComponentsDto = new List<ERPShipmentComponentDto>();
		ERPResponseMessageDto<IList<ERPShipmentComponentDto>> result;
		try
		{
			IERPShipmentComponentRepository iERPShipmentComponentRepository = (base.ERPShipmentComponentRepository = new ERPShipmentComponentRepository(base.ApiClientContext));
			using (iERPShipmentComponentRepository)
			{
				foreach (ERPShipmentComponentInformationDto item2 in await base.ERPShipmentComponentRepository.GetAllShipmentComponents(pageSize, pageNumber, filter, orderBy))
				{
					ERPShipmentComponentDto item = new ERPShipmentComponentDto
					{
						smoAdditionalQuantity = item2.smoAdditionalQuantity,
						smoCreatedBy = item2.smoCreatedBy,
						smoCreatedDate = item2.smoCreatedDate,
						smoDescription = item2.smoDescription,
						smoUniqueID = item2.smoUniqueID,
						smoClosed = item2.smoClosed,
						smoPostedToGl = item2.smoPostedToGl,
						smoReversed = item2.smoReversed,
						smoShippedComplete = item2.smoShippedComplete,
						smoJobID = item2.smoJobID,
						smoJobParentQuantity = item2.smoJobParentQuantity,
						smoJobQuantityShipped = item2.smoJobQuantityShipped,
						smoParentQuantity = item2.smoParentQuantity,
						smoPartBinID = item2.smoPartBinID,
						smoPartID = item2.smoPartID,
						smoPartRevisionID = item2.smoPartRevisionID,
						smoPartWarehouseLocationID = item2.smoPartWarehouseLocationID,
						smoQuantityPerParent = item2.smoQuantityPerParent,
						smoQuantityShipped = item2.smoQuantityShipped,
						smoReverseShipmentComponentID = item2.smoReverseShipmentComponentID,
						smoReverseShipmentID = item2.smoReverseShipmentID,
						smoReverseShipmentLineID = item2.smoReverseShipmentLineID,
						smoRowVersion = item2.smoRowVersion,
						smoSalesOrderComponentID = item2.smoSalesOrderComponentID,
						smoSalesOrderDeliveryID = item2.smoSalesOrderDeliveryID,
						smoSalesOrderID = item2.smoSalesOrderID,
						smoSalesOrderLineID = item2.smoSalesOrderLineID,
						smoShipmentComponentID = item2.smoShipmentComponentID,
						smoShipmentID = item2.smoShipmentID,
						smoShipmentLineID = item2.smoShipmentLineID,
						smoSourceTableName = item2.smoSourceTableName,
						smoSourceTableUniqueID = item2.smoSourceTableUniqueID,
						smoUnitOfMeasure = item2.smoUnitOfMeasure,
						smoWeight = item2.smoWeight,
						CustomFields = item2.CustomFields
					};
					allShipmentComponentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ShipmentComponents]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPShipmentComponentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allShipmentComponentsDto,
				RecordCount = allShipmentComponentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShipmentComponentDto>> Process_GetShipmentComponent(Guid shipmentComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPShipmentComponentDto shipmentComponentDto = null;
		ERPResponseMessageDto<ERPShipmentComponentDto> result;
		try
		{
			IERPShipmentComponentRepository iERPShipmentComponentRepository = (base.ERPShipmentComponentRepository = new ERPShipmentComponentRepository(base.ApiClientContext));
			using (iERPShipmentComponentRepository)
			{
				ERPShipmentComponentInformationDto eRPShipmentComponentInformationDto = await base.ERPShipmentComponentRepository.GetShipmentComponent(shipmentComponentId);
				shipmentComponentDto = new ERPShipmentComponentDto
				{
					smoAdditionalQuantity = eRPShipmentComponentInformationDto.smoAdditionalQuantity,
					smoCreatedBy = eRPShipmentComponentInformationDto.smoCreatedBy,
					smoCreatedDate = eRPShipmentComponentInformationDto.smoCreatedDate,
					smoDescription = eRPShipmentComponentInformationDto.smoDescription,
					smoUniqueID = eRPShipmentComponentInformationDto.smoUniqueID,
					smoClosed = eRPShipmentComponentInformationDto.smoClosed,
					smoPostedToGl = eRPShipmentComponentInformationDto.smoPostedToGl,
					smoReversed = eRPShipmentComponentInformationDto.smoReversed,
					smoShippedComplete = eRPShipmentComponentInformationDto.smoShippedComplete,
					smoJobID = eRPShipmentComponentInformationDto.smoJobID,
					smoJobParentQuantity = eRPShipmentComponentInformationDto.smoJobParentQuantity,
					smoJobQuantityShipped = eRPShipmentComponentInformationDto.smoJobQuantityShipped,
					smoParentQuantity = eRPShipmentComponentInformationDto.smoParentQuantity,
					smoPartBinID = eRPShipmentComponentInformationDto.smoPartBinID,
					smoPartID = eRPShipmentComponentInformationDto.smoPartID,
					smoPartRevisionID = eRPShipmentComponentInformationDto.smoPartRevisionID,
					smoPartWarehouseLocationID = eRPShipmentComponentInformationDto.smoPartWarehouseLocationID,
					smoQuantityPerParent = eRPShipmentComponentInformationDto.smoQuantityPerParent,
					smoQuantityShipped = eRPShipmentComponentInformationDto.smoQuantityShipped,
					smoReverseShipmentComponentID = eRPShipmentComponentInformationDto.smoReverseShipmentComponentID,
					smoReverseShipmentID = eRPShipmentComponentInformationDto.smoReverseShipmentID,
					smoReverseShipmentLineID = eRPShipmentComponentInformationDto.smoReverseShipmentLineID,
					smoRowVersion = eRPShipmentComponentInformationDto.smoRowVersion,
					smoSalesOrderComponentID = eRPShipmentComponentInformationDto.smoSalesOrderComponentID,
					smoSalesOrderDeliveryID = eRPShipmentComponentInformationDto.smoSalesOrderDeliveryID,
					smoSalesOrderID = eRPShipmentComponentInformationDto.smoSalesOrderID,
					smoSalesOrderLineID = eRPShipmentComponentInformationDto.smoSalesOrderLineID,
					smoShipmentComponentID = eRPShipmentComponentInformationDto.smoShipmentComponentID,
					smoShipmentID = eRPShipmentComponentInformationDto.smoShipmentID,
					smoShipmentLineID = eRPShipmentComponentInformationDto.smoShipmentLineID,
					smoSourceTableName = eRPShipmentComponentInformationDto.smoSourceTableName,
					smoSourceTableUniqueID = eRPShipmentComponentInformationDto.smoSourceTableUniqueID,
					smoUnitOfMeasure = eRPShipmentComponentInformationDto.smoUnitOfMeasure,
					smoWeight = eRPShipmentComponentInformationDto.smoWeight,
					CustomFields = eRPShipmentComponentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ShipmentComponents []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = shipmentComponentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShipmentComponentDto>> Process_PutShipmentComponent(ERPShipmentComponentDto shipmentComponent)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPShipmentComponentDto createdObject = null;
		ERPResponseMessageDto<ERPShipmentComponentDto> result;
		try
		{
			IERPShipmentComponentRepository iERPShipmentComponentRepository = (base.ERPShipmentComponentRepository = new ERPShipmentComponentRepository(base.ApiClientContext));
			using (iERPShipmentComponentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPShipmentComponentRepository.SaveShipmentComponent(shipmentComponent);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPShipmentComponentInformationDto eRPShipmentComponentInformationDto = await base.ERPShipmentComponentRepository.GetShipmentComponent(shipmentComponent.smoUniqueID);
					createdObject = new ERPShipmentComponentDto
					{
						smoAdditionalQuantity = eRPShipmentComponentInformationDto.smoAdditionalQuantity,
						smoCreatedBy = eRPShipmentComponentInformationDto.smoCreatedBy,
						smoCreatedDate = eRPShipmentComponentInformationDto.smoCreatedDate,
						smoDescription = eRPShipmentComponentInformationDto.smoDescription,
						smoUniqueID = eRPShipmentComponentInformationDto.smoUniqueID,
						smoClosed = eRPShipmentComponentInformationDto.smoClosed,
						smoPostedToGl = eRPShipmentComponentInformationDto.smoPostedToGl,
						smoReversed = eRPShipmentComponentInformationDto.smoReversed,
						smoShippedComplete = eRPShipmentComponentInformationDto.smoShippedComplete,
						smoJobID = eRPShipmentComponentInformationDto.smoJobID,
						smoJobParentQuantity = eRPShipmentComponentInformationDto.smoJobParentQuantity,
						smoJobQuantityShipped = eRPShipmentComponentInformationDto.smoJobQuantityShipped,
						smoParentQuantity = eRPShipmentComponentInformationDto.smoParentQuantity,
						smoPartBinID = eRPShipmentComponentInformationDto.smoPartBinID,
						smoPartID = eRPShipmentComponentInformationDto.smoPartID,
						smoPartRevisionID = eRPShipmentComponentInformationDto.smoPartRevisionID,
						smoPartWarehouseLocationID = eRPShipmentComponentInformationDto.smoPartWarehouseLocationID,
						smoQuantityPerParent = eRPShipmentComponentInformationDto.smoQuantityPerParent,
						smoQuantityShipped = eRPShipmentComponentInformationDto.smoQuantityShipped,
						smoReverseShipmentComponentID = eRPShipmentComponentInformationDto.smoReverseShipmentComponentID,
						smoReverseShipmentID = eRPShipmentComponentInformationDto.smoReverseShipmentID,
						smoReverseShipmentLineID = eRPShipmentComponentInformationDto.smoReverseShipmentLineID,
						smoRowVersion = eRPShipmentComponentInformationDto.smoRowVersion,
						smoSalesOrderComponentID = eRPShipmentComponentInformationDto.smoSalesOrderComponentID,
						smoSalesOrderDeliveryID = eRPShipmentComponentInformationDto.smoSalesOrderDeliveryID,
						smoSalesOrderID = eRPShipmentComponentInformationDto.smoSalesOrderID,
						smoSalesOrderLineID = eRPShipmentComponentInformationDto.smoSalesOrderLineID,
						smoShipmentComponentID = eRPShipmentComponentInformationDto.smoShipmentComponentID,
						smoShipmentID = eRPShipmentComponentInformationDto.smoShipmentID,
						smoShipmentLineID = eRPShipmentComponentInformationDto.smoShipmentLineID,
						smoSourceTableName = eRPShipmentComponentInformationDto.smoSourceTableName,
						smoSourceTableUniqueID = eRPShipmentComponentInformationDto.smoSourceTableUniqueID,
						smoUnitOfMeasure = eRPShipmentComponentInformationDto.smoUnitOfMeasure,
						smoWeight = eRPShipmentComponentInformationDto.smoWeight,
						CustomFields = eRPShipmentComponentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ShipmentComponent [{shipmentComponent.smoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteShipmentComponent(Guid shipmentComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentComponentRepository iERPShipmentComponentRepository = (base.ERPShipmentComponentRepository = new ERPShipmentComponentRepository(base.ApiClientContext));
		using (iERPShipmentComponentRepository)
		{
			if (!(await base.ERPShipmentComponentRepository.DoesShipmentComponentExist(shipmentComponentId)))
			{
				base.ErrorsList.Add($"ShipmentComponent [{shipmentComponentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPShipmentComponentInformationDto eRPShipmentComponentInformationDto = await base.ERPShipmentComponentRepository.GetShipmentComponent(shipmentComponentId);
				string text = await base.ERPShipmentComponentRepository.WhereUsed("ShipmentComponents", new object[3] { eRPShipmentComponentInformationDto.smoShipmentID, eRPShipmentComponentInformationDto.smoShipmentLineID, eRPShipmentComponentInformationDto.smoShipmentComponentID }, new object[3] { "smoShipmentID", "smoShipmentLineID", "smoShipmentComponentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ShipmentComponent cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPShipmentComponentDto>> Process_DeleteShipmentComponent(Guid shipmentComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPShipmentComponentDto> result;
		try
		{
			IERPShipmentComponentRepository iERPShipmentComponentRepository = (base.ERPShipmentComponentRepository = new ERPShipmentComponentRepository(base.ApiClientContext));
			using (iERPShipmentComponentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPShipmentComponentRepository.DeleteRowFromTable("ShipmentComponents", "smo", shipmentComponentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ShipmentComponent [{shipmentComponentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPShipmentComponentDto()
			};
		}
		return result;
	}
}
