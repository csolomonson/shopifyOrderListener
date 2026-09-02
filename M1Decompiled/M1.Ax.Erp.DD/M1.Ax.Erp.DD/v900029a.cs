using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.029", "Add fields to PurchaseOrderComponents table", "2015-04-10")]
public class v900029a
{
	public v900029a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchaseOrderComponents", "pmoIntraCompanyPosted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderComponents", "pmoIntraCompanyPosted", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
