using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;
using M1.Extensions;
using M1.ServiceCore.AxScript;

namespace M1.Ax.Erp;

[AxScript("MaterialIssue")]
[ComVisible(true)]
public class AppAxMaterialIssue : IDisposable, IWebAxMaterialIssue
{
	private IServiceProvider _provider;

	private M1Database _database;

	public AppAxMaterialIssue(IServiceProvider parentProvider)
	{
		_provider = parentProvider;
		_database = _provider.GetService(typeof(M1Database)) as M1Database;
	}

	public string PostMaterialIssueCheck(M1BindingSource bindingSource)
	{
		return new MaterialIssue().PostMaterialIssueCheck(bindingSource);
	}

	public string VerifyInactiveBinsMiscOrJobIssue(M1BindingSource bindingSource)
	{
		return new MaterialIssue().VerifyInactiveBinsMiscOrJobIssue(bindingSource);
	}

	public string VerifyInactiveBinsForReturnToJob(M1BindingSource bindingSource)
	{
		return new MaterialIssue().VerifyInactiveBinsForReturnToJob(bindingSource);
	}

	public string PostMaterialIssueCheckScript(object transaction, string materialIssueID)
	{
		if (string.IsNullOrWhiteSpace(materialIssueID))
		{
			return string.Empty;
		}
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		M1BindingSource m1BindingSource = new M1BindingSource(_database, (SqlTransaction)transaction);
		m1BindingSource.LoadDefinition(string.Empty, "MaterialIssues", null, true);
		m1BindingSource.ClearCache();
		m1BindingSource.NavigateTo(_database, "iniMaterialIssueID = " + M1Util.ConvertToSql(materialIssueID));
		m1BindingSource.PrimaryTable.GetChildBindingSource("MaterialIssueLines");
		return new MaterialIssue().PostMaterialIssueCheck(m1BindingSource);
	}

	public bool MaterialIssuePeriodCheck(M1BindingSource bindingSource)
	{
		return new MaterialIssue().MaterialIssuePeriodCheck(bindingSource);
	}

	public bool MaterialIssuePeriodCheckScript(object transaction, string materialIssueID)
	{
		if (string.IsNullOrWhiteSpace(materialIssueID))
		{
			return false;
		}
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		M1BindingSource m1BindingSource = new M1BindingSource(_database, (SqlTransaction)transaction);
		m1BindingSource.LoadDefinition(string.Empty, "MaterialIssues", null, true);
		m1BindingSource.ClearCache();
		m1BindingSource.NavigateTo(_database, "iniMaterialIssueID = " + M1Util.ConvertToSql(materialIssueID));
		m1BindingSource.PrimaryTable.GetChildBindingSource("MaterialIssueLines");
		return new MaterialIssue().MaterialIssuePeriodCheck(m1BindingSource);
	}

	public bool MaterialIssuePostedCheck(object transaction, string materialIssueID)
	{
		if (string.IsNullOrWhiteSpace(materialIssueID))
		{
			return false;
		}
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new MaterialIssue().MaterialIssuePostedCheck(_database, (SqlTransaction)transaction, materialIssueID);
	}

	public void PostMaterialIssue(M1BindingSource bindingSource)
	{
		new MaterialIssue().PostMaterialIssue(bindingSource);
	}

	public void PostMaterialIssueScript(object transaction, string materialIssueID)
	{
		if (string.IsNullOrWhiteSpace(materialIssueID))
		{
			return;
		}
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		using M1BindingSource m1BindingSource = new M1BindingSource(_database, (SqlTransaction)transaction);
		m1BindingSource.LoadDefinition(string.Empty, "MaterialIssues", null, true);
		m1BindingSource.ClearCache();
		m1BindingSource.NavigateTo(_database, "iniMaterialIssueID = " + M1Util.ConvertToSql(materialIssueID));
		using (m1BindingSource.PrimaryTable.GetChildBindingSource("MaterialIssueLines"))
		{
			new MaterialIssue().PostMaterialIssue(m1BindingSource);
			m1BindingSource.SaveData();
		}
	}

	public string CheckMaterialIssueForFutureAdjustmentTransactions(M1BindingSource bindingsource)
	{
		return new MaterialIssue().CheckMaterialIssueForFutureAdjustmentTransactions(bindingsource);
	}

	public string RunMaterialIssueReversal(object[] promptValues)
	{
		return new ScriptApp(_database).RunTransferProcess("M1.Ax.Erp.MaterialIssueReversalProcess", _database, promptValues);
	}

	public void CreateMaterialIssueJournalsFromBackflush(string materialIssueID)
	{
		new MaterialIssue().CreateMaterialIssueJournalsFromBackflush(_database, materialIssueID);
	}

	public void Dispose()
	{
		_provider = null;
	}
}
