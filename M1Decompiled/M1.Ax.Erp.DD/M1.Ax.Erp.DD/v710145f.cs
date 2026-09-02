using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.145", "Add Part Class Plants and COGS accounts", "2008-08-04")]
public class v710145f
{
	public v710145f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartClasses", "imcInventoryGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartClasses", "imcInventoryGLAccountID", "char", 11, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PartClassPlants"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartClassPlants");
		}
	}
}
