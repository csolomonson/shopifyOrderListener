using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using M1.Extensions;

namespace M1.Core;

public class ServerFileSystem
{
	private ServerManager serverManager;

	private AppContext context;

	public ServerFileSystem(AppContext newContext, ServerManager newServerManager)
	{
		serverManager = newServerManager;
		context = newContext;
	}

	public ServerFileSystem(ServerManager newServerManager)
	{
		serverManager = newServerManager;
	}

	[MethodImpl(MethodImplOptions.NoOptimization)]
	public bool CreateFolder(string folder)
	{
		if ((object)serverManager.ExecuteCommand(null, null, "master", "EXEC xp_cmdshell 'mkdir \"" + folder.Replace("'", "''") + "\"'") == null)
		{
			return true;
		}
		return false;
	}

	public bool MoveFile(string sourceFilePath, string destFilePath)
	{
		if (serverManager.ExecuteScalar(null, null, "master", "EXEC xp_cmdshell 'move /Y \"" + sourceFilePath.Replace("'", "''") + "\" \"" + destFilePath.Replace("'", "''") + "\"'") == null)
		{
			return true;
		}
		return false;
	}

	public bool RenameFile(string sourceFilePath, string destFileName)
	{
		if (serverManager.ExecuteScalar(null, null, "master", "EXEC xp_cmdshell 'REN \"" + sourceFilePath.Replace("'", "''") + "\" \"" + destFileName.Replace("'", "''") + "\"'") == null)
		{
			return true;
		}
		return false;
	}

	public bool DeleteFile(string filePath)
	{
		if ((object)serverManager.ExecuteCommand(null, null, "master", "EXEC xp_cmdshell 'DEL \"" + filePath.Replace("'", "''") + "\"'") == null)
		{
			return true;
		}
		return false;
	}

	public bool CopyFile(string sourceFilePath, string destFilePath)
	{
		serverManager.ExecuteCommand(null, null, "master", "EXEC xp_cmdshell 'COPY \"" + sourceFilePath.Replace("'", "''") + "\" \"" + destFilePath.Replace("'", "''") + "\" /Y'");
		return true;
	}

	public bool FolderExists(string folderName)
	{
		using DataTable dataTable = serverManager.GetDataTable(null, null, "master", 0, "master.dbo.xp_fileexist " + folderName.ToSql());
		if (dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0].Field<byte>("File is a Directory") == 1;
		}
		return false;
	}

	public bool FileExists(string fileName)
	{
		using DataTable dataTable = serverManager.GetDataTable(null, null, "master", 0, "master.dbo.xp_fileexist " + fileName.ToSql());
		if (dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0].Field<byte>("File Exists") == 1;
		}
		return false;
	}

	public List<string> GetFolders(string folderPath)
	{
		DataTable serverDirListing = GetServerDirListing(folderPath);
		List<string> list = new List<string>();
		foreach (DataRow row in serverDirListing.Rows)
		{
			if (row.Field<string>("diroutput") != null && row.Field<short>("isFolder") == 1)
			{
				list.Add(row.Field<string>("diroutput").Trim());
			}
		}
		return list;
	}

	public List<string> GetFiles(string folderPath, string extension)
	{
		DataTable serverDirListing = GetServerDirListing(folderPath);
		List<string> list = new List<string>();
		string empty = string.Empty;
		if (extension.Length != 0)
		{
			extension = "." + extension;
		}
		foreach (DataRow row in serverDirListing.Rows)
		{
			if (row.Field<string>("diroutput") != null && row.Field<short>("isFolder") == 0)
			{
				empty = row.Field<string>("diroutput").Trim();
				if (extension.Length == 0 || extension == "*" || extension == "*.*" || Path.GetExtension(empty).Equals(extension, StringComparison.CurrentCultureIgnoreCase))
				{
					list.Add(empty);
				}
			}
		}
		return list;
	}

	public DataTable GetServerDirListing(string folderPath)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Set NoCount On");
		stringBuilder.AppendLine("Declare @BasePath varchar(1024)");
		stringBuilder.AppendLine("Select @BasePath = '" + folderPath.AddBackslash().Replace("'", "''''") + "'");
		stringBuilder.AppendLine("Declare @ShellCommand varchar(1024)");
		stringBuilder.AppendLine("Select @ShellCommand = 'xp_cmdshell ''dir \"' + @BasePath + '\" /B'' '");
		stringBuilder.AppendLine("Declare @FullPath varchar(1024)");
		stringBuilder.AppendLine("Create Table #FileListStart (diroutput varchar(1024))");
		stringBuilder.AppendLine("Create Table #FileListFinal (diroutput varchar(1024), isFile smallint, isFolder smallint)");
		stringBuilder.AppendLine("Create Table #FileExistTest ([File Exists] smallint, [File is a Directory] smallint, [Parent Directory Exists] smallint)");
		stringBuilder.AppendLine("Insert Into #FileListStart Exec (@ShellCommand)");
		stringBuilder.AppendLine("Declare @CurPath varchar(1024)");
		stringBuilder.AppendLine("Declare BrowseCursor Cursor Read_Only For Select diroutput From #FileListStart Where not diroutput is null Order By diroutput");
		stringBuilder.AppendLine("Open BrowseCursor");
		stringBuilder.AppendLine("Fetch Next From BrowseCursor INTO @CurPath");
		stringBuilder.AppendLine("While @@Fetch_Status = 0");
		stringBuilder.AppendLine("Begin");
		stringBuilder.AppendLine("    Select @FullPath = @BasePath + @CurPath");
		stringBuilder.AppendLine("    Insert Into #FileExistTest exec xp_fileexist @FullPath");
		stringBuilder.AppendLine("    Insert Into #FileListFinal (diroutput, isFile, isFolder) Select @CurPath, [File Exists], [File is a Directory] From #FileExistTest");
		stringBuilder.AppendLine("    Delete From #FileExistTest");
		stringBuilder.AppendLine("    Fetch Next From BrowseCursor Into @CurPath");
		stringBuilder.AppendLine("End");
		stringBuilder.AppendLine("Close BrowseCursor");
		stringBuilder.AppendLine("DeAllocate BrowseCursor");
		stringBuilder.AppendLine("Drop Table #FileExistTest");
		stringBuilder.AppendLine("Drop Table #FileListStart");
		stringBuilder.AppendLine("Set NoCount Off");
		stringBuilder.AppendLine("Select * From #FileListFinal");
		stringBuilder.AppendLine("Drop Table #FileListFinal");
		return serverManager.GetDataTable(null, null, "master", 0, stringBuilder.ToString());
	}
}
