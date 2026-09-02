using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.094", "Add parStdDeductionLowerLimit to IncomeTaxTableRevisions table", "2011-01-27")]
public class v800094b
{
	public v800094b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxTableRevisions", "parStdDeductionLowerLimit"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTableRevisions", "parStdDeductionLowerLimit", "money", 10, 2, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
