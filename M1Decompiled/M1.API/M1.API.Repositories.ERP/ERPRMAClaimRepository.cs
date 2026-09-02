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

public class ERPRMAClaimRepository : APIBaseRepository, IERPRMAClaimRepository, IAPIBaseRepository, IDisposable
{
	public ERPRMAClaimRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesRMAClaimExist(Guid rMAClaimId)
	{
		InitializeParameterLists();
		base.filterList.Add("rapUniqueID|C", rMAClaimId);
		base.selectList.Add("rapUniqueID");
		return Task.FromResult(GetAsObject("RMAClaims", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPRMAClaimInformationDto>> GetAllRMAClaims(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPRMAClaimInformationDto> collection = new List<ERPRMAClaimInformationDto>();
		InitializeParameterLists();
		string[] array = new string[52]
		{
			"rapActualHoursTotal", "rapArInvoiceContactID", "rapArInvoiceLocationID", "rapAuthorizationDate", "rapAuthorizationNumber", "rapAuthorizedByEmployeeID", "rapClaimDate", "rapClaimTotal", "rapClaimTotalForeign", "rapClosedDate",
			"rapClosedReasonID", "rapRmaClaimID", "rapCreatedBy", "rapCreatedDate", "rapCurrencyRateID", "rapCustomerOrganizationID", "rapDiscountAmount", "rapDiscountAmountForeign", "rapUniqueID", "rapExchangeRate",
			"rapFreightAmount", "rapFreightAmountForeign", "rapCustomRate", "rapLaborRate", "rapLaborRateForeign", "rapLaborTotal", "rapLaborTotalForeign", "rapLongDescriptionRtf", "rapLongDescriptionText", "rapPartID",
			"rapPartRevisionID", "rapPartShortDescription", "rapPartsTotal", "rapPartsTotalForeign", "rapPayTo", "rapPlantDepartmentID", "rapPlantID", "rapProcessedByEmployeeID", "rapProjectID", "rapReference",
			"rapRequestedDate", "rapResellerContactID", "rapResellerLocationID", "rapResellerOrganizationID", "rapRowVersion", "rapSerialNumberID", "rapShipContactID", "rapShipLocationID", "rapShipOrganizationID", "rapStatus",
			"rapSubcontractTotal", "rapSubcontractTotalForeign"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("RMAClaims");
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
		using (DataTable dataTable = GetAsDataTable("RMAClaims", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPRMAClaimInformationDto eRPRMAClaimInformationDto = new ERPRMAClaimInformationDto();
				eRPRMAClaimInformationDto.rapActualHoursTotal = dataTable.Rows[i].Field<decimal>("rapActualHoursTotal");
				eRPRMAClaimInformationDto.rapArInvoiceContactID = dataTable.Rows[i].Field<string>("rapArInvoiceContactID");
				eRPRMAClaimInformationDto.rapArInvoiceLocationID = dataTable.Rows[i].Field<string>("rapArInvoiceLocationID");
				eRPRMAClaimInformationDto.rapAuthorizationDate = dataTable.Rows[i].Field<DateTime?>("rapAuthorizationDate");
				eRPRMAClaimInformationDto.rapAuthorizationNumber = dataTable.Rows[i].Field<string>("rapAuthorizationNumber");
				eRPRMAClaimInformationDto.rapAuthorizedByEmployeeID = dataTable.Rows[i].Field<string>("rapAuthorizedByEmployeeID");
				eRPRMAClaimInformationDto.rapClaimDate = dataTable.Rows[i].Field<DateTime?>("rapClaimDate");
				eRPRMAClaimInformationDto.rapClaimTotal = dataTable.Rows[i].Field<decimal>("rapClaimTotal");
				eRPRMAClaimInformationDto.rapClaimTotalForeign = dataTable.Rows[i].Field<decimal>("rapClaimTotalForeign");
				eRPRMAClaimInformationDto.rapClosedDate = dataTable.Rows[i].Field<DateTime?>("rapClosedDate");
				eRPRMAClaimInformationDto.rapClosedReasonID = dataTable.Rows[i].Field<string>("rapClosedReasonID");
				eRPRMAClaimInformationDto.rapRmaClaimID = dataTable.Rows[i].Field<string>("rapRmaClaimID");
				eRPRMAClaimInformationDto.rapCreatedBy = dataTable.Rows[i].Field<string>("rapCreatedBy");
				eRPRMAClaimInformationDto.rapCreatedDate = dataTable.Rows[i].Field<DateTime?>("rapCreatedDate");
				eRPRMAClaimInformationDto.rapCurrencyRateID = dataTable.Rows[i].Field<string>("rapCurrencyRateID");
				eRPRMAClaimInformationDto.rapCustomerOrganizationID = dataTable.Rows[i].Field<string>("rapCustomerOrganizationID");
				eRPRMAClaimInformationDto.rapDiscountAmount = dataTable.Rows[i].Field<decimal>("rapDiscountAmount");
				eRPRMAClaimInformationDto.rapDiscountAmountForeign = dataTable.Rows[i].Field<decimal>("rapDiscountAmountForeign");
				eRPRMAClaimInformationDto.rapUniqueID = dataTable.Rows[i].Field<Guid>("rapUniqueID");
				eRPRMAClaimInformationDto.rapExchangeRate = dataTable.Rows[i].Field<decimal>("rapExchangeRate");
				eRPRMAClaimInformationDto.rapFreightAmount = dataTable.Rows[i].Field<decimal>("rapFreightAmount");
				eRPRMAClaimInformationDto.rapFreightAmountForeign = dataTable.Rows[i].Field<decimal>("rapFreightAmountForeign");
				eRPRMAClaimInformationDto.rapCustomRate = dataTable.Rows[i].Field<bool>("rapCustomRate");
				eRPRMAClaimInformationDto.rapLaborRate = dataTable.Rows[i].Field<decimal>("rapLaborRate");
				eRPRMAClaimInformationDto.rapLaborRateForeign = dataTable.Rows[i].Field<decimal>("rapLaborRateForeign");
				eRPRMAClaimInformationDto.rapLaborTotal = dataTable.Rows[i].Field<decimal>("rapLaborTotal");
				eRPRMAClaimInformationDto.rapLaborTotalForeign = dataTable.Rows[i].Field<decimal>("rapLaborTotalForeign");
				eRPRMAClaimInformationDto.rapLongDescriptionRtf = dataTable.Rows[i].Field<string>("rapLongDescriptionRtf");
				eRPRMAClaimInformationDto.rapLongDescriptionText = dataTable.Rows[i].Field<string>("rapLongDescriptionText");
				eRPRMAClaimInformationDto.rapPartID = dataTable.Rows[i].Field<string>("rapPartID");
				eRPRMAClaimInformationDto.rapPartRevisionID = dataTable.Rows[i].Field<string>("rapPartRevisionID");
				eRPRMAClaimInformationDto.rapPartShortDescription = dataTable.Rows[i].Field<string>("rapPartShortDescription");
				eRPRMAClaimInformationDto.rapPartsTotal = dataTable.Rows[i].Field<decimal>("rapPartsTotal");
				eRPRMAClaimInformationDto.rapPartsTotalForeign = dataTable.Rows[i].Field<decimal>("rapPartsTotalForeign");
				eRPRMAClaimInformationDto.rapPayTo = dataTable.Rows[i].Field<byte>("rapPayTo");
				eRPRMAClaimInformationDto.rapPlantDepartmentID = dataTable.Rows[i].Field<string>("rapPlantDepartmentID");
				eRPRMAClaimInformationDto.rapPlantID = dataTable.Rows[i].Field<string>("rapPlantID");
				eRPRMAClaimInformationDto.rapProcessedByEmployeeID = dataTable.Rows[i].Field<string>("rapProcessedByEmployeeID");
				eRPRMAClaimInformationDto.rapProjectID = dataTable.Rows[i].Field<string>("rapProjectID");
				eRPRMAClaimInformationDto.rapReference = dataTable.Rows[i].Field<string>("rapReference");
				eRPRMAClaimInformationDto.rapRequestedDate = dataTable.Rows[i].Field<DateTime?>("rapRequestedDate");
				eRPRMAClaimInformationDto.rapResellerContactID = dataTable.Rows[i].Field<string>("rapResellerContactID");
				eRPRMAClaimInformationDto.rapResellerLocationID = dataTable.Rows[i].Field<string>("rapResellerLocationID");
				eRPRMAClaimInformationDto.rapResellerOrganizationID = dataTable.Rows[i].Field<string>("rapResellerOrganizationID");
				eRPRMAClaimInformationDto.rapRowVersion = dataTable.Rows[i].Field<byte[]>("rapRowVersion");
				eRPRMAClaimInformationDto.rapSerialNumberID = dataTable.Rows[i].Field<string>("rapSerialNumberID");
				eRPRMAClaimInformationDto.rapShipContactID = dataTable.Rows[i].Field<string>("rapShipContactID");
				eRPRMAClaimInformationDto.rapShipLocationID = dataTable.Rows[i].Field<string>("rapShipLocationID");
				eRPRMAClaimInformationDto.rapShipOrganizationID = dataTable.Rows[i].Field<string>("rapShipOrganizationID");
				eRPRMAClaimInformationDto.rapStatus = dataTable.Rows[i].Field<string>("rapStatus");
				eRPRMAClaimInformationDto.rapSubcontractTotal = dataTable.Rows[i].Field<decimal>("rapSubcontractTotal");
				eRPRMAClaimInformationDto.rapSubcontractTotalForeign = dataTable.Rows[i].Field<decimal>("rapSubcontractTotalForeign");
				eRPRMAClaimInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPRMAClaimInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPRMAClaimInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPRMAClaimInformationDto> GetRMAClaim(Guid rMAClaimId)
	{
		ERPRMAClaimInformationDto eRPRMAClaimInformationDto = new ERPRMAClaimInformationDto();
		InitializeParameterLists();
		string[] collection = new string[52]
		{
			"rapActualHoursTotal", "rapArInvoiceContactID", "rapArInvoiceLocationID", "rapAuthorizationDate", "rapAuthorizationNumber", "rapAuthorizedByEmployeeID", "rapClaimDate", "rapClaimTotal", "rapClaimTotalForeign", "rapClosedDate",
			"rapClosedReasonID", "rapRmaClaimID", "rapCreatedBy", "rapCreatedDate", "rapCurrencyRateID", "rapCustomerOrganizationID", "rapDiscountAmount", "rapDiscountAmountForeign", "rapUniqueID", "rapExchangeRate",
			"rapFreightAmount", "rapFreightAmountForeign", "rapCustomRate", "rapLaborRate", "rapLaborRateForeign", "rapLaborTotal", "rapLaborTotalForeign", "rapLongDescriptionRtf", "rapLongDescriptionText", "rapPartID",
			"rapPartRevisionID", "rapPartShortDescription", "rapPartsTotal", "rapPartsTotalForeign", "rapPayTo", "rapPlantDepartmentID", "rapPlantID", "rapProcessedByEmployeeID", "rapProjectID", "rapReference",
			"rapRequestedDate", "rapResellerContactID", "rapResellerLocationID", "rapResellerOrganizationID", "rapRowVersion", "rapSerialNumberID", "rapShipContactID", "rapShipLocationID", "rapShipOrganizationID", "rapStatus",
			"rapSubcontractTotal", "rapSubcontractTotalForeign"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rapUniqueID|C", rMAClaimId);
		AddCustomFieldsToSelectList("RMAClaims");
		using (DataTable dataTable = GetAsDataTable("RMAClaims", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPRMAClaimInformationDto);
			}
			eRPRMAClaimInformationDto.rapActualHoursTotal = dataTable.Rows[0].Field<decimal>("rapActualHoursTotal");
			eRPRMAClaimInformationDto.rapArInvoiceContactID = dataTable.Rows[0].Field<string>("rapArInvoiceContactID");
			eRPRMAClaimInformationDto.rapArInvoiceLocationID = dataTable.Rows[0].Field<string>("rapArInvoiceLocationID");
			eRPRMAClaimInformationDto.rapAuthorizationDate = dataTable.Rows[0].Field<DateTime?>("rapAuthorizationDate");
			eRPRMAClaimInformationDto.rapAuthorizationNumber = dataTable.Rows[0].Field<string>("rapAuthorizationNumber");
			eRPRMAClaimInformationDto.rapAuthorizedByEmployeeID = dataTable.Rows[0].Field<string>("rapAuthorizedByEmployeeID");
			eRPRMAClaimInformationDto.rapClaimDate = dataTable.Rows[0].Field<DateTime?>("rapClaimDate");
			eRPRMAClaimInformationDto.rapClaimTotal = dataTable.Rows[0].Field<decimal>("rapClaimTotal");
			eRPRMAClaimInformationDto.rapClaimTotalForeign = dataTable.Rows[0].Field<decimal>("rapClaimTotalForeign");
			eRPRMAClaimInformationDto.rapClosedDate = dataTable.Rows[0].Field<DateTime?>("rapClosedDate");
			eRPRMAClaimInformationDto.rapClosedReasonID = dataTable.Rows[0].Field<string>("rapClosedReasonID");
			eRPRMAClaimInformationDto.rapRmaClaimID = dataTable.Rows[0].Field<string>("rapRmaClaimID");
			eRPRMAClaimInformationDto.rapCreatedBy = dataTable.Rows[0].Field<string>("rapCreatedBy");
			eRPRMAClaimInformationDto.rapCreatedDate = dataTable.Rows[0].Field<DateTime?>("rapCreatedDate");
			eRPRMAClaimInformationDto.rapCurrencyRateID = dataTable.Rows[0].Field<string>("rapCurrencyRateID");
			eRPRMAClaimInformationDto.rapCustomerOrganizationID = dataTable.Rows[0].Field<string>("rapCustomerOrganizationID");
			eRPRMAClaimInformationDto.rapDiscountAmount = dataTable.Rows[0].Field<decimal>("rapDiscountAmount");
			eRPRMAClaimInformationDto.rapDiscountAmountForeign = dataTable.Rows[0].Field<decimal>("rapDiscountAmountForeign");
			eRPRMAClaimInformationDto.rapUniqueID = dataTable.Rows[0].Field<Guid>("rapUniqueID");
			eRPRMAClaimInformationDto.rapExchangeRate = dataTable.Rows[0].Field<decimal>("rapExchangeRate");
			eRPRMAClaimInformationDto.rapFreightAmount = dataTable.Rows[0].Field<decimal>("rapFreightAmount");
			eRPRMAClaimInformationDto.rapFreightAmountForeign = dataTable.Rows[0].Field<decimal>("rapFreightAmountForeign");
			eRPRMAClaimInformationDto.rapCustomRate = dataTable.Rows[0].Field<bool>("rapCustomRate");
			eRPRMAClaimInformationDto.rapLaborRate = dataTable.Rows[0].Field<decimal>("rapLaborRate");
			eRPRMAClaimInformationDto.rapLaborRateForeign = dataTable.Rows[0].Field<decimal>("rapLaborRateForeign");
			eRPRMAClaimInformationDto.rapLaborTotal = dataTable.Rows[0].Field<decimal>("rapLaborTotal");
			eRPRMAClaimInformationDto.rapLaborTotalForeign = dataTable.Rows[0].Field<decimal>("rapLaborTotalForeign");
			eRPRMAClaimInformationDto.rapLongDescriptionRtf = dataTable.Rows[0].Field<string>("rapLongDescriptionRtf");
			eRPRMAClaimInformationDto.rapLongDescriptionText = dataTable.Rows[0].Field<string>("rapLongDescriptionText");
			eRPRMAClaimInformationDto.rapPartID = dataTable.Rows[0].Field<string>("rapPartID");
			eRPRMAClaimInformationDto.rapPartRevisionID = dataTable.Rows[0].Field<string>("rapPartRevisionID");
			eRPRMAClaimInformationDto.rapPartShortDescription = dataTable.Rows[0].Field<string>("rapPartShortDescription");
			eRPRMAClaimInformationDto.rapPartsTotal = dataTable.Rows[0].Field<decimal>("rapPartsTotal");
			eRPRMAClaimInformationDto.rapPartsTotalForeign = dataTable.Rows[0].Field<decimal>("rapPartsTotalForeign");
			eRPRMAClaimInformationDto.rapPayTo = dataTable.Rows[0].Field<byte>("rapPayTo");
			eRPRMAClaimInformationDto.rapPlantDepartmentID = dataTable.Rows[0].Field<string>("rapPlantDepartmentID");
			eRPRMAClaimInformationDto.rapPlantID = dataTable.Rows[0].Field<string>("rapPlantID");
			eRPRMAClaimInformationDto.rapProcessedByEmployeeID = dataTable.Rows[0].Field<string>("rapProcessedByEmployeeID");
			eRPRMAClaimInformationDto.rapProjectID = dataTable.Rows[0].Field<string>("rapProjectID");
			eRPRMAClaimInformationDto.rapReference = dataTable.Rows[0].Field<string>("rapReference");
			eRPRMAClaimInformationDto.rapRequestedDate = dataTable.Rows[0].Field<DateTime?>("rapRequestedDate");
			eRPRMAClaimInformationDto.rapResellerContactID = dataTable.Rows[0].Field<string>("rapResellerContactID");
			eRPRMAClaimInformationDto.rapResellerLocationID = dataTable.Rows[0].Field<string>("rapResellerLocationID");
			eRPRMAClaimInformationDto.rapResellerOrganizationID = dataTable.Rows[0].Field<string>("rapResellerOrganizationID");
			eRPRMAClaimInformationDto.rapRowVersion = dataTable.Rows[0].Field<byte[]>("rapRowVersion");
			eRPRMAClaimInformationDto.rapSerialNumberID = dataTable.Rows[0].Field<string>("rapSerialNumberID");
			eRPRMAClaimInformationDto.rapShipContactID = dataTable.Rows[0].Field<string>("rapShipContactID");
			eRPRMAClaimInformationDto.rapShipLocationID = dataTable.Rows[0].Field<string>("rapShipLocationID");
			eRPRMAClaimInformationDto.rapShipOrganizationID = dataTable.Rows[0].Field<string>("rapShipOrganizationID");
			eRPRMAClaimInformationDto.rapStatus = dataTable.Rows[0].Field<string>("rapStatus");
			eRPRMAClaimInformationDto.rapSubcontractTotal = dataTable.Rows[0].Field<decimal>("rapSubcontractTotal");
			eRPRMAClaimInformationDto.rapSubcontractTotalForeign = dataTable.Rows[0].Field<decimal>("rapSubcontractTotalForeign");
			eRPRMAClaimInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPRMAClaimInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPRMAClaimInformationDto);
	}

	public Task<APIValidationInfoDto> SaveRMAClaim(ERPRMAClaimDto rMAClaim)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM RMAClaims WHERE rapUniqueID = " + M1Util.ConvertToLinq(rMAClaim.rapUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rapRmaClaimID"] = rMAClaim.rapRmaClaimID.ToUpper();
				rMAClaim.rapUniqueID = ((rMAClaim.rapUniqueID == Guid.Empty) ? Guid.NewGuid() : rMAClaim.rapUniqueID);
				dataRow["rapUniqueID"] = rMAClaim.rapUniqueID;
				dataRow["rapCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rapCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The RMAClaim could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (rMAClaim.rapRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the RMAClaim is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rapRowVersion"], rMAClaim.rapRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the RMAClaim has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the RMAClaim again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rapActualHoursTotal"] = rMAClaim.rapActualHoursTotal;
			dataRow["rapArInvoiceContactID"] = rMAClaim.rapArInvoiceContactID;
			dataRow["rapArInvoiceLocationID"] = rMAClaim.rapArInvoiceLocationID;
			DataRow dataRow2 = dataRow;
			DateTime? rapAuthorizationDate = rMAClaim.rapAuthorizationDate;
			dataRow2["rapAuthorizationDate"] = (rapAuthorizationDate.HasValue ? ((object)rapAuthorizationDate.GetValueOrDefault()) : dataRow["rapAuthorizationDate"]);
			dataRow["rapAuthorizationNumber"] = rMAClaim.rapAuthorizationNumber;
			dataRow["rapAuthorizedByEmployeeID"] = rMAClaim.rapAuthorizedByEmployeeID;
			DataRow dataRow3 = dataRow;
			rapAuthorizationDate = rMAClaim.rapClaimDate;
			dataRow3["rapClaimDate"] = (rapAuthorizationDate.HasValue ? ((object)rapAuthorizationDate.GetValueOrDefault()) : dataRow["rapClaimDate"]);
			dataRow["rapClaimTotal"] = rMAClaim.rapClaimTotal;
			dataRow["rapClaimTotalForeign"] = rMAClaim.rapClaimTotalForeign;
			DataRow dataRow4 = dataRow;
			rapAuthorizationDate = rMAClaim.rapClosedDate;
			dataRow4["rapClosedDate"] = (rapAuthorizationDate.HasValue ? ((object)rapAuthorizationDate.GetValueOrDefault()) : dataRow["rapClosedDate"]);
			dataRow["rapClosedReasonID"] = rMAClaim.rapClosedReasonID;
			dataRow["rapCurrencyRateID"] = rMAClaim.rapCurrencyRateID;
			dataRow["rapCustomerOrganizationID"] = rMAClaim.rapCustomerOrganizationID;
			dataRow["rapDiscountAmount"] = rMAClaim.rapDiscountAmount;
			dataRow["rapDiscountAmountForeign"] = rMAClaim.rapDiscountAmountForeign;
			dataRow["rapExchangeRate"] = rMAClaim.rapExchangeRate;
			dataRow["rapFreightAmount"] = rMAClaim.rapFreightAmount;
			dataRow["rapFreightAmountForeign"] = rMAClaim.rapFreightAmountForeign;
			dataRow["rapCustomRate"] = rMAClaim.rapCustomRate;
			dataRow["rapLaborRate"] = rMAClaim.rapLaborRate;
			dataRow["rapLaborRateForeign"] = rMAClaim.rapLaborRateForeign;
			dataRow["rapLaborTotal"] = rMAClaim.rapLaborTotal;
			dataRow["rapLaborTotalForeign"] = rMAClaim.rapLaborTotalForeign;
			dataRow["rapLongDescriptionRtf"] = rMAClaim.rapLongDescriptionRtf ?? dataRow["rapLongDescriptionRtf"];
			dataRow["rapLongDescriptionText"] = rMAClaim.rapLongDescriptionText ?? dataRow["rapLongDescriptionText"];
			dataRow["rapPartID"] = rMAClaim.rapPartID;
			dataRow["rapPartRevisionID"] = rMAClaim.rapPartRevisionID;
			dataRow["rapPartShortDescription"] = rMAClaim.rapPartShortDescription;
			dataRow["rapPartsTotal"] = rMAClaim.rapPartsTotal;
			dataRow["rapPartsTotalForeign"] = rMAClaim.rapPartsTotalForeign;
			dataRow["rapPayTo"] = rMAClaim.rapPayTo;
			dataRow["rapPlantDepartmentID"] = rMAClaim.rapPlantDepartmentID;
			dataRow["rapPlantID"] = rMAClaim.rapPlantID;
			dataRow["rapProcessedByEmployeeID"] = rMAClaim.rapProcessedByEmployeeID;
			dataRow["rapProjectID"] = rMAClaim.rapProjectID;
			dataRow["rapReference"] = rMAClaim.rapReference;
			DataRow dataRow5 = dataRow;
			rapAuthorizationDate = rMAClaim.rapRequestedDate;
			dataRow5["rapRequestedDate"] = (rapAuthorizationDate.HasValue ? ((object)rapAuthorizationDate.GetValueOrDefault()) : dataRow["rapRequestedDate"]);
			dataRow["rapResellerContactID"] = rMAClaim.rapResellerContactID;
			dataRow["rapResellerLocationID"] = rMAClaim.rapResellerLocationID;
			dataRow["rapResellerOrganizationID"] = rMAClaim.rapResellerOrganizationID;
			dataRow["rapSerialNumberID"] = rMAClaim.rapSerialNumberID;
			dataRow["rapShipContactID"] = rMAClaim.rapShipContactID;
			dataRow["rapShipLocationID"] = rMAClaim.rapShipLocationID;
			dataRow["rapShipOrganizationID"] = rMAClaim.rapShipOrganizationID;
			dataRow["rapStatus"] = rMAClaim.rapStatus;
			dataRow["rapSubcontractTotal"] = rMAClaim.rapSubcontractTotal;
			dataRow["rapSubcontractTotalForeign"] = rMAClaim.rapSubcontractTotalForeign;
			if (rMAClaim.CustomFields != null && rMAClaim.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in rMAClaim.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the RMAClaim [{rMAClaim.rapUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the RMAClaim [{rMAClaim.rapUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
