using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.696", "Insert lot/serial number transactions for inventory counts", "2018-05-03")]
public class v92696
{
	public v92696(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "SerialNumberTransactions") && parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "LotNumberTransactions"))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("insert into SerialNumberTransactions(sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntSerialNumberID, sntTransactionType, sntStatus, sntTransactionDate, sntQuantity, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntPartTransactionID, sntCreatedBy, sntCreatedDate, sntTableName, sntTableUniqueID) select sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntSerialNumberID, 6, sntStatus, sntTransactionDate, sntQuantity, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntPartTransactionID, 'CONVERSION' as sntCreatedBy, Getdate() as sntCreatedDate, sntTableName, sntTableUniqueID from SerialNumberTransactions where sntTransactionType = 27  and sntTableName In ('InventoryCountLines') and sntTableUniqueID not in (select sntTableUniqueID from SerialNumberTransactions where sntTransactionType = 6); ");
			stringBuilder.AppendLine("insert into LotNumberTransactions(abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtLotNumberID, abtTransactionType, abtTransactionDate, abtQuantity, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtPartTransactionID, abtCreatedBy, abtCreatedDate, abtTableName, abtTableUniqueID) select abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtLotNumberID, 6, abtTransactionDate, abtQuantity, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtPartTransactionID, 'CONVERSION' as abtCreatedBy, Getdate() as abtCreatedDate, abtTableName, abtTableUniqueID from LotNumberTransactions where abtTransactionType = 27 and abtTableName In ('InventoryCountLines') and abtTableUniqueID not in (select abtTableUniqueID from LotNumberTransactions where abtTransactionType = 6); ");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, stringBuilder.ToString());
		}
	}
}
