using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Utilities;
using M1.Extensions;

namespace M1.API.Repositories.ERP;

public class ERPShipmentLineRepository : APIBaseRepository, IERPShipmentLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPShipmentLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesShipmentLineExist(Guid shipmentLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("smlUniqueID|C", shipmentLineId);
		base.selectList.Add("smlUniqueID");
		return Task.FromResult(GetAsObject("ShipmentLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPShipmentLineInformationDto>> GetAllShipmentLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPShipmentLineInformationDto> collection = new List<ERPShipmentLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[50]
		{
			"smlCreatedBy", "smlCreatedDate", "smlDescription", "smlUniqueID", "smlExtendedPriceBase", "smlExtendedPriceForeign", "smlExtendedWeight", "smlFreightAmount", "smlFreightAmountForeign", "smlHeatLot",
			"smlClosed", "smlInvoicedComplete", "smlKitPart", "smlOverridePrice", "smlPostedToGl", "smlRequiresInspection", "smlReversed", "smlShippedComplete", "smlJobID", "smlJobQuantityShipped",
			"smlOrgPartID", "smlOrgPartShortDescription", "smlPartBinID", "smlPartGroupID", "smlPartID", "smlPartLongDescriptionRtf", "smlPartLongDescriptionText", "smlPartRevisionID", "smlPartWarehouseLocationID", "smlProjectAreaID",
			"smlProjectID", "smlQuantityShipped", "smlReverseShipmentID", "smlReverseShipmentLineID", "smlRowVersion", "smlSalesOrderDeliveryID", "smlSalesOrderID", "smlSalesOrderLineID", "smlShipmentLineID", "smlShipmentID",
			"smlShipmentIDNumber", "smlSODeliveryQuantity", "smlSOOpenQuantity", "smlSourceTableName", "smlSourceTableUniqueID", "smlUnitOfMeasure", "smlUnitPrice", "smlUnitPriceForeign", "smlWeight", "smlWeightUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ShipmentLines");
		List<string> list = new List<string>();
		string[] fields = ((base.selectList.Count != array.Count()) ? base.selectList.ToArray() : array);
		if (orderBy != null && orderBy.Length > 0)
		{
			ParseAndAddOrderByFields(orderBy, list, fields);
		}
		if (list.Count == 0)
		{
			list = new List<string> { "1" };
		}
		if (filter != null && filter.Length != 0)
		{
			ParseAndAddFilter(filter, base.filterList, fields);
		}
		using (DataTable dataTable = GetAsDataTable("ShipmentLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPShipmentLineInformationDto eRPShipmentLineInformationDto = new ERPShipmentLineInformationDto();
				eRPShipmentLineInformationDto.smlCreatedBy = dataTable.Rows[i].Field<string>("smlCreatedBy");
				eRPShipmentLineInformationDto.smlCreatedDate = dataTable.Rows[i].Field<DateTime?>("smlCreatedDate");
				eRPShipmentLineInformationDto.smlDescription = dataTable.Rows[i].Field<string>("smlDescription");
				eRPShipmentLineInformationDto.smlUniqueID = dataTable.Rows[i].Field<Guid>("smlUniqueID");
				eRPShipmentLineInformationDto.smlExtendedPriceBase = dataTable.Rows[i].Field<decimal>("smlExtendedPriceBase");
				eRPShipmentLineInformationDto.smlExtendedPriceForeign = dataTable.Rows[i].Field<decimal>("smlExtendedPriceForeign");
				eRPShipmentLineInformationDto.smlExtendedWeight = dataTable.Rows[i].Field<decimal>("smlExtendedWeight");
				eRPShipmentLineInformationDto.smlFreightAmount = dataTable.Rows[i].Field<decimal>("smlFreightAmount");
				eRPShipmentLineInformationDto.smlFreightAmountForeign = dataTable.Rows[i].Field<decimal>("smlFreightAmountForeign");
				eRPShipmentLineInformationDto.smlHeatLot = dataTable.Rows[i].Field<string>("smlHeatLot");
				eRPShipmentLineInformationDto.smlClosed = dataTable.Rows[i].Field<bool>("smlClosed");
				eRPShipmentLineInformationDto.smlInvoicedComplete = dataTable.Rows[i].Field<bool>("smlInvoicedComplete");
				eRPShipmentLineInformationDto.smlKitPart = dataTable.Rows[i].Field<bool>("smlKitPart");
				eRPShipmentLineInformationDto.smlOverridePrice = dataTable.Rows[i].Field<bool>("smlOverridePrice");
				eRPShipmentLineInformationDto.smlPostedToGl = dataTable.Rows[i].Field<bool>("smlPostedToGl");
				eRPShipmentLineInformationDto.smlRequiresInspection = dataTable.Rows[i].Field<bool>("smlRequiresInspection");
				eRPShipmentLineInformationDto.smlReversed = dataTable.Rows[i].Field<bool>("smlReversed");
				eRPShipmentLineInformationDto.smlShippedComplete = dataTable.Rows[i].Field<bool>("smlShippedComplete");
				eRPShipmentLineInformationDto.smlJobID = dataTable.Rows[i].Field<string>("smlJobID");
				eRPShipmentLineInformationDto.smlJobQuantityShipped = dataTable.Rows[i].Field<decimal>("smlJobQuantityShipped");
				eRPShipmentLineInformationDto.smlOrgPartID = dataTable.Rows[i].Field<string>("smlOrgPartID");
				eRPShipmentLineInformationDto.smlOrgPartShortDescription = dataTable.Rows[i].Field<string>("smlOrgPartShortDescription");
				eRPShipmentLineInformationDto.smlPartBinID = dataTable.Rows[i].Field<string>("smlPartBinID");
				eRPShipmentLineInformationDto.smlPartGroupID = dataTable.Rows[i].Field<string>("smlPartGroupID");
				eRPShipmentLineInformationDto.smlPartID = dataTable.Rows[i].Field<string>("smlPartID");
				eRPShipmentLineInformationDto.smlPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("smlPartLongDescriptionRtf");
				eRPShipmentLineInformationDto.smlPartLongDescriptionText = dataTable.Rows[i].Field<string>("smlPartLongDescriptionText");
				eRPShipmentLineInformationDto.smlPartRevisionID = dataTable.Rows[i].Field<string>("smlPartRevisionID");
				eRPShipmentLineInformationDto.smlPartWarehouseLocationID = dataTable.Rows[i].Field<string>("smlPartWarehouseLocationID");
				eRPShipmentLineInformationDto.smlProjectAreaID = dataTable.Rows[i].Field<string>("smlProjectAreaID");
				eRPShipmentLineInformationDto.smlProjectID = dataTable.Rows[i].Field<string>("smlProjectID");
				eRPShipmentLineInformationDto.smlQuantityShipped = dataTable.Rows[i].Field<decimal>("smlQuantityShipped");
				eRPShipmentLineInformationDto.smlReverseShipmentID = dataTable.Rows[i].Field<string>("smlReverseShipmentID");
				eRPShipmentLineInformationDto.smlReverseShipmentLineID = dataTable.Rows[i].Field<short>("smlReverseShipmentLineID");
				eRPShipmentLineInformationDto.smlRowVersion = dataTable.Rows[i].Field<byte[]>("smlRowVersion");
				eRPShipmentLineInformationDto.smlSalesOrderDeliveryID = dataTable.Rows[i].Field<short>("smlSalesOrderDeliveryID");
				eRPShipmentLineInformationDto.smlSalesOrderID = dataTable.Rows[i].Field<string>("smlSalesOrderID");
				eRPShipmentLineInformationDto.smlSalesOrderLineID = dataTable.Rows[i].Field<short>("smlSalesOrderLineID");
				eRPShipmentLineInformationDto.smlShipmentLineID = dataTable.Rows[i].Field<short>("smlShipmentLineID");
				eRPShipmentLineInformationDto.smlShipmentID = dataTable.Rows[i].Field<string>("smlShipmentID");
				eRPShipmentLineInformationDto.smlShipmentIDNumber = dataTable.Rows[i].Field<string>("smlShipmentIDNumber");
				eRPShipmentLineInformationDto.smlSODeliveryQuantity = dataTable.Rows[i].Field<decimal>("smlSODeliveryQuantity");
				eRPShipmentLineInformationDto.smlSOOpenQuantity = dataTable.Rows[i].Field<decimal>("smlSOOpenQuantity");
				eRPShipmentLineInformationDto.smlSourceTableName = dataTable.Rows[i].Field<string>("smlSourceTableName");
				eRPShipmentLineInformationDto.smlSourceTableUniqueID = dataTable.Rows[i].Field<Guid>("smlSourceTableUniqueID");
				eRPShipmentLineInformationDto.smlUnitOfMeasure = dataTable.Rows[i].Field<string>("smlUnitOfMeasure");
				eRPShipmentLineInformationDto.smlUnitPrice = dataTable.Rows[i].Field<decimal>("smlUnitPrice");
				eRPShipmentLineInformationDto.smlUnitPriceForeign = dataTable.Rows[i].Field<decimal>("smlUnitPriceForeign");
				eRPShipmentLineInformationDto.smlWeight = dataTable.Rows[i].Field<decimal>("smlWeight");
				eRPShipmentLineInformationDto.smlWeightUnitOfMeasure = dataTable.Rows[i].Field<string>("smlWeightUnitOfMeasure");
				eRPShipmentLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPShipmentLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPShipmentLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPShipmentLineInformationDto> GetShipmentLine(Guid shipmentLineId)
	{
		ERPShipmentLineInformationDto eRPShipmentLineInformationDto = new ERPShipmentLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[50]
		{
			"smlCreatedBy", "smlCreatedDate", "smlDescription", "smlUniqueID", "smlExtendedPriceBase", "smlExtendedPriceForeign", "smlExtendedWeight", "smlFreightAmount", "smlFreightAmountForeign", "smlHeatLot",
			"smlClosed", "smlInvoicedComplete", "smlKitPart", "smlOverridePrice", "smlPostedToGl", "smlRequiresInspection", "smlReversed", "smlShippedComplete", "smlJobID", "smlJobQuantityShipped",
			"smlOrgPartID", "smlOrgPartShortDescription", "smlPartBinID", "smlPartGroupID", "smlPartID", "smlPartLongDescriptionRtf", "smlPartLongDescriptionText", "smlPartRevisionID", "smlPartWarehouseLocationID", "smlProjectAreaID",
			"smlProjectID", "smlQuantityShipped", "smlReverseShipmentID", "smlReverseShipmentLineID", "smlRowVersion", "smlSalesOrderDeliveryID", "smlSalesOrderID", "smlSalesOrderLineID", "smlShipmentLineID", "smlShipmentID",
			"smlShipmentIDNumber", "smlSODeliveryQuantity", "smlSOOpenQuantity", "smlSourceTableName", "smlSourceTableUniqueID", "smlUnitOfMeasure", "smlUnitPrice", "smlUnitPriceForeign", "smlWeight", "smlWeightUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("smlUniqueID|C", shipmentLineId);
		AddCustomFieldsToSelectList("ShipmentLines");
		using (DataTable dataTable = GetAsDataTable("ShipmentLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPShipmentLineInformationDto);
			}
			eRPShipmentLineInformationDto.smlCreatedBy = dataTable.Rows[0].Field<string>("smlCreatedBy");
			eRPShipmentLineInformationDto.smlCreatedDate = dataTable.Rows[0].Field<DateTime?>("smlCreatedDate");
			eRPShipmentLineInformationDto.smlDescription = dataTable.Rows[0].Field<string>("smlDescription");
			eRPShipmentLineInformationDto.smlUniqueID = dataTable.Rows[0].Field<Guid>("smlUniqueID");
			eRPShipmentLineInformationDto.smlExtendedPriceBase = dataTable.Rows[0].Field<decimal>("smlExtendedPriceBase");
			eRPShipmentLineInformationDto.smlExtendedPriceForeign = dataTable.Rows[0].Field<decimal>("smlExtendedPriceForeign");
			eRPShipmentLineInformationDto.smlExtendedWeight = dataTable.Rows[0].Field<decimal>("smlExtendedWeight");
			eRPShipmentLineInformationDto.smlFreightAmount = dataTable.Rows[0].Field<decimal>("smlFreightAmount");
			eRPShipmentLineInformationDto.smlFreightAmountForeign = dataTable.Rows[0].Field<decimal>("smlFreightAmountForeign");
			eRPShipmentLineInformationDto.smlHeatLot = dataTable.Rows[0].Field<string>("smlHeatLot");
			eRPShipmentLineInformationDto.smlClosed = dataTable.Rows[0].Field<bool>("smlClosed");
			eRPShipmentLineInformationDto.smlInvoicedComplete = dataTable.Rows[0].Field<bool>("smlInvoicedComplete");
			eRPShipmentLineInformationDto.smlKitPart = dataTable.Rows[0].Field<bool>("smlKitPart");
			eRPShipmentLineInformationDto.smlOverridePrice = dataTable.Rows[0].Field<bool>("smlOverridePrice");
			eRPShipmentLineInformationDto.smlPostedToGl = dataTable.Rows[0].Field<bool>("smlPostedToGl");
			eRPShipmentLineInformationDto.smlRequiresInspection = dataTable.Rows[0].Field<bool>("smlRequiresInspection");
			eRPShipmentLineInformationDto.smlReversed = dataTable.Rows[0].Field<bool>("smlReversed");
			eRPShipmentLineInformationDto.smlShippedComplete = dataTable.Rows[0].Field<bool>("smlShippedComplete");
			eRPShipmentLineInformationDto.smlJobID = dataTable.Rows[0].Field<string>("smlJobID");
			eRPShipmentLineInformationDto.smlJobQuantityShipped = dataTable.Rows[0].Field<decimal>("smlJobQuantityShipped");
			eRPShipmentLineInformationDto.smlOrgPartID = dataTable.Rows[0].Field<string>("smlOrgPartID");
			eRPShipmentLineInformationDto.smlOrgPartShortDescription = dataTable.Rows[0].Field<string>("smlOrgPartShortDescription");
			eRPShipmentLineInformationDto.smlPartBinID = dataTable.Rows[0].Field<string>("smlPartBinID");
			eRPShipmentLineInformationDto.smlPartGroupID = dataTable.Rows[0].Field<string>("smlPartGroupID");
			eRPShipmentLineInformationDto.smlPartID = dataTable.Rows[0].Field<string>("smlPartID");
			eRPShipmentLineInformationDto.smlPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("smlPartLongDescriptionRtf");
			eRPShipmentLineInformationDto.smlPartLongDescriptionText = dataTable.Rows[0].Field<string>("smlPartLongDescriptionText");
			eRPShipmentLineInformationDto.smlPartRevisionID = dataTable.Rows[0].Field<string>("smlPartRevisionID");
			eRPShipmentLineInformationDto.smlPartWarehouseLocationID = dataTable.Rows[0].Field<string>("smlPartWarehouseLocationID");
			eRPShipmentLineInformationDto.smlProjectAreaID = dataTable.Rows[0].Field<string>("smlProjectAreaID");
			eRPShipmentLineInformationDto.smlProjectID = dataTable.Rows[0].Field<string>("smlProjectID");
			eRPShipmentLineInformationDto.smlQuantityShipped = dataTable.Rows[0].Field<decimal>("smlQuantityShipped");
			eRPShipmentLineInformationDto.smlReverseShipmentID = dataTable.Rows[0].Field<string>("smlReverseShipmentID");
			eRPShipmentLineInformationDto.smlReverseShipmentLineID = dataTable.Rows[0].Field<short>("smlReverseShipmentLineID");
			eRPShipmentLineInformationDto.smlRowVersion = dataTable.Rows[0].Field<byte[]>("smlRowVersion");
			eRPShipmentLineInformationDto.smlSalesOrderDeliveryID = dataTable.Rows[0].Field<short>("smlSalesOrderDeliveryID");
			eRPShipmentLineInformationDto.smlSalesOrderID = dataTable.Rows[0].Field<string>("smlSalesOrderID");
			eRPShipmentLineInformationDto.smlSalesOrderLineID = dataTable.Rows[0].Field<short>("smlSalesOrderLineID");
			eRPShipmentLineInformationDto.smlShipmentLineID = dataTable.Rows[0].Field<short>("smlShipmentLineID");
			eRPShipmentLineInformationDto.smlShipmentID = dataTable.Rows[0].Field<string>("smlShipmentID");
			eRPShipmentLineInformationDto.smlShipmentIDNumber = dataTable.Rows[0].Field<string>("smlShipmentIDNumber");
			eRPShipmentLineInformationDto.smlSODeliveryQuantity = dataTable.Rows[0].Field<decimal>("smlSODeliveryQuantity");
			eRPShipmentLineInformationDto.smlSOOpenQuantity = dataTable.Rows[0].Field<decimal>("smlSOOpenQuantity");
			eRPShipmentLineInformationDto.smlSourceTableName = dataTable.Rows[0].Field<string>("smlSourceTableName");
			eRPShipmentLineInformationDto.smlSourceTableUniqueID = dataTable.Rows[0].Field<Guid>("smlSourceTableUniqueID");
			eRPShipmentLineInformationDto.smlUnitOfMeasure = dataTable.Rows[0].Field<string>("smlUnitOfMeasure");
			eRPShipmentLineInformationDto.smlUnitPrice = dataTable.Rows[0].Field<decimal>("smlUnitPrice");
			eRPShipmentLineInformationDto.smlUnitPriceForeign = dataTable.Rows[0].Field<decimal>("smlUnitPriceForeign");
			eRPShipmentLineInformationDto.smlWeight = dataTable.Rows[0].Field<decimal>("smlWeight");
			eRPShipmentLineInformationDto.smlWeightUnitOfMeasure = dataTable.Rows[0].Field<string>("smlWeightUnitOfMeasure");
			eRPShipmentLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPShipmentLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPShipmentLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveShipmentLine(ERPShipmentLineDto shipmentLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ShipmentLines WHERE smlUniqueID = " + M1Util.ConvertToLinq(shipmentLine.smlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["smlShipmentID"] = shipmentLine.smlShipmentID.ToUpper();
				dataRow["smlShipmentLineID"] = shipmentLine.smlShipmentLineID;
				shipmentLine.smlUniqueID = ((shipmentLine.smlUniqueID == Guid.Empty) ? Guid.NewGuid() : shipmentLine.smlUniqueID);
				dataRow["smlUniqueID"] = shipmentLine.smlUniqueID;
				dataRow["smlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["smlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ShipmentLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (shipmentLine.smlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ShipmentLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["smlRowVersion"], shipmentLine.smlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ShipmentLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ShipmentLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["smlDescription"] = shipmentLine.smlDescription;
			dataRow["smlExtendedPriceBase"] = shipmentLine.smlExtendedPriceBase;
			dataRow["smlExtendedPriceForeign"] = shipmentLine.smlExtendedPriceForeign;
			dataRow["smlExtendedWeight"] = shipmentLine.smlExtendedWeight;
			dataRow["smlFreightAmount"] = shipmentLine.smlFreightAmount;
			dataRow["smlFreightAmountForeign"] = shipmentLine.smlFreightAmountForeign;
			dataRow["smlHeatLot"] = shipmentLine.smlHeatLot;
			dataRow["smlClosed"] = shipmentLine.smlClosed;
			dataRow["smlInvoicedComplete"] = shipmentLine.smlInvoicedComplete;
			dataRow["smlKitPart"] = shipmentLine.smlKitPart;
			dataRow["smlOverridePrice"] = shipmentLine.smlOverridePrice;
			dataRow["smlPostedToGl"] = shipmentLine.smlPostedToGl;
			dataRow["smlRequiresInspection"] = shipmentLine.smlRequiresInspection;
			dataRow["smlReversed"] = shipmentLine.smlReversed;
			dataRow["smlShippedComplete"] = shipmentLine.smlShippedComplete;
			dataRow["smlJobID"] = shipmentLine.smlJobID;
			dataRow["smlJobQuantityShipped"] = shipmentLine.smlJobQuantityShipped;
			dataRow["smlOrgPartID"] = shipmentLine.smlOrgPartID;
			dataRow["smlOrgPartShortDescription"] = shipmentLine.smlOrgPartShortDescription;
			dataRow["smlPartBinID"] = shipmentLine.smlPartBinID;
			dataRow["smlPartGroupID"] = shipmentLine.smlPartGroupID;
			dataRow["smlPartID"] = shipmentLine.smlPartID;
			dataRow["smlPartLongDescriptionRtf"] = shipmentLine.smlPartLongDescriptionRtf ?? dataRow["smlPartLongDescriptionRtf"];
			dataRow["smlPartLongDescriptionText"] = shipmentLine.smlPartLongDescriptionText ?? dataRow["smlPartLongDescriptionText"];
			dataRow["smlPartRevisionID"] = shipmentLine.smlPartRevisionID;
			dataRow["smlPartWarehouseLocationID"] = shipmentLine.smlPartWarehouseLocationID;
			dataRow["smlProjectAreaID"] = shipmentLine.smlProjectAreaID;
			dataRow["smlProjectID"] = shipmentLine.smlProjectID;
			dataRow["smlQuantityShipped"] = shipmentLine.smlQuantityShipped;
			dataRow["smlReverseShipmentID"] = shipmentLine.smlReverseShipmentID;
			dataRow["smlReverseShipmentLineID"] = shipmentLine.smlReverseShipmentLineID;
			dataRow["smlSalesOrderDeliveryID"] = shipmentLine.smlSalesOrderDeliveryID;
			dataRow["smlSalesOrderID"] = shipmentLine.smlSalesOrderID;
			dataRow["smlSalesOrderLineID"] = shipmentLine.smlSalesOrderLineID;
			dataRow["smlShipmentIDNumber"] = shipmentLine.smlShipmentIDNumber;
			dataRow["smlSODeliveryQuantity"] = shipmentLine.smlSODeliveryQuantity;
			dataRow["smlSOOpenQuantity"] = shipmentLine.smlSOOpenQuantity;
			dataRow["smlSourceTableName"] = shipmentLine.smlSourceTableName;
			dataRow["smlSourceTableUniqueID"] = shipmentLine.smlSourceTableUniqueID;
			dataRow["smlUnitOfMeasure"] = shipmentLine.smlUnitOfMeasure;
			dataRow["smlUnitPrice"] = shipmentLine.smlUnitPrice;
			dataRow["smlUnitPriceForeign"] = shipmentLine.smlUnitPriceForeign;
			dataRow["smlWeight"] = shipmentLine.smlWeight;
			dataRow["smlWeightUnitOfMeasure"] = shipmentLine.smlWeightUnitOfMeasure;
			if (shipmentLine.CustomFields != null && shipmentLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in shipmentLine.CustomFields)
				{
					if (dataTable.Columns.Contains(customField.Key))
					{
						dataRow[customField.Key] = customField.Value;
					}
				}
			}
			dataRow.EndEdit();
			if (flag)
			{
				dataTable.Rows.Add(dataRow);
			}
			if (base.M1database.UpdateData(dataTable, adapter))
			{
				if (flag)
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Created;
				}
				else
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.OK;
				}
			}
			else
			{
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.InternalServerError;
			}
		}
		catch (SqlException ex)
		{
			SqlErrorResult httpStatusCodeForSqlException = SqlExceptionMapper.GetHttpStatusCodeForSqlException(ex);
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ShipmentLine [{shipmentLine.smlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ShipmentLine [{shipmentLine.smlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
