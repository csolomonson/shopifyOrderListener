using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using M1.Core.Script;
using M1.Extensions;

namespace M1.Core;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[ComDefaultInterface(typeof(IM1ComFieldsCollection))]
public class FieldCollection : KeyedCollection<string, FieldDefinition>, IM1ComFieldsCollection, IScriptContainsRef
{
	public FieldCollection()
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
	}

	protected override string GetKeyForItem(FieldDefinition item)
	{
		return item.FieldName;
	}

	protected override void ClearItems()
	{
		using (IEnumerator<FieldDefinition> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				enumerator.Current.Dispose();
			}
		}
		base.ClearItems();
	}

	protected override void RemoveItem(int index)
	{
		base[index].Dispose();
		base.RemoveItem(index);
	}

	public List<FieldDefinition> Load(M1User m1User, M1Database m1Database, M1DataDictionary m1DataDictionary, AppContext context, DataTable datasource)
	{
		List<FieldDefinition> list = new List<FieldDefinition>();
		foreach (DataColumn column in datasource.Columns)
		{
			if (column.ColumnName.Length != 0 && !Contains(column.ColumnName))
			{
				FieldDefinition fieldDefinition = new FieldDefinition(context, m1User, m1DataDictionary, m1Database);
				fieldDefinition.FieldName = column.ColumnName;
				fieldDefinition.FieldNameFormatted = column.ColumnName;
				fieldDefinition.Load(column, allowEditing: true);
				Add(fieldDefinition);
				list.Add(fieldDefinition);
			}
		}
		return list;
	}

	public bool Load(M1User m1User, M1Database m1Database, M1DataDictionary m1DataDictionary, AppContext context, string[] databases, string fromClause, M1BindingSource bindingSource, bool allowEditing)
	{
		fromClause = fromClause.Trim();
		if (fromClause.Length != 0)
		{
			SqlDataAdapter adapter;
			DataTable datasource = ((!fromClause.StartsWith("DD", StringComparison.CurrentCultureIgnoreCase)) ? m1Database.GetDataTable(m1Database.PrepareQuery($"select * from {fromClause} Where 0=1"), fillSchema: true, out adapter) : m1DataDictionary.GetDataTable(m1Database.PrepareQuery($"select * from {fromClause} Where 0=1"), fillSchema: true, out adapter));
			return Load(m1User, m1Database, m1DataDictionary, context, databases, datasource, bindingSource, allowEditing);
		}
		return false;
	}

	public bool Load(M1User m1User, M1Database m1Database, M1DataDictionary m1DataDictionary, AppContext context, string[] databases, DataTable datasource, M1BindingSource bindingSource, bool allowEditing)
	{
		bool result = false;
		Clear();
		if (datasource != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataColumn column in datasource.Columns)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(column.ColumnName.ToSql());
			}
			if (stringBuilder.Length != 0)
			{
				result = loadFields(m1User, m1Database, m1DataDictionary, context, databases, datasource, bindingSource, allowEditing, stringBuilder.ToString());
			}
		}
		return result;
	}

	public bool Load(M1User m1User, M1Database m1Database, M1DataDictionary m1DataDictionary, AppContext context, string[] databases, string fieldList)
	{
		Clear();
		return loadFields(m1User, m1Database, m1DataDictionary, context, databases, null, null, allowEditing: true, fieldList);
	}

	public string GetExtensionsQuery(M1User m1User, M1Database m1Database, M1DataDictionary m1DataDictionary, string initialDbFull, string fieldList)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("Select DDFieldExtensions.*,dhClass,dhCaption,dhOpenWithID,DDOpenWiths.* From DDFieldExtensions Inner Join DDFieldExtensionTypes On dqFieldExtensionTypeID = dhFieldExtensionTypeID Left Outer Join DDOpenWiths On dhOpenWithID = dwID Inner Join DDFields On dqTable = dfTable And dqField = dfField Where DDFields.dfField In ({0}) Order By dqSequence", fieldList);
		return stringBuilder.ToString();
	}

	public string GetActionsQuery(M1User m1User, M1Database m1Database, M1DataDictionary m1DataDictionary, string initialDbFull, string fieldList)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("Select DDOpenWiths.* From DDOpenWiths Inner Join DDFields On dwTable = dfTable And dwField = dfField And dwType = 6 Where DDFields.dfField In ({0}) Order By dwSequence,dwDesc", fieldList);
		return stringBuilder.ToString();
	}

	public string GetQuery(M1User m1User, M1Database m1Database, M1DataDictionary m1DataDictionary, string initialDbFull, string fieldList)
	{
		if (m1DataDictionary != null && m1User != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select DDFields.dfUniqueID,DDFields.dfAppExtensionID,DDFields.dfTable,DDFields.dfField,DDFields.dfDisplayName,");
			stringBuilder.Append(m1DataDictionary.Language.GetdfCaptionField(m1Database, "DDFields"));
			stringBuilder.Append(",DDFields.dfdbtype,DDFields.dfLength,DDFields.dfDecimals,DDFields.dfSequence,DDFields.dfSequenceUser,DDFields.dfLower,DDFields.dfAllowNulls,");
			stringBuilder.Append(" IsNull(DDFields.dfRequiredExpression,'') As dfRequiredExpression,IsNull(DDFields.dfRequiredExpressionUser,'') As dfRequiredExpressionUser,DDFields.dfFormat,IsNull(DDFields.dfCalculationExpression,'') As dfCalculationExpression,IsNull(DDFields.dfBoundParentFieldExpression,'') As dfBoundParentFieldExpression,");
			stringBuilder.Append(" DDFields.dfDefaultExpression,DDFields.dfDefaultExpressionUser,DDFieldUserSettings.daDefault,DDFields.dfdprv,DDFields.dfudpr,");
			stringBuilder.Append(" IsNull(DDFields.dfForeignKeyRequiredExpression,'') As dfForeignKeyRequiredExpression,IsNull(DDFields.dfForeignKeyRequiredExpressionUser,'') As dfForeignKeyRequiredExpressionUser,");
			stringBuilder.Append(" IsNull(DDFields.dfReadonlyExpression,'') As dfReadonlyExpression,IsNull(DDFields.dfReadonlyExpressionUser,'') As dfReadonlyExpressionUser,IsNull(DDFields.dfVisibleExpression,'') As dfVisibleExpression,IsNull(DDFields.dfVisibleExpressionUser,'') As dfVisibleExpressionUser,DDFields.dfModule,DDFields.dfCurrencyType,DDFields.dfCurrencyRelatedField,DDFields.dfCurrencyUpdateRelatedField,");
			stringBuilder.Append(" DDFields.dfBoundParentField,IsNull(boundParentFields.dfRelatedFields,'') As parentdfRelatedFields,DDFields.dfBoundParentFieldType,DDFields.dfBoundParentFieldProxy,");
			stringBuilder.Append(" DDFields.dfRelatedTable,DDFields.dfRelatedFields,DDFields.dfRequiredForeignRelation,DDFields.dfffil,");
			stringBuilder.Append(" IsNull(DDFields.dfValueList,'') As dfValueList,DDFields.dfRelatedTableSearchGridId,");
			stringBuilder.Append(" IsNull(DDFields.dfShowAsDropdownUser,DDFields.dfShowAsDropdown) As dfShowAsDropdown,DDFields.dfRelatedTableReturnField,DDFields.dfRelatedTabledescriptionField,DDFields.dfRelatedTableOrderByField,IsNull(DDFields.dfRelatedTableFilter,'') As dfRelatedTableFilter,");
			stringBuilder.Append(" DDFields.dfhide,DDFields.dfGroup,DDFields.dfGroupParameters,DDFields.dfCustom, ");
			stringBuilder.AppendFormat(" IsNull({0},'') As dtCaption,", m1DataDictionary.Language.GetdtCaptionField(m1Database, removeAsClause: true));
			stringBuilder.Append(" IsNull(dtQuickSearchFieldsUser,dtQuickSearchFields) As dtQuickSearchFields,");
			stringBuilder.Append(" IsNull(dtKeyFields,'') As dtKeyFields,IsNull(dtLastKeyCanBeEmpty,0) As dtLastKeyCanBeEmpty,IsNull(dtModule,'') As dtModule,");
			stringBuilder.Append(" IsNull(IsNull(Case When dtMemoDescriptionUser = '' Then Null Else dtMemoDescriptionUser End,dtMemoDescription),'') As dtMemoDescription, IsNull(IsNull(dtShowMemosUser,dtShowMemos),0) As dtShowMemos, IsNull(dtUniqueField,'') As dtUniqueField,");
			stringBuilder.Append(" IsNull(relatedTableCurModeField.dfRelatedFields,'') As CurrencyModeLocationRelatedFields,");
			stringBuilder.Append(" IsNull(dtCurrencyModeLocationField,'') As dtCurrencyModeLocationField,IsNull(dtCurrencyRateIdField,'') As dtCurrencyRateIdField, IsNull(dtCurrencyCustomRateField,'') As dtCurrencyCustomRateField, ");
			stringBuilder.Append(" IsNull(dtCurrencyExchangeRateField,'') As dtCurrencyExchangeRateField, IsNull(dtDocumentDateField,'') As dtDocumentDateField,");
			stringBuilder.Append(getSecurityQueryFields());
			stringBuilder.Append(" , DDFields.* ");
			stringBuilder.AppendFormat(" from DDFields DDFields With(Nolock) {0}", m1DataDictionary.Language.GetdfCaptionJoin(m1Database, "DDFields"));
			stringBuilder.Append(" Left Outer Join DDFields boundParentFields With(NoLock) On DDFields.dfBoundParentField = boundParentFields.dfField ");
			stringBuilder.Append(" Left Outer Join DDTables With(NoLock) On DDFields.dfRelatedTable = dtTable ");
			stringBuilder.Append(" Left Outer Join DDFields relatedTableCurModeField With(NoLock) On dtCurrencyModeLocationField = relatedTableCurModeField.dfField ");
			stringBuilder.AppendFormat(" Left Outer Join DDFieldUserSettings On DDFields.dfTable=DDFieldUserSettings.daTable And DDFields.dfField=DDFieldUserSettings.daField And DDFieldUserSettings.daUser = {0} ", M1Util.ConvertToSql(m1User.ID));
			stringBuilder.Append(m1DataDictionary.Language.GetdtCaptionJoin(m1Database));
			stringBuilder.Append(getSecurityQueryJoins(m1User.ID, initialDbFull, fieldList));
			stringBuilder.AppendFormat(" Where DDFields.dfField In ({0})", fieldList);
			return stringBuilder.ToString();
		}
		return string.Empty;
	}

	public void UpdateCollectionFromString(string fieldListWithProps)
	{
		if (fieldListWithProps.Length == 0)
		{
			return;
		}
		Hashtable hashtable = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
		int num = 0;
		string empty = string.Empty;
		FieldDefinition fieldDefinition = null;
		string[] array = fieldListWithProps.Split(',');
		foreach (string text in array)
		{
			empty = text;
			num = empty.IndexOf(":");
			if (num > 0)
			{
				empty = empty.Substring(0, num);
			}
			empty = empty.Trim();
			fieldDefinition = ((!Contains(empty)) ? null : base[empty]);
			if (fieldDefinition != null)
			{
				fieldDefinition.LoadFieldProperties(text);
				if (!hashtable.ContainsKey(empty))
				{
					hashtable.Add(empty, string.Empty);
				}
			}
		}
	}

	private bool loadFields(M1User m1User, M1Database m1Database, M1DataDictionary m1DataDictionary, AppContext context, string[] databases, DataTable datasource, M1BindingSource bindingSource, bool allowEditing, string fieldList)
	{
		string text = string.Empty;
		if (m1Database != null)
		{
			text = m1Database.ID;
		}
		if (databases.Length != 0 && databases[0].Length != 0)
		{
			text = databases[0];
		}
		DataTable dataTable = null;
		DataTable dataTable2 = null;
		DataTable dataTable3 = null;
		if (m1DataDictionary != null && m1User != null)
		{
			dataTable = m1DataDictionary.GetDataTable(GetQuery(m1User, m1Database, m1DataDictionary, text, fieldList));
			dataTable2 = m1DataDictionary.GetDataTable(GetExtensionsQuery(m1User, m1Database, m1DataDictionary, text, fieldList));
			dataTable3 = m1DataDictionary.GetDataTable(GetActionsQuery(m1User, m1Database, m1DataDictionary, text, fieldList));
		}
		else
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select dfUniqueID,dfAppExtensionID,dfTable,dfField,dfDisplayName,dfCaption,IsNull(dfCaptionExpression,'') As dfCaptionExpression,IsNull(dfCaptionExpressionUser,'') As dfCaptionExpressionUser,dfSaveAsExpression,dfSaveAsExpressionUser,dfdbtype,dfLength,dfDecimals,dfSequence,dfSequenceUser,");
			stringBuilder.Append(" dfLower,dfAllowNulls,IsNull(dfRequiredExpression,'') As dfRequiredExpression,IsNull(DDFields.dfRequiredExpressionUser,'') As dfRequiredExpressionUser,dfFormat,dfFormat,IsNull(dfCalculationExpression,'') As dfCalculationExpression,IsNull(dfBoundParentFieldExpression,'') As dfBoundParentFieldExpression,dfDefaultExpression,dfDefaultExpressionUser,'' as daDefault,dfdprv,dfudpr, ");
			stringBuilder.Append(" IsNull(dfForeignKeyRequiredExpression,'') As dfForeignKeyRequiredExpression,IsNull(dfForeignKeyRequiredExpressionUser,'') As dfForeignKeyRequiredExpressionUser,IsNull(dfReadonlyExpression,'') As dfReadonlyExpression,IsNull(dfReadonlyExpressionUser,'') As dfReadonlyExpressionUser,IsNull(dfVisibleExpression,'') As dfVisibleExpression,IsNull(dfVisibleExpressionUser,'') As dfVisibleExpressionUser,dfModule,dfCurrencyType,dfCurrencyRelatedField,dfCurrencyUpdateRelatedField,");
			stringBuilder.Append(" dfBoundParentField,dfBoundParentFieldType,dfBoundParentFieldProxy,'' As parentdfRelatedFields,dfRelatedTable,dfRelatedFields,dfRequiredForeignRelation,dfffil,IsNull(dfValueList,'') As dfValueList,dfShowAsDropdown,dfRelatedTableSearchGridId,dfRelatedTableReturnField,dfRelatedTabledescriptionField,dfRelatedTableOrderByField,IsNull(dfRelatedTableFilter,'') As dfRelatedTableFilter,dfhide,dfGroup,dfGroupParameters,dfCustom,");
			stringBuilder.Append(" IsNull(dtCaption,'') As dtCaption,IsNull(dtKeyFields,'') As dtKeyFields,IsNull(dtLastKeyCanBeEmpty,0) As dtLastKeyCanBeEmpty,IsNull(dtModule,'') As dtModule,'' As CurrencyModeLocationRelatedFields,");
			stringBuilder.Append(" IsNull(dtQuickSearchFieldsUser,dtQuickSearchFields) As dtQuickSearchFields,");
			stringBuilder.Append(" IsNull(IsNull(Case When dtMemoDescriptionUser = '' Then Null Else dtMemoDescriptionUser End,dtMemoDescription),'') As dtMemoDescription, IsNull(IsNull(dtShowMemosUser,dtShowMemos),0) As dtShowMemos, IsNull(dtUniqueField,'') As dtUniqueField,");
			stringBuilder.Append(" IsNull(dtCurrencyModeLocationField,'') As dtCurrencyModeLocationField,IsNull(dtCurrencyRateIdField,'') As dtCurrencyRateIdField, IsNull(dtCurrencyCustomRateField,'') As dtCurrencyCustomRateField, ");
			stringBuilder.Append(" IsNull(dtCurrencyExchangeRateField,'') As dtCurrencyExchangeRateField, IsNull(dtDocumentDateField,'') As dtDocumentDateField,");
			stringBuilder.Append(getSecurityQueryFields());
			stringBuilder.Append(" from DDFields With(Nolock) Left Outer Join DDTables With(NoLock) On dfRelatedTable = dtTable ");
			stringBuilder.Append(getSecurityQueryJoins("", "", fieldList));
			stringBuilder.AppendFormat(" Where dfField In ({0})", fieldList);
			dataTable = DesignMode.DesignModeGetDataTable(stringBuilder.ToString());
			dataTable2 = DesignMode.DesignModeGetDataTable(GetExtensionsQuery(m1User, m1Database, m1DataDictionary, text, fieldList));
			dataTable3 = DesignMode.DesignModeGetDataTable(GetActionsQuery(m1User, m1Database, m1DataDictionary, text, fieldList));
		}
		FieldDefinition fieldDefinition = null;
		DataRow[] array = null;
		bool flag = false;
		if (datasource == null)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				fieldDefinition = new FieldDefinition(context, m1User, m1DataDictionary, m1Database);
				fieldDefinition.FieldName = row.Field<string>("dfField");
				fieldDefinition.FieldNameFormatted = row.Field<string>("dfField");
				fieldDefinition.Load(row, dataTable2.Select("dqField = " + M1Util.ConvertToLinq(fieldDefinition.FieldName)), dataTable3.Select("dwField = " + M1Util.ConvertToLinq(fieldDefinition.FieldName)), allowEditing);
				fieldDefinition.LoadDatabase(text, row, m1User);
				fieldDefinition.BindingSource = bindingSource;
				Add(fieldDefinition);
			}
		}
		else
		{
			Dictionary<string, string> dictionary = ((bindingSource == null) ? new Dictionary<string, string>() : bindingSource.Query.GetFieldListProps(includeFieldsWithNoProps: false));
			foreach (DataColumn column in datasource.Columns)
			{
				if (column.ColumnName.Length == 0)
				{
					continue;
				}
				fieldDefinition = new FieldDefinition(context, m1User, m1DataDictionary, m1Database);
				fieldDefinition.FieldName = column.ColumnName;
				fieldDefinition.FieldNameFormatted = column.ColumnName;
				flag = false;
				array = null;
				if (dataTable != null)
				{
					array = dataTable.Select("dfField = " + fieldDefinition.FieldName.ToLinq());
					if (array.Length != 0)
					{
						fieldDefinition.Load(array[0], dataTable2.Select("dqField = " + M1Util.ConvertToLinq(fieldDefinition.FieldName)), dataTable3.Select("dwField = " + M1Util.ConvertToLinq(fieldDefinition.FieldName)), allowEditing);
					}
				}
				if (array == null || array.Length == 0)
				{
					fieldDefinition.Load(column, allowEditing);
					if (dictionary != null && dictionary.ContainsKey(column.ColumnName))
					{
						fieldDefinition.LoadFieldProperties(dictionary[column.ColumnName]);
						if (dataTable != null && fieldDefinition.TableName.Length == 0 && fieldDefinition.CalculationExpressionType == FieldDefinition.CalculationExpressionTypeEnum.RunningTotal && fieldDefinition.CalculationExpressionReferencedFields != null && fieldDefinition.CalculationExpressionReferencedFields.Count != 0)
						{
							array = dataTable.Select("dfField = " + fieldDefinition.CalculationExpressionReferencedFields[0].ToLinq());
							if (array.Length != 0)
							{
								fieldDefinition.TableName = array[0].Field<string>("dfTable");
								fieldDefinition.FieldType = FieldDefinition.charToFieldType(array[0].Field<string>("dfDBType"));
								fieldDefinition.FieldLength = array[0].Field<byte>("dfLength");
								fieldDefinition.FieldDecimals = array[0].Field<byte>("dfDecimals");
								fieldDefinition.AllowLowerCaseOrNegative = array[0].Field<bool>("dfLower");
								fieldDefinition.Module = array[0].Field<string>("dfModule");
							}
						}
					}
				}
				if (array == null || array.Length == 0)
				{
					fieldDefinition.LoadDatabase(text, null, m1User);
				}
				else
				{
					fieldDefinition.LoadDatabase(text, array[0], m1User);
				}
				fieldDefinition.BindingSource = bindingSource;
				Add(fieldDefinition);
			}
		}
		DataTable dataTable4 = null;
		for (int i = 1; i < databases.Length; i++)
		{
			dataTable4 = ((m1DataDictionary == null || m1User == null || m1Database == null) ? DesignMode.DesignModeGetDataTable(string.Format("Select dfField, {0} From DDFields With(Nolock) {1} Where dfField In ({2})", getSecurityQueryFields(), getSecurityQueryJoins("", "", fieldList), fieldList)) : m1DataDictionary.GetDataTable($"Select dfField, {getSecurityQueryFields()} From DDFields With(Nolock) {getSecurityQueryJoins(m1User.ID, databases[i], fieldList)} Where dfField In ({fieldList})"));
			using IEnumerator<FieldDefinition> enumerator2 = GetEnumerator();
			while (enumerator2.MoveNext())
			{
				FieldDefinition current = enumerator2.Current;
				flag = false;
				if (dataTable4 != null)
				{
					array = dataTable4.Select($"dfField = {current.FieldName.ToLinq()}");
					if (array.Length != 0)
					{
						current.LoadDatabase(databases[i], array[0], m1User);
						flag = true;
					}
					else if (fieldDefinition.CalculationExpressionType == FieldDefinition.CalculationExpressionTypeEnum.RunningTotal && fieldDefinition.CalculationExpressionReferencedFields != null && fieldDefinition.CalculationExpressionReferencedFields.Count != 0)
					{
						array = dataTable4.Select($"dfField = {fieldDefinition.CalculationExpressionReferencedFields[0].ToLinq()}");
						if (array.Length != 0)
						{
							current.LoadDatabase(databases[i], array[0], m1User);
							flag = true;
						}
					}
				}
				if (!flag)
				{
					current.LoadDatabase(databases[i], null, m1User);
				}
			}
		}
		return true;
	}

	private string getSecurityQueryFields()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(" Case When DDsecurityTablesField.dtLevel   Is Null Then Convert(tinyint,0) Else DDsecurityTablesField.dtLevel   End AS dtLevelField, ");
		stringBuilder.Append(" Case When DDsecurityTablesTable.dtLevel   Is Null Then Convert(tinyint,0) Else DDsecurityTablesTable.dtLevel   End AS dtLevelTable, ");
		stringBuilder.Append(" Case When DDsecurityTablesDataset.dtLevel Is Null Then Convert(tinyint,0) Else DDsecurityTablesDataset.dtLevel End AS dtLevelDataset, ");
		stringBuilder.Append(" Case When DDsecurityTablesFieldGroup.dtLevel   Is Null Then Convert(tinyint,0) Else DDsecurityTablesFieldGroup.dtLevel   End AS dtLevelFieldGroup, ");
		stringBuilder.Append(" Case When DDsecurityTablesTableGroup.dtLevel   Is Null Then Convert(tinyint,0) Else DDsecurityTablesTableGroup.dtLevel   End AS dtLevelTableGroup, ");
		stringBuilder.Append(" Case When DDsecurityTablesDatasetGroup.dtLevel Is Null Then Convert(tinyint,0) Else DDsecurityTablesDatasetGroup.dtLevel End AS dtLevelDatasetGroup ");
		return stringBuilder.ToString();
	}

	private string getSecurityQueryJoins(string userID, string databaseName, string fieldList)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat(" LEFT OUTER JOIN DDSecurityTables DDSecurityTablesField   With(Nolock) ON DDSecurityTablesField.dtTable = DDFields.dfTable And DDSecurityTablesField.dtField   = DDFields.dfField AND DDSecurityTablesField.dtUserID   = {0}  AND DDSecurityTablesField.dtDataSet =  {1} ", userID.ToSql(), databaseName.ToSql());
		stringBuilder.AppendFormat(" LEFT OUTER JOIN DDSecurityTables DDSecurityTablesTable   With(Nolock) ON DDSecurityTablesTable.dtTable = DDFields.dfTable And DDSecurityTablesTable.dtField   = '' AND DDSecurityTablesTable.dtUserID   = {0}  AND DDSecurityTablesTable.dtDataSet =  {1} ", userID.ToSql(), databaseName.ToSql());
		stringBuilder.AppendFormat(" LEFT OUTER JOIN DDSecurityTables DDSecurityTablesDataset With(Nolock) ON DDSecurityTablesDataset.dtTable = '' And DDSecurityTablesDataset.dtField = '' AND DDSecurityTablesDataset.dtUserID = {0} AND DDSecurityTablesDataset.dtDataSet = {1}", userID.ToSql(), databaseName.ToSql());
		stringBuilder.AppendFormat(" LEFT OUTER JOIN (Select dtTable,dtField,Max(Case When dtLevel Is Null Then Convert(tinyint,0) Else dtLevel End) as dtLevel From DDSecurityTables sub1 With(Nolock) Inner Join DDFields With(Nolock) On dfField = dtField Where sub1.dtDataSet = {0} AND sub1.dtTable = DDFields.dfTable And sub1.dtField = DDFields.dfField AND sub1.dtUserID In (Select dzGroupID From DDSecurityGroups Where dzDataset = {0} And dzUserID = {1} ) And dfField In ({2}) Group By dtTable,dtField) As DDSecurityTablesFieldGroup   ON DDFields.dfTable = DDSecurityTablesFieldGroup.dtTable   And DDFields.dfField = DDSecurityTablesFieldGroup.dtField ", databaseName.ToSql(), userID.ToSql(), fieldList);
		stringBuilder.AppendFormat(" LEFT OUTER JOIN (Select dtTable, Max(Case When dtLevel Is Null Then Convert(tinyint,0) Else dtLevel End) as dtLevel From DDSecurityTables sub2 With(Nolock) Inner Join DDFields With(Nolock) On dfTable = dtTable Where sub2.dtDataSet = {0} AND sub2.dtTable = DDFields.dfTable And sub2.dtField = '' AND sub2.dtUserID In (Select dzGroupID From DDSecurityGroups Where dzDataset = {0} And dzUserID = {1} ) And dfField In ({2}) Group By dtTable) As DDSecurityTablesTableGroup   ON DDFields.dfTable = DDSecurityTablesTableGroup.dtTable  ", databaseName.ToSql(), userID.ToSql(), fieldList);
		stringBuilder.AppendFormat(" LEFT OUTER JOIN (Select dtDataset, Max(Case When dtLevel Is Null Then Convert(tinyint,0) Else dtLevel End) as dtLevel From DDSecurityTables sub3 With(Nolock) Where sub3.dtDataSet = {0} AND sub3.dtTable = '' And sub3.dtField = '' AND sub3.dtUserID In (Select dzGroupID From DDSecurityGroups Where dzDataset = {0} And dzUserID = {1} ) Group By dtDataset ) As DDSecurityTablesDatasetGroup ON 0=0", databaseName.ToSql(), userID.ToSql());
		return stringBuilder.ToString();
	}

	public object ContainsRef(string id)
	{
		if (Contains(id))
		{
			return base[id];
		}
		return null;
	}

	bool IM1ComFieldsCollection.Remove(string value)
	{
		return Remove(value);
	}

	FieldDefinition IM1ComFieldsCollection.get__Default(string name)
	{
		return base[name];
	}
}
