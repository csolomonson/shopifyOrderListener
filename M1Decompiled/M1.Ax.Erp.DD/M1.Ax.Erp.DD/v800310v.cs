using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.310", "Add fields to ARPAYMENTHEADERS table", "2015-05-19")]
public class v800310v
{
	public v800310v(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARPAYMENTHEADERS", "artNet1PaymentProcessed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPAYMENTHEADERS", "artNet1PaymentProcessed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
