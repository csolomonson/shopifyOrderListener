using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

public class EasyOrder
{
	private short _module = 10;

	public string CreateOfflineFileFromSalesOrder(IServiceProvider provider, M1Database database, string salesOrderID)
	{
		using M1BindingSource m1BindingSource = new M1BindingSource(provider, isManuallyAdded: true);
		m1BindingSource.LoadDefinition(string.Empty, "SalesOrders", null, true, loadDataNow: false);
		m1BindingSource.ClearCache();
		m1BindingSource.NavigateTo($"ompSalesOrderID = '{salesOrderID}'");
		return CreateOfflineFile(provider, database, m1BindingSource);
	}

	public string CreateOfflineFile(IServiceProvider provider, M1Database database, object bindingSource)
	{
		M1BindingSource salesOrderBS = (M1BindingSource)bindingSource;
		AppSecurityInfo appSecurityInfo = new AppSecurityInfo(provider);
		StringBuilder errorString = new StringBuilder();
		StringBuilder infoString = new StringBuilder();
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		int num = 0;
		if (!appSecurityInfo.IsCustomModulePurchased(_module))
		{
			return string.Empty;
		}
		if (salesOrderBS.Fields["ompEasyOrderEnabled"].Value is DBNull || !(bool)salesOrderBS.Fields["ompEasyOrderEnabled"].Value)
		{
			return string.Empty;
		}
		empty = salesOrderBS.Fields["ompSalesOrderID"].Value.ToString();
		empty2 = salesOrderBS.Fields["ompEasyOrderID"].Value.ToString();
		empty3 = salesOrderBS.Fields["ompCustomerOrganizationID"].Value.ToString();
		num = int.Parse(salesOrderBS.Fields["ompEasyOrderStatus"].Value.ToString());
		EasyOrderInterfaceFileModel easyOrderInterfaceFileModel = new EasyOrderInterfaceFileModel(database, salesOrderBS.Context);
		if (!easyOrderInterfaceFileModel.ProcessExportManagerActions(empty, empty2, empty3, num, ref salesOrderBS, ref errorString, ref infoString))
		{
			return "Error creating EasyOrder Offline files, please review the log details in:\n" + easyOrderInterfaceFileModel.CurrentLogFile;
		}
		return string.Empty;
	}

	internal string CreateOfflineFileFromShipment(IServiceProvider provider, M1Database database, string shipmentID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (SqlCommand sqlCommand = new SqlCommand("SELECT DISTINCT smlSalesOrderID FROM ShipmentLines WHERE smlShipmentID = @ShipmentID"))
		{
			sqlCommand.Parameters.AddWithValue("@ShipmentID", shipmentID);
			foreach (DataRow item in database.GetDataTable(sqlCommand)?.Rows)
			{
				string value = CreateOfflineFileFromSalesOrder(provider, database, item["smlSalesOrderID"].ToString());
				stringBuilder.Append(value);
			}
		}
		return stringBuilder.ToString();
	}

	public void UpdateEasyOrderExternalStatusFromShipment(M1Database database, string shipmentID, object transaction)
	{
		if (!new AppSecurityInfo(database).IsCustomModulePurchased(_module) || string.IsNullOrWhiteSpace(shipmentID))
		{
			return;
		}
		shipmentID = shipmentID.Trim();
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		using SqlCommand sqlCommand = new SqlCommand("SELECT DISTINCT smlSalesOrderID FROM ShipmentLines WHERE smlShipmentID = @ShipmentID");
		sqlCommand.Parameters.AddWithValue("@ShipmentID", shipmentID);
		foreach (DataRow item in database.GetDataTable(sqlCommand, (SqlTransaction)transaction)?.Rows)
		{
			UpdateEasyOrderExternalStatus(database, item["smlSalesOrderID"].ToString(), transaction);
		}
	}

