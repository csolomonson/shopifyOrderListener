using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.Utilities;
using M1.Core;
using M1.Extensions;

namespace M1.API.Repositories;

public sealed class APIClientRepository : IDisposable
{
	private M1ProductCode ProductCodes;

	private LoginCredentials LoginCredentialsObj;

	private string GetModuleID(string contextModuleId, out string message)
	{
		string result = string.Empty;
		message = string.Empty;
		if (contextModuleId.Trim().ToUpper().Equals("EDI", StringComparison.CurrentCultureIgnoreCase))
		{
			result = "12";
		}
		else if (contextModuleId.Trim().ToUpper().Equals("EO", StringComparison.CurrentCultureIgnoreCase))
		{
			result = "10";
		}
		else if (contextModuleId.Trim().ToUpper().Equals("BOM", StringComparison.CurrentCultureIgnoreCase))
		{
			result = "9";
		}
		else if (contextModuleId.Trim().ToUpper().Equals("ERP", StringComparison.CurrentCultureIgnoreCase))
		{
			result = "16";
		}
		else
		{
			message = string.Format("{0}: {1}", "FAIL", "Invalid Module Key");
		}
		return result;
	}

	private M1ProductCode LoadProductCodes(APIClientContext apiClientContext)
	{
		ProductCodes = new M1ProductCode(apiClientContext.DataDictionary, apiClientContext.DbContext);
		ProductCodes.LoadProductCode(ProductCodes.GetDDProductCode(apiClientContext.DataDictionaryID));
		ProductCodes.LoadCustomProductIDFromIni();
		return ProductCodes;
	}

	private M1User GetUser(APIClientContext apiClientContext)
	{
		if (string.IsNullOrEmpty(apiClientContext.Module) || (!apiClientContext.Module.Equals("EO", StringComparison.CurrentCultureIgnoreCase) && !apiClientContext.Module.Equals("EDI", StringComparison.CurrentCultureIgnoreCase) && !apiClientContext.Module.Equals("SFE", StringComparison.CurrentCultureIgnoreCase) && !apiClientContext.Module.Equals("BOM", StringComparison.CurrentCultureIgnoreCase) && !apiClientContext.Module.Equals("ERP", StringComparison.CurrentCultureIgnoreCase)))
		{
			return apiClientContext.DataDictionary.Users.LoginUsingPassedCredentials(LoginCredentialsObj, apiClientContext.ID.ToString()).User;
		}
		return apiClientContext.DataDictionary.Users.LoginUsingPassedCredentials(LoginCredentialsObj, string.Empty).User;
	}

