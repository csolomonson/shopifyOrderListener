using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ShippingPaymentTypes to support unicode", "2013-10-17")]
public class v810RebuildShippingPaymentTypes
{
	public v810RebuildShippingPaymentTypes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingPaymentTypes", new DmoField[8]
		{
			new DmoField("xayShippingPaymentTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xayDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xayInactive", "bit", 1, 0, nullable: false),
			new DmoField("xayInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("xayDoNotXferShipCostsToAR", "bit", 1, 0, nullable: false),
			new DmoField("xayCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xayCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xayUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("XAYSHIPPINGPAYMENTTYPEID", unique: true),
			new DmoIndex("XAYUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
