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

public class ERPJobAssemblyRepository : APIBaseRepository, IERPJobAssemblyRepository, IAPIBaseRepository, IDisposable
{
	public ERPJobAssemblyRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesJobAssemblyExist(Guid jobAssemblyId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmaUniqueID|C", jobAssemblyId);
		base.selectList.Add("jmaUniqueID");
		return Task.FromResult(GetAsObject("JobAssemblies", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPJobAssemblyInformationDto>> GetAllJobAssemblies(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPJobAssemblyInformationDto> collection = new List<ERPJobAssemblyInformationDto>();
		InitializeParameterLists();
		string[] array = new string[54]
		{
			"jmaAssemblyOverlap", "jmaCompletedDate", "jmaCreatedBy", "jmaCreatedDate", "jmaDocuments", "jmaUniqueID", "jmaEstimatedUnitCost", "jmaInventoryQuantity", "jmaClosed", "jmaIssuedComplete",
			"jmaProductionComplete", "jmaPullAllFromStock", "jmaReceivedComplete", "jmaJobID", "jmaLevel", "jmaOrderQuantity", "jmaOverlapDestinationLink", "jmaOverlapOffsetTime", "jmaOverlapOperationID", "jmaOverlapSourceLink",
			"jmaOverlapSourceOperationID", "jmaOverlapType", "jmaParentAssemblyID", "jmaPartBinID", "jmaPartID", "jmaPartLongDescriptionRtf", "jmaPartLongDescriptionText", "jmaPartRevisionID", "jmaPartShortDescription", "jmaPartWareHouseLocationID",
			"jmaProductionNotesRTF", "jmaProductionNotesText", "jmaProductionQuantity", "jmaQuantityCompleted", "jmaQuantityIssued", "jmaQuantityPerParent", "jmaQuantityReceivedToInventory", "jmaQuantityToInspect", "jmaQuantityToMake", "jmaQuantityToPull",
			"jmaQuantityToReturn", "jmaReworkDate", "jmaReworkQuantity", "jmaRowVersion", "jmaScheduledDueDate", "jmaScheduledDueHour", "jmaScheduledStartDate", "jmaScheduledStartHour", "jmaScrapQuantity", "jmaScrapQuantityCompleted",
			"jmaJobAssemblyID", "jmaSourceMethodID", "jmaSourceRevisionID", "jmaUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("JobAssemblies");
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
		using (DataTable dataTable = GetAsDataTable("JobAssemblies", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPJobAssemblyInformationDto eRPJobAssemblyInformationDto = new ERPJobAssemblyInformationDto();
				eRPJobAssemblyInformationDto.jmaAssemblyOverlap = dataTable.Rows[i].Field<byte>("jmaAssemblyOverlap");
				eRPJobAssemblyInformationDto.jmaCompletedDate = dataTable.Rows[i].Field<DateTime?>("jmaCompletedDate");
				eRPJobAssemblyInformationDto.jmaCreatedBy = dataTable.Rows[i].Field<string>("jmaCreatedBy");
				eRPJobAssemblyInformationDto.jmaCreatedDate = dataTable.Rows[i].Field<DateTime?>("jmaCreatedDate");
				eRPJobAssemblyInformationDto.jmaDocuments = dataTable.Rows[i].Field<string>("jmaDocuments");
				eRPJobAssemblyInformationDto.jmaUniqueID = dataTable.Rows[i].Field<Guid>("jmaUniqueID");
				eRPJobAssemblyInformationDto.jmaEstimatedUnitCost = dataTable.Rows[i].Field<decimal>("jmaEstimatedUnitCost");
				eRPJobAssemblyInformationDto.jmaInventoryQuantity = dataTable.Rows[i].Field<decimal>("jmaInventoryQuantity");
				eRPJobAssemblyInformationDto.jmaClosed = dataTable.Rows[i].Field<bool>("jmaClosed");
				eRPJobAssemblyInformationDto.jmaIssuedComplete = dataTable.Rows[i].Field<bool>("jmaIssuedComplete");
				eRPJobAssemblyInformationDto.jmaProductionComplete = dataTable.Rows[i].Field<bool>("jmaProductionComplete");
				eRPJobAssemblyInformationDto.jmaPullAllFromStock = dataTable.Rows[i].Field<bool>("jmaPullAllFromStock");
				eRPJobAssemblyInformationDto.jmaReceivedComplete = dataTable.Rows[i].Field<bool>("jmaReceivedComplete");
				eRPJobAssemblyInformationDto.jmaJobID = dataTable.Rows[i].Field<string>("jmaJobID");
				eRPJobAssemblyInformationDto.jmaLevel = dataTable.Rows[i].Field<short>("jmaLevel");
				eRPJobAssemblyInformationDto.jmaOrderQuantity = dataTable.Rows[i].Field<decimal>("jmaOrderQuantity");
				eRPJobAssemblyInformationDto.jmaOverlapDestinationLink = dataTable.Rows[i].Field<byte>("jmaOverlapDestinationLink");
				eRPJobAssemblyInformationDto.jmaOverlapOffsetTime = dataTable.Rows[i].Field<decimal>("jmaOverlapOffsetTime");
				eRPJobAssemblyInformationDto.jmaOverlapOperationID = dataTable.Rows[i].Field<int>("jmaOverlapOperationID");
				eRPJobAssemblyInformationDto.jmaOverlapSourceLink = dataTable.Rows[i].Field<byte>("jmaOverlapSourceLink");
				eRPJobAssemblyInformationDto.jmaOverlapSourceOperationID = dataTable.Rows[i].Field<int>("jmaOverlapSourceOperationID");
				eRPJobAssemblyInformationDto.jmaOverlapType = dataTable.Rows[i].Field<byte>("jmaOverlapType");
				eRPJobAssemblyInformationDto.jmaParentAssemblyID = dataTable.Rows[i].Field<int>("jmaParentAssemblyID");
				eRPJobAssemblyInformationDto.jmaPartBinID = dataTable.Rows[i].Field<string>("jmaPartBinID");
				eRPJobAssemblyInformationDto.jmaPartID = dataTable.Rows[i].Field<string>("jmaPartID");
				eRPJobAssemblyInformationDto.jmaPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("jmaPartLongDescriptionRtf");
				eRPJobAssemblyInformationDto.jmaPartLongDescriptionText = dataTable.Rows[i].Field<string>("jmaPartLongDescriptionText");
				eRPJobAssemblyInformationDto.jmaPartRevisionID = dataTable.Rows[i].Field<string>("jmaPartRevisionID");
				eRPJobAssemblyInformationDto.jmaPartShortDescription = dataTable.Rows[i].Field<string>("jmaPartShortDescription");
				eRPJobAssemblyInformationDto.jmaPartWareHouseLocationID = dataTable.Rows[i].Field<string>("jmaPartWareHouseLocationID");
				eRPJobAssemblyInformationDto.jmaProductionNotesRTF = dataTable.Rows[i].Field<string>("jmaProductionNotesRTF");
				eRPJobAssemblyInformationDto.jmaProductionNotesText = dataTable.Rows[i].Field<string>("jmaProductionNotesText");
				eRPJobAssemblyInformationDto.jmaProductionQuantity = dataTable.Rows[i].Field<decimal>("jmaProductionQuantity");
				eRPJobAssemblyInformationDto.jmaQuantityCompleted = dataTable.Rows[i].Field<decimal>("jmaQuantityCompleted");
				eRPJobAssemblyInformationDto.jmaQuantityIssued = dataTable.Rows[i].Field<decimal>("jmaQuantityIssued");
				eRPJobAssemblyInformationDto.jmaQuantityPerParent = dataTable.Rows[i].Field<decimal>("jmaQuantityPerParent");
				eRPJobAssemblyInformationDto.jmaQuantityReceivedToInventory = dataTable.Rows[i].Field<decimal>("jmaQuantityReceivedToInventory");
				eRPJobAssemblyInformationDto.jmaQuantityToInspect = dataTable.Rows[i].Field<decimal>("jmaQuantityToInspect");
				eRPJobAssemblyInformationDto.jmaQuantityToMake = dataTable.Rows[i].Field<decimal>("jmaQuantityToMake");
				eRPJobAssemblyInformationDto.jmaQuantityToPull = dataTable.Rows[i].Field<decimal>("jmaQuantityToPull");
				eRPJobAssemblyInformationDto.jmaQuantityToReturn = dataTable.Rows[i].Field<decimal>("jmaQuantityToReturn");
				eRPJobAssemblyInformationDto.jmaReworkDate = dataTable.Rows[i].Field<DateTime?>("jmaReworkDate");
				eRPJobAssemblyInformationDto.jmaReworkQuantity = dataTable.Rows[i].Field<decimal>("jmaReworkQuantity");
				eRPJobAssemblyInformationDto.jmaRowVersion = dataTable.Rows[i].Field<byte[]>("jmaRowVersion");
				eRPJobAssemblyInformationDto.jmaScheduledDueDate = dataTable.Rows[i].Field<DateTime?>("jmaScheduledDueDate");
				eRPJobAssemblyInformationDto.jmaScheduledDueHour = dataTable.Rows[i].Field<decimal>("jmaScheduledDueHour");
				eRPJobAssemblyInformationDto.jmaScheduledStartDate = dataTable.Rows[i].Field<DateTime?>("jmaScheduledStartDate");
				eRPJobAssemblyInformationDto.jmaScheduledStartHour = dataTable.Rows[i].Field<decimal>("jmaScheduledStartHour");
				eRPJobAssemblyInformationDto.jmaScrapQuantity = dataTable.Rows[i].Field<decimal>("jmaScrapQuantity");
				eRPJobAssemblyInformationDto.jmaScrapQuantityCompleted = dataTable.Rows[i].Field<decimal>("jmaScrapQuantityCompleted");
				eRPJobAssemblyInformationDto.jmaJobAssemblyID = dataTable.Rows[i].Field<int>("jmaJobAssemblyID");
				eRPJobAssemblyInformationDto.jmaSourceMethodID = dataTable.Rows[i].Field<string>("jmaSourceMethodID");
				eRPJobAssemblyInformationDto.jmaSourceRevisionID = dataTable.Rows[i].Field<string>("jmaSourceRevisionID");
				eRPJobAssemblyInformationDto.jmaUnitOfMeasure = dataTable.Rows[i].Field<string>("jmaUnitOfMeasure");
				eRPJobAssemblyInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPJobAssemblyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPJobAssemblyInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPJobAssemblyInformationDto> GetJobAssembly(Guid jobAssemblyId)
	{
		ERPJobAssemblyInformationDto eRPJobAssemblyInformationDto = new ERPJobAssemblyInformationDto();
		InitializeParameterLists();
		string[] collection = new string[54]
		{
			"jmaAssemblyOverlap", "jmaCompletedDate", "jmaCreatedBy", "jmaCreatedDate", "jmaDocuments", "jmaUniqueID", "jmaEstimatedUnitCost", "jmaInventoryQuantity", "jmaClosed", "jmaIssuedComplete",
			"jmaProductionComplete", "jmaPullAllFromStock", "jmaReceivedComplete", "jmaJobID", "jmaLevel", "jmaOrderQuantity", "jmaOverlapDestinationLink", "jmaOverlapOffsetTime", "jmaOverlapOperationID", "jmaOverlapSourceLink",
			"jmaOverlapSourceOperationID", "jmaOverlapType", "jmaParentAssemblyID", "jmaPartBinID", "jmaPartID", "jmaPartLongDescriptionRtf", "jmaPartLongDescriptionText", "jmaPartRevisionID", "jmaPartShortDescription", "jmaPartWareHouseLocationID",
			"jmaProductionNotesRTF", "jmaProductionNotesText", "jmaProductionQuantity", "jmaQuantityCompleted", "jmaQuantityIssued", "jmaQuantityPerParent", "jmaQuantityReceivedToInventory", "jmaQuantityToInspect", "jmaQuantityToMake", "jmaQuantityToPull",
			"jmaQuantityToReturn", "jmaReworkDate", "jmaReworkQuantity", "jmaRowVersion", "jmaScheduledDueDate", "jmaScheduledDueHour", "jmaScheduledStartDate", "jmaScheduledStartHour", "jmaScrapQuantity", "jmaScrapQuantityCompleted",
			"jmaJobAssemblyID", "jmaSourceMethodID", "jmaSourceRevisionID", "jmaUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("jmaUniqueID|C", jobAssemblyId);
		AddCustomFieldsToSelectList("JobAssemblies");
		using (DataTable dataTable = GetAsDataTable("JobAssemblies", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPJobAssemblyInformationDto);
			}
			eRPJobAssemblyInformationDto.jmaAssemblyOverlap = dataTable.Rows[0].Field<byte>("jmaAssemblyOverlap");
			eRPJobAssemblyInformationDto.jmaCompletedDate = dataTable.Rows[0].Field<DateTime?>("jmaCompletedDate");
			eRPJobAssemblyInformationDto.jmaCreatedBy = dataTable.Rows[0].Field<string>("jmaCreatedBy");
			eRPJobAssemblyInformationDto.jmaCreatedDate = dataTable.Rows[0].Field<DateTime?>("jmaCreatedDate");
			eRPJobAssemblyInformationDto.jmaDocuments = dataTable.Rows[0].Field<string>("jmaDocuments");
			eRPJobAssemblyInformationDto.jmaUniqueID = dataTable.Rows[0].Field<Guid>("jmaUniqueID");
			eRPJobAssemblyInformationDto.jmaEstimatedUnitCost = dataTable.Rows[0].Field<decimal>("jmaEstimatedUnitCost");
			eRPJobAssemblyInformationDto.jmaInventoryQuantity = dataTable.Rows[0].Field<decimal>("jmaInventoryQuantity");
			eRPJobAssemblyInformationDto.jmaClosed = dataTable.Rows[0].Field<bool>("jmaClosed");
			eRPJobAssemblyInformationDto.jmaIssuedComplete = dataTable.Rows[0].Field<bool>("jmaIssuedComplete");
			eRPJobAssemblyInformationDto.jmaProductionComplete = dataTable.Rows[0].Field<bool>("jmaProductionComplete");
			eRPJobAssemblyInformationDto.jmaPullAllFromStock = dataTable.Rows[0].Field<bool>("jmaPullAllFromStock");
			eRPJobAssemblyInformationDto.jmaReceivedComplete = dataTable.Rows[0].Field<bool>("jmaReceivedComplete");
			eRPJobAssemblyInformationDto.jmaJobID = dataTable.Rows[0].Field<string>("jmaJobID");
			eRPJobAssemblyInformationDto.jmaLevel = dataTable.Rows[0].Field<short>("jmaLevel");
			eRPJobAssemblyInformationDto.jmaOrderQuantity = dataTable.Rows[0].Field<decimal>("jmaOrderQuantity");
			eRPJobAssemblyInformationDto.jmaOverlapDestinationLink = dataTable.Rows[0].Field<byte>("jmaOverlapDestinationLink");
			eRPJobAssemblyInformationDto.jmaOverlapOffsetTime = dataTable.Rows[0].Field<decimal>("jmaOverlapOffsetTime");
			eRPJobAssemblyInformationDto.jmaOverlapOperationID = dataTable.Rows[0].Field<int>("jmaOverlapOperationID");
			eRPJobAssemblyInformationDto.jmaOverlapSourceLink = dataTable.Rows[0].Field<byte>("jmaOverlapSourceLink");
			eRPJobAssemblyInformationDto.jmaOverlapSourceOperationID = dataTable.Rows[0].Field<int>("jmaOverlapSourceOperationID");
			eRPJobAssemblyInformationDto.jmaOverlapType = dataTable.Rows[0].Field<byte>("jmaOverlapType");
			eRPJobAssemblyInformationDto.jmaParentAssemblyID = dataTable.Rows[0].Field<int>("jmaParentAssemblyID");
			eRPJobAssemblyInformationDto.jmaPartBinID = dataTable.Rows[0].Field<string>("jmaPartBinID");
			eRPJobAssemblyInformationDto.jmaPartID = dataTable.Rows[0].Field<string>("jmaPartID");
			eRPJobAssemblyInformationDto.jmaPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("jmaPartLongDescriptionRtf");
			eRPJobAssemblyInformationDto.jmaPartLongDescriptionText = dataTable.Rows[0].Field<string>("jmaPartLongDescriptionText");
			eRPJobAssemblyInformationDto.jmaPartRevisionID = dataTable.Rows[0].Field<string>("jmaPartRevisionID");
			eRPJobAssemblyInformationDto.jmaPartShortDescription = dataTable.Rows[0].Field<string>("jmaPartShortDescription");
			eRPJobAssemblyInformationDto.jmaPartWareHouseLocationID = dataTable.Rows[0].Field<string>("jmaPartWareHouseLocationID");
			eRPJobAssemblyInformationDto.jmaProductionNotesRTF = dataTable.Rows[0].Field<string>("jmaProductionNotesRTF");
			eRPJobAssemblyInformationDto.jmaProductionNotesText = dataTable.Rows[0].Field<string>("jmaProductionNotesText");
			eRPJobAssemblyInformationDto.jmaProductionQuantity = dataTable.Rows[0].Field<decimal>("jmaProductionQuantity");
			eRPJobAssemblyInformationDto.jmaQuantityCompleted = dataTable.Rows[0].Field<decimal>("jmaQuantityCompleted");
			eRPJobAssemblyInformationDto.jmaQuantityIssued = dataTable.Rows[0].Field<decimal>("jmaQuantityIssued");
			eRPJobAssemblyInformationDto.jmaQuantityPerParent = dataTable.Rows[0].Field<decimal>("jmaQuantityPerParent");
			eRPJobAssemblyInformationDto.jmaQuantityReceivedToInventory = dataTable.Rows[0].Field<decimal>("jmaQuantityReceivedToInventory");
			eRPJobAssemblyInformationDto.jmaQuantityToInspect = dataTable.Rows[0].Field<decimal>("jmaQuantityToInspect");
			eRPJobAssemblyInformationDto.jmaQuantityToMake = dataTable.Rows[0].Field<decimal>("jmaQuantityToMake");
			eRPJobAssemblyInformationDto.jmaQuantityToPull = dataTable.Rows[0].Field<decimal>("jmaQuantityToPull");
			eRPJobAssemblyInformationDto.jmaQuantityToReturn = dataTable.Rows[0].Field<decimal>("jmaQuantityToReturn");
			eRPJobAssemblyInformationDto.jmaReworkDate = dataTable.Rows[0].Field<DateTime?>("jmaReworkDate");
			eRPJobAssemblyInformationDto.jmaReworkQuantity = dataTable.Rows[0].Field<decimal>("jmaReworkQuantity");
			eRPJobAssemblyInformationDto.jmaRowVersion = dataTable.Rows[0].Field<byte[]>("jmaRowVersion");
			eRPJobAssemblyInformationDto.jmaScheduledDueDate = dataTable.Rows[0].Field<DateTime?>("jmaScheduledDueDate");
			eRPJobAssemblyInformationDto.jmaScheduledDueHour = dataTable.Rows[0].Field<decimal>("jmaScheduledDueHour");
			eRPJobAssemblyInformationDto.jmaScheduledStartDate = dataTable.Rows[0].Field<DateTime?>("jmaScheduledStartDate");
			eRPJobAssemblyInformationDto.jmaScheduledStartHour = dataTable.Rows[0].Field<decimal>("jmaScheduledStartHour");
			eRPJobAssemblyInformationDto.jmaScrapQuantity = dataTable.Rows[0].Field<decimal>("jmaScrapQuantity");
			eRPJobAssemblyInformationDto.jmaScrapQuantityCompleted = dataTable.Rows[0].Field<decimal>("jmaScrapQuantityCompleted");
			eRPJobAssemblyInformationDto.jmaJobAssemblyID = dataTable.Rows[0].Field<int>("jmaJobAssemblyID");
			eRPJobAssemblyInformationDto.jmaSourceMethodID = dataTable.Rows[0].Field<string>("jmaSourceMethodID");
			eRPJobAssemblyInformationDto.jmaSourceRevisionID = dataTable.Rows[0].Field<string>("jmaSourceRevisionID");
			eRPJobAssemblyInformationDto.jmaUnitOfMeasure = dataTable.Rows[0].Field<string>("jmaUnitOfMeasure");
			eRPJobAssemblyInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPJobAssemblyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPJobAssemblyInformationDto);
	}

	public Task<APIValidationInfoDto> SaveJobAssembly(ERPJobAssemblyDto jobAssembly)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM JobAssemblies WHERE jmaUniqueID = " + M1Util.ConvertToLinq(jobAssembly.jmaUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["jmaJobID"] = jobAssembly.jmaJobID.ToUpper();
				dataRow["jmaJobAssemblyID"] = jobAssembly.jmaJobAssemblyID;
				jobAssembly.jmaUniqueID = ((jobAssembly.jmaUniqueID == Guid.Empty) ? Guid.NewGuid() : jobAssembly.jmaUniqueID);
				dataRow["jmaUniqueID"] = jobAssembly.jmaUniqueID;
				dataRow["jmaCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["jmaCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The JobAssembly could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (jobAssembly.jmaRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the JobAssembly is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["jmaRowVersion"], jobAssembly.jmaRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the JobAssembly has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the JobAssembly again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["jmaAssemblyOverlap"] = jobAssembly.jmaAssemblyOverlap;
			DataRow dataRow2 = dataRow;
			DateTime? jmaCompletedDate = jobAssembly.jmaCompletedDate;
			dataRow2["jmaCompletedDate"] = (jmaCompletedDate.HasValue ? ((object)jmaCompletedDate.GetValueOrDefault()) : dataRow["jmaCompletedDate"]);
			dataRow["jmaDocuments"] = jobAssembly.jmaDocuments ?? dataRow["jmaDocuments"];
			dataRow["jmaEstimatedUnitCost"] = jobAssembly.jmaEstimatedUnitCost;
			dataRow["jmaInventoryQuantity"] = jobAssembly.jmaInventoryQuantity;
			dataRow["jmaClosed"] = jobAssembly.jmaClosed;
			dataRow["jmaIssuedComplete"] = jobAssembly.jmaIssuedComplete;
			dataRow["jmaProductionComplete"] = jobAssembly.jmaProductionComplete;
			dataRow["jmaPullAllFromStock"] = jobAssembly.jmaPullAllFromStock;
			dataRow["jmaReceivedComplete"] = jobAssembly.jmaReceivedComplete;
			dataRow["jmaLevel"] = jobAssembly.jmaLevel;
			dataRow["jmaOrderQuantity"] = jobAssembly.jmaOrderQuantity;
			dataRow["jmaOverlapDestinationLink"] = jobAssembly.jmaOverlapDestinationLink;
			dataRow["jmaOverlapOffsetTime"] = jobAssembly.jmaOverlapOffsetTime;
			dataRow["jmaOverlapOperationID"] = jobAssembly.jmaOverlapOperationID;
			dataRow["jmaOverlapSourceLink"] = jobAssembly.jmaOverlapSourceLink;
			dataRow["jmaOverlapSourceOperationID"] = jobAssembly.jmaOverlapSourceOperationID;
			dataRow["jmaOverlapType"] = jobAssembly.jmaOverlapType;
			dataRow["jmaParentAssemblyID"] = jobAssembly.jmaParentAssemblyID;
			dataRow["jmaPartBinID"] = jobAssembly.jmaPartBinID;
			dataRow["jmaPartID"] = jobAssembly.jmaPartID;
			dataRow["jmaPartLongDescriptionRtf"] = jobAssembly.jmaPartLongDescriptionRtf ?? dataRow["jmaPartLongDescriptionRtf"];
			dataRow["jmaPartLongDescriptionText"] = jobAssembly.jmaPartLongDescriptionText ?? dataRow["jmaPartLongDescriptionText"];
			dataRow["jmaPartRevisionID"] = jobAssembly.jmaPartRevisionID;
			dataRow["jmaPartShortDescription"] = jobAssembly.jmaPartShortDescription;
			dataRow["jmaPartWareHouseLocationID"] = jobAssembly.jmaPartWareHouseLocationID;
			dataRow["jmaProductionNotesRTF"] = jobAssembly.jmaProductionNotesRTF ?? dataRow["jmaProductionNotesRTF"];
			dataRow["jmaProductionNotesText"] = jobAssembly.jmaProductionNotesText ?? dataRow["jmaProductionNotesText"];
			dataRow["jmaProductionQuantity"] = jobAssembly.jmaProductionQuantity;
			dataRow["jmaQuantityCompleted"] = jobAssembly.jmaQuantityCompleted;
			dataRow["jmaQuantityIssued"] = jobAssembly.jmaQuantityIssued;
			dataRow["jmaQuantityPerParent"] = jobAssembly.jmaQuantityPerParent;
			dataRow["jmaQuantityReceivedToInventory"] = jobAssembly.jmaQuantityReceivedToInventory;
			dataRow["jmaQuantityToInspect"] = jobAssembly.jmaQuantityToInspect;
			dataRow["jmaQuantityToMake"] = jobAssembly.jmaQuantityToMake;
			dataRow["jmaQuantityToPull"] = jobAssembly.jmaQuantityToPull;
			dataRow["jmaQuantityToReturn"] = jobAssembly.jmaQuantityToReturn;
			DataRow dataRow3 = dataRow;
			jmaCompletedDate = jobAssembly.jmaReworkDate;
			dataRow3["jmaReworkDate"] = (jmaCompletedDate.HasValue ? ((object)jmaCompletedDate.GetValueOrDefault()) : dataRow["jmaReworkDate"]);
			dataRow["jmaReworkQuantity"] = jobAssembly.jmaReworkQuantity;
			DataRow dataRow4 = dataRow;
			jmaCompletedDate = jobAssembly.jmaScheduledDueDate;
			dataRow4["jmaScheduledDueDate"] = (jmaCompletedDate.HasValue ? ((object)jmaCompletedDate.GetValueOrDefault()) : dataRow["jmaScheduledDueDate"]);
			dataRow["jmaScheduledDueHour"] = jobAssembly.jmaScheduledDueHour;
			DataRow dataRow5 = dataRow;
			jmaCompletedDate = jobAssembly.jmaScheduledStartDate;
			dataRow5["jmaScheduledStartDate"] = (jmaCompletedDate.HasValue ? ((object)jmaCompletedDate.GetValueOrDefault()) : dataRow["jmaScheduledStartDate"]);
			dataRow["jmaScheduledStartHour"] = jobAssembly.jmaScheduledStartHour;
			dataRow["jmaScrapQuantity"] = jobAssembly.jmaScrapQuantity;
			dataRow["jmaScrapQuantityCompleted"] = jobAssembly.jmaScrapQuantityCompleted;
			dataRow["jmaSourceMethodID"] = jobAssembly.jmaSourceMethodID;
			dataRow["jmaSourceRevisionID"] = jobAssembly.jmaSourceRevisionID;
			dataRow["jmaUnitOfMeasure"] = jobAssembly.jmaUnitOfMeasure;
			if (jobAssembly.CustomFields != null && jobAssembly.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in jobAssembly.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the JobAssembly [{jobAssembly.jmaUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the JobAssembly [{jobAssembly.jmaUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
