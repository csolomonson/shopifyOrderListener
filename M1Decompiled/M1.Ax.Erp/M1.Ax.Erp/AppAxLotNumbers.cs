using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("LotNumbers")]
[ComVisible(true)]
public class AppAxLotNumbers : IDisposable
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxLotNumbers(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public bool IsLotTracked(string partID)
	{
		return new LotNumber().IsLotTracked(_Database, partID);
	}

	public bool IsLotInActive(object transaction, string partID, string revisionID, string lotNumberID)
	{
		return new LotNumber().IsLotInactive(_Database, (SqlTransaction)transaction, partID, revisionID, lotNumberID);
	}

	public bool IsLotUnassigned(object transaction, string partID, string revisionID, string lotNumberID)
	{
		return new LotNumber().IsLotUnassigned(_Database, (SqlTransaction)transaction, partID, revisionID, lotNumberID);
	}

	public void RefreshLotNumberStatuses(object transaction, string partID, string partRevisionID, string lotNumberID)
	{
		new LotNumber().RefreshLotNumberStatuses(_Database, (SqlTransaction)transaction, partID, partRevisionID, lotNumberID);
	}

	public void Dispose()
	{
		provider = null;
	}
}
