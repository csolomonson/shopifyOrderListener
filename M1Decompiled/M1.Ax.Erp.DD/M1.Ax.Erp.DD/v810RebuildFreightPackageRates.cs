using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert FreightPackageRates to support unicode", "2013-10-17")]
public class v810RebuildFreightPackageRates
{
	public v810RebuildFreightPackageRates(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FreightPackageRates", new DmoField[31]
		{
			new DmoField("fprFreightShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fprFreightPackageID", "smallint", 4, 0, nullable: false),
			new DmoField("fprFreightPackageRateID", "smallint", 4, 0, nullable: false),
			new DmoField("fprRCTI", "nvarchar", 40, 0, nullable: false),
			new DmoField("fprFdxService", "nvarchar", 35, 0, nullable: false),
			new DmoField("fprFdxPackaging", "nvarchar", 35, 0, nullable: false),
			new DmoField("fprFdxDeliveryDate", "datetime", 14, 0, nullable: true),
			new DmoField("fprFdxDeliveryDay", "nvarchar", 3, 0, nullable: false),
			new DmoField("fprFdxDestinationStationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("fprFdxTimeInTransit", "smallint", 3, 0, nullable: false),
			new DmoField("fprFdxTotalBillingWeight", "numeric", 13, 2, nullable: false),
			new DmoField("fprFdxTotalDimWeight", "numeric", 13, 2, nullable: false),
			new DmoField("fprFdxTotalFreightDiscount", "numeric", 13, 2, nullable: false),
			new DmoField("fprFdxTotalSurcharges", "numeric", 13, 2, nullable: false),
			new DmoField("fprFdxTotalNetCharge", "numeric", 13, 2, nullable: false),
			new DmoField("fprFdxTotalNetFreightCharge", "numeric", 13, 2, nullable: false),
			new DmoField("fprFdxPackageBillingWeight", "numeric", 13, 2, nullable: false),
			new DmoField("fprFdxPackageDimWeight", "numeric", 13, 2, nullable: false),
			new DmoField("fprFdxPackageBaseCharge", "numeric", 13, 2, nullable: false),
			new DmoField("fprFdxPackageNetCharge", "numeric", 13, 2, nullable: false),
			new DmoField("fprFdxPackageNetFreight", "numeric", 13, 2, nullable: false),
			new DmoField("fprFdxPackageSurcharges", "numeric", 13, 2, nullable: false),
			new DmoField("fprFdxPackageFreightDiscount", "numeric", 13, 2, nullable: false),
			new DmoField("fprFdxCurrency", "nvarchar", 3, 0, nullable: false),
			new DmoField("fprFdxUnits", "nvarchar", 3, 0, nullable: false),
			new DmoField("fprFdxBaseCharge", "numeric", 13, 2, nullable: false),
			new DmoField("fprFdxVariableHandlingCharge", "numeric", 13, 2, nullable: false),
			new DmoField("fprFdxTotalCustomerCharge", "numeric", 13, 2, nullable: false),
			new DmoField("fprCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("fprCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("fprUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("FPRFREIGHTSHIPMENTID,FPRFREIGHTPACKAGEID,FPRFREIGHTPACKAGERATEID", unique: true),
			new DmoIndex("FPRUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
