using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.6.100", "Add Inactive and InactiveDate to PartBins", "2022-10-27")]
public class v96100a
{
	public v96100a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartBins", "imbInactiveBinDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartBins", "imbInactiveBinDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartBins", "imbInactiveBin"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartBins", "imbInactiveBin", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PartBins Set imbInactiveBin = 0");
		}
	}
}
