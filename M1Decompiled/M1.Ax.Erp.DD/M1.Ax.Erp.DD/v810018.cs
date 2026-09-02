using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.018", "Add Column cmpSAGEGUID to OrganizationPaymentMethods table.", "2012-12-04")]
public class v810018
{
	public v810018(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationPaymentMethods", "cmpSageGUID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationPaymentMethods", "cmpSageGUID", "uniqueidentifier", 16, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
