using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.316", "Add fields to PRODUCTIONPROPERTIES table", "2015-05-19")]
public class v800316w
{
	public v800316w(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapEasyOrderReceiveLibraryID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapEasyOrderReceiveLibraryID", "nvarchar", 33, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapEasyOrderSmallImageURL"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapEasyOrderSmallImageURL", "nvarchar", 200, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapEasyOrderThumbnailImageURL"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapEasyOrderThumbnailImageURL", "nvarchar", 200, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapEasyOrderReceiveMachine"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapEasyOrderReceiveMachine", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapEasyOrderURL"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapEasyOrderURL", "nvarchar", 200, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapEasyOrderLargeImageURL"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapEasyOrderLargeImageURL", "nvarchar", 200, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapEasyOrderExportFilePath"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapEasyOrderExportFilePath", "nvarchar", 250, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapIMIgnoreLCInStdCostRollup"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PRODUCTIONPROPERTIES", "xapIMIgnoreLCInStdCostRollup", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
