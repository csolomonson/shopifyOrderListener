using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.051", "Add fields to DMRShipmentComponents table", "2015-06-25")]
public class v900051h
{
	public v900051h(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentComponents", "dsoReturnQuantityShipped"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentComponents", "dsoReturnQuantityShipped", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentComponents", "dsoReturnParentQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentComponents", "dsoReturnParentQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentComponents", "dsoReturnQuantityShipped"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DMRShipmentComponents Set dsoReturnParentQuantity = dslReturnQuantityShipped From DMRShipmentLines Inner Join DMRShipmentComponents On DSLDMRSHIPMENTID = dsoDMRShipmentID And DSLDMRSHIPMENTLINEID = dsoDMRShipmentLineID");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DMRShipmentComponents Set dsoReturnQuantityShipped = dsoReturnParentQuantity*dsoAdditionalQuantity");
		}
	}
}
