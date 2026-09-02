using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.600", "Add imjProposedNewPrice to PartPriceBreaks table", "2023-03-29")]
public class v95600
{
	public v95600(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartPriceBreaks", "imjProposedNewPrice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartPriceBreaks", "imjProposedNewPrice", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
