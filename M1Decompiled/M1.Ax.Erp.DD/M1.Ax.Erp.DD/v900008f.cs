using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.008", "Add fields to MfgReceipts table", "2014-10-23")]
public class v900008f
{
	public v900008f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmPlantDepartmentID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceipts", "rmmPlantDepartmentID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
