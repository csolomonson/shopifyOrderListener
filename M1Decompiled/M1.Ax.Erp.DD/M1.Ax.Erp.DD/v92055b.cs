using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.055", "Update field bindings", "2016-12-19")]
public class v92055b
{
	public v92055b(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update RMAReceiptLines Set rrlTotalComponentCosts = DetailAmount From RMAReceiptLines Inner Join (Select rroRMAReceiptID,rroRMAReceiptLineID,Sum(rroExtendedCost) As DetailAmount From RMAReceiptComponents Group By rroRMAReceiptID,rroRMAReceiptLineID) As DetailTable On RRLRMARECEIPTID = rroRMAReceiptID And RRLRMARECEIPTLINEID = rroRMAReceiptLineID");
	}
}
