using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.Repositories.Core;
using M1.API.Utilities;
using M1.Core;

namespace M1.API.Repositories.EDI;

public class EDIShipmentRepository : ShipmentRepository, IEDIShipmentRepository, IShipmentRepository, IAPIBaseRepository, IDisposable
{
	private readonly string SHIPMENT_CHECK_EDISHIPMENT_FORSHIPMENTID = "SELECT Shipments.smpShipmentID  \r\n                                               FROM Shipments INNER JOIN Organizations ON Shipments.smpCustomerOrganizationID = Organizations.cmoOrganizationID \r\n                                               WHERE ((Shipments.smpShipmentID = @p1) AND Organizations.cmoEDIIntegrated=1)";

	private readonly string SHIPMENTLINES_GET_NONEDISALESORDER_COUNT_FORSHIPMENT = "SELECT DISTINCT COUNT(ShipmentLines.smlShipmentID) AS ShipmentIdCount \r\n                                               FROM ShipmentLines INNER JOIN SalesOrders ON ShipmentLines.smlSalesOrderID = SalesOrders.ompSalesOrderID \r\n                                               WHERE(ShipmentLines.smlShipmentID = @p1) AND (SalesOrders.ompCreatedByEDI = 0)  \r\n                                                AND ((ShipmentLines.smlQuantityShipped > 0) OR (ShipmentLines.smlJobQuantityShipped > 0))";

	private readonly string SHIPMENT_GET_SHIPMENTID_LIST_PENDINGEDITRANSFER_ALLEDIUNMAPPED = "SELECT DISTINCT Shipments.smpShipmentID\r\n                                                FROM            Shipments INNER JOIN\r\n                                                                Organizations ON Shipments.smpCustomerOrganizationID = Organizations.cmoOrganizationID\r\n                                                WHERE\t\t\t(Shipments.smpEDITransferred = 0) AND (Organizations.cmoCustomerStatus = 2) AND (Shipments.smpEDIShipmentReady = 1) \r\n\t\t\t\t                                                AND ( Organizations.cmoEDIIntegrated=1)";

	private readonly string SHIPMENT_GET_SHIPMENTHEADERINFO_FORSHIPMENTID = "SELECT Shipments.smpShipmentID, Shipments.smpShipDate, Shipments.smpCustomerOrganizationID, \r\n                                                Shipments.smpShipLocationID, Shipments.smpShippingMethodID, Shipments.smpTrackingNumber, Shipments.smpShipmentTotal, \r\n                                                Shipments.smpARInvoiceLocationID, Shipments.smpPlantID, Shipments.smpWeightTotal, Shipments.smpShipOrganizationID, \r\n                                                Shipments.smpAdditionalWeight, Shipments.smpCurrencyRateID, Shipments.smpPostedToGL, Shipments.smpShippingCommentsText, \r\n                                                Shipments.smpEDITransferred, Shipments.smpEDITransferredDate,smpARInvoiceContactID, smpShipContactID,\r\n                                                OrganizationLocations.cmlLocationID, OrganizationLocations.cmlEDILocationID \r\n                                                FROM Shipments LEFT OUTER JOIN OrganizationLocations \r\n                                                    ON Shipments.smpCustomerOrganizationID = OrganizationLocations.cmlOrganizationID \r\n                                                    AND Shipments.smpShipLocationID = OrganizationLocations.cmlLocationID \r\n                                                WHERE (Shipments.smpShipmentID = @p1) ";

	private readonly string SHIPMENT_UPDATE_EDIFLAG_FORSHIMENT = "UPDATE Shipments SET smpEDITransferred=@status, smpEDITransferredDate=@updateDate \r\n                                                                       WHERE  (smpShipmentID = @smpShipmentID)";

