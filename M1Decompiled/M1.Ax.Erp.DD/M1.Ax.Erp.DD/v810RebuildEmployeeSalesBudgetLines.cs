using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeeSalesBudgetLines to support unicode", "2013-10-17")]
public class v810RebuildEmployeeSalesBudgetLines
{
	public v810RebuildEmployeeSalesBudgetLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeSalesBudgetLines", new DmoField[7]
		{
			new DmoField("lnlEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lnlSalesBudgetYearID", "smallint", 4, 0, nullable: false),
			new DmoField("lnlSalesBudgetPeriodID", "smallint", 4, 0, nullable: false),
			new DmoField("lnlStartDate", "date", 14, 0, nullable: true),
			new DmoField("lnlEndDate", "date", 14, 0, nullable: true),
			new DmoField("lnlBudgetAmount", "money", 12, 2, nullable: false),
			new DmoField("lnlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("LNLEMPLOYEEID,LNLSALESBUDGETYEARID,LNLSALESBUDGETPERIODID", unique: true),
			new DmoIndex("LNLUNIQUEID", unique: true),
			new DmoIndex("lnlEmployeeID", unique: false),
			new DmoIndex("lnlSalesBudgetYearID", unique: false),
			new DmoIndex("lnlSalesBudgetPeriodID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
