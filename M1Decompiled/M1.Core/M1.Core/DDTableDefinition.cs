using System;
using System.Collections.Generic;
using System.Text;

namespace M1.Core;

public class DDTableDefinition
{
	public string TableName = string.Empty;

	public List<DDFieldDefinition> Fields;

	public List<DDIndexDefinition> Indexes;

	public string ExtraQuery = string.Empty;

	public bool ExportForSetup = true;

	public string[] PackageKeyFields;

	public string[] PackageDisplayFields;

	public string[] PackageDisplayTypes;

	public string PackageFilter = string.Empty;

	public string[] DesignerKeyFields;

	public DDTableDefinition(string tableName, bool exportForSetup, DDFieldDefinition[] fields, DDIndexDefinition[] indexes, string[] packageKeyFields, string[] packageDisplayFields, string[] packageDisplayTypes, string[] designerKeyFields)
	{
		TableName = tableName;
		ExportForSetup = exportForSetup;
		Fields = new List<DDFieldDefinition>(fields);
		Indexes = new List<DDIndexDefinition>(indexes);
		PackageKeyFields = packageKeyFields;
		PackageDisplayFields = packageDisplayFields;
		PackageDisplayTypes = packageDisplayTypes;
		DesignerKeyFields = designerKeyFields;
	}

	public DDTableDefinition(string tableName, bool exportForSetup, DDFieldDefinition[] fields, DDIndexDefinition[] indexes, string[] packageKeyFields, string[] packageDisplayFields, string[] packageDisplayTypes, string[] designerKeyFields, string extraQuery)
	{
		TableName = tableName;
		ExportForSetup = exportForSetup;
		Fields = new List<DDFieldDefinition>(fields);
		Indexes = new List<DDIndexDefinition>(indexes);
		PackageKeyFields = packageKeyFields;
		PackageDisplayFields = packageDisplayFields;
		PackageDisplayTypes = packageDisplayTypes;
		DesignerKeyFields = designerKeyFields;
		ExtraQuery = extraQuery;
	}

	public string GetCustomFilterField()
	{
		foreach (DDFieldDefinition field in Fields)
		{
			if (field.Flag == DDFieldFlag.CustomFilterField)
			{
				return field.FieldName;
			}
		}
		return string.Empty;
	}

