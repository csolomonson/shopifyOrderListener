using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.4.100", "Add default values for Email Client for Multiple Email at a user level", "2021-07-27")]
public class v94100d
{
	public v94100d(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "UPDATE DDUsers SET duProperties = CONCAT(duProperties, 'EmailClientMultipleEmail = ''M1EMAIL''' + CHAR(13)) WHERE duType = 0");
	}
}
