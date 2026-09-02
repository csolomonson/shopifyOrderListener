using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using M1.Core;

namespace M1.Ax.Erp;

public class QuickSuperExportProcess : ProcessParameters
{
	public M1Database database { get; set; }

	public string processQuery { get; set; }

	public DateTime? dateStart { get; set; }

	public DateTime? dateEnd { get; set; }

	public string dateFormat { get; set; }

	public string employerID { get; set; }

	public bool missingRequiredSettings { get; set; }

	public QuickSuperExportProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		bool flag = false;
		string text = "This tool exports superannuation data for submission to a clearinghouse, in the QuickSuper format and using the export settings from Database Options->Payroll:";
		GridID = "M1CLEARINGHOUSEEXPORTQUICKSUPER";
		BindingSourceTable = string.Empty;
		HelpLink = "payroll_exportClearingHouseQuickSuper.htm";
		ContinueMessage = "This will export the super information, in the QuickSuper format. Are you sure you want to continue?";
		database = ServiceProvider.GetService(typeof(M1Database)) as M1Database;
		dateFormat = database.Props("FN").Field<string>("xafSuperExportDateFormat");
		employerID = database.Props("FN").Field<string>("xafSuperEmployerID");
		dateStart = database.Props("FN").Field<DateTime?>("xafSuperStartDate");
		dateEnd = database.Props("FN").Field<DateTime?>("xafSuperEndDate");
		string text2 = dateFormat.ToString();
		string empty = string.Empty;
		string empty2 = string.Empty;
		string text3 = "MISSING";
		if (dateFormat.ToString().Trim() == "")
		{
			text2 = text3;
			flag = true;
		}
		if (!dateStart.HasValue)
		{
			empty = text3;
			flag = true;
		}
		else
		{
			empty = dateStart.Value.ToString("dd/MM/yyyy");
		}
		if (!dateEnd.HasValue)
		{
			empty2 = text3;
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
			text = ">>>> SOME REQUIRED SETTINGS ARE MISSING!.  THESE MUST BE SET IN 'Database Options->Payroll' BEFORE USING THIS TOOL: <<<<";
			empty3 = "2000-01-01";
			empty4 = "2000-01-01";
		}
		else
		{
			empty3 = dateStart.Value.ToString("yyyy-MM-dd");
			empty4 = dateEnd.Value.ToString("yyyy-MM-dd");
		}
		Description = text + "\r       [ Date Format: '" + text2 + "'  Date Range: '" + empty + " to " + empty2 + "'  Employer ID: '" + employerID.ToString() + "' ]";
		string iD = database.ID;
		string empty5 = string.Empty;
		empty5 = ((dateEnd.HasValue && dateStart.HasValue) ? (iD + dateEnd.Value.ToString("yyMMdd") + dateStart.Value.ToString("yyMMdd")) : (iD + text3));
		DateTime date = DateTime.Today.Date;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("select  ");
		stringBuilder.AppendFormat(" 'YourFileReference' = '{0}', ", empty5);
		stringBuilder.AppendFormat(" 'YourFileDate' = '{0}', ", date.ToString(dateFormat.ToString().Trim()));
		if (dateStart.HasValue)
		{
			stringBuilder.AppendFormat(" 'ContributionPeriodStartDate' = '{0}', ", dateStart.Value.ToString(dateFormat.ToString().Trim()));
		}
		if (dateEnd.HasValue)
		{
			stringBuilder.AppendFormat(" 'ContributionPeriodEndDate' = '{0}', ", dateEnd.Value.ToString(dateFormat.ToString().Trim()));
		}
		stringBuilder.AppendFormat(" 'EmployerID' = '{0}', ", employerID);
		stringBuilder.AppendLine(" 'PayrollID' = Replace(lmeEmployeeID,',',''), ");
		stringBuilder.AppendLine(" 'NameTitle' = '', ");
		stringBuilder.AppendLine(" 'FamilyName' = Replace(lmdEmployeeLastName,',',''),  ");
		stringBuilder.AppendLine(" 'GivenName' = RTrim(Replace(lmdEmployeeFirstName,',','')),  ");
		stringBuilder.AppendLine(" 'OtherGivenName' = RTrim(Replace(lmdEmployeeMiddleName,',','')),  ");
		stringBuilder.AppendLine(" 'NameSuffix' = '',  ");
		stringBuilder.AppendLine(" 'DateOfBirth_SourceDate' = lmdBirthdate,  ");
		stringBuilder.AppendLine(" 'DateOfBirth' = '',  ");
		stringBuilder.AppendLine(" 'Gender' = lmdGender,  ");
		stringBuilder.AppendLine(" 'TaxFileNumber' = Replace(lmdTaxFileNumber,',',''),  ");
		stringBuilder.AppendLine(" 'PhoneNumber' = Left(Replace(lmdPhoneNumber,',',''),15),  ");
		stringBuilder.AppendLine(" 'MobileNumber' = Left(Replace(lmdMobileNumber,',',''),15),  ");
		stringBuilder.AppendLine(" 'EmailAddress' = (select top 1 Replace(convert(varchar(8000),Case When lmeUseEmailPayslips = 2 Then lmdPersonalEmailAddress When lmeUseEmailPayslips = 1 Then lmeWorkEMailAddress When lmeUseEmail = 2 Then lmdPersonalEmailAddress Else lmeWorkEMailAddress End),',','') from Employees E2 Inner Join EmployeePersonalData On E2.lmeEmployeeID = lmdEmployeeID Where E2.lmeEmployeeID = Employees.lmeEmployeeID), ");
		stringBuilder.AppendLine(" 'AddressLine1' = Left(Replace(lmdAddressLine1,',',''),40),  ");
		stringBuilder.AppendLine(" 'AddressLine2' = (Case When Upper(Left(lmdCountry,2)) = 'AU' or RTrim(lmdCountry) = '' Then Left(RTrim(Replace(lmdAddressLine2,',','')) + ' ' + RTrim(Replace(lmdAddressLine3,',','')),40) Else Left(RTrim(Replace(lmdAddressLine2,',','')),40) End), ");
		stringBuilder.AppendLine(" 'AddressLine3' = (Case When Upper(Left(lmdCountry,2)) = 'AU' or RTrim(lmdCountry) = '' Then '' Else Left(RTrim(Replace(lmdAddressLine3,',','')),40) End), ");
		stringBuilder.AppendLine(" 'AddressLine4' = (Case When Upper(Left(lmdCountry,2)) = 'AU' or RTrim(lmdCountry) = '' Then '' Else Left(RTrim(Replace(lmdCity,',','')) + ' ' + RTrim(Replace(lmdPostCode,',','')),40) End), ");
		stringBuilder.AppendLine(" 'Suburb' = (Case When Upper(Left(lmdCountry,2)) = 'AU' or RTrim(lmdCountry) = '' Then RTrim(Replace(lmdCity,',','')) Else '' End), ");
		stringBuilder.AppendLine(" 'State' = (Case When Upper(Left(lmdCountry,2)) = 'AU' or RTrim(lmdCountry) = '' Then RTrim(Replace(lmdState,',','')) Else '' End), ");
		stringBuilder.AppendLine(" 'PostCode' = (Case When Upper(Left(lmdCountry,2)) = 'AU' or RTrim(lmdCountry) = '' Then Left(RTrim(Replace(lmdPostCode,',','')),4) Else '' End), ");
		stringBuilder.AppendLine(" 'Country' = Lower(RTrim(Replace(lmdCountry,',',''))),  ");
		stringBuilder.AppendLine(" 'EmploymentStartDate_SourceDate' = lmeHireDate, ");
		stringBuilder.AppendLine(" 'EmploymentStartDate' = '', ");
		stringBuilder.AppendLine(" 'EmploymentEndDate_SourceDate' = lmeTerminationDate, ");
		stringBuilder.AppendLine(" 'EmploymentEndDate' = '', ");
		stringBuilder.AppendLine(" 'EmploymentEndReason' = '',  ");
		stringBuilder.AppendLine(" 'FundID' = (Case When IsNull(lnfSuperFundSpinID, '') <> '' Then Replace(lnfSuperFundSpinID,',','') When IsNull(lnfSuperFundID,'') <> '' Then Replace(lnfSuperFundID,',','') Else '' End), ");
		stringBuilder.AppendLine(" 'FundName' = Replace(lnfSuperFundName,',',''), ");
		stringBuilder.AppendLine(" 'FundEmployerID' = Replace(lnfSuperFundEmployerID,',',''), ");
		stringBuilder.AppendLine(" 'MemberID' = RTrim((Case When IsNull(pawMemberID,'') <> '' then Replace(pawMemberID,',','') When IsNull(paeMemberID,'') <> '' then Replace(paeMemberID,',','') When IsNull(pawReference,'') <> '' then Left(Replace(pawReference,',',''),20) When IsNull(paeReference,'') <> '' then Left(Replace(paeReference,',',''),20) else Left(Replace(panReference,',',''),20) end)), ");
		stringBuilder.AppendLine(" 'EmployerSuperGuaranteeAmount' = Round(Sum(Case When IsNull(paoSuperannuation,0) <> 0 Then panAmount-panAUSReportableAmount Else 0 End),2), ");
		stringBuilder.AppendLine(" 'EmployerAdditionalAmount' = Round(Sum(Case When IsNull(paoSuperannuation,0) <> 0 Then panAUSReportableAmount Else 0 End),2),  ");
		stringBuilder.AppendLine(" 'MemberSalarySacrificeAmount' = Round(Sum(Case When IsNull(paeSuperannuation,IsNull(padSuperannuation,0)) <> 0 and panSalarySacrifice <> 0 Then panAmount Else 0 End),2),   ");
		stringBuilder.AppendLine(" 'MemberAdditionalAmount' = Round(Sum(Case When IsNull(paeSuperannuation,IsNull(padSuperannuation,0)) <> 0 and panSalarySacrifice = 0 Then panAmount Else 0 End),2),  ");
		stringBuilder.AppendLine(" 'OtherContributorType' = '' , 'OtherContributorName' = '', 'YourContributionReference' = '' ");
		stringBuilder.AppendLine("from PayrollLines left outer join  ");
		stringBuilder.AppendLine("PayrollSessions on panPayrollSessionID = pasPayrollSessionID left outer join  ");
		stringBuilder.AppendLine("EmployeeAllowances on panAllowanceID = pawAllowanceID and panEmployeeID = pawEmployeeID and panEmployeeAllowanceID = pawEmployeeAllowanceID left outer join  ");
		stringBuilder.AppendLine("Allowances on pawAllowanceID = paoAllowanceID and paoSuperannuation <> 0 left outer join ");
		stringBuilder.AppendLine("EmployeeDeductions on panDeductionID = paeDeductionID and panEmployeeID = paeEmployeeID and panEmployeeDeductionID = paeEmployeeDeductionID left outer join ");
		stringBuilder.AppendLine("Deductions on paeDeductionID = padDeductionID and padSuperannuation <> 0 left outer join  ");
		stringBuilder.AppendLine("SuperannuationFunds On lnfSuperannuationFundID = (Case When pawSuperannuationFundID <> '' Then pawSuperannuationFundID When paeSuperannuationFundID <> '' Then paeSuperannuationFundID When paoSuperannuationFundID <> '' Then paoSuperannuationFundID When padSuperannuationFundID <> '' Then padSuperannuationFundID Else '' End) Left Outer Join ");
		stringBuilder.AppendLine("Employees on panEmployeeID = lmeEmployeeID left outer join  ");
		stringBuilder.AppendLine("EmployeePersonalData on lmeEmployeeID = lmdEmployeeID left outer join  ");
		stringBuilder.AppendLine("PayrollHeaders on panPayrollSessionID = patPayrollSessionID And panPayrollHeaderID = patPayrollHeaderID  ");
		stringBuilder.AppendLine("Where ((panAllowanceID <> '' and paoSuperannuation <> 0) or (panDeductionID <> '' and IsNull(paeSuperannuation,padSuperannuation) <> 0) )  ");
		stringBuilder.AppendLine(" and pasPostedToGL <> 0 ");
		stringBuilder.AppendFormat(" and pasPayrollDate >= '{0}' and pasPayrollDate <= '{1}' ", empty3, empty4);
		stringBuilder.AppendLine("Group By lmeEmployeeID,  ");
		stringBuilder.AppendLine(" (Case When IsNull(pawMemberID,'') <> '' then Replace(pawMemberID,',','') When IsNull(paeMemberID,'') <> '' then Replace(paeMemberID,',','') When IsNull(pawReference,'') <> '' then Left(Replace(pawReference,',',''),20) When IsNull(paeReference,'') <> '' then Left(Replace(paeReference,',',''),20) else Left(Replace(panReference,',',''),20) end),  ");
		stringBuilder.AppendLine(" (Case When IsNull(lnfSuperFundSpinID, '') <> '' Then Replace(lnfSuperFundSpinID,',','') When IsNull(lnfSuperFundID,'') <> '' Then Replace(lnfSuperFundID,',','') Else '' End), ");
		stringBuilder.AppendLine(" Replace(lnfSuperFundName,',',''), ");
		stringBuilder.AppendLine(" Replace(lnfSuperFundEmployerID,',',''),  ");
		stringBuilder.AppendLine(" lmdEmployeeLastName, lmdEmployeeFirstName, lmdEmployeeMiddleName, lmdBirthdate, lmdGender, lmdAddressLine1, lmdAddressLine2, lmdAddressLine3, lmdCity, lmdState, lmdPostCode, lmdTaxFileNumber, ");
		stringBuilder.AppendLine(" lmeHireDate, lmeTerminationDate, lmdPhoneNumber, lmdMobileNumber, lmdCountry ");
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
		new SuperClearingHouseExportFunctions().ExportSuperClearingHouseFile(ServiceProvider, processQuery, dateStart.Value, dateEnd.Value, employerID, SuperClearingHouseExportFunctions.SuperFormat.QuickSuper, dateFormat, messages);
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
