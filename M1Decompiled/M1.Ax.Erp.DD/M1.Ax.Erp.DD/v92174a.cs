using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.174", "Add fields to PartClassPlants table", "2017-02-24")]
public class v92174a
{
	public v92174a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartClassPlants", "imfInvToReturnGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartClassPlants", "imfInvToReturnGLAccountID", "nvarchar", 11, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartClassPlants", "imfInvInInspectionGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartClassPlants", "imfInvInInspectionGLAccountID", "nvarchar", 11, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartClassPlants", "imfInvInTransferGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartClassPlants", "imfInvInTransferGLAccountID", "nvarchar", 11, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
