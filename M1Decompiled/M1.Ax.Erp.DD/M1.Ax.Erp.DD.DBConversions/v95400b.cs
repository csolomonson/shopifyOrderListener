using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.400", "Add cloud document fields to DocumentLinks table", "2022-08-29")]
public class v95400b
{
	public v95400b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DocumentLinks", "xalFileNameWhenUploaded"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DocumentLinks", "xalFileNameWhenUploaded", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DocumentLinks", "xalCloudFileId"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DocumentLinks", "xalCloudFileId", "nvarchar", 255, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DocumentLinks", "xalFileLastModifiedDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DocumentLinks", "xalFileLastModifiedDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
