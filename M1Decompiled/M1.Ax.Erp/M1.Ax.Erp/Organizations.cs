using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using M1.Core;
using M1.ShippingServices.DTO;
using M1.ShippingServices.FedEx;
using M1.ShippingServices.Repository;
using M1.ShippingServices.UPS;

namespace M1.Ax.Erp;

public class Organizations
{
	private string BulidAddressValidationText(DataTable dataTable)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DataRow row in dataTable.Rows)
		{
			stringBuilder.Append(string.Join(", ", row.ItemArray));
			stringBuilder.Append("|");
		}
		return stringBuilder.ToString();
	}

	public void CustomerCreditCheck(M1Database database, string customerID, string locationID, byte creditMsgType, byte holdMsgType, decimal orderOffsetAmount, decimal shipmentOffsetAmount, decimal invoiceOffsetAmount, ValidationInfo validationInfo, bool isShipment = false)
	{
		customerID = customerID.Trim();
		if (customerID.Length == 0 || (creditMsgType <= 0 && holdMsgType <= 0))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select cmoName,cmoCreditHold,cmoCustomerCreditLimit,cmlName,cmlCreditCheckForLocation,cmlCreditHold,cmlCustomerCreditLimit from OrganizationLocations WITH (NOLOCK) Inner Join Organizations On cmoOrganizationID = cmlOrganizationID where cmlOrganizationID = @CustomerID And cmlLocationID = @LocationID");
		sqlCommand.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.NVarChar)).Value = customerID;
		sqlCommand.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.NVarChar)).Value = locationID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		decimal num = default(decimal);
		DataRow row = dataTable.Rows[0];
		string empty = string.Empty;
		string empty2 = string.Empty;
		object obj;
		if (row.Field<bool>("cmlCreditCheckForLocation"))
		{
			if (row.Field<bool>("cmlCreditHold") && holdMsgType > 0)
			{
				empty = "Customer location " + row.Field<string>("cmlName").Trim() + " is on credit hold";
				if (holdMsgType == 1)
				{
					validationInfo.AddWarning(empty);
				}
				else
				{
					validationInfo.AddError(empty);
				}
			}
			if (row.Field<decimal>("cmlCustomerCreditLimit") > 0m && creditMsgType > 0)
			{
				num = default(decimal);
				if (database.Props("PN").Field<bool>("xapCMCreditLimitSourceOrder"))
				{
					sqlCommand = database.NewSqlCommand("select sum(case when omlOrderQuantity - omlQuantityShipped <= 0 THEN 0 ELSE (omlOrderQuantity - omlQuantityShipped) * omlUnitPriceBase END) as TotalAmount from SalesOrderLines Inner Join SalesOrders ON omlSalesOrderID = ompSalesOrderID where ompCustomerOrganizationID = @CustomerID And ompARInvoiceLocationID = @LocationID AND ompClosed = 0");
					sqlCommand.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.NVarChar)).Value = customerID;
					sqlCommand.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.NVarChar)).Value = locationID;
					obj = database.ExecuteScalar(sqlCommand);
					if (obj != null && obj != DBNull.Value)
					{
						num += (decimal)obj;
					}
					num += orderOffsetAmount;
					if (isShipment)
					{
						num -= shipmentOffsetAmount;
						if (num < 0m)
						{
							num = default(decimal);
						}
					}
				}
				if (database.Props("PN").Field<bool>("xapCMCreditLimitSourceShip"))
				{
					sqlCommand = database.NewSqlCommand("select sum(Round((smlQuantityShipped+smlJobQuantityShipped) * smlUnitPrice,2)) as TotalAmount from ShipmentLines Inner Join Shipments On smlShipmentID=smpShipmentID where smlInvoicedComplete = 0 and smpClosed = 0 and smpCustomerOrganizationID = @CustomerID And smpARInvoiceLocationID = @LocationID");
					sqlCommand.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.NVarChar)).Value = customerID;
					sqlCommand.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.NVarChar)).Value = locationID;
					obj = database.ExecuteScalar(sqlCommand);
					if (obj != null && obj != DBNull.Value)
					{
						num += (decimal)obj;
					}
					num += shipmentOffsetAmount;
				}
				if (database.Props("PN").Field<bool>("xapCMCreditLimitSourceInv"))
				{
					sqlCommand = database.NewSqlCommand("select sum(arpInvoiceBalanceBase) as TotalAmount from ARInvoices where arpCustomerOrganizationID = @CustomerID And arpARInvoiceLocationID = @LocationID And arpOnHold = 0");
					sqlCommand.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.NVarChar)).Value = customerID;
					sqlCommand.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.NVarChar)).Value = locationID;
					obj = database.ExecuteScalar(sqlCommand);
					if (obj != null && obj != DBNull.Value)
					{
						num += (decimal)obj;
					}
					num += invoiceOffsetAmount;
				}
				if (num > row.Field<decimal>("cmlCustomerCreditLimit"))
				{
					empty2 = database.HomeCurrencySymbol;
					if (empty2.Length != 0)
					{
						empty2 = "\"" + empty2 + "\"";
					}
					empty = "Customer location " + row.Field<string>("cmlName").Trim() + " is over their credit limit (" + row.Field<decimal>("cmlCustomerCreditLimit").ToString(empty2 + "###,###,##0.00") + " limit, " + num.ToString(empty2 + "###,###,##0.00") + " taken)";
					if (creditMsgType == 1)
					{
						validationInfo.AddWarning(empty);
					}
					else
					{
						validationInfo.AddError(empty);
					}
				}
			}
		}
		else
		{
			if (row.Field<bool>("cmoCreditHold") && holdMsgType > 0)
			{
				empty = "Customer " + row.Field<string>("cmoName").Trim() + " is on credit hold";
				if (holdMsgType == 1)
				{
					validationInfo.AddWarning(empty);
				}
				else
				{
					validationInfo.AddError(empty);
				}
			}
			if (row.Field<decimal>("cmoCustomerCreditLimit") > 0m && creditMsgType > 0)
			{
				num = default(decimal);
				if (database.Props("PN").Field<bool>("xapCMCreditLimitSourceOrder"))
				{
					sqlCommand = database.NewSqlCommand("select sum(case when omlOrderQuantity - omlQuantityShipped <= 0 THEN 0 ELSE (omlOrderQuantity - omlQuantityShipped) * omlUnitPriceBase END) as TotalAmount from SalesOrderLines Inner Join SalesOrders ON omlSalesOrderID = ompSalesOrderID where ompCustomerOrganizationID = @CustomerID AND ompClosed = 0");
					sqlCommand.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.NVarChar)).Value = customerID;
					obj = database.ExecuteScalar(sqlCommand);
					if (obj != null && obj != DBNull.Value)
					{
						num += (decimal)obj;
					}
					num += orderOffsetAmount;
					if (isShipment)
					{
						num -= shipmentOffsetAmount;
						if (num < 0m)
						{
							num = default(decimal);
						}
					}
				}
				if (database.Props("PN").Field<bool>("xapCMCreditLimitSourceShip"))
				{
					sqlCommand = database.NewSqlCommand("select sum(Round((smlQuantityShipped+smlJobQuantityShipped) * smlUnitPrice,2)) as TotalAmount from ShipmentLines Inner Join Shipments On smlShipmentID=smpShipmentID where smlInvoicedComplete = 0 and smpClosed = 0 and smpCustomerOrganizationID = @CustomerID");
					sqlCommand.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.NVarChar)).Value = customerID;
					obj = database.ExecuteScalar(sqlCommand);
					if (obj != null && obj != DBNull.Value)
					{
						num += (decimal)obj;
					}
					num += shipmentOffsetAmount;
				}
				if (database.Props("PN").Field<bool>("xapCMCreditLimitSourceInv"))
				{
					sqlCommand = database.NewSqlCommand("select sum(arpInvoiceBalanceBase) as TotalAmount from ARInvoices where arpCustomerOrganizationID = @CustomerID And arpOnHold = 0");
					sqlCommand.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.NVarChar)).Value = customerID;
					obj = database.ExecuteScalar(sqlCommand);
					if (obj != null && obj != DBNull.Value)
					{
						num += (decimal)obj;
					}
					num += invoiceOffsetAmount;
				}
				if (num > row.Field<decimal>("cmoCustomerCreditLimit"))
				{
					empty2 = database.HomeCurrencySymbol;
					if (empty2.Length != 0)
					{
						empty2 = "\"" + empty2 + "\"";
					}
					empty = "Customer " + row.Field<string>("cmoName").Trim() + " is over their credit limit (" + row.Field<decimal>("cmoCustomerCreditLimit").ToString(empty2 + "###,###,##0.00") + " limit, " + num.ToString(empty2 + "###,###,##0.00") + " taken)";
					if (creditMsgType == 1)
					{
						validationInfo.AddWarning(empty);
					}
					else
					{
						validationInfo.AddError(empty);
					}
				}
			}
		}
		if (creditMsgType <= 0)
		{
			return;
		}
		if (row.Field<bool>("cmlCreditCheckForLocation"))
		{
			sqlCommand = database.NewSqlCommand("select (sum(arpInvoiceBalanceBase - arpRetentionBalanceBase)) As TotalAmount from ARInvoices Inner Join PaymentTerms On arpPaymentTermID = xatPaymentTermID Where arpCustomerOrganizationID = @CustomerID And arpARInvoiceLocationID = @LocationID and arpInvoiceBalanceBase > 0 and dateadd(d,xatGracePeriod,arpDueDate) < {fn CURDATE()} And arpOnHold = 0");
			sqlCommand.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.NVarChar)).Value = customerID;
			sqlCommand.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.NVarChar)).Value = locationID;
			obj = database.ExecuteScalar(sqlCommand);
			if (obj != null && obj != DBNull.Value && (decimal)obj > 0m)
			{
				empty = "Customer location " + row.Field<string>("cmlName").Trim() + " has open invoices that are past the grace period";
				if (creditMsgType == 1)
				{
					validationInfo.AddWarning(empty);
				}
				else
				{
					validationInfo.AddError(empty);
				}
			}
			return;
		}
		sqlCommand = database.NewSqlCommand("select (sum(arpInvoiceBalanceBase - arpRetentionBalanceBase)) As TotalAmount from ARInvoices Inner Join PaymentTerms On arpPaymentTermID = xatPaymentTermID Where arpCustomerOrganizationID = @CustomerID and arpInvoiceBalanceBase > 0 and dateadd(d,xatGracePeriod,arpDueDate) < {fn CURDATE()} And arpOnHold = 0");
		sqlCommand.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.NVarChar)).Value = customerID;
		obj = database.ExecuteScalar(sqlCommand);
		if (obj != null && obj != DBNull.Value && (decimal)obj > 0m)
		{
			empty = "Customer " + row.Field<string>("cmoName").Trim() + " has open invoices that are past the grace period";
			if (creditMsgType == 1)
			{
				validationInfo.AddWarning(empty);
			}
			else
			{
				validationInfo.AddError(empty);
			}
		}
	}

	public void AddressValidation(M1BindingSource bindingSource)
	{
		ShippingAddressDto addressToValidate = new ShippingAddressDto();
		new ShippingAddressDto();
		StringBuilder errorText = new StringBuilder();
		DataTable dataTable = null;
		UPSDatasetLevelConfigInfoDto uPSDatasetLevelConfigInfoDto = null;
		FedExDatasetLevelConfigInfoDto fedExDatasetLevelConfigInfoDto = null;
		IShipmentRepository shipmentRepository = new ShipmentRepository();
		string empty = string.Empty;
		string text = string.Empty;
		string text2 = string.Empty;
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		if (bindingSource.DataSourceTable.ToString().Trim().Equals("ORGANIZATIONLOCATIONS", StringComparison.CurrentCultureIgnoreCase))
		{
			text = currentAsDataRow.Field<string>("cmlCustomerShippingMethodID").Trim();
			currentAsDataRow.SetField("cmlAddressValidationResult", string.Empty);
		}
		else if (bindingSource.DataSourceTable.ToString().Trim().Equals("ORGANIZATIONS", StringComparison.CurrentCultureIgnoreCase))
		{
			text = currentAsDataRow.Field<string>("cmoCustomerShippingMethodID").Trim();
			currentAsDataRow.SetField("cmoAddressValidationResult", string.Empty);
		}
		if (!string.IsNullOrEmpty(text))
		{
			text2 = shipmentRepository.GetShippingCarrier(bindingSource.Database, text);
		}
		if (text2.Trim().Equals("UPS", StringComparison.CurrentCultureIgnoreCase))
		{
			dataTable = new UPSAddressValidation(bindingSource.Database).GetUPSValidatedOrganisationAddressAsTable(currentAsDataRow, bindingSource.DataSourceTable, out addressToValidate, out uPSDatasetLevelConfigInfoDto, ref errorText);
		}
		else if (text2.Trim().Equals("FDXG", StringComparison.CurrentCultureIgnoreCase) || text2.Trim().Equals("FDXE", StringComparison.CurrentCultureIgnoreCase) || text2.Trim().Equals("FEDEX", StringComparison.CurrentCultureIgnoreCase))
		{
			dataTable = new FedExAddressValidation(bindingSource.Database).GetFedExAddressValidatedOrganizationAsTable(currentAsDataRow, bindingSource.DataSourceTable, out addressToValidate, out fedExDatasetLevelConfigInfoDto, ref errorText);
		}
		else
		{
			MessageBox.Show($"Invalid Shipping Method - [{text}]!", "Address Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		if (dataTable.Rows.Count == 0)
		{
			MessageBox.Show(errorText.ToString(), "Address Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		empty = BulidAddressValidationText(dataTable);
		if (bindingSource.DataSourceTable.ToString().Trim().Equals("ORGANIZATIONLOCATIONS", StringComparison.CurrentCultureIgnoreCase))
		{
			currentAsDataRow.SetField("cmlAddressValidationResult", empty);
		}
		else if (bindingSource.DataSourceTable.ToString().Trim().Equals("ORGANIZATIONS", StringComparison.CurrentCultureIgnoreCase))
		{
			currentAsDataRow.SetField("cmoAddressValidationResult", empty);
		}
	}

	public string GetCaptionAddressValidation(M1Database database)
	{
		_ = string.Empty;
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT xsmFedExAuthenticationMethod FROM ShippingProperties");
		if (!(database.GetDataTable(sqlCommand).Rows[0].Field<string>("xsmFedExAuthenticationMethod") == "2"))
		{
			return "UPS Validate Address";
		}
		return "FedEx Validate Address";
	}
}
