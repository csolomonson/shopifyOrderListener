using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Parts to support unicode", "2013-10-17")]
public class v810RebuildParts
{
	public v810RebuildParts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Parts", new DmoField[29]
		{
			new DmoField("impPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("impShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("impLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("impLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("impPartType", "tinyint", 1, 0, nullable: false),
			new DmoField("impPartGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("impPartClassID", "nvarchar", 5, 0, nullable: false),
			new DmoField("impCycleCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("impNonStockedItem", "bit", 1, 0, nullable: false),
			new DmoField("impOEMOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("impWebSellableToAll", "bit", 1, 0, nullable: false),
			new DmoField("impAlwaysNonTaxable", "bit", 1, 0, nullable: false),
			new DmoField("impSecondTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("impTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("impNonTaxReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("impContractLength", "smallint", 4, 0, nullable: false),
			new DmoField("impContractLengthType", "nvarchar", 1, 0, nullable: false),
			new DmoField("impDeliveryType", "tinyint", 1, 0, nullable: false),
			new DmoField("impTrackSerialNumbers", "bit", 1, 0, nullable: false),
			new DmoField("impNextSerialNumberIDFormula", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("impTrackLotNumbers", "bit", 1, 0, nullable: false),
			new DmoField("impNonPhysicalShipment", "bit", 1, 0, nullable: false),
			new DmoField("impPhantomOrKitPart", "bit", 1, 0, nullable: false),
			new DmoField("impBuyForInventory", "bit", 1, 0, nullable: false),
			new DmoField("impInactive", "bit", 1, 0, nullable: false),
			new DmoField("impInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("impCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("impCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("impUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[10]
		{
			new DmoIndex("IMPPARTID", unique: true),
			new DmoIndex("IMPUNIQUEID", unique: true),
			new DmoIndex("impPartType", unique: false),
			new DmoIndex("impPartClassID", unique: false),
			new DmoIndex("impCycleCodeID", unique: false),
			new DmoIndex("impOEMOrganizationID", unique: false),
			new DmoIndex("impWebSellableToAll", unique: false),
			new DmoIndex("impSecondTaxCodeID", unique: false),
			new DmoIndex("impTaxCodeID", unique: false),
			new DmoIndex("impInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
