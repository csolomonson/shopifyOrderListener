using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert BankAccounts to support unicode", "2013-10-17")]
public class v810RebuildBankAccounts
{
	public v810RebuildBankAccounts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "BankAccounts", new DmoField[37]
		{
			new DmoField("glnBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("glnDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("glnOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("glnCashGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("glnBankName", "nvarchar", 30, 0, nullable: false),
			new DmoField("glnBankInitials", "nvarchar", 3, 0, nullable: false),
			new DmoField("glnBSBNumber", "nvarchar", 10, 0, nullable: false),
			new DmoField("glnBankAccountName", "nvarchar", 50, 0, nullable: false),
			new DmoField("glnBankAccountNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("glnDirectEntryUserName", "nvarchar", 30, 0, nullable: false),
			new DmoField("glnDirectEntryUserID", "nvarchar", 6, 0, nullable: false),
			new DmoField("glnEFTCompanyName", "nvarchar", 30, 0, nullable: false),
			new DmoField("glnEFTCompanyID", "nvarchar", 10, 0, nullable: false),
			new DmoField("glnEFTFileID", "nvarchar", 10, 0, nullable: false),
			new DmoField("glnEFTFileIDModifier", "nvarchar", 1, 0, nullable: false),
			new DmoField("glnEFTReferenceCode", "nvarchar", 8, 0, nullable: false),
			new DmoField("glnEFTDiscretionaryData", "nvarchar", 20, 0, nullable: false),
			new DmoField("glnEFTAPDescription", "nvarchar", 20, 0, nullable: false),
			new DmoField("glnEFTPayrollDescription", "nvarchar", 20, 0, nullable: false),
			new DmoField("glnEFTCreateOffsettingDebit", "bit", 1, 0, nullable: false),
			new DmoField("glnEFTFileLocation", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("glnCanadianEFTType", "nvarchar", 5, 0, nullable: false),
			new DmoField("glnFileCreationNumber", "smallint", 4, 0, nullable: false),
			new DmoField("glnNextPaymentNumber", "int", 6, 0, nullable: false),
			new DmoField("glnNextEFTNumber", "int", 6, 0, nullable: false),
			new DmoField("glnInactive", "bit", 1, 0, nullable: false),
			new DmoField("glnInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("glnACHFormat", "bit", 1, 0, nullable: false),
			new DmoField("glnPayrollOnly", "bit", 1, 0, nullable: false),
			new DmoField("glnCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("glnNZEFTType", "nvarchar", 5, 0, nullable: false),
			new DmoField("glnCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("glnCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("glnUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("glnBIC", "nvarchar", 50, 0, nullable: false),
			new DmoField("glnIBAN", "nvarchar", 50, 0, nullable: false),
			new DmoField("glnDataCenterCode", "numeric", 5, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("GLNBANKACCOUNTID", unique: true),
			new DmoIndex("GLNUNIQUEID", unique: true),
			new DmoIndex("glnOrganizationID", unique: false),
			new DmoIndex("glnCashGLAccountID", unique: false),
			new DmoIndex("glnInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
