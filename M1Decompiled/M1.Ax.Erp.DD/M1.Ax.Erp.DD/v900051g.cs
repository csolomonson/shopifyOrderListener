using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.051", "Add fields to DMRShipmentLines table", "2015-06-25")]
public class v900051g
{
	public v900051g(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslReturnQuantityShipped"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentLines", "dslReturnQuantityShipped", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "update DMRShipmentLines set dslReturnQuantityShipped = dslInventoryQuantityShipped + dslJobMatQuantityShipped, dslInventoryQuantityShipped = 0, dslJobMatQuantityShipped = 0 from DMRShipmentLines Inner Join DMRClaimLines on dslDMRClaimID = dmlDMRClaimID and dslDMRClaimLineID = dmlDMRClaimLineID Inner Join InspectionLines on dmlInspectionID = qalInspectionID and dmlInspectionLineID = qalInspectionLineID");
		}
	}
}
