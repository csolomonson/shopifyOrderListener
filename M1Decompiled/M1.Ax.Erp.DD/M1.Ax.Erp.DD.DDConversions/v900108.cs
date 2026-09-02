using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.108", "", "")]
public class v900108
{
	public v900108(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1SERIALNUMBERTRANSACTIONSALL' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1LOTNUMBERTRANSACTIONSALL' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1LOTNUMBERTRANSACTIONSENTRY' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1SERIALNUMBERTRANSACTIONSENTRY' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1PENDINGINSPECTIONS' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1OPENINSPECTIONS' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1INSPECTIONSCLOSEDTODAY' and dgUserID <> ''");
		string queryString = "Update ddfields set dffield = REPLACE(dfField, 'UQAN', 'UQAL'), dfTable = 'INSPECTIONLINES' where dftable = 'QUALITYREGISTERS' and abs(dfCustom) = 1 and REPLACE(dfField, 'UQAN', 'UQAL') not in (select dfField from ddfields where dfTable = 'INSPECTIONLINES');Update ddfields set dffield = REPLACE(dfField, 'URAR', 'UQAR'), dfTable = 'NONCONFORMANCES' where dftable = 'RMACLAIMPROBLEMS' and abs(dfCustom) = 1 and REPLACE(dfField, 'URAR', 'UQAR') not in (select dffield from ddfields where dfTable = 'NONCONFORMANCES');";
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, queryString);
	}
}
