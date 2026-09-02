using System.Collections.Generic;
using System.Data;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.108", "Convert quality registers to inspections", "2015-11-25")]
public class v900108d
{
	public v900108d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmPurchaseToJobQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmPurchaseToJobQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmPullFromStockQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmPullFromStockQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmPullAllFromStock"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmPullAllFromStock", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobAssemblies", "jmaOverlapSourceJobOperationID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobAssemblies", "jmaOverlapSourceJobOperationID", "jmaOverlapSourceOperationID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoOverlapJobOperationID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoOverlapJobOperationID", "jmoOverlapOperationID", dropTriggers: true);
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleTaskBuckets"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTaskBuckets", new DmoField[11]
			{
				new DmoField("sxeScheduleTreeID", "int", 4, 0, nullable: false),
				new DmoField("sxeScheduleBranchID", "int", 4, 0, nullable: false),
				new DmoField("sxeScheduleTaskID", "int", 4, 0, nullable: false),
				new DmoField("sxeScheduleTaskBucketID", "tinyint", 1, 0, nullable: false),
				new DmoField("sxeScheduleTypeID", "tinyint", 1, 0, nullable: false),
				new DmoField("sxeScheduleTypeBucketID", "tinyint", 1, 0, nullable: false),
				new DmoField("sxeHours", "numeric", 8, 2, nullable: false),
				new DmoField("sxePercentComplete", "smallint", 3, 0, nullable: false),
				new DmoField("sxeCompletedHours", "numeric", 8, 2, nullable: false),
				new DmoField("sxeCompleted", "bit", 1, 0, nullable: false),
				new DmoField("sxeUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[1]
			{
				new DmoIndex("sxeScheduleTreeID,sxeScheduleBranchID,sxeScheduleTaskID,sxeScheduleTaskBucketID", unique: true)
			});
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTaskBuckets", "sxeCompletedHours"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTaskBuckets", "sxeCompletedHours", "sxeCompletedMinutes", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTaskBuckets", "sxeCompletedMinutes"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTaskBuckets", "sxeCompletedMinutes", "int", 4, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTaskBuckets", "sxeHours"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTaskBuckets", "sxeHours", "sxeMinutes", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTaskBuckets", "sxeMinutes"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTaskBuckets", "sxeMinutes", "int", 4, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTrees", "sxtScheduleTreeID"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTrees", "sxtScheduleTreeID", "int", 4, 0, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTrees", "sxtType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTrees", "sxtType", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTrees", "sxtDescription"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTrees", "sxtDescription", "nvarchar", 30, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTrees", "sxtGroupUniqueID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTrees", "sxtGroupUniqueID", "uniqueidentifier", 16, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Inspections", "qapPosted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Inspections", "qapPosted", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Inspections", "qapReversalEntry"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Inspections", "qapReversalEntry", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalReversed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalReversed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalPosted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalPosted", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionComponents", "qamPosted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionComponents", "qamPosted", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalManualInspectionFinalized"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalManualInspectionFinalized", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionComponents", "qamManualInspectionFinalized"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionComponents", "qamManualInspectionFinalized", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "QualityRegisters") && parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalQualityRegisterID"))
		{
			Inspection inspection = new Inspection();
			DataTable customFieldsbyTable = inspection.GetCustomFieldsbyTable(parms.Database);
			inspection.ConvertQualityRegistersToInspections(customFields: CreateCustomFields(parms, customFieldsbyTable, "uqal", "InspectionLines"), database: parms.Database, transaction: null);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralQualityRegisterID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralQualityRegisterID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumberTransactions", "abtQualityRegisterID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumberTransactions", "abtQualityRegisterID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumberTransactions", "sntQualityRegisterID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumberTransactions", "sntQualityRegisterID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAReceiptLines", "rrlQualityRegisterID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAReceiptLines", "rrlQualityRegisterID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Attachments", "cmaQualityRegisterID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Attachments", "cmaQualityRegisterID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalQualityRegisterID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalQualityRegisterID", dropTriggers: true);
		}
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "QualityRegisters"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "QualityRegisters");
		}
	}

	public static List<KeyValuePair<string, string>> CreateCustomFields(DBConversionParms parms, DataTable customFields, string targetPrefix, string targetTable)
	{
		List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
		foreach (DataRow row in customFields.Rows)
		{
			string text = row.Field<string>("COLUMN_NAME");
			string oldValue = row.Field<string>("PREFIX");
			KeyValuePair<string, string> item = new KeyValuePair<string, string>(text, text.Replace(oldValue, targetPrefix));
			string text2 = row.Field<string>("DATA_TYPE").ToLower();
			text2 = ((text2.Contains("varchar") && row.Field<int>("LENGTH") == -1) ? "nvarchar(max)" : text2);
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, targetTable, item.Value))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, targetTable, item.Value, text2, row.Field<int>("LENGTH"), row.Field<int>("SCALE"), verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
			}
			list.Add(item);
		}
		return list;
	}
}
