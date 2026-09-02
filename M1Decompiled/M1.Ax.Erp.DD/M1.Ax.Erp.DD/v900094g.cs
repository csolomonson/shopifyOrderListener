using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.094", "Update field bindings", "2015-10-16")]
public class v900094g
{
	public v900094g(DBConversionParms parms)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("begin SET NOCOUNT ON DECLARE @SQL3 varchar(8000) set @sql3 = ' Update SalesOrders Set ompSplitPercentTotal = DetailAmount From SalesOrders Inner Join (Select OMISALESORDERID,Sum(omiPercent) As DetailAmount From SalesOrderSalesPeople Group By OMISALESORDERID) As DetailTable On OMPSALESORDERID = OMISALESORDERID; Update Quotes Set qmpSplitPercentTotal = DetailAmount From Quotes Inner Join (Select QMJQUOTEID,Sum(qmjPercent) As DetailAmount From QuoteSalesPeople Group By QMJQUOTEID) As DetailTable On QMPQUOTEID = QMJQUOTEID; Update Leads Set lopSplitPercentTotal = DetailAmount From Leads Inner Join (Select LOJLEADID,Sum(lojPercent) As DetailAmount From LeadSalesPeople Group By LOJLEADID) As DetailTable On LOPLEADID = LOJLEADID; Update OrganizationLocations Set cmlSplitPercentTotal = DetailAmount From OrganizationLocations Inner Join (Select cmkOrganizationID, cmkLocationID, Sum(cmkPercent)As DetailAmount From OrganizationLocSalesPeople Group By cmkOrganizationID, cmkLocationID) As DetailTable On cmlOrganizationID = cmkOrganizationID And cmlLocationID = cmkLocationID; Update Organizations Set cmoSplitPercentTotal = DetailAmount From Organizations Inner Join (Select cmkOrganizationID, cmkLocationID, Sum(cmkPercent)As DetailAmount From OrganizationLocSalesPeople Where cmkLocationID = '''' Group By cmkOrganizationID, cmkLocationID) As DetailTable On cmoOrganizationID = cmkOrganizationID; ' exec(@sql3) SET NOCOUNT OFF end;");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, stringBuilder.ToString());
	}
}
