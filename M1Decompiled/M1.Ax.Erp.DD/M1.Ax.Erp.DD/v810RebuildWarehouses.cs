using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Warehouses to support unicode", "2013-10-17")]
public class v810RebuildWarehouses
{
	public v810RebuildWarehouses(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Warehouses", new DmoField[26]
		{
			new DmoField("imwWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imwName", "nvarchar", 50, 0, nullable: false),
			new DmoField("imwPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imwPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imwAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("imwAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("imwAddressLine3", "nvarchar", 50, 0, nullable: false),
			new DmoField("imwCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("imwState", "nvarchar", 3, 0, nullable: false),
			new DmoField("imwPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("imwCountry", "nvarchar", 20, 0, nullable: false),
			new DmoField("imwPhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("imwFaxNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("imwEmailAddress", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("imwEstablishedDate", "date", 14, 0, nullable: true),
			new DmoField("imwDefaultWarehouse", "bit", 1, 0, nullable: false),
			new DmoField("imwShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imwInactive", "bit", 1, 0, nullable: false),
			new DmoField("imwInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("imwNonNettable", "bit", 1, 0, nullable: false),
			new DmoField("imwDoNotIncludeInJobCosts", "bit", 1, 0, nullable: false),
			new DmoField("imwAvalaraAddressValidated", "bit", 1, 0, nullable: false),
			new DmoField("imwCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imwCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imwUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("imwNonNettableType", "tinyint", 1, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("IMWWAREHOUSEID", unique: true),
			new DmoIndex("IMWUNIQUEID", unique: true),
			new DmoIndex("imwName", unique: false),
			new DmoIndex("imwPlantDepartmentID", unique: false),
			new DmoIndex("imwPlantID", unique: false),
			new DmoIndex("imwDefaultWarehouse", unique: false),
			new DmoIndex("imwInactive", unique: false),
			new DmoIndex("imwNonNettable", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
