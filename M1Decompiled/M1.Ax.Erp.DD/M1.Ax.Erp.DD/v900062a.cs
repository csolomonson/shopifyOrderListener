using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.062", "Change fields in Form940YearTotals table", "2015-07-23")]
public class v900062a
{
	public v900062a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form940YearTotals", "pftSignDate"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form940YearTotals", "pftSignDate", "date", 14, 0, isNullable: true, parms.Messages);
		}
	}
}