	public EDIShipmentRepository(APIClientContext clientContext)
		: base(clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public EDIShipmentRepository(M1Database database)
		: base(database)
	{
		base.M1database = database;
	}

	public Task<bool> IsEDIShipment(string shipmentId)
	{
		int num = 0;
		InitializeParameterLists();
		base.filterList.Add("@p1", shipmentId);
		using (DataTable dataTable = GetAsDataTable(SHIPMENT_CHECK_EDISHIPMENT_FORSHIPMENTID, base.filterList, null))
		{
			num = (dataTable?.Rows?.Count).GetValueOrDefault();
		}
		return Task.FromResult(num > 0);
	}

	public Task<bool> DoesNonEDISalesordersExist_ForShipment(string shipmentId)
	{
		int num = 0;
		InitializeParameterLists();
		base.filterList.Add("@p1", shipmentId);
		using (DataTable dataTable = GetAsDataTable(SHIPMENTLINES_GET_NONEDISALESORDER_COUNT_FORSHIPMENT, base.filterList, null))
		{
			num = (dataTable?.Rows?.Count).GetValueOrDefault();
		}
		return Task.FromResult(num > 0);
	}

	public Task<IList<ShipmentDto>> GetShipments_PendingEDITransfer_AllUnmapped()
	{
		List<ShipmentDto> list = new List<ShipmentDto>();
		InitializeParameterLists();
		using (DataTable dataTable = GetAsDataTable(SHIPMENT_GET_SHIPMENTID_LIST_PENDINGEDITRANSFER_ALLEDIUNMAPPED, null, null))
		{
			foreach (DataRow row in dataTable.Rows)
			{
				list.Add(GetEDIShipment_Details_ForShipmentID(row["smpShipmentID"].ToString()).Result);
			}
		}
		return Task.FromResult((IList<ShipmentDto>)list);
	}

	public Task<ShipmentDto> GetEDIShipment_Details_ForShipmentID(string shipmentId)
	{
		ShipmentDto result = null;
		IList<ShipmentLineDto> list = new List<ShipmentLineDto>();
		IList<ShipmentPackageDto> list2 = null;
		IList<ShipmentPackageDetailsDto> list3 = null;
		InitializeParameterLists();
		base.filterList.Add("spaShipmentID|C", shipmentId);
		base.selectList.Add("spaShipmentID, spaShipmentPackageID,spaFedExPackageTypes,spaUPSPackageTypes, spaPackageWeightUOM, spaLabelFilePath, spaEDI856CustomLabel");
		using (DataTable dataTable = GetAsDataTable("ShipmentPackages", base.filterList, base.selectList, null, null))
		{
			list2 = new List<ShipmentPackageDto>();
			foreach (DataRow row in dataTable.Rows)
			{
				list2.Add(new ShipmentPackageDto
				{
					ShipmentID = row["spaShipmentID"].ToString().Trim(),
					ShipmentPackageID = Convert.ToInt16(row["spaShipmentPackageID"]),
					UPSPackageTypes = row["spaUPSPackageTypes"].ToString().Trim(),
					FedExPackageTypes = row["spaFedExPackageTypes"].ToString().Trim(),
					PackageWeightUOM = row["spaPackageWeightUOM"].ToString().Trim(),
					UserDefinedLabel = row["spaEDI856CustomLabel"].ToString().Trim()
				});
			}
		}
		InitializeParameterLists();
		base.filterList.Add("smlShipmentID|C", shipmentId);
		base.selectList.Add("smlShipmentID, smlShipmentLineID, smlSalesOrderID, smlSalesOrderLineID, smlSalesOrderDeliveryID, smlPartID, smlPartRevisionID, smlOrgPartID, smlOrgPartShortDescription, smlUnitOfMeasure, smlJobQuantityShipped, smlQuantityShipped, smlWeight, smlDescription,smlUnitPriceForeign");
		base.OrderOrGroupByList.Add("smlShipmentID, smlShipmentLineID");
		using (DataTable dataTable2 = GetAsDataTable("ShipmentLines", base.filterList, base.selectList, base.OrderOrGroupByList, null))
		{
			InitializeParameterLists();
			base.filterList.Add("spdShipmentID|C", shipmentId);
			base.selectList.Add("spdShipmentID,spdShipmentLineID,spdShipmentPackageID, spdQuantity, spdWeight, spdCountryOfManufacture");
			using DataTable source = GetAsDataTable("ShipmentPackageDetails", base.filterList, base.selectList, null, null);
			foreach (DataRow dLineRow in dataTable2.Rows)
			{
				list3 = new List<ShipmentPackageDetailsDto>();
				foreach (DataRow item in (from l in source.AsEnumerable()
					where l.Field<short>("spdShipmentLineID") == dLineRow.Field<short>("smlShipmentLineID")
					select l).ToList())
				{
					list3.Add(new ShipmentPackageDetailsDto
					{
						ShipmentID = item["spdShipmentID"].ToString().Trim(),
						ShipmentLineID = Convert.ToInt16(item["spdShipmentLineID"]),
						ShipmentPackageID = Convert.ToInt16(item["spdShipmentPackageID"]),
						Weight = Convert.ToDecimal(item["spdWeight"]),
						Quantity = Convert.ToDecimal(item["spdQuantity"]),
						CountryOfManufacture = item["spdCountryOfManufacture"].ToString().Trim()
					});
				}
				list.Add(new ShipmentLineDto
				{
					ShipmentID = dLineRow["smlShipmentID"].ToString().Trim(),
					ShipmentLineID = Convert.ToInt16(dLineRow["smlShipmentLineID"]),
					SalesOrderID = dLineRow["smlSalesOrderID"].ToString().Trim(),
					SalesOrderLineID = Convert.ToInt16(dLineRow["smlSalesOrderLineID"]),
					SalesOrderDeliveryID = Convert.ToInt16(dLineRow["smlSalesOrderDeliveryID"]),
					PartID = dLineRow["smlPartID"].ToString().Trim(),
					PartRevisionID = dLineRow["smlPartRevisionID"].ToString().Trim(),
					OrgPartID = dLineRow["smlOrgPartID"].ToString().Trim(),
					OrgPartShortDescription = dLineRow["smlOrgPartShortDescription"].ToString().Trim(),
					UnitOfMeasure = dLineRow["smlUnitOfMeasure"].ToString().Trim(),
					QuantityShipped = Convert.ToDecimal(dLineRow["smlQuantityShipped"]) + Convert.ToDecimal(dLineRow["smlJobQuantityShipped"]),
					UnitPrice = Convert.ToDecimal(dLineRow["smlUnitPriceForeign"]),
					Weight = Convert.ToDecimal(dLineRow["smlWeight"]),
					Description = dLineRow["smlDescription"].ToString().Trim(),
					ShipmentPackageDetails = list3
				});
			}
		}
		InitializeParameterLists();
		base.filterList.Add("@p1", shipmentId);
		using (DataTable source2 = GetAsDataTable(SHIPMENT_GET_SHIPMENTHEADERINFO_FORSHIPMENTID, base.filterList, null))
		{
			DataRow dataRow2 = source2.AsEnumerable().FirstOrDefault();
			result = new ShipmentDto
			{
				ShipmentID = dataRow2["smpShipmentID"].ToString().Trim(),
				ShipDate = Convert.ToDateTime(dataRow2["smpShipDate"]),
				CustomerOrganizationID = dataRow2["smpCustomerOrganizationID"].ToString().Trim(),
				ShipOrganizationID = dataRow2["smpShipOrganizationID"].ToString().Trim(),
				ShipLocationID = dataRow2["smpShipLocationID"].ToString().Trim(),
				ShipContactID = dataRow2["smpShipContactID"].ToString().Trim(),
				ARInvoiceLocationID = dataRow2["smpARInvoiceLocationID"].ToString().Trim(),
				ARInvoiceContactID = dataRow2["smpARInvoiceContactID"].ToString().Trim(),
				PlantID = dataRow2["smpPlantID"].ToString().Trim(),
				ShippingMethodID = dataRow2["smpShippingMethodID"].ToString().Trim(),
				TrackingNumber = dataRow2["smpTrackingNumber"].ToString().Trim(),
				ShippingCommentsText = dataRow2["smpShippingCommentsText"].ToString().Trim(),
				WeightTotal = Convert.ToDecimal(dataRow2["smpWeightTotal"]),
				AdditionalWeight = Convert.ToDecimal(dataRow2["smpAdditionalWeight"]),
				CurrencyRateID = dataRow2["smpCurrencyRateID"].ToString().Trim(),
				ShipmentLines = list,
				ShipmentPackages = list2
			};
		}
		return Task.FromResult(result);
	}

	public Task<bool> UpdateEdiFlag(IDictionary<string, bool> shipmentDictionary, SqlTransaction sqlTransaction)
	{
		SqlCommand sqlCommand = null;
		bool result = true;
		foreach (KeyValuePair<string, bool> item in shipmentDictionary)
		{
			sqlCommand = new SqlCommand(SHIPMENT_UPDATE_EDIFLAG_FORSHIMENT);
			sqlCommand.Parameters.AddWithValue("@smpShipmentID", item.Key);
			sqlCommand.Parameters.AddWithValue("@status", item.Value);
			if (item.Value)
			{
				sqlCommand.Parameters.AddWithValue("@updateDate", DateTime.Now);
			}
			else
			{
				sqlCommand.Parameters.AddWithValue("@updateDate", DBNull.Value);
			}
			result = base.M1database.ExecuteCommand(sqlCommand, sqlTransaction) > 0;
		}
		sqlCommand.Dispose();
		return Task.FromResult(result);
	}
}
