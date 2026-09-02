using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.826", "Add use email for payslips field to Employees table", "2020-02-19")]
public class v92826b
{
	public v92826b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Employees", "lmeUseEmailPayslips"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Employees", "lmeUseEmailPayslips", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
