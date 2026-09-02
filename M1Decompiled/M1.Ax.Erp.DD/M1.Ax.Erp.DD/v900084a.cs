using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.084", "Add fields to Organizations table", "2015-09-16")]
public class v900084a
{
	public v900084a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoCustomerShippingCarrier"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoCustomerShippingCarrier", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
