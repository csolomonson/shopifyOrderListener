using System;
using System.Collections;
using System.IO;
using System.Text;

namespace M1.Core;

public class IniSettings
{
	private ArrayList settingsList = new ArrayList();

	private string currentPath = string.Empty;

	public string Get(string settingName, string defaultValue)
	{
		settingName = settingName.Trim().ToUpper();
		foreach (string settings in settingsList)
		{
			if (!settings.StartsWith("'"))
			{
				int num = settings.IndexOf('=');
				if (num > 0 && settings.Substring(0, num).Trim().ToUpper() == settingName)
				{
					return settings.Substring(num + 1).Trim();
				}
			}
		}
		return defaultValue;
	}

	public bool GetAsBool(string settingName, bool defaultValue)
	{
		string text = Get(settingName, string.Empty).Trim().ToUpper();
		if (text.Length == 0)
		{
			return defaultValue;
		}
		if (!(text == "TRUE"))
		{
			return text == "YES";
		}
		return true;
	}

	public bool SetAsBool(string settingName, bool settingValue)
	{
		if (settingValue)
		{
			return Set(settingName, "True");
		}
		return Set(settingName, "False");
	}

	public int GetAsInt(string settingName, int defaultValue)
	{
		int result = defaultValue;
		string text = Get(settingName, string.Empty).Trim().ToUpper();
		if (text.Length != 0)
		{
			if (!int.TryParse(text, out result))
			{
				result = defaultValue;
			}
		}
		else
		{
			result = defaultValue;
		}
		return result;
	}

	public bool SetAsInt(string settingName, int settingValue)
	{
		return Set(settingName, settingValue.ToString());
	}

	public bool Set(string settingName, string settingValue, string defaultValue)
	{
		if (settingValue.Equals(defaultValue, StringComparison.CurrentCultureIgnoreCase))
		{
			return Remove(settingName);
		}
		return Set(settingName, settingValue);
	}

	public bool Set(string settingName, string settingValue)
	{
		string text = settingName.Trim().ToUpper();
		bool flag = false;
		for (int i = 0; i < settingsList.Count; i++)
		{
			string text2 = settingsList[i].ToString();
			if (!text2.StartsWith("'"))
			{
				int num = text2.IndexOf('=');
				if (num > 0 && text2.Substring(0, num).Trim().ToUpper() == text)
				{
					settingsList[i] = settingName + "=" + settingValue;
					flag = true;
				}
			}
		}
		if (!flag)
		{
			settingsList.Add(settingName + "=" + settingValue);
		}
		saveM1IniSettings();
		return true;
	}

	public void setCurrentPath(string path)
	{
		currentPath = path;
	}

	public void clearSettings()
	{
		settingsList.Clear();
	}

	public bool setM1MobileValue(string settingName, string settingValue)
	{
		string path = currentPath;
		if (!currentPath.Contains("m1.ini"))
		{
			path = Path.Combine(currentPath, "m1.ini");
		}
		if (File.Exists(path))
		{
			string text = settingName.Trim().ToUpper();
			bool flag = false;
			for (int i = 0; i < settingsList.Count; i++)
			{
				string text2 = settingsList[i].ToString();
				if (!text2.StartsWith("'"))
				{
					int num = text2.IndexOf('=');
					if (num > 0 && text2.Substring(0, num).Trim().ToUpper() == text)
					{
						settingsList[i] = $"{settingName} = {settingValue} ";
						flag = true;
					}
				}
			}
			if (!flag)
			{
				settingsList.Insert(settingsList.IndexOf("[M1 Mobile Settings]") + 1, $"{settingName} = {settingValue} ");
			}
		}
		else
		{
			settingsList.Insert(settingsList.IndexOf("[M1 Mobile Settings]") + 1, $"{settingName} = {settingValue} ");
		}
		return true;
	}

	public bool Remove(string settingName)
	{
		string text = settingName.Trim().ToUpper();
		bool flag = false;
		for (int i = 0; i < settingsList.Count; i++)
		{
			string text2 = settingsList[i].ToString();
			if (!text2.StartsWith("'"))
			{
				int num = text2.IndexOf('=');
				if (num > 0 && text2.Substring(0, num).Trim().ToUpper() == text)
				{
					settingsList.RemoveAt(i);
					flag = true;
				}
			}
		}
		if (flag)
		{
			saveM1IniSettings();
		}
		return true;
	}

	public bool saveM1MobileSettings()
	{
		string text = currentPath;
		text = ((!text.EndsWith("\\")) ? (text + "\\m1mobile.ini") : (text + "m1mobile.ini"));
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("[System Info]");
		foreach (string settings in settingsList)
		{
			if (settings != "[System Info]")
			{
				stringBuilder.AppendLine(settings);
			}
		}
		File.WriteAllText(text, stringBuilder.ToString());
		return true;
	}

	private bool saveM1IniSettings()
	{
		string text = currentPath;
		if (!currentPath.Contains("m1.ini"))
		{
			text = ((!text.EndsWith("\\")) ? (text + "\\m1.ini") : (text + "m1.ini"));
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("[System Info]");
		foreach (string settings in settingsList)
		{
			if (settings != "[System Info]")
			{
				stringBuilder.AppendLine(settings);
			}
		}
		File.WriteAllText(text, stringBuilder.ToString());
		return true;
	}

	public bool LoadM1IniSettings(string path)
	{
		if (!path.Contains("m1.ini"))
		{
			path = ((!path.EndsWith("\\")) ? (path + "\\m1.ini") : (path + "m1.ini"));
		}
		currentPath = path;
		if (!File.Exists(path))
		{
			return false;
		}
		settingsList = new ArrayList(File.ReadAllText(path).Replace("\n", string.Empty).Split(new char[1] { '\r' }, StringSplitOptions.RemoveEmptyEntries));
		return true;
	}

	public void SetCategoryDisplayTitle(string sPath, string serviceTitle)
	{
		try
		{
			string text = $"[M1 {serviceTitle} Settings]";
			if (File.Exists(sPath) && !File.ReadAllText(sPath).Contains(text))
			{
				File.AppendAllText(sPath, text);
				settingsList.Add(text);
			}
		}
		catch
		{
			throw;
		}
	}

	public bool SetM1ServicesValues(string Name, string Value, string serviceTitle)
	{
		string path = currentPath;
		string value = string.Format(serviceTitle.Equals("System", StringComparison.CurrentCultureIgnoreCase) ? "[{0} Info]" : "[M1 {0} Settings]", serviceTitle.Trim());
		if (!currentPath.Contains("m1.ini"))
		{
			path = Path.Combine(currentPath, "m1.ini");
		}
		if (File.Exists(path))
		{
			bool flag = false;
			for (int i = 0; i < settingsList.Count; i++)
			{
				string text = settingsList[i].ToString();
				if (!text.StartsWith("'"))
				{
					int num = text.IndexOf('=');
					if (num > 0 && string.Equals(text.Substring(0, num).Trim(), Name.Trim(), StringComparison.InvariantCultureIgnoreCase))
					{
						settingsList[i] = Name + " = " + Value + " ";
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				settingsList.Insert(settingsList.IndexOf(value) + 1, Name + " = " + Value + " ");
			}
		}
		else
		{
			settingsList.Insert(settingsList.IndexOf(value) + 1, Name + " = " + Value + " ");
		}
		return true;
	}

	public bool SaveM1Ini()
	{
		return saveM1IniSettings();
	}
}
