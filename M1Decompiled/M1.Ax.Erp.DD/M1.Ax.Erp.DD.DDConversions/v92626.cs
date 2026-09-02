using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.626", "", "")]
public class v92626
{
	public v92626(DDConversionParms parms)
	{
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[1]
		{
			new TranslateInfo("App.CreateFolder", "App.IO.CreateFolder", ignoreCase: true)
		});
	}
}
