using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.006", "Add fields to ToolMovements table", "2014-10-08")]
public class v900006a
{
	public v900006a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ToolMovements"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ToolMovements", new DmoField[18]
			{
				new DmoField("xtaToolMovementID", "int", 9, 0, nullable: false),
				new DmoField("xtaToolID", "nvarchar", 10, 0, nullable: false),
				new DmoField("xtaMovementType", "nvarchar", 10, 0, nullable: false),
				new DmoField("xtaMovementDate", "date", 14, 0, nullable: true),
				new DmoField("xtaCheckedOutToEmployeeID", "nvarchar", 10, 0, nullable: false),
				new DmoField("xtaPlannedReturnDate", "date", 14, 0, nullable: true),
				new DmoField("xtaCheckoutReasonID", "nvarchar", 10, 0, nullable: false),
				new DmoField("xtaNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("xtaNotesText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("xtaJobID", "nvarchar", 20, 0, nullable: false),
				new DmoField("xtaWorkCenterID", "nvarchar", 5, 0, nullable: false),
				new DmoField("xtaProductionDepartmentID", "nvarchar", 5, 0, nullable: false),
				new DmoField("xtaPlantID", "nvarchar", 5, 0, nullable: false),
				new DmoField("xtaPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
				new DmoField("xtaLocation", "nvarchar", 30, 0, nullable: false),
				new DmoField("xtaCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("xtaCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("xtaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[6]
			{
				new DmoIndex("xtaToolMovementID", unique: true),
				new DmoIndex("xtaUniqueID", unique: true),
				new DmoIndex("xtaCheckedOutToEmployeeID", unique: false),
				new DmoIndex("xtaJobID", unique: false),
				new DmoIndex("xtaWorkCenterID", unique: false),
				new DmoIndex("xtaProductionDepartmentID", unique: false)
			});
		}
	}
}
