using System;
using System.Runtime.InteropServices;
using M1.Script.Interfaces;

namespace M1.Core.Script;

[ComVisible(true)]
public class AppSecurityInfo : ISecurityInfo
{
	private M1DataDictionary _dataDictionary;

	private M1User _user;

	private M1Database _database;

	public string AllModules => _dataDictionary.ProductCode.AllModules;

	public string LastLoadedProductCode => _dataDictionary.ProductCode.LastLoadedProductCode;

	public int SerialNumber => _dataDictionary.ProductCode.SerialNumber;

	public AppSecurityInfo(IServiceProvider provider)
	{
		_database = provider.GetService(typeof(M1Database)) as M1Database;
		_dataDictionary = _database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		_user = _database.GetService(typeof(M1User)) as M1User;
	}

	public bool IsModulePurchased(string module)
	{
		if (module.Length <= 2)
		{
			return _dataDictionary.ProductCode.IsModulePurchased(module, _database);
		}
		return _database.Security.IsInRole(module);
	}

	public bool IsCustomModulePurchased(short customID)
	{
		return _dataDictionary.ProductCode.IsCustomModulePurchased(customID.ToString());
	}

	public bool HasCustomProductCodeExpired(short customModule)
	{
		return _dataDictionary.ProductCode.HasCustomProductCodeExpired(customModule.ToString());
	}

	public object UsersResolvedSecurity(string userID)
	{
		return _user.ResolvedUsersSecurity(userID);
	}

	public object UsersResolvedDatabaseSecurity(string userID)
	{
		return _user.ResolvedDatabaseSecurity(userID);
	}

	public object UsersResolvedTableSecurity(string userID, bool showInReport)
	{
		return _user.ResolvedTableSecurity(userID, showInReport);
	}

	public object UsersResolvedFieldSecurity(string userID, bool showInReport)
	{
		return _user.ResolvedFieldSecurity(userID, showInReport);
	}

	public object UsersResolvedReportSecurity(string userID, bool showInReport)
	{
		return _user.ResolvedReportSecurity(userID, showInReport);
	}

	public object UsersResolvedComponentSecurity(string userID, bool showInReport)
	{
		return _user.ResolvedComponentSecurity(userID, showInReport);
	}
}
