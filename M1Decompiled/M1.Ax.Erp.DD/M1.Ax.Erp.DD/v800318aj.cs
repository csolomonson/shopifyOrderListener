using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.318", "Add fields to FINANCIALPROPERTIES table", "2015-05-19")]
public class v800318aj
{
	public v800318aj(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FINANCIALPROPERTIES", "xafSuperExportFilePath"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FINANCIALPROPERTIES", "xafSuperExportFilePath", "nvarchar", 200, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FINANCIALPROPERTIES", "xafMiscReceiptVarianceAccount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FINANCIALPROPERTIES", "xafMiscReceiptVarianceAccount", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FinancialProperties SET xafMiscReceiptVarianceAccount = 2 WHERE xafGLCreateStockJournals <> 0");
		}
	}
}
