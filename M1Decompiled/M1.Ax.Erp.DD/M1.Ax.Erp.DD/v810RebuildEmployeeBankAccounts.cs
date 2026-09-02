using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeeBankAccounts to support unicode", "2013-10-17")]
public class v810RebuildEmployeeBankAccounts
{
	public v810RebuildEmployeeBankAccounts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeBankAccounts", new DmoField[19]
		{
			new DmoField("pabEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pabEmployeeBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pabDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("pabBankAccountType", "nvarchar", 2, 0, nullable: false),
			new DmoField("pabBankInitials", "nvarchar", 3, 0, nullable: false),
			new DmoField("pabBSBNumber", "nvarchar", 10, 0, nullable: false),
			new DmoField("pabBankAccountName", "nvarchar", 50, 0, nullable: false),
			new DmoField("pabBankAccountNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("pabCalculationMethod", "tinyint", 1, 0, nullable: false),
			new DmoField("pabPercent", "numeric", 8, 4, nullable: false),
			new DmoField("pabAmount", "money", 10, 2, nullable: false),
			new DmoField("pabInactive", "bit", 1, 0, nullable: false),
			new DmoField("pabInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("pabCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pabCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pabUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("pabEFTDescription", "nvarchar", 20, 0, nullable: false),
			new DmoField("pabIBAN", "nvarchar", 50, 0, nullable: false),
			new DmoField("pabBIC", "nvarchar", 50, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("PABEMPLOYEEID,PABEMPLOYEEBANKACCOUNTID", unique: true),
			new DmoIndex("PABUNIQUEID", unique: true),
			new DmoIndex("pabEmployeeID", unique: false),
			new DmoIndex("pabEmployeeBankAccountID", unique: false),
			new DmoIndex("pabInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
