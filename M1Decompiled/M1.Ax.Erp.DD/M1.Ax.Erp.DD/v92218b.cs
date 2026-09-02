using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.218", "Add fields to ProductionCalendars table", "2017-04-10")]
public class v92218b
{
	public v92218b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionCalendars", "jmlWorkCenterID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionCalendars", "jmlWorkCenterID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.Dmo.DropIndexes(null, parms.User, parms.DatabaseName, "ProductionCalendars", new DmoIndex[1]
		{
			new DmoIndex("JMLPRODUCTIONCALENDARYEARID,jmlPlantID", unique: true)
		}, parms.Messages);
		parms.Dmo.DropIndexes(null, parms.User, parms.DatabaseName, "ProductionCalendars", new DmoIndex[1]
		{
			new DmoIndex("JMLPRODUCTIONCALENDARYEARID_jmlPlantID_jmlWorkCenterID", unique: true)
		}, parms.Messages);
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionCalendars", "jmlWorkCenterID"))
		{
			parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionCalendars", new DmoIndex[1]
			{
				new DmoIndex("JMLPRODUCTIONCALENDARYEARID,jmlPlantID,jmlWorkCenterID", unique: true)
			}, parms.Messages);
		}
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ProductionCalendarDaysWC"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Insert Into ProductionCalendars (jmlProductionCalendarYearID,jmlPlantID,jmlWorkCenterID) Select Distinct jmyProductionCalendarYearID,xawPlantID,jmyWorkCenterID From ProductionCalendarDaysWC Inner Join WorkCenters On jmyWorkCenterID = xawWorkCenterID COLLATE SQL_Latin1_General_CP1_CS_AS Where jmyWorkCenterID <> '' And Not Exists (Select A.jmlProductionCalendarYearID, A.jmlPlantID, A.jmlWorkCenterID From ProductionCalendars A Where ProductionCalendarDaysWC.jmyProductionCalendarYearID = A.jmlProductionCalendarYearID and WorkCenters.xawPlantID = A.jmlPlantID and ProductionCalendarDaysWC.jmyWorkCenterID = A.jmlWorkCenterID)");
		}
	}
}
