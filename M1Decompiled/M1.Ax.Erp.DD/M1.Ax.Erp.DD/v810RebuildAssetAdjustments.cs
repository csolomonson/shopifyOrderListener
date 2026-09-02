using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert AssetAdjustments to support unicode", "2013-10-17")]
public class v810RebuildAssetAdjustments
{
	public v810RebuildAssetAdjustments(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AssetAdjustments", new DmoField[32]
		{
			new DmoField("faaAssetAdjustmentID", "int", 9, 0, nullable: false),
			new DmoField("faaAdjustmentType", "nvarchar", 1, 0, nullable: false),
			new DmoField("faaAssetID", "nvarchar", 10, 0, nullable: false),
			new DmoField("faaQuantity", "int", 9, 0, nullable: false),
			new DmoField("faaOpeningAssetValue", "money", 12, 2, nullable: false),
			new DmoField("faaAccumulatedDepreciation", "money", 12, 2, nullable: false),
			new DmoField("faaDepreciationThisYear", "money", 12, 2, nullable: false),
			new DmoField("faaClosingPeriodDepreciation", "money", 12, 2, nullable: false),
			new DmoField("faaClosingPercent", "numeric", 6, 2, nullable: false),
			new DmoField("faaNetAssetValue", "money", 12, 2, nullable: false),
			new DmoField("faaValue", "money", 12, 2, nullable: false),
			new DmoField("faaProfitOrLoss", "money", 12, 2, nullable: false),
			new DmoField("faaAdjustmentDate", "date", 14, 0, nullable: true),
			new DmoField("faaGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("faaGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("faaAuthorizedByEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("faaSourcePlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("faaDestinationPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("faaLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("faaLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("faaCustomerOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("faaARInvoiceLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("faaARInvoiceContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("faaCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("faaCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("faaExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("faaValueForeign", "numeric", 12, 2, nullable: false),
			new DmoField("faaPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("faaPostedDate", "date", 14, 0, nullable: true),
			new DmoField("faaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("faaCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("faaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[11]
		{
			new DmoIndex("FAAASSETADJUSTMENTID", unique: true),
			new DmoIndex("FAAUNIQUEID", unique: true),
			new DmoIndex("faaAdjustmentType", unique: false),
			new DmoIndex("faaAssetID", unique: false),
			new DmoIndex("faaAdjustmentDate", unique: false),
			new DmoIndex("faaAuthorizedByEmployeeID", unique: false),
			new DmoIndex("faaSourcePlantID", unique: false),
			new DmoIndex("faaDestinationPlantID", unique: false),
			new DmoIndex("faaCustomerOrganizationID", unique: false),
			new DmoIndex("faaARInvoiceLocationID", unique: false),
			new DmoIndex("faaPostedToGL", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
