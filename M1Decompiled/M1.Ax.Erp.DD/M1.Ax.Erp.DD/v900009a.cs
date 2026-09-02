using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.009", "Add fields to DMRShipmentLines table", "2014-10-31")]
public class v900009a
{
	public v900009a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslInventoryQuantityShipped"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentLines", "dslInventoryQuantityShipped", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslInventoryUnitOfMeasure"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentLines", "dslInventoryUnitOfMeasure", "nvarchar", 2, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslConversionFactor"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentLines", "dslConversionFactor", "numeric", 14, 8, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslConversionFactor"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DMRShipmentLines Set dslConversionFactor = ISNULL((Select Top 1 dmlConversionFactor From DMRShipmentLines INNER JOIN DMRClaimLines on dslDMRClaimID = dmlDMRClaimID and dslDMRClaimLineID = dmlDMRClaimLineID), 1)");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslInventoryQuantityShipped"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DMRShipmentLines Set dslInventoryQuantityShipped = dslQuantityShipped / CASE WHEN dslConversionFactor = 0 THEN 1 ELSE dslConversionFactor END");
		}
	}
}
