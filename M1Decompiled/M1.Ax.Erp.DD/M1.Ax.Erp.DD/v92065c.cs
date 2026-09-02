using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.065", "Modify field in InventoryCounts table", "2017-01-06")]
public class v92065c
{
	public v92065c(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InventoryCounts", "imnNumberofRecordsGenerated"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InventoryCounts", "imnNumberofRecordsGenerated", "int", 8, 0, isNullable: false, parms.Messages);
		}
	}
}
