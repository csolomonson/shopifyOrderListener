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

public class ERPJobMaterialComponentRepository : APIBaseRepository, IERPJobMaterialComponentRepository, IAPIBaseRepository, IDisposable
{
	public ERPJobMaterialComponentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesJobMaterialComponentExist(Guid jobMaterialComponentId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmtUniqueID|C", jobMaterialComponentId);
		base.selectList.Add("jmtUniqueID");
		return Task.FromResult(GetAsObject("JobMaterialComponents", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPJobMaterialComponentInformationDto>> GetAllJobMaterialComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPJobMaterialComponentInformationDto> collection = new List<ERPJobMaterialComponentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[27]
		{
			"jmtAdditionalQuantity", "jmtCreatedBy", "jmtCreatedDate", "jmtDescription", "jmtUniqueID", "jmtClosed", "jmtPullAllFromStock", "jmtReceivedComplete", "jmtJobAssemblyID", "jmtJobID",
			"jmtJobMaterialID", "jmtMaterialQuantity", "jmtParentQuantity", "jmtPartBinID", "jmtPartID", "jmtPartRevisionID", "jmtPartWarehouseLocationID", "jmtQuantityAllocated", "jmtQuantityPerParent", "jmtQuantityReceived",
			"jmtQuantityToInspect", "jmtQuantityToReturn", "jmtRowVersion", "jmtScrapQuantityReceived", "jmtJobMaterialComponentID", "jmtUnitOfMeasure", "jmtWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("JobMaterialComponents");
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
		using (DataTable dataTable = GetAsDataTable("JobMaterialComponents", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPJobMaterialComponentInformationDto eRPJobMaterialComponentInformationDto = new ERPJobMaterialComponentInformationDto();
				eRPJobMaterialComponentInformationDto.jmtAdditionalQuantity = dataTable.Rows[i].Field<decimal>("jmtAdditionalQuantity");
				eRPJobMaterialComponentInformationDto.jmtCreatedBy = dataTable.Rows[i].Field<string>("jmtCreatedBy");
				eRPJobMaterialComponentInformationDto.jmtCreatedDate = dataTable.Rows[i].Field<DateTime?>("jmtCreatedDate");
				eRPJobMaterialComponentInformationDto.jmtDescription = dataTable.Rows[i].Field<string>("jmtDescription");
				eRPJobMaterialComponentInformationDto.jmtUniqueID = dataTable.Rows[i].Field<Guid>("jmtUniqueID");
				eRPJobMaterialComponentInformationDto.jmtClosed = dataTable.Rows[i].Field<bool>("jmtClosed");
				eRPJobMaterialComponentInformationDto.jmtPullAllFromStock = dataTable.Rows[i].Field<bool>("jmtPullAllFromStock");
				eRPJobMaterialComponentInformationDto.jmtReceivedComplete = dataTable.Rows[i].Field<bool>("jmtReceivedComplete");
				eRPJobMaterialComponentInformationDto.jmtJobAssemblyID = dataTable.Rows[i].Field<int>("jmtJobAssemblyID");
				eRPJobMaterialComponentInformationDto.jmtJobID = dataTable.Rows[i].Field<string>("jmtJobID");
				eRPJobMaterialComponentInformationDto.jmtJobMaterialID = dataTable.Rows[i].Field<int>("jmtJobMaterialID");
				eRPJobMaterialComponentInformationDto.jmtMaterialQuantity = dataTable.Rows[i].Field<decimal>("jmtMaterialQuantity");
				eRPJobMaterialComponentInformationDto.jmtParentQuantity = dataTable.Rows[i].Field<decimal>("jmtParentQuantity");
				eRPJobMaterialComponentInformationDto.jmtPartBinID = dataTable.Rows[i].Field<string>("jmtPartBinID");
				eRPJobMaterialComponentInformationDto.jmtPartID = dataTable.Rows[i].Field<string>("jmtPartID");
				eRPJobMaterialComponentInformationDto.jmtPartRevisionID = dataTable.Rows[i].Field<string>("jmtPartRevisionID");
				eRPJobMaterialComponentInformationDto.jmtPartWarehouseLocationID = dataTable.Rows[i].Field<string>("jmtPartWarehouseLocationID");
				eRPJobMaterialComponentInformationDto.jmtQuantityAllocated = dataTable.Rows[i].Field<decimal>("jmtQuantityAllocated");
				eRPJobMaterialComponentInformationDto.jmtQuantityPerParent = dataTable.Rows[i].Field<decimal>("jmtQuantityPerParent");
				eRPJobMaterialComponentInformationDto.jmtQuantityReceived = dataTable.Rows[i].Field<decimal>("jmtQuantityReceived");
				eRPJobMaterialComponentInformationDto.jmtQuantityToInspect = dataTable.Rows[i].Field<decimal>("jmtQuantityToInspect");
				eRPJobMaterialComponentInformationDto.jmtQuantityToReturn = dataTable.Rows[i].Field<decimal>("jmtQuantityToReturn");
				eRPJobMaterialComponentInformationDto.jmtRowVersion = dataTable.Rows[i].Field<byte[]>("jmtRowVersion");
				eRPJobMaterialComponentInformationDto.jmtScrapQuantityReceived = dataTable.Rows[i].Field<decimal>("jmtScrapQuantityReceived");
				eRPJobMaterialComponentInformationDto.jmtJobMaterialComponentID = dataTable.Rows[i].Field<int>("jmtJobMaterialComponentID");
				eRPJobMaterialComponentInformationDto.jmtUnitOfMeasure = dataTable.Rows[i].Field<string>("jmtUnitOfMeasure");
				eRPJobMaterialComponentInformationDto.jmtWeight = dataTable.Rows[i].Field<decimal>("jmtWeight");
				eRPJobMaterialComponentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPJobMaterialComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPJobMaterialComponentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPJobMaterialComponentInformationDto> GetJobMaterialComponent(Guid jobMaterialComponentId)
	{
		ERPJobMaterialComponentInformationDto eRPJobMaterialComponentInformationDto = new ERPJobMaterialComponentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[27]
		{
			"jmtAdditionalQuantity", "jmtCreatedBy", "jmtCreatedDate", "jmtDescription", "jmtUniqueID", "jmtClosed", "jmtPullAllFromStock", "jmtReceivedComplete", "jmtJobAssemblyID", "jmtJobID",
			"jmtJobMaterialID", "jmtMaterialQuantity", "jmtParentQuantity", "jmtPartBinID", "jmtPartID", "jmtPartRevisionID", "jmtPartWarehouseLocationID", "jmtQuantityAllocated", "jmtQuantityPerParent", "jmtQuantityReceived",
			"jmtQuantityToInspect", "jmtQuantityToReturn", "jmtRowVersion", "jmtScrapQuantityReceived", "jmtJobMaterialComponentID", "jmtUnitOfMeasure", "jmtWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("jmtUniqueID|C", jobMaterialComponentId);
		AddCustomFieldsToSelectList("JobMaterialComponents");
		using (DataTable dataTable = GetAsDataTable("JobMaterialComponents", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPJobMaterialComponentInformationDto);
			}
			eRPJobMaterialComponentInformationDto.jmtAdditionalQuantity = dataTable.Rows[0].Field<decimal>("jmtAdditionalQuantity");
			eRPJobMaterialComponentInformationDto.jmtCreatedBy = dataTable.Rows[0].Field<string>("jmtCreatedBy");
			eRPJobMaterialComponentInformationDto.jmtCreatedDate = dataTable.Rows[0].Field<DateTime?>("jmtCreatedDate");
			eRPJobMaterialComponentInformationDto.jmtDescription = dataTable.Rows[0].Field<string>("jmtDescription");
			eRPJobMaterialComponentInformationDto.jmtUniqueID = dataTable.Rows[0].Field<Guid>("jmtUniqueID");
			eRPJobMaterialComponentInformationDto.jmtClosed = dataTable.Rows[0].Field<bool>("jmtClosed");
			eRPJobMaterialComponentInformationDto.jmtPullAllFromStock = dataTable.Rows[0].Field<bool>("jmtPullAllFromStock");
			eRPJobMaterialComponentInformationDto.jmtReceivedComplete = dataTable.Rows[0].Field<bool>("jmtReceivedComplete");
			eRPJobMaterialComponentInformationDto.jmtJobAssemblyID = dataTable.Rows[0].Field<int>("jmtJobAssemblyID");
			eRPJobMaterialComponentInformationDto.jmtJobID = dataTable.Rows[0].Field<string>("jmtJobID");
			eRPJobMaterialComponentInformationDto.jmtJobMaterialID = dataTable.Rows[0].Field<int>("jmtJobMaterialID");
			eRPJobMaterialComponentInformationDto.jmtMaterialQuantity = dataTable.Rows[0].Field<decimal>("jmtMaterialQuantity");
			eRPJobMaterialComponentInformationDto.jmtParentQuantity = dataTable.Rows[0].Field<decimal>("jmtParentQuantity");
			eRPJobMaterialComponentInformationDto.jmtPartBinID = dataTable.Rows[0].Field<string>("jmtPartBinID");
			eRPJobMaterialComponentInformationDto.jmtPartID = dataTable.Rows[0].Field<string>("jmtPartID");
			eRPJobMaterialComponentInformationDto.jmtPartRevisionID = dataTable.Rows[0].Field<string>("jmtPartRevisionID");
			eRPJobMaterialComponentInformationDto.jmtPartWarehouseLocationID = dataTable.Rows[0].Field<string>("jmtPartWarehouseLocationID");
			eRPJobMaterialComponentInformationDto.jmtQuantityAllocated = dataTable.Rows[0].Field<decimal>("jmtQuantityAllocated");
			eRPJobMaterialComponentInformationDto.jmtQuantityPerParent = dataTable.Rows[0].Field<decimal>("jmtQuantityPerParent");
			eRPJobMaterialComponentInformationDto.jmtQuantityReceived = dataTable.Rows[0].Field<decimal>("jmtQuantityReceived");
			eRPJobMaterialComponentInformationDto.jmtQuantityToInspect = dataTable.Rows[0].Field<decimal>("jmtQuantityToInspect");
			eRPJobMaterialComponentInformationDto.jmtQuantityToReturn = dataTable.Rows[0].Field<decimal>("jmtQuantityToReturn");
			eRPJobMaterialComponentInformationDto.jmtRowVersion = dataTable.Rows[0].Field<byte[]>("jmtRowVersion");
			eRPJobMaterialComponentInformationDto.jmtScrapQuantityReceived = dataTable.Rows[0].Field<decimal>("jmtScrapQuantityReceived");
			eRPJobMaterialComponentInformationDto.jmtJobMaterialComponentID = dataTable.Rows[0].Field<int>("jmtJobMaterialComponentID");
			eRPJobMaterialComponentInformationDto.jmtUnitOfMeasure = dataTable.Rows[0].Field<string>("jmtUnitOfMeasure");
			eRPJobMaterialComponentInformationDto.jmtWeight = dataTable.Rows[0].Field<decimal>("jmtWeight");
			eRPJobMaterialComponentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPJobMaterialComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPJobMaterialComponentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveJobMaterialComponent(ERPJobMaterialComponentDto jobMaterialComponent)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM JobMaterialComponents WHERE jmtUniqueID = " + M1Util.ConvertToLinq(jobMaterialComponent.jmtUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["jmtJobID"] = jobMaterialComponent.jmtJobID.ToUpper();
				dataRow["jmtJobAssemblyID"] = jobMaterialComponent.jmtJobAssemblyID;
				dataRow["jmtJobMaterialID"] = jobMaterialComponent.jmtJobMaterialID;
				dataRow["jmtJobMaterialComponentID"] = jobMaterialComponent.jmtJobMaterialComponentID;
				jobMaterialComponent.jmtUniqueID = ((jobMaterialComponent.jmtUniqueID == Guid.Empty) ? Guid.NewGuid() : jobMaterialComponent.jmtUniqueID);
				dataRow["jmtUniqueID"] = jobMaterialComponent.jmtUniqueID;
				dataRow["jmtCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["jmtCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The JobMaterialComponent could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (jobMaterialComponent.jmtRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the JobMaterialComponent is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["jmtRowVersion"], jobMaterialComponent.jmtRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the JobMaterialComponent has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the JobMaterialComponent again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["jmtAdditionalQuantity"] = jobMaterialComponent.jmtAdditionalQuantity;
			dataRow["jmtDescription"] = jobMaterialComponent.jmtDescription;
			dataRow["jmtClosed"] = jobMaterialComponent.jmtClosed;
			dataRow["jmtPullAllFromStock"] = jobMaterialComponent.jmtPullAllFromStock;
			dataRow["jmtReceivedComplete"] = jobMaterialComponent.jmtReceivedComplete;
			dataRow["jmtMaterialQuantity"] = jobMaterialComponent.jmtMaterialQuantity;
			dataRow["jmtParentQuantity"] = jobMaterialComponent.jmtParentQuantity;
			dataRow["jmtPartBinID"] = jobMaterialComponent.jmtPartBinID;
			dataRow["jmtPartID"] = jobMaterialComponent.jmtPartID;
			dataRow["jmtPartRevisionID"] = jobMaterialComponent.jmtPartRevisionID;
			dataRow["jmtPartWarehouseLocationID"] = jobMaterialComponent.jmtPartWarehouseLocationID;
			dataRow["jmtQuantityAllocated"] = jobMaterialComponent.jmtQuantityAllocated;
			dataRow["jmtQuantityPerParent"] = jobMaterialComponent.jmtQuantityPerParent;
			dataRow["jmtQuantityReceived"] = jobMaterialComponent.jmtQuantityReceived;
			dataRow["jmtQuantityToInspect"] = jobMaterialComponent.jmtQuantityToInspect;
			dataRow["jmtQuantityToReturn"] = jobMaterialComponent.jmtQuantityToReturn;
			dataRow["jmtScrapQuantityReceived"] = jobMaterialComponent.jmtScrapQuantityReceived;
			dataRow["jmtUnitOfMeasure"] = jobMaterialComponent.jmtUnitOfMeasure;
			dataRow["jmtWeight"] = jobMaterialComponent.jmtWeight;
			if (jobMaterialComponent.CustomFields != null && jobMaterialComponent.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in jobMaterialComponent.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the JobMaterialComponent [{jobMaterialComponent.jmtUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the JobMaterialComponent [{jobMaterialComponent.jmtUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
