namespace M1.Core.Database;

public static class SystemGeneratedFields
{
	public static bool IsGenerated(string fieldName)
	{
		if (fieldName == null || fieldName.Length <= 3)
		{
			return false;
		}
		string text = fieldName.Substring(3).ToUpper();
		if (text == "UNIQUEID" || text == "ROWVERSION")
		{
			return true;
		}
		return false;
	}
}
