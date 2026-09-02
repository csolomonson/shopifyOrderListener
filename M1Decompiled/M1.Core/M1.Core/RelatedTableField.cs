using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using M1.Extensions;

namespace M1.Core;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class RelatedTableField : IRelatedTableField
{
	internal string[] ParentJoinFields;

	internal string CurrentTableName;

	internal string[] CurrentKeyFields;

	internal SqlCommand Command;

	internal Dictionary<string, RelatedTableField> RelatedTableFields;

	internal RelatedTableField Parent;

	internal FieldDefinition _Field;

	private DataRow _RelatedTableLookupRow;

	internal DataTable _RelatedTable;

	private M1DataDictionary _DataDictionary;

	internal bool haveGottenFieldInfo;

	internal DataRow lastLoadedRow;

	private bool _RowExists;

	public string Name;

	internal DataRow RelatedTableLookupRow
	{
		get
		{
			return _RelatedTableLookupRow;
		}
		set
		{
			if (_RelatedTableLookupRow == value)
			{
				return;
			}
			_RelatedTableLookupRow = value;
			if (RelatedTableFields == null)
			{
				return;
			}
			foreach (KeyValuePair<string, RelatedTableField> relatedTableField in RelatedTableFields)
			{
				if (relatedTableField.Value != null)
				{
					relatedTableField.Value.RelatedTableLookupRow = null;
				}
			}
		}
	}

	internal M1Database Database
	{
		get
		{
			if (Parent == null)
			{
				return _Field.Table.GetCurrentDataRowForProcessingQuick().Database;
			}
			return Parent.Database;
		}
	}

	internal SqlTransaction Transaction
	{
		get
		{
			if (_Field != null)
			{
				return _Field.Table.GetCurrentDataRowForProcessingQuick().SqlTransaction;
			}
			return Parent.Transaction;
		}
	}

	internal DataRow ParentJoinRow
	{
		get
		{
			if (_Field == null)
			{
				return Parent._RelatedTableLookupRow;
			}
			return _Field.Table.GetCurrentDataRowForProcessingQuick().Row;
		}
	}

	internal M1DataDictionary DataDictionary
	{
		get
		{
			if (Parent == null)
			{
				return _DataDictionary;
			}
			return Parent.DataDictionary;
		}
		set
		{
			_DataDictionary = value;
		}
	}

	public bool RowExists
	{
		get
		{
			CheckRowIsLoaded();
			return _RowExists;
		}
	}

	public object Value
	{
		get
		{
			CheckRowIsLoaded();
			if (RelatedTableLookupRow.RowState == DataRowState.Deleted)
			{
				return RelatedTableLookupRow[Name, DataRowVersion.Original];
			}
			return RelatedTableLookupRow[Name];
		}
	}

	public RelatedTableField(FieldDefinition field)
	{
		Parent = null;
		_Field = field;
		RelatedTableFields = new Dictionary<string, RelatedTableField>(StringComparer.CurrentCultureIgnoreCase);
		ParentJoinFields = field.RelatedFieldsAndCurrentFieldArray;
		CurrentTableName = field.RelatedTable;
		CurrentKeyFields = field.RelatedTableKeyFieldsArray;
		DataDictionary = field.DataDictionary;
	}

	public RelatedTableField(RelatedTableField parent)
	{
		Parent = parent;
		RelatedTableFields = new Dictionary<string, RelatedTableField>(StringComparer.CurrentCultureIgnoreCase);
	}

	internal bool AddRelatedTableLookupField(string field, string parentField)
	{
		if (RelatedTableFields.ContainsKey(parentField))
		{
			RelatedTableField relatedTableField = RelatedTableFields[parentField];
			if (relatedTableField == null)
			{
				relatedTableField = new RelatedTableField(this);
				RelatedTableFields[parentField] = relatedTableField;
			}
			if (!relatedTableField.RelatedTableFields.ContainsKey(field))
			{
				relatedTableField.RelatedTableFields.Add(field, null);
			}
			return true;
		}
		foreach (KeyValuePair<string, RelatedTableField> relatedTableField2 in RelatedTableFields)
		{
			if (relatedTableField2.Value != null && relatedTableField2.Value.AddRelatedTableLookupField(field, parentField))
			{
				return true;
			}
		}
		return false;
	}

