using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.077", "Add fields to CustomerPackages table", "2015-08-21")]
public class v900077a
{
	public v900077a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "CustomerPackages"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CustomerPackages", new DmoField[11]
			{
				new DmoField("cpaCustomerPackageID", "char", 10, 0, nullable: false),
				new DmoField("cpaPackageDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("cpaPackageDimensionsUOM", "nvarchar", 2, 0, nullable: false),
				new DmoField("cpaPackageHeight", "int", 4, 0, nullable: false),
				new DmoField("cpaPackageWidth", "int", 4, 0, nullable: false),
				new DmoField("cpaPackageLength", "int", 4, 0, nullable: false),
				new DmoField("cpaInactive", "bit", 1, 0, nullable: false),
				new DmoField("cpaInactiveDate", "date", 14, 0, nullable: true),
				new DmoField("cpaCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("cpaCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("cpaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("cpaCustomerPackageID", unique: true),
				new DmoIndex("cpaUniqueID", unique: true)
			});
		}
	}
}
