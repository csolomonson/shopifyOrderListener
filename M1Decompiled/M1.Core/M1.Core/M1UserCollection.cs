using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using M1.Extensions;

namespace M1.Core;

public class M1UserCollection : KeyedCollection<string, M1User>
{
	private M1DataDictionary dataDictionary;

	private AppContext currentContext;

	public M1UserCollection(M1DataDictionary m1DataDictionary, AppContext context)
	{
		dataDictionary = m1DataDictionary;
		currentContext = context;
	}

	protected override string GetKeyForItem(M1User item)
	{
		return item.ID.ToUpper();
	}

	public LoginReturnInfo LoginUsingPassedCredentials(LoginCredentials loginCredentials)
	{
		return LoginUsingPassedCredentials(loginCredentials, string.Empty);
	}

	public LoginReturnInfo LoginUsingPassedCredentials(LoginCredentials loginCredentials, string additionalHashString)
	{
		Mutex mutex = new Mutex(initiallyOwned: false, "M1" + dataDictionary.Version + "_" + dataDictionary.ID.ToUpper() + "_" + loginCredentials.UserID.ToUpper());
		mutex.WaitOne();
		try
		{
			LoginReturnInfo loginReturnInfo = new LoginReturnInfo();
			if (Contains(loginCredentials.UserID.ToUpper()))
			{
				loginReturnInfo.User = base[loginCredentials.UserID.ToUpper()];
				base[loginCredentials.UserID.ToUpper()].CheckPassword(loginCredentials.Password, additionalHashString);
			}
			if (loginReturnInfo.User == null)
			{
				loginReturnInfo.User = new M1User(dataDictionary);
				loginReturnInfo.User.Login(loginCredentials.UserID, loginCredentials.Password, additionalHashString);
				loginReturnInfo.UserCreated = true;
				Add(loginReturnInfo.User);
			}
			return loginReturnInfo;
		}
		finally
		{
			mutex.ReleaseMutex();
			mutex = null;
		}
	}

	public LoginReturnInfo LoginUsingPassedCredentials(LoginCredentials loginCredentials, string additionalHashString, string databaseName)
	{
		Mutex mutex = new Mutex(initiallyOwned: false, "M1" + dataDictionary.Version + "_" + dataDictionary.ID.ToUpper() + "_" + loginCredentials.UserID.ToUpper());
		mutex.WaitOne();
		try
		{
			LoginReturnInfo loginReturnInfo = new LoginReturnInfo();
			if (Contains(loginCredentials.UserID.ToUpper()))
			{
				loginReturnInfo.User = base[loginCredentials.UserID.ToUpper()];
				base[loginCredentials.UserID.ToUpper()].CheckPassword(loginCredentials.Password, additionalHashString);
			}
			if (loginReturnInfo.User == null)
			{
				loginReturnInfo.User = new M1User(dataDictionary);
				loginReturnInfo.User.Login(loginCredentials.UserID, loginCredentials.Password, additionalHashString, databaseName);
				loginReturnInfo.UserCreated = true;
				Add(loginReturnInfo.User);
			}
			return loginReturnInfo;
		}
		finally
		{
			mutex.ReleaseMutex();
			mutex = null;
		}
	}

	public bool LogoutAndRemove(M1User m1User)
	{
		M1User m1User2 = null;
		for (int num = base.Count - 1; num >= 0; num--)
		{
			if (m1User == null || base[num] == m1User)
			{
				m1User2 = base[num];
				if (!m1User2.IsLoggingOut)
				{
					string key = m1User2.ID.ToUpper();
					if (!m1User2.Logout())
					{
						return false;
					}
					if (base.Dictionary.ContainsKey(key))
					{
						base.Dictionary.Remove(key);
					}
					if (Contains(m1User2))
					{
						Remove(m1User2);
					}
					m1User2.Dispose();
				}
			}
		}
		return true;
	}

