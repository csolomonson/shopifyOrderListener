using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.771", "Add fields to InspectionComponents table", "2018-08-20")]
public class v92771b
{
	public v92771b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionComponents", "qamManualInspectionFinalized"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionComponents", "qamManualInspectionFinalized", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
