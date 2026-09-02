using System;
using System.Data;
using System.Data.SqlClient;
using M1.Ax.Erp.JobSchedule;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class StartUp : IAppExtensionLogin
{
	protected M1Database _Database;

	public void OnLogin(M1Database database)
	{
		_Database = database;
		checkForLanguage();
		_Database.PropsRefreshed -= _Database_PropsRefreshed;
		_Database.PropsRefreshed += _Database_PropsRefreshed;
		_Database.Security.RoleCheck -= Security_RoleCheck;
		_Database.Security.RoleCheck += Security_RoleCheck;
		_Database.ConstructWhere -= _Database_ConstructWhere;
		_Database.ConstructWhere += _Database_ConstructWhere;
		if ((database.GetService(typeof(M1DataDictionary)) as M1DataDictionary).ProductCode.IsModulePurchased("HD", database))
		{
			_Database.EmailMessageSent -= _Database_EmailMessageSent;
			_Database.EmailMessageSent += _Database_EmailMessageSent;
		}
		_Database.RemoveService(typeof(IGetWorkingDaysService));
		_Database.AddService(typeof(IGetWorkingDaysService), new ScheduleProcess.GetCheckRange());
	}

	private void _Database_ConstructWhere(object sender, ConstructWhereEventArgs e)
	{
		if (e.ExtraFilter.Length == 0 && e.TableName.Equals("GLAccounts", StringComparison.CurrentCultureIgnoreCase))
		{
			string text = e.Database.Props("DatasetProperties").Field<string>("xadGLDivisionID").Trim();
			if (text.Length != 0)
			{
				e.AddToWhereClause("glaGLDivisionID = " + text.ToSql());
			}
			text = e.Database.Props("DatasetProperties").Field<string>("xadGLDepartmentID").Trim();
			if (text.Length != 0)
			{
				e.AddToWhereClause("glaGLDepartmentID = " + text.ToSql());
			}
			text = e.Database.Props("DatasetProperties").Field<string>("xadGLChartPrefix").Trim();
			if (text.Length != 0)
			{
				e.AddToWhereClause("LEFT(glaGLChartID," + text.Length + ") = " + text.ToSql());
			}
		}
	}

	private void _Database_EmailMessageSent(object sender, EmailMessageSentEventArgs e)
	{
		if (e.Message.CreateCall)
		{
			new Call().CreateCallForEmail(e.Database, e.Message);
		}
	}

	private void Security_RoleCheck(object sender, RoleCheckEventArgs e)
	{
		if (e.RoleID.Equals("SHIPMENTPOST", StringComparison.CurrentCultureIgnoreCase) || e.RoleID.Equals("RECEIPTPOST", StringComparison.CurrentCultureIgnoreCase))
		{
			return;
		}
		if (e.RoleID.Equals("CREDITCARDPAYPAL", StringComparison.CurrentCultureIgnoreCase))
		{
			if (!new Financial().IsPaypalActivated(_Database))
			{
				e.Cancel = true;
			}
		}
		else if (e.RoleID.Equals("CREDITCARDNET1", StringComparison.CurrentCultureIgnoreCase))
		{
			if (!new Financial().IsNET1Activated(_Database))
			{
				e.Cancel = true;
			}
		}
		else if (e.RoleID.Equals("INTRACOMPANYPOST", StringComparison.CurrentCultureIgnoreCase))
		{
			if (!_Database.Props("DS").Field<bool>("xadAllowIntraCompanyTrans"))
			{
				e.Cancel = true;
			}
		}
		else if (e.RoleID.Equals("EASYORDER", StringComparison.CurrentCultureIgnoreCase) && string.IsNullOrEmpty(_Database.Props("PM").Field<string>("xapEasyOrderURL")))
		{
			e.Cancel = true;
		}
	}

	private void _Database_PropsRefreshed(object sender, EventArgs e)
	{
		checkForLanguage();
	}

	private void checkForLanguage()
	{
		SqlCommand sqlCommand = _Database.NewSqlCommand("select TOP 1 RTrim(lmeLanguage) from Employees where lmeUserID = @UserID and lmeTerminationDate IS NULL order by lmeEmployeeID");
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = _Database.User.ID;
		string text = (string)_Database.ExecuteScalar(sqlCommand);
		if (!string.IsNullOrEmpty(text))
		{
			_Database.LanguageTable = text;
		}
	}
}