	public void ReloadUsers()
	{
		using IEnumerator<M1User> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ReloadUser();
		}
	}

	public void DeleteUser(string userID)
	{
		if (!userID.Equals("ADMIN", StringComparison.CurrentCultureIgnoreCase))
		{
			SqlCommand sqlCommand = dataDictionary.NewSqlCommand("DELETE FROM DDUsers Where duUserID = @UserID");
			sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userID;
			dataDictionary.ExecuteCommand(sqlCommand);
			sqlCommand = dataDictionary.NewSqlCommand("DELETE FROM DDSecurityTables WHERE dtUserID = @UserID");
			sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userID;
			dataDictionary.ExecuteCommand(sqlCommand);
			sqlCommand = dataDictionary.NewSqlCommand("DELETE FROM DDSecurityReports WHERE drUserID = @UserID");
			sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userID;
			dataDictionary.ExecuteCommand(sqlCommand);
			sqlCommand = dataDictionary.NewSqlCommand("DELETE FROM DDExplorer WHERE dxUser = @UserID");
			sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userID;
			dataDictionary.ExecuteCommand(sqlCommand);
			sqlCommand = dataDictionary.NewSqlCommand("DELETE FROM DDSearches WHERE dsUserID = @UserID");
			sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userID;
			dataDictionary.ExecuteCommand(sqlCommand);
			sqlCommand = dataDictionary.NewSqlCommand("DELETE FROM DDSecurityGroups WHERE dzUserID = @UserID Or dzGroupID = @GroupID");
			sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userID;
			sqlCommand.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.NVarChar)).Value = userID;
			dataDictionary.ExecuteCommand(sqlCommand);
			sqlCommand = dataDictionary.NewSqlCommand("DELETE FROM DDFieldUserSettings WHERE daUser = @UserID");
			sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userID;
			dataDictionary.ExecuteCommand(sqlCommand);
			sqlCommand = dataDictionary.NewSqlCommand("DELETE FROM DDObjectsUser WHERE doUserID = @UserID");
			sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userID;
			dataDictionary.ExecuteCommand(sqlCommand);
			sqlCommand = dataDictionary.NewSqlCommand("DELETE FROM DDObjectDetailsUser WHERE dlUserID = @UserID");
			sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userID;
			dataDictionary.ExecuteCommand(sqlCommand);
			return;
		}
		throw new M1Exception("User ADMIN cannot be deleted.");
	}

	public void ChangePassword(string userID, DataRow userRowToChange, string password, bool runUpdateCommand)
	{
		password = currentContext.DDServerManager.Encrypt(password, 20);
		if (userRowToChange != null)
		{
			userRowToChange.SetField("duPassword", password);
			userRowToChange.SetField("duMustChangePassword", value: false);
			userRowToChange.SetField("duPasswordSetDate", DateTime.Now);
		}
		if (runUpdateCommand)
		{
			SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Update DDusers Set duPassword = @Password, duMustChangePassword = 0, duPasswordSetDate = GetDate() Where duUserID = @User");
			sqlCommand.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar)).Value = password;
			sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
			dataDictionary.ExecuteCommand(sqlCommand);
		}
	}

	public void CopySettings(string fromUserID, string toUserID, bool copyDatabaseSecurity, bool copyTableSecurity, bool copyFieldSecurity, bool copyReportSecurity, bool copyReportSettings, bool copyComponents, bool copyGroups, bool copyGeneralSettings, bool copyAutoLogoutSettings, bool copyShortcuts, bool copyExplorer, bool copyStartPage, bool copyCustomGrids, bool copyUserOptions)
	{
		SqlTransaction sqlTransaction = dataDictionary.BeginTransaction();
		try
		{
			if (copyDatabaseSecurity)
			{
				ResetSettingsDatabase(toUserID, sqlTransaction);
				SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Insert Into DDSecurityTables (dtUserID,dtDataset,dtTable,dtField,dtLevel) Select @ToUser As dtUserID,dtDataset,dtTable,dtField,dtLevel From DDSecurityTables Where dtUserID = @FromUser And dtDataset <> '' And dtTable = '' And dtField = ''");
				sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			if (copyTableSecurity)
			{
				ResetSettingsTable(toUserID, sqlTransaction);
				SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Insert Into DDSecurityTables (dtUserID,dtDataset,dtTable,dtField,dtLevel) Select @ToUser As dtUserID,dtDataset,dtTable,dtField,dtLevel From DDSecurityTables Where dtUserID = @FromUser And dtDataset <> '' And dtTable <> '' And dtField = ''");
				sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			if (copyFieldSecurity)
			{
				ResetSettingsField(toUserID, sqlTransaction);
				SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Insert Into DDSecurityTables (dtUserID,dtDataset,dtTable,dtField,dtLevel) Select @ToUser As dtUserID,dtDataset,dtTable,dtField,dtLevel From DDSecurityTables Where dtUserID = @FromUser And dtDataset <> '' And dtTable <> '' And dtField <> ''");
				sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			if (copyReportSecurity)
			{
				ResetSettingsReportSecurity(toUserID, sqlTransaction);
				SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Update dest Set dest.drLevel = source.drLevel From DDSecurityReports dest Right Outer Join DDSecurityReports source On dest.drDataset = source.drDataset And dest.drUserID = @ToUser And source.drUserID = @FromUser And dest.drFolder = source.drFolder And dest.drReport = source.drReport Where dest.drUserID = @ToUser");
				sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
				sqlCommand = dataDictionary.NewSqlCommand("Insert Into DDSecurityReports (drUserID,drDataset,drFolder,drReport,drLevel) Select @ToUser As drUserID,drDataset,drFolder,drReport,drLevel From DDSecurityReports Where drUserID = @FromUser And drDataset+drFolder+drReport Not In (Select drDataset+drFolder+drReport From DDSecurityReports Where drUserID = @ToUser)");
				sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			if (copyReportSettings)
			{
				ResetSettingsReportSettings(toUserID, sqlTransaction);
				SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Update dest Set dest.drSettings = source.drSettings From DDSecurityReports dest Right Outer Join DDSecurityReports source On dest.drDataset = source.drDataset And dest.drUserID = @ToUser And source.drUserID = @FromUser And dest.drFolder = source.drFolder And dest.drReport = source.drReport Where dest.drUserID = @ToUser");
				sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
				sqlCommand = dataDictionary.NewSqlCommand("Insert Into DDSecurityReports (drUserID,drDataset,drFolder,drReport,drSettings) Select @ToUser As drUserID,drDataset,drFolder,drReport,drSettings From DDSecurityReports Where drUserID = @FromUser And drDataset+drFolder+drReport Not In (Select drDataset+drFolder+drReport From DDSecurityReports Where drUserID = @ToUser)");
				sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			if (copyComponents)
			{
				ResetSettingsComponent(toUserID, sqlTransaction);
				SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Insert Into DDSecurityGroups (dzUserID,dzGroupID,dzDataset) Select @ToUser As dzUserID,dzGroupID,dzDataset From DDSecurityGroups Inner Join DDUsers On dzGroupID = duUserID Where dzUserID = @FromUser And duType = 2");
				sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			if (copyGroups)
			{
				ResetSettingsGroup(toUserID, sqlTransaction);
				SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Insert Into DDSecurityGroups (dzUserID,dzGroupID,dzDataset) Select @ToUser As dzUserID,dzGroupID,dzDataset From DDSecurityGroups Inner Join DDUsers On dzGroupID = duUserID Where dzUserID = @FromUser And duType = 1");
				sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			if (copyGeneralSettings)
			{
				SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Update DDUsers Set DDUsers.duAdministrator = Case When dest.duUserID = 'ADMIN' Then 1 Else source.duAdministrator End, DDUsers.duDBAdministrator = Case When dest.duUserID = 'ADMIN' Then 1 Else source.duDBAdministrator End, DDUsers.duDeveloper = source.duDeveloper, DDUsers.duPasswordExpirationDays = source.duPasswordExpirationDays, DDUsers.duMustChangePassword = source.duMustChangePassword, DDUsers.duBackupVerifyDays = source.duBackupVerifyDays, DDUsers.duDDAlertUser = source.duDDAlertUser, DDUsers.duPasswordLocked = source.duPasswordLocked, DDUsers.duAutoLogin = source.duAutoLogin, DDUsers.duGridDeveloper = source.duGridDeveloper From DDUsers dest Inner Join (Select @ToUser As sourceUser,* From DDUsers Where duUserID = @FromUser) As source On dest.duUserID = source.sourceUser Where dest.duUserID = @ToUser");
				sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			if (copyStartPage)
			{
				SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Update DDUsers Set duPortal = (Select duPortal From DDUsers Where duUserID = @FromUser) Where duUserID = @ToUser");
				sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
				string portalGridList = getPortalGridList(fromUserID, toUserID, dataDictionary, sqlTransaction);
				bool flag = false;
				if (portalGridList.Length > 0)
				{
					sqlCommand = new SqlCommand("select Count(*) From DDGridDetails Where dgUserID = @ToUser and dgGridID in (" + portalGridList + ")");
					sqlCommand.Parameters.Add(new SqlParameter("ToUser", SqlDbType.NVarChar)).Value = toUserID;
					if ((int)dataDictionary.ExecuteScalar(sqlCommand, sqlTransaction) > 0)
					{
						flag = true;
					}
				}
				KPIManager kPIManager = new KPIManager();
				if (kPIManager.RecordsExist(toUserID, dataDictionary) || flag)
				{
					switch (MessageBox.Show("Some grids used in Start Page already exist for copy to user.\r If you wish to overwrite them press Yes.\r If you wish to preserve existing KPI's press No.\r If you want to abandon the process press Cancel.", "KPI's Exist", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
					{
					case DialogResult.Yes:
						kPIManager.MoveKpis(fromUserID, toUserID, dataDictionary, bOverwriteExisting: true);
						if (!string.IsNullOrWhiteSpace(portalGridList))
						{
							copyPortalGridSettings(fromUserID, toUserID, portalGridList, dataDictionary, bOverwrite: true, sqlTransaction);
						}
						break;
					case DialogResult.No:
						kPIManager.MoveKpis(fromUserID, toUserID, dataDictionary, bOverwriteExisting: false);
						if (!string.IsNullOrWhiteSpace(portalGridList))
						{
							copyPortalGridSettings(fromUserID, toUserID, portalGridList, dataDictionary, bOverwrite: false, sqlTransaction);
						}
						break;
					}
				}
				else
				{
					kPIManager.MoveKpis(fromUserID, toUserID, dataDictionary, bOverwriteExisting: true);
					if (!string.IsNullOrWhiteSpace(portalGridList))
					{
						copyPortalGridSettings(fromUserID, toUserID, portalGridList, dataDictionary, bOverwrite: true, sqlTransaction);
					}
				}
			}
			if (copyAutoLogoutSettings)
			{
				SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Update DDUsers Set DDUsers.duAutoLogout = source.duAutoLogout, DDUsers.duInactiveCheckMinutes = source.duInactiveCheckMinutes From DDUsers dest Inner Join (Select @ToUser As sourceUser,* From DDUsers Where duUserID = @FromUser) As source On dest.duUserID = source.sourceUser Where dest.duUserID = @ToUser");
				sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			if (copyShortcuts)
			{
				CopyShortcuts(fromUserID, toUserID, sqlTransaction);
			}
			if (copyExplorer)
			{
				SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Delete From DDExplorer Where dxUser = @User And dxMode = @Mode");
				sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = toUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@Mode", SqlDbType.NVarChar)).Value = "TREE";
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
				sqlCommand = dataDictionary.NewSqlCommand("Insert Into DDExplorer (dxUser,dxMode,dxUniqueID,dxText,dxParentUniqueID,dxType,dxExtd,dxGridID,dxSMod,dxSCom,dxSequence,dxDisabled,dxCollapse,dxRemoved,dxCustom,dxLinkedUniqueID,dxViewer,dxVisualizerID,dxVisualizerType) Select @ToUser As dxUser,dxMode,NEWID(),dxText,dxParentUniqueID,dxType,dxExtd,dxGridID,dxSMod,dxSCom,dxSequence,dxDisabled,dxCollapse,dxRemoved,dxCustom,dxLinkedUniqueID,dxViewer,dxVisualizerID,dxVisualizerType From DDExplorer Where dxUser = @FromUser And dxMode = @Mode");
				sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@Mode", SqlDbType.NVarChar)).Value = "TREE";
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			if (copyCustomGrids)
			{
				ResetSettingsCustomGrids(toUserID, sqlTransaction);
				SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Insert Into DDGridDetails (dgUserID,dgGridID,dgGBox,dgExp,dgGrp,dgOrd,dgFlds,dgFrom,dgReqOpt,dgWher,dgSGrp,dgSOrd,dgFBox,dgSQLSet,dgADOSet,dgLOpt,dgShar,dgCustom,dgEdit,dgDatasets,dgPrePane,dgPaneSize,dgPortrait,dgFreeze,dgWebGrid,dgWebSeq,dgSPGroup,dgSPSeq,dgSPText,dgSPCalc,dgCalDateF,dgWgRMACS,dgS1Bold,dgS1Italic,dgS1BColor,dgS1FColor,dgS2Bold,dgS2Italic,dgS2BColor,dgS2FColor,dgS3Bold,dgS3Italic,dgS3BColor,dgS3FColor,dgS4Bold,dgS4Italic,dgS4BColor,dgS4FColor,dgS5Bold,dgS5Italic,dgS5BColor,dgS5FColor,dgSFormula,dgLockd,dgLockf,dgLockg,dgLocks,dgLocko,dgFBoxSP,dgWGFilt) Select @ToUser As dgUserID,dgGridID,dgGBox,dgExp,dgGrp,dgOrd,dgFlds,dgFrom,dgReqOpt,dgWher,dgSGrp,dgSOrd,dgFBox,dgSQLSet,dgADOSet,dgLOpt,dgShar,dgCustom,dgEdit,dgDatasets,dgPrePane,dgPaneSize,dgPortrait,dgFreeze,dgWebGrid,dgWebSeq,dgSPGroup,dgSPSeq,dgSPText,dgSPCalc,dgCalDateF,dgWgRMACS,dgS1Bold,dgS1Italic,dgS1BColor,dgS1FColor,dgS2Bold,dgS2Italic,dgS2BColor,dgS2FColor,dgS3Bold,dgS3Italic,dgS3BColor,dgS3FColor,dgS4Bold,dgS4Italic,dgS4BColor,dgS4FColor,dgS5Bold,dgS5Italic,dgS5BColor,dgS5FColor,dgSFormula,dgLockd,dgLockf,dgLockg,dgLocks,dgLocko,dgFBoxSP,dgWGFilt From DDGridDetails Inner Join DDGrids On dgGridID = djGridID Where dgUserID = @FromUser And djUserID = ''");
				sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			if (copyUserOptions)
			{
				SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Update DDUsers Set duProperties = (Select duProperties From DDUsers Where duUserID = @FromUser) Where duUserID = @ToUser");
				sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
				sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
				dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			dataDictionary.CommitTransaction(sqlTransaction);
		}
		catch
		{
			dataDictionary.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	private void CopyShortcuts(string fromUserID, string toUserID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Delete From DDExplorer Where dxUser = @User And dxMode = @Mode");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = toUserID;
		sqlCommand.Parameters.Add(new SqlParameter("@Mode", SqlDbType.NVarChar)).Value = "SBAR";
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
		CopyParentShortcuts(fromUserID, toUserID, transaction);
		CopyChildrenShortcuts(fromUserID, toUserID, transaction);
	}

	private void CopyChildrenShortcuts(string fromUserID, string toUserID, SqlTransaction transaction)
	{
		string text = "Select newParent.dxUser,copyfrom.dxMode,NEWID() as dxUniqueID\r\n                    ,copyfrom.dxText,  newParent.dxUniqueID as dxParentUniqueID\r\n                    ,copyfrom.dxType,copyfrom.dxExtd,copyfrom.dxGridID,copyfrom.dxSMod,copyfrom.dxSCom,copyfrom.dxSequence,copyfrom.dxDisabled\r\n                    ,copyfrom.dxCollapse,copyfrom.dxRemoved,copyfrom.dxCustom,copyfrom.dxLinkedUniqueID,copyfrom.dxViewer \r\n           ,copyfrom.dxVisualizerID\r\n           ,copyfrom.dxVisualizerType\r\n           ,copyfrom.dxImageLarge\r\n           ,copyfrom.dxImageSmall\r\n           ,copyfrom.dxLanguageID\r\n           ,copyfrom.dxAppExtensionID\r\n                    From DDExplorer copyfrom\r\n                    inner join DDExplorer newParent\r\n                    on newParent.dxMode=copyfrom.dxMode \r\n                    and newParent.dxUser=@ToUser \r\n                    inner join DDExplorer copyfromParent\r\n                    on copyfromParent.dxText=newParent.dxText \r\n                    and copyfromParent.dxuser=copyfrom.dxuser \r\n                    and copyfromParent.dxMode=copyfrom.dxmode\r\n                    and copyfromParent.dxParentUniqueID is null\r\n                    and copyfrom.dxParentUniqueID=copyfromParent.dxUniqueID\r\n                    Where copyfrom.dxUser = @FromUser And copyfrom.dxMode = @Mode";
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Insert Into DDExplorer (dxUser,dxMode,dxUniqueID,dxText,dxParentUniqueID,dxType,dxExtd\r\n                                                   ,dxGridID,dxSMod,dxSCom,dxSequence,dxDisabled,dxCollapse,dxRemoved,dxCustom,dxLinkedUniqueID,dxViewer\r\n           ,dxVisualizerID\r\n           ,dxVisualizerType\r\n           ,dxImageLarge\r\n           ,dxImageSmall\r\n           ,dxLanguageID\r\n            ,dxAppExtensionID\r\n            ) " + text);
		sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
		sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
		sqlCommand.Parameters.Add(new SqlParameter("@Mode", SqlDbType.NVarChar)).Value = "SBAR";
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
	}

	private void CopyParentShortcuts(string fromUserID, string toUserID, SqlTransaction transaction)
	{
		string text = "Select @ToUser As dxUser, dxMode, NEWID(), dxText,null,dxType,dxExtd,dxGridID,dxSMod\r\n                           ,dxSCom,dxSequence,dxDisabled,dxCollapse,dxRemoved,dxCustom,dxLinkedUniqueID,dxViewer\r\n                   ,dxVisualizerID\r\n                   ,dxVisualizerType\r\n                   ,dxImageLarge\r\n                   ,dxImageSmall\r\n                   ,dxLanguageID\r\n                   ,dxAppExtensionID                   \r\n                          From DDExplorer \r\n                          Where dxUser = @FromUser And dxMode = @Mode and dxParentUniqueID is null";
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Insert Into DDExplorer (dxUser,dxMode,dxUniqueID,dxText,dxParentUniqueID,dxType,dxExtd,dxGridID\r\n                                          ,dxSMod,dxSCom,dxSequence,dxDisabled,dxCollapse,dxRemoved,dxCustom,dxLinkedUniqueID,dxViewer\r\n               ,dxVisualizerID\r\n               ,dxVisualizerType\r\n               ,dxImageLarge\r\n               ,dxImageSmall\r\n               ,dxLanguageID\r\n               ,dxAppExtensionID                   \r\n                ) " + text);
		sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUserID;
		sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUserID;
		sqlCommand.Parameters.Add(new SqlParameter("@Mode", SqlDbType.NVarChar)).Value = "SBAR";
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
	}

	private string getPortalGridList(string fromUser, string toUser, M1DataDictionary dataDictionary, SqlTransaction transaction)
	{
		string text = string.Empty;
		SqlCommand sqlCommand = new SqlCommand("Select duPortal From DDUsers Where duUserID = @fromUserID");
		sqlCommand.Parameters.Add(new SqlParameter("@fromUserID", SqlDbType.NVarChar)).Value = fromUser;
		DataTable dataTable = dataDictionary.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count > 0)
		{
			string text2 = dataTable.Rows[0].Field<string>("duPortal").Trim();
			text2.Replace("\n", string.Empty);
			string[] array = text2.Split('\r');
			foreach (string text3 in array)
			{
				string text4 = string.Empty;
				int num = text3.IndexOf("^");
				if (num > 0)
				{
					text4 = text3.Substring(num + 1);
				}
				string[] array2 = text4.Split('|');
				foreach (string obj in array2)
				{
					string[] array3 = null;
					array3 = obj.Split('~');
					if (array3.Length >= 3 && array3[2].Length > 0)
					{
						text = text + "," + array3[2].ToSql();
					}
				}
			}
			if (text.Length > 1)
			{
				return text.Substring(1);
			}
		}
		return text;
	}

	private void copyPortalGridSettings(string fromUser, string toUser, string gridList, M1DataDictionary dataDictionary, bool bOverwrite, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = null;
		string empty = string.Empty;
		DDTableDefinition table = new DDDatabaseDefinition().GetTable("DDGridDetails");
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		foreach (DDFieldDefinition field in table.Fields)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(",");
				stringBuilder2.Append(",");
			}
			stringBuilder2.Append(field.FieldName);
			if (field.FieldName.Equals("dgUserID", StringComparison.CurrentCultureIgnoreCase))
			{
				stringBuilder.Append("@ToUser");
			}
			else
			{
				stringBuilder.Append(field.FieldName);
			}
		}
		empty = ((!bOverwrite) ? ("INSERT INTO DDGridDetails (" + stringBuilder2.ToString() + ") Select " + stringBuilder.ToString() + " FROM DDGridDetails Where dgUserID = @FromUser and dgGridID in (" + gridList + ") and dgGridID Not In (Select dgGridID From DDGridDetails Where dgUserID = @ToUser And dgGridID in (" + gridList + "))") : ("Delete From DDGridDetails Where dgUserID = @ToUser And dgGridID in (" + gridList + ");INSERT INTO DDGridDetails (" + stringBuilder2.ToString() + ") Select " + stringBuilder.ToString() + " FROM DDGridDetails Where dgUserID = @FromUser and dgGridID in (" + gridList + ")"));
		sqlCommand = new SqlCommand(empty);
		sqlCommand.Parameters.Add(new SqlParameter("@FromUser", SqlDbType.NVarChar)).Value = fromUser;
		sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = toUser;
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
	}

	public void ResetSettingsDatabase(string userID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Delete From DDSecurityTables Where dtUserID = @User And dtDataset <> '' And dtTable = '' And dtField = ''");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
	}

	public void ResetSettingsTable(string userID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Delete From DDSecurityTables Where dtUserID = @User And dtDataset <> '' And dtTable <> '' And dtField = ''");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
	}

	public void ResetSettingsField(string userID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Delete From DDSecurityTables Where dtUserID = @User And dtDataset <> '' And dtTable <> '' And dtField <> ''");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
	}

	public void ResetSettingsReportSecurity(string userID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Update DDSecurityReports Set drLevel = 0 Where drUserID = @User");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
	}

	public void ResetSettingsReportSettings(string userID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Update DDSecurityReports Set drSettings = Null Where drUserID = @User");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
	}

	public void ResetSettingsComponent(string userID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Delete DDSecurityGroups From DDSecurityGroups Inner Join DDUsers On dzGroupID = duUserID Where dzUserID = @User And duType = 2");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
	}

	public void ResetSettingsGroup(string userID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Delete DDSecurityGroups From DDSecurityGroups Inner Join DDUsers On dzGroupID = duUserID Where dzUserID = @User And duType = 1");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
	}

	public void ResetSettingsGeneral(string userID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Update DDUsers Set DDUsers.duAdministrator = Case When duUserID = 'ADMIN' Then 1 Else 0 End, DDUsers.duDBAdministrator = Case When duUserID = 'ADMIN' Then 1 Else 0 End, DDUsers.duDeveloper = 0, DDUsers.duPasswordExpirationDays = 0, DDUsers.duMustChangePassword = 0, DDUsers.duBackupVerifyDays = 0, DDUsers.duDDAlertUser = 0, DDUsers.duPasswordLocked = 0, DDUsers.duAutoLogin = 0, DDUsers.duGridDeveloper = 0 Where duUserID = @ToUser");
		sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = userID;
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
	}

	public void ResetSettingsStartPage(string userID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Update DDUsers Set duPortal = @Portal Where duUserID = @ToUser");
		sqlCommand.Parameters.Add(new SqlParameter("@Portal", SqlDbType.NVarChar)).Value = GetDefaultPortalString();
		sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = userID;
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
	}

	public void ResetSettingsAutoLogout(string userID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Update DDUsers Set DDUsers.duAutoLogout = Null, DDUsers.duInactiveCheckMinutes = 0 Where duUserID = @ToUser");
		sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = userID;
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
	}

	public void ResetSettingsShortcuts(string userID, SqlTransaction transaction)
	{
		new DmoDD(currentContext).LoadDDExplorerDefault(dataDictionary.ID, userID, "SBAR", transaction);
	}

	public void ResetSettingsExplorer(string userID, SqlTransaction transaction)
	{
		new DmoDD(currentContext).LoadDDExplorerDefault(dataDictionary.ID, userID, "TREE", transaction);
	}

	public void ResetSettingsCustomGrids(string userID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Delete DDGridDetails From DDGridDetails Inner Join DDGrids On dgGridID = djGridID Where dgUserID = @User And djUserID = ''");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
	}

	public void ResetSettingsUserOptions(string userID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Update DDUsers Set duProperties = Null Where duUserID = @ToUser");
		sqlCommand.Parameters.Add(new SqlParameter("@ToUser", SqlDbType.NVarChar)).Value = userID;
		dataDictionary.ExecuteCommand(sqlCommand, transaction);
	}

	public string GetDefaultPortalString()
	{
		return "100%^M1.M1ViewPortalGroup,100%,";
	}

	public bool CreateUser(string userID, string userName, bool developer, bool administrator, bool dbadministrator, bool gridDeveloper, bool group)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Select * From DDUsers Where duUserID = @User");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		SqlDataAdapter adapter;
		DataTable dataTable = dataDictionary.GetDataTable(sqlCommand, fillSchema: true, out adapter);
		if (dataTable.Rows.Count == 0)
		{
			DataRow dataRow = dataTable.NewRow();
			dataRow.BlankRow();
			dataRow.BeginEdit();
			dataRow.SetField("duUserID", userID);
			dataRow.SetField("duName", userName);
			dataRow.SetField("duAdministrator", administrator);
			dataRow.SetField("duDBAdministrator", dbadministrator);
			dataRow.SetField("duDeveloper", developer);
			dataRow.SetField("duGridDeveloper", gridDeveloper);
			dataRow.SetField("duType", group ? ((byte)1) : ((byte)0));
			dataRow.SetField("duPortal", GetDefaultPortalString());
			dataRow.SetField("duCustom", value: true);
			dataRow.SetField<string>("duAutoLogout", null);
			dataRow.EndEdit();
			dataTable.Rows.Add(dataRow);
			dataDictionary.UpdateData(new DataRow[1] { dataRow }, adapter);
			if (dataDictionary.Version.CompareTo("7.50.007") >= 0)
			{
				ResetSettingsShortcuts(userID, null);
				ResetSettingsExplorer(userID, null);
			}
			M1BindingSource.ChangedRowsInfo changedRowsInfo = new M1BindingSource.ChangedRowsInfo(dataTable);
			using IEnumerator<M1User> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				foreach (M1Database database in enumerator.Current.Databases)
				{
					database.OnTableChanged(new TableChangedEventArgs("DDUsers", changedRowsInfo.AddedRows, changedRowsInfo.ChangedRows, changedRowsInfo.DeletedRows));
				}
			}
		}
		return true;
	}

	public void CopySecurityToDatabase(string fromDatabase, string toDatabase)
	{
		string empty = string.Empty;
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Select * From DDUsers Where duType <> 2");
		foreach (DataRow row in dataDictionary.GetDataTable(sqlCommand).Rows)
		{
			empty = row.Field<string>("duUserID");
			CopySecurityToDatabase(fromDatabase, toDatabase, empty);
		}
	}

	public void CopySecurityToDatabase(string fromDatabase, string toDatabase, string userID)
	{
		SqlTransaction sqlTransaction = dataDictionary.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Delete From DDSecurityTables Where dtUserID = @User And dtDataset = @ToDatabase");
			sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
			sqlCommand.Parameters.Add(new SqlParameter("@ToDatabase", SqlDbType.NVarChar)).Value = toDatabase;
			dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand = dataDictionary.NewSqlCommand("Insert Into DDSecurityTables (dtUserID,dtDataset,dtTable,dtField,dtLevel) Select @User As dtUserID,@ToDatabase AS dtDataset,dtTable,dtField,dtLevel From DDSecurityTables Where dtUserID = @User And dtDataset = @FromDatabase");
			sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
			sqlCommand.Parameters.Add(new SqlParameter("@FromDatabase", SqlDbType.NVarChar)).Value = fromDatabase;
			sqlCommand.Parameters.Add(new SqlParameter("@toDatabase", SqlDbType.NVarChar)).Value = toDatabase;
			dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand = dataDictionary.NewSqlCommand("Delete From DDSecurityReports Where drUserID = @User And drDataset = @ToDatabase");
			sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
			sqlCommand.Parameters.Add(new SqlParameter("@ToDatabase", SqlDbType.NVarChar)).Value = toDatabase;
			dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand = dataDictionary.NewSqlCommand("Insert Into DDSecurityReports (drUserID,drDataset,drFolder,drReport,drLevel,drSettings) Select @User As drUserID,@ToDatabase AS drDataset,drFolder,drReport,drLevel,drSettings From DDSecurityReports Where drUserID = @User And drDataset = @FromDatabase");
			sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
			sqlCommand.Parameters.Add(new SqlParameter("@FromDatabase", SqlDbType.NVarChar)).Value = fromDatabase;
			sqlCommand.Parameters.Add(new SqlParameter("@toDatabase", SqlDbType.NVarChar)).Value = toDatabase;
			dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand = dataDictionary.NewSqlCommand("Delete From DDSecurityGroups Where dzUserID = @User And dzDataset = @ToDatabase");
			sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
			sqlCommand.Parameters.Add(new SqlParameter("@ToDatabase", SqlDbType.NVarChar)).Value = toDatabase;
			dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand = dataDictionary.NewSqlCommand("Insert Into DDSecurityGroups (dzUserID,dzDataset,dzGroupID) Select @User As dzUserID,@ToDatabase AS dzDataset,dzGroupId From DDSecurityGroups Where dzUserID = @User And dzDataset = @FromDatabase");
			sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
			sqlCommand.Parameters.Add(new SqlParameter("@FromDatabase", SqlDbType.NVarChar)).Value = fromDatabase;
			sqlCommand.Parameters.Add(new SqlParameter("@toDatabase", SqlDbType.NVarChar)).Value = toDatabase;
			dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			dataDictionary.CommitTransaction(sqlTransaction);
		}
		catch (Exception)
		{
			dataDictionary.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	public void RenameDatabaseSecurityUpdate(string fromDatabase, string toDatabase)
	{
		SqlTransaction sqlTransaction = dataDictionary.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Delete From DDSecurityTables Where dtDataset = @ToDatabase");
			sqlCommand.Parameters.Add(new SqlParameter("@ToDatabase", SqlDbType.NVarChar)).Value = toDatabase;
			dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand = dataDictionary.NewSqlCommand("Update DDSecurityTables Set dtDataset = @toDatabase Where dtDataset = @FromDatabase");
			sqlCommand.Parameters.Add(new SqlParameter("@FromDatabase", SqlDbType.NVarChar)).Value = fromDatabase;
			sqlCommand.Parameters.Add(new SqlParameter("@toDatabase", SqlDbType.NVarChar)).Value = toDatabase;
			dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand = dataDictionary.NewSqlCommand("Delete From DDSecurityReports Where drDataset = @ToDatabase");
			sqlCommand.Parameters.Add(new SqlParameter("@ToDatabase", SqlDbType.NVarChar)).Value = toDatabase;
			dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand = dataDictionary.NewSqlCommand("Update DDSecurityReports Set drDataset = @toDatabase Where drDataset = @FromDatabase");
			sqlCommand.Parameters.Add(new SqlParameter("@FromDatabase", SqlDbType.NVarChar)).Value = fromDatabase;
			sqlCommand.Parameters.Add(new SqlParameter("@toDatabase", SqlDbType.NVarChar)).Value = toDatabase;
			dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand = dataDictionary.NewSqlCommand("Delete From DDSecurityGroups Where dzDataset = @ToDatabase");
			sqlCommand.Parameters.Add(new SqlParameter("@ToDatabase", SqlDbType.NVarChar)).Value = toDatabase;
			dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand = dataDictionary.NewSqlCommand("Update DDSecurityGroups Set dzDataset = @toDatabase Where dzDataset = @FromDatabase");
			sqlCommand.Parameters.Add(new SqlParameter("@FromDatabase", SqlDbType.NVarChar)).Value = fromDatabase;
			sqlCommand.Parameters.Add(new SqlParameter("@toDatabase", SqlDbType.NVarChar)).Value = toDatabase;
			dataDictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			dataDictionary.CommitTransaction(sqlTransaction);
		}
		catch (Exception)
		{
			throw;
		}
	}
}
