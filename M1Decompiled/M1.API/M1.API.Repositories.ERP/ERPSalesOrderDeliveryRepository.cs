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

public class ERPSalesOrderDeliveryRepository : APIBaseRepository, IERPSalesOrderDeliveryRepository, IAPIBaseRepository, IDisposable
{
	public ERPSalesOrderDeliveryRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSalesOrderDeliveryExist(Guid salesOrderDeliveryId)
	{
		InitializeParameterLists();
		base.filterList.Add("omdUniqueID|C", salesOrderDeliveryId);
		base.selectList.Add("omdUniqueID");
		return Task.FromResult(GetAsObject("SalesOrderDeliveries", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSalesOrderDeliveryInformationDto>> GetAllSalesOrderDeliveries(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSalesOrderDeliveryInformationDto> collection = new List<ERPSalesOrderDeliveryInformationDto>();
		InitializeParameterLists();
		string[] array = new string[44]
		{
			"omdAmountToInvoice", "omdAmountToInvoiceForeign", "omdAvalaraNonTaxReasonID", "omdCreatedBy", "omdCreatedDate", "omdCustomerOrganizationID", "omdDeliveryDate", "omdDeliveryQuantity", "omdDeliveryType", "omdUniqueID",
			"omdExtendedWeight", "omdFreightAmountBase", "omdFreightAmountForeign", "omdClosed", "omdDifferentLocation", "omdFirm", "omdInvoicedComplete", "omdKitPart", "omdPickInProgress", "omdReceivedComplete",
			"omdRequiresInspection", "omdShippedComplete", "omdPartBinID", "omdPartID", "omdPartRevisionID", "omdPartWarehouseLocationID", "omdPurchaseLocationID", "omdPurchaseUnitCostBase", "omdPurchaseUnitCostForeign", "omdQuantityAllocated",
			"omdQuantityInvoiced", "omdQuantityOnOrder", "omdQuantityReceived", "omdQuantityShipped", "omdRowVersion", "omdSalesOrderID", "omdSalesOrderLineID", "omdSalesOrderDeliveryID", "omdShipContactID", "omdShipLocationID",
			"omdShippingMethodID", "omdShippingPaymentTypeID", "omdSupplierOrganizationID", "omdWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SalesOrderDeliveries");
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
		using (DataTable dataTable = GetAsDataTable("SalesOrderDeliveries", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSalesOrderDeliveryInformationDto eRPSalesOrderDeliveryInformationDto = new ERPSalesOrderDeliveryInformationDto();
				eRPSalesOrderDeliveryInformationDto.omdAmountToInvoice = dataTable.Rows[i].Field<decimal>("omdAmountToInvoice");
				eRPSalesOrderDeliveryInformationDto.omdAmountToInvoiceForeign = dataTable.Rows[i].Field<decimal>("omdAmountToInvoiceForeign");
				eRPSalesOrderDeliveryInformationDto.omdAvalaraNonTaxReasonID = dataTable.Rows[i].Field<string>("omdAvalaraNonTaxReasonID");
				eRPSalesOrderDeliveryInformationDto.omdCreatedBy = dataTable.Rows[i].Field<string>("omdCreatedBy");
				eRPSalesOrderDeliveryInformationDto.omdCreatedDate = dataTable.Rows[i].Field<DateTime?>("omdCreatedDate");
				eRPSalesOrderDeliveryInformationDto.omdCustomerOrganizationID = dataTable.Rows[i].Field<string>("omdCustomerOrganizationID");
				eRPSalesOrderDeliveryInformationDto.omdDeliveryDate = dataTable.Rows[i].Field<DateTime?>("omdDeliveryDate");
				eRPSalesOrderDeliveryInformationDto.omdDeliveryQuantity = dataTable.Rows[i].Field<decimal>("omdDeliveryQuantity");
				eRPSalesOrderDeliveryInformationDto.omdDeliveryType = dataTable.Rows[i].Field<byte>("omdDeliveryType");
				eRPSalesOrderDeliveryInformationDto.omdUniqueID = dataTable.Rows[i].Field<Guid>("omdUniqueID");
				eRPSalesOrderDeliveryInformationDto.omdExtendedWeight = dataTable.Rows[i].Field<decimal>("omdExtendedWeight");
				eRPSalesOrderDeliveryInformationDto.omdFreightAmountBase = dataTable.Rows[i].Field<decimal>("omdFreightAmountBase");
				eRPSalesOrderDeliveryInformationDto.omdFreightAmountForeign = dataTable.Rows[i].Field<decimal>("omdFreightAmountForeign");
				eRPSalesOrderDeliveryInformationDto.omdClosed = dataTable.Rows[i].Field<bool>("omdClosed");
				eRPSalesOrderDeliveryInformationDto.omdDifferentLocation = dataTable.Rows[i].Field<bool>("omdDifferentLocation");
				eRPSalesOrderDeliveryInformationDto.omdFirm = dataTable.Rows[i].Field<bool>("omdFirm");
				eRPSalesOrderDeliveryInformationDto.omdInvoicedComplete = dataTable.Rows[i].Field<bool>("omdInvoicedComplete");
				eRPSalesOrderDeliveryInformationDto.omdKitPart = dataTable.Rows[i].Field<bool>("omdKitPart");
				eRPSalesOrderDeliveryInformationDto.omdPickInProgress = dataTable.Rows[i].Field<bool>("omdPickInProgress");
				eRPSalesOrderDeliveryInformationDto.omdReceivedComplete = dataTable.Rows[i].Field<bool>("omdReceivedComplete");
				eRPSalesOrderDeliveryInformationDto.omdRequiresInspection = dataTable.Rows[i].Field<bool>("omdRequiresInspection");
				eRPSalesOrderDeliveryInformationDto.omdShippedComplete = dataTable.Rows[i].Field<bool>("omdShippedComplete");
				eRPSalesOrderDeliveryInformationDto.omdPartBinID = dataTable.Rows[i].Field<string>("omdPartBinID");
				eRPSalesOrderDeliveryInformationDto.omdPartID = dataTable.Rows[i].Field<string>("omdPartID");
				eRPSalesOrderDeliveryInformationDto.omdPartRevisionID = dataTable.Rows[i].Field<string>("omdPartRevisionID");
				eRPSalesOrderDeliveryInformationDto.omdPartWarehouseLocationID = dataTable.Rows[i].Field<string>("omdPartWarehouseLocationID");
				eRPSalesOrderDeliveryInformationDto.omdPurchaseLocationID = dataTable.Rows[i].Field<string>("omdPurchaseLocationID");
				eRPSalesOrderDeliveryInformationDto.omdPurchaseUnitCostBase = dataTable.Rows[i].Field<decimal>("omdPurchaseUnitCostBase");
				eRPSalesOrderDeliveryInformationDto.omdPurchaseUnitCostForeign = dataTable.Rows[i].Field<decimal>("omdPurchaseUnitCostForeign");
				eRPSalesOrderDeliveryInformationDto.omdQuantityAllocated = dataTable.Rows[i].Field<decimal>("omdQuantityAllocated");
				eRPSalesOrderDeliveryInformationDto.omdQuantityInvoiced = dataTable.Rows[i].Field<decimal>("omdQuantityInvoiced");
				eRPSalesOrderDeliveryInformationDto.omdQuantityOnOrder = dataTable.Rows[i].Field<decimal>("omdQuantityOnOrder");
				eRPSalesOrderDeliveryInformationDto.omdQuantityReceived = dataTable.Rows[i].Field<decimal>("omdQuantityReceived");
				eRPSalesOrderDeliveryInformationDto.omdQuantityShipped = dataTable.Rows[i].Field<decimal>("omdQuantityShipped");
				eRPSalesOrderDeliveryInformationDto.omdRowVersion = dataTable.Rows[i].Field<byte[]>("omdRowVersion");
				eRPSalesOrderDeliveryInformationDto.omdSalesOrderID = dataTable.Rows[i].Field<string>("omdSalesOrderID");
				eRPSalesOrderDeliveryInformationDto.omdSalesOrderLineID = dataTable.Rows[i].Field<short>("omdSalesOrderLineID");
				eRPSalesOrderDeliveryInformationDto.omdSalesOrderDeliveryID = dataTable.Rows[i].Field<short>("omdSalesOrderDeliveryID");
				eRPSalesOrderDeliveryInformationDto.omdShipContactID = dataTable.Rows[i].Field<string>("omdShipContactID");
				eRPSalesOrderDeliveryInformationDto.omdShipLocationID = dataTable.Rows[i].Field<string>("omdShipLocationID");
				eRPSalesOrderDeliveryInformationDto.omdShippingMethodID = dataTable.Rows[i].Field<string>("omdShippingMethodID");
				eRPSalesOrderDeliveryInformationDto.omdShippingPaymentTypeID = dataTable.Rows[i].Field<string>("omdShippingPaymentTypeID");
				eRPSalesOrderDeliveryInformationDto.omdSupplierOrganizationID = dataTable.Rows[i].Field<string>("omdSupplierOrganizationID");
				eRPSalesOrderDeliveryInformationDto.omdWeight = dataTable.Rows[i].Field<decimal>("omdWeight");
				eRPSalesOrderDeliveryInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSalesOrderDeliveryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSalesOrderDeliveryInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSalesOrderDeliveryInformationDto> GetSalesOrderDelivery(Guid salesOrderDeliveryId)
	{
		ERPSalesOrderDeliveryInformationDto eRPSalesOrderDeliveryInformationDto = new ERPSalesOrderDeliveryInformationDto();
		InitializeParameterLists();
		string[] collection = new string[44]
		{
			"omdAmountToInvoice", "omdAmountToInvoiceForeign", "omdAvalaraNonTaxReasonID", "omdCreatedBy", "omdCreatedDate", "omdCustomerOrganizationID", "omdDeliveryDate", "omdDeliveryQuantity", "omdDeliveryType", "omdUniqueID",
			"omdExtendedWeight", "omdFreightAmountBase", "omdFreightAmountForeign", "omdClosed", "omdDifferentLocation", "omdFirm", "omdInvoicedComplete", "omdKitPart", "omdPickInProgress", "omdReceivedComplete",
			"omdRequiresInspection", "omdShippedComplete", "omdPartBinID", "omdPartID", "omdPartRevisionID", "omdPartWarehouseLocationID", "omdPurchaseLocationID", "omdPurchaseUnitCostBase", "omdPurchaseUnitCostForeign", "omdQuantityAllocated",
			"omdQuantityInvoiced", "omdQuantityOnOrder", "omdQuantityReceived", "omdQuantityShipped", "omdRowVersion", "omdSalesOrderID", "omdSalesOrderLineID", "omdSalesOrderDeliveryID", "omdShipContactID", "omdShipLocationID",
			"omdShippingMethodID", "omdShippingPaymentTypeID", "omdSupplierOrganizationID", "omdWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("omdUniqueID|C", salesOrderDeliveryId);
		AddCustomFieldsToSelectList("SalesOrderDeliveries");
		using (DataTable dataTable = GetAsDataTable("SalesOrderDeliveries", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSalesOrderDeliveryInformationDto);
			}
			eRPSalesOrderDeliveryInformationDto.omdAmountToInvoice = dataTable.Rows[0].Field<decimal>("omdAmountToInvoice");
			eRPSalesOrderDeliveryInformationDto.omdAmountToInvoiceForeign = dataTable.Rows[0].Field<decimal>("omdAmountToInvoiceForeign");
			eRPSalesOrderDeliveryInformationDto.omdAvalaraNonTaxReasonID = dataTable.Rows[0].Field<string>("omdAvalaraNonTaxReasonID");
			eRPSalesOrderDeliveryInformationDto.omdCreatedBy = dataTable.Rows[0].Field<string>("omdCreatedBy");
			eRPSalesOrderDeliveryInformationDto.omdCreatedDate = dataTable.Rows[0].Field<DateTime?>("omdCreatedDate");
			eRPSalesOrderDeliveryInformationDto.omdCustomerOrganizationID = dataTable.Rows[0].Field<string>("omdCustomerOrganizationID");
			eRPSalesOrderDeliveryInformationDto.omdDeliveryDate = dataTable.Rows[0].Field<DateTime?>("omdDeliveryDate");
			eRPSalesOrderDeliveryInformationDto.omdDeliveryQuantity = dataTable.Rows[0].Field<decimal>("omdDeliveryQuantity");
			eRPSalesOrderDeliveryInformationDto.omdDeliveryType = dataTable.Rows[0].Field<byte>("omdDeliveryType");
			eRPSalesOrderDeliveryInformationDto.omdUniqueID = dataTable.Rows[0].Field<Guid>("omdUniqueID");
			eRPSalesOrderDeliveryInformationDto.omdExtendedWeight = dataTable.Rows[0].Field<decimal>("omdExtendedWeight");
			eRPSalesOrderDeliveryInformationDto.omdFreightAmountBase = dataTable.Rows[0].Field<decimal>("omdFreightAmountBase");
			eRPSalesOrderDeliveryInformationDto.omdFreightAmountForeign = dataTable.Rows[0].Field<decimal>("omdFreightAmountForeign");
			eRPSalesOrderDeliveryInformationDto.omdClosed = dataTable.Rows[0].Field<bool>("omdClosed");
			eRPSalesOrderDeliveryInformationDto.omdDifferentLocation = dataTable.Rows[0].Field<bool>("omdDifferentLocation");
			eRPSalesOrderDeliveryInformationDto.omdFirm = dataTable.Rows[0].Field<bool>("omdFirm");
			eRPSalesOrderDeliveryInformationDto.omdInvoicedComplete = dataTable.Rows[0].Field<bool>("omdInvoicedComplete");
			eRPSalesOrderDeliveryInformationDto.omdKitPart = dataTable.Rows[0].Field<bool>("omdKitPart");
			eRPSalesOrderDeliveryInformationDto.omdPickInProgress = dataTable.Rows[0].Field<bool>("omdPickInProgress");
			eRPSalesOrderDeliveryInformationDto.omdReceivedComplete = dataTable.Rows[0].Field<bool>("omdReceivedComplete");
			eRPSalesOrderDeliveryInformationDto.omdRequiresInspection = dataTable.Rows[0].Field<bool>("omdRequiresInspection");
			eRPSalesOrderDeliveryInformationDto.omdShippedComplete = dataTable.Rows[0].Field<bool>("omdShippedComplete");
			eRPSalesOrderDeliveryInformationDto.omdPartBinID = dataTable.Rows[0].Field<string>("omdPartBinID");
			eRPSalesOrderDeliveryInformationDto.omdPartID = dataTable.Rows[0].Field<string>("omdPartID");
			eRPSalesOrderDeliveryInformationDto.omdPartRevisionID = dataTable.Rows[0].Field<string>("omdPartRevisionID");
			eRPSalesOrderDeliveryInformationDto.omdPartWarehouseLocationID = dataTable.Rows[0].Field<string>("omdPartWarehouseLocationID");
			eRPSalesOrderDeliveryInformationDto.omdPurchaseLocationID = dataTable.Rows[0].Field<string>("omdPurchaseLocationID");
			eRPSalesOrderDeliveryInformationDto.omdPurchaseUnitCostBase = dataTable.Rows[0].Field<decimal>("omdPurchaseUnitCostBase");
			eRPSalesOrderDeliveryInformationDto.omdPurchaseUnitCostForeign = dataTable.Rows[0].Field<decimal>("omdPurchaseUnitCostForeign");
			eRPSalesOrderDeliveryInformationDto.omdQuantityAllocated = dataTable.Rows[0].Field<decimal>("omdQuantityAllocated");
			eRPSalesOrderDeliveryInformationDto.omdQuantityInvoiced = dataTable.Rows[0].Field<decimal>("omdQuantityInvoiced");
			eRPSalesOrderDeliveryInformationDto.omdQuantityOnOrder = dataTable.Rows[0].Field<decimal>("omdQuantityOnOrder");
			eRPSalesOrderDeliveryInformationDto.omdQuantityReceived = dataTable.Rows[0].Field<decimal>("omdQuantityReceived");
			eRPSalesOrderDeliveryInformationDto.omdQuantityShipped = dataTable.Rows[0].Field<decimal>("omdQuantityShipped");
			eRPSalesOrderDeliveryInformationDto.omdRowVersion = dataTable.Rows[0].Field<byte[]>("omdRowVersion");
			eRPSalesOrderDeliveryInformationDto.omdSalesOrderID = dataTable.Rows[0].Field<string>("omdSalesOrderID");
			eRPSalesOrderDeliveryInformationDto.omdSalesOrderLineID = dataTable.Rows[0].Field<short>("omdSalesOrderLineID");
			eRPSalesOrderDeliveryInformationDto.omdSalesOrderDeliveryID = dataTable.Rows[0].Field<short>("omdSalesOrderDeliveryID");
			eRPSalesOrderDeliveryInformationDto.omdShipContactID = dataTable.Rows[0].Field<string>("omdShipContactID");
			eRPSalesOrderDeliveryInformationDto.omdShipLocationID = dataTable.Rows[0].Field<string>("omdShipLocationID");
			eRPSalesOrderDeliveryInformationDto.omdShippingMethodID = dataTable.Rows[0].Field<string>("omdShippingMethodID");
			eRPSalesOrderDeliveryInformationDto.omdShippingPaymentTypeID = dataTable.Rows[0].Field<string>("omdShippingPaymentTypeID");
			eRPSalesOrderDeliveryInformationDto.omdSupplierOrganizationID = dataTable.Rows[0].Field<string>("omdSupplierOrganizationID");
			eRPSalesOrderDeliveryInformationDto.omdWeight = dataTable.Rows[0].Field<decimal>("omdWeight");
			eRPSalesOrderDeliveryInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSalesOrderDeliveryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSalesOrderDeliveryInformationDto);
	}

	public Task<APIValidationInfoDto> SaveSalesOrderDelivery(ERPSalesOrderDeliveryDto salesOrderDelivery)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM SalesOrderDeliveries WHERE omdUniqueID = " + M1Util.ConvertToLinq(salesOrderDelivery.omdUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["omdSalesOrderID"] = salesOrderDelivery.omdSalesOrderID.ToUpper();
				dataRow["omdSalesOrderLineID"] = salesOrderDelivery.omdSalesOrderLineID;
				dataRow["omdSalesOrderDeliveryID"] = salesOrderDelivery.omdSalesOrderDeliveryID;
				salesOrderDelivery.omdUniqueID = ((salesOrderDelivery.omdUniqueID == Guid.Empty) ? Guid.NewGuid() : salesOrderDelivery.omdUniqueID);
				dataRow["omdUniqueID"] = salesOrderDelivery.omdUniqueID;
				dataRow["omdCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["omdCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The SalesOrderDelivery could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (salesOrderDelivery.omdRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the SalesOrderDelivery is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["omdRowVersion"], salesOrderDelivery.omdRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the SalesOrderDelivery has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the SalesOrderDelivery again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["omdAmountToInvoice"] = salesOrderDelivery.omdAmountToInvoice;
			dataRow["omdAmountToInvoiceForeign"] = salesOrderDelivery.omdAmountToInvoiceForeign;
			dataRow["omdAvalaraNonTaxReasonID"] = salesOrderDelivery.omdAvalaraNonTaxReasonID;
			dataRow["omdCustomerOrganizationID"] = salesOrderDelivery.omdCustomerOrganizationID;
			DataRow dataRow2 = dataRow;
			DateTime? omdDeliveryDate = salesOrderDelivery.omdDeliveryDate;
			dataRow2["omdDeliveryDate"] = (omdDeliveryDate.HasValue ? ((object)omdDeliveryDate.GetValueOrDefault()) : dataRow["omdDeliveryDate"]);
			dataRow["omdDeliveryQuantity"] = salesOrderDelivery.omdDeliveryQuantity;
			dataRow["omdDeliveryType"] = salesOrderDelivery.omdDeliveryType;
			dataRow["omdExtendedWeight"] = salesOrderDelivery.omdExtendedWeight;
			dataRow["omdFreightAmountBase"] = salesOrderDelivery.omdFreightAmountBase;
			dataRow["omdFreightAmountForeign"] = salesOrderDelivery.omdFreightAmountForeign;
			dataRow["omdClosed"] = salesOrderDelivery.omdClosed;
			dataRow["omdDifferentLocation"] = salesOrderDelivery.omdDifferentLocation;
			dataRow["omdFirm"] = salesOrderDelivery.omdFirm;
			dataRow["omdInvoicedComplete"] = salesOrderDelivery.omdInvoicedComplete;
			dataRow["omdKitPart"] = salesOrderDelivery.omdKitPart;
			dataRow["omdPickInProgress"] = salesOrderDelivery.omdPickInProgress;
			dataRow["omdReceivedComplete"] = salesOrderDelivery.omdReceivedComplete;
			dataRow["omdRequiresInspection"] = salesOrderDelivery.omdRequiresInspection;
			dataRow["omdShippedComplete"] = salesOrderDelivery.omdShippedComplete;
			dataRow["omdPartBinID"] = salesOrderDelivery.omdPartBinID;
			dataRow["omdPartID"] = salesOrderDelivery.omdPartID;
			dataRow["omdPartRevisionID"] = salesOrderDelivery.omdPartRevisionID;
			dataRow["omdPartWarehouseLocationID"] = salesOrderDelivery.omdPartWarehouseLocationID;
			dataRow["omdPurchaseLocationID"] = salesOrderDelivery.omdPurchaseLocationID;
			dataRow["omdPurchaseUnitCostBase"] = salesOrderDelivery.omdPurchaseUnitCostBase;
			dataRow["omdPurchaseUnitCostForeign"] = salesOrderDelivery.omdPurchaseUnitCostForeign;
			dataRow["omdQuantityAllocated"] = salesOrderDelivery.omdQuantityAllocated;
			dataRow["omdQuantityInvoiced"] = salesOrderDelivery.omdQuantityInvoiced;
			dataRow["omdQuantityOnOrder"] = salesOrderDelivery.omdQuantityOnOrder;
			dataRow["omdQuantityReceived"] = salesOrderDelivery.omdQuantityReceived;
			dataRow["omdQuantityShipped"] = salesOrderDelivery.omdQuantityShipped;
			dataRow["omdShipContactID"] = salesOrderDelivery.omdShipContactID;
			dataRow["omdShipLocationID"] = salesOrderDelivery.omdShipLocationID;
			dataRow["omdShippingMethodID"] = salesOrderDelivery.omdShippingMethodID;
			dataRow["omdShippingPaymentTypeID"] = salesOrderDelivery.omdShippingPaymentTypeID;
			dataRow["omdSupplierOrganizationID"] = salesOrderDelivery.omdSupplierOrganizationID;
			dataRow["omdWeight"] = salesOrderDelivery.omdWeight;
			if (salesOrderDelivery.CustomFields != null && salesOrderDelivery.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in salesOrderDelivery.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the SalesOrderDelivery [{salesOrderDelivery.omdUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the SalesOrderDelivery [{salesOrderDelivery.omdUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
