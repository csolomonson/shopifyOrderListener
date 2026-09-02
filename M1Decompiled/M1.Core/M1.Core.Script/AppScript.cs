using System;
using System.Runtime.InteropServices;
using M1.Script.Interfaces;

namespace M1.Core.Script;

public class AppScript : IScript
{
	private M1Database databaseRef;

	public AppScript(M1Database database)
	{
		databaseRef = database;
	}

	public object Eval(string code, object fields = null)
	{
		if (fields == null)
		{
			return databaseRef.ScriptingQuick.Eval(code);
		}
		return databaseRef.Scripting.Eval(code, fields, "Fields");
	}

	public void Execute(string code)
	{
		databaseRef.Scripting.Execute(code);
	}

	public object ExecuteReportCode(string code)
	{
		using ReportScripting reportScripting = new ReportScripting(databaseRef);
		object obj = reportScripting.ExecuteReportCodeRs(code);
		if (obj == null)
		{
			return new UnknownWrapper(DBNull.Value);
		}
		return obj;
	}

	public void RunCustomAppCode(string cFunction, object cParameter = null)
	{
		databaseRef.Scripting.RunCustomAppCode(databaseRef.GetService(typeof(M1DataDictionary)) as M1DataDictionary, cFunction, (cParameter == null) ? null : new object[1] { cParameter });
	}

	public IScriptEngine CreateScriptEngine()
	{
		return new ComScriptEngine(databaseRef);
	}
}
