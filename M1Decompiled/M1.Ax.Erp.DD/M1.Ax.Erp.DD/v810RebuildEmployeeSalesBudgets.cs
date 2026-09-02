using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeeSalesBudgets to support unicode", "2013-10-17")]
public class v810RebuildEmployeeSalesBudgets
{
	public v810RebuildEmployeeSalesBudgets(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeSalesBudgets", new DmoField[6]
		{
			new DmoField("lnsEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lnsSalesBudgetYearID", "smallint", 4, 0, nullable: false),
			new DmoField("lnsStartDate", "date", 14, 0, nullable: true),
			new DmoField("lnsEndDate", "date", 14, 0, nullable: true),
			new DmoField("lnsAnnualAmount", "money", 12, 2, nullable: false),
			new DmoField("lnsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("LNSEMPLOYEEID,LNSSALESBUDGETYEARID", unique: true),
			new DmoIndex("LNSUNIQUEID", unique: true),
			new DmoIndex("lnsEmployeeID", unique: false),
			new DmoIndex("lnsSalesBudgetYearID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
