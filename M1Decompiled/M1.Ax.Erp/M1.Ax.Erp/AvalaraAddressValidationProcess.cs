using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Avalara.AvaTax.Adapter;
using M1.Ax.Erp.Financials.Avalara;
using M1.Core;

namespace M1.Ax.Erp;

public class AvalaraAddressValidationProcess : ProcessParameters
{
	public AvalaraAddressValidationProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[2] { "cmlOrganizationID", "cmlLocationID" };
		KeyValueTableName = "OrganizationLocations";
		Description = "Select the Organization/Locations address to be validated via Avalara.";
		GridID = "M1PROCESSAVALARAADDRESSVALIDATION";
		BindingSourceTable = "OrganizationLocations";
		ShowRefresh = true;
		HelpLink = "Avalara_Address_Validation.htm";
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Show Addresses Not Validated Only?")
		{
			AdoFilterExpression = "cmlAvalaraAddressValidated = 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "cmlAvalaraAddressValidated"
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Show Customers Only?")
		{
			AdoFilterExpression = "cmoCustomerStatus <> 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "cmoCustomerStatus"
		});
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length != 0)
		{
			M1Database database = BindingSource.Database;
			M1User user = BindingSource.User;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			DataTable dataTable = database.GetDataTable("SELECT cmlOrganizationID, cmlLocationID, cmlAddressLine1, cmlAddressLine2, cmlAddressLine3, cmlCity, cmlCounty, cmlState, cmlPostCode, cmlCountry From OrganizationLocations Where " + text + " Order By cmlOrganizationID, cmlLocationID");
			if (dataTable.Rows.Count != 0)
			{
				AvalaraAddressFunctions avalaraAddressFunctions = new AvalaraAddressFunctions(database, user);
				SqlCommand sqlCommand = database.NewSqlCommand("Update OrganizationLocations Set cmlAvalaraAddressValidated = -1,  cmlAddressLine1 = @line1, cmlAddressLine2 = @line2, cmlAddressLine3 = @line3, cmlCity = @city, cmlCounty = @county, cmlState = @state, cmlPostCode = @postcode, cmlCountry = @country Where cmlOrganizationID = @orgID And cmlLocationID = @locID");
				sqlCommand.Parameters.Add(new SqlParameter("@orgID", SqlDbType.Char, 10));
				sqlCommand.Parameters.Add(new SqlParameter("@locID", SqlDbType.Char, 5));
				sqlCommand.Parameters.Add(new SqlParameter("@line1", SqlDbType.VarChar));
				sqlCommand.Parameters.Add(new SqlParameter("@line2", SqlDbType.VarChar));
				sqlCommand.Parameters.Add(new SqlParameter("@line3", SqlDbType.VarChar));
				sqlCommand.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar));
				sqlCommand.Parameters.Add(new SqlParameter("@state", SqlDbType.VarChar));
				sqlCommand.Parameters.Add(new SqlParameter("@postcode", SqlDbType.VarChar));
				sqlCommand.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar));
				sqlCommand.Parameters.Add(new SqlParameter("@county", SqlDbType.VarChar));
				SqlCommand sqlCommand2 = database.NewSqlCommand("Update Organizations Set cmoAvalaraAddressValidated = -1, cmoAddressLine1 = @line1, cmoAddressLine2 = @line2, cmoAddressLine3 = @line3, cmoCity = @city, cmoCounty = @county, cmoState = @state, cmoPostCode = @postcode, cmoCountry = @country Where cmoOrganizationID = @orgID");
				sqlCommand2.Parameters.Add(new SqlParameter("@orgID", SqlDbType.Char, 10));
				sqlCommand2.Parameters.Add(new SqlParameter("@line1", SqlDbType.VarChar));
				sqlCommand2.Parameters.Add(new SqlParameter("@line2", SqlDbType.VarChar));
				sqlCommand2.Parameters.Add(new SqlParameter("@line3", SqlDbType.VarChar));
				sqlCommand2.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar));
				sqlCommand2.Parameters.Add(new SqlParameter("@state", SqlDbType.VarChar));
				sqlCommand2.Parameters.Add(new SqlParameter("@postcode", SqlDbType.VarChar));
				sqlCommand2.Parameters.Add(new SqlParameter("@country", SqlDbType.VarChar));
				sqlCommand2.Parameters.Add(new SqlParameter("@county", SqlDbType.VarChar));
				foreach (DataRow row in dataTable.Rows)
				{
					string text2 = row.Field<string>("cmlOrganizationID").Trim() + ((row.Field<string>("cmlLocationID").Trim().Length > 0) ? ("-" + row.Field<string>("cmlLocationID")) : string.Empty);
					AvalaraAddressFunctions.AddressInfo addressInfo = new AvalaraAddressFunctions.AddressInfo();
					addressInfo.Line1 = row.Field<string>("cmlAddressLine1");
					addressInfo.Line2 = row.Field<string>("cmlAddressLine2");
					addressInfo.Line3 = row.Field<string>("cmlAddressLine3");
					addressInfo.City = row.Field<string>("cmlCity");
					addressInfo.Region = row.Field<string>("cmlState");
					addressInfo.PostalCode = row.Field<string>("cmlPostCode");
					addressInfo.Country = row.Field<string>("cmlCountry");
					addressInfo.table = "OrganizationLocations";
					addressInfo.recordID = text2;
					AvalaraAddressFunctions.AddressInfo addressInfo2 = avalaraAddressFunctions.ValidateAddresses(addressInfo);
					if ((addressInfo2.Severity == SeverityLevel.Success || addressInfo2.Severity == SeverityLevel.Warning) && addressInfo2.Updated)
					{
						sqlCommand.Parameters["@orgID"].Value = row.Field<string>("cmlOrganizationID");
						sqlCommand.Parameters["@locID"].Value = row.Field<string>("cmlLocationID");
						sqlCommand.Parameters["@line1"].Value = addressInfo2.Line1;
						sqlCommand.Parameters["@line2"].Value = addressInfo2.Line2;
						sqlCommand.Parameters["@line3"].Value = addressInfo2.Line3;
						sqlCommand.Parameters["@city"].Value = addressInfo2.City;
						sqlCommand.Parameters["@state"].Value = addressInfo2.Region;
						sqlCommand.Parameters["@postcode"].Value = addressInfo2.PostalCode;
						sqlCommand.Parameters["@country"].Value = addressInfo2.Country;
						sqlCommand.Parameters["@county"].Value = addressInfo2.County;
						database.ExecuteCommand(sqlCommand);
						if (row.Field<string>("cmlLocationID").Trim().Length == 0)
						{
							sqlCommand2.Parameters["@orgID"].Value = row.Field<string>("cmlOrganizationID");
							sqlCommand2.Parameters["@line1"].Value = addressInfo2.Line1;
							sqlCommand2.Parameters["@line2"].Value = addressInfo2.Line2;
							sqlCommand2.Parameters["@line3"].Value = addressInfo2.Line3;
							sqlCommand2.Parameters["@city"].Value = addressInfo2.City;
							sqlCommand2.Parameters["@state"].Value = addressInfo2.Region;
							sqlCommand2.Parameters["@postcode"].Value = addressInfo2.PostalCode;
							sqlCommand2.Parameters["@country"].Value = addressInfo2.Country;
							sqlCommand2.Parameters["@county"].Value = addressInfo2.County;
							database.ExecuteCommand(sqlCommand2);
						}
						list.Add(text2);
					}
					else if (addressInfo2.Severity == SeverityLevel.Error || addressInfo2.Severity == SeverityLevel.Exception)
					{
						list2.Add(text2);
						messages.Add("Organization " + row.Field<string>("cmlOrganizationID").Trim() + ((row.Field<string>("cmlLocationID").Trim().Length > 0) ? (" : Location " + row.Field<string>("cmlLocationID").Trim()) : string.Empty) + " returned error when validating address.");
					}
				}
			}
		}
		BindingSource = null;
	}
}
