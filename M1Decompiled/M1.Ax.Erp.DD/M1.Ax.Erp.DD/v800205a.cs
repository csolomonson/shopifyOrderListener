using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Add Reason Plants tables and related fields", "2011-12-06")]
public class v800205a
{
	public v800205a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ReasonPlants"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReasonPlants");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Reasons", "xarReasonGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Reasons", "xarReasonGLAccountID", "char", 11, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
