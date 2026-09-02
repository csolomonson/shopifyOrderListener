using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using M1.Ax.Erp.JobSchedule;
using M1.Core;
using M1.Core.Script;
using M1.Extensions;

namespace M1.Ax.Erp;

[AxScript("FollowUps")]
[ComVisible(true)]
public class AppAxFollowUps : IDisposable
{
	private IServiceProvider provider;

	public AppAxFollowUps(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	public bool TransferQuoteToFollowup(object data, object bindingSource, object sqlTransaction)
	{
		int num = 0;
		M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
		if (sqlTransaction == DBNull.Value)
		{
			sqlTransaction = null;
		}
		short num2 = m1Database.Props("QM").Field<short>("xapQMFollowUpDays");
		if (num2 != 0)
		{
			DataRow dataRow = null;
			if (data is FieldCollection)
			{
				dataRow = ((FieldCollection)data)[0].BindingSource.CurrentAsDataRow;
			}
			else if (data is DataRow)
			{
				dataRow = (DataRow)data;
			}
			else if (data is string)
			{
				SqlCommand sqlCommand = m1Database.NewSqlCommand("Select qmpQuoteID,qmpClosed,qmpQuoteDate,qmpCustomerOrganizationID,qmpQuoteLocationID,qmpQuoteContactID,qmpQuoterEmployeeID,qmpPlantID From Quotes Where qmpQuoteID = @QuoteID");
				sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = (string)data;
				DataTable dataTable = m1Database.GetDataTable(sqlCommand);
				if (dataTable.Rows.Count != 0)
				{
					dataRow = dataTable.Rows[0];
				}
			}
			if (dataRow == null)
			{
				throw new M1Exception("Unknown object type in TransferQuoteToFollowup. M1 could not perform the expected operation.");
			}
			if (!dataRow.Field<bool>("qmpClosed") && num2 != 0 && dataRow["qmpQuoteDate"] != DBNull.Value)
			{
				List<string> list = new List<string>();
				if (m1Database.Props("QM").Field<byte>("xapQMFollowUpType") == 2)
				{
					if (bindingSource != DBNull.Value)
					{
						M1BindingSource childBindingSource = ((M1BindingSource)bindingSource).PrimaryTable.GetChildBindingSource("QuoteSalespeople");
						if (childBindingSource != null && childBindingSource.Count != 0)
						{
							foreach (DataRow row3 in childBindingSource.GetDataView(dataRow).ToTable().Rows)
							{
								list.Add(row3.Field<string>("qmjSalesEmployeeID"));
							}
						}
					}
					else
					{
						SqlCommand sqlCommand2 = m1Database.NewSqlCommand("select qmjSalesEmployeeID From QuoteSalespeople Where qmjQuoteID = @QuoteID And qmjSalesEmployeeID <> ''");
						sqlCommand2.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = dataRow.Field<string>("qmpQuoteID");
						foreach (DataRow row4 in m1Database.GetDataTable(sqlCommand2, (SqlTransaction)sqlTransaction).Rows)
						{
							list.Add(row4.Field<string>("qmjSalesEmployeeID"));
						}
					}
				}
				else if (!string.IsNullOrWhiteSpace(dataRow.Field<string>("qmpQuoterEmployeeID")))
				{
					list.Add(dataRow.Field<string>("qmpQuoterEmployeeID"));
				}
				if (list.Count != 0)
				{
					DateTime value = ScheduleProcess.DateAddByDays(m1Database, dataRow.Field<string>("qmpPlantID"), dataRow.Field<DateTime>("qmpQuoteDate"), num2);
					M1BindingSource m1BindingSource = null;
					if (bindingSource != DBNull.Value)
					{
						m1BindingSource = ((M1BindingSource)bindingSource).PrimaryTable.GetChildBindingSource("FollowUps");
					}
					else
					{
						m1BindingSource = new M1BindingSource(m1Database, (SqlTransaction)sqlTransaction);
						m1BindingSource.DataSourceTable = "FollowUps";
						m1BindingSource.ParentFieldValue = dataRow.Field<string>("qmpQuoteID");
					}
					SqlCommand sqlCommand3 = m1Database.NewSqlCommand("select cmfAssignedToEmployeeID from Followups Where cmfQuoteID = @QuoteID");
					sqlCommand3.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = dataRow.Field<string>("qmpQuoteID");
					DataTable dataTable2 = m1Database.GetDataTable(sqlCommand3, (SqlTransaction)sqlTransaction);
					foreach (string item in list)
					{
						if (dataTable2.Select("cmfAssignedToEmployeeID = " + item.ToLinq()).Length == 0)
						{
							DataRow dataRow2 = m1BindingSource.AddNew() as DataRow;
							dataRow2["cmfFollowupType"] = 2;
							dataRow2["cmfOrganizationID"] = dataRow["qmpCustomerOrganizationID"];
							dataRow2["cmfLocationID"] = dataRow["qmpQuoteLocationID"];
							dataRow2["cmfContactID"] = dataRow["qmpQuoteContactID"];
							dataRow2["cmfQuoteID"] = dataRow["qmpQuoteID"];
							dataRow2.SetField("cmfDueDate", (DateTime?)value);
							dataRow2["cmfShortDescription"] = string.Format("Follow-up for quote {0}", dataRow.Field<string>("qmpQuoteID"));
							dataRow2["cmfAssignedToEmployeeID"] = item;
							m1BindingSource.SetKeyToNextAvailable(dataRow2);
							num++;
						}
					}
					if (bindingSource == DBNull.Value)
					{
						m1BindingSource.SaveData();
					}
				}
			}
		}
		return num != 0;
	}

	public bool TransferPurchaseOrderToFollowup(object data)
	{
		int num = 0;
		M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
		short num2 = m1Database.Props("PM").Field<short>("xapPMFollowUpDays");
		if (num2 != 0)
		{
			DataRow dataRow = null;
			if (data is FieldCollection)
			{
				dataRow = ((FieldCollection)data)[0].BindingSource.CurrentAsDataRow;
			}
			else if (data is DataRow)
			{
				dataRow = (DataRow)data;
			}
			else if (data is string)
			{
				SqlCommand sqlCommand = m1Database.NewSqlCommand("Select pmpPurchaseOrderID,pmpClosed,pmpDueDate,pmpSupplierOrganizationID,pmpPurchaseLocationID,pmpPurchaseContactID,pmpBuyerEmployeeID,pmpPlantID From PurchaseOrders Where pmpPurchaseOrderID = @PurchaseOrderID");
				sqlCommand.Parameters.Add(new SqlParameter("@PurchaseOrderID", SqlDbType.NVarChar)).Value = (string)data;
				DataTable dataTable = m1Database.GetDataTable(sqlCommand);
				if (dataTable.Rows.Count != 0)
				{
					dataRow = dataTable.Rows[0];
				}
			}
			if (dataRow == null)
			{
				throw new M1Exception("Unknown object type in TransferQuoteToFollowup. M1 could not perform the expected operation.");
			}
			if (!dataRow.Field<bool>("pmpClosed") && num2 != 0 && dataRow["pmpDueDate"] != DBNull.Value)
			{
				List<string> list = new List<string>();
				if (!string.IsNullOrWhiteSpace(dataRow.Field<string>("pmpBuyerEmployeeID")))
				{
					list.Add(dataRow.Field<string>("pmpBuyerEmployeeID"));
				}
				if (list.Count != 0)
				{
					DateTime value = ScheduleProcess.DateSubtractByDays(m1Database, dataRow.Field<string>("pmpPlantID"), dataRow.Field<DateTime>("pmpDueDate"), num2);
					using M1BindingSource m1BindingSource = new M1BindingSource(m1Database);
					m1BindingSource.DataSourceTable = "FollowUps";
					SqlCommand sqlCommand2 = m1Database.NewSqlCommand("select cmfAssignedToEmployeeID from Followups Where cmfPurchaseOrderID = @PurchaseOrderID");
					sqlCommand2.Parameters.Add(new SqlParameter("@PurchaseOrderID", SqlDbType.NVarChar)).Value = dataRow.Field<string>("pmpPurchaseOrderID");
					DataTable dataTable2 = m1Database.GetDataTable(sqlCommand2);
					foreach (string item in list)
					{
						if (dataTable2.Select("cmfAssignedToEmployeeID = " + item.ToLinq()).Length == 0)
						{
							DataRow dataRow2 = m1BindingSource.AddNew() as DataRow;
							dataRow2["cmfFollowupType"] = 2;
							dataRow2["cmfOrganizationID"] = dataRow["pmpSupplierOrganizationID"];
							dataRow2["cmfLocationID"] = dataRow["pmpPurchaseLocationID"];
							dataRow2["cmfContactID"] = dataRow["pmpPurchaseContactID"];
							dataRow2["cmfPurchaseOrderID"] = dataRow["pmpPurchaseOrderID"];
							dataRow2.SetField("cmfDueDate", (DateTime?)value);
							dataRow2["cmfShortDescription"] = string.Format("Follow-up for purchase order {0}", dataRow.Field<string>("pmpPurchaseOrderID"));
							dataRow2["cmfAssignedToEmployeeID"] = item;
							m1BindingSource.SetKeyToNextAvailable(dataRow2);
							num++;
						}
					}
					m1BindingSource.SaveData();
				}
			}
		}
		return num != 0;
	}

	public void ExportFollowUpToExchange(object data, object sqlTransaction, bool bDelete = false)
	{
		if (sqlTransaction == DBNull.Value)
		{
			sqlTransaction = null;
		}
		M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
		SqlDataAdapter adapter = null;
		DataTable dataTable = null;
		DataRow dataRow;
		if (data is string)
		{
			SqlCommand sqlCommand = m1Database.NewSqlCommand("Select * From FollowUps Where cmfFollowupID = @FollowupID");
			sqlCommand.Parameters.Add(new SqlParameter("@FollowupID", SqlDbType.NVarChar)).Value = data.ToString();
			dataTable = m1Database.GetDataTable(sqlCommand, fillSchema: true, out adapter, (SqlTransaction)sqlTransaction);
			if (dataTable.Rows.Count == 0)
			{
				throw new M1Exception($"Followup {data.ToString()} not found.");
			}
			dataRow = dataTable.Rows[0];
		}
		else
		{
			if (!(data is DataRow))
			{
				throw new ArgumentException("data argument is invalid.");
			}
			dataRow = (DataRow)data;
		}
		if (!bDelete && dataRow.RowState == DataRowState.Modified && (dataRow.Field<byte>("cmfFollowupType") != dataRow.Field<byte>("cmfFollowupType", DataRowVersion.Original) || dataRow.Field<string>("cmfAssignedToEmployeeID") != dataRow.Field<string>("cmfAssignedToEmployeeID", DataRowVersion.Original)))
		{
			try
			{
				new FollowUp().ExportFollowUpToExchange(m1Database, dataRow, (SqlTransaction)sqlTransaction, bDelete: true);
				new FollowUp().ExportFollowUpToExchange(m1Database, dataRow, (SqlTransaction)sqlTransaction);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
		else
		{
			try
			{
				new FollowUp().ExportFollowUpToExchange(m1Database, dataRow, (SqlTransaction)sqlTransaction, bDelete);
			}
			catch (Exception ex2)
			{
				MessageBox.Show(ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
		if (dataTable != null)
		{
			m1Database.UpdateData(dataTable, adapter);
		}
	}

	public void RefreshFollowUpsFromExchange()
	{
		try
		{
			new FollowUp().RefreshFollowUpsFromExchange((M1Database)provider.GetService(typeof(M1Database)));
		}
		catch (Exception)
		{
		}
	}

	public void Dispose()
	{
		provider = null;
	}
}
