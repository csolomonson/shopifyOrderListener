using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.012", "Add fields to DMRShipmentComponents table", "2016-11-06")]
public class v92012a
{
	public v92012a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentComponents", "dsoPosted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentComponents", "dsoPosted", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
