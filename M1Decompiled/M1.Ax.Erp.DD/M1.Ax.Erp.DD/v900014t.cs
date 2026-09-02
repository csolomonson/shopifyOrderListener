using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.014", "Add fields to PartMemos table", "2014-12-15")]
public class v900014t
{
	public v900014t(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartMemos", "imkShowInQualityRegisters"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartMemos", "imkShowInQualityRegisters", dropTriggers: true);
		}
	}
}
