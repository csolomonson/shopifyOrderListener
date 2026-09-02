using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add Attachment Path fields to Org/Loc", "2008-05-13")]
public class v710000l
{
	public v710000l(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoHDAttachmentFilePath"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoHDAttachmentFilePath", "text", 50, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationLocations", "cmlHDAttachmentFilePath"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationLocations", "cmlHDAttachmentFilePath", "text", 50, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
