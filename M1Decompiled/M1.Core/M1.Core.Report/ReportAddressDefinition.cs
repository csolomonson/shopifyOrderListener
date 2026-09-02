using System.Runtime.InteropServices;
using M1.Core.Script;

namespace M1.Core.Report;

[ComVisible(true)]
public class ReportAddressDefinition
{
	public ScriptingBase ScriptObj;

	public M1AdoRecordsetProxy RecordsetObj;

	public string AddressTable;

	private string[] _AddressContactFields = new string[0];

	public string LastContactField = string.Empty;

	public string Caption;

	public string AddressQuery;

	public string DocumentTable;

	public string[] DocumentKeyFields;

	public string[] AddressContactFields
	{
		get
		{
			return _AddressContactFields;
		}
		set
		{
			_AddressContactFields = value;
			if (_AddressContactFields != null && _AddressContactFields.Length != 0)
			{
				LastContactField = _AddressContactFields[_AddressContactFields.Length - 1];
			}
			else
			{
				LastContactField = string.Empty;
			}
		}
	}
}
