using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.5.300", "Update Integration Service Config Polling Frequency to Seconds From Minutes", "2022-04-07")]
public class v95300b
{
	public v95300b(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "UPDATE IntegrationServiceInfo SET diPollingFrequency = CASE WHEN diPollingFrequency * 60 > 32767 THEN 32767 ELSE diPollingFrequency * 60 END");
	}
}
