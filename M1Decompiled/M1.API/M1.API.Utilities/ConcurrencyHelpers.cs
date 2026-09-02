using System.Linq;

namespace M1.API.Utilities;

public class ConcurrencyHelpers
{
	public static bool AreRowVersionsEqual(byte[] rowVersion1, byte[] rowVersion2)
	{
		if (rowVersion1 == null || rowVersion2 == null)
		{
			return false;
		}
		if (rowVersion1.Length != rowVersion2.Length)
		{
			return false;
		}
		return rowVersion1.SequenceEqual(rowVersion2);
	}
}
