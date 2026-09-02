using System;
using M1.Core.Script;

namespace M1.Ax.Erp;

public class SerialNumberScripting : ScriptingBase
{
	public SerialNumberScripting(IServiceProvider provider)
		: base(provider)
	{
	}

	public string TestSerialNumberFormula(string code)
	{
		try
		{
			LoadEnvironment();
			AddCode("Dim formula\rDim NextNumber\rNextNumber = 0\rDim CurrentYear\rCurrentYear = " + DateTime.Now.Year + "\rDim CurrentMonth\rCurrentMonth = " + DateTime.Now.Month + "\rDim CurrentDay\rCurrentDay = " + DateTime.Now.Day + "\rDim PartID\rPartID = \"\"\rDim PartRevisionID\rPartRevisionID = \"\"\rDim PartGroupID\rPartGroupID = \"\"\r");
			base.ExecuteStatement(code);
			return "Formula Verified.";
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}
}
