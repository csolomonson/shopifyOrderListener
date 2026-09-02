using System.Runtime.InteropServices;
using M1.Script.Interfaces;

namespace M1.Core.Script;

[ComVisible(true)]
public class AppSecurity : ISecurity
{
	private M1Database databaseRef;

	public bool ViewOnlyUser => databaseRef.Security.GetDatabaseAccessLevel(SecurityAccessLevel.Default) == SecurityAccessLevel.View;

	public AppSecurity(M1Database database)
	{
		databaseRef = database;
	}

	public string GetRowFilterForTable(string table)
	{
		return databaseRef.Security.GetRowFilter(table);
	}

	public bool IsInRole(string roleID)
	{
		return databaseRef.Security.IsInRole(roleID);
	}

	public bool IsInRoleByTable(string table, string type)
	{
		return databaseRef.Security.IsInRoleByTable(table, type);
	}

	public bool IsInRoleByField(string table, string field, string accessType)
	{
		return databaseRef.Security.IsInRoleByField(table, field, accessType);
	}

	public short GetObjectAccessLevel(string objectID)
	{
		return (short)databaseRef.Security.GetObjectAccessLevel(objectID);
	}

	public short GetTableAccessLevel(string table)
	{
		return (short)databaseRef.Security.GetTableAccessLevel(table);
	}

	public short GetGridAccessLevel(string gridID)
	{
		return (short)databaseRef.Security.GetGridAccessLevel(gridID);
	}

	public short GetFormAccessLevel(string formID)
	{
		return (short)databaseRef.Security.GetFormAccessLevel(formID);
	}

	public short GetModuleAccessLevel(string module)
	{
		return (short)databaseRef.Security.GetModuleAccessLevel(module);
	}

	public short GetReportAccessLevel(string folder, string report)
	{
		return (short)databaseRef.Security.GetReportAccessLevel(folder, report);
	}

	public bool IsAccessType(object level, string type)
	{
		return databaseRef.Security.IsAccessType((short)level, type);
	}
}
