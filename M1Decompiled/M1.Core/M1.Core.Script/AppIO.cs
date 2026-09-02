using System.IO;
using M1.Script.Interfaces;

namespace M1.Core.Script;

public class AppIO : IIO
{
	public void CopyFile(string sourceFile, string destinationFile, bool overwrite = false)
	{
		File.Copy(sourceFile, destinationFile, overwrite);
	}

	public void MoveFile(string sourceFile, string destinationFile)
	{
		File.Move(sourceFile, destinationFile);
	}

	public void DeleteFile(string path)
	{
		File.Delete(path);
	}

	public bool CreateFolder(string folder)
	{
		if (!folder.EndsWith("\\"))
		{
			folder += "\\";
		}
		if (!Directory.Exists(folder))
		{
			Directory.CreateDirectory(folder);
		}
		return true;
	}
}