	public void UpdateEasyOrderExternalStatus(M1Database database, string salesOrderID, object transaction)
	{
		if (new AppSecurityInfo(database).IsCustomModulePurchased(_module) && !string.IsNullOrWhiteSpace(salesOrderID))
		{
			salesOrderID = salesOrderID.Trim();
			if (transaction == DBNull.Value)
			{
				transaction = null;
			}
			SqlCommand sqlCommand = new SqlCommand("\r\n                ;WITH\r\n                #SalesOrderDeliveries AS (\r\n\t                SELECT omdSalesOrderID,omdSalesOrderLineID,omdSalesOrderDeliveryID,omdQuantityShipped,omdShippedComplete,omdInvoicedComplete,omdDeliveryQuantity\r\n\t                FROM SalesOrderDeliveries\r\n\t                WHERE omdSalesOrderID = @SalesOrderID\r\n                ),\r\n                #SalesOrderLines AS (\r\n\t                SELECT omlSalesOrderID,omlSalesOrderLineID,omlOrderQuantity,\r\n\t                ISNULL(COUNT(omdSalesOrderDeliveryID), 0) as omlDeliveryLines,\r\n\t                ISNULL(SUM(omdDeliveryQuantity), 0) AS omlDeliveryQuantity,\r\n\t                ISNULL(SUM(omdQuantityShipped), 0) AS omlShippedQuantity,\r\n\t                ISNULL(SUM(CASE WHEN omdShippedComplete <> 0 THEN 1 ELSE 0 END), 0) AS omlShippedLines,\r\n\t                ISNULL(SUM(CASE WHEN omdInvoicedComplete <> 0 THEN 1 ELSE 0 END), 0) AS omlInvoicedLines\r\n\t                FROM SalesOrderLines\r\n\t                LEFT JOIN #SalesOrderDeliveries ON (omlSalesOrderID = omdSalesOrderID AND omlSalesOrderLineID = omdSalesOrderLineID)\r\n\t                WHERE omlSalesOrderID = @SalesOrderID\r\n\t                GROUP BY omlSalesOrderID,omlSalesOrderLineID,omlOrderQuantity\r\n                )\r\n\r\n                UPDATE SalesOrderLines SET omlEasyOrderExternalStatus = CASE\r\n\t                WHEN #SalesOrderLines.omlDeliveryLines > 0 AND #SalesOrderLines.omlDeliveryLines = #SalesOrderLines.omlInvoicedLines THEN 'INV'\r\n\t                WHEN #SalesOrderLines.omlDeliveryLines > 0 AND #SalesOrderLines.omlDeliveryLines = #SalesOrderLines.omlShippedLines AND SalesOrderLines.omlOrderQuantity = #SalesOrderLines.omlShippedQuantity THEN 'DLV' \r\n\t                WHEN #SalesOrderLines.omlDeliveryLines > 0 AND #SalesOrderLines.omlDeliveryLines = #SalesOrderLines.omlShippedLines AND SalesOrderLines.omlOrderQuantity <> #SalesOrderLines.omlShippedQuantity THEN 'BCK'\r\n\t                ELSE 'CON'\r\n                END\r\n                FROM SalesOrderLines LEFT JOIN #SalesOrderLines ON (SalesOrderLines.omlSalesOrderID = #SalesOrderLines.omlSalesOrderID AND SalesOrderLines.omlSalesOrderLineID = #SalesOrderLines.omlSalesOrderLineID)\r\n                WHERE SalesOrderLines.omlSalesOrderID = @SalesOrderID");
			SqlCommand sqlCommand2 = new SqlCommand("\r\n                ;WITH\r\n                #SalesOrders AS (\r\n\t                SELECT ompSalesOrderID,\r\n\t                ISNULL(COUNT(omlSalesOrderLineID), 0) AS ompSalesOrderLines,\r\n\t                ISNULL(SUM(CASE WHEN omlEasyOrderExternalStatus = 'CON' THEN 1 ELSE 0 END), 0) AS ompConfirmedLines,\r\n\t                ISNULL(SUM(CASE WHEN omlEasyOrderExternalStatus = 'BCK'  THEN 1 ELSE 0 END), 0) AS ompBackorderLines,\r\n\t                ISNULL(SUM(CASE WHEN omlEasyOrderExternalStatus = 'DLV'  THEN 1 ELSE 0 END), 0) AS ompDeliveredLines,\r\n\t                ISNULL(SUM(CASE WHEN omlEasyOrderExternalStatus = 'INV'  THEN 1 ELSE 0 END), 0) AS ompInvoicedLines\r\n\t                FROM SalesOrders\r\n\t                LEFT JOIN SalesOrderLines ON (ompSalesOrderID = omlSalesOrderID)\r\n\t                WHERE omlSalesOrderID = @SalesOrderID\r\n\t                GROUP BY ompSalesOrderID\r\n                )\r\n\r\n                UPDATE SalesOrders SET ompEasyOrderExternalStatus = CASE\r\n\t                WHEN #SalesOrders.ompSalesOrderLines > 0 AND #SalesOrders.ompDeliveredLines = 0 AND #SalesOrders.ompInvoicedLines = 0 THEN 'CON'\r\n\t                WHEN #SalesOrders.ompSalesOrderLines > 0 AND #SalesOrders.ompSalesOrderLines = #SalesOrders.ompInvoicedLines THEN 'INV'\t\r\n\t                WHEN #SalesOrders.ompSalesOrderLines > 0 AND #SalesOrders.ompSalesOrderLines = #SalesOrders.ompDeliveredLines THEN 'DLV'\t\r\n\t                WHEN #SalesOrders.ompSalesOrderLines > 0 AND #SalesOrders.ompSalesOrderLines <> #SalesOrders.ompInvoicedLines THEN 'BCK'\r\n\t                ELSE 'CON'\r\n                END\r\n                FROM SalesOrders LEFT JOIN #SalesOrders ON (SalesOrders.ompSalesOrderID = #SalesOrders.ompSalesOrderID)\r\n                WHERE SalesOrders.ompSalesOrderID = @SalesOrderID");
			sqlCommand.Parameters.AddWithValue("@SalesOrderID", salesOrderID);
			sqlCommand2.Parameters.AddWithValue("@SalesOrderID", salesOrderID);
			database.ExecuteCommand(sqlCommand, (SqlTransaction)transaction);
			database.ExecuteCommand(sqlCommand2, (SqlTransaction)transaction);
		}
	}

