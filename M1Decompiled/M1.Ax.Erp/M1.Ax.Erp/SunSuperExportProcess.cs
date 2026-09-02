using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using M1.Core;

namespace M1.Ax.Erp;

public class SunSuperExportProcess : ProcessParameters
{
	public M1Database database { get; set; }

	public string processQuery { get; set; }

	public DateTime? dateStart { get; set; }

	public DateTime? dateEnd { get; set; }

	public string dateFormat { get; set; }

	public string employerID { get; set; }

	public bool missingRequiredSettings { get; set; }

	public SunSuperExportProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		bool flag = false;
		string text = "This tool exports superannuation data for submission to a clearinghouse, in the SunSuper format and using the export settings from Database Options->Payroll:";
		GridID = "M1CLEARINGHOUSEEXPORTSUNSUPER";
		BindingSourceTable = string.Empty;
		HelpLink = "payroll_exportClearingHouseSunSuper.htm";
		ContinueMessage = "This will export the super information, in the SunSuper format. Are you sure you want to continue?";
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
		stringBuilder.AppendLine("select USI = IsNull(Replace(lnfSuperFundID,',',''),''),  ");
		stringBuilder.AppendLine("PayrollID = Replace(lmeEmployeeID,',',''),  ");
		stringBuilder.AppendLine("MemberID = RTrim(Case When IsNull(pawMemberID,'') <> '' then Replace(pawMemberID,',','') When IsNull(paeMemberID,'') <> '' then Replace(paeMemberID,',','') When IsNull(pawReference,'') <> '' then Replace(pawReference,',','') When IsNull(paeReference,'') <> '' then Replace(paeReference,',','') else Replace(panReference,',','') end),  ");
		stringBuilder.AppendLine("FamilyName = RTrim(Replace(lmdEmployeeLastName,',','')), ");
		stringBuilder.AppendLine("GivenNames = Left(RTrim(Replace(lmdEmployeeFirstName,',','')),30), ");
		stringBuilder.AppendLine("OtherGivenNames = Left(RTrim(Replace(lmdEmployeeMiddleName,',','')),30), ");
		stringBuilder.AppendLine("Title = '', NameSuffix = '',  ");
		stringBuilder.AppendLine("DOB = IsNull(Right('0' + Convert(varchar,DatePart(dd,lmdBirthdate)),2) + '/' + Right('0' + Convert(varchar,DatePart(mm,lmdBirthdate)),2) + '/' + Convert(varchar,DatePart(yyyy,lmdBirthdate)),''), ");
		stringBuilder.AppendLine("Gender = lmdGender,  ");
		stringBuilder.AppendLine("SuperAnnuationGuaranteeAmount = Round(Sum(Case When IsNull(paoSuperannuation,0) <> 0 Then panAmount-panAUSReportableAmount Else 0 End),2), ");
		stringBuilder.AppendLine("SalarySacrificedAmount = Round(Sum(Case When IsNull(paeSuperannuation,IsNull(padSuperannuation,0)) <> 0 and panSalarySacrifice <> 0 Then panAmount Else 0 End),2), ");
		stringBuilder.AppendLine("PersonalContributionsAmount = Round(Sum(Case When IsNull(paeSuperannuation,IsNull(padSuperannuation,0)) <> 0 and panSalarySacrifice = 0 Then panAmount Else 0 End),2), ");
		stringBuilder.AppendLine("AwardOrProductivityAmount = Round(Sum(Case When IsNull(paoSuperannuation,0) <> 0 Then panAUSReportableAmount Else 0 End),2), ");
		stringBuilder.AppendLine("VoluntaryAmount = 0, ");
		stringBuilder.AppendLine("AddressType = 'RES', ");
		stringBuilder.AppendLine("Address1 = Left(RTrim(Replace(lmdAddressLine1,',','')),30), ");
		stringBuilder.AppendLine("Address2 = Left(RTrim(Replace(lmdAddressLine2,',','')),30), ");
		stringBuilder.AppendLine("Address3 = Left(RTrim(Replace(lmdAddressLine3,',','')),30), ");
		stringBuilder.AppendLine("Address4 = '', ");
		stringBuilder.AppendLine("Suburb = Left(RTrim(Replace(lmdCity,',','')),28), ");
		stringBuilder.AppendLine("State = Replace(lmdState,',',''), ");
		stringBuilder.AppendLine("PostCode = Left(RTrim(Replace(lmdPostCode,',','')),4), ");
		stringBuilder.AppendLine("Country = Lower(RTrim(Replace(lmdCountry,',',''))), ");
		stringBuilder.AppendLine("TFN = Left(RTrim(Replace(lmdTaxFileNumber,',','')),9), ");
		stringBuilder.AppendLine("EmploymentStartDate = Right('0' + Convert(varchar,DatePart(dd,lmeHireDate)),2) + '/' + Right('0' + Convert(varchar,DatePart(mm,lmeHireDate)),2) + '/' + Convert(varchar,DatePart(yyyy,lmeHireDate)), ");
		stringBuilder.AppendLine("AnnualSalaryForInsurance = Round(0,2), ");
		stringBuilder.AppendLine("EmploymentEndDate = IsNull(Right('0' + Convert(varchar,DatePart(dd,lmeTerminationDate)),2) + '/' + Right('0' + Convert(varchar,DatePart(mm,lmeTerminationDate)),2) + '/' + Convert(varchar,DatePart(yyyy,lmeTerminationDate)),''), ");
		stringBuilder.AppendLine("EmploymentStatus = Replace(lmdEmploymentStatus,',',''), ");
		stringBuilder.AppendLine("Email = (select top 1 IsNull(Replace(convert(varchar(8000),Case When lmeUseEmailPayslips = 2 Then lmdPersonalEmailAddress When lmeUseEmailPayslips = 1 Then lmeWorkEMailAddress When lmeUseEmail = 2 Then lmdPersonalEmailAddress Else lmeWorkEMailAddress End),',',''),'') from Employees E2 Inner Join EmployeePersonalData On E2.lmeEmployeeID = lmdEmployeeID Where E2.lmeEmployeeID = Employees.lmeEmployeeID), ");
		stringBuilder.AppendLine("Mobile = Replace(lmdMobileNumber,',',''), ");
		stringBuilder.AppendLine("Landline = Replace(lmdPhoneNumber,',','') ");
		stringBuilder.AppendLine("from PayrollLines left outer join ");
		stringBuilder.AppendLine("PayrollSessions on panPayrollSessionID = pasPayrollSessionID left outer join ");
		stringBuilder.AppendLine("EmployeeAllowances on panAllowanceID = pawAllowanceID and panEmployeeID = pawEmployeeID and panEmployeeAllowanceID = pawEmployeeAllowanceID left outer join  ");
		stringBuilder.AppendLine("Allowances on pawAllowanceID = paoAllowanceID and paoSuperannuation <> 0 left outer join ");
		stringBuilder.AppendLine("EmployeeDeductions on panDeductionID = paeDeductionID and panEmployeeID = paeEmployeeID and panEmployeeDeductionID = paeEmployeeDeductionID left outer join ");
		stringBuilder.AppendLine("Deductions on paeDeductionID = padDeductionID and padSuperannuation <> 0 left outer join ");
		stringBuilder.AppendLine("SuperannuationFunds On lnfSuperannuationFundID = (Case When pawSuperannuationFundID <> '' Then pawSuperannuationFundID When paeSuperannuationFundID <> '' Then paeSuperannuationFundID When paoSuperannuationFundID <> '' Then paoSuperannuationFundID When padSuperannuationFundID <> '' Then padSuperannuationFundID Else '' End) Left Outer Join ");
		stringBuilder.AppendLine("Employees on panEmployeeID = lmeEmployeeID left outer join ");
		stringBuilder.AppendLine("EmployeePersonalData on lmeEmployeeID = lmdEmployeeID left outer join ");
		stringBuilder.AppendLine("PayrollHeaders on panPayrollSessionID = patPayrollSessionID And panPayrollHeaderID = patPayrollHeaderID ");
		stringBuilder.AppendLine("Where ((panAllowanceID <> '' and paoSuperannuation <> 0) or (panDeductionID <> '' and IsNull(paeSuperannuation,padSuperannuation) <> 0) ) ");
		stringBuilder.AppendLine(" and pasPostedToGL <> 0 ");
		stringBuilder.AppendFormat(" and pasPayrollDate >= '{0}' and pasPayrollDate <= '{1}' ", empty3, empty4);
		stringBuilder.AppendLine("Group By lmeEmployeeID, ");
		stringBuilder.AppendLine("(Case When IsNull(pawMemberID,'') <> '' then Replace(pawMemberID,',','') When IsNull(paeMemberID,'') <> '' then Replace(paeMemberID,',','') When IsNull(pawReference,'') <> '' then Replace(pawReference,',','') When IsNull(paeReference,'') <> '' then Replace(paeReference,',','') else Replace(panReference,',','') end), ");
		stringBuilder.AppendLine("Replace(lnfSuperFundID,',',''), ");
		stringBuilder.AppendLine("lmdEmployeeLastName, lmdEmployeeFirstName, lmdEmployeeMiddleName, lmdBirthdate, lmdGender, lmdAddressLine1, lmdAddressLine2, lmdAddressLine3, lmdCity, lmdState, lmdPostCode, lmdCountry, ");
		stringBuilder.AppendLine("lmdTaxFileNumber, lmeHireDate, lmdEmploymentStatus, lmeTerminationDate, lmdMobileNumber, lmdPhoneNumber ");
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
		new SuperClearingHouseExportFunctions().ExportSuperClearingHouseFile(ServiceProvider, processQuery, dateStart.Value, dateEnd.Value, employerID, SuperClearingHouseExportFunctions.SuperFormat.SunSuper, dateFormat, messages);
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
