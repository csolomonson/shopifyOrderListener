using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Rename apaBox1B to apaBox1Nec Form1099Types", "2013-10-17")]
public class v810RenameForm1099TypesField
{
	public v810RenameForm1099TypesField(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1099Types", "apaBox1B"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099Types", "apaBox1B", "apaBox1Nec", dropTriggers: true);
		}
	}
}
