using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.035", "Add Inactive flag to Tax Codes", "2008-07-08")]
public class v710035
{
	public v710035(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "TaxCodes", "xaxInactive"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TaxCodes", "xaxInactive", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "TaxCodes", "xaxInactiveDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TaxCodes", "xaxInactiveDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
