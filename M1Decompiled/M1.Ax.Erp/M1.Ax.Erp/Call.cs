using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using M1.Core;
using M1.Core.Script;
using M1.Extensions;
using M1Classes92;

namespace M1.Ax.Erp;

public class Call
{
	private string[] getContactIDFromRecipients(M1Database database, List<string> recipients)
	{
		string[] array = new string[3];
		_ = new string[3];
		SqlCommand sqlCommand = database.NewSqlCommand("Select cmcOrganizationID From OrganizationContacts Where cmcOrganizationID = @OrgID And cmcLocationID = @LocID And cmcContactID = @ConID");
		sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar));
		sqlCommand.Parameters.Add(new SqlParameter("@LocID", SqlDbType.NVarChar));
		sqlCommand.Parameters.Add(new SqlParameter("@ConID", SqlDbType.NVarChar));
		foreach (string recipient in recipients)
		{
			int num = recipient.IndexOf('[');
			if (num == -1)
			{
				continue;
			}
			string text = recipient.Substring(num + 1);
			num = text.IndexOf(']');
			if (num == -1)
			{
				continue;
			}
			text = text.Substring(0, num);
			string[] array2 = text.Split(',');
			if (array2.Length < 1)
			{
				continue;
			}
			assignType(array2[0], array);
			if (array2.Length >= 2)
			{
				assignType(array2[1], array);
				if (array2.Length >= 3)
				{
					assignType(array2[2], array);
				}
			}
			if (!string.IsNullOrWhiteSpace(array[0]))
			{
				sqlCommand.Parameters["@OrgID"].Value = array[0];
				sqlCommand.Parameters["@LocID"].Value = array[1];
				sqlCommand.Parameters["@ConID"].Value = array[2];
				if (Convert.ToString(database.ExecuteScalar(sqlCommand)) != null)
				{
					return array;
				}
			}
			array = new string[3];
		}
		sqlCommand = database.NewSqlCommand("Select cmcOrganizationID,cmcLocationID,cmcContactID From OrganizationContacts Where cmcEmailAddress = @Address");
		sqlCommand.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar));
		foreach (string recipient2 in recipients)
		{
			sqlCommand.Parameters["@Address"].Value = recipient2;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				array[0] = dataTable.Rows[0].Field<string>("cmcOrganizationID");
				array[1] = dataTable.Rows[0].Field<string>("cmcLocationID");
				array[2] = dataTable.Rows[0].Field<string>("cmcContactID");
				return array;
			}
		}
		return null;
	}

	private void assignType(string data, string[] ids)
	{
		splitID(data, out var type, out var id);
		if (type.Equals("Org", StringComparison.CurrentCultureIgnoreCase))
		{
			ids[0] = id;
		}
		else if (type.Equals("Loc", StringComparison.CurrentCultureIgnoreCase))
		{
			ids[1] = id;
		}
		else if (type.Equals("Con", StringComparison.CurrentCultureIgnoreCase))
		{
			ids[2] = id;
		}
	}

	private void splitID(string data, out string type, out string id)
	{
		int num = data.IndexOf(':');
		if (num != -1)
		{
			type = data.Substring(0, num);
			id = data.Substring(num + 1);
		}
		else
		{
			type = string.Empty;
			id = data;
		}
	}

	public void CreateCallForEmail(M1Database database, MessageData message)
	{
		string[] contactIDFromRecipients = getContactIDFromRecipients(database, message.Recipients);
		if (contactIDFromRecipients == null || string.IsNullOrWhiteSpace(contactIDFromRecipients[0]))
		{
			return;
		}
		M1.Core.AppContext appContext = database.GetService(typeof(M1.Core.AppContext)) as M1.Core.AppContext;
		M1BindingSource m1BindingSource = new M1BindingSource(database);
		m1BindingSource.LoadDefinition(string.Empty, "Calls", null, true);
		DataRow dataRow = m1BindingSource.AddNew() as DataRow;
		if (!string.IsNullOrWhiteSpace(database.Props("PN").Field<string>("xapHDMailMergeCallTypeID")))
		{
			dataRow.SetField("kbpCallTypeID", database.Props("PN").Field<string>("xapHDMailMergeCallTypeID"));
		}
		if (!string.IsNullOrWhiteSpace(database.Props("PN").Field<string>("xapHDMailMergeContactMethodID")))
		{
			dataRow.SetField("kbpContactMethodID", database.Props("PN").Field<string>("xapHDMailMergeContactMethodID"));
		}
		dataRow.SetField("kbpPartID", "MAILMERGE");
		dataRow.SetField("kbpOrganizationID", contactIDFromRecipients[0]);
		dataRow.SetField("kbpLocationID", contactIDFromRecipients[1]);
		dataRow.SetField("kbpContactID", contactIDFromRecipients[2]);
		dataRow.SetField("kbpShortDescription", message.Subject.Substring(0, Math.Min(m1BindingSource.Fields["kbpShortDescription"].FieldLength, message.Subject.Length)));
		if (string.IsNullOrWhiteSpace(message.Body.Text))
		{
			dataRow.SetField<string>("kbpLongDescriptionText", null);
			dataRow.SetField<string>("kbpLongDescriptionRTF", null);
		}
		else
		{
			dataRow.SetField("kbpLongDescriptionText", message.Body.Text);
			dataRow.SetField("kbpLongDescriptionRTF", message.Body.Text);
		}
		dataRow.SetField("kbpInbound", value: false);
		dataRow.SetField("kbpBillable", value: false);
		dataRow.SetField("kbpInternalOnly", value: true);
		dataRow.SetField("kbpStatus", "C");
		if (!string.IsNullOrWhiteSpace(message.TemplateFile))
		{
			string text = message.TemplateFile;
			string text2 = Path.Combine(appContext.IsHosted ? appContext.Metadata.FileShareLocation : appContext.Server.Location, "Templates") + "\\";
			if (text.StartsWith(text2, StringComparison.CurrentCultureIgnoreCase))
			{
				text = text.Substring(text2.Length);
			}
			text2 = appContext.Reports.Location;
			if (text.StartsWith(text2, StringComparison.CurrentCultureIgnoreCase))
			{
				text = text.Substring(text2.Length);
			}
			dataRow.SetField("kbpTemplateFile", text.Substring(0, Math.Min(m1BindingSource.Fields["kbpTemplateFile"].FieldLength, text.Length)));
		}
		try
		{
			if (!string.IsNullOrWhiteSpace(message.DocumentTable) && message.DocumentKeys != null && message.DocumentKeys.Count != 0 && message.DocumentKeys[0] != null && message.DocumentKeys[0].Length != 0 && message.DocumentKeyFields != null && message.DocumentKeyFields.Length != 0 && message.DocumentKeys[0].Length.Equals(message.DocumentKeyFields.Length))
			{
				for (int i = 0; i < message.DocumentKeyFields.Length; i++)
				{
					if (message.DocumentKeyFields[i].ToString().Length > 2 && dataRow.Table.Columns.Contains(m1BindingSource.PrimaryTable.FieldPrefix + message.DocumentKeyFields[i].ToString().Substring(3)))
					{
						dataRow[m1BindingSource.PrimaryTable.FieldPrefix + message.DocumentKeyFields[i].ToString().Substring(3)] = message.DocumentKeys[0][i];
					}
				}
			}
		}
		catch
		{
		}
		m1BindingSource.SetKeyToNextAvailable(dataRow);
		m1BindingSource.SaveData();
	}

	private void saveAttachments(M1Database database, string callID, List<MessageAttachment> attachments)
	{
		M1BindingSource m1BindingSource = new M1BindingSource(database);
		m1BindingSource.LoadDefinition(string.Empty, "Attachments", null, true, loadDataNow: false);
		foreach (MessageAttachment attachment in attachments)
		{
			if (attachment.FileName.Length != 0)
			{
				string text = attachment.Description;
				if (string.IsNullOrWhiteSpace(text))
				{
					text = Path.GetFileName(attachment.FileName);
				}
				DataRow dataRow = m1BindingSource.AddNew() as DataRow;
				m1BindingSource.SetKeyToNextAvailable(dataRow);
				dataRow.SetField("cmaDate", DateTime.Today);
				dataRow.SetField("cmaCallID", callID);
				attachment.CopyToFile(attachment.FileName);
				dataRow.SetField("cmaFileLocation", attachment.FileName.Substring(0, Math.Min(m1BindingSource.Fields["cmaFileLocation"].FieldLength, attachment.FileName.Length)));
				dataRow.SetField("cmaShortDescription", text.Substring(0, Math.Min(m1BindingSource.Fields["cmaShortDescription"].FieldLength, text.Length)));
			}
		}
		m1BindingSource.SaveData();
	}

	public string CreateFieldServiceInvoice(M1Database database, string callID, DateTime? invoiceDate, int year, short period)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select * From Calls Where kbpCallID = @CallID");
		sqlCommand.Parameters.Add(new SqlParameter("@CallID", SqlDbType.NVarChar)).Value = callID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			using M1BindingSource m1BindingSource = new M1BindingSource(database);
			m1BindingSource.DataSourceTable = "ARInvoices";
			DataRow dataRow2 = (DataRow)m1BindingSource.AddNew();
			m1BindingSource.SetKeyToNextAvailable(dataRow2);
			dataRow2["arpInvoiceType"] = 1;
			if (string.IsNullOrWhiteSpace(dataRow.Field<string>("kbpARInvoiceOrganizationID")))
			{
				dataRow2["arpCustomerOrganizationID"] = dataRow["KbpOrganizationID"];
				dataRow2["arpARInvoiceLocationID"] = dataRow["KbpLocationID"];
				dataRow2["arpARInvoiceContactID"] = dataRow["KbpContactID"];
			}
			else
			{
				dataRow2["arpCustomerOrganizationID"] = dataRow["kbpARInvoiceOrganizationID"];
				dataRow2["arpARInvoiceLocationID"] = dataRow["kbpARInvoiceLocationID"];
				dataRow2["arpARInvoiceContactID"] = dataRow["kbpARInvoiceContactID"];
			}
			dataRow2["arpShipOrganizationID"] = dataRow["KbpOrganizationID"];
			dataRow2["arpShipLocationID"] = dataRow["KbpLocationID"];
			dataRow2["arpShipContactID"] = dataRow["KbpContactID"];
			if (invoiceDate.HasValue)
			{
				dataRow2["arpInvoiceDate"] = invoiceDate.Value;
			}
			if (year != 0 && period != 0)
			{
				dataRow2["arpGLFiscalYearID"] = year;
				dataRow2["arpGLFiscalYearPeriodID"] = period;
			}
			dataRow2["arpCurrencyRateID"] = dataRow["kbpCurrencyRateID"];
			dataRow2["arpCustomRate"] = dataRow["kbpCustomRate"];
			if (dataRow2.Field<bool>("arpCustomRate"))
			{
				dataRow2["arpExchangeRate"] = dataRow["kbpExchangeRate"];
			}
			sqlCommand = database.NewSqlCommand("Select jmpPlantID,jmpPlantDepartmentID From Jobs Where jmpCallID = @CallID And jmpPlantID <> ''");
			sqlCommand.Parameters.Add(new SqlParameter("@CallID", SqlDbType.NVarChar)).Value = callID;
			DataTable dataTable2 = database.GetDataTable(sqlCommand);
			if (dataTable2.Rows.Count != 0)
			{
				dataRow2["arpPlantID"] = dataTable2.Rows[0]["jmpPlantID"];
				dataRow2["arpPlantDepartmentID"] = dataTable2.Rows[0]["jmpPlantDepartmentID"];
			}
			M1BindingSource childBindingSource = m1BindingSource.PrimaryTable.GetChildBindingSource("ARInvoiceLines");
			new AR().AddTimeAndMaterial(database, childBindingSource, string.Empty, 0, string.Empty, 0, callID);
			m1BindingSource.SaveData();
			sqlCommand = database.NewSqlCommand("UPDATE Calls SET kbpInvoicedComplete = 1 WHERE kbpCallID = @CallID");
			sqlCommand.Parameters.Add(new SqlParameter("@CallID", SqlDbType.NVarChar)).Value = callID;
			database.ExecuteCommand(sqlCommand);
			return dataRow2.Field<string>("arpARInvoiceID");
		}
		return string.Empty;
	}

	public void CreateFieldServiceJob(M1Database database, string callID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select * From Calls Where kbpCallID = @CallID");
		sqlCommand.Parameters.Add(new SqlParameter("@CallID", SqlDbType.NVarChar)).Value = callID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			throw new M1Exception($"Call {callID} does not exist in the Calls table.");
		}
		DataRow dataRow = dataTable.Rows[0];
		string text = dataRow.Field<string>("kbpMethodPartID");
		string text2 = dataRow.Field<string>("kbpMethodRevisionID");
		string text3 = dataRow["kbpShortDescription"].ToString().Substring(0, (dataRow["kbpShortDescription"].ToString().Length > 50) ? 50 : dataRow["kbpShortDescription"].ToString().Length);
		if (string.IsNullOrWhiteSpace(text))
		{
			text = dataRow.Field<string>("kbpPartID");
			text2 = dataRow.Field<string>("kbpPartRevisionID");
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			throw new M1Exception("Part ID is required.");
		}
		string text4 = dataRow.Field<string>("kbpARInvoiceOrganizationID");
		string text5 = dataRow.Field<string>("kbpARInvoiceLocationID");
		string value = dataRow.Field<string>("kbpARInvoiceContactID");
		if (string.IsNullOrWhiteSpace(text4))
		{
			text4 = dataRow.Field<string>("kbpOrganizationID");
			text5 = dataRow.Field<string>("kbpLocationID");
			value = dataRow.Field<string>("kbpContactID");
		}
		DateTime dateTime = ((dataRow["kbpDueDate"] != DBNull.Value) ? dataRow.Field<DateTime>("kbpDueDate") : DateTime.Today);
		clsJobFunctions clsJobFunctions2 = (clsJobFunctions)((ScriptApp)database.GetService(typeof(ScriptApp))).Ax("JobFunctions");
		IOpenObject openObject = (IOpenObject)database.GetService(typeof(IOpenObject));
		try
		{
			string empty = string.Empty;
			if (!database.Props("PN").Field<bool>("xapCMCreateJobOnly"))
			{
				ValidationInfo validationInfo = new ValidationInfo();
				new Organizations().CustomerCreditCheck(database, text4, text5, database.Props("PN").Field<byte>("xapCMFieldServiceCreditMessage"), database.Props("PN").Field<byte>("xapCMFieldServiceHoldMessage"), 0m, 0m, 0m, validationInfo);
				if (validationInfo.ErrorCount != 0)
				{
					throw new M1Exception(validationInfo.ToString());
				}
				M1BindingSource obj = new M1BindingSource(database)
				{
					DataSourceTable = "SalesOrders"
				};
				DataRow dataRow2 = (DataRow)obj.AddNew();
				obj.SetKeyToNextAvailable(dataRow2);
				dataRow2["ompCustomerOrganizationID"] = text4;
				dataRow2["ompShipOrganizationID"] = text4;
				if (string.IsNullOrWhiteSpace(dataRow.Field<string>("kbpARInvoiceOrganizationID")))
				{
					dataRow2["ompShipLocationID"] = text5;
					dataRow2["ompShipContactID"] = value;
				}
				else
				{
					dataRow2["ompARInvoiceLocationID"] = text5;
					dataRow2["ompARInvoiceContactID"] = value;
				}
				dataRow2["ompRequestedShipDate"] = dateTime;
				dataRow2["ompOrderDate"] = dataRow["kbpOpenedDate"];
				dataRow2["ompProjectID"] = dataRow["kbpProjectID"];
				dataRow2["ompStatus"] = 3;
				dataRow2["ompCallID"] = dataRow["kbpCallID"];
				dataRow2["ompCurrencyRateID"] = dataRow["kbpCurrencyRateID"];
				dataRow2["ompCustomRate"] = dataRow["kbpCustomRate"];
				if (dataRow2.Field<bool>("ompCustomRate"))
				{
					dataRow2["ompExchangeRate"] = dataRow["kbpExchangeRate"];
				}
				M1BindingSource childBindingSource = obj.PrimaryTable.GetChildBindingSource("SalesOrderLines");
				DataRow dataRow3 = (DataRow)childBindingSource.AddNew();
				childBindingSource.SetKeyToNextAvailable(dataRow3);
				dataRow3["omlPartID"] = text;
				dataRow3["omlPartRevisionID"] = text2;
				dataRow3["omlPartShortDescription"] = text3;
				dataRow3["omlPartLongdescriptionRTF"] = dataRow["kbpLongDescriptionRTF"];
				dataRow3["omlPartLongDescriptionText"] = dataRow["kbpLongDescriptionText"];
				dataRow3["omlPartGroupID"] = dataRow["kbpPartGroupID"];
				dataRow3["omlOrderQuantity"] = 1;
				dataRow3["omlTimeAndMaterial"] = true;
				M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("SalesOrderDeliveries");
				DataRow[] array = childBindingSource2.GetDataTable().Select("omdSalesOrderID = " + dataRow3.Field<string>("omlSalesOrderID").ToLinq() + " And omdSalesOrderLineID = " + dataRow3.Field<short>("omlSalesOrderLineID").ToLinq());
				DataRow dataRow4;
				if (array.Length == 0)
				{
					dataRow4 = (DataRow)childBindingSource2.AddNew();
					childBindingSource2.SetKeyToNextAvailable(dataRow4);
				}
				else
				{
					dataRow4 = array[0];
				}
				if (dataRow4 != null)
				{
					dataRow4["omdPartID"] = dataRow3["omlPartID"];
					dataRow4["omdPartRevisionID"] = dataRow3["omlPartRevisionID"];
					dataRow4["omdDeliveryQuantity"] = 1;
					dataRow4["omdDeliveryDate"] = dateTime;
					dataRow4["omdDeliveryType"] = 1;
				}
				obj.SaveData();
				empty = ((!database.Props("PN").Field<bool>("xapOMIncludeOrderLineInJob")) ? ((string)database.NextIDs.GetNextIDForTable("Jobs")) : new Job().GetJobIDForOrder(database, dataRow3.Field<string>("omlSalesOrderID"), dataRow3.Field<short>("omlSalesOrderLineID"), false));
				clsJobFunctions2.CreateJob(empty, text, text2, text3, string.Empty, 1.0, dateTime, dataRow4.Field<string>("omdSalesOrderID"), dataRow4.Field<short>("omdSalesOrderLineID"), dataRow4.Field<short>("omdSalesOrderDeliveryID"), 0.0, string.Empty, string.Empty, dataRow.Field<string>("kbpCallID"), text4);
				clsJobFunctions obj2 = clsJobFunctions2;
				string cJob = empty;
				string cSourcePartID = text;
				string cSourceRevisionID = text2;
				object aParameters = "";
				obj2.GetMethod(cJob, 0, cSourcePartID, cSourceRevisionID, bOverwriteMethod: true, bOverwriteDescription: false, bOverwriteDocuments: true, bRefreshMaterialDescriptions: true, bRefreshMaterialCosts: true, ref aParameters);
				openObject.OpenObject("SalesOrder", new string[1] { dataRow2.Field<string>("ompSalesOrderID") });
			}
			else
			{
				empty = (string)database.NextIDs.GetNextIDForTable("Jobs");
				clsJobFunctions2.CreateJob(empty, text, text2, text3, string.Empty, 1.0, dateTime, string.Empty, 0, 0, 0.0, string.Empty, string.Empty, dataRow.Field<string>("kbpCallID"), text4);
				clsJobFunctions obj3 = clsJobFunctions2;
				string cJob2 = empty;
				string cSourcePartID2 = text;
				string cSourceRevisionID2 = text2;
				object aParameters = "";
				obj3.GetMethod(cJob2, 0, cSourcePartID2, cSourceRevisionID2, bOverwriteMethod: true, bOverwriteDescription: false, bOverwriteDocuments: true, bRefreshMaterialDescriptions: true, bRefreshMaterialCosts: true, ref aParameters);
				openObject.OpenObject("Job", new string[1] { empty });
			}
		}
		finally
		{
			clsJobFunctions2 = null;
			openObject = null;
		}
	}

	public string GetCallInfo(M1Database databaseRef, DataRow drSource, bool bIncludeDate, bool bDescending = false, bool bIncludeInternal = true)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string text4 = string.Empty;
		empty = drSource.Field<string>("kbpCallID").Trim();
		if (bIncludeDate)
		{
			empty2 = drSource["kbpOrganizationID"].ToString().Trim();
			empty3 = drSource["kbpLocationID"].ToString().Trim();
			empty4 = drSource["kbpContactID"].ToString().Trim();
			DataTable dataTable;
			SqlCommand sqlCommand;
			if (empty4.Length != 0)
			{
				sqlCommand = databaseRef.NewSqlCommand("select cmcName,cmcPhoneNumber from OrganizationContacts Where cmcOrganizationID = @OrgID And cmcLocationID = @LocID And cmcContactID = @ContactID");
				sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = empty2;
				sqlCommand.Parameters.Add(new SqlParameter("@LocID", SqlDbType.NVarChar)).Value = empty3;
				sqlCommand.Parameters.Add(new SqlParameter("@ContactID", SqlDbType.NVarChar)).Value = empty4;
				dataTable = databaseRef.GetDataTable(sqlCommand);
				if (dataTable.Rows.Count > 0)
				{
					text = dataTable.Rows[0]["cmcPhoneNumber"].ToString().Trim();
					text2 = dataTable.Rows[0]["cmcName"].ToString().Trim();
				}
				dataTable.Clear();
			}
			sqlCommand = databaseRef.NewSqlCommand("select cmlName,cmlPhoneNumber from OrganizationLocations Where cmlOrganizationID = @OrgID And cmlLocationID = @LocID");
			sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = empty2;
			sqlCommand.Parameters.Add(new SqlParameter("@LocID", SqlDbType.NVarChar)).Value = empty3;
			dataTable = databaseRef.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count > 0)
			{
				if (text.Length == 0)
				{
					text = dataTable.Rows[0]["cmlPhoneNumber"].ToString().Trim();
				}
				if (text2.Length != 0)
				{
					text2 += " - ";
				}
				text2 += dataTable.Rows[0]["cmlName"].ToString().Trim();
			}
			dataTable.Clear();
			text3 = text3 + "Caller: " + text2 + "\r";
			text3 = text3 + "Phone: " + text + "\r";
			if (drSource["kbpOpenedDate"] != DBNull.Value)
			{
				text4 = drSource.Field<DateTime>("kbpOpenedDate").ToString("G");
			}
			sqlCommand = databaseRef.NewSqlCommand("select lmeEmployeeName from Employees Where lmeEmployeeID = @EmployeeID");
			sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar)).Value = drSource.Field<string>("kbpOpenedByEmployeeID");
			DataTable dataTable2 = databaseRef.GetDataTable(sqlCommand);
			text4 = ((dataTable2.Rows.Count <= 0) ? (text4 + "  " + dataTable2.Rows[0]["kbpOpenedByEmployeeID"].ToString().Trim()) : (text4 + "  " + dataTable2.Rows[0]["lmeEmployeeName"].ToString().Trim()));
			dataTable2.Clear();
			text4 += "\r";
		}
		text4 = text4 + drSource.Field<string>("kbpShortDescription").Trim() + "\r";
		if (!string.IsNullOrWhiteSpace(drSource.Field<string>("kbpLongDescriptionText")))
		{
			text4 += drSource.Field<string>("kbpLongDescriptionText").Trim();
		}
		if (!bDescending)
		{
			text3 += text4;
		}
		if (empty.Length != 0)
		{
			SqlCommand sqlCommand = ((!bIncludeInternal) ? databaseRef.NewSqlCommand("select kblShortDescription,kblLongDescriptionText,kblAddedDate,kblAddedByEmployeeID,lmeEmployeeName From CallLines Left Outer Join Employees On kblAddedByEmployeeID = lmeEmployeeID Where kblCallID = @CallID and kblInternalOnly = 0 and kblCallLineID <> 0 Order by kblCallLineID " + (bDescending ? "Desc" : "")) : databaseRef.NewSqlCommand("select kblShortDescription,kblLongDescriptionText,kblAddedDate,kblAddedByEmployeeID,lmeEmployeeName From CallLines Left Outer Join Employees On kblAddedByEmployeeID = lmeEmployeeID Where kblCallID = @CallID and kblCallLineID <> 0 Order by kblCallLineID " + (bDescending ? "Desc" : "")));
			sqlCommand.Parameters.Add(new SqlParameter("@CallID", SqlDbType.NVarChar)).Value = empty;
			foreach (DataRow row in databaseRef.GetDataTable(sqlCommand).Rows)
			{
				if (!text3.EndsWith("\r") && !text3.EndsWith("\n"))
				{
					text3 += "\r";
				}
				if (bIncludeDate)
				{
					if (row["kblAddedDate"] != DBNull.Value)
					{
						text3 = text3 + "\r" + row.Field<DateTime>("kblAddedDate").ToString("G");
					}
					text3 = ((row["lmeEmployeeName"] == DBNull.Value) ? (text3 + "  " + row.Field<string>("kblAddedByEmployeeID").Trim()) : (text3 + "  " + row.Field<string>("lmeEmployeeName").Trim()));
					text3 += "\r";
				}
				text3 = text3 + row.Field<string>("kblShortDescription").Trim() + "\r";
				if (!string.IsNullOrWhiteSpace(row.Field<string>("kblLongDescriptionText")))
				{
					text3 += row.Field<string>("kblLongDescriptionText").Trim();
				}
			}
		}
		if (bDescending)
		{
			text3 = text3 + "\r" + text4.Trim();
		}
		return text3;
	}
}
