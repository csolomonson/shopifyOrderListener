using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.824", "Update Manual Part flag in PartMaterials", "2019-10-16")]
public class v92824b
{
	public v92824b(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PartMaterials Set immManualPart = 0 Where immManualPart <> 0");
	}
}
