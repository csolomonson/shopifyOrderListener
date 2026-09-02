using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;
using M1.Script.Interfaces;
using M1Classes92;

namespace M1.Ax.Erp;

[AxScript("Legacy")]
[ComVisible(true)]
public class AppAxLegacy : IDisposable
{
	private IServiceProvider provider;

	private M1Database databaseRef;

	private clsLegacyFunctions vbRef;

	public AppAxLegacy(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		databaseRef = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public void RefreshCurrencyForDetails(string table, object data, bool fixForeign)
	{
		getRef().RefreshCurrencyForDetails(table, data, fixForeign, "", Type.Missing);
	}

	private clsLegacyFunctions getRef()
	{
		if (vbRef == null)
		{
			vbRef = new clsLegacyFunctionsClass();
			vbRef.SetReferences(provider.GetService(typeof(ScriptApp)), provider.GetService(typeof(IForms)));
		}
		return vbRef;
	}

	public void Dispose()
	{
		if (vbRef != null)
		{
			vbRef = null;
		}
		databaseRef = null;
		provider = null;
	}
}
