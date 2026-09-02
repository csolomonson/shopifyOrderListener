using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class Inspection
{
	public bool InspectorApprovedCheck(M1Database database, SqlTransaction transaction, string inspector, bool inspComplete)
	{
		if (inspComplete)
		{
			return true;
		}
		if (!string.IsNullOrWhiteSpace(new AppAxProduction(database).InspectorID))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(count(*), 0) From EmployeeQAApprovals Where lmbEmployeeID = @EmployeeID");
			sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar)).Value = inspector;
			object obj = database.ExecuteScalar(sqlCommand, transaction);
			if (obj != DBNull.Value && obj != null && Convert.ToInt16(obj) > 0)
			{
				return true;
			}
		}
		return false;
	}

	public bool ConvertQualityRegistersToInspections(M1Database database, SqlTransaction transaction, List<KeyValuePair<string, string>> customFields = null)
	{
		DataTable dataTable = database.GetDataTable("select * from QualityRegisters where qanStatus = 0 and qanClosed = 0", transaction);
		if (dataTable.Rows.Count == 0)
		{
			return false;
		}
		int num = 1000;
		int num2 = dataTable.Rows.Count / num;
		IEnumerable<DataRow> qualityRegisterData = dataTable.AsEnumerable().Take(num);
		if (!ConvertBlockOfQualityRegistersToInspections(database, transaction, qualityRegisterData, customFields))
		{
			return false;
		}
		int num3 = 0;
		for (int i = 1; i <= num2; i++)
		{
			num3 += num;
			IEnumerable<DataRow> qualityRegisterData2 = dataTable.AsEnumerable().Skip(num3).Take(num);
			if (!ConvertBlockOfQualityRegistersToInspections(database, transaction, qualityRegisterData2, customFields))
			{
				return false;
			}
		}
		return true;
	}

	public bool ConvertBlockOfQualityRegistersToInspections(M1Database database, SqlTransaction transaction, IEnumerable<DataRow> qualityRegisterData, List<KeyValuePair<string, string>> customFields = null)
	{
		M1BindingSource m1BindingSource = new M1BindingSource(database, transaction);
		m1BindingSource.LoadDefinition(string.Empty, "Inspections", null, true, loadDataNow: false);
		m1BindingSource.ClearCache();
		M1BindingSource childBindingSource = m1BindingSource.PrimaryTable.GetChildBindingSource("InspectionLines");
		M1BindingSource m1BindingSource2 = new M1BindingSource(database, transaction);
		m1BindingSource2.LoadDefinition(string.Empty, "InspectionLines", null, true, loadDataNow: false);
		string text = string.Empty;
		foreach (DataRow qualityRegisterDatum in qualityRegisterData)
		{
			m1BindingSource2.ClearCache();
			m1BindingSource2.NavigateTo(database, "qalQualityRegisterID = " + M1Util.ConvertToSql(qualityRegisterDatum.Field<string>("qanQualityRegisterID")));
			if (m1BindingSource2.CurrentAsDataRow != null)
			{
				continue;
			}
			DataRow dataRow = m1BindingSource.AddNew() as DataRow;
			m1BindingSource.SetKeyToNextAvailable(dataRow);
			dataRow.SetField("qapPlantID", qualityRegisterDatum.Field<string>("qanPlantID"));
			dataRow.SetField("qapPlantDepartmentID", qualityRegisterDatum.Field<string>("qanPlantDepartmentID"));
			dataRow.SetField("qapProjectID", qualityRegisterDatum.Field<string>("qanProjectID"));
			dataRow.SetField("qapOpenedByEmployeeID", qualityRegisterDatum.Field<string>("qanOpenedByEmployeeID"));
			dataRow.SetField("qapOpenedDate", qualityRegisterDatum.Field<DateTime?>("qanOpenedDate"));
			dataRow.SetField("qapCreatedBy", qualityRegisterDatum.Field<string>("qanCreatedBy"));
			dataRow.SetField("qapCreatedDate", qualityRegisterDatum.Field<DateTime?>("qanCreatedDate"));
			DataRow dataRow2 = childBindingSource.AddNew() as DataRow;
			dataRow2.SetField("qalInspectionID", dataRow.Field<string>("qapInspectionID"));
			dataRow2.SetField("qalInspectionLineID", (short)1);
			dataRow2.SetField("qalPartID", qualityRegisterDatum.Field<string>("qanPartID"));
			dataRow2.SetField("qalPartRevisionID", qualityRegisterDatum.Field<string>("qanPartRevisionID"));
			dataRow2.SetField("qalPartWarehouseLocationID", qualityRegisterDatum.Field<string>("qanPartWarehouseLocationID"));
			dataRow2.SetField("qalPartBinID", qualityRegisterDatum.Field<string>("qanPartBinID"));
			dataRow2.SetField("qalPartShortDescription", qualityRegisterDatum.Field<string>("qanpartshortdescription"));
			dataRow2.SetField("qalPartLongDescriptionRTF", qualityRegisterDatum.Field<string>("qanLongDescriptionRTF"));
			dataRow2.SetField("qalPartLongDescriptionText", qualityRegisterDatum.Field<string>("qanLongDescriptionText"));
			dataRow2.SetField("qalUnitOfMeasure", qualityRegisterDatum.Field<string>("qanInventoryUnitOfMeasure"));
			dataRow2.SetField("qalQuantitytoinspect", qualityRegisterDatum.Field<decimal>("qanRegisterQuantity"));
			dataRow2.SetField("qalInspectionNotesRTF", qualityRegisterDatum.Field<string>("qanInspectionNotesRTF"));
			dataRow2.SetField("qalInspectionNotesText", qualityRegisterDatum.Field<string>("qanInspectionNotesText"));
			dataRow2.SetField("qalFirstOffInspection", qualityRegisterDatum.Field<bool>("qanFirstOffInspection"));
			dataRow2.SetField("qalSupplierOrganizationID", qualityRegisterDatum.Field<string>("qanSupplierOrganizationID"));
			dataRow2.SetField("qalPurchaseLocationID", qualityRegisterDatum.Field<string>("qanPurchaseLocationID"));
			dataRow2.SetField("qalProjectID", qualityRegisterDatum.Field<string>("qanProjectID"));
			dataRow2.SetField("qalProjectAreaID", qualityRegisterDatum.Field<string>("qanProjectAreaID"));
			if (qualityRegisterDatum.Field<decimal>("qanUnitCost") == 0m)
			{
				PartCost partCosts = new Part().GetPartCosts(database, transaction, qualityRegisterDatum.Field<string>("qanPartID"), qualityRegisterDatum.Field<string>("qanPartRevisionID"));
				if (partCosts != null)
				{
					dataRow2.SetField("qalUnitCost", partCosts.LaborCost + partCosts.MaterialCost + partCosts.OverheadCost + partCosts.SubcontractCost);
				}
			}
			else
			{
				dataRow2.SetField("qalUnitCost", qualityRegisterDatum.Field<decimal>("qanUnitCost"));
			}
			if (!string.IsNullOrWhiteSpace(qualityRegisterDatum.Field<string>("qanJobID")))
			{
				dataRow2.SetField("qalJobID", qualityRegisterDatum.Field<string>("qanJobID"));
				dataRow2.SetField("qalJobAssemblyID", qualityRegisterDatum.Field<int>("qanJobAssemblyID"));
				dataRow2["qalInspectionType"] = 2;
				if (qualityRegisterDatum.Field<int>("qanJobMaterialID") != 0)
				{
					dataRow2.SetField("qalJobType", (byte)1);
					dataRow2.SetField("qalJobMaterialID", qualityRegisterDatum.Field<int>("qanJobMaterialID"));
				}
				else if (qualityRegisterDatum.Field<int>("qanJobOperationID") != 0)
				{
					dataRow2.SetField("qalJobType", (byte)2);
					dataRow2.SetField("qalJobOperationID", qualityRegisterDatum.Field<int>("qanJobOperationID"));
					SqlCommand sqlCommand = database.NewSqlCommand("Update JobOperations set jmoInspectionStatus = 1  where jmojobid = @JobID and jmojobassemblyid = @Asm and jmoJobOperationID = @JobOp");
					sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = dataRow2["qalJobID"];
					sqlCommand.Parameters.Add(new SqlParameter("@Asm", SqlDbType.Int)).Value = dataRow2["qalJobAssemblyID"];
					sqlCommand.Parameters.Add(new SqlParameter("@JobOp", SqlDbType.Int)).Value = dataRow2["qalJobOperationID"];
					database.ExecuteCommand(sqlCommand, transaction);
				}
				else
				{
					dataRow2["qalInspectionType"] = 3;
					dataRow2["qalJobType"] = 3;
				}
			}
			else
			{
				dataRow2["qalInspectionType"] = 1;
				dataRow2["qalJobType"] = 0;
			}
			dataRow2.SetField("qalStatus", "P");
			string value = string.Empty;
			Guid guid = Guid.Empty;
			if (!string.IsNullOrWhiteSpace(qualityRegisterDatum.Field<string>("qanReceiptID")))
			{
				SqlCommand sqlCommand2 = database.NewSqlCommand("SELECT rmlUniqueID FROM ReceiptLines WHERE rmlReceiptID = @Receipt and rmlReceiptLineID = @Line");
				sqlCommand2.Parameters.Add(new SqlParameter("@Receipt", SqlDbType.NVarChar)).Value = qualityRegisterDatum.Field<string>("qanReceiptID");
				sqlCommand2.Parameters.Add(new SqlParameter("@Line", SqlDbType.SmallInt)).Value = qualityRegisterDatum.Field<short>("qanReceiptLineID");
				DataTable dataTable = database.GetDataTable(sqlCommand2, transaction);
				if (dataTable.Rows.Count != 0)
				{
					value = "ReceiptLines";
					guid = (Guid)dataTable.Rows[0]["rmlUniqueID"];
				}
			}
			if (!string.IsNullOrWhiteSpace(qualityRegisterDatum.Field<string>("qanShipmentID")))
			{
				SqlCommand sqlCommand3 = database.NewSqlCommand("SELECT smlUniqueID FROM ShipmentLines WHERE smlShipmentID = @Shipment and smlShipmentLineID = @Line");
				sqlCommand3.Parameters.Add(new SqlParameter("@Shipment", SqlDbType.NVarChar)).Value = qualityRegisterDatum.Field<string>("qanShipmentID");
				sqlCommand3.Parameters.Add(new SqlParameter("@Line", SqlDbType.SmallInt)).Value = qualityRegisterDatum.Field<short>("qanShipmentLineID");
				DataTable dataTable2 = database.GetDataTable(sqlCommand3, transaction);
				if (dataTable2.Rows.Count != 0)
				{
					value = "ShipmentLines";
					guid = (Guid)dataTable2.Rows[0]["smlUniqueID"];
				}
			}
			if (customFields.Count != 0)
			{
				text += BuildUpdateDataCustomFields(dataRow, qualityRegisterDatum, customFields);
			}
			dataRow2["qalSourceTableName"] = value;
			dataRow2["qalSourceTableUniqueID"] = guid;
		}
		m1BindingSource.SaveData();
		if (text.Length != 0)
		{
			database.ExecuteCommand(text);
		}
		return true;
	}

	private string BuildUpdateDataCustomFields(DataRow inspection, DataRow qualityRegister, List<KeyValuePair<string, string>> customFields)
	{
		string format = "UPDATE InspectionLines SET {0} where qalInspectionID = {1}; ";
		List<string> list = new List<string>();
		if (customFields != null)
		{
			foreach (KeyValuePair<string, string> customField in customFields)
			{
				list.Add($"{customField.Value} = {M1Util.ConvertToSql(qualityRegister[customField.Key])}");
			}
		}
		string arg = string.Join(", ", list);
		return string.Format(format, arg, inspection.Field<string>("qapInspectionID"));
	}

	public DataTable GetCustomFieldsbyTable(M1Database database)
	{
		return database.GetDataTable("SELECT COLUMN_NAME, DATA_TYPE, isnull(ISNULL(CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION), 0) as LENGTH, ISNULL(NUMERIC_SCALE, 0) as SCALE, SUBSTRING(COLUMN_NAME, 1, 4) AS PREFIX FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'QualityRegisters'AND COLUMN_NAME like 'u%' ");
	}

	public bool PostInspectionCheck(M1BindingSource bindingsource)
	{
		if (bindingsource.CurrentAsDataRow != null)
		{
			DataTable dataTable = bindingsource.PrimaryTable.GetChildBindingSource("InspectionLines").GetDataTable();
			if (dataTable != null && dataTable.Rows.Count != 0 && dataTable.AsEnumerable().Sum((DataRow x) => x.Field<decimal>("qalQuantityToInspect") - (x.Field<decimal>("qalInvQuantityAccepted") + x.Field<decimal>("qalInvQuantityToScrap") + x.Field<decimal>("qalInvQuantityToReturn") + x.Field<decimal>("qalJobMatQuantityAccepted") + x.Field<decimal>("qalJobMatQuantityToScrap") + x.Field<decimal>("qalJobMatQuantityToReturn") + x.Field<decimal>("qalJobOprQuantityAccepted") + x.Field<decimal>("qalJobOprQuantityToScrap") + x.Field<decimal>("qalJobOprQuantityToReturn") + x.Field<decimal>("qalMfgReceiptQuantityAccepted") + x.Field<decimal>("qalMfgReceiptQuantityToScrap") + x.Field<decimal>("qalMfgReceiptQuantityToReturn"))) != 0m)
			{
				return false;
			}
			dataTable = null;
			DataTable dataTable2 = bindingsource.PrimaryTable.GetChildBindingSource("InspectionComponents").GetDataTable();
			if (dataTable2 != null && dataTable2.Rows.Count != 0 && dataTable2.AsEnumerable().Sum((DataRow x) => x.Field<decimal>("qamComponentQtyToInspect") - (x.Field<decimal>("qamInvQuantityAccepted") + x.Field<decimal>("qamInvQuantityToScrap") + x.Field<decimal>("qamInvQuantityToReturn") + x.Field<decimal>("qamJobMatQuantityAccepted") + x.Field<decimal>("qamJobMatQuantityToScrap") + x.Field<decimal>("qamJobMatQuantityToReturn"))) != 0m)
			{
				return false;
			}
			dataTable2 = null;
		}
		return true;
	}

	public string ValidatePartsWithInactiveBinInInspection(M1BindingSource bindingSource)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		string text = string.Empty;
		if (bindingSource.CurrentAsDataRow != null)
		{
			DataTable dataTable = bindingSource.PrimaryTable.GetChildBindingSource("InspectionLines").GetDataTable();
			if (dataTable != null && dataTable.Rows.Count != 0)
			{
				foreach (DataRow row3 in dataTable.Rows)
				{
					string text2 = row3.Field<string>("qalInspectionID");
					short num = row3.Field<short>("qalInspectionLineID");
					string text3 = row3.Field<string>("qalPartID");
					string text4 = row3.Field<string>("qalPartRevisionID");
					string text5 = row3.Field<string>("qalPartWarehouseLocationID");
					string text6 = row3.Field<string>("qalPartBinID");
					byte b = row3.Field<byte>("qalInspectionType");
					string value = row3.Field<string>("qalSourceTableName");
					if (new Part().IsPartBinInactive(bindingSource.Database, text3, text4, text5, text6))
					{
						string value2 = $"[Inspection ID: '{text2}', Line: '{num}', Part: '{text3}', Revision: '{text4}', Warehouse: '{text5}', Bin: '{text6}' is inactive]";
						if (b == 3 || (b == 1 && !string.IsNullOrEmpty(value)))
						{
							stringBuilder.AppendLine(value2);
						}
						else if (b == 1 && string.IsNullOrEmpty(value))
						{
							stringBuilder2.AppendLine(value2);
						}
					}
				}
			}
			dataTable = null;
			DataTable dataTable2 = bindingSource.PrimaryTable.GetChildBindingSource("InspectionComponents").GetDataTable();
			if (dataTable2 != null && dataTable2.Rows.Count != 0)
			{
				foreach (DataRow row4 in dataTable2.Rows)
				{
					string text7 = row4.Field<string>("qamInspectionID");
					short num2 = row4.Field<short>("qamInspectionLineID");
					string text8 = row4.Field<string>("qamPartID");
					string text9 = row4.Field<string>("qamPartRevisionID");
					string text10 = row4.Field<string>("qamPartWarehouseLocationID");
					string text11 = row4.Field<string>("qamPartBinID");
					byte b2 = row4.Field<byte>("qamInspectionType");
					string value3 = row4.Field<string>("qamSourceTableName");
					if (new Part().IsPartBinInactive(bindingSource.Database, text8, text9, text10, text11))
					{
						string value4 = $"[Inspection ID: '{text7}', Line: '{num2}', Part: '{text8}', Revision: '{text9}', Warehouse: '{text10}', Bin: '{text11}' is inactive]";
						if (b2 == 3 || (b2 == 1 && !string.IsNullOrEmpty(value3)))
						{
							stringBuilder.AppendLine(value4);
						}
						else if (b2 == 1 && string.IsNullOrEmpty(value3))
						{
							stringBuilder2.AppendLine(value4);
						}
					}
				}
			}
			dataTable2 = null;
			if (stringBuilder.Length != 0)
			{
				text = "This transaction CAN NOT be posted because an INACTIVE bin exists for the part(s) indicated." + Environment.NewLine + stringBuilder.ToString();
			}
			else if (stringBuilder2.Length != 0 && text.Length == 0)
			{
				stringBuilder2.AppendLine("Are you sure?");
				text = "Inspection Lines exist with INACTIVE Bins and will adjust the quantity on hand for those bins.\r\n" + stringBuilder2.ToString();
			}
		}
		return text;
	}

	public bool InspectionPeriodCheck(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		DateTime dateTime = default(DateTime);
		string value = string.Empty;
		if (bindingSource.CurrentAsDataRow.Table.Columns.Contains("qapInspectionID"))
		{
			value = bindingSource.CurrentAsDataRow.Field<string>("qapInspectionID");
		}
		else if (bindingSource.CurrentAsDataRow.Table.Columns.Contains("qalInspectionID"))
		{
			value = bindingSource.CurrentAsDataRow.Field<string>("qalInspectionID");
		}
		bool result = true;
		if (!string.IsNullOrWhiteSpace(value))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select qalInspectionDate, qalInspectionLineID from InspectionLines where qalInspectionID = @ID order by qalInspectionLineID");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					if (row.Table.Columns.Contains("qalInspectionDate"))
					{
						dateTime = row.Field<DateTime>("qalInspectionDate");
					}
					if (!new Financial().GetYearAndPeriod(database, dateTime.Date, "").Success)
					{
						result = false;
						break;
					}
				}
			}
		}
		return result;
	}

	public void PostInspection(M1BindingSource bindingsource)
	{
		M1Database database = bindingsource.Database;
		SqlTransaction sqlTransaction = bindingsource.Transaction;
		if (sqlTransaction == null)
		{
			sqlTransaction = database.BeginTransaction();
		}
		try
		{
			if (bindingsource.CurrentAsDataRow == null)
			{
				return;
			}
			M1BindingSource childBindingSource = bindingsource.PrimaryTable.GetChildBindingSource("InspectionLines");
			string value = bindingsource.CurrentAsDataRow.Field<string>("qapInspectionID");
			bindingsource.CurrentAsDataRow.SetField("qapPosted", value: true);
			foreach (DataRowView item in childBindingSource)
			{
				item.Row.SetField("qalPosted", value: true);
				item.Row.SetField("qalStatus", "C");
				foreach (DataRowView item2 in childBindingSource.PrimaryTable.GetChildBindingSource("InspectionComponents"))
				{
					item2.Row.SetField("qamPosted", value: true);
				}
			}
			if (string.IsNullOrWhiteSpace(value))
			{
				return;
			}
			SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, qalUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntNegativeTransaction from InspectionLines inner join SerialNumberTransactions on qalUniqueID = sntTableUniqueID where qalInspectionID = @ID and qalPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			DataTable dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
				foreach (DataRow row in dataTable.Rows)
				{
					byte status = 0;
					byte transType = 0;
					serialNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row.Field<string>("sntSerialNumberID"));
					bool flag = row.Field<bool>("sntNegativeTransaction");
					switch (row.Field<byte>("sntTransactionType"))
					{
					case 53:
						status = (byte)(flag ? 5 : 2);
						transType = 16;
						break;
					case 54:
						status = (byte)(flag ? 5 : 3);
						transType = 4;
						break;
					case 55:
						status = (byte)(flag ? 5 : 6);
						transType = 23;
						break;
					case 56:
						status = (byte)(flag ? 5 : 6);
						transType = 22;
						break;
					case 57:
						status = (byte)(flag ? 5 : 6);
						transType = 17;
						break;
					case 58:
						status = (byte)(flag ? 5 : 7);
						transType = 18;
						break;
					case 59:
						status = (byte)(flag ? 5 : 7);
						transType = 18;
						break;
					case 60:
						status = (byte)(flag ? 5 : 7);
						transType = 24;
						break;
					}
					serialNumberDefinition.AddSerialTransaction(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), row.Field<decimal>("sntQuantity"), status, transType, "InspectionLines", row.Field<Guid>("QALUniqueID"), row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), 0, row.Field<bool>("sntNegativeTransaction"), row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, qamUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from InspectionComponents inner join SerialNumberTransactions on qamUniqueID = sntTableUniqueID where qamInspectionID = @ID and qamPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				SerialNumberDefinition serialNumberDefinition2 = new SerialNumberDefinition();
				foreach (DataRow row2 in dataTable.Rows)
				{
					byte status2 = 0;
					byte transType2 = 0;
					serialNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row2.Field<string>("sntSerialNumberID"));
					bool flag2 = row2.Field<bool>("sntNegativeTransaction");
					switch (row2.Field<byte>("sntTransactionType"))
					{
					case 53:
						status2 = (byte)(flag2 ? 5 : 2);
						transType2 = 16;
						break;
					case 54:
						status2 = (byte)(flag2 ? 5 : 3);
						transType2 = 4;
						break;
					case 55:
						status2 = (byte)(flag2 ? 5 : 6);
						transType2 = 23;
						break;
					case 56:
						status2 = (byte)(flag2 ? 5 : 6);
						transType2 = 22;
						break;
					case 57:
						status2 = (byte)(flag2 ? 5 : 6);
						transType2 = 17;
						break;
					case 58:
						status2 = (byte)(flag2 ? 5 : 7);
						transType2 = 18;
						break;
					case 59:
						status2 = (byte)(flag2 ? 5 : 7);
						transType2 = 18;
						break;
					case 60:
						status2 = (byte)(flag2 ? 5 : 7);
						transType2 = 24;
						break;
					}
					serialNumberDefinition2.AddSerialTransaction(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntPartWarehouseLocationID"), row2.Field<string>("sntPartBinID"), row2.Field<string>("sntSerialNumberID"), row2.Field<decimal>("sntQuantity"), status2, transType2, "InspectionComponents", row2.Field<Guid>("qamUniqueID"), row2.Field<string>("sntJobID"), Convert.ToInt32(row2["sntJobAssemblyID"]), Convert.ToInt32(row2["sntJobMaterialID"]), Convert.ToInt32(row2["sntJobMaterialComponentID"]), row2.Field<bool>("sntNegativeTransaction"), row2.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition2.RefreshStatuses(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, qalUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from InspectionLines inner join LotNumberTransactions on qalUniqueID = abtTableUniqueID where qalInspectionID = @ID and qalPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
				foreach (DataRow row3 in dataTable.Rows)
				{
					byte status3 = 0;
					byte transType3 = 0;
					lotNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row3.Field<string>("abtLotNumberID"));
					bool flag3 = row3.Field<bool>("abtNegativeTransaction");
					switch (row3.Field<byte>("abtTransactionType"))
					{
					case 53:
						status3 = (byte)(flag3 ? 5 : 2);
						transType3 = 16;
						break;
					case 54:
						status3 = (byte)(flag3 ? 5 : 3);
						transType3 = 4;
						break;
					case 55:
						status3 = (byte)(flag3 ? 5 : 6);
						transType3 = 23;
						break;
					case 56:
						status3 = (byte)(flag3 ? 5 : 6);
						transType3 = 22;
						break;
					case 57:
						status3 = (byte)(flag3 ? 5 : 6);
						transType3 = 17;
						break;
					case 58:
						status3 = (byte)(flag3 ? 5 : 7);
						transType3 = 18;
						break;
					case 59:
						status3 = (byte)(flag3 ? 5 : 7);
						transType3 = 18;
						break;
					case 60:
						status3 = (byte)(flag3 ? 5 : 7);
						transType3 = 24;
						break;
					}
					lotNumberDefinition.AddLotTransaction(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtPartWarehouseLocationID"), row3.Field<string>("abtPartBinID"), row3.Field<string>("abtLotNumberID"), row3.Field<decimal>("abtQuantity"), status3, transType3, "InspectionLines", row3.Field<Guid>("qalUniqueID"), row3.Field<string>("abtJobID"), Convert.ToInt32(row3["abtJobAssemblyID"]), Convert.ToInt32(row3["abtJobMaterialID"]), Convert.ToInt32(row3["abtJobMaterialComponentID"]), row3.Field<bool>("abtNegativeTransaction"), row3.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition.RefreshStatuses(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtLotNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, qamUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from InspectionComponents inner join LotNumberTransactions on qamUniqueID = abtTableUniqueID where qamInspectionID = @ID and qamPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition2 = new LotNumberDefinition();
				foreach (DataRow row4 in dataTable.Rows)
				{
					byte status4 = 0;
					byte transType4 = 0;
					lotNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row4.Field<string>("abtLotNumberID"));
					bool flag4 = row4.Field<bool>("abtNegativeTransaction");
					switch (row4.Field<byte>("abtTransactionType"))
					{
					case 53:
						status4 = (byte)(flag4 ? 5 : 2);
						transType4 = 16;
						break;
					case 54:
						status4 = (byte)(flag4 ? 5 : 3);
						transType4 = 4;
						break;
					case 55:
						status4 = (byte)(flag4 ? 5 : 6);
						transType4 = 23;
						break;
					case 56:
						status4 = (byte)(flag4 ? 5 : 6);
						transType4 = 22;
						break;
					case 57:
						status4 = (byte)(flag4 ? 5 : 6);
						transType4 = 17;
						break;
					case 58:
						status4 = (byte)(flag4 ? 5 : 7);
						transType4 = 18;
						break;
					case 59:
						status4 = (byte)(flag4 ? 5 : 7);
						transType4 = 18;
						break;
					case 60:
						status4 = (byte)(flag4 ? 5 : 7);
						transType4 = 24;
						break;
					}
					lotNumberDefinition2.AddLotTransaction(database, sqlTransaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtPartWarehouseLocationID"), row4.Field<string>("abtPartBinID"), row4.Field<string>("abtLotNumberID"), row4.Field<decimal>("abtQuantity"), status4, transType4, "InspectionComponents", row4.Field<Guid>("qamUniqueID"), row4.Field<string>("abtJobID"), Convert.ToInt32(row4["abtJobAssemblyID"]), Convert.ToInt32(row4["abtJobMaterialID"]), Convert.ToInt32(row4["abtJobMaterialComponentID"]), row4.Field<bool>("abtNegativeTransaction"), row4.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition2.RefreshStatuses(database, sqlTransaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtLotNumberID"));
				}
			}
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	public void UpdateInspectorInGrid(M1Database database, DataRow row, string changedField)
	{
		if (row.Table.Columns.Contains("qalInspectorEmployeeID") && row.Table.Columns.Contains("FieldSelected") && changedField.Equals("FieldSelected"))
		{
			if (row.Field<bool>("FieldSelected"))
			{
				row.SetField("qalInspectorEmployeeID", new AppAxProduction(database).InspectorID);
			}
			else
			{
				row.SetField("qalInspectorEmployeeID", string.Empty);
			}
		}
	}

	public void PostQtyToInspect(M1BindingSource bindingsource)
	{
		M1Database database = bindingsource.Database;
		SqlTransaction sqlTransaction = bindingsource.Transaction;
		if (sqlTransaction == null)
		{
			sqlTransaction = database.BeginTransaction();
		}
		try
		{
			if (HasRealErrors(bindingsource) || bindingsource.CurrentAsDataRow == null)
			{
				return;
			}
			bindingsource.CurrentAsDataRow.SetField("qalManualInspectionFinalized", value: true);
			foreach (DataRowView item in bindingsource.PrimaryTable.GetChildBindingSource("InspectionComponents"))
			{
				item.Row.SetField("qamManualInspectionFinalized", value: true);
			}
			string value = bindingsource.CurrentAsDataRow.Field<string>("qalInspectionID");
			if (string.IsNullOrWhiteSpace(value))
			{
				return;
			}
			SqlCommand sqlCommand = database.NewSqlCommand("select sntSerialNumberID, sntPartID, sntPartRevisionID from InspectionLines inner join SerialNumberTransactions on qalUniqueID = sntTableUniqueID where qalInspectionID = @ID and qalPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			DataTable dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
				foreach (DataRow row5 in dataTable.Rows)
				{
					serialNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row5.Field<string>("sntSerialNumberID"));
					serialNumberDefinition.RefreshStatuses(database, sqlTransaction, row5.Field<string>("sntPartID"), row5.Field<string>("sntPartRevisionID"), row5.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select sntSerialNumberID, sntPartID, sntPartRevisionID from InspectionComponents inner join SerialNumberTransactions on qamUniqueID = sntTableUniqueID where qamInspectionID = @ID and qamPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				SerialNumberDefinition serialNumberDefinition2 = new SerialNumberDefinition();
				foreach (DataRow row6 in dataTable.Rows)
				{
					serialNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row6.Field<string>("sntSerialNumberID"));
					serialNumberDefinition2.RefreshStatuses(database, sqlTransaction, row6.Field<string>("sntPartID"), row6.Field<string>("sntPartRevisionID"), row6.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select abtLotNumberID, abtPartID, abtPartRevisionID from InspectionLines inner join LotNumberTransactions on qalUniqueID = abtTableUniqueID where qalInspectionID = @ID and qalPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
				foreach (DataRow row7 in dataTable.Rows)
				{
					lotNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row7.Field<string>("abtLotNumberID"));
					lotNumberDefinition.RefreshStatuses(database, sqlTransaction, row7.Field<string>("abtPartID"), row7.Field<string>("abtPartRevisionID"), row7.Field<string>("abtLotNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select abtLotNumberID, abtPartID, abtPartRevisionID from InspectionComponents inner join LotNumberTransactions on qamUniqueID = abtTableUniqueID where qamInspectionID = @ID and qamPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition2 = new LotNumberDefinition();
				foreach (DataRow row8 in dataTable.Rows)
				{
					lotNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row8.Field<string>("abtLotNumberID"));
					lotNumberDefinition2.RefreshStatuses(database, sqlTransaction, row8.Field<string>("abtPartID"), row8.Field<string>("abtPartRevisionID"), row8.Field<string>("abtLotNumberID"));
				}
			}
			database.CommitTransaction(sqlTransaction);
			bindingsource.SaveData();
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	private bool HasRealErrors(M1BindingSource bindingsource)
	{
		if (bindingsource.Errors.Count == 0)
		{
			return false;
		}
		foreach (ValidationInfo error in bindingsource.Errors)
		{
			if (error.ErrorCount > 0)
			{
				return true;
			}
		}
		return false;
	}

	public void CreateQtyToInspectJournals(M1BindingSource bindingsource)
	{
		if (!bindingsource.Database.Props("GL").Field<bool>("xafGLCreateStockJournals") || bindingsource.CurrentAsDataRow == null)
		{
			return;
		}
		DataRow currentAsDataRow = bindingsource.CurrentAsDataRow;
		M1Database database = bindingsource.Database;
		if (!currentAsDataRow.Field<bool>("qalKitPart"))
		{
			new CostOfGoodSoldDefinition(bindingsource, "qalInvQuantityAccepted", "qalPartBinID", DateTime.Now, 32, 4, reverseSign: false, 1m, "CheckForKitPart,IGNOREPOSTED", "InspectionLines", "QALUNIQUEID", string.Empty).AddJournal(bindingsource.Database, currentAsDataRow, DataRowVersion.Current, bindingsource.Transaction);
			return;
		}
		CostOfGoodSoldDefinition costOfGoodSoldDefinition = new CostOfGoodSoldDefinition(bindingsource.PrimaryTable.GetChildBindingSource("InspectionComponents"), "qamInvQuantityAccepted", "qamPartBinID", DateTime.Now, 32, 4, reverseSign: false, 1m, "IGNOREPOSTED", "InspectionComponents", "QAMUNIQUEID", string.Empty);
		SqlCommand sqlCommand = database.NewSqlCommand("Select * From InspectionComponents Where qamInspectionID = @InspectionID And qamInspectionLineID = @InspectionLineID");
		sqlCommand.Parameters.AddWithValue("@InspectionID", currentAsDataRow.Field<string>("qalInspectionID"));
		sqlCommand.Parameters.AddWithValue("@InspectionLineID", currentAsDataRow.Field<short>("qalInspectionLineID"));
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable == null)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			costOfGoodSoldDefinition.AddJournal(bindingsource.Database, row, DataRowVersion.Current, bindingsource.Transaction);
		}
	}

	public bool PostQtyToInspectCheck(M1BindingSource bindingsource)
	{
		if (bindingsource.CurrentAsDataRow == null)
		{
			return false;
		}
		DataRow currentAsDataRow = bindingsource.CurrentAsDataRow;
		M1Database database = bindingsource.Database;
		bool num = currentAsDataRow.Field<bool>("qalKitPart");
		decimal qtyToInspect = currentAsDataRow.Field<decimal>("qalQuantityToInspect");
		bool nonStockedStatus = getNonStockedStatus(database, currentAsDataRow.Field<string>("qalPartID"));
		if (num)
		{
			return checkComponentsCanBePosted(database, currentAsDataRow.Field<string>("qalInspectionID"), currentAsDataRow.Field<short>("qalInspectionLineID"), qtyToInspect);
		}
		if (nonStockedStatus)
		{
			return true;
		}
		return checkQtyOnHand(database, currentAsDataRow.Field<string>("qalPartID"), currentAsDataRow.Field<string>("qalPartRevisionID"), currentAsDataRow.Field<string>("qalPartWarehouseLocationID"), currentAsDataRow.Field<string>("qalPartBinID"), qtyToInspect);
	}

	private bool getNonStockedStatus(M1Database database, string partID)
	{
		partID = partID.Trim();
		if (partID.Length != 0)
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select impNonStockedItem From Parts Where impPartID = @PartID");
			sqlCommand.Parameters.AddWithValue("@PartID", partID);
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable != null && dataTable.Rows.Count != 0)
			{
				return dataTable.Rows[0].Field<bool>("impNonStockedItem");
			}
		}
		return false;
	}

	private bool checkComponentsCanBePosted(M1Database database, string inspectionID, int inspectionLineID, decimal qtyToInspect)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select qamPartID, qamPartRevisionID, qamPartWarehouseLocationID, qamPartBinID, qamUniqueID From InspectionComponents Where qamInspectionID = @InspectionID And qamInspectionLineID = @InspectionLineID");
		sqlCommand.Parameters.AddWithValue("@InspectionID", inspectionID);
		sqlCommand.Parameters.AddWithValue("@InspectionLineID", inspectionLineID);
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable == null)
		{
			return false;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			if (!checkQtyOnHand(database, row.Field<string>("qamPartID"), row.Field<string>("qamPartRevisionID"), row.Field<string>("qamPartWarehouseLocationID"), row.Field<string>("qamPartBinID"), qtyToInspect))
			{
				return false;
			}
			bool flag = true;
			if (new SerialNumber().IsSerialTracked(database, row.Field<string>("qamPartID")))
			{
				flag = checkComponentsSerialOrLotTransactionsQuantity(database, "SerialNumberTransactions", "snt", row.Field<Guid>("qamUniqueID")) == qtyToInspect;
			}
			bool flag2 = true;
			if (new LotNumber().IsLotTracked(database, row.Field<string>("qamPartID")))
			{
				flag2 = checkComponentsSerialOrLotTransactionsQuantity(database, "LotNumberTransactions", "abt", row.Field<Guid>("qamUniqueID")) == qtyToInspect;
			}
			if (!flag || !flag2)
			{
				return false;
			}
		}
		return true;
	}

	private static bool checkQtyOnHand(M1Database database, string partID, string partRevisionID, string partWarehouseID, string partBinID, decimal qtyToInspect)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select imbQuantityOnHand From PartBins Where imbPartID = @PartID And imbPartRevisionID = @PartRevisionID And imbWarehouseID = @WarehouseID And imbPartBinID = @PartBinID");
		sqlCommand.Parameters.AddWithValue("@PartID", partID);
		sqlCommand.Parameters.AddWithValue("@PartRevisionID", partRevisionID);
		sqlCommand.Parameters.AddWithValue("@WarehouseID", partWarehouseID);
		sqlCommand.Parameters.AddWithValue("@PartBinID", partBinID);
		return (decimal)Convert.ToInt32(database.ExecuteScalar(sqlCommand)) >= qtyToInspect;
	}

	private static decimal checkComponentsSerialOrLotTransactionsQuantity(M1Database database, string tableName, string tablePrefix, Guid transactionGuid)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select ISNULL(SUM(" + tablePrefix + "Quantity),0) From " + tableName + " Where " + tablePrefix + "TableUniqueID = @UniqueID");
		sqlCommand.Parameters.AddWithValue("@UniqueID", transactionGuid);
		return (decimal)database.ExecuteScalar(sqlCommand);
	}
}
