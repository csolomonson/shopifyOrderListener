using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.521", "Add fields to IncomeTaxTableRevisions table", "2015-03-05")]
public class v800521b
{
	public v800521b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxTableRevisions", "parThirdDeductTaxLimit"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTableRevisions", "parThirdDeductTaxLimit", "money", 10, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxTableRevisions", "parFourthDeductTaxLimit"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTableRevisions", "parFourthDeductTaxLimit", "money", 10, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
