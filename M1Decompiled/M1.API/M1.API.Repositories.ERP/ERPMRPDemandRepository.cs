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

public class ERPMRPDemandRepository : APIBaseRepository, IERPMRPDemandRepository, IAPIBaseRepository, IDisposable
{
	public ERPMRPDemandRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesMRPDemandExist(Guid mRPDemandId)
	{
		InitializeParameterLists();
		base.filterList.Add("mrrUniqueID|C", mRPDemandId);
		base.selectList.Add("mrrUniqueID");
		return Task.FromResult(GetAsObject("MRPDemands", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPMRPDemandInformationDto>> GetAllMRPDemands(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPMRPDemandInformationDto> collection = new List<ERPMRPDemandInformationDto>();
		InitializeParameterLists();
		string[] array = new string[28]
		{
			"mrrCreatedBy", "mrrCreatedDate", "mrrCustomerOrganizationID", "mrrDemandID", "mrrDemandQuantity", "mrrDueDate", "mrrUniqueID", "mrrJobAssemblyID", "mrrJobID", "mrrJobMaterialID",
			"mrrLineID", "mrrOriginalQuantity", "mrrPartBinID", "mrrPartID", "mrrPartPlantID", "mrrPartRevisionID", "mrrPartWarehouseLocationID", "mrrQuantityReceived", "mrrQuantityShipped", "mrrRowVersion",
			"mrrSalesOrderDeliveryID", "mrrSalesOrderID", "mrrSalesOrderLineID", "mrrSessionID", "mrrShipLocationID", "mrrShipOrganizationID", "mrrSource", "mrrType"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("MRPDemands");
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
		using (DataTable dataTable = GetAsDataTable("MRPDemands", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPMRPDemandInformationDto eRPMRPDemandInformationDto = new ERPMRPDemandInformationDto();
				eRPMRPDemandInformationDto.mrrCreatedBy = dataTable.Rows[i].Field<string>("mrrCreatedBy");
				eRPMRPDemandInformationDto.mrrCreatedDate = dataTable.Rows[i].Field<DateTime?>("mrrCreatedDate");
				eRPMRPDemandInformationDto.mrrCustomerOrganizationID = dataTable.Rows[i].Field<string>("mrrCustomerOrganizationID");
				eRPMRPDemandInformationDto.mrrDemandID = dataTable.Rows[i].Field<int>("mrrDemandID");
				eRPMRPDemandInformationDto.mrrDemandQuantity = dataTable.Rows[i].Field<decimal>("mrrDemandQuantity");
				eRPMRPDemandInformationDto.mrrDueDate = dataTable.Rows[i].Field<DateTime?>("mrrDueDate");
				eRPMRPDemandInformationDto.mrrUniqueID = dataTable.Rows[i].Field<Guid>("mrrUniqueID");
				eRPMRPDemandInformationDto.mrrJobAssemblyID = dataTable.Rows[i].Field<int>("mrrJobAssemblyID");
				eRPMRPDemandInformationDto.mrrJobID = dataTable.Rows[i].Field<string>("mrrJobID");
				eRPMRPDemandInformationDto.mrrJobMaterialID = dataTable.Rows[i].Field<int>("mrrJobMaterialID");
				eRPMRPDemandInformationDto.mrrLineID = dataTable.Rows[i].Field<int>("mrrLineID");
				eRPMRPDemandInformationDto.mrrOriginalQuantity = dataTable.Rows[i].Field<decimal>("mrrOriginalQuantity");
				eRPMRPDemandInformationDto.mrrPartBinID = dataTable.Rows[i].Field<string>("mrrPartBinID");
				eRPMRPDemandInformationDto.mrrPartID = dataTable.Rows[i].Field<string>("mrrPartID");
				eRPMRPDemandInformationDto.mrrPartPlantID = dataTable.Rows[i].Field<string>("mrrPartPlantID");
				eRPMRPDemandInformationDto.mrrPartRevisionID = dataTable.Rows[i].Field<string>("mrrPartRevisionID");
				eRPMRPDemandInformationDto.mrrPartWarehouseLocationID = dataTable.Rows[i].Field<string>("mrrPartWarehouseLocationID");
				eRPMRPDemandInformationDto.mrrQuantityReceived = dataTable.Rows[i].Field<decimal>("mrrQuantityReceived");
				eRPMRPDemandInformationDto.mrrQuantityShipped = dataTable.Rows[i].Field<decimal>("mrrQuantityShipped");
				eRPMRPDemandInformationDto.mrrRowVersion = dataTable.Rows[i].Field<byte[]>("mrrRowVersion");
				eRPMRPDemandInformationDto.mrrSalesOrderDeliveryID = dataTable.Rows[i].Field<short>("mrrSalesOrderDeliveryID");
				eRPMRPDemandInformationDto.mrrSalesOrderID = dataTable.Rows[i].Field<string>("mrrSalesOrderID");
				eRPMRPDemandInformationDto.mrrSalesOrderLineID = dataTable.Rows[i].Field<short>("mrrSalesOrderLineID");
				eRPMRPDemandInformationDto.mrrSessionID = dataTable.Rows[i].Field<string>("mrrSessionID");
				eRPMRPDemandInformationDto.mrrShipLocationID = dataTable.Rows[i].Field<string>("mrrShipLocationID");
				eRPMRPDemandInformationDto.mrrShipOrganizationID = dataTable.Rows[i].Field<string>("mrrShipOrganizationID");
				eRPMRPDemandInformationDto.mrrSource = dataTable.Rows[i].Field<string>("mrrSource");
				eRPMRPDemandInformationDto.mrrType = dataTable.Rows[i].Field<string>("mrrType");
				eRPMRPDemandInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPMRPDemandInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPMRPDemandInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPMRPDemandInformationDto> GetMRPDemand(Guid mRPDemandId)
	{
		ERPMRPDemandInformationDto eRPMRPDemandInformationDto = new ERPMRPDemandInformationDto();
		InitializeParameterLists();
		string[] collection = new string[28]
		{
			"mrrCreatedBy", "mrrCreatedDate", "mrrCustomerOrganizationID", "mrrDemandID", "mrrDemandQuantity", "mrrDueDate", "mrrUniqueID", "mrrJobAssemblyID", "mrrJobID", "mrrJobMaterialID",
			"mrrLineID", "mrrOriginalQuantity", "mrrPartBinID", "mrrPartID", "mrrPartPlantID", "mrrPartRevisionID", "mrrPartWarehouseLocationID", "mrrQuantityReceived", "mrrQuantityShipped", "mrrRowVersion",
			"mrrSalesOrderDeliveryID", "mrrSalesOrderID", "mrrSalesOrderLineID", "mrrSessionID", "mrrShipLocationID", "mrrShipOrganizationID", "mrrSource", "mrrType"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("mrrUniqueID|C", mRPDemandId);
		AddCustomFieldsToSelectList("MRPDemands");
		using (DataTable dataTable = GetAsDataTable("MRPDemands", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPMRPDemandInformationDto);
			}
			eRPMRPDemandInformationDto.mrrCreatedBy = dataTable.Rows[0].Field<string>("mrrCreatedBy");
			eRPMRPDemandInformationDto.mrrCreatedDate = dataTable.Rows[0].Field<DateTime?>("mrrCreatedDate");
			eRPMRPDemandInformationDto.mrrCustomerOrganizationID = dataTable.Rows[0].Field<string>("mrrCustomerOrganizationID");
			eRPMRPDemandInformationDto.mrrDemandID = dataTable.Rows[0].Field<int>("mrrDemandID");
			eRPMRPDemandInformationDto.mrrDemandQuantity = dataTable.Rows[0].Field<decimal>("mrrDemandQuantity");
			eRPMRPDemandInformationDto.mrrDueDate = dataTable.Rows[0].Field<DateTime?>("mrrDueDate");
			eRPMRPDemandInformationDto.mrrUniqueID = dataTable.Rows[0].Field<Guid>("mrrUniqueID");
			eRPMRPDemandInformationDto.mrrJobAssemblyID = dataTable.Rows[0].Field<int>("mrrJobAssemblyID");
			eRPMRPDemandInformationDto.mrrJobID = dataTable.Rows[0].Field<string>("mrrJobID");
			eRPMRPDemandInformationDto.mrrJobMaterialID = dataTable.Rows[0].Field<int>("mrrJobMaterialID");
			eRPMRPDemandInformationDto.mrrLineID = dataTable.Rows[0].Field<int>("mrrLineID");
			eRPMRPDemandInformationDto.mrrOriginalQuantity = dataTable.Rows[0].Field<decimal>("mrrOriginalQuantity");
			eRPMRPDemandInformationDto.mrrPartBinID = dataTable.Rows[0].Field<string>("mrrPartBinID");
			eRPMRPDemandInformationDto.mrrPartID = dataTable.Rows[0].Field<string>("mrrPartID");
			eRPMRPDemandInformationDto.mrrPartPlantID = dataTable.Rows[0].Field<string>("mrrPartPlantID");
			eRPMRPDemandInformationDto.mrrPartRevisionID = dataTable.Rows[0].Field<string>("mrrPartRevisionID");
			eRPMRPDemandInformationDto.mrrPartWarehouseLocationID = dataTable.Rows[0].Field<string>("mrrPartWarehouseLocationID");
			eRPMRPDemandInformationDto.mrrQuantityReceived = dataTable.Rows[0].Field<decimal>("mrrQuantityReceived");
			eRPMRPDemandInformationDto.mrrQuantityShipped = dataTable.Rows[0].Field<decimal>("mrrQuantityShipped");
			eRPMRPDemandInformationDto.mrrRowVersion = dataTable.Rows[0].Field<byte[]>("mrrRowVersion");
			eRPMRPDemandInformationDto.mrrSalesOrderDeliveryID = dataTable.Rows[0].Field<short>("mrrSalesOrderDeliveryID");
			eRPMRPDemandInformationDto.mrrSalesOrderID = dataTable.Rows[0].Field<string>("mrrSalesOrderID");
			eRPMRPDemandInformationDto.mrrSalesOrderLineID = dataTable.Rows[0].Field<short>("mrrSalesOrderLineID");
			eRPMRPDemandInformationDto.mrrSessionID = dataTable.Rows[0].Field<string>("mrrSessionID");
			eRPMRPDemandInformationDto.mrrShipLocationID = dataTable.Rows[0].Field<string>("mrrShipLocationID");
			eRPMRPDemandInformationDto.mrrShipOrganizationID = dataTable.Rows[0].Field<string>("mrrShipOrganizationID");
			eRPMRPDemandInformationDto.mrrSource = dataTable.Rows[0].Field<string>("mrrSource");
			eRPMRPDemandInformationDto.mrrType = dataTable.Rows[0].Field<string>("mrrType");
			eRPMRPDemandInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPMRPDemandInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPMRPDemandInformationDto);
	}

	public Task<APIValidationInfoDto> SaveMRPDemand(ERPMRPDemandDto mRPDemand)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM MRPDemands WHERE mrrUniqueID = " + M1Util.ConvertToLinq(mRPDemand.mrrUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["mrrSessionID"] = mRPDemand.mrrSessionID.ToUpper();
				dataRow["mrrLineID"] = mRPDemand.mrrLineID;
				dataRow["mrrDemandID"] = mRPDemand.mrrDemandID;
				mRPDemand.mrrUniqueID = ((mRPDemand.mrrUniqueID == Guid.Empty) ? Guid.NewGuid() : mRPDemand.mrrUniqueID);
				dataRow["mrrUniqueID"] = mRPDemand.mrrUniqueID;
				dataRow["mrrCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["mrrCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The MRPDemand could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (mRPDemand.mrrRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the MRPDemand is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["mrrRowVersion"], mRPDemand.mrrRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the MRPDemand has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the MRPDemand again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["mrrCustomerOrganizationID"] = mRPDemand.mrrCustomerOrganizationID;
			dataRow["mrrDemandQuantity"] = mRPDemand.mrrDemandQuantity;
			DataRow dataRow2 = dataRow;
			DateTime? mrrDueDate = mRPDemand.mrrDueDate;
			dataRow2["mrrDueDate"] = (mrrDueDate.HasValue ? ((object)mrrDueDate.GetValueOrDefault()) : dataRow["mrrDueDate"]);
			dataRow["mrrJobAssemblyID"] = mRPDemand.mrrJobAssemblyID;
			dataRow["mrrJobID"] = mRPDemand.mrrJobID;
			dataRow["mrrJobMaterialID"] = mRPDemand.mrrJobMaterialID;
			dataRow["mrrOriginalQuantity"] = mRPDemand.mrrOriginalQuantity;
			dataRow["mrrPartBinID"] = mRPDemand.mrrPartBinID;
			dataRow["mrrPartID"] = mRPDemand.mrrPartID;
			dataRow["mrrPartPlantID"] = mRPDemand.mrrPartPlantID;
			dataRow["mrrPartRevisionID"] = mRPDemand.mrrPartRevisionID;
			dataRow["mrrPartWarehouseLocationID"] = mRPDemand.mrrPartWarehouseLocationID;
			dataRow["mrrQuantityReceived"] = mRPDemand.mrrQuantityReceived;
			dataRow["mrrQuantityShipped"] = mRPDemand.mrrQuantityShipped;
			dataRow["mrrSalesOrderDeliveryID"] = mRPDemand.mrrSalesOrderDeliveryID;
			dataRow["mrrSalesOrderID"] = mRPDemand.mrrSalesOrderID;
			dataRow["mrrSalesOrderLineID"] = mRPDemand.mrrSalesOrderLineID;
			dataRow["mrrShipLocationID"] = mRPDemand.mrrShipLocationID;
			dataRow["mrrShipOrganizationID"] = mRPDemand.mrrShipOrganizationID;
			dataRow["mrrSource"] = mRPDemand.mrrSource;
			dataRow["mrrType"] = mRPDemand.mrrType;
			if (mRPDemand.CustomFields != null && mRPDemand.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in mRPDemand.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the MRPDemand [{mRPDemand.mrrUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the MRPDemand [{mRPDemand.mrrUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
