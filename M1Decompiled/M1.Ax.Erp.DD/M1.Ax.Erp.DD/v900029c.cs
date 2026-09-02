using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.029", "Add fields to PurchaseOrderLines table", "2015-04-10")]
public class v900029c
{
	public v900029c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchaseOrderLines", "pmlIntraCompanyPosted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderLines", "pmlIntraCompanyPosted", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
