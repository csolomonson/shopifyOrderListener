using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.691", "", "")]
public class v91691
{
	public v91691(DDConversionParms parms)
	{
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[1]
		{
			new TranslateInfo("App.Ax(\"OrderFunctions\").RefreshOrderTotal", "App.Ax(\"SalesOrder\").RefreshOrderTotal", ignoreCase: true)
		});
	}
}
