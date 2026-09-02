using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.014", "Add source fields to shipments", "2014-12-22")]
public class v900014z
{
	public v900014z(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentLines", "smlSourceTableName"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentLines", "smlSourceTableName", "nvarchar", 30, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentLines", "smlSourceTableUniqueID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentLines", "smlSourceTableUniqueID", "uniqueidentifier", 16, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "update ShipmentLines set smlSourceTableName = 'SalesOrderDeliveries', smlSourceTableUniqueID = omdUniqueID from ShipmentLines inner join SalesOrderDeliveries on smlSalesOrderID=omdSalesOrderID and smlSalesOrderLineID=omdSalesOrderLineID and smlSalesOrderDeliveryID=omdSalesOrderDeliveryID");
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentComponents", "smoSourceTableName"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentComponents", "smoSourceTableName", "nvarchar", 30, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentComponents", "smoSourceTableUniqueID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentComponents", "smoSourceTableUniqueID", "uniqueidentifier", 16, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "update ShipmentComponents set smoSourceTableName = 'SalesOrderComponents', smoSourceTableUniqueID = omoUniqueID from ShipmentComponents inner join SalesOrderComponents on smoSalesOrderID=omoSalesOrderID and smoSalesOrderLineID=omoSalesOrderLineID and smoSalesOrderDeliveryID=omoSalesOrderDeliveryID and smoSalesOrderComponentID=omoSalesOrderComponentID");
	}
}
