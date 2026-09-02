using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using M1.Core;

namespace M1.Ax.Erp;

internal class SuperClearingHouseExportFunctions
{
	public enum SuperFormat
	{
		None,
		LUCRFSuper,
		QuickSuper,
		SunSuper
	}

	public void ExportSuperClearingHouseFile(IServiceProvider provider, string processQuery, DateTime? startDate, DateTime? endDate, string employerID, SuperFormat superFileFormat, string dateFormat, List<string> messages)
	{
		if (!startDate.HasValue)
		{
			messages.Add("Super start date is required.");
		}
		if (!endDate.HasValue)
		{
			messages.Add("Super end date is required.");
		}
		M1Database m1Database = provider.GetService(typeof(M1Database)) as M1Database;
		string iD = m1Database.ID;
		DataTable dataTable = null;
		dataTable = m1Database.GetDataTable(generateSuperQuery(processQuery, superFileFormat));
		evaluateSuperData(dataTable, superFileFormat, messages);
		if (messages.Count == 0)
		{
			if (superFileFormat == SuperFormat.QuickSuper)
			{
				formatQuickSuperValues(dataTable, dateFormat);
			}
			if (superFileFormat == SuperFormat.LUCRFSuper)
			{
				FormatLUCRFSuperValues(iD, dataTable);
			}
			generateCSV(m1Database, dataTable, superFileFormat);
		}
	}

	private SqlCommand generateSuperQuery(string query, SuperFormat superFileFormat)
	{
		return new SqlCommand(query);
	}

	private void evaluateSuperData(DataTable dtDetails, SuperFormat SuperFileFormat, List<string> messages)
	{
		new StringBuilder();
		int num = 0;
		switch (SuperFileFormat)
		{
		case SuperFormat.SunSuper:
		{
			string text = "C-PDSSR0100";
			{
				foreach (DataRow row in dtDetails.Rows)
				{
					num++;
					_ = string.Empty;
					string sEmployeeID2 = row["PayrollID"].ToString().Trim();
					IsColumnEmpty(row, sEmployeeID2, "USI", num, messages);
					IsColumnEmpty(row, sEmployeeID2, "FamilyName", num, messages);
					IsColumnEmpty(row, sEmployeeID2, "GivenNames", num, messages);
					IsColumnEmpty(row, sEmployeeID2, "DOB", num, messages);
					IsColumnEmpty(row, sEmployeeID2, "Gender", num, messages);
					IsColumnEmpty(row, sEmployeeID2, "TFN", num, messages);
					isAddressValid(row, sEmployeeID2, SuperFileFormat, num, messages);
					if (row["USI"].ToString().Trim() == text)
					{
						row["USI"] = string.Empty;
					}
				}
				break;
			}
		}
		case SuperFormat.QuickSuper:
		{
			foreach (DataRow row2 in dtDetails.Rows)
			{
				num++;
				_ = string.Empty;
				string sEmployeeID3 = row2["PayrollID"].ToString().Trim();
				IsColumnEmpty(row2, sEmployeeID3, "FamilyName", num, messages);
				IsColumnEmpty(row2, sEmployeeID3, "GivenName", num, messages);
				IsColumnEmpty(row2, sEmployeeID3, "DateOfBirth_SourceDate", num, messages);
				IsColumnEmpty(row2, sEmployeeID3, "FundID", num, messages);
				IsColumnEmpty(row2, sEmployeeID3, "MemberID", num, messages);
				row2["Country"] = GetCountryCode(row2["Country"].ToString());
				if (row2["Country"].ToString() == "AU")
				{
					isAddressValid(row2, sEmployeeID3, SuperFileFormat, num, messages);
				}
				else
				{
					IsColumnEmpty(row2, sEmployeeID3, "AddressLine1", num, messages);
				}
			}
			break;
		}
		case SuperFormat.LUCRFSuper:
			if (dtDetails.Rows.Count <= 0)
			{
				break;
			}
			{
				foreach (DataRow row3 in dtDetails.Rows)
				{
					num++;
					_ = string.Empty;
					string sEmployeeID = row3["Payroll_ID"].ToString().Trim();
					IsColumnEmpty(row3, sEmployeeID, "Employer_ABN", num, messages);
					IsColumnEmpty(row3, sEmployeeID, "Employer_name", num, messages);
					IsColumnEmpty(row3, sEmployeeID, "Employee_TFN", num, messages);
					IsColumnEmpty(row3, sEmployeeID, "Date_of_birth", num, messages);
					IsColumnEmpty(row3, sEmployeeID, "Gender", num, messages);
					IsColumnEmpty(row3, sEmployeeID, "Last_name", num, messages);
					IsColumnEmpty(row3, sEmployeeID, "First_name", num, messages);
					IsColumnEmpty(row3, sEmployeeID, "Address_line_1", num, messages);
					IsColumnEmpty(row3, sEmployeeID, "Suburb", num, messages);
					if (!row3["lnfSMSF"].Equals(DBNull.Value) && Convert.ToBoolean(row3["lnfSMSF"]).Equals(obj: true))
					{
						IsColumnEmpty(row3, sEmployeeID, "SMSF_ABN", num, messages);
						IsColumnEmpty(row3, sEmployeeID, "SMSF_name", num, messages);
						IsColumnEmpty(row3, sEmployeeID, "SMSF_electronic_service_address", num, messages);
						IsColumnEmpty(row3, sEmployeeID, "SMSF_BSB", num, messages);
						IsColumnEmpty(row3, sEmployeeID, "SMSF_account_number", num, messages);
						IsColumnEmpty(row3, sEmployeeID, "SMSF_account_name", num, messages);
						row3["USI"] = string.Empty;
					}
					else
					{
						row3["SMSF_ABN"] = string.Empty;
						row3["SMSF_name"] = string.Empty;
						row3["SMSF_electronic_service_address"] = string.Empty;
						row3["SMSF_BSB"] = string.Empty;
						row3["SMSF_account_number"] = string.Empty;
						row3["SMSF_account_name"] = string.Empty;
						IsColumnEmpty(row3, sEmployeeID, "USI", num, messages);
					}
					if (row3["Country"].ToString() == string.Empty)
					{
						row3["Country"] = "AU";
					}
					row3["Country"] = GetCountryCode(row3["Country"].ToString());
					if (row3["Country"].ToString() == "AU")
					{
						isAddressValid(row3, sEmployeeID, SuperFileFormat, num, messages);
					}
				}
				break;
			}
		}
	}

