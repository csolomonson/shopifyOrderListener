using System;
using System.Data;
using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.6.100", "Set WarehouseBins default values for Inactive and InactiveDate, and set PartBins default values for Inactive and InactiveDate", "2023-2-24")]
public class v96100c
{
	public v96100c(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseBins", "inbInactive"))
		{
			string value = "Database: '" + parms.DatabaseName + "' Description: '" + parms.Database.Props("DatasetProperties").Field<string>("xadDescription") + "'\r\n";
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			string queryString = "SELECT inbWarehouseID,inbWarehouseBinID,imwInactive,inbInactive FROM WarehouseBins INNER JOIN Warehouses ON inbWarehouseID = imwWarehouseID WHERE imwInactive = 1 and inbInactive = 0";
			parms.Database.ExecuteCommand(queryString);
			DataTable dataTable = parms.Database.GetDataTable(queryString);
			stringBuilder2.Clear();
			foreach (DataRow row12 in dataTable.Rows)
			{
				stringBuilder2.AppendLine("[Warehouse:'" + row12.Field<string>("inbWarehouseID") + "', Bin:'" + row12.Field<string>("inbWarehouseBinID") + "']");
			}
			if (stringBuilder2.Length > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("The following warehouse bins have been set to inactive.");
				stringBuilder.AppendLine();
				stringBuilder.Append(stringBuilder2);
			}
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE WarehouseBins SET inbInactive = 1, inbDefaultBin = 0 FROM WarehouseBins INNER JOIN Warehouses ON inbWarehouseID = imwWarehouseID WHERE imwInactive = 1");
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartBins", "imbInactiveBin"))
			{
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE PartBins SET imbInactiveBin = 1, imbDefaultBin = 0  FROM PartBins INNER JOIN WarehouseBins ON imbWarehouseID = inbWarehouseID AND imbPartBinID = inbWarehouseBinID WHERE inbInactive = 1 AND imbQuantityOnHand >= 0");
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE PartBins SET imbInactiveBin = 0 WHERE imbQuantityOnHand < 0");
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartBins", "imbInactiveBinDate"))
			{
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE PartBins SET imbInactiveBinDate = GETDATE() FROM PartBins INNER JOIN WarehouseBins ON imbWarehouseID = inbWarehouseID AND imbPartBinID = inbWarehouseBinID WHERE imbInactiveBin = 1");
			}
			queryString = "SELECT imbPartID,imbPartRevisionID,imbWarehouseID,imbPartBinID FROM PartBins INNER JOIN WarehouseBins ON imbWarehouseID = inbWarehouseID AND imbPartBinID = inbWarehouseBinID WHERE inbInactive = 1 AND imbQuantityOnHand >= 0 AND imbInactiveBin = 1";
			parms.Database.ExecuteCommand(queryString);
			DataTable dataTable2 = parms.Database.GetDataTable(queryString);
			stringBuilder2.Clear();
			foreach (DataRow row13 in dataTable2.Rows)
			{
				stringBuilder2.AppendLine("[Part: '" + row13.Field<string>("imbPartID") + "', Revision:'" + row13.Field<string>("imbPartRevisionID") + "', Warehouse:'" + row13.Field<string>("imbWarehouseID") + "', Bin:'" + row13.Field<string>("imbPartBinID") + "']");
			}
			if (stringBuilder2.Length > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("The following part revision bin locations have been set to inactive.");
				stringBuilder.AppendLine();
				stringBuilder.Append(stringBuilder2);
			}
			queryString = "SELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbQuantityOnHand FROM PartBins INNER JOIN Warehouses ON imbWarehouseID = imwWarehouseID WHERE imwInactive = 1 AND imbQuantityOnHand < 0 Union SELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbQuantityOnHand FROM PartBins INNER JOIN WarehouseBins ON imbWarehouseID = inbWarehouseID and imbPartBinID = inbWarehouseBinID WHERE inbInactive = 1 AND imbQuantityOnHand < 0 ";
			parms.Database.ExecuteCommand(queryString);
			DataTable dataTable3 = parms.Database.GetDataTable(queryString);
			stringBuilder2.Clear();
			foreach (DataRow row14 in dataTable3.Rows)
			{
				stringBuilder2.AppendLine(string.Format("[Part: '{0}', Revision:'{1}', Warehouse:'{2}', Bin:'{3}', Quantity on Hand: {4}]", row14.Field<string>("imbPartID"), row14.Field<string>("imbPartRevisionID"), row14.Field<string>("imbWarehouseID"), row14.Field<string>("imbPartBinID"), row14.Field<decimal>("imbQuantityOnHand")));
			}
			if (stringBuilder2.Length > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("The following part revision bin locations utilize an inactive warehouse bin but have a negative quantity on hand. These part revision bin locations will remain active. The negative quantity on hand must be addressed and the part revision bin manually set to inactive to correspond with the inactive warehouse bin.");
				stringBuilder.AppendLine();
				stringBuilder.Append(stringBuilder2);
			}
			queryString = "SELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbQuantityOnHand FROM WarehouseBins INNER JOIN PartBins on imbWarehouseID = inbWarehouseID and imbPartBinID = inbWarehouseBinID WHERE inbInactive = 1 AND imbQuantityOnHand > 0";
			parms.Database.ExecuteCommand(queryString);
			DataTable dataTable4 = parms.Database.GetDataTable(queryString);
			stringBuilder2.Clear();
			foreach (DataRow row15 in dataTable4.Rows)
			{
				stringBuilder2.AppendLine(string.Format("[Part: '{0}', Revision:'{1}', Warehouse:'{2}', Bin:'{3}', Quantity on Hand: {4}]", row15.Field<string>("imbPartID"), row15.Field<string>("imbPartRevisionID"), row15.Field<string>("imbWarehouseID"), row15.Field<string>("imbPartBinID"), row15.Field<decimal>("imbQuantityOnHand")));
			}
			if (stringBuilder2.Length > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("The following part revision bin locations have been set to inactive, but still have quantity on hand. You will still be able to use these part revision bins for outbound transactions to utilize the remaining quantity on hand.");
				stringBuilder.AppendLine();
				stringBuilder.Append(stringBuilder2);
			}
			queryString = "SELECT qalInspectionID, qalInspectionLineID, imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, qalQuantityToInspect FROM WarehouseBins INNER JOIN PartBins on imbWarehouseID = inbWarehouseID and imbPartBinID = inbWarehouseBinID INNER JOIN InspectionLines on qalPartID=imbPartID and qalPartRevisionID=imbPartRevisionID and qalPartWarehouseLocationID = imbWarehouseID and qalPartBinID = imbPartBinID WHERE inbInactive = 1 AND qalQuantityToInspect > 0 and qalPosted = 0";
			parms.Database.ExecuteCommand(queryString);
			DataTable dataTable5 = parms.Database.GetDataTable(queryString);
			stringBuilder2.Clear();
			foreach (DataRow row16 in dataTable5.Rows)
			{
				stringBuilder2.AppendLine(string.Format("[Inspection ID: '{0}', Inspection Line: '{1}',Part: '{2}', Revision:'{3}', Warehouse:'{4}', Bin:'{5}', Quantity to Inspect: {6}]", row16.Field<string>("qalInspectionID"), row16.Field<short>("qalInspectionLineID"), row16.Field<string>("imbPartID"), row16.Field<string>("imbPartRevisionID"), row16.Field<string>("imbWarehouseID"), row16.Field<string>("imbPartBinID"), row16.Field<decimal>("qalQuantityToInspect")));
			}
			if (stringBuilder2.Length > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("The following part revision bin locations have been set to inactive, but still have quantity to inspect. You will still be able to use these part revision bins for outbound transactions to utilize the remaining inspection quantity.");
				stringBuilder.AppendLine();
				stringBuilder.Append(stringBuilder2);
			}
			queryString = "SELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, omdQuantityAllocated, omdSalesOrderID, omdSalesOrderLineID, omjJobID FROM SalesOrderDeliveries INNER JOIN PartBins ON imbPartID = omdPartID AND imbPartRevisionID = omdPartRevisionID AND imbWarehouseID = omdPartWarehouseLocationID AND imbPartBinID = omdPartBinID INNER JOIN SalesOrderJobLinks ON omdSalesOrderID = omjSalesOrderID AND omdSalesOrderLineID = omjSalesOrderLineID WHERE omdDeliveryType = 2 AND omdClosed = 0 AND imbInactiveBin = 1";
			parms.Database.ExecuteCommand(queryString);
			DataTable dataTable6 = parms.Database.GetDataTable(queryString);
			stringBuilder2.Clear();
			foreach (DataRow row17 in dataTable6.Rows)
			{
				stringBuilder2.AppendLine(string.Format("[Part: '{0}', Revision:'{1}', Warehouse:'{2}', Bin:'{3}', Quantity on Allocated: {4}, Order ID:'{5}', Line: '{6}', Job ID: '{7}']", row17.Field<string>("imbPartID"), row17.Field<string>("imbPartRevisionID"), row17.Field<string>("imbWarehouseID"), row17.Field<string>("imbPartBinID"), row17.Field<decimal>("omdQuantityAllocated"), row17.Field<string>("omdSalesOrderID"), row17.Field<short>("omdSalesOrderLineID"), row17.Field<string>("omjJobID")));
			}
			queryString = "SELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, omdQuantityAllocated, omdSalesOrderID, omdSalesOrderLineID, omjJobID FROM SalesOrderDeliveries INNER JOIN PartBins ON imbPartID = omdPartID AND imbPartRevisionID = omdPartRevisionID AND imbWarehouseID = omdPartWarehouseLocationID AND imbPartBinID = omdPartBinID LEFT JOIN SalesOrderJobLinks ON omdSalesOrderID = omjSalesOrderID AND omdSalesOrderLineID = omjSalesOrderLineID WHERE omdDeliveryType = 2 AND omdClosed = 0 AND imbInactiveBin = 1 AND omjJobID is null";
			parms.Database.ExecuteCommand(queryString);
			foreach (DataRow row18 in parms.Database.GetDataTable(queryString).Rows)
			{
				stringBuilder2.AppendLine(string.Format("[Part: '{0}', Revision:'{1}', Warehouse:'{2}', Bin:'{3}', Quantity on Allocated: {4}, Order ID:'{5}', Line: '{6}']", row18.Field<string>("imbPartID"), row18.Field<string>("imbPartRevisionID"), row18.Field<string>("imbWarehouseID"), row18.Field<string>("imbPartBinID"), row18.Field<decimal>("omdQuantityAllocated"), row18.Field<string>("omdSalesOrderID"), row18.Field<short>("omdSalesOrderLineID")));
			}
			queryString = "SELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, omoQuantityAllocated, omoSalesOrderID, omoSalesOrderLineID, omoSalesOrderComponentID FROM SalesOrderComponents INNER JOIN PartBins ON imbPartID = omoPartID AND imbPartRevisionID = omoPartRevisionID AND imbWarehouseID = omoPartWarehouseLocationID AND imbPartBinID = omoPartBinID LEFT JOIN SalesOrderJobLinks ON omoSalesOrderID = omjSalesOrderID AND omoSalesOrderLineID = omjSalesOrderLineID WHERE omoQuantityAllocated > 0 AND omoClosed = 0 AND imbInactiveBin = 1 AND omjJobID is null";
			parms.Database.ExecuteCommand(queryString);
			foreach (DataRow row19 in parms.Database.GetDataTable(queryString).Rows)
			{
				stringBuilder2.AppendLine(string.Format("[Part: '{0}', Revision:'{1}', Warehouse:'{2}', Bin:'{3}', Quantity on Allocated: {4}, Order ID:'{5}', Line: '{6}', Component ID: '{7}']", row19.Field<string>("imbPartID"), row19.Field<string>("imbPartRevisionID"), row19.Field<string>("imbWarehouseID"), row19.Field<string>("imbPartBinID"), row19.Field<decimal>("omoQuantityAllocated"), row19.Field<string>("omoSalesOrderID"), row19.Field<short>("omoSalesOrderLineID"), row19.Field<short>("omoSalesOrderComponentID")));
			}
			if (stringBuilder2.Length > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("The following part revision bin locations have been set to inactive, but still have associated allocations in Sales Orders. Consider changing these allocations to use a different bin location.");
				stringBuilder.AppendLine();
				stringBuilder.Append(stringBuilder2);
			}
			queryString = "SELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, jmaQuantityToPull, jmaJobID, jmaJobAssemblyID FROM JobAssemblies INNER JOIN PartBins ON imbPartID = jmaPartID AND imbPartRevisionID = jmaPartRevisionID AND imbWarehouseID = jmaPartWareHouseLocationID AND imbPartBinID = jmaPartBinID LEFT JOIN SalesOrderJobLinks ON jmaJobID = omjJobID WHERE jmaQuantityToPull > 0 AND jmaClosed = 0 AND imbInactiveBin = 1 AND omjJobID is null";
			parms.Database.ExecuteCommand(queryString);
			DataTable dataTable7 = parms.Database.GetDataTable(queryString);
			stringBuilder2.Clear();
			foreach (DataRow row20 in dataTable7.Rows)
			{
				stringBuilder2.AppendLine(string.Format("[Part: '{0}', Revision:'{1}', Warehouse:'{2}', Bin:'{3}', Quantity on Allocated: {4}, Job ID:'{5}', Job Assembly ID: '{6}']", row20.Field<string>("imbPartID"), row20.Field<string>("imbPartRevisionID"), row20.Field<string>("imbWarehouseID"), row20.Field<string>("imbPartBinID"), row20.Field<decimal>("jmaQuantityToPull"), row20.Field<string>("jmaJobID"), row20.Field<int>("jmaJobAssemblyID")));
			}
			if (stringBuilder2.Length > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("The following part revision bin locations have been set to inactive, but still have associated allocations in Job Assemblies. Consider changing these allocations to use a different bin location.");
				stringBuilder.AppendLine();
				stringBuilder.Append(stringBuilder2);
			}
			queryString = "SELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, jmmQuantityAllocated, jmmJobID, jmmJobAssemblyID, jmmJobMaterialID  FROM JobMaterials INNER JOIN PartBins ON imbPartID = jmmPartID AND imbPartRevisionID = jmmPartRevisionID AND imbWarehouseID = jmmPartWarehouseLocationID AND imbPartBinID = jmmPartBinID LEFT JOIN SalesOrderJobLinks ON jmmJobID = omjJobID WHERE jmmQuantityAllocated > 0 AND jmmClosed = 0 AND imbInactiveBin = 1 AND omjJobID is null";
			parms.Database.ExecuteCommand(queryString);
			DataTable dataTable8 = parms.Database.GetDataTable(queryString);
			stringBuilder2.Clear();
			foreach (DataRow row21 in dataTable8.Rows)
			{
				stringBuilder2.AppendLine(string.Format("[Part: '{0}', Revision:'{1}', Warehouse:'{2}', Bin:'{3}', Quantity on Allocated: {4}, Job ID:'{5}', Job Assembly ID: '{6}', Job Material ID: '{7}']", row21.Field<string>("imbPartID"), row21.Field<string>("imbPartRevisionID"), row21.Field<string>("imbWarehouseID"), row21.Field<string>("imbPartBinID"), row21.Field<decimal>("jmmQuantityAllocated"), row21.Field<string>("jmmJobID"), row21.Field<int>("jmmJobAssemblyID"), row21.Field<int>("jmmJobMaterialID")));
			}
			queryString = "SELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, jmtQuantityAllocated, jmtJobID, jmtJobAssemblyID, jmtJobMaterialID, jmtJobMaterialComponentID FROM JobMaterialComponents INNER JOIN PartBins ON imbPartID = jmtPartID AND imbPartRevisionID = jmtPartRevisionID AND imbWarehouseID = jmtPartWarehouseLocationID AND imbPartBinID = jmtPartBinID LEFT JOIN SalesOrderJobLinks ON jmtJobID = omjJobID WHERE jmtQuantityAllocated > 0 AND jmtClosed = 0 AND imbInactiveBin = 1 AND omjJobID is null";
			parms.Database.ExecuteCommand(queryString);
			foreach (DataRow row22 in parms.Database.GetDataTable(queryString).Rows)
			{
				stringBuilder2.AppendLine(string.Format("[Part: '{0}', Revision:'{1}', Warehouse:'{2}', Bin:'{3}', Quantity on Allocated: {4}, Job ID:'{5}', Job Assembly ID: '{6}', Job Material ID: '{7}', Job Material Component ID: '{8}']", row22.Field<string>("imbPartID"), row22.Field<string>("imbPartRevisionID"), row22.Field<string>("imbWarehouseID"), row22.Field<string>("imbPartBinID"), row22.Field<decimal>("jmtQuantityAllocated"), row22.Field<string>("jmtJobID"), row22.Field<int>("jmtJobAssemblyID"), row22.Field<int>("jmtJobMaterialID"), row22.Field<int>("jmtJobMaterialComponentID")));
			}
			if (stringBuilder2.Length > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("The following part revision bin locations have been set to inactive, but still have associated allocations in Job Materials. Consider changing these allocations to use a different bin location.");
				stringBuilder.AppendLine();
				stringBuilder.Append(stringBuilder2);
			}
			if (stringBuilder.Length > 0)
			{
				parms.HeaderMessage = "\r\nThe following Warehouse and Part Revision bins have been affected by this upgrade.";
				parms.ShowSaveMessageButton = false;
				stringBuilder.Insert(0, value);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("This information will be automatically saved to the M1 root directory for your reference.");
				parms.Messages.Add(stringBuilder.ToString());
				parms.FileToSave = DateTime.Now.ToString("yyyydMM_hh.mm.ss") + "_" + parms.DatabaseName + "_DBUPDATE.txt";
			}
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseBins", "inbInactiveDate"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE WarehouseBins SET inbInactiveDate = GETDATE() FROM WarehouseBins INNER JOIN Warehouses ON inbWarehouseID = imwWarehouseID WHERE imwInactive = 1 AND (inbInactiveDate is null or inbInactiveDate = '')");
		}
	}
}
