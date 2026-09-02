using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.092", "", "")]
public class v91092
{
	public v91092(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "UPDATE DDSearches SET dsGridID ='M1LOOKUPARPERIODOPEN' WHERE dsSearchID = 'DFARRENDGLFISCALYEARPERIODID' AND dsGridID = 'M1LOOKUPGLYEAR'");
	}
}