	private void generateCSV(M1Database database, DataTable dtDetails, SuperFormat exportFormat)
	{
		try
		{
			string text = database.Props("FN").Field<string>("xafSuperExportFilePath");
			text = text.Trim();
			if (string.IsNullOrEmpty(text))
			{
				FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
				folderBrowserDialog.Description = "The default file path for Super Export Files is not designated in Database Options->Payroll. You can select a 'just this once' directory for saving this export file:";
				if (folderBrowserDialog.ShowDialog() == DialogResult.Cancel)
				{
					return;
				}
				text = folderBrowserDialog.SelectedPath;
			}
			if (!Directory.Exists(text))
			{
				if (MessageBox.Show("Directory " + text + " could not be found.\rDo you want M1 to create the folder?", "Directory does not exist", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				{
					MessageBox.Show("The super export has been aborted.", "File Export Aborted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				Directory.CreateDirectory(text);
			}
			else if (string.IsNullOrEmpty(text))
			{
				return;
			}
			string text2 = exportFormat.ToString() + "Export_" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".CSV";
			switch (exportFormat)
			{
			case SuperFormat.QuickSuper:
			{
				string fieldList2 = "YourFileReference,YourFileDate,ContributionPeriodStartDate,ContributionPeriodEndDate,EmployerID,PayrollID,NameTitle,FamilyName,GivenName,OtherGivenName,NameSuffix,DateOfBirth,Gender,TaxFileNumber,PhoneNumber,MobileNumber,EmailAddress,AddressLine1,AddressLine2,AddressLine3,AddressLine4,Suburb,State,PostCode,Country,EmploymentStartDate,EmploymentEndDate,EmploymentEndReason,FundID,FundName,FundEmployerID,MemberID,EmployerSuperGuaranteeAmount,EmployerAdditionalAmount,MemberSalarySacrificeAmount,MemberAdditionalAmount,OtherContributorType,OtherContributorName,YourContributionReference";
				string fieldCaptionList2 = "Your File Reference,Your File Date,Contribution Period Start Date,Contribution Period End Date,Employer ID,Payroll ID,Name Title,Family Name,Given Name,Other Given Name,Name Suffix,Date Of Birth,Gender,Tax File Number,Phone Number,Mobile Number,Email Address,Address Line 1,Address Line 2,Address Line 3,Address Line 4,Suburb,State,Post Code,Country,Employment Start Date,Employment End Date,Employment End Reason,Fund ID,Fund Name,Fund Employer ID,Member ID,Employer Super Guarantee Amount,Employer Additional Amount,Member Salary Sacrifice Amount,Member Additional Amount,Other Contributor Type,Other Contributor Name,Your Contribution Reference";
				new ExportService(database).Csv(dtDetails, Path.Combine(text, text2), ",", "", includeFieldHeadings: true, fieldList2, fieldCaptionList2);
				break;
			}
			case SuperFormat.LUCRFSuper:
			{
				string fieldList = "Employer_ABN,Employer_name,Employer_ID,USI,SMSF_ABN,SMSF_name,SMSF_electronic_service_address,SMSF_BSB,SMSF_account_number,SMSF_account_name,Employee_TFN,Member_number,Payroll_ID,Date_of_birth,Gender,Title,Last_name,First_name,Middle_names,Address_type,Address_line_1,Address_line_2,Suburb,State,Postcode,Country,Email_address,Mobile_phone_number,Home_phone_number,Member_add_change_indicator,Employment_start_date,Employment_end_date,Payroll_period_start_date,Payroll_period_end_date,SG_contribution,Award_contribution,Member_voluntary_contribution,Salary_sacrifice_contribution,Employer_voluntary_contribution,Spouse_contribution,Child_contribution,Other_contribution";
				string fieldCaptionList = "Employer ABN,Employer name,Employer ID,USI,SMSF ABN,SMSF name,SMSF electronic service address,SMSF BSB,SMSF account number,SMSF account name,Employee TFN,Member number,Payroll ID,Date of birth,Gender,Title,Last name,First name,Middle name(s),Address type,Address line 1,Address line 2,Suburb,State,Postcode,Country,Email address,Mobile phone number,Home phone number,Member add/change indicator,Employment start date,Employment end date,Payroll period start date,Payroll period end date,SG contribution,Award contribution,Member voluntary contribution,Salary sacrifice contribution,Employer voluntary contribution,Spouse contribution,Child contribution,Other contribution";
				new ExportService(database).Csv(dtDetails, Path.Combine(text, text2), ",", "", includeFieldHeadings: true, fieldList, fieldCaptionList);
				break;
			}
			default:
				new ExportService(database).Csv(dtDetails, Path.Combine(text, text2), ",", "", includeFieldHeadings: true, "");
				break;
			}
			MessageBox.Show("Super Export File '" + text2 + "' has been created in directory: " + text, "Export File Created", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		catch (Exception ex)
		{
			throw new M1Exception("Super Export File Could Not Be Created", ex.InnerException);
		}
	}

	private List<string> IsColumnEmpty(DataRow dr, string sEmployeeID, string sColumnName, int nCount, List<string> messages)
	{
		string text = "Record " + nCount + ":  Employee '" + sEmployeeID + "' ";
		string text2 = "missing ";
		if (dr[sColumnName] == null || dr[sColumnName].ToString().Trim() == string.Empty)
		{
			if (sColumnName.Contains("_SourceDate"))
			{
				sColumnName = sColumnName.Replace("_SourceDate", "");
			}
			messages.Add(text + text2 + sColumnName);
		}
		return messages;
	}

	private List<string> isAddressValid(DataRow dr, string sEmployeeID, SuperFormat SuperFileFormat, int nCount, List<string> messages)
	{
		string text = "Record " + nCount + ":  Employee '" + sEmployeeID + "' ";
		string text2 = "address is missing ";
		string text3 = string.Empty;
		if (SuperFileFormat == SuperFormat.QuickSuper)
		{
			if (dr["AddressLine1"].ToString().Trim() == string.Empty)
			{
				text3 += "AddressLine1, ";
			}
			if (dr["Suburb"].ToString().Trim() == string.Empty)
			{
				text3 += "Suburb, ";
			}
			if (dr["State"].ToString().Trim() == string.Empty)
			{
				text3 += "State, ";
			}
			if (dr["PostCode"].ToString().Trim() == string.Empty)
			{
				text3 += "PostCode, ";
			}
		}
		if (SuperFileFormat == SuperFormat.SunSuper)
		{
			if (dr["Address1"].ToString().Trim() == string.Empty)
			{
				text3 += "Address1, ";
			}
			if (dr["Suburb"].ToString().Trim() == string.Empty)
			{
				text3 += "Suburb, ";
			}
			if (dr["State"].ToString().Trim() == string.Empty)
			{
				text3 += "State, ";
			}
			if (dr["Postcode"].ToString().Trim() == string.Empty)
			{
				text3 += "PostCode, ";
			}
			if (dr["Mobile"].ToString().Trim() == string.Empty && dr["Landline"].ToString().Trim() == string.Empty)
			{
				text3 += "Mobile OR Landline Phone Number, ";
			}
			string text4 = dr["Country"].ToString().Trim().ToUpper();
			if (!(text4 == string.Empty))
			{
				switch (text4)
				{
				case "AU":
				case "AUS":
				case "AUSTRALIA":
					break;
				default:
					goto IL_0261;
				}
			}
			dr["Country"] = "Australia";
		}
		goto IL_0261;
		IL_0261:
		if (SuperFileFormat == SuperFormat.LUCRFSuper)
		{
			if (dr["State"].ToString().Trim() != "ACT" && dr["State"].ToString().Trim() != "NSW" && dr["State"].ToString().Trim() != "NT" && dr["State"].ToString().Trim() != "QLD" && dr["State"].ToString().Trim() != "SA" && dr["State"].ToString().Trim() != "TAS" && dr["State"].ToString().Trim() != "VIC" && dr["State"].ToString().Trim() != "WA")
			{
				text3 += "a Valid State Abbreviation, ";
			}
			if (dr["State"].ToString().Trim() == string.Empty)
			{
				text3 += "State, ";
			}
			if (dr["Postcode"].ToString().Trim() == string.Empty)
			{
				text3 += "Post code, ";
			}
			if (dr["Mobile_phone_number"].ToString().Trim() == string.Empty && dr["Home_phone_number"].ToString().Trim() == string.Empty)
			{
				text3 += "Mobile OR Home Phone Number, ";
			}
		}
		if (text3 != string.Empty)
		{
			text3 = text3.Remove(text3.TrimEnd().Length - 1);
			messages.Add(text + text2 + text3);
		}
		return messages;
	}

	private bool formatQuickSuperValues(DataTable dtDetails, string DateFormat)
	{
		foreach (DataRow row in dtDetails.Rows)
		{
			row["DateOfBirth"] = convertDateToStringFormat(row, "DateOfBirth_SourceDate", DateFormat);
			row["EmploymentStartDate"] = convertDateToStringFormat(row, "EmploymentStartDate_SourceDate", DateFormat);
			row["EmploymentEndDate"] = convertDateToStringFormat(row, "EmploymentEndDate_SourceDate", DateFormat);
		}
		dtDetails.Columns.Remove("DateOfBirth_SourceDate");
		dtDetails.Columns.Remove("EmploymentStartDate_SourceDate");
		dtDetails.Columns.Remove("EmploymentEndDate_SourceDate");
		return true;
	}

	private bool FormatLUCRFSuperValues(string sDatabaseID, DataTable dtDetails)
	{
		dtDetails.Columns.Remove("lnfSMSF");
		return true;
	}

	private string convertDateToStringFormat(DataRow dr, string sColumnName, string DateFormat)
	{
		if (dr[sColumnName] != null && dr[sColumnName].ToString().Trim() != string.Empty)
		{
			return Convert.ToDateTime(dr[sColumnName]).ToString(DateFormat);
		}
		return "";
	}

	public string GetCountryCode(string sCountryName)
	{
		switch (sCountryName)
		{
		case "au":
		case "aus":
		case "":
			sCountryName = "Australia";
			break;
		case "nz":
			sCountryName = "New Zealand";
			break;
		case "us":
		case "usa":
		case "u.s.a.":
		case "united states of america":
			sCountryName = "United States";
			break;
		case "uk":
			sCountryName = "United Kingdom";
			break;
		}
		if (sCountryName.Length == 2)
		{
			return sCountryName.ToUpper();
		}
		TextInfo textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
		sCountryName = textInfo.ToTitleCase(sCountryName);
		RegionInfo regionInfo = (from x in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
			select new RegionInfo(x.LCID)).FirstOrDefault((RegionInfo r) => r.EnglishName.Contains(sCountryName));
		if (regionInfo != null)
		{
			return regionInfo.TwoLetterISORegionName;
		}
		return "";
	}
}
