using System;
using System.Collections.Generic;

namespace M1.Core;

public class DDDatabaseDefinition
{
	public List<DDTableDefinition> Tables = new List<DDTableDefinition>();

	public List<DDCustomTableInfo> LoadedTableInfos = new List<DDCustomTableInfo>();

	public DDTableDefinition GetTable(string table)
	{
		foreach (DDTableDefinition table2 in Tables)
		{
			if (table2.TableName.Equals(table, StringComparison.CurrentCultureIgnoreCase))
			{
				return table2;
			}
		}
		return null;
	}

	public DDCustomTableInfo GetUpdateInfoForTable(string table, string serverCollation)
	{
		foreach (DDCustomTableInfo loadedTableInfo in LoadedTableInfos)
		{
			if (loadedTableInfo.Table.Equals(table, StringComparison.CurrentCultureIgnoreCase))
			{
				return loadedTableInfo;
			}
		}
		foreach (DDTableDefinition table2 in Tables)
		{
			if (table2.TableName.Equals(table, StringComparison.CurrentCultureIgnoreCase))
			{
				DDCustomTableInfo updateInfo = table2.GetUpdateInfo(serverCollation);
				LoadedTableInfos.Add(updateInfo);
				return updateInfo;
			}
		}
		return null;
	}

	public DDDatabaseDefinition()
	{
		Tables.Add(new DDTableDefinition("DDInfo", exportForSetup: false, new DDFieldDefinition[14]
		{
			new DDFieldDefinition("ddVersion", "nvarchar(10)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddRegion", "nvarchar(3)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddLanguage", "nvarchar(10)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddProductCode", "nvarchar(16)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddCustomProductCodes", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddProperties", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddDSProperties", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddUpgradeVersions", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddHosted", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddEasyOrder", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddEDI", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddCompanyId", "nvarchar(36)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddMobile", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddWebRegion", "nvarchar(20)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None)
		}, new DDIndexDefinition[0], null, null, null, null));
		Tables.Add(new DDTableDefinition("DDAppExtensions", exportForSetup: true, new DDFieldDefinition[9]
		{
			new DDFieldDefinition("dpAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.AppExtensionID),
			new DDFieldDefinition("dpCaption", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dpCodeAssembly", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dpFormsAssembly", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dpDDAssembly", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dpLastUpdatedDDVersion", "nvarchar(10)", nullable: false, "''", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dpDependencies", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dpUniqueID", "uniqueidentifier", nullable: false, "NEWID()", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dpCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None)
		}, new DDIndexDefinition[2]
		{
			new DDIndexDefinition("dpAppExtensionID", unique: true),
			new DDIndexDefinition("dpUniqueID", unique: true)
		}, new string[1] { "dpUniqueID" }, new string[1] { "dpCaption" }, new string[1] { "App Extension" }, new string[1] { "dpAppExtensionID" }));
		Tables.Add(new DDTableDefinition("DDTables", exportForSetup: true, new DDFieldDefinition[88]
		{
			new DDFieldDefinition("dtUniqueID", "uniqueidentifier", nullable: false, "NEWID()", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dtTable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("dtDisplayName", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("dtDefaultObjectId", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.ObjectID),
			new DDFieldDefinition("dtGridID", "nvarchar(35)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.GridID),
			new DDFieldDefinition("dtKeyFields", "nvarchar(150)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtLastKeyCanBeEmpty", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtOverrideDelete", "nvarchar(150)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtOverrideDeleteEnabledExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dtEmptyKeyCanBeEdited", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtKeysAtThisLevel", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtEnterInSequenceField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtKeyGroup", "nvarchar(10)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtAddFld1", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtAddFld2", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtAddFld3", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtUAddFld1", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Custom, DDFieldContentType.Field),
			new DDFieldDefinition("dtUAddFld2", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Custom, DDFieldContentType.Field),
			new DDFieldDefinition("dtUAddFld3", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Custom, DDFieldContentType.Field),
			new DDFieldDefinition("dtColorExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dtColorExpressionUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dtNumericOnly", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtAutoIncrement", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtAutoIncrementUser", "tinyint", nullable: false, "0", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dtIncrementAmount", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtIncrementAmountUser", "smallint", nullable: false, "0", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dtInitialValue", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtCaption", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtPrefix", "nvarchar(3)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtPrefixUser", "nvarchar(4)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtReadonlyExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dtReadonlyExpressionUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dtDisableAddNewExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dtDisableAddNewExpressionUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dtDisableDeleteExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dtDisableDeleteExpressionUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dtDisableChangeIDExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dtDisableChangeIDExpressionUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dtFieldToCheckOnUpdate", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtHasDeleteCode", "bit", nullable: false, "0", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dtCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("dtVTbl", "nvarchar(6)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtV3Tb", "nvarchar(6)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtUTbl", "nvarchar(6)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtVRel", "nvarchar(65)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtGridEdit", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtChangeDetailIdsFilter", "nvarchar(35)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Filter),
			new DDFieldDefinition("dtForeignKeyDeleteFilter", "nvarchar(75)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Filter),
			new DDFieldDefinition("dtChangeID", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtSaveAs", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtModule", "nvarchar(2)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtMailMerge", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtMap", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtImport", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtShowMemos", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtShowMemosUser", "bit", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dtMemoDescription", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtMemoDescriptionUser", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dtSQLView", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtViewDef", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtViewSeq", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtWebSeq", "smallint", nullable: false, "0", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dtContactField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtPromptOnAddField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtQuickSearchOption", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtQuickSearchFields", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtQuickSearchFieldsUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Field),
			new DDFieldDefinition("dtDocumentPlantIdField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtDocumentDateField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtCurrencyModeLocationField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtCurrencyRateIdField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtCurrencyCustomRateField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtCurrencyExchangeRateField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtCurrencyUpdateType", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtClosedField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtClosedValue", "nvarchar(10)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dtClosedDateField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtClosedExtraSetExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Filter),
			new DDFieldDefinition("dtClosedIncludeOptionText", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtClosedIncludeOptionSqlExpr", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Filter),
			new DDFieldDefinition("dtClosedCutoffDateField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtClosedRoleCheck", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtClosedHelpLink", "nvarchar(75)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtPurgeCutoffDateField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtPurgeHelpLink", "nvarchar(75)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtParentTable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtUniqueField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dtAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[3]
		{
			new DDIndexDefinition("dtUniqueID", unique: true),
			new DDIndexDefinition("dtTable", unique: true),
			new DDIndexDefinition("dtDefaultObjectId", unique: false)
		}, new string[1] { "dtUniqueID" }, new string[1] { "dtTable" }, new string[1] { "Table" }, new string[1] { "dtTable" }));
		Tables.Add(new DDTableDefinition("DDFields", exportForSetup: true, new DDFieldDefinition[67]
		{
			new DDFieldDefinition("dfUniqueID", "uniqueidentifier", nullable: false, "NEWID()", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dfTable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("dfField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dfDisplayName", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dfSequence", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfSequenceUser", "smallint", nullable: false, "0", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dfCaption", "nvarchar(40)", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfCustomCaption", "bit", nullable: false, "0", DDFieldFlag.Custom, DDFieldContentType.None, "dfCaption"),
			new DDFieldDefinition("dfCaptionExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dfCaptionExpressionUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dfDBType", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfLength", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfDecimals", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfAllowNulls", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfLower", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfIndexed", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfHasChangeCode", "bit", nullable: false, "0", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dfCalculationExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dfRequiredExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dfRequiredExpressionUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dfOLType", "nvarchar(1)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfOLRelFld", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dfFormat", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfDefaultExpression", "nvarchar(max)", nullable: true, "''", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dfDPrv", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfDefaultExpressionUser", "nvarchar(max)", nullable: true, "''", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dfUDPr", "tinyint", nullable: false, "0", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dfSaveAsExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dfSaveAsExpressionUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dfForeignKeyRequiredExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dfForeignKeyRequiredExpressionUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dfReadonlyExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dfReadonlyExpressionUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dfModule", "nvarchar(15)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfVisibleExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dfVisibleExpressionUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dfBoundParentField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dfBoundParentFieldType", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfBoundParentFieldProxy", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dfBoundParentFieldExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dfRelationType", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfRelatedTable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("dfRelatedFields", "nvarchar(100)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dfRequiredForeignRelation", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfFFil", "nvarchar(110)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Filter),
			new DDFieldDefinition("dfCurrencyType", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfCurrencyRelatedField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dfCurrencyUpdateRelatedField", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dfValueList", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfShowAsDropdown", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfShowAsDropdownUser", "bit", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dfRelatedTableSearchGridId", "nvarchar(35)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.GridID),
			new DDFieldDefinition("dfRelatedTableReturnField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dfRelatedTableDescriptionField", "nvarchar(40)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dfRelatedTableOrderByField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dfRelatedTableFilter", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dfHide", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfHelp", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfGroup", "nvarchar(10)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfGroupParameters", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfComments", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfStatus", "nvarchar(75)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("dfConv", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfVFld", "nvarchar(6)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfv3fd", "nvarchar(6)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dfAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[5]
		{
			new DDIndexDefinition("dfUniqueID", unique: true),
			new DDIndexDefinition("dfTable", unique: false),
			new DDIndexDefinition("dfField", unique: true),
			new DDIndexDefinition("dfOLType", unique: false),
			new DDIndexDefinition("dfTable,dfField", unique: true)
		}, new string[1] { "dfUniqueID" }, new string[2] { "dfTable", "dfField" }, new string[2] { "Table", "Field" }, new string[2] { "dfTable", "dfField" }, "UPDATE DDFields SET DDFields.dfCaption = RTrim(DDFieldsEx.dfCaption) FROM DDFields INNER JOIN DDFieldsEx ON DDFields.dfUniqueID=DDFieldsEx.dfUniqueID WHERE DDFields.dfCustomCaption <> 0"));
		Tables.Add(new DDTableDefinition("DDFieldExtensions", exportForSetup: true, new DDFieldDefinition[24]
		{
			new DDFieldDefinition("dqUniqueID", "uniqueidentifier", nullable: false, "NEWID()", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dqTable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("dqField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dqFieldExtensionID", "nvarchar(10)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dqFieldExtensionTypeID", "nvarchar(10)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dqSequence", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dqStatusPositive", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dqStatusNegative", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dqTransactionType", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dqSource", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dqPartBinField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dqTransactionDateField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dqRelatedJobField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dqRelatedJobStatusField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dqRelatedPlantField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dqReverseSign", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dqParameters", "nvarchar(100)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("dqAvailableFilterPositiveExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dqAvailableFilterNegativeExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dqAllowMismatchedQuantity", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dqRequiredExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dqRequiredExpressionUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dqCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("dqAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[3]
		{
			new DDIndexDefinition("dqUniqueID", unique: true),
			new DDIndexDefinition("dqTable,dqField,dqFieldExtensionID", unique: true),
			new DDIndexDefinition("dqFieldExtensionTypeID", unique: false)
		}, new string[1] { "dqUniqueID" }, new string[2] { "dqTable", "dqField" }, new string[2] { "Table", "Field" }, new string[2] { "dqTable", "dqField" }));
		Tables.Add(new DDTableDefinition("DDFieldExtensionTypes", exportForSetup: true, new DDFieldDefinition[7]
		{
			new DDFieldDefinition("dhFieldExtensionTypeID", "nvarchar(10)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dhCaption", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dhClass", "nvarchar(100)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dhOpenWithID", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dhAllowMultiple", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dhCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("dhAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[1]
		{
			new DDIndexDefinition("dhFieldExtensionTypeID", unique: true)
		}, new string[1] { "dhFieldExtensionTypeID" }, new string[1] { "dhFieldExtensionTypeID" }, new string[1] { "Extension" }, new string[1] { "dhFieldExtensionTypeID" }));
		Tables.Add(new DDTableDefinition("DDFieldGroups", exportForSetup: true, new DDFieldDefinition[10]
		{
			new DDFieldDefinition("ddgFieldGroupID", "nvarchar(10)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("ddgCaption", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddgDefaultControl", "nvarchar(100)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddgDefaultControlUser", "nvarchar(100)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("ddgValidFieldTypes", "nvarchar(100)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddgTextFormatter", "nvarchar(100)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddgTextFormatterUser", "nvarchar(100)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("ddgUniqueID", "uniqueidentifier", nullable: false, "NEWID()", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddgCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("ddgAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[2]
		{
			new DDIndexDefinition("ddgFieldGroupID", unique: true),
			new DDIndexDefinition("ddgUniqueID", unique: true)
		}, new string[1] { "ddgUniqueID" }, new string[1] { "ddgFieldGroupID" }, new string[1] { "Field Group" }, new string[1] { "ddgFieldGroupID" }));
		Tables.Add(new DDTableDefinition("DDCode", exportForSetup: true, new DDFieldDefinition[6]
		{
			new DDFieldDefinition("dkCodeID", "uniqueidentifier", nullable: false, "NEWID()", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dkSourceTable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("dkSourceUniqueID", "uniqueidentifier", nullable: false, "NEWID()", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dkCode", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Code),
			new DDFieldDefinition("dkCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("dkAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[3]
		{
			new DDIndexDefinition("dkCodeID", unique: true),
			new DDIndexDefinition("dkSourceTable", unique: false),
			new DDIndexDefinition("dkSourceUniqueID", unique: false)
		}, new string[1] { "dkCodeID" }, new string[2] { "dkSourceTable", "dkCodeID" }, new string[2] { "Code Type", "Code ID" }, new string[1] { "dkCodeID" }));
		Tables.Add(new DDTableDefinition("DDRelations", exportForSetup: true, new DDFieldDefinition[15]
		{
			new DDFieldDefinition("drRelationID", "uniqueIdentifier", nullable: false, "NEWID()", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("drPTable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("drPField", "nvarchar(90)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("drCTable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("drCField", "nvarchar(90)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("drFilter", "nvarchar(35)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Filter),
			new DDFieldDefinition("drPersist", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("drSaveAs", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("drForeign", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("drNonKey", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("drReseqDetails", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("drDFilter", "nvarchar(35)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Filter),
			new DDFieldDefinition("drCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("drIgnore", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("drAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[3]
		{
			new DDIndexDefinition("drRelationID", unique: true),
			new DDIndexDefinition("drCTable", unique: false),
			new DDIndexDefinition("drPTable", unique: false)
		}, new string[1] { "drRelationID" }, new string[2] { "drPTable", "drCTable" }, new string[2] { "Table", "Relation" }, new string[2] { "drPTable", "drCTable" }));
		Tables.Add(new DDTableDefinition("DDExplorer", exportForSetup: true, new DDFieldDefinition[26]
		{
			new DDFieldDefinition("dxUniqueId", "uniqueidentifier", nullable: false, "NEWID()", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dxUser", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxMode", "nvarchar(4)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxText", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxParentUniqueID", "uniqueidentifier", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxLinkedUniqueID", "uniqueidentifier", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxType", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxViewer", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxExtd", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Code),
			new DDFieldDefinition("dxGridID", "nvarchar(35)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.GridID),
			new DDFieldDefinition("dxSMod", "nvarchar(2)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxVisualizerID", "nvarchar(35)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxVisualizerType", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxSCom", "nvarchar(100)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxImageLarge", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxImageSmall", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxSequence", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxDisabled", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxCollapse", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxRemoved", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxOldId", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxOldParentId", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxOldLinkedId", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dxAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID),
			new DDFieldDefinition("dxCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("dxLanguageID", "nvarchar(55)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None)
		}, new DDIndexDefinition[5]
		{
			new DDIndexDefinition("dxUniqueID", unique: true),
			new DDIndexDefinition("dxParentUniqueID", unique: false),
			new DDIndexDefinition("DXUSER", unique: false),
			new DDIndexDefinition("dxLinkedUniqueID", unique: false),
			new DDIndexDefinition("DXMODE", unique: false)
		}, new string[1] { "dxUniqueID" }, new string[1] { "dxText,dxUniqueID" }, new string[1] { "Explorer" }, null, "DELETE FROM DDExplorer Where dxUser <> '' And dxMode = 'TREE' And dxCustom <> 0 And Not dxLinkedUniqueID Is Null And dxLinkedUniqueID Not In (Select dxUniqueID From DDExplorer Where dxUser = '' And dxMode = 'TREE' And dxLinkedUniqueID Is Null)")
		{
			PackageFilter = ".dxUser = ''"
		});
		Tables.Add(new DDTableDefinition("DDUsers", exportForSetup: true, new DDFieldDefinition[31]
		{
			new DDFieldDefinition("duUserID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("duPassword", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duName", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duAdministrator", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duDBAdministrator", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duDeveloper", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duGridDeveloper", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duPasswordLocked", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duDeveloperProperties", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duProperties", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duWebTemplate", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duWebAnonymous", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duWebTemplateDefault", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duDDAlertUser", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duBackupVerifyDays", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duAutoLogin", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duAutoLogout", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duInactiveCheckMinutes", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duInactiveDate", "datetime", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duPasswordSetDate", "datetime", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duLastLoginTime", "datetime", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duLastLogoutTime", "datetime", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duLastLoginMachine", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duPasswordExpirationDays", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duMustChangePassword", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duPortal", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duType", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID),
			new DDFieldDefinition("duCustom", "bit", nullable: false, "1", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("duCloudPrincipalId", "uniqueidentifier", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("duPortalUserEmail", "nvarchar(250)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None)
		}, new DDIndexDefinition[1]
		{
			new DDIndexDefinition("duUserID", unique: true)
		}, new string[1] { "duUserID" }, new string[1] { "duUserID" }, new string[1] { "Component Security" }, null)
		{
			PackageFilter = ".duType = 2"
		});
		Tables.Add(new DDTableDefinition("DDUserLog", exportForSetup: false, new DDFieldDefinition[13]
		{
			new DDFieldDefinition("ulUserClientID", "uniqueidentifier", nullable: false, "NEWID()", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("ulUserID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ulUserType", "nvarchar(2)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ulMachine", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ulUserName", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ulDatabaseClientID", "uniqueidentifier", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ulDatabase", "nvarchar(25)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ulEmailAddress", "nvarchar(100)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ulLoginTime", "datetime", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ulLastActivityTime", "datetime", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ulLastActionTime", "datetime", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ulMessageType", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ulMessageText", "nvarchar(100)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None)
		}, new DDIndexDefinition[5]
		{
			new DDIndexDefinition("ulUserClientID, ulDatabaseClientID", unique: true),
			new DDIndexDefinition("ulUserClientID", unique: false),
			new DDIndexDefinition("ulUserID", unique: false),
			new DDIndexDefinition("ulDatabaseClientID", unique: false),
			new DDIndexDefinition("ulDatabase", unique: false)
		}, null, null, null, null));
		Tables.Add(new DDTableDefinition("DDObjectDetails", exportForSetup: true, new DDFieldDefinition[23]
		{
			new DDFieldDefinition("dlObjectID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.ObjectID),
			new DDFieldDefinition("dlLine", "tinyint", nullable: false, "0", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dlTable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("dlParent", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("dlSequence", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dlLevel", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dlView", "nvarchar(100)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dlCollapse", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dlUCollaps", "smallint", nullable: false, "0", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dlDHide", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dlUHide", "tinyint", nullable: false, "0", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dlSearchID", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dlGridID", "nvarchar(35)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.GridID),
			new DDFieldDefinition("dlOrder", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dlUOrder", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Field),
			new DDFieldDefinition("dlCField", "nvarchar(90)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dlFilter", "nvarchar(35)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Filter),
			new DDFieldDefinition("dlrlf", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dlNoAdd", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dlHide", "nvarchar(70)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dlROnI", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dlCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("dlAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[5]
		{
			new DDIndexDefinition("dlObjectID,DLLINE", unique: true),
			new DDIndexDefinition("dlTable", unique: false),
			new DDIndexDefinition("dlObjectID", unique: false),
			new DDIndexDefinition("DLLINE", unique: false),
			new DDIndexDefinition("dlParent", unique: false)
		}, new string[2] { "dlObjectID", "dlLine" }, new string[3] { "dlObjectID", "dlTable", "dlView" }, new string[3] { "FormCollection", "Table", "Form" }, new string[2] { "dlObjectID", "dlLine" }));
		Tables.Add(new DDTableDefinition("DDObjects", exportForSetup: true, new DDFieldDefinition[9]
		{
			new DDFieldDefinition("doObjectID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.ObjectID),
			new DDFieldDefinition("doTable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("doName", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("doTitle", "nvarchar(40)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("doTreeLoader", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("doTreeLoaderUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("doModule", "nvarchar(2)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("doAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID),
			new DDFieldDefinition("doCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None)
		}, new DDIndexDefinition[2]
		{
			new DDIndexDefinition("doObjectID", unique: true),
			new DDIndexDefinition("doTable", unique: false)
		}, new string[1] { "doObjectID" }, new string[1] { "doObjectID" }, new string[1] { "FormCollection" }, new string[1] { "doObjectID" }));
		Tables.Add(new DDTableDefinition("DDForms", exportForSetup: true, new DDFieldDefinition[18]
		{
			new DDFieldDefinition("dmUniqueID", "uniqueidentifier", nullable: false, "NEWID()", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dmFormID", "nvarchar(100)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dmTable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("dmCaption", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dmHelpLink", "nvarchar(75)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dmVID", "nvarchar(10)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dmCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("dmType", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dmFormType", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dmAllInDD", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dmDesGroup", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dmAssemblies", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dmAssembliesUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dmCompiled", "varbinary(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dmRunType", "tinyint", nullable: false, "0", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dmIsChanged", "bit", nullable: false, "0", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dmNeedToCompile", "bit", nullable: false, "0", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dmAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[4]
		{
			new DDIndexDefinition("dmFormID", unique: true),
			new DDIndexDefinition("dmUniqueID", unique: true),
			new DDIndexDefinition("dmTable", unique: false),
			new DDIndexDefinition("dmType", unique: false)
		}, new string[1] { "dmUniqueID" }, new string[1] { "dmFormID" }, new string[1] { "Form" }, new string[1] { "dmFormID" }));
		Tables.Add(new DDTableDefinition("DDFormDetails", exportForSetup: true, new DDFieldDefinition[13]
		{
			new DDFieldDefinition("deFormID", "nvarchar(100)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("deControlName", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("deParentID", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("deParentIDUser", "nvarchar(50)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("deNestedName", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("deNestedNameUser", "nvarchar(50)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("deClassID", "nvarchar(100)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("deSequence", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("deSequenceUser", "smallint", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("deProperties", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dePropertiesUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("deAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID),
			new DDFieldDefinition("deCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None)
		}, new DDIndexDefinition[3]
		{
			new DDIndexDefinition("deFormID,deControlName", unique: true),
			new DDIndexDefinition("deFormID", unique: false),
			new DDIndexDefinition("deControlName", unique: false)
		}, new string[2] { "deFormID", "deControlName" }, new string[2] { "deFormID", "deControlName" }, new string[2] { "Form", "Control" }, new string[2] { "deFormID", "deControlName" }));
		Tables.Add(new DDTableDefinition("DDSecurityTables", exportForSetup: false, new DDFieldDefinition[10]
		{
			new DDFieldDefinition("dtUserID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dtDataset", "nvarchar(25)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dtTable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.Table),
			new DDFieldDefinition("dtField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.Field),
			new DDFieldDefinition("dtLevel", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtRowFilter", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtEditExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtAddExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtDeleteExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dtChangeIDExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None)
		}, new DDIndexDefinition[5]
		{
			new DDIndexDefinition("dtUserID,dtDataset,dtTable,dtField", unique: true),
			new DDIndexDefinition("dtUserID", unique: false),
			new DDIndexDefinition("dtDataset", unique: false),
			new DDIndexDefinition("dtTable", unique: false),
			new DDIndexDefinition("dtField", unique: false)
		}, null, null, null, null));
		Tables.Add(new DDTableDefinition("DDSecurityReports", exportForSetup: false, new DDFieldDefinition[6]
		{
			new DDFieldDefinition("drFolder", "nvarchar(75)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("drReport", "nvarchar(75)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("drUserID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("drDataset", "nvarchar(25)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("drLevel", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("drSettings", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None)
		}, new DDIndexDefinition[4]
		{
			new DDIndexDefinition("drUserID,drDataset,drFolder,drReport", unique: true),
			new DDIndexDefinition("drUserID", unique: false),
			new DDIndexDefinition("drDataset", unique: false),
			new DDIndexDefinition("drReport", unique: false)
		}, null, null, null, null));
		Tables.Add(new DDTableDefinition("DDSecurityGroups", exportForSetup: false, new DDFieldDefinition[3]
		{
			new DDFieldDefinition("dzGroupID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dzUserID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dzDataset", "nvarchar(25)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None)
		}, new DDIndexDefinition[4]
		{
			new DDIndexDefinition("dzUserID,dzDataset,dzGroupID", unique: true),
			new DDIndexDefinition("dzUserID", unique: false),
			new DDIndexDefinition("dzDataset", unique: false),
			new DDIndexDefinition("dzGroupID", unique: false)
		}, null, null, null, null));
		Tables.Add(new DDTableDefinition("DDGrids", exportForSetup: true, new DDFieldDefinition[11]
		{
			new DDFieldDefinition("djGridID", "nvarchar(35)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.GridID),
			new DDFieldDefinition("djUserID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("djTable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("djDesc", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("djExtd", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("djSPGroup", "nvarchar(10)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("djSPSequence", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("djCustom", "bit", nullable: false, "1", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("djExplorer", "nvarchar(2)", nullable: false, "''", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("djNoPrimaryTable", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("djAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[4]
		{
			new DDIndexDefinition("djGridID", unique: true),
			new DDIndexDefinition("djUserID", unique: false),
			new DDIndexDefinition("djTable", unique: false),
			new DDIndexDefinition("djExplorer", unique: false)
		}, new string[1] { "djGridID" }, new string[1] { "djGridID" }, new string[1] { "Grid" }, new string[1] { "djGridID" })
		{
			PackageFilter = ".djUserID = '' Or .djUserID = 'DEFAULT'"
		});
		Tables.Add(new DDTableDefinition("DDGridDetails", exportForSetup: true, new DDFieldDefinition[68]
		{
			new DDFieldDefinition("dgGridID", "nvarchar(35)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.GridID),
			new DDFieldDefinition("dgUserID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dgGBox", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgExp", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgGrp", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dgOrd", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dgFlds", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dgFrom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dgReqOpt", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dgWher", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dgSGrp", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dgSOrd", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dgFBox", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgSQLSet", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgADOSet", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgLOpt", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgShar", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgCustom", "bit", nullable: false, "1", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("dgEdit", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgDatasets", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgPrePane", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgPaneSize", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgPortrait", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgFreeze", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgWebGrid", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgWebSeq", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgSPGroup", "nvarchar(10)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgSPSeq", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgSPText", "nvarchar(75)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dgSPCalc", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dgCalDateF", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dgWgRMACS", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS1Bold", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS1Italic", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS1BColor", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS1FColor", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS2Bold", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS2Italic", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS2BColor", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS2FColor", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS3Bold", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS3Italic", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS3BColor", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS3FColor", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS4Bold", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS4Italic", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS4BColor", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS4FColor", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS5Bold", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS5Italic", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS5BColor", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgS5FColor", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgSFormula", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dgLockd", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgLockf", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgLockg", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgLocks", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgLocko", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgFBoxSP", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgWGFilt", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgOpenWithID", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgTreeVisible", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgTreeWidth", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgTreeSettings", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgExportProperties", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgPrintingProperties", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgUseCurrencyMode", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dgAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[4]
		{
			new DDIndexDefinition("dgGridID,dgUserID", unique: true),
			new DDIndexDefinition("dgGridID", unique: false),
			new DDIndexDefinition("dgUserID", unique: false),
			new DDIndexDefinition("dgSPGroup", unique: false)
		}, new string[2] { "dgGridID", "dgUserID" }, new string[2] { "dgGridID", "dgUserID" }, new string[2] { "Grid", "User" }, new string[2] { "dgGridID", "dgUserID" })
		{
			PackageFilter = ".dgUserID = '' Or .dgUserID = 'DEFAULT'"
		});
		Tables.Add(new DDTableDefinition("DDSearches", exportForSetup: false, new DDFieldDefinition[11]
		{
			new DDFieldDefinition("dsSearchID", "nvarchar(100)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dsUserID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dsGridID", "nvarchar(35)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.GridID),
			new DDFieldDefinition("dsTop", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dsLeft", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dsHeight", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dsWidth", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dsWindowState", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dsField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dsPreviousGrids", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dsMonitor", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None)
		}, new DDIndexDefinition[4]
		{
			new DDIndexDefinition("dsSearchID,dsUserID", unique: true),
			new DDIndexDefinition("dsSearchID", unique: false),
			new DDIndexDefinition("dsUserID", unique: false),
			new DDIndexDefinition("dsGridID", unique: false)
		}, null, null, null, null));
		Tables.Add(new DDTableDefinition("DDOpenWiths", exportForSetup: true, new DDFieldDefinition[23]
		{
			new DDFieldDefinition("dwID", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dwTable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("dwField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dwExtension", "nvarchar(10)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dwDesc", "nvarchar(40)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dwCaptionExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dwCaptionExpressionUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dwType", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dwSequence", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dwButtonImage", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dwButtonImageUser", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("dwHide", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dwUHide", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dwCode", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Code),
			new DDFieldDefinition("dwObject", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.ObjectID),
			new DDFieldDefinition("dwActionName", "nvarchar(100)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dwEnabledExpression", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Expression),
			new DDFieldDefinition("dwEnabledExpressionUser", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Expression),
			new DDFieldDefinition("dwSaveBefore", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dwBindReadOnly", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dwPromptField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("dwAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID),
			new DDFieldDefinition("dwCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None)
		}, new DDIndexDefinition[6]
		{
			new DDIndexDefinition("DWID", unique: true),
			new DDIndexDefinition("DWSEQUENCE", unique: false),
			new DDIndexDefinition("DWTABLE", unique: false),
			new DDIndexDefinition("DWTYPE", unique: false),
			new DDIndexDefinition("DWFIELD", unique: false),
			new DDIndexDefinition("DWOBJECT", unique: false)
		}, new string[1] { "dwID" }, new string[1] { "dwDesc,dwID" }, new string[1] { "OpenWith" }, new string[1] { "dwID" }));
		Tables.Add(new DDTableDefinition("DDFieldUserSettings", exportForSetup: false, new DDFieldDefinition[5]
		{
			new DDFieldDefinition("datable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.Table),
			new DDFieldDefinition("daField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.Field),
			new DDFieldDefinition("daUser", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("daDefault", "nvarchar(60)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("daPrevious", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None)
		}, new DDIndexDefinition[4]
		{
			new DDIndexDefinition("DATABLE,DAFIELD,DAUSER", unique: true),
			new DDIndexDefinition("DAUSER", unique: false),
			new DDIndexDefinition("DATABLE", unique: false),
			new DDIndexDefinition("DAFIELD", unique: false)
		}, null, null, null, null));
		Tables.Add(new DDTableDefinition("DDCustomModules", exportForSetup: true, new DDFieldDefinition[3]
		{
			new DDFieldDefinition("dcCustomID", "smallint", nullable: false, "0", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dcCaption", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dcAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[1]
		{
			new DDIndexDefinition("DCCUSTOMID", unique: true)
		}, new string[1] { "dcCustomID" }, new string[1] { "dcCustomID" }, new string[1] { "Custom Module" }, null));
		Tables.Add(new DDTableDefinition("DDModules", exportForSetup: true, new DDFieldDefinition[10]
		{
			new DDFieldDefinition("ddmModuleID", "nvarchar(2)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("ddmCaption", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddmPropertiesTable", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("ddmSecurityTables", "nvarchar(100)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("ddmSecurityModules", "nvarchar(10)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddmVirtual", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddmPropertiesFieldName", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("ddmPropertiesFieldValue", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("ddmCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("ddmAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[1]
		{
			new DDIndexDefinition("ddmModuleID", unique: true)
		}, new string[1] { "ddmModuleID" }, new string[1] { "ddmModuleID" }, new string[1] { "Module" }, null));
		Tables.Add(new DDTableDefinition("DDSolutions", exportForSetup: true, new DDFieldDefinition[4]
		{
			new DDFieldDefinition("dnSolutionID", "nvarchar(75)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dnName", "nvarchar(75)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dnAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID),
			new DDFieldDefinition("dnCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None)
		}, new DDIndexDefinition[1]
		{
			new DDIndexDefinition("dnSolutionID", unique: true)
		}, new string[1] { "dnSolutionID" }, new string[1] { "dnSolutionID" }, new string[1] { "Solution" }, new string[1] { "dnSolutionID" }));
		Tables.Add(new DDTableDefinition("DDSolutionDetails", exportForSetup: true, new DDFieldDefinition[7]
		{
			new DDFieldDefinition("diSolutionID", "nvarchar(75)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("diParentID", "uniqueidentifier", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diLineID", "uniqueidentifier", nullable: true, "NEWID()", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diName", "nvarchar(75)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diType", "nvarchar(75)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID),
			new DDFieldDefinition("diCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None)
		}, new DDIndexDefinition[2]
		{
			new DDIndexDefinition("diLineID", unique: true),
			new DDIndexDefinition("diSolutionID", unique: false)
		}, new string[1] { "diLineID" }, new string[2] { "diSolutionID", "diName" }, new string[2] { "Solution", "Name" }, new string[2] { "diSolutionID", "diName" }));
		Tables.Add(new DDTableDefinition("DDScripts", exportForSetup: true, new DDFieldDefinition[4]
		{
			new DDFieldDefinition("dyName", "nvarchar(75)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dyAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID),
			new DDFieldDefinition("dyUniqueID", "uniqueidentifier", nullable: false, "NEWID()", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dyCustom", "bit", nullable: false, "0", DDFieldFlag.CustomFilterField, DDFieldContentType.None)
		}, new DDIndexDefinition[2]
		{
			new DDIndexDefinition("dyName", unique: true),
			new DDIndexDefinition("dyUniqueID", unique: true)
		}, new string[1] { "dyName" }, new string[1] { "dyName" }, new string[1] { "Scripts" }, new string[1] { "dyName" }));
		Tables.Add(new DDTableDefinition("DDVisualizers", exportForSetup: true, new DDFieldDefinition[13]
		{
			new DDFieldDefinition("dvVisualizerID", "nvarchar(35)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dvUserID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dvType", "tinyint", nullable: false, "0", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dvVisualizerName", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dvShowTitle", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dvShowLegend", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dvShowValues", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dvChartDisplayType", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dvChartGroupType", "tinyint", nullable: false, "3", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dvChartRangeType", "tinyint", nullable: false, "5", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dvMinimumPercent", "numeric(5,2)", nullable: false, "5", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("dvAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID),
			new DDFieldDefinition("dvCustom", "bit", nullable: false, "1", DDFieldFlag.CustomFilterField, DDFieldContentType.None)
		}, new DDIndexDefinition[3]
		{
			new DDIndexDefinition("dvType,dvVisualizerID,dvUserID", unique: true),
			new DDIndexDefinition("dvVisualizerID", unique: false),
			new DDIndexDefinition("dvUserID", unique: false)
		}, new string[3] { "dvVisualizerID", "dvType", "dvUserID" }, new string[1] { "dvVisualizerID" }, new string[1] { "Visualizer" }, null));
		Tables.Add(new DDTableDefinition("DDSeries", exportForSetup: true, new DDFieldDefinition[22]
		{
			new DDFieldDefinition("diVisualizerID", "nvarchar(35)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("diUserID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("diType", "tinyint", nullable: false, "0", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("diSeriesID", "int", nullable: false, "0", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("diSeriesName", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diGridID", "nvarchar(35)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.GridID),
			new DDFieldDefinition("diSeriesColor", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diNegative", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diTotal", "bit", nullable: false, "1", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diExpanded", "bit", nullable: false, "1", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diAdditionalFilter", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Filter),
			new DDFieldDefinition("diAdditionalFilterSettings", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diTotalField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("diDateField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("diLocationField", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("diGroupFields", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("diDateOffsetType", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diDateOffsetAmount", "tinyint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diMapPointGroupBy", "int", nullable: false, "18", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diMapPointMapType", "int", nullable: false, "4", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID),
			new DDFieldDefinition("diCustom", "bit", nullable: false, "1", DDFieldFlag.CustomFilterField, DDFieldContentType.None)
		}, new DDIndexDefinition[4]
		{
			new DDIndexDefinition("diType, diVisualizerID, diSeriesID, diUserID", unique: true),
			new DDIndexDefinition("diVisualizerID", unique: false),
			new DDIndexDefinition("diSeriesID", unique: false),
			new DDIndexDefinition("diUserID", unique: false)
		}, new string[4] { "diVisualizerID", "diSeriesId", "diUserID", "diType" }, new string[2] { "diVisualizerID", "diSeriesId" }, new string[2] { "Visualizer", "Series" }, null));
		Tables.Add(new DDTableDefinition("WebBarcodes", exportForSetup: true, new DDFieldDefinition[8]
		{
			new DDFieldDefinition("wbKey", "nvarchar(5)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("wbModule", "nvarchar(5)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("wbDescription", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wbOnScan", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wbOnScanCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wbCustom", "bit", nullable: false, "1", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("wbParameterList", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wbAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[1]
		{
			new DDIndexDefinition("wbKey,wbmodule", unique: true)
		}, new string[2] { "wbKey", "wbModule" }, new string[1] { "wbDescription" }, new string[1] { "Web Barcode" }, null));
		Tables.Add(new DDTableDefinition("WebControls", exportForSetup: true, new DDFieldDefinition[18]
		{
			new DDFieldDefinition("wcScreenID", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("wcControlID", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("wcSequence", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wcControlType", "nvarchar(1)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wcControlName", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wcFieldID", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wcListID", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wcOnClick", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Code),
			new DDFieldDefinition("wcOnChange", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Code),
			new DDFieldDefinition("wcReadOnly", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wcParameters", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wcSequenceCustom", "int", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wcOnClickCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Code),
			new DDFieldDefinition("wcOnChangeCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Code),
			new DDFieldDefinition("wcHiddenCustom", "bit", nullable: false, "1", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wcCustom", "bit", nullable: false, "1", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("wcModule", "nvarchar(5)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("wcAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[1]
		{
			new DDIndexDefinition("wcScreenID,wcControlID,wcmodule", unique: true)
		}, new string[3] { "wcScreenID", "wcControlID", "wcModule" }, new string[2] { "wcScreenID", "wcControlID" }, new string[2] { "Web Screen", "Web Control" }, null));
		Tables.Add(new DDTableDefinition("WebLists", exportForSetup: true, new DDFieldDefinition[30]
		{
			new DDFieldDefinition("wlListID", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("wlModule", "nvarchar(5)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("wlListName", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wlListNameCustom", "nvarchar(50)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("wlSequence", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wlSequenceCustom", "int", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("wlFieldList", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wlFieldListCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("wlFilterFieldList", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wlFilterFieldListCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("wlOnItemClick", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Code),
			new DDFieldDefinition("wlOnItemClickCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Code),
			new DDFieldDefinition("wlSmallLayout", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wlSmallLayoutCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("wlLargeLayout", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wlLargeLayoutCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("wlCustom", "bit", nullable: false, "1", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("wlFieldClause", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("wlFieldClauseCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Field),
			new DDFieldDefinition("wlFromClause", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("wlFromClauseCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Table),
			new DDFieldDefinition("wlWhereClause", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Filter),
			new DDFieldDefinition("wlWhereClauseCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Filter),
			new DDFieldDefinition("wlGroupByClause", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("wlGroupByClauseCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Field),
			new DDFieldDefinition("wlOrderByClause", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("wlOrderByClauseCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Field),
			new DDFieldDefinition("wlParameterList", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wlIsIndexed", "bit", nullable: false, "1", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wlAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[1]
		{
			new DDIndexDefinition("wlListID,wlModule", unique: true)
		}, new string[2] { "wlListID", "wlModule" }, new string[1] { "wlListID" }, new string[1] { "Web List" }, null));
		Tables.Add(new DDTableDefinition("WebScreens", exportForSetup: true, new DDFieldDefinition[23]
		{
			new DDFieldDefinition("wsScreenID", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("wsModule", "nvarchar(5)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("wsScreenName", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wsScreenNameCustom", "nvarchar(50)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("wsSequence", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wsSequenceCustom", "int", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("wsFieldList", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wsFieldListCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("wsOnDataLoad", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wsOnDataLoadCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("wsCustom", "bit", nullable: false, "1", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("wsFieldClause", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("wsFieldClauseCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Field),
			new DDFieldDefinition("wsFromClause", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Table),
			new DDFieldDefinition("wsFromClauseCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Table),
			new DDFieldDefinition("wsWhereClause", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Filter),
			new DDFieldDefinition("wsWhereClauseCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Filter),
			new DDFieldDefinition("wsGroupByClause", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("wsGroupByClauseCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Field),
			new DDFieldDefinition("wsOrderByClause", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Field),
			new DDFieldDefinition("wsOrderByClauseCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.Field),
			new DDFieldDefinition("wsParameterList", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wsAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[1]
		{
			new DDIndexDefinition("wsScreenID,wsModule", unique: true)
		}, new string[2] { "wsScreenID", "wsModule" }, new string[1] { "wsScreenID" }, new string[1] { "Web Screen" }, null));
		Tables.Add(new DDTableDefinition("WebOptions", exportForSetup: true, new DDFieldDefinition[13]
		{
			new DDFieldDefinition("woOptionID", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("woModule", "nvarchar(5)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("woOptionName", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("woSequence", "int", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("woOptionDescription", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("woType", "nvarchar(1)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("woValue", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("woValueList", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("woEditOnWeb", "bit", nullable: false, "1", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("woCustom", "bit", nullable: false, "1", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("woValueCustom", "nvarchar(50)", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("woEditOnWebCustom", "bit", nullable: true, "", DDFieldFlag.Custom, DDFieldContentType.None),
			new DDFieldDefinition("woAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[1]
		{
			new DDIndexDefinition("woOptionID,woModule", unique: true)
		}, new string[2] { "woOptionID", "woModule" }, new string[1] { "woOptionID" }, new string[1] { "Web Option" }, null));
		Tables.Add(new DDTableDefinition("WebServerFunctions", exportForSetup: true, new DDFieldDefinition[7]
		{
			new DDFieldDefinition("wfFunctionID", "nvarchar(30)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("wfCodeCustom", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.Code),
			new DDFieldDefinition("wfModule", "nvarchar(5)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wfCustom", "bit", nullable: false, "1", DDFieldFlag.CustomFilterField, DDFieldContentType.None),
			new DDFieldDefinition("wfParameterList", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wfReturnType", "nvarchar(15)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("wfAppExtensionID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.AppExtensionID)
		}, new DDIndexDefinition[1]
		{
			new DDIndexDefinition("wfFunctionID", unique: true)
		}, new string[1] { "wfFunctionID" }, new string[1] { "wfFunctionID" }, new string[1] { "Web Server Function" }, null));
		Tables.Add(new DDTableDefinition("WebSessions", exportForSetup: false, new DDFieldDefinition[10]
		{
			new DDFieldDefinition("weSessionID", "uniqueIdentifier", nullable: false, "NEWID()", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("weModule", "nvarchar(5)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("weDataset", "nvarchar(25)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("weUserID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("weWebUserID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("weDateCreated", "datetime", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("weDateLastUsed", "datetime", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("weActive", "bit", nullable: false, "1", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("weParentSessionID", "uniqueIdentifier", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("weExpirationTime", "int", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None)
		}, new DDIndexDefinition[1]
		{
			new DDIndexDefinition("weSessionID", unique: true)
		}, null, null, null, null));
		Tables.Add(new DDTableDefinition("DDObjectsUser", exportForSetup: false, new DDFieldDefinition[3]
		{
			new DDFieldDefinition("doObjectID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.ObjectID),
			new DDFieldDefinition("doUserID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("doOther", "nvarchar(max)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None)
		}, new DDIndexDefinition[1]
		{
			new DDIndexDefinition("doObjectID, doUserID", unique: true)
		}, null, null, null, null));
		Tables.Add(new DDTableDefinition("DDObjectDetailsUser", exportForSetup: false, new DDFieldDefinition[4]
		{
			new DDFieldDefinition("dlObjectID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.ObjectID),
			new DDFieldDefinition("dlLine", "tinyint", nullable: false, "0", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dlUserID", "nvarchar(20)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("dlCollapseUser", "smallint", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None)
		}, new DDIndexDefinition[1]
		{
			new DDIndexDefinition("dlObjectID,dlLine,dlUserID", unique: true)
		}, null, null, null, null));
		Tables.Add(new DDTableDefinition("DDAPIInfo", exportForSetup: false, new DDFieldDefinition[9]
		{
			new DDFieldDefinition("daModuleID", "nchar(5)", nullable: false, "''", DDFieldFlag.Key, DDFieldContentType.None),
			new DDFieldDefinition("daAPIID", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("daAPIKey", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("daDatabaseID", "nvarchar(25)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("daAPIUserID", "nvarchar(25)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("daAPIUserPWD", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("daExtraSettings", "nvarchar(max)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("daRemarks", "nvarchar(50)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("daIsReadOnly", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None)
		}, new DDIndexDefinition[1]
		{
			new DDIndexDefinition("daAPIID,daModuleID", unique: true)
		}, null, null, null, null));
		Tables.Add(new DDTableDefinition("IntegrationServiceInfo", exportForSetup: false, new DDFieldDefinition[8]
		{
			new DDFieldDefinition("diIntegrationType", "nvarchar(25)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diUsername", "nvarchar(256)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diPassword", "nvarchar(256)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diDatabaseId", "nvarchar(25)", nullable: false, "''", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diPollingFrequency", "smallint", nullable: false, "5", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diInactive", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diTenantId", "nvarchar(36)", nullable: true, "", DDFieldFlag.Standard, DDFieldContentType.None),
			new DDFieldDefinition("diIsSynced", "bit", nullable: false, "0", DDFieldFlag.Standard, DDFieldContentType.None)
		}, new DDIndexDefinition[0], null, null, null, null));
	}
}
