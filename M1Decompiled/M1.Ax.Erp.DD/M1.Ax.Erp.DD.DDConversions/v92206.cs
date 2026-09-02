using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.206", "", "")]
public class v92206
{
	public v92206(DDConversionParms parms)
	{
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[1]
		{
			new TranslateInfo("Call App.PayrollFunctions.ShowLoadTaxTablesForm(\"Import\")", "Forms.OpenProcessForm \"M1.Ax.Erp.ImportTaxTableProcess\"", ignoreCase: true)
		});
	}
}
