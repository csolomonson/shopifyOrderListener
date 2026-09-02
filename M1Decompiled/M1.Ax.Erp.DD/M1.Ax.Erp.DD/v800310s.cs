using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.310", "Add fields to EMPLOYEEPERSONALDATA table", "2015-05-19")]
public class v800310s
{
	public v800310s(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EMPLOYEEPERSONALDATA", "lmdNZTaxCode"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EMPLOYEEPERSONALDATA", "lmdNZTaxCode", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
