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

public class ERPMRPJobDetailRepository : APIBaseRepository, IERPMRPJobDetailRepository, IAPIBaseRepository, IDisposable
{
	public ERPMRPJobDetailRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesMRPJobDetailExist(Guid mRPJobDetailId)
	{
		InitializeParameterLists();
		base.filterList.Add("mrjUniqueID|C", mRPJobDetailId);
		base.selectList.Add("mrjUniqueID");
		return Task.FromResult(GetAsObject("MRPJobDetails", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPMRPJobDetailInformationDto>> GetAllMRPJobDetails(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPMRPJobDetailInformationDto> collection = new List<ERPMRPJobDetailInformationDto>();
		InitializeParameterLists();
		string[] array = new string[31]
		{
			"mrjCreatedBy", "mrjCreatedDate", "mrjCustomerOrganizationID", "mrjUniqueID", "mrjInventoryQuantity", "mrjCompleted", "mrjConsolidated", "mrjDataMissing", "mrjDirectLink", "mrjExistingJob",
			"mrjFirm", "mrjGetPartMethod", "mrjIndirectLink", "mrjJobAssemblyID", "mrjJobDetailID", "mrjJobID", "mrjLineID", "mrjOrderQuantity", "mrjPartBinID", "mrjPartID",
			"mrjPartPlantID", "mrjPartRevisionID", "mrjPartWarehouseLocationID", "mrjProductionDueDate", "mrjRowVersion", "mrjSalesOrderDeliveryID", "mrjSalesOrderID", "mrjSalesOrderLineID", "mrjSessionID", "mrjShipLocationID",
			"mrjShipOrganizationID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("MRPJobDetails");
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
		using (DataTable dataTable = GetAsDataTable("MRPJobDetails", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPMRPJobDetailInformationDto eRPMRPJobDetailInformationDto = new ERPMRPJobDetailInformationDto();
				eRPMRPJobDetailInformationDto.mrjCreatedBy = dataTable.Rows[i].Field<string>("mrjCreatedBy");
				eRPMRPJobDetailInformationDto.mrjCreatedDate = dataTable.Rows[i].Field<DateTime?>("mrjCreatedDate");
				eRPMRPJobDetailInformationDto.mrjCustomerOrganizationID = dataTable.Rows[i].Field<string>("mrjCustomerOrganizationID");
				eRPMRPJobDetailInformationDto.mrjUniqueID = dataTable.Rows[i].Field<Guid>("mrjUniqueID");
				eRPMRPJobDetailInformationDto.mrjInventoryQuantity = dataTable.Rows[i].Field<decimal>("mrjInventoryQuantity");
				eRPMRPJobDetailInformationDto.mrjCompleted = dataTable.Rows[i].Field<bool>("mrjCompleted");
				eRPMRPJobDetailInformationDto.mrjConsolidated = dataTable.Rows[i].Field<bool>("mrjConsolidated");
				eRPMRPJobDetailInformationDto.mrjDataMissing = dataTable.Rows[i].Field<bool>("mrjDataMissing");
				eRPMRPJobDetailInformationDto.mrjDirectLink = dataTable.Rows[i].Field<bool>("mrjDirectLink");
				eRPMRPJobDetailInformationDto.mrjExistingJob = dataTable.Rows[i].Field<bool>("mrjExistingJob");
				eRPMRPJobDetailInformationDto.mrjFirm = dataTable.Rows[i].Field<bool>("mrjFirm");
				eRPMRPJobDetailInformationDto.mrjGetPartMethod = dataTable.Rows[i].Field<bool>("mrjGetPartMethod");
				eRPMRPJobDetailInformationDto.mrjIndirectLink = dataTable.Rows[i].Field<bool>("mrjIndirectLink");
				eRPMRPJobDetailInformationDto.mrjJobAssemblyID = dataTable.Rows[i].Field<int>("mrjJobAssemblyID");
				eRPMRPJobDetailInformationDto.mrjJobDetailID = dataTable.Rows[i].Field<int>("mrjJobDetailID");
				eRPMRPJobDetailInformationDto.mrjJobID = dataTable.Rows[i].Field<string>("mrjJobID");
				eRPMRPJobDetailInformationDto.mrjLineID = dataTable.Rows[i].Field<int>("mrjLineID");
				eRPMRPJobDetailInformationDto.mrjOrderQuantity = dataTable.Rows[i].Field<decimal>("mrjOrderQuantity");
				eRPMRPJobDetailInformationDto.mrjPartBinID = dataTable.Rows[i].Field<string>("mrjPartBinID");
				eRPMRPJobDetailInformationDto.mrjPartID = dataTable.Rows[i].Field<string>("mrjPartID");
				eRPMRPJobDetailInformationDto.mrjPartPlantID = dataTable.Rows[i].Field<string>("mrjPartPlantID");
				eRPMRPJobDetailInformationDto.mrjPartRevisionID = dataTable.Rows[i].Field<string>("mrjPartRevisionID");
				eRPMRPJobDetailInformationDto.mrjPartWarehouseLocationID = dataTable.Rows[i].Field<string>("mrjPartWarehouseLocationID");
				eRPMRPJobDetailInformationDto.mrjProductionDueDate = dataTable.Rows[i].Field<DateTime?>("mrjProductionDueDate");
				eRPMRPJobDetailInformationDto.mrjRowVersion = dataTable.Rows[i].Field<byte[]>("mrjRowVersion");
				eRPMRPJobDetailInformationDto.mrjSalesOrderDeliveryID = dataTable.Rows[i].Field<short>("mrjSalesOrderDeliveryID");
				eRPMRPJobDetailInformationDto.mrjSalesOrderID = dataTable.Rows[i].Field<string>("mrjSalesOrderID");
				eRPMRPJobDetailInformationDto.mrjSalesOrderLineID = dataTable.Rows[i].Field<short>("mrjSalesOrderLineID");
				eRPMRPJobDetailInformationDto.mrjSessionID = dataTable.Rows[i].Field<string>("mrjSessionID");
				eRPMRPJobDetailInformationDto.mrjShipLocationID = dataTable.Rows[i].Field<string>("mrjShipLocationID");
				eRPMRPJobDetailInformationDto.mrjShipOrganizationID = dataTable.Rows[i].Field<string>("mrjShipOrganizationID");
				eRPMRPJobDetailInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPMRPJobDetailInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPMRPJobDetailInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPMRPJobDetailInformationDto> GetMRPJobDetail(Guid mRPJobDetailId)
	{
		ERPMRPJobDetailInformationDto eRPMRPJobDetailInformationDto = new ERPMRPJobDetailInformationDto();
		InitializeParameterLists();
		string[] collection = new string[31]
		{
			"mrjCreatedBy", "mrjCreatedDate", "mrjCustomerOrganizationID", "mrjUniqueID", "mrjInventoryQuantity", "mrjCompleted", "mrjConsolidated", "mrjDataMissing", "mrjDirectLink", "mrjExistingJob",
			"mrjFirm", "mrjGetPartMethod", "mrjIndirectLink", "mrjJobAssemblyID", "mrjJobDetailID", "mrjJobID", "mrjLineID", "mrjOrderQuantity", "mrjPartBinID", "mrjPartID",
			"mrjPartPlantID", "mrjPartRevisionID", "mrjPartWarehouseLocationID", "mrjProductionDueDate", "mrjRowVersion", "mrjSalesOrderDeliveryID", "mrjSalesOrderID", "mrjSalesOrderLineID", "mrjSessionID", "mrjShipLocationID",
			"mrjShipOrganizationID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("mrjUniqueID|C", mRPJobDetailId);
		AddCustomFieldsToSelectList("MRPJobDetails");
		using (DataTable dataTable = GetAsDataTable("MRPJobDetails", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPMRPJobDetailInformationDto);
			}
			eRPMRPJobDetailInformationDto.mrjCreatedBy = dataTable.Rows[0].Field<string>("mrjCreatedBy");
			eRPMRPJobDetailInformationDto.mrjCreatedDate = dataTable.Rows[0].Field<DateTime?>("mrjCreatedDate");
			eRPMRPJobDetailInformationDto.mrjCustomerOrganizationID = dataTable.Rows[0].Field<string>("mrjCustomerOrganizationID");
			eRPMRPJobDetailInformationDto.mrjUniqueID = dataTable.Rows[0].Field<Guid>("mrjUniqueID");
			eRPMRPJobDetailInformationDto.mrjInventoryQuantity = dataTable.Rows[0].Field<decimal>("mrjInventoryQuantity");
			eRPMRPJobDetailInformationDto.mrjCompleted = dataTable.Rows[0].Field<bool>("mrjCompleted");
			eRPMRPJobDetailInformationDto.mrjConsolidated = dataTable.Rows[0].Field<bool>("mrjConsolidated");
			eRPMRPJobDetailInformationDto.mrjDataMissing = dataTable.Rows[0].Field<bool>("mrjDataMissing");
			eRPMRPJobDetailInformationDto.mrjDirectLink = dataTable.Rows[0].Field<bool>("mrjDirectLink");
			eRPMRPJobDetailInformationDto.mrjExistingJob = dataTable.Rows[0].Field<bool>("mrjExistingJob");
			eRPMRPJobDetailInformationDto.mrjFirm = dataTable.Rows[0].Field<bool>("mrjFirm");
			eRPMRPJobDetailInformationDto.mrjGetPartMethod = dataTable.Rows[0].Field<bool>("mrjGetPartMethod");
			eRPMRPJobDetailInformationDto.mrjIndirectLink = dataTable.Rows[0].Field<bool>("mrjIndirectLink");
			eRPMRPJobDetailInformationDto.mrjJobAssemblyID = dataTable.Rows[0].Field<int>("mrjJobAssemblyID");
			eRPMRPJobDetailInformationDto.mrjJobDetailID = dataTable.Rows[0].Field<int>("mrjJobDetailID");
			eRPMRPJobDetailInformationDto.mrjJobID = dataTable.Rows[0].Field<string>("mrjJobID");
			eRPMRPJobDetailInformationDto.mrjLineID = dataTable.Rows[0].Field<int>("mrjLineID");
			eRPMRPJobDetailInformationDto.mrjOrderQuantity = dataTable.Rows[0].Field<decimal>("mrjOrderQuantity");
			eRPMRPJobDetailInformationDto.mrjPartBinID = dataTable.Rows[0].Field<string>("mrjPartBinID");
			eRPMRPJobDetailInformationDto.mrjPartID = dataTable.Rows[0].Field<string>("mrjPartID");
			eRPMRPJobDetailInformationDto.mrjPartPlantID = dataTable.Rows[0].Field<string>("mrjPartPlantID");
			eRPMRPJobDetailInformationDto.mrjPartRevisionID = dataTable.Rows[0].Field<string>("mrjPartRevisionID");
			eRPMRPJobDetailInformationDto.mrjPartWarehouseLocationID = dataTable.Rows[0].Field<string>("mrjPartWarehouseLocationID");
			eRPMRPJobDetailInformationDto.mrjProductionDueDate = dataTable.Rows[0].Field<DateTime?>("mrjProductionDueDate");
			eRPMRPJobDetailInformationDto.mrjRowVersion = dataTable.Rows[0].Field<byte[]>("mrjRowVersion");
			eRPMRPJobDetailInformationDto.mrjSalesOrderDeliveryID = dataTable.Rows[0].Field<short>("mrjSalesOrderDeliveryID");
			eRPMRPJobDetailInformationDto.mrjSalesOrderID = dataTable.Rows[0].Field<string>("mrjSalesOrderID");
			eRPMRPJobDetailInformationDto.mrjSalesOrderLineID = dataTable.Rows[0].Field<short>("mrjSalesOrderLineID");
			eRPMRPJobDetailInformationDto.mrjSessionID = dataTable.Rows[0].Field<string>("mrjSessionID");
			eRPMRPJobDetailInformationDto.mrjShipLocationID = dataTable.Rows[0].Field<string>("mrjShipLocationID");
			eRPMRPJobDetailInformationDto.mrjShipOrganizationID = dataTable.Rows[0].Field<string>("mrjShipOrganizationID");
			eRPMRPJobDetailInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPMRPJobDetailInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPMRPJobDetailInformationDto);
	}

	public Task<APIValidationInfoDto> SaveMRPJobDetail(ERPMRPJobDetailDto mRPJobDetail)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM MRPJobDetails WHERE mrjUniqueID = " + M1Util.ConvertToLinq(mRPJobDetail.mrjUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["mrjSessionID"] = mRPJobDetail.mrjSessionID.ToUpper();
				dataRow["mrjLineID"] = mRPJobDetail.mrjLineID;
				dataRow["mrjJobDetailID"] = mRPJobDetail.mrjJobDetailID;
				mRPJobDetail.mrjUniqueID = ((mRPJobDetail.mrjUniqueID == Guid.Empty) ? Guid.NewGuid() : mRPJobDetail.mrjUniqueID);
				dataRow["mrjUniqueID"] = mRPJobDetail.mrjUniqueID;
				dataRow["mrjCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["mrjCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The MRPJobDetail could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (mRPJobDetail.mrjRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the MRPJobDetail is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["mrjRowVersion"], mRPJobDetail.mrjRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the MRPJobDetail has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the MRPJobDetail again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["mrjCustomerOrganizationID"] = mRPJobDetail.mrjCustomerOrganizationID;
			dataRow["mrjInventoryQuantity"] = mRPJobDetail.mrjInventoryQuantity;
			dataRow["mrjCompleted"] = mRPJobDetail.mrjCompleted;
			dataRow["mrjConsolidated"] = mRPJobDetail.mrjConsolidated;
			dataRow["mrjDataMissing"] = mRPJobDetail.mrjDataMissing;
			dataRow["mrjDirectLink"] = mRPJobDetail.mrjDirectLink;
			dataRow["mrjExistingJob"] = mRPJobDetail.mrjExistingJob;
			dataRow["mrjFirm"] = mRPJobDetail.mrjFirm;
			dataRow["mrjGetPartMethod"] = mRPJobDetail.mrjGetPartMethod;
			dataRow["mrjIndirectLink"] = mRPJobDetail.mrjIndirectLink;
			dataRow["mrjJobAssemblyID"] = mRPJobDetail.mrjJobAssemblyID;
			dataRow["mrjJobID"] = mRPJobDetail.mrjJobID;
			dataRow["mrjOrderQuantity"] = mRPJobDetail.mrjOrderQuantity;
			dataRow["mrjPartBinID"] = mRPJobDetail.mrjPartBinID;
			dataRow["mrjPartID"] = mRPJobDetail.mrjPartID;
			dataRow["mrjPartPlantID"] = mRPJobDetail.mrjPartPlantID;
			dataRow["mrjPartRevisionID"] = mRPJobDetail.mrjPartRevisionID;
			dataRow["mrjPartWarehouseLocationID"] = mRPJobDetail.mrjPartWarehouseLocationID;
			DataRow dataRow2 = dataRow;
			DateTime? mrjProductionDueDate = mRPJobDetail.mrjProductionDueDate;
			dataRow2["mrjProductionDueDate"] = (mrjProductionDueDate.HasValue ? ((object)mrjProductionDueDate.GetValueOrDefault()) : dataRow["mrjProductionDueDate"]);
			dataRow["mrjSalesOrderDeliveryID"] = mRPJobDetail.mrjSalesOrderDeliveryID;
			dataRow["mrjSalesOrderID"] = mRPJobDetail.mrjSalesOrderID;
			dataRow["mrjSalesOrderLineID"] = mRPJobDetail.mrjSalesOrderLineID;
			dataRow["mrjShipLocationID"] = mRPJobDetail.mrjShipLocationID;
			dataRow["mrjShipOrganizationID"] = mRPJobDetail.mrjShipOrganizationID;
			if (mRPJobDetail.CustomFields != null && mRPJobDetail.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in mRPJobDetail.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the MRPJobDetail [{mRPJobDetail.mrjUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the MRPJobDetail [{mRPJobDetail.mrjUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
