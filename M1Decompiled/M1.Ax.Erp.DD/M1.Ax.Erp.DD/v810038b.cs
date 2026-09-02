using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.038", "Add fields to OrganizationPaymentMethods", "2013-09-19")]
public class v810038b
{
	public v810038b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationPaymentMethods", "cmpInactiveDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationPaymentMethods", "cmpInactiveDate", "date", 14, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationPaymentMethods", "cmpCardDescription"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationPaymentMethods", "cmpCardDescription", "char", 50, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE OrganizationPaymentMethods SET cmpInactive = 1, cmpInactiveDate = GETDATE() WHERE cmpPaymentType = 'CC'");
	}
}
