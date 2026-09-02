using System.Collections.Generic;

namespace M1.Core;

public class ExchangeFolder
{
	public string Name;

	public string ID;

	public List<ExchangeFolder> Folders = new List<ExchangeFolder>();

	public ExchangeFolder(string name, string id)
	{
		Name = name;
		ID = id;
	}
}
