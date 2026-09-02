using System;
using System.Data;
using System.Text;

namespace M1.Core;

public class M1DatabaseTableSecurity : IDisposable
{
	public string Database = string.Empty;

	public SecurityAccessLevel ResolvedAccessLevel;

	public SecurityAccessLevel AccessLevelTable;

	public SecurityAccessLevel AccessLevelDataset;

	public SecurityAccessLevel AccessLevelTableFromGroups;

	public SecurityAccessLevel AccessLevelDatasetFromGroups;

	public TableSecurityExpressions SecurityExpressions;

	public M1DatabaseTableSecurity(string databaseName, DataRow row, TableSecurityExpressions securityExpressions)
	{
		Database = databaseName;
		SecurityExpressions = securityExpressions;
		SetAccessLevels(row);
	}

	private void SetAccessLevels(DataRow row)
	{
		if (row == null)
		{
			ResolvedAccessLevel = (SecurityAccessLevel)28;
			return;
		}
		if (row.Field<byte>("dtLevelTable") == 0)
		{
			AccessLevelTable = SecurityAccessLevel.Default;
		}
		else
		{
			AccessLevelTable = row.Field<SecurityAccessLevel>("dtLevelTable");
		}
		if (row.Field<byte>("dtLevelTableGroup") == 0)
		{
			AccessLevelTableFromGroups = SecurityAccessLevel.Default;
		}
		else
		{
			AccessLevelTableFromGroups = row.Field<SecurityAccessLevel>("dtLevelTableGroup");
		}
		if (row.Field<byte>("dtLevelDataset") == 0)
		{
			AccessLevelDataset = SecurityAccessLevel.Default;
		}
		else
		{
			AccessLevelDataset = row.Field<SecurityAccessLevel>("dtLevelDataset");
		}
		if (row.Field<byte>("dtLevelDatasetGroup") == 0)
		{
			AccessLevelDatasetFromGroups = SecurityAccessLevel.Default;
		}
		else
		{
			AccessLevelDatasetFromGroups = row.Field<SecurityAccessLevel>("dtLevelDatasetGroup");
		}
		if (AccessLevelTable != SecurityAccessLevel.Default)
		{
			ResolvedAccessLevel = AccessLevelTable;
		}
		else if (AccessLevelTableFromGroups != SecurityAccessLevel.Default)
		{
			ResolvedAccessLevel = AccessLevelTableFromGroups;
		}
		else if (AccessLevelDataset != SecurityAccessLevel.Default)
		{
			ResolvedAccessLevel = AccessLevelDataset;
		}
		else if (AccessLevelDatasetFromGroups != SecurityAccessLevel.Default)
		{
			ResolvedAccessLevel = AccessLevelDatasetFromGroups;
		}
		else
		{
			ResolvedAccessLevel = SecurityAccessLevel.None;
		}
	}

	public string GetReadOnlyReasons(TableDefinition tableDef)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (AccessLevelTable == SecurityAccessLevel.None)
		{
			stringBuilder.AppendLine("SEC: Table " + tableDef.TableNameFormatted + " access level is no access.");
		}
		if (AccessLevelTable == SecurityAccessLevel.View)
		{
			stringBuilder.AppendLine("SEC: Table " + tableDef.TableNameFormatted + " access level is read only.");
		}
		if (AccessLevelTableFromGroups == SecurityAccessLevel.None)
		{
			stringBuilder.AppendLine("SEC: Table " + tableDef.TableNameFormatted + " access level from a group is no access.");
		}
		if (AccessLevelTableFromGroups == SecurityAccessLevel.View)
		{
			stringBuilder.AppendLine("SEC: Table " + tableDef.TableNameFormatted + " access level from a group is read only.");
		}
		if (AccessLevelDataset == SecurityAccessLevel.None)
		{
			stringBuilder.AppendLine("SEC: Table " + tableDef.TableNameFormatted + " inherited database access level of no access.");
		}
		if (AccessLevelDataset == SecurityAccessLevel.View)
		{
			stringBuilder.AppendLine("SEC: Table " + tableDef.TableNameFormatted + " inherited database access level of read only.");
		}
		if (AccessLevelDatasetFromGroups == SecurityAccessLevel.None)
		{
			stringBuilder.AppendLine("SEC: Table " + tableDef.TableNameFormatted + " inherited database access level from a group of no access.");
		}
		if (AccessLevelDatasetFromGroups == SecurityAccessLevel.View)
		{
			stringBuilder.AppendLine("SEC: Table " + tableDef.TableNameFormatted + " inherited database access level from a group of read only.");
		}
		if (stringBuilder.Length == 0 && ResolvedAccessLevel == SecurityAccessLevel.None)
		{
			stringBuilder.AppendLine("SEC: Table " + tableDef.TableNameFormatted + " did not inherit any security access settings, so it was set to no access.");
		}
		return stringBuilder.ToString();
	}

	public void Dispose()
	{
		SecurityExpressions = null;
	}
}
