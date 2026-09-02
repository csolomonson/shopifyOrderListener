using System.Data;
using System.Text;

namespace M1.Core;

public class M1DatabaseFieldSecurity
{
	public string Database = string.Empty;

	public SecurityAccessLevel ResolvedAccessLevel;

	public SecurityAccessLevel AccessLevelField;

	public SecurityAccessLevel AccessLevelTable;

	public SecurityAccessLevel AccessLevelDataset;

	public SecurityAccessLevel AccessLevelFieldFromGroups;

	public SecurityAccessLevel AccessLevelTableFromGroups;

	public SecurityAccessLevel AccessLevelDatasetFromGroups;

	public void SetAccessLevels(DataRow row, M1User m1User)
	{
		if (row == null)
		{
			ResolvedAccessLevel = (SecurityAccessLevel)28;
			return;
		}
		if (row.Field<byte>("dtLevelField") == 0)
		{
			AccessLevelField = SecurityAccessLevel.Default;
		}
		else
		{
			AccessLevelField = row.Field<SecurityAccessLevel>("dtLevelField");
		}
		if (row.Field<byte>("dtLevelFieldGroup") == 0)
		{
			AccessLevelFieldFromGroups = SecurityAccessLevel.Default;
		}
		else
		{
			AccessLevelFieldFromGroups = row.Field<SecurityAccessLevel>("dtLevelFieldGroup");
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
		if (AccessLevelField != SecurityAccessLevel.Default)
		{
			ResolvedAccessLevel = AccessLevelField;
		}
		else if (AccessLevelFieldFromGroups != SecurityAccessLevel.Default)
		{
			ResolvedAccessLevel = AccessLevelFieldFromGroups;
		}
		else if (AccessLevelTable != SecurityAccessLevel.Default)
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

	public string GetReadOnlyReasons(FieldDefinition fieldDef)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (AccessLevelField == SecurityAccessLevel.None)
		{
			stringBuilder.AppendLine("SEC: Field " + fieldDef.FieldNameFormatted + " access level is no access.");
		}
		if (AccessLevelField == SecurityAccessLevel.View)
		{
			stringBuilder.AppendLine("SEC: Field " + fieldDef.FieldNameFormatted + " access level is read only.");
		}
		if (AccessLevelFieldFromGroups == SecurityAccessLevel.None)
		{
			stringBuilder.AppendLine("SEC: Field " + fieldDef.FieldNameFormatted + " access level from a group is no access.");
		}
		if (AccessLevelFieldFromGroups == SecurityAccessLevel.View)
		{
			stringBuilder.AppendLine("SEC: Field " + fieldDef.FieldNameFormatted + " access level from a group is read only.");
		}
		if (AccessLevelTable == SecurityAccessLevel.None)
		{
			stringBuilder.AppendLine("SEC: Field " + fieldDef.FieldNameFormatted + " inherited table access level of no access.");
		}
		if (AccessLevelTable == SecurityAccessLevel.View)
		{
			stringBuilder.AppendLine("SEC: Field " + fieldDef.FieldNameFormatted + " inherited table access level of read only.");
		}
		if (AccessLevelTableFromGroups == SecurityAccessLevel.None)
		{
			stringBuilder.AppendLine("SEC: Field " + fieldDef.FieldNameFormatted + " inherited table access level from a group of no access.");
		}
		if (AccessLevelTableFromGroups == SecurityAccessLevel.View)
		{
			stringBuilder.AppendLine("SEC: Field " + fieldDef.FieldNameFormatted + " inherited table access level from a group of read only.");
		}
		if (AccessLevelDataset == SecurityAccessLevel.None)
		{
			stringBuilder.AppendLine("SEC: Field " + fieldDef.FieldNameFormatted + " inherited database access level of no access.");
		}
		if (AccessLevelDataset == SecurityAccessLevel.View)
		{
			stringBuilder.AppendLine("SEC: Field " + fieldDef.FieldNameFormatted + " inherited database access level of read only.");
		}
		if (AccessLevelDatasetFromGroups == SecurityAccessLevel.None)
		{
			stringBuilder.AppendLine("SEC: Field " + fieldDef.FieldNameFormatted + " inherited database access level from a group of no access.");
		}
		if (AccessLevelDatasetFromGroups == SecurityAccessLevel.View)
		{
			stringBuilder.AppendLine("SEC: Field " + fieldDef.FieldNameFormatted + " inherited database access level from a group of read only.");
		}
		if (stringBuilder.Length == 0 && ResolvedAccessLevel == SecurityAccessLevel.None)
		{
			stringBuilder.AppendLine("SEC: Field " + fieldDef.FieldNameFormatted + " did not inherit any security access settings, so it was set to no access.");
		}
		return stringBuilder.ToString();
	}
}
