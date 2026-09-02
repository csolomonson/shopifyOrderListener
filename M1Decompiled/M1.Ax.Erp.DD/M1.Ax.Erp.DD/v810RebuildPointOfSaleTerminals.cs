using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PointOfSaleTerminals to support unicode", "2013-10-17")]
public class v810RebuildPointOfSaleTerminals
{
	public v810RebuildPointOfSaleTerminals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PointOfSaleTerminals", new DmoField[13]
		{
			new DmoField("armPointOfSaleTerminalID", "nvarchar", 5, 0, nullable: false),
			new DmoField("armDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("armMachineName", "nvarchar", 50, 0, nullable: false),
			new DmoField("armPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("armPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("armInactivityLogoutMinutes", "tinyint", 2, 0, nullable: false),
			new DmoField("armCashDrawer", "bit", 1, 0, nullable: false),
			new DmoField("armARPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("armInactive", "bit", 1, 0, nullable: false),
			new DmoField("armInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("armCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("armCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("armUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("ARMPOINTOFSALETERMINALID", unique: true),
			new DmoIndex("ARMUNIQUEID", unique: true),
			new DmoIndex("armMachineName", unique: false),
			new DmoIndex("armPlantDepartmentID", unique: false),
			new DmoIndex("armPlantID", unique: false),
			new DmoIndex("armARPaymentSessionID", unique: false),
			new DmoIndex("armInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
