using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.301", "Add new leave type ID and Description fields for Payroll Header Totals table", "2022-07-11")]
public class v95301c
{
	public v95301c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollHeaderTotals", "pagAusLeaveTypeID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaderTotals", "pagAusLeaveTypeID", "nvarchar", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollHeaderTotals", "pagAusLeaveTypeDescription"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaderTotals", "pagAusLeaveTypeDescription", "nvarchar", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