	/// <summary>
	/// Create an api context with log-in verfication
	/// </summary>
	/// <param name="apiSession">The apiSession as APISessionDto</param>
	/// <param name="module">The module as Enums.WebAPIModules </param>
	/// <returns></returns>
	public Task<APIClientContext> GetApiClientContextAfterLoginVerificationAsync(APISessionDto apiSession, APIEnums.WebAPIModules module)
	{
		string empty = string.Empty;
		string message = string.Empty;
		APIClientContext aPIClientContext = new APIClientContext();
		aPIClientContext.DbContext = new M1.Core.AppContext(designMode: false, loadMetadata: false);
		aPIClientContext.ID = Guid.NewGuid();
		aPIClientContext.APIID = apiSession.APIID;
		aPIClientContext.M1ModuleCode = apiSession.M1ModuleCode;
		aPIClientContext.DataDictionaryID = apiSession.DatadictionaryID.Trim();
		aPIClientContext.DatabaseID = apiSession.DatabaseID;
		aPIClientContext.UserID = apiSession.APIUserID;
		aPIClientContext.Module = Enum.GetName(typeof(APIEnums.WebAPIModules), module);
		aPIClientContext.UserPassword = aPIClientContext.DbContext.DBServerManager.Decrypt((apiSession.APIUserPassword == null) ? string.Empty : apiSession.APIUserPassword);
		aPIClientContext.HashedUserPassword = M1Util.HashString(aPIClientContext.UserPassword).Trim();
		aPIClientContext.IsReadOnly = apiSession.IsReadOnly;
		aPIClientContext.DbContext.DBServerManager.ConnectionInfo.Server = apiSession.Server;
		aPIClientContext.DbContext.DBServerManager.ConnectionInfo.NetworkLibrary = apiSession.NetworkLibrary;
		aPIClientContext.DbContext.DBServerManager.ConnectionInfo.SqlUserID = apiSession.SQLUserID;
		aPIClientContext.DbContext.DBServerManager.sqlPassword = apiSession.SQLUserPassword;
		aPIClientContext.DbContext.DBServerManager.ConnectionInfo.TrustedConnection = apiSession.TrustedConnection;
		aPIClientContext.DbContext.DDServerManager.ConnectionInfo.Server = apiSession.Server;
		aPIClientContext.DbContext.DDServerManager.ConnectionInfo.NetworkLibrary = apiSession.NetworkLibrary;
		aPIClientContext.DbContext.DDServerManager.ConnectionInfo.SqlUserID = apiSession.SQLUserID;
		aPIClientContext.DbContext.DDServerManager.sqlPassword = apiSession.SQLUserPassword;
		aPIClientContext.DbContext.DDServerManager.ConnectionInfo.TrustedConnection = apiSession.TrustedConnection;
		aPIClientContext.DbContext.DBServerManager.Dmo = new Dmo(aPIClientContext.DbContext, aPIClientContext.DbContext.DBServerManager);
		aPIClientContext.DbContext.DDServerManager.Dmo = new Dmo(aPIClientContext.DbContext, aPIClientContext.DbContext.DDServerManager);
		aPIClientContext.LoginAuthenticated = false;
		LoginCredentialsObj = new LoginCredentials(apiSession.APIUserID, string.Empty);
		if (LoginCredentialsObj == null || !LoginCredentialsObj.UserID.Equals(aPIClientContext.UserID))
		{
			LoginCredentialsObj = new LoginCredentials(aPIClientContext.UserID, aPIClientContext.HashedUserPassword);
		}
		else
		{
			LoginCredentialsObj.Password = aPIClientContext.HashedUserPassword;
		}
		aPIClientContext.DataDictionary = aPIClientContext.DbContext.DataDictionaries.LoginUsingPassedCredentials(aPIClientContext.DataDictionaryID).DataDictionary;
		aPIClientContext.User = GetUser(aPIClientContext);
		aPIClientContext.Database = aPIClientContext.User.Databases.LoginUsingPassedCredentials(aPIClientContext.DatabaseID, LoginCredentialsObj, readOnlyLogin: true).Database;
		aPIClientContext.Active = true;
		empty = GetModuleID(aPIClientContext.Module, out message);
		if (string.IsNullOrEmpty(empty))
		{
			aPIClientContext.LoginErrorOutputString = message;
			return Task.FromResult(aPIClientContext);
		}
		LoadProductCodes(aPIClientContext);
		if (!Debugger.IsAttached)
		{
			if (!ProductCodes.IsCustomModulePurchased(empty))
			{
				message = string.Format("{0}: {1}", "FAIL", "Custom Module Product License not found to use this product. Contact your DB/System Administrator to add the appropriate license for the product.");
				aPIClientContext.LoginErrorOutputString = message;
				return Task.FromResult(aPIClientContext);
			}
			if (ProductCodes.HasCustomProductCodeExpired(empty))
			{
				message = string.Format("{0}: {1}", "FAIL", "Custom Module Product License has expired. Contact your DB/System Administrator.");
				aPIClientContext.LoginErrorOutputString = message;
				return Task.FromResult(aPIClientContext);
			}
		}
		message = string.Format("{0}: {1}", "OK", "Logged in");
		aPIClientContext.LoginErrorOutputString = message;
		aPIClientContext.LoginAuthenticated = true;
		return Task.FromResult(aPIClientContext);
	}

	/// <summary>
	/// Does the log out after complete the processing.
	/// </summary>
	/// <param name="apiClientContext">The apiClientContext as APIClientContext</param>
	public Task<bool> DoLogOutAsync(APIClientContext apiClientContext)
	{
		if (apiClientContext.DataDictionary != null && apiClientContext.User != null)
		{
			apiClientContext.DataDictionary.Users.LogoutAndRemove(apiClientContext.User);
		}
		return Task.FromResult(result: true);
	}

	public void InsertWebSession(APIClientContext apiClientContext, APIEnums.WebAPIModules module)
	{
		string name = Enum.GetName(typeof(APIEnums.WebAPIModules), module);
		if (!string.IsNullOrEmpty(name))
		{
			SqlCommand sqlCommand = new SqlCommand("Insert Into WebSessions (weSessionID, weModule, weDataset, weUserID, weDateCreated, weDateLastUsed) Values (@SessionID, @Module, @Dataset, @UserID, getDate(), getDate())");
			sqlCommand.Parameters.Add(new SqlParameter("@SessionID", apiClientContext.WebSessionID));
			sqlCommand.Parameters.Add(new SqlParameter("@Module", name));
			sqlCommand.Parameters.Add(new SqlParameter("@Dataset", apiClientContext.DatabaseID));
			sqlCommand.Parameters.Add(new SqlParameter("@UserID", apiClientContext.UserID));
			apiClientContext.Database.ExecuteCommand(sqlCommand);
		}
	}

	public void Dispose()
	{
	}
}
