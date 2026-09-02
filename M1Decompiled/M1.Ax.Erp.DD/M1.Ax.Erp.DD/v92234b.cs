using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.234", "Remove tariff field from PartRevisions table", "2017-04-28")]
public class v92234b
{
	public v92234b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrTariffID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrTariffID", dropTriggers: true);
		}
	}
}
