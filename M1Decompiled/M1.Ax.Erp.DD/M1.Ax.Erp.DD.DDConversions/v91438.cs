using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.438", "", "")]
public class v91438
{
	public v91438(DDConversionParms parms)
	{
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[1]
		{
			new TranslateInfo("Call App.PayrollFunctions.ShowLoadTaxTablesForm(\"Import\")", "Forms.OpenProcessForm \"M1.Ax.Erp.ImportTaxTableProcess\"", ignoreCase: true)
		});
	}
}
