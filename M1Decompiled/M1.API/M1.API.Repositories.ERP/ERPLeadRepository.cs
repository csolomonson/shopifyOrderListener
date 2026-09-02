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

public class ERPLeadRepository : APIBaseRepository, IERPLeadRepository, IAPIBaseRepository, IDisposable
{
	public ERPLeadRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesLeadExist(Guid leadId)
	{
		InitializeParameterLists();
		base.filterList.Add("lopUniqueID|C", leadId);
		base.selectList.Add("lopUniqueID");
		return Task.FromResult(GetAsObject("Leads", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPLeadInformationDto>> GetAllLeads(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPLeadInformationDto> collection = new List<ERPLeadInformationDto>();
		InitializeParameterLists();
		string[] array = new string[40]
		{
			"lopClosedByEmployeeID", "lopClosedDate", "lopClosedReasonID", "lopLeadID", "lopContactID", "lopCreatedBy", "lopCreatedDate", "lopCurrencyRateID", "lopCustomerOrganizationID", "lopUniqueID",
			"lopExchangeRate", "lopExpectedCloseDate", "lopExpirationDate", "lopCreatedFromMobile", "lopCustomRate", "lopLeadDate", "lopLeadTotal", "lopLeadTotalForeign", "lopLocationID", "lopLongDescriptionRtf",
			"lopLongDescriptionText", "lopMarketingProgramID", "lopMilestoneDate", "lopMilestoneID", "lopPlantDepartmentID", "lopPlantID", "lopProjectAreaID", "lopProjectID", "lopQuoteContactID", "lopQuoteLocationID",
			"lopQuoterEmployeeID", "lopReferredBy", "lopResponseMethodID", "lopRowVersion", "lopShipContactID", "lopShipLocationID", "lopShipOrganizationID", "lopShortDescription", "lopSplitPercentTotal", "lopStatus"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Leads");
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
		using (DataTable dataTable = GetAsDataTable("Leads", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPLeadInformationDto eRPLeadInformationDto = new ERPLeadInformationDto();
				eRPLeadInformationDto.lopClosedByEmployeeID = dataTable.Rows[i].Field<string>("lopClosedByEmployeeID");
				eRPLeadInformationDto.lopClosedDate = dataTable.Rows[i].Field<DateTime?>("lopClosedDate");
				eRPLeadInformationDto.lopClosedReasonID = dataTable.Rows[i].Field<string>("lopClosedReasonID");
				eRPLeadInformationDto.lopLeadID = dataTable.Rows[i].Field<string>("lopLeadID");
				eRPLeadInformationDto.lopContactID = dataTable.Rows[i].Field<string>("lopContactID");
				eRPLeadInformationDto.lopCreatedBy = dataTable.Rows[i].Field<string>("lopCreatedBy");
				eRPLeadInformationDto.lopCreatedDate = dataTable.Rows[i].Field<DateTime?>("lopCreatedDate");
				eRPLeadInformationDto.lopCurrencyRateID = dataTable.Rows[i].Field<string>("lopCurrencyRateID");
				eRPLeadInformationDto.lopCustomerOrganizationID = dataTable.Rows[i].Field<string>("lopCustomerOrganizationID");
				eRPLeadInformationDto.lopUniqueID = dataTable.Rows[i].Field<Guid>("lopUniqueID");
				eRPLeadInformationDto.lopExchangeRate = dataTable.Rows[i].Field<decimal>("lopExchangeRate");
				eRPLeadInformationDto.lopExpectedCloseDate = dataTable.Rows[i].Field<DateTime?>("lopExpectedCloseDate");
				eRPLeadInformationDto.lopExpirationDate = dataTable.Rows[i].Field<DateTime?>("lopExpirationDate");
				eRPLeadInformationDto.lopCreatedFromMobile = dataTable.Rows[i].Field<bool>("lopCreatedFromMobile");
				eRPLeadInformationDto.lopCustomRate = dataTable.Rows[i].Field<bool>("lopCustomRate");
				eRPLeadInformationDto.lopLeadDate = dataTable.Rows[i].Field<DateTime?>("lopLeadDate");
				eRPLeadInformationDto.lopLeadTotal = dataTable.Rows[i].Field<decimal>("lopLeadTotal");
				eRPLeadInformationDto.lopLeadTotalForeign = dataTable.Rows[i].Field<decimal>("lopLeadTotalForeign");
				eRPLeadInformationDto.lopLocationID = dataTable.Rows[i].Field<string>("lopLocationID");
				eRPLeadInformationDto.lopLongDescriptionRtf = dataTable.Rows[i].Field<string>("lopLongDescriptionRtf");
				eRPLeadInformationDto.lopLongDescriptionText = dataTable.Rows[i].Field<string>("lopLongDescriptionText");
				eRPLeadInformationDto.lopMarketingProgramID = dataTable.Rows[i].Field<string>("lopMarketingProgramID");
				eRPLeadInformationDto.lopMilestoneDate = dataTable.Rows[i].Field<DateTime?>("lopMilestoneDate");
				eRPLeadInformationDto.lopMilestoneID = dataTable.Rows[i].Field<string>("lopMilestoneID");
				eRPLeadInformationDto.lopPlantDepartmentID = dataTable.Rows[i].Field<string>("lopPlantDepartmentID");
				eRPLeadInformationDto.lopPlantID = dataTable.Rows[i].Field<string>("lopPlantID");
				eRPLeadInformationDto.lopProjectAreaID = dataTable.Rows[i].Field<string>("lopProjectAreaID");
				eRPLeadInformationDto.lopProjectID = dataTable.Rows[i].Field<string>("lopProjectID");
				eRPLeadInformationDto.lopQuoteContactID = dataTable.Rows[i].Field<string>("lopQuoteContactID");
				eRPLeadInformationDto.lopQuoteLocationID = dataTable.Rows[i].Field<string>("lopQuoteLocationID");
				eRPLeadInformationDto.lopQuoterEmployeeID = dataTable.Rows[i].Field<string>("lopQuoterEmployeeID");
				eRPLeadInformationDto.lopReferredBy = dataTable.Rows[i].Field<string>("lopReferredBy");
				eRPLeadInformationDto.lopResponseMethodID = dataTable.Rows[i].Field<string>("lopResponseMethodID");
				eRPLeadInformationDto.lopRowVersion = dataTable.Rows[i].Field<byte[]>("lopRowVersion");
				eRPLeadInformationDto.lopShipContactID = dataTable.Rows[i].Field<string>("lopShipContactID");
				eRPLeadInformationDto.lopShipLocationID = dataTable.Rows[i].Field<string>("lopShipLocationID");
				eRPLeadInformationDto.lopShipOrganizationID = dataTable.Rows[i].Field<string>("lopShipOrganizationID");
				eRPLeadInformationDto.lopShortDescription = dataTable.Rows[i].Field<string>("lopShortDescription");
				eRPLeadInformationDto.lopSplitPercentTotal = dataTable.Rows[i].Field<decimal>("lopSplitPercentTotal");
				eRPLeadInformationDto.lopStatus = dataTable.Rows[i].Field<string>("lopStatus");
				eRPLeadInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPLeadInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPLeadInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPLeadInformationDto> GetLead(Guid leadId)
	{
		ERPLeadInformationDto eRPLeadInformationDto = new ERPLeadInformationDto();
		InitializeParameterLists();
		string[] collection = new string[40]
		{
			"lopClosedByEmployeeID", "lopClosedDate", "lopClosedReasonID", "lopLeadID", "lopContactID", "lopCreatedBy", "lopCreatedDate", "lopCurrencyRateID", "lopCustomerOrganizationID", "lopUniqueID",
			"lopExchangeRate", "lopExpectedCloseDate", "lopExpirationDate", "lopCreatedFromMobile", "lopCustomRate", "lopLeadDate", "lopLeadTotal", "lopLeadTotalForeign", "lopLocationID", "lopLongDescriptionRtf",
			"lopLongDescriptionText", "lopMarketingProgramID", "lopMilestoneDate", "lopMilestoneID", "lopPlantDepartmentID", "lopPlantID", "lopProjectAreaID", "lopProjectID", "lopQuoteContactID", "lopQuoteLocationID",
			"lopQuoterEmployeeID", "lopReferredBy", "lopResponseMethodID", "lopRowVersion", "lopShipContactID", "lopShipLocationID", "lopShipOrganizationID", "lopShortDescription", "lopSplitPercentTotal", "lopStatus"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("lopUniqueID|C", leadId);
		AddCustomFieldsToSelectList("Leads");
		using (DataTable dataTable = GetAsDataTable("Leads", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPLeadInformationDto);
			}
			eRPLeadInformationDto.lopClosedByEmployeeID = dataTable.Rows[0].Field<string>("lopClosedByEmployeeID");
			eRPLeadInformationDto.lopClosedDate = dataTable.Rows[0].Field<DateTime?>("lopClosedDate");
			eRPLeadInformationDto.lopClosedReasonID = dataTable.Rows[0].Field<string>("lopClosedReasonID");
			eRPLeadInformationDto.lopLeadID = dataTable.Rows[0].Field<string>("lopLeadID");
			eRPLeadInformationDto.lopContactID = dataTable.Rows[0].Field<string>("lopContactID");
			eRPLeadInformationDto.lopCreatedBy = dataTable.Rows[0].Field<string>("lopCreatedBy");
			eRPLeadInformationDto.lopCreatedDate = dataTable.Rows[0].Field<DateTime?>("lopCreatedDate");
			eRPLeadInformationDto.lopCurrencyRateID = dataTable.Rows[0].Field<string>("lopCurrencyRateID");
			eRPLeadInformationDto.lopCustomerOrganizationID = dataTable.Rows[0].Field<string>("lopCustomerOrganizationID");
			eRPLeadInformationDto.lopUniqueID = dataTable.Rows[0].Field<Guid>("lopUniqueID");
			eRPLeadInformationDto.lopExchangeRate = dataTable.Rows[0].Field<decimal>("lopExchangeRate");
			eRPLeadInformationDto.lopExpectedCloseDate = dataTable.Rows[0].Field<DateTime?>("lopExpectedCloseDate");
			eRPLeadInformationDto.lopExpirationDate = dataTable.Rows[0].Field<DateTime?>("lopExpirationDate");
			eRPLeadInformationDto.lopCreatedFromMobile = dataTable.Rows[0].Field<bool>("lopCreatedFromMobile");
			eRPLeadInformationDto.lopCustomRate = dataTable.Rows[0].Field<bool>("lopCustomRate");
			eRPLeadInformationDto.lopLeadDate = dataTable.Rows[0].Field<DateTime?>("lopLeadDate");
			eRPLeadInformationDto.lopLeadTotal = dataTable.Rows[0].Field<decimal>("lopLeadTotal");
			eRPLeadInformationDto.lopLeadTotalForeign = dataTable.Rows[0].Field<decimal>("lopLeadTotalForeign");
			eRPLeadInformationDto.lopLocationID = dataTable.Rows[0].Field<string>("lopLocationID");
			eRPLeadInformationDto.lopLongDescriptionRtf = dataTable.Rows[0].Field<string>("lopLongDescriptionRtf");
			eRPLeadInformationDto.lopLongDescriptionText = dataTable.Rows[0].Field<string>("lopLongDescriptionText");
			eRPLeadInformationDto.lopMarketingProgramID = dataTable.Rows[0].Field<string>("lopMarketingProgramID");
			eRPLeadInformationDto.lopMilestoneDate = dataTable.Rows[0].Field<DateTime?>("lopMilestoneDate");
			eRPLeadInformationDto.lopMilestoneID = dataTable.Rows[0].Field<string>("lopMilestoneID");
			eRPLeadInformationDto.lopPlantDepartmentID = dataTable.Rows[0].Field<string>("lopPlantDepartmentID");
			eRPLeadInformationDto.lopPlantID = dataTable.Rows[0].Field<string>("lopPlantID");
			eRPLeadInformationDto.lopProjectAreaID = dataTable.Rows[0].Field<string>("lopProjectAreaID");
			eRPLeadInformationDto.lopProjectID = dataTable.Rows[0].Field<string>("lopProjectID");
			eRPLeadInformationDto.lopQuoteContactID = dataTable.Rows[0].Field<string>("lopQuoteContactID");
			eRPLeadInformationDto.lopQuoteLocationID = dataTable.Rows[0].Field<string>("lopQuoteLocationID");
			eRPLeadInformationDto.lopQuoterEmployeeID = dataTable.Rows[0].Field<string>("lopQuoterEmployeeID");
			eRPLeadInformationDto.lopReferredBy = dataTable.Rows[0].Field<string>("lopReferredBy");
			eRPLeadInformationDto.lopResponseMethodID = dataTable.Rows[0].Field<string>("lopResponseMethodID");
			eRPLeadInformationDto.lopRowVersion = dataTable.Rows[0].Field<byte[]>("lopRowVersion");
			eRPLeadInformationDto.lopShipContactID = dataTable.Rows[0].Field<string>("lopShipContactID");
			eRPLeadInformationDto.lopShipLocationID = dataTable.Rows[0].Field<string>("lopShipLocationID");
			eRPLeadInformationDto.lopShipOrganizationID = dataTable.Rows[0].Field<string>("lopShipOrganizationID");
			eRPLeadInformationDto.lopShortDescription = dataTable.Rows[0].Field<string>("lopShortDescription");
			eRPLeadInformationDto.lopSplitPercentTotal = dataTable.Rows[0].Field<decimal>("lopSplitPercentTotal");
			eRPLeadInformationDto.lopStatus = dataTable.Rows[0].Field<string>("lopStatus");
			eRPLeadInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPLeadInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPLeadInformationDto);
	}

	public Task<APIValidationInfoDto> SaveLead(ERPLeadDto lead)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Leads WHERE lopUniqueID = " + M1Util.ConvertToLinq(lead.lopUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["lopLeadID"] = lead.lopLeadID.ToUpper();
				lead.lopUniqueID = ((lead.lopUniqueID == Guid.Empty) ? Guid.NewGuid() : lead.lopUniqueID);
				dataRow["lopUniqueID"] = lead.lopUniqueID;
				dataRow["lopCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["lopCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Lead could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (lead.lopRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Lead is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["lopRowVersion"], lead.lopRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Lead has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Lead again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["lopClosedByEmployeeID"] = lead.lopClosedByEmployeeID;
			DataRow dataRow2 = dataRow;
			DateTime? lopClosedDate = lead.lopClosedDate;
			dataRow2["lopClosedDate"] = (lopClosedDate.HasValue ? ((object)lopClosedDate.GetValueOrDefault()) : dataRow["lopClosedDate"]);
			dataRow["lopClosedReasonID"] = lead.lopClosedReasonID;
			dataRow["lopContactID"] = lead.lopContactID;
			dataRow["lopCurrencyRateID"] = lead.lopCurrencyRateID;
			dataRow["lopCustomerOrganizationID"] = lead.lopCustomerOrganizationID;
			dataRow["lopExchangeRate"] = lead.lopExchangeRate;
			DataRow dataRow3 = dataRow;
			lopClosedDate = lead.lopExpectedCloseDate;
			dataRow3["lopExpectedCloseDate"] = (lopClosedDate.HasValue ? ((object)lopClosedDate.GetValueOrDefault()) : dataRow["lopExpectedCloseDate"]);
			DataRow dataRow4 = dataRow;
			lopClosedDate = lead.lopExpirationDate;
			dataRow4["lopExpirationDate"] = (lopClosedDate.HasValue ? ((object)lopClosedDate.GetValueOrDefault()) : dataRow["lopExpirationDate"]);
			dataRow["lopCreatedFromMobile"] = lead.lopCreatedFromMobile;
			dataRow["lopCustomRate"] = lead.lopCustomRate;
			DataRow dataRow5 = dataRow;
			lopClosedDate = lead.lopLeadDate;
			dataRow5["lopLeadDate"] = (lopClosedDate.HasValue ? ((object)lopClosedDate.GetValueOrDefault()) : dataRow["lopLeadDate"]);
			dataRow["lopLeadTotal"] = lead.lopLeadTotal;
			dataRow["lopLeadTotalForeign"] = lead.lopLeadTotalForeign;
			dataRow["lopLocationID"] = lead.lopLocationID;
			dataRow["lopLongDescriptionRtf"] = lead.lopLongDescriptionRtf ?? dataRow["lopLongDescriptionRtf"];
			dataRow["lopLongDescriptionText"] = lead.lopLongDescriptionText ?? dataRow["lopLongDescriptionText"];
			dataRow["lopMarketingProgramID"] = lead.lopMarketingProgramID;
			DataRow dataRow6 = dataRow;
			lopClosedDate = lead.lopMilestoneDate;
			dataRow6["lopMilestoneDate"] = (lopClosedDate.HasValue ? ((object)lopClosedDate.GetValueOrDefault()) : dataRow["lopMilestoneDate"]);
			dataRow["lopMilestoneID"] = lead.lopMilestoneID;
			dataRow["lopPlantDepartmentID"] = lead.lopPlantDepartmentID;
			dataRow["lopPlantID"] = lead.lopPlantID;
			dataRow["lopProjectAreaID"] = lead.lopProjectAreaID;
			dataRow["lopProjectID"] = lead.lopProjectID;
			dataRow["lopQuoteContactID"] = lead.lopQuoteContactID;
			dataRow["lopQuoteLocationID"] = lead.lopQuoteLocationID;
			dataRow["lopQuoterEmployeeID"] = lead.lopQuoterEmployeeID;
			dataRow["lopReferredBy"] = lead.lopReferredBy;
			dataRow["lopResponseMethodID"] = lead.lopResponseMethodID;
			dataRow["lopShipContactID"] = lead.lopShipContactID;
			dataRow["lopShipLocationID"] = lead.lopShipLocationID;
			dataRow["lopShipOrganizationID"] = lead.lopShipOrganizationID;
			dataRow["lopShortDescription"] = lead.lopShortDescription;
			dataRow["lopSplitPercentTotal"] = lead.lopSplitPercentTotal;
			dataRow["lopStatus"] = lead.lopStatus;
			if (lead.CustomFields != null && lead.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in lead.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Lead [{lead.lopUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Lead [{lead.lopUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
