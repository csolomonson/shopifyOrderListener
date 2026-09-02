using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.059", "Update field bindings", "2016-05-20")]
public class v91059c
{
	public v91059c(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PurchasePlannerLines Set pplDataMissing = DetailAmount From PurchasePlannerLines Inner Join (Select ppoSessionID,ppoLineID,Sum(ppoDataMissing) As DetailAmount From PurchasePlannerOrderDetails Group By ppoSessionID,ppoLineID) As DetailTable On pplSessionID = ppoSessionID And pplLineID = ppoLineID");
	}
}
