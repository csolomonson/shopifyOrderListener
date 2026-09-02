using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.199", "Update SerialNumberTransactions for Assigned to Job Order Qty", "2017-03-23")]
public class v92199a
{
	public v92199a(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SerialNumberTransactions Set sntTableName = 'Jobs', sntTableUniqueID = jmpUniqueID From SerialNumberTransactions Inner Join Jobs on sntJobID = jmpJobID where sntJobID <> '' and sntJobAssemblyID = 0 and sntTableName = ''");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SerialNumberTransactions Set sntTableName = 'JobAssemblies', sntTableUniqueID = jmaUniqueID From SerialNumberTransactions Inner Join JobAssemblies on sntJobID = jmaJobID and sntJobAssemblyID = jmaJobAssemblyID where sntJobID <> '' and sntJobAssemblyID <> 0 and sntTableName = ''");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SerialNumberTransactions set sntTransactionType = 47 From SerialNumberTransactions inner join Jobs on sntJobID = jmpjobid and sntJobAssemblyID = 0 Where sntTransactionType = 1 and (jmpOrderQuantity <> 0 and jmpInventoryQuantity = 0)");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SerialNumberTransactions set sntTransactionType = 47 From SerialNumberTransactions inner join JobAssemblies on sntJobID = jmajobid and sntJobAssemblyID = jmaJobAssemblyID Where sntTransactionType = 1 and jmaJobAssemblyID <> 0 and (jmaOrderQuantity <> 0 and jmaInventoryQuantity = 0)");
	}
}
