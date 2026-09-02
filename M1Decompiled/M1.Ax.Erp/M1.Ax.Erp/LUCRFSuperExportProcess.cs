using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using M1.Core;

namespace M1.Ax.Erp;

public class LUCRFSuperExportProcess : ProcessParameters
{
	public M1Database database { get; set; }

	public string processQuery { get; set; }

	public DateTime? dateStart { get; set; }

	public DateTime? dateEnd { get; set; }

	public string dateFormat { get; set; }

	public string employerID { get; set; }

	public bool missingRequiredSettings { get; set; }

	public LUCRFSuperExportProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		bool flag = false;
		string text = "This tool exports superannuation data for submission to a clearinghouse, in the LUCRFSuper format and using the export settings from Database Options->Payroll:";
		GridID = "M1CLEARINGHOUSEEXPORTLUCRFSuper";
		BindingSourceTable = string.Empty;
		HelpLink = "payroll_exportClearingHouseLUCRFSuper.htm";
		ContinueMessage = "This will export the super information, in the LUCRFSuper format. Are you sure you want to continue?";
		database = ServiceProvider.GetService(typeof(M1Database)) as M1Database;
		dateFormat = database.Props("FN").Field<string>("xafSuperExportDateFormat");
		employerID = database.Props("FN").Field<string>("xafSuperEmployerID");
		dateStart = database.Props("FN").Field<DateTime?>("xafSuperStartDate");
		dateEnd = database.Props("FN").Field<DateTime?>("xafSuperEndDate");
		dateFormat.ToString();
		string empty = string.Empty;
		string empty2 = string.Empty;
		string text2 = "MISSING";
		if (!dateStart.HasValue)
		{
			empty = text2;
			flag = true;
		}
		else
		{
			empty = dateStart.Value.ToString("dd/MM/yyyy");
		}
		if (!dateEnd.HasValue)
		{
			empty2 = text2;
			flag = true;
		}
		else
		{
			empty2 = dateEnd.Value.ToString("dd/MM/yyyy");
		}
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		if (flag)
		{
			text = ">>>> SOME REQUIRED SETTINGS ARE MISSING!  THESE MUST BE SET IN 'Database Options->Payroll' BEFORE USING THIS TOOL: <<<<";
			empty3 = "2000-01-01";
			empty4 = "2000-01-01";
		}
		else
		{
			empty3 = dateStart.Value.ToString("yyyy-MM-dd");
			empty4 = dateEnd.Value.ToString("yyyy-MM-dd");
		}
		Description = text + "\r       [ Date Range: '" + empty + " to " + empty2 + "' ]";
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("select  ");
		stringBuilder.AppendLine(" lnfSMSF,  ");
		stringBuilder.AppendLine(" \"Employer_ABN\" = Left(xadFederalID,11), ");
		stringBuilder.AppendLine(" \"Employer_name\" = Replace(xadName,',',''), ");
		stringBuilder.AppendLine(" \"Employer_ID\" = Replace(lnfSuperFundEmployerID,',',''), ");
		stringBuilder.AppendLine(" \"USI\" = Replace(lnfSuperFundID,',',''),  ");
		stringBuilder.AppendLine(" \"SMSF_ABN\" = Replace(lnfSMSFABN,',',''),  ");
		stringBuilder.AppendLine(" \"SMSF_name\" = Replace(lnfSMSFName,',',''),  ");
		stringBuilder.AppendLine(" \"SMSF_electronic_service_address\" = Replace(lnfSMSFServiceAddress,',',''),  ");
		stringBuilder.AppendLine(" \"SMSF_BSB\" = Replace(lnfSMSFBSB,',',''),  ");
		stringBuilder.AppendLine(" \"SMSF_account_number\" = Replace(lnfSMSFAccountNumber,',',''),  ");
		stringBuilder.AppendLine(" \"SMSF_account_name\" = Replace(lnfSMSFAccountName,',',''),  ");
		stringBuilder.AppendLine(" \"Employee_TFN\" = Replace(lmdTaxFileNumber,',',''),  ");
		stringBuilder.AppendLine(" \"Member_number\" = RTrim((Case When IsNull(pawMemberID,'') <> '' then Left(Replace(pawMemberID,',',''),16) When IsNull(paeMemberID,'') <> '' then Left(Replace(paeMemberID,',',''),16) When IsNull(pawReference,'') <> '' then Left(Replace(pawReference,',',''),16) When IsNull(paeReference,'') <> '' then Left(Replace(paeReference,',',''),16) else Left(Replace(panReference,',',''),16) end)), ");
		stringBuilder.AppendLine(" \"Payroll_ID\" = Replace(lmeEmployeeID,',',''), ");
		stringBuilder.AppendLine(" \"Date_of_birth\" = Right('0' + Convert(varchar,DatePart(dd,lmdBirthdate)),2) + '/' + Right('0' + Convert(varchar,DatePart(mm,lmdBirthdate)),2) + '/' + Convert(varchar,DatePart(yyyy,lmdBirthdate)),  ");
		stringBuilder.AppendLine(" \"Gender\" = lmdGender,  ");
		stringBuilder.AppendLine(" \"Title\" = '', ");
		stringBuilder.AppendLine(" \"Last_name\" = Replace(lmdEmployeeLastName,',',''),  ");
		stringBuilder.AppendLine(" \"First_name\" = RTrim(Replace(lmdEmployeeFirstName,',','')),  ");
		stringBuilder.AppendLine(" \"Middle_names\" = RTrim(Replace(lmdEmployeeMiddleName,',','')),  ");
		stringBuilder.AppendLine(" \"Address_type\" = (Case When upper(lmdAddressLine1) like 'PO %' or upper(lmdAddressLine1) like 'P O %' or upper(lmdAddressLine1) like 'P.%O. %' then 'POS' Else 'RES' End), ");
		stringBuilder.AppendLine(" \"Address_line_1\" = Left(Replace(lmdAddressLine1,',',''),50),  ");
		stringBuilder.AppendLine(" \"Address_line_2\" = Left(RTrim(Replace(lmdAddressLine2,',','')) + ' ' + RTrim(Replace(lmdAddressLine3,',','')),50), ");
		stringBuilder.AppendLine(" \"Suburb\" = Left(Replace(lmdCity,',',''),50), ");
		stringBuilder.AppendLine(" \"State\" = (Case When Upper(Left(lmdCountry,2)) = 'AU' or RTrim(lmdCountry) = '' Then Left(Replace(lmdState,',',''),3) Else '' End), ");
		stringBuilder.AppendLine(" \"Postcode\" = (Case When Upper(Left(lmdCountry,2)) = 'AU' or RTrim(lmdCountry) = '' Then Left(RTrim(Replace(lmdPostCode,',','')),4) Else '' End), ");
		stringBuilder.AppendLine(" \"Country\" = Lower(RTrim(Replace(lmdCountry,',',''))),  ");
		stringBuilder.AppendLine(" \"Email_address\" = (select top 1 Left(Replace(convert(varchar(8000),Case When lmeUseEmailPayslips = 2 Then lmdPersonalEmailAddress When lmeUseEmailPayslips = 1 Then lmeWorkEMailAddress When lmeUseEmail = 2 Then lmdPersonalEmailAddress Else lmeWorkEMailAddress End),',',''),100) from Employees E2 Inner Join EmployeePersonalData On E2.lmeEmployeeID = lmdEmployeeID Where E2.lmeEmployeeID = Employees.lmeEmployeeID), ");
		stringBuilder.AppendLine(" \"Mobile_phone_number\" = Left(Replace(lmdMobileNumber,',',''),15),  ");
		stringBuilder.AppendLine(" \"Home_phone_number\" = Left(Replace(lmdPhoneNumber,',',''),15),  ");
		stringBuilder.AppendFormat(" \"Member_add_change_indicator\" = (Case When lmeCreatedDate >= '{0}' and lmeCreatedDate <= '{1}' Then 'A' Else '' End), ", empty3, empty4);
		stringBuilder.AppendLine(" \"Employment_start_date\" = Right('0' + Convert(varchar,DatePart(dd,lmeHireDate)),2) + '/' + Right('0' + Convert(varchar,DatePart(mm,lmeHireDate)),2) + '/' + Convert(varchar,DatePart(yyyy,lmeHireDate)), ");
		stringBuilder.AppendLine(" \"Employment_end_date\" = Right('0' + Convert(varchar,DatePart(dd,lmeTerminationDate)),2) + '/' + Right('0' + Convert(varchar,DatePart(mm,lmeTerminationDate)),2) + '/' + Convert(varchar,DatePart(yyyy,lmeTerminationDate)), ");
		stringBuilder.AppendFormat(" \"Payroll_period_start_date\" = Right('0' + Convert(varchar,DatePart(dd,'{0}')),2) + '/' + Right('0' + Convert(varchar,DatePart(mm,'{0}')),2) + '/' + Convert(varchar,DatePart(yyyy,'{0}')), ", empty3);
		stringBuilder.AppendFormat(" \"Payroll_period_end_date\"   = Right('0' + Convert(varchar,DatePart(dd,'{0}')),2) + '/' + Right('0' + Convert(varchar,DatePart(mm,'{0}')),2) + '/' + Convert(varchar,DatePart(yyyy,'{0}')), ", empty4);
		stringBuilder.AppendLine(" \"SG_contribution\" = Round(Sum(Case When IsNull(paoSuperannuation,0) <> 0 Then panAmount-panAUSReportableAmount Else 0 End),2), ");
		stringBuilder.AppendLine(" \"Award_contribution\" = Round(Sum(Case When IsNull(paoSuperannuation,0) <> 0 Then panAUSReportableAmount Else 0 End),2), ");
		stringBuilder.AppendLine(" \"Member_voluntary_contribution\" = Round(Sum(Case When IsNull(paeSuperannuation,IsNull(padSuperannuation,0)) <> 0 and panSalarySacrifice = 0 Then panAmount Else 0 End),2), ");
		stringBuilder.AppendLine(" \"Salary_sacrifice_contribution\" = Round(Sum(Case When IsNull(paeSuperannuation,IsNull(padSuperannuation,0)) <> 0 and panSalarySacrifice <> 0 Then panAmount Else 0 End),2), ");
		stringBuilder.AppendLine(" \"Employer_voluntary_contribution\" = Round(Sum(Case When IsNull(paoSuperannuation,0) <> 0 Then panAUSReportableAmount Else 0 End),2),  ");
		stringBuilder.AppendLine(" \"Spouse_contribution\" = 0, ");
		stringBuilder.AppendLine(" \"Child_contribution\" = 0, ");
		stringBuilder.AppendLine(" \"Other_contribution\" = 0 ");
		stringBuilder.AppendLine("from PayrollLines left outer join  ");
		stringBuilder.AppendLine("PayrollSessions on panPayrollSessionID = pasPayrollSessionID left outer join  ");
		stringBuilder.AppendLine("EmployeeAllowances on panAllowanceID = pawAllowanceID and panEmployeeID = pawEmployeeID and panEmployeeAllowanceID = pawEmployeeAllowanceID left outer join  ");
		stringBuilder.AppendLine("Allowances on pawAllowanceID = paoAllowanceID and paoSuperannuation <> 0 left outer join ");
		stringBuilder.AppendLine("EmployeeDeductions on panDeductionID = paeDeductionID and panEmployeeID = paeEmployeeID and panEmployeeDeductionID = paeEmployeeDeductionID left outer join ");
		stringBuilder.AppendLine("Deductions on paeDeductionID = padDeductionID and padSuperannuation <> 0 left outer join  ");
		stringBuilder.AppendLine("SuperannuationFunds On lnfSuperannuationFundID = (Case When pawSuperannuationFundID <> '' Then pawSuperannuationFundID When paeSuperannuationFundID <> '' Then paeSuperannuationFundID When paoSuperannuationFundID <> '' Then paoSuperannuationFundID When padSuperannuationFundID <> '' Then padSuperannuationFundID Else '' End) Left Outer Join ");
		stringBuilder.AppendLine("Employees on panEmployeeID = lmeEmployeeID left outer join  ");
		stringBuilder.AppendLine("EmployeePersonalData on lmeEmployeeID = lmdEmployeeID left outer join  ");
		stringBuilder.AppendLine("PayrollHeaders on panPayrollSessionID = patPayrollSessionID And panPayrollHeaderID = patPayrollHeaderID left outer join  ");
		stringBuilder.AppendLine("DatasetProperties on 1=1 left outer join ");
		stringBuilder.AppendLine("FinancialProperties on 1=1 ");
		stringBuilder.AppendLine("Where ((panAllowanceID <> '' and paoSuperannuation <> 0) or (panDeductionID <> '' and IsNull(paeSuperannuation,padSuperannuation) <> 0) )  ");
		stringBuilder.AppendLine(" and pasPostedToGL <> 0 ");
		stringBuilder.AppendFormat(" and pasPayrollDate >= '{0}' and pasPayrollDate <= '{1}' ", empty3, empty4);
		stringBuilder.AppendLine("Group By lmeEmployeeID,  ");
		stringBuilder.AppendLine(" (Case When IsNull(pawMemberID,'') <> '' then Left(Replace(pawMemberID,',',''),16) When IsNull(paeMemberID,'') <> '' then Left(Replace(paeMemberID,',',''),16) When IsNull(pawReference,'') <> '' then Left(Replace(pawReference,',',''),16) When IsNull(paeReference,'') <> '' then Left(Replace(paeReference,',',''),16) else Left(Replace(panReference,',',''),16) end),  ");
		stringBuilder.AppendLine(" (Case When IsNull(lnfSuperFundSpinID, '') <> '' Then Replace(lnfSuperFundSpinID,',','') When IsNull(lnfSuperFundID,'') <> '' Then Replace(lnfSuperFundID,',','') Else '' End), ");
		stringBuilder.AppendLine(" Replace(lnfSuperFundName,',',''), ");
		stringBuilder.AppendLine(" Replace(lnfSuperFundEmployerID,',',''),  ");
		stringBuilder.AppendLine(" lmdEmployeeLastName, lmdEmployeeFirstName, lmdEmployeeMiddleName, lmdBirthdate, lmdGender, lmdAddressLine1, lmdAddressLine2, lmdAddressLine3, lmdCity, lmdState, lmdPostCode, lmdTaxFileNumber, ");
		stringBuilder.AppendLine(" lmeHireDate, lmeTerminationDate, lmdPhoneNumber, lmdMobileNumber, lmdCountry, xadFederalID, xadName, lnfSuperFundID, lnfSMSFABN, lnfSMSF, lnfSMSFName, lnfSMSFServiceAddress, lnfSMSFBSB, lnfSMSFAccountNumber, ");
		stringBuilder.AppendLine(" lnfSMSFAccountName, lmeCreatedDate ");
		stringBuilder.AppendLine("Order by lmeEmployeeID ");
		processQuery = stringBuilder.ToString();
	}

	protected override void OnGetData(GetDataEventArgs arg)
	{
		DataTable table = arg.Table;
		DataTable dataTable = database.GetDataTable(processQuery);
		database.GetDataTable(processQuery);
		foreach (DataRow row in dataTable.Rows)
		{
			DataRow dataRow2 = arg.BindingSource.AddNew() as DataRow;
			foreach (DataColumn column in table.Columns)
			{
				if (dataTable.Columns.Contains(column.ColumnName))
				{
					dataRow2[column] = row[column.ColumnName];
				}
			}
		}
		base.OnGetData(arg);
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		if (missingRequiredSettings)
		{
			arg.Cancel = true;
		}
		List<string> messages = arg.Messages;
		new SuperClearingHouseExportFunctions().ExportSuperClearingHouseFile(ServiceProvider, processQuery, dateStart.Value, dateEnd.Value, employerID, SuperClearingHouseExportFunctions.SuperFormat.LUCRFSuper, dateFormat, messages);
		if (messages.Count != 0)
		{
			arg.Cancel = true;
		}
	}

	private DataTable getTable(M1Database database, string query, string name)
	{
		DataTable dataTable = database.GetDataTable(query);
		dataTable.TableName = name;
		return dataTable;
	}
}
