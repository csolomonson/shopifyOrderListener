using System.Collections.Generic;
using System.IO;
using M1.Extensions;

namespace M1.Core;

public class Templates
{
	private AppContext currentContext;

	public string Location
	{
		get
		{
			return (currentContext.IsHosted ? currentContext.Metadata.FileShareLocation : currentContext.Server.Location) + "Templates\\";
		}
		private set
		{
		}
	}

	public Templates(AppContext context)
	{
		currentContext = context;
	}

	public string[] GetTemplateFolders(string cBase)
	{
		return GetTemplateFolders(cBase, string.Empty);
	}

	public string[] GetTemplateFolders(string cBase, string topFolder)
	{
		string empty = string.Empty;
		string[] array = null;
		cBase = cBase.Trim();
		if (cBase.Length != 0)
		{
			cBase.AddBackslash();
		}
		empty = Location + topFolder + cBase.Trim();
		if (Directory.Exists(empty))
		{
			array = Directory.GetDirectories(empty);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = array[i].Replace(empty, "");
			}
			return array;
		}
		return new string[0];
	}

	public string[] GetTemplatesInFolder(string folder)
	{
		List<string> list = new List<string>();
		folder = folder.Trim();
		if (folder.Length != 0)
		{
			folder.AddBackslash();
		}
		folder = Location + folder;
		if (Directory.Exists(folder))
		{
			string[] files = Directory.GetFiles(folder);
			foreach (string text in files)
			{
				if ((File.GetAttributes(text) & FileAttributes.Hidden) != FileAttributes.Hidden)
				{
					string text2 = text.Replace(folder, "");
					string text3 = text2.Trim().Substring(text2.Length - 4).ToUpper();
					if (currentContext.IsInstalled.Word && (text3 == ".DOC" || text3 == ".DOT"))
					{
						list.Add(text2.Trim());
					}
				}
			}
		}
		return list.ToArray();
	}

	private void createTemplateFolder()
	{
		string path = Location.Trim();
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
	}
}
