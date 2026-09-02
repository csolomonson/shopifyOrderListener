using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeeAllowances to support unicode", "2013-10-17")]
public class v810RebuildEmployeeAllowances
{
	public v810RebuildEmployeeAllowances(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeAllowances", new DmoField[29]
		{
			new DmoField("pawEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pawEmployeeAllowanceID", "smallint", 4, 0, nullable: false),
			new DmoField("pawAllowanceID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pawMethod", "tinyint", 1, 0, nullable: false),
			new DmoField("pawAllowanceTaxMethod", "tinyint", 1, 0, nullable: false),
			new DmoField("pawAmount", "money", 10, 2, nullable: false),
			new DmoField("pawPercent", "numeric", 8, 4, nullable: false),
			new DmoField("pawReference", "nvarchar", 30, 0, nullable: false),
			new DmoField("pawExpenseGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("pawAccrualGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("pawOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pawAllowanceStartDate", "date", 14, 0, nullable: true),
			new DmoField("pawAllowanceEndDate", "date", 14, 0, nullable: true),
			new DmoField("pawPeriod1", "bit", 1, 0, nullable: false),
			new DmoField("pawPeriod2", "bit", 1, 0, nullable: false),
			new DmoField("pawPeriod3", "bit", 1, 0, nullable: false),
			new DmoField("pawPeriod4", "bit", 1, 0, nullable: false),
			new DmoField("pawPeriod5", "bit", 1, 0, nullable: false),
			new DmoField("pawPeriod6", "bit", 1, 0, nullable: false),
			new DmoField("pawRate", "numeric", 8, 4, nullable: false),
			new DmoField("pawInactive", "bit", 1, 0, nullable: false),
			new DmoField("pawInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("pawAUSReportablePercent", "numeric", 8, 4, nullable: false),
			new DmoField("pawCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pawCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pawUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("pawMemberID", "nvarchar", 20, 0, nullable: false),
			new DmoField("pawEmployerAddContrib", "bit", 1, 0, nullable: false),
			new DmoField("pawSuperannuationFundID", "nvarchar", 10, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("PAWEMPLOYEEID,PAWEMPLOYEEALLOWANCEID", unique: true),
			new DmoIndex("PAWUNIQUEID", unique: true),
			new DmoIndex("pawEmployeeID", unique: false),
			new DmoIndex("pawEmployeeAllowanceID", unique: false),
			new DmoIndex("pawOrganizationID", unique: false),
			new DmoIndex("pawInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
