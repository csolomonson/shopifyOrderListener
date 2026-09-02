namespace M1.Core;

public class DDConversionParms
{
	public DmoDD DmoDD;

	public string DatabaseName;

	public bool ConvertCustomFormCode;

	public DDConversionParms(DmoDD dmoDD, string databaseName, bool convertCustomFormCode)
	{
		DmoDD = dmoDD;
		DatabaseName = databaseName;
		ConvertCustomFormCode = convertCustomFormCode;
	}
}