	public string GetCreateTableCommand()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DDFieldDefinition field in Fields)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(',');
			}
			stringBuilder.Append(field.FieldName + " " + field.FieldType + (field.Nullable ? " Null " : " Not Null ") + ((field.DefaultValue.Length != 0) ? ("Default(" + field.DefaultValue + ")") : string.Empty));
		}
		return "Create Table dbo." + TableName + " (" + stringBuilder.ToString() + ")";
	}

	public string GetCreateIndexesCommand()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DDIndexDefinition index in Indexes)
		{
			stringBuilder.Append("Create " + (index.Unique ? "Unique Index " : "Index ") + index.IndexName + " On " + TableName + " (" + index.Fields + ")\r");
		}
		return stringBuilder.ToString();
	}

	public DDCustomTableInfo GetUpdateInfo(string serverCollation)
	{
		DDCustomTableInfo dDCustomTableInfo = new DDCustomTableInfo();
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		StringBuilder stringBuilder3 = new StringBuilder();
		StringBuilder stringBuilder4 = new StringBuilder();
		StringBuilder stringBuilder5 = new StringBuilder();
		StringBuilder stringBuilder6 = new StringBuilder();
		string text = string.Empty;
		bool flag = false;
		dDCustomTableInfo.TempTable = TableName + "Ex";
		dDCustomTableInfo.Table = TableName;
		foreach (DDFieldDefinition field in Fields)
		{
			if (field.ContentType == DDFieldContentType.AppExtensionID)
			{
				dDCustomTableInfo.AppExtensionField = field.FieldName;
			}
			if (field.Flag == DDFieldFlag.Standard || field.Flag == DDFieldFlag.Key || field.Flag == DDFieldFlag.CustomFilterField)
			{
				if (stringBuilder6.Length != 0)
				{
					stringBuilder6.Append(',');
				}
				stringBuilder6.Append(field.FieldName);
			}
			if (field.Flag == DDFieldFlag.Custom || field.Flag == DDFieldFlag.Key || field.Flag == DDFieldFlag.CustomFilterField)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(field.FieldName);
				if (field.RelatedFieldForCustom.Length != 0)
				{
					stringBuilder.Append("," + field.RelatedFieldForCustom);
				}
			}
			if (field.Flag == DDFieldFlag.Custom)
			{
				flag = true;
				if (stringBuilder2.Length != 0)
				{
					stringBuilder2.Append(" Or ");
				}
				if (field.Nullable)
				{
					stringBuilder2.Append("Not " + field.FieldName + " Is Null");
				}
				else
				{
					stringBuilder2.Append(field.FieldName + " <> " + field.DefaultValue);
				}
				if (stringBuilder3.Length != 0)
				{
					stringBuilder3.Append(',');
				}
				if (field.Nullable && (field.FieldType.StartsWith("text", StringComparison.CurrentCultureIgnoreCase) || field.FieldType.StartsWith("ntext", StringComparison.CurrentCultureIgnoreCase) || field.FieldType.Equals("varchar(max)", StringComparison.CurrentCultureIgnoreCase) || field.FieldType.StartsWith("nvarchar(max)", StringComparison.CurrentCultureIgnoreCase)))
				{
					stringBuilder3.Append(TableName + "." + field.FieldName + " = Case When LTrim(IsNull(Convert(nvarchar(50)," + dDCustomTableInfo.TempTable + "." + field.FieldName + "),'')) = '' Then Null Else " + dDCustomTableInfo.TempTable + "." + field.FieldName + " End");
				}
				else if (field.FieldType.StartsWith("varchar", StringComparison.CurrentCultureIgnoreCase) || field.FieldType.StartsWith("nvarchar", StringComparison.CurrentCultureIgnoreCase))
				{
					stringBuilder3.Append(TableName + "." + field.FieldName + " = RTrim(" + dDCustomTableInfo.TempTable + "." + field.FieldName + ")");
				}
				else
				{
					stringBuilder3.Append(TableName + "." + field.FieldName + " = " + dDCustomTableInfo.TempTable + "." + field.FieldName);
				}
			}
			else if (field.Flag == DDFieldFlag.CustomFilterField)
			{
				text = field.FieldName;
			}
			if (field.Flag == DDFieldFlag.Key)
			{
				if (stringBuilder4.Length != 0)
				{
					stringBuilder4.Append(" And ");
				}
				stringBuilder4.Append(TableName + "." + field.FieldName + " = " + dDCustomTableInfo.TempTable + "." + field.FieldName);
				if (!string.IsNullOrWhiteSpace(serverCollation) && field.FieldType.StartsWith("nvarchar", StringComparison.CurrentCultureIgnoreCase))
				{
					stringBuilder4.Append($" COLLATE {serverCollation}");
				}
				if (stringBuilder5.Length != 0)
				{
					stringBuilder5.Append(',');
				}
				stringBuilder5.Append(field.FieldName);
			}
		}
		dDCustomTableInfo.StandardFieldsSelectStatement = "Select " + stringBuilder6.ToString() + " From " + TableName + ((text.Length != 0) ? (" Where " + text + " = 0") : string.Empty);
		if (dDCustomTableInfo.AppExtensionField.Length != 0)
		{
			dDCustomTableInfo.StandardFieldsSelectStatementWithAppExtension = dDCustomTableInfo.StandardFieldsSelectStatement + ((text.Length != 0) ? " And " : " Where ") + dDCustomTableInfo.AppExtensionField + " = @AppExtensionID";
			if (stringBuilder5.Length != 0)
			{
				dDCustomTableInfo.StandardFieldsSelectStatementWithAppExtension = dDCustomTableInfo.StandardFieldsSelectStatementWithAppExtension + " Order By " + stringBuilder5.ToString();
			}
		}
		if (stringBuilder5.Length != 0)
		{
			dDCustomTableInfo.StandardFieldsSelectStatement = dDCustomTableInfo.StandardFieldsSelectStatement + " Order By " + stringBuilder5.ToString();
		}
		if (flag)
		{
			if (TableName.Equals("DDFormDetails", StringComparison.CurrentCultureIgnoreCase))
			{
				stringBuilder.Clear();
				stringBuilder.Append("*");
			}
			dDCustomTableInfo.CustomFieldsSelectStatement = "Select " + stringBuilder.ToString() + " Into " + dDCustomTableInfo.TempTable + " From " + TableName + " Where " + text + " = 0 And (" + stringBuilder2.ToString() + ")";
			dDCustomTableInfo.ReloadStatements.Add("Update " + TableName + " Set " + stringBuilder3.ToString() + " From " + TableName + " Inner Join " + dDCustomTableInfo.TempTable + " On " + stringBuilder4.ToString());
		}
		if (text.Length != 0)
		{
			dDCustomTableInfo.LoadTableExpression = text + " <> 0";
		}
		else
		{
			dDCustomTableInfo.LoadTableExpression = "0=0";
		}
		if (ExtraQuery.Length != 0)
		{
			dDCustomTableInfo.ReloadStatements.Add(ExtraQuery);
		}
		return dDCustomTableInfo;
	}
}
