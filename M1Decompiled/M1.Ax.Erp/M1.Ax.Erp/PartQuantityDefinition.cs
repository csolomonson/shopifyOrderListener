using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class PartQuantityDefinition : FieldExtension
{
	public string BinQuantityField = string.Empty;

	public FieldDefinition jobField;

	public FieldDefinition jobStatusField;

	private FieldDefinition binField;

	public PartQuantityDefinition()
	{
	}

	public PartQuantityDefinition(M1BindingSource bs, string qtyFieldName, string binFieldName)
	{
		PartBinField = binFieldName;
		binField = bs.Fields[binFieldName];
		FieldName = qtyFieldName;
		base.Field = bs.Fields[qtyFieldName];
	}

	public void ProcessPartQuantityChange(M1Database database, DataRow sourceRow, DataRowVersion rowVersion, SqlTransaction transaction, bool backoutQty)
	{
		Part part = new Part();
		string parms = "";
		if (Parameters != null)
		{
			parms = Parameters.ToUpper();
		}
		if (!checkParms(sourceRow, rowVersion, base.Field.Table.TableName, base.Field.Table.FieldPrefix, parms))
		{
			string partID = sourceRow.Field<string>(binField.RelatedFieldsAndCurrentFieldArray[0], rowVersion);
			string revisionID = sourceRow.Field<string>(binField.RelatedFieldsAndCurrentFieldArray[1], rowVersion);
			string warehouseID = sourceRow.Field<string>(binField.RelatedFieldsAndCurrentFieldArray[2], rowVersion);
			string binID = sourceRow.Field<string>(binField.FieldName, rowVersion);
			decimal qtyChange = ((!backoutQty) ? Convert.ToDecimal(sourceRow[base.Field.FieldName, rowVersion]) : (-Convert.ToDecimal(sourceRow[base.Field.FieldName, rowVersion])));
			if (ReverseSign)
			{
				qtyChange *= -1m;
			}
			UpdateQuantity(database, transaction, partID, revisionID, warehouseID, binID, qtyChange, sourceRow, rowVersion);
			part.RefreshPartAllocations(database, transaction, partID, revisionID);
		}
	}

	public override void LoadComplete(FieldCollection fields, bool allowEditing)
	{
		if (PartBinField.Length != 0 && base.Field.DataDictionary != null && allowEditing)
		{
			base.LoadComplete(fields, add: true);
			if (jobField == null && RelatedJobField.Length != 0)
			{
				jobField = base.Field.BindingSource.Fields[RelatedJobField];
			}
			if (jobStatusField == null && RelatedJobStatusField.Length != 0)
			{
				jobStatusField = base.Field.BindingSource.Fields[RelatedJobStatusField];
			}
			binField = base.Field.BindingSource.Fields[PartBinField];
			base.Field.BindingSource.SaveDataStarted += BindingSource_SaveDataStarted;
			base.Field.BindingSource.RowUpdateAddAfter += BindingSource_RowUpdateAddAfter;
			base.Field.BindingSource.RowUpdateSaveAfter += BindingSource_RowUpdateSaveAfter;
			base.Field.BindingSource.RowUpdateDeleteBefore += BindingSource_RowUpdateDeleteBefore;
		}
		else
		{
			base.LoadComplete(fields, add: false);
		}
	}

	protected virtual void UpdateQuantity(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, decimal qtyChange, DataRow sourceRow, DataRowVersion rowVersion)
	{
		if (string.IsNullOrWhiteSpace(BinQuantityField))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("UPDATE PartBins SET " + BinQuantityField + " = " + BinQuantityField + " + @QtyChange WHERE imbPartID = @PartID And imbPartRevisionID = @RevisionID AND imbWarehouseID = @WarehouseID AND imbPartBinID = @BinID");
		sqlCommand.Parameters.Add(new SqlParameter("@QtyChange", SqlDbType.Decimal)).Value = qtyChange;
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
		if (database.ExecuteCommand(sqlCommand, transaction) != 0)
		{
			return;
		}
		SqlCommand sqlCommand2 = database.NewSqlCommand("Select imrPartRevisionID From PartRevisions Where imrPartID = @Part And imrPartRevisionID = @Revision");
		sqlCommand2.Parameters.Add(new SqlParameter("@Part", SqlDbType.NVarChar)).Value = partID;
		sqlCommand2.Parameters.Add(new SqlParameter("@Revision", SqlDbType.NVarChar)).Value = revisionID;
		if (database.ExecuteScalar(sqlCommand2, transaction) != null && !string.IsNullOrWhiteSpace(warehouseID))
		{
			SqlCommand sqlCommand3 = database.NewSqlCommand("INSERT INTO PartWarehouseLocations (imlPartID,imlPartRevisionID,imlPartWarehouseID) SELECT imrPartID,imrPartRevisionID,@WarehouseID FROM PartRevisions WHERE imrPartID = @PartID And imrPartRevisionID = @RevisionID And imrPartID+imrPartRevisionID NOT IN (SELECT imlPartID+imlPartRevisionID FROM PartWarehouseLocations WHERE imlPartID = @PartID And imlPartRevisionID = @RevisionID And imlPartWarehouseID = @WarehouseID)");
			sqlCommand3.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand3.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
			sqlCommand3.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
			database.ExecuteCommand(sqlCommand3, transaction);
			if (!string.IsNullOrWhiteSpace(binID))
			{
				sqlCommand3 = database.NewSqlCommand("INSERT INTO PartBins (imbPartID,imbPartRevisionID,imbWarehouseID,imbPartBinID,imbConversionFactor) VALUES (@PartID,@RevisionID,@WarehouseID,@BinID,1)");
				sqlCommand3.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				sqlCommand3.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
				sqlCommand3.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
				sqlCommand3.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
				database.ExecuteCommand(sqlCommand3, transaction);
			}
		}
	}

	private void AddCurrentValues(RowUpdateEventArgs e)
	{
		string value = e.Row.Field<string>(binField.RelatedFieldsAndCurrentFieldArray[0]);
		decimal num = Convert.ToDecimal(e.Row[base.Field.FieldName]);
		if (!string.IsNullOrWhiteSpace(value) && num != 0m)
		{
			ProcessPartQuantityChange(e.Database, e.Row, DataRowVersion.Current, e.SqlTransaction, backoutQty: false);
		}
	}

	private void RemoveOriginalValues(RowUpdateEventArgs e)
	{
		string value = e.Row.Field<string>(binField.RelatedFieldsAndCurrentFieldArray[0], DataRowVersion.Original);
		decimal num = Convert.ToDecimal(e.Row[base.Field.FieldName, DataRowVersion.Original]);
		if (!string.IsNullOrWhiteSpace(value) && num != 0m)
		{
			ProcessPartQuantityChange(e.Database, e.Row, DataRowVersion.Original, e.SqlTransaction, backoutQty: true);
		}
	}

	private void BindingSource_RowUpdateDeleteBefore(object sender, RowUpdateEventArgs e)
	{
		RemoveOriginalValues(e);
	}

	private bool isRowChanged(DataRow row)
	{
		string[] relatedFieldsAndCurrentFieldArray = binField.RelatedFieldsAndCurrentFieldArray;
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
		if (jobField != null)
		{
			relatedFieldsAndCurrentFieldArray = jobField.RelatedFieldsAndCurrentFieldArray;
			foreach (string columnName2 in relatedFieldsAndCurrentFieldArray)
			{
				if (!row[columnName2].ToString().Equals(row[columnName2, DataRowVersion.Original].ToString(), StringComparison.CurrentCultureIgnoreCase))
				{
					return true;
				}
			}
		}
		if (jobStatusField != null && !row.Field<bool>(jobStatusField.FieldName).Equals(row.Field<bool>(jobStatusField.FieldName, DataRowVersion.Original)))
		{
			return true;
		}
		if (!string.IsNullOrWhiteSpace(base.Field.Table.DocumentPlantIdField) && !row.Field<string>(base.Field.Table.DocumentPlantIdField).Equals(row.Field<string>(base.Field.Table.DocumentPlantIdField, DataRowVersion.Original)))
		{
			return true;
		}
		if (row.Table.Columns.Contains(base.Field.Table.FieldPrefix + "Posted") && !row.Field<bool>(base.Field.Table.FieldPrefix + "Posted").Equals(row.Field<bool>(base.Field.Table.FieldPrefix + "Posted", DataRowVersion.Original)))
		{
			return true;
		}
		if (row.Table.Columns.Contains(base.Field.Table.FieldPrefix + "PostedToGL") && !row.Field<bool>(base.Field.Table.FieldPrefix + "PostedToGL").Equals(row.Field<bool>(base.Field.Table.FieldPrefix + "PostedToGL", DataRowVersion.Original)))
		{
			return true;
		}
		if (row.Table.Columns.Contains(base.Field.Table.FieldPrefix + "DeliveryType") && !row.Field<byte>(base.Field.Table.FieldPrefix + "DeliveryType").Equals(row.Field<byte>(base.Field.Table.FieldPrefix + "DeliveryType", DataRowVersion.Original)))
		{
			return true;
		}
		if (row.Table.Columns.Contains(base.Field.Table.FieldPrefix + "ShippedComplete") && !row.Field<bool>(base.Field.Table.FieldPrefix + "ShippedComplete").Equals(row.Field<bool>(base.Field.Table.FieldPrefix + "ShippedComplete", DataRowVersion.Original)))
		{
			return true;
		}
		return false;
	}

	public bool checkParms(DataRow sourceRow, DataRowVersion rowVersion, string sourceTableName, string fieldPrefix, string parms)
	{
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "Posted") && sourceRow.Field<bool>(fieldPrefix + "Posted", rowVersion).Equals(obj: false))
		{
			return true;
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "PostedToGL") && sourceRow.Field<bool>(fieldPrefix + "PostedToGL", rowVersion).Equals(obj: false))
		{
			return true;
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
			if (!sourceRow.Field<byte>(fieldPrefix + "InspectionType", rowVersion).Equals(1))
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

	private void BindingSource_RowUpdateSaveAfter(object sender, RowUpdateEventArgs e)
	{
		if (isRowChanged(e.Row))
		{
			RemoveOriginalValues(e);
			AddCurrentValues(e);
		}
	}

	private void BindingSource_RowUpdateAddAfter(object sender, RowUpdateEventArgs e)
	{
		AddCurrentValues(e);
	}

	private void BindingSource_SaveDataStarted(object sender, SaveDataStartedEventArgs e)
	{
		_ = base.Field.BindingSource.ChangedRows;
	}
}
