using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.310", "Add fields to EmployeeDeductions table", "2015-06-23")]
public class v800310ss
{
	public v800310ss(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeeDeductions", "paeSuperannuationFundID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeDeductions", "paeSuperannuationFundID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
