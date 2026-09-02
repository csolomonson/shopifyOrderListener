using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.178", "Add fields to GLCharts table", "2017-03-02")]
public class v92178a
{
	public v92178a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLCharts", "glcCOGSAccountType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLCharts", "glcCOGSAccountType", "tinyint", 2, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
