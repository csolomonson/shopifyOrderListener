using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.683", "", "")]
public class v92683
{
	public v92683(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Delete from DDFormDetails where deControlName = 'grpEasyOrderInfo' and deFormID = 'M1.Ax.Erp.Forms.Sales.SalesOrder.SalesOrderView' and deCustom = 1");
	}
}
