using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.582", "Add fields to FinancialProperties table", "2015-06-23")]
public class v800582j
{
	public v800582j(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAvalaraCanadaPSTTaxCodeID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAvalaraCanadaPSTTaxCodeID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAvalaraCanadaQSTTaxCodeID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAvalaraCanadaQSTTaxCodeID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAvalaraCanadaHSTTaxCodeID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAvalaraCanadaHSTTaxCodeID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAvalaraCanadaGSTTaxCodeID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAvalaraCanadaGSTTaxCodeID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
