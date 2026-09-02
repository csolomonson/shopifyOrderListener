using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class InspectionDefinition : FieldExtension
{
	public string BinQuantityField = string.Empty;

	private FieldDefinition jobField;

	private FieldDefinition jobStatusField;

	private FieldDefinition binField;

	public InspectionDefinition()
	{
	}

	public InspectionDefinition(M1BindingSource bs, string qtyFieldName, string binFieldName)
	{
		PartBinField = binFieldName;
		binField = bs.Fields[binFieldName];
		FieldName = qtyFieldName;
		base.Field = bs.Fields[qtyFieldName];
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
			base.Field.BindingSource.RowUpdateAddBefore += BindingSource_RowUpdateAddBefore;
			base.Field.BindingSource.RowUpdateSaveAfter += BindingSource_RowUpdateSaveAfter;
			base.Field.BindingSource.RowUpdateDeleteBefore += BindingSource_RowUpdateDeleteBefore;
		}
		else
		{
			base.LoadComplete(fields, add: false);
		}
	}

	private void AddCurrentValues(RowUpdateEventArgs e)
	{
		string value = e.Row.Field<string>(binField.RelatedFieldsAndCurrentFieldArray[0]);
		decimal num = Convert.ToDecimal(e.Row[base.Field.FieldName]);
		if (!string.IsNullOrWhiteSpace(value) && num != 0m)
		{
			CreateInspection(e.Database, e.Row, DataRowVersion.Current, e.SqlTransaction);
		}
	}

	private void RemoveOriginalValues(RowUpdateEventArgs e)
	{
		string value = e.Row.Field<string>(binField.RelatedFieldsAndCurrentFieldArray[0], DataRowVersion.Original);
		decimal num = Convert.ToDecimal(e.Row[base.Field.FieldName, DataRowVersion.Original]);
		if (!string.IsNullOrWhiteSpace(value) && num != 0m)
		{
			DeleteInspection(e.Database, e.Row, DataRowVersion.Original, e.SqlTransaction);
		}
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
		return false;
	}

	private void BindingSource_RowUpdateDeleteBefore(object sender, RowUpdateEventArgs e)
	{
		RemoveOriginalValues(e);
	}

	private void BindingSource_RowUpdateSaveAfter(object sender, RowUpdateEventArgs e)
	{
		if (isRowChanged(e.Row))
		{
			AddCurrentValues(e);
		}
	}

	private void BindingSource_RowUpdateAddAfter(object sender, RowUpdateEventArgs e)
	{
		AddCurrentValues(e);
	}

	private void BindingSource_RowUpdateAddBefore(object sender, RowUpdateEventArgs e)
	{
	}

	private void BindingSource_SaveDataStarted(object sender, SaveDataStartedEventArgs e)
	{
		_ = base.Field.BindingSource.ChangedRows;
	}

	public void CreateInspection(M1Database database, DataRow sourceRow, DataRowVersion rowVersion, SqlTransaction transaction)
	{
		try
		{
			if ((sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "Posted") && sourceRow.Field<bool>(base.Field.Table.FieldPrefix + "Posted", rowVersion).Equals(obj: false)) || (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "PostedToGL") && sourceRow.Field<bool>(base.Field.Table.FieldPrefix + "PostedToGL", rowVersion).Equals(obj: false)))
			{
				return;
			}
			try
			{
				if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "InInspection") && sourceRow.Table.Columns.Contains(base.Field.Table.UniqueField))
				{
					database.ExecuteScalar("Update " + base.Field.BindingSource.PrimaryTable.TableName + " Set " + base.Field.Table.FieldPrefix + "InInspection = 1 Where " + base.Field.Table.UniqueField + " = " + sourceRow[base.Field.Table.UniqueField, rowVersion].ToSql(), transaction);
				}
			}
			catch
			{
			}
			DateTime value = ((base.Field.Table.DocumentDateField.Length == 0 || !base.Field.Table.GetDocumentDate(database, sourceRow, transaction, rowVersion).HasValue) ? DateTime.Now : Convert.ToDateTime(base.Field.Table.GetDocumentDate(database, sourceRow, transaction, rowVersion)));
			M1BindingSource m1BindingSource = new M1BindingSource(database, transaction);
			M1BindingSource parentBindingSource = base.Field.Table.GetParentBindingSource(sourceRow);
			DataRow dataRow = null;
			Guid guid = Guid.Empty;
			if (parentBindingSource != null)
			{
				dataRow = parentBindingSource.CurrentAsDataRow;
				if (dataRow.Table.Columns.Contains(parentBindingSource.PrimaryTable.FieldPrefix + "UniqueID"))
				{
					guid = dataRow.Field<Guid>(parentBindingSource.PrimaryTable.FieldPrefix + "UniqueID", rowVersion);
				}
			}
			DataRow dataRow2 = null;
			m1BindingSource.LoadDefinition(string.Empty, "Inspections", null, true, loadDataNow: false);
			m1BindingSource.ClearCache();
			m1BindingSource.NavigateTo(database, "qapSourceTableUniqueID = " + M1Util.ConvertToSql(guid));
			DataRow currentAsDataRow = m1BindingSource.CurrentAsDataRow;
			if (currentAsDataRow != null)
			{
				dataRow2 = currentAsDataRow;
			}
			else
			{
				dataRow2 = m1BindingSource.AddNew() as DataRow;
				m1BindingSource.SetKeyToNextAvailable(dataRow2);
				if (dataRow != null)
				{
					if (dataRow.Table.Columns.Contains(parentBindingSource.PrimaryTable.FieldPrefix + "ProjectID"))
					{
						dataRow2.SetField("qapProjectID", dataRow.Field<string>(parentBindingSource.PrimaryTable.FieldPrefix + "ProjectID", rowVersion));
					}
					Employee employee = new Employee();
					dataRow2.SetField("qapOpenedByEmployeeID", employee.GetEmployeeIDforUserId(database, database.User.ID));
					dataRow2.SetField("qapOpenedDate", value);
					dataRow2["qapSourceTableName"] = parentBindingSource.PrimaryTable.TableName;
					dataRow2["qapSourceTableUniqueID"] = guid;
				}
			}
			if (base.Field.Table.GetDocumentPlantID(database, sourceRow, transaction, rowVersion) != null)
			{
				dataRow2["qapPlantID"] = base.Field.Table.GetDocumentPlantID(database, sourceRow, transaction, rowVersion);
			}
			M1BindingSource childBindingSource = m1BindingSource.PrimaryTable.GetChildBindingSource("InspectionLines");
			childBindingSource.ClearCache();
			childBindingSource.NavigateTo(database, "qalSourceTableUniqueID = " + M1Util.ConvertToSql(sourceRow[base.Field.Table.UniqueField, rowVersion]));
			currentAsDataRow = childBindingSource.CurrentAsDataRow;
			if (currentAsDataRow != null)
			{
				return;
			}
			childBindingSource.ClearCache();
			childBindingSource.NavigateTo(database, "qalInspectionID = " + M1Util.ConvertToSql(dataRow2.Field<string>("qapInspectionID")));
			DataRow dataRow3 = childBindingSource.AddNew() as DataRow;
			childBindingSource.SetKeyToNextAvailable(dataRow3);
			childBindingSource.SetPositionByDataRow(dataRow3);
			M1BindingSource m1BindingSource2 = null;
			string text = sourceRow.Field<string>(binField.RelatedFieldsAndCurrentFieldArray[0], rowVersion);
			string text2 = sourceRow.Field<string>(binField.RelatedFieldsAndCurrentFieldArray[1], rowVersion);
			string value2 = sourceRow.Field<string>(binField.RelatedFieldsAndCurrentFieldArray[2], rowVersion);
			string value3 = sourceRow.Field<string>(binField.FieldName, rowVersion);
			dataRow3.SetField("qalPartID", text);
			dataRow3.SetField("qalPartRevisionID", text2);
			dataRow3.SetField("qalPartWarehouseLocationID", value2);
			dataRow3.SetField("qalPartBinID", value3);
			if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "KitPart") && sourceRow.Field<bool>(base.Field.Table.FieldPrefix + "KitPart", rowVersion).Equals(obj: true))
			{
				string text3 = "";
				text3 = ((!base.Field.Table.TopLevelTable.Equals("Jobs", StringComparison.CurrentCultureIgnoreCase)) ? (M1Util.GetSingularOfTableName(base.Field.Table.TopLevelTable) + "Components") : (M1Util.GetSingularOfTableName(base.Field.Table.TableName) + "Components"));
				if (!DoesTableExist(base.Field.BindingSource.GetDatabaseForRow(sourceRow), base.Field.BindingSource.Transaction, text3))
				{
					return;
				}
				M1BindingSource m1BindingSource3 = null;
				m1BindingSource3 = base.Field.Table.GetChildBindingSource(text3);
				m1BindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("InspectionComponents");
				foreach (DataRow row in m1BindingSource3.GetDataView(sourceRow).ToTable().Rows)
				{
					if (m1BindingSource2.Query.DataView.Table.Select("qamSourceTableUniqueID = " + row.Field<Guid>(m1BindingSource3.PrimaryTable.FieldPrefix + "UniqueID", rowVersion).ToLinq()).Length == 0)
					{
						DataRow dataRow5 = m1BindingSource2.AddNew() as DataRow;
						m1BindingSource2.SetKeyToNextAvailable(dataRow5);
						if (row.Table.Columns.Contains(m1BindingSource3.PrimaryTable.FieldPrefix + "PartID"))
						{
							dataRow5.SetField("qamPartID", row.Field<string>(m1BindingSource3.PrimaryTable.FieldPrefix + "PartID", rowVersion));
						}
						if (row.Table.Columns.Contains(m1BindingSource3.PrimaryTable.FieldPrefix + "PartRevisionID"))
						{
							dataRow5.SetField("qamPartRevisionID", row.Field<string>(m1BindingSource3.PrimaryTable.FieldPrefix + "PartRevisionID", rowVersion));
						}
						if (row.Table.Columns.Contains(m1BindingSource3.PrimaryTable.FieldPrefix + "PartWarehouseLocationID"))
						{
							dataRow5.SetField("qamPartWarehouseLocationID", row.Field<string>(m1BindingSource3.PrimaryTable.FieldPrefix + "PartWarehouseLocationID", rowVersion));
						}
						if (row.Table.Columns.Contains(m1BindingSource3.PrimaryTable.FieldPrefix + "PartBinID"))
						{
							dataRow5.SetField("qamPartBinID", row.Field<string>(m1BindingSource3.PrimaryTable.FieldPrefix + "PartBinID", rowVersion));
						}
						if (row.Table.Columns.Contains(m1BindingSource3.PrimaryTable.FieldPrefix + "QuantityPerParent"))
						{
							dataRow5.SetField("qamQuantityPerParent", row.Field<decimal>(m1BindingSource3.PrimaryTable.FieldPrefix + "QuantityPerParent", rowVersion));
						}
						if (row.Table.Columns.Contains(m1BindingSource3.PrimaryTable.FieldPrefix + "AdditionalQuantity"))
						{
							dataRow5.SetField("qamAdditionalQuantity", row.Field<decimal>(m1BindingSource3.PrimaryTable.FieldPrefix + "AdditionalQuantity", rowVersion));
						}
						if (row.Table.Columns.Contains(m1BindingSource3.PrimaryTable.FieldPrefix + "UnitOfMeasure"))
						{
							dataRow5.SetField("qamUnitOfMeasure", row.Field<string>(m1BindingSource3.PrimaryTable.FieldPrefix + "UnitOfMeasure", rowVersion));
						}
						if (row.Table.Columns.Contains(m1BindingSource3.PrimaryTable.FieldPrefix + "Description"))
						{
							dataRow5.SetField("qamDescription", row.Field<string>(m1BindingSource3.PrimaryTable.FieldPrefix + "Description", rowVersion));
						}
						if (row.Table.Columns.Contains(m1BindingSource3.PrimaryTable.FieldPrefix + "Weight"))
						{
							dataRow5.SetField("qamWeight", row.Field<decimal>(m1BindingSource3.PrimaryTable.FieldPrefix + "Weight", rowVersion));
						}
						if (row.Table.Columns.Contains(m1BindingSource3.PrimaryTable.FieldPrefix + "JobID"))
						{
							dataRow5.SetField("qamJobID", row.Field<string>(m1BindingSource3.PrimaryTable.FieldPrefix + "JobID", rowVersion));
						}
						if (row.Table.Columns.Contains(m1BindingSource3.PrimaryTable.FieldPrefix + "JobAssemblyID"))
						{
							dataRow5.SetField("qamJobAssemblyID", row.Field<int>(m1BindingSource3.PrimaryTable.FieldPrefix + "JobAssemblyID", rowVersion));
						}
						if (row.Table.Columns.Contains(m1BindingSource3.PrimaryTable.FieldPrefix + "JobMaterialID"))
						{
							dataRow5.SetField("qamJobMaterialID", row.Field<int>(m1BindingSource3.PrimaryTable.FieldPrefix + "JobMaterialID", rowVersion));
						}
						if (row.Table.Columns.Contains(m1BindingSource3.PrimaryTable.FieldPrefix + "JobMaterialComponentID"))
						{
							dataRow5.SetField("qamJobMaterialComponentID", row.Field<int>(m1BindingSource3.PrimaryTable.FieldPrefix + "JobMaterialComponentID", rowVersion));
						}
						if (row.Table.Columns.Contains(m1BindingSource3.PrimaryTable.FieldPrefix + "UniqueID"))
						{
							dataRow5.SetField("qamSourceTableName", text3);
							dataRow5.SetField("qamSourceTableUniqueID", row.Field<Guid>(m1BindingSource3.PrimaryTable.FieldPrefix + "UniqueID", rowVersion));
						}
					}
				}
			}
			if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "PartShortDescription"))
			{
				dataRow3.SetField("qalPartShortDescription", sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PartShortDescription", rowVersion));
			}
			else if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "Description"))
			{
				dataRow3.SetField("qalPartShortDescription", sourceRow.Field<string>(base.Field.Table.FieldPrefix + "Description", rowVersion));
			}
			if (string.IsNullOrWhiteSpace(dataRow3.Field<string>("qalPartShortDescription")))
			{
				dataRow3.SetField("qalPartShortDescription", text);
			}
			if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "PartLongDescriptionText"))
			{
				dataRow3.SetField("qalPartLongDescriptionText", sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PartLongDescriptionText", rowVersion));
			}
			if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "PartLongDescriptionRTF"))
			{
				dataRow3.SetField("qalPartLongDescriptionRTF", sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PartLongDescriptionRTF", rowVersion));
			}
			if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "InspectionNotesRTF"))
			{
				dataRow3.SetField("qalInspectionNotesRTF", sourceRow.Field<string>(base.Field.Table.FieldPrefix + "InspectionNotesRTF", rowVersion));
			}
			if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "InspectionNotesText"))
			{
				dataRow3.SetField("qalInspectionNotesText", sourceRow.Field<string>(base.Field.Table.FieldPrefix + "InspectionNotesText", rowVersion));
			}
			if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "InventoryUnitOfMeasure"))
			{
				dataRow3.SetField("qalUnitOfMeasure", sourceRow.Field<string>(base.Field.Table.FieldPrefix + "InventoryUnitOfMeasure", rowVersion));
			}
			if (dataRow != null)
			{
				if (dataRow.Table.Columns.Contains(parentBindingSource.PrimaryTable.FieldPrefix + "SupplierOrganizationID"))
				{
					dataRow3.SetField("qalSupplierOrganizationID", dataRow.Field<string>(parentBindingSource.PrimaryTable.FieldPrefix + "SupplierOrganizationID", rowVersion));
				}
				if (dataRow.Table.Columns.Contains(parentBindingSource.PrimaryTable.FieldPrefix + "PurchaseLocationID"))
				{
					dataRow3.SetField("qalPurchaseLocationID", dataRow.Field<string>(parentBindingSource.PrimaryTable.FieldPrefix + "PurchaseLocationID", rowVersion));
				}
			}
			if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "ProjectID"))
			{
				dataRow3.SetField("qalProjectID", sourceRow.Field<string>(base.Field.Table.FieldPrefix + "ProjectID", rowVersion));
			}
			if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "ProjectAreaID"))
			{
				dataRow3.SetField("qalProjectAreaID", sourceRow.Field<string>(base.Field.Table.FieldPrefix + "ProjectAreaID", rowVersion));
			}
			if (base.Field.Table.TableName.Equals("RECEIPTLINES", StringComparison.CurrentCultureIgnoreCase) && sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "JobID") && !string.IsNullOrWhiteSpace(sourceRow.Field<string>(base.Field.Table.FieldPrefix + "JobID", rowVersion)) && (sourceRow.Field<byte>(base.Field.Table.FieldPrefix + "JobType", rowVersion) == 2 || sourceRow.Field<byte>(base.Field.Table.FieldPrefix + "JobType", rowVersion) == 1))
			{
				dataRow3.SetField("qalUnitCost", sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "InventoryUnitCost", rowVersion) + Math.Round(sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "SetupCharge", rowVersion) / sourceRow.Field<decimal>(base.Field.Table.FieldPrefix + "PurchaseQuantityReceived", rowVersion), 5));
			}
			else
			{
				PartCost partCosts = new Part().GetPartCosts(database, transaction, text, text2);
				if (partCosts != null)
				{
					dataRow3.SetField("qalUnitCost", partCosts.LaborCost + partCosts.MaterialCost + partCosts.OverheadCost + partCosts.SubcontractCost);
				}
			}
			if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "JobID"))
			{
				if (!string.IsNullOrWhiteSpace(sourceRow.Field<string>(base.Field.Table.FieldPrefix + "JobID", rowVersion)))
				{
					dataRow3.SetField("qalJobID", sourceRow.Field<string>(base.Field.Table.FieldPrefix + "JobID", rowVersion));
					if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "JobAssemblyID"))
					{
						dataRow3.SetField("qalJobAssemblyID", sourceRow.Field<int>(base.Field.Table.FieldPrefix + "JobAssemblyID", rowVersion));
					}
					if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "JobType"))
					{
						switch (sourceRow.Field<byte>(base.Field.Table.FieldPrefix + "JobType", rowVersion))
						{
						case 1:
							dataRow3["qalInspectionType"] = 2;
							if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "JobMaterialID"))
							{
								dataRow3.SetField("qalJobMaterialID", sourceRow.Field<int>(base.Field.Table.FieldPrefix + "JobMaterialID", rowVersion));
							}
							break;
						case 2:
							dataRow3["qalInspectionType"] = 2;
							if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "JobOperationID"))
							{
								dataRow3.SetField("qalJobOperationID", sourceRow.Field<int>(base.Field.Table.FieldPrefix + "JobOperationID", rowVersion));
								SqlCommand sqlCommand = database.NewSqlCommand("Update JobOperations set jmoInspectionStatus = 1  where jmojobid = @JobID and jmojobassemblyid = @Asm and jmoJobOperationID = @JobOp");
								sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = dataRow3["qalJobID"];
								sqlCommand.Parameters.Add(new SqlParameter("@Asm", SqlDbType.Int)).Value = dataRow3["qalJobAssemblyID"];
								sqlCommand.Parameters.Add(new SqlParameter("@JobOp", SqlDbType.Int)).Value = dataRow3["qalJobOperationID"];
								database.ExecuteCommand(sqlCommand, transaction);
							}
							break;
						case 3:
							dataRow3["qalInspectionType"] = 3;
							break;
						}
						dataRow3.SetField("qalJobType", sourceRow.Field<byte>(base.Field.Table.FieldPrefix + "JobType", rowVersion));
					}
					else
					{
						dataRow3["qalInspectionType"] = 3;
						dataRow3["qalJobType"] = 3;
					}
				}
				else
				{
					dataRow3["qalInspectionType"] = 1;
				}
			}
			else
			{
				dataRow3["qalInspectionType"] = 1;
			}
			dataRow3["qalQuantityToInspect"] = sourceRow[base.Field.FieldName, rowVersion];
			dataRow3.SetField("qalStatus", "P");
			dataRow3["qalSourceTableName"] = base.Field.Table.TableName;
			dataRow3["qalSourceTableUniqueID"] = sourceRow[base.Field.Table.UniqueField, rowVersion];
			m1BindingSource.SaveData();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void DeleteInspection(M1Database database, DataRow sourceRow, DataRowVersion rowVersion, SqlTransaction transaction)
	{
		try
		{
			M1BindingSource m1BindingSource = new M1BindingSource(database, transaction);
			m1BindingSource.LoadDefinition(string.Empty, "InspectionLines", null, true, loadDataNow: false);
			m1BindingSource.ClearCache();
			m1BindingSource.NavigateTo(database, "qalSourceTableUniqueID = " + M1Util.ConvertToSql(sourceRow[base.Field.Table.UniqueField, rowVersion]));
			DataRow currentAsDataRow = m1BindingSource.CurrentAsDataRow;
			if (currentAsDataRow != null)
			{
				if (currentAsDataRow.Field<int>("qalJobOperationID", rowVersion) != 0)
				{
					SqlCommand sqlCommand = database.NewSqlCommand("Update JobOperations set jmoInspectionStatus = 0  where jmojobid = @JobID and jmojobassemblyid = @Asm and jmoJobOperationID = @JobOp");
					sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = currentAsDataRow.Field<string>("qalJobID", rowVersion);
					sqlCommand.Parameters.Add(new SqlParameter("@Asm", SqlDbType.Int)).Value = currentAsDataRow.Field<short>("qalJobAssemblyID", rowVersion);
					sqlCommand.Parameters.Add(new SqlParameter("@JobOp", SqlDbType.Int)).Value = currentAsDataRow.Field<short>("qalJobOperationID", rowVersion);
					database.ExecuteCommand(sqlCommand, transaction);
				}
				currentAsDataRow.Delete();
				m1BindingSource.SaveData();
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void UpdateInspection(M1Database database, DataRow sourceRow, DataRowVersion rowVersion, SqlTransaction transaction)
	{
		try
		{
			M1BindingSource m1BindingSource = new M1BindingSource(database, transaction);
			m1BindingSource.LoadDefinition(string.Empty, "InspectionLines", null, true, loadDataNow: false);
			m1BindingSource.ClearCache();
			m1BindingSource.NavigateTo(database, "qalSourceTableUniqueID = " + M1Util.ConvertToSql(sourceRow[base.Field.Table.UniqueField, rowVersion]));
			DataRow currentAsDataRow = m1BindingSource.CurrentAsDataRow;
			if (currentAsDataRow != null)
			{
				currentAsDataRow["qalQuantityToInspect"] = sourceRow[base.Field.FieldName, rowVersion];
				m1BindingSource.SaveData();
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private bool DoesTableExist(M1Database database, SqlTransaction transaction, string tableName)
	{
		SqlCommand sqlCommand = new SqlCommand("SELECT Table_name FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = @TableName");
		sqlCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = tableName;
		return database.ExecuteScalar(sqlCommand, transaction) != null;
	}
}
