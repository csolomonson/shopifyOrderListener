using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.001", "Drop PartBinDetailCosts table", "2016-10-24")]
public class v92001b
{
	public v92001b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PartBinDetailCosts"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "PartBinDetailCosts");
		}
	}
}
