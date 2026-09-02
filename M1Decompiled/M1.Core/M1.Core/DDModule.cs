using System;
using System.Data;
using System.Diagnostics;

namespace M1.Core;

[DebuggerDisplay("{ModuleID} - {Caption}")]
public class DDModule
{
	public string AppID;

	public string ModuleID;

	public string Caption;

	public string PropertiesTable;

	public string SecurityTables;

	public string[] SeucurityTablesArray;

	public string SecurityModules;

	public string[] SecurityModulesArray;

	public string PropertiesFieldName;

	public bool PropertiesFieldValue;

	public bool Virtual;

	public DDModule(DataRow row)
	{
		AppID = row.Field<string>("ddmAppExtensionID");
		ModuleID = row.Field<string>("ddmModuleID");
		Caption = row.Field<string>("ddmCaption");
		PropertiesTable = row.Field<string>("ddmPropertiesTable");
		SecurityTables = row.Field<string>("ddmSecurityTables");
		SeucurityTablesArray = SecurityTables.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
		SecurityModules = row.Field<string>("ddmSecurityModules");
		SecurityModulesArray = SecurityModules.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
		PropertiesFieldName = row.Field<string>("ddmPropertiesFieldName");
		PropertiesFieldValue = row.Field<bool>("ddmPropertiesFieldValue");
		Virtual = row.Field<bool>("ddmVirtual");
	}
}
