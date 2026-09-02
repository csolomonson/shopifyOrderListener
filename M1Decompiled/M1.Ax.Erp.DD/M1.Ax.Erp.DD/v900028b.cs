using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.028", "Add fields to WarehouseBins table", "2015-04-08")]
public class v900028b
{
	public v900028b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "WarehouseBins"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseBins", new DmoField[10]
			{
				new DmoField("inbWarehouseID", "nvarchar", 5, 0, nullable: false),
				new DmoField("inbWarehouseBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("inbDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("inbLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("inbLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("inbInactive", "bit", 1, 0, nullable: false),
				new DmoField("inbInactiveDate", "date", 14, 0, nullable: true),
				new DmoField("inbCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("inbCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("inbUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[3]
			{
				new DmoIndex("inbWarehouseID,inbWarehouseBinID", unique: true),
				new DmoIndex("inbUniqueID", unique: true),
				new DmoIndex("inbWarehouseBinID", unique: false)
			});
		}
	}
}
