using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PaymentTerms to support unicode", "2013-10-17")]
public class v810RebuildPaymentTerms
{
	public v810RebuildPaymentTerms(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PaymentTerms", new DmoField[13]
		{
			new DmoField("xatPaymentTermID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xatDescription", "nvarchar", 20, 0, nullable: false),
			new DmoField("xatDaysDue", "smallint", 3, 0, nullable: false),
			new DmoField("xatDiscountDays", "smallint", 3, 0, nullable: false),
			new DmoField("xatDiscountPercent", "numeric", 5, 2, nullable: false),
			new DmoField("xatGracePeriod", "smallint", 3, 0, nullable: false),
			new DmoField("xatCalculationType", "tinyint", 1, 0, nullable: false),
			new DmoField("xatImmediatePaymentRequired", "bit", 1, 0, nullable: false),
			new DmoField("xatCalculationDayOfMonth", "tinyint", 2, 0, nullable: false),
			new DmoField("xatDiscountDayOfMonth", "tinyint", 2, 0, nullable: false),
			new DmoField("xatCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xatCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xatUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("XATPAYMENTTERMID", unique: true),
			new DmoIndex("XATUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
