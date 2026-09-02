using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.028", "Add fields to PartClasses table", "2015-04-08")]
public class v900028d
{
	public v900028d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartClasses", "imcPickingMethod"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartClasses", "imcPickingMethod", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PartClasses Set imcPickingMethod = 1");
		}
	}
}
