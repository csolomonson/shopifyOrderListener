using System.Data;
using System.Data.SqlClient;

namespace M1.Core;

public class DDFindInfo
{
	public string[] KeyFields;

	public object[] KeyValues;

	public string[] DisplayFields;

	public object[] DisplayValues;

	public string[] DesignerFields;

	public object[] DesignerValues;

	public string Table = string.Empty;

	public string Field = string.Empty;

	public int FieldSize;

	public DDFieldContentType ContentType = DDFieldContentType.None;

	public bool WholeWordMatch;

	public string WholeWord = string.Empty;

	public string FoundText = string.Empty;

	public bool CaseMatch;

	public int FileLineNumber;

	public int FunctionLineNumber;

	public int CharacterPosition;

	public string FunctionName = string.Empty;

	public string FunctionType = string.Empty;

	public string LineText = string.Empty;

	public string ReplaceType = string.Empty;

	public DataRow Row;

	public SqlDataAdapter Adapter;

	public bool CustomData;
}