	internal string ConstructFields()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, RelatedTableField> relatedTableField in RelatedTableFields)
		{
			if (!list.Contains(relatedTableField.Key, StringComparer.CurrentCultureIgnoreCase))
			{
				list.Add(relatedTableField.Key);
			}
			if (relatedTableField.Value == null || relatedTableField.Value.ParentJoinFields == null)
			{
				continue;
			}
			string[] parentJoinFields = relatedTableField.Value.ParentJoinFields;
			foreach (string text in parentJoinFields)
			{
				if (!list.Contains(text, StringComparer.CurrentCultureIgnoreCase))
				{
					list.Add(text);
				}
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string item in list)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(',');
			}
			stringBuilder.Append(item);
		}
		return stringBuilder.ToString();
	}

	internal void ConstructCommand()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < CurrentKeyFields.Length; i++)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(" And ");
			}
			stringBuilder.Append(CurrentKeyFields[i] + "=@p" + i);
		}
		Command = Database.NewSqlCommand("Select " + ConstructFields() + " From " + CurrentTableName + " Where " + stringBuilder.ToString());
	}

	internal void GetFieldInfo(string parentName)
	{
		haveGottenFieldInfo = true;
		if (string.IsNullOrWhiteSpace(CurrentTableName))
		{
			SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Select dfField,dfRelatedFields,dfRelatedTable,dtKeyFields From DDFields Inner Join DDTables On dfRelatedTable = dtTable Where dfTable = @Table And dfField = @Field");
			sqlCommand.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = Parent.CurrentTableName;
			sqlCommand.Parameters.Add(new SqlParameter("@Field", SqlDbType.NVarChar)).Value = parentName;
			DataTable dataTable = DataDictionary.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row = dataTable.Rows[0];
				ParentJoinFields = (row.Field<string>("dfRelatedFields") + "," + row.Field<string>("dfField")).Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				CurrentKeyFields = row.Field<string>("dtKeyFields").Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				CurrentTableName = row.Field<string>("dfRelatedTable");
			}
		}
		foreach (KeyValuePair<string, RelatedTableField> relatedTableField in RelatedTableFields)
		{
			if (relatedTableField.Value != null)
			{
				relatedTableField.Value.GetFieldInfo(relatedTableField.Key);
			}
		}
	}

	internal void SetParameters()
	{
		if (Command == null)
		{
			ConstructCommand();
		}
		Command.Parameters.Clear();
		for (int i = 0; i < ParentJoinFields.Length; i++)
		{
			Command.Parameters.Add(new SqlParameter("@p" + i, FieldDefinition.GetSqlDbType(ParentJoinRow.Table.Columns[ParentJoinFields[i]].DataType))).Value = ParentJoinRow[ParentJoinFields[i]];
		}
	}

	internal void FillRow()
	{
		if (!haveGottenFieldInfo)
		{
			GetFieldInfo(Name);
		}
		if (_Field != null && _Field.IsPartOfKey && _Field.Table != null)
		{
			RelatedTableLookupRow = _Field.RelatedTableGetDataRow(ConstructFields(), Database, ParentJoinRow, alwaysReturnValidRow: true, Transaction);
			_RowExists = true;
			return;
		}
		SetParameters();
		if (_RelatedTable == null)
		{
			_RelatedTable = Database.GetDataTable(Command, Transaction);
		}
		else
		{
			_RelatedTable.Rows.Clear();
			Database.Fill(_RelatedTable, Command, Transaction);
		}
		if (_RelatedTable.Rows.Count == 0)
		{
			_RelatedTable.AddBlankRow(allowNullForDefaultValue: true);
			_RowExists = false;
		}
		else
		{
			_RowExists = true;
		}
		RelatedTableLookupRow = _RelatedTable.Rows[0];
	}

	internal void CheckRowIsLoaded()
	{
		if (RelatedTableLookupRow != null)
		{
			DataRow relatedTableLookupRow = RelatedTableLookupRow;
			if ((relatedTableLookupRow == null || relatedTableLookupRow.RowState != DataRowState.Detached) && lastLoadedRow == ParentJoinRow)
			{
				return;
			}
		}
		FillRow();
		lastLoadedRow = ParentJoinRow;
	}

	public RelatedTableField Fields(string name)
	{
		return GetFields(name);
	}

	private RelatedTableField GetFields(string name)
	{
		CheckRowIsLoaded();
		if (RelatedTableFields.ContainsKey(name))
		{
			Name = name;
			return this;
		}
		if (RelatedTableFields.ContainsKey(Name))
		{
			return RelatedTableFields[Name].Fields(name);
		}
		throw new M1Exception($"Fields(\"{Parent.Name}\").Fields(\"\").Value call specified without being parsed.");
	}
}
