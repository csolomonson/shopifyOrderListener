using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.4.200", "Rename apaBox14 field to apaBox10 field of Form1099Types table", "2021-08-23")]
public class v94200i
{
	public v94200i(DBConversionParms parms)
	{
		string initialVersion = parms.InitialVersion;
		if (("8.10.050".CompareTo(initialVersion) == -1 || "8.10.050".CompareTo(initialVersion) == 0) && parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1099Types", "apaBox14"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099Types", "apaBox14", "apaBox10", dropTriggers: true);
		}
	}
}
