using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.081", "Add fields to Tools table", "2014-09-22")]
public class v810081b
{
	public v810081b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Tools"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Tools", new DmoField[17]
			{
				new DmoField("xttToolID", "nvarchar", 10, 0, nullable: false),
				new DmoField("xttToolCategoryID", "nvarchar", 10, 0, nullable: false),
				new DmoField("xttDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("xttLocation", "nvarchar", 30, 0, nullable: false),
				new DmoField("xttLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("xttLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("xttMovementType", "tinyint", 2, 0, nullable: false),
				new DmoField("xttMovementDate", "date", 14, 0, nullable: true),
				new DmoField("xttCheckoutReasonID", "nvarchar", 5, 0, nullable: false),
				new DmoField("xttPlannedReturnDate", "date", 14, 0, nullable: true),
				new DmoField("xttCheckedOutToEmployeeID", "nvarchar", 10, 0, nullable: false),
				new DmoField("xttToolImage", "image", 4, 0, nullable: true),
				new DmoField("xttInactive", "bit", 1, 0, nullable: false),
				new DmoField("xttInactiveDate", "date", 14, 0, nullable: true),
				new DmoField("xttCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("xttCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("xttUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("xttToolID", unique: true),
				new DmoIndex("xttUniqueID", unique: true)
			});
		}
	}
}
