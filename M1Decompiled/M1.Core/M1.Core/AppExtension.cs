using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace M1.Core;

[DebuggerDisplay("{AppID} - {Caption}")]
public class AppExtension
{
	public string AppID;

	public string Caption;

	public string LastUpdatedDDVersion;

	public string CodeAssembly;

	public string CodeAssemblyVersion;

	public string FormsAssembly;

	public string FormsAssemblyVersion;

	public string DDAssembly;

	public string DDAssemblyVersion;

	public Assembly LoadedFormsAssembly;

	public Assembly LoadedCodeAssembly;

	public AppExtension(DataRow row, string asmFolder)
	{
		AppID = row.Field<string>("dpAppExtensionID");
		Caption = row.Field<string>("dpCaption");
		LastUpdatedDDVersion = row.Field<string>("dpLastUpdatedDDVersion");
		CodeAssembly = addFolder(row.Field<string>("dpCodeAssembly"), asmFolder);
		FormsAssembly = addFolder(row.Field<string>("dpFormsAssembly"), asmFolder);
		DDAssembly = addFolder(row.Field<string>("dpDDAssembly"), asmFolder);
		CodeAssemblyVersion = getVersion(CodeAssembly);
		FormsAssemblyVersion = getVersion(FormsAssembly);
		DDAssemblyVersion = getVersion(DDAssembly);
	}

	private string getVersion(string file)
	{
		if (string.IsNullOrEmpty(file))
		{
			return string.Empty;
		}
		FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(file);
		return versionInfo.ProductMajorPart + "." + versionInfo.ProductMinorPart.ToString().PadLeft(1, '0') + "." + versionInfo.ProductBuildPart.ToString().PadLeft(3, '0');
	}

	public Assembly GetFormsAssembly()
	{
		if (LoadedFormsAssembly == null && FormsAssembly.Length != 0)
		{
			LoadedFormsAssembly = Assembly.LoadFrom(FormsAssembly);
		}
		return LoadedFormsAssembly;
	}

	public Assembly GetCodeAssembly()
	{
		if (LoadedCodeAssembly == null && CodeAssembly.Length != 0)
		{
			LoadedCodeAssembly = Assembly.LoadFrom(CodeAssembly);
		}
		return LoadedCodeAssembly;
	}

	public Assembly GetDDAssembly()
	{
		if (DDAssembly.Length != 0)
		{
			return Assembly.LoadFrom(DDAssembly);
		}
		return null;
	}

	private string addFolder(string file, string folder)
	{
		if (!string.IsNullOrEmpty(file))
		{
			return folder + file;
		}
		return string.Empty;
	}
}
