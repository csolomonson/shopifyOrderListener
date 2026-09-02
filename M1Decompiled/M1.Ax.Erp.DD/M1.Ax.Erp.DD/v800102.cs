using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.102", "Add fields to IncomeTaxTableRevisions table", "2011-02-09")]
public class v800102
{
	public v800102(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxTableRevisions", "parTaxCreditReductionPercent"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTableRevisions", "parTaxCreditReductionPercent", "numeric", 8, 4, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxTableRevisions", "parTaxCreditExcessAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTableRevisions", "parTaxCreditExcessAmount", "money", 10, 2, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
