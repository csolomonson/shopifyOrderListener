using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using M1.Extensions;

namespace M1.Core;

public class DmoDD
{
	private class ConvertRowInfo
	{
		public string OldClassID;

		public string NewClassID;

		public string Name;

		public int? Top;

		public int? Left;

		public int? TopForOrdering;

		public Point? Location;

		public int? Height;

		public int? Width;

		public string DataSource = string.Empty;

		public string DataField = string.Empty;

		public string DataFieldText = string.Empty;

		public int Sequence;

		public bool IsCustom;

		public DataRow Row;

		public ConvertRowInfo PreviousNodeInZOrder;

		public ConvertRowInfo NextNodeInZOrder;

		public Dictionary<string, string> StandardProperties = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

		public Dictionary<string, string> CustomProperties = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

		public ConvertRowInfo Group;

		public ConvertRowInfo(string name)
		{
			Name = name;
		}

		public string GetCustomProperties()
		{
			if (CustomProperties == null && CustomProperties.Count == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> customProperty in CustomProperties)
			{
				stringBuilder.Append(customProperty.Key + " = " + customProperty.Value + "\r\n");
			}
			return stringBuilder.ToString();
		}

		public bool IsProperty(string name, string value)
		{
			if (CustomProperties.ContainsKey(name) && CustomProperties[name].Equals(value, StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
			if (StandardProperties.ContainsKey(name) && StandardProperties[name].Equals(value))
			{
				return true;
			}
			return false;
		}

		public void SetCustomProperty(string name, string value)
		{
			if (CustomProperties.ContainsKey(name))
			{
				CustomProperties[name] = value;
			}
			else
			{
				CustomProperties.Add(name, value);
			}
			if (StandardProperties.ContainsKey(name) && StandardProperties[name].Equals(value))
			{
				CustomProperties.Remove(name);
			}
		}

		public override string ToString()
		{
			return Name + " - " + NewClassID + ((Group == null) ? "" : (" - " + Group.Name)) + ((!Location.HasValue) ? "" : ("(" + Location.Value.X + "," + Location.Value.Y + ")")) + " - " + DataSource + ((string.IsNullOrWhiteSpace(DataSource) && string.IsNullOrWhiteSpace(DataField)) ? "" : ".") + DataField + " - " + Sequence;
		}
	}

	private SqlConnection currentConnection;

	private AppContext currentContext;

	private bool? isDDFormCodeType;

	private Dmo dmo;

	private IDictionary<string, string> events = new Dictionary<string, string>
	{
		{ "_UserChange(aParms)", "_CellChange(sender, e)" },
		{ "M1DataControl_ViewInitialize()", "this_initialize(sender, e)" },
		{ "M1DataControl_ViewDestroy()", "this_Destroy(sender, e)" },
		{ "_RecordChange()", "_RecordChange(sender, e)" },
		{ "_RecordNew()", "_AddNewCompleted(sender, e)" },
		{ "_RecordSave()", "_SaveDataCompleted(sender, e)" },
		{ "_RecordValid()", "_Validate(sender, e)" },
		{ "_RecordDelete()", "_RowUpdateDeleteBefore(sender, e)" },
		{ "_UserChange()", "_UserChange(sender, e)" },
		{ "_Change()", "_ValueChanged(sender, e)" },
		{ "_Click()", "_Click(sender, e)" },
		{ "_DblClick()", "_DoubleClick(sender, e)" },
		{ "_Timer()", "_Tick(sender, e)" },
		{ "_RowChange()", "_RowChange(sender, e)" },
		{ "_ActionMessage(cMessageID, aParameters, aParametersEx)", "_ActionMessage(sender, e)" },
		{ "_NavigateAway()", "_NavigateAway(sender, e)" },
		{ "_GetNextID()", "_GetNextID(sender, e)" }
	};

	private void updateDDData(string databaseName, Action<string> msgDelegate, DDDatabaseDefinition ddDef, AppExtensionCollection appExtensions)
	{
		List<string> list = new List<string>();
		foreach (AppExtension appExtension in appExtensions)
		{
			Assembly dDAssembly = appExtension.GetDDAssembly();
			if (!(dDAssembly != null))
			{
				continue;
			}
			object[] customAttributes = dDAssembly.GetCustomAttributes(typeof(DDTableVersionAttribute), inherit: false);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				DDTableVersionAttribute dDTableVersionAttribute = (DDTableVersionAttribute)customAttributes[i];
				if ((appExtension.LastUpdatedDDVersion.Length == 0 || appExtension.LastUpdatedDDVersion.CompareTo(dDTableVersionAttribute.Version) < 0) && !list.Contains(dDTableVersionAttribute.Table, StringComparer.CurrentCultureIgnoreCase))
				{
					list.Add(dDTableVersionAttribute.Table);
				}
			}
		}
		Dmo dmo = new Dmo(currentContext, currentContext.DDServerManager);
		foreach (string item in list)
		{
			if (item.StartsWith("Web", StringComparison.CurrentCultureIgnoreCase))
			{
				dmo.DropTable(null, null, databaseName, item);
				CreateDataDictionaryTables(null, databaseName, item, string.Empty, ddDef);
				continue;
			}
			if (item.Equals("DDSeries", StringComparison.CurrentCultureIgnoreCase))
			{
				ExecuteCommand(databaseName, "UPDATE DDSeries SET diCustom = 0 WHERE diUserid ='' and diCustom = 1");
			}
			if (item.Equals("DDVisualizers", StringComparison.CurrentCultureIgnoreCase))
			{
				ExecuteCommand(databaseName, "UPDATE DDVisualizers SET dvCustom = 0 WHERE dvUserid ='' and dvCustom = 1");
			}
			ReloadTable(databaseName, item, recreateTable: false, msgDelegate, ddDef);
		}
		if (list.Contains("DDCode", StringComparer.CurrentCultureIgnoreCase) || list.Contains("DDFields", StringComparer.CurrentCultureIgnoreCase))
		{
			refreshHasChangeCode(databaseName);
		}
		if (list.Contains("DDCode", StringComparer.CurrentCultureIgnoreCase) || list.Contains("DDTables", StringComparer.CurrentCultureIgnoreCase))
		{
			refreshHasDeleteCode(databaseName);
		}
		ReloadDDLangTables(databaseName, msgDelegate);
	}

	private void updateDDCustomizations(string databaseName, Action<string> msgDelegate, AppExtensionCollection appExtensions, bool convertCustomFormCode)
	{
		Dictionary<DDConversionAttribute, Type> dictionary = new Dictionary<DDConversionAttribute, Type>();
		foreach (AppExtension appExtension in appExtensions)
		{
			Assembly dDAssembly = appExtension.GetDDAssembly();
			if (!(dDAssembly != null))
			{
				continue;
			}
			Type[] types = dDAssembly.GetTypes();
			foreach (Type type in types)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(DDConversionAttribute), inherit: false);
				if (customAttributes != null && customAttributes.Length != 0)
				{
					DDConversionAttribute dDConversionAttribute = (DDConversionAttribute)customAttributes[0];
					if (appExtension.LastUpdatedDDVersion.Length != 0 && appExtension.LastUpdatedDDVersion.CompareTo(dDConversionAttribute.Version) < 0)
					{
						dictionary.Add(dDConversionAttribute, type);
					}
				}
			}
		}
		if (dictionary.Count == 0)
		{
			return;
		}
		DDConversionParms dDConversionParms = new DDConversionParms(this, databaseName, convertCustomFormCode);
		foreach (KeyValuePair<DDConversionAttribute, Type> item in dictionary.OrderBy((KeyValuePair<DDConversionAttribute, Type> r) => r.Key.Version + r.Value.Name))
		{
			if (msgDelegate != null)
			{
				if (!string.IsNullOrWhiteSpace(item.Key.Description))
				{
					msgDelegate(item.Key.Description + " - " + item.Key.Version);
				}
				else
				{
					msgDelegate("Processing customization conversion - " + item.Key.Version);
				}
			}
			Activator.CreateInstance(item.Value, dDConversionParms);
		}
	}

	public void UpdateAppExtensionVersions(string databaseName, AppExtensionCollection appExtensions)
	{
		UpdateAppExtensionVersions(databaseName, appExtensions, null, null);
	}

	public void UpdateAppExtensionVersions(string databaseName, AppExtensionCollection appExtensions, SqlConnection connection, M1User user)
	{
		if (appExtensions == null)
		{
			appExtensions = new AppExtensionCollection(this, currentContext, databaseName);
			appExtensions.Refresh();
		}
		foreach (AppExtension appExtension in appExtensions)
		{
			if (appExtension.DDAssemblyVersion.Length != 0)
			{
				ExecuteCommand(connection, databaseName, "Update DDAppExtensions Set dpLastUpdatedDDVersion = " + appExtension.DDAssemblyVersion.ToSql() + " Where dpAppExtensionID = " + appExtension.AppID.ToSql());
			}
		}
	}

	public void UpdateDD(string databaseName, string fromVersion, Action<string> msgDelegate, bool convertCustomFormCode)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		msgDelegate?.Invoke("Opening " + databaseName + " for exclusive use");
		currentContext.DDServerManager.SetSingleUserMode(null, databaseName, turnOn: true);
		try
		{
			DDDatabaseDefinition dDDatabaseDefinition = new DDDatabaseDefinition();
			try
			{
				updateDDStructure(databaseName, fromVersion, msgDelegate, dDDatabaseDefinition);
				AppExtensionCollection appExtensionCollection = new AppExtensionCollection(this, currentContext, databaseName);
				appExtensionCollection.Refresh();
				updateDDData(databaseName, msgDelegate, dDDatabaseDefinition, appExtensionCollection);
				msgDelegate?.Invoke("Processing custom expressions");
				doSecondCheckReloadTable(null, databaseName, dDDatabaseDefinition);
				updateDDCustomizations(databaseName, msgDelegate, appExtensionCollection, convertCustomFormCode);
				if (DoesTableExist(null, databaseName, "DDFormCodeTemp"))
				{
					string arg = (string)currentContext.DDServerManager.ExecuteScalar(null, null, databaseName, "Select SERVERPROPERTY('collation')");
					ExecuteCommand(databaseName, $"Update DDCode Set dkSourceUniqueID = dmUniqueID From DDCode Inner Join DDFormCodeTemp On DDCode.dkCodeID = DDFormCodeTemp.dkCodeID Inner Join DDForms On DDFormCodeTemp.dmFormID = DDForms.dmFormID COLLATE {arg}");
					new Dmo(currentContext, currentContext.DDServerManager).DropTable(null, null, databaseName, "DDFormCodeTemp");
				}
				UpdateAppExtensionVersions(databaseName, appExtensionCollection);
			}
			finally
			{
				foreach (DDCustomTableInfo loadedTableInfo in dDDatabaseDefinition.LoadedTableInfos)
				{
					if (loadedTableInfo.QueryHasRun)
					{
						ExecuteCommand(null, databaseName, "DROP TABLE " + loadedTableInfo.TempTable);
						loadedTableInfo.QueryHasRun = false;
					}
				}
				dDDatabaseDefinition.LoadedTableInfos.Clear();
			}
			ExecuteCommand(databaseName, "UPDATE DDInfo SET ddVersion = " + currentContext.Version.ToSql());
			if (!fromVersion.Equals(currentContext.Version))
			{
				ExecuteCommand(databaseName, "UPDATE DDInfo SET ddUpgradeVersions = " + (fromVersion + "->" + currentContext.Version + " (" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm") + ")").ToSql() + " + Char(13) + Convert(nvarchar(max),ddUpgradeVersions)");
			}
		}
		finally
		{
			currentContext.DDServerManager.SetSingleUserMode(null, databaseName, turnOn: false);
		}
		if (fromVersion.CompareTo("8.10.000") < 0)
		{
			new Dmo(currentContext, currentContext.DDServerManager).ShrinkDatabase(null, null, databaseName);
		}
		stopwatch.Stop();
		msgDelegate?.Invoke("The data dictionary conversion took " + stopwatch.Elapsed.TotalSeconds.ToString("###,###") + " seconds to complete");
	}

	private void updateDDStructure(string databaseName, string fromVersion, Action<string> msgDelegate, DDDatabaseDefinition ddDef)
	{
		msgDelegate?.Invoke("Verifying data dictionary structure");
		if (currentContext.InstalledDatabases.Count == 0)
		{
			currentContext.InstalledDatabases.Refresh();
		}
		Dmo dmo = new Dmo(currentContext, currentContext.DDServerManager);
		string collation = dmo.GetCollation(null, string.Empty);
		if (fromVersion.CompareTo("9.00.015") < 0 && dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfRelatedFields"))
		{
			ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfRelatedFields', 'dfRelatedFieldsEx', 'COLUMN'");
			ExecuteCommand(databaseName, "ALTER TABLE DDFields Add dfRelatedFields nvarchar(100)");
			ExecuteCommand(databaseName, "Update DDFields Set dfRelatedFields = dfRelatedFieldsEx");
			dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfRelatedFieldsEx", dropTriggers: false);
		}
		if (fromVersion.CompareTo("9.00.007") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDForms", "dmAssembliesUser"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDForms Add dmAssembliesUser nvarchar(max)");
		}
		if (fromVersion.CompareTo("8.10.083") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDOpenWiths", "dwCaptionExpression"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDOpenWiths Add dwCaptionExpression nvarchar(max)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDOpenWiths", "dwCaptionExpressionUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDOpenWiths Add dwCaptionExpressionUser nvarchar(max)");
			}
		}
		if (fromVersion.CompareTo("8.10.082") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtQuickSearchFields"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables Add dtQuickSearchFields nvarchar(max)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtQuickSearchFieldsUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables Add dtQuickSearchFieldsUser nvarchar(max)");
			}
		}
		if (fromVersion.CompareTo("8.10.081") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtHasDeleteCode"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDTables Add dtHasDeleteCode bit Not Null Default(0)");
		}
		if (fromVersion.CompareTo("8.10.079") < 0)
		{
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtCurrencyDateField"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtCurrencyDateField', 'dtDocumentDateField', 'COLUMN'");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtDocumentPlantIdField"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtDocumentPlantIdField nvarchar(30) Not Null Default('')");
			}
		}
		if (fromVersion.CompareTo("9.01.003") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgUseCurrencyMode"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDGridDetails ADD dgUseCurrencyMode bit Not Null Default(0)");
		}
		if (fromVersion.CompareTo("8.10.066") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtDisableChangeIDExpression"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtDisableChangeIDExpression nvarchar(max)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtDisableChangeIDExpressionUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtDisableChangeIDExpressionUser nvarchar(max)");
			}
		}
		if (fromVersion.CompareTo("8.10.065") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDSecurityTables", "dtRowFilter"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDSecurityTables ADD dtRowFilter nvarchar(max)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDSecurityTables", "dtEditExpression"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDSecurityTables ADD dtEditExpression nvarchar(max)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDSecurityTables", "dtAddExpression"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDSecurityTables ADD dtAddExpression nvarchar(max)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDSecurityTables", "dtDeleteExpression"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDSecurityTables ADD dtDeleteExpression nvarchar(max)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDSecurityTables", "dtChangeIDExpression"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDSecurityTables ADD dtChangeIDExpression nvarchar(max)");
			}
		}
		if (fromVersion.CompareTo("8.10.032") < 0 && !DoesTableExist(null, databaseName, "DDAppExtensions"))
		{
			CreateDataDictionaryTables(null, databaseName, "DDAppExtensions", string.Empty, ddDef);
		}
		if (fromVersion.CompareTo("8.10.038") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDAppExtensions", "dpLastUpdatedDDVersion"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDAppExtensions ADD dpLastUpdatedDDVersion varchar(10) Not Null Default('')");
		}
		if (fromVersion.CompareTo("8.10.038") < 0)
		{
			ReloadTable(databaseName, "DDAppExtensions", recreateTable: false, msgDelegate, ddDef);
		}
		if (fromVersion.CompareTo("8.10.040") < 0)
		{
			ExecuteCommand(databaseName, "Update DDAppExtensions Set dpLastUpdatedDDVersion = " + fromVersion.ToSql());
		}
		if (fromVersion.CompareTo("8.10.043") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxImageLarge"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD dxImageLarge varchar(50) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxImageSmall"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD dxImageSmall varchar(50) Not Null Default('')");
			}
			ExecuteCommand(databaseName, "Update DDExplorer Set dxType = 32, dxImageLarge = 'gantt32', dxImageSmall = 'gantt16', dxExtd = 'Call Forms.Ax(\"Jobs\").ShowSchedulingBoard()' Where dxType = 11");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxType = 32, dxImageLarge = 'shopload32', dxImageSmall = 'shopload16', dxExtd = 'Call Forms.Ax(\"Jobs\").ShowShopLoad()' Where dxType = 6");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxType = 32, dxImageLarge = 'gantt32', dxImageSmall = 'gantt16', dxExtd = 'Call Forms.Ax(\"Payroll\").ShowLeaveBoard()' Where dxType = 21");
			if (dmo.DoesFieldExist(null, null, databaseName, "DDObjectDetails", "dlForeign"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDObjectDetails", "dlForeign", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDObjectDetails", "dlJoin1"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDObjectDetails", "dlJoin1", dropTriggers: false);
			}
		}
		if (fromVersion.CompareTo("8.10.062") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfCaptionExpression"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfCaptionExpression nvarchar(max)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfCaptionExpressionUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfCaptionExpressionUser nvarchar(max)");
			}
		}
		if (fromVersion.CompareTo("9.1.023") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfSaveAsExpression"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfSaveAsExpression nvarchar(max)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfSaveAsExpressionUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfSaveAsExpressionUser nvarchar(max)");
			}
		}
		if (fromVersion.CompareTo("9.1.026") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfGroupParameters"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfGroupParameters nvarchar(max)");
		}
		if (fromVersion.CompareTo("8.10.063") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtEnterInSequenceField"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtEnterInSequenceField nvarchar(30) Not Null Default('')");
		}
		if (fromVersion.CompareTo("8.10.047") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfDBType"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfDBType varchar(20) Not Null Default ''");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfType"))
			{
				ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'char' Where dfType = 'C'");
				ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'varchar' Where dfType = 'V'");
				ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'numeric' Where dfType = 'N'");
				ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'int' Where dfType = 'I'");
				ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'money' Where dfType = 'Y'");
				ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'text' Where dfType = 'M'");
				ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'date' Where dfType = 'D'");
				ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'datetime' Where dfType = 'T'");
				ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'uniqueidentifier' Where dfType = 'U'");
				ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'identity' Where dfType = 'E'");
				ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'image' Where dfType = 'G'");
				ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'boolean' Where dfType = 'L'");
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfType", dropTriggers: false);
			}
		}
		if (fromVersion.CompareTo("8.00.000") < 0)
		{
			if (DoesTableExist(null, databaseName, "DD30FieldMatcher"))
			{
				dmo.DropTable(null, null, databaseName, "DD30FieldMatcher");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfvalid"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfvalid', 'dfValidCode', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfuvalid"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfuvalid', 'dfValidCodeUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfread"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfread', 'dfReadonlyExpression', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfuread"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfuread', 'dfReadonlyExpressionUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfchange"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfchange', 'dfChangeCode', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfuchange"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfuchange', 'dfChangeCodeUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfbpf"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfbpf', 'dfBoundParentField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfbpft"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfbpft', 'dfBoundParentFieldType', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfbpfp"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfbpfp', 'dfBoundParentFieldProxy', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfcurtype"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfcurtype', 'dfCurrencyType', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfcurrf"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfcurrf', 'dfCurrencyRelatedField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfcururf"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfcururf', 'dfCurrencyUpdateRelatedField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfserstpe"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfserstpe', 'dfSerialStatusPositiveExpression', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfserstne"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfserstne', 'dfSerialStatusNegativeExpression', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfsertrtyp"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfsertrtyp', 'dfSerialTransactionType', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfserpbf"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfserpbf', 'dfSerialPartBinField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfsertrdf"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfsertrdf', 'dfSerialTransactionDateField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfserrjf"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfserrjf', 'dfSerialRelatedJobField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfserafpe"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfserafpe', 'dfSerialAvailableFilterPositiveExpression', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfserafne"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfserafne', 'dfSerialAvailableFilterNegativeExpression', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfseramq"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfseramq', 'dfSerialAllowMismatchedQuantity', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfserreqe"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfserreqe', 'dfSerialRequiredExpression', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfserreqeu"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfserreqeu', 'dfSerialRequiredExpressionUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfFKReq"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfFKReq', 'dfForeignKeyRequiredExpression', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfufkreq"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfufkreq', 'dfForeignKeyRequiredExpressionUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dffkvalid"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dffkvalid', 'dfForeignKeyValidCode', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfufkvalid"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfufkvalid', 'dfForeignKeyValidCodeUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfrsgridid"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfrsgridid', 'dfRelatedTableSearchGridId', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfdefault"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfdefault', 'dfDefaultExpression', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfudefault"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfudefault', 'dfDefaultExpressionUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfrequired"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfrequired', 'dfRequiredExpression', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfurequire"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfurequire', 'dfRequiredExpressionUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfcalc"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfcalc', 'dfCalculationExpression', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfcustcap"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfcustcap', 'dfCustomCaption', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfdispname"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfdispname', 'dfDisplayName', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfrfr"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfrfr', 'dfRequiredForeignRelation', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfrsrf"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfrsrf', 'dfRelatedTableReturnField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfrsdf"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfrsdf', 'dfRelatedTabledescriptionField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfrsof"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfrsof', 'dfRelatedTableOrderByField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfsfilter"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfsfilter', 'dfRelatedTableFilter', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfRelField"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfRelField', 'dfRelatedFields', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfreltable"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfreltable', 'dfRelatedTable', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfreltype"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfreltype', 'dfRelationType', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfrsel"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDFields.dfrsel', 'dfValueList', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtCurMLF"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtCurMLF', 'dtCurrencyModeLocationField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtcurrif"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtcurrif', 'dtCurrencyRateIdField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtCurCRF"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtCurCRF', 'dtCurrencyCustomRateField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtcurerf"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtcurerf', 'dtCurrencyExchangeRateField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtcurdf"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtcurdf', 'dtDocumentDateField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtPromptOA"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtPromptOA', 'dtPromptOnAddField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtfcou"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtfcou', 'dtFieldToCheckOnUpdate', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtuddel"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtuddel', 'dtDisableDeleteExpressionUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtddel"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtddel', 'dtDisableDeleteExpression', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtudnew"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtudnew', 'dtDisableAddNewExpressionUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtdnew"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtdnew', 'dtDisableAddNewExpression', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dturead"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dturead', 'dtReadonlyExpressionUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtread"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtread', 'dtReadonlyExpression', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtcolor"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtcolor', 'dtColorExpression', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtucolor"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtucolor', 'dtColorExpressionUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtkeycbe"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtkeycbe', 'dtLastKeyCanBeEmpty', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dttriggers"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dttriggers', 'dtTriggersCode', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtutrigger"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtutrigger', 'dtTriggersCodeUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtptable"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtptable', 'dtParentTable', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtdispname"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtdispname', 'dtDisplayName', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtkey"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtkey', 'dtKeyFields', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtinitval"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtinitval', 'dtInitialValue', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtfilter"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtfilter', 'dtChangeDetailIdsFilter', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtfkdfilt"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtfkdfilt', 'dtForeignKeyDeleteFilter', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtmailmerg"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtmailmerg', 'dtMailMerge', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtuniqfld"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtuniqfld', 'dtUniqueField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtautoinc"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtautoinc', 'dtAutoIncrement', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtuautoinc"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtuautoinc', 'dtAutoIncrementUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtincamt"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtincamt', 'dtIncrementAmount', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtuincamt"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtuincamt', 'dtIncrementAmountUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtuprefix"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtuprefix', 'dtPrefixUser', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtnumonly"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtnumonly', 'dtNumericOnly', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtcontact"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtcontact', 'dtContactField', 'COLUMN'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtdefobj"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDTables.dtdefobj', 'dtDefaultObjectId', 'COLUMN'");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfRelatedTableSearchGridId"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfRelatedTableSearchGridId varchar(35) NOT NULL DEFAULT('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfShowAsDropdown"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfShowAsDropdown bit NOT NULL DEFAULT(0)");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfselt"))
			{
				ExecuteCommand(databaseName, "Update DDFields Set dfRelatedTableSearchGridId = Convert(nvarchar(35),IsNull(dfValueList,'')) Where dfselt = 4 And dfcustom <> 0");
				ExecuteCommand(databaseName, "Update DDFields Set dfValueList = '' Where dfselt = 4 And dfcustom <> 0");
				ExecuteCommand(databaseName, "Update DDFields Set dfShowAsDropDown = 1 Where dfselt <> 0 And dfselt <> 3 And dfcustom <> 0 And RTrim(dfRelatedTableReturnField) <> '' And RTrim(dfRelatedTableDescriptionField) <> ''");
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfselt", dropTriggers: false);
			}
			ExecuteCommand(databaseName, "Update DDObjectDetails Set dlSequence = Case When dlSequence < 0 Then 0 Else Case When dlSequence > 255 Then 255 Else dlSequence End End");
			ExecuteCommand(databaseName, "Update DDObjectDetails Set dlLine = Case When dlLine < 0 Then 0 Else Case When dlLine > 255 Then 255 Else dlLine End End");
			ExecuteCommand(databaseName, "Update DDObjectDetails Set dlLevel = Case When dlLevel < 0 Then 0 Else Case When dlLevel > 255 Then 255 Else dlLevel End End");
			ExecuteCommand(databaseName, "Update DDObjectDetails Set dlCollapse = Case When dlCollapse < 0 Then 0 Else Case When dlCollapse > 255 Then 255 Else dlCollapse End End");
			ExecuteCommand(databaseName, "Update DDObjectDetails Set dlDHide = Case When dlDHide < 0 Then 0 Else Case When dlDHide > 255 Then 255 Else dlDHide End End");
			ExecuteCommand(databaseName, "Update DDObjectDetails Set dlUHide = Case When dlUHide < 0 Then 0 Else Case When dlUHide > 255 Then 255 Else dlUHide End End");
			ExecuteCommand(databaseName, "Update DDObjectDetails Set dlrlf = Case When dlrlf < 0 Then 0 Else Case When dlrlf > 255 Then 255 Else dlrlf End End");
			ExecuteCommand(databaseName, "Update DDObjectDetails Set dlROnI = Case When dlROnI < 0 Then 0 Else Case When dlROnI > 255 Then 255 Else dlROnI End End");
			ExecuteCommand(databaseName, "Update DDFields Set dfLength = Case When dfLength < 0 Then 0 Else Case When dfLength > 255 Then 255 Else dfLength End End");
			ExecuteCommand(databaseName, "Update DDFields Set dfDecimals = Case When dfDecimals < 0 Then 0 Else Case When dfDecimals > 255 Then 255 Else dfDecimals End End");
			ExecuteCommand(databaseName, "Update DDFields Set dfUDPr = Case When dfUDPr < 0 Then 0 Else Case When dfUDPr > 255 Then 255 Else dfUDPr End End");
			ExecuteCommand(databaseName, "Update DDOpenWiths Set dwType = Case When dwType < 0 Then 0 Else Case When dwType > 255 Then 255 Else dwType End End");
			ExecuteCommand(databaseName, "Update DDOpenWiths Set dwSequence = Case When dwSequence < 0 Then 0 Else Case When dwSequence > 255 Then 255 Else dwSequence End End");
			ExecuteCommand(databaseName, "Update DDSearches Set dsWindowState = Case When dsWindowState < 0 Then 0 Else Case When dsWindowState > 255 Then 255 Else dsWindowState End End");
			ExecuteCommand(databaseName, "Update DDSearches Set dsMonitor = Case When dsMonitor < 0 Then 0 Else Case When dsMonitor > 255 Then 255 Else dsMonitor End End");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxType = Case When dxType < 0 Then 0 Else Case When dxType > 255 Then 255 Else dxType End End");
			ExecuteCommand(databaseName, "Update DDFieldUserSettings Set daPrevious = Case When daPrevious < 0 Then 0 Else Case When daPrevious > 255 Then 255 Else daPrevious End End");
			ExecuteCommand(databaseName, "Update DDFormDetails Set deType = Case When deType < 0 Then 0 Else Case When deType > 255 Then 255 Else deType End End");
			ExecuteCommand(databaseName, "Update DDForms Set dmType = Case When dmType < 0 Then 0 Else Case When dmType > 255 Then 255 Else dmType End End");
			ExecuteCommand(databaseName, "Update DDForms Set dmFormType = Case When dmFormType < 0 Then 0 Else Case When dmFormType > 255 Then 255 Else dmFormType End End");
			ExecuteCommand(databaseName, "Update DDGridDetails Set dgFreeze = Case When dgFreeze < 0 Then 0 Else Case When dgFreeze > 255 Then 255 Else dgFreeze End End");
			ExecuteCommand(databaseName, "Update DDGridDetails Set dgWebGrid = Case When dgWebGrid < 0 Then 0 Else Case When dgWebGrid > 255 Then 255 Else dgWebGrid End End");
			ExecuteCommand(databaseName, "Update DDGridDetails Set dgWebSeq = Case When dgWebSeq < 0 Then 0 Else Case When dgWebSeq > 255 Then 255 Else dgWebSeq End End");
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgCalNum1"))
			{
				ExecuteCommand(databaseName, "Update DDGridDetails Set dgCalNum1 = Case When dgCalNum1 < 0 Then 0 Else Case When dgCalNum1 > 255 Then 255 Else dgCalNum1 End End");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgCalNum2"))
			{
				ExecuteCommand(databaseName, "Update DDGridDetails Set dgCalNum2 = Case When dgCalNum2 < 0 Then 0 Else Case When dgCalNum2 > 255 Then 255 Else dgCalNum2 End End");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgBarBuck"))
			{
				ExecuteCommand(databaseName, "Update DDGridDetails Set dgBarBuck = Case When dgBarBuck < 0 Then 0 Else Case When dgBarBuck > 255 Then 255 Else dgBarBuck End End");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgBarBSize"))
			{
				ExecuteCommand(databaseName, "Update DDGridDetails Set dgBarBSize = Case When dgBarBSize < 0 Then 0 Else Case When dgBarBSize > 255 Then 255 Else dgBarBSize End End");
			}
			ExecuteCommand(databaseName, "Update DDTables Set dtNumericOnly = 2 Where dtNumericOnly < 0");
			ExecuteCommand(databaseName, "Update DDTables Set dtNumericOnly = Case When dtNumericOnly < 0 Then 0 Else Case When dtNumericOnly > 255 Then 255 Else dtNumericOnly End End");
			ExecuteCommand(databaseName, "Update DDTables Set dtAutoIncrementUser = Case When dtAutoIncrementUser < 0 Then 0 Else Case When dtAutoIncrementUser > 255 Then 255 Else dtAutoIncrementUser End End");
			ExecuteCommand(databaseName, "Update DDSecurityReports Set drLevel = Case When drLevel < 0 Then 0 Else Case When drLevel > 255 Then 255 Else drLevel End End");
			ExecuteCommand(databaseName, "Update DDSecurityTables Set dtLevel = Case When dtLevel < 0 Then 0 Else Case When dtLevel > 255 Then 255 Else dtLevel End End");
			ExecuteCommand(databaseName, "Update DDUserLog Set ulMessageType = Case When ulMessageType < 0 Then 0 Else Case When ulMessageType > 255 Then 255 Else ulMessageType End End");
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtReadonlyExpressionUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtReadonlyExpressionUser text NULL");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtdisableaddnewexpressionuser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtDisableAddNewExpressionUser text NULL");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtdisabledeleteexpressionuser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtDisableDeleteExpressionUser text NULL");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfreadonlyexpressionuser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfReadonlyExpressionUser text NULL");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfrequiredexpressionuser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfRequiredExpressionUser text NULL");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfureq"))
			{
				ExecuteCommand(databaseName, "Update DDFields Set dfRequiredExpressionUser = 'True' Where dfureq <> 0");
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfureq", dropTriggers: false);
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfvalidcode"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfValidCode text NULL");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfvalidcodeuser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfValidCodeUser text NULL");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfforeignkeyvalidcode"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfForeignKeyValidCode text NULL");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfforeignkeyvalidcodeuser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfForeignKeyValidCodeUser text NULL");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfserialrequiredexpressionuser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfSerialRequiredExpressionUser text NULL");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfforeignkeyrequiredexpressionuser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfForeignKeyRequiredExpressionUser text NULL");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfShowAsDropdownUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfShowAsDropdownUser bit NULL");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfAllowNulls"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfAllowNulls bit Not Null Default 0");
				ExecuteCommand(databaseName, "Update DDFields Set dfAllowNulls = 1 Where dfDBType = 'datetime' Or dfDBType = 'date' Or dfDBType = 'image'");
			}
		}
		if (fromVersion.CompareTo("8.10.048") < 0)
		{
			ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'nvarchar' Where dfDBType= 'varchar'");
			ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'nvarchar' Where dfDBType= 'char'");
			ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'nvarchar(max)', dfAllowNulls = 1 Where dfDBType= 'text'");
			ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'nvarchar(max)', dfAllowNulls = 1 Where dfDBType= 'ntext'");
			ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'bit' Where dfDBType= 'boolean'");
			ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'tinyint' Where dfDBType= 'numeric' And dfLength <= 2 And dfDecimals = 0");
			ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'smallint' Where dfDBType= 'numeric' And dfLength <= 4 And dfDecimals = 0");
			ExecuteCommand(databaseName, "Update DDFields Set dfDBType = 'int' Where dfDBType= 'numeric' And dfLength <= 9 And dfDecimals = 0");
		}
		if (fromVersion.CompareTo("8.00.094") < 0 && !DoesTableExist(null, databaseName, "DDScripts"))
		{
			ExecuteCommand(databaseName, "CREATE TABLE dbo.DDScripts (dyName varchar(75) NOT NULL DEFAULT(''), dyCode text NULL, dyCustom bit Not Null Default(0))");
			ExecuteCommand(databaseName, "CREATE UNIQUE INDEX dyName ON DDScripts (dyName)");
		}
		if (fromVersion.CompareTo("8.00.166") < 0)
		{
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "deCode"))
			{
				DataTable dataTable = GetDataTable(databaseName, "Select Count(*) as Rec_Count From DDScripts Where dyName = 'APP'");
				if (dataTable.Rows.Count == 0 || Convert.ToInt32(dataTable.Rows[0]["Rec_Count"]) <= 0)
				{
					ExecuteCommand(databaseName, "Insert Into DDScripts (dyName, dyCode, dyCustom) Select deFormID, Case When LTrim(IsNull(Convert(nvarchar(100),deCode),'')) = '' Then Null Else deCode End, 1 From DDFormDetails Where deFormID = 'APP' And deControlName = 'APP' And deClassID = 'APP'");
				}
			}
			ExecuteCommand(databaseName, "Delete From DDFormDetails Where deFormID = 'APP' And deControlName = 'APP' And deClassID = 'APP'");
		}
		if (fromVersion.CompareTo("8.10.033") < 0)
		{
			if (!DoesTableExist(null, databaseName, "DDScripts") && DoesTableExist(null, databaseName, "DDCode"))
			{
				ExecuteCommand(databaseName, "sp_rename 'DDCode', 'DDScripts'");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDScripts", "dyUniqueID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDScripts ADD dyUniqueID uniqueidentifier Not Null Default(NEWID())");
				ExecuteCommand(databaseName, "CREATE Unique INDEX dyUniqueID ON DDScripts (dyUniqueID)");
			}
		}
		if (fromVersion.CompareTo("8.10.024") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDForms", "dmCode"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDForms ADD dmCode varchar(max) Null");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDForms", "dmCodeUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDForms ADD dmCodeUser varchar(max) Null");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "deCode"))
			{
				DataTable dataTable2 = GetDataTable(databaseName, "Select deFormID From DDFormDetails Where LTrim(IsNull(Convert(nvarchar(100),deCode),'')) <> '' Group By deFormID");
				if (dataTable2.Rows.Count != 0)
				{
					SqlDataAdapter adapter = new SqlDataAdapter();
					foreach (DataRow row in dataTable2.Rows)
					{
						string text = row.Field<string>("deFormID").Trim();
						DataTable dataTable3 = GetDataTable(databaseName, "Select deType,deControlName,deClassID,deCode From DDFormDetails Where deFormID = " + text.ToSql() + " And Not deCode Is Null");
						StringBuilder stringBuilder = CombineCodeForAllRows(dataTable3);
						if (stringBuilder != null && stringBuilder.Length != 0)
						{
							DataTable dataTable4 = GetDataTable(null, databaseName, "Select dmFormID,dmCustom,dmFormType,dmCodeUser From DDForms Where dmFormID = " + text.ToSql(), fillSchema: true, out adapter);
							if (dataTable4.Rows.Count == 0)
							{
								DataRow dataRow = dataTable4.NewRow();
								dataRow.SetField("dmFormID", text);
								dataRow["dmFormType"] = 1;
								dataRow["dmCustom"] = 1;
								dataTable4.Rows.Add(dataRow);
							}
							dataTable4.Rows[0].SetField("dmCodeUser", stringBuilder.ToString());
							UpdateData(null, null, databaseName, dataTable4, adapter, null);
						}
					}
				}
				dmo.DropColumn(null, null, null, databaseName, "DDFormDetails", "deCode", dropTriggers: false);
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDForms", "dmCompiled"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDForms ADD dmCompiled varbinary(max) Null");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDForms", "dmRunType"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDForms ADD dmRunType tinyint Not Null Default(0)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDForms", "dmIsChanged"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDForms ADD dmIsChanged bit Not Null Default(0)");
				ExecuteCommand(databaseName, $"Update DDForms Set dmIsChanged = Case When sub.subCount > 0 Then 1 Else 0 End From DDForms Inner Join (Select deFormID,Count(*) As subcount From DDFormDetails Group By deFormID) As sub On dmFormID = deFormID COLLATE {collation}");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDForms", "dmNeedToCompile"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDForms ADD dmNeedToCompile bit Not Null Default(0)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "deCustom"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFormDetails ADD deCustom bit Not Null Default(0)");
				ExecuteCommand(databaseName, "Update DDFormDetails Set deCustom = 1");
			}
		}
		if (fromVersion.CompareTo("8.10.075") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDForms", "dmAllInDD"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDForms ADD dmAllInDD bit NOT NULL DEFAULT(0)");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "deType"))
			{
				ExecuteCommand(databaseName, $"Update DDFormDetails Set deCustom = Case When deType = 2 Then 1 Else Case When IsNull(dmCustom,0) <> 0 Then 1 Else 0 End End From DDFormDetails Left Outer Join DDForms on deFormID=dmFormID COLLATE {collation}");
				dmo.DropColumn(null, null, null, databaseName, "DDFormDetails", "deType", dropTriggers: false);
			}
		}
		if (fromVersion.CompareTo("8.10.033") < 0)
		{
			if (!DoesTableExist(null, databaseName, "DDCode"))
			{
				CreateDataDictionaryTables(null, databaseName, "DDCode", string.Empty, ddDef);
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDForms", "dmUniqueID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDForms ADD dmUniqueID uniqueidentifier Not Null Default(NEWID())");
				ExecuteCommand(databaseName, "CREATE Unique INDEX dmUniqueID ON DDForms (dmUniqueID)");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDForms", "dmCode"))
			{
				ExecuteCommand(databaseName, "Insert Into DDCode (dkCodeID, dkSourceTable, dkSourceUniqueID, dkCode, dkCustom) Select NewID(), 'DDForms', dmUniqueID, dmCode, dmCustom From DDForms Where Not dmCode Is Null");
				dmo.DropColumn(null, null, null, databaseName, "DDForms", "dmCode", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDForms", "dmCodeUser"))
			{
				ExecuteCommand(databaseName, "Insert Into DDCode (dkCodeID, dkSourceTable, dkSourceUniqueID, dkCode, dkCustom) Select NewID(), 'DDForms', dmUniqueID, dmCodeUser, 1 From DDForms Where Not dmCodeUser Is Null");
				dmo.DropColumn(null, null, null, databaseName, "DDForms", "dmCodeUser", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDScripts", "dyCode"))
			{
				ExecuteCommand(databaseName, "Insert Into DDCode (dkCodeID, dkSourceTable, dkSourceUniqueID, dkCode, dkCustom) Select NewID(), 'DDScripts', dyUniqueID, dyCode, 1 From DDScripts Where Not dyCode Is Null");
				dmo.DropColumn(null, null, null, databaseName, "DDScripts", "dyCode", dropTriggers: false);
			}
			if (dmo.DoesTableExist(null, null, databaseName, "DDFormCodeTemp"))
			{
				dmo.DropTable(null, null, databaseName, "DDFormCodeTemp");
			}
			ExecuteCommand(databaseName, "Select Convert(nvarchar(100),Replace(dmFormID, 'M1.VIEW', 'VIEW')) As dmFormID, dkCodeID Into DDFormCodeTemp From DDCode Inner Join DDForms On dkSourceUniqueID = dmUniqueID Where dkSourceTable = 'DDForms' And dmCustom = 0");
		}
		if (fromVersion.CompareTo("8.10.026") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "dePropertiesUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFormDetails ADD dePropertiesUser varchar(max) Null");
				if (dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "deProperties"))
				{
					ExecuteCommand(databaseName, "Update DDFormDetails Set dePropertiesUser = Case When LTrim(IsNull(Convert(nvarchar(100),deProperties),'')) = '' Then Null Else deProperties End ");
					dmo.DropColumn(null, null, null, databaseName, "DDFormDetails", "deProperties", dropTriggers: false);
				}
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "deProperties"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFormDetails ADD deProperties varchar(max) Null");
			}
		}
		if (fromVersion.CompareTo("8.10.027") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "deParentID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFormDetails ADD deParentID varchar(50) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "deNestedName"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFormDetails ADD deNestedName varchar(50) Not Null Default('')");
			}
		}
		if (fromVersion.CompareTo("8.10.046") < 0)
		{
			if (DoesTableExist(null, databaseName, "DDFieldExtensions"))
			{
				dmo.DropTable(null, null, databaseName, "DDFieldExtensions");
			}
			if (DoesTableExist(null, databaseName, "DDFieldExtensionTypes"))
			{
				dmo.DropTable(null, null, databaseName, "DDFieldExtensionTypes");
			}
			if (!DoesTableExist(null, databaseName, "DDFieldExtensions"))
			{
				CreateDataDictionaryTables(null, databaseName, "DDFieldExtensions", string.Empty, ddDef);
			}
			if (!DoesTableExist(null, databaseName, "DDFieldExtensionTypes"))
			{
				CreateDataDictionaryTables(null, databaseName, "DDFieldExtensionTypes", string.Empty, ddDef);
			}
		}
		if (fromVersion.CompareTo("9.00.005") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFieldExtensions", "dqParameters"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFieldExtensions ADD dqParameters nvarchar(100) Default(NULL)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFieldExtensions", "dqSource"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFieldExtensions ADD dqSource tinyint Not Null Default(0)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFieldExtensions", "dqReverseSign"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFieldExtensions ADD dqReverseSign bit Not Null Default(0)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFieldExtensions", "dqRelatedJobStatusField"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFieldExtensions ADD dqRelatedJobStatusField nvarchar(30) Not Null Default('')");
			}
		}
		if (fromVersion.CompareTo("9.00.051") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFieldExtensions", "dqStatusPositive"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFieldExtensions ADD dqStatusPositive tinyint Not Null Default(0)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFieldExtensions", "dqStatusNegative"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFieldExtensions ADD dqStatusNegative tinyint Not Null Default(0)");
			}
		}
		if (fromVersion.CompareTo("9.00.059") < 0)
		{
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFieldExtensions", "dqStatusPositiveExpression"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFieldExtensions", "dqStatusPositiveExpression", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFieldExtensions", "dqStatusNegativeExpression"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFieldExtensions", "dqStatusNegativeExpression", dropTriggers: false);
			}
		}
		if (fromVersion.CompareTo("9.00.067") < 0)
		{
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFieldExtensions", "dqExtension"))
			{
				dmo.RenameColumnEx(null, null, null, databaseName, "DDFieldExtensions", "dqExtension", "dqFieldExtensionID", dropTriggers: false);
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFieldExtensions", "dqFieldExtensionTypeID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFieldExtensions ADD dqFieldExtensionTypeID nvarchar(10) Not Null Default('')");
				ExecuteCommand(databaseName, "Update DDFieldExtensions Set dqFieldExtensionTypeID = dqFieldExtensionID");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFieldExtensionTypes", "dhExtension"))
			{
				dmo.RenameColumnEx(null, null, null, databaseName, "DDFieldExtensionTypes", "dhExtension", "dhFieldExtensionTypeID", dropTriggers: false);
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFieldExtensionTypes", "dhAllowMultiple"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFieldExtensionTypes ADD dhAllowMultiple bit Not Null Default(0)");
			}
		}
		if (fromVersion.CompareTo("8.10.042") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDObjects", "doTreeLoader"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDObjects ADD doTreeLoader varchar(max) Null");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDObjects", "doTreeLoaderUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDObjects ADD doTreeLoaderUser varchar(max) Null");
			}
		}
		if (fromVersion.CompareTo("8.10.044") < 0 && !DoesTableExist(null, databaseName, "DDFieldGroups"))
		{
			CreateDataDictionaryTables(null, databaseName, "DDFieldGroups", string.Empty, ddDef);
		}
		if (fromVersion.CompareTo("8.10.055") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtQuickSearchOption"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtQuickSearchOption tinyint Not Null Default(0)");
		}
		if (fromVersion.CompareTo("8.10.030") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtClosedField"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtClosedField varchar(30) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtClosedValue"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtClosedValue varchar(10) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtClosedDateField"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtClosedDateField varchar(30) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtClosedExtraSetExpression"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtClosedExtraSetExpression varchar(max) Null");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtClosedIncludeOptionText"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtClosedIncludeOptionText varchar(50) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtClosedIncludeOptionSqlExpr"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtClosedIncludeOptionSqlExpr varchar(max) Null");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtClosedCutoffDateField"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtClosedCutoffDateField varchar(30) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtClosedRoleCheck"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtClosedRoleCheck varchar(30) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtClosedHelpLink"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtClosedHelpLink varchar(75) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtPurgeCutoffDateField"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtPurgeCutoffDateField varchar(30) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtPurgeHelpLink"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtPurgeHelpLink varchar(75) Not Null Default('')");
			}
		}
		if (fromVersion.CompareTo("8.10.023") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfBoundParentFieldExpression"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfBoundParentFieldExpression text Null");
		}
		if (fromVersion.CompareTo("8.10.017") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfSequenceUser"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfSequenceUser smallint Not Null Default(0)");
		}
		if (fromVersion.CompareTo("8.10.015") < 0 && dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgTotals"))
		{
			dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgTotals", dropTriggers: false);
		}
		if (fromVersion.CompareTo("8.10.009") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgCustom"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDGridDetails ADD dgCustom bit Not Null Default(1)");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgDef"))
			{
				ExecuteCommand(databaseName, "Update DDGridDetails Set dgCustom = Case When dgDef = 0 Then 1 Else 0 End");
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgDef", dropTriggers: false);
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDGrids", "djCustom"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDGrids ADD djCustom bit Not Null Default(1)");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGrids", "djDefault"))
			{
				ExecuteCommand(databaseName, "Update DDGrids Set djCustom = Case When djDefault = 0 Then 1 Else 0 End");
				dmo.DropColumn(null, null, null, databaseName, "DDGrids", "djDefault", dropTriggers: false);
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "ddUsers", "duCustom"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDUsers ADD duCustom bit NOT NULL DEFAULT(1)");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "ddUsers", "duDefault"))
			{
				ExecuteCommand(databaseName, "Update DDUsers Set duCustom = Case When duDefault = 0 Then 1 Else 0 End");
				dmo.DropColumn(null, null, null, databaseName, "DDUsers", "duDefault", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgFTyp"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgFTyp", dropTriggers: false);
			}
		}
		if (fromVersion.CompareTo("8.10.057") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDGrids", "djSPGroup"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDGrids ADD djSPGroup nvarchar(10) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDGrids", "djSPSequence"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDGrids ADD djSPSequence smallint Not Null Default(0)");
			}
		}
		if (fromVersion.CompareTo("8.10.035") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDOpenWiths", "dwExtension"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDOpenWiths ADD dwExtension varchar(10) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDOpenWiths", "dwButtonImage"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDOpenWiths ADD dwButtonImage varchar(50) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDOpenWiths", "dwButtonImageUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDOpenWiths ADD dwButtonImageUser varchar(50) Not Null Default('')");
			}
		}
		if (fromVersion.CompareTo("8.10.007") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDOpenWiths", "dwBindReadOnly"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDOpenWiths ADD dwBindReadOnly bit Not Null Default(0)");
		}
		if (fromVersion.CompareTo("8.10.001") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDOpenWiths", "dwSaveBefore"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDOpenWiths ADD dwSaveBefore bit Not Null Default(0)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDOpenWiths", "dwPromptField"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDOpenWiths ADD dwPromptField varchar(30) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDOpenWiths", "dwActionName"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDOpenWiths ADD dwActionName varchar(100) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDOpenWiths", "dwEnabledExpression"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDOpenWiths ADD dwEnabledExpression text NULL");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDOpenWiths", "dwEnabledExpressionUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDOpenWiths ADD dwEnabledExpressionUser text NULL");
			}
		}
		if (fromVersion.CompareTo("8.00.288") < 0)
		{
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtDisableDeleteExpression"))
			{
				ExecuteCommand(databaseName, "UPDATE DDTables SET dtDisableDeleteExpression=NULL WHERE dtTable = 'Reasons'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfBoundParentField"))
			{
				ExecuteCommand(databaseName, "UPDATE DDFields SET dfBoundParentField='rarNonConformanceCategoryID' WHERE dfTable = 'RMAClaimProblems' AND dfField= 'rarNonConformanceCodeID'");
				ExecuteCommand(databaseName, "UPDATE DDFields SET dfBoundParentField='rarCorrectiveActionCategoryID' WHERE dfTable = 'RMAClaimProblems' AND dfField='rarCorrectiveActionCodeID'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfReadonlyExpression"))
			{
				ExecuteCommand(databaseName, "UPDATE DDFields SET dfReadonlyExpression='CInt(Fields(\"qalInspectionComplete\").Value) = True' WHERE dfTable = 'InspectionLines' AND dfField= 'qalTransferredToDMR'");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDRelations", "drSaveAs"))
			{
				ExecuteCommand(databaseName, "UPDATE DDRelations SET drSaveAs=1 WHERE drPTable = 'RMACLAIMLINES' AND drCTable = 'RMAClaimProblems'");
			}
		}
		if (fromVersion.CompareTo("8.00.254") < 0)
		{
			string arg = "ALTER TABLE DDFields ADD ";
			string text2 = "dfButtonLabel";
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", text2))
			{
				ExecuteCommand(databaseName, $"{arg} {text2} varchar(25) NULL");
			}
		}
		if (fromVersion.CompareTo("8.00.219") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtCurrencyUpdateType"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtCurrencyUpdateType tinyint Not Null Default(0)");
		}
		if (fromVersion.CompareTo("8.00.214") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtKeysAtThisLevel"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtKeysAtThisLevel tinyint Not Null Default(0)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtEmptyKeyCanBeEdited"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtEmptyKeyCanBeEdited bit Not Null Default(0)");
			}
		}
		if (fromVersion.CompareTo("8.00.210") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfButtonToolTip"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfButtonToolTip varchar(50) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfButtonToolTipUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfButtonToolTipUser varchar(50) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfButtonImage"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfButtonImage varchar(50) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfButtonImageUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfButtonImageUser varchar(50) Not Null Default('')");
			}
		}
		if (fromVersion.CompareTo("8.00.207") < 0 && dmo.DoesFieldExist(null, null, databaseName, "DDObjectDetails", "dlView"))
		{
			ExecuteCommand(databaseName, "Exec sp_rename 'DDObjectDetails.dlView', 'dlViewTemp', 'COLUMN'");
			ExecuteCommand(databaseName, "Alter Table DDObjectDetails Add dlView varchar(100) Not Null Default('')");
			ExecuteCommand(databaseName, "Update DDObjectDetails Set dlView = dlViewTemp");
			dmo.DropColumn(null, null, null, databaseName, "DDObjectDetails", "dlViewTemp", dropTriggers: false);
		}
		if (fromVersion.CompareTo("8.00.146") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDForms", "dmAssemblies"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDForms Add dmAssemblies text");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDObjectDetails", "dlCField"))
			{
				ExecuteCommand(databaseName, "Exec sp_rename 'DDObjectDetails.dlCField', 'dlCFieldTemp', 'COLUMN'");
				ExecuteCommand(databaseName, "Alter Table DDObjectDetails Add dlCField varchar(90) Not Null Default('')");
				ExecuteCommand(databaseName, "Update DDObjectDetails Set dlCField = dlCFieldTemp");
				dmo.DropColumn(null, null, null, databaseName, "DDObjectDetails", "dlCFieldTemp", dropTriggers: false);
			}
		}
		if (fromVersion.CompareTo("8.00.061") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDRelations", "drRelationID"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDRelations Add drRelationID uniqueIdentifier Not Null Default(NewID())");
			ExecuteCommand(databaseName, "Update DDRelations Set drRelationID = NewID()");
			ExecuteCommand(databaseName, "CREATE UNIQUE INDEX drRelationID ON DDRELATIONS (drRelationID)");
		}
		if (fromVersion.CompareTo("8.00.016") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfButtonCode"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfButtonCode text NULL");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfButtonCodeUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfButtonCodeUser text NULL");
			}
		}
		if (fromVersion.CompareTo("8.00.124") < 0)
		{
			if (DoesTableExist(null, databaseName, "DDVisualizers"))
			{
				ExecuteCommand(databaseName, "Update DDVisualizers Set dvCustom = 1 Where dvUserID <> ''");
				dmo.DropTable(null, null, databaseName, "DDVisualizers");
			}
			if (DoesTableExist(null, databaseName, "DDSeries"))
			{
				ExecuteCommand(databaseName, "Update DDSeries Set diCustom = 1 Where diUserID <> ''");
				dmo.DropTable(null, null, databaseName, "DDSeries");
			}
			if (!DoesTableExist(null, databaseName, "DDVisualizers"))
			{
				CreateDataDictionaryTables(null, databaseName, "DDVisualizers", string.Empty, ddDef);
			}
			if (!DoesTableExist(null, databaseName, "DDSeries"))
			{
				CreateDataDictionaryTables(null, databaseName, "DDSeries", string.Empty, ddDef);
			}
		}
		if (fromVersion.CompareTo("8.00.125") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgTreeVisible"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDGridDetails Add dgTreeVisible bit Not Null Default(0)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgTreeWidth"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDGridDetails Add dgTreeWidth int Not Null Default(0)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgTreeSettings"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDGridDetails Add dgTreeSettings text");
			}
		}
		if (fromVersion.CompareTo("8.00.134") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDSeries", "diNegative"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDSeries Add diNegative bit Not Null Default(0)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDSeries", "diTotal"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDSeries Add diTotal bit Not Null Default(1)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDSeries", "diExpanded"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDSeries Add diExpanded bit Not Null Default(1)");
			}
		}
		if (fromVersion.CompareTo("8.00.137") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDVisualizers", "dvMinimumPercent"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDVisualizers Add dvMinimumPercent numeric(5,2) Not Null Default(5)");
		}
		if (fromVersion.CompareTo("8.00.156") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtShowMemos"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables Add dtShowMemos bit Not Null Default 0");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtShowMemosUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables Add dtShowMemosUser bit");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtMemoDescription"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables Add dtMemoDescription varchar(30) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtMemoDescriptionUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables Add dtMemoDescriptionUser varchar(30) Not Null Default('')");
			}
		}
		if (fromVersion.CompareTo("8.00.191") < 0)
		{
			if (!DoesTableExist(null, databaseName, "DDObjectsUser"))
			{
				CreateDataDictionaryTables(null, databaseName, "DDObjectsUser", string.Empty, ddDef);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDObjects", "doOther"))
			{
				if (dmo.DoesFieldExist(null, null, databaseName, "DDUsers", "duType"))
				{
					ExecuteCommand(databaseName, $"Insert Into DDObjectsUser (doObjectID, doUserID, doOther) Select doObjectID, duUserID, Case When LTrim(IsNull(Convert(nvarchar(100),doOther),'')) = '' Then Null Else doOther End As doOther From DDObjects, DDUsers Where LTrim(IsNull(Convert(nvarchar(100),doOther),'')) <> '' And duType = 0 And Not duUserID In (Select doUserID COLLATE {collation} From DDObjectsUser)");
				}
				else
				{
					ExecuteCommand(databaseName, $"Insert Into DDObjectsUser (doObjectID, doUserID, doOther) Select doObjectID, duUserID, Case When LTrim(IsNull(Convert(nvarchar(100),doOther),'')) = '' Then Null Else doOther End As doOther From DDObjects, DDUsers Where LTrim(IsNull(Convert(nvarchar(100),doOther),'')) <> '' And Not duUserID In (Select doUserID COLLATE {collation} From DDObjectsUser)");
				}
				dmo.DropColumn(null, null, null, databaseName, "DDObjects", "doOther", dropTriggers: false);
			}
			if (!DoesTableExist(null, databaseName, "DDObjectDetailsUser"))
			{
				CreateDataDictionaryTables(null, databaseName, "DDObjectDetailsUser", string.Empty, ddDef);
			}
		}
		if (fromVersion.CompareTo("8.00.214") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfvisibleexpression"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfVisibleExpression text NULL");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfvisibleexpressionuser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfVisibleExpressionUser text NULL");
			}
		}
		if (fromVersion.CompareTo("8.10.012") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtUniqueID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtUniqueID uniqueidentifier Not Null Default(NEWID())");
				ExecuteCommand(databaseName, "CREATE Unique INDEX dtUniqueID ON DDTables (dtUniqueID)");
			}
			if (dmo.DoesTableExist(null, null, databaseName, "DDTablesTemp"))
			{
				dmo.DropTable(null, null, databaseName, "DDTablesTemp");
			}
			if (dmo.DoesTableExist(null, null, databaseName, "DDTablesTemp"))
			{
				ExecuteCommand(databaseName, "Drop Table DDTablesTemp");
			}
			ExecuteCommand(databaseName, "sp_rename 'DDTables', 'DDTablesTemp'");
			try
			{
				CreateDataDictionaryTables(null, databaseName, "DDTables", string.Empty, ddDef);
				try
				{
					ExecuteCommand(databaseName, $"Update DDTablesTemp Set dtUniqueID = b.dtUniqueID From DDTablesTemp a Inner Join DDTables b On a.dtTable = b.dtTable COLLATE {collation}");
				}
				finally
				{
					ExecuteCommand(databaseName, "Drop Table DDTables");
				}
			}
			finally
			{
				ExecuteCommand(databaseName, "sp_rename 'DDTablesTemp', 'DDTables'");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfUniqueID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfUniqueID uniqueidentifier Not Null Default(NEWID())");
				ExecuteCommand(databaseName, "CREATE Unique INDEX dfUniqueID ON DDFields (dfUniqueID)");
			}
			if (dmo.DoesTableExist(null, null, databaseName, "DDFieldsTemp"))
			{
				ExecuteCommand(databaseName, "Drop Table DDFieldsTemp");
			}
			ExecuteCommand(databaseName, "sp_rename 'DDFields', 'DDFieldsTemp'");
			try
			{
				CreateDataDictionaryTables(null, databaseName, "DDFields", string.Empty, ddDef);
				try
				{
					ExecuteCommand(databaseName, $"Update DDFieldsTemp Set dfUniqueID = b.dfUniqueID From DDFieldsTemp a Inner Join DDFields b On a.dfField = b.dfField COLLATE {collation}");
				}
				finally
				{
					ExecuteCommand(databaseName, "Drop Table DDFields");
				}
			}
			finally
			{
				ExecuteCommand(databaseName, "sp_rename 'DDFieldsTemp', 'DDFields'");
			}
		}
		if (fromVersion.CompareTo("8.10.016") < 0 && dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtTriggersCode") && dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtTriggersCodeUser"))
		{
			convertTriggerNames(databaseName);
		}
		if (fromVersion.CompareTo("8.10.034") < 0 && dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtTriggersCode") && dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtTriggersCodeUser"))
		{
			convertTableAndFieldCode(databaseName);
		}
		if (fromVersion.CompareTo("8.10.035") < 0)
		{
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtTriggersCode"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDTables", "dtTriggersCode", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtTriggersCodeUser"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDTables", "dtTriggersCodeUser", dropTriggers: false);
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfHasChangeCode"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields Add dfHasChangeCode bit Not Null Default(0)");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfChangeCode"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfChangeCode", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfChangeCodeUser"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfChangeCodeUser", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfValidCode"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfValidCode", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfValidCodeUser"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfValidCodeUser", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfForeignKeyValidCode"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfForeignKeyValidCode", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfForeignKeyValidCodeUser"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfForeignKeyValidCodeUser", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfButtonToolTip"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfButtonToolTip", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfButtonToolTipUser"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfButtonToolTipUser", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfButtonImage"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfButtonImage", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfButtonImageUser"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfButtonImageUser", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfButtonLabel"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfButtonLabel", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfButtonCode"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfButtonCode", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfButtonCodeUser"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfButtonCodeUser", dropTriggers: false);
			}
		}
		if (fromVersion.CompareTo("8.10.037") < 0)
		{
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfSerialStatusPositiveExpression"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfSerialStatusPositiveExpression", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfSerialStatusNegativeExpression"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfSerialStatusNegativeExpression", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfSerialTransactionType"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfSerialTransactionType", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfSerialPartBinField"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfSerialPartBinField", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfSerialTransactionDateField"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfSerialTransactionDateField", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfSerialRelatedJobField"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfSerialRelatedJobField", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfSerialAvailableFilterPositiveExpression"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfSerialAvailableFilterPositiveExpression", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfSerialAvailableFilterNegativeExpression"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfSerialAvailableFilterNegativeExpression", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfSerialAllowMismatchedQuantity"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfSerialAllowMismatchedQuantity", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfSerialRequiredExpression"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfSerialRequiredExpression", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfSerialRequiredExpressionUser"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfSerialRequiredExpressionUser", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfLotStatusPositiveExpression"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfLotStatusPositiveExpression", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfLotStatusNegativeExpression"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfLotStatusNegativeExpression", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfLotTransactionType"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfLotTransactionType", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfLotPartBinField"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfLotPartBinField", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfLotTransactionDateField"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfLotTransactionDateField", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfLotRelatedJobField"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfLotRelatedJobField", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfLotAvailableFilterPositiveExpression"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfLotAvailableFilterPositiveExpression", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfLotAvailableFilterNegativeExpression"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfLotAvailableFilterNegativeExpression", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfLotAllowMismatchedQuantity"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfLotAllowMismatchedQuantity", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfLotRequiredExpression"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfLotRequiredExpression", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfLotRequiredExpressionUser"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfLotRequiredExpressionUser", dropTriggers: false);
			}
		}
		if (fromVersion.CompareTo("8.10.028") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "deAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFormDetails ADD deAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "deSequence"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFormDetails ADD deSequence smallint Not Null Default(0)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "deSequenceUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFormDetails ADD deSequenceUser smallint");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "deParentIDUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFormDetails ADD deParentIDUser varchar(50)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "deNestedNameUser"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFormDetails ADD deNestedNameUser varchar(50)");
			}
		}
		if (fromVersion.CompareTo("8.00.000") < 0)
		{
			if (!DoesTableExist(null, databaseName, "DDSecurityGroups"))
			{
				CreateDataDictionaryTables(null, databaseName, "DDSecurityGroups", string.Empty, ddDef);
				updateDDSecurityGroups(databaseName, dmo, collation);
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxViewer"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD dxViewer numeric(2,0) NOT NULL DEFAULT(0)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxVisualizerID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD dxVisualizerID varchar(35) Not Null Default ''");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxVisualizerType"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD dxVisualizerType tinyint Not Null Default 0");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxextd"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD tempextd text Null");
				ExecuteCommand(databaseName, "Update DDExplorer Set tempextd = dxextd");
				dmo.DropColumn(null, null, null, databaseName, "DDExplorer", "dxextd", dropTriggers: false);
				ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD dxextd text Null");
				ExecuteCommand(databaseName, "Update DDExplorer Set dxextd = tempextd");
				dmo.DropColumn(null, null, null, databaseName, "DDExplorer", "tempextd", dropTriggers: false);
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDInfo", "ddProductCode"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDInfo ADD ddProductCode varchar(15) NOT NULL DEFAULT('')");
				ExecuteCommand(databaseName, "Update DDInfo Set ddProductCode = " + currentContext.Server.IniSettings.Get("ProductID", string.Empty).Trim().ToSql());
				currentContext.Server.IniSettings.Remove("ProductID");
			}
			currentContext.Server.IniSettings.Remove("BackupLocation");
			currentContext.Server.IniSettings.Remove("BackupCopies");
			currentContext.Server.IniSettings.Remove("DefaultDBSize");
			currentContext.Server.IniSettings.Remove("DefaultDataDictionarySize");
			if (!DoesTableExist(currentConnection, databaseName, "DDVisualizers"))
			{
				CreateDataDictionaryTables(null, databaseName, "DDVisualizers", "", ddDef);
			}
			else
			{
				ExecuteCommand(databaseName, "delete DDVisualizers");
			}
			if (!DoesTableExist(currentConnection, databaseName, "DDSeries"))
			{
				CreateDataDictionaryTables(null, databaseName, "DDSeries", "", ddDef);
			}
			else
			{
				ExecuteCommand(databaseName, "delete DDSeries");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxset"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDExplorer", "dxset", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxvmod"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDExplorer", "dxvmod", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxcalc"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDExplorer", "dxcalc", dropTriggers: false);
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxLinkedID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD dxLinkedID numeric(8,0) NOT NULL DEFAULT 0");
				ExecuteCommand(databaseName, "CREATE INDEX dxLinkedID ON DDExplorer (dxLinkedID)");
			}
			ExecuteCommand(databaseName, "Delete DDExplorer Where dxMode = 'SBAR' and dxUser = ''");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxCustom = 1 Where dxMode = 'SBAR'");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxType = 8 Where dxMode = 'SBAR' And dxParentID = 0");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxGridID = Convert(nvarchar(35),Replace(SubString(dxextd,CharIndex(Char(13),dxextd)+1,35),Char(10),'')) where dxType = 1 And CharIndex(Char(13),dxextd) > 0 And Convert(nvarchar(35),dxextd) <> '' And dxGridID = ''");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxGridID = Convert(nvarchar(35),dxextd) Where dxType = 1 And CharIndex(Char(13),dxextd) = 0 And Convert(nvarchar(35),dxextd) <> '' And dxGridID = ''");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxextd = '' Where dxType = 1 And dxGridID <> ''");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxGridID = Convert(nvarchar(35),dxextd) Where (dxType = 20) And dxGridID = ''");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxextd = '' Where (dxType = 20) And dxGridID <> ''");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxGridID = 'M1' + Convert(nvarchar(30),dxextd) + 'ALL' Where (dxType = 15 Or dxType = 16 Or dxType = 17) And dxGridID = ''");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxextd = '' Where (dxType = 15 Or dxType = 16 Or dxType = 17) And dxGridID <> ''");
			if (dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxbmp"))
			{
				ExecuteCommand(databaseName, "Update DDExplorer Set dxType = 33 Where dxType = 4 And dxbmp = 'print'");
				ExecuteCommand(databaseName, "Update DDExplorer Set dxType = 34 Where dxType = 4 And dxbmp like '%Maintenance%'");
				ExecuteCommand(databaseName, "Update DDExplorer Set dxType = 30 Where dxType = 4 And dxbmp = 'wizwand'");
				ExecuteCommand(databaseName, "Update DDExplorer Set dxType = 31 Where dxType = 4 And dxbmp = 'erase'");
				ExecuteCommand(databaseName, "Update DDExplorer Set dxType = 32 Where dxmode = 'SBAR' And dxType = 4 And (dxbmp = '' Or dxbmp = 'search.ico' Or dxbmp = 'bargraph.ico' Or dxbmp = 'calendar.ico' Or dxbmp = 'unknown.ico' Or dxbmp = 'shopload.ico')");
				dmo.DropColumn(null, null, null, databaseName, "DDExplorer", "dxbmp", dropTriggers: false);
			}
			ExecuteCommand(databaseName, "Update DDExplorer Set dxViewer = 93 Where dxType = 1 And dxViewer = 0");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxType = 37, dxVisualizerType = 5, dxVisualizerID = dxGridID Where dxType = 17");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxType = 37, dxVisualizerType = 1, dxVisualizerID = dxGridID Where dxType = 16");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxType = 37, dxVisualizerType = 2, dxVisualizerID = dxGridID Where dxType = 15");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxType = 37, dxVisualizerType = 3, dxVisualizerID = dxGridID Where dxType = 20");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxType = 37, dxVisualizerType = 4, dxVisualizerID = dxGridID Where dxType = 23");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxGridID = '' Where dxType = 37");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 141 Where dxType = 1 And dxParentID = 1");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 281 Where dxType = 1 And dxParentID = 2");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 283 Where dxType = 1 And dxParentID = 3");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 296 Where dxType = 1 And dxParentID = 4");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 308 Where dxType = 1 And dxParentID = 5");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 312 Where dxType = 1 And dxParentID = 6");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 446 Where dxType = 1 And dxParentID = 99");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 275 Where dxType = 1 And dxParentID = 110");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 274 Where dxType = 1 And dxParentID = 221");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 362 Where dxType = 1 And dxParentID = 236");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 278 Where dxType = 1 And dxParentID = 255");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 412 Where dxType = 1 And dxParentID = 401");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 878 Where dxType = 1 And dxParentID = 421");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 481 Where dxType = 1 And dxParentID = 478");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 501 Where dxType = 1 And dxParentID = 498");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 511 Where dxType = 1 And dxParentID = 508");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 539 Where dxType = 1 And dxParentID = 542");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 656 Where dxType = 1 And dxParentID = 654");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 732 Where dxType = 1 And dxParentID = 729");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 766 Where dxType = 1 And dxParentID = 763");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 832 Where dxType = 1 And dxParentID = 829");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 853 Where dxType = 1 And dxParentID = 850");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 888 Where dxType = 1 And dxParentID = 885");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 912 Where dxType = 1 And dxParentID = 910");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 954 Where dxType = 1 And dxParentID = 951");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 1000 Where dxType = 1 And dxParentID = 997");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 1037 Where dxType = 1 And dxParentID = 1032");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 1057 Where dxType = 1 And dxParentID = 1055");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxParentID = 1124 Where dxType = 1 And dxParentID = 1122");
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGrids", "djExplorer"))
			{
				DataTable dataTable5 = GetDataTable(databaseName, "Select duUserID From DDUsers Order By duUserID");
				if (dataTable5.Rows.Count > 0)
				{
					msgDelegate?.Invoke("Converting DDExplorer search records for existing users");
					string queryString = "Select dxUser, dxMode, Identity(int,10000,1) As dxID, dxText, dxParentID, dxType, dxViewer, dxVisualizerID, dxVisualizerType, dxGridID, dxsmod, dxSequence, dxCustom Into TempExp From (Select djUserID As dxUser,'TREE' As dxMode,0 As dxid,djDesc as dxText,Case djExplorer When 'OM' Then 141 When 'IM' Then 281 When 'PM' Then 283 When 'AR' Then 296 When 'AP' Then 308 When 'GL' Then 312 When 'VS' Then 446 When 'JM' Then 275 When 'QM' Then 274 When 'LM' Then 362 When 'SM' Then 278 When 'RQ' Then 412 When 'LO' Then 481 When 'HD' Then 501 When 'CM' Then 511 When 'MW' Then 539 When 'CH' Then 656 When 'RM' Then 766 When 'PA' Then 888 When 'EM' Then 912 When 'RA' Then 954 When 'DM' Then 1000 When 'FA' Then 1057 When 'QA' Then 1037 When 'PR' Then 853 Else 0 End As dxParentID,1 As dxType, 93 As dxViewer,Convert(nvarchar(35),'') As dxVisualizerID,0 As dxVisualizerType,djGridID As dxGridID,djExplorer As dxsmod, 0 As dxSequence, -1 As dxCustom From ddgrids Where djexplorer <> '') As test Where dxParentID <> 0 Order By dxsmod,dxText ";
					ExecuteCommand(databaseName, queryString);
					try
					{
						ExecuteCommand(databaseName, "Update TempExp Set dxSequence = dxid-9899");
						string empty = string.Empty;
						foreach (DataRow row2 in dataTable5.Rows)
						{
							empty = row2.Field<string>("duUserID").Trim();
							ExecuteCommand(databaseName, string.Format("Insert Into DDExplorer (dxUser,dxMode,dxID,dxText,dxParentID,dxType,dxViewer,dxVisualizerID,dxVisualizerType,dxGridID,dxsmod,dxSequence,dxCustom) Select " + empty.ToSql() + ",dxMode,dxID,dxText,dxParentID,dxType,dxViewer,dxVisualizerID,dxVisualizerType,dxGridID,dxsmod,dxSequence,dxCustom From TempExp Where (dxUser = '' Or dxUser = " + empty.ToSql() + ") And dxMode+Convert(char(10),dxid) COLLATE {0} Not In (select dxMode+Convert(char(10),dxid) from DDExplorer Where dxUser = " + empty.ToSql() + ")", collation));
						}
					}
					finally
					{
						ExecuteCommand(databaseName, "Drop Table TempExp");
					}
				}
			}
			ExecuteCommand(databaseName, "Update DDFormDetails Set deClassID = Replace(deClassID, 'M1.', 'M1CONTROLS92.')");
			if (DoesTableExist(null, databaseName, "DDTutorialDetails"))
			{
				dmo.DropTable(null, null, databaseName, "DDTutorialDetails");
			}
			if (DoesTableExist(null, databaseName, "DDTutorials"))
			{
				dmo.DropTable(null, null, databaseName, "DDTutorials");
			}
			if (DoesTableExist(null, databaseName, "DDConversions"))
			{
				dmo.DropTable(null, null, databaseName, "DDConversions");
			}
			ReloadTable(databaseName, "DDTables", recreateTable: true, msgDelegate, ddDef);
			ExecuteCommand(databaseName, "Update DDTables Set dttable = RTrim(dttable), dtDisplayName = RTrim(dtDisplayName), dtcaption = RTrim(dtcaption), dtParentTable = RTrim(dtParentTable), dtDefaultObjectId = RTrim(dtDefaultObjectId), dtgridid = RTrim(dtgridid), dtKeyFields = RTrim(dtKeyFields), dtKeyGroup = RTrim(dtKeyGroup), dtmodule = RTrim(dtmodule), dtaddfld1 = RTrim(dtaddfld1), dtaddfld2 = RTrim(dtaddfld2), dtaddfld3 = RTrim(dtaddfld3), dtColorExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dtColorExpression),'')) = '' Then Null Else dtColorExpression End, dtInitialValue = RTrim(dtInitialValue), dtprefix = RTrim(dtprefix), dtPrefixUser = RTrim(dtPrefixUser), dtReadonlyExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dtReadonlyExpression),'')) = '' Then Null Else dtReadonlyExpression End, dtDisableAddNewExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dtDisableAddNewExpression),'')) = '' Then Null Else dtDisableAddNewExpression End, dtDisableDeleteExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dtDisableDeleteExpression),'')) = '' Then Null Else dtDisableDeleteExpression End, dtFieldToCheckOnUpdate = RTrim(dtFieldToCheckOnUpdate), dtvtbl = RTrim(dtvtbl), dtv3tb = RTrim(dtv3tb), dtutbl = RTrim(dtutbl), dtvrel = RTrim(dtvrel), dtChangeDetailIdsFilter = RTrim(dtChangeDetailIdsFilter), dtForeignKeyDeleteFilter = RTrim(dtForeignKeyDeleteFilter), dtCurrencyModeLocationField = RTrim(dtCurrencyModeLocationField), dtCurrencyRateIdField = RTrim(dtCurrencyRateIdField), dtCurrencyCustomRateField = RTrim(dtCurrencyCustomRateField), dtCurrencyExchangeRateField = RTrim(dtCurrencyExchangeRateField), dtDocumentDateField = RTrim(dtDocumentDateField), dtContactField = RTrim(dtContactField), dtPromptOnAddField = RTrim(dtPromptOnAddField), dtUniqueField = RTrim(dtUniqueField), dtviewdef = Case When LTrim(IsNull(Convert(nvarchar(100),dtviewdef),'')) = '' Then Null Else dtviewdef End Where dtCustom <> 0");
			ExecuteCommand(databaseName, "UPDATE DDTables SET dtUAddFld1 = RTrim(dtUAddFld1), dtUAddFld2 = RTrim(dtUAddFld2),dtUAddFld3 = RTrim(dtUAddFld3), dtColorExpressionUser = Case When LTrim(IsNull(Convert(nvarchar(100),dtColorExpressionUser),'')) = '' Then Null Else dtColorExpressionUser End, dtReadonlyExpressionUser = Case When LTrim(IsNull(Convert(nvarchar(100),dtReadonlyExpressionUser),'')) = '' Then Null Else dtReadonlyExpressionUser End, dtDisableAddNewExpressionUser = Case When LTrim(IsNull(Convert(nvarchar(100),dtDisableAddNewExpressionUser),'')) = '' Then Null Else dtDisableAddNewExpressionUser End, dtDisableDeleteExpressionUser = Case When LTrim(IsNull(Convert(nvarchar(100),dtDisableDeleteExpressionUser),'')) = '' Then Null Else dtDisableDeleteExpressionUser End");
			ReloadTable(databaseName, "DDFields", recreateTable: true, msgDelegate, ddDef);
			ExecuteCommand(databaseName, "UPDATE DDFields SET dftable = RTrim(dftable), dffield = RTrim(dffield), dfDisplayName = RTrim(dfDisplayName), dfCalculationExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dfCalculationExpression),'')) = '' Then Null Else dfCalculationExpression End, dfRequiredExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dfRequiredExpression),'')) = '' Then Null Else dfRequiredExpression End, dfoltype = RTrim(dfoltype), dfolrelfld = RTrim(dfolrelfld), dfformat = RTrim(dfformat), dfDefaultExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dfDefaultExpression),'')) = '' Then Null Else dfDefaultExpression End, dfReadonlyExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dfReadonlyExpression),'')) = '' Then Null Else dfReadonlyExpression End, dfmodule = RTrim(dfmodule), dfBoundParentField = RTrim(dfBoundParentField), dfBoundParentFieldProxy = RTrim(dfBoundParentFieldProxy), dfCurrencyRelatedField = RTrim(dfCurrencyRelatedField), dfRelatedTable = RTrim(dfRelatedTable), dfRelatedFields = RTrim(dfRelatedFields), dfffil = RTrim(dfffil), dfForeignKeyRequiredExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dfForeignKeyRequiredExpression),'')) = '' Then Null Else dfForeignKeyRequiredExpression End, dfValueList = Case When LTrim(IsNull(Convert(nvarchar(100),dfValueList),'')) = '' Then Null Else dfValueList End, dfRelatedTableSearchGridId = RTrim(dfRelatedTableSearchGridId), dfRelatedTableReturnField = RTrim(dfRelatedTableReturnField), dfRelatedTabledescriptionField = RTrim(dfRelatedTabledescriptionField), dfRelatedTableOrderByField = RTrim(dfRelatedTableOrderByField), dfRelatedTableFilter = Case When LTrim(IsNull(Convert(nvarchar(100),dfRelatedTableFilter),'')) = '' Then Null Else dfRelatedTableFilter End, dfhelp = Case When LTrim(IsNull(Convert(nvarchar(100),dfhelp),'')) = '' Then Null Else dfhelp End, dfgroup = RTrim(dfgroup), dfstatus = RTrim(dfstatus), dfconv = RTrim(dfconv), dfvfld = RTrim(dfvfld), dfv3fd = RTrim(dfv3fd), dfcomments = Case When LTrim(IsNull(Convert(nvarchar(100),dfcomments),'')) = '' Then Null Else dfcomments End Where dfCustom <> 0");
			ExecuteCommand(databaseName, "UPDATE DDFields SET dfCaption = RTrim(dfCaption),dfDefaultExpressionUser = Case When LTrim(IsNull(Convert(nvarchar(100),dfDefaultExpressionUser),'')) = '' Then Null Else dfDefaultExpressionUser End, dfRequiredExpressionUser = Case When LTrim(IsNull(Convert(nvarchar(100),dfRequiredExpressionUser),'')) = '' Then Null Else dfRequiredExpressionUser End, dfReadonlyExpressionUser = Case When LTrim(IsNull(Convert(nvarchar(100),dfReadonlyExpressionUser),'')) = '' Then Null Else dfReadonlyExpressionUser End, dfForeignKeyRequiredExpressionUser = Case When LTrim(IsNull(Convert(nvarchar(100),dfForeignKeyRequiredExpressionUser),'')) = '' Then Null Else dfForeignKeyRequiredExpressionUser End ");
			ReloadTable(databaseName, "DDRelations", recreateTable: true, msgDelegate, ddDef);
			ExecuteCommand(databaseName, "Update DDRelations Set drptable = RTrim(drptable), drpfield = RTrim(drpfield), drctable = RTrim(drctable), drcfield = RTrim(drcfield), drfilter = RTrim(drfilter), drdfilter = RTrim(drdfilter) Where drCustom <> 0");
			ReloadTable(databaseName, "DDSecurityGroups", recreateTable: true, msgDelegate, ddDef);
			ExecuteCommand(databaseName, "Update DDSecurityGroups Set dzGroupID = RTrim(dzGroupID), dzUserID = RTrim(dzUserID), dzDataset = RTrim(dzDataset)");
			ReloadTable(databaseName, "DDSecurityTables", recreateTable: true, msgDelegate, ddDef);
			ExecuteCommand(databaseName, "Update DDSecurityTables Set dtUserID = RTrim(dtUserID), dtDataset = RTrim(dtDataset), dtTable = RTrim(dtTable), dtField = RTrim(dtField)");
			ReloadTable(databaseName, "DDSecurityReports", recreateTable: true, msgDelegate, ddDef);
			ExecuteCommand(databaseName, "Update DDSecurityReports Set drFolder = RTrim(drFolder), drReport = RTrim(drReport), drUserID = RTrim(drUserID), drDataset = RTrim(drDataset), drSettings = Case When LTrim(IsNull(Convert(nvarchar(100),drSettings),'')) = '' Then Null Else drSettings End ");
			if (dmo.DoesFieldExist(null, null, databaseName, "ddSecurityGroups", "dzDataset"))
			{
				ExecuteCommand(databaseName, "Update DDSecurityGroups Set dzDataset = 'M1_' + right(RTRIM(dzDataset),2) Where Len(RTrim(dzDataset)) = 2");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDSecurityReports", "drDataset"))
			{
				ExecuteCommand(databaseName, "Update DDSecurityReports Set drDataset = 'M1_' + right(RTRIM(drDataset),2) Where Len(RTrim(drDataset)) = 2");
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDSecurityTables", "dtDataset"))
			{
				ExecuteCommand(databaseName, "Update DDSecurityTables Set dtDataset = 'M1_' + right(RTRIM(dtDataset),2) Where Len(RTrim(dtDataset)) = 2");
			}
			updateDDAddDefaultDbSecurity(databaseName, dmo, collation);
			ExecuteCommand(databaseName, "Select RTrim(duUserID) As duUserID, duReportDefault Into TempUsers From DDUsers");
			try
			{
				ReloadTable(databaseName, "DDUsers", recreateTable: true, msgDelegate, ddDef);
				ExecuteCommand(databaseName, "Update DDUsers Set duuserid = RTrim(duuserid),dupassword = RTrim(dupassword), duname = RTrim(duname), duproperties = Case When LTrim(IsNull(Convert(nvarchar(100),duproperties),'')) = '' Then Null Else duproperties End, duportal = Case When LTrim(IsNull(Convert(nvarchar(100),duportal),'')) = '' Then Null Else duportal End ");
				updateDDSecurityGroupsStep2(databaseName, dmo, collation);
				updateAutologoutSettings(databaseName);
				ExecuteCommand(databaseName, "Update DDUsers Set duDBAdministrator = duAdministrator");
				if (DoesTableExist(null, databaseName, "DDUserLog"))
				{
					dmo.DropTable(null, null, databaseName, "DDUserLog");
				}
				CreateDataDictionaryTables(null, databaseName, "DDUserLog", string.Empty, ddDef);
				ReloadTable(databaseName, "DDObjectDetails", recreateTable: true, msgDelegate, ddDef);
				ExecuteCommand(databaseName, "Update DDObjectDetails Set dlobjectid = RTrim(dlobjectid), dltable = RTrim(dltable), dlparent = RTrim(dlparent), dlview = RTrim(dlview), dlsearchid = RTrim(dlsearchid), dlgridid = RTrim(dlgridid), dlorder = RTrim(dlorder), dluorder = Case When LTrim(IsNull(Convert(nvarchar(100),dluorder),'')) = '' Then Null Else dluorder End, dlcfield = RTrim(dlcfield), dlfilter = RTrim(dlfilter), dlhide = RTrim(dlhide) Where dlCustom <> 0");
				ReloadTable(databaseName, "DDObjects", recreateTable: true, msgDelegate, ddDef);
				ExecuteCommand(databaseName, "Update DDObjects Set doobjectid = RTrim(doobjectid), dotable = RTrim(dotable), doname = RTrim(doname), dotitle = RTrim(dotitle), domodule = RTrim(domodule) Where doCustom <> 0");
				ReloadTable(databaseName, "DDForms", recreateTable: true, msgDelegate, ddDef);
				ExecuteCommand(databaseName, "Update DDForms Set dmformid = RTrim(dmformid), dmtable = RTrim(dmtable), dmcaption = RTrim(dmcaption), dmhelplink = RTrim(dmhelplink), dmvid = RTrim(dmvid), dmdesgroup = RTrim(dmdesgroup) Where dmCustom <> 0");
				GetDataTable(databaseName, "select * from ddformdetails where deformid like '%viewquoteline%'");
				ReloadTable(databaseName, "DDFormDetails", recreateTable: true, msgDelegate, ddDef);
				ExecuteCommand(databaseName, "Update DDFormDetails Set deFormID = RTrim(deFormID),deControlName = RTrim(deControlName), deClassID = RTrim(deClassID), deProperties = Case When LTrim(IsNull(Convert(nvarchar(100),deProperties),'')) = '' Then Null Else deProperties End ");
				ReloadTable(databaseName, "DDGrids", recreateTable: true, msgDelegate, ddDef);
				ExecuteCommand(databaseName, "Update DDGrids Set djGridID = RTrim(djGridID), djUserID = RTrim(djUserID), djTable = RTrim(djTable), DJDESC = RTrim(DJDESC), DJEXTD = Case When LTrim(IsNull(Convert(nvarchar(100),DJEXTD),'')) = '' Then Null Else DJEXTD End Where djCustom <> 0");
				ReloadTable(databaseName, "DDGridDetails", recreateTable: true, msgDelegate, ddDef);
				ExecuteCommand(databaseName, "Update DDGridDetails Set dgGridID = RTrim(dgGridID), dgUserID = RTrim(dgUserID), DGGRP = Case When LTrim(IsNull(Convert(nvarchar(100),DGGRP),'')) = '' Then Null Else DGGRP End, DGORD = Case When LTrim(IsNull(Convert(nvarchar(100),DGORD),'')) = '' Then Null Else DGORD End, DGFLDS = Case When LTrim(IsNull(Convert(nvarchar(100),DGFLDS),'')) = '' Then Null Else DGFLDS End, DGFROM = Case When LTrim(IsNull(Convert(nvarchar(100),DGFROM),'')) = '' Then Null Else DGFROM End, DGREQOPT = Case When LTrim(IsNull(Convert(nvarchar(100),DGREQOPT),'')) = '' Then Null Else DGREQOPT End, DGWHER = Case When LTrim(IsNull(Convert(nvarchar(100),DGWHER),'')) = '' Then Null Else DGWHER End, DGSGRP = Case When LTrim(IsNull(Convert(nvarchar(100),DGSGRP),'')) = '' Then Null Else DGSGRP End, DGSORD = Case When LTrim(IsNull(Convert(nvarchar(100),DGSORD),'')) = '' Then Null Else DGSORD End, dgSQLSet = Case When LTrim(IsNull(Convert(nvarchar(100),dgSQLSet),'')) = '' Then Null Else dgSQLSet End, dgADOSet = Case When LTrim(IsNull(Convert(nvarchar(100),dgADOSet),'')) = '' Then Null Else dgADOSet End, dgDatasets = Case When LTrim(IsNull(Convert(nvarchar(100),dgDatasets),'')) = '' Then Null Else dgDatasets End, DGSPGROUP = RTrim(DGSPGROUP), DGSPTEXT = RTrim(DGSPTEXT), DGSPCALC = Case When LTrim(IsNull(Convert(nvarchar(100),DGSPCALC),'')) = '' Then Null Else DGSPCALC End, dgCalDateF = RTrim(dgCalDateF), dgSFormula = Case When LTrim(IsNull(Convert(nvarchar(100),dgSFormula),'')) = '' Then Null Else dgSFormula End Where dgCustom <> 0");
				ReloadTable(databaseName, "DDSearches", recreateTable: true, msgDelegate, ddDef);
				ExecuteCommand(databaseName, "Update DDSearches Set dsSearchID = RTrim(dsSearchID), dsUserID = RTrim(dsUserID), dsGridID = RTrim(dsGridID), dsField = RTrim(dsField), dsPreviousGrids = Case When LTrim(IsNull(Convert(nvarchar(100),dsPreviousGrids),'')) = '' Then Null Else dsPreviousGrids End ");
				ReloadTable(databaseName, "DDOpenWiths", recreateTable: true, msgDelegate, ddDef);
				ExecuteCommand(databaseName, "Update DDOpenWiths Set dwID = RTrim(dwID), dwTable = RTrim(dwTable), dwField = RTrim(dwField), dwDesc = RTrim(dwDesc), dwObject = RTrim(dwObject), dwHide = Case When LTrim(IsNull(Convert(nvarchar(100),dwHide),'')) = '' Then Null Else dwHide End, dwUHide = Case When LTrim(IsNull(Convert(nvarchar(100),dwUHide),'')) = '' Then Null Else dwUHide End, dwCode = Case When LTrim(IsNull(Convert(nvarchar(100),dwCode),'')) = '' Then Null Else dwCode End Where dwCustom <> 0");
				ReloadTable(databaseName, "DDFieldUserSettings", recreateTable: true, msgDelegate, ddDef);
				ExecuteCommand(databaseName, "Update DDFieldUserSettings Set datable = RTrim(datable), daField = RTrim(daField), daUser = RTrim(daUser), daDefault = RTrim(daDefault)");
				ReloadTable(databaseName, "DDCustomModules", recreateTable: true, msgDelegate, ddDef);
				ReloadTable(databaseName, "DDInfo", recreateTable: true, msgDelegate, ddDef);
				ExecuteCommand(databaseName, "Update DDInfo Set ddVersion = RTrim(ddVersion), ddRegion = RTrim(ddRegion), ddLanguage = RTrim(ddLanguage), ddProductCode = RTrim(ddProductCode), ddProperties = Case When LTrim(IsNull(Convert(nvarchar(100),ddProperties),'')) = '' Then Null Else ddProperties End, ddUpgradeVersions = Case When LTrim(IsNull(Convert(nvarchar(100),ddUpgradeVersions),'')) = '' Then Null Else ddUpgradeVersions End ");
				msgDelegate?.Invoke("Converting security table values to new format");
				if (dmo.DoesFieldExist(null, null, databaseName, "DDSecurityTables", "dtLevel"))
				{
					ExecuteCommand(databaseName, "Update DDSecurityTables Set dtLevel = 28 Where dtLevel = 6");
					ExecuteCommand(databaseName, "Update DDSecurityTables Set dtLevel = 20 Where dtLevel = 5");
					ExecuteCommand(databaseName, "Update DDSecurityTables Set dtLevel = 12 Where dtLevel = 4");
					ExecuteCommand(databaseName, "Update DDSecurityTables Set dtLevel = 4 Where dtLevel = 3");
					ExecuteCommand(databaseName, $"Update DDSecurityTables Set dtLevel = 60 From DDSecurityTables Inner Join DDSecurityGroups On dtUserID = dzUserID COLLATE {collation} And dtDataset = dzDataset And dzGroupID = 'CHANGEID' Where dtLevel = 28");
					ExecuteCommand(databaseName, "Delete From DDSecurityGroups Where dzGroupID = 'CHANGEID' Or dzGroupID = 'CHANGEDETAILIDS'");
				}
				if (dmo.DoesFieldExist(null, null, databaseName, "DDSecurityReports", "drLevel"))
				{
					msgDelegate?.Invoke("Copying all report ini settings into the data dictionary");
				}
				updateDDSecurityReports(databaseName, collation);
			}
			finally
			{
				ExecuteCommand(databaseName, "Drop Table TempUsers");
			}
		}
		if (fromVersion.CompareTo("8.10.032") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables ADD dtAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDRelations", "drAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDRelations ADD drAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD dxAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDUsers", "duAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDUsers ADD duAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDObjectDetails", "dlAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDObjectDetails ADD dlAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDObjects", "doAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDObjects ADD doAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDForms", "dmAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDForms ADD dmAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "deAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFormDetails ADD deAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDGrids", "djAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDGrids ADD djAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDGridDetails ADD dgAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDOpenWiths", "dwAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDOpenWiths ADD dwAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDScripts", "dyAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDScripts ADD dyAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDVisualizers", "dvAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDVisualizers ADD dvAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDSeries", "diAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDSeries ADD diAppExtensionID varchar(20) Not Null Default('')");
			}
		}
		if (fromVersion.CompareTo("8.00.028") < 0)
		{
			ExecuteCommand(databaseName, "Update DDFormDetails Set deFormID = Replace(deFormID, 'M1.VIEW', 'VIEW')");
			ExecuteCommand(databaseName, "Update DDFormDetails Set deFormID = 'FRMARCREATEINVOICESFROMSHIPMENT' Where deFormID = 'FRMARCREATEINVOICESFROMSHIPMENTS'");
		}
		if (fromVersion.CompareTo("8.00.029") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDUsers", "duDeveloperProperties"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDUsers ADD duDeveloperProperties text");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDFormDetails", "deSequence"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDFormDetails ADD deSequence smallint Not Null Default(0)");
			}
		}
		if (fromVersion.CompareTo("8.00.036") < 0 && dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgWGFilt"))
		{
			ExecuteCommand(databaseName, "EXEC sp_rename 'DDGridDetails.dgWGFilt', 'dgWGFilt_A', 'COLUMN'");
			ExecuteCommand(databaseName, "ALTER TABLE DDGridDetails Add dgWGFilt bit Not Null Default(0)");
			ExecuteCommand(databaseName, "Update DDGridDetails Set dgWGFilt = Case When dgWGFilt_A <> 0 Then 1 Else 0 End");
			dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgWGFilt_A", dropTriggers: false);
		}
		if (fromVersion.CompareTo("8.00.044") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgOpenWithID"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDGridDetails Add dgOpenWithID varchar(30) Not Null Default ''");
		}
		if (fromVersion.CompareTo("8.00.059") < 0)
		{
			ExecuteCommand(databaseName, "Delete From DDSearches Where Left(dsSearchID, 6) = 'OVVIEW'");
			ExecuteCommand(databaseName, "Update DDSearches Set dsSearchID = 'OV' + SUBSTRING(dsSearchID, 6, 50) Where Left(dsSearchID, 5) = 'OVM1.'");
		}
		if (fromVersion.CompareTo("8.00.091") < 0)
		{
			ExecuteCommand(databaseName, "Update DDFormDetails Set deFormID = 'FRMARINVOICEWIZARD' Where deFormID = 'FRMARCREATEINVOICESFROMSHIPMENT'");
			ExecuteCommand(databaseName, "Update DDFormDetails Set deFormID = 'FRMAPINVOICEWIZARD' Where deFormID = 'FRMAPCREATEINVOICESFROMRECEIPTS'");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxextd = Replace(Convert(nvarchar(4000),dxextd),'FRMARCREATEINVOICESFROMSHIPMENTS','frmARInvoiceWizard') Where dxMode = 'SBAR' And Convert(nvarchar(4000),dxextd) Like '%FRMARCREATEINVOICESFROMSHIPMENTS%'");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxextd = Replace(Convert(nvarchar(4000),dxextd),'FRMARCREATEINVOICESFROMSHIPMENT','frmARInvoiceWizard') Where dxMode = 'SBAR' And Convert(nvarchar(4000),dxextd) Like '%FRMARCREATEINVOICESFROMSHIPMENT%'");
			ExecuteCommand(databaseName, "Update DDExplorer Set dxextd = Replace(Convert(nvarchar(4000),dxextd),'FRMAPCREATEINVOICESFROMRECEIPTS','frmAPInvoiceWizard') Where dxMode = 'SBAR' And Convert(nvarchar(4000),dxextd) Like '%FRMAPCREATEINVOICESFROMRECEIPTS%'");
		}
		if (fromVersion.CompareTo("8.00.096") < 0 && dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfureq"))
		{
			ExecuteCommand(databaseName, "Update DDFields Set dfRequiredExpressionUser = 'True' Where dfureq <> 0");
			dmo.DropColumn(null, null, null, databaseName, "DDFields", "dfureq", dropTriggers: false);
		}
		if (fromVersion.CompareTo("8.00.098") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgExportProperties"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDGridDetails Add dgExportProperties text Null");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgPrintingProperties"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDGridDetails Add dgPrintingProperties text Null");
			}
		}
		if (fromVersion.CompareTo("8.00.101") < 0)
		{
			if (DoesTableExist(null, databaseName, "DDSolutionDetails"))
			{
				dmo.DropTable(null, null, databaseName, "DDSolutionDetails");
			}
			if (!DoesTableExist(null, databaseName, "DDSolutionDetails"))
			{
				CreateDataDictionaryTables(null, databaseName, "DDSolutionDetails", string.Empty, ddDef);
			}
			if (DoesTableExist(null, databaseName, "DDSolutions"))
			{
				dmo.DropTable(null, null, databaseName, "DDSolutions");
			}
			if (!DoesTableExist(null, databaseName, "DDSolutions"))
			{
				CreateDataDictionaryTables(null, databaseName, "DDSolutions", string.Empty, ddDef);
			}
		}
		if (fromVersion.CompareTo("8.10.039") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDSolutions", "dnAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDSolutions ADD dnAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDSolutionDetails", "diAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDSolutionDetails ADD diAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDCustomModules", "dcAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDCustomModules ADD dcAppExtensionID varchar(20) Not Null Default('')");
			}
		}
		if (fromVersion.CompareTo("8.00.112") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxVisualizerType"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD dxVisualizerType tinyint Not Null Default 0");
		}
		if (fromVersion.CompareTo("8.00.121") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfAllowNulls"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDFields ADD dfAllowNulls bit Not Null Default 0");
			ExecuteCommand(databaseName, "Update DDFields Set dfAllowNulls = 1 Where dfDBType = 'datetime' Or dfDBType = 'date' Or dfDBType = 'image'");
		}
		if (fromVersion.CompareTo("9.00.000") < 0 && dmo.DoesFieldExist(null, null, databaseName, "DDFields", "dfAllowNulls"))
		{
			ExecuteCommand(databaseName, "Update DDFields Set dfAllowNulls = 1 Where dfDBType = 'datetime' Or dfDBType = 'date' Or dfDBType = 'image'");
		}
		if (fromVersion.CompareTo("8.00.125") < 0)
		{
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgMapProperties"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgMapProperties", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgCalendarProperties"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgCalendarProperties", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgPieChartProperties"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgPieChartProperties", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgColumnChartProperties"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgColumnChartProperties", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgBarChartProperties"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgBarChartProperties", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgLineChartProperties"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgLineChartProperties", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgFunnelChartProperties"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgFunnelChartProperties", dropTriggers: false);
			}
		}
		if (fromVersion.CompareTo("8.00.127") < 0)
		{
			ReloadTable(databaseName, "DDFormDetails", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDForms", recreateTable: true, msgDelegate, ddDef);
		}
		if (fromVersion.CompareTo("8.00.134") < 0)
		{
			ExecuteCommand(databaseName, "Delete From DDSearches Where dsSearchID = 'DFRMCGLFISCALYEARPERIODID'");
		}
		if (fromVersion.CompareTo("8.00.135") < 0)
		{
			ExecuteCommand(databaseName, $"Update DDForms Set dmFormType = 2 From DDForms Inner Join DDFormDetails on deFormID = dmFormID COLLATE {collation} Where deClassID like 'M1SFE%' And dmCustom <> 0 And dmFormType <> 2");
			ExecuteCommand(databaseName, "Update DDForms Set dmFormType = 1 Where dmCustom <> 0 And dmFormType = 0");
		}
		if (fromVersion.CompareTo("8.00.146") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDInfo", "ddDSProperties"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDInfo ADD ddDSProperties text");
		}
		if (fromVersion.CompareTo("8.00.148") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDGrids", "djNoPrimaryTable"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDGrids Add djNoPrimaryTable bit Not Null Default 0");
		}
		if (fromVersion.CompareTo("8.00.155") < 0 && Convert.ToInt32(currentContext.DDServerManager.ExecuteScalar(null, null, databaseName, "Select Count(*) From DDSecurityGroups Where dzGroupID = 'FAX'")) == 0)
		{
			ExecuteCommand(databaseName, "Insert Into DDSecurityGroups (dzGroupID, dzUserID, dzDataset) Select 'FAX',dzUserID,dzDataset From DDSecurityGroups Where dzGroupID = 'EMAILREPORTS'");
		}
		if (fromVersion.CompareTo("8.00.188") < 0)
		{
			if (!DoesTableExist(null, databaseName, "SFEBarcodes"))
			{
				CreateDataDictionaryTables(null, databaseName, "SFEBarcodes", string.Empty, ddDef);
			}
			if (!DoesTableExist(null, databaseName, "SFEControls"))
			{
				CreateDataDictionaryTables(null, databaseName, "SFEControls", string.Empty, ddDef);
			}
			if (!DoesTableExist(null, databaseName, "SFEGrids"))
			{
				CreateDataDictionaryTables(null, databaseName, "SFEGrids", string.Empty, ddDef);
			}
			if (!DoesTableExist(null, databaseName, "SFEScreens"))
			{
				CreateDataDictionaryTables(null, databaseName, "SFEScreens", string.Empty, ddDef);
			}
		}
		if (fromVersion.CompareTo("8.00.193") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDRelations", "drReseqDetails"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDRelations Add drReseqDetails bit Not Null Default 0");
		}
		if (fromVersion.CompareTo("8.00.198") < 0)
		{
			updateComponents(databaseName, "SAVEAS");
		}
		if (fromVersion.CompareTo("8.00.219") < 0)
		{
			if (!DoesTableExist(null, databaseName, "WebBarcodes"))
			{
				CreateDataDictionaryTables(null, databaseName, "WebBarcodes", string.Empty, ddDef);
			}
			if (!DoesTableExist(null, databaseName, "WebControls"))
			{
				CreateDataDictionaryTables(null, databaseName, "WebControls", string.Empty, ddDef);
			}
			if (!DoesTableExist(null, databaseName, "WebLists"))
			{
				CreateDataDictionaryTables(null, databaseName, "WebLists", string.Empty, ddDef);
			}
			if (!DoesTableExist(null, databaseName, "WebOptions"))
			{
				CreateDataDictionaryTables(null, databaseName, "WebOptions", string.Empty, ddDef);
			}
			if (!DoesTableExist(null, databaseName, "WebScreens"))
			{
				CreateDataDictionaryTables(null, databaseName, "WebScreens", string.Empty, ddDef);
			}
			if (!DoesTableExist(null, databaseName, "WebServerFunctions"))
			{
				CreateDataDictionaryTables(null, databaseName, "WebServerFunctions", string.Empty, ddDef);
			}
			if (!DoesTableExist(null, databaseName, "WebSessions"))
			{
				CreateDataDictionaryTables(null, databaseName, "WebSessions", string.Empty, ddDef);
			}
		}
		if (fromVersion.CompareTo("8.10.032") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "WebBarcodes", "wbAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE WebBarcodes ADD wbAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "WebControls", "wcAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE WebControls ADD wcAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "WebLists", "wlAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE WebLists ADD wlAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "WebScreens", "wsAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE WebScreens ADD wsAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "WebOptions", "woAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE WebOptions ADD woAppExtensionID varchar(20) Not Null Default('')");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "WebServerFunctions", "wfAppExtensionID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE WebServerFunctions ADD wfAppExtensionID varchar(20) Not Null Default('')");
			}
		}
		if (fromVersion.CompareTo("8.10.027") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDInfo", "ddCustomProductCodes"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDInfo ADD ddCustomProductCodes text Null");
			string value = currentContext.Server.IniSettings.Get("CustomProductID", string.Empty).Trim();
			if (!string.IsNullOrEmpty(value))
			{
				using SqlCommand sqlCommand = new SqlCommand("Update DDInfo Set ddCustomProductCodes =@oldKeys");
				sqlCommand.Parameters.Add(new SqlParameter("@oldKeys", SqlDbType.NVarChar)).Value = value;
				ExecuteCommand(currentConnection, databaseName, sqlCommand);
			}
		}
		if (fromVersion.CompareTo("8.10.008") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "WebLists", "wlIsIndexed"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE WebLists ADD wlIsIndexed bit NOT NULL DEFAULT(0)");
		}
		if (fromVersion.CompareTo("8.10.009") < 0)
		{
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgCalDec"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgCalDec", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgCalColor"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgCalColor", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgCalTotF"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgCalTotF", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgCalNum1"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgCalNum1", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgCalNum2"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgCalNum2", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgCalFSize"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgCalFSize", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgBarDate1"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgBarDate1", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgBarDate2"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgBarDate2", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgBarTot1"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgBarTot1", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgBarTot2"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgBarTot2", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgBarComp"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgBarComp", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgBarPY"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgBarPY", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgBarPerc"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgBarPerc", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgBarBuck"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgBarBuck", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgBarBSize"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgBarBSize", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgBarBType"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgBarBType", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgPieGrpF"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgPieGrpF", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgPieGrpS"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgPieGrpS", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgPieTotF"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgPieTotF", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgPieLab"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgPieLab", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgPiePerc"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgPiePerc", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgPieLeg"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgPieLeg", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgPieLock"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgPieLock", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgPieOLim"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgPieOLim", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgPieOType"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgPieOType", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgPieFontN"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgPieFontN", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgPieFontS"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgPieFontS", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgPieFontB"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgPieFontB", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDGridDetails", "dgPieFontI"))
			{
				dmo.DropColumn(null, null, null, databaseName, "DDGridDetails", "dgPieFontI", dropTriggers: false);
			}
		}
		if (fromVersion.CompareTo("8.10.009") < 0)
		{
			if (dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxLinkedID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD dxOldLinkedID integer NOT NULL DEFAULT(0)");
				ExecuteCommand(databaseName, "Update DDExplorer Set dxOldLinkedID = dxLinkedID");
				dmo.DropColumn(null, null, null, databaseName, "DDExplorer", "dxLinkedID", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD dxOldID integer NOT NULL DEFAULT(0)");
				ExecuteCommand(databaseName, "Update DDExplorer Set dxOldID = dxID");
				dmo.DropColumn(null, null, null, databaseName, "DDExplorer", "dxID", dropTriggers: false);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxParentID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD dxOldParentID integer NOT NULL DEFAULT(0)");
				ExecuteCommand(databaseName, "Update DDExplorer Set dxOldParentID = dxParentID");
				dmo.DropColumn(null, null, null, databaseName, "DDExplorer", "dxParentID", dropTriggers: false);
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxLinkedUniqueID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD dxLinkedUniqueID uniqueidentifier");
				ExecuteCommand(databaseName, "CREATE INDEX dxLinkedUniqueID ON DDExplorer (dxLinkedUniqueID)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxParentUniqueID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD dxParentUniqueID uniqueidentifier");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", "dxUniqueID"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDExplorer ADD dxUniqueID uniqueidentifier Not Null Default(NEWID())");
				ExecuteCommand(databaseName, "CREATE Unique INDEX dxUniqueID ON DDExplorer (dxUniqueID)");
			}
			if (dmo.DoesTableExist(null, null, databaseName, "DDExplorerTemp"))
			{
				dmo.DropTable(null, null, databaseName, "DDExplorerTemp");
			}
			ExecuteCommand(databaseName, "sp_rename 'DDExplorer', 'DDExplorerTemp'");
			try
			{
				CreateDataDictionaryTables(null, databaseName, "DDExplorer", string.Empty, ddDef);
				try
				{
					ExecuteCommand(databaseName, string.Format("Update DDExplorerTemp Set dxUniqueID = b.dxUniqueID, dxLinkedUniqueID = b.dxLinkedUniqueID, dxParentUniqueID = b.dxParentUniqueID From DDExplorerTemp a Inner Join DDExplorer b On a.dxOldID = b.dxOldID And a.dxUser = b.dxUser COLLATE {0} And a.dxMode = b.dxMode COLLATE {0} Where b.dxOldID <> 0", collation));
					ExecuteCommand(databaseName, $"Update a Set a.dxLinkedUniqueID = b.dxUniqueID From DDExplorerTemp a Inner Join DDExplorer b On a.dxOldID = b.dxOldID And b.dxUser = '' And b.dxMode = a.dxMode COLLATE {collation} Where a.dxMode = 'TREE' And a.dxUser <> '' And a.dxCustom <> 0");
					ExecuteCommand(databaseName, string.Format("Update a Set a.dxParentUniqueID = b.dxUniqueID From DDExplorerTemp a Inner Join DDExplorer b On a.dxOldParentID = b.dxOldID And b.dxUser = '' And b.dxMode = 'TREE' Where a.dxOldParentID <> 0 And a.dxMode = 'TREE' And a.dxUser <> ''", collation));
					ExecuteCommand(databaseName, string.Format("Update a Set a.dxParentUniqueID = b.dxUniqueID From DDExplorerTemp a Inner Join DDExplorer b On a.dxOldParentID = b.dxOldID And a.dxUser = b.dxUser COLLATE {0} And a.dxMode = b.dxMode COLLATE {0} Where a.dxOldParentID <> 0", collation));
				}
				finally
				{
					ExecuteCommand(databaseName, "Drop Table DDExplorer");
				}
			}
			finally
			{
				ExecuteCommand(databaseName, "sp_rename 'DDExplorerTemp', 'DDExplorer'");
			}
			ExecuteCommand(databaseName, $"Update a Set a.dxParentUniqueID = b.dxUniqueID From DDExplorer a Inner Join DDExplorer b On a.dxOldParentID = b.dxOldID And a.dxUser = b.dxUser COLLATE {collation} And a.dxMode = b.dxMode Where a.dxOldParentID <> 0 And a.dxCustom <> 0");
			ExecuteCommand(databaseName, "delete from ddexplorer where dxuser<>'' and dxmode='TREE' and dxoldid in (select dxoldid from ddexplorer where dxuser='' and dxmode='TREE')");
			ReloadTable(databaseName, "DDExplorer", recreateTable: true, msgDelegate, ddDef);
			ExecuteCommand(databaseName, "Update DDExplorer Set dxuser = RTrim(dxuser), dxmode = RTrim(dxmode), dxtext = RTrim(dxtext), dxextd = Case When LTrim(IsNull(Convert(nvarchar(100),dxextd),'')) = '' Then Null Else dxextd End, dxgridid = RTrim(dxgridid), dxsmod = RTrim(dxsmod), dxscom = RTrim(dxscom) Where dxCustom <> 0");
			ExecuteCommand(databaseName, string.Format("Update a Set dxUniqueID = b.dxUniqueID, dxLinkedUniqueID = b.dxLinkedUniqueID, dxParentUniqueID = b.dxParentUniqueID From DDExplorer a Inner Join DDExplorer b On a.dxOldID = b.dxOldID And a.dxUser = b.dxUser COLLATE {0} And a.dxMode = b.dxMode COLLATE {0} Where b.dxOldID <> 0", collation));
			ExecuteCommand(databaseName, $"Update a Set a.dxLinkedUniqueID = b.dxUniqueID From DDExplorer a Inner Join DDExplorer b On a.dxOldID = b.dxOldID And b.dxUser = '' And b.dxMode = a.dxMode COLLATE {collation} Where a.dxMode = 'TREE' And a.dxUser <> '' And a.dxCustom <> 0");
			ExecuteCommand(databaseName, string.Format("Update a Set a.dxParentUniqueID = b.dxUniqueID From DDExplorer a Inner Join DDExplorer b On a.dxOldParentID = b.dxOldID And b.dxUser = '' And b.dxMode = 'TREE' Where a.dxOldParentID <> 0 And a.dxMode = 'TREE' And a.dxUser <> ''", collation));
			ExecuteCommand(databaseName, string.Format("Update a Set a.dxParentUniqueID = b.dxUniqueID From DDExplorer a Inner Join DDExplorer b On a.dxOldParentID = b.dxOldID And a.dxUser = b.dxUser COLLATE {0} And a.dxMode = b.dxMode COLLATE {0} Where a.dxOldParentID <> 0", collation));
			ExecuteCommand(databaseName, "Update a Set a.dxParentUniqueID = b.dxUniqueID From DDExplorer a Inner Join DDExplorer b On a.dxOldParentID = b.dxOldID And a.dxUser = b.dxUser And a.dxMode = b.dxMode Where a.dxOldParentID <> 0 And a.dxCustom <> 0");
		}
		if (fromVersion.CompareTo("8.10.054") < 0 && !DoesTableExist(null, databaseName, "DDModules"))
		{
			CreateDataDictionaryTables(null, databaseName, "DDModules", string.Empty, ddDef);
		}
		if (fromVersion.CompareTo("8.10.050") < 0)
		{
			if (DoesTableExist(null, databaseName, "DDVisualizers"))
			{
				ExecuteCommand(databaseName, "Update DDVisualizers Set dvCustom = 1 Where dvUserID <> ''");
			}
			if (DoesTableExist(null, databaseName, "DDSeries"))
			{
				ExecuteCommand(databaseName, "Update DDSeries Set diCustom = 1 Where diUserID <> ''");
			}
		}
		if (fromVersion.CompareTo("8.10.050") < 0)
		{
			ReloadTable(databaseName, "DDAppExtensions", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDCode", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDCustomModules", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDExplorer", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDFieldExtensions", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDFieldExtensionTypes", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDFieldGroups", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDFields", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDFieldUserSettings", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDFormDetails", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDForms", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDGridDetails", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDGrids", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDInfo", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDObjectDetails", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDObjectDetailsUser", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDObjects", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDObjectsUser", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDOpenWiths", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDRelations", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDScripts", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDSearches", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDSecurityGroups", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDSecurityReports", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDSecurityTables", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDSeries", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDSolutionDetails", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDSolutions", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDTables", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDUserLog", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDUsers", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "DDVisualizers", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "WebBarcodes", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "WebControls", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "WebLists", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "WebOptions", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "WebScreens", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "WebServerFunctions", recreateTable: true, msgDelegate, ddDef);
			ReloadTable(databaseName, "WebSessions", recreateTable: true, msgDelegate, ddDef);
		}
		if (fromVersion.CompareTo("8.10.050") < 0)
		{
			ExecuteCommand(databaseName, "Update DDTables Set dttable = RTrim(dttable), dtDisplayName = RTrim(dtDisplayName), dtcaption = RTrim(dtcaption), dtParentTable = RTrim(dtParentTable), dtDefaultObjectId = RTrim(dtDefaultObjectId), dtgridid = RTrim(dtgridid), dtKeyFields = RTrim(dtKeyFields), dtKeyGroup = RTrim(dtKeyGroup), dtmodule = RTrim(dtmodule), dtaddfld1 = RTrim(dtaddfld1), dtaddfld2 = RTrim(dtaddfld2), dtaddfld3 = RTrim(dtaddfld3), dtColorExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dtColorExpression),'')) = '' Then Null Else dtColorExpression End, dtInitialValue = RTrim(dtInitialValue), dtprefix = RTrim(dtprefix), dtPrefixUser = RTrim(dtPrefixUser), dtReadonlyExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dtReadonlyExpression),'')) = '' Then Null Else dtReadonlyExpression End, dtDisableAddNewExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dtDisableAddNewExpression),'')) = '' Then Null Else dtDisableAddNewExpression End, dtDisableDeleteExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dtDisableDeleteExpression),'')) = '' Then Null Else dtDisableDeleteExpression End, dtFieldToCheckOnUpdate = RTrim(dtFieldToCheckOnUpdate), dtvtbl = RTrim(dtvtbl), dtv3tb = RTrim(dtv3tb), dtutbl = RTrim(dtutbl), dtvrel = RTrim(dtvrel), dtChangeDetailIdsFilter = RTrim(dtChangeDetailIdsFilter), dtForeignKeyDeleteFilter = RTrim(dtForeignKeyDeleteFilter), dtCurrencyModeLocationField = RTrim(dtCurrencyModeLocationField), dtCurrencyRateIdField = RTrim(dtCurrencyRateIdField), dtCurrencyCustomRateField = RTrim(dtCurrencyCustomRateField), dtCurrencyExchangeRateField = RTrim(dtCurrencyExchangeRateField), dtDocumentDateField = RTrim(dtDocumentDateField), dtContactField = RTrim(dtContactField), dtPromptOnAddField = RTrim(dtPromptOnAddField), dtUniqueField = RTrim(dtUniqueField), dtviewdef = Case When LTrim(IsNull(Convert(nvarchar(100),dtviewdef),'')) = '' Then Null Else dtviewdef End Where dtCustom <> 0");
			ExecuteCommand(databaseName, "UPDATE DDTables SET dtUAddFld1 = RTrim(dtUAddFld1), dtUAddFld2 = RTrim(dtUAddFld2),dtUAddFld3 = RTrim(dtUAddFld3), dtColorExpressionUser = Case When LTrim(IsNull(Convert(nvarchar(100),dtColorExpressionUser),'')) = '' Then Null Else dtColorExpressionUser End, dtReadonlyExpressionUser = Case When LTrim(IsNull(Convert(nvarchar(100),dtReadonlyExpressionUser),'')) = '' Then Null Else dtReadonlyExpressionUser End, dtDisableAddNewExpressionUser = Case When LTrim(IsNull(Convert(nvarchar(100),dtDisableAddNewExpressionUser),'')) = '' Then Null Else dtDisableAddNewExpressionUser End, dtDisableDeleteExpressionUser = Case When LTrim(IsNull(Convert(nvarchar(100),dtDisableDeleteExpressionUser),'')) = '' Then Null Else dtDisableDeleteExpressionUser End");
			ExecuteCommand(databaseName, "UPDATE DDFields SET dftable = RTrim(dftable), dffield = RTrim(dffield), dfDisplayName = RTrim(dfDisplayName), dfCalculationExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dfCalculationExpression),'')) = '' Then Null Else dfCalculationExpression End, dfRequiredExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dfRequiredExpression),'')) = '' Then Null Else dfRequiredExpression End, dfoltype = RTrim(dfoltype), dfolrelfld = RTrim(dfolrelfld), dfformat = RTrim(dfformat), dfDefaultExpression = RTrim(dfDefaultExpression), dfReadonlyExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dfReadonlyExpression),'')) = '' Then Null Else dfReadonlyExpression End, dfmodule = RTrim(dfmodule), dfBoundParentField = RTrim(dfBoundParentField), dfBoundParentFieldProxy = RTrim(dfBoundParentFieldProxy), dfCurrencyRelatedField = RTrim(dfCurrencyRelatedField), dfRelatedTable = RTrim(dfRelatedTable), dfRelatedFields = RTrim(dfRelatedFields), dfffil = RTrim(dfffil), dfForeignKeyRequiredExpression = Case When LTrim(IsNull(Convert(nvarchar(100),dfForeignKeyRequiredExpression),'')) = '' Then Null Else dfForeignKeyRequiredExpression End, dfValueList = Case When LTrim(IsNull(Convert(nvarchar(100),dfValueList),'')) = '' Then Null Else dfValueList End, dfRelatedTableSearchGridId = RTrim(dfRelatedTableSearchGridId), dfRelatedTableReturnField = RTrim(dfRelatedTableReturnField), dfRelatedTabledescriptionField = RTrim(dfRelatedTabledescriptionField), dfRelatedTableOrderByField = RTrim(dfRelatedTableOrderByField), dfRelatedTableFilter = Case When LTrim(IsNull(Convert(nvarchar(100),dfRelatedTableFilter),'')) = '' Then Null Else dfRelatedTableFilter End, dfhelp = Case When LTrim(IsNull(Convert(nvarchar(100),dfhelp),'')) = '' Then Null Else dfhelp End, dfgroup = RTrim(dfgroup), dfstatus = RTrim(dfstatus), dfconv = RTrim(dfconv), dfvfld = RTrim(dfvfld), dfv3fd = RTrim(dfv3fd), dfcomments = Case When LTrim(IsNull(Convert(nvarchar(100),dfcomments),'')) = '' Then Null Else dfcomments End Where dfCustom <> 0");
			ExecuteCommand(databaseName, "UPDATE DDFields SET dfCaption = RTrim(dfCaption),dfDefaultExpressionUser = RTrim(dfDefaultExpressionUser), dfRequiredExpressionUser = Case When LTrim(IsNull(Convert(nvarchar(100),dfRequiredExpressionUser),'')) = '' Then Null Else dfRequiredExpressionUser End, dfReadonlyExpressionUser = Case When LTrim(IsNull(Convert(nvarchar(100),dfReadonlyExpressionUser),'')) = '' Then Null Else dfReadonlyExpressionUser End, dfForeignKeyRequiredExpressionUser = Case When LTrim(IsNull(Convert(nvarchar(100),dfForeignKeyRequiredExpressionUser),'')) = '' Then Null Else dfForeignKeyRequiredExpressionUser End ");
			ExecuteCommand(databaseName, "Update DDRelations Set drptable = RTrim(drptable), drpfield = RTrim(drpfield), drctable = RTrim(drctable), drcfield = RTrim(drcfield), drfilter = RTrim(drfilter), drdfilter = RTrim(drdfilter) Where drCustom <> 0");
			ExecuteCommand(databaseName, "Update DDObjectDetails Set dlobjectid = RTrim(dlobjectid), dltable = RTrim(dltable), dlparent = RTrim(dlparent), dlview = RTrim(dlview), dlsearchid = RTrim(dlsearchid), dlgridid = RTrim(dlgridid), dlorder = RTrim(dlorder), dluorder = Case When LTrim(IsNull(Convert(nvarchar(100),dluorder),'')) = '' Then Null Else dluorder End, dlcfield = RTrim(dlcfield), dlfilter = RTrim(dlfilter), dlhide = RTrim(dlhide) Where dlCustom <> 0");
			ExecuteCommand(databaseName, "Update DDObjects Set doobjectid = RTrim(doobjectid), dotable = RTrim(dotable), doname = RTrim(doname), dotitle = RTrim(dotitle), domodule = RTrim(domodule) Where doCustom <> 0");
			ExecuteCommand(databaseName, "Update DDForms Set dmformid = RTrim(dmformid), dmtable = RTrim(dmtable), dmcaption = RTrim(dmcaption), dmhelplink = RTrim(dmhelplink), dmvid = RTrim(dmvid), dmdesgroup = RTrim(dmdesgroup) Where dmCustom <> 0");
			ExecuteCommand(databaseName, "Update DDFormDetails Set deFormID = RTrim(deFormID),deControlName = RTrim(deControlName), deClassID = RTrim(deClassID), deProperties = Case When LTrim(IsNull(Convert(nvarchar(100),deProperties),'')) = '' Then Null Else deProperties End ");
			ExecuteCommand(databaseName, "Update DDGrids Set djGridID = RTrim(djGridID), djUserID = RTrim(djUserID), djTable = RTrim(djTable), DJDESC = RTrim(DJDESC), DJEXTD = Case When LTrim(IsNull(Convert(nvarchar(100),DJEXTD),'')) = '' Then Null Else DJEXTD End Where djCustom <> 0");
			ExecuteCommand(databaseName, "Update DDGridDetails Set dgGridID = RTrim(dgGridID), dgUserID = RTrim(dgUserID), DGGRP = Case When LTrim(IsNull(Convert(nvarchar(100),DGGRP),'')) = '' Then Null Else DGGRP End, DGORD = Case When LTrim(IsNull(Convert(nvarchar(100),DGORD),'')) = '' Then Null Else DGORD End, DGFLDS = Case When LTrim(IsNull(Convert(nvarchar(100),DGFLDS),'')) = '' Then Null Else DGFLDS End, DGFROM = Case When LTrim(IsNull(Convert(nvarchar(100),DGFROM),'')) = '' Then Null Else DGFROM End, DGREQOPT = Case When LTrim(IsNull(Convert(nvarchar(100),DGREQOPT),'')) = '' Then Null Else DGREQOPT End, DGWHER = Case When LTrim(IsNull(Convert(nvarchar(100),DGWHER),'')) = '' Then Null Else DGWHER End, DGSGRP = Case When LTrim(IsNull(Convert(nvarchar(100),DGSGRP),'')) = '' Then Null Else DGSGRP End, DGSORD = Case When LTrim(IsNull(Convert(nvarchar(100),DGSORD),'')) = '' Then Null Else DGSORD End, dgSQLSet = Case When LTrim(IsNull(Convert(nvarchar(100),dgSQLSet),'')) = '' Then Null Else dgSQLSet End, dgADOSet = Case When LTrim(IsNull(Convert(nvarchar(100),dgADOSet),'')) = '' Then Null Else dgADOSet End, dgDatasets = Case When LTrim(IsNull(Convert(nvarchar(100),dgDatasets),'')) = '' Then Null Else dgDatasets End, DGSPGROUP = RTrim(DGSPGROUP), DGSPTEXT = RTrim(DGSPTEXT), DGSPCALC = Case When LTrim(IsNull(Convert(nvarchar(100),DGSPCALC),'')) = '' Then Null Else DGSPCALC End, dgCalDateF = RTrim(dgCalDateF), dgSFormula = Case When LTrim(IsNull(Convert(nvarchar(100),dgSFormula),'')) = '' Then Null Else dgSFormula End Where dgCustom <> 0");
			ExecuteCommand(databaseName, "Update DDSearches Set dsSearchID = RTrim(dsSearchID), dsUserID = RTrim(dsUserID), dsGridID = RTrim(dsGridID), dsField = RTrim(dsField), dsPreviousGrids = Case When LTrim(IsNull(Convert(nvarchar(100),dsPreviousGrids),'')) = '' Then Null Else dsPreviousGrids End ");
			ExecuteCommand(databaseName, "Update DDOpenWiths Set dwID = RTrim(dwID), dwTable = RTrim(dwTable), dwField = RTrim(dwField), dwDesc = RTrim(dwDesc), dwObject = RTrim(dwObject), dwHide = Case When LTrim(IsNull(Convert(nvarchar(100),dwHide),'')) = '' Then Null Else dwHide End, dwUHide = Case When LTrim(IsNull(Convert(nvarchar(100),dwUHide),'')) = '' Then Null Else dwUHide End, dwCode = Case When LTrim(IsNull(Convert(nvarchar(100),dwCode),'')) = '' Then Null Else dwCode End Where dwCustom <> 0");
		}
		if (fromVersion.CompareTo("9.00.014") < 0)
		{
			ExecuteCommand(databaseName, "Update DDUsers Set duPortal = Replace(CAST(duPortal AS varchar(MAX)), ',', '~')");
		}
		if (fromVersion.CompareTo("9.00.036") < 0)
		{
			string arg2 = "ALTER TABLE WebSessions ADD ";
			string text3 = "weParentSessionID";
			if (!dmo.DoesFieldExist(null, null, databaseName, "WebSessions", text3))
			{
				ExecuteCommand(databaseName, $"{arg2} {text3} UNIQUEIDENTIFIER DEFAULT NULL");
			}
		}
		if (fromVersion.CompareTo("9.00.054") < 0)
		{
			string arg3 = "ALTER TABLE DDExplorer ADD ";
			string text4 = "dxLanguageID";
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDExplorer", text4))
			{
				ExecuteCommand(databaseName, $"{arg3} {text4} NVARCHAR(55) NOT NULL DEFAULT '' ");
			}
		}
		if (fromVersion.CompareTo("9.00.085") < 0 && dmo.DoesTableExist(null, null, databaseName, "DDSearches"))
		{
			ReloadTable(databaseName, "DDSearches", recreateTable: true, msgDelegate, ddDef);
		}
		if (fromVersion.CompareTo("9.00.118") < 0 && dmo.DoesTableExist(null, null, databaseName, "DDUsers"))
		{
			updateMyFolderUserSettings(databaseName);
		}
		if (fromVersion.CompareTo("9.1.006") < 0 && dmo.DoesTableExist(null, null, databaseName, "DDFormDetails"))
		{
			updateInputMaskForNumericControls(databaseName);
		}
		if (fromVersion.CompareTo("9.1.035") < 0 && dmo.DoesTableExist(null, null, databaseName, "DDExplorer"))
		{
			ReloadTable(databaseName, "DDExplorer", recreateTable: true, msgDelegate, ddDef);
		}
		if (fromVersion.CompareTo("9.1.054") < 0 && dmo.DoesTableExist(null, null, databaseName, "DDFields"))
		{
			ReloadTable(databaseName, "DDFields", recreateTable: true, msgDelegate, ddDef);
		}
		if (fromVersion.CompareTo("9.1.064") < 0)
		{
			ExecuteCommand(databaseName, "Update DDFormDetails Set deClassID = Replace(deClassID, 'M1CONTROLS90.', 'M1CONTROLS92.')");
			ExecuteCommand(databaseName, "Update DDFormDetails Set deClassID = Replace(deClassID, 'M1CONTROLS91.', 'M1CONTROLS92.')");
		}
		if (fromVersion.CompareTo("9.2.050") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtOverrideDelete"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables Add dtOverrideDelete nvarchar(150) Not Null Default ''");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtOverrideDeleteEnabledExpression"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDTables Add dtOverrideDeleteEnabledExpression nvarchar(max)");
			}
		}
		if (fromVersion.CompareTo("9.2.065") < 0 && dmo.DoesFieldExist(null, null, databaseName, "DDTables", "dtOverrideDelete"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDTables ALTER COLUMN dtOverrideDelete nvarchar(150) Not Null");
		}
		if (fromVersion.CompareTo("9.2.347") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "WebSessions", "weExpirationTime"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE WebSessions ADD weExpirationTime int");
		}
		if (fromVersion.CompareTo("9.2.575") < 0)
		{
			if (dmo.DoesFieldExist(null, null, databaseName, "DDSecurityGroups", "dzDataset"))
			{
				ExecuteCommand(databaseName, "EXEC sp_rename 'DDSecurityGroups.dzDataset', 'dzDataset_A', 'COLUMN'");
				ExecuteCommand(databaseName, "ALTER TABLE DDSecurityGroups Add dzDataset NVARCHAR(10) Not Null Default('')");
				ExecuteCommand(databaseName, "Update DDSecurityGroups Set dzDataset = dzDataset_A");
				dmo.DropColumn(null, null, null, databaseName, "DDSecurityGroups", "dzDataset_A", dropTriggers: false);
				ReloadTable(databaseName, "DDSecurityGroups", recreateTable: true, msgDelegate, ddDef);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDSecurityReports", "drDataset"))
			{
				ExecuteCommand(databaseName, "EXEC sp_rename 'DDSecurityReports.drDataset', 'drDataset_A', 'COLUMN'");
				ExecuteCommand(databaseName, "ALTER TABLE DDSecurityReports Add drDataset NVARCHAR(10) Not Null Default('')");
				ExecuteCommand(databaseName, "Update DDSecurityReports Set drDataset = drDataset_A");
				dmo.DropColumn(null, null, null, databaseName, "DDSecurityReports", "drDataset_A", dropTriggers: false);
				ReloadTable(databaseName, "DDSecurityReports", recreateTable: true, msgDelegate, ddDef);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDSecurityTables", "dtDataset"))
			{
				ExecuteCommand(databaseName, "EXEC sp_rename 'DDSecurityTables.dtDataset', 'dtDataset_A', 'COLUMN'");
				ExecuteCommand(databaseName, "ALTER TABLE DDSecurityTables Add dtDataset NVARCHAR(10) Not Null Default('')");
				ExecuteCommand(databaseName, "Update DDSecurityTables Set dtDataset = dtDataset_A");
				dmo.DropColumn(null, null, null, databaseName, "DDSecurityTables", "dtDataset_A", dropTriggers: false);
				ReloadTable(databaseName, "DDSecurityTables", recreateTable: true, msgDelegate, ddDef);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "WebSessions", "weDataset"))
			{
				ExecuteCommand(databaseName, "EXEC sp_rename 'WebSessions.weDataset', 'weDataset_A', 'COLUMN'");
				ExecuteCommand(databaseName, "ALTER TABLE WebSessions Add weDataset NVARCHAR(10) Not Null Default('')");
				ExecuteCommand(databaseName, "Update WebSessions Set weDataset = weDataset_A");
				dmo.DropColumn(null, null, null, databaseName, "WebSessions", "weDataset_A", dropTriggers: false);
				ReloadTable(databaseName, "WebSessions", recreateTable: true, msgDelegate, ddDef);
			}
		}
		if (fromVersion.CompareTo("9.2.575") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDInfo", "ddHosted"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDInfo Add ddHosted bit Not Null Default(0)");
		}
		if (fromVersion.CompareTo("9.2.575") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDInfo", "ddEasyOrder"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDInfo Add ddEasyOrder NVARCHAR(max)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDInfo", "ddEDI"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDInfo Add ddEDI NVARCHAR(max)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDInfo", "ddMobile"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDInfo Add ddMobile NVARCHAR(max)");
			}
		}
		if (fromVersion.CompareTo("9.2.611") < 0 && dmo.DoesTableExist(null, null, databaseName, "DDGridDetails"))
		{
			ReloadTable(databaseName, "DDGridDetails", recreateTable: true, msgDelegate, ddDef);
		}
		if (fromVersion.CompareTo("9.2.639") < 0)
		{
			if (dmo.DoesFieldExist(null, null, databaseName, "DDSecurityGroups", "dzDataset"))
			{
				ExecuteCommand(databaseName, "EXEC sp_rename 'DDSecurityGroups.dzDataset', 'dzDataset_A', 'COLUMN'");
				ExecuteCommand(databaseName, "ALTER TABLE DDSecurityGroups Add dzDataset NVARCHAR(25) Not Null Default('')");
				ExecuteCommand(databaseName, "Update DDSecurityGroups Set dzDataset = dzDataset_A");
				dmo.DropColumn(null, null, null, databaseName, "DDSecurityGroups", "dzDataset_A", dropTriggers: false);
				ReloadTable(databaseName, "DDSecurityGroups", recreateTable: true, msgDelegate, ddDef);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDSecurityReports", "drDataset"))
			{
				ExecuteCommand(databaseName, "EXEC sp_rename 'DDSecurityReports.drDataset', 'drDataset_A', 'COLUMN'");
				ExecuteCommand(databaseName, "ALTER TABLE DDSecurityReports Add drDataset NVARCHAR(25) Not Null Default('')");
				ExecuteCommand(databaseName, "Update DDSecurityReports Set drDataset = drDataset_A");
				dmo.DropColumn(null, null, null, databaseName, "DDSecurityReports", "drDataset_A", dropTriggers: false);
				ReloadTable(databaseName, "DDSecurityReports", recreateTable: true, msgDelegate, ddDef);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDSecurityTables", "dtDataset"))
			{
				ExecuteCommand(databaseName, "EXEC sp_rename 'DDSecurityTables.dtDataset', 'dtDataset_A', 'COLUMN'");
				ExecuteCommand(databaseName, "ALTER TABLE DDSecurityTables Add dtDataset NVARCHAR(25) Not Null Default('')");
				ExecuteCommand(databaseName, "Update DDSecurityTables Set dtDataset = dtDataset_A");
				dmo.DropColumn(null, null, null, databaseName, "DDSecurityTables", "dtDataset_A", dropTriggers: false);
				ReloadTable(databaseName, "DDSecurityTables", recreateTable: true, msgDelegate, ddDef);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "WebSessions", "weDataset"))
			{
				ExecuteCommand(databaseName, "EXEC sp_rename 'WebSessions.weDataset', 'weDataset_A', 'COLUMN'");
				ExecuteCommand(databaseName, "ALTER TABLE WebSessions Add weDataset NVARCHAR(25) Not Null Default('')");
				ExecuteCommand(databaseName, "Update WebSessions Set weDataset = weDataset_A");
				dmo.DropColumn(null, null, null, databaseName, "WebSessions", "weDataset_A", dropTriggers: false);
				ReloadTable(databaseName, "WebSessions", recreateTable: true, msgDelegate, ddDef);
			}
			if (dmo.DoesFieldExist(null, null, databaseName, "DDUserLog", "ulDatabase"))
			{
				ExecuteCommand(databaseName, "EXEC sp_rename 'DDUserLog.ulDatabase', 'ulDatabase_A', 'COLUMN'");
				ExecuteCommand(databaseName, "ALTER TABLE DDUserLog Add ulDatabase NVARCHAR(25) Not Null Default('')");
				ExecuteCommand(databaseName, "Update DDUserLog Set ulDatabase = ulDatabase_A");
				dmo.DropColumn(null, null, null, databaseName, "DDUserLog", "ulDatabase_A", dropTriggers: false);
				ReloadTable(databaseName, "DDUserLog", recreateTable: true, msgDelegate, ddDef);
			}
		}
		if (fromVersion.CompareTo("9.2.754") < 0 && !dmo.DoesTableExist(null, null, databaseName, "DDAPIInfo", null))
		{
			CreateDataDictionaryTables(null, databaseName, "DDAPIInfo", string.Empty, ddDef);
		}
		if (fromVersion.CompareTo("9.2.827") < 0 && dmo.DoesFieldExist(null, null, databaseName, "DDInfo", "ddProductCode"))
		{
			ExecuteCommand(databaseName, "DECLARE @DropDefaultProductCode NVARCHAR(MAX);SELECT @DropDefaultProductCode = q FROM(SELECT 'ALTER TABLE [dbo].[DDInfo] DROP CONSTRAINT ' + name + ';' FROM[sys].[objects] where type_desc = 'DEFAULT_CONSTRAINT' and name Like 'DF__DDInfo__ddProduc__%') T(q);EXEC(@DropDefaultProductCode);");
			ExecuteCommand(databaseName, "ALTER TABLE[dbo].[DDInfo] ALTER COLUMN ddProductCode VARCHAR(16)");
			ExecuteCommand(databaseName, "ALTER TABLE[dbo].[DDInfo] ADD DEFAULT('') FOR[ddProductCode]");
		}
		if (fromVersion.CompareTo("9.2.828") < 0 && !dmo.DoesTableExist(null, null, databaseName, "IntegrationServiceInfo", null))
		{
			CreateDataDictionaryTables(null, databaseName, "IntegrationServiceInfo", string.Empty, ddDef);
		}
		if (fromVersion.CompareTo("9.4.101") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "IntegrationServiceInfo", "diTenantId"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE IntegrationServiceInfo Add diTenantId nvarchar(36) Null");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "IntegrationServiceInfo", "diIsSynced"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE IntegrationServiceInfo ADD diIsSynced bit Not Null Default(0)");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDInfo", "ddCompanyId"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDInfo Add ddCompanyId NVARCHAR(36)");
			}
		}
		if (fromVersion.CompareTo("9.4.200") < 0)
		{
			if (DoesTableExist(null, databaseName, "DDVisualizers"))
			{
				ExecuteCommand(databaseName, "UPDATE DDVisualizers SET dvCustom = 0 WHERE dvUserID LIKE '' AND dvCustom = 1");
			}
			if (DoesTableExist(null, databaseName, "DDSeries"))
			{
				ExecuteCommand(databaseName, "UPDATE DDSeries SET diCustom = 0 WHERE diUserID LIKE '' AND diCustom = 1");
			}
		}
		if (fromVersion.CompareTo("9.5.300") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDInfo", "ddHomeEnabled"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDInfo ADD ddHomeEnabled bit Not Null Default(0)");
		}
		if (fromVersion.CompareTo("9.5.350") < 0)
		{
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDUsers", "duCloudPrincipalId"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDUsers ADD duCloudPrincipalId uniqueidentifier Null");
			}
			if (!dmo.DoesFieldExist(null, null, databaseName, "DDUsers", "duPortalUserEmail"))
			{
				ExecuteCommand(databaseName, "ALTER TABLE DDUsers ADD duPortalUserEmail nvarchar(250) Null");
			}
		}
		if (fromVersion.CompareTo("9.5.500") < 0 && dmo.DoesFieldExist(null, null, databaseName, "DDInfo", "ddHomeEnabled"))
		{
			dmo.DropColumn(null, null, null, databaseName, "DDInfo", "ddHomeEnabled", dropTriggers: true);
		}
		if (fromVersion.CompareTo("9.6.200") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDInfo", "ddWebRegion"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDInfo Add ddWebRegion NVARCHAR(20)");
		}
		if (fromVersion.CompareTo("9.6.300") < 0 && dmo.DoesFieldExist(null, null, databaseName, "DDOpenWiths", "dwObject"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDOpenWiths ALTER COLUMN dwObject NVARCHAR(50)");
		}
		if (fromVersion.CompareTo("9.7.400") < 0 && !dmo.DoesFieldExist(null, null, databaseName, "DDAPIInfo", "daIsReadOnly"))
		{
			ExecuteCommand(databaseName, "ALTER TABLE DDAPIInfo ADD daIsReadOnly bit Not Null Default(0)");
		}
	}

	private void updateDDSecurityGroups(string databaseName, Dmo dmoInstance, string serverCollation)
	{
		string empty = string.Empty;
		bool flag = dmoInstance.DoesFieldExist(null, null, databaseName, "DDUsers", "duExportLocked");
		bool flag2 = dmoInstance.DoesFieldExist(null, null, databaseName, "DDUsers", "duMailMergeLocked");
		bool flag3 = dmoInstance.DoesFieldExist(null, null, databaseName, "DDUsers", "duMailReportsLocked");
		foreach (DatabaseInfo installedDatabase in currentContext.InstalledDatabases)
		{
			empty = installedDatabase.Name.ToUpper();
			if (empty.Length <= 5)
			{
				if (flag)
				{
					ExecuteCommand(databaseName, $"Insert Into DDSecurityGroups (dzGroupID, dzUserID, dzDataset) Select 'EXPORT',RTrim(duUserID),{empty.ToSql()} From DDUsers Where duExportLocked = 0");
				}
				if (flag2)
				{
					ExecuteCommand(databaseName, $"Insert Into DDSecurityGroups (dzGroupID, dzUserID, dzDataset) Select 'MAILMERGE',RTrim(duUserID), {empty.ToSql()}  From DDUsers Where duMailMergeLocked = 0");
				}
				if (flag3)
				{
					ExecuteCommand(databaseName, $"Insert Into DDSecurityGroups (dzGroupID, dzUserID, dzDataset) Select 'EMAILREPORTS',RTrim(duUserID), {empty.ToSql()} From DDUsers Where duMailReportsLocked = 0");
				}
				if (dmoInstance.DoesFieldExist(null, null, databaseName, "DDUsers", "duComponentDefault"))
				{
					ExecuteCommand(databaseName, $"Insert Into DDSecurityGroups (dzGroupID, dzUserID, dzDataset) Select 'CONVCOMPDEF',RTrim(duUserID), {empty.ToSql()}  From DDUsers Where duComponentDefault = 0");
				}
				else
				{
					ExecuteCommand(databaseName, $"Insert Into DDSecurityGroups (dzGroupID, dzUserID, dzDataset) Select 'CONVCOMPDEF',RTrim(duUserID), {empty.ToSql()} From DDUsers ");
				}
			}
		}
		if (DoesTableExist(null, databaseName, "DDSecurityComponents") && dmoInstance.DoesFieldExist(null, null, databaseName, "DDUsers", "duComponentDefault"))
		{
			ExecuteCommand(databaseName, string.Format("Insert Into DDSecurityGroups (dzGroupID, dzUserID, dzDataset) Select dcComponent, dcUserID, 'M1_' + dcDataset COLLATE {0} From DDSecurityComponents Inner Join DDUsers On dcUserID = duUserID COLLATE {0} Where dcLevel = 2 and duComponentDefault <> 0", serverCollation));
		}
	}

	private void updateDDSecurityGroupsStep2(string databaseName, Dmo dmoInstance, string serverCollation)
	{
		ExecuteCommand(databaseName, $"Insert Into DDSecurityGroups (dzGroupID, dzUserID, dzDataset) Select groups.duUserID As dzGroupID, users.dzUserID, users.dzDataset From DDUsers groups, (Select dzUserID, dzDataset From DDUsers Inner Join DDSecurityGroups On duUserID = dzUserID COLLATE {serverCollation} Where dzGroupID = 'CONVCOMPDEF') as users Where duType = 2  And duUserID Not In ('EXPORT', 'MAILMERGE', 'EMAILREPORTS')");
		ExecuteCommand(databaseName, string.Format("Delete DDSecurityGroups From DDSecurityGroups Inner Join DDSecurityComponents On dzGroupID = dcComponent COLLATE {0} And dzUserID = dcUserID COLLATE {0} And dzDataset = 'M1_' + dcDataset COLLATE {0} Where dcLevel = 1", serverCollation));
		dmoInstance.DropTable(null, null, databaseName, "DDSecurityComponents");
		ExecuteCommand(databaseName, "Delete From DDSecurityGroups Where dzGroupID = 'CONVCOMPDEF'");
	}

	private void updateDDAddDefaultDbSecurity(string databaseName, Dmo dmoInstance, string serverCollation)
	{
		string empty = string.Empty;
		foreach (DatabaseInfo installedDatabase in currentContext.InstalledDatabases)
		{
			empty = installedDatabase.Name.ToUpper();
			ExecuteCommand(databaseName, string.Format("Insert Into DDSecurityTables (dtUserID, dtDataset, dtLevel) Select RTrim(duUserId), " + empty.ToSql() + ", 28 From DDUsers Where duUserId Not In (Select dtUserID COLLATE {0} From DDSecurityTables Where dtDataset = " + empty.ToSql() + " And dtTable = '')", serverCollation));
		}
		if (dmoInstance.DoesFieldExist(null, null, databaseName, "DDUsers", "duTableDefault"))
		{
			ExecuteCommand(databaseName, $"Update DDSecurityTables Set dtLevel = Case When duTableDefault = 1 Then 0 Else duTableDefault End From DDSecurityTables Inner Join DDUsers On dtUserID = duUserID COLLATE {serverCollation} Where dtTable = '' And dtField = '' And duTableDefault <> 0 And dtLevel <> 1");
		}
		if (dmoInstance.DoesFieldExist(null, null, databaseName, "DDUsers", "duViewOnly"))
		{
			ExecuteCommand(databaseName, $"Update DDSecurityTables Set dtLevel = 2 From DDSecurityTables Inner Join DDUsers On dtUserID = duUserID COLLATE {serverCollation} Where (dtLevel = 0 Or dtLevel > 2) And duViewOnly <> 0");
		}
	}

	private void updateDDSecurityReports(string databaseName, string serverCollation)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		string empty5 = string.Empty;
		List<string> reportFolders = currentContext.Reports.GetReportFolders();
		DataTable dataTable = GetDataTable(databaseName, "SELECT duUserID,duReportDefault FROM TempUsers ORDER BY duUserID");
		bool needToClose = false;
		SqlConnection connection = currentContext.DDServerManager.GetConnection(null, databaseName, openImmediately: true, currentConnection, null, ref needToClose);
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable2 = GetDataTable(connection, databaseName, "Select * from DDSecurityReports WHERE 0=1", fillSchema: true, out adapter);
			foreach (string item in reportFolders)
			{
				ArrayList reportIniSettings = GetReportIniSettings(item + "\\" + item + ".ini");
				foreach (DatabaseInfo installedDatabase in currentContext.InstalledDatabases)
				{
					empty = installedDatabase.Name.ToUpper();
					empty5 = string.Empty;
					if (reportIniSettings != null)
					{
						empty2 = empty;
						if (empty2.StartsWith("M1_", StringComparison.CurrentCultureIgnoreCase))
						{
							empty2 = empty2.Substring(3);
						}
						foreach (string item2 in reportIniSettings)
						{
							if (item2.StartsWith($"DefaultReport\\{empty2}=", StringComparison.CurrentCultureIgnoreCase))
							{
								empty5 = $"DefaultReport={item2.Substring($"DefaultReport\\{empty2}=".Length)}";
								break;
							}
						}
					}
					foreach (DataRow row3 in dataTable.Rows)
					{
						empty3 = row3.Field<string>("duUserID").Trim();
						DataRow dataRow = dataTable2.AddBlankRow();
						dataRow.BeginEdit();
						dataRow.SetField("drReport", string.Empty);
						dataRow.SetField("drFolder", item);
						dataRow.SetField("drDataset", empty);
						dataRow.SetField("drUserID", empty3);
						dataRow.SetField("drLevel", (!(row3.Field<decimal>("duReportDefault") == 0m)) ? SecurityAccessLevel.None : SecurityAccessLevel.View);
						dataRow.SetField("drSettings", (empty5.Length == 0) ? null : empty5);
						dataRow.EndEdit();
					}
				}
				foreach (FileInfo item3 in currentContext.Reports.GetReportsForTemplate(item, string.Empty))
				{
					empty4 = Path.GetFileNameWithoutExtension(item3.Name);
					foreach (DataRow row4 in dataTable.Rows)
					{
						empty3 = row4.Field<string>("duUserID").Trim();
						empty5 = string.Empty;
						if (reportIniSettings != null)
						{
							empty5 = getReportIniSettingsText(item, empty4, empty3, reportIniSettings);
						}
						foreach (DatabaseInfo installedDatabase2 in currentContext.InstalledDatabases)
						{
							empty = installedDatabase2.Name.ToUpper();
							DataRow dataRow2 = dataTable2.AddBlankRow();
							dataRow2.BeginEdit();
							dataRow2.SetField("drReport", empty4);
							dataRow2.SetField("drFolder", item);
							dataRow2.SetField("drDataset", empty);
							dataRow2.SetField("drUserID", empty3);
							dataRow2.SetField("drLevel", (!(row4.Field<decimal>("duReportDefault") == 0m)) ? SecurityAccessLevel.None : SecurityAccessLevel.View);
							dataRow2.SetField("drSettings", (empty5.Length == 0) ? null : empty5);
							dataRow2.EndEdit();
						}
					}
				}
			}
			ExecuteCommand(databaseName, "Select drReport, drUserID, drDataset, drLevel Into TempReports From DDSecurityReports");
			try
			{
				ExecuteCommand(databaseName, "Update TempReports Set drLevel = 2 Where drLevel = 0");
				ExecuteCommand(databaseName, "Truncate Table DDSecurityReports");
				adapter.SelectCommand.Connection = connection;
				new SqlCommandBuilder(adapter);
				adapter.Update(dataTable2);
				ExecuteCommand(databaseName, string.Format("Update DDSecurityReports Set DDSecurityReports.drLevel = TempReports.drLevel From DDSecurityReports Inner Join TempReports On DDSecurityReports.drReport = TempReports.drReport COLLATE {0} And DDSecurityReports.drUserID = TempReports.drUserID COLLATE {0} And DDSecurityReports.drDataset = TempReports.drDataset COLLATE {0}", serverCollation));
				ExecuteCommand(databaseName, string.Format("Update DDSecurityReports Set drLevel = MaxLevel From DDSecurityReports Inner Join (select drFolder,drUserID,drDataset,max(drLevel) as MaxLevel from ddsecurityreports Group By drFolder,drUserID,drDataset) As MaxData On DDSecurityReports.drFolder = MaxData.drFolder COLLATE {0} And DDSecurityReports.drUserID = MaxData.drUserID COLLATE {0} And DDSecurityReports.drDataset = MaxData.drDataset COLLATE {0} Where DDSecurityReports.drReport = ''", serverCollation));
			}
			finally
			{
				ExecuteCommand(databaseName, "Drop Table TempReports");
			}
		}
		finally
		{
			if (needToClose)
			{
				connection.Close();
			}
			connection = null;
		}
	}

	private void updateComponents(string databaseName, string componentID)
	{
		foreach (DatabaseInfo installedDatabase in currentContext.InstalledDatabases)
		{
			string text = installedDatabase.Name.ToUpper();
			if (text.Length <= 5)
			{
				ExecuteCommand(databaseName, string.Format("Insert Into DDSecurityGroups (dzGroupID, dzUserID, dzDataset) Select '{0} ',RTrim(duUserID), {1} From DDUsers Where duType = 0 And duUserID Not In (Select dzUserID From DDSecurityGroups Where dzGroupID = '{2} ' And dzDataset = {1} )", componentID, text.ToSql(), componentID));
			}
		}
	}

	private void convertSettingToDD(string oldName, string newName, ref StringBuilder newProps)
	{
		string text = currentContext.Server.IniSettings.Get(oldName, string.Empty);
		currentContext.Server.IniSettings.Remove(oldName);
		if (text.Length != 0)
		{
			newProps.AppendFormat("{0} = {1} \r\n", newName, text);
		}
	}

	private StringBuilder CombineCodeForAllRows(DataTable data)
	{
		if (data != null && data.Rows.Count != 0)
		{
			List<DataRow> list = new List<DataRow>();
			List<DataRow> list2 = new List<DataRow>();
			List<DataRow> list3 = new List<DataRow>();
			DataRow[] array = data.Select(string.Empty, "deType,deControlName");
			foreach (DataRow dataRow in array)
			{
				if (dataRow.Field<string>("deClassID").Trim().Length == 0)
				{
					list.Add(dataRow);
				}
				else if (dataRow.Field<string>("deClassID").Trim().EndsWith("M1DataControl", StringComparison.CurrentCultureIgnoreCase))
				{
					list2.Add(dataRow);
				}
				else
				{
					list3.Add(dataRow);
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			CombineCodeForAllRows(list, stringBuilder);
			CombineCodeForAllRows(list2, stringBuilder);
			CombineCodeForAllRows(list3, stringBuilder);
			return stringBuilder;
		}
		return null;
	}

	private void CombineCodeForAllRows(List<DataRow> rows, StringBuilder allCode)
	{
		if (rows == null || rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in rows)
		{
			string text = row.Field<string>("deCode");
			if (text != null && text.Length != 0)
			{
				if (allCode.Length != 0 && allCode[allCode.Length - 1] != '\r' && allCode[allCode.Length - 1] != '\n')
				{
					allCode.Append('\r');
				}
				allCode.Append(text);
			}
		}
		allCode.Replace("\r\n", "\r");
	}

	private void convertTableAndFieldCode(string databaseName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		SqlDataAdapter adapter;
		DataTable dataTable = GetDataTable(null, databaseName, "Select * From DDCode Where 0=1", fillSchema: true, out adapter);
		DataTable dataTable2 = GetDataTable(null, databaseName, "Select dtTable,dtTriggersCode,dtTriggersCodeUser,dtUniqueID,dtCustom From DDTables");
		DataTable dataTable3 = GetDataTable(null, databaseName, "Select dfTable,dfField,dfCustom,dfChangeCode,dfChangeCodeUser,dfButtonCode,dfButtonCodeUser,dfButtonLabel,dfButtonImage,dfButtonImageUser,dfButtonToolTip,dfButtonToolTipUser,dfValidCode,dfValidCodeUser,dfForeignKeyValidCode,dfForeignKeyValidCodeUser From DDFields");
		SqlDataAdapter adapter2;
		DataTable dataTable4 = GetDataTable(null, databaseName, "Select * From DDOpenWiths Where 0=1", fillSchema: true, out adapter2);
		foreach (DataRow row in dataTable2.Rows)
		{
			stringBuilder.Length = 0;
			if (Convert.ToBoolean(row["dtCustom"]))
			{
				convertTableTriggers(row, "dtTriggersCode", stringBuilder);
			}
			convertTableTriggers(row, "dtTriggersCodeUser", stringBuilder);
			combineAllFieldCode(dataTable3.Select("dfTable = " + M1Util.ConvertToLinq(row.Field<string>("dtTable").Trim())), stringBuilder);
			convertButtonCode(dataTable3.Select("dfTable = " + M1Util.ConvertToLinq(row.Field<string>("dtTable").Trim())), dataTable4);
			convertTableDoSave(dataTable, stringBuilder, isCustom: true, row.Field<Guid>("dtUniqueID"));
		}
		if (dataTable.Rows.Count != 0)
		{
			UpdateData(null, null, databaseName, dataTable, adapter, null);
		}
		if (dataTable4.Rows.Count != 0)
		{
			UpdateData(null, null, databaseName, dataTable4, adapter2, null);
		}
	}

	private void convertTableDoSave(DataTable codeData, StringBuilder builder, bool isCustom, Guid uniqueID)
	{
		if (builder.Length != 0)
		{
			DataRow dataRow = codeData.NewRow().BlankRow();
			dataRow["dkCodeID"] = Guid.NewGuid();
			dataRow["dkSourceTable"] = "DDTables";
			dataRow["dkSourceUniqueID"] = uniqueID;
			dataRow["dkCode"] = builder.ToString();
			dataRow["dkCustom"] = isCustom;
			codeData.Rows.Add(dataRow);
		}
	}

	private void convertTableTriggers(DataRow row, string field, StringBuilder passedBuilder)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = row.Field<string>(field);
		if (text == null || text.Trim().Length == 0)
		{
			return;
		}
		stringBuilder.Append(text);
		if (stringBuilder.Length > 2)
		{
			if (stringBuilder[stringBuilder.Length - 2] != '\r')
			{
				stringBuilder.Append('\r');
			}
			if (stringBuilder[stringBuilder.Length - 2] != '\r')
			{
				stringBuilder.Append('\r');
			}
		}
		if (stringBuilder.Length != 0)
		{
			passedBuilder.Append(convertTriggerCode(stringBuilder.ToString(), row.Field<string>("dtTable").Trim()));
		}
	}

	private string convertTriggerCode(string code, string table)
	{
		code = Regex.Replace(code, table + "_AfterInsert", table + "_AddNewCompleted", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_UserAfterInsert", table + "_AddNewCompleted", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_Insert", table + "_SetDefaultValues", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_UserInsert", table + "_SetDefaultValues", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_UserValid =", table + "_Valid", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_Valid\\s*=", "e.AddError ", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_AfterRemove", table + "_RemoveCompleted", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_UserAfterRemove", table + "_RemoveCompleted", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_Update", table + "_UpdateStarted", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_UserUpdate", table + "_UpdateStarted", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_AfterUpdate", table + "_UpdateCompleted", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_UserAfterUpdate", table + "_UpdateCompleted", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_Delete", table + "_DeleteStarted", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_UserDelete", table + "_DeleteStarted", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_AfterDelete", table + "_DeleteCompleted", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_UserAfterDelete", table + "_DeleteCompleted", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_UserGetNextID", table + "_GetNextID", RegexOptions.IgnoreCase);
		code = Regex.Replace(code, table + "_UserSaveAs", table + "_SaveAs", RegexOptions.IgnoreCase);
		return code;
	}

	private void combineAllFieldCode(DataRow[] rows, StringBuilder customBuilder)
	{
		if (rows != null && rows.Length != 0)
		{
			foreach (DataRow row in rows)
			{
				convertFieldCode(row, customBuilder);
			}
		}
	}

	private void convertButtonCode(DataRow[] rows, DataTable openWithsTable)
	{
		if (rows != null && rows.Length != 0)
		{
			foreach (DataRow fieldRow in rows)
			{
				convertButtonCode(fieldRow, openWithsTable);
			}
		}
	}

	private void getCode(string signature, string fieldName, string code, StringBuilder codeBuilder, string findText, string replaceText)
	{
		if (code != null && code.Length != 0 && code.Trim().Length != 0)
		{
			if (findText.Length != 0)
			{
				code = Regex.Replace(code, findText, replaceText, RegexOptions.IgnoreCase);
			}
			codeBuilder.AppendFormat(signature, fieldName, code);
		}
	}

	private void convertButtonCode(DataRow fieldRow, DataTable openWithsTable)
	{
		string text = fieldRow.Field<string>("dfButtonCode");
		if (text != null && text.Trim().Length != 0 && Convert.ToBoolean(fieldRow["dfCustom"]))
		{
			text = text.Replace("Fields", "arg.BindingSource.Fields", caseInsensitive: true);
			DataRow dataRow = openWithsTable.AddBlankRow();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Function ProcessCommand(arg)");
			stringBuilder.AppendLine(text);
			stringBuilder.AppendLine("End Function");
			string text2 = "M1" + fieldRow.Field<string>("dfField").Trim().ToUpper() + "BTN";
			dataRow["dwID"] = text2.Substring(0, Math.Min(text2.Length, 30));
			dataRow["dwTable"] = fieldRow["dfTable"];
			dataRow["dwField"] = fieldRow["dfField"];
			if (fieldRow["dfButtonLabel"] == DBNull.Value || fieldRow.Field<string>("dfButtonLabel").Trim().Length == 0)
			{
				dataRow["dwDesc"] = fieldRow["dfButtonToolTip"];
			}
			else
			{
				dataRow["dwDesc"] = fieldRow["dfButtonLabel"];
			}
			dataRow["dwType"] = 6;
			dataRow["dwCustom"] = true;
			dataRow["dwSequence"] = 100;
			dataRow["dwCode"] = stringBuilder.ToString();
			dataRow["dwButtonImage"] = fieldRow["dfButtonImage"];
			dataRow["dwButtonImageUser"] = fieldRow["dfButtonImageUser"];
			dataRow["dwHide"] = DBNull.Value;
			dataRow["dwUHide"] = DBNull.Value;
			dataRow["dwEnabledExpression"] = DBNull.Value;
			dataRow["dwEnabledExpressionUser"] = DBNull.Value;
		}
	}

	private void convertFieldCode(DataRow row, StringBuilder customCodeBuilder)
	{
		string text = row.Field<string>("dfField");
		if (Convert.ToBoolean(row["dfCustom"]))
		{
			getCode("Function {0}_ValueChanged(sender, e)\r{1}\rEnd Function\r\r", text, row.Field<string>("dfChangeCode"), customCodeBuilder, text + "_Change", "e.Cancel");
			getCode("Function {0}_Valid(sender, e)\r{1}\rEnd Function\r\r", text, row.Field<string>("dfValidCode"), customCodeBuilder, text + "_Valid\\s*=", "e.AddError ");
			getCode("Function {0}_ForeignKeyValid(sender, e)\r{1}\rEnd Function\r\r", text, row.Field<string>("dfForeignKeyValidCode"), customCodeBuilder, string.Empty, string.Empty);
		}
		getCode("Function {0}_ValueChanged(sender, e)\r{1}\rEnd Function\r\r", text, row.Field<string>("dfChangeCodeUser"), customCodeBuilder, text + "_UserChange", "e.Cancel");
		getCode("Function {0}_Valid(sender, e)\r{1}\rEnd Function\r\r", text, row.Field<string>("dfValidCodeUser"), customCodeBuilder, text + "_UserValid\\s*=", "e.AddError ");
		getCode("Function {0}_ForeignKeyValid(sender, e)\r{1}\rEnd Function\r\r", text, row.Field<string>("dfForeignKeyValidCodeUser"), customCodeBuilder, string.Empty, string.Empty);
	}

	private void convertTriggerNames(string databaseName)
	{
		bool needToClose = false;
		SqlConnection connection = currentContext.DDServerManager.GetConnection(null, databaseName, openImmediately: true, currentConnection, null, ref needToClose);
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = GetDataTable(connection, databaseName, "Select * From DDTables Where Not dtTriggersCodeUser Is Null And Convert(nvarchar(max),dtTriggersCodeUser) like '%Trigger_%'", fillSchema: true, out adapter);
			adapter.SelectCommand.Connection = connection;
			foreach (DataRow row in dataTable.Rows)
			{
				if (row["dtTriggersCodeUser"] != DBNull.Value)
				{
					row["dtTriggersCodeUser"] = row.Field<string>("dtTriggersCodeUser").Replace("CustomTrigger_", row.Field<string>("dtTable").Trim() + "_User");
					row["dtTriggersCodeUser"] = row.Field<string>("dtTriggersCodeUser").Replace("Trigger_", row.Field<string>("dtTable").Trim() + "_");
				}
			}
			new SqlCommandBuilder(adapter);
			adapter.Update(dataTable);
			dataTable = GetDataTable(connection, databaseName, "Select * From DDTables Where Not dtTriggersCode Is Null And dtCustom <> 0 And Convert(nvarchar(max),dtTriggersCode) like '%Trigger_%'", fillSchema: true, out adapter);
			adapter.SelectCommand.Connection = connection;
			foreach (DataRow row2 in dataTable.Rows)
			{
				if (row2["dtTriggersCode"] != DBNull.Value)
				{
					row2["dtTriggersCode"] = row2.Field<string>("dtTriggersCode").Replace("CustomTrigger_", row2.Field<string>("dtTable").Trim() + "_User");
					row2["dtTriggersCode"] = row2.Field<string>("dtTriggersCode").Replace("Trigger_", row2.Field<string>("dtTable").Trim() + "_");
				}
			}
			new SqlCommandBuilder(adapter);
			adapter.Update(dataTable);
		}
		finally
		{
			if (needToClose)
			{
				connection.Close();
			}
			connection = null;
		}
	}

	public DmoDD(AppContext context)
	{
		currentContext = context;
	}

	public DmoDD(Dmo dmoRef, AppContext context)
	{
		currentContext = context;
		dmo = dmoRef;
	}

	public bool IsUserDbAdmin(string userID, string dataDictionaryName)
	{
		SqlCommand sqlCommand = currentContext.DDServerManager.NewSqlCommand(null, null, dataDictionaryName, "Select * From DDUsers Where duUserID = @UserID");
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userID;
		SqlDataAdapter adapter;
		DataTable dataTable = currentContext.DDServerManager.GetDataTable(null, null, dataDictionaryName, 0, sqlCommand, fillSchema: false, out adapter);
		if (dataTable.Rows.Count != 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			foreach (DataColumn column in dataTable.Columns)
			{
				string text = column.ColumnName.ToUpper();
				if ((text == "DUADMINISTRATOR" || text == "DUDBADMINISTRATOR") && Convert.ToBoolean(dataRow[column]))
				{
					return true;
				}
			}
		}
		else if (userID.Equals("admin", StringComparison.CurrentCultureIgnoreCase))
		{
			return true;
		}
		return false;
	}

	public string GetDDProductCode(string dataDictionaryName)
	{
		object obj = currentContext.DDServerManager.ExecuteScalar(null, null, dataDictionaryName, "Select ddProductCode From DDInfo");
		if (obj != null)
		{
			return obj.ToString();
		}
		return string.Empty;
	}

	public string GetDDCustomProductCodes(string dataDictionaryName)
	{
		object obj = currentContext.DDServerManager.ExecuteScalar(null, null, dataDictionaryName, "Select ddCustomProductCodes From DDInfo");
		if (obj != null)
		{
			return obj.ToString();
		}
		return string.Empty;
	}

	public string GetDataDictionaryVersion(string databaseName)
	{
		string result = string.Empty;
		DataTable dataTable = new DataTable();
		try
		{
			dataTable = currentContext.DDServerManager.GetDataTable(null, null, databaseName, 0, "select ddVersion from ddinfo");
			if (dataTable.Rows.Count > 0)
			{
				result = dataTable.Rows[0].Field<string>("ddVersion").Trim();
			}
		}
		catch
		{
			dataTable = currentContext.DDServerManager.GetDataTable(null, null, databaseName, 0, "select ddvers from ddinfo");
			if (dataTable.Rows.Count > 0)
			{
				result = dataTable.Rows[0].Field<string>("ddvers").Trim();
			}
		}
		return result;
	}

	public int ExecuteCommand(string databaseName, string queryString)
	{
		return currentContext.DDServerManager.ExecuteCommand(null, null, databaseName, queryString);
	}

	public int ExecuteCommand(string databaseName, string queryString, SqlTransaction transaction)
	{
		return currentContext.DDServerManager.ExecuteCommand(null, null, databaseName, queryString, transaction);
	}

	public int ExecuteCommand(SqlConnection sqlConnection, string databaseName, string queryString)
	{
		return currentContext.DDServerManager.ExecuteCommand(sqlConnection, null, databaseName, queryString);
	}

	public int ExecuteCommand(SqlConnection sqlConnection, string databaseName, SqlCommand command)
	{
		return currentContext.DDServerManager.ExecuteCommand(sqlConnection, null, databaseName, command, null);
	}

	public DataTable GetDataTable(string databaseName, string queryString)
	{
		return currentContext.DDServerManager.GetDataTable(null, null, databaseName, 0, queryString);
	}

	public DataTable GetDataTable(SqlConnection sqlConnection, string databaseName, string queryString)
	{
		return currentContext.DDServerManager.GetDataTable(sqlConnection, null, databaseName, 0, queryString);
	}

	public DataTable GetDataTable(string databaseName, string queryString, bool fillSchema)
	{
		return currentContext.DDServerManager.GetDataTable(null, null, databaseName, 0, queryString, fillSchema);
	}

	public DataTable GetDataTable(SqlConnection sqlConnection, string databaseName, string queryString, bool fillSchema, out SqlDataAdapter adapter)
	{
		return currentContext.DDServerManager.GetDataTable(sqlConnection, null, databaseName, 0, queryString, fillSchema, out adapter);
	}

	public DataTable GetDataTable(SqlConnection sqlConnection, string databaseName, string queryString, bool fillSchema, out SqlDataAdapter adapter, SqlTransaction transaction)
	{
		return currentContext.DDServerManager.GetDataTable(sqlConnection, null, databaseName, 0, queryString, fillSchema, out adapter, transaction);
	}

	public bool UpdateData(SqlConnection sqlConnection, M1User m1User, string databaseName, DataTable dataToUpdate, SqlDataAdapter adapter, SqlTransaction sqlTransaction)
	{
		return currentContext.DDServerManager.UpdateData(sqlConnection, null, databaseName, dataToUpdate, adapter, sqlTransaction);
	}

	public void LoadDDExplorerDefault(string databaseName, string userID, string mode, SqlTransaction transaction)
	{
		if (userID.Length == 0)
		{
			return;
		}
		ExecuteCommand(databaseName, $"Delete From DDExplorer Where dxUser = {userID.ToSql()} And dxMode = {mode.ToSql()}", transaction);
		if (!mode.Equals("SBAR", StringComparison.CurrentCultureIgnoreCase))
		{
			return;
		}
		bool needToClose = false;
		SqlConnection connection = currentContext.DDServerManager.GetConnection(null, databaseName, openImmediately: true, null, transaction, ref needToClose);
		SqlDataAdapter adapter = new SqlDataAdapter();
		GetDataTable(connection, databaseName, "Select * From DDExplorer Where 0=1", fillSchema: false, out adapter, transaction);
		SqlDataAdapter adapter2;
		DataTable dataTable = GetDataTable(connection, databaseName, "Select * From DDExplorer Where dxUser = '' And dxMode = 'SBAR'", fillSchema: false, out adapter2, transaction);
		SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(adapter);
		adapter.SelectCommand.Connection = connection;
		adapter.InsertCommand = sqlCommandBuilder.GetInsertCommand();
		Dictionary<Guid, DataRow> dictionary = new Dictionary<Guid, DataRow>();
		foreach (DataRow row in dataTable.Rows)
		{
			if (row["dxParentUniqueID"] == DBNull.Value)
			{
				dictionary.Add(row.Field<Guid>("dxUniqueID"), row);
			}
		}
		foreach (DataRow row2 in dataTable.Rows)
		{
			row2.SetAdded();
			row2.SetField("dxCustom", value: true);
			row2.SetField("dxUser", userID);
			row2.SetField("dxUniqueID", Guid.NewGuid());
		}
		foreach (DataRow row3 in dataTable.Rows)
		{
			if (row3["dxParentUniqueID"] != DBNull.Value && dictionary.ContainsKey(row3.Field<Guid>("dxParentUniqueID")))
			{
				row3.SetField("dxParentUniqueID", dictionary[row3.Field<Guid>("dxParentUniqueID")].Field<Guid>("dxUniqueID"));
			}
		}
		adapter.Update(dataTable);
		if (needToClose)
		{
			connection.Close();
		}
	}

	public void LoadDDUsersDefault(string databaseName)
	{
		ExecuteCommand(databaseName, "Delete From DDUsers Where duCustom = 0");
		loadTable(null, databaseName, "DDUsers");
	}

	public void CreateDataDictionaryDB(DDStartCreate ddCreate, string location, string dbName, int defaultDbSizeInMB, string productCode)
	{
		dbName = dbName.Trim();
		if (dbName.Length == 0)
		{
			dbName = "M1DD";
		}
		location = location.AddBackslash();
		ServerFileSystem serverFileSystem = new ServerFileSystem(currentContext.DDServerManager);
		if (!serverFileSystem.FolderExists(location))
		{
			new Dmo(currentContext, currentContext.DDServerManager).SetConfigure();
			serverFileSystem.CreateFolder(location);
			if (!serverFileSystem.FolderExists(location))
			{
				throw new M1Exception($"M1 was unable to create folder {location}.");
			}
		}
		try
		{
			string arg = $"{location}{dbName}";
			if (serverFileSystem.FileExists($"{arg}.mdf"))
			{
				if (currentContext.DDServerManager.DoesDatabaseExist(null, null, dbName))
				{
					currentContext.DDServerManager.ClearAllPools();
					ExecuteCommand("master", $"EXEC sp_detach_db @dbname = '{dbName}'");
				}
				serverFileSystem.DeleteFile($"{arg}.mdf");
			}
			if (serverFileSystem.FileExists($"{arg}_log.ldf"))
			{
				serverFileSystem.DeleteFile($"{arg}_log.ldf");
			}
		}
		catch
		{
		}
		if (defaultDbSizeInMB <= 0)
		{
			defaultDbSizeInMB = 6;
		}
		string queryString = "CREATE DATABASE " + dbName + " ON (NAME = " + dbName + ", FILENAME = '" + location + dbName + ".mdf', SIZE = " + defaultDbSizeInMB + "MB)\r";
		ExecuteCommand("master", queryString);
		CreateDataDictionaryTables(ddCreate, dbName, string.Empty, productCode, null);
		refreshHasChangeCode(dbName);
		refreshHasDeleteCode(dbName);
	}

	public void CreateDataDictionaryTables(DDStartCreate ddCreate, string databaseName, string tableToCreate, string productCode, DDDatabaseDefinition ddDef)
	{
		CreateDataDictionaryTables(ddCreate, databaseName, tableToCreate, productCode, ddDef, null, null);
	}

	public void CreateDataDictionaryTables(DDStartCreate ddCreate, string databaseName, string tableToCreate, string productCode, DDDatabaseDefinition ddDef, SqlConnection connection, M1User user)
	{
		if (ddDef == null)
		{
			ddDef = new DDDatabaseDefinition();
		}
		bool needToClose = false;
		SqlConnection sqlConnection = ((connection == null) ? currentContext.DDServerManager.GetConnection(null, databaseName, openImmediately: true, null, null, ref needToClose) : connection);
		try
		{
			tableToCreate = tableToCreate.Trim().ToUpper();
			if (ddCreate != null)
			{
				foreach (DDTableDefinition table3 in ddDef.Tables)
				{
					ddCreate.FormRef.Invoke(new DDStartCreate.AddItemDelegate(ddCreate.AddItemFunc.Invoke), table3.TableName);
				}
				ddCreate.FormRef.Invoke(new DDStartCreate.RedrawListDelegate(ddCreate.RedrawListFunc.Invoke));
			}
			if ((tableToCreate.Length == 0 || tableToCreate.Equals("DDInfo", StringComparison.CurrentCultureIgnoreCase)) && !DoesTableExist(sqlConnection, databaseName, "DDInfo"))
			{
				DDTableDefinition table = ddDef.GetTable("DDInfo");
				ddCreate?.FormRef.Invoke(new DDStartCreate.SelectItemDelegate(ddCreate.SelectItemFunc.Invoke), table.TableName);
				ExecuteCommand(sqlConnection, databaseName, table.GetCreateTableCommand() + "\r" + table.GetCreateIndexesCommand());
				addDDInfoRecord(sqlConnection, databaseName, productCode);
			}
			if ((tableToCreate.Length == 0 || tableToCreate.Equals("DDAppExtensions", StringComparison.CurrentCultureIgnoreCase)) && !DoesTableExist(sqlConnection, databaseName, "DDAppExtensions"))
			{
				DDTableDefinition table2 = ddDef.GetTable("DDAppExtensions");
				ddCreate?.FormRef.Invoke(new DDStartCreate.SelectItemDelegate(ddCreate.SelectItemFunc.Invoke), table2.TableName);
				ExecuteCommand(sqlConnection, databaseName, table2.GetCreateTableCommand() + "\r" + table2.GetCreateIndexesCommand());
				loadTable(sqlConnection, databaseName, table2.TableName);
			}
			foreach (DDTableDefinition table4 in ddDef.Tables)
			{
				if ((tableToCreate.Length == 0 || table4.TableName.Equals(tableToCreate, StringComparison.CurrentCultureIgnoreCase)) && !table4.TableName.Equals("DDInfo", StringComparison.CurrentCultureIgnoreCase) && !table4.TableName.Equals("DDAppExtensions", StringComparison.CurrentCultureIgnoreCase))
				{
					ddCreate?.FormRef.Invoke(new DDStartCreate.SelectItemDelegate(ddCreate.SelectItemFunc.Invoke), table4.TableName);
					if (!DoesTableExist(sqlConnection, databaseName, table4.TableName))
					{
						ExecuteCommand(sqlConnection, databaseName, table4.GetCreateTableCommand() + "\r" + table4.GetCreateIndexesCommand());
						loadTable(sqlConnection, databaseName, table4.TableName);
					}
				}
			}
			if (tableToCreate.Length == 0)
			{
				UpdateAppExtensionVersions(databaseName, null, sqlConnection, user);
			}
		}
		finally
		{
			if (needToClose)
			{
				sqlConnection.Close();
			}
		}
	}

	private void addDDInfoRecord(SqlConnection sqlConnection, string databaseName, string productCode)
	{
		SqlDataAdapter adapter = new SqlDataAdapter();
		DataTable dataTable = GetDataTable(sqlConnection, databaseName, "Select ddVersion,ddRegion,ddProductCode From DDInfo", fillSchema: true, out adapter);
		SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(adapter);
		adapter.SelectCommand.Connection = sqlConnection;
		adapter.InsertCommand = sqlCommandBuilder.GetInsertCommand();
		if (dataTable.Rows.Count == 0)
		{
			dataTable.AddBlankRow();
		}
		dataTable.Rows[0].SetField("ddVersion", currentContext.Version);
		dataTable.Rows[0].SetField("ddRegion", currentContext.GetWindowsRegion());
		dataTable.Rows[0].SetField("ddProductCode", productCode);
		adapter.Update(dataTable);
	}

	private void addDDInfoRecord(SqlConnection sqlConnection, string databaseName, string productCode, string customProductCode)
	{
		SqlDataAdapter adapter = new SqlDataAdapter();
		DataTable dataTable = GetDataTable(sqlConnection, databaseName, "Select ddVersion,ddRegion,ddProductCode, ddCustomProductCodes From DDInfo", fillSchema: true, out adapter);
		SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(adapter);
		adapter.SelectCommand.Connection = sqlConnection;
		adapter.InsertCommand = sqlCommandBuilder.GetInsertCommand();
		if (dataTable.Rows.Count == 0)
		{
			dataTable.AddBlankRow();
		}
		dataTable.Rows[0].SetField("ddVersion", currentContext.Version);
		dataTable.Rows[0].SetField("ddRegion", currentContext.GetWindowsRegion());
		dataTable.Rows[0].SetField("ddProductCode", productCode);
		dataTable.Rows[0].SetField("ddCustomProductCodes", customProductCode);
		adapter.Update(dataTable);
	}

	private void reloadCustomizedTable(string databaseName, string table, string filter, bool recreateTable, DDDatabaseDefinition ddDef)
	{
		reloadCustomizedTable(databaseName, table, filter, recreateTable, ddDef, null, null);
	}

	private void reloadCustomizedTable(string databaseName, string table, string filter, bool recreateTable, DDDatabaseDefinition ddDef, SqlConnection connection, M1User user)
	{
		bool needToClose = false;
		SqlConnection sqlConnection = ((connection == null) ? currentContext.DDServerManager.GetConnection(null, databaseName, openImmediately: true, currentConnection, null, ref needToClose) : connection);
		try
		{
			ExecuteCommand(sqlConnection, databaseName, $"select * Into ReloadTemp from {table} Where {filter}");
			DataTable dataTable = GetDataTable(sqlConnection, databaseName, "select * from ReloadTemp Where 0=1");
			if (recreateTable)
			{
				ExecuteCommand(sqlConnection, databaseName, $"DROP TABLE {table}");
				CreateDataDictionaryTables(null, databaseName, table, string.Empty, ddDef, sqlConnection, user);
			}
			else
			{
				ExecuteCommand(sqlConnection, databaseName, $"Truncate Table {table}");
				loadTable(sqlConnection, databaseName, table);
			}
			DataTable dataTable2 = GetDataTable(sqlConnection, databaseName, $"Select * From {table} Where 0=1");
			string text = string.Empty;
			foreach (DataColumn column in dataTable.Columns)
			{
				foreach (DataColumn column2 in dataTable2.Columns)
				{
					if (column.ColumnName.Equals(column2.ColumnName, StringComparison.CurrentCultureIgnoreCase))
					{
						text = text + ((text.Length == 0) ? string.Empty : ",") + column.ColumnName;
						break;
					}
				}
			}
			string arg = (string)currentContext.DDServerManager.ExecuteScalar(sqlConnection, user, databaseName, "Select SERVERPROPERTY('collation')");
			switch (table)
			{
			case "DDINFO":
				ExecuteCommand(sqlConnection, databaseName, $"Truncate Table {table}");
				ExecuteCommand(sqlConnection, databaseName, string.Format("Insert Into {0} ( {1} ) Select {1} From ReloadTemp ", table, text));
				break;
			case "DDFORMDETAILS":
				ExecuteCommand(sqlConnection, databaseName, string.Format("Insert Into {0} ( {1} ) Select {1} From ReloadTemp ", table, text));
				ExecuteCommand(sqlConnection, databaseName, string.Format("Insert Into DDFormDetails (deFormID,deControlName,deParentID,deNestedName,deParentIDUser,deNestedNameUser,deClassID,deSequence,deSequenceUser,deProperties,dePropertiesUser,deCustom,deAppExtensionID) Select deFormID,deControlName,deParentID,deNestedName,deParentIDUser,deNestedNameUser,deClassID,deSequence,deSequenceUser,deProperties,dePropertiesUser,deCustom,deAppExtensionID From DDFormDetailsEx a Where a.deFormID COLLATE {0} + a.deControlName COLLATE {0} Not In (Select b.deFormID+b.deControlName From DDFormDetails b)", arg));
				break;
			case "DDEXPLORER":
				VerifyUniqueIdsInReloadTempTable(sqlConnection, databaseName);
				ExecuteCommand(sqlConnection, databaseName, string.Format("Insert Into {0} ( {1} ) Select {1} From ReloadTemp ", table, text));
				break;
			default:
				ExecuteCommand(sqlConnection, databaseName, string.Format("Insert Into {0} ( {1} ) Select {1} From ReloadTemp ", table, text));
				break;
			case "DDCUSTOMMODULES":
				break;
			}
		}
		finally
		{
			ExecuteCommand(sqlConnection, databaseName, "Drop Table ReloadTemp");
			if (needToClose)
			{
				sqlConnection.Close();
			}
			sqlConnection = null;
		}
	}

	private void VerifyUniqueIdsInReloadTempTable(SqlConnection connection, string databaseName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("SELECT  ReloadTemp.dxUniqueId FROM (SELECT dxUniqueId, dxUser, dxMode FROM DDExplorer WHERE(dxUser = '')) AS Explorer INNER JOIN ReloadTemp ON Explorer.dxUniqueId = ReloadTemp.dxUniqueId");
		DataTable dataTable = GetDataTable(connection, databaseName, stringBuilder.ToString());
		if (dataTable != null && dataTable.Rows.Count > 0)
		{
			SqlCommand sqlCommand = null;
			foreach (DataRow row in dataTable.Rows)
			{
				Guid guid = row.Field<Guid>("dxUniqueId");
				Guid guid2 = Guid.NewGuid();
				stringBuilder.Length = 0;
				stringBuilder.Append("Update ReloadTemp SET dxUniqueId=@NewUid WHERE dxUniqueId=@oldUid");
				sqlCommand = new SqlCommand(stringBuilder.ToString());
				sqlCommand.Parameters.AddWithValue("@oldUid", guid);
				sqlCommand.Parameters.AddWithValue("@NewUid", guid2);
				ExecuteCommand(connection, databaseName, sqlCommand);
			}
			sqlCommand?.Dispose();
		}
		dataTable?.Dispose();
	}

	private bool DoesTableExist(SqlConnection sqlConnection, string databaseName, string tableName)
	{
		return DoesTableExist(sqlConnection, databaseName, tableName, null);
	}

	private bool DoesTableExist(SqlConnection sqlConnection, string databaseName, string tableName, SqlTransaction transaction)
	{
		return currentContext.DDServerManager.DoesTableExist(sqlConnection, null, databaseName, tableName, transaction);
	}

	private void doSecondCheckReloadTable(SqlConnection sqlConnection, string databaseName, DDDatabaseDefinition ddDef)
	{
		foreach (DDCustomTableInfo loadedTableInfo in ddDef.LoadedTableInfos)
		{
			if (!loadedTableInfo.QueryHasRun || loadedTableInfo.ReloadStatements == null || loadedTableInfo.ReloadStatements.Count == 0)
			{
				continue;
			}
			foreach (string reloadStatement in loadedTableInfo.ReloadStatements)
			{
				ExecuteCommand(databaseName, reloadStatement);
			}
		}
	}

	public void ReloadTable(string databaseName, string table, bool recreateTable, Action<string> msgDelegate, DDDatabaseDefinition ddDef)
	{
		ReloadTable(databaseName, table, recreateTable, msgDelegate, ddDef, null, null);
	}

	public void ReloadTable(string databaseName, string table, bool recreateTable, Action<string> msgDelegate, DDDatabaseDefinition ddDef, SqlConnection connection, M1User user)
	{
		msgDelegate?.Invoke("Updating table " + table);
		table = table.Trim().ToUpper();
		DDCustomTableInfo dDCustomTableInfo = null;
		if (ddDef != null)
		{
			string serverCollation = (string)currentContext.DDServerManager.ExecuteScalar(connection, user, databaseName, "Select SERVERPROPERTY('collation')");
			dDCustomTableInfo = ddDef.GetUpdateInfoForTable(table, serverCollation);
		}
		if (dDCustomTableInfo != null)
		{
			if (dDCustomTableInfo.CustomFieldsSelectStatement.Length != 0)
			{
				if (dDCustomTableInfo.QueryHasRun || DoesTableExist(connection, databaseName, dDCustomTableInfo.TempTable))
				{
					ExecuteCommand(connection, databaseName, "DROP TABLE " + dDCustomTableInfo.TempTable);
				}
				ExecuteCommand(connection, databaseName, dDCustomTableInfo.CustomFieldsSelectStatement);
				dDCustomTableInfo.QueryHasRun = true;
			}
			reloadCustomizedTable(databaseName, table, dDCustomTableInfo.LoadTableExpression, recreateTable, ddDef, connection, user);
			if (dDCustomTableInfo.ReloadStatements == null || dDCustomTableInfo.ReloadStatements.Count == 0)
			{
				return;
			}
			{
				foreach (string reloadStatement in dDCustomTableInfo.ReloadStatements)
				{
					ExecuteCommand(connection, databaseName, reloadStatement);
				}
				return;
			}
		}
		if (recreateTable)
		{
			reloadCustomizedTable(databaseName, table, "0=1", recreateTable, ddDef, connection, user);
			return;
		}
		ExecuteCommand(connection, databaseName, $"TRUNCATE TABLE {table}");
		loadTable(connection, databaseName, table);
	}

	private void loadTable(SqlConnection sqlConnection, string databaseName, string table)
	{
		loadTable(sqlConnection, databaseName, table, table, string.Empty, string.Empty, string.Empty);
	}

	private void loadTable(SqlConnection sqlConnection, string databaseName, string tableSource, string tableDest, string field, string value, string filter)
	{
		loadTable(sqlConnection, databaseName, tableSource, tableDest, field, value, filter, null);
	}

	private void loadTable(SqlConnection sqlConnection, string databaseName, string tableSource, string tableDest, string field, string value, string filter, SqlTransaction transaction)
	{
		tableSource = tableSource.Trim().ToUpper();
		field = field.ToUpper();
		DataSet dataSet = null;
		string text = currentContext.Server.Location + "DataDict\\" + tableSource + ".xml";
		if (File.Exists(text))
		{
			dataSet = new DataSet();
			dataSet.ReadXml(text, XmlReadMode.Auto);
			loadTable(sqlConnection, databaseName, tableSource, tableDest, field, value, filter, transaction, dataSet);
		}
		foreach (DataRow row in GetDataTable(sqlConnection, databaseName, "Select dpDDAssembly From DDAppExtensions Where dpDDAssembly <> ''").Rows)
		{
			dataSet = loadXmlFromAssembly(tableSource, row.Field<string>("dpDDAssembly"));
			loadTable(sqlConnection, databaseName, tableSource, tableDest, field, value, filter, transaction, dataSet);
		}
	}

	private void loadTable(SqlConnection sqlConnection, string databaseName, string tableSource, string tableDest, string field, string value, string filter, SqlTransaction transaction, DataSet xmlDataset)
	{
		if (xmlDataset == null)
		{
			return;
		}
		DataRow[] array = null;
		if (xmlDataset.Tables.Contains("row"))
		{
			loadTableAlt(sqlConnection, databaseName, tableSource, tableDest, field, value, filter, transaction);
			return;
		}
		if (xmlDataset.Tables.Contains(tableSource))
		{
			DataTable dataTable = xmlDataset.Tables[tableSource];
			array = ((filter.Length == 0) ? dataTable.Select() : dataTable.Select(filter));
			if (array.Length == 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (!column.ColumnName.Equals("data_id", StringComparison.CurrentCultureIgnoreCase))
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(',');
					}
					stringBuilder.Append(column.ColumnName);
				}
			}
			bool needToClose = false;
			SqlConnection connection = currentContext.DDServerManager.GetConnection(null, databaseName, openImmediately: true, sqlConnection, transaction, ref needToClose);
			SqlDataAdapter adapter = new SqlDataAdapter();
			GetDataTable(connection, databaseName, "Select " + stringBuilder.ToString() + " From " + tableDest + " Where 0=1", fillSchema: false, out adapter, transaction);
			SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(adapter);
			adapter.SelectCommand.Connection = connection;
			adapter.InsertCommand = sqlCommandBuilder.GetInsertCommand();
			adapter.Update(array);
			translateTable(connection, databaseName, tableDest, transaction);
			if (needToClose)
			{
				connection.Close();
			}
			return;
		}
		throw new M1Exception($"Unable to find table {tableSource} in xml file.");
	}

	private DataSet loadXmlFromAssembly(string table, string file)
	{
		Assembly assembly = Assembly.LoadFile(Path.Combine(currentContext.Server.Location + "Tools\\Assemblies\\", file));
		string value = Path.GetFileNameWithoutExtension(file) + "." + table + ".xml";
		string[] manifestResourceNames = assembly.GetManifestResourceNames();
		foreach (string text in manifestResourceNames)
		{
			if (text.Equals(value, StringComparison.CurrentCultureIgnoreCase))
			{
				DataSet dataSet = new DataSet();
				dataSet.ReadXml(assembly.GetManifestResourceStream(text), XmlReadMode.Auto);
				return dataSet;
			}
		}
		return null;
	}

	private void loadTableAlt(SqlConnection sqlConnection, string databaseName, string tableSource, string tableDest, string field, string value, string filter, SqlTransaction transaction)
	{
		tableSource = tableSource.Trim().ToUpper();
		string text = currentContext.Server.Location + "DataDict\\" + tableSource + ".xml";
		if (!File.Exists(text))
		{
			return;
		}
		DataRow[] array = null;
		field = field.ToUpper();
		DataSet dataSet = new DataSet();
		dataSet.ReadXml(text, XmlReadMode.Auto);
		DataTable dataTable;
		if (dataSet.Tables.Contains("row"))
		{
			dataTable = dataSet.Tables["row"];
		}
		else
		{
			if (!dataSet.Tables.Contains(tableSource))
			{
				throw new M1Exception($"Unable to find table {tableSource} in xml file.");
			}
			dataTable = dataSet.Tables[tableSource];
		}
		array = ((filter.Length == 0) ? dataTable.Select() : dataTable.Select(filter));
		bool needToClose = false;
		SqlConnection connection = currentContext.DDServerManager.GetConnection(null, databaseName, openImmediately: true, sqlConnection, transaction, ref needToClose);
		SqlDataAdapter adapter = new SqlDataAdapter();
		DataTable dataTable2 = GetDataTable(connection, databaseName, $"Select * From {tableDest} Where 0=1", fillSchema: false, out adapter, transaction);
		dataTable2.Constraints.Clear();
		adapter.SelectCommand.Connection = connection;
		SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(adapter);
		adapter.SelectCommand.Connection = connection;
		adapter.InsertCommand = sqlCommandBuilder.GetInsertCommand();
		DataRow[] array2 = array;
		foreach (DataRow dataRow in array2)
		{
			DataRow dataRow2 = dataTable2.NewRow();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (!column.ColumnName.Equals("DATA_ID", StringComparison.CurrentCultureIgnoreCase) && column.ColumnMapping != MappingType.Hidden)
				{
					if (field.Length != 0 && column.ColumnName.Equals(field, StringComparison.CurrentCultureIgnoreCase))
					{
						dataRow2[column.ColumnName] = value;
					}
					else if (dataRow2.Table.Columns[column.ColumnName].DataType == typeof(decimal) && column.DataType == typeof(string))
					{
						string text2 = dataRow.Field<string>(column).ToUpper();
						if (!(text2 == "TRUE"))
						{
							if (text2 == "FALSE")
							{
								dataRow2.SetField(column.ColumnName, 0m);
							}
							else
							{
								dataRow2[column.ColumnName] = decimal.Parse(dataRow.Field<string>(column.ColumnName));
							}
						}
						else
						{
							dataRow2.SetField(column.ColumnName, 1m);
						}
					}
					else if (dataRow2.Table.Columns[column.ColumnName].DataType == typeof(string) && column.DataType == typeof(string) && dataRow[column.ColumnName] != DBNull.Value)
					{
						dataRow2.SetField(column.ColumnName, dataRow.Field<string>(column.ColumnName).TrimEnd());
					}
					else
					{
						dataRow2[column.ColumnName] = dataRow[column.ColumnName];
					}
				}
				else if (column.ColumnMapping == MappingType.Hidden && dataTable2.TableName.StartsWith("DDLANG", StringComparison.CurrentCultureIgnoreCase))
				{
					dataRow2.SetField("dnCustText", "");
				}
			}
			dataTable2.Rows.Add(dataRow2);
		}
		adapter.Update(dataTable2);
		translateTable(connection, databaseName, tableDest, transaction);
		if (needToClose)
		{
			connection.Close();
		}
	}

	public void SetRegionOnDD(string databaseName, string newRegion)
	{
		newRegion = newRegion.Trim().ToUpper();
		ExecuteCommand(databaseName, $"Update DDInfo Set ddRegion = {newRegion.ToSql()}");
		translateTable(null, databaseName, string.Empty, null);
	}

	private string getRegionForDD(SqlConnection sqlConnection, string databaseName, SqlTransaction transaction)
	{
		string text = string.Empty;
		SqlDataAdapter adapter;
		using (DataTable dataTable = GetDataTable(sqlConnection, databaseName, "select ddRegion from DDInfo", fillSchema: false, out adapter, transaction))
		{
			if (dataTable.Rows.Count > 0)
			{
				text = dataTable.Rows[0].Field<string>("ddRegion").Trim().ToUpper();
				if (text.Length == 0)
				{
					text = currentContext.GetWindowsRegion();
				}
			}
		}
		return text;
	}

	public void ReplaceInDDCode(SqlConnection sqlConnection, string databaseName, SqlTransaction transaction, IEnumerable<TranslateInfo> wordList)
	{
		DDFind dDFind = new DDFind();
		foreach (TranslateInfo word in wordList)
		{
			dDFind.FindTextInDD(currentContext, sqlConnection, null, databaseName, word.SourceText, (DDFieldContentType)96, customOnly: false, word.DestinationText);
		}
	}

	private void refreshHasChangeCode(string databaseName)
	{
		string methodType = string.Empty;
		List<string> list = new List<string>();
		ExecuteCommand(databaseName, "Update DDFields Set dfHasChangeCode = 0 Where dfHasChangeCode <> 0");
		foreach (DataRow row in GetDataTable(databaseName, "Select dkCode,dtTable From DDTables Inner Join DDCode On dkSourceUniqueID = dtUniqueID And dkSourceTable = 'DDTables' And Convert(nvarchar(max),dkCode) Like '%_ValueChanged%'").Rows)
		{
			string text = row.Field<string>("dkCode");
			if (text == null)
			{
				continue;
			}
			list.Clear();
			string[] array = text.Split('\r');
			for (int i = 0; i < array.Length; i++)
			{
				string methodNameInText = M1Util.GetMethodNameInText(array[i], ref methodType);
				if (methodNameInText.Length == 0 || methodNameInText.IndexOf("_ValueChanged", StringComparison.CurrentCultureIgnoreCase) == -1)
				{
					continue;
				}
				int num = methodNameInText.IndexOf('_');
				if (num != -1)
				{
					methodNameInText = methodNameInText.Substring(0, num);
					if (methodNameInText.Length != 0 && !list.Contains(methodNameInText, StringComparer.CurrentCultureIgnoreCase))
					{
						list.Add(methodNameInText);
					}
				}
			}
			string o = row.Field<string>("dtTable");
			foreach (string item in list)
			{
				ExecuteCommand(databaseName, "Update DDFields Set dfHasChangeCode = 1 Where dfTable = " + M1Util.ConvertToSql(o) + " And dfField = " + M1Util.ConvertToSql(item));
			}
		}
		ExecuteCommand(databaseName, "Update a Set dfHasChangeCode = 1 from DDFields a Inner Join DDFields b on a.dfTable = b.dfTable And b.dfCalculationExpression Like '%' + a.dfField + '%' And a.dfHasChangeCode = 0");
		ExecuteCommand(databaseName, "Update DDFields Set dfHasChangeCode = 1 From DDFields Where dfBoundParentField <> '' And dfBoundParentFieldType = 2 and dfHasChangeCode = 0");
	}

	public void refreshHasDeleteCode(string databaseName)
	{
		ExecuteCommand(databaseName, "Update DDTables Set dtHasDeleteCode = 0 Where dtHasDeleteCode <> 0");
		ExecuteCommand(databaseName, "Update DDTables Set dtHasDeleteCode = 1 From DDTables Inner Join DDCode On dkSourceUniqueID = dtUniqueID And dkSourceTable = 'DDTables' And (Convert(nvarchar(max),dkCode) Like '%_DeleteStarted%' Or Convert(nvarchar(max),dkCode) Like '%_DeleteCompleted%')");
	}

	private void translateTable(SqlConnection sqlConnection, string databaseName, string table, SqlTransaction transaction)
	{
		string regionForDD = getRegionForDD(sqlConnection, databaseName, transaction);
		List<TranslateInfo> list = new List<TranslateInfo>();
		List<TranslateInfo> list2 = new List<TranslateInfo>();
		switch (regionForDD.Trim().ToUpper())
		{
		case "US":
			list.AddRange(new TranslateInfo[13]
			{
				new TranslateInfo("labour", "labor", ignoreCase: false),
				new TranslateInfo("centre", "center", ignoreCase: false),
				new TranslateInfo("colour", "color", ignoreCase: false),
				new TranslateInfo("ABN", "FedID", ignoreCase: true),
				new TranslateInfo("FBN", "FedID", ignoreCase: true),
				new TranslateInfo("VATRegID", "FedID", ignoreCase: true),
				new TranslateInfo("Tax File Number", "Social Security No.", ignoreCase: true),
				new TranslateInfo("Post Code", "Zip Code", ignoreCase: true),
				new TranslateInfo("Organisation", "Organization", ignoreCase: true),
				new TranslateInfo("BSB Number", "Routing Number", ignoreCase: true),
				new TranslateInfo("Employment Declaration", "I-9", ignoreCase: true),
				new TranslateInfo("GST ", "VAT ", ignoreCase: true),
				new TranslateInfo("Prov", "State", ignoreCase: true, matchWholeWord: true)
			});
			list2.Add(new TranslateInfo("cheque", "check", ignoreCase: false));
			break;
		case "CAN":
			list.AddRange(new TranslateInfo[13]
			{
				new TranslateInfo("labor", "labour", ignoreCase: false),
				new TranslateInfo("center", "centre", ignoreCase: false),
				new TranslateInfo("color", "colour", ignoreCase: false),
				new TranslateInfo("ABN", "FBN", ignoreCase: true),
				new TranslateInfo("FedID", "FBN", ignoreCase: true),
				new TranslateInfo("VATRegID", "FBN", ignoreCase: true),
				new TranslateInfo("Social Security No.", "Social Insurance No.", ignoreCase: true),
				new TranslateInfo("Zip Code", "Post Code", ignoreCase: true),
				new TranslateInfo("Organisation", "Organization", ignoreCase: true),
				new TranslateInfo("BSB Number", "Routing Number", ignoreCase: true),
				new TranslateInfo("I-9", "Employment Declaration", ignoreCase: true),
				new TranslateInfo("VAT ", "GST ", ignoreCase: true),
				new TranslateInfo("State", "Prov", ignoreCase: true, matchWholeWord: true)
			});
			list2.Add(new TranslateInfo("check", "cheque", ignoreCase: false, matchWholeWord: true));
			list2.Add(new TranslateInfo("checking", "chequeing", ignoreCase: false, matchWholeWord: true));
			break;
		case "UK":
			list.AddRange(new TranslateInfo[13]
			{
				new TranslateInfo("labor", "labour", ignoreCase: false),
				new TranslateInfo("center", "centre", ignoreCase: false),
				new TranslateInfo("color", "colour", ignoreCase: false),
				new TranslateInfo("ABN", "VATRegID", ignoreCase: true),
				new TranslateInfo("FedID", "VATRegID", ignoreCase: true),
				new TranslateInfo("FBN", "VATRegID", ignoreCase: true),
				new TranslateInfo("Social Security No.", "Tax File Number", ignoreCase: true),
				new TranslateInfo("Zip Code", "Post Code", ignoreCase: true),
				new TranslateInfo("Organization", "Organisation", ignoreCase: true),
				new TranslateInfo("Routing Number", "BSB Number", ignoreCase: true),
				new TranslateInfo("I-9", "Employment Declaration", ignoreCase: true),
				new TranslateInfo("VAT ", "GST ", ignoreCase: true),
				new TranslateInfo("Prov", "State", ignoreCase: true, matchWholeWord: true)
			});
			list2.Add(new TranslateInfo("check", "cheque", ignoreCase: false, matchWholeWord: true));
			list2.Add(new TranslateInfo("checking", "chequeing", ignoreCase: false, matchWholeWord: true));
			break;
		default:
			list.AddRange(new TranslateInfo[13]
			{
				new TranslateInfo("labor", "labour", ignoreCase: false),
				new TranslateInfo("center", "centre", ignoreCase: false),
				new TranslateInfo("color", "colour", ignoreCase: false),
				new TranslateInfo("VATRegID", "ABN", ignoreCase: true),
				new TranslateInfo("FedID", "ABN", ignoreCase: true),
				new TranslateInfo("FBN", "ABN", ignoreCase: true),
				new TranslateInfo("Social Security No.", "Tax File Number", ignoreCase: true),
				new TranslateInfo("Zip Code", "Post Code", ignoreCase: true),
				new TranslateInfo("Organization", "Organisation", ignoreCase: true),
				new TranslateInfo("Routing Number", "BSB Number", ignoreCase: true),
				new TranslateInfo("I-9", "Employment Declaration", ignoreCase: true),
				new TranslateInfo("VAT ", "GST ", ignoreCase: true),
				new TranslateInfo("Prov", "State", ignoreCase: true, matchWholeWord: true)
			});
			list2.Add(new TranslateInfo("check", "cheque", ignoreCase: false, matchWholeWord: true));
			list2.Add(new TranslateInfo("checking", "chequeing", ignoreCase: false, matchWholeWord: true));
			break;
		}
		table = table.Trim().ToUpper();
		if (table == "DDFIELDS" || table.Length == 0)
		{
			foreach (TranslateInfo item in list)
			{
				translateWordInTable(sqlConnection, databaseName, "DDFields", "dfCaption", item.SourceText, item.DestinationText, item.IgnoreCase, item.MatchWholeWord, transaction);
				translateWordInTable(sqlConnection, databaseName, "DDFields", "dfStatus", item.SourceText, item.DestinationText, item.IgnoreCase, item.MatchWholeWord, transaction);
				translateWordInTable(sqlConnection, databaseName, "DDFields", "dfValueList", item.SourceText, item.DestinationText, item.IgnoreCase, item.MatchWholeWord, transaction);
			}
			translateWordInTable(sqlConnection, databaseName, "DDFields", "dfCaption", list2[0].SourceText, list2[0].DestinationText, list2[0].IgnoreCase, list2[0].MatchWholeWord, transaction);
			translateWordInTable(sqlConnection, databaseName, "DDFields", "dfValueList", list2[0].SourceText, list2[0].DestinationText, list2[0].IgnoreCase, list2[0].MatchWholeWord, transaction);
		}
		if (table == "DDTABLES" || table.Length == 0)
		{
			foreach (TranslateInfo item2 in list)
			{
				translateWordInTable(sqlConnection, databaseName, "DDTables", "dtCaption", item2.SourceText, item2.DestinationText, item2.IgnoreCase, item2.MatchWholeWord, transaction);
			}
		}
		if (table == "DDEXPLORER" || table.Length == 0)
		{
			foreach (TranslateInfo item3 in list)
			{
				translateWordInTable(sqlConnection, databaseName, "DDExplorer", "dxText", item3.SourceText, item3.DestinationText, item3.IgnoreCase, item3.MatchWholeWord, transaction);
			}
		}
		if (table == "DDOPENWITHS" || table.Length == 0)
		{
			foreach (TranslateInfo item4 in list)
			{
				translateWordInTable(sqlConnection, databaseName, "DDOpenWiths", "dwDesc", item4.SourceText, item4.DestinationText, item4.IgnoreCase, item4.MatchWholeWord, transaction);
			}
		}
		if (table == "DDFORMS" || table.Length == 0)
		{
			foreach (TranslateInfo item5 in list)
			{
				translateWordInTable(sqlConnection, databaseName, "DDForms", "dmCaption", item5.SourceText, item5.DestinationText, item5.IgnoreCase, item5.MatchWholeWord, transaction);
			}
		}
		if (table == "DDOBJECTS" || table.Length == 0)
		{
			foreach (TranslateInfo item6 in list)
			{
				translateWordInTable(sqlConnection, databaseName, "DDObjects", "doTitle", item6.SourceText, item6.DestinationText, item6.IgnoreCase, item6.MatchWholeWord, transaction);
			}
		}
		if (!(table == "DDGRIDS") && table.Length != 0)
		{
			return;
		}
		foreach (TranslateInfo item7 in list)
		{
			translateWordInTable(sqlConnection, databaseName, "DDGrids", "djDesc", item7.SourceText, item7.DestinationText, item7.IgnoreCase, item7.MatchWholeWord, transaction);
		}
	}

	private void translateWordInTable(SqlConnection sqlConnection, string databaseName, string table, string field, string sourceText, string destText, bool ignoreCase, bool matchWholeWord, SqlTransaction transaction)
	{
		DataTable dataTable = null;
		SqlDataAdapter adapter = null;
		field = field.Trim().ToUpper();
		dataTable = ((!(field == "DFVALUELIST")) ? GetDataTable(sqlConnection, databaseName, $"select * from {table}  where {field} like '%{sourceText.Trim()}%'", fillSchema: true, out adapter, transaction) : GetDataTable(sqlConnection, databaseName, $"select * from {table} where Not dfValueList Is Null and dfValueList like '%{sourceText.Trim()}%'", fillSchema: true, out adapter, transaction));
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		int maxLength = dataTable.Columns[field].MaxLength;
		int length = sourceText.Length;
		string text = destText.Substring(0, 1);
		string text2 = destText.Substring(1);
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		string empty5 = string.Empty;
		string empty6 = string.Empty;
		int num = 0;
		foreach (DataRow row in dataTable.Rows)
		{
			if (field == "DFVALUELIST")
			{
				StringBuilder stringBuilder = new StringBuilder();
				string empty7 = string.Empty;
				string empty8 = string.Empty;
				empty = row.Field<string>(field);
				string[] array = empty.Split('\r');
				foreach (string text3 in array)
				{
					num = text3.ToUpper().IndexOf(',');
					if (num == -1)
					{
						continue;
					}
					empty7 = text3.Substring(0, num + 1);
					empty8 = text3.Substring(num + 1);
					num = empty8.ToUpper().IndexOf(sourceText.ToUpper());
					if (num != -1)
					{
						empty2 = empty8.Substring(0, num);
						empty8 = empty8.Substring(num);
						empty5 = empty8.Substring(0, length);
						empty3 = empty8.Substring(length);
						if ((matchWholeWord && (empty3.Length == 0 || empty3.StartsWith(" "))) || !matchWholeWord)
						{
							if (!ignoreCase)
							{
								empty4 = ((!(empty8.Substring(0, 1) == empty5.Substring(0, 1).ToUpper())) ? text.ToLower() : text.ToUpper());
								empty4 = ((!(empty5.Substring(1, 1) == empty5.Substring(1, 1).ToUpper())) ? (empty4 + text2.ToLower()) : (empty4 + text2.ToUpper()));
							}
							else
							{
								empty4 = destText;
							}
							empty6 = empty7 + empty2 + empty4 + empty3;
							stringBuilder.Append(empty6 + "\r");
						}
						else
						{
							stringBuilder.Append(text3 + "\r");
						}
					}
					else
					{
						stringBuilder.Append(text3 + "\r");
					}
				}
				if (!empty.EndsWith("\r") && !empty.EndsWith("\n"))
				{
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
				}
				empty6 = stringBuilder.ToString();
				if (maxLength > 0 && empty6.Length > maxLength)
				{
					empty6 = empty6.Substring(0, maxLength);
				}
				row.SetField(field, empty6);
				continue;
			}
			empty = row.Field<string>(field);
			num = empty.ToUpper().IndexOf(sourceText.ToUpper());
			if (num == -1)
			{
				continue;
			}
			empty2 = empty.Substring(0, num);
			empty = empty.Substring(num);
			empty5 = empty.Substring(0, length);
			empty3 = empty.Substring(length);
			if ((matchWholeWord && (empty3.Length == 0 || empty3.StartsWith(" "))) || !matchWholeWord)
			{
				if (!ignoreCase)
				{
					empty4 = ((!(empty.Substring(0, 1) == empty5.Substring(0, 1).ToUpper())) ? text.ToLower() : text.ToUpper());
					empty4 = ((!(empty5.Substring(1, 1) == empty5.Substring(1, 1).ToUpper())) ? (empty4 + text2.ToLower()) : (empty4 + text2.ToUpper()));
				}
				else
				{
					empty4 = destText;
				}
				empty6 = empty2 + empty4 + empty3;
				if (maxLength > 0 && empty6.Length > maxLength)
				{
					empty6 = empty6.Substring(0, maxLength);
				}
				row.SetField(field, empty6);
			}
		}
		currentContext.DDServerManager.UpdateData(sqlConnection, null, databaseName, dataTable, adapter);
	}

	private void updateAutologoutSettings(string databaseName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		AutoLogoutSettings autoLogoutSettings = new AutoLogoutSettings();
		SqlDataAdapter adapter;
		DataTable dataTable = GetDataTable(null, databaseName, "Select duUserID,duProperties,duAutoLogout,duInactiveCheckMinutes From DDUsers", fillSchema: true, out adapter);
		foreach (DataRow row in dataTable.Rows)
		{
			string text = row.Field<string>("duProperties");
			if (text == null || text.Length == 0)
			{
				continue;
			}
			autoLogoutSettings.LoadDefaults();
			stringBuilder.Length = 0;
			stringBuilder2.Length = 0;
			string[] array = text.Split('\r');
			foreach (string text2 in array)
			{
				int num = text2.IndexOf("=");
				if (num > 0)
				{
					string text3 = text2.Substring(0, num - 1).Trim().ToUpper();
					string value = text2.Substring(num + 1).Trim();
					switch (text3)
					{
					case "ALOSUNDAYSTARTTIME":
						autoLogoutSettings.ALOSundayStartTime = Convert.ToDecimal(value);
						break;
					case "ALOSUNDAYHOURS":
						autoLogoutSettings.ALOSundayHours = Convert.ToDecimal(value);
						break;
					case "ALOMONDAYSTARTTIME":
						autoLogoutSettings.ALOMondayStartTime = Convert.ToDecimal(value);
						break;
					case "ALOMONDAYHOURS":
						autoLogoutSettings.ALOMondayHours = Convert.ToDecimal(value);
						break;
					case "ALOTUESDAYSTARTTIME":
						autoLogoutSettings.ALOTuesdayStartTime = Convert.ToDecimal(value);
						break;
					case "ALOTUESDAYHOURS":
						autoLogoutSettings.ALOTuesdayHours = Convert.ToDecimal(value);
						break;
					case "ALOWEDNESDAYSTARTTIME":
						autoLogoutSettings.ALOWednesdayStartTime = Convert.ToDecimal(value);
						break;
					case "ALOWEDNESDAYHOURS":
						autoLogoutSettings.ALOWednesdayHours = Convert.ToDecimal(value);
						break;
					case "ALOTHURSDAYSTARTTIME":
						autoLogoutSettings.ALOThursdayStartTime = Convert.ToDecimal(value);
						break;
					case "ALOTHURSDAYHOURS":
						autoLogoutSettings.ALOThursdayHours = Convert.ToDecimal(value);
						break;
					case "ALOFRIDAYSTARTTIME":
						autoLogoutSettings.ALOFridayStartTime = Convert.ToDecimal(value);
						break;
					case "ALOFRIDAYHOURS":
						autoLogoutSettings.ALOFridayHours = Convert.ToDecimal(value);
						break;
					case "ALOSATURDAYSTARTTIME":
						autoLogoutSettings.ALOSaturdayStartTime = Convert.ToDecimal(value);
						break;
					case "ALOSATURDAYHOURS":
						autoLogoutSettings.ALOSaturdayHours = Convert.ToDecimal(value);
						break;
					case "ALOINACTIVEHOURS":
					{
						decimal num2 = Convert.ToDecimal(text2.Substring(num + 1).Trim());
						row.SetField("duInactiveCheckMinutes", Convert.ToInt16(num2 * 60m));
						break;
					}
					default:
						stringBuilder.Append(text2 + "\r");
						break;
					}
				}
			}
			if (stringBuilder.Length == 0)
			{
				row.SetField<string>("duProperties", null);
			}
			else
			{
				row.SetField("duProperties", stringBuilder.ToString());
			}
			autoLogoutSettings.SaveSettings(row);
		}
		currentContext.DDServerManager.UpdateData(null, null, databaseName, dataTable, adapter);
	}

	private void updateMyFolderUserSettings(string databaseName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		SqlDataAdapter adapter;
		DataTable dataTable = GetDataTable(null, databaseName, "Select duUserID,duProperties From DDUsers", fillSchema: true, out adapter);
		foreach (DataRow row in dataTable.Rows)
		{
			string text = row.Field<string>("duProperties");
			if (text == null || text.Length == 0)
			{
				continue;
			}
			stringBuilder.Length = 0;
			string[] array = text.Split('\r');
			foreach (string text2 in array)
			{
				int num = text2.IndexOf("=");
				if (num <= 0)
				{
					continue;
				}
				string text3 = text2.Substring(0, num - 1).Trim().ToUpper();
				string text4 = text2.Substring(num + 1).Trim();
				if (text3 == "MYFOLDERS")
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					if (!text4.Contains('{'))
					{
						text4 = text4.Remove(text4.IndexOf('\''), 1);
						text4 = text4.Remove(text4.LastIndexOf('\''), 1);
						stringBuilder2.Length = 0;
						string[] array2 = text4.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
						foreach (string value in array2)
						{
							DataTable dataTable2 = GetDataTable(databaseName, $"Select dxUniqueID From DDExplorer Where dxOldId = {Convert.ToInt32(value)} And dxType In (2,5) and dxUser = ''");
							if (dataTable2.Rows.Count != 0)
							{
								if (stringBuilder2.Length == 0)
								{
									stringBuilder2.Append("'{" + dataTable2.Rows[0].Field<Guid>("dxUniqueID").ToString() + "}");
								}
								else
								{
									stringBuilder2.Append(",{" + dataTable2.Rows[0].Field<Guid>("dxUniqueID").ToString() + "}");
								}
							}
						}
						if (stringBuilder2.Length != 0)
						{
							stringBuilder2.Append('\'');
							stringBuilder2.Append('\r');
							stringBuilder.Append("MyFolders = " + stringBuilder2);
						}
					}
					else
					{
						stringBuilder2.Append(text4.ToString());
						if (stringBuilder2.Length != 0)
						{
							stringBuilder2.Append('\r');
							stringBuilder.Append("MyFolders = " + stringBuilder2);
						}
					}
				}
				else
				{
					stringBuilder.Append(text2 + "\r");
				}
			}
			if (stringBuilder.Length == 0)
			{
				row.SetField<string>("duProperties", null);
			}
			else
			{
				row.SetField("duProperties", stringBuilder.ToString());
			}
		}
		currentContext.DDServerManager.UpdateData(null, null, databaseName, dataTable, adapter);
	}

	private void updateInputMaskForNumericControls(string databaseName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		SqlDataAdapter adapter;
		DataTable dataTable = GetDataTable(null, databaseName, "select deFormID, deClassID, deControlName, dePropertiesUser from DDFormDetails where deClassID like '%M1Numeric%' and dePropertiesUser like '%InputMask%' and deCustom <> 0", fillSchema: true, out adapter);
		foreach (DataRow row in dataTable.Rows)
		{
			string text = row.Field<string>("dePropertiesUser");
			if (text == null || text.Length == 0)
			{
				continue;
			}
			stringBuilder.Length = 0;
			string[] array = text.Split('\r');
			foreach (string text2 in array)
			{
				int num = text2.IndexOf("=");
				if (num > 0)
				{
					string text3 = text2.Substring(0, num - 1).Trim().ToUpper();
					string text4 = text2.Substring(num + 1).Trim();
					if (text3 == "INPUTMASK")
					{
						text4 = text4.Replace('N', 'n');
						text4 = text4.Replace('0', 'n');
						stringBuilder.AppendFormat("InputMask = {0}\r", text4);
					}
					else
					{
						stringBuilder.Append(text2 + "\r");
					}
				}
			}
			if (stringBuilder.Length == 0)
			{
				row.SetField<string>("dePropertiesUser", null);
			}
			else
			{
				row.SetField("dePropertiesUser", stringBuilder.ToString());
			}
		}
		currentContext.DDServerManager.UpdateData(null, null, databaseName, dataTable, adapter);
	}

	private ArrayList GetReportIniSettings(string iniFile)
	{
		iniFile = currentContext.Reports.Location + iniFile;
		if (File.Exists(iniFile))
		{
			return new ArrayList(File.ReadAllText(iniFile).Replace("\n", string.Empty).Split(new char[1] { '\r' }, StringSplitOptions.RemoveEmptyEntries));
		}
		return null;
	}

	private string getReportIniSettingsText(string folderName, string reportName, string userID, ArrayList settingsList)
	{
		bool flag = false;
		_ = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string settings in settingsList)
		{
			if (flag)
			{
				if (settings.StartsWith("["))
				{
					break;
				}
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(settings);
				}
				else
				{
					stringBuilder.AppendFormat("\r\n {0} ", settings);
				}
			}
			else if (settings[0] == '[' && settings.Equals("[" + reportName + "\\" + userID + "]", StringComparison.CurrentCultureIgnoreCase))
			{
				flag = true;
			}
		}
		return stringBuilder.ToString();
	}

	public void ReloadDDLangTables(string databaseName, Action<string> msgDelegate)
	{
		string empty = string.Empty;
		DataTable dataTable = GetDataTable(databaseName, "exec sp_tables @table_name = 'DDLANG%', @table_type = \"'TABLE'\"");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			empty = row.Field<string>("Table_Name").Trim();
			if (File.Exists(currentContext.Server.Location + "DataDict\\" + empty + ".xml"))
			{
				ReloadTable(databaseName, empty, recreateTable: false, msgDelegate, null);
			}
		}
	}

	public void MoveControlsOnForm(string databaseName, string formName, int oldHeight, int newHeight)
	{
		formName = formName.Trim().ToUpper();
		if (formName.Length == 0)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		int num = newHeight - oldHeight;
		string empty = string.Empty;
		string empty2 = string.Empty;
		int num2 = 0;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		int num3 = 0;
		SqlDataAdapter adapter = new SqlDataAdapter();
		DataTable dataTable = GetDataTable(null, databaseName, $"Select * From DDFormDetails Where deFormID = {formName.ToSql()} ", fillSchema: true, out adapter);
		foreach (DataRow row in dataTable.Rows)
		{
			flag2 = false;
			empty = string.Empty;
			string[] array = row.Field<string>("dePropertiesUser").Replace("\n", "").Split('\r');
			foreach (string text in array)
			{
				empty2 = text.Trim();
				if (empty2.Length == 0)
				{
					continue;
				}
				num2 = empty2.IndexOf("=");
				if (num2 < 0)
				{
					continue;
				}
				empty3 = empty2.Substring(0, num2).Trim();
				empty4 = empty2.Substring(num2 + 1).Trim();
				string text2 = empty3.ToUpper();
				if (!(text2 == "TOP"))
				{
					if (text2 == "HEIGHT")
					{
						if (row.Field<string>("deControlName").Trim().Length == 0)
						{
							empty = empty + "Height = " + (Convert.ToInt32(empty4) + num).ToSql() + "\r";
							flag2 = true;
						}
						else
						{
							empty = empty + text + "\r";
						}
					}
					else
					{
						empty = empty + text + "\r";
					}
					continue;
				}
				num3 = Convert.ToInt32(empty4);
				if (num3 >= oldHeight)
				{
					num3 += num;
					empty = empty + empty3 + " = " + num3.ToSql() + "\r";
					flag2 = true;
				}
				else
				{
					empty = empty + text + "\r";
				}
			}
			if (flag2)
			{
				flag = true;
				row.SetField("dePropertiesUser", empty);
			}
		}
		if (flag)
		{
			SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(adapter);
			adapter.UpdateCommand = sqlCommandBuilder.GetUpdateCommand();
			adapter.Update(dataTable);
		}
	}

	private string convertClassID(string oldClassID)
	{
		switch (oldClassID.ToUpper())
		{
		case "M1CONTROLS.M1EDITBOX":
		case "M1CONTROLS92.M1EDITBOX":
		case "M1CONTROLS91.M1EDITBOX":
			return "M1.Forms.Controls.M1RichTextEditor";
		case "M1CONTROLS.M1TEXTBOX":
		case "M1CONTROLS92.M1TEXTBOX":
		case "M1CONTROLS91.M1TEXTBOX":
		case "VB.TEXTBOX":
		case "VBTEXTBOX":
			return "M1.Forms.Controls.M1MaskedTextEditor";
		case "M1CONTROLS92.M1LABEL":
		case "M1CONTROLS91.M1LABEL":
		case "M1CONTROLS.M1LABEL":
			return "M1.Forms.Controls.M1Label";
		case "M1CONTROLS.M1COMMAND":
		case "M1CONTROLS92.M1COMMAND":
		case "M1CONTROLS91.M1COMMAND":
			return "M1.Forms.Controls.M1Button";
		case "M1CONTROLS.M1CHECKBOX":
		case "M1CONTROLS92.M1CHECKBOX":
		case "M1CONTROLS91.M1CHECKBOX":
			return "M1.Forms.Controls.M1CheckBox";
		case "M1CONTROLS.M1NUMERICBOX":
		case "M1CONTROLS91.M1NUMERICBOX":
		case "M1CONTROLS92.M1NUMERICBOX":
			return "M1.Forms.Controls.M1NumericEditor";
		case "M1CONTROLS.M1COMBOBOX":
		case "M1CONTROLS92.M1COMBOBOX":
		case "M1CONTROLS91.M1COMBOBOX":
			return "M1.Forms.Controls.M1ComboBox";
		case "M1CONTROLS.M1DATEBOX":
		case "M1CONTROLS92.M1DATEBOX":
		case "M1CONTROLS91.M1DATEBOX":
			return "M1.Forms.Controls.M1DateEditor";
		case "M1CONTROLS.M1GROUPLINE":
		case "M1CONTROLS91.M1GROUPLINE":
		case "M1CONTROLS92.M1GROUPLINE":
			return "M1.Forms.Controls.M1GroupLine";
		case "M1CONTROLS92.M1SEARCH":
		case "M1CONTROLS91.M1SEARCH":
		case "M1CONTROLS.M1SEARCH":
			return "M1.Forms.Controls.M1SearchButton";
		case "M1CONTROLS.M1GRIDLITE":
		case "M1CONTROLS92.M1GRIDLITE":
		case "M1CONTROLS91.M1GRIDLITE":
		case "M1CONTROLS91.M1GRID":
		case "M1CONTROLS92.M1GRID":
		case "M1CONTROLS.M1GRID":
			return "M1.Forms.Controls.M1Grid";
		case "M1CONTROLS.M1OPTIONBUTTON":
		case "M1CONTROLS91.M1OPTIONBUTTON":
		case "M1CONTROLS92.M1OPTIONBUTTON":
			return "M1.Forms.Controls.M1OptionButton";
		case "M1CONTROLS.M1DATACONTROL":
		case "M1CONTROLS91.M1DATACONTROL":
		case "M1CONTROLS92.M1DATACONTROL":
			return "M1.Forms.Controls.M1UIBindingSource";
		case "M1CONTROLS.M1PLANTDEPARTMENT":
		case "M1CONTROLS92.M1PLANTDEPARTMENT":
		case "M1CONTROLS91.M1PLANTDEPARTMENT":
			return "M1.Forms.Controls.M1ComboBox";
		case "M1CONTROLS92.M1AGING":
		case "M1CONTROLS91.M1AGING":
		case "M1CONTROLS.M1AGING":
			return "M1.Ax.Erp.Forms.Financial.M1Aging";
		case "M1CONTROLS92.M1DOCMGR":
		case "M1CONTROLS91.M1DOCMGR":
		case "M1CONTROLS.M1DOCMGR":
			return "M1.Ax.Erp.Forms.DocumentRegister.M1DocMgr";
		case "M1CONTROLS.M1EXPENSEACCOUNTS":
		case "M1CONTROLS92.M1EXPENSEACCOUNTS":
		case "M1CONTROLS91.M1EXPENSEACCOUNTS":
			return "M1.Forms.Controls.M1MultiItemComboBox";
		case "M1CONTROLS.M1COMMISSIONSPLIT":
		case "M1CONTROLS92.M1COMMISSIONSPLIT":
		case "M1CONTROLS91.M1COMMISSIONSPLIT":
			return "M1.Forms.Controls.M1MultiItemComboBox";
		case "M1CONTROLS92.M1IMAGE":
		case "M1CONTROLS91.M1IMAGE":
		case "M1CONTROLS.M1IMAGE":
			return "M1.Forms.Controls.M1Image";
		case "M1CONTROLS.M13DHLINE":
		case "M1CONTROLS92.M13DHLINE":
		case "M1CONTROLS91.M13DHLINE":
			return "M1.Forms.Controls.M13DHLine";
		case "M1CONTROLS.M13DVLINE":
		case "M1CONTROLS92.M13DVLINE":
		case "M1CONTROLS91.M13DVLINE":
			return "M1.Forms.Controls.M13DVLine";
		case "M1CONTROLS91.M1PRODUCTCONFIGURATOR":
		case "M1CONTROLS92.M1PRODUCTCONFIGURATOR":
		case "M1CONTROLS.M1PRODUCTCONFIGURATOR":
			return "M1.Ax.Erp.Forms.Production.ProductConfigurator.M1ProductConfigurator";
		case "M1CONTROLS92.M1TIMER":
		case "M1CONTROLS91.M1TIMER":
		case "M1CONTROLS.M1TIMER":
			return "M1.Forms.Controls.M1Timer";
		default:
			return oldClassID;
		}
	}

	private Stream GetEmbeddedFile(string filePath, string caseInsensitiveName)
	{
		Stream stream = null;
		stream = GetEmbeddedFile(caseInsensitiveName, Assembly.LoadFile(filePath));
		if (stream != null)
		{
			return stream;
		}
		return null;
	}

	private Stream GetEmbeddedFile(string caseInsensitiveName, Assembly asm)
	{
		if (asm != null)
		{
			string[] manifestResourceNames = asm.GetManifestResourceNames();
			foreach (string text in manifestResourceNames)
			{
				if (text.EndsWith(caseInsensitiveName, StringComparison.CurrentCultureIgnoreCase))
				{
					return asm.GetManifestResourceStream(text);
				}
			}
		}
		return null;
	}

	private Dictionary<string, ConvertRowInfo> LoadVersion8Form(string oldFormName)
	{
		DataTable dataTable = new DataTable();
		string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "M1.UI.Design.Definitions.dll");
		dataTable.ReadXmlSchema(GetEmbeddedFile(filePath, "schema.xml"));
		dataTable.ReadXml(GetEmbeddedFile(filePath, oldFormName + ".xml"));
		Dictionary<string, ConvertRowInfo> dictionary = new Dictionary<string, ConvertRowInfo>(StringComparer.CurrentCultureIgnoreCase);
		foreach (DataRow row in dataTable.Rows)
		{
			ConvertRowInfo convertRowInfo = LoadRowInfo(row, new ConvertRowInfo(row.Field<string>("deControlName")), row.Field<string>("deProperties"));
			convertRowInfo.IsCustom = false;
			foreach (KeyValuePair<string, string> customProperty in convertRowInfo.CustomProperties)
			{
				convertRowInfo.StandardProperties.Add(customProperty.Key, customProperty.Value);
			}
			convertRowInfo.CustomProperties.Clear();
			dictionary.Add(convertRowInfo.Name, convertRowInfo);
		}
		return dictionary;
	}

	private Dictionary<string, ConvertRowInfo> LoadVersion9Form(string oldFormName)
	{
		DataTable dataTable = new DataTable();
		string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "M1.Ax.Erp.Forms.dll");
		dataTable.ReadXmlSchema(GetEmbeddedFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "M1.Forms.Design.Definitions.dll"), "schema.xml"));
		dataTable.ReadXml(GetEmbeddedFile(filePath, oldFormName + ".xml"));
		Dictionary<string, ConvertRowInfo> dictionary = new Dictionary<string, ConvertRowInfo>(StringComparer.CurrentCultureIgnoreCase);
		foreach (DataRow row in dataTable.Rows)
		{
			ConvertRowInfo convertRowInfo = LoadRowInfo(row, new ConvertRowInfo(row.Field<string>("deControlName")), row.Field<string>("deProperties"));
			convertRowInfo.IsCustom = false;
			foreach (KeyValuePair<string, string> customProperty in convertRowInfo.CustomProperties)
			{
				convertRowInfo.StandardProperties.Add(customProperty.Key, customProperty.Value);
			}
			convertRowInfo.CustomProperties.Clear();
			dictionary.Add(convertRowInfo.Name, convertRowInfo);
		}
		return dictionary;
	}

	private ConvertRowInfo LoadRowInfo(DataRow row, ConvertRowInfo info, string properties)
	{
		info.Row = row;
		info.OldClassID = row.Field<string>("deClassID");
		info.NewClassID = convertClassID(info.OldClassID.ToUpper());
		if (!string.IsNullOrWhiteSpace(properties))
		{
			string[] array = properties.Replace("\n", "").Split('\r');
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (text.Length == 0)
				{
					continue;
				}
				int num = text.IndexOf("=");
				if (num >= 0)
				{
					string text2 = text.Substring(0, num).Trim();
					string text3 = text.Substring(num + 1).Trim();
					switch (text2.ToUpper())
					{
					case "TOP":
						info.Top = Convert.ToInt32(text3);
						break;
					case "LEFT":
						info.Left = Convert.ToInt32(text3);
						break;
					case "HEIGHT":
						info.Height = Convert.ToInt32(text3);
						break;
					case "WIDTH":
						info.Width = Convert.ToInt32(text3);
						break;
					case "DATASOURCE":
						info.DataSource = text3.Replace("\"", "");
						break;
					case "DATAFIELD":
						info.DataField = text3.Replace("\"", "");
						break;
					case "DATAFIELDTEXT":
						info.DataFieldText = text3.Replace("\"", "");
						break;
					case "CAPTION":
						info.SetCustomProperty("Text", text3);
						break;
					default:
						info.SetCustomProperty(text2, text3);
						break;
					}
				}
			}
		}
		return info;
	}

	private void getDataBindingsFromProperties(string properties, ref string dataSource, ref string dataField)
	{
		if (string.IsNullOrWhiteSpace(properties))
		{
			return;
		}
		string[] array = properties.Replace("\n", "").Split('\r');
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (text.Length == 0)
			{
				continue;
			}
			int num = text.IndexOf("=");
			if (num < 0)
			{
				continue;
			}
			string text2 = text.Substring(0, num).Trim();
			string text3 = text.Substring(num + 1).Trim();
			if (text2.ToUpper() == "DATABINDINGS")
			{
				string[] array2 = text3.Split(',');
				if (array2.Length != 0 && array2.Length == 5)
				{
					dataSource = array2[1];
					dataField = array2[2];
				}
			}
		}
	}

	private string convertCustomFormCodeForGrids(string databaseName, string code, Dictionary<string, ConvertRowInfo> controls, SqlTransaction transaction)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string[] array = code.Replace("\n", string.Empty).Split('\r');
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i];
			string text2 = text.TrimStart(' ', '\t');
			if (!text2.StartsWith("'") && !text2.StartsWith("Rem", StringComparison.CurrentCultureIgnoreCase) && text.IndexOf(".SetGridRs", StringComparison.CurrentCultureIgnoreCase) != -1)
			{
				string text3 = text.Substring(text.IndexOf("Controls(\"", StringComparison.CurrentCultureIgnoreCase) + 10);
				int num = text3.IndexOf("\"");
				text3 = ((num == -1) ? string.Empty : text3.Substring(0, num));
				if (!string.IsNullOrWhiteSpace(text3))
				{
					string text4 = string.Empty;
					string text5 = text.Substring(text.IndexOf(".SetGridRs", StringComparison.CurrentCultureIgnoreCase) + 10).Trim();
					num = text5.IndexOf(',');
					string text6;
					if (num != -1)
					{
						if (text5.IndexOf("\"") != -1)
						{
							text6 = text5.Substring(num + 1);
							text6 = text6.Replace('"', ' ').Replace(')', ' ').Trim();
							if (text6.IndexOf("'") != -1)
							{
								text6 = text6.Substring(0, text6.IndexOf("'"));
							}
							text5 = text5.Substring(0, num);
							text5 = text5.Replace('(', ' ').Trim();
						}
						else
						{
							string empty = string.Empty;
							empty = text5.Substring(num + 1).Trim();
							text6 = string.Empty;
							text5 = text5.Substring(0, num).Trim();
							text5 = text5.Replace('(', ' ').Trim();
							for (int num2 = i - 1; num2 >= 0; num2--)
							{
								string text7 = array[num2].Replace(" ", string.Empty);
								string text8 = text7.TrimStart(' ', '\t');
								if (!text8.StartsWith("'") && !text8.StartsWith("Rem", StringComparison.CurrentCultureIgnoreCase) && text7.IndexOf(empty, StringComparison.CurrentCultureIgnoreCase) != -1)
								{
									text6 = text7.Replace("=", string.Empty).Substring(text7.IndexOf(empty, StringComparison.CurrentCultureIgnoreCase) + empty.Length).Trim();
									text6 = text6.Replace('"', ' ').Replace(')', ' ').Trim();
									text6 = text6.TrimEnd(' ', '\t');
									break;
								}
							}
						}
					}
					else
					{
						text6 = string.Empty;
					}
					for (int num3 = i - 1; num3 >= 0; num3--)
					{
						string text7 = array[num3];
						string text8 = text7.TrimStart(' ', '\t');
						if (!text8.StartsWith("'") && !text8.StartsWith("Rem", StringComparison.CurrentCultureIgnoreCase) && text7.IndexOf(text5 + ".Open", StringComparison.CurrentCultureIgnoreCase) != -1)
						{
							text4 = text7.Substring(text7.IndexOf(".Open", StringComparison.CurrentCultureIgnoreCase) + 5).Trim();
							num = text4.LastIndexOf("Connection", StringComparison.CurrentCultureIgnoreCase);
							if (num != -1)
							{
								text4 = text4.Substring(0, num);
							}
							num = text4.LastIndexOf(',');
							if (num != -1)
							{
								text4 = text4.Substring(0, num);
							}
							text4 = text4.TrimEnd(' ', '\t');
							if (text4.IndexOf("\"") != -1)
							{
								break;
							}
							for (int num4 = num3 - 1; num4 >= 0; num4--)
							{
								text7 = array[num4];
								text8 = text7.TrimStart(' ', '\t');
								if (!text8.StartsWith("'") && !text8.StartsWith("Rem", StringComparison.CurrentCultureIgnoreCase) && text7.IndexOf(text4, StringComparison.CurrentCultureIgnoreCase) != -1)
								{
									text4 = text7.Substring(text7.IndexOf(text4, StringComparison.CurrentCultureIgnoreCase) + text4.Length).Trim();
									text4 = text4.TrimEnd(' ', '\t');
									num = text4.IndexOf("Select", 0, StringComparison.CurrentCultureIgnoreCase);
									if (num != -1)
									{
										text4 = text4.Substring(num);
									}
									break;
								}
							}
							break;
						}
					}
					if (!string.IsNullOrWhiteSpace(text4) && !string.IsNullOrWhiteSpace(text3) && controls.ContainsKey(text3))
					{
						int num5 = 1;
						string text9 = "uCustomBs";
						while (controls.ContainsKey(text9 + num5))
						{
							num5++;
						}
						string text10 = text9 + num5;
						ConvertRowInfo convertRowInfo = new ConvertRowInfo(text10);
						convertRowInfo.NewClassID = "M1.Forms.Controls.M1UIBindingSource";
						convertRowInfo.IsCustom = true;
						_ = string.Empty;
						num = 0;
						string value = "Controls(\"";
						num = text4.LastIndexOf(value, StringComparison.CurrentCultureIgnoreCase);
						if (num != -1)
						{
							value = "\").Value";
							string key = text4.Substring(num + 10, text4.LastIndexOf(value, StringComparison.CurrentCultureIgnoreCase) - num - 10);
							if (controls.ContainsKey(key) && !string.IsNullOrWhiteSpace(controls[key].DataField))
							{
								convertRowInfo.SetCustomProperty("DataBindings", "\"ParentFieldValue," + controls[key].DataSource + "," + controls[key].DataField + ",true,OnPropertyChanged\"");
							}
						}
						else
						{
							num = 0;
							value = "0=1";
							num = text4.LastIndexOf(value, StringComparison.CurrentCultureIgnoreCase);
							if (num != -1)
							{
								for (int j = 0; j < array.Length; j++)
								{
									string text7 = array[j].Replace(" ", string.Empty);
									string text8 = text7.TrimStart(' ', '\t');
									value = "Controls(\"" + text3 + "\").RefreshRS";
									if (text8.StartsWith("'") || text8.StartsWith("Rem", StringComparison.CurrentCultureIgnoreCase) || text7.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) == -1)
									{
										continue;
									}
									for (int num6 = j - 1; num6 >= 0; num6--)
									{
										text7 = array[num6];
										text8 = text7.TrimStart(' ', '\t');
										if (!text8.StartsWith("'") && !text8.StartsWith("Rem", StringComparison.CurrentCultureIgnoreCase) && text7.IndexOf(text5 + ".Open", StringComparison.CurrentCultureIgnoreCase) != -1)
										{
											text4 = text7.Substring(text7.IndexOf(".Open", StringComparison.CurrentCultureIgnoreCase) + 5).Trim();
											num = text4.LastIndexOf("Connection", StringComparison.CurrentCultureIgnoreCase);
											if (num != -1)
											{
												text4 = text4.Substring(0, num);
											}
											num = text4.LastIndexOf(',');
											if (num != -1)
											{
												text4 = text4.Substring(0, num);
											}
											text4 = text4.TrimEnd(' ', '\t');
											if (text4.IndexOf("\"") != -1)
											{
												break;
											}
											for (int num7 = num6 - 1; num7 >= 0; num7--)
											{
												text7 = array[num7].Replace(" ", string.Empty);
												text8 = text7.TrimStart(' ', '\t');
												if (!text8.StartsWith("'") && !text8.StartsWith("Rem", StringComparison.CurrentCultureIgnoreCase) && text7.IndexOf(text4, StringComparison.CurrentCultureIgnoreCase) != -1)
												{
													text4 = text7.Replace("=", string.Empty).Substring(text7.IndexOf(text4, StringComparison.CurrentCultureIgnoreCase) + text4.Length).Trim();
													text4 = text6.Replace('"', ' ').Replace(')', ' ').Trim();
													text4 = text6.TrimEnd(' ', '\t');
													break;
												}
											}
											break;
										}
									}
									num = 0;
									value = "Controls(\"";
									num = text4.LastIndexOf(value, StringComparison.CurrentCultureIgnoreCase);
									if (num != -1)
									{
										value = "\").Value";
										string key = text4.Substring(num + 10, text4.LastIndexOf(value, StringComparison.CurrentCultureIgnoreCase) - num - 10);
										if (controls.ContainsKey(key) && !string.IsNullOrWhiteSpace(controls[key].DataField))
										{
											convertRowInfo.SetCustomProperty("DataBindings", "\"ParentFieldValue," + controls[key].DataSource + "," + controls[key].DataField + ",true,OnPropertyChanged\"");
										}
									}
									break;
								}
							}
						}
						string text11 = createGridDef(databaseName, transaction, text4, text6);
						convertRowInfo.SetCustomProperty("DataSourceGridID", "\"" + text11 + "\"");
						controls.Add(text10, convertRowInfo);
						controls[text3].SetCustomProperty("DataSource", text10);
					}
				}
				text = "' " + text + "\r\n";
			}
			stringBuilder.AppendLine(text);
		}
		array = stringBuilder.ToString().Replace("\n", string.Empty).Split('\r');
		stringBuilder.Length = 0;
		for (int k = 0; k < array.Length; k++)
		{
			string text = array[k];
			string text2 = text.TrimStart(' ', '\t');
			if (!text2.StartsWith("'") && !text2.StartsWith("Rem", StringComparison.CurrentCultureIgnoreCase) && (text.IndexOf(".RefreshRS", StringComparison.CurrentCultureIgnoreCase) != -1 || text.IndexOf(".SetGridRs", StringComparison.CurrentCultureIgnoreCase) != -1))
			{
				text = "' " + text + "\r\n";
			}
			stringBuilder.AppendLine(text);
		}
		return stringBuilder.ToString();
	}

	private string getNextGridID(string databaseName, SqlTransaction transaction, string baseName)
	{
		int num = 0;
		string text;
		do
		{
			num++;
			text = baseName + num;
		}
		while (GetDataTable(databaseName, "Select djGridID From DDGrids Where djGridID = " + M1Util.ConvertToSql(text)).Rows.Count != 0);
		return text;
	}

	private string createGridDef(string databaseName, SqlTransaction transaction, string query, string fieldsList)
	{
		string nextGridID = getNextGridID(databaseName, transaction, "uEntry");
		QueryParseResult queryParseResult = QueryParser.Parse(query, cleanupFormatting: false);
		if (queryParseResult.Errors != null && queryParseResult.Errors.Count != 0)
		{
			return string.Empty;
		}
		SqlDataAdapter adapter;
		DataTable dataTable = GetDataTable(null, databaseName, "Select * From DDGrids Where 0=1", fillSchema: true, out adapter, transaction);
		DataRow dataRow = dataTable.AddBlankRow();
		dataRow["djGridID"] = nextGridID;
		dataRow["djTable"] = ((queryParseResult.PrimaryTable.Length > 30) ? queryParseResult.PrimaryTable.Substring(0, 30) : queryParseResult.PrimaryTable);
		string text = "Entry grid for " + queryParseResult.PrimaryTable;
		dataRow["djDesc"] = ((text.Length > 50) ? text.Substring(0, 50) : text);
		dataRow["djCustom"] = true;
		SqlDataAdapter adapter2;
		DataTable dataTable2 = GetDataTable(null, databaseName, "Select * From DDGridDetails Where 0=1", fillSchema: true, out adapter2, transaction);
		DataRow dataRow2 = dataTable2.AddBlankRow();
		dataRow2["dgGridID"] = nextGridID;
		dataRow2.SetField("dgFlds", (!string.IsNullOrWhiteSpace(fieldsList)) ? fieldsList : (string.IsNullOrWhiteSpace(queryParseResult.Fields) ? null : queryParseResult.Fields));
		dataRow2.SetField("dgFrom", queryParseResult.From);
		dataRow2.SetField("dgGrp", string.IsNullOrWhiteSpace(queryParseResult.GroupBy) ? null : queryParseResult.GroupBy);
		dataRow2.SetField("dgSOrd", string.IsNullOrWhiteSpace(queryParseResult.OrderBy) ? null : queryParseResult.OrderBy);
		dataRow2["dgEdit"] = true;
		dataRow2["dgCustom"] = true;
		currentContext.DDServerManager.UpdateData(null, null, databaseName, dataTable, adapter, transaction);
		currentContext.DDServerManager.UpdateData(null, null, databaseName, dataTable2, adapter2, transaction);
		return nextGridID;
	}

	public void convertQueryToGridDefinition(string databaseName, string formName, string code, string gridName, SqlTransaction transaction)
	{
		new StringBuilder();
		string empty = string.Empty;
		Dictionary<string, ConvertRowInfo> dictionary = new Dictionary<string, ConvertRowInfo>();
		List<string> list = new List<string>();
		string text = code.TrimEnd(' ').Replace(" _", string.Empty).Replace("\n", string.Empty)
			.Replace("\r", string.Empty)
			.Replace("\t", string.Empty);
		Dictionary<string, ConvertRowInfo> dictionary2 = LoadVersion9Form(formName);
		SqlDataAdapter adapter;
		DataTable dataTable = GetDataTable(null, databaseName, $"Select * From DDFormDetails Where deFormID = {formName.ToSql()}", fillSchema: true, out adapter, transaction);
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				string text2 = row.Field<string>("deControlName");
				ConvertRowInfo convertRowInfo = LoadRowInfo(row, new ConvertRowInfo(text2), row.Field<string>("dePropertiesUser"));
				convertRowInfo.IsCustom = row.Field<bool>("deCustom");
				dictionary.Add(convertRowInfo.Name, convertRowInfo);
				if (convertRowInfo.NewClassID.Contains("M1GRID"))
				{
					list.Add(text2);
				}
			}
		}
		if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(gridName) || !dictionary.ContainsKey(gridName))
		{
			return;
		}
		int num = 1;
		string text3 = "uCustomBs";
		while (dictionary.ContainsKey(text3 + num))
		{
			num++;
		}
		string text4 = text3 + num;
		ConvertRowInfo convertRowInfo2 = new ConvertRowInfo(text4);
		convertRowInfo2.NewClassID = "M1.Forms.Controls.M1UIBindingSource";
		convertRowInfo2.IsCustom = true;
		string dataField = string.Empty;
		string dataSource = string.Empty;
		int num2 = 0;
		string value = "Controls(\"";
		num2 = text.LastIndexOf(value, StringComparison.CurrentCultureIgnoreCase);
		if (num2 != -1)
		{
			value = "\").Value";
			string key = text.Substring(num2 + 10, text.LastIndexOf(value, StringComparison.CurrentCultureIgnoreCase) - num2 - 10);
			if (dictionary2.ContainsKey(key))
			{
				getDataBindingsFromProperties(dictionary2[key].Row.Field<string>("deProperties"), ref dataSource, ref dataField);
				if (!string.IsNullOrWhiteSpace(dataField) && !string.IsNullOrWhiteSpace(dataSource))
				{
					convertRowInfo2.SetCustomProperty("DataBindings", "\"ParentFieldValue," + dataSource + "," + dataField + ",true,OnPropertyChanged\"");
				}
			}
		}
		string text5 = createGridDef(databaseName, transaction, text, empty);
		convertRowInfo2.SetCustomProperty("DataSourceGridID", "\"" + text5 + "\"");
		dictionary.Add(text4, convertRowInfo2);
		ConvertRowInfo convertRowInfo3 = dictionary[gridName];
		convertRowInfo3.SetCustomProperty("DataSource", text4);
		DataRow dataRow = null;
		foreach (ConvertRowInfo value2 in dictionary.Values)
		{
			if (value2.Row == null)
			{
				DataRow dataRow2 = dataTable.NewRow().BlankRow();
				dataRow2["deParentID"] = string.Empty;
				dataRow2["deParentIDUser"] = DBNull.Value;
				dataRow2["deNestedName"] = string.Empty;
				dataRow2["deNestedNameUser"] = DBNull.Value;
				dataRow2["deAppExtensionID"] = string.Empty;
				dataRow2["deSequence"] = 0;
				dataRow2["deProperties"] = DBNull.Value;
				dataRow2["dePropertiesUser"] = DBNull.Value;
				dataRow2["deSequenceUser"] = DBNull.Value;
				value2.Row = dataRow2;
				value2.Row.SetField("deFormID", formName);
				value2.Row.SetField("deControlName", value2.Name);
				dataTable.Rows.Add(dataRow2);
				dataRow = dataRow2;
			}
			ConvertItemProperties(value2, value2.IsCustom, string.Empty);
			value2.Row.SetField("dePropertiesUser", value2.GetCustomProperties());
			value2.Row.SetField("deFormID", formName);
			value2.Row.SetField("deClassID", value2.NewClassID);
			value2.Row.SetField("deSequenceUser", value2.Sequence);
			value2.Row.SetField("deCustom", value2.IsCustom);
			if (value2.Group != null && !value2.Row.Field<string>("deParentID").Equals(value2.Group.Name, StringComparison.CurrentCultureIgnoreCase))
			{
				value2.Row.SetField("deParentIDUser", value2.Group.Name);
				value2.Row.SetField("deNestedNameUser", "WorkingArea");
			}
		}
		dictionary.Clear();
		currentContext.DDServerManager.UpdateData(null, null, databaseName, new DataRow[2] { dataRow, convertRowInfo3.Row }, adapter, transaction);
	}

	public string convertCustomFormCode(string code)
	{
		return convertCustomFormCode(code, null);
	}

	private string commentLine(string line)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string[] array = line.Replace("\n", string.Empty).Split('\r');
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].TrimStart(' ', '\t').StartsWith("'"))
			{
				stringBuilder.Append("'");
			}
			stringBuilder.Append(array[i]);
			if (i < array.Length - 1)
			{
				stringBuilder.Append("\r\n");
			}
		}
		return stringBuilder.ToString();
	}

	private string convertCustomFormCode(string code, Dictionary<string, ConvertRowInfo> controls)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		StringBuilder stringBuilder3 = new StringBuilder();
		bool flag = false;
		string[] array = code.Replace("\n", string.Empty).Split('\r');
		foreach (string text in array)
		{
			string text2 = text;
			if (!text2.StartsWith("Rem", StringComparison.CurrentCultureIgnoreCase) && !text2.StartsWith("'") && text.IndexOf("Sender", StringComparison.CurrentCulture) != -1)
			{
				text2 = text.Replace("Sender", "Sender1", caseInsensitive: true);
			}
			stringBuilder.AppendLine(text2);
		}
		code = stringBuilder.ToString();
		stringBuilder.Length = 0;
		string[] array2 = code.Replace("\n", string.Empty).Split('\r');
		for (int j = 0; j < array2.Length; j++)
		{
			string text3 = array2[j];
			while (text3.TrimEnd().EndsWith("_"))
			{
				j++;
				text3 = ((!text3.TrimStart(' ', '\t').StartsWith("'") || array2[j].TrimStart(' ', '\t').StartsWith("'")) ? (text3 + "\r\n" + array2[j]) : (text3 + "\r\n'" + array2[j]));
			}
			string text4 = text3.TrimStart(' ', '\t');
			if (text4.StartsWith("Function ", StringComparison.CurrentCultureIgnoreCase) || text4.StartsWith("Function\t", StringComparison.CurrentCultureIgnoreCase) || text4.StartsWith("Sub ", StringComparison.CurrentCultureIgnoreCase) || text4.StartsWith("Private Function ", StringComparison.CurrentCultureIgnoreCase) || text4.StartsWith("Private Sub ", StringComparison.CurrentCultureIgnoreCase) || text4.StartsWith("Public Function ", StringComparison.CurrentCultureIgnoreCase) || text4.StartsWith("Public Sub ", StringComparison.CurrentCultureIgnoreCase))
			{
				flag = true;
			}
			if (flag)
			{
				string text2 = text3;
				if (text2.IndexOf(".SaveRecordEx", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					int startIndex = 0;
					string value = "Controls(\"";
					startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
					if (startIndex != -1)
					{
						value = "\").";
						string text5 = text2.Substring(startIndex, text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase) - startIndex);
						text2 = commentLine(text2) + "\r\n" + text2.Replace(text5 + value + "SaveRecordEx", "this.SaveView", caseInsensitive: true);
					}
				}
				else if (text2.IndexOf(".SaveRecord", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					int startIndex = 0;
					string value = "Controls(\"";
					startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
					if (startIndex != -1)
					{
						value = "\").";
						string text5 = text2.Substring(startIndex, text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase) - startIndex);
						text2 = commentLine(text2) + "\r\n" + text2.Replace(text5 + value + "SaveRecord", "this.SaveView", caseInsensitive: true);
					}
				}
				else if (text2.IndexOf(".IsRecordValid", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					int startIndex = 0;
					string value = "Controls(\"";
					startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
					if (startIndex != -1)
					{
						value = "\").";
						string text5 = text2.Substring(startIndex, text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase) - startIndex);
						text2 = commentLine(text2) + "\r\n" + text2.Replace(text5 + value + "IsRecordValid", "this.IsRecordValid", caseInsensitive: true);
					}
				}
				else if (text2.IndexOf(".Revert", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					int startIndex = 0;
					string value = "Controls(\"";
					startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
					if (startIndex != -1)
					{
						value = "\").";
						string text5 = text2.Substring(startIndex, text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase) - startIndex);
						text2 = commentLine(text2) + "\r\n" + text2.Replace(text5 + value + "Revert", "this.RevertView", caseInsensitive: true);
					}
				}
				else if (text2.IndexOf(".ReloadCurrentData", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					int startIndex = 0;
					string value = "Controls(\"";
					startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
					if (startIndex != -1)
					{
						value = "\").";
						string text5 = text2.Substring(startIndex, text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase) - startIndex);
						text2 = commentLine(text2) + "\r\n" + text2.Replace(text5 + value + "ReloadCurrentData", "this.RevertView", caseInsensitive: true);
					}
				}
				else if (text2.IndexOf(".SetActiveView", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					int startIndex = 0;
					string value = "Controls(\"";
					startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
					if (startIndex != -1)
					{
						value = "\").";
						string text5 = text2.Substring(startIndex, text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase) - startIndex);
						text2 = commentLine(text2) + "\r\n" + text2.Replace(text5 + value + "SetActiveView", "this.SetActiveView", caseInsensitive: true);
					}
				}
				else if (text2.IndexOf(".NavigateTo", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					int startIndex = 0;
					string value = "Controls(\"";
					startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
					if (startIndex != -1)
					{
						value = "\").";
						string text5 = text2.Substring(startIndex, text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase) - startIndex);
						text2 = commentLine(text2) + "\r\n" + text2.Replace(text5 + value + "NavigateTo", "this.NavigateToByArray", caseInsensitive: true);
					}
				}
				else if (text2.IndexOf(".RemoveNodeByKeys", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					int startIndex = 0;
					string value = "Controls(\"";
					startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
					if (startIndex != -1)
					{
						value = "\").";
						string text5 = text2.Substring(startIndex, text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase) - startIndex);
						text2 = commentLine(text2) + "\r\n" + text2.Replace(text5 + value + "RemoveNodeByKeys", "this.RemoveNodeByKeys", caseInsensitive: true);
					}
				}
				else if (text2.IndexOf(".ViewNavigateEvent", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					int startIndex = 0;
					string value = "Controls(\"";
					startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
					if (startIndex != -1)
					{
						value = "\").";
						string text5 = text2.Substring(startIndex, text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase) - startIndex);
						text2 = commentLine(text2) + "\r\n" + text2.Replace(text5 + value + "ViewNavigateEvent", "this.SetActiveView", caseInsensitive: true);
					}
				}
				else if (text2.IndexOf(".ForceReadOnly", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					int startIndex = 0;
					string value = "Controls(\"";
					startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
					if (startIndex != -1)
					{
						value = "\").";
						string text5 = text2.Substring(startIndex, text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase) - startIndex);
						text2 = commentLine(text2) + "\r\n" + text2.Replace(text5 + value + "ForceReadOnly", "this.SetReadOnlyOverride", caseInsensitive: true);
					}
				}
				else if (text2.IndexOf(".ShowInfoMsg", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					int startIndex = 0;
					string value = "Controls(\"";
					startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
					if (startIndex != -1)
					{
						value = "\").";
						string text5 = text2.Substring(startIndex, text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase) - startIndex);
						text2 = commentLine(text2) + "\r\n" + text2.Replace(text5 + value + "ShowInfoMsg", "this.ShowInfoMsg", caseInsensitive: true);
					}
				}
				else if (text2.IndexOf(".CloseForm", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					int startIndex = 0;
					string value = "Controls(\"";
					startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
					if (startIndex != -1)
					{
						value = "\").";
						string text5 = text2.Substring(startIndex, text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase) - startIndex);
						text2 = commentLine(text2) + "\r\n" + text2.Replace(text5 + value + "CloseForm", "this.CloseForm", caseInsensitive: true);
					}
				}
				else if (text2.IndexOf("\").RunReport", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					int startIndex = 0;
					string value = "Controls(\"";
					startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
					if (startIndex != -1)
					{
						value = "\").";
						string text5 = text2.Substring(startIndex, text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase) - startIndex);
						text2 = commentLine(text2) + "\r\n" + text2.Replace(text5 + value + "RunReport", "this.RunReport", caseInsensitive: true);
					}
				}
				if (text2.IndexOf("Forms.Report", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace("App.Convert.StringToSql", "App.Convert.ToScript", caseInsensitive: true);
				}
				if (text2.IndexOf("App.OpenReport", StringComparison.CurrentCultureIgnoreCase) != -1 || text2.IndexOf("App.RunReport", StringComparison.CurrentCultureIgnoreCase) != -1 || text2.IndexOf("App.RunReportByTableKeys", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace("App.AddQuotes", "App.Convert.ToScript", caseInsensitive: true);
				}
				convertEvents(events, ref text2);
				if (text2.IndexOf("aParms(0)", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace("aParms(0)", "e.FieldName", caseInsensitive: true);
				}
				if (text2.IndexOf("UBound(aParametersEx)", StringComparison.CurrentCultureIgnoreCase) != -1 && text2.IndexOf("_ActionMessage(", StringComparison.CurrentCultureIgnoreCase) == -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace("UBound(aParametersEx)", "e.ParametersExLength - 1", caseInsensitive: true);
				}
				if (text2.IndexOf("UBound(aParameters)", StringComparison.CurrentCultureIgnoreCase) != -1 && text2.IndexOf("_ActionMessage(", StringComparison.CurrentCultureIgnoreCase) == -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace("UBound(aParameters)", "e.ParametersLength - 1", caseInsensitive: true);
				}
				if (text2.IndexOf("LBound(aParametersEx)", StringComparison.CurrentCultureIgnoreCase) != -1 && text2.IndexOf("_ActionMessage(", StringComparison.CurrentCultureIgnoreCase) == -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace("LBound(aParametersEx)", "0", caseInsensitive: true);
				}
				if (text2.IndexOf("LBound(aParameters)", StringComparison.CurrentCultureIgnoreCase) != -1 && text2.IndexOf("_ActionMessage(", StringComparison.CurrentCultureIgnoreCase) == -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace("LBound(aParameters)", "0", caseInsensitive: true);
				}
				if (text2.IndexOf("aParameters", StringComparison.CurrentCultureIgnoreCase) != -1 && text2.IndexOf("_ActionMessage(", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("UBound(aParametersEx)", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("LBound(aParametersEx)", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("UBound(aParameters)", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("LBound(aParameters)", StringComparison.CurrentCultureIgnoreCase) == -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace("aParameters", "e.Parameters", caseInsensitive: true);
				}
				else if (text2.IndexOf("aParametersEx", StringComparison.CurrentCultureIgnoreCase) != -1 && text2.IndexOf("_ActionMessage(", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("UBound(aParametersEx)", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("LBound(aParametersEx)", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("UBound(aParameters)", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("LBound(aParameters)", StringComparison.CurrentCultureIgnoreCase) == -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace("aParametersEx", "e.ParametersEx", caseInsensitive: true);
				}
				else if (text2.IndexOf("aParameters", StringComparison.CurrentCultureIgnoreCase) != -1 && text2.IndexOf("_ActionMessage(", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("UBound(aParametersEx)", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("LBound(aParametersEx)", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("UBound(aParameters)", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("LBound(aParameters)", StringComparison.CurrentCultureIgnoreCase) == -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace("aParameters", "e.Parameters", caseInsensitive: true);
				}
				else if (text2.IndexOf("cMessageID", StringComparison.CurrentCultureIgnoreCase) != -1 && text2.IndexOf("_ActionMessage(", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("UBound(aParametersEx)", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("LBound(aParametersEx)", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("UBound(aParameters)", StringComparison.CurrentCultureIgnoreCase) == -1 && text2.IndexOf("LBound(aParameters)", StringComparison.CurrentCultureIgnoreCase) == -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace("cMessageID", "e.MessageID", caseInsensitive: true);
				}
				if (text2.IndexOf(".RefreshTree", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\nthis.OnDataChanged(3)";
				}
				else if (text2.IndexOf(".Caption", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".Caption", ".Text", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SetFocusEx", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SetFocusEx", ".Focus", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SetFocus", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SetFocus", ".Focus", caseInsensitive: true);
				}
				else if (text2.IndexOf(".VisibleEx", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".VisibleEx", ".Visible", caseInsensitive: true);
				}
				else if (text2.IndexOf("Connection.Execute =", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace("Connection.Execute =", "Connection.Execute ", caseInsensitive: true);
				}
				else if (text2.IndexOf("Connection.Execute=", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace("Connection.Execute=", "Connection.Execute ", caseInsensitive: true);
				}
				else if (text2.IndexOf(".ListSource", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".ListSource", ".Search.RowSource", caseInsensitive: true);
				}
				else if (text2.IndexOf(".UpdateList", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".UpdateList", ".Search.UpdateList", caseInsensitive: true);
				}
				else if (text2.IndexOf(".ShowSearchButton", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".ShowSearchButton", ".Search.SearchButtonVisible", caseInsensitive: true);
				}
				else if (text2.IndexOf(".GridRowSource", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".GridRowSource", ".Search.RowSource", caseInsensitive: true);
				}
				else if (text2.IndexOf(".GridViewID", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".GridViewID", ".Search.GridID", caseInsensitive: true);
				}
				else if (text2.IndexOf(".DefaultField", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".DefaultField", ".Search.DefaultField", caseInsensitive: true);
				}
				else if (text2.IndexOf(".DefaultValue", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".DefaultValue", ".Search.DefaultValue", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SearchID", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SearchID", ".Search.SearchID", caseInsensitive: true);
				}
				else if (text2.IndexOf(".AdditionalFilter", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".AdditionalFilter", ".Search.AdditionalFilter", caseInsensitive: true);
				}
				else if (text2.IndexOf(".AdditionalFields", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".AdditionalFields", ".Search.AdditionalFields", caseInsensitive: true);
				}
				else if (text2.IndexOf(".AdditionalFilterOverride", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".AdditionalFilterOverride", ".Search.AdditionalFilterOverride", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SearchTitle", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SearchTitle", ".Search.SearchTitle", caseInsensitive: true);
				}
				else if (text2.IndexOf(".GridMultiSelect", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".GridMultiSelect", ".Search.GridMultiSelect", caseInsensitive: true);
				}
				else if (text2.IndexOf(".ShowCloseOnly", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".ShowCloseOnly", ".Search.ShowCloseOnly", caseInsensitive: true);
				}
				else if (text2.IndexOf(".DataSource", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".DataSource", ".DataSource.Recordset", caseInsensitive: true);
				}
				else if (text2.IndexOf(".ValueEx", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".ValueEx", ".Value", caseInsensitive: true);
				}
				else if (text2.IndexOf("\"CalcUnitPrice\"", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace("\"CalcUnitPrice\"", "\"qmqCalculatedUnitPrice\"", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SelColor", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SelColor", ".SelectionColorOle", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SelStrikeThru", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SelStrikeThru", ".SelectionStrikeout", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SelBold", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SelBold", ".SelectionBold", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SelAlignment", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SelAlignment", ".SelectionAlignment", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SelBullet", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SelBullet", ".SelectionBullet", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SelFontName", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SelFontName", ".SelectionFontName", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SelFontSize", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SelFontSize", ".SelectionFontSize", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SelItalic", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SelItalic", ".SelectionItalic", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SelLength", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SelLength", ".SelectionLength", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SelRTF", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SelRTF", ".SelectionRTF", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SelStart", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SelStart", ".SelectionStart", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SelText", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SelText", ".SelectionText", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SelUnderline", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SelUnderline", ".SelectionUnderline", caseInsensitive: true);
				}
				else if (text2.IndexOf(".SelText", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".SelText", ".SelectionText", caseInsensitive: true);
				}
				else if (text2.IndexOf(".NoneText", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".NoneText", ".NoneOptionText", caseInsensitive: true);
				}
				else if (text2.IndexOf(".ShowNoneOption", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".ShowNoneOption", ".NoneOptionVisible", caseInsensitive: true);
				}
				else if (text2.IndexOf(".AllowSetDefault", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".AllowSetDefault", ".EnableSetDefault", caseInsensitive: true);
				}
				else if (text2.IndexOf(".Picture", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".Picture", ".Image", caseInsensitive: true);
				}
				else if (text2.IndexOf(".Table", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".Table", ".DataSourceTable", caseInsensitive: true);
				}
				else if (text2.IndexOf(".ViewID", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".ViewID", ".GridDefinitionID", caseInsensitive: true);
				}
				else if (text2.IndexOf(".GetNextID", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".GetNextID", ".GenerateNextID", caseInsensitive: true);
				}
				else if (text2.IndexOf(".IsRecordChanged", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".IsRecordChanged", ".Modified", caseInsensitive: true);
				}
				else if (text2.IndexOf(".DoClick", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2) + "\r\n" + text2.Replace(".DoClick", ".PerformClick", caseInsensitive: true);
				}
				else if (text2.IndexOf(".EnableAutoRefresh", StringComparison.CurrentCultureIgnoreCase) != -1 || text2.IndexOf(".GridRef", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = commentLine(text2);
				}
				if (text2.IndexOf(".Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					if (controls != null)
					{
						foreach (KeyValuePair<string, ConvertRowInfo> control in controls)
						{
							int startIndex = 0;
							string value = "Controls(\"" + control.Key + "\").Value";
							startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
							if (control.Value.NewClassID.EndsWith("M1RichTextEditor", StringComparison.CurrentCultureIgnoreCase))
							{
								while (startIndex != -1)
								{
									if (text2.Length <= startIndex + value.Length || !char.IsLetter(text2[startIndex + value.Length]))
									{
										text2 = text2.Substring(0, startIndex + value.Length) + "Rtf" + text2.Substring(startIndex + value.Length);
									}
									startIndex = text2.IndexOf(value, startIndex + value.Length, StringComparison.CurrentCultureIgnoreCase);
								}
							}
							else if (control.Value.NewClassID.EndsWith("M1OptionButton", StringComparison.CurrentCultureIgnoreCase) && startIndex != -1)
							{
								text2 = commentLine(text2) + "\r\n" + text2.Replace(".Value", ".ValueString", caseInsensitive: true);
							}
						}
					}
				}
				else if (text2.IndexOf(".Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					if (controls != null)
					{
						foreach (KeyValuePair<string, ConvertRowInfo> control2 in controls)
						{
							if (control2.Value.NewClassID.EndsWith("M1MaskedTextEditor", StringComparison.CurrentCultureIgnoreCase) || control2.Value.NewClassID.EndsWith("M1NumericEditor", StringComparison.CurrentCultureIgnoreCase) || control2.Value.NewClassID.EndsWith("TEXTBOX", StringComparison.CurrentCultureIgnoreCase))
							{
								int startIndex = 0;
								string value = "Controls(\"" + control2.Key + "\").Text";
								startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
								if (startIndex != -1)
								{
									text2 = commentLine(text2) + "\r\n" + text2.Replace(".Text", ".Value", caseInsensitive: true);
								}
							}
						}
					}
				}
				else if (text2.IndexOf(".ReadOnly", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					if (controls != null)
					{
						foreach (KeyValuePair<string, ConvertRowInfo> control3 in controls)
						{
							if (control3.Value.NewClassID.EndsWith("M1CheckBox", StringComparison.CurrentCultureIgnoreCase))
							{
								int startIndex = 0;
								string value = "Controls(\"" + control3.Key + "\").ReadOnly";
								startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
								if (startIndex != -1)
								{
									text2 = commentLine(text2) + "\r\n" + text2.Replace(".ReadOnly", ".Enabled", caseInsensitive: true);
								}
							}
						}
					}
				}
				else if (text2.IndexOf(".Caption", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					if (controls != null)
					{
						foreach (KeyValuePair<string, ConvertRowInfo> control4 in controls)
						{
							if (control4.Value.NewClassID.EndsWith("M1Button", StringComparison.CurrentCultureIgnoreCase) || control4.Value.NewClassID.EndsWith("M1OptionButton", StringComparison.CurrentCultureIgnoreCase) || control4.Value.NewClassID.EndsWith("M1CheckBox", StringComparison.CurrentCultureIgnoreCase) || control4.Value.NewClassID.EndsWith("M1GroupLine", StringComparison.CurrentCultureIgnoreCase))
							{
								int startIndex = 0;
								string value = "Controls(\"" + control4.Key + "\").Caption";
								startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
								if (startIndex != -1)
								{
									text2 = commentLine(text2) + "\r\n" + text2.Replace(".Caption", ".Text", caseInsensitive: true);
								}
							}
						}
					}
				}
				else if (text2.IndexOf(".Style", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					if (controls != null)
					{
						foreach (KeyValuePair<string, ConvertRowInfo> control5 in controls)
						{
							if (control5.Value.NewClassID.EndsWith("M1ComboBox", StringComparison.CurrentCultureIgnoreCase))
							{
								int startIndex = 0;
								string value = "Controls(\"" + control5.Key + "\").Style";
								startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
								if (startIndex != -1)
								{
									text2 = commentLine(text2) + "\r\n" + text2.Replace(".Style", ".DropDownStyle", caseInsensitive: true);
								}
							}
						}
					}
				}
				else if (text2.IndexOf(".Delete", StringComparison.CurrentCultureIgnoreCase) != -1 && controls != null)
				{
					foreach (KeyValuePair<string, ConvertRowInfo> control6 in controls)
					{
						if (control6.Value.NewClassID.EndsWith("BindingSource", StringComparison.CurrentCultureIgnoreCase))
						{
							int startIndex = 0;
							string value = "Controls(\"" + control6.Key + "\").Delete";
							startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
							if (startIndex != -1)
							{
								text2 = commentLine(text2) + "\r\n" + text2.Replace(".Delete", ".RemoveCurrent", caseInsensitive: true);
							}
						}
					}
				}
				if (text2.IndexOf("Controls(\"txtLocationName\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationName\").Value", "Controls(\"uxAddressLocationName\").OrganizationName", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationAddressLine1\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationAddressLine1\").Value", "Controls(\"uxAddressLocationName\").AddressLine1", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationAddressLine2\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationAddressLine2\").Value", "Controls(\"uxAddressLocationName\").AddressLine2", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationAddressLine3\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationAddressLine3\").Value", "Controls(\"uxAddressLocationName\").AddressLine3", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationCity\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationCity\").Value", "Controls(\"uxAddressLocationName\").City", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationState\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationState\").Value", "Controls(\"uxAddressLocationName\").State", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationPostCode\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationPostCode\").Value", "Controls(\"uxAddressLocationName\").PostCode", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationPhone\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationPhone\").Value", "Controls(\"uxAddressLocationName\").PhoneNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationFax\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationFax\").Value", "Controls(\"uxAddressLocationName\").FaxNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationName\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationName\").Value", "Controls(\"uxAddressDropShipLocationName\").OrganizationName", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationAddressLine1\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationAddressLine1\").Value", "Controls(\"uxAddressDropShipLocationName\").AddressLine1", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationAddressLine2\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationAddressLine2\").Value", "Controls(\"uxAddressDropShipLocationName\").AddressLine2", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationAddressLine3\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationAddressLine3\").Value", "Controls(\"uxAddressDropShipLocationName\").AddressLine3", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationCity\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationCity\").Value", "Controls(\"uxAddressDropShipLocationName\").City", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationState\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationState\").Value", "Controls(\"uxAddressDropShipLocationName\").State", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationPostCode\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationPostCode\").Value", "Controls(\"uxAddressDropShipLocationName\").PostCode", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationPhone\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationPhone\").Value", "Controls(\"uxAddressDropShipLocationName\").PhoneNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationFax\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationFax\").Value", "Controls(\"uxAddressDropShipLocationName\").FaxNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgName\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgName\").Value", "Controls(\"uxAddressOrgName\").OrganizationName", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgAddressLine1\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgAddressLine1\").Value", "Controls(\"uxAddressOrgName\").AddressLine1", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgAddressLine2\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgAddressLine2\").Value", "Controls(\"uxAddressOrgName\").AddressLine2", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgAddressLine3\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgAddressLine3\").Value", "Controls(\"uxAddressOrgName\").AddressLine3", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgCity\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgCity\").Value", "Controls(\"uxAddressOrgName\").City", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgState\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgState\").Value", "Controls(\"uxAddressOrgName\").State", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgPostCode\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgPostCode\").Value", "Controls(\"uxAddressOrgName\").PostCode", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgPhone\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgPhone\").Value", "Controls(\"uxAddressOrgName\").PhoneNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgFax\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgFax\").Value", "Controls(\"uxAddressOrgName\").FaxNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"Controls(\"txtCustName\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustName\").Value", "Controls(\"uxAddressCustName\").OrganizationName", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustAddressLine1\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustAddressLine1\").Value", "Controls(\"uxAddressCustName\").AddressLine1", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustAddressLine2\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustAddressLine2\").Value", "Controls(\"uxAddressCustName\").AddressLine2", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustAddressLine3\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustAddressLine3\").Value", "Controls(\"uxAddressCustName\").AddressLine3", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustCity\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustCity\").Value", "Controls(\"uxAddressCustName\").City", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustState\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustState\").Value", "Controls(\"uxAddressCustName\").State", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustZip\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustZip\").Value", "Controls(\"uxAddressCustName\").PostCode", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustPhone\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustPhone\").Value", "Controls(\"uxAddressCustName\").PhoneNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustFax\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustFax\").Value", "Controls(\"uxAddressCustName\").FaxNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteName\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteName\").Value", "Controls(\"uxAddressQuoteName\").OrganizationName", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteAddressLine1\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteAddressLine1\").Value", "Controls(\"uxAddressQuoteName\").AddressLine1", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteAddressLine2\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteAddressLine2\").Value", "Controls(\"uxAddressQuoteName\").AddressLine2", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteAddressLine3\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteAddressLine3\").Value", "Controls(\"uxAddressQuoteName\").AddressLine3", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteCity\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteCity\").Value", "Controls(\"uxAddressQuoteName\").City", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteState\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteState\").Value", "Controls(\"uxAddressQuoteName\").State", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteZip\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteZip\").Value", "Controls(\"uxAddressQuoteName\").PostCode", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuotePhone\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuotePhone\").Value", "Controls(\"uxAddressQuoteName\").PhoneNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteFax\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteFax\").Value", "Controls(\"uxAddressQuoteName\").FaxNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipName\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipName\").Value", "Controls(\"uxAddressShipName\").OrganizationName", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipAddressLine1\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipAddressLine1\").Value", "Controls(\"uxAddressShipName\").AddressLine1", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipAddressLine2\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipAddressLine2\").Value", "Controls(\"uxAddressShipName\").AddressLine2", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipAddressLine3\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipAddressLine3\").Value", "Controls(\"uxAddressShipName\").AddressLine3", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipCity\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipCity\").Value", "Controls(\"uxAddressShipName\").City", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipState\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipState\").Value", "Controls(\"uxAddressShipName\").State", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipZip\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipZip\").Value", "Controls(\"uxAddressShipName\").PostCode", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipPhone\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipPhone\").Value", "Controls(\"uxAddressShipName\").PhoneNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipFax\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipFax\").Value", "Controls(\"uxAddressShipName\").FaxNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierName\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierName\").Value", "Controls(\"uxAddressSupplierName\").OrganizationName", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierAddressLine1\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierAddressLine1\").Value", "Controls(\"uxAddressSupplierName\").AddressLine1", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierAddressLine2\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierAddressLine2\").Value", "Controls(\"uxAddressSupplierName\").AddressLine2", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierAddressLine3\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierAddressLine3\").Value", "Controls(\"uxAddressSupplierName\").AddressLine3", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierCity\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierCity\").Value", "Controls(\"uxAddressSupplierName\").City", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierState\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierState\").Value", "Controls(\"uxAddressSupplierName\").State", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierZip\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierZip\").Value", "Controls(\"uxAddressSupplierName\").PostCode", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierPhone\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierPhone\").Value", "Controls(\"uxAddressSupplierName\").PhoneNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierFax\").Value", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierFax\").Value", "Controls(\"uxAddressSupplierName\").FaxNumber", caseInsensitive: true);
				}
				if (text2.IndexOf("Controls(\"txtLocationName\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationName\").Text", "Controls(\"uxAddressLocationName\").OrganizationName", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationAddressLine1\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationAddressLine1\").Text", "Controls(\"uxAddressLocationName\").AddressLine1", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationAddressLine2\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationAddressLine2\").Text", "Controls(\"uxAddressLocationName\").AddressLine2", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationAddressLine3\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationAddressLine3\").Text", "Controls(\"uxAddressLocationName\").AddressLine3", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationCity\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationCity\").Text", "Controls(\"uxAddressLocationName\").City", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationState\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationState\").Text", "Controls(\"uxAddressLocationName\").State", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationPostCode\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationPostCode\").Text", "Controls(\"uxAddressLocationName\").PostCode", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationPhone\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationPhone\").Text", "Controls(\"uxAddressLocationName\").PhoneNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtLocationFax\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtLocationFax\").Text", "Controls(\"uxAddressLocationName\").FaxNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationName\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationName\").Text", "Controls(\"uxAddressDropShipLocationName\").OrganizationName", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationAddressLine1\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationAddressLine1\").Text", "Controls(\"uxAddressDropShipLocationName\").AddressLine1", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationAddressLine2\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationAddressLine2\").Text", "Controls(\"uxAddressDropShipLocationName\").AddressLine2", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationAddressLine3\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationAddressLine3\").Text", "Controls(\"uxAddressDropShipLocationName\").AddressLine3", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationCity\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationCity\").Text", "Controls(\"uxAddressDropShipLocationName\").City", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationState\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationState\").Text", "Controls(\"uxAddressDropShipLocationName\").State", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationPostCode\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationPostCode\").Text", "Controls(\"uxAddressDropShipLocationName\").PostCode", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationPhone\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationPhone\").Text", "Controls(\"uxAddressDropShipLocationName\").PhoneNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtDropShipLocationFax\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtDropShipLocationFax\").Text", "Controls(\"uxAddressDropShipLocationName\").FaxNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgName\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgName\").Text", "Controls(\"uxAddressOrgName\").OrganizationName", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgAddressLine1\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgAddressLine1\").Text", "Controls(\"uxAddressOrgName\").AddressLine1", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgAddressLine2\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgAddressLine2\").Text", "Controls(\"uxAddressOrgName\").AddressLine2", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgAddressLine3\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgAddressLine3\").Text", "Controls(\"uxAddressOrgName\").AddressLine3", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgCity\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgCity\").Text", "Controls(\"uxAddressOrgName\").City", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgState\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgState\").Text", "Controls(\"uxAddressOrgName\").State", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgPostCode\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgPostCode\").Text", "Controls(\"uxAddressOrgName\").PostCode", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgPhone\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgPhone\").Text", "Controls(\"uxAddressOrgName\").PhoneNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtOrgFax\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtOrgFax\").Text", "Controls(\"uxAddressOrgName\").FaxNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"Controls(\"txtCustName\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustName\").Text", "Controls(\"uxAddressCustName\").OrganizationName", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustAddressLine1\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustAddressLine1\").Text", "Controls(\"uxAddressCustName\").AddressLine1", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustAddressLine2\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustAddressLine2\").Text", "Controls(\"uxAddressCustName\").AddressLine2", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustAddressLine3\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustAddressLine3\").Text", "Controls(\"uxAddressCustName\").AddressLine3", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustCity\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustCity\").Text", "Controls(\"uxAddressCustName\").City", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustState\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustState\").Text", "Controls(\"uxAddressCustName\").State", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustZip\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustZip\").Text", "Controls(\"uxAddressCustName\").PostCode", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustPhone\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustPhone\").Text", "Controls(\"uxAddressCustName\").PhoneNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtCustFax\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtCustFax\").Text", "Controls(\"uxAddressCustName\").FaxNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteName\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteName\").Text", "Controls(\"uxAddressQuoteName\").OrganizationName", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteAddressLine1\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteAddressLine1\").Text", "Controls(\"uxAddressQuoteName\").AddressLine1", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteAddressLine2\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteAddressLine2\").Text", "Controls(\"uxAddressQuoteName\").AddressLine2", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteAddressLine3\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteAddressLine3\").Text", "Controls(\"uxAddressQuoteName\").AddressLine3", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteCity\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteCity\").Text", "Controls(\"uxAddressQuoteName\").City", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteState\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteState\").Text", "Controls(\"uxAddressQuoteName\").State", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteZip\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteZip\").Text", "Controls(\"uxAddressQuoteName\").PostCode", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuotePhone\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuotePhone\").Text", "Controls(\"uxAddressQuoteName\").PhoneNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtQuoteFax\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtQuoteFax\").Text", "Controls(\"uxAddressQuoteName\").FaxNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipName\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipName\").Text", "Controls(\"uxAddressShipName\").OrganizationName", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipAddressLine1\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipAddressLine1\").Text", "Controls(\"uxAddressShipName\").AddressLine1", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipAddressLine2\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipAddressLine2\").Text", "Controls(\"uxAddressShipName\").AddressLine2", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipAddressLine3\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipAddressLine3\").Text", "Controls(\"uxAddressShipName\").AddressLine3", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipCity\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipCity\").Text", "Controls(\"uxAddressShipName\").City", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipState\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipState\").Text", "Controls(\"uxAddressShipName\").State", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipZip\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipZip\").Text", "Controls(\"uxAddressShipName\").PostCode", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipPhone\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipPhone\").Text", "Controls(\"uxAddressShipName\").PhoneNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtShipFax\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtShipFax\").Text", "Controls(\"uxAddressShipName\").FaxNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierName\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierName\").Text", "Controls(\"uxAddressSupplierName\").OrganizationName", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierAddressLine1\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierAddressLine1\").Text", "Controls(\"uxAddressSupplierName\").AddressLine1", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierAddressLine2\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierAddressLine2\").Text", "Controls(\"uxAddressSupplierName\").AddressLine2", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierAddressLine3\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierAddressLine3\").Text", "Controls(\"uxAddressSupplierName\").AddressLine3", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierCity\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierCity\").Text", "Controls(\"uxAddressSupplierName\").City", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierState\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierState\").Text", "Controls(\"uxAddressSupplierName\").State", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierZip\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierZip\").Text", "Controls(\"uxAddressSupplierName\").PostCode", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierPhone\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierPhone\").Text", "Controls(\"uxAddressSupplierName\").PhoneNumber", caseInsensitive: true);
				}
				else if (text2.IndexOf("Controls(\"txtSupplierFax\").Text", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					string value = "Controls(\"";
					text2 = commentLine(text2) + "\r\n" + text2.Replace(value + "txtSupplierFax\").Text", "Controls(\"uxAddressSupplierName\").FaxNumber", caseInsensitive: true);
				}
				if (text2.IndexOf(".Duplex", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					text2 = ((text2.IndexOf("True", StringComparison.CurrentCultureIgnoreCase) == -1) ? (commentLine(text2) + "\r\n" + text2.Replace("False", "1", caseInsensitive: true)) : (commentLine(text2) + "\r\n" + text2.Replace("True", "2", caseInsensitive: true)));
				}
				stringBuilder.AppendLine(text2);
				if (text4.Replace(" ", string.Empty).StartsWith("EndFunction", StringComparison.CurrentCultureIgnoreCase) || text4.Replace(" ", string.Empty).StartsWith("EndSub", StringComparison.CurrentCultureIgnoreCase))
				{
					flag = false;
				}
			}
			else if (text4.StartsWith("Rem", StringComparison.CurrentCultureIgnoreCase) || text4.StartsWith("'"))
			{
				stringBuilder.AppendLine(text3);
			}
			else if (text4.StartsWith("Dim ", StringComparison.CurrentCultureIgnoreCase))
			{
				stringBuilder2.AppendLine(text3);
			}
			else if (text4.StartsWith("Const ", StringComparison.CurrentCultureIgnoreCase))
			{
				string text6 = text3.Replace("Const ", "Dim ", caseInsensitive: true);
				string empty = string.Empty;
				string value2 = text3;
				int startIndex = text6.IndexOf("=");
				if (startIndex != -1)
				{
					value2 = text6.Substring(0, startIndex - 1);
					empty = text6.Substring(4);
					stringBuilder3.AppendLine(empty);
				}
				stringBuilder2.AppendLine(value2);
			}
			else if (!string.IsNullOrWhiteSpace(text3))
			{
				stringBuilder3.AppendLine(text3);
			}
		}
		if (stringBuilder2.Length != 0)
		{
			stringBuilder.Insert(0, stringBuilder2.ToString());
		}
		if (stringBuilder3.Length != 0)
		{
			code = stringBuilder.ToString();
			stringBuilder.Length = 0;
			bool flag2 = false;
			array = code.Replace("\n", string.Empty).Split('\r');
			foreach (string text7 in array)
			{
				stringBuilder.AppendLine(text7);
				if (!flag2 && (text7.Replace(" ", string.Empty).Replace("\t", string.Empty).StartsWith("Functionthis_initialize(", StringComparison.CurrentCultureIgnoreCase) || text7.Replace(" ", string.Empty).Replace("\t", string.Empty).StartsWith("Subthis_initialize(", StringComparison.CurrentCultureIgnoreCase)))
				{
					flag2 = true;
					stringBuilder.AppendLine("' The following lines were moved here from outside function calls during the conversion.");
					stringBuilder.Append(stringBuilder3);
					stringBuilder.AppendLine("' End moved code");
				}
			}
			if (!flag2)
			{
				stringBuilder.AppendLine("Function this_Initialize(sender, e)");
				stringBuilder.AppendLine("' The following lines were moved here from outside function calls during the conversion.");
				stringBuilder.Append(stringBuilder3);
				stringBuilder.AppendLine("' End moved code");
				stringBuilder.AppendLine("End Function");
			}
		}
		if (controls != null)
		{
			code = stringBuilder.ToString();
			stringBuilder.Length = 0;
			array = code.Replace("\n", string.Empty).Split('\r');
			foreach (string obj in array)
			{
				bool flag3 = false;
				string text2 = obj;
				if (!text2.StartsWith("Rem", StringComparison.CurrentCultureIgnoreCase) && !text2.StartsWith("'") && text2.IndexOf("Controls(\"", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					int startIndex = 0;
					string value = "Controls(\"";
					startIndex = text2.IndexOf(value, startIndex, StringComparison.CurrentCultureIgnoreCase);
					value = "\").";
					string text8 = string.Empty;
					if (text2.IndexOf(value, startIndex + 10, StringComparison.CurrentCultureIgnoreCase) != -1)
					{
						text8 = text2.Substring(startIndex + 10, text2.IndexOf(value, startIndex + 10, StringComparison.CurrentCultureIgnoreCase) - startIndex - 10);
					}
					else
					{
						flag3 = true;
					}
					if (!string.IsNullOrWhiteSpace(text8))
					{
						foreach (KeyValuePair<string, ConvertRowInfo> control7 in controls)
						{
							if (text8.Equals(control7.Value.Name, StringComparison.CurrentCultureIgnoreCase))
							{
								flag3 = true;
								break;
							}
						}
					}
					if (!flag3)
					{
						text2 = "' Control not found\r\n'" + text2;
					}
				}
				stringBuilder.AppendLine(text2);
			}
		}
		return stringBuilder.ToString();
	}

	private void convertEvents(IDictionary<string, string> eventsToConvert, ref string transformedLine)
	{
		foreach (KeyValuePair<string, string> item in eventsToConvert)
		{
			int num = transformedLine.IndexOf(item.Key, StringComparison.CurrentCultureIgnoreCase);
			if (num == -1)
			{
				continue;
			}
			string text = commentLine(transformedLine) + "\r\n";
			transformedLine = transformedLine.Replace(item.Key, item.Value, caseInsensitive: true);
			if (transformedLine.IndexOf("Function", StringComparison.CurrentCultureIgnoreCase) == -1 && transformedLine.IndexOf("'") == -1)
			{
				int num2 = transformedLine.LastIndexOf("\t", num, StringComparison.CurrentCultureIgnoreCase);
				if (num2 != -1)
				{
					transformedLine = transformedLine.Insert(num2 + 1, "Call ");
				}
				else
				{
					transformedLine = $"Call {transformedLine}";
				}
			}
			transformedLine = text + transformedLine;
			break;
		}
	}

	public void ConvertCustomControls(string databaseName, string oldFormName, string newFormName, string bindingSourceName, int newLeft, int newTop, bool convertDataBindings, SqlTransaction transaction, bool moveCustomControlsToBottom, bool convertAndCopyCustomFormCode)
	{
		oldFormName = oldFormName.Trim().ToUpper();
		if (oldFormName.Length == 0)
		{
			return;
		}
		if (oldFormName.Equals("ViewQuoteLine", StringComparison.CurrentCultureIgnoreCase) || oldFormName.Equals("ViewProject", StringComparison.CurrentCultureIgnoreCase) || oldFormName.Equals("ViewProjectArea", StringComparison.CurrentCultureIgnoreCase))
		{
			ExecuteCommand(databaseName, $"Delete From DDFormDetails Where deFormID = {oldFormName.ToSql()} And deCustom = 0", transaction);
		}
		if (!isDDFormCodeType.HasValue)
		{
			isDDFormCodeType = DoesTableExist(null, databaseName, "DDFormCodeTemp", transaction);
		}
		if (isDDFormCodeType.Value)
		{
			ExecuteCommand(databaseName, "Update DDFormCodeTemp Set dmFormID = " + newFormName.ToSql() + " Where dmFormID = " + oldFormName.ToSql(), transaction);
		}
		Dictionary<string, ConvertRowInfo> dictionary = new Dictionary<string, ConvertRowInfo>(StringComparer.CurrentCultureIgnoreCase);
		SqlDataAdapter adapter = new SqlDataAdapter();
		List<ConvertRowInfo> list = new List<ConvertRowInfo>();
		List<ConvertRowInfo> list2 = new List<ConvertRowInfo>();
		List<ConvertRowInfo> list3 = new List<ConvertRowInfo>();
		DataTable dataTable = GetDataTable(null, databaseName, $"Select * From DDFormDetails Where deFormID = {oldFormName.ToSql()} Order By IsNull(deSequenceUser, deSequence)", fillSchema: true, out adapter, transaction);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		Dictionary<string, ConvertRowInfo> dictionary2 = LoadVersion8Form(oldFormName);
		Dictionary<string, ConvertRowInfo> dictionary3 = LoadVersion9Form(newFormName);
		foreach (DataRow row3 in dataTable.Rows)
		{
			string text = row3.Field<string>("deControlName");
			if (dictionary2.ContainsKey(text))
			{
				ConvertRowInfo convertRowInfo = LoadRowInfo(row3, dictionary2[text], row3.Field<string>("dePropertiesUser"));
			}
			else if (!dictionary3.ContainsKey(text))
			{
				ConvertRowInfo convertRowInfo = LoadRowInfo(row3, new ConvertRowInfo(text), row3.Field<string>("dePropertiesUser"));
				convertRowInfo.IsCustom = true;
				dictionary2.Add(convertRowInfo.Name, convertRowInfo);
			}
		}
		foreach (KeyValuePair<string, ConvertRowInfo> item in dictionary2)
		{
			dictionary.Add(item.Key, item.Value);
			if (item.Value.Row.Table == dataTable)
			{
				continue;
			}
			DataRow dataRow = dataTable.NewRow().BlankRow();
			foreach (DataColumn column in item.Value.Row.Table.Columns)
			{
				if (dataRow.Table.Columns.Contains(column.ColumnName))
				{
					dataRow[column.ColumnName] = item.Value.Row[column.ColumnName];
				}
			}
			dataRow["deParentID"] = string.Empty;
			dataRow["deParentIDUser"] = DBNull.Value;
			dataRow["deNestedName"] = string.Empty;
			dataRow["deNestedNameUser"] = DBNull.Value;
			dataRow["deAppExtensionID"] = string.Empty;
			dataRow["deSequence"] = 0;
			dataRow["deProperties"] = DBNull.Value;
			dataRow["dePropertiesUser"] = DBNull.Value;
			dataRow["deSequenceUser"] = DBNull.Value;
			item.Value.CustomProperties.Clear();
			item.Value.Row = dataRow;
			dataTable.Rows.Add(dataRow);
		}
		foreach (KeyValuePair<string, ConvertRowInfo> item2 in dictionary)
		{
			if (item2.Value.NewClassID.EndsWith("M1Label", StringComparison.CurrentCultureIgnoreCase))
			{
				list.Add(item2.Value);
			}
		}
		int bottom;
		foreach (ConvertRowInfo label in list.Where((ConvertRowInfo l) => l.Top.HasValue && l.Left.HasValue))
		{
			bottom = label.Top.Value;
			if (!label.Height.HasValue || label.Height <= 0)
			{
				bottom += 16;
			}
			else
			{
				bottom += label.Height.Value;
			}
			ConvertRowInfo convertRowInfo2 = dictionary2.Values.FirstOrDefault((ConvertRowInfo v) => v != label && v.Top > bottom - 2 && v.Top < bottom + 10 && v.Left > label.Left - 10 && v.Left < label.Left + 10);
			if (convertRowInfo2 == null)
			{
				continue;
			}
			if (!convertRowInfo2.IsProperty("ShowLabel", "True"))
			{
				convertRowInfo2.Top -= 16;
				convertRowInfo2.Height += 16;
			}
			convertRowInfo2.SetCustomProperty("ShowLabel", "True");
			if (!convertRowInfo2.Height.HasValue)
			{
				convertRowInfo2.SetCustomProperty("AutoSize", "True");
			}
			if ((string.IsNullOrWhiteSpace(label.DataField) || !label.DataField.Equals(convertRowInfo2.DataField, StringComparison.CurrentCultureIgnoreCase)) && label.CustomProperties.ContainsKey("Text"))
			{
				bool flag = false;
				if (!string.IsNullOrWhiteSpace(convertRowInfo2.DataField))
				{
					string value = label.CustomProperties["Text"].Replace("\"", "");
					SqlDataAdapter adapter2 = new SqlDataAdapter();
					DataTable dataTable2 = GetDataTable(null, databaseName, "Select dfCaption From DDFields Where dfField = " + M1Util.ConvertToSql(convertRowInfo2.DataField), fillSchema: true, out adapter2, transaction);
					if (dataTable2.Rows.Count != 0 && dataTable2.Rows[0].Field<string>("dfCaption").Equals(value, StringComparison.CurrentCultureIgnoreCase))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					convertRowInfo2.SetCustomProperty("UnboundLabelText", label.CustomProperties["Text"]);
				}
			}
			if (dictionary.ContainsKey(label.Name))
			{
				dictionary.Remove(label.Name);
			}
			list3.Add(label);
			label.Row.Delete();
		}
		foreach (ConvertRowInfo item3 in list3)
		{
			list.Remove(item3);
		}
		list3.Clear();
		foreach (ConvertRowInfo label2 in list)
		{
			if (string.IsNullOrWhiteSpace(label2.DataField))
			{
				continue;
			}
			ConvertRowInfo convertRowInfo2 = dictionary2.Values.FirstOrDefault((ConvertRowInfo v) => v != label2 && v.DataField.Equals(label2.DataField, StringComparison.CurrentCultureIgnoreCase));
			if (convertRowInfo2 != null)
			{
				if (!convertRowInfo2.IsProperty("ShowLabel", "True"))
				{
					convertRowInfo2.Top -= 16;
					convertRowInfo2.Height += 16;
				}
				convertRowInfo2.SetCustomProperty("ShowLabel", "True");
				if (dictionary.ContainsKey(label2.Name))
				{
					dictionary.Remove(label2.Name);
				}
				list3.Add(label2);
				label2.Row.Delete();
			}
		}
		list3.Clear();
		foreach (KeyValuePair<string, ConvertRowInfo> item4 in dictionary)
		{
			if (item4.Value.NewClassID.EndsWith("M1CheckBox", StringComparison.CurrentCultureIgnoreCase) && dictionary3.ContainsKey(item4.Key) && dictionary3[item4.Key].IsProperty("ShowLabel", "True"))
			{
				item4.Value.Top -= 16;
				item4.Value.Height += 16;
			}
		}
		foreach (KeyValuePair<string, ConvertRowInfo> item5 in dictionary)
		{
			if (!item5.Value.IsCustom && !dictionary3.ContainsKey(item5.Key))
			{
				list3.Add(item5.Value);
			}
		}
		foreach (ConvertRowInfo item6 in list3)
		{
			item6.Row.Delete();
			dictionary.Remove(item6.Name);
		}
		foreach (KeyValuePair<string, ConvertRowInfo> item7 in dictionary)
		{
			if (item7.Value.NewClassID.EndsWith("M1GroupLine", StringComparison.CurrentCultureIgnoreCase))
			{
				list2.Add(item7.Value);
				item7.Value.SetCustomProperty("AutoSize", "True");
				item7.Value.SetCustomProperty("LayoutType", "1");
			}
		}
		if (list2.Count != 0)
		{
			List<ConvertRowInfo> list4 = list2.OrderBy((ConvertRowInfo group) => group.Top).ToList();
			Dictionary<ConvertRowInfo, List<ConvertRowInfo>> dictionary4 = new Dictionary<ConvertRowInfo, List<ConvertRowInfo>>();
			List<ConvertRowInfo> list5 = new List<ConvertRowInfo>();
			for (int num = 0; num < list4.Count; num++)
			{
				ConvertRowInfo convertRowInfo3 = list4[num];
				convertRowInfo3.Sequence = num;
				ConvertRowInfo convertRowInfo4 = ((num + 1 >= list4.Count) ? null : list4[num + 1]);
				list5.Clear();
				foreach (ConvertRowInfo value2 in dictionary.Values)
				{
					if (value2.Top >= convertRowInfo3.Top && (convertRowInfo4 == null || value2.Top < convertRowInfo4.Top) && value2 != convertRowInfo3)
					{
						list5.Add(value2);
					}
				}
				int? num2 = null;
				foreach (ConvertRowInfo item8 in list5.OrderBy((ConvertRowInfo i) => i.Top))
				{
					if (!num2.HasValue)
					{
						num2 = item8.Top;
						item8.TopForOrdering = item8.Top;
					}
					else if (item8.Top > num2.Value + 5)
					{
						num2 = item8.Top;
						item8.TopForOrdering = item8.Top;
					}
					else
					{
						item8.TopForOrdering = num2.Value;
					}
				}
				List<ConvertRowInfo> list6 = (from item in list5
					orderby item.Left
					orderby item.TopForOrdering
					select item).ToList();
				ConvertRowInfo previousNodeInZOrder = null;
				int num3 = 0;
				foreach (ConvertRowInfo item9 in list6)
				{
					item9.Name.Contains("Upgrade");
					item9.Sequence = num3;
					num3++;
					item9.Group = convertRowInfo3;
					item9.PreviousNodeInZOrder = previousNodeInZOrder;
					previousNodeInZOrder = item9;
					if (item9.PreviousNodeInZOrder != null)
					{
						item9.PreviousNodeInZOrder.NextNodeInZOrder = item9;
					}
				}
				dictionary4.Add(convertRowInfo3, list6);
			}
		}
		IOrderedEnumerable<ConvertRowInfo> orderedEnumerable = from i in dictionary3.Values
			where i.Row.Field<string>("deParentID") != null
			orderby i.Row.Field<short>("deSequence")
			select i;
		new List<ConvertRowInfo>();
		ConvertRowInfo tempItem;
		foreach (ConvertRowInfo item10 in orderedEnumerable)
		{
			if (dictionary.ContainsKey(item10.Name))
			{
				if (!item10.Row.IsNull("deParentID"))
				{
					dictionary[item10.Name].Row.SetField("deParentID", item10.Row.Field<string>("deParentID"));
					dictionary[item10.Name].Row.SetField("deNestedName", item10.Row.Field<string>("deNestedName"));
				}
				dictionary[item10.Name].Row.SetField("deSequence", item10.Row.Field<short>("deSequence"));
				continue;
			}
			tempItem = LoadRowInfo(item10.Row, new ConvertRowInfo(item10.Name), item10.Row.Field<string>("deProperties"));
			tempItem.CustomProperties.Clear();
			tempItem.Row.SetField<string>("deProperties", null);
			tempItem.Row.SetField<string>("dePropertiesUser", null);
			tempItem.Row.SetField("deCustom", tempItem.IsCustom);
			DataRow dataRow = dataTable.NewRow().BlankRow();
			foreach (DataColumn column2 in tempItem.Row.Table.Columns)
			{
				if (tempItem.Row.IsNull(column2.ColumnName) && (column2.ColumnName.Equals("deParentID", StringComparison.CurrentCultureIgnoreCase) || column2.ColumnName.Equals("deNestedName", StringComparison.CurrentCultureIgnoreCase) || column2.ColumnName.Equals("deAppExtensionID", StringComparison.CurrentCultureIgnoreCase)))
				{
					dataRow[column2.ColumnName] = string.Empty;
				}
				else
				{
					dataRow[column2.ColumnName] = tempItem.Row[column2.ColumnName];
				}
			}
			tempItem.Row = dataRow;
			dataTable.Rows.Add(dataRow);
			ConvertRowInfo prevItem = (from i in orderedEnumerable
				where i.Row.Field<string>("deParentID").Equals(tempItem.Row.Field<string>("deParentID"), StringComparison.CurrentCultureIgnoreCase) && i.Row.Field<short>("deSequence") <= tempItem.Row.Field<short>("deSequence") && !i.Name.Equals(tempItem.Name, StringComparison.CurrentCultureIgnoreCase)
				orderby i.Row.Field<short>("deSequence")
				select i).LastOrDefault();
			if (prevItem != null)
			{
				if (dictionary.ContainsKey(prevItem.Name))
				{
					prevItem = dictionary[prevItem.Name];
				}
				else
				{
					prevItem = null;
				}
			}
			if (prevItem != null)
			{
				foreach (ConvertRowInfo item11 in (from i in dictionary.Values
					where i.Group == prevItem.Group && i.Sequence > prevItem.Sequence
					orderby i.Sequence
					select i).ToList())
				{
					item11.Sequence++;
				}
				tempItem.Sequence = prevItem.Sequence + 1;
				tempItem.Group = prevItem.Group;
			}
			else if (dictionary.ContainsKey(tempItem.Row.Field<string>("deParentID")))
			{
				tempItem.Group = dictionary[tempItem.Row.Field<string>("deParentID")];
			}
			dictionary.Add(tempItem.Name, tempItem);
		}
		if (moveCustomControlsToBottom)
		{
			List<KeyValuePair<string, ConvertRowInfo>> list7 = dictionary.Where((KeyValuePair<string, ConvertRowInfo> i) => i.Value.Group != null && !i.Value.Group.IsCustom && i.Value.IsCustom).ToList();
			if (list7.Count != 0)
			{
				int num4 = 1;
				string text2 = "grpCustomControls";
				string empty = string.Empty;
				while (dictionary.ContainsKey(text2 + num4))
				{
					num4++;
				}
				empty = text2 + num4;
				ConvertRowInfo newTempGroup = new ConvertRowInfo(empty);
				newTempGroup.NewClassID = "M1.Forms.Controls.M1GroupLine";
				newTempGroup.IsCustom = true;
				newTempGroup.SetCustomProperty("AutoSize", "True");
				newTempGroup.SetCustomProperty("Anchor", "13");
				newTempGroup.SetCustomProperty("LayoutType", "1");
				newTempGroup.SetCustomProperty("Text", "\"Custom Controls\"");
				newTempGroup.TopForOrdering = list2.Count + 1;
				newTempGroup.Sequence = list2.Count + 1;
				int seq = newTempGroup.Sequence;
				List<KeyValuePair<string, ConvertRowInfo>> list8 = dictionary.Where((KeyValuePair<string, ConvertRowInfo> i) => i.Value.NewClassID.Equals("M1.Forms.Controls.M1GroupLine", StringComparison.CurrentCultureIgnoreCase) && i.Value.IsCustom).ToList();
				if (list8.Count != 0)
				{
					list8.ForEach(delegate(KeyValuePair<string, ConvertRowInfo> i)
					{
						i.Value.Sequence = seq++;
					});
				}
				list7.ForEach(delegate(KeyValuePair<string, ConvertRowInfo> i)
				{
					i.Value.Group = newTempGroup;
				});
				dictionary.Add(empty, newTempGroup);
			}
		}
		Guid? guid = null;
		if (isDDFormCodeType.Value)
		{
			DataTable dataTable3 = GetDataTable(databaseName, "Select dkCodeID From DDFormCodeTemp Where dmFormID = " + M1Util.ConvertToSql(newFormName));
			if (dataTable3.Rows.Count != 0)
			{
				guid = dataTable3.Rows[0].Field<Guid>("dkCodeID");
			}
		}
		else
		{
			DataTable dataTable4 = GetDataTable(databaseName, "Select dkCodeID From DDCode Inner Join DDForms On dmUniqueID = dkSourceUniqueID And dkSourceTable = 'DDFORMS' Where dmFormID = " + M1Util.ConvertToSql(newFormName));
			if (dataTable4.Rows.Count != 0)
			{
				guid = dataTable4.Rows[0].Field<Guid>("dkCodeID");
			}
		}
		if (guid.HasValue)
		{
			SqlDataAdapter adapter3;
			DataTable dataTable5 = GetDataTable(null, databaseName, "Select * From DDCode Where dkCodeID = " + M1Util.ConvertToSql(guid.Value), fillSchema: true, out adapter3);
			if (dataTable5.Rows.Count != 0)
			{
				string text3 = dataTable5.Rows[0].Field<string>("dkCode");
				string text4 = string.Empty;
				if (convertAndCopyCustomFormCode)
				{
					text4 = convertCustomFormCode(text3, dictionary);
				}
				if (text3 != text4)
				{
					dataTable5.Rows[0].SetField("dkCode", text4);
					UpdateData(null, null, databaseName, dataTable5, adapter3, transaction);
				}
			}
		}
		int num5 = newTop;
		foreach (ConvertRowInfo item12 in dictionary2.Values.OrderBy((ConvertRowInfo x) => x.Top))
		{
			if (newLeft == -1 && newTop == -1)
			{
				if (item12.Left.HasValue && item12.Top.HasValue)
				{
					item12.Location = new Point(item12.Left.Value, item12.Top.Value);
				}
			}
			else
			{
				item12.Location = new Point(newLeft, num5);
			}
			if (item12.Height.HasValue)
			{
				num5 += item12.Height.Value + 3;
			}
		}
		foreach (ConvertRowInfo value3 in dictionary.Values)
		{
			if (value3.Row == null)
			{
				DataRow dataRow = dataTable.NewRow().BlankRow();
				dataRow["deParentID"] = string.Empty;
				dataRow["deParentIDUser"] = DBNull.Value;
				dataRow["deNestedName"] = string.Empty;
				dataRow["deNestedNameUser"] = DBNull.Value;
				dataRow["deAppExtensionID"] = string.Empty;
				dataRow["deSequence"] = 0;
				dataRow["deProperties"] = DBNull.Value;
				dataRow["dePropertiesUser"] = DBNull.Value;
				dataRow["deSequenceUser"] = DBNull.Value;
				value3.Row = dataRow;
				value3.Row.SetField("deFormID", newFormName);
				value3.Row.SetField("deControlName", value3.Name);
				dataTable.Rows.Add(dataRow);
			}
			ConvertItemProperties(value3, convertDataBindings && value3.IsCustom, bindingSourceName);
			value3.Row.SetField("dePropertiesUser", value3.GetCustomProperties());
			value3.Row.SetField("deFormID", newFormName);
			value3.Row.SetField("deClassID", value3.NewClassID);
			value3.Row.SetField("deSequenceUser", value3.Sequence);
			value3.Row.SetField("deCustom", value3.IsCustom);
			if (value3.Group != null && !value3.Row.Field<string>("deParentID").Equals(value3.Group.Name, StringComparison.CurrentCultureIgnoreCase))
			{
				value3.Row.SetField("deParentIDUser", value3.Group.Name);
				value3.Row.SetField("deNestedNameUser", "WorkingArea");
			}
		}
		dictionary.Clear();
		currentContext.DDServerManager.UpdateData(null, null, databaseName, dataTable, adapter, transaction);
		if (newFormName.Equals("M1.Ax.Erp.Forms.Sales.Quote.QuoteLineView", StringComparison.CurrentCultureIgnoreCase) || newFormName.Equals("M1.Ax.Erp.Forms.Production.Project.ProjectView", StringComparison.CurrentCultureIgnoreCase) || newFormName.Equals("M1.Ax.Erp.Forms.Production.Project.ProjectAreaView", StringComparison.CurrentCultureIgnoreCase))
		{
			ExecuteCommand(databaseName, $"Delete From DDFormDetails Where deFormID = {newFormName.ToSql()} And deCustom = 0", transaction);
		}
		dataTable = GetDataTable(null, databaseName, $"Select * From DDFormDetails Where deFormID = {newFormName.ToSql()} And deControlName = ''", fillSchema: true, out adapter, transaction);
		if (dataTable.Rows.Count == 0)
		{
			dataTable.AddBlankRow(allowNullForDefaultValue: true);
			dataTable.Rows[0].SetField("deFormID", newFormName);
		}
		if (newLeft != -1 && newTop != -1)
		{
			DataRow row2 = dataTable.Rows[0];
			row2.SetField("dePropertiesUser", setProp(row2.Field<string>("dePropertiesUser"), "Height", num5.ToString()));
		}
		currentContext.DDServerManager.UpdateData(null, null, databaseName, dataTable, adapter, transaction);
	}

	public void ConvertCustomControlsAbsolute(string databaseName, string oldFormName, string newFormName, string bindingSourceName, int newLeft, int newTop, bool convertDataBindings, SqlTransaction transaction)
	{
		oldFormName = oldFormName.Trim().ToUpper();
		if (oldFormName.Length == 0)
		{
			return;
		}
		if (!isDDFormCodeType.HasValue)
		{
			isDDFormCodeType = DoesTableExist(null, databaseName, "DDFormCodeTemp", transaction);
		}
		if (isDDFormCodeType.Value)
		{
			ExecuteCommand(databaseName, "Update DDFormCodeTemp Set dmFormID = " + newFormName.ToSql() + " Where dmFormID = " + oldFormName.ToSql(), transaction);
		}
		Dictionary<string, ConvertRowInfo> dictionary = new Dictionary<string, ConvertRowInfo>(StringComparer.CurrentCultureIgnoreCase);
		SqlDataAdapter adapter = new SqlDataAdapter();
		List<ConvertRowInfo> list = new List<ConvertRowInfo>();
		List<ConvertRowInfo> list2 = new List<ConvertRowInfo>();
		List<ConvertRowInfo> list3 = new List<ConvertRowInfo>();
		DataTable dataTable = GetDataTable(null, databaseName, $"Select * From DDFormDetails Where deFormID = {oldFormName.ToSql()}", fillSchema: true, out adapter, transaction);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		Dictionary<string, ConvertRowInfo> dictionary2 = LoadVersion8Form(oldFormName);
		Dictionary<string, ConvertRowInfo> dictionary3 = LoadVersion9Form(newFormName);
		foreach (DataRow row3 in dataTable.Rows)
		{
			string text = row3.Field<string>("deControlName");
			if (dictionary2.ContainsKey(text))
			{
				ConvertRowInfo convertRowInfo = LoadRowInfo(row3, dictionary2[text], row3.Field<string>("dePropertiesUser"));
			}
			else if (!dictionary3.ContainsKey(text))
			{
				ConvertRowInfo convertRowInfo = LoadRowInfo(row3, new ConvertRowInfo(text), row3.Field<string>("dePropertiesUser"));
				convertRowInfo.IsCustom = true;
				dictionary2.Add(convertRowInfo.Name, convertRowInfo);
			}
		}
		foreach (KeyValuePair<string, ConvertRowInfo> item in dictionary2)
		{
			dictionary.Add(item.Key, item.Value);
			if (item.Value.Row.Table == dataTable)
			{
				continue;
			}
			DataRow dataRow = dataTable.NewRow().BlankRow();
			foreach (DataColumn column in item.Value.Row.Table.Columns)
			{
				if (dataRow.Table.Columns.Contains(column.ColumnName))
				{
					dataRow[column.ColumnName] = item.Value.Row[column.ColumnName];
				}
			}
			dataRow["deParentID"] = string.Empty;
			dataRow["deParentIDUser"] = DBNull.Value;
			dataRow["deNestedName"] = string.Empty;
			dataRow["deNestedNameUser"] = DBNull.Value;
			dataRow["deAppExtensionID"] = string.Empty;
			dataRow["deSequence"] = 0;
			dataRow["deProperties"] = DBNull.Value;
			dataRow["dePropertiesUser"] = DBNull.Value;
			dataRow["deSequenceUser"] = DBNull.Value;
			item.Value.CustomProperties.Clear();
			item.Value.Row = dataRow;
			dataTable.Rows.Add(dataRow);
		}
		foreach (KeyValuePair<string, ConvertRowInfo> item2 in dictionary)
		{
			if (!string.IsNullOrWhiteSpace(item2.Key))
			{
				item2.Value.SetCustomProperty("Anchor", "5");
				item2.Value.SetCustomProperty("AutoSize", "False");
				if (item2.Value.Width.HasValue && item2.Value.Height.HasValue)
				{
					item2.Value.SetCustomProperty("Size", "\"" + item2.Value.Width.Value + ", " + item2.Value.Height.Value + "\"");
				}
				if (item2.Value.Left.HasValue && item2.Value.Top.HasValue)
				{
					item2.Value.SetCustomProperty("Location", "\"" + item2.Value.Left.Value + ", " + item2.Value.Top.Value + "\"");
				}
			}
		}
		foreach (KeyValuePair<string, ConvertRowInfo> item3 in dictionary)
		{
			if (item3.Value.NewClassID.EndsWith("M1Label", StringComparison.CurrentCultureIgnoreCase))
			{
				list.Add(item3.Value);
			}
		}
		int bottom;
		foreach (ConvertRowInfo label in list.Where((ConvertRowInfo l) => l.Top.HasValue && l.Left.HasValue))
		{
			bottom = label.Top.Value;
			if (!label.Height.HasValue || label.Height <= 0)
			{
				bottom += 16;
			}
			else
			{
				bottom += label.Height.Value;
			}
			ConvertRowInfo convertRowInfo2 = dictionary2.Values.FirstOrDefault((ConvertRowInfo v) => v != label && v.Top > bottom - 2 && v.Top < bottom + 10 && v.Left > label.Left - 10 && v.Left < label.Left + 10);
			if (convertRowInfo2 == null)
			{
				continue;
			}
			if (!convertRowInfo2.IsProperty("ShowLabel", "True"))
			{
				convertRowInfo2.Top -= 16;
				convertRowInfo2.Height += 16;
			}
			convertRowInfo2.SetCustomProperty("ShowLabel", "True");
			if (!convertRowInfo2.Height.HasValue)
			{
				convertRowInfo2.SetCustomProperty("AutoSize", "True");
			}
			if ((string.IsNullOrWhiteSpace(label.DataField) || !label.DataField.Equals(convertRowInfo2.DataField, StringComparison.CurrentCultureIgnoreCase)) && label.CustomProperties.ContainsKey("Text"))
			{
				bool flag = false;
				if (!string.IsNullOrWhiteSpace(convertRowInfo2.DataField))
				{
					string value = label.CustomProperties["Text"].Replace("\"", "");
					SqlDataAdapter adapter2 = new SqlDataAdapter();
					DataTable dataTable2 = GetDataTable(null, databaseName, "Select dfCaption From DDFields Where dfField = " + M1Util.ConvertToSql(convertRowInfo2.DataField), fillSchema: true, out adapter2, transaction);
					if (dataTable2.Rows.Count != 0 && dataTable2.Rows[0].Field<string>("dfCaption").Equals(value, StringComparison.CurrentCultureIgnoreCase))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					convertRowInfo2.SetCustomProperty("UnboundLabelText", label.CustomProperties["Text"]);
				}
			}
			if (dictionary.ContainsKey(label.Name))
			{
				dictionary.Remove(label.Name);
			}
			list3.Add(label);
			label.Row.Delete();
		}
		foreach (ConvertRowInfo item4 in list3)
		{
			list.Remove(item4);
		}
		list3.Clear();
		foreach (ConvertRowInfo label2 in list)
		{
			if (string.IsNullOrWhiteSpace(label2.DataField))
			{
				continue;
			}
			ConvertRowInfo convertRowInfo2 = dictionary2.Values.FirstOrDefault((ConvertRowInfo v) => v != label2 && v.DataField.Equals(label2.DataField, StringComparison.CurrentCultureIgnoreCase));
			if (convertRowInfo2 != null)
			{
				if (!convertRowInfo2.IsProperty("ShowLabel", "True"))
				{
					convertRowInfo2.Top -= 16;
					convertRowInfo2.Height += 16;
				}
				convertRowInfo2.SetCustomProperty("ShowLabel", "True");
				if (dictionary.ContainsKey(label2.Name))
				{
					dictionary.Remove(label2.Name);
				}
				list3.Add(label2);
				label2.Row.Delete();
			}
		}
		list3.Clear();
		foreach (KeyValuePair<string, ConvertRowInfo> item5 in dictionary)
		{
			if (item5.Value.NewClassID.EndsWith("M1CheckBox", StringComparison.CurrentCultureIgnoreCase) && dictionary3.ContainsKey(item5.Key) && dictionary3[item5.Key].IsProperty("ShowLabel", "True"))
			{
				item5.Value.Top -= 16;
				item5.Value.Height += 16;
			}
		}
		foreach (KeyValuePair<string, ConvertRowInfo> item6 in dictionary)
		{
			if (!item6.Value.IsCustom && !dictionary3.ContainsKey(item6.Key))
			{
				list3.Add(item6.Value);
			}
		}
		foreach (ConvertRowInfo item7 in list3)
		{
			item7.Row.Delete();
			dictionary.Remove(item7.Name);
		}
		foreach (KeyValuePair<string, ConvertRowInfo> item8 in dictionary)
		{
			if (item8.Value.NewClassID.EndsWith("M1GroupLine", StringComparison.CurrentCultureIgnoreCase))
			{
				list2.Add(item8.Value);
				item8.Value.SetCustomProperty("LayoutType", "0");
				item8.Value.SetCustomProperty("AutoSize", "False");
			}
		}
		if (list2.Count != 0)
		{
			List<ConvertRowInfo> list4 = list2.OrderBy((ConvertRowInfo group) => group.Top).ToList();
			Dictionary<ConvertRowInfo, List<ConvertRowInfo>> dictionary4 = new Dictionary<ConvertRowInfo, List<ConvertRowInfo>>();
			List<ConvertRowInfo> list5 = new List<ConvertRowInfo>();
			for (int num = 0; num < list4.Count; num++)
			{
				ConvertRowInfo convertRowInfo3 = list4[num];
				convertRowInfo3.Sequence = num;
				ConvertRowInfo convertRowInfo4 = ((num + 1 >= list4.Count) ? null : list4[num + 1]);
				list5.Clear();
				foreach (ConvertRowInfo value2 in dictionary.Values)
				{
					if (value2.Top >= convertRowInfo3.Top && (convertRowInfo4 == null || value2.Top < convertRowInfo4.Top) && value2 != convertRowInfo3)
					{
						list5.Add(value2);
					}
				}
				int? num2 = null;
				foreach (ConvertRowInfo item9 in list5.OrderBy((ConvertRowInfo i) => i.Top))
				{
					if (!num2.HasValue)
					{
						num2 = item9.Top;
						item9.TopForOrdering = item9.Top;
					}
					else if (item9.Top > num2.Value + 5)
					{
						num2 = item9.Top;
						item9.TopForOrdering = item9.Top;
					}
					else
					{
						item9.TopForOrdering = num2.Value;
					}
				}
				List<ConvertRowInfo> list6 = (from item in list5
					orderby item.Left
					orderby item.TopForOrdering
					select item).ToList();
				ConvertRowInfo previousNodeInZOrder = null;
				int num3 = 0;
				foreach (ConvertRowInfo item10 in list6)
				{
					item10.Sequence = num3;
					num3++;
					item10.Group = convertRowInfo3;
					item10.PreviousNodeInZOrder = previousNodeInZOrder;
					previousNodeInZOrder = item10;
					if (item10.PreviousNodeInZOrder != null)
					{
						item10.PreviousNodeInZOrder.NextNodeInZOrder = item10;
					}
				}
				dictionary4.Add(convertRowInfo3, list6);
			}
		}
		IOrderedEnumerable<ConvertRowInfo> orderedEnumerable = from i in dictionary3.Values
			where i.Row.Field<string>("deParentID") != null
			orderby i.Row.Field<short>("deSequence")
			select i;
		new List<ConvertRowInfo>();
		List<ConvertRowInfo> list7 = new List<ConvertRowInfo>();
		ConvertRowInfo tempItem;
		foreach (ConvertRowInfo item11 in orderedEnumerable)
		{
			if (dictionary.ContainsKey(item11.Name))
			{
				if (!item11.Row.IsNull("deParentID"))
				{
					dictionary[item11.Name].Row.SetField("deParentID", item11.Row.Field<string>("deParentID"));
					dictionary[item11.Name].Row.SetField("deNestedName", item11.Row.Field<string>("deNestedName"));
				}
				dictionary[item11.Name].Row.SetField("deSequence", item11.Row.Field<short>("deSequence"));
				continue;
			}
			tempItem = LoadRowInfo(item11.Row, new ConvertRowInfo(item11.Name), item11.Row.Field<string>("deProperties"));
			list7.Add(tempItem);
			tempItem.CustomProperties.Clear();
			tempItem.Row.SetField<string>("deProperties", null);
			tempItem.Row.SetField<string>("dePropertiesUser", null);
			tempItem.Row.SetField("deCustom", tempItem.IsCustom);
			DataRow dataRow = dataTable.NewRow().BlankRow();
			foreach (DataColumn column2 in tempItem.Row.Table.Columns)
			{
				if (tempItem.Row.IsNull(column2.ColumnName) && (column2.ColumnName.Equals("deParentID", StringComparison.CurrentCultureIgnoreCase) || column2.ColumnName.Equals("deNestedName", StringComparison.CurrentCultureIgnoreCase) || column2.ColumnName.Equals("deAppExtensionID", StringComparison.CurrentCultureIgnoreCase)))
				{
					dataRow[column2.ColumnName] = string.Empty;
				}
				else
				{
					dataRow[column2.ColumnName] = tempItem.Row[column2.ColumnName];
				}
			}
			tempItem.Row = dataRow;
			dataTable.Rows.Add(dataRow);
			ConvertRowInfo prevItem = (from i in orderedEnumerable
				where i.Row.Field<string>("deParentID").Equals(tempItem.Row.Field<string>("deParentID"), StringComparison.CurrentCultureIgnoreCase) && i.Row.Field<short>("deSequence") <= tempItem.Row.Field<short>("deSequence") && !i.Name.Equals(tempItem.Name, StringComparison.CurrentCultureIgnoreCase)
				orderby i.Row.Field<short>("deSequence")
				select i).LastOrDefault();
			if (prevItem != null)
			{
				if (dictionary.ContainsKey(prevItem.Name))
				{
					prevItem = dictionary[prevItem.Name];
				}
				else
				{
					prevItem = null;
				}
			}
			if (prevItem != null)
			{
				foreach (ConvertRowInfo item12 in (from i in dictionary.Values
					where i.Group == prevItem.Group && i.Sequence > prevItem.Sequence
					orderby i.Sequence
					select i).ToList())
				{
					item12.Sequence++;
				}
				tempItem.Sequence = prevItem.Sequence + 1;
				tempItem.Group = prevItem.Group;
			}
			else if (dictionary.ContainsKey(tempItem.Row.Field<string>("deParentID")))
			{
				tempItem.Group = dictionary[tempItem.Row.Field<string>("deParentID")];
			}
			dictionary.Add(tempItem.Name, tempItem);
		}
		int num4 = newTop;
		foreach (ConvertRowInfo item13 in dictionary2.Values.OrderBy((ConvertRowInfo x) => x.Top))
		{
			if (newLeft == -1 && newTop == -1)
			{
				if (item13.Left.HasValue && item13.Top.HasValue)
				{
					item13.Location = new Point(item13.Left.Value, item13.Top.Value);
				}
			}
			else
			{
				item13.Location = new Point(newLeft, num4);
			}
			if (item13.Height.HasValue && item13.Location.HasValue && num4 < item13.Location.Value.Y + item13.Height.Value + 3)
			{
				num4 = item13.Location.Value.Y + item13.Height.Value + 3;
			}
		}
		if (list7.Count != 0)
		{
			int num5 = 500;
			int num6 = 0;
			int num7 = 0;
			foreach (ConvertRowInfo item14 in list7)
			{
				item14.Location = new Point(num6, num4);
				num6 = ((!item14.Width.HasValue) ? (num6 + 150) : (num6 + item14.Width.Value));
				if (item14.Height.HasValue)
				{
					if (num7 < item14.Height.Value)
					{
						num7 = item14.Height.Value;
					}
					else if (num7 < 40)
					{
						num7 = 40;
					}
				}
				if (num6 >= num5)
				{
					num6 = 0;
					num4 += num7;
					num7 = 0;
					item14.Location = new Point(num6, num4);
					num6 = ((!item14.Width.HasValue) ? (num6 + 150) : (num6 + item14.Width.Value));
				}
			}
			num4 += num7;
		}
		foreach (ConvertRowInfo value3 in dictionary.Values)
		{
			ConvertItemProperties(value3, convertDataBindings && value3.IsCustom, bindingSourceName);
			value3.Row.SetField("dePropertiesUser", value3.GetCustomProperties());
			value3.Row.SetField("deFormID", newFormName);
			value3.Row.SetField("deClassID", value3.NewClassID);
			value3.Row.SetField("deSequenceUser", value3.Sequence);
			value3.Row.SetField("deCustom", value3.IsCustom);
			if (value3.Group != null && !value3.Row.Field<string>("deParentID").Equals(value3.Group.Name, StringComparison.CurrentCultureIgnoreCase))
			{
				value3.Row.SetField("deParentIDUser", value3.Group.Name);
				value3.Row.SetField("deNestedNameUser", "WorkingArea");
			}
			value3.Row.SetField("deParentIDUser", string.Empty);
			value3.Row.SetField("deNestedNameUser", string.Empty);
		}
		dictionary.Clear();
		currentContext.DDServerManager.UpdateData(null, null, databaseName, dataTable, adapter, transaction);
		dataTable = GetDataTable(null, databaseName, $"Select * From DDFormDetails Where deFormID = {newFormName.ToSql()} And deControlName = ''", fillSchema: true, out adapter, transaction);
		if (dataTable.Rows.Count == 0)
		{
			dataTable.AddBlankRow(allowNullForDefaultValue: true);
			dataTable.Rows[0].SetField("deFormID", newFormName);
		}
		DataRow row2 = dataTable.Rows[0];
		if (newLeft != -1 && newTop != -1)
		{
			row2.SetField("dePropertiesUser", setProp(row2.Field<string>("dePropertiesUser"), "Height", num4.ToString()));
		}
		row2.SetField("dePropertiesUser", setProp(row2.Field<string>("dePropertiesUser"), "LayoutType", "0"));
		currentContext.DDServerManager.UpdateData(null, null, databaseName, dataTable, adapter, transaction);
	}

	private void ConvertItemProperties(ConvertRowInfo item, bool convertDataBindings, string bindingSourceName)
	{
		if (convertDataBindings)
		{
			string text = string.Empty;
			if (item.DataSource.Length != 0)
			{
				text = ((bindingSourceName.Length == 0) ? item.DataSource : bindingSourceName);
			}
			switch (item.Row.Field<string>("deClassID").Trim().ToUpper())
			{
			case "M1CONTROLS.M1EDITBOX":
			case "M1CONTROLS92.M1EDITBOX":
			case "M1CONTROLS91.M1EDITBOX":
				if (item.DataField.Length != 0 && text.Length != 0)
				{
					item.SetCustomProperty("DataBindings", $"\"ValueRtf, {text}, {item.DataField}, true, OnPropertyChanged\"");
				}
				if (item.DataFieldText.Length != 0 && text.Length != 0)
				{
					item.SetCustomProperty("DataBindings", $"\"ValueText, {text}, {item.DataFieldText}, true, OnPropertyChanged\"");
				}
				break;
			case "M1CONTROLS.M1OPTIONBUTTON":
			case "M1CONTROLS92.M1OPTIONBUTTON":
			case "M1CONTROLS91.M1OPTIONBUTTON":
				if (item.DataField.Length != 0 && text.Length != 0)
				{
					item.SetCustomProperty("DataBindings", $"\"ValueString, {text}, {item.DataField}, true, OnPropertyChanged\"\r\n");
				}
				break;
			default:
				if (item.DataField.Length != 0 && text.Length != 0)
				{
					item.SetCustomProperty("DataBindings", $"\"Value, {text}, {item.DataField}, true, OnPropertyChanged\"\r\n");
				}
				break;
			}
		}
		if (item.Location.HasValue)
		{
			item.SetCustomProperty("Location", string.Format("\"" + item.Location.Value.X + ", " + item.Location.Value.Y + "\""));
		}
		else
		{
			if (item.Top.HasValue)
			{
				item.SetCustomProperty("Top", item.Top.Value.ToString());
			}
			if (item.Left.HasValue)
			{
				item.SetCustomProperty("Left", item.Left.Value.ToString());
			}
		}
		if (item.Height.HasValue && item.Height != 0 && item.Width.HasValue && item.Width != 0)
		{
			item.SetCustomProperty("Size", "\"" + item.Width.Value + ", " + item.Height.Value + "\"");
		}
		else
		{
			if (item.Height.HasValue && item.Height != 0)
			{
				item.SetCustomProperty("Height", item.Height.Value.ToString());
			}
			if (item.Width.HasValue && item.Width != 0)
			{
				item.SetCustomProperty("Width", item.Width.Value.ToString());
			}
		}
		if (item.IsCustom && item.NewClassID.Equals("M1.Forms.Controls.M1ComboBox", StringComparison.CurrentCultureIgnoreCase) && item.Row.Field<string>("dePropertiesUser").Contains("ListSource") && item.CustomProperties.TryGetValue("ListSource", out var value))
		{
			item.CustomProperties.Remove("ListSource");
			item.CustomProperties.Add("Search.RowSource", value);
			item.Row.Field<string>("dePropertiesUser").Replace("ListSource", "Search.RowSource");
		}
		if (item.IsCustom && (item.NewClassID.Contains("M1MaskedTextEditor") || item.NewClassID.Contains("M1NumericEditor")) && item.Row.Field<string>("dePropertiesUser").Contains("ShowSearchButton") && item.CustomProperties.TryGetValue("ShowSearchButton", out var value2))
		{
			item.CustomProperties.Remove("ShowSearchButton");
			item.CustomProperties.Add("Search.SearchButtonVisible", value2);
			item.Row.Field<string>("dePropertiesUser").Replace("ShowSearchButton", "Search.SearchButtonVisible");
		}
		if (item.IsCustom && item.NewClassID.Contains("M1OptionButton") && item.Row.Field<string>("dePropertiesUser").Contains("Value") && item.CustomProperties.TryGetValue("Value", out var value3))
		{
			item.CustomProperties.Remove("Value");
			item.CustomProperties.Add("ValueString", value3);
			item.Row.Field<string>("dePropertiesUser").Replace("Value", "ValueString");
		}
		if (item.IsCustom && item.NewClassID.Contains("M1NumericEditor") && item.Row.Field<string>("dePropertiesUser").Contains("InputMask") && item.CustomProperties.TryGetValue("InputMask", out var value4))
		{
			item.CustomProperties.Remove("InputMask");
			value4 = new Regex("[1234567890N]", RegexOptions.None).Replace(value4, "n");
			item.CustomProperties.Add("InputMask", value4);
			item.Row.SetField("dePropertiesUser", setProp(item.Row.Field<string>("dePropertiesUser"), "InputMask", value4));
		}
	}

	private string setProp(string props, string propName, string value)
	{
		if (props == null)
		{
			return $"{propName} = {value}";
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		string[] array = props.Replace("\n", "").Split('\r');
		foreach (string text in array)
		{
			int num = text.IndexOf('=');
			if (num != -1)
			{
				string text2 = text.Substring(0, num).Trim();
				string arg = text.Substring(num + 1).Trim();
				if (text2.Equals(propName, StringComparison.CurrentCultureIgnoreCase))
				{
					arg = value;
					flag = true;
				}
				stringBuilder.AppendFormat("{0} = {1} \r\n", text2, arg);
			}
			else
			{
				stringBuilder.AppendFormat("{0}\r\n", text);
			}
		}
		if (!flag)
		{
			stringBuilder.AppendFormat("{0} = {1} \r\n", propName, value);
		}
		return stringBuilder.ToString();
	}

	public void SetCollation(M1User m1User, M1DataDictionary m1DataDictionary, string dataBaseName, string newCollation, List<string> messages, Dmo.SetCollationDelegate func)
	{
		DDDatabaseDefinition dDDatabaseDefinition = new DDDatabaseDefinition();
		ServerManager dDServerManager = currentContext.DDServerManager;
		dDServerManager.ClearAllPools();
		using SqlConnection sqlConnection = dDServerManager.GetConnection(m1User, dataBaseName, openImmediately: true);
		func?.Invoke("Opening " + dataBaseName + " in single user mode");
		dDServerManager.ExecuteCommand(sqlConnection, m1User, "master", "ALTER DATABASE " + dataBaseName.ToString() + " Set SINGLE_USER WITH ROLLBACK IMMEDIATE");
		try
		{
			string database = sqlConnection.Database;
			string text = (string)dDServerManager.ExecuteScalar(sqlConnection, m1User, dataBaseName, "select DATABASEPROPERTYEX('" + dataBaseName + "','collation')");
			dDServerManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "USE master");
			dDServerManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "ALTER DATABASE " + dataBaseName + " COLLATE " + newCollation);
			dDServerManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "USE " + database);
			try
			{
				foreach (DataRow item in dDServerManager.GetDataTable(sqlConnection, m1User, dataBaseName, 0, "exec sp_tables @table_type = \"'TABLE'\"").Rows)
				{
					if (item.Field<string>("table_name") != "dtproperties" && !item.Field<string>("table_owner").Trim().Equals("sys", StringComparison.CurrentCultureIgnoreCase) && dDDatabaseDefinition.Tables.Exists((DDTableDefinition x) => x.TableName.Equals(item.Field<string>("table_name").Trim(), StringComparison.CurrentCultureIgnoreCase)))
					{
						func(item.Field<string>("table_name").Trim());
						ReloadTable(dataBaseName, item.Field<string>("table_name").Trim(), recreateTable: true, null, dDDatabaseDefinition, sqlConnection, m1User);
					}
				}
			}
			catch
			{
				dDServerManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "USE master");
				dDServerManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "ALTER DATABASE " + dataBaseName + " COLLATE " + text);
				dDServerManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "USE " + database);
				throw;
			}
			finally
			{
				dDServerManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "USE " + database);
			}
		}
		finally
		{
			dDServerManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "ALTER DATABASE " + dataBaseName.ToString() + " Set MULTI_USER");
			func?.Invoke(string.Empty);
		}
	}
}
