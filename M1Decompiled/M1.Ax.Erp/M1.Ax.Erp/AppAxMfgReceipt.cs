using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;
using M1.Extensions;

namespace M1.Ax.Erp;

[AxScript("MfgReceipt")]
[ComVisible(true)]
public class AppAxMfgReceipt : IDisposable
{
	private IServiceProvider _provider;

	private M1Database _database;

	public AppAxMfgReceipt(IServiceProvider parentProvider)
	{
		_provider = parentProvider;
		_database = _provider.GetService(typeof(M1Database)) as M1Database;
	}

	public void PostMfgReceipt(M1BindingSource bindingSource)
	{
		new MfgReceipt().PostMfgReceipt(bindingSource);
	}

	public void PostMfgReceiptScript(object transaction, string mfgReceiptID)
	{
		if (!string.IsNullOrWhiteSpace(mfgReceiptID))
		{
			if (transaction == DBNull.Value)
			{
				transaction = null;
			}
			M1BindingSource m1BindingSource = new M1BindingSource(_database, (SqlTransaction)transaction);
			m1BindingSource.LoadDefinition(string.Empty, "MfgReceipts", null, true);
			m1BindingSource.ClearCache();
			m1BindingSource.NavigateTo(_database, "rmmMfgReceiptID = " + M1Util.ConvertToSql(mfgReceiptID));
			new MfgReceipt().PostMfgReceipt(m1BindingSource);
			m1BindingSource.SaveData();
		}
	}

	public bool MfgReceiptPeriodCheck(M1BindingSource bindingSource)
	{
		return new MfgReceipt().MfgReceiptPeriodCheck(bindingSource);
	}

	public bool MfgReceiptPeriodCheckScript(object transaction, string mfgReceiptID)
	{
		if (string.IsNullOrWhiteSpace(mfgReceiptID))
		{
			return false;
		}
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		M1BindingSource m1BindingSource = new M1BindingSource(_database, (SqlTransaction)transaction);
		m1BindingSource.LoadDefinition(string.Empty, "MfgReceipts", null, true);
		m1BindingSource.ClearCache();
		m1BindingSource.NavigateTo(_database, "rmmMfgReceiptID = " + M1Util.ConvertToSql(mfgReceiptID));
		return new MfgReceipt().MfgReceiptPeriodCheck(m1BindingSource);
	}

	public string MfgReceiptPostCheck(M1BindingSource bindingSource)
	{
		return new MfgReceipt().MfgReceiptPostCheck(bindingSource);
	}

	public string MfgReceiptPostCheckScript(object transaction, string mfgReceiptID)
	{
		if (string.IsNullOrWhiteSpace(mfgReceiptID))
		{
			return string.Empty;
		}
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		M1BindingSource m1BindingSource = new M1BindingSource(_database, (SqlTransaction)transaction);
		m1BindingSource.LoadDefinition(string.Empty, "MfgReceipts", null, true);
		m1BindingSource.ClearCache();
		m1BindingSource.NavigateTo(_database, "rmmMfgReceiptID = " + M1Util.ConvertToSql(mfgReceiptID));
		return new MfgReceipt().MfgReceiptPostCheck(m1BindingSource);
	}

	public bool MfgReceiptPostedCheck(object transaction, string mfgReceiptID)
	{
		if (!string.IsNullOrWhiteSpace(mfgReceiptID))
		{
			if (transaction == DBNull.Value)
			{
				transaction = null;
			}
			return new MfgReceipt().MfgReceiptPostedCheck(_database, (SqlTransaction)transaction, mfgReceiptID);
		}
		return false;
	}

	public void SetMfgReceiptCosts(DataRow row)
	{
		new MfgReceipt().SetMfgReceiptCosts(row, _database);
	}

	public void CompareQtyWithJob(DataRow row, object transaction = null, bool updateJob = false, bool updateMfgReceiptCost = true)
	{
		new MfgReceipt().CompareQtyWithJob(row, _database, (SqlTransaction)transaction, updateMfgReceiptCost, "", completeDateChanged: true, updateJob);
	}

	public void CompareLatestMfgQtyWithJob(string jobId, object transaction, string mfgReceiptId = "")
	{
		new MfgReceipt().CompareLatestMfgQtyWithJob(jobId, mfgReceiptId, _database, (SqlTransaction)transaction);
	}

	public void RefreshMfgReceiptQuantityComplete(DataRow row, object transaction = null, bool useActualRowOnCalculation = true)
	{
		new MfgReceipt().RefreshMfgReceiptQuantityComplete(row, (SqlTransaction)transaction, useActualRowOnCalculation, _database);
	}

	public void UpdateMfgReceiptsToComplete(M1BindingSource mfgReceiptbindingSource, object transaction = null)
	{
		DataRow currentAsDataRow = mfgReceiptbindingSource.CurrentAsDataRow;
		new MfgReceipt().UpdateMfgReceiptsToComplete(currentAsDataRow, (SqlTransaction)transaction, _database);
	}

	public bool GetMfgReceiptInactivePartBinsMessage(M1BindingSource mfgReceiptbindingSource)
	{
		DataRow currentAsDataRow = mfgReceiptbindingSource.CurrentAsDataRow;
		string inactiveBinsMessage;
		return new MfgReceipt().GetMfgReceiptInactivePartBinsMessage(mfgReceiptbindingSource.Database, currentAsDataRow, out inactiveBinsMessage);
	}

	public void Dispose()
	{
		_provider = null;
	}
}
