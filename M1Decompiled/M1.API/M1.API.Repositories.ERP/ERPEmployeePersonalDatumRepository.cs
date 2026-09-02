using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Utilities;
using M1.Extensions;

namespace M1.API.Repositories.ERP;

public class ERPEmployeePersonalDatumRepository : APIBaseRepository, IERPEmployeePersonalDatumRepository, IAPIBaseRepository, IDisposable
{
	public ERPEmployeePersonalDatumRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesEmployeePersonalDatumExist(Guid employeePersonalDatumId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmdUniqueID|C", employeePersonalDatumId);
		base.selectList.Add("lmdUniqueID");
		return Task.FromResult(GetAsObject("EmployeePersonalData", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPEmployeePersonalDatumInformationDto>> GetAllEmployeePersonalData(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPEmployeePersonalDatumInformationDto> collection = new List<ERPEmployeePersonalDatumInformationDto>();
		InitializeParameterLists();
		string[] array = new string[50]
		{
			"lmdAddressLine1", "lmdAddressLine2", "lmdAddressLine3", "lmdBasisOfPayment", "lmdBirthDate", "lmdCity", "lmdContact1HomePhoneNumber", "lmdContact1MobilePhoneNumber", "lmdContact1Name", "lmdContact1Relationship",
			"lmdContact1WorkPhoneNumber", "lmdContact2HomePhoneNumber", "lmdContact2MobilePhoneNumber", "lmdContact2Name", "lmdContact2Relationship", "lmdContact2WorkPhoneNumber", "lmdCountry", "lmdCreatedBy", "lmdCreatedDate", "lmdEmployeeFirstName",
			"lmdEmployeeID", "lmdEmployeeLastName", "lmdEmployeeMiddleName", "lmdEmploymentDeclarationDate", "lmdEmploymentStatus", "lmdUniqueID", "lmdFaxNumber", "lmdGender", "lmdHomeCountry", "lmdEmploymentDeclarationOnFile",
			"lmdPayrollEmployee", "lmdStdntFinSupplSchemeLoan", "lmdStudyTrainLoanRepayment", "lmdTaxFreeThresholdClaimed", "lmdWorkingHolidayMaker", "lmdLaborRate", "lmdMaritalStatus", "lmdMobileNumber", "lmdNZTaxCode", "lmdPAYGSummaryType",
			"lmdPayrollDefinitionID", "lmdPayrollExportEmployeeID", "lmdPersonalEmailAddress", "lmdPhoneNumber", "lmdPostCode", "lmdResidencyStatus", "lmdRowVersion", "lmdState", "lmdStateAus", "lmdTaxFileNumber"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("EmployeePersonalData");
		List<string> list = new List<string>();
		string[] fields = ((base.selectList.Count != array.Count()) ? base.selectList.ToArray() : array);
		if (orderBy != null && orderBy.Length > 0)
		{
			ParseAndAddOrderByFields(orderBy, list, fields);
		}
		if (list.Count == 0)
		{
			list = new List<string> { "1" };
		}
		if (filter != null && filter.Length != 0)
		{
			ParseAndAddFilter(filter, base.filterList, fields);
		}
		using (DataTable dataTable = GetAsDataTable("EmployeePersonalData", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPEmployeePersonalDatumInformationDto eRPEmployeePersonalDatumInformationDto = new ERPEmployeePersonalDatumInformationDto();
				eRPEmployeePersonalDatumInformationDto.lmdAddressLine1 = dataTable.Rows[i].Field<string>("lmdAddressLine1");
				eRPEmployeePersonalDatumInformationDto.lmdAddressLine2 = dataTable.Rows[i].Field<string>("lmdAddressLine2");
				eRPEmployeePersonalDatumInformationDto.lmdAddressLine3 = dataTable.Rows[i].Field<string>("lmdAddressLine3");
				eRPEmployeePersonalDatumInformationDto.lmdBasisOfPayment = dataTable.Rows[i].Field<string>("lmdBasisOfPayment");
				eRPEmployeePersonalDatumInformationDto.lmdBirthDate = dataTable.Rows[i].Field<DateTime?>("lmdBirthDate");
				eRPEmployeePersonalDatumInformationDto.lmdCity = dataTable.Rows[i].Field<string>("lmdCity");
				eRPEmployeePersonalDatumInformationDto.lmdContact1HomePhoneNumber = dataTable.Rows[i].Field<string>("lmdContact1HomePhoneNumber");
				eRPEmployeePersonalDatumInformationDto.lmdContact1MobilePhoneNumber = dataTable.Rows[i].Field<string>("lmdContact1MobilePhoneNumber");
				eRPEmployeePersonalDatumInformationDto.lmdContact1Name = dataTable.Rows[i].Field<string>("lmdContact1Name");
				eRPEmployeePersonalDatumInformationDto.lmdContact1Relationship = dataTable.Rows[i].Field<string>("lmdContact1Relationship");
				eRPEmployeePersonalDatumInformationDto.lmdContact1WorkPhoneNumber = dataTable.Rows[i].Field<string>("lmdContact1WorkPhoneNumber");
				eRPEmployeePersonalDatumInformationDto.lmdContact2HomePhoneNumber = dataTable.Rows[i].Field<string>("lmdContact2HomePhoneNumber");
				eRPEmployeePersonalDatumInformationDto.lmdContact2MobilePhoneNumber = dataTable.Rows[i].Field<string>("lmdContact2MobilePhoneNumber");
				eRPEmployeePersonalDatumInformationDto.lmdContact2Name = dataTable.Rows[i].Field<string>("lmdContact2Name");
				eRPEmployeePersonalDatumInformationDto.lmdContact2Relationship = dataTable.Rows[i].Field<string>("lmdContact2Relationship");
				eRPEmployeePersonalDatumInformationDto.lmdContact2WorkPhoneNumber = dataTable.Rows[i].Field<string>("lmdContact2WorkPhoneNumber");
				eRPEmployeePersonalDatumInformationDto.lmdCountry = dataTable.Rows[i].Field<string>("lmdCountry");
				eRPEmployeePersonalDatumInformationDto.lmdCreatedBy = dataTable.Rows[i].Field<string>("lmdCreatedBy");
				eRPEmployeePersonalDatumInformationDto.lmdCreatedDate = dataTable.Rows[i].Field<DateTime?>("lmdCreatedDate");
				eRPEmployeePersonalDatumInformationDto.lmdEmployeeFirstName = dataTable.Rows[i].Field<string>("lmdEmployeeFirstName");
				eRPEmployeePersonalDatumInformationDto.lmdEmployeeID = dataTable.Rows[i].Field<string>("lmdEmployeeID");
				eRPEmployeePersonalDatumInformationDto.lmdEmployeeLastName = dataTable.Rows[i].Field<string>("lmdEmployeeLastName");
				eRPEmployeePersonalDatumInformationDto.lmdEmployeeMiddleName = dataTable.Rows[i].Field<string>("lmdEmployeeMiddleName");
				eRPEmployeePersonalDatumInformationDto.lmdEmploymentDeclarationDate = dataTable.Rows[i].Field<DateTime?>("lmdEmploymentDeclarationDate");
				eRPEmployeePersonalDatumInformationDto.lmdEmploymentStatus = dataTable.Rows[i].Field<string>("lmdEmploymentStatus");
				eRPEmployeePersonalDatumInformationDto.lmdUniqueID = dataTable.Rows[i].Field<Guid>("lmdUniqueID");
				eRPEmployeePersonalDatumInformationDto.lmdFaxNumber = dataTable.Rows[i].Field<string>("lmdFaxNumber");
				eRPEmployeePersonalDatumInformationDto.lmdGender = dataTable.Rows[i].Field<string>("lmdGender");
				eRPEmployeePersonalDatumInformationDto.lmdHomeCountry = dataTable.Rows[i].Field<string>("lmdHomeCountry");
				eRPEmployeePersonalDatumInformationDto.lmdEmploymentDeclarationOnFile = dataTable.Rows[i].Field<bool>("lmdEmploymentDeclarationOnFile");
				eRPEmployeePersonalDatumInformationDto.lmdPayrollEmployee = dataTable.Rows[i].Field<bool>("lmdPayrollEmployee");
				eRPEmployeePersonalDatumInformationDto.lmdStdntFinSupplSchemeLoan = dataTable.Rows[i].Field<bool>("lmdStdntFinSupplSchemeLoan");
				eRPEmployeePersonalDatumInformationDto.lmdStudyTrainLoanRepayment = dataTable.Rows[i].Field<bool>("lmdStudyTrainLoanRepayment");
				eRPEmployeePersonalDatumInformationDto.lmdTaxFreeThresholdClaimed = dataTable.Rows[i].Field<bool>("lmdTaxFreeThresholdClaimed");
				eRPEmployeePersonalDatumInformationDto.lmdWorkingHolidayMaker = dataTable.Rows[i].Field<bool>("lmdWorkingHolidayMaker");
				eRPEmployeePersonalDatumInformationDto.lmdLaborRate = dataTable.Rows[i].Field<decimal>("lmdLaborRate");
				eRPEmployeePersonalDatumInformationDto.lmdMaritalStatus = dataTable.Rows[i].Field<string>("lmdMaritalStatus");
				eRPEmployeePersonalDatumInformationDto.lmdMobileNumber = dataTable.Rows[i].Field<string>("lmdMobileNumber");
				eRPEmployeePersonalDatumInformationDto.lmdNZTaxCode = dataTable.Rows[i].Field<string>("lmdNZTaxCode");
				eRPEmployeePersonalDatumInformationDto.lmdPAYGSummaryType = dataTable.Rows[i].Field<string>("lmdPAYGSummaryType");
				eRPEmployeePersonalDatumInformationDto.lmdPayrollDefinitionID = dataTable.Rows[i].Field<string>("lmdPayrollDefinitionID");
				eRPEmployeePersonalDatumInformationDto.lmdPayrollExportEmployeeID = dataTable.Rows[i].Field<string>("lmdPayrollExportEmployeeID");
				eRPEmployeePersonalDatumInformationDto.lmdPersonalEmailAddress = dataTable.Rows[i].Field<string>("lmdPersonalEmailAddress");
				eRPEmployeePersonalDatumInformationDto.lmdPhoneNumber = dataTable.Rows[i].Field<string>("lmdPhoneNumber");
				eRPEmployeePersonalDatumInformationDto.lmdPostCode = dataTable.Rows[i].Field<string>("lmdPostCode");
				eRPEmployeePersonalDatumInformationDto.lmdResidencyStatus = dataTable.Rows[i].Field<string>("lmdResidencyStatus");
				eRPEmployeePersonalDatumInformationDto.lmdRowVersion = dataTable.Rows[i].Field<byte[]>("lmdRowVersion");
				eRPEmployeePersonalDatumInformationDto.lmdState = dataTable.Rows[i].Field<string>("lmdState");
				eRPEmployeePersonalDatumInformationDto.lmdStateAus = dataTable.Rows[i].Field<string>("lmdStateAus");
				eRPEmployeePersonalDatumInformationDto.lmdTaxFileNumber = dataTable.Rows[i].Field<string>("lmdTaxFileNumber");
				eRPEmployeePersonalDatumInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPEmployeePersonalDatumInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPEmployeePersonalDatumInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPEmployeePersonalDatumInformationDto> GetEmployeePersonalDatum(Guid employeePersonalDatumId)
	{
		ERPEmployeePersonalDatumInformationDto eRPEmployeePersonalDatumInformationDto = new ERPEmployeePersonalDatumInformationDto();
		InitializeParameterLists();
		string[] collection = new string[50]
		{
			"lmdAddressLine1", "lmdAddressLine2", "lmdAddressLine3", "lmdBasisOfPayment", "lmdBirthDate", "lmdCity", "lmdContact1HomePhoneNumber", "lmdContact1MobilePhoneNumber", "lmdContact1Name", "lmdContact1Relationship",
			"lmdContact1WorkPhoneNumber", "lmdContact2HomePhoneNumber", "lmdContact2MobilePhoneNumber", "lmdContact2Name", "lmdContact2Relationship", "lmdContact2WorkPhoneNumber", "lmdCountry", "lmdCreatedBy", "lmdCreatedDate", "lmdEmployeeFirstName",
			"lmdEmployeeID", "lmdEmployeeLastName", "lmdEmployeeMiddleName", "lmdEmploymentDeclarationDate", "lmdEmploymentStatus", "lmdUniqueID", "lmdFaxNumber", "lmdGender", "lmdHomeCountry", "lmdEmploymentDeclarationOnFile",
			"lmdPayrollEmployee", "lmdStdntFinSupplSchemeLoan", "lmdStudyTrainLoanRepayment", "lmdTaxFreeThresholdClaimed", "lmdWorkingHolidayMaker", "lmdLaborRate", "lmdMaritalStatus", "lmdMobileNumber", "lmdNZTaxCode", "lmdPAYGSummaryType",
			"lmdPayrollDefinitionID", "lmdPayrollExportEmployeeID", "lmdPersonalEmailAddress", "lmdPhoneNumber", "lmdPostCode", "lmdResidencyStatus", "lmdRowVersion", "lmdState", "lmdStateAus", "lmdTaxFileNumber"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("lmdUniqueID|C", employeePersonalDatumId);
		AddCustomFieldsToSelectList("EmployeePersonalData");
		using (DataTable dataTable = GetAsDataTable("EmployeePersonalData", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPEmployeePersonalDatumInformationDto);
			}
			eRPEmployeePersonalDatumInformationDto.lmdAddressLine1 = dataTable.Rows[0].Field<string>("lmdAddressLine1");
			eRPEmployeePersonalDatumInformationDto.lmdAddressLine2 = dataTable.Rows[0].Field<string>("lmdAddressLine2");
			eRPEmployeePersonalDatumInformationDto.lmdAddressLine3 = dataTable.Rows[0].Field<string>("lmdAddressLine3");
			eRPEmployeePersonalDatumInformationDto.lmdBasisOfPayment = dataTable.Rows[0].Field<string>("lmdBasisOfPayment");
			eRPEmployeePersonalDatumInformationDto.lmdBirthDate = dataTable.Rows[0].Field<DateTime?>("lmdBirthDate");
			eRPEmployeePersonalDatumInformationDto.lmdCity = dataTable.Rows[0].Field<string>("lmdCity");
			eRPEmployeePersonalDatumInformationDto.lmdContact1HomePhoneNumber = dataTable.Rows[0].Field<string>("lmdContact1HomePhoneNumber");
			eRPEmployeePersonalDatumInformationDto.lmdContact1MobilePhoneNumber = dataTable.Rows[0].Field<string>("lmdContact1MobilePhoneNumber");
			eRPEmployeePersonalDatumInformationDto.lmdContact1Name = dataTable.Rows[0].Field<string>("lmdContact1Name");
			eRPEmployeePersonalDatumInformationDto.lmdContact1Relationship = dataTable.Rows[0].Field<string>("lmdContact1Relationship");
			eRPEmployeePersonalDatumInformationDto.lmdContact1WorkPhoneNumber = dataTable.Rows[0].Field<string>("lmdContact1WorkPhoneNumber");
			eRPEmployeePersonalDatumInformationDto.lmdContact2HomePhoneNumber = dataTable.Rows[0].Field<string>("lmdContact2HomePhoneNumber");
			eRPEmployeePersonalDatumInformationDto.lmdContact2MobilePhoneNumber = dataTable.Rows[0].Field<string>("lmdContact2MobilePhoneNumber");
			eRPEmployeePersonalDatumInformationDto.lmdContact2Name = dataTable.Rows[0].Field<string>("lmdContact2Name");
			eRPEmployeePersonalDatumInformationDto.lmdContact2Relationship = dataTable.Rows[0].Field<string>("lmdContact2Relationship");
			eRPEmployeePersonalDatumInformationDto.lmdContact2WorkPhoneNumber = dataTable.Rows[0].Field<string>("lmdContact2WorkPhoneNumber");
			eRPEmployeePersonalDatumInformationDto.lmdCountry = dataTable.Rows[0].Field<string>("lmdCountry");
			eRPEmployeePersonalDatumInformationDto.lmdCreatedBy = dataTable.Rows[0].Field<string>("lmdCreatedBy");
			eRPEmployeePersonalDatumInformationDto.lmdCreatedDate = dataTable.Rows[0].Field<DateTime?>("lmdCreatedDate");
			eRPEmployeePersonalDatumInformationDto.lmdEmployeeFirstName = dataTable.Rows[0].Field<string>("lmdEmployeeFirstName");
			eRPEmployeePersonalDatumInformationDto.lmdEmployeeID = dataTable.Rows[0].Field<string>("lmdEmployeeID");
			eRPEmployeePersonalDatumInformationDto.lmdEmployeeLastName = dataTable.Rows[0].Field<string>("lmdEmployeeLastName");
			eRPEmployeePersonalDatumInformationDto.lmdEmployeeMiddleName = dataTable.Rows[0].Field<string>("lmdEmployeeMiddleName");
			eRPEmployeePersonalDatumInformationDto.lmdEmploymentDeclarationDate = dataTable.Rows[0].Field<DateTime?>("lmdEmploymentDeclarationDate");
			eRPEmployeePersonalDatumInformationDto.lmdEmploymentStatus = dataTable.Rows[0].Field<string>("lmdEmploymentStatus");
			eRPEmployeePersonalDatumInformationDto.lmdUniqueID = dataTable.Rows[0].Field<Guid>("lmdUniqueID");
			eRPEmployeePersonalDatumInformationDto.lmdFaxNumber = dataTable.Rows[0].Field<string>("lmdFaxNumber");
			eRPEmployeePersonalDatumInformationDto.lmdGender = dataTable.Rows[0].Field<string>("lmdGender");
			eRPEmployeePersonalDatumInformationDto.lmdHomeCountry = dataTable.Rows[0].Field<string>("lmdHomeCountry");
			eRPEmployeePersonalDatumInformationDto.lmdEmploymentDeclarationOnFile = dataTable.Rows[0].Field<bool>("lmdEmploymentDeclarationOnFile");
			eRPEmployeePersonalDatumInformationDto.lmdPayrollEmployee = dataTable.Rows[0].Field<bool>("lmdPayrollEmployee");
			eRPEmployeePersonalDatumInformationDto.lmdStdntFinSupplSchemeLoan = dataTable.Rows[0].Field<bool>("lmdStdntFinSupplSchemeLoan");
			eRPEmployeePersonalDatumInformationDto.lmdStudyTrainLoanRepayment = dataTable.Rows[0].Field<bool>("lmdStudyTrainLoanRepayment");
			eRPEmployeePersonalDatumInformationDto.lmdTaxFreeThresholdClaimed = dataTable.Rows[0].Field<bool>("lmdTaxFreeThresholdClaimed");
			eRPEmployeePersonalDatumInformationDto.lmdWorkingHolidayMaker = dataTable.Rows[0].Field<bool>("lmdWorkingHolidayMaker");
			eRPEmployeePersonalDatumInformationDto.lmdLaborRate = dataTable.Rows[0].Field<decimal>("lmdLaborRate");
			eRPEmployeePersonalDatumInformationDto.lmdMaritalStatus = dataTable.Rows[0].Field<string>("lmdMaritalStatus");
			eRPEmployeePersonalDatumInformationDto.lmdMobileNumber = dataTable.Rows[0].Field<string>("lmdMobileNumber");
			eRPEmployeePersonalDatumInformationDto.lmdNZTaxCode = dataTable.Rows[0].Field<string>("lmdNZTaxCode");
			eRPEmployeePersonalDatumInformationDto.lmdPAYGSummaryType = dataTable.Rows[0].Field<string>("lmdPAYGSummaryType");
			eRPEmployeePersonalDatumInformationDto.lmdPayrollDefinitionID = dataTable.Rows[0].Field<string>("lmdPayrollDefinitionID");
			eRPEmployeePersonalDatumInformationDto.lmdPayrollExportEmployeeID = dataTable.Rows[0].Field<string>("lmdPayrollExportEmployeeID");
			eRPEmployeePersonalDatumInformationDto.lmdPersonalEmailAddress = dataTable.Rows[0].Field<string>("lmdPersonalEmailAddress");
			eRPEmployeePersonalDatumInformationDto.lmdPhoneNumber = dataTable.Rows[0].Field<string>("lmdPhoneNumber");
			eRPEmployeePersonalDatumInformationDto.lmdPostCode = dataTable.Rows[0].Field<string>("lmdPostCode");
			eRPEmployeePersonalDatumInformationDto.lmdResidencyStatus = dataTable.Rows[0].Field<string>("lmdResidencyStatus");
			eRPEmployeePersonalDatumInformationDto.lmdRowVersion = dataTable.Rows[0].Field<byte[]>("lmdRowVersion");
			eRPEmployeePersonalDatumInformationDto.lmdState = dataTable.Rows[0].Field<string>("lmdState");
			eRPEmployeePersonalDatumInformationDto.lmdStateAus = dataTable.Rows[0].Field<string>("lmdStateAus");
			eRPEmployeePersonalDatumInformationDto.lmdTaxFileNumber = dataTable.Rows[0].Field<string>("lmdTaxFileNumber");
			eRPEmployeePersonalDatumInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPEmployeePersonalDatumInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPEmployeePersonalDatumInformationDto);
	}

	public Task<APIValidationInfoDto> SaveEmployeePersonalDatum(ERPEmployeePersonalDatumDto employeePersonalDatum)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM EmployeePersonalData WHERE lmdUniqueID = " + M1Util.ConvertToLinq(employeePersonalDatum.lmdUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["lmdEmployeeID"] = employeePersonalDatum.lmdEmployeeID.ToUpper();
				employeePersonalDatum.lmdUniqueID = ((employeePersonalDatum.lmdUniqueID == Guid.Empty) ? Guid.NewGuid() : employeePersonalDatum.lmdUniqueID);
				dataRow["lmdUniqueID"] = employeePersonalDatum.lmdUniqueID;
				dataRow["lmdCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["lmdCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The EmployeePersonalDatum could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (employeePersonalDatum.lmdRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the EmployeePersonalDatum is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["lmdRowVersion"], employeePersonalDatum.lmdRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the EmployeePersonalDatum has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the EmployeePersonalDatum again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["lmdAddressLine1"] = employeePersonalDatum.lmdAddressLine1;
			dataRow["lmdAddressLine2"] = employeePersonalDatum.lmdAddressLine2;
			dataRow["lmdAddressLine3"] = employeePersonalDatum.lmdAddressLine3;
			dataRow["lmdBasisOfPayment"] = employeePersonalDatum.lmdBasisOfPayment;
			DataRow dataRow2 = dataRow;
			DateTime? lmdBirthDate = employeePersonalDatum.lmdBirthDate;
			dataRow2["lmdBirthDate"] = (lmdBirthDate.HasValue ? ((object)lmdBirthDate.GetValueOrDefault()) : dataRow["lmdBirthDate"]);
			dataRow["lmdCity"] = employeePersonalDatum.lmdCity;
			dataRow["lmdContact1HomePhoneNumber"] = employeePersonalDatum.lmdContact1HomePhoneNumber;
			dataRow["lmdContact1MobilePhoneNumber"] = employeePersonalDatum.lmdContact1MobilePhoneNumber;
			dataRow["lmdContact1Name"] = employeePersonalDatum.lmdContact1Name;
			dataRow["lmdContact1Relationship"] = employeePersonalDatum.lmdContact1Relationship;
			dataRow["lmdContact1WorkPhoneNumber"] = employeePersonalDatum.lmdContact1WorkPhoneNumber;
			dataRow["lmdContact2HomePhoneNumber"] = employeePersonalDatum.lmdContact2HomePhoneNumber;
			dataRow["lmdContact2MobilePhoneNumber"] = employeePersonalDatum.lmdContact2MobilePhoneNumber;
			dataRow["lmdContact2Name"] = employeePersonalDatum.lmdContact2Name;
			dataRow["lmdContact2Relationship"] = employeePersonalDatum.lmdContact2Relationship;
			dataRow["lmdContact2WorkPhoneNumber"] = employeePersonalDatum.lmdContact2WorkPhoneNumber;
			dataRow["lmdCountry"] = employeePersonalDatum.lmdCountry;
			dataRow["lmdEmployeeFirstName"] = employeePersonalDatum.lmdEmployeeFirstName;
			dataRow["lmdEmployeeLastName"] = employeePersonalDatum.lmdEmployeeLastName;
			dataRow["lmdEmployeeMiddleName"] = employeePersonalDatum.lmdEmployeeMiddleName;
			DataRow dataRow3 = dataRow;
			lmdBirthDate = employeePersonalDatum.lmdEmploymentDeclarationDate;
			dataRow3["lmdEmploymentDeclarationDate"] = (lmdBirthDate.HasValue ? ((object)lmdBirthDate.GetValueOrDefault()) : dataRow["lmdEmploymentDeclarationDate"]);
			dataRow["lmdEmploymentStatus"] = employeePersonalDatum.lmdEmploymentStatus;
			dataRow["lmdFaxNumber"] = employeePersonalDatum.lmdFaxNumber;
			dataRow["lmdGender"] = employeePersonalDatum.lmdGender;
			dataRow["lmdHomeCountry"] = employeePersonalDatum.lmdHomeCountry;
			dataRow["lmdEmploymentDeclarationOnFile"] = employeePersonalDatum.lmdEmploymentDeclarationOnFile;
			dataRow["lmdPayrollEmployee"] = employeePersonalDatum.lmdPayrollEmployee;
			dataRow["lmdStdntFinSupplSchemeLoan"] = employeePersonalDatum.lmdStdntFinSupplSchemeLoan;
			dataRow["lmdStudyTrainLoanRepayment"] = employeePersonalDatum.lmdStudyTrainLoanRepayment;
			dataRow["lmdTaxFreeThresholdClaimed"] = employeePersonalDatum.lmdTaxFreeThresholdClaimed;
			dataRow["lmdWorkingHolidayMaker"] = employeePersonalDatum.lmdWorkingHolidayMaker;
			dataRow["lmdLaborRate"] = employeePersonalDatum.lmdLaborRate;
			dataRow["lmdMaritalStatus"] = employeePersonalDatum.lmdMaritalStatus;
			dataRow["lmdMobileNumber"] = employeePersonalDatum.lmdMobileNumber;
			dataRow["lmdNZTaxCode"] = employeePersonalDatum.lmdNZTaxCode;
			dataRow["lmdPAYGSummaryType"] = employeePersonalDatum.lmdPAYGSummaryType;
			dataRow["lmdPayrollDefinitionID"] = employeePersonalDatum.lmdPayrollDefinitionID;
			dataRow["lmdPayrollExportEmployeeID"] = employeePersonalDatum.lmdPayrollExportEmployeeID;
			dataRow["lmdPersonalEmailAddress"] = employeePersonalDatum.lmdPersonalEmailAddress ?? dataRow["lmdPersonalEmailAddress"];
			dataRow["lmdPhoneNumber"] = employeePersonalDatum.lmdPhoneNumber;
			dataRow["lmdPostCode"] = employeePersonalDatum.lmdPostCode;
			dataRow["lmdResidencyStatus"] = employeePersonalDatum.lmdResidencyStatus;
			dataRow["lmdState"] = employeePersonalDatum.lmdState;
			dataRow["lmdStateAus"] = employeePersonalDatum.lmdStateAus;
			dataRow["lmdTaxFileNumber"] = employeePersonalDatum.lmdTaxFileNumber;
			if (employeePersonalDatum.CustomFields != null && employeePersonalDatum.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in employeePersonalDatum.CustomFields)
				{
					if (dataTable.Columns.Contains(customField.Key))
					{
						dataRow[customField.Key] = customField.Value;
					}
				}
			}
			dataRow.EndEdit();
			if (flag)
			{
				dataTable.Rows.Add(dataRow);
			}
			if (base.M1database.UpdateData(dataTable, adapter))
			{
				if (flag)
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Created;
				}
				else
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.OK;
				}
			}
			else
			{
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.InternalServerError;
			}
		}
		catch (SqlException ex)
		{
			SqlErrorResult httpStatusCodeForSqlException = SqlExceptionMapper.GetHttpStatusCodeForSqlException(ex);
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the EmployeePersonalDatum [{employeePersonalDatum.lmdUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the EmployeePersonalDatum [{employeePersonalDatum.lmdUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
