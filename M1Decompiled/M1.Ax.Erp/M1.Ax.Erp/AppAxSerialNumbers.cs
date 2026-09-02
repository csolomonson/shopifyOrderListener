using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("SerialNumbers")]
[ComVisible(true)]
public class AppAxSerialNumbers : IDisposable
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxSerialNumbers(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public string TestSerialNumberFormula(string code)
	{
		using SerialNumberScripting serialNumberScripting = new SerialNumberScripting(provider);
		return serialNumberScripting.TestSerialNumberFormula(code);
	}

	public bool IsSerialTracked(string partID)
	{
		return new SerialNumber().IsSerialTracked(_Database, partID);
	}

	public byte GetCurrentStatus(object transaction, string partID, string revisionID, string serialNumberID)
	{
		return new SerialNumber().GetCurrentStatus(_Database, (SqlTransaction)transaction, partID, revisionID, serialNumberID);
	}

	public void RefreshSerialNumberStatuses(object transaction, string partID, string partRevisionID, string serialNumberID)
	{
		new SerialNumber().RefreshSerialNumberStatuses(_Database, (SqlTransaction)transaction, partID, partRevisionID, serialNumberID);
	}

	public void Dispose()
	{
		provider = null;
	}
}
