using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.043", "Rename field in Followups table", "2015-06-10")]
public class v900043a
{
	public v900043a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Followups", "cmfCreateFromMobile"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Followups", "cmfCreateFromMobile", "cmfCreatedFromMobile", dropTriggers: true);
		}
		else if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Followups", "cmfCreatedFromMobile"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Followups", "cmfCreatedFromMobile", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
