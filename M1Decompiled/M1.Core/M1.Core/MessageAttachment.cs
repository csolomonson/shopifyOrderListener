using System;
using System.IO;
using M1.Extensions;

namespace M1.Core;

public class MessageAttachment : IDisposable
{
	public string FileName;

	public string Description;

	protected byte[] Data;

	protected string TempFileName;

	public MessageAttachment(string fileName, string description)
	{
		FileName = fileName;
		Description = description;
		TempFileName = Path.Combine(Path.GetTempPath(), M1Util.GenerateTempFileName(Path.GetExtension(Description)));
		File.Copy(fileName, TempFileName);
	}

	public MessageAttachment(string fileName, string description, Stream stream)
	{
		FileName = fileName;
		Description = description;
		using MemoryStream memoryStream = new MemoryStream();
		stream.CopyTo(memoryStream);
		Data = memoryStream.ToArray();
	}

	public byte[] GetData()
	{
		if (string.IsNullOrWhiteSpace(TempFileName))
		{
			return Data;
		}
		return File.ReadAllBytes(TempFileName);
	}

	public bool IsDataNull()
	{
		return Data == null;
	}

	public string CopyToFile()
	{
		return CopyToFile(Path.Combine(Path.GetTempPath(), M1Util.GenerateTempFileName(Path.GetExtension(Description))));
	}

	public string CopyToFile(string file, bool overwriteFile = false)
	{
		if (string.IsNullOrWhiteSpace(TempFileName))
		{
			File.WriteAllBytes(file, Data);
		}
		else
		{
			File.Copy(TempFileName, file, overwriteFile);
		}
		return file;
	}

	public void Dispose()
	{
		if (string.IsNullOrWhiteSpace(TempFileName))
		{
			Data = null;
			return;
		}
		if (File.Exists(TempFileName))
		{
			FileAttributes attributes = File.GetAttributes(TempFileName);
			if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
			{
				attributes = RemoveAttribute(attributes, FileAttributes.ReadOnly);
				File.SetAttributes(TempFileName, attributes);
			}
			File.Delete(TempFileName);
		}
		TempFileName = null;
	}

	private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
	{
		return attributes & ~attributesToRemove;
	}

	~MessageAttachment()
	{
		Dispose();
	}
}
