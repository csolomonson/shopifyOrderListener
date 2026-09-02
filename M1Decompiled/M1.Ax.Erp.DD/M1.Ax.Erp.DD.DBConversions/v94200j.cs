using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.4.200", "Rename apaBox7 field to apaBox1Nec field of Form1099Types table", "2021-08-23")]
public class v94200j
{
	public v94200j(DBConversionParms parms)
	{
		string initialVersion = parms.InitialVersion;
		if (("8.10.050".CompareTo(initialVersion) == -1 || "8.10.050".CompareTo(initialVersion) == 0) && parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1099Types", "apaBox7"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099Types", "apaBox7", "apaBox1Nec", dropTriggers: true);
		}
	}
}
