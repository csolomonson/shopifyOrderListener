using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.003", "Add fields to QualityRegisters table", "2014-09-25")]
public class v900003c
{
	public v900003c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "QualityRegisters"))
		{
			return;
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QualityRegisters", "qanSourceTableName"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QualityRegisters", "qanSourceTableName", "nvarchar", 30, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QualityRegisters", "qanSourceTableUniqueID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QualityRegisters", "qanSourceTableUniqueID", "uniqueidentifier", 16, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QualityRegisters", "qanSourceTableName") && parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QualityRegisters", "qanSourceTableUniqueID"))
		{
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QualityRegisters", "qanReceiptID"))
			{
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update QualityRegisters Set qanSourceTableName = 'ReceiptLines', qanSourceTableUniqueID = rmlUniqueID From QualityRegisters inner join ReceiptLines on qanReceiptID = rmlReceiptID and qanReceiptLineID = rmlReceiptLineID Where qanReceiptID <> ''");
			}
			else
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QualityRegisters", "qanReceiptID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QualityRegisters", "qanReceiptLineID"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QualityRegisters", "qanReceiptLineID", "smallint", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
		}
	}
}
