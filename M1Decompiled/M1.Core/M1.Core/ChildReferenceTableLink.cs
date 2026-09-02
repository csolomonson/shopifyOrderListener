using System;
using System.Collections.Generic;
using System.Data;

namespace M1.Core;

public class ChildReferenceTableLink : IDisposable
{
	public FieldDefinition.BoundParentFieldTypeEnum BindingType;

	public string ParentTable = string.Empty;

	public string ParentField = string.Empty;

	public string ChildTable = string.Empty;

	public string ChildField = string.Empty;

	public string ChildKeyFields = string.Empty;

	public string[] ChildKeyFieldsArray;

	public string ChildClosedSetExpression = string.Empty;

	public M1BindingSource ChildBindingSource;

	public TableDefinition ChildTableDefinition;

	public List<ChildReferenceTableLink> ChildReferenceTableLinks = new List<ChildReferenceTableLink>();

	public bool CodeExists;

	public ChildReferenceTableLink(DataRow row)
	{
		ParentTable = row.Field<string>("ParentTable").Trim();
		ParentField = row.Field<string>("ParentField").Trim();
		ChildTable = row.Field<string>("ChildTable").Trim();
		ChildField = row.Field<string>("ChildField").Trim();
		ChildKeyFields = row.Field<string>("ChildKeyFields").Trim();
		ChildKeyFieldsArray = ChildKeyFields.Split(',');
		ChildClosedSetExpression = row.Field<string>("ChildClosedSetExpression");
		CodeExists = row.Field<bool>("CodeExists");
		BindingType = row.Field<FieldDefinition.BoundParentFieldTypeEnum>("BindingType");
	}

	public void Dispose()
	{
		if (ChildBindingSource != null)
		{
			ChildBindingSource.Dispose();
			ChildBindingSource = null;
		}
		if (ChildTableDefinition != null)
		{
			ChildTableDefinition.Dispose();
			ChildTableDefinition = null;
		}
		if (ChildReferenceTableLinks != null)
		{
			ChildReferenceTableLinks.Clear();
			ChildReferenceTableLinks = null;
		}
	}
}
