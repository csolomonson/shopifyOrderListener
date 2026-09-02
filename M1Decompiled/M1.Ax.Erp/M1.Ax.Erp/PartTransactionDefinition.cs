using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class PartTransactionDefinition : FieldExtension
{
	public enum QuantityType : byte
	{
		None,
		OnHand,
		ToInspect,
		ToReturn,
		InTransit,
		ToReturnJob
	}

	public enum CostType : byte
	{
		Average = 1,
		Last,
		Standard,
		Actual
	}

	public enum MfgReceiptType : byte
	{
		MiscReceiptToJob = 1,
		MiscReceiptToInventory,
		MfgReceipt
	}

	public enum QuantityAdjustmentType : byte
	{
		QuantityOnHand = 1,
		BinTransfer
	}

	public enum InspectionType : byte
	{
		Inventory = 1,
		Job,
		MfgReceipt
	}

	public enum JobType : byte
	{
		Material = 1,
		Subcontract,
		Assembly
	}

	public enum CostingMethod : byte
	{
		Average = 1,
		Last,
		Standard,
		LIFO,
		FIFO
	}

	public string BinQuantityField = string.Empty;

	public string PartTransactionQuantityField = string.Empty;

	public QuantityType BinDetailQuantityType;

	private CostingMethod _costingMethod;

	private DataTable _partTransactions;

	private SqlDataAdapter _adapterPartTransactions;

	private bool? _isNonNettable;

	private bool _nonStocked;

	private bool _nonNettable;

	private FieldDefinition _jobField;

	private FieldDefinition _jobStatusField;

	protected FieldDefinition _binField;

	private string _parms = string.Empty;

	private string _sortString = string.Empty;

	private bool _reversalEntry;

	protected bool IsAlternate;

	private bool _allowNegativeQuantityOnHand;

	public PartTransactionDefinition()
	{
	}

	public PartTransactionDefinition(M1BindingSource bs, string qtyFieldName, string binFieldName)
	{
		PartBinField = binFieldName;
		_binField = bs.Fields[binFieldName];
		FieldName = qtyFieldName;
		base.Field = bs.Fields[qtyFieldName];
	}

	public void AddTransaction(M1Database database, DataRow sourceRow, DataRowVersion rowVersion, SqlTransaction transaction, bool backoutQty)
	{
		if (Parameters != null)
		{
			_parms = Parameters.ToUpper();
		}
		if (CheckParms(sourceRow, rowVersion, base.Field.Table.TableName, base.Field.Table.FieldPrefix, _parms))
		{
			return;
		}
		if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "ManualInspectionFinalized") && string.IsNullOrEmpty(sourceRow.Field<string>(base.Field.Table.FieldPrefix + "SourceTableName", rowVersion)))
		{
			bool flag = sourceRow.Field<bool>(base.Field.Table.FieldPrefix + "ManualInspectionFinalized", rowVersion);
			if (!flag || (flag && backoutQty && sourceRow.RowState != DataRowState.Deleted))
			{
				return;
			}
		}
		if (base.Field.BindingSource.Fields.Contains(base.Field.Table.FieldPrefix + "ReversalEntry"))
		{
			_reversalEntry = sourceRow.Field<bool>(base.Field.Table.FieldPrefix + "ReversalEntry", rowVersion);
		}
		else
		{
			try
			{
				if (base.Field.BindingSource.Fields[base.Field.Table.KeyFieldsArray[0]].RelatedTableGetDataRow(base.Field.Table.TopLevelKeyFields.Substring(0, 3) + "ReversalEntry", database, sourceRow, alwaysReturnValidRow: true, transaction).Table.Columns.Contains(base.Field.Table.TopLevelKeyFields.Substring(0, 3) + "ReversalEntry"))
				{
					_reversalEntry = base.Field.BindingSource.Fields[base.Field.Table.KeyFieldsArray[0]].RelatedTableGetDataRow(base.Field.Table.TopLevelKeyFields.Substring(0, 3) + "ReversalEntry", database, sourceRow, alwaysReturnValidRow: true, transaction).Field<bool>(base.Field.Table.TopLevelKeyFields.Substring(0, 3) + "ReversalEntry", rowVersion);
				}
			}
			catch
			{
			}
		}
		if (!_isNonNettable.HasValue)
		{
			_isNonNettable = Convert.ToBoolean(database.Props("DatasetProperties")["xadEnableNonNettable"]);
		}
		string text = sourceRow.Field<string>(_binField.RelatedFieldsAndCurrentFieldArray[0], rowVersion);
		string partRevisionID = sourceRow.Field<string>(_binField.RelatedFieldsAndCurrentFieldArray[1], rowVersion);
		string text2 = sourceRow.Field<string>(_binField.RelatedFieldsAndCurrentFieldArray[2], rowVersion);
		string text3 = sourceRow.Field<string>(_binField.FieldName, rowVersion);
		DateTime dateTime = ((!string.IsNullOrWhiteSpace(TransactionDateField)) ? sourceRow.Field<DateTime>(TransactionDateField, rowVersion) : ((!base.Field.Table.GetDocumentDate(database, sourceRow, transaction, rowVersion).HasValue) ? DateTime.Now : Convert.ToDateTime(base.Field.Table.GetDocumentDate(database, sourceRow, transaction, rowVersion))));
		decimal num = ((!backoutQty) ? Convert.ToDecimal(sourceRow[base.Field.FieldName, rowVersion]) : (-Convert.ToDecimal(sourceRow[base.Field.FieldName, rowVersion])));
		if (ReverseSign)
		{
			num *= -1m;
		}
		_costingMethod = (CostingMethod)database.Props("PN")["xapIMCostingMethod"];
		_sortString = ((_costingMethod == CostingMethod.LIFO) ? " DESC" : " ASC");
		_allowNegativeQuantityOnHand = (bool)database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
		SetNettableAndNonStockedStatus(database, transaction, text, partRevisionID, text2);
		List<PartCostsBase> partCosts = GetPartCosts(database, transaction, text, ref partRevisionID, text2, text3, num, backoutQty, dateTime, sourceRow, rowVersion);
		if (partCosts == null)
		{
			return;
		}
		if (_partTransactions == null)
		{
			_partTransactions = database.GetDataTable("Select * From PartTransactions Where 0=1", fillSchema: false, out _adapterPartTransactions, transaction);
		}
		DataRow dataRow = _partTransactions.NewRow().BlankRow();
		dataRow["imtPartID"] = text;
		dataRow["imtPartRevisionID"] = partRevisionID;
		dataRow["imtPartWarehouseLocationID"] = text2;
		dataRow["imtPartBinID"] = text3;
		dataRow["imtTableName"] = base.Field.Table.TableName;
		dataRow["imtTableUniqueID"] = sourceRow[base.Field.Table.UniqueField, rowVersion];
		dataRow["imtTransactionDate"] = dateTime;
		dataRow["imtTransactionType"] = TransactionType;
		dataRow["imtSource"] = Source;
		dataRow[PartTransactionQuantityField] = num;
		dataRow["imtJobType"] = 0;
		if (_jobField != null)
		{
			dataRow["imtJobID"] = sourceRow[_jobField.RelatedFieldsAndCurrentFieldArray[0], rowVersion];
			if (_jobField.RelatedFieldsAndCurrentFieldArray.Length > 1)
			{
				dataRow["imtJobAssemblyID"] = sourceRow[_jobField.RelatedFieldsAndCurrentFieldArray[1], rowVersion];
				if (_jobField.RelatedTable.Equals("JobMaterials", StringComparison.CurrentCultureIgnoreCase))
				{
					dataRow["imtJobMaterialID"] = sourceRow[_jobField.FieldName, rowVersion];
					dataRow["imtJobType"] = 1;
				}
				else if (_jobField.RelatedTable.Equals("JobOperations", StringComparison.CurrentCultureIgnoreCase))
				{
					dataRow["imtJobOperationID"] = sourceRow[_jobField.FieldName, rowVersion];
					dataRow["imtJobType"] = 2;
				}
				else if (_jobField.RelatedTable.Equals("JobAssemblies", StringComparison.CurrentCultureIgnoreCase))
				{
					dataRow["imtJobType"] = 3;
				}
				else if (_jobField.RelatedFieldsAndCurrentFieldArray.Length > 2 && _jobField.RelatedTable.Equals("JobMaterialComponents", StringComparison.CurrentCultureIgnoreCase))
				{
					dataRow["imtJobMaterialID"] = sourceRow[_jobField.RelatedFieldsAndCurrentFieldArray[2], rowVersion];
					dataRow["imtJobMaterialComponentID"] = sourceRow[_jobField.FieldName, rowVersion];
					dataRow["imtJobType"] = 1;
				}
			}
		}
		if (_jobStatusField != null)
		{
			if (backoutQty)
			{
				if (sourceRow.Field<bool>(_jobStatusField.RelatedFieldsAndCurrentFieldArray[0], rowVersion))
				{
					dataRow["imtJobCompleteStatus"] = 0;
				}
			}
			else
			{
				dataRow["imtJobCompleteStatus"] = sourceRow[_jobStatusField.RelatedFieldsAndCurrentFieldArray[0], rowVersion];
			}
		}
		dataRow["imtCOGSCalculatedDate"] = DateTime.Now;
		dataRow["imtCOGSPostedToGL"] = true;
		dataRow["imtPreviousQuantityOnHand"] = GetQuantityOnHand(database, transaction, text, partRevisionID, text2, text3, dateTime);
		if (_isNonNettable.Value)
		{
			dataRow["imtNonNettable"] = _nonNettable;
		}
		if (_nonStocked || _parms.Contains("NONINVENTORY") || _parms.Contains("NONINVJOB"))
		{
			dataRow["imtNonInventoryTransaction"] = true;
		}
		else
		{
			dataRow["imtNonInventoryTransaction"] = false;
		}
		if (base.Field.Table.GetDocumentPlantID(database, sourceRow, transaction, rowVersion) != null)
		{
			dataRow["imtPlantID"] = base.Field.Table.GetDocumentPlantID(database, sourceRow, transaction, rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "ProjectID"))
		{
			dataRow["imtProjectID"] = sourceRow.Field<string>(base.Field.Table.FieldPrefix + "ProjectID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "ProjectAreaID"))
		{
			dataRow["imtProjectAreaID"] = sourceRow.Field<string>(base.Field.Table.FieldPrefix + "ProjectAreaID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "Reference"))
		{
			dataRow["imtReference"] = sourceRow.Field<string>(base.Field.Table.FieldPrefix + "Reference", rowVersion);
		}
		else if (base.Field.Table.KeyFieldsArray.Length != 0)
		{
			dataRow["imtReference"] = $"{base.Field.Table.Module} {sourceRow[base.Field.Table.KeyFieldsArray[0], rowVersion]}";
		}
		dataRow["imtCreatedBy"] = database.User.ID;
		dataRow["imtCreatedDate"] = DateTime.Now;
		dataRow["imtPartTransactionID"] = database.ExecuteScalar("Select IsNull(Max(imtPartTransactionID),0)+1 From PartTransactions", transaction);
		_partTransactions.Rows.Add(dataRow);
		database.UpdateData(new DataRow[1] { dataRow }, _adapterPartTransactions, transaction);
		bool flag2 = false;
		if (_parms.Contains("CHECKFORADJUSTMENTTRANSACTIONS"))
		{
			flag2 = new Part().GetFutureAdjustmentTransactionStatus(database, transaction, text, partRevisionID, text2, text3, dateTime);
		}
		bool flag3 = false;
		if (!_nonStocked && !_parms.Contains("NONINVENTORY") && !flag2)
		{
			UpdateQuantity(database, transaction, text, partRevisionID, text2, text3, dataRow.Field<decimal>(PartTransactionQuantityField), dataRow.Field<string>("imtJobID"), dataRow.Field<int>("imtJobAssemblyID"), Convert.ToInt16(dataRow.Field<int>("imtJobMaterialID")), Convert.ToInt16(dataRow.Field<int>("imtJobOperationID")), Convert.ToInt16(dataRow.Field<int>("imtJobMaterialComponentID")), dataRow.Field<bool>("imtJobCompleteStatus"), dataRow);
			UpdateWarehouseAndBin(database, transaction, sourceRow, text, partRevisionID, num);
			flag3 = true;
		}
		DataTable dataTable = null;
		SqlDataAdapter adapter = null;
		DataTable dataTable2 = null;
		SqlDataAdapter adapter2 = null;
		bool flag4 = num < 0m;
		if (!_reversalEntry && !ReverseSign && !backoutQty && (!flag4 || !base.Field.Table.TableName.Equals("QuantityAdjustments", StringComparison.CurrentCultureIgnoreCase)))
		{
			int num2 = 1;
			foreach (PartCostsBase item in partCosts)
			{
				if (flag3 && !_allowNegativeQuantityOnHand && item.CostType == CostType.Actual && BinDetailQuantityType != QuantityType.None)
				{
					if (dataTable == null)
					{
						dataTable = database.GetDataTable("Select * From PartBinDetails Where 0=1", fillSchema: false, out adapter, transaction);
					}
					DataRow dataRow2 = dataTable.NewRow().BlankRow();
					dataRow2["imgPartID"] = text;
					dataRow2["imgPartRevisionID"] = partRevisionID;
					dataRow2["imgWarehouseID"] = text2;
					dataRow2["imgPartBinID"] = text3;
					dataRow2["imgTransactionDate"] = dateTime;
					dataRow2["imgQuantityType"] = BinDetailQuantityType;
					dataRow2["imgOriginalQuantity"] = item.Quantity;
					dataRow2["imgRemainingQuantity"] = item.Quantity;
					dataRow2["imgUnitLaborCost"] = item.ActualUnitLaborCost;
					dataRow2["imgUnitOverheadCost"] = item.ActualUnitOverheadCost;
					dataRow2["imgUnitMaterialCost"] = item.ActualUnitMaterialCost;
					dataRow2["imgUnitSubcontractCost"] = item.ActualUnitSubcontractCost;
					dataRow2["imgUnitDutyCost"] = item.ActualUnitDutyCost;
					dataRow2["imgUnitFreightCost"] = item.ActualUnitFreightCost;
					dataRow2["imgUnitMiscCost"] = item.ActualUnitMiscCost;
					dataRow2["imgSourceTableName"] = base.Field.Table.TableName;
					dataRow2["imgSourceTableUniqueID"] = sourceRow[base.Field.Table.UniqueField, rowVersion];
					dataRow2["imgCreatedBy"] = database.User.ID;
					dataRow2["imgCreatedDate"] = DateTime.Now;
					dataRow2["imgUniqueID"] = item.SourcePartBinDetailID;
					SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(Max(imgPartBinDetailID),0)+1 From PartBinDetails WHERE imgPartID = @PartID AND imgPartRevisionID = @RevisionID AND imgWarehouseID = @WarehouseID AND imgPartBinID = @BinID");
					sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.VarChar)).Value = text;
					sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.VarChar)).Value = partRevisionID;
					sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.VarChar)).Value = text2;
					sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.VarChar)).Value = text3;
					dataRow2["imgPartBinDetailID"] = database.ExecuteScalar(sqlCommand, transaction);
					dataTable.Rows.Add(dataRow2);
					database.UpdateData(new DataRow[1] { dataRow2 }, adapter, transaction);
				}
				if (dataTable2 == null)
				{
					dataTable2 = database.GetDataTable("Select * From PartTransactionCosts Where 0=1", fillSchema: false, out adapter2, transaction);
				}
				DataRow dataRow3 = AddPartTransactionCostsRecord(database, transaction, item, dataTable2, dataRow.Field<int>("imtPartTransactionID"), num2);
				dataRow3["intSourceTableUniqueID"] = item.SourcePartBinDetailID;
				dataTable2.Rows.Add(dataRow3);
				database.UpdateData(new DataRow[1] { dataRow3 }, adapter2, transaction);
				num2++;
			}
		}
		else
		{
			int num3 = 1;
			foreach (PartCostsBase item2 in partCosts)
			{
				if (item2.CostType == CostType.Actual && flag3 && !_allowNegativeQuantityOnHand)
				{
					SqlCommand sqlCommand2 = database.NewSqlCommand("Select imgPartID,imgPartRevisionID,imgWarehouseID,imgPartBinID,imgPartBinDetailID,imgRemainingQuantity,imgOriginalQuantity,imgCreatedBy From PartBinDetails WHERE imgUniqueID = @UniqueID");
					sqlCommand2.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = item2.SourcePartBinDetailID;
					DataTable dataTable3 = database.GetDataTable(sqlCommand2, fillSchema: false, out adapter, transaction);
					if (dataTable3.Rows.Count != 0)
					{
						DataRow row = dataTable3.Rows[0];
						decimal num4 = row.Field<decimal>("imgOriginalQuantity");
						decimal num5 = row.Field<decimal>("imgRemainingQuantity") + item2.Quantity;
						if (row.Field<string>("imgCreatedBy").Equals("CONVERSION", StringComparison.CurrentCultureIgnoreCase) && !backoutQty && !flag4 && num5 > num4)
						{
							row.SetField("imgOriginalQuantity", num5);
						}
						row.SetField("imgRemainingQuantity", num5);
						database.UpdateData(dataTable3, adapter, transaction);
					}
				}
				if (dataTable2 == null)
				{
					dataTable2 = database.GetDataTable("Select * From PartTransactionCosts Where 0=1", fillSchema: false, out adapter2, transaction);
				}
				DataRow dataRow4 = AddPartTransactionCostsRecord(database, transaction, item2, dataTable2, dataRow.Field<int>("imtPartTransactionID"), num3);
				dataTable2.Rows.Add(dataRow4);
				database.UpdateData(new DataRow[1] { dataRow4 }, adapter2, transaction);
				num3++;
			}
		}
		if (_parms.Contains("UPDATEPREVIOUSQTYONHAND"))
		{
			SqlCommand sqlCommand3 = database.NewSqlCommand("UPDATE PartTransactions SET imtPreviousQuantityOnHand = CASE WHEN imtSource <> 7 THEN imtPreviousQuantityOnHand + @QtyChanged ELSE imtPreviousQuantityOnHand END  , imtInventoryQuantityReceived = CASE WHEN imtTransactionType = 3 AND imtSource <> 7 THEN imtInventoryQuantityReceived - @QtyChanged WHEN imtTransactionType = 3 AND imtSource = 7 THEN imtInventoryQuantityReceived + @QtyChanged ELSE imtInventoryQuantityReceived END  FROM PartTransactions pt1 WHERE pt1.imtPartID = @PartID AND pt1.imtPartRevisionID = @RevisionID AND pt1.imtPartWarehouseLocationID = @WarehouseID AND pt1.imtPartBinID = @BinID  AND pt1.imtTransactionDate > @CountedDate AND pt1.imtTransactionDate <=  ISNULL((SELECT MIN(imtTransactionDate) FROM PartTransactions pt2 WHERE (pt2.imtTransactionType = 3 AND imtSource <> 7) AND pt2.imtPartID = @PartID AND pt2.imtPartRevisionID = @RevisionID AND pt2.imtPartWarehouseLocationID = @WarehouseID AND pt2.imtPartBinID = @BinID AND pt2.imtTransactionDate > @CountedDate),'20990101')");
			sqlCommand3.Parameters.Add(new SqlParameter("@PartID", SqlDbType.VarChar)).Value = text;
			sqlCommand3.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.VarChar)).Value = partRevisionID;
			sqlCommand3.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.VarChar)).Value = text2;
			sqlCommand3.Parameters.Add(new SqlParameter("@BinID", SqlDbType.VarChar)).Value = text3;
			sqlCommand3.Parameters.Add(new SqlParameter("@QtyChanged", SqlDbType.Decimal)).Value = dataRow.Field<decimal>(PartTransactionQuantityField);
			sqlCommand3.Parameters.Add(new SqlParameter("@CountedDate", SqlDbType.DateTime)).Value = dateTime;
			database.ExecuteScalar(sqlCommand3, transaction);
		}
		UpdateLastTransactionDate(database, transaction, text, partRevisionID, dateTime);
	}

	protected virtual void UpdateWarehouseAndBin(M1Database database, SqlTransaction transaction, DataRow row, string partID, string revisionID, decimal quantity)
	{
	}

	private DataRow AddPartTransactionCostsRecord(M1Database database, SqlTransaction transaction, PartCostsBase costs, DataTable partTransactionCosts, int partTransactionID, int partTransactionCostID)
	{
		DataRow dataRow = partTransactionCosts.NewRow().BlankRow();
		dataRow["intPartTransactionID"] = partTransactionID;
		dataRow["intPartTransactionCostID"] = partTransactionCostID;
		dataRow["intCostType"] = costs.CostType;
		dataRow["intQuantity"] = costs.Quantity;
		dataRow["intUnitLaborCost"] = costs.LaborCost;
		dataRow["intUnitOverheadCost"] = costs.OverheadCost;
		dataRow["intUnitMaterialCost"] = costs.MaterialCost;
		dataRow["intUnitSubcontractCost"] = costs.SubcontractCost;
		dataRow["intUnitDutyCost"] = costs.DutyCost;
		dataRow["intUnitFreightCost"] = costs.FreightCost;
		dataRow["intUnitMiscCost"] = costs.MiscCost;
		dataRow["intActualUnitLaborCost"] = costs.ActualUnitLaborCost;
		dataRow["intActualUnitOverheadCost"] = costs.ActualUnitOverheadCost;
		dataRow["intActualUnitMaterialCost"] = costs.ActualUnitMaterialCost;
		dataRow["intActualUnitSubcontractCost"] = costs.ActualUnitSubcontractCost;
		dataRow["intActualUnitDutyCost"] = costs.ActualUnitDutyCost;
		dataRow["intActualUnitFreightCost"] = costs.ActualUnitFreightCost;
		dataRow["intActualUnitMiscCost"] = costs.ActualUnitMiscCost;
		if (costs.SourcePartBinDetailID.HasValue)
		{
			dataRow["intSourceTableName"] = "PartBinDetails";
			dataRow["intSourceTableUniqueID"] = costs.SourcePartBinDetailID;
		}
		dataRow["intCreatedBy"] = database.User.ID;
		dataRow["intCreatedDate"] = DateTime.Now;
		return dataRow;
	}

	public bool CheckParms(DataRow sourceRow, DataRowVersion rowVersion, string sourceTableName, string fieldPrefix, string parms)
	{
		if (!parms.Contains("IGNOREPOSTED"))
		{
			if (sourceRow.Table.Columns.Contains(fieldPrefix + "Posted") && sourceRow.Field<bool>(fieldPrefix + "Posted", rowVersion).Equals(obj: false))
			{
				return true;
			}
			if (sourceRow.Table.Columns.Contains(fieldPrefix + "PostedToGL") && sourceRow.Field<bool>(fieldPrefix + "PostedToGL", rowVersion).Equals(obj: false))
			{
				return true;
			}
		}
		else
		{
			if (sourceRow.Table.Columns.Contains(fieldPrefix + "Posted") && sourceRow.Field<bool>(fieldPrefix + "Posted", rowVersion).Equals(obj: true))
			{
				return true;
			}
			if (sourceRow.Table.Columns.Contains(fieldPrefix + "PostedToGL") && sourceRow.Field<bool>(fieldPrefix + "PostedToGL", rowVersion).Equals(obj: true))
			{
				return true;
			}
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "KitPart") && parms.Contains("CHECKFORKITPART") && sourceRow.Field<bool>(fieldPrefix + "KitPart", rowVersion).Equals(obj: true))
		{
			return true;
		}
		if (parms.Contains("CHECKFORINSP") && sourceRow.Table.Columns.Contains(fieldPrefix + "InspectionID") && !string.IsNullOrWhiteSpace(sourceRow.Field<string>(fieldPrefix + "InspectionID", rowVersion)))
		{
			return true;
		}
		if (parms.Contains("CHECKFORJOB") && sourceRow.Table.Columns.Contains(fieldPrefix + "JobID") && !string.IsNullOrWhiteSpace(sourceRow.Field<string>(fieldPrefix + "JobID", rowVersion)))
		{
			return true;
		}
		if (parms.Contains("CHECKFORSOURCE") && sourceTableName.Equals("INSPECTIONLINES", StringComparison.CurrentCultureIgnoreCase) && sourceRow.Table.Columns.Contains(fieldPrefix + "SourceTableName") && sourceRow.Table.Columns.Contains(fieldPrefix + "InspectionType"))
		{
			if (!string.IsNullOrWhiteSpace(sourceRow.Field<string>(fieldPrefix + "SourceTableName", rowVersion)))
			{
				return true;
			}
			if (sourceRow.Field<InspectionType>(fieldPrefix + "InspectionType", rowVersion) != InspectionType.Inventory)
			{
				return true;
			}
		}
		if (parms.Contains("CHECKFORSOURCE") && sourceTableName.Equals("INSPECTIONCOMPONENTS", StringComparison.CurrentCultureIgnoreCase) && sourceRow.Table.Columns.Contains(fieldPrefix + "SourceTableName") && sourceRow.Table.Columns.Contains(fieldPrefix + "InspectionType"))
		{
			if (!string.IsNullOrWhiteSpace(sourceRow.Field<string>(fieldPrefix + "SourceTableName", rowVersion)))
			{
				return true;
			}
			if (sourceRow.Field<InspectionType>(fieldPrefix + "InspectionType", rowVersion) != InspectionType.Inventory)
			{
				return true;
			}
		}
		if (parms.Contains("CHECKFORSOURCE") && sourceTableName.Equals("SHIPMENTLINES", StringComparison.CurrentCultureIgnoreCase) && sourceRow.Table.Columns.Contains(fieldPrefix + "SourceTableName") && string.IsNullOrWhiteSpace(sourceRow.Field<string>(fieldPrefix + "SourceTableName", rowVersion)))
		{
			return true;
		}
		if (parms.Contains("CHECKFORSOURCE") && (sourceTableName.Equals("WAREHOUSERECEIPTLINES", StringComparison.CurrentCultureIgnoreCase) || sourceTableName.Equals("WAREHOUSERECEIPTCOMPONENTS", StringComparison.CurrentCultureIgnoreCase)) && sourceRow.Table.Columns.Contains(fieldPrefix + "SourceTableName") && string.IsNullOrWhiteSpace(sourceRow.Field<string>(fieldPrefix + "SourceTableName", rowVersion)))
		{
			return true;
		}
		if (parms.Contains("CHECKCOMPONENT") && sourceTableName.Trim().ToUpper().Contains("COMPONENTS"))
		{
			return true;
		}
		return false;
	}

	public override void LoadComplete(FieldCollection fields, bool allowEditing)
	{
		if (PartBinField.Length != 0 && TransactionType != 0 && base.Field.DataDictionary != null && allowEditing)
		{
			base.LoadComplete(fields, add: true);
			if (_jobField == null && RelatedJobField.Length != 0)
			{
				_jobField = base.Field.BindingSource.Fields[RelatedJobField];
			}
			if (_jobStatusField == null && RelatedJobStatusField.Length != 0)
			{
				_jobStatusField = base.Field.BindingSource.Fields[RelatedJobStatusField];
			}
			_binField = base.Field.BindingSource.Fields[PartBinField];
			base.Field.BindingSource.RowUpdateAddAfter += BindingSource_RowUpdateAddAfter;
			base.Field.BindingSource.RowUpdateSaveAfter += BindingSource_RowUpdateSaveAfter;
			base.Field.BindingSource.RowUpdateDeleteBefore += BindingSource_RowUpdateDeleteBefore;
		}
		else
		{
			base.LoadComplete(fields, add: false);
		}
	}

	protected virtual void UpdateQuantity(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, decimal qtyChange, string jobID, int asmID, short jobMatID, short jobOpID, short compID, bool jobCompleteStatus, DataRow partTransactionRow)
	{
		if (string.IsNullOrWhiteSpace(BinQuantityField) || UpdateQtyInPartBins(database, transaction, partID, revisionID, warehouseID, binID, qtyChange) != 0)
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("Select imrPartRevisionID From PartRevisions Where imrPartID = @Part And imrPartRevisionID = @Revision");
		sqlCommand.Parameters.Add(new SqlParameter("@Part", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@Revision", SqlDbType.NVarChar)).Value = revisionID;
		if (database.ExecuteScalar(sqlCommand, transaction) != null && !string.IsNullOrWhiteSpace(warehouseID))
		{
			SqlCommand sqlCommand2 = database.NewSqlCommand("INSERT INTO PartWarehouseLocations (imlPartID,imlPartRevisionID,imlPartWarehouseID) SELECT imrPartID,imrPartRevisionID,@WarehouseID FROM PartRevisions WHERE imrPartID = @PartID And imrPartRevisionID = @RevisionID And imrPartID+imrPartRevisionID NOT IN (SELECT imlPartID+imlPartRevisionID FROM PartWarehouseLocations WHERE imlPartID = @PartID And imlPartRevisionID = @RevisionID And imlPartWarehouseID = @WarehouseID)");
			sqlCommand2.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand2.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
			sqlCommand2.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
			database.ExecuteCommand(sqlCommand2, transaction);
			if (!string.IsNullOrWhiteSpace(binID))
			{
				sqlCommand2 = database.NewSqlCommand("INSERT INTO PartBins (imbPartID,imbPartRevisionID,imbWarehouseID,imbPartBinID,imbConversionFactor) VALUES (@PartID,@RevisionID,@WarehouseID,@BinID,1)");
				sqlCommand2.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				sqlCommand2.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
				sqlCommand2.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
				sqlCommand2.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
				database.ExecuteCommand(sqlCommand2, transaction);
			}
		}
	}

	protected int UpdateQtyInPartBins(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, decimal qtyChange)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("UPDATE PartBins SET " + BinQuantityField + " = " + BinQuantityField + " + @QtyChange WHERE imbPartID = @PartID And imbPartRevisionID = @RevisionID AND imbWarehouseID = @WarehouseID AND imbPartBinID = @BinID");
		sqlCommand.Parameters.Add(new SqlParameter("@QtyChange", SqlDbType.Decimal)).Value = qtyChange;
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
		return database.ExecuteCommand(sqlCommand, transaction);
	}

	private void UpdateLastTransactionDate(M1Database database, SqlTransaction transaction, string partID, string revisionID, DateTime transactionDate)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("UPDATE PartRevisions SET imrLastTransactionDate=@imrTransactionDate Where imrPartID = @PartID And imrPartRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@imrTransactionDate", SqlDbType.DateTime)).Value = transactionDate;
		database.ExecuteCommand(sqlCommand, transaction);
	}

	protected virtual void AddCurrentValues(RowUpdateEventArgs e)
	{
		string value = e.Row.Field<string>(_binField.RelatedFieldsAndCurrentFieldArray[0]);
		decimal num = Convert.ToDecimal(e.Row[base.Field.FieldName]);
		if (!string.IsNullOrWhiteSpace(value) && num != 0m)
		{
			AddTransaction(e.Database, e.Row, DataRowVersion.Current, e.SqlTransaction, backoutQty: false);
		}
	}

	protected virtual void RemoveOriginalValues(RowUpdateEventArgs e)
	{
		string value = e.Row.Field<string>(_binField.RelatedFieldsAndCurrentFieldArray[0], DataRowVersion.Original);
		decimal num = Convert.ToDecimal(e.Row[base.Field.FieldName, DataRowVersion.Original]);
		if (!string.IsNullOrWhiteSpace(value) && num != 0m)
		{
			if (!base.Field.FieldName.Equals("qamComponentQtyToInspect", StringComparison.CurrentCultureIgnoreCase) && !base.Field.FieldName.Equals("qalQuantityToInspect", StringComparison.CurrentCultureIgnoreCase))
			{
				AddTransaction(e.Database, e.Row, DataRowVersion.Original, e.SqlTransaction, backoutQty: true);
			}
			else if ((e.Row.HasVersion(DataRowVersion.Current) && num != Convert.ToDecimal(e.Row[base.Field.FieldName, DataRowVersion.Current])) || e.Row.RowState == DataRowState.Deleted)
			{
				AddTransaction(e.Database, e.Row, DataRowVersion.Original, e.SqlTransaction, backoutQty: true);
			}
		}
	}

	private void BindingSource_RowUpdateDeleteBefore(object sender, RowUpdateEventArgs e)
	{
		RemoveOriginalValues(e);
	}

	private void BindingSource_RowUpdateSaveAfter(object sender, RowUpdateEventArgs e)
	{
		SaveCheck(e);
	}

	protected virtual void SaveCheck(RowUpdateEventArgs e)
	{
		if (isRowChanged(e.Row))
		{
			if (IsAlternate)
			{
				RemoveOriginalValues(e);
			}
			AddCurrentValues(e);
		}
	}

	private void BindingSource_RowUpdateAddAfter(object sender, RowUpdateEventArgs e)
	{
		AddCurrentValues(e);
	}

	private bool CheckForQuantityIssued(M1Database database, DataRow row, DataRowVersion rowVersion, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT COUNT(*) FROM PartBinDetails WHERE imgSourceTableUniqueID = @UniqueID AND imgSourceTableName = @Table AND imgRemainingQuantity <> imgOriginalQuantity AND imgRemainingQuantity <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = row[base.Field.Table.UniqueField, rowVersion];
		sqlCommand.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = base.Field.Table.TableName;
		object obj = database.ExecuteScalar(sqlCommand, transaction);
		if (obj != null)
		{
			return !(Convert.ToDecimal(obj) == 0m);
		}
		return false;
	}

	protected virtual bool isRowChanged(DataRow row)
	{
		string[] relatedFieldsAndCurrentFieldArray = _binField.RelatedFieldsAndCurrentFieldArray;
		foreach (string columnName in relatedFieldsAndCurrentFieldArray)
		{
			if (!row.Field<string>(columnName).Trim().Equals(row.Field<string>(columnName, DataRowVersion.Original).Trim(), StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
		}
		if (!row.Field<decimal>(FieldName).Equals(row.Field<decimal>(FieldName, DataRowVersion.Original)))
		{
			return true;
		}
		if (_jobField != null)
		{
			relatedFieldsAndCurrentFieldArray = _jobField.RelatedFieldsAndCurrentFieldArray;
			foreach (string columnName2 in relatedFieldsAndCurrentFieldArray)
			{
				if (!row[columnName2].ToString().Equals(row[columnName2, DataRowVersion.Original].ToString(), StringComparison.CurrentCultureIgnoreCase))
				{
					return true;
				}
			}
		}
		if (_jobStatusField != null && !row.Field<bool>(_jobStatusField.FieldName).Equals(row.Field<bool>(_jobStatusField.FieldName, DataRowVersion.Original)))
		{
			return true;
		}
		if (!string.IsNullOrWhiteSpace(base.Field.Table.DocumentPlantIdField) && !row.Field<string>(base.Field.Table.DocumentPlantIdField).Equals(row.Field<string>(base.Field.Table.DocumentPlantIdField, DataRowVersion.Original)))
		{
			return true;
		}
		if (row.Table.Columns.Contains(base.Field.Table.FieldPrefix + "Posted") && !row.Field<bool>(base.Field.Table.FieldPrefix + "Posted").Equals(row.Field<bool>(base.Field.Table.FieldPrefix + "Posted", DataRowVersion.Original)) && !_parms.Contains("IGNOREPOSTED"))
		{
			return true;
		}
		if (row.Table.Columns.Contains(base.Field.Table.FieldPrefix + "ManualInspectionFinalized") && !row.Field<bool>(base.Field.Table.FieldPrefix + "ManualInspectionFinalized").Equals(row.Field<bool>(base.Field.Table.FieldPrefix + "ManualInspectionFinalized", DataRowVersion.Original)))
		{
			return true;
		}
		if (row.Table.Columns.Contains(base.Field.Table.FieldPrefix + "PostedToGL") && !row.Field<bool>(base.Field.Table.FieldPrefix + "PostedToGL").Equals(row.Field<bool>(base.Field.Table.FieldPrefix + "PostedToGL", DataRowVersion.Original)) && !_parms.Contains("IGNOREPOSTED"))
		{
			return true;
		}
		if (row.Table.Columns.Contains(base.Field.Table.FieldPrefix + "DeliveryType") && !row.Field<byte>(base.Field.Table.FieldPrefix + "DeliveryType").Equals(row.Field<byte>(base.Field.Table.FieldPrefix + "DeliveryType", DataRowVersion.Original)))
		{
			return true;
		}
		return false;
	}

	private decimal GetQuantityOnHand(M1Database database, SqlTransaction transaction, string partID, string partRevisionID, string warehouseID, string binID, DateTime transactionDate)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select isnull(imbQuantityOnHand,imrQuantityOnHand) As imbQuantityOnHand,imrLastTransactionDate From PartRevisions Left Outer Join PartBins On imbPartID = imrPartID And imbPartRevisionID = imrPartRevisionID And imbWarehouseID = @WarehouseID AND imbPartBinID = @BinID WHERE imrPartID = @PartID And imrPartRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			if (dataRow["imrLastTransactionDate"] == DBNull.Value || dataRow.Field<DateTime>("imrLastTransactionDate").CompareTo(transactionDate) <= 0)
			{
				return dataRow.Field<decimal>("imbQuantityOnHand");
			}
		}
		sqlCommand = database.NewSqlCommand("SELECT TOP 1 CASE WHEN imtSource = 7 THEN imtInventoryQuantityReceived ELSE imtPreviousQuantityOnHand END AS imtPreviousQuantityOnHand FROM PartTransactions  WHERE imtPartID = @PartID AND imtPartRevisionID = @RevisionID AND imtPartWarehouseLocationID = @WarehouseID AND imtPartBinID = @BinID AND imtTransactionDate > @TransactionDate  ORDER BY imtTransactionDate ASC");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
		sqlCommand.Parameters.Add(new SqlParameter("@TransactionDate", SqlDbType.DateTime)).Value = transactionDate;
		object obj = database.ExecuteScalar(sqlCommand, transaction);
		if (obj != null)
		{
			return Convert.ToDecimal(obj);
		}
		return 0m;
	}

	private bool SetNettableAndNonStockedStatus(M1Database database, SqlTransaction transaction, string partID, string partRevisionID, string warehouseID)
	{
		partID = partID.Trim();
		if (partID.Length != 0)
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select Isnull(imlNonNettable,0) As imlNonNettable, impNonStockedItem From Parts Inner Join PartWarehouseLocations on impPartID = imlPartID Where imlPartID = @PartID and imlPartRevisionID = @RevisionID and imlPartWarehouseID = @WarehouseID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
			sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
			DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable != null && dataTable.Rows.Count != 0)
			{
				_nonNettable = dataTable.Rows[0].Field<bool>("imlNonNettable");
				_nonStocked = dataTable.Rows[0].Field<bool>("impNonStockedItem");
				return true;
			}
		}
		return false;
	}

	private List<PartCostsBase> GetPartCosts(M1Database database, SqlTransaction transaction, string partID, ref string partRevisionID, string warehouseID, string binID, decimal quantity, bool backoutQty, DateTime transactionDate, DataRow sourceRow, DataRowVersion rowVersion)
	{
		bool flag = quantity < 0m;
		List<PartCostsBase> list = new List<PartCostsBase>();
		Guid value = Guid.NewGuid();
		if (!backoutQty)
		{
			if (!_reversalEntry)
			{
				SqlCommand sqlCommand = database.NewSqlCommand("Select imrLastTransactionDate, imrLastLaborCost,imrLastOverheadCost,imrLastMaterialCost,imrLastSubcontractCost,imrLastDutyCost,imrLastFreightCost,imrLastMiscCost, imrStandardLaborCost,imrStandardOverheadCost,imrStandardMaterialCost,imrStandardSubcontractCost,imrStandardDutyCost,imrStandardFreightCost,imrStandardMiscCost, imrAverageLaborCost,imrAverageOverheadCost,imrAverageMaterialCost,imrAverageSubcontractCost,imrAverageDutyCost,imrAverageFreightCost,imrAverageMiscCost from PartRevisions Inner Join Parts On imrPartID = impPartID Left Outer Join PartWarehouseLocations On imlPartID = imrPartID And imlPartRevisionID = imrPartRevisionID And imlPartWarehouseID = @WarehouseID  where imrPartID = @PartID And imrPartRevisionID = @RevisionID");
				sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
				sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
				DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
				if (!ReverseSign && (!flag || !base.Field.Table.TableName.Equals("QuantityAdjustments", StringComparison.CurrentCultureIgnoreCase)))
				{
					if (dataTable.Rows.Count == 0 && GetLatestPartRevision(database, transaction, partID, ref partRevisionID))
					{
						sqlCommand.Parameters["@RevisionID"].Value = partRevisionID;
						dataTable = database.GetDataTable(sqlCommand, transaction);
					}
					if (dataTable.Rows.Count != 0)
					{
						PartCostsBase partCostsBase = new PartCostsBase
						{
							SourcePartBinDetailID = value,
							CostType = CostType.Actual,
							Quantity = quantity
						};
						if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "UnitMaterialCost") && sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "UnitSubcontractCost") && sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "UnitLaborCost") && sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "UnitOverheadCost") && (!base.Field.Table.TableName.Equals("MfgReceipts", StringComparison.CurrentCultureIgnoreCase) || _jobField == null || !sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "ReceiptType") || sourceRow.Field<MfgReceiptType>(base.Field.Table.FieldPrefix + "ReceiptType", rowVersion) != MfgReceiptType.MfgReceipt))
						{
							partCostsBase.LaborCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "UnitLaborCost", rowVersion);
							partCostsBase.OverheadCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "UnitOverheadCost", rowVersion);
							if (sourceRow.Field<MfgReceiptType>(base.Field.Table.FieldPrefix + "ReceiptType", rowVersion) == MfgReceiptType.MiscReceiptToInventory)
							{
								partCostsBase.MaterialCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "UnitMaterialCost", rowVersion) + Math.Round(sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "SetupCharge", rowVersion) / sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "MiscInvQuantityReceived", rowVersion), 5);
							}
							else
							{
								switch (sourceRow.Field<JobType>(base.Field.Table.FieldPrefix + "JobType", rowVersion))
								{
								case JobType.Material:
									partCostsBase.MaterialCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "UnitMaterialCost", rowVersion) + Math.Round(sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "SetupCharge", rowVersion) / sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "JobMatQuantityReceived", rowVersion), 5);
									partCostsBase.SubcontractCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "UnitSubcontractCost", rowVersion);
									break;
								case JobType.Subcontract:
									partCostsBase.MaterialCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "UnitMaterialCost", rowVersion);
									partCostsBase.SubcontractCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "UnitSubcontractCost", rowVersion) + Math.Round(sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "SetupCharge", rowVersion) / sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "JobOprQuantityReceived", rowVersion), 5);
									break;
								default:
								{
									decimal num = Math.Round(sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "SetupCharge", rowVersion) / sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "JobAsmQuantityReceived", rowVersion) / 2m, 5);
									partCostsBase.MaterialCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "UnitMaterialCost", rowVersion) + num;
									partCostsBase.SubcontractCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "UnitSubcontractCost", rowVersion) + num;
									break;
								}
								}
							}
							partCostsBase.DutyCost = default(decimal);
							partCostsBase.FreightCost = default(decimal);
							partCostsBase.MiscCost = default(decimal);
						}
						else if (base.Field.Table.TableName.Equals("MfgReceipts", StringComparison.CurrentCultureIgnoreCase) && _jobField != null && sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "ReceiptType") && sourceRow.Field<MfgReceiptType>(base.Field.Table.FieldPrefix + "ReceiptType", rowVersion) == MfgReceiptType.MfgReceipt)
						{
							partCostsBase.LaborCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "UnitLaborCost", rowVersion);
							partCostsBase.MaterialCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "UnitMaterialCost", rowVersion);
							partCostsBase.SubcontractCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "UnitSubcontractCost", rowVersion);
							partCostsBase.OverheadCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "UnitOverheadCost", rowVersion);
						}
						else if (base.Field.Table.TableName.Equals("ReceiptLines", StringComparison.CurrentCultureIgnoreCase) || base.Field.Table.TableName.Equals("ReceiptComponents", StringComparison.CurrentCultureIgnoreCase))
						{
							partCostsBase.LaborCost = default(decimal);
							partCostsBase.OverheadCost = default(decimal);
							partCostsBase.MaterialCost = default(decimal);
							partCostsBase.SubcontractCost = default(decimal);
							if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "JobType"))
							{
								if (sourceRow.Field<JobType>(base.Field.Table.FieldPrefix + "JobType", rowVersion) == JobType.Subcontract)
								{
									decimal num2 = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "InventoryUnitCost", rowVersion);
									decimal num3 = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "SetupCharge", rowVersion);
									decimal num4 = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "JobOprQuantityReceived", rowVersion);
									decimal num5 = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "InventoryQuantityReceived", rowVersion);
									decimal num6 = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "JobMatQuantityReceived", rowVersion);
									decimal num7 = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "QuantityToInspect", rowVersion);
									decimal num8 = num5 + num6 + num7 + num4;
									partCostsBase.SubcontractCost = num2 + Math.Round(num3 / num8, 5);
								}
								else
								{
									decimal num9 = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "InventoryUnitCost", rowVersion);
									decimal num10 = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "SetupCharge", rowVersion);
									decimal num11 = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "JobOprQuantityReceived", rowVersion);
									decimal num12 = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "InventoryQuantityReceived", rowVersion);
									decimal num13 = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "JobMatQuantityReceived", rowVersion);
									decimal num14 = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "QuantityToInspect", rowVersion);
									decimal num15 = num12 + num13 + num14 + num11;
									partCostsBase.MaterialCost = num9 + Math.Round(num10 / num15, 5);
								}
							}
							else
							{
								partCostsBase.MaterialCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "InventoryUnitCost", rowVersion);
							}
							if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "DutyUnitCost"))
							{
								partCostsBase.DutyCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "DutyUnitCost", rowVersion);
							}
							if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "FreightUnitCost"))
							{
								partCostsBase.FreightCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "FreightUnitCost", rowVersion);
							}
							if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "MiscUnitCost"))
							{
								partCostsBase.MiscCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "MiscUnitCost", rowVersion);
							}
						}
						else if (base.Field.Table.TableName.Equals("MfgReceiptComponents", StringComparison.CurrentCultureIgnoreCase))
						{
							partCostsBase.LaborCost = default(decimal);
							partCostsBase.OverheadCost = default(decimal);
							partCostsBase.MaterialCost = sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "UnitCost", rowVersion);
							partCostsBase.SubcontractCost = default(decimal);
							partCostsBase.DutyCost = default(decimal);
							partCostsBase.FreightCost = default(decimal);
							partCostsBase.MiscCost = default(decimal);
						}
						else if (base.Field.Table.TableName.Equals("RMAReceiptLines", StringComparison.CurrentCultureIgnoreCase))
						{
							DataTable rmaReceiptLinkedShipmentCostsDataTable = new RMAReceipt().GetRmaReceiptLinkedShipmentCostsDataTable(database, transaction, "RMAReceiptLines", sourceRow.Field<string>("rrlRMAReceiptID", rowVersion), sourceRow.Field<short>("rrlRMAReceiptLineID", rowVersion));
							if (rmaReceiptLinkedShipmentCostsDataTable.Rows.Count != 0)
							{
								addCostsToCostsList(quantity, flag, list, rmaReceiptLinkedShipmentCostsDataTable);
								return list;
							}
							SetStandardCostingFieldsForActualCostRow(_costingMethod, dataTable, partCostsBase);
						}
						else if (base.Field.Table.TableName.Equals("RMAReceiptComponents", StringComparison.CurrentCultureIgnoreCase))
						{
							DataTable rmaReceiptLinkedShipmentCostsDataTable2 = new RMAReceipt().GetRmaReceiptLinkedShipmentCostsDataTable(database, transaction, "RMAReceiptComponents", sourceRow.Field<string>("rroRMAReceiptID", rowVersion), sourceRow.Field<short>("rroRMAReceiptLineID", rowVersion), sourceRow.Field<int>("rroRMAReceiptComponentID", rowVersion));
							if (rmaReceiptLinkedShipmentCostsDataTable2.Rows.Count != 0)
							{
								addCostsToCostsList(quantity, flag, list, rmaReceiptLinkedShipmentCostsDataTable2);
								return list;
							}
							SetStandardCostingFieldsForActualCostRow(_costingMethod, dataTable, partCostsBase);
						}
						else if (base.Field.Table.TableName.Equals("InspectionLines", StringComparison.CurrentCultureIgnoreCase) || base.Field.Table.TableName.Equals("InspectionComponents", StringComparison.CurrentCultureIgnoreCase))
						{
							if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "SourceTableName") && sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "SourceTableUniqueID"))
							{
								if (sourceRow.Field<string>(base.Field.Table.FieldPrefix + "SourceTableName", rowVersion).Equals("ReceiptLines", StringComparison.CurrentCultureIgnoreCase))
								{
									sqlCommand = database.NewSqlCommand("Select rmlInventoryUnitCost,rmlJobType,rmlDutyUnitCost,rmlFreightUnitCost,rmlMiscUnitCost, rmlSetupCharge, rmlPurchaseQuantityReceived, rmlJobOprQuantityReceived, rmlInventoryQuantityReceived, rmlJobMatQuantityReceived, rmlQuantityToInspect from ReceiptLines Where rmlUniqueID = @UniqueID");
									sqlCommand.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = sourceRow.Field<Guid>(base.Field.Table.FieldPrefix + "SourceTableUniqueID", rowVersion);
									DataTable dataTable2 = database.GetDataTable(sqlCommand, transaction);
									if (dataTable2.Rows.Count != 0)
									{
										partCostsBase.LaborCost = default(decimal);
										partCostsBase.OverheadCost = default(decimal);
										partCostsBase.MaterialCost = default(decimal);
										partCostsBase.SubcontractCost = default(decimal);
										if (dataTable2.Rows[0].Field<JobType>("rmlJobType") == JobType.Subcontract)
										{
											decimal num16 = dataTable2.Rows[0].Field<decimal>("rmlInventoryUnitCost");
											decimal num17 = dataTable2.Rows[0].Field<decimal>("rmlSetupCharge");
											decimal num18 = dataTable2.Rows[0].Field<decimal>("rmlJobOprQuantityReceived");
											decimal num19 = dataTable2.Rows[0].Field<decimal>("rmlInventoryQuantityReceived");
											decimal num20 = dataTable2.Rows[0].Field<decimal>("rmlJobMatQuantityReceived");
											decimal num21 = dataTable2.Rows[0].Field<decimal>("rmlQuantityToInspect");
											decimal num22 = num19 + num20 + num21 + num18;
											partCostsBase.SubcontractCost = num16 + Math.Round(num17 / num22, 5);
										}
										else
										{
											decimal num23 = dataTable2.Rows[0].Field<decimal>("rmlInventoryUnitCost");
											decimal num24 = dataTable2.Rows[0].Field<decimal>("rmlSetupCharge");
											decimal num25 = dataTable2.Rows[0].Field<decimal>("rmlJobOprQuantityReceived");
											decimal num26 = dataTable2.Rows[0].Field<decimal>("rmlInventoryQuantityReceived");
											decimal num27 = dataTable2.Rows[0].Field<decimal>("rmlJobMatQuantityReceived");
											decimal num28 = dataTable2.Rows[0].Field<decimal>("rmlQuantityToInspect");
											decimal num29 = num26 + num27 + num28 + num25;
											partCostsBase.MaterialCost = num23 + Math.Round(num24 / num29, 5);
										}
										partCostsBase.DutyCost = dataTable2.Rows[0].Field<decimal>("rmlDutyUnitCost");
										partCostsBase.FreightCost = dataTable2.Rows[0].Field<decimal>("rmlFreightUnitCost");
										partCostsBase.MiscCost = dataTable2.Rows[0].Field<decimal>("rmlMiscUnitCost");
									}
								}
								else if (sourceRow.Field<string>(base.Field.Table.FieldPrefix + "SourceTableName", rowVersion).Equals("ReceiptComponents", StringComparison.CurrentCultureIgnoreCase))
								{
									sqlCommand = database.NewSqlCommand("Select rmoInventoryUnitCost from ReceiptComponents Where rmoUniqueID = @UniqueID");
									sqlCommand.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = sourceRow.Field<Guid>(base.Field.Table.FieldPrefix + "SourceTableUniqueID", rowVersion);
									DataTable dataTable3 = database.GetDataTable(sqlCommand, transaction);
									if (dataTable3.Rows.Count != 0)
									{
										partCostsBase.LaborCost = default(decimal);
										partCostsBase.OverheadCost = default(decimal);
										partCostsBase.MaterialCost = dataTable3.Rows[0].Field<decimal>("rmoInventoryUnitCost");
										partCostsBase.SubcontractCost = default(decimal);
										partCostsBase.DutyCost = default(decimal);
										partCostsBase.FreightCost = default(decimal);
										partCostsBase.MiscCost = default(decimal);
									}
								}
								else if (sourceRow.Field<string>(base.Field.Table.FieldPrefix + "SourceTableName", rowVersion).Equals("RMAReceiptLines", StringComparison.CurrentCultureIgnoreCase))
								{
									sqlCommand = database.NewSqlCommand("select intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, IsNull(imgPartBinDetailID,1) as imgPartBinDetailID  from RMAReceiptLines inner join RMAClaimLines on rrlRMAClaimID = ralRMAClaimID and rrlRMAClaimLineID = ralRMAClaimLineID  inner join ShipmentLines on ralShipmentID = smlShipmentID and ralShipmentLineID = smlShipmentLineID  inner join PartTransactions on smlUniqueID = imtTableUniqueID inner join PartTransactionCosts on intPartTransactionID = imtPartTransactionID  left join PartBinDetails on intSourceTableUniqueID = imgUniqueID  where rrlUniqueID = @UniqueID ORDER BY imgTransactionDate " + _sortString + ", imgPartBinDetailID ASC");
									sqlCommand.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = sourceRow.Field<Guid>(base.Field.Table.FieldPrefix + "SourceTableUniqueID", rowVersion);
									DataTable dataTable4 = database.GetDataTable(sqlCommand, transaction);
									if (dataTable4.Rows.Count != 0)
									{
										addCostsToCostsList(quantity, flag, list, dataTable4);
										return list;
									}
									SetStandardCostingFieldsForActualCostRow(_costingMethod, dataTable, partCostsBase);
								}
								else if (sourceRow.Field<string>(base.Field.Table.FieldPrefix + "SourceTableName", rowVersion).Equals("RMAReceiptComponents", StringComparison.CurrentCultureIgnoreCase))
								{
									sqlCommand = database.NewSqlCommand("select intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, imgPartBinDetailID  from RMAReceiptComponents inner join RMAClaimComponents on rroRMAClaimID = raoRMAClaimID and rroRMAClaimLineID = raoRMAClaimLineID and rroRMAClaimComponentID = raoRMAClaimComponentID  inner join ShipmentComponents on raoShipmentID = smoShipmentID and raoShipmentLineID = smoShipmentLineID and raoShipmentComponentID = smoShipmentComponentID  inner join PartTransactions on smoUniqueID = imtTableUniqueID inner join PartTransactionCosts on intPartTransactionID = imtPartTransactionID  inner join PartBinDetails on intSourceTableUniqueID = imgUniqueID  where rroUniqueID = @UniqueID ORDER BY imgTransactionDate " + _sortString + ", imgPartBinDetailID ASC");
									sqlCommand.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = sourceRow.Field<Guid>(base.Field.Table.FieldPrefix + "SourceTableUniqueID", rowVersion);
									DataTable dataTable5 = database.GetDataTable(sqlCommand, transaction);
									if (dataTable5.Rows.Count != 0)
									{
										addCostsToCostsList(quantity, flag, list, dataTable5);
										return list;
									}
									SetStandardCostingFieldsForActualCostRow(_costingMethod, dataTable, partCostsBase);
								}
								else if (sourceRow.Field<string>(base.Field.Table.FieldPrefix + "SourceTableName", rowVersion).Equals("MfgReceipts", StringComparison.CurrentCultureIgnoreCase))
								{
									sqlCommand = database.NewSqlCommand("Select rmmUnitLaborCost,rmmUnitOverheadCost,rmmUnitMaterialCost,rmmUnitSubcontractCost from MfgReceipts Where rmmUniqueID = @UniqueID");
									sqlCommand.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = sourceRow.Field<Guid>(base.Field.Table.FieldPrefix + "SourceTableUniqueID", rowVersion);
									DataTable dataTable6 = database.GetDataTable(sqlCommand, transaction);
									if (dataTable6.Rows.Count != 0)
									{
										partCostsBase.LaborCost = dataTable6.Rows[0].Field<decimal>("rmmUnitLaborCost");
										partCostsBase.OverheadCost = dataTable6.Rows[0].Field<decimal>("rmmUnitOverheadCost");
										partCostsBase.MaterialCost = dataTable6.Rows[0].Field<decimal>("rmmUnitMaterialCost");
										partCostsBase.SubcontractCost = dataTable6.Rows[0].Field<decimal>("rmmUnitSubcontractCost");
										partCostsBase.DutyCost = default(decimal);
										partCostsBase.FreightCost = default(decimal);
										partCostsBase.MiscCost = default(decimal);
									}
								}
								else if (sourceRow.Field<string>(base.Field.Table.FieldPrefix + "SourceTableName", rowVersion).Equals("MfgReceiptComponents", StringComparison.CurrentCultureIgnoreCase))
								{
									sqlCommand = database.NewSqlCommand("Select rmnUnitCost from MfgReceiptComponents Where rmnUniqueID = @UniqueID");
									sqlCommand.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = sourceRow.Field<Guid>(base.Field.Table.FieldPrefix + "SourceTableUniqueID", rowVersion);
									DataTable dataTable7 = database.GetDataTable(sqlCommand, transaction);
									if (dataTable7.Rows.Count != 0)
									{
										partCostsBase.LaborCost = default(decimal);
										partCostsBase.OverheadCost = default(decimal);
										partCostsBase.MaterialCost = dataTable7.Rows[0].Field<decimal>("rmnUnitCost");
										partCostsBase.SubcontractCost = default(decimal);
										partCostsBase.DutyCost = default(decimal);
										partCostsBase.FreightCost = default(decimal);
										partCostsBase.MiscCost = default(decimal);
									}
								}
								else
								{
									SetStandardCostingFieldsForActualCostRow(_costingMethod, dataTable, partCostsBase);
								}
							}
						}
						else if (base.Field.Table.TableName.Equals("QuantityAdjustments", StringComparison.CurrentCultureIgnoreCase))
						{
							if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "AdjustmentType"))
							{
								if (sourceRow.Field<QuantityAdjustmentType>(base.Field.Table.FieldPrefix + "AdjustmentType", rowVersion) == QuantityAdjustmentType.QuantityOnHand)
								{
									sqlCommand = database.NewSqlCommand("Select Top 1 imgUnitLaborCost,imgUnitOverheadCost,imgUnitMaterialCost,imgUnitSubcontractCost,imgUnitDutyCost,imgUnitFreightCost,imgUnitMiscCost,imgRemainingQuantity,imgUniqueID FROM PartBinDetails Where imgPartID = @PartID AND imgPartRevisionID = @RevisionID AND imgWarehouseID = @WarehouseID AND imgPartBinID = @BinID  AND imgQuantityType = @QtyType ORDER BY imgTransactionDate DESC, imgRemainingQuantity ASC");
									sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
									sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
									sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
									sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
									sqlCommand.Parameters.Add(new SqlParameter("@QtyType", SqlDbType.TinyInt)).Value = BinDetailQuantityType;
									DataTable dataTable8 = database.GetDataTable(sqlCommand, transaction);
									if (dataTable8.Rows.Count != 0)
									{
										partCostsBase.LaborCost = dataTable8.Rows[0].Field<decimal>("imgUnitLaborCost");
										partCostsBase.OverheadCost = dataTable8.Rows[0].Field<decimal>("imgUnitOverheadCost");
										partCostsBase.MaterialCost = dataTable8.Rows[0].Field<decimal>("imgUnitMaterialCost");
										partCostsBase.SubcontractCost = dataTable8.Rows[0].Field<decimal>("imgUnitSubcontractCost");
										partCostsBase.DutyCost = dataTable8.Rows[0].Field<decimal>("imgUnitDutyCost");
										partCostsBase.FreightCost = dataTable8.Rows[0].Field<decimal>("imgUnitFreightCost");
										partCostsBase.MiscCost = dataTable8.Rows[0].Field<decimal>("imgUnitMiscCost");
									}
									else
									{
										SetStandardCostingFieldsForActualCostRow(_costingMethod, dataTable, partCostsBase);
									}
								}
								else if (sourceRow.Field<QuantityAdjustmentType>(base.Field.Table.FieldPrefix + "AdjustmentType", rowVersion) == QuantityAdjustmentType.BinTransfer && sourceRow.Table.Columns.Contains("inqUniqueID"))
								{
									sqlCommand = database.NewSqlCommand("select intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, imgPartBinDetailID  from PartTransactions inner join PartTransactionCosts on imtPartTransactionID = intPartTransactionID inner join PartBinDetails on intSourceTableUniqueID = imgUniqueID  where imtTableUniqueID = @UniqueID and imtTransactionType = 2 Order By imgPartBinDetailID ASC");
									sqlCommand.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = sourceRow.Field<Guid>("inqUniqueID", rowVersion);
									DataTable dataTable9 = database.GetDataTable(sqlCommand, transaction);
									if (dataTable9.Rows.Count != 0)
									{
										addCostsToCostsList(quantity, flag, list, dataTable9);
										return list;
									}
								}
							}
						}
						else if (base.Field.Table.TableName.Equals("WarehouseReceiptLines", StringComparison.CurrentCultureIgnoreCase))
						{
							sqlCommand = database.NewSqlCommand("select intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, imgPartBinDetailID  from WarehouseTransferLines inner join PartTransactions on mwlUniqueID = imtTableUniqueID inner join PartTransactionCosts on intPartTransactionID = imtPartTransactionID  inner join PartBinDetails on intSourceTableUniqueID = imgUniqueID where mwlWarehouseTransferID = @WHTransferID and mwlWarehouseTransferLineID = @WHTransferLineID Order By imgPartBinDetailID ASC");
							sqlCommand.Parameters.Add(new SqlParameter("@WHTransferID", SqlDbType.NVarChar)).Value = sourceRow.Field<string>("wrlWarehouseTransferID", rowVersion);
							sqlCommand.Parameters.Add(new SqlParameter("@WHTransferLineID", SqlDbType.Int)).Value = sourceRow.Field<short>("wrlWarehouseTransferLineID", rowVersion);
							DataTable dataTable10 = database.GetDataTable(sqlCommand, transaction);
							if (dataTable10.Rows.Count != 0)
							{
								addCostsToCostsList(quantity, flag, list, dataTable10);
								return list;
							}
						}
						else if (base.Field.Table.TableName.Equals("WarehouseReceiptComponents", StringComparison.CurrentCultureIgnoreCase))
						{
							sqlCommand = database.NewSqlCommand("select intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, imgPartBinDetailID  from WarehouseTransferComponents inner join PartTransactions on mwoUniqueID = imtTableUniqueID inner join PartTransactionCosts on intPartTransactionID = imtPartTransactionID  inner join PartBinDetails on intSourceTableUniqueID = imgUniqueID where mwoWarehouseTransferID = @WHTransferID and mwoWarehouseTransferLineID = @WHTransferLineID and mwoWarehouseTransComponentID = @WHTransferCompID Order By imgPartBinDetailID ASC");
							sqlCommand.Parameters.Add(new SqlParameter("@WHTransferID", SqlDbType.NVarChar)).Value = sourceRow.Field<string>("wroWarehouseTransferID", rowVersion);
							sqlCommand.Parameters.Add(new SqlParameter("@WHTransferLineID", SqlDbType.Int)).Value = sourceRow.Field<short>("wroWarehouseTransferLineID", rowVersion);
							sqlCommand.Parameters.Add(new SqlParameter("@WHTransferCompID", SqlDbType.Int)).Value = sourceRow.Field<short>("wroWarehouseTransComponentID", rowVersion);
							DataTable dataTable11 = database.GetDataTable(sqlCommand, transaction);
							if (dataTable11.Rows.Count != 0)
							{
								addCostsToCostsList(quantity, flag, list, dataTable11);
								return list;
							}
						}
						else if (base.Field.Table.TableName.Equals("MaterialIssueLines", StringComparison.CurrentCultureIgnoreCase) || base.Field.Table.TableName.Equals("MaterialIssueComponents", StringComparison.CurrentCultureIgnoreCase))
						{
							if (base.Field.Table.TableName.Equals("MaterialIssueLines", StringComparison.CurrentCultureIgnoreCase))
							{
								sqlCommand = database.NewSqlCommand("select top 4 imtPartTransactionID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, imgPartBinDetailID  from PartTransactions inner join PartTransactionCosts on imtPartTransactionID = intPartTransactionID  left join PartBinDetails on intSourceTableUniqueID = imgUniqueID  inner join JobMaterials on imtJobID = jmmJobID and imtJobAssemblyID = jmmJobAssemblyID and imtJobMaterialID = jmmJobMaterialID  where imtJobID = @JobID and imtJobAssemblyID = @AsmID and imtJobMaterialID = @SeqID and imtPartTransactionID = (  select top 1 imtPartTransactionID from PartTransactions where imtJobID = @JobID and imtJobAssemblyID = @AsmID and imtJobMaterialID = @SeqID and((imtTransactionType = 2 and imtSource = 3 and (imtInventoryQuantityReceived < 0 or imtScrapQuantity < 0)) or (imtTransactionType = 1 and imtSource = 2 and (imtInventoryQuantityReceived > 0 or imtScrapQuantity > 0))) order by imtTransactionDate desc  ) order by intPartTransactionID desc, intPartTransactionCostID asc");
								sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = sourceRow.Field<string>("injJobID", rowVersion);
								sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = sourceRow.Field<int>("injJobAssemblyID", rowVersion);
								sqlCommand.Parameters.Add(new SqlParameter("@SeqID", SqlDbType.Int)).Value = sourceRow.Field<int>("injJobMaterialID", rowVersion);
							}
							else if (base.Field.Table.TableName.Equals("MaterialIssueComponents", StringComparison.CurrentCultureIgnoreCase))
							{
								sqlCommand = database.NewSqlCommand("select top 4 imtPartTransactionID, intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, imgPartBinDetailID  from PartTransactions inner join PartTransactionCosts on imtPartTransactionID = intPartTransactionID  left join PartBinDetails on intSourceTableUniqueID = imgUniqueID  inner join JobMaterialComponents on imtJobID = jmtJobID and imtJobAssemblyID = jmtJobAssemblyID and imtJobMaterialID = jmtJobMaterialID and imtJobMaterialComponentID = jmtJobMaterialComponentID  where imtJobID = @JobID and imtJobAssemblyID = @AsmID and imtJobMaterialID = @SeqID and imtJobMaterialComponentID = @CompID and imtPartTransactionID = (  select top 1 imtPartTransactionID from PartTransactions where imtJobID = @JobID and imtJobAssemblyID = @AsmID and imtJobMaterialID = @SeqID and imtJobMaterialComponentID = @CompID and ((imtTransactionType = 2 and imtSource = 3 and (imtInventoryQuantityReceived < 0 or imtScrapQuantity < 0)) or (imtTransactionType = 1 and imtSource = 2 and (imtInventoryQuantityReceived > 0 or imtScrapQuantity > 0))) order by imtTransactionDate desc  ) order by intPartTransactionID desc, intPartTransactionCostID asc");
								sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = sourceRow.Field<string>("inkJobID", rowVersion);
								sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = sourceRow.Field<int>("inkJobAssemblyID", rowVersion);
								sqlCommand.Parameters.Add(new SqlParameter("@SeqID", SqlDbType.Int)).Value = sourceRow.Field<int>("inkJobMaterialID", rowVersion);
								sqlCommand.Parameters.Add(new SqlParameter("@CompID", SqlDbType.Int)).Value = sourceRow.Field<int>("inkJobMaterialComponentID", rowVersion);
							}
							DataTable dataTable12 = database.GetDataTable(sqlCommand, transaction);
							if (dataTable12.Rows.Count != 0)
							{
								{
									foreach (DataRow row5 in dataTable12.Rows)
									{
										list.Add(SetPartCostFields((CostType)row5.Field<byte>("intCostType"), quantity, row5.Field<decimal>("intUnitLaborCost"), row5.Field<decimal>("intUnitOverheadCost"), row5.Field<decimal>("intUnitMaterialCost"), row5.Field<decimal>("intUnitSubcontractCost"), row5.Field<decimal>("intUnitDutyCost"), row5.Field<decimal>("intUnitFreightCost"), row5.Field<decimal>("intUnitMiscCost"), value));
									}
									return list;
								}
							}
							SetStandardCostingFieldsForActualCostRow(_costingMethod, dataTable, partCostsBase);
						}
						else
						{
							SetStandardCostingFieldsForActualCostRow(_costingMethod, dataTable, partCostsBase);
						}
						partCostsBase.ActualUnitLaborCost = partCostsBase.LaborCost;
						partCostsBase.ActualUnitOverheadCost = partCostsBase.OverheadCost;
						partCostsBase.ActualUnitMaterialCost = partCostsBase.MaterialCost;
						partCostsBase.ActualUnitSubcontractCost = partCostsBase.SubcontractCost;
						partCostsBase.ActualUnitDutyCost = partCostsBase.DutyCost;
						partCostsBase.ActualUnitFreightCost = partCostsBase.FreightCost;
						partCostsBase.ActualUnitMiscCost = partCostsBase.MiscCost;
						list.Add(SetPartCostFields(CostType.Average, quantity, dataTable.Rows[0].Field<decimal>("imrAverageLaborCost"), dataTable.Rows[0].Field<decimal>("imrAverageOverheadCost"), dataTable.Rows[0].Field<decimal>("imrAverageMaterialCost"), dataTable.Rows[0].Field<decimal>("imrAverageSubcontractCost"), dataTable.Rows[0].Field<decimal>("imrAverageDutyCost"), dataTable.Rows[0].Field<decimal>("imrAverageFreightCost"), dataTable.Rows[0].Field<decimal>("imrAverageMiscCost"), value));
						list.Add(SetPartCostFields(CostType.Last, quantity, dataTable.Rows[0].Field<decimal>("imrLastLaborCost"), dataTable.Rows[0].Field<decimal>("imrLastOverheadCost"), dataTable.Rows[0].Field<decimal>("imrLastMaterialCost"), dataTable.Rows[0].Field<decimal>("imrLastSubcontractCost"), dataTable.Rows[0].Field<decimal>("imrLastDutyCost"), dataTable.Rows[0].Field<decimal>("imrLastFreightCost"), dataTable.Rows[0].Field<decimal>("imrLastMiscCost"), value));
						list.Add(SetPartCostFields(CostType.Standard, quantity, dataTable.Rows[0].Field<decimal>("imrStandardLaborCost"), dataTable.Rows[0].Field<decimal>("imrStandardOverheadCost"), dataTable.Rows[0].Field<decimal>("imrStandardMaterialCost"), dataTable.Rows[0].Field<decimal>("imrStandardSubcontractCost"), dataTable.Rows[0].Field<decimal>("imrStandardDutyCost"), dataTable.Rows[0].Field<decimal>("imrStandardFreightCost"), dataTable.Rows[0].Field<decimal>("imrStandardMiscCost"), value));
						list.Add(partCostsBase);
						return list;
					}
					return null;
				}
				if (base.Field.Table.TableName.Equals("ShipmentLines", StringComparison.CurrentCultureIgnoreCase) && _jobField != null)
				{
					string jobID = sourceRow[_jobField.RelatedFieldsAndCurrentFieldArray[0], rowVersion].ToString();
					int jobAssemblyID = 0;
					return getJobCosts(database, transaction, quantity, jobID, jobAssemblyID, setGuid: true, (dataTable != null && dataTable.Rows.Count > 0) ? dataTable.Rows[0] : null);
				}
				if (!_nonStocked && !_allowNegativeQuantityOnHand)
				{
					sqlCommand = database.NewSqlCommand("Select  imgUnitLaborCost,imgUnitOverheadCost,imgUnitMaterialCost,imgUnitSubcontractCost,imgUnitDutyCost,imgUnitFreightCost,imgUnitMiscCost,imgRemainingQuantity,imgUniqueID, imrLastLaborCost,imrLastOverheadCost,imrLastMaterialCost,imrLastSubcontractCost,imrLastDutyCost,imrLastFreightCost,imrLastMiscCost, imrStandardLaborCost,imrStandardOverheadCost,imrStandardMaterialCost,imrStandardSubcontractCost,imrStandardDutyCost,imrStandardFreightCost,imrStandardMiscCost, imrAverageLaborCost,imrAverageOverheadCost,imrAverageMaterialCost,imrAverageSubcontractCost,imrAverageDutyCost,imrAverageFreightCost,imrAverageMiscCost from PartRevisions Inner Join Parts On imrPartID = impPartID Left Outer Join PartWarehouseLocations On imlPartID = imrPartID And imlPartRevisionID = imrPartRevisionID And imlPartWarehouseID = @WarehouseID  Inner Join PartBinDetails On imgPartID = imrPartID AND imgPartRevisionID = imrPartRevisionID AND imgWarehouseID = imlPartWarehouseID AND imgPartBinID = @BinID  where imrPartID = @PartID And imrPartRevisionID = @RevisionID And imgQuantityType = @QtyType And imgRemainingQuantity <> 0 ORDER BY imgTransactionDate " + _sortString + ", imgRemainingQuantity ASC");
					sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
					sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
					sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
					sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
					sqlCommand.Parameters.Add(new SqlParameter("@QtyType", SqlDbType.TinyInt)).Value = BinDetailQuantityType;
					DataTable dataTable13 = database.GetDataTable(sqlCommand, transaction);
					if (dataTable13.Rows.Count == 0 && GetLatestPartRevision(database, transaction, partID, ref partRevisionID))
					{
						sqlCommand.Parameters["@RevisionID"].Value = partRevisionID;
						dataTable13 = database.GetDataTable(sqlCommand, transaction);
					}
					if (dataTable13.Rows.Count != 0)
					{
						decimal num30 = Math.Abs(quantity);
						foreach (DataRow row6 in dataTable13.Rows)
						{
							if (!(num30 > 0m))
							{
								break;
							}
							decimal quantity2;
							if (num30 < row6.Field<decimal>("imgRemainingQuantity"))
							{
								quantity2 = num30;
								if (flag)
								{
									quantity2 *= -1m;
								}
								num30 = default(decimal);
							}
							else
							{
								num30 -= row6.Field<decimal>("imgRemainingQuantity");
								quantity2 = row6.Field<decimal>("imgRemainingQuantity");
								if (flag)
								{
									quantity2 *= -1m;
								}
							}
							list.Add(SetPartCostFields(CostType.Average, quantity2, row6.Field<decimal>("imrAverageLaborCost"), row6.Field<decimal>("imrAverageOverheadCost"), row6.Field<decimal>("imrAverageMaterialCost"), row6.Field<decimal>("imrAverageSubcontractCost"), row6.Field<decimal>("imrAverageDutyCost"), row6.Field<decimal>("imrAverageFreightCost"), row6.Field<decimal>("imrAverageMiscCost"), row6.Field<Guid>("imgUniqueID")));
							list.Add(SetPartCostFields(CostType.Last, quantity2, row6.Field<decimal>("imrLastLaborCost"), row6.Field<decimal>("imrLastOverheadCost"), row6.Field<decimal>("imrLastMaterialCost"), row6.Field<decimal>("imrLastSubcontractCost"), row6.Field<decimal>("imrLastDutyCost"), row6.Field<decimal>("imrLastFreightCost"), row6.Field<decimal>("imrLastMiscCost"), row6.Field<Guid>("imgUniqueID")));
							list.Add(SetPartCostFields(CostType.Standard, quantity2, row6.Field<decimal>("imrStandardLaborCost"), row6.Field<decimal>("imrStandardOverheadCost"), row6.Field<decimal>("imrStandardMaterialCost"), row6.Field<decimal>("imrStandardSubcontractCost"), row6.Field<decimal>("imrStandardDutyCost"), row6.Field<decimal>("imrStandardFreightCost"), row6.Field<decimal>("imrStandardMiscCost"), row6.Field<Guid>("imgUniqueID")));
							list.Add(SetPartCostFields(CostType.Actual, quantity2, row6.Field<decimal>("imgUnitLaborCost"), row6.Field<decimal>("imgUnitOverheadCost"), row6.Field<decimal>("imgUnitMaterialCost"), row6.Field<decimal>("imgUnitSubcontractCost"), row6.Field<decimal>("imgUnitDutyCost"), row6.Field<decimal>("imgUnitFreightCost"), row6.Field<decimal>("imgUnitMiscCost"), row6.Field<Guid>("imgUniqueID")));
						}
						return list;
					}
					return null;
				}
				sqlCommand = database.NewSqlCommand("Select imrLastTransactionDate, imrLastLaborCost,imrLastOverheadCost,imrLastMaterialCost,imrLastSubcontractCost,imrLastDutyCost,imrLastFreightCost,imrLastMiscCost, imrStandardLaborCost,imrStandardOverheadCost,imrStandardMaterialCost,imrStandardSubcontractCost,imrStandardDutyCost,imrStandardFreightCost,imrStandardMiscCost, imrAverageLaborCost,imrAverageOverheadCost,imrAverageMaterialCost,imrAverageSubcontractCost,imrAverageDutyCost,imrAverageFreightCost,imrAverageMiscCost from PartRevisions Inner Join Parts On imrPartID = impPartID Left Outer Join PartWarehouseLocations On imlPartID = imrPartID And imlPartRevisionID = imrPartRevisionID And imlPartWarehouseID = @WarehouseID where imrPartID = @PartID And imrPartRevisionID = @RevisionID");
				sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
				sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
				DataTable dataTable14 = database.GetDataTable(sqlCommand, transaction);
				if (dataTable14.Rows.Count == 0 && GetLatestPartRevision(database, transaction, partID, ref partRevisionID))
				{
					sqlCommand.Parameters["@RevisionID"].Value = partRevisionID;
					dataTable14 = database.GetDataTable(sqlCommand, transaction);
				}
				if (dataTable14.Rows.Count != 0)
				{
					list.Add(SetPartCostFields(CostType.Average, quantity, dataTable14.Rows[0].Field<decimal>("imrAverageLaborCost"), dataTable14.Rows[0].Field<decimal>("imrAverageOverheadCost"), dataTable14.Rows[0].Field<decimal>("imrAverageMaterialCost"), dataTable14.Rows[0].Field<decimal>("imrAverageSubcontractCost"), dataTable14.Rows[0].Field<decimal>("imrAverageDutyCost"), dataTable14.Rows[0].Field<decimal>("imrAverageFreightCost"), dataTable14.Rows[0].Field<decimal>("imrAverageMiscCost"), null));
					list.Add(SetPartCostFields(CostType.Last, quantity, dataTable14.Rows[0].Field<decimal>("imrLastLaborCost"), dataTable14.Rows[0].Field<decimal>("imrLastOverheadCost"), dataTable14.Rows[0].Field<decimal>("imrLastMaterialCost"), dataTable14.Rows[0].Field<decimal>("imrLastSubcontractCost"), dataTable14.Rows[0].Field<decimal>("imrLastDutyCost"), dataTable14.Rows[0].Field<decimal>("imrLastFreightCost"), dataTable14.Rows[0].Field<decimal>("imrLastMiscCost"), null));
					list.Add(SetPartCostFields(CostType.Standard, quantity, dataTable14.Rows[0].Field<decimal>("imrStandardLaborCost"), dataTable14.Rows[0].Field<decimal>("imrStandardOverheadCost"), dataTable14.Rows[0].Field<decimal>("imrStandardMaterialCost"), dataTable14.Rows[0].Field<decimal>("imrStandardSubcontractCost"), dataTable14.Rows[0].Field<decimal>("imrStandardDutyCost"), dataTable14.Rows[0].Field<decimal>("imrStandardFreightCost"), dataTable14.Rows[0].Field<decimal>("imrStandardMiscCost"), null));
					PartCostsBase partCostsBase2 = new PartCostsBase
					{
						CostType = CostType.Actual,
						Quantity = quantity
					};
					SetStandardCostingFieldsForActualCostRow(_costingMethod, dataTable14, partCostsBase2);
					partCostsBase2.ActualUnitLaborCost = partCostsBase2.LaborCost;
					partCostsBase2.ActualUnitOverheadCost = partCostsBase2.OverheadCost;
					partCostsBase2.ActualUnitMaterialCost = partCostsBase2.MaterialCost;
					partCostsBase2.ActualUnitSubcontractCost = partCostsBase2.SubcontractCost;
					partCostsBase2.ActualUnitDutyCost = partCostsBase2.DutyCost;
					partCostsBase2.ActualUnitFreightCost = partCostsBase2.FreightCost;
					partCostsBase2.ActualUnitMiscCost = partCostsBase2.MiscCost;
					list.Add(partCostsBase2);
				}
				return list;
			}
			object[] array = base.Field.Table.TableName switch
			{
				"ReceiptLines" => new object[2] { "rmlReverseReceiptID", "rmlReverseReceiptLineID" }, 
				"ReceiptComponents" => new object[3] { "rmoReverseReceiptID", "rmoReverseReceiptLineID", "rmoReverseReceiptComponentID" }, 
				"ShipmentLines" => new object[2] { "smlReverseShipmentID", "smlReverseShipmentLineID" }, 
				"ShipmentComponents" => new object[3] { "smoReverseShipmentID", "smoReverseShipmentLineID", "smoReverseShipmentComponentID" }, 
				"MaterialIssueLines" => new object[2] { "injReverseMaterialIssueID", "injReverseMaterialIssueLineID" }, 
				"MaterialIssueComponents" => new object[3] { "inkReverseMaterialIssueID", "inkReverseMaterialIssueLineID", "inkReverseMaterialIssueCompID" }, 
				"MfgReceipts" => new object[1] { "rmmReverseMfgReceiptID" }, 
				"MfgReceiptComponents" => new object[2] { "rmnReverseMfgReceiptID", "rmnReverseMfgReceiptCompID" }, 
				"InspectionLines" => new object[2] { "qalReverseInspectionID", "qalReverseInspectionLineID" }, 
				"InspectionComponents" => new object[3] { "qamReverseInspectionID", "qalReverseInspectionLineID", "qamReverseInspectionComponentID" }, 
				"RMAReceiptLines" => new object[2] { "rrlReverseRMAReceiptID", "rrlReverseRMAReceiptLineID" }, 
				"RMAReceiptComponents" => new object[3] { "rroReverseRMAReceiptID", "rroReverseRMAReceiptLineID", "rroReverseRMAReceiptCompID" }, 
				"DMRShipmentLines" => new object[2] { "dslReverseDMRShipmentID", "dslReverseDMRShipmentLineID" }, 
				"DMRShipmentComponents" => new object[3] { "dsoReverseDMRShipmentID", "dsoReverseDMRShipmentLineID", "dsoReverseDMRShipmentCompID" }, 
				"WarehouseReceiptLines" => new object[2] { "wrlReverseWHReceiptID", "wrlReverseWHReceiptLineID" }, 
				"WarehouseReceiptComponents" => new object[3] { "wroReverseWHReceiptID", "wroReverseWHReceiptLineID", "wroReverseWHReceiptCompID" }, 
				"WarehouseTransferLines" => new object[2] { "mwlReverseWHTransferID", "mwlReverseWHTransferLineID" }, 
				"WarehouseTransferComponents" => new object[3] { "mwoReverseWHTransferID", "mwoReverseWHTransferLineID", "mwoReverseWHTransComponentID" }, 
				_ => new object[0], 
			};
			if (array.Length != 0)
			{
				foreach (DataRow row7 in getReversalTransactions(database, transaction, sourceRow, rowVersion, array).Rows)
				{
					list.Add(SetPartCostFields((CostType)row7.Field<byte>("intCostType"), -row7.Field<decimal>("intQuantity"), row7.Field<decimal>("intUnitLaborCost"), row7.Field<decimal>("intUnitOverheadCost"), row7.Field<decimal>("intUnitMaterialCost"), row7.Field<decimal>("intUnitSubcontractCost"), row7.Field<decimal>("intUnitDutyCost"), row7.Field<decimal>("intUnitFreightCost"), row7.Field<decimal>("intUnitMiscCost"), row7.Field<Guid>("intSourceTableUniqueID")));
				}
			}
			return list;
		}
		foreach (DataRow row8 in GetExistingTransactions(database, transaction, sourceRow, rowVersion, quantity).Rows)
		{
			list.Add(SetPartCostFields((CostType)row8.Field<byte>("intCostType"), -row8.Field<decimal>("intQuantity"), row8.Field<decimal>("intUnitLaborCost"), row8.Field<decimal>("intUnitOverheadCost"), row8.Field<decimal>("intUnitMaterialCost"), row8.Field<decimal>("intUnitSubcontractCost"), row8.Field<decimal>("intUnitDutyCost"), row8.Field<decimal>("intUnitFreightCost"), row8.Field<decimal>("intUnitMiscCost"), row8.Field<Guid>("intSourceTableUniqueID")));
		}
		return list;
	}

	private List<PartCostsBase> getJobCosts(M1Database database, SqlTransaction transaction, decimal quantity, string jobID, int jobAssemblyID, bool setGuid, DataRow partRevDataRow)
	{
		List<PartCostsBase> list = new List<PartCostsBase>();
		decimal qtyCompleted = default(decimal);
		SqlCommand sqlCommand = database.NewSqlCommand("select jmaQuantityCompleted from JobAssemblies where jmaJobID = @JobID and jmaJobAssemblyID = @AssemblyID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		sqlCommand.Parameters.Add(new SqlParameter("@AssemblyID", SqlDbType.Int)).Value = jobAssemblyID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			qtyCompleted = dataTable.Rows[0].Field<decimal>("jmaQuantityCompleted");
		}
		Guid? uniqueID = (setGuid ? new Guid?(Guid.NewGuid()) : ((Guid?)null));
		Job job = new Job();
		foreach (CostType value in Enum.GetValues(typeof(CostType)))
		{
			if (value == CostType.Standard && _costingMethod == CostingMethod.Standard && partRevDataRow != null)
			{
				list.Add(SetPartCostFields(CostType.Standard, quantity, partRevDataRow.Field<decimal>("imrStandardLaborCost"), partRevDataRow.Field<decimal>("imrStandardOverheadCost"), partRevDataRow.Field<decimal>("imrStandardMaterialCost"), partRevDataRow.Field<decimal>("imrStandardSubcontractCost"), partRevDataRow.Field<decimal>("imrStandardDutyCost"), partRevDataRow.Field<decimal>("imrStandardFreightCost"), partRevDataRow.Field<decimal>("imrStandardMiscCost"), uniqueID));
				continue;
			}
			JobCost jobCosts = job.GetJobCosts(database, transaction, jobID, jobAssemblyID, qtyCompleted, (byte)value);
			if (jobCosts != null)
			{
				list.Add(SetPartCostFields(value, quantity, jobCosts.LaborCost, jobCosts.OverheadCost, jobCosts.MaterialCost, jobCosts.SubcontractCost, 0m, 0m, 0m, uniqueID));
			}
		}
		return list;
	}

	private DataTable GetExistingTransactions(M1Database database, SqlTransaction transaction, DataRow sourceRow, DataRowVersion rowVersion, decimal quantity)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		string text = string.Empty;
		object[] array = base.Field.BindingSource.PrimaryTable.GetParentBindingSource(sourceRow)?.PrimaryTable.KeyFieldsArray;
		object[] array2 = array;
		array = base.Field.Table.KeyFieldsArray;
		object[] array3 = array;
		string text2 = base.Field.BindingSource.PrimaryTable.ParentTable();
		if (array2 == null)
		{
			array2 = array3;
			text2 = base.Field.Table.TableName;
		}
		if (!string.IsNullOrWhiteSpace(text2))
		{
			for (int i = 0; i < array2.Length; i++)
			{
				string text3 = $"{array2[i]} = {base.Field.Table.KeyFieldsArray[i]}";
				if (stringBuilder2.Length == 0)
				{
					stringBuilder2.Append(text3);
				}
				else
				{
					stringBuilder2.AppendFormat(" And {0}", text3);
				}
				text3 = $"{array2[i]} = {sourceRow[(string)array3[i], rowVersion].ToSql()}";
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(text3);
				}
				else
				{
					stringBuilder.AppendFormat(" And {0}", text3);
				}
			}
			StringBuilder stringBuilder3 = new StringBuilder();
			stringBuilder3.AppendFormat(" intPartTransactionID In (Select TOP 1 imtPartTransactionID From PartTransactions Inner Join PartTransactionCosts on imtPartTransactionID=intPartTransactionID Where");
			stringBuilder3.AppendFormat(" imtTableUniqueID In (Select {0} ", base.Field.Table.UniqueField);
			stringBuilder3.AppendFormat(" From {0}", base.Field.Table.TableName);
			if (!string.IsNullOrWhiteSpace(text2) && !text2.Equals(base.Field.Table.TableName, StringComparison.CurrentCultureIgnoreCase))
			{
				stringBuilder3.AppendFormat(" Inner Join {0}  On {1}", text2, stringBuilder2.ToString());
			}
			if (BinDetailQuantityType == QuantityType.OnHand || BinDetailQuantityType == QuantityType.ToInspect)
			{
				stringBuilder3.AppendFormat(" Where {0}) And {1} = {2} ORDER BY imtTransactionDate DESC)", stringBuilder.ToString(), PartTransactionQuantityField, -quantity);
			}
			else
			{
				stringBuilder3.AppendFormat(" Where {0}))", stringBuilder.ToString());
			}
			text = stringBuilder3.ToString();
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			return database.GetDataTable("Select * from PartTransactionCosts where " + text, transaction);
		}
		return null;
	}

	private DataTable getReversalTransactions(M1Database database, SqlTransaction transaction, DataRow sourceRow, DataRowVersion rowVersion, object[] reverseKeyFields)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = string.Empty;
		object[] keyFieldsArray = base.Field.Table.KeyFieldsArray;
		object[] array = keyFieldsArray;
		if (reverseKeyFields.Length <= array.Length)
		{
			string empty = string.Empty;
			for (int i = 0; i < reverseKeyFields.Length; i++)
			{
				empty = $"{array[i]} = {sourceRow[(string)reverseKeyFields[i], rowVersion].ToSql()}";
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(empty);
				}
				else
				{
					stringBuilder.AppendFormat(" And {0}", empty);
				}
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.AppendFormat(" intPartTransactionID In (Select imtPartTransactionID From PartTransactions Inner Join PartTransactionCosts on imtPartTransactionID=intPartTransactionID Where");
			stringBuilder2.AppendFormat(" imtTransactionType = {0}", TransactionType);
			stringBuilder2.AppendFormat(" And imtTableUniqueID In (Select {0} ", base.Field.Table.UniqueField);
			stringBuilder2.AppendFormat(" From {0}", base.Field.Table.TableName);
			stringBuilder2.AppendFormat(" Where {0})) ", stringBuilder.ToString());
			text = stringBuilder2.ToString();
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			return database.GetDataTable("Select * from PartTransactionCosts where " + text, transaction);
		}
		return null;
	}

	private void addCostsToCostsList(decimal quantity, bool negativeQty, List<PartCostsBase> costsList, DataTable data)
	{
		decimal quantity2 = default(decimal);
		decimal num = Math.Abs(quantity);
		int num2 = 0;
		Guid value = default(Guid);
		foreach (DataRow row in data.Rows)
		{
			if (num2 != row.Field<int>("imgPartBinDetailID"))
			{
				value = Guid.NewGuid();
				if (!(num > 0m))
				{
					break;
				}
				if (num < Math.Abs(row.Field<decimal>("intQuantity")))
				{
					quantity2 = num;
					if (negativeQty)
					{
						quantity2 *= -1m;
					}
					num = default(decimal);
				}
				else
				{
					num -= Math.Abs(row.Field<decimal>("intQuantity"));
					quantity2 = Math.Abs(row.Field<decimal>("intQuantity"));
					if (negativeQty)
					{
						quantity2 *= -1m;
					}
				}
			}
			costsList.Add(SetPartCostFields((CostType)row.Field<byte>("intCostType"), quantity2, row.Field<decimal>("intUnitLaborCost"), row.Field<decimal>("intUnitOverheadCost"), row.Field<decimal>("intUnitMaterialCost"), row.Field<decimal>("intUnitSubcontractCost"), row.Field<decimal>("intUnitDutyCost"), row.Field<decimal>("intUnitFreightCost"), row.Field<decimal>("intUnitMiscCost"), value));
			num2 = row.Field<int>("imgPartBinDetailID");
		}
	}

	private static void SetStandardCostingFieldsForActualCostRow(CostingMethod costingMethod, DataTable data, PartCostsBase costActual)
	{
		string text = costingMethod switch
		{
			CostingMethod.Last => "Last", 
			CostingMethod.Standard => "Standard", 
			_ => "Average", 
		};
		if (!string.IsNullOrWhiteSpace(text))
		{
			costActual.LaborCost = data.Rows[0].Field<decimal>("imr" + text + "LaborCost");
			costActual.OverheadCost = data.Rows[0].Field<decimal>("imr" + text + "OverheadCost");
			costActual.MaterialCost = data.Rows[0].Field<decimal>("imr" + text + "MaterialCost");
			costActual.SubcontractCost = data.Rows[0].Field<decimal>("imr" + text + "SubcontractCost");
			costActual.DutyCost = data.Rows[0].Field<decimal>("imr" + text + "DutyCost");
			costActual.FreightCost = data.Rows[0].Field<decimal>("imr" + text + "FreightCost");
			costActual.MiscCost = data.Rows[0].Field<decimal>("imr" + text + "MiscCost");
		}
	}

	private PartCostsBase SetPartCostFields(CostType costType, decimal quantity, decimal labor, decimal overhead, decimal material, decimal subcontract, decimal duty, decimal freight, decimal misc, Guid? uniqueID)
	{
		PartCostsBase partCostsBase = new PartCostsBase
		{
			CostType = costType,
			Quantity = quantity,
			LaborCost = labor,
			OverheadCost = overhead,
			MaterialCost = material,
			SubcontractCost = subcontract,
			DutyCost = duty,
			FreightCost = freight,
			MiscCost = misc
		};
		partCostsBase.ActualUnitLaborCost = partCostsBase.LaborCost;
		partCostsBase.ActualUnitOverheadCost = partCostsBase.OverheadCost;
		partCostsBase.ActualUnitMaterialCost = partCostsBase.MaterialCost;
		partCostsBase.ActualUnitSubcontractCost = partCostsBase.SubcontractCost;
		partCostsBase.ActualUnitDutyCost = partCostsBase.DutyCost;
		partCostsBase.ActualUnitFreightCost = partCostsBase.FreightCost;
		partCostsBase.ActualUnitMiscCost = partCostsBase.MiscCost;
		if (uniqueID.HasValue)
		{
			partCostsBase.SourcePartBinDetailID = uniqueID.Value;
		}
		return partCostsBase;
	}

	private bool GetLatestPartRevision(M1Database database, SqlTransaction transaction, string partID, ref string partRevisionID)
	{
		return new Part().GetLatestPartRevision(database, transaction, partID, ref partRevisionID);
	}
}
