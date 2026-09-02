using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("WorkCenters")]
public class WorkCentersChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
		if (parm.ChangeIDType != 1)
		{
			parm.DeleteStatements.AppendLine("DELETE FROM WorkCenterMachines WHERE xaqWorkCenterID = " + parm.OldKeyValues[0].ToSql());
			parm.DeleteStatements.AppendLine("DELETE FROM ProductionCalendarWorkCenters WHERE jmrWorkCenterID = " + parm.OldKeyValues[0].ToSql() + " AND jmrProductionCalendarYearID IN (SELECT jmrProductionCalendarYearID FROM ProductionCalendarWorkCenters WHERE jmrWorkCenterID = " + parm.NewKeyValues[0].ToSql() + ")");
			parm.DeleteStatements.AppendLine("DELETE FROM ProductionCalendarDays WHERE jmyWorkCenterID = " + parm.OldKeyValues[0].ToSql() + " AND jmyProductionCalendarYearID IN (SELECT jmyProductionCalendarYearID FROM ProductionCalendarDays WHERE jmyWorkCenterID = " + parm.NewKeyValues[0].ToSql() + ")");
		}
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
	}
}
