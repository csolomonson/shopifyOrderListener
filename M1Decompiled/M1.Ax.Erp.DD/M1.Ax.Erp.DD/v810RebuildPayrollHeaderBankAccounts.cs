using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollHeaderBankAccounts to support unicode", "2013-10-17")]
public class v810RebuildPayrollHeaderBankAccounts
{
	public v810RebuildPayrollHeaderBankAccounts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaderBankAccounts", new DmoField[17]
		{
			new DmoField("paaPayrollSessionID", "int", 9, 0, nullable: false),
			new DmoField("paaPayrollHeaderID", "int", 7, 0, nullable: false),
			new DmoField("paaPayrollHeaderBankAccountID", "smallint", 4, 0, nullable: false),
			new DmoField("paaEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("paaEmployeeBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paaBankAccountType", "nvarchar", 2, 0, nullable: false),
			new DmoField("paaBankInitials", "nvarchar", 3, 0, nullable: false),
			new DmoField("paaBSBNumber", "nvarchar", 10, 0, nullable: false),
			new DmoField("paaBankAccountName", "nvarchar", 50, 0, nullable: false),
			new DmoField("paaBankAccountNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("paaAmount", "money", 10, 2, nullable: false),
			new DmoField("paaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("paaCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("paaUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("paaEFTDescription", "nvarchar", 20, 0, nullable: false),
			new DmoField("paaIBAN", "nvarchar", 50, 0, nullable: false),
			new DmoField("paaBIC", "nvarchar", 50, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("PAAPAYROLLSESSIONID,PAAPAYROLLHEADERID,PAAPAYROLLHEADERBANKACCOUNTID", unique: true),
			new DmoIndex("PAAUNIQUEID", unique: true),
			new DmoIndex("paaPayrollSessionID", unique: false),
			new DmoIndex("paaPayrollHeaderID", unique: false),
			new DmoIndex("paaPayrollHeaderBankAccountID", unique: false),
			new DmoIndex("paaEmployeeID", unique: false),
			new DmoIndex("paaEmployeeBankAccountID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
