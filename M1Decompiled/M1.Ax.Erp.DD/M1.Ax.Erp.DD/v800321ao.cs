using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.321", "Add fields to FORM941YEARQUARTERS table", "2015-05-19")]
public class v800321ao
{
	public v800321ao(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FORM941YEARQUARTERS", "ptqTaxableAdditionalMedicare"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FORM941YEARQUARTERS", "ptqTaxableAdditionalMedicare", "money", 10, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FORM941YEARQUARTERS", "ptqAdditionalMedicareRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FORM941YEARQUARTERS", "ptqAdditionalMedicareRate", "numeric", 5, 4, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FORM941YEARQUARTERS", "ptqAdditionalMedicareTax"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FORM941YEARQUARTERS", "ptqAdditionalMedicareTax", "money", 10, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FORM941YEARQUARTERS", "ptqTaxDueOnUnreportedTips"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FORM941YEARQUARTERS", "ptqTaxDueOnUnreportedTips", "money", 10, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