	public static string GetDefaultDataset(string webshops)
	{
		string result = "M1_M1";
		if (!string.IsNullOrEmpty(webshops))
		{
			string[] array = webshops.Split(';');
			for (int i = 0; i < array.Length; i++)
			{
				List<string> list = array[i].Split(',').ToList();
				if (bool.Parse((list.ElementAtOrDefault(3) != null) ? list[3] : "False"))
				{
					result = list[0];
					break;
				}
			}
		}
		return result;
	}

	public static string GetDataset(string webshops, string identity)
	{
		string result = "M1_M1";
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!string.IsNullOrEmpty(webshops))
		{
			string[] array = webshops.Split(';');
			for (int i = 0; i < array.Length; i++)
			{
				List<string> list = array[i].Split(',').ToList();
				string value = ((list.ElementAtOrDefault(0) != null) ? list[0] : "M1_M1");
				string key = ((list.ElementAtOrDefault(1) != null) ? list[1] : string.Empty);
				if (!dictionary.ContainsKey(key))
				{
					dictionary.Add(key, value);
				}
			}
			result = ((!dictionary.ContainsKey(identity)) ? GetDefaultDataset(webshops) : dictionary[identity]);
		}
		return result;
	}

	public static string GetPlant(string webshops, string identity)
	{
		string result = string.Empty;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!string.IsNullOrEmpty(webshops))
		{
			string[] array = webshops.Split(';');
			for (int i = 0; i < array.Length; i++)
			{
				List<string> list = array[i].Split(',').ToList();
				string value = ((list.ElementAtOrDefault(2) != null) ? list[2] : string.Empty);
				string key = ((list.ElementAtOrDefault(1) != null) ? list[1] : string.Empty);
				if (!dictionary.ContainsKey(key))
				{
					dictionary.Add(key, value);
				}
			}
			if (dictionary.ContainsKey(identity))
			{
				result = dictionary[identity];
			}
		}
		return result;
	}
}
