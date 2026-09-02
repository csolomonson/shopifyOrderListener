using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.130", "Add Attachment Memos table", "2008-08-21")]
public class v710130
{
	public v710130(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "AttachmentMemos"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AttachmentMemos");
		}
	}
}
