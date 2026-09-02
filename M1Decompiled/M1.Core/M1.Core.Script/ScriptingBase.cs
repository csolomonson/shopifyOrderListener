using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using MSScriptControl;

namespace M1.Core.Script;

public class ScriptingBase : IDisposable
{
	private ScriptApp _comApp;

	protected bool isExecuting;

	private ScriptControlClass _scriptControl = new ScriptControlClass
	{
		Language = "VBScript",
		Timeout = -1
	};

	protected IServiceProvider _Provider;

	private M1AdoConnectionProxy _adoConnectionProxy;

	public DbAndRowEventArgs ProcessingArgs;

	public ScriptApp ComApp
	{
		get
		{
			return _comApp;
		}
		set
		{
			_comApp = value;
		}
	}

	public ScriptControlClass ScriptControl => _scriptControl;

	public ScriptingBase(IServiceProvider provider)
	{
		_Provider = provider;
		if (provider != null)
		{
			_comApp = provider.GetService(typeof(ScriptApp)) as ScriptApp;
		}
	}

	public virtual void Dispose()
	{
		if (_scriptControl != null)
		{
			if ((_comApp == null || _comApp.databaseRef != null) && !isExecuting)
			{
				_scriptControl.Reset();
			}
			_scriptControl = null;
		}
		if (_adoConnectionProxy != null)
		{
			_adoConnectionProxy.Database = null;
			_adoConnectionProxy = null;
		}
		_comApp = null;
		_Provider = null;
	}

	public void ResetEnvironment()
	{
		_scriptControl.Reset();
	}

	public virtual object Eval(string code)
	{
		if (string.IsNullOrWhiteSpace(code))
		{
			return string.Empty;
		}
		return _scriptControl.Eval(code);
	}

	public virtual void ExecuteStatement(string code)
	{
		_scriptControl.ExecuteStatement(code);
	}

	public void ExecuteEvent(string eventName, object eventSender, object eventArgs)
	{
		if (eventName.Length == 0)
		{
			return;
		}
		bool flag = isExecuting;
		try
		{
			isExecuting = true;
			object[] Parameters;
			if (eventArgs is DbAndRowEventArgs)
			{
				DbAndRowEventArgs e = (DbAndRowEventArgs)eventArgs;
				DbAndRowEventArgs currentDataRowForProcessingQuick = SetCurrentDataRowForProcessingQuick(e);
				SqlTransaction sqlTransaction = _comApp.SqlTransaction;
				_comApp.SqlTransaction = e.SqlTransaction;
				_adoConnectionProxy.SqlTransaction = e.SqlTransaction;
				try
				{
					ScriptControlClass scriptControl = _scriptControl;
					Parameters = new object[3] { eventName, eventSender, eventArgs };
					scriptControl.Run("M1CallEvent", ref Parameters);
					return;
				}
				finally
				{
					_comApp.SqlTransaction = sqlTransaction;
					_adoConnectionProxy.SqlTransaction = sqlTransaction;
					SetCurrentDataRowForProcessingQuick(currentDataRowForProcessingQuick);
				}
			}
			ScriptControlClass scriptControl2 = _scriptControl;
			Parameters = new object[3] { eventName, eventSender, eventArgs };
			scriptControl2.Run("M1CallEvent", ref Parameters);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Script error: " + ex.Message + "\r\nLine: " + _scriptControl.Error.Line + "\r\nFunction: " + eventName.Split('.')[1], "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			isExecuting = flag;
		}
	}

	public DbAndRowEventArgs SetCurrentDataRowForProcessingQuick(DbAndRowEventArgs args)
	{
		DbAndRowEventArgs processingArgs = ProcessingArgs;
		ProcessingArgs = args;
		return processingArgs;
	}

