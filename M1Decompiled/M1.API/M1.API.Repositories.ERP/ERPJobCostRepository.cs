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

public class ERPJobCostRepository : APIBaseRepository, IERPJobCostRepository, IAPIBaseRepository, IDisposable
{
	public ERPJobCostRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesJobCostExist(Guid jobCostId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmcUniqueID|C", jobCostId);
		base.selectList.Add("jmcUniqueID");
		return Task.FromResult(GetAsObject("JobCosts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPJobCostInformationDto>> GetAllJobCosts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPJobCostInformationDto> collection = new List<ERPJobCostInformationDto>();
		InitializeParameterLists();
		string[] array = new string[29]
		{
			"jmcApInvoiceID", "jmcApInvoiceLineID", "jmcCostSequence", "jmcCreatedBy", "jmcCreatedDate", "jmcUniqueID", "jmcHeatLot", "jmcJobAssemblyID", "jmcJobID", "jmcJobMaterialComponentID",
			"jmcJobMaterialID", "jmcJobOperationID", "jmcJobSequence", "jmcJobType", "jmcPartDescription", "jmcPartID", "jmcPartRevisionID", "jmcQuantityReceived", "jmcReceiptComponentID", "jmcReceiptID",
			"jmcReceiptLineID", "jmcReceivedUnitOfMeasure", "jmcReference", "jmcRowVersion", "jmcSource", "jmcSupplierOrganizationID", "jmcTotalCogsCost", "jmcTotalCost", "jmcTransactionDate"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("JobCosts");
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
		using (DataTable dataTable = GetAsDataTable("JobCosts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPJobCostInformationDto eRPJobCostInformationDto = new ERPJobCostInformationDto();
				eRPJobCostInformationDto.jmcApInvoiceID = dataTable.Rows[i].Field<string>("jmcApInvoiceID");
				eRPJobCostInformationDto.jmcApInvoiceLineID = dataTable.Rows[i].Field<short>("jmcApInvoiceLineID");
				eRPJobCostInformationDto.jmcCostSequence = dataTable.Rows[i].Field<int>("jmcCostSequence");
				eRPJobCostInformationDto.jmcCreatedBy = dataTable.Rows[i].Field<string>("jmcCreatedBy");
				eRPJobCostInformationDto.jmcCreatedDate = dataTable.Rows[i].Field<DateTime?>("jmcCreatedDate");
				eRPJobCostInformationDto.jmcUniqueID = dataTable.Rows[i].Field<Guid>("jmcUniqueID");
				eRPJobCostInformationDto.jmcHeatLot = dataTable.Rows[i].Field<string>("jmcHeatLot");
				eRPJobCostInformationDto.jmcJobAssemblyID = dataTable.Rows[i].Field<int>("jmcJobAssemblyID");
				eRPJobCostInformationDto.jmcJobID = dataTable.Rows[i].Field<string>("jmcJobID");
				eRPJobCostInformationDto.jmcJobMaterialComponentID = dataTable.Rows[i].Field<int>("jmcJobMaterialComponentID");
				eRPJobCostInformationDto.jmcJobMaterialID = dataTable.Rows[i].Field<int>("jmcJobMaterialID");
				eRPJobCostInformationDto.jmcJobOperationID = dataTable.Rows[i].Field<int>("jmcJobOperationID");
				eRPJobCostInformationDto.jmcJobSequence = dataTable.Rows[i].Field<int>("jmcJobSequence");
				eRPJobCostInformationDto.jmcJobType = dataTable.Rows[i].Field<byte>("jmcJobType");
				eRPJobCostInformationDto.jmcPartDescription = dataTable.Rows[i].Field<string>("jmcPartDescription");
				eRPJobCostInformationDto.jmcPartID = dataTable.Rows[i].Field<string>("jmcPartID");
				eRPJobCostInformationDto.jmcPartRevisionID = dataTable.Rows[i].Field<string>("jmcPartRevisionID");
				eRPJobCostInformationDto.jmcQuantityReceived = dataTable.Rows[i].Field<decimal>("jmcQuantityReceived");
				eRPJobCostInformationDto.jmcReceiptComponentID = dataTable.Rows[i].Field<short>("jmcReceiptComponentID");
				eRPJobCostInformationDto.jmcReceiptID = dataTable.Rows[i].Field<string>("jmcReceiptID");
				eRPJobCostInformationDto.jmcReceiptLineID = dataTable.Rows[i].Field<short>("jmcReceiptLineID");
				eRPJobCostInformationDto.jmcReceivedUnitOfMeasure = dataTable.Rows[i].Field<string>("jmcReceivedUnitOfMeasure");
				eRPJobCostInformationDto.jmcReference = dataTable.Rows[i].Field<string>("jmcReference");
				eRPJobCostInformationDto.jmcRowVersion = dataTable.Rows[i].Field<byte[]>("jmcRowVersion");
				eRPJobCostInformationDto.jmcSource = dataTable.Rows[i].Field<byte>("jmcSource");
				eRPJobCostInformationDto.jmcSupplierOrganizationID = dataTable.Rows[i].Field<string>("jmcSupplierOrganizationID");
				eRPJobCostInformationDto.jmcTotalCogsCost = dataTable.Rows[i].Field<decimal>("jmcTotalCogsCost");
				eRPJobCostInformationDto.jmcTotalCost = dataTable.Rows[i].Field<decimal>("jmcTotalCost");
				eRPJobCostInformationDto.jmcTransactionDate = dataTable.Rows[i].Field<DateTime?>("jmcTransactionDate");
				eRPJobCostInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPJobCostInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPJobCostInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPJobCostInformationDto> GetJobCost(Guid jobCostId)
	{
		ERPJobCostInformationDto eRPJobCostInformationDto = new ERPJobCostInformationDto();
		InitializeParameterLists();
		string[] collection = new string[29]
		{
			"jmcApInvoiceID", "jmcApInvoiceLineID", "jmcCostSequence", "jmcCreatedBy", "jmcCreatedDate", "jmcUniqueID", "jmcHeatLot", "jmcJobAssemblyID", "jmcJobID", "jmcJobMaterialComponentID",
			"jmcJobMaterialID", "jmcJobOperationID", "jmcJobSequence", "jmcJobType", "jmcPartDescription", "jmcPartID", "jmcPartRevisionID", "jmcQuantityReceived", "jmcReceiptComponentID", "jmcReceiptID",
			"jmcReceiptLineID", "jmcReceivedUnitOfMeasure", "jmcReference", "jmcRowVersion", "jmcSource", "jmcSupplierOrganizationID", "jmcTotalCogsCost", "jmcTotalCost", "jmcTransactionDate"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("jmcUniqueID|C", jobCostId);
		AddCustomFieldsToSelectList("JobCosts");
		using (DataTable dataTable = GetAsDataTable("JobCosts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPJobCostInformationDto);
			}
			eRPJobCostInformationDto.jmcApInvoiceID = dataTable.Rows[0].Field<string>("jmcApInvoiceID");
			eRPJobCostInformationDto.jmcApInvoiceLineID = dataTable.Rows[0].Field<short>("jmcApInvoiceLineID");
			eRPJobCostInformationDto.jmcCostSequence = dataTable.Rows[0].Field<int>("jmcCostSequence");
			eRPJobCostInformationDto.jmcCreatedBy = dataTable.Rows[0].Field<string>("jmcCreatedBy");
			eRPJobCostInformationDto.jmcCreatedDate = dataTable.Rows[0].Field<DateTime?>("jmcCreatedDate");
			eRPJobCostInformationDto.jmcUniqueID = dataTable.Rows[0].Field<Guid>("jmcUniqueID");
			eRPJobCostInformationDto.jmcHeatLot = dataTable.Rows[0].Field<string>("jmcHeatLot");
			eRPJobCostInformationDto.jmcJobAssemblyID = dataTable.Rows[0].Field<int>("jmcJobAssemblyID");
			eRPJobCostInformationDto.jmcJobID = dataTable.Rows[0].Field<string>("jmcJobID");
			eRPJobCostInformationDto.jmcJobMaterialComponentID = dataTable.Rows[0].Field<int>("jmcJobMaterialComponentID");
			eRPJobCostInformationDto.jmcJobMaterialID = dataTable.Rows[0].Field<int>("jmcJobMaterialID");
			eRPJobCostInformationDto.jmcJobOperationID = dataTable.Rows[0].Field<int>("jmcJobOperationID");
			eRPJobCostInformationDto.jmcJobSequence = dataTable.Rows[0].Field<int>("jmcJobSequence");
			eRPJobCostInformationDto.jmcJobType = dataTable.Rows[0].Field<byte>("jmcJobType");
			eRPJobCostInformationDto.jmcPartDescription = dataTable.Rows[0].Field<string>("jmcPartDescription");
			eRPJobCostInformationDto.jmcPartID = dataTable.Rows[0].Field<string>("jmcPartID");
			eRPJobCostInformationDto.jmcPartRevisionID = dataTable.Rows[0].Field<string>("jmcPartRevisionID");
			eRPJobCostInformationDto.jmcQuantityReceived = dataTable.Rows[0].Field<decimal>("jmcQuantityReceived");
			eRPJobCostInformationDto.jmcReceiptComponentID = dataTable.Rows[0].Field<short>("jmcReceiptComponentID");
			eRPJobCostInformationDto.jmcReceiptID = dataTable.Rows[0].Field<string>("jmcReceiptID");
			eRPJobCostInformationDto.jmcReceiptLineID = dataTable.Rows[0].Field<short>("jmcReceiptLineID");
			eRPJobCostInformationDto.jmcReceivedUnitOfMeasure = dataTable.Rows[0].Field<string>("jmcReceivedUnitOfMeasure");
			eRPJobCostInformationDto.jmcReference = dataTable.Rows[0].Field<string>("jmcReference");
			eRPJobCostInformationDto.jmcRowVersion = dataTable.Rows[0].Field<byte[]>("jmcRowVersion");
			eRPJobCostInformationDto.jmcSource = dataTable.Rows[0].Field<byte>("jmcSource");
			eRPJobCostInformationDto.jmcSupplierOrganizationID = dataTable.Rows[0].Field<string>("jmcSupplierOrganizationID");
			eRPJobCostInformationDto.jmcTotalCogsCost = dataTable.Rows[0].Field<decimal>("jmcTotalCogsCost");
			eRPJobCostInformationDto.jmcTotalCost = dataTable.Rows[0].Field<decimal>("jmcTotalCost");
			eRPJobCostInformationDto.jmcTransactionDate = dataTable.Rows[0].Field<DateTime?>("jmcTransactionDate");
			eRPJobCostInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPJobCostInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPJobCostInformationDto);
	}

	public Task<APIValidationInfoDto> SaveJobCost(ERPJobCostDto jobCost)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM JobCosts WHERE jmcUniqueID = " + M1Util.ConvertToLinq(jobCost.jmcUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["jmcJobID"] = jobCost.jmcJobID.ToUpper();
				dataRow["jmcJobAssemblyID"] = jobCost.jmcJobAssemblyID;
				dataRow["jmcJobType"] = jobCost.jmcJobType;
				dataRow["jmcJobSequence"] = jobCost.jmcJobSequence;
				dataRow["jmcCostSequence"] = jobCost.jmcCostSequence;
				jobCost.jmcUniqueID = ((jobCost.jmcUniqueID == Guid.Empty) ? Guid.NewGuid() : jobCost.jmcUniqueID);
				dataRow["jmcUniqueID"] = jobCost.jmcUniqueID;
				dataRow["jmcCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["jmcCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The JobCost could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (jobCost.jmcRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the JobCost is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["jmcRowVersion"], jobCost.jmcRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the JobCost has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the JobCost again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["jmcApInvoiceID"] = jobCost.jmcApInvoiceID;
			dataRow["jmcApInvoiceLineID"] = jobCost.jmcApInvoiceLineID;
			dataRow["jmcHeatLot"] = jobCost.jmcHeatLot;
			dataRow["jmcJobMaterialComponentID"] = jobCost.jmcJobMaterialComponentID;
			dataRow["jmcJobMaterialID"] = jobCost.jmcJobMaterialID;
			dataRow["jmcJobOperationID"] = jobCost.jmcJobOperationID;
			dataRow["jmcPartDescription"] = jobCost.jmcPartDescription;
			dataRow["jmcPartID"] = jobCost.jmcPartID;
			dataRow["jmcPartRevisionID"] = jobCost.jmcPartRevisionID;
			dataRow["jmcQuantityReceived"] = jobCost.jmcQuantityReceived;
			dataRow["jmcReceiptComponentID"] = jobCost.jmcReceiptComponentID;
			dataRow["jmcReceiptID"] = jobCost.jmcReceiptID;
			dataRow["jmcReceiptLineID"] = jobCost.jmcReceiptLineID;
			dataRow["jmcReceivedUnitOfMeasure"] = jobCost.jmcReceivedUnitOfMeasure;
			dataRow["jmcReference"] = jobCost.jmcReference;
			dataRow["jmcSource"] = jobCost.jmcSource;
			dataRow["jmcSupplierOrganizationID"] = jobCost.jmcSupplierOrganizationID;
			dataRow["jmcTotalCogsCost"] = jobCost.jmcTotalCogsCost;
			dataRow["jmcTotalCost"] = jobCost.jmcTotalCost;
			DataRow dataRow2 = dataRow;
			DateTime? jmcTransactionDate = jobCost.jmcTransactionDate;
			dataRow2["jmcTransactionDate"] = (jmcTransactionDate.HasValue ? ((object)jmcTransactionDate.GetValueOrDefault()) : dataRow["jmcTransactionDate"]);
			if (jobCost.CustomFields != null && jobCost.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in jobCost.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the JobCost [{jobCost.jmcUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the JobCost [{jobCost.jmcUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
