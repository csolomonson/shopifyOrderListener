using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.663", "", "")]
public class v92663
{
	public v92663(DDConversionParms parms)
	{
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[1]
		{
			new TranslateInfo("App.Ax(\"OrderFunctions\").RefreshOrderTotal", "App.Ax(\"SalesOrder\").RefreshOrderTotal", ignoreCase: true)
		});
	}
}
