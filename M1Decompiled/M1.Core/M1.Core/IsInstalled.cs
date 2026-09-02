using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace M1.Core;

public class IsInstalled
{
	public bool Word { get; private set; }

	public bool OpenOffice { get; private set; }

	public bool MapPoint { get; private set; }

	public bool Outlook { get; private set; }

	public bool Outlook64Bit { get; private set; }

	public bool Thunderbird { get; private set; }

	public IsInstalled()
	{
		Word = getValue("HKEY_CLASSES_ROOT\\Word.Application", string.Empty).Length != 0;
		OpenOffice = getValue("HKEY_CLASSES_ROOT\\com.sun.star.ServiceManager", string.Empty).Length != 0;
		MapPoint = getValue("HKEY_CLASSES_ROOT\\MapPoint.Map", string.Empty).Length != 0;
		string value = getValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\MAILTO\\UserChoice", "ProgID");
		if (value.Length == 0)
		{
			value = getValue("HKEY_CLASSES_ROOT\\mailto\\shell\\open\\command", string.Empty);
		}
		Outlook = value.ToLower().Contains("outlook.");
		if (getValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Office\\14.0\\Outlook", "Bitness") == "x64")
		{
			Outlook64Bit = true;
		}
		else if (getValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Office\\15.0\\Outlook", "Bitness") == "x64")
		{
			Outlook64Bit = true;
		}
		else if (getValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Office\\16.0\\Outlook", "Bitness") == "x64")
		{
			Outlook64Bit = true;
		}
		else
		{
			Outlook64Bit = false;
		}
		Thunderbird = value.ToLower().Contains("thunderbird.");
		checkWebBrowserCompatibility();
	}

	private string getValue(string keyName, string valueName)
	{
		object value = Registry.GetValue(keyName, valueName, string.Empty);
		if (value == null)
		{
			return string.Empty;
		}
		return value.ToString();
	}

	private void checkWebBrowserCompatibility()
	{
		string valueName = Process.GetCurrentProcess().ProcessName + ".exe";
		string empty = string.Empty;
		empty = ((!Environment.Is64BitProcess) ? "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION" : "HKEY_CURRENT_USER\\SOFTWARE\\Wow6432Node\\Microsoft\\Internet Explorer\\MAIN\\FeatureControl\\FEATURE_BROWSER_EMULATION");
		if (getValue(empty, valueName).Length == 0)
		{
			Registry.SetValue(empty, valueName, 11001, RegistryValueKind.DWord);
		}
	}
}