	public object Run(string procedureName, object[] myParams)
	{
		try
		{
			if (!string.IsNullOrEmpty(procedureName))
			{
				Procedure procedure = null;
				for (int i = 1; i <= _scriptControl.Procedures.Count; i++)
				{
					if (_scriptControl.Procedures[i].Name.Equals(procedureName, StringComparison.CurrentCultureIgnoreCase))
					{
						procedure = _scriptControl.Procedures[i];
						break;
					}
				}
				if (procedure != null)
				{
					if (myParams == null)
					{
						myParams = new object[0];
					}
					if (procedure.NumArgs != myParams.Length)
					{
						object[] array = new object[procedure.NumArgs];
						for (int j = 0; j < Math.Min(myParams.Length, array.Length); j++)
						{
							array[j] = myParams[j];
						}
						myParams = array;
					}
					ScriptControlClass scriptControl = _scriptControl;
					object[] Parameters = myParams;
					return scriptControl.Run(procedureName, ref Parameters);
				}
			}
		}
		catch (Exception ex)
		{
			throw new M1Exception("Script error '" + ex.Message + "' on line " + _scriptControl.Error.Line + " in function " + procedureName + ".", ex);
		}
		return string.Empty;
	}

	public void LoadEnvironment()
	{
		LoadEnvironment(useConnectionProxy: false);
	}

	public void LoadEnvironment(bool useConnectionProxy)
	{
		if (isExecuting)
		{
			return;
		}
		if (_adoConnectionProxy == null)
		{
			_adoConnectionProxy = new M1AdoConnectionProxy();
			if (_Provider != null)
			{
				_adoConnectionProxy.Database = _Provider.GetService(typeof(M1Database)) as M1Database;
			}
		}
		_scriptControl.Reset();
		_scriptControl.Timeout = -1;
		_scriptControl.AddCode("Const adOpenStatic=3:Const adLockBatchOptimistic=4:Const adLockReadOnly=1:Const adEditAdd=2:Const adEditInProgress=1:Const adCmdText=1:Const vbHourglass=11:Const vbNormal=0:Const vbDefault=0");
		_scriptControl.AddCode("sub includescript(sScriptName): executeglobal App.GetCode(sScriptName): end sub");
		_scriptControl.AddCode("Dim eventSender:Dim eventArgs");
		_scriptControl.AddCode("sub M1CallEvent(eventName, arg1, arg2): Execute(eventName + \" arg1, arg2 \"):end sub");
		if (_comApp != null)
		{
			_scriptControl.AddCode("Function IsNull(value)\r\nIsNull = App.IsNull(value)\r\nEnd Function\r\n");
			if (useConnectionProxy)
			{
				_scriptControl.AddCode("Function CreateObject(classId)\r\nSet CreateObject = Connection.CreateObject(classId)\r\nEnd Function\r\n");
				_scriptControl.AddObject("Connection", _adoConnectionProxy);
			}
			else if (_comApp.Connection != null)
			{
				_scriptControl.AddObject("Connection", _comApp.Connection);
			}
			_scriptControl.AddObject("App", _comApp);
		}
	}

	public void AddCode(string code)
	{
		try
		{
			_scriptControl.AddCode(code);
		}
		catch
		{
			if (_scriptControl.Error == null || _scriptControl.Error.Description.Length == 0)
			{
				throw;
			}
			throw new M1Exception(_scriptControl.Error.Description + " \rLine: " + _scriptControl.Error.Line + " \rColumn: " + _scriptControl.Error.Column + " \rSource: " + _scriptControl.Error.Text);
		}
	}

	public void AddObject(string name, object objRef)
	{
		_scriptControl.AddObject(name, objRef);
	}

	public void StopTimers()
	{
		if (isExecuting)
		{
			return;
		}
		foreach (string timerControlName in GetTimerControlNames())
		{
			string text = timerControlName + "_Stop";
			string code = "Function " + text + "\r\ncontrols(\"" + timerControlName + "\").Enabled = false\r\nEnd Function\r\n";
			_scriptControl.AddCode(code);
			_scriptControl.ExecuteStatement(text);
		}
	}

	private IEnumerable<string> GetTimerControlNames()
	{
		List<string> list = new List<string>();
		for (int i = 1; i <= _scriptControl.Procedures.Count; i++)
		{
			string name = _scriptControl.Procedures[i].Name;
			if (name.EndsWith("_Timer", StringComparison.CurrentCultureIgnoreCase))
			{
				int lastPositionOfACharacter = GetLastPositionOfACharacter(name, '_');
				string item = name.Substring(0, lastPositionOfACharacter);
				list.Add(item);
			}
		}
		return list;
	}

	private static int GetLastPositionOfACharacter(string word, char character)
	{
		int result = 0;
		for (int i = 0; i < word.Length; i++)
		{
			if (word[i] == character)
			{
				result = i;
			}
		}
		return result;
	}
}
