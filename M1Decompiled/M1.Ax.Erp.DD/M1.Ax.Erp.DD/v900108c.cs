using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.108", "Update quality register and inspection links", "2015-11-25")]
public class v900108c
{
	public v900108c(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "QualityRegisters"))
		{
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Attachments", "cmaQualityRegisterID"))
			{
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "update Attachments set cmaInspectionID = qalInspectionID, cmaInspectionLineID = qalInspectionLineID from Attachments inner join QualityRegisters on cmaQualityRegisterID = qanQualityRegisterID inner join InspectionLines on qalQualityRegisterID = qanQualityRegisterID");
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAReceiptLines", "rrlQualityRegisterID"))
			{
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "update InspectionLines set qalSourceTableName = 'RMAReceiptLines', qalSourceTableUniqueID = rrlUniqueID from InspectionLines inner join QualityRegisters on qalQualityRegisterID=qanQualityRegisterID inner join RMAReceiptLines on qanReceiptID=rrlRMAReceiptID and qanReceiptLineID=rrlRMAReceiptLineID");
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalQualityRegisterID"))
			{
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "update InspectionLines set qalSourceTableName = 'ReceiptLines', qalSourceTableUniqueID = rmlUniqueID from InspectionLines inner join QualityRegisters on qalQualityRegisterID=qanQualityRegisterID inner join ReceiptLines on qanReceiptID=rmlReceiptID and qanReceiptLineID=rmlReceiptLineID");
			}
		}
	}
}
