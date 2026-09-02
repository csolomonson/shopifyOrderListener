using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.771", "Add fields to InspectionLines table", "2018-08-20")]
public class v92771a
{
	public v92771a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalManualInspectionFinalized"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalManualInspectionFinalized", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
