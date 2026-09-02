using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.094", "Add ABN and Logo to Plants", "2010-12-21")]
public class v800094a
{
	public v800094a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauFederalID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauFederalID", "char", 20, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauCompanyLogo"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauCompanyLogo", "image", 4, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
