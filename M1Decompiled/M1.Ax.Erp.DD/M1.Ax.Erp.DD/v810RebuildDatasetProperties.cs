using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert DatasetProperties to support unicode", "2013-10-17")]
public class v810RebuildDatasetProperties
{
	public v810RebuildDatasetProperties(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", new DmoField[64]
		{
			new DmoField("xadDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xadUpgradeVersions", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xadVersion", "nvarchar", 10, 0, nullable: false),
			new DmoField("xadExtensionVersions", "nvarchar(max)", 4, 0, nullable: true),
			new DmoField("xadColor", "int", 8, 0, nullable: false),
			new DmoField("xadForeColor", "int", 8, 0, nullable: false),
			new DmoField("xadName", "nvarchar", 50, 0, nullable: false),
			new DmoField("xadAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("xadAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("xadAddressLine3", "nvarchar", 50, 0, nullable: false),
			new DmoField("xadCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("xadState", "nvarchar", 3, 0, nullable: false),
			new DmoField("xadPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("xadCountry", "nvarchar", 20, 0, nullable: false),
			new DmoField("xadLanguage", "nvarchar", 10, 0, nullable: false),
			new DmoField("xadRegion", "nvarchar", 3, 0, nullable: false),
			new DmoField("xadCountryCode", "nvarchar", 5, 0, nullable: false),
			new DmoField("xadCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xadTimeFormat", "tinyint", 1, 0, nullable: false),
			new DmoField("xadGLDivisionID", "nvarchar", 3, 0, nullable: false),
			new DmoField("xadGLChartPrefix", "nvarchar", 2, 0, nullable: false),
			new DmoField("xadGLDepartmentID", "nvarchar", 3, 0, nullable: false),
			new DmoField("xadPhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("xadFaxNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("xadEmailAddress", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xadMailServer", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xadFederalID", "nvarchar", 20, 0, nullable: false),
			new DmoField("xadSellQuantityDecimals", "tinyint", 1, 0, nullable: false),
			new DmoField("xadBuyQuantityDecimals", "tinyint", 1, 0, nullable: false),
			new DmoField("xadInventoryQuantityDecimals", "tinyint", 1, 0, nullable: false),
			new DmoField("xadEditInExplorers", "bit", 1, 0, nullable: false),
			new DmoField("xadExtendedSearchOptions", "bit", 1, 0, nullable: false),
			new DmoField("xadExportFollowups", "bit", 1, 0, nullable: false),
			new DmoField("xadRequireWarehouse", "bit", 1, 0, nullable: false),
			new DmoField("xadBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xadCreditCardBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xadCompanyMessageRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xadCompanyMessageText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xadCompanyLogo", "image", 4, 0, nullable: true),
			new DmoField("xadSuppressAddressOnReports", "bit", 1, 0, nullable: false),
			new DmoField("xadWebGearWebsiteURL", "nvarchar", 50, 0, nullable: false),
			new DmoField("xadDisableSerialNumbers", "bit", 1, 0, nullable: false),
			new DmoField("xadDisableLotNumbers", "bit", 1, 0, nullable: false),
			new DmoField("xadDisableWarehouses", "bit", 1, 0, nullable: false),
			new DmoField("xadDisableOrganizationParts", "bit", 1, 0, nullable: false),
			new DmoField("xadDisableRevisions", "bit", 1, 0, nullable: false),
			new DmoField("xadDisablePOSRetail", "bit", 1, 0, nullable: false),
			new DmoField("xadDisablePOSManufacturing", "bit", 1, 0, nullable: false),
			new DmoField("xadDisableRetention", "bit", 1, 0, nullable: false),
			new DmoField("xadUPSInterfaceFolderName", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xadBackupCheck", "bit", 1, 0, nullable: false),
			new DmoField("xadViewForeign", "bit", 1, 0, nullable: false),
			new DmoField("xadEnableNonNettable", "bit", 1, 0, nullable: false),
			new DmoField("xadPayrollBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xadCATransmitterNumber", "nvarchar", 8, 0, nullable: false),
			new DmoField("xadCASubmissionReference", "int", 8, 0, nullable: false),
			new DmoField("xadWebAddress", "nvarchar", 100, 0, nullable: false),
			new DmoField("xadCARPPRegistrationNumber", "nvarchar", 7, 0, nullable: false),
			new DmoField("xadMaxGridRow", "int", 7, 0, nullable: false),
			new DmoField("xadCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xadCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xadUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("xadIgnoreSSLCertValidate", "bit", 1, 0, nullable: false),
			new DmoField("xadTINType", "nvarchar", 20, 0, nullable: false)
		}, new DmoIndex[1]
		{
			new DmoIndex("XADUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
