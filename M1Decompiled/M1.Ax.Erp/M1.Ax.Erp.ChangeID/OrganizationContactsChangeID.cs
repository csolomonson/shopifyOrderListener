using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("OrganizationContacts")]
public class OrganizationContactsChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
		parm.Database.ExecuteCommand("UPDATE Organizations SET CMOARINVOICECONTACTID = '' WHERE CMOORGANIZATIONID = " + parm.OldKeyValues[0].ToSql() + " And CMOARINVOICECONTACTID = " + parm.OldKeyValues[2].ToSql(), parm.SqlTransaction);
		parm.Database.ExecuteCommand("UPDATE Organizations SET CMOQUOTECONTACTID = '' WHERE CMOORGANIZATIONID = " + parm.OldKeyValues[0].ToSql() + " And CMOQUOTECONTACTID = " + parm.OldKeyValues[2].ToSql(), parm.SqlTransaction);
		parm.Database.ExecuteCommand("UPDATE Organizations SET CMOSHIPCONTACTID = '' WHERE CMOORGANIZATIONID = " + parm.OldKeyValues[0].ToSql() + " And CMOSHIPCONTACTID = " + parm.OldKeyValues[2].ToSql(), parm.SqlTransaction);
		parm.Database.ExecuteCommand("UPDATE Organizations SET CMOPURCHASECONTACTID = '' WHERE CMOORGANIZATIONID = " + parm.OldKeyValues[0].ToSql() + " And CMOPURCHASECONTACTID = " + parm.OldKeyValues[2].ToSql(), parm.SqlTransaction);
		parm.Database.ExecuteCommand("UPDATE Organizations SET CMOAPINVOICECONTACTID = '' WHERE CMOORGANIZATIONID = " + parm.OldKeyValues[0].ToSql() + " And CMOAPINVOICECONTACTID = " + parm.OldKeyValues[2].ToSql(), parm.SqlTransaction);
		parm.Database.ExecuteCommand("UPDATE OrganizationLocations SET CMLQUOTECONTACTID = '' WHERE CMLORGANIZATIONID = " + parm.OldKeyValues[0].ToSql() + " And CMLLOCATIONID = " + parm.OldKeyValues[1].ToSql() + " And CMLQUOTECONTACTID = " + parm.OldKeyValues[2].ToSql(), parm.SqlTransaction);
		parm.Database.ExecuteCommand("UPDATE OrganizationLocations SET CMLSHIPCONTACTID = '' WHERE CMLORGANIZATIONID = " + parm.OldKeyValues[0].ToSql() + " And CMLLOCATIONID = " + parm.OldKeyValues[1].ToSql() + " And CMLSHIPCONTACTID = " + parm.OldKeyValues[2].ToSql(), parm.SqlTransaction);
		parm.Database.ExecuteCommand("UPDATE OrganizationLocations SET CMLPURCHASECONTACTID = '' WHERE CMLORGANIZATIONID = " + parm.OldKeyValues[0].ToSql() + " And CMLLOCATIONID = " + parm.OldKeyValues[1].ToSql() + " And CMLPURCHASECONTACTID = " + parm.OldKeyValues[2].ToSql(), parm.SqlTransaction);
		parm.Database.ExecuteCommand("UPDATE OrganizationLocations SET CMLARINVOICECONTACTID = '' WHERE CMLORGANIZATIONID = " + parm.OldKeyValues[0].ToSql() + " And CMLLOCATIONID = " + parm.OldKeyValues[1].ToSql() + " And CMLARINVOICECONTACTID = " + parm.OldKeyValues[2].ToSql(), parm.SqlTransaction);
		parm.Database.ExecuteCommand("UPDATE OrganizationLocations SET CMLAPINVOICECONTACTID = '' WHERE CMLORGANIZATIONID = " + parm.OldKeyValues[0].ToSql() + " And CMLLOCATIONID = " + parm.OldKeyValues[1].ToSql() + " And CMLAPINVOICECONTACTID = " + parm.OldKeyValues[2].ToSql(), parm.SqlTransaction);
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
	}
}
