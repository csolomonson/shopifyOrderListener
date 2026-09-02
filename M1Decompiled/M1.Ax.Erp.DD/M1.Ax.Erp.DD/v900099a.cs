using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.099", "Rename fields in PartRevisions table", "2015-10-29")]
public class v900099a
{
	public v900099a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrPurchaseableItem"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrPurchaseableItem", "imrPurchasableItem", dropTriggers: true);
		}
	}
}
