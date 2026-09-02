using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.036", "Re-create EmployeePayRateProcesses table", "2015-05-19")]
public class v900036f
{
	public v900036f(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "EmployeePayRateProcesses"))
		{
			if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "EmployeePayRateProcessesTemp"))
			{
				parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "EmployeePayRateProcessesTemp");
			}
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "SELECT * Into EmployeePayRateProcessesTemp FROM EmployeePayRateProcesses");
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "EmployeePayRateProcesses");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "EmployeePayRateProcesses"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeePayRateProcesses", new DmoField[8]
			{
				new DmoField("lnqEmployeeID", "nvarchar", 10, 0, nullable: false),
				new DmoField("lnqEmployeePayRateID", "smallint", 4, 0, nullable: false),
				new DmoField("lnqEmployeePayRateProcessID", "smallint", 4, 0, nullable: false),
				new DmoField("lnqProcessID", "nvarchar", 5, 0, nullable: false),
				new DmoField("lnqProcessPayRate", "numeric", 8, 4, nullable: false),
				new DmoField("lnqCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("lnqCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("lnqUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[6]
			{
				new DmoIndex("lnqEmployeeID,lnqEmployeePayRateID,lnqEmployeePayRateProcessID", unique: true),
				new DmoIndex("lnqUniqueID", unique: true),
				new DmoIndex("lnqEmployeeID", unique: false),
				new DmoIndex("lnqEmployeePayRateID", unique: false),
				new DmoIndex("lnqEmployeePayRateProcessID", unique: false),
				new DmoIndex("lnqProcessID", unique: false)
			});
		}
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "EmployeePayRateProcessesTemp"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "INSERT Into EmployeePayRateProcesses (lnqEmployeeID,lnqEmployeePayRateID,lnqEmployeePayRateProcessID,lnqProcessID,lnqProcessPayRate,lnqCreatedBy,lnqCreatedDate,lnqUniqueID) SELECT lnpEmployeeID,lnpEmployeePayRateID,lnpEmployeePayRateProcessID,lnpProcessID,lnpProcessPayRate,lnpCreatedBy,lnpCreatedDate,lnpUniqueID FROM EmployeePayRateProcessesTemp");
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "EmployeePayRateProcessesTemp");
		}
	}
}
