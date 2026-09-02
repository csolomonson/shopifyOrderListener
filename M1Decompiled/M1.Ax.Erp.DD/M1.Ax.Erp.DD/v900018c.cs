using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.018", "Add Parent Quantity to ShipmentComponents table", "2015-02-19")]
public class v900018c
{
	public v900018c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentComponents", "smoParentQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentComponents", "smoParentQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
