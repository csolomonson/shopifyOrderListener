using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using M1.Extensions;

namespace M1.Core;

public class TableCollection : KeyedCollection<string, TableDefinition>
{
	public TableCollection()
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
	}

	protected override string GetKeyForItem(TableDefinition item)
	{
		return item.TableName;
	}

	protected override void ClearItems()
	{
		using (IEnumerator<TableDefinition> enumerator = GetEnumerator())
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

	public bool Load(M1User m1User, M1Database m1Database, M1DataDictionary m1DataDictionary, AppContext context, FieldCollection fields, string[] databases, M1BindingSource bindingSource, bool allowEditing)
	{
		bool result = false;
		Clear();
		StringBuilder stringBuilder = new StringBuilder();
		List<string> list = new List<string>();
		foreach (FieldDefinition field in fields)
		{
			if (field.TableName.Length != 0 && !list.Contains(field.TableName, StringComparer.CurrentCultureIgnoreCase))
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(field.TableName.ToSql());
				list.Add(field.TableName);
			}
		}
		if (stringBuilder.Length != 0)
		{
			string text = stringBuilder.ToString();
			string text2 = string.Empty;
			if (m1Database != null)
			{
				text2 = m1Database.ID;
			}
			if (databases.Length != 0 && databases[0].Length != 0)
			{
				text2 = databases[0];
			}
			DataTable dataTable = null;
			DataTable dataTable2 = null;
			TableDefinition tableDefinition = null;
			DataRow[] array = null;
			bool flag = false;
			if (m1DataDictionary != null && m1User != null && m1Database != null)
			{
				dataTable = m1DataDictionary.GetDataTable(string.Format("select DDTables.dtUniqueID,DDTables.dtAppExtensionID,DDTables.dtTable,DDTables.dtDisplayName, {0} ,DDTables.dtOverrideDelete,DDTables.dtOverrideDeleteEnabledExpression, DDTables.dtDefaultObjectId,DDTables.dtGridID,DDTables.dtQuickSearchFields,DDTables.dtQuickSearchFieldsUser,DDTables.dtKeyFields,DDTables.dtKeyGroup,DDTables.dtModule,DDTables.dtEnterInSequenceField,DDTables.dtAddFld1,DDTables.dtAddFld2,DDTables.dtAddFld3,DDTables.dtUAddFld1,DDTables.dtUAddFld2,DDTables.dtUAddFld3,DDTables.dtForeignKeyDeleteFilter, IsNull(DDTables.dtColorExpression,'') As dtColorExpression,IsNull(DDTables.dtColorExpressionUser,'') As dtColorExpressionUser,DDTables.dtNumericOnly,DDTables.dtLastKeyCanBeEmpty,DDTables.dtEmptyKeyCanBeEdited,DDTables.dtKeysAtThisLevel,DDTables.dtAutoIncrement,DDTables.dtAutoIncrementUser,DDTables.dtIncrementAmount,DDTables.dtIncrementAmountUser,DDTables.dtInitialValue,DDTables.dtPrefix,DDTables.dtPrefixUser,IsNull(DDTables.dtReadonlyExpression,'') As dtReadonlyExpression,IsNull(DDTables.dtReadonlyExpressionUser,'') As dtReadonlyExpressionUser,IsNull(DDTables.dtDisableAddNewExpression,'') As dtDisableAddNewExpression,IsNull(DDTables.dtDisableAddNewExpressionUser,'') As dtDisableAddNewExpressionUser,IsNull(DDTables.dtDisableDeleteExpression,'') As dtDisableDeleteExpression,IsNull(DDTables.dtDisableDeleteExpressionUser,'') As dtDisableDeleteExpressionUser,IsNull(DDTables.dtDisableChangeIDExpression,'') As dtDisableChangeIDExpression,IsNull(DDTables.dtDisableChangeIDExpressionUser,'') As dtDisableChangeIDExpressionUser, DDTables.dtCustom,DDTables.dtGridEdit,DDTables.dtChangeDetailIdsFilter,DDTables.dtChangeID,DDTables.dtSaveAs,DDTables.dtImport,DDTables.dtMailMerge,DDTables.dtMap,DDTables.dtContactField,DDTables.dtPromptOnAddField,DDTables.dtCurrencyModeLocationField,DDTables.dtCurrencyRateIdField,DDTables.dtCurrencyCustomRateField,DDTables.dtCurrencyExchangeRateField,DDTables.dtDocumentDateField,DDTables.dtDocumentPlantIdField,DDTables.dtCurrencyUpdateType, DDTables.dtClosedField,DDTables.dtClosedValue,DDTables.dtClosedDateField,DDTables.dtClosedExtraSetExpression,DDTables.dtClosedIncludeOptionText,DDTables.dtClosedIncludeOptionSqlExpr,DDTables.dtClosedCutoffDateField,DDTables.dtClosedRoleCheck,DDTables.dtClosedHelpLink,DDTables.dtPurgeCutoffDateField,DDTables.dtPurgeHelpLink, DDTables.dtSqlView,DDTables.dtUniqueField,DDTables.dtFieldToCheckOnUpdate,DDTables.dtQuickSearchOption, DDTables.dtParentTable,parentTable.dtKeyFields As parentKeyFields, TopLevelTable.dtTable As TopLevelTable,TopLevelTable.dtDocumentDateField As TopLevelDateField,TopLevelTable.dtDocumentPlantIDField As TopLevelPlantIdField,TopLevelTable.dtKeyFields As TopLevelKeyFields, {1}  from DDTables With(Nolock) Left Outer Join DDTables parentTable With(NoLock) On DDTables.dtParentTable=parentTable.dtTable  Left Outer Join DDFields With(NoLock) On dfTable=DDTables.dtTable and DDTables.dtKeyFields<>'' and dfField = SUBSTRING(DDTables.dtkeyFields,0,Len(dfField)+1) Left Outer Join DDTables TopLevelTable With(NoLock) On TopLevelTable.dtTable=dfRelatedTable  {2} {3} Where DDTables.dtTable In ({4})", m1DataDictionary.Language.GetdtCaptionField(m1Database, removeAsClause: false, "DDTables"), getSecurityQueryFields(), m1DataDictionary.Language.GetdtCaptionJoin(m1Database, "DDTables"), getSecurityQueryJoins(m1User.ID, text2, text), text));
				if (allowEditing)
				{
					dataTable2 = m1DataDictionary.GetDataTable($"select parentFields.dfTable as ParentTable,parentFields.dfField As ParentField,childFields.dfTable as ChildTable,childFields.dfField as ChildField,childTable.dtKeyFields as ChildKeyFields,childTable.dtClosedExtraSetExpression as ChildClosedSetExpression,childFields.dfBoundParentFieldType As BindingType,childFields.dfHasChangeCode As CodeExists from DDFields parentFields Inner Join DDFields childFields on parentFields.dfField = childFields.dfBoundParentField And (childFields.dfBoundParentFieldType = 1 Or childFields.dfBoundParentFieldType = 2) Inner Join DDTables childTable on childFields.dfTable = childTable.dtTable Where parentFields.dftable In ({text}) ");
				}
			}
			else
			{
				dataTable = DesignMode.DesignModeGetDataTable(string.Format("select DDTables.dtUniqueID,DDTables.dtAppExtensionID,DDTables.dtTable,DDTables.dtDisplayName,DDTables.dtCaption,DDTables.dtOverrideDelete,DDTables.dtOverrideDeleteEnabledExpression, DDTables.dtDefaultObjectId,DDTables.dtGridID,DDTables.dtQuickSearchFields,DDTables.dtQuickSearchFieldsUser,DDTables.dtKeyFields,DDTables.dtKeyGroup,DDTables.dtModule,DDTables.dtEnterInSequenceField,DDTables.dtAddFld1,DDTables.dtAddFld2,DDTables.dtAddFld3,DDTables.dtUAddFld1,DDTables.dtUAddFld2,DDTables.dtUAddFld3,DDTables.dtForeignKeyDeleteFilter, IsNull(DDTables.dtColorExpression,'') As dtColorExpression,IsNull(DDTables.dtColorExpressionUser,'') As dtColorExpressionUser,DDTables.dtNumericOnly,DDTables.dtLastKeyCanBeEmpty,DDTables.dtEmptyKeyCanBeEdited,DDTables.dtKeysAtThisLevel,DDTables.dtAutoIncrement,DDTables.dtAutoIncrementUser,DDTables.dtIncrementAmount,DDTables.dtIncrementAmountUser,DDTables.dtInitialValue,DDTables.dtPrefix,DDTables.dtPrefixUser,IsNull(DDTables.dtReadonlyExpression,'') As dtReadonlyExpression,IsNull(DDTables.dtReadonlyExpressionUser,'') As dtReadonlyExpressionUser,IsNull(DDTables.dtDisableAddNewExpression,'') As dtDisableAddNewExpression,IsNull(DDTables.dtDisableAddNewExpressionUser,'') As dtDisableAddNewExpressionUser,IsNull(DDTables.dtDisableDeleteExpression,'') As dtDisableDeleteExpression,IsNull(DDTables.dtDisableDeleteExpressionUser,'') As dtDisableDeleteExpressionUser,IsNull(DDTables.dtDisableChangeIDExpression,'') As dtDisableChangeIDExpression,IsNull(DDTables.dtDisableChangeIDExpressionUser,'') As dtDisableChangeIDExpressionUser, DDTables.dtCustom,DDTables.dtGridEdit,DDTables.dtChangeDetailIdsFilter,DDTables.dtChangeID,DDTables.dtSaveAs,DDTables.dtImport,DDTables.dtMailMerge,DDTables.dtMap,DDTables.dtContactField,DDTables.dtPromptOnAddField,DDTables.dtCurrencyModeLocationField,DDTables.dtCurrencyRateIdField,DDTables.dtCurrencyCustomRateField,DDTables.dtCurrencyExchangeRateField,DDTables.dtDocumentDateField,DDTables.dtDocumentPlantIdField,DDTables.dtCurrencyUpdateType, DDTables.dtClosedField,DDTables.dtClosedValue,DDTables.dtClosedDateField,DDTables.dtClosedExtraSetExpression,DDTables.dtClosedIncludeOptionText,DDTables.dtClosedIncludeOptionSqlExpr,DDTables.dtClosedCutoffDateField,DDTables.dtClosedRoleCheck,DDTables.dtClosedHelpLink,DDTables.dtPurgeCutoffDateField,DDTables.dtPurgeHelpLink, DDTables.dtSqlView,DDTables.dtUniqueField,DDTables.dtFieldToCheckOnUpdate,DDTables.dtQuickSearchOption, TopLevelTable.dtTable As TopLevelTable,TopLevelTable.dtDocumentDateField As TopLevelDateField,TopLevelTable.dtDocumentPlantIDField As TopLevelPlantIdField,TopLevelTable.dtKeyFields As TopLevelKeyFields, DDTables.dtParentTable,'' As parentKeyFields, {0} from DDTables With(Nolock) {1}  Left Outer Join DDFields With(NoLock) On dfTable=DDTables.dtTable and DDTables.dtKeyFields<>'' and dfField = SUBSTRING(DDTables.dtkeyFields,0,Len(dfField)+1) Left Outer Join DDTables TopLevelTable With(NoLock) On TopLevelTable.dtTable=dfRelatedTable  Where DDTables.dtTable In ({2})", getSecurityQueryFields(), getSecurityQueryJoins("", "", text), text));
			}
			foreach (string item in list)
			{
				tableDefinition = new TableDefinition();
				tableDefinition.TableName = item.ToUpper();
				tableDefinition.TableNameFormatted = item;
				flag = false;
				if (dataTable != null)
				{
					array = dataTable.Select($"dtTable = {tableDefinition.TableName.ToUpper().ToLinq()}");
					if (array.Length != 0)
					{
						tableDefinition.Load(childReferences: dataTable2?.Select($"ParentTable = {tableDefinition.TableName.ToUpper().ToLinq()}"), row: array[0], dataDictionary: m1DataDictionary, allowEditing: allowEditing && bindingSource.Query.TableName.Equals(tableDefinition.TableName, StringComparison.CurrentCultureIgnoreCase));
						tableDefinition.LoadDatabase(text2, array[0], m1Database?.Security.GetTableExpression(text2, tableDefinition.TableName));
						flag = true;
					}
				}
				if (!flag)
				{
					tableDefinition.LoadDatabase(text2, null, m1Database?.Security.GetTableExpression(text2, tableDefinition.TableName));
				}
				tableDefinition.BindingSource = bindingSource;
				Add(tableDefinition);
			}
			DataTable dataTable3 = null;
			for (int i = 1; i < databases.Length; i++)
			{
				dataTable3 = ((m1DataDictionary == null || m1User == null || m1Database == null) ? DesignMode.DesignModeGetDataTable(string.Format("Select DDTables.dtTable, {0} From DDTables With(Nolock) {1} Where DDTables.dtTable In ({2})", getSecurityQueryFields(), getSecurityQueryJoins("", "", text), text)) : m1DataDictionary.GetDataTable($"Select DDTables.dtTable, {getSecurityQueryFields()} From DDTables With(Nolock) {getSecurityQueryJoins(m1User.ID, databases[i], text)} Where DDTables.dtTable In ({text}) "));
				using IEnumerator<TableDefinition> enumerator3 = GetEnumerator();
				while (enumerator3.MoveNext())
				{
					TableDefinition current3 = enumerator3.Current;
					flag = false;
					if (dataTable3 != null)
					{
						array = dataTable3.Select($"dtTable = {current3.TableName.ToUpper().ToLinq()}");
						if (array.Length != 0)
						{
							current3.LoadDatabase(databases[i], array[0], m1Database?.Security.GetTableExpression(databases[i], current3.TableName));
							flag = true;
						}
					}
					if (!flag)
					{
						current3.LoadDatabase(databases[i], null, m1Database?.Security.GetTableExpression(databases[i], current3.TableName));
					}
				}
			}
			foreach (FieldDefinition field2 in fields)
			{
				if (field2.TableName.Length != 0 && Contains(field2.TableName))
				{
					field2.Table = base[field2.TableName];
				}
				else
				{
					field2.Table = null;
				}
			}
			result = true;
		}
		return result;
	}

	private string getSecurityQueryFields()
	{
		return " Case When DDsecurityTablesTable.dtLevel   Is Null Then Convert(tinyint,0) Else DDsecurityTablesTable.dtLevel   End AS dtLevelTable,  Case When DDsecurityTablesDataset.dtLevel Is Null Then Convert(tinyint,0) Else DDsecurityTablesDataset.dtLevel End AS dtLevelDataset,  Case When DDsecurityTablesTableGroup.dtLevel   Is Null Then Convert(tinyint,0) Else DDsecurityTablesTableGroup.dtLevel   End AS dtLevelTableGroup,  Case When DDsecurityTablesDatasetGroup.dtLevel Is Null Then Convert(tinyint,0) Else DDsecurityTablesDatasetGroup.dtLevel End AS dtLevelDatasetGroup ";
	}

	private string getSecurityQueryJoins(string userID, string databaseName, string tableList)
	{
		return string.Format(" LEFT OUTER JOIN DDSecurityTables DDSecurityTablesTable  With(Nolock) ON DDSecurityTablesTable.dtTable = DDTables.dtTable And DDSecurityTablesTable.dtField = '' AND DDSecurityTablesTable.dtUserID = {0}  AND DDSecurityTablesTable.dtDataSet = {1}  LEFT OUTER JOIN DDSecurityTables DDSecurityTablesDataset With(Nolock) ON DDSecurityTablesDataset.dtTable = '' And DDSecurityTablesDataset.dtField = '' AND DDSecurityTablesDataset.dtUserID = {0} AND DDSecurityTablesDataset.dtDataSet = {1}  LEFT OUTER JOIN (Select dtTable  ,Max(Case When dtLevel Is Null Then Convert(tinyint,0) Else dtLevel End) as dtLevel  From DDSecurityTables sub2 With(Nolock) Where sub2.dtDataSet = {1}  And sub2.dtTable In ({2})  And sub2.dtField = '' And  sub2.dtUserID In (Select dzGroupID From DDSecurityGroups Where dzDataset = {1} And dzUserID = {0}) Group By dtTable)  As DDSecurityTablesTableGroup   ON DDTables.dtTable = DDSecurityTablesTableGroup.dtTable  LEFT OUTER JOIN (Select dtDataset,Max(Case When dtLevel Is Null Then Convert(tinyint,0) Else dtLevel End) as dtLevel  From DDSecurityTables sub3 With(Nolock) Where sub3.dtDataSet ={1}  And sub3.dtTable = '' And sub3.dtField = '' And  sub3.dtUserID In (Select dzGroupID From DDSecurityGroups Where dzDataset = {1} And dzUserID = {0} ) Group By dtDataset) As DDSecurityTablesDatasetGroup ON 0=0 ", userID.ToSql(), databaseName.ToSql(), tableList);
	}

	public TableDefinition GetParentTable(TableDefinition table)
	{
		string[] keyFieldsArray = table.KeyFieldsArray;
		foreach (string key in keyFieldsArray)
		{
			if (table.BindingSource != null && Contains(table.BindingSource.Fields[key].RelatedTable))
			{
				return base[table.BindingSource.Fields[key].RelatedTable];
			}
		}
		return null;
	}
}
