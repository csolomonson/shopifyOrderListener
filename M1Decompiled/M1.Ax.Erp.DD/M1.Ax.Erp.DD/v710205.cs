using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.205", "Resize Deduction ID in DeductionPayRateExemptions", "2009-01-12")]
public class v710205
{
	public v710205(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DeductionPayRateExemptions", "lndDeductionID"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DeductionPayRateExemptions", "lndDeductionID", "char", 10, 0, parms.Messages);
		}
		parms.Dmo.VerifyIndexesOnTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DeductionPayRateExemptions", parms.Messages, null);
	}
}
