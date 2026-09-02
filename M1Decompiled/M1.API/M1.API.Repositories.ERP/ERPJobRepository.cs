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

public class ERPJobRepository : APIBaseRepository, IERPJobRepository, IAPIBaseRepository, IDisposable
{
	public ERPJobRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesJobExist(Guid jobId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmpUniqueID|C", jobId);
		base.selectList.Add("jmpUniqueID");
		return Task.FromResult(GetAsObject("Jobs", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPJobInformationDto>> GetAllJobs(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPJobInformationDto> collection = new List<ERPJobInformationDto>();
		InitializeParameterLists();
		string[] array = new string[64]
		{
			"jmpCallID", "jmpClosedDate", "jmpJobID", "jmpCompletedDate", "jmpCreatedBy", "jmpCreatedDate", "jmpCustomerOrganizationID", "jmpDocuments", "jmpUniqueID", "jmpInventoryQuantity",
			"jmpClosed", "jmpFirm", "jmpNestlinkProcessed", "jmpOnHold", "jmpPlanningComplete", "jmpProductionComplete", "jmpReadyToPrint", "jmpReleasedToFloor", "jmpScheduleComplete", "jmpScheduleLocked",
			"jmpTimeAndMaterial", "jmpJobDate", "jmpJobPriorityID", "jmpNonConformanceID", "jmpOrderQuantity", "jmpPartBinID", "jmpPartForecastPeriodID", "jmpPartForecastYearID", "jmpPartID", "jmpPartLongDescriptionRtf",
			"jmpPartLongDescriptionText", "jmpPartRevisionID", "jmpPartShortDescription", "jmpPartWareHouseLocationID", "jmpPlannerEmployeeID", "jmpPlantDepartmentID", "jmpPlantID", "jmpProductionDueDate", "jmpProductionNotesRTF", "jmpProductionNotesText",
			"jmpProductionQuantity", "jmpProjectAreaID", "jmpProjectID", "jmpQuantityCompleted", "jmpQuantityReceivedToInventory", "jmpQuantityShipped", "jmpQuoteID", "jmpQuoteLineID", "jmpReworkDate", "jmpReworkQuantity",
			"jmpRmaClaimID", "jmpRmaClaimLineID", "jmpRowVersion", "jmpScheduledDueDate", "jmpScheduledDueHour", "jmpScheduledStartDate", "jmpScheduledStartHour", "jmpScrapQuantity", "jmpScrapQuantityCompleted", "jmpShipLocationID",
			"jmpShipOrganizationID", "jmpSourceMethodID", "jmpSourceRevisionID", "jmpUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Jobs");
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
		using (DataTable dataTable = GetAsDataTable("Jobs", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPJobInformationDto eRPJobInformationDto = new ERPJobInformationDto();
				eRPJobInformationDto.jmpCallID = dataTable.Rows[i].Field<string>("jmpCallID");
				eRPJobInformationDto.jmpClosedDate = dataTable.Rows[i].Field<DateTime?>("jmpClosedDate");
				eRPJobInformationDto.jmpJobID = dataTable.Rows[i].Field<string>("jmpJobID");
				eRPJobInformationDto.jmpCompletedDate = dataTable.Rows[i].Field<DateTime?>("jmpCompletedDate");
				eRPJobInformationDto.jmpCreatedBy = dataTable.Rows[i].Field<string>("jmpCreatedBy");
				eRPJobInformationDto.jmpCreatedDate = dataTable.Rows[i].Field<DateTime?>("jmpCreatedDate");
				eRPJobInformationDto.jmpCustomerOrganizationID = dataTable.Rows[i].Field<string>("jmpCustomerOrganizationID");
				eRPJobInformationDto.jmpDocuments = dataTable.Rows[i].Field<string>("jmpDocuments");
				eRPJobInformationDto.jmpUniqueID = dataTable.Rows[i].Field<Guid>("jmpUniqueID");
				eRPJobInformationDto.jmpInventoryQuantity = dataTable.Rows[i].Field<decimal>("jmpInventoryQuantity");
				eRPJobInformationDto.jmpClosed = dataTable.Rows[i].Field<bool>("jmpClosed");
				eRPJobInformationDto.jmpFirm = dataTable.Rows[i].Field<bool>("jmpFirm");
				eRPJobInformationDto.jmpNestlinkProcessed = dataTable.Rows[i].Field<bool>("jmpNestlinkProcessed");
				eRPJobInformationDto.jmpOnHold = dataTable.Rows[i].Field<bool>("jmpOnHold");
				eRPJobInformationDto.jmpPlanningComplete = dataTable.Rows[i].Field<bool>("jmpPlanningComplete");
				eRPJobInformationDto.jmpProductionComplete = dataTable.Rows[i].Field<bool>("jmpProductionComplete");
				eRPJobInformationDto.jmpReadyToPrint = dataTable.Rows[i].Field<bool>("jmpReadyToPrint");
				eRPJobInformationDto.jmpReleasedToFloor = dataTable.Rows[i].Field<bool>("jmpReleasedToFloor");
				eRPJobInformationDto.jmpScheduleComplete = dataTable.Rows[i].Field<bool>("jmpScheduleComplete");
				eRPJobInformationDto.jmpScheduleLocked = dataTable.Rows[i].Field<bool>("jmpScheduleLocked");
				eRPJobInformationDto.jmpTimeAndMaterial = dataTable.Rows[i].Field<bool>("jmpTimeAndMaterial");
				eRPJobInformationDto.jmpJobDate = dataTable.Rows[i].Field<DateTime?>("jmpJobDate");
				eRPJobInformationDto.jmpJobPriorityID = dataTable.Rows[i].Field<short>("jmpJobPriorityID");
				eRPJobInformationDto.jmpNonConformanceID = dataTable.Rows[i].Field<string>("jmpNonConformanceID");
				eRPJobInformationDto.jmpOrderQuantity = dataTable.Rows[i].Field<decimal>("jmpOrderQuantity");
				eRPJobInformationDto.jmpPartBinID = dataTable.Rows[i].Field<string>("jmpPartBinID");
				eRPJobInformationDto.jmpPartForecastPeriodID = dataTable.Rows[i].Field<short>("jmpPartForecastPeriodID");
				eRPJobInformationDto.jmpPartForecastYearID = dataTable.Rows[i].Field<short>("jmpPartForecastYearID");
				eRPJobInformationDto.jmpPartID = dataTable.Rows[i].Field<string>("jmpPartID");
				eRPJobInformationDto.jmpPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("jmpPartLongDescriptionRtf");
				eRPJobInformationDto.jmpPartLongDescriptionText = dataTable.Rows[i].Field<string>("jmpPartLongDescriptionText");
				eRPJobInformationDto.jmpPartRevisionID = dataTable.Rows[i].Field<string>("jmpPartRevisionID");
				eRPJobInformationDto.jmpPartShortDescription = dataTable.Rows[i].Field<string>("jmpPartShortDescription");
				eRPJobInformationDto.jmpPartWareHouseLocationID = dataTable.Rows[i].Field<string>("jmpPartWareHouseLocationID");
				eRPJobInformationDto.jmpPlannerEmployeeID = dataTable.Rows[i].Field<string>("jmpPlannerEmployeeID");
				eRPJobInformationDto.jmpPlantDepartmentID = dataTable.Rows[i].Field<string>("jmpPlantDepartmentID");
				eRPJobInformationDto.jmpPlantID = dataTable.Rows[i].Field<string>("jmpPlantID");
				eRPJobInformationDto.jmpProductionDueDate = dataTable.Rows[i].Field<DateTime?>("jmpProductionDueDate");
				eRPJobInformationDto.jmpProductionNotesRTF = dataTable.Rows[i].Field<string>("jmpProductionNotesRTF");
				eRPJobInformationDto.jmpProductionNotesText = dataTable.Rows[i].Field<string>("jmpProductionNotesText");
				eRPJobInformationDto.jmpProductionQuantity = dataTable.Rows[i].Field<decimal>("jmpProductionQuantity");
				eRPJobInformationDto.jmpProjectAreaID = dataTable.Rows[i].Field<string>("jmpProjectAreaID");
				eRPJobInformationDto.jmpProjectID = dataTable.Rows[i].Field<string>("jmpProjectID");
				eRPJobInformationDto.jmpQuantityCompleted = dataTable.Rows[i].Field<decimal>("jmpQuantityCompleted");
				eRPJobInformationDto.jmpQuantityReceivedToInventory = dataTable.Rows[i].Field<decimal>("jmpQuantityReceivedToInventory");
				eRPJobInformationDto.jmpQuantityShipped = dataTable.Rows[i].Field<decimal>("jmpQuantityShipped");
				eRPJobInformationDto.jmpQuoteID = dataTable.Rows[i].Field<string>("jmpQuoteID");
				eRPJobInformationDto.jmpQuoteLineID = dataTable.Rows[i].Field<short>("jmpQuoteLineID");
				eRPJobInformationDto.jmpReworkDate = dataTable.Rows[i].Field<DateTime?>("jmpReworkDate");
				eRPJobInformationDto.jmpReworkQuantity = dataTable.Rows[i].Field<decimal>("jmpReworkQuantity");
				eRPJobInformationDto.jmpRmaClaimID = dataTable.Rows[i].Field<string>("jmpRmaClaimID");
				eRPJobInformationDto.jmpRmaClaimLineID = dataTable.Rows[i].Field<short>("jmpRmaClaimLineID");
				eRPJobInformationDto.jmpRowVersion = dataTable.Rows[i].Field<byte[]>("jmpRowVersion");
				eRPJobInformationDto.jmpScheduledDueDate = dataTable.Rows[i].Field<DateTime?>("jmpScheduledDueDate");
				eRPJobInformationDto.jmpScheduledDueHour = dataTable.Rows[i].Field<decimal>("jmpScheduledDueHour");
				eRPJobInformationDto.jmpScheduledStartDate = dataTable.Rows[i].Field<DateTime?>("jmpScheduledStartDate");
				eRPJobInformationDto.jmpScheduledStartHour = dataTable.Rows[i].Field<decimal>("jmpScheduledStartHour");
				eRPJobInformationDto.jmpScrapQuantity = dataTable.Rows[i].Field<decimal>("jmpScrapQuantity");
				eRPJobInformationDto.jmpScrapQuantityCompleted = dataTable.Rows[i].Field<decimal>("jmpScrapQuantityCompleted");
				eRPJobInformationDto.jmpShipLocationID = dataTable.Rows[i].Field<string>("jmpShipLocationID");
				eRPJobInformationDto.jmpShipOrganizationID = dataTable.Rows[i].Field<string>("jmpShipOrganizationID");
				eRPJobInformationDto.jmpSourceMethodID = dataTable.Rows[i].Field<string>("jmpSourceMethodID");
				eRPJobInformationDto.jmpSourceRevisionID = dataTable.Rows[i].Field<string>("jmpSourceRevisionID");
				eRPJobInformationDto.jmpUnitOfMeasure = dataTable.Rows[i].Field<string>("jmpUnitOfMeasure");
				eRPJobInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPJobInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPJobInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPJobInformationDto> GetJob(Guid jobId)
	{
		ERPJobInformationDto eRPJobInformationDto = new ERPJobInformationDto();
		InitializeParameterLists();
		string[] collection = new string[64]
		{
			"jmpCallID", "jmpClosedDate", "jmpJobID", "jmpCompletedDate", "jmpCreatedBy", "jmpCreatedDate", "jmpCustomerOrganizationID", "jmpDocuments", "jmpUniqueID", "jmpInventoryQuantity",
			"jmpClosed", "jmpFirm", "jmpNestlinkProcessed", "jmpOnHold", "jmpPlanningComplete", "jmpProductionComplete", "jmpReadyToPrint", "jmpReleasedToFloor", "jmpScheduleComplete", "jmpScheduleLocked",
			"jmpTimeAndMaterial", "jmpJobDate", "jmpJobPriorityID", "jmpNonConformanceID", "jmpOrderQuantity", "jmpPartBinID", "jmpPartForecastPeriodID", "jmpPartForecastYearID", "jmpPartID", "jmpPartLongDescriptionRtf",
			"jmpPartLongDescriptionText", "jmpPartRevisionID", "jmpPartShortDescription", "jmpPartWareHouseLocationID", "jmpPlannerEmployeeID", "jmpPlantDepartmentID", "jmpPlantID", "jmpProductionDueDate", "jmpProductionNotesRTF", "jmpProductionNotesText",
			"jmpProductionQuantity", "jmpProjectAreaID", "jmpProjectID", "jmpQuantityCompleted", "jmpQuantityReceivedToInventory", "jmpQuantityShipped", "jmpQuoteID", "jmpQuoteLineID", "jmpReworkDate", "jmpReworkQuantity",
			"jmpRmaClaimID", "jmpRmaClaimLineID", "jmpRowVersion", "jmpScheduledDueDate", "jmpScheduledDueHour", "jmpScheduledStartDate", "jmpScheduledStartHour", "jmpScrapQuantity", "jmpScrapQuantityCompleted", "jmpShipLocationID",
			"jmpShipOrganizationID", "jmpSourceMethodID", "jmpSourceRevisionID", "jmpUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("jmpUniqueID|C", jobId);
		AddCustomFieldsToSelectList("Jobs");
		using (DataTable dataTable = GetAsDataTable("Jobs", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPJobInformationDto);
			}
			eRPJobInformationDto.jmpCallID = dataTable.Rows[0].Field<string>("jmpCallID");
			eRPJobInformationDto.jmpClosedDate = dataTable.Rows[0].Field<DateTime?>("jmpClosedDate");
			eRPJobInformationDto.jmpJobID = dataTable.Rows[0].Field<string>("jmpJobID");
			eRPJobInformationDto.jmpCompletedDate = dataTable.Rows[0].Field<DateTime?>("jmpCompletedDate");
			eRPJobInformationDto.jmpCreatedBy = dataTable.Rows[0].Field<string>("jmpCreatedBy");
			eRPJobInformationDto.jmpCreatedDate = dataTable.Rows[0].Field<DateTime?>("jmpCreatedDate");
			eRPJobInformationDto.jmpCustomerOrganizationID = dataTable.Rows[0].Field<string>("jmpCustomerOrganizationID");
			eRPJobInformationDto.jmpDocuments = dataTable.Rows[0].Field<string>("jmpDocuments");
			eRPJobInformationDto.jmpUniqueID = dataTable.Rows[0].Field<Guid>("jmpUniqueID");
			eRPJobInformationDto.jmpInventoryQuantity = dataTable.Rows[0].Field<decimal>("jmpInventoryQuantity");
			eRPJobInformationDto.jmpClosed = dataTable.Rows[0].Field<bool>("jmpClosed");
			eRPJobInformationDto.jmpFirm = dataTable.Rows[0].Field<bool>("jmpFirm");
			eRPJobInformationDto.jmpNestlinkProcessed = dataTable.Rows[0].Field<bool>("jmpNestlinkProcessed");
			eRPJobInformationDto.jmpOnHold = dataTable.Rows[0].Field<bool>("jmpOnHold");
			eRPJobInformationDto.jmpPlanningComplete = dataTable.Rows[0].Field<bool>("jmpPlanningComplete");
			eRPJobInformationDto.jmpProductionComplete = dataTable.Rows[0].Field<bool>("jmpProductionComplete");
			eRPJobInformationDto.jmpReadyToPrint = dataTable.Rows[0].Field<bool>("jmpReadyToPrint");
			eRPJobInformationDto.jmpReleasedToFloor = dataTable.Rows[0].Field<bool>("jmpReleasedToFloor");
			eRPJobInformationDto.jmpScheduleComplete = dataTable.Rows[0].Field<bool>("jmpScheduleComplete");
			eRPJobInformationDto.jmpScheduleLocked = dataTable.Rows[0].Field<bool>("jmpScheduleLocked");
			eRPJobInformationDto.jmpTimeAndMaterial = dataTable.Rows[0].Field<bool>("jmpTimeAndMaterial");
			eRPJobInformationDto.jmpJobDate = dataTable.Rows[0].Field<DateTime?>("jmpJobDate");
			eRPJobInformationDto.jmpJobPriorityID = dataTable.Rows[0].Field<short>("jmpJobPriorityID");
			eRPJobInformationDto.jmpNonConformanceID = dataTable.Rows[0].Field<string>("jmpNonConformanceID");
			eRPJobInformationDto.jmpOrderQuantity = dataTable.Rows[0].Field<decimal>("jmpOrderQuantity");
			eRPJobInformationDto.jmpPartBinID = dataTable.Rows[0].Field<string>("jmpPartBinID");
			eRPJobInformationDto.jmpPartForecastPeriodID = dataTable.Rows[0].Field<short>("jmpPartForecastPeriodID");
			eRPJobInformationDto.jmpPartForecastYearID = dataTable.Rows[0].Field<short>("jmpPartForecastYearID");
			eRPJobInformationDto.jmpPartID = dataTable.Rows[0].Field<string>("jmpPartID");
			eRPJobInformationDto.jmpPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("jmpPartLongDescriptionRtf");
			eRPJobInformationDto.jmpPartLongDescriptionText = dataTable.Rows[0].Field<string>("jmpPartLongDescriptionText");
			eRPJobInformationDto.jmpPartRevisionID = dataTable.Rows[0].Field<string>("jmpPartRevisionID");
			eRPJobInformationDto.jmpPartShortDescription = dataTable.Rows[0].Field<string>("jmpPartShortDescription");
			eRPJobInformationDto.jmpPartWareHouseLocationID = dataTable.Rows[0].Field<string>("jmpPartWareHouseLocationID");
			eRPJobInformationDto.jmpPlannerEmployeeID = dataTable.Rows[0].Field<string>("jmpPlannerEmployeeID");
			eRPJobInformationDto.jmpPlantDepartmentID = dataTable.Rows[0].Field<string>("jmpPlantDepartmentID");
			eRPJobInformationDto.jmpPlantID = dataTable.Rows[0].Field<string>("jmpPlantID");
			eRPJobInformationDto.jmpProductionDueDate = dataTable.Rows[0].Field<DateTime?>("jmpProductionDueDate");
			eRPJobInformationDto.jmpProductionNotesRTF = dataTable.Rows[0].Field<string>("jmpProductionNotesRTF");
			eRPJobInformationDto.jmpProductionNotesText = dataTable.Rows[0].Field<string>("jmpProductionNotesText");
			eRPJobInformationDto.jmpProductionQuantity = dataTable.Rows[0].Field<decimal>("jmpProductionQuantity");
			eRPJobInformationDto.jmpProjectAreaID = dataTable.Rows[0].Field<string>("jmpProjectAreaID");
			eRPJobInformationDto.jmpProjectID = dataTable.Rows[0].Field<string>("jmpProjectID");
			eRPJobInformationDto.jmpQuantityCompleted = dataTable.Rows[0].Field<decimal>("jmpQuantityCompleted");
			eRPJobInformationDto.jmpQuantityReceivedToInventory = dataTable.Rows[0].Field<decimal>("jmpQuantityReceivedToInventory");
			eRPJobInformationDto.jmpQuantityShipped = dataTable.Rows[0].Field<decimal>("jmpQuantityShipped");
			eRPJobInformationDto.jmpQuoteID = dataTable.Rows[0].Field<string>("jmpQuoteID");
			eRPJobInformationDto.jmpQuoteLineID = dataTable.Rows[0].Field<short>("jmpQuoteLineID");
			eRPJobInformationDto.jmpReworkDate = dataTable.Rows[0].Field<DateTime?>("jmpReworkDate");
			eRPJobInformationDto.jmpReworkQuantity = dataTable.Rows[0].Field<decimal>("jmpReworkQuantity");
			eRPJobInformationDto.jmpRmaClaimID = dataTable.Rows[0].Field<string>("jmpRmaClaimID");
			eRPJobInformationDto.jmpRmaClaimLineID = dataTable.Rows[0].Field<short>("jmpRmaClaimLineID");
			eRPJobInformationDto.jmpRowVersion = dataTable.Rows[0].Field<byte[]>("jmpRowVersion");
			eRPJobInformationDto.jmpScheduledDueDate = dataTable.Rows[0].Field<DateTime?>("jmpScheduledDueDate");
			eRPJobInformationDto.jmpScheduledDueHour = dataTable.Rows[0].Field<decimal>("jmpScheduledDueHour");
			eRPJobInformationDto.jmpScheduledStartDate = dataTable.Rows[0].Field<DateTime?>("jmpScheduledStartDate");
			eRPJobInformationDto.jmpScheduledStartHour = dataTable.Rows[0].Field<decimal>("jmpScheduledStartHour");
			eRPJobInformationDto.jmpScrapQuantity = dataTable.Rows[0].Field<decimal>("jmpScrapQuantity");
			eRPJobInformationDto.jmpScrapQuantityCompleted = dataTable.Rows[0].Field<decimal>("jmpScrapQuantityCompleted");
			eRPJobInformationDto.jmpShipLocationID = dataTable.Rows[0].Field<string>("jmpShipLocationID");
			eRPJobInformationDto.jmpShipOrganizationID = dataTable.Rows[0].Field<string>("jmpShipOrganizationID");
			eRPJobInformationDto.jmpSourceMethodID = dataTable.Rows[0].Field<string>("jmpSourceMethodID");
			eRPJobInformationDto.jmpSourceRevisionID = dataTable.Rows[0].Field<string>("jmpSourceRevisionID");
			eRPJobInformationDto.jmpUnitOfMeasure = dataTable.Rows[0].Field<string>("jmpUnitOfMeasure");
			eRPJobInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPJobInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPJobInformationDto);
	}

	public Task<APIValidationInfoDto> SaveJob(ERPJobDto job)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Jobs WHERE jmpUniqueID = " + M1Util.ConvertToLinq(job.jmpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["jmpJobID"] = job.jmpJobID.ToUpper();
				job.jmpUniqueID = ((job.jmpUniqueID == Guid.Empty) ? Guid.NewGuid() : job.jmpUniqueID);
				dataRow["jmpUniqueID"] = job.jmpUniqueID;
				dataRow["jmpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["jmpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Job could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (job.jmpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Job is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["jmpRowVersion"], job.jmpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Job has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Job again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["jmpCallID"] = job.jmpCallID;
			DataRow dataRow2 = dataRow;
			DateTime? jmpClosedDate = job.jmpClosedDate;
			dataRow2["jmpClosedDate"] = (jmpClosedDate.HasValue ? ((object)jmpClosedDate.GetValueOrDefault()) : dataRow["jmpClosedDate"]);
			DataRow dataRow3 = dataRow;
			jmpClosedDate = job.jmpCompletedDate;
			dataRow3["jmpCompletedDate"] = (jmpClosedDate.HasValue ? ((object)jmpClosedDate.GetValueOrDefault()) : dataRow["jmpCompletedDate"]);
			dataRow["jmpCustomerOrganizationID"] = job.jmpCustomerOrganizationID;
			dataRow["jmpDocuments"] = job.jmpDocuments ?? dataRow["jmpDocuments"];
			dataRow["jmpInventoryQuantity"] = job.jmpInventoryQuantity;
			dataRow["jmpClosed"] = job.jmpClosed;
			dataRow["jmpFirm"] = job.jmpFirm;
			dataRow["jmpNestlinkProcessed"] = job.jmpNestlinkProcessed;
			dataRow["jmpOnHold"] = job.jmpOnHold;
			dataRow["jmpPlanningComplete"] = job.jmpPlanningComplete;
			dataRow["jmpProductionComplete"] = job.jmpProductionComplete;
			dataRow["jmpReadyToPrint"] = job.jmpReadyToPrint;
			dataRow["jmpReleasedToFloor"] = job.jmpReleasedToFloor;
			dataRow["jmpScheduleComplete"] = job.jmpScheduleComplete;
			dataRow["jmpScheduleLocked"] = job.jmpScheduleLocked;
			dataRow["jmpTimeAndMaterial"] = job.jmpTimeAndMaterial;
			DataRow dataRow4 = dataRow;
			jmpClosedDate = job.jmpJobDate;
			dataRow4["jmpJobDate"] = (jmpClosedDate.HasValue ? ((object)jmpClosedDate.GetValueOrDefault()) : dataRow["jmpJobDate"]);
			dataRow["jmpJobPriorityID"] = job.jmpJobPriorityID;
			dataRow["jmpNonConformanceID"] = job.jmpNonConformanceID;
			dataRow["jmpOrderQuantity"] = job.jmpOrderQuantity;
			dataRow["jmpPartBinID"] = job.jmpPartBinID;
			dataRow["jmpPartForecastPeriodID"] = job.jmpPartForecastPeriodID;
			dataRow["jmpPartForecastYearID"] = job.jmpPartForecastYearID;
			dataRow["jmpPartID"] = job.jmpPartID;
			dataRow["jmpPartLongDescriptionRtf"] = job.jmpPartLongDescriptionRtf ?? dataRow["jmpPartLongDescriptionRtf"];
			dataRow["jmpPartLongDescriptionText"] = job.jmpPartLongDescriptionText ?? dataRow["jmpPartLongDescriptionText"];
			dataRow["jmpPartRevisionID"] = job.jmpPartRevisionID;
			dataRow["jmpPartShortDescription"] = job.jmpPartShortDescription;
			dataRow["jmpPartWareHouseLocationID"] = job.jmpPartWareHouseLocationID;
			dataRow["jmpPlannerEmployeeID"] = job.jmpPlannerEmployeeID;
			dataRow["jmpPlantDepartmentID"] = job.jmpPlantDepartmentID;
			dataRow["jmpPlantID"] = job.jmpPlantID;
			DataRow dataRow5 = dataRow;
			jmpClosedDate = job.jmpProductionDueDate;
			dataRow5["jmpProductionDueDate"] = (jmpClosedDate.HasValue ? ((object)jmpClosedDate.GetValueOrDefault()) : dataRow["jmpProductionDueDate"]);
			dataRow["jmpProductionNotesRTF"] = job.jmpProductionNotesRTF ?? dataRow["jmpProductionNotesRTF"];
			dataRow["jmpProductionNotesText"] = job.jmpProductionNotesText ?? dataRow["jmpProductionNotesText"];
			dataRow["jmpProductionQuantity"] = job.jmpProductionQuantity;
			dataRow["jmpProjectAreaID"] = job.jmpProjectAreaID;
			dataRow["jmpProjectID"] = job.jmpProjectID;
			dataRow["jmpQuantityCompleted"] = job.jmpQuantityCompleted;
			dataRow["jmpQuantityReceivedToInventory"] = job.jmpQuantityReceivedToInventory;
			dataRow["jmpQuantityShipped"] = job.jmpQuantityShipped;
			dataRow["jmpQuoteID"] = job.jmpQuoteID;
			dataRow["jmpQuoteLineID"] = job.jmpQuoteLineID;
			DataRow dataRow6 = dataRow;
			jmpClosedDate = job.jmpReworkDate;
			dataRow6["jmpReworkDate"] = (jmpClosedDate.HasValue ? ((object)jmpClosedDate.GetValueOrDefault()) : dataRow["jmpReworkDate"]);
			dataRow["jmpReworkQuantity"] = job.jmpReworkQuantity;
			dataRow["jmpRmaClaimID"] = job.jmpRmaClaimID;
			dataRow["jmpRmaClaimLineID"] = job.jmpRmaClaimLineID;
			DataRow dataRow7 = dataRow;
			jmpClosedDate = job.jmpScheduledDueDate;
			dataRow7["jmpScheduledDueDate"] = (jmpClosedDate.HasValue ? ((object)jmpClosedDate.GetValueOrDefault()) : dataRow["jmpScheduledDueDate"]);
			dataRow["jmpScheduledDueHour"] = job.jmpScheduledDueHour;
			DataRow dataRow8 = dataRow;
			jmpClosedDate = job.jmpScheduledStartDate;
			dataRow8["jmpScheduledStartDate"] = (jmpClosedDate.HasValue ? ((object)jmpClosedDate.GetValueOrDefault()) : dataRow["jmpScheduledStartDate"]);
			dataRow["jmpScheduledStartHour"] = job.jmpScheduledStartHour;
			dataRow["jmpScrapQuantity"] = job.jmpScrapQuantity;
			dataRow["jmpScrapQuantityCompleted"] = job.jmpScrapQuantityCompleted;
			dataRow["jmpShipLocationID"] = job.jmpShipLocationID;
			dataRow["jmpShipOrganizationID"] = job.jmpShipOrganizationID;
			dataRow["jmpSourceMethodID"] = job.jmpSourceMethodID;
			dataRow["jmpSourceRevisionID"] = job.jmpSourceRevisionID;
			dataRow["jmpUnitOfMeasure"] = job.jmpUnitOfMeasure;
			if (job.CustomFields != null && job.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in job.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Job [{job.jmpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Job [{job.jmpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
