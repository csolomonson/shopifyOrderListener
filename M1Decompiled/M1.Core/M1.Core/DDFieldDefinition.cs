using System;

namespace M1.Core;

public class DDFieldDefinition
{
	public string FieldName = string.Empty;

	public string FieldType = string.Empty;

	public bool Nullable;

	public DDFieldFlag Flag;

	public DDFieldContentType ContentType = DDFieldContentType.None;

	public string DefaultValue = string.Empty;

	public string RelatedFieldForCustom = string.Empty;

	public DDFieldDefinition(string name, string type, bool nullable, string defaultValue, DDFieldFlag flag, DDFieldContentType contentType, string relatedFieldForCustom)
	{
		RelatedFieldForCustom = relatedFieldForCustom;
		FieldName = name;
		FieldType = type;
		Nullable = nullable;
		Flag = flag;
		ContentType = contentType;
		DefaultValue = defaultValue;
	}

	public DDFieldDefinition(string name, string type, bool nullable, string defaultValue, DDFieldFlag flag, DDFieldContentType contentType)
	{
		FieldName = name;
		FieldType = type;
		Nullable = nullable;
		Flag = flag;
		ContentType = contentType;
		DefaultValue = defaultValue;
	}

	public int GetSize()
	{
		int num = FieldType.IndexOf('(');
		if (num != -1)
		{
			string text = FieldType.Substring(num + 1);
			num = text.IndexOf(')');
			if (num != -1)
			{
				text = text.Substring(0, num);
				if (text.Equals("max", StringComparison.CurrentCultureIgnoreCase))
				{
					return -1;
				}
				return Convert.ToInt32(text);
			}
		}
		return 0;
	}
}
