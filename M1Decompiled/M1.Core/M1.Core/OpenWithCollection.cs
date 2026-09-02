using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace M1.Core;

public class OpenWithCollection : KeyedCollection<string, OpenWithDefinition>
{
	public OpenWithCollection()
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
	}

	protected override string GetKeyForItem(OpenWithDefinition item)
	{
		return item.ID;
	}

	public void Load(M1Database database, M1User m1User, M1DataDictionary m1DataDictionary, AppContext context, string tableName, string fieldName)
	{
		if (tableName.Length == 0 && fieldName.Length == 0)
		{
			return;
		}
		SqlCommand sqlCommand = m1DataDictionary.NewSqlCommand("Select dwID,dwAppExtensionID,dwTable,dwField,dwExtension,dwButtonImage,dwButtonImageUser,dwType,dwSequence,dwCode,dwObject,dwActionName,dwEnabledExpression,dwEnabledExpressionUser,dwSaveBefore,dwBindReadOnly,dwPromptField,dwHide,dwUHide,dwCustom,dwCaptionExpression,dwCaptionExpressionUser," + m1DataDictionary.Language.GetdwDescField(database) + " From DDOpenWiths " + m1DataDictionary.Language.GetdwDescJoin(database) + " Where ((dwTable = @TableName And dwTable <> '' And dwField = '') Or (dwTable = '' And dwField <> '' And dwField = @FieldName)) And dwType <> 6 Order By dwSequence,dwDesc");
		sqlCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = tableName;
		sqlCommand.Parameters.Add(new SqlParameter("@FieldName", SqlDbType.NVarChar)).Value = fieldName;
		foreach (DataRow row in m1DataDictionary.GetDataTable(sqlCommand).Rows)
		{
			if (!Contains(row.Field<string>("dwID")))
			{
				Add(new OpenWithDefinition(row));
			}
		}
	}
}
