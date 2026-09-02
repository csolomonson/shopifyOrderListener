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

public class ERPLandedCostRepository : APIBaseRepository, IERPLandedCostRepository, IAPIBaseRepository, IDisposable
{
	public ERPLandedCostRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesLandedCostExist(Guid landedCostId)
	{
		InitializeParameterLists();
		base.filterList.Add("rmcUniqueID|C", landedCostId);
		base.selectList.Add("rmcUniqueID");
		return Task.FromResult(GetAsObject("LandedCosts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPLandedCostInformationDto>> GetAllLandedCosts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPLandedCostInformationDto> collection = new List<ERPLandedCostInformationDto>();
		InitializeParameterLists();
		string[] array = new string[37]
		{
			"rmcCarrierName", "rmcClosedDate", "rmcLandedCostID", "rmcConsigneeContactID", "rmcConsigneeLocationID", "rmcConsigneeOrganizationID", "rmcCreatedBy", "rmcCreatedDate", "rmcDischargePoint", "rmcUniqueID",
			"rmcGlFiscalYearID", "rmcGlFiscalYearPeriodID", "rmcChargesComplete", "rmcChargesJournalsCreated", "rmcClosed", "rmcPoInTransitComplete", "rmcPoInTransitJournalsCreated", "rmcPostedToGl", "rmcReversalEntry", "rmcReversed",
			"rmcLandedCostChargesTotal", "rmcLandedCostDate", "rmcLandedCostPurchasesTotal", "rmcLandedCostReceiptsTotal", "rmcLandedCostTotal", "rmcLoadingPoint", "rmcLongDescriptionRtf", "rmcLongDescriptionText", "rmcPlantDepartmentID", "rmcPlantID",
			"rmcPostedDate", "rmcReverseLandedCostID", "rmcRowVersion", "rmcShipContactID", "rmcShipLocationID", "rmcShipOrganizationID", "rmcTrackingNumber"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("LandedCosts");
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
		using (DataTable dataTable = GetAsDataTable("LandedCosts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPLandedCostInformationDto eRPLandedCostInformationDto = new ERPLandedCostInformationDto();
				eRPLandedCostInformationDto.rmcCarrierName = dataTable.Rows[i].Field<string>("rmcCarrierName");
				eRPLandedCostInformationDto.rmcClosedDate = dataTable.Rows[i].Field<DateTime?>("rmcClosedDate");
				eRPLandedCostInformationDto.rmcLandedCostID = dataTable.Rows[i].Field<string>("rmcLandedCostID");
				eRPLandedCostInformationDto.rmcConsigneeContactID = dataTable.Rows[i].Field<string>("rmcConsigneeContactID");
				eRPLandedCostInformationDto.rmcConsigneeLocationID = dataTable.Rows[i].Field<string>("rmcConsigneeLocationID");
				eRPLandedCostInformationDto.rmcConsigneeOrganizationID = dataTable.Rows[i].Field<string>("rmcConsigneeOrganizationID");
				eRPLandedCostInformationDto.rmcCreatedBy = dataTable.Rows[i].Field<string>("rmcCreatedBy");
				eRPLandedCostInformationDto.rmcCreatedDate = dataTable.Rows[i].Field<DateTime?>("rmcCreatedDate");
				eRPLandedCostInformationDto.rmcDischargePoint = dataTable.Rows[i].Field<string>("rmcDischargePoint");
				eRPLandedCostInformationDto.rmcUniqueID = dataTable.Rows[i].Field<Guid>("rmcUniqueID");
				eRPLandedCostInformationDto.rmcGlFiscalYearID = dataTable.Rows[i].Field<short>("rmcGlFiscalYearID");
				eRPLandedCostInformationDto.rmcGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("rmcGlFiscalYearPeriodID");
				eRPLandedCostInformationDto.rmcChargesComplete = dataTable.Rows[i].Field<bool>("rmcChargesComplete");
				eRPLandedCostInformationDto.rmcChargesJournalsCreated = dataTable.Rows[i].Field<bool>("rmcChargesJournalsCreated");
				eRPLandedCostInformationDto.rmcClosed = dataTable.Rows[i].Field<bool>("rmcClosed");
				eRPLandedCostInformationDto.rmcPoInTransitComplete = dataTable.Rows[i].Field<bool>("rmcPoInTransitComplete");
				eRPLandedCostInformationDto.rmcPoInTransitJournalsCreated = dataTable.Rows[i].Field<bool>("rmcPoInTransitJournalsCreated");
				eRPLandedCostInformationDto.rmcPostedToGl = dataTable.Rows[i].Field<bool>("rmcPostedToGl");
				eRPLandedCostInformationDto.rmcReversalEntry = dataTable.Rows[i].Field<bool>("rmcReversalEntry");
				eRPLandedCostInformationDto.rmcReversed = dataTable.Rows[i].Field<bool>("rmcReversed");
				eRPLandedCostInformationDto.rmcLandedCostChargesTotal = dataTable.Rows[i].Field<decimal>("rmcLandedCostChargesTotal");
				eRPLandedCostInformationDto.rmcLandedCostDate = dataTable.Rows[i].Field<DateTime?>("rmcLandedCostDate");
				eRPLandedCostInformationDto.rmcLandedCostPurchasesTotal = dataTable.Rows[i].Field<decimal>("rmcLandedCostPurchasesTotal");
				eRPLandedCostInformationDto.rmcLandedCostReceiptsTotal = dataTable.Rows[i].Field<decimal>("rmcLandedCostReceiptsTotal");
				eRPLandedCostInformationDto.rmcLandedCostTotal = dataTable.Rows[i].Field<decimal>("rmcLandedCostTotal");
				eRPLandedCostInformationDto.rmcLoadingPoint = dataTable.Rows[i].Field<string>("rmcLoadingPoint");
				eRPLandedCostInformationDto.rmcLongDescriptionRtf = dataTable.Rows[i].Field<string>("rmcLongDescriptionRtf");
				eRPLandedCostInformationDto.rmcLongDescriptionText = dataTable.Rows[i].Field<string>("rmcLongDescriptionText");
				eRPLandedCostInformationDto.rmcPlantDepartmentID = dataTable.Rows[i].Field<string>("rmcPlantDepartmentID");
				eRPLandedCostInformationDto.rmcPlantID = dataTable.Rows[i].Field<string>("rmcPlantID");
				eRPLandedCostInformationDto.rmcPostedDate = dataTable.Rows[i].Field<DateTime?>("rmcPostedDate");
				eRPLandedCostInformationDto.rmcReverseLandedCostID = dataTable.Rows[i].Field<string>("rmcReverseLandedCostID");
				eRPLandedCostInformationDto.rmcRowVersion = dataTable.Rows[i].Field<byte[]>("rmcRowVersion");
				eRPLandedCostInformationDto.rmcShipContactID = dataTable.Rows[i].Field<string>("rmcShipContactID");
				eRPLandedCostInformationDto.rmcShipLocationID = dataTable.Rows[i].Field<string>("rmcShipLocationID");
				eRPLandedCostInformationDto.rmcShipOrganizationID = dataTable.Rows[i].Field<string>("rmcShipOrganizationID");
				eRPLandedCostInformationDto.rmcTrackingNumber = dataTable.Rows[i].Field<string>("rmcTrackingNumber");
				eRPLandedCostInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPLandedCostInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPLandedCostInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPLandedCostInformationDto> GetLandedCost(Guid landedCostId)
	{
		ERPLandedCostInformationDto eRPLandedCostInformationDto = new ERPLandedCostInformationDto();
		InitializeParameterLists();
		string[] collection = new string[37]
		{
			"rmcCarrierName", "rmcClosedDate", "rmcLandedCostID", "rmcConsigneeContactID", "rmcConsigneeLocationID", "rmcConsigneeOrganizationID", "rmcCreatedBy", "rmcCreatedDate", "rmcDischargePoint", "rmcUniqueID",
			"rmcGlFiscalYearID", "rmcGlFiscalYearPeriodID", "rmcChargesComplete", "rmcChargesJournalsCreated", "rmcClosed", "rmcPoInTransitComplete", "rmcPoInTransitJournalsCreated", "rmcPostedToGl", "rmcReversalEntry", "rmcReversed",
			"rmcLandedCostChargesTotal", "rmcLandedCostDate", "rmcLandedCostPurchasesTotal", "rmcLandedCostReceiptsTotal", "rmcLandedCostTotal", "rmcLoadingPoint", "rmcLongDescriptionRtf", "rmcLongDescriptionText", "rmcPlantDepartmentID", "rmcPlantID",
			"rmcPostedDate", "rmcReverseLandedCostID", "rmcRowVersion", "rmcShipContactID", "rmcShipLocationID", "rmcShipOrganizationID", "rmcTrackingNumber"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rmcUniqueID|C", landedCostId);
		AddCustomFieldsToSelectList("LandedCosts");
		using (DataTable dataTable = GetAsDataTable("LandedCosts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPLandedCostInformationDto);
			}
			eRPLandedCostInformationDto.rmcCarrierName = dataTable.Rows[0].Field<string>("rmcCarrierName");
			eRPLandedCostInformationDto.rmcClosedDate = dataTable.Rows[0].Field<DateTime?>("rmcClosedDate");
			eRPLandedCostInformationDto.rmcLandedCostID = dataTable.Rows[0].Field<string>("rmcLandedCostID");
			eRPLandedCostInformationDto.rmcConsigneeContactID = dataTable.Rows[0].Field<string>("rmcConsigneeContactID");
			eRPLandedCostInformationDto.rmcConsigneeLocationID = dataTable.Rows[0].Field<string>("rmcConsigneeLocationID");
			eRPLandedCostInformationDto.rmcConsigneeOrganizationID = dataTable.Rows[0].Field<string>("rmcConsigneeOrganizationID");
			eRPLandedCostInformationDto.rmcCreatedBy = dataTable.Rows[0].Field<string>("rmcCreatedBy");
			eRPLandedCostInformationDto.rmcCreatedDate = dataTable.Rows[0].Field<DateTime?>("rmcCreatedDate");
			eRPLandedCostInformationDto.rmcDischargePoint = dataTable.Rows[0].Field<string>("rmcDischargePoint");
			eRPLandedCostInformationDto.rmcUniqueID = dataTable.Rows[0].Field<Guid>("rmcUniqueID");
			eRPLandedCostInformationDto.rmcGlFiscalYearID = dataTable.Rows[0].Field<short>("rmcGlFiscalYearID");
			eRPLandedCostInformationDto.rmcGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("rmcGlFiscalYearPeriodID");
			eRPLandedCostInformationDto.rmcChargesComplete = dataTable.Rows[0].Field<bool>("rmcChargesComplete");
			eRPLandedCostInformationDto.rmcChargesJournalsCreated = dataTable.Rows[0].Field<bool>("rmcChargesJournalsCreated");
			eRPLandedCostInformationDto.rmcClosed = dataTable.Rows[0].Field<bool>("rmcClosed");
			eRPLandedCostInformationDto.rmcPoInTransitComplete = dataTable.Rows[0].Field<bool>("rmcPoInTransitComplete");
			eRPLandedCostInformationDto.rmcPoInTransitJournalsCreated = dataTable.Rows[0].Field<bool>("rmcPoInTransitJournalsCreated");
			eRPLandedCostInformationDto.rmcPostedToGl = dataTable.Rows[0].Field<bool>("rmcPostedToGl");
			eRPLandedCostInformationDto.rmcReversalEntry = dataTable.Rows[0].Field<bool>("rmcReversalEntry");
			eRPLandedCostInformationDto.rmcReversed = dataTable.Rows[0].Field<bool>("rmcReversed");
			eRPLandedCostInformationDto.rmcLandedCostChargesTotal = dataTable.Rows[0].Field<decimal>("rmcLandedCostChargesTotal");
			eRPLandedCostInformationDto.rmcLandedCostDate = dataTable.Rows[0].Field<DateTime?>("rmcLandedCostDate");
			eRPLandedCostInformationDto.rmcLandedCostPurchasesTotal = dataTable.Rows[0].Field<decimal>("rmcLandedCostPurchasesTotal");
			eRPLandedCostInformationDto.rmcLandedCostReceiptsTotal = dataTable.Rows[0].Field<decimal>("rmcLandedCostReceiptsTotal");
			eRPLandedCostInformationDto.rmcLandedCostTotal = dataTable.Rows[0].Field<decimal>("rmcLandedCostTotal");
			eRPLandedCostInformationDto.rmcLoadingPoint = dataTable.Rows[0].Field<string>("rmcLoadingPoint");
			eRPLandedCostInformationDto.rmcLongDescriptionRtf = dataTable.Rows[0].Field<string>("rmcLongDescriptionRtf");
			eRPLandedCostInformationDto.rmcLongDescriptionText = dataTable.Rows[0].Field<string>("rmcLongDescriptionText");
			eRPLandedCostInformationDto.rmcPlantDepartmentID = dataTable.Rows[0].Field<string>("rmcPlantDepartmentID");
			eRPLandedCostInformationDto.rmcPlantID = dataTable.Rows[0].Field<string>("rmcPlantID");
			eRPLandedCostInformationDto.rmcPostedDate = dataTable.Rows[0].Field<DateTime?>("rmcPostedDate");
			eRPLandedCostInformationDto.rmcReverseLandedCostID = dataTable.Rows[0].Field<string>("rmcReverseLandedCostID");
			eRPLandedCostInformationDto.rmcRowVersion = dataTable.Rows[0].Field<byte[]>("rmcRowVersion");
			eRPLandedCostInformationDto.rmcShipContactID = dataTable.Rows[0].Field<string>("rmcShipContactID");
			eRPLandedCostInformationDto.rmcShipLocationID = dataTable.Rows[0].Field<string>("rmcShipLocationID");
			eRPLandedCostInformationDto.rmcShipOrganizationID = dataTable.Rows[0].Field<string>("rmcShipOrganizationID");
			eRPLandedCostInformationDto.rmcTrackingNumber = dataTable.Rows[0].Field<string>("rmcTrackingNumber");
			eRPLandedCostInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPLandedCostInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPLandedCostInformationDto);
	}

	public Task<APIValidationInfoDto> SaveLandedCost(ERPLandedCostDto landedCost)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM LandedCosts WHERE rmcUniqueID = " + M1Util.ConvertToLinq(landedCost.rmcUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rmcLandedCostID"] = landedCost.rmcLandedCostID.ToUpper();
				landedCost.rmcUniqueID = ((landedCost.rmcUniqueID == Guid.Empty) ? Guid.NewGuid() : landedCost.rmcUniqueID);
				dataRow["rmcUniqueID"] = landedCost.rmcUniqueID;
				dataRow["rmcCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rmcCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The LandedCost could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (landedCost.rmcRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the LandedCost is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rmcRowVersion"], landedCost.rmcRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the LandedCost has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the LandedCost again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rmcCarrierName"] = landedCost.rmcCarrierName;
			DataRow dataRow2 = dataRow;
			DateTime? rmcClosedDate = landedCost.rmcClosedDate;
			dataRow2["rmcClosedDate"] = (rmcClosedDate.HasValue ? ((object)rmcClosedDate.GetValueOrDefault()) : dataRow["rmcClosedDate"]);
			dataRow["rmcConsigneeContactID"] = landedCost.rmcConsigneeContactID;
			dataRow["rmcConsigneeLocationID"] = landedCost.rmcConsigneeLocationID;
			dataRow["rmcConsigneeOrganizationID"] = landedCost.rmcConsigneeOrganizationID;
			dataRow["rmcDischargePoint"] = landedCost.rmcDischargePoint;
			dataRow["rmcGlFiscalYearID"] = landedCost.rmcGlFiscalYearID;
			dataRow["rmcGlFiscalYearPeriodID"] = landedCost.rmcGlFiscalYearPeriodID;
			dataRow["rmcChargesComplete"] = landedCost.rmcChargesComplete;
			dataRow["rmcChargesJournalsCreated"] = landedCost.rmcChargesJournalsCreated;
			dataRow["rmcClosed"] = landedCost.rmcClosed;
			dataRow["rmcPoInTransitComplete"] = landedCost.rmcPoInTransitComplete;
			dataRow["rmcPoInTransitJournalsCreated"] = landedCost.rmcPoInTransitJournalsCreated;
			dataRow["rmcPostedToGl"] = landedCost.rmcPostedToGl;
			dataRow["rmcReversalEntry"] = landedCost.rmcReversalEntry;
			dataRow["rmcReversed"] = landedCost.rmcReversed;
			dataRow["rmcLandedCostChargesTotal"] = landedCost.rmcLandedCostChargesTotal;
			DataRow dataRow3 = dataRow;
			rmcClosedDate = landedCost.rmcLandedCostDate;
			dataRow3["rmcLandedCostDate"] = (rmcClosedDate.HasValue ? ((object)rmcClosedDate.GetValueOrDefault()) : dataRow["rmcLandedCostDate"]);
			dataRow["rmcLandedCostPurchasesTotal"] = landedCost.rmcLandedCostPurchasesTotal;
			dataRow["rmcLandedCostReceiptsTotal"] = landedCost.rmcLandedCostReceiptsTotal;
			dataRow["rmcLandedCostTotal"] = landedCost.rmcLandedCostTotal;
			dataRow["rmcLoadingPoint"] = landedCost.rmcLoadingPoint;
			dataRow["rmcLongDescriptionRtf"] = landedCost.rmcLongDescriptionRtf ?? dataRow["rmcLongDescriptionRtf"];
			dataRow["rmcLongDescriptionText"] = landedCost.rmcLongDescriptionText ?? dataRow["rmcLongDescriptionText"];
			dataRow["rmcPlantDepartmentID"] = landedCost.rmcPlantDepartmentID;
			dataRow["rmcPlantID"] = landedCost.rmcPlantID;
			DataRow dataRow4 = dataRow;
			rmcClosedDate = landedCost.rmcPostedDate;
			dataRow4["rmcPostedDate"] = (rmcClosedDate.HasValue ? ((object)rmcClosedDate.GetValueOrDefault()) : dataRow["rmcPostedDate"]);
			dataRow["rmcReverseLandedCostID"] = landedCost.rmcReverseLandedCostID;
			dataRow["rmcShipContactID"] = landedCost.rmcShipContactID;
			dataRow["rmcShipLocationID"] = landedCost.rmcShipLocationID;
			dataRow["rmcShipOrganizationID"] = landedCost.rmcShipOrganizationID;
			dataRow["rmcTrackingNumber"] = landedCost.rmcTrackingNumber;
			if (landedCost.CustomFields != null && landedCost.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in landedCost.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the LandedCost [{landedCost.rmcUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the LandedCost [{landedCost.rmcUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
