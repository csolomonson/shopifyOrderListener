using System;
using System.Data;
using System.IO;
using M1.Script.Interfaces;

namespace M1.Core.Script;

public class Scripting : ScriptingBase
{
	public bool IsLoadComplete { get; set; }

	public Scripting(IServiceProvider provider)
		: base(provider)
	{
	}

	public override object Eval(string code)
	{
		if (code.Length == 0)
		{
			return string.Empty;
		}
		LoadEnvironment();
		return base.Eval(code);
	}

	public virtual object Eval(string code, object fields, string objectName)
	{
		if (code.Length == 0)
		{
			return string.Empty;
		}
		LoadEnvironment();
		AddObject(objectName, fields);
		return base.Eval(code);
	}

	public void Execute(string code)
	{
		Execute(code, null, null, string.Empty);
	}

	public void Execute(string code, object forms)
	{
		Execute(code, forms, null, string.Empty);
	}

	public void Execute(string code, object forms, IM1ComCollection controls, string objectName)
	{
		LoadEnvironment();
		if (forms != null)
		{
			AddObject("Forms", forms);
		}
		if (controls != null)
		{
			AddObject(objectName, controls);
		}
		bool flag = isExecuting;
		isExecuting = true;
		try
		{
			base.ExecuteStatement(code);
		}
		finally
		{
			isExecuting = flag;
		}
	}

	private string getProcName(string lineOfCode)
	{
		lineOfCode = lineOfCode.TrimStart();
		if (lineOfCode.StartsWith("Sub ", StringComparison.CurrentCultureIgnoreCase))
		{
			lineOfCode = lineOfCode.Substring(4);
			int num = lineOfCode.IndexOf('(');
			if (num != -1)
			{
				lineOfCode = lineOfCode.Substring(0, num);
			}
			return lineOfCode;
		}
		if (lineOfCode.StartsWith("Function ", StringComparison.CurrentCultureIgnoreCase))
		{
			lineOfCode = lineOfCode.Substring(9);
			int num = lineOfCode.IndexOf('(');
			if (num != -1)
			{
				lineOfCode = lineOfCode.Substring(0, num);
			}
			return lineOfCode;
		}
		return string.Empty;
	}

	public void ExecuteCodeInFile(string fileName)
	{
		if (fileName.Length == 0 || !File.Exists(fileName))
		{
			return;
		}
		string text = File.ReadAllText(fileName);
		if (text.Length == 0)
		{
			return;
		}
		string empty = string.Empty;
		int num = text.IndexOf('\r');
		string lineOfCode = ((num == -1) ? text : text.Substring(0, num));
		empty = getProcName(lineOfCode);
		if (empty.Length == 0)
		{
			bool flag = false;
			int num2 = 0;
			string text2 = "Sub Main()\r";
			string[] array = text.Replace("\n", "").Split('\r');
			foreach (string text3 in array)
			{
				if (!flag && !text3.TrimStart().StartsWith("'"))
				{
					if (text3.TrimStart().StartsWith("Sub ", StringComparison.CurrentCultureIgnoreCase) || text3.TrimStart().StartsWith("Function ", StringComparison.CurrentCultureIgnoreCase))
					{
						flag = true;
						if (num2 == 0)
						{
							empty = getProcName(text3);
							if (empty.Length != 0)
							{
								break;
							}
						}
						text2 += "End Sub\r";
					}
					num2++;
				}
				text2 = text2 + text3 + "\r";
			}
			if (empty.Length == 0)
			{
				if (!flag)
				{
					text2 += "End Sub\r";
				}
				empty = "Main";
				text = text2;
			}
		}
		if (empty.Length != 0)
		{
			ExecuteEmbeddedFunction(text, empty);
		}
	}

	public object ExecuteEmbeddedFunction(string code, string procName)
	{
		return ExecuteEmbeddedFunction(code, procName, new object[0]);
	}

	public object ExecuteEmbeddedFunction(string code, string procName, object[] myParams)
	{
		if (procName.Length != 0)
		{
			LoadEnvironment();
			AddCode(code);
			object service = _Provider.GetService(typeof(IForms));
			if (service != null)
			{
				AddObject("Forms", service);
			}
			return Run(procName, myParams);
		}
		return true;
	}

	public void RunCustomAppCode(M1DataDictionary dataDictionary, string procName)
	{
		RunCustomAppCode(dataDictionary, procName, null);
	}

	public void RunCustomAppCode(M1DataDictionary dataDictionary, string procName, object[] parameters)
	{
		if (procName.Length == 0)
		{
			return;
		}
		foreach (DataRow row in dataDictionary.GetDataTable("Select dkCode from DDScripts Inner Join DDCode On dyUniqueID = dkSourceUniqueID And dkSourceTable = 'DDScripts' Where dyName = 'APP'").Rows)
		{
			string text = row.Field<string>("dkCode");
			if (text != null && text.Length != 0)
			{
				if (parameters == null)
				{
					ExecuteEmbeddedFunction(text, procName);
				}
				else
				{
					ExecuteEmbeddedFunction(text, procName, parameters);
				}
			}
		}
	}
}
