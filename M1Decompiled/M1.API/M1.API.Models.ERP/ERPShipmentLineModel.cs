using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPShipmentLineModel : ERPBaseModel, IERPShipmentLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllShipmentLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPShipmentLineRepository iERPShipmentLineRepository = (base.ERPShipmentLineRepository = new ERPShipmentLineRepository(base.ApiClientContext));
		using (iERPShipmentLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPShipmentLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPShipmentLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPShipmentLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPShipmentLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetShipmentLine(Guid shipmentLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentLineRepository iERPShipmentLineRepository = (base.ERPShipmentLineRepository = new ERPShipmentLineRepository(base.ApiClientContext));
		using (iERPShipmentLineRepository)
		{
			if (!(await base.ERPShipmentLineRepository.DoesShipmentLineExist(shipmentLineId)))
			{
				errorsList.Add($"ShipmentLine [{shipmentLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutShipmentLine(ERPShipmentLineDto shipmentLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentLineRepository iERPShipmentLineRepository = (base.ERPShipmentLineRepository = new ERPShipmentLineRepository(base.ApiClientContext));
		using (iERPShipmentLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(shipmentLine.smlShipmentID) && !(await base.ERPShipmentLineRepository.DoesRecordExistInTableUsingKeys("Shipments", new object[1] { "SMPSHIPMENTID" }, new object[1] { shipmentLine.smlShipmentID })))
			{
				errorsList.Add("smlShipmentID [" + shipmentLine.smlShipmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentLine.smlPartID) && !(await base.ERPShipmentLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { shipmentLine.smlPartID })))
			{
				errorsList.Add("smlPartID [" + shipmentLine.smlPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentLine.smlPartRevisionID) && !(await base.ERPShipmentLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { shipmentLine.smlPartID, shipmentLine.smlPartRevisionID })))
			{
				errorsList.Add("smlPartRevisionID [" + shipmentLine.smlPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentLine.smlPartWarehouseLocationID) && !(await base.ERPShipmentLineRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { shipmentLine.smlPartID, shipmentLine.smlPartRevisionID, shipmentLine.smlPartWarehouseLocationID })))
			{
				errorsList.Add("smlPartWarehouseLocationID [" + shipmentLine.smlPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentLine.smlPartBinID) && !(await base.ERPShipmentLineRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { shipmentLine.smlPartID, shipmentLine.smlPartRevisionID, shipmentLine.smlPartWarehouseLocationID, shipmentLine.smlPartBinID })))
			{
				errorsList.Add("smlPartBinID [" + shipmentLine.smlPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentLine.smlSalesOrderID) && !(await base.ERPShipmentLineRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { shipmentLine.smlSalesOrderID })))
			{
				errorsList.Add("smlSalesOrderID [" + shipmentLine.smlSalesOrderID + "] not found.");
			}
			if (shipmentLine.smlSalesOrderLineID > 0 && !(await base.ERPShipmentLineRepository.DoesRecordExistInTableUsingKeys("SalesOrderLines", new object[2] { "OMLSALESORDERID", "OMLSALESORDERLINEID" }, new object[2] { shipmentLine.smlSalesOrderID, shipmentLine.smlSalesOrderLineID })))
			{
				errorsList.Add($"smlSalesOrderLineID [{shipmentLine.smlSalesOrderLineID}] not found.");
			}
			if (shipmentLine.smlSalesOrderDeliveryID > 0 && !(await base.ERPShipmentLineRepository.DoesRecordExistInTableUsingKeys("SalesOrderDeliveries", new object[3] { "OMDSALESORDERID", "OMDSALESORDERLINEID", "OMDSALESORDERDELIVERYID" }, new object[3] { shipmentLine.smlSalesOrderID, shipmentLine.smlSalesOrderLineID, shipmentLine.smlSalesOrderDeliveryID })))
			{
				errorsList.Add($"smlSalesOrderDeliveryID [{shipmentLine.smlSalesOrderDeliveryID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentLine.smlJobID) && !(await base.ERPShipmentLineRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { shipmentLine.smlJobID })))
			{
				errorsList.Add("smlJobID [" + shipmentLine.smlJobID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentLine.smlPartGroupID) && !(await base.ERPShipmentLineRepository.DoesRecordExistInTableUsingKeys("PartGroups", new object[1] { "IMUPARTGROUPID" }, new object[1] { shipmentLine.smlPartGroupID })))
			{
				errorsList.Add("smlPartGroupID [" + shipmentLine.smlPartGroupID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentLine.smlProjectID) && !(await base.ERPShipmentLineRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { shipmentLine.smlProjectID })))
			{
				errorsList.Add("smlProjectID [" + shipmentLine.smlProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentLine.smlProjectAreaID) && !(await base.ERPShipmentLineRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { shipmentLine.smlProjectID, shipmentLine.smlProjectAreaID })))
			{
				errorsList.Add("smlProjectAreaID [" + shipmentLine.smlProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipmentLine.smlReverseShipmentID) && !(await base.ERPShipmentLineRepository.DoesRecordExistInTableUsingKeys("Shipments", new object[1] { "SMPSHIPMENTID" }, new object[1] { shipmentLine.smlReverseShipmentID })))
			{
				errorsList.Add("smlReverseShipmentID [" + shipmentLine.smlReverseShipmentID + "] not found.");
			}
			if (shipmentLine.smlReverseShipmentLineID > 0 && !(await base.ERPShipmentLineRepository.DoesRecordExistInTableUsingKeys("ShipmentLines", new object[2] { "SMLSHIPMENTID", "SMLSHIPMENTLINEID" }, new object[2] { shipmentLine.smlReverseShipmentID, shipmentLine.smlReverseShipmentLineID })))
			{
				errorsList.Add($"smlReverseShipmentLineID [{shipmentLine.smlReverseShipmentLineID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPShipmentLineDto>>> Process_GetAllShipmentLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPShipmentLineDto> allShipmentLinesDto = new List<ERPShipmentLineDto>();
		ERPResponseMessageDto<IList<ERPShipmentLineDto>> result;
		try
		{
			IERPShipmentLineRepository iERPShipmentLineRepository = (base.ERPShipmentLineRepository = new ERPShipmentLineRepository(base.ApiClientContext));
			using (iERPShipmentLineRepository)
			{
				foreach (ERPShipmentLineInformationDto item2 in await base.ERPShipmentLineRepository.GetAllShipmentLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPShipmentLineDto item = new ERPShipmentLineDto
					{
						smlCreatedBy = item2.smlCreatedBy,
						smlCreatedDate = item2.smlCreatedDate,
						smlDescription = item2.smlDescription,
						smlUniqueID = item2.smlUniqueID,
						smlExtendedPriceBase = item2.smlExtendedPriceBase,
						smlExtendedPriceForeign = item2.smlExtendedPriceForeign,
						smlExtendedWeight = item2.smlExtendedWeight,
						smlFreightAmount = item2.smlFreightAmount,
						smlFreightAmountForeign = item2.smlFreightAmountForeign,
						smlHeatLot = item2.smlHeatLot,
						smlClosed = item2.smlClosed,
						smlInvoicedComplete = item2.smlInvoicedComplete,
						smlKitPart = item2.smlKitPart,
						smlOverridePrice = item2.smlOverridePrice,
						smlPostedToGl = item2.smlPostedToGl,
						smlRequiresInspection = item2.smlRequiresInspection,
						smlReversed = item2.smlReversed,
						smlShippedComplete = item2.smlShippedComplete,
						smlJobID = item2.smlJobID,
						smlJobQuantityShipped = item2.smlJobQuantityShipped,
						smlOrgPartID = item2.smlOrgPartID,
						smlOrgPartShortDescription = item2.smlOrgPartShortDescription,
						smlPartBinID = item2.smlPartBinID,
						smlPartGroupID = item2.smlPartGroupID,
						smlPartID = item2.smlPartID,
						smlPartLongDescriptionRtf = item2.smlPartLongDescriptionRtf,
						smlPartLongDescriptionText = item2.smlPartLongDescriptionText,
						smlPartRevisionID = item2.smlPartRevisionID,
						smlPartWarehouseLocationID = item2.smlPartWarehouseLocationID,
						smlProjectAreaID = item2.smlProjectAreaID,
						smlProjectID = item2.smlProjectID,
						smlQuantityShipped = item2.smlQuantityShipped,
						smlReverseShipmentID = item2.smlReverseShipmentID,
						smlReverseShipmentLineID = item2.smlReverseShipmentLineID,
						smlRowVersion = item2.smlRowVersion,
						smlSalesOrderDeliveryID = item2.smlSalesOrderDeliveryID,
						smlSalesOrderID = item2.smlSalesOrderID,
						smlSalesOrderLineID = item2.smlSalesOrderLineID,
						smlShipmentLineID = item2.smlShipmentLineID,
						smlShipmentID = item2.smlShipmentID,
						smlShipmentIDNumber = item2.smlShipmentIDNumber,
						smlSODeliveryQuantity = item2.smlSODeliveryQuantity,
						smlSOOpenQuantity = item2.smlSOOpenQuantity,
						smlSourceTableName = item2.smlSourceTableName,
						smlSourceTableUniqueID = item2.smlSourceTableUniqueID,
						smlUnitOfMeasure = item2.smlUnitOfMeasure,
						smlUnitPrice = item2.smlUnitPrice,
						smlUnitPriceForeign = item2.smlUnitPriceForeign,
						smlWeight = item2.smlWeight,
						smlWeightUnitOfMeasure = item2.smlWeightUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allShipmentLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ShipmentLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPShipmentLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allShipmentLinesDto,
				RecordCount = allShipmentLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShipmentLineDto>> Process_GetShipmentLine(Guid shipmentLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPShipmentLineDto shipmentLineDto = null;
		ERPResponseMessageDto<ERPShipmentLineDto> result;
		try
		{
			IERPShipmentLineRepository iERPShipmentLineRepository = (base.ERPShipmentLineRepository = new ERPShipmentLineRepository(base.ApiClientContext));
			using (iERPShipmentLineRepository)
			{
				ERPShipmentLineInformationDto eRPShipmentLineInformationDto = await base.ERPShipmentLineRepository.GetShipmentLine(shipmentLineId);
				shipmentLineDto = new ERPShipmentLineDto
				{
					smlCreatedBy = eRPShipmentLineInformationDto.smlCreatedBy,
					smlCreatedDate = eRPShipmentLineInformationDto.smlCreatedDate,
					smlDescription = eRPShipmentLineInformationDto.smlDescription,
					smlUniqueID = eRPShipmentLineInformationDto.smlUniqueID,
					smlExtendedPriceBase = eRPShipmentLineInformationDto.smlExtendedPriceBase,
					smlExtendedPriceForeign = eRPShipmentLineInformationDto.smlExtendedPriceForeign,
					smlExtendedWeight = eRPShipmentLineInformationDto.smlExtendedWeight,
					smlFreightAmount = eRPShipmentLineInformationDto.smlFreightAmount,
					smlFreightAmountForeign = eRPShipmentLineInformationDto.smlFreightAmountForeign,
					smlHeatLot = eRPShipmentLineInformationDto.smlHeatLot,
					smlClosed = eRPShipmentLineInformationDto.smlClosed,
					smlInvoicedComplete = eRPShipmentLineInformationDto.smlInvoicedComplete,
					smlKitPart = eRPShipmentLineInformationDto.smlKitPart,
					smlOverridePrice = eRPShipmentLineInformationDto.smlOverridePrice,
					smlPostedToGl = eRPShipmentLineInformationDto.smlPostedToGl,
					smlRequiresInspection = eRPShipmentLineInformationDto.smlRequiresInspection,
					smlReversed = eRPShipmentLineInformationDto.smlReversed,
					smlShippedComplete = eRPShipmentLineInformationDto.smlShippedComplete,
					smlJobID = eRPShipmentLineInformationDto.smlJobID,
					smlJobQuantityShipped = eRPShipmentLineInformationDto.smlJobQuantityShipped,
					smlOrgPartID = eRPShipmentLineInformationDto.smlOrgPartID,
					smlOrgPartShortDescription = eRPShipmentLineInformationDto.smlOrgPartShortDescription,
					smlPartBinID = eRPShipmentLineInformationDto.smlPartBinID,
					smlPartGroupID = eRPShipmentLineInformationDto.smlPartGroupID,
					smlPartID = eRPShipmentLineInformationDto.smlPartID,
					smlPartLongDescriptionRtf = eRPShipmentLineInformationDto.smlPartLongDescriptionRtf,
					smlPartLongDescriptionText = eRPShipmentLineInformationDto.smlPartLongDescriptionText,
					smlPartRevisionID = eRPShipmentLineInformationDto.smlPartRevisionID,
					smlPartWarehouseLocationID = eRPShipmentLineInformationDto.smlPartWarehouseLocationID,
					smlProjectAreaID = eRPShipmentLineInformationDto.smlProjectAreaID,
					smlProjectID = eRPShipmentLineInformationDto.smlProjectID,
					smlQuantityShipped = eRPShipmentLineInformationDto.smlQuantityShipped,
					smlReverseShipmentID = eRPShipmentLineInformationDto.smlReverseShipmentID,
					smlReverseShipmentLineID = eRPShipmentLineInformationDto.smlReverseShipmentLineID,
					smlRowVersion = eRPShipmentLineInformationDto.smlRowVersion,
					smlSalesOrderDeliveryID = eRPShipmentLineInformationDto.smlSalesOrderDeliveryID,
					smlSalesOrderID = eRPShipmentLineInformationDto.smlSalesOrderID,
					smlSalesOrderLineID = eRPShipmentLineInformationDto.smlSalesOrderLineID,
					smlShipmentLineID = eRPShipmentLineInformationDto.smlShipmentLineID,
					smlShipmentID = eRPShipmentLineInformationDto.smlShipmentID,
					smlShipmentIDNumber = eRPShipmentLineInformationDto.smlShipmentIDNumber,
					smlSODeliveryQuantity = eRPShipmentLineInformationDto.smlSODeliveryQuantity,
					smlSOOpenQuantity = eRPShipmentLineInformationDto.smlSOOpenQuantity,
					smlSourceTableName = eRPShipmentLineInformationDto.smlSourceTableName,
					smlSourceTableUniqueID = eRPShipmentLineInformationDto.smlSourceTableUniqueID,
					smlUnitOfMeasure = eRPShipmentLineInformationDto.smlUnitOfMeasure,
					smlUnitPrice = eRPShipmentLineInformationDto.smlUnitPrice,
					smlUnitPriceForeign = eRPShipmentLineInformationDto.smlUnitPriceForeign,
					smlWeight = eRPShipmentLineInformationDto.smlWeight,
					smlWeightUnitOfMeasure = eRPShipmentLineInformationDto.smlWeightUnitOfMeasure,
					CustomFields = eRPShipmentLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ShipmentLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = shipmentLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShipmentLineDto>> Process_PutShipmentLine(ERPShipmentLineDto shipmentLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPShipmentLineDto createdObject = null;
		ERPResponseMessageDto<ERPShipmentLineDto> result;
		try
		{
			IERPShipmentLineRepository iERPShipmentLineRepository = (base.ERPShipmentLineRepository = new ERPShipmentLineRepository(base.ApiClientContext));
			using (iERPShipmentLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPShipmentLineRepository.SaveShipmentLine(shipmentLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPShipmentLineInformationDto eRPShipmentLineInformationDto = await base.ERPShipmentLineRepository.GetShipmentLine(shipmentLine.smlUniqueID);
					createdObject = new ERPShipmentLineDto
					{
						smlCreatedBy = eRPShipmentLineInformationDto.smlCreatedBy,
						smlCreatedDate = eRPShipmentLineInformationDto.smlCreatedDate,
						smlDescription = eRPShipmentLineInformationDto.smlDescription,
						smlUniqueID = eRPShipmentLineInformationDto.smlUniqueID,
						smlExtendedPriceBase = eRPShipmentLineInformationDto.smlExtendedPriceBase,
						smlExtendedPriceForeign = eRPShipmentLineInformationDto.smlExtendedPriceForeign,
						smlExtendedWeight = eRPShipmentLineInformationDto.smlExtendedWeight,
						smlFreightAmount = eRPShipmentLineInformationDto.smlFreightAmount,
						smlFreightAmountForeign = eRPShipmentLineInformationDto.smlFreightAmountForeign,
						smlHeatLot = eRPShipmentLineInformationDto.smlHeatLot,
						smlClosed = eRPShipmentLineInformationDto.smlClosed,
						smlInvoicedComplete = eRPShipmentLineInformationDto.smlInvoicedComplete,
						smlKitPart = eRPShipmentLineInformationDto.smlKitPart,
						smlOverridePrice = eRPShipmentLineInformationDto.smlOverridePrice,
						smlPostedToGl = eRPShipmentLineInformationDto.smlPostedToGl,
						smlRequiresInspection = eRPShipmentLineInformationDto.smlRequiresInspection,
						smlReversed = eRPShipmentLineInformationDto.smlReversed,
						smlShippedComplete = eRPShipmentLineInformationDto.smlShippedComplete,
						smlJobID = eRPShipmentLineInformationDto.smlJobID,
						smlJobQuantityShipped = eRPShipmentLineInformationDto.smlJobQuantityShipped,
						smlOrgPartID = eRPShipmentLineInformationDto.smlOrgPartID,
						smlOrgPartShortDescription = eRPShipmentLineInformationDto.smlOrgPartShortDescription,
						smlPartBinID = eRPShipmentLineInformationDto.smlPartBinID,
						smlPartGroupID = eRPShipmentLineInformationDto.smlPartGroupID,
						smlPartID = eRPShipmentLineInformationDto.smlPartID,
						smlPartLongDescriptionRtf = eRPShipmentLineInformationDto.smlPartLongDescriptionRtf,
						smlPartLongDescriptionText = eRPShipmentLineInformationDto.smlPartLongDescriptionText,
						smlPartRevisionID = eRPShipmentLineInformationDto.smlPartRevisionID,
						smlPartWarehouseLocationID = eRPShipmentLineInformationDto.smlPartWarehouseLocationID,
						smlProjectAreaID = eRPShipmentLineInformationDto.smlProjectAreaID,
						smlProjectID = eRPShipmentLineInformationDto.smlProjectID,
						smlQuantityShipped = eRPShipmentLineInformationDto.smlQuantityShipped,
						smlReverseShipmentID = eRPShipmentLineInformationDto.smlReverseShipmentID,
						smlReverseShipmentLineID = eRPShipmentLineInformationDto.smlReverseShipmentLineID,
						smlRowVersion = eRPShipmentLineInformationDto.smlRowVersion,
						smlSalesOrderDeliveryID = eRPShipmentLineInformationDto.smlSalesOrderDeliveryID,
						smlSalesOrderID = eRPShipmentLineInformationDto.smlSalesOrderID,
						smlSalesOrderLineID = eRPShipmentLineInformationDto.smlSalesOrderLineID,
						smlShipmentLineID = eRPShipmentLineInformationDto.smlShipmentLineID,
						smlShipmentID = eRPShipmentLineInformationDto.smlShipmentID,
						smlShipmentIDNumber = eRPShipmentLineInformationDto.smlShipmentIDNumber,
						smlSODeliveryQuantity = eRPShipmentLineInformationDto.smlSODeliveryQuantity,
						smlSOOpenQuantity = eRPShipmentLineInformationDto.smlSOOpenQuantity,
						smlSourceTableName = eRPShipmentLineInformationDto.smlSourceTableName,
						smlSourceTableUniqueID = eRPShipmentLineInformationDto.smlSourceTableUniqueID,
						smlUnitOfMeasure = eRPShipmentLineInformationDto.smlUnitOfMeasure,
						smlUnitPrice = eRPShipmentLineInformationDto.smlUnitPrice,
						smlUnitPriceForeign = eRPShipmentLineInformationDto.smlUnitPriceForeign,
						smlWeight = eRPShipmentLineInformationDto.smlWeight,
						smlWeightUnitOfMeasure = eRPShipmentLineInformationDto.smlWeightUnitOfMeasure,
						CustomFields = eRPShipmentLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ShipmentLine [{shipmentLine.smlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteShipmentLine(Guid shipmentLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentLineRepository iERPShipmentLineRepository = (base.ERPShipmentLineRepository = new ERPShipmentLineRepository(base.ApiClientContext));
		using (iERPShipmentLineRepository)
		{
			if (!(await base.ERPShipmentLineRepository.DoesShipmentLineExist(shipmentLineId)))
			{
				base.ErrorsList.Add($"ShipmentLine [{shipmentLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPShipmentLineInformationDto eRPShipmentLineInformationDto = await base.ERPShipmentLineRepository.GetShipmentLine(shipmentLineId);
				string text = await base.ERPShipmentLineRepository.WhereUsed("ShipmentLines", new object[2] { eRPShipmentLineInformationDto.smlShipmentID, eRPShipmentLineInformationDto.smlShipmentLineID }, new object[2] { "smlShipmentID", "smlShipmentLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ShipmentLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPShipmentLineDto>> Process_DeleteShipmentLine(Guid shipmentLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPShipmentLineDto> result;
		try
		{
			IERPShipmentLineRepository iERPShipmentLineRepository = (base.ERPShipmentLineRepository = new ERPShipmentLineRepository(base.ApiClientContext));
			using (iERPShipmentLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPShipmentLineRepository.DeleteRowFromTable("ShipmentLines", "sml", shipmentLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ShipmentLine [{shipmentLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPShipmentLineDto()
			};
		}
		return result;
	}
}
