using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.4.100", "Add default values for Email Client for Single Email at a user level", "2021-06-28")]
public class v94100e
{
	public v94100e(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "UPDATE DDUsers SET duProperties = CONCAT(duProperties, 'EmailClientSingleEmail = ''M1EMAIL''' + CHAR(13)) WHERE duType = 0");
	}
}
