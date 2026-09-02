using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.665", "", "")]
public class v92665
{
	public v92665(DDConversionParms parms)
	{
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[1]
		{
			new TranslateInfo("Forms.OpenForm \"frmProcessFindAndReplace\"", "Call Forms.Ax(\"Parts\").OpenProcessFindAndReplaceForm(\"\",\"\")", ignoreCase: true)
		});
	}
}
