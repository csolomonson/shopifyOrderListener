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

public class ERPMRPSupplyRepository : APIBaseRepository, IERPMRPSupplyRepository, IAPIBaseRepository, IDisposable
{
	public ERPMRPSupplyRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesMRPSupplyExist(Guid mRPSupplyId)
	{
		InitializeParameterLists();
		base.filterList.Add("mrsUniqueID|C", mRPSupplyId);
		base.selectList.Add("mrsUniqueID");
		return Task.FromResult(GetAsObject("MRPSupply", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPMRPSupplyInformationDto>> GetAllMRPSupply(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPMRPSupplyInformationDto> collection = new List<ERPMRPSupplyInformationDto>();
		InitializeParameterLists();
		string[] array = new string[19]
		{
			"mrsCreatedBy", "mrsCreatedDate", "mrsCustomerOrganizationID", "mrsDueDate", "mrsUniqueID", "mrsJobAssemblyID", "mrsJobID", "mrsLineID", "mrsPartBinID", "mrsPartID",
			"mrsPartRevisionID", "mrsPartWarehouseLocationID", "mrsQuantityReceived", "mrsQuantityShipped", "mrsRowVersion", "mrsSessionID", "mrsSource", "mrsSupplyID", "mrsType"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("MRPSupply");
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
		using (DataTable dataTable = GetAsDataTable("MRPSupply", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPMRPSupplyInformationDto eRPMRPSupplyInformationDto = new ERPMRPSupplyInformationDto();
				eRPMRPSupplyInformationDto.mrsCreatedBy = dataTable.Rows[i].Field<string>("mrsCreatedBy");
				eRPMRPSupplyInformationDto.mrsCreatedDate = dataTable.Rows[i].Field<DateTime?>("mrsCreatedDate");
				eRPMRPSupplyInformationDto.mrsCustomerOrganizationID = dataTable.Rows[i].Field<string>("mrsCustomerOrganizationID");
				eRPMRPSupplyInformationDto.mrsDueDate = dataTable.Rows[i].Field<DateTime?>("mrsDueDate");
				eRPMRPSupplyInformationDto.mrsUniqueID = dataTable.Rows[i].Field<Guid>("mrsUniqueID");
				eRPMRPSupplyInformationDto.mrsJobAssemblyID = dataTable.Rows[i].Field<int>("mrsJobAssemblyID");
				eRPMRPSupplyInformationDto.mrsJobID = dataTable.Rows[i].Field<string>("mrsJobID");
				eRPMRPSupplyInformationDto.mrsLineID = dataTable.Rows[i].Field<int>("mrsLineID");
				eRPMRPSupplyInformationDto.mrsPartBinID = dataTable.Rows[i].Field<string>("mrsPartBinID");
				eRPMRPSupplyInformationDto.mrsPartID = dataTable.Rows[i].Field<string>("mrsPartID");
				eRPMRPSupplyInformationDto.mrsPartRevisionID = dataTable.Rows[i].Field<string>("mrsPartRevisionID");
				eRPMRPSupplyInformationDto.mrsPartWarehouseLocationID = dataTable.Rows[i].Field<string>("mrsPartWarehouseLocationID");
				eRPMRPSupplyInformationDto.mrsQuantityReceived = dataTable.Rows[i].Field<decimal>("mrsQuantityReceived");
				eRPMRPSupplyInformationDto.mrsQuantityShipped = dataTable.Rows[i].Field<decimal>("mrsQuantityShipped");
				eRPMRPSupplyInformationDto.mrsRowVersion = dataTable.Rows[i].Field<byte[]>("mrsRowVersion");
				eRPMRPSupplyInformationDto.mrsSessionID = dataTable.Rows[i].Field<string>("mrsSessionID");
				eRPMRPSupplyInformationDto.mrsSource = dataTable.Rows[i].Field<string>("mrsSource");
				eRPMRPSupplyInformationDto.mrsSupplyID = dataTable.Rows[i].Field<int>("mrsSupplyID");
				eRPMRPSupplyInformationDto.mrsType = dataTable.Rows[i].Field<string>("mrsType");
				eRPMRPSupplyInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPMRPSupplyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPMRPSupplyInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPMRPSupplyInformationDto> GetMRPSupply(Guid mRPSupplyId)
	{
		ERPMRPSupplyInformationDto eRPMRPSupplyInformationDto = new ERPMRPSupplyInformationDto();
		InitializeParameterLists();
		string[] collection = new string[19]
		{
			"mrsCreatedBy", "mrsCreatedDate", "mrsCustomerOrganizationID", "mrsDueDate", "mrsUniqueID", "mrsJobAssemblyID", "mrsJobID", "mrsLineID", "mrsPartBinID", "mrsPartID",
			"mrsPartRevisionID", "mrsPartWarehouseLocationID", "mrsQuantityReceived", "mrsQuantityShipped", "mrsRowVersion", "mrsSessionID", "mrsSource", "mrsSupplyID", "mrsType"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("mrsUniqueID|C", mRPSupplyId);
		AddCustomFieldsToSelectList("MRPSupply");
		using (DataTable dataTable = GetAsDataTable("MRPSupply", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPMRPSupplyInformationDto);
			}
			eRPMRPSupplyInformationDto.mrsCreatedBy = dataTable.Rows[0].Field<string>("mrsCreatedBy");
			eRPMRPSupplyInformationDto.mrsCreatedDate = dataTable.Rows[0].Field<DateTime?>("mrsCreatedDate");
			eRPMRPSupplyInformationDto.mrsCustomerOrganizationID = dataTable.Rows[0].Field<string>("mrsCustomerOrganizationID");
			eRPMRPSupplyInformationDto.mrsDueDate = dataTable.Rows[0].Field<DateTime?>("mrsDueDate");
			eRPMRPSupplyInformationDto.mrsUniqueID = dataTable.Rows[0].Field<Guid>("mrsUniqueID");
			eRPMRPSupplyInformationDto.mrsJobAssemblyID = dataTable.Rows[0].Field<int>("mrsJobAssemblyID");
			eRPMRPSupplyInformationDto.mrsJobID = dataTable.Rows[0].Field<string>("mrsJobID");
			eRPMRPSupplyInformationDto.mrsLineID = dataTable.Rows[0].Field<int>("mrsLineID");
			eRPMRPSupplyInformationDto.mrsPartBinID = dataTable.Rows[0].Field<string>("mrsPartBinID");
			eRPMRPSupplyInformationDto.mrsPartID = dataTable.Rows[0].Field<string>("mrsPartID");
			eRPMRPSupplyInformationDto.mrsPartRevisionID = dataTable.Rows[0].Field<string>("mrsPartRevisionID");
			eRPMRPSupplyInformationDto.mrsPartWarehouseLocationID = dataTable.Rows[0].Field<string>("mrsPartWarehouseLocationID");
			eRPMRPSupplyInformationDto.mrsQuantityReceived = dataTable.Rows[0].Field<decimal>("mrsQuantityReceived");
			eRPMRPSupplyInformationDto.mrsQuantityShipped = dataTable.Rows[0].Field<decimal>("mrsQuantityShipped");
			eRPMRPSupplyInformationDto.mrsRowVersion = dataTable.Rows[0].Field<byte[]>("mrsRowVersion");
			eRPMRPSupplyInformationDto.mrsSessionID = dataTable.Rows[0].Field<string>("mrsSessionID");
			eRPMRPSupplyInformationDto.mrsSource = dataTable.Rows[0].Field<string>("mrsSource");
			eRPMRPSupplyInformationDto.mrsSupplyID = dataTable.Rows[0].Field<int>("mrsSupplyID");
			eRPMRPSupplyInformationDto.mrsType = dataTable.Rows[0].Field<string>("mrsType");
			eRPMRPSupplyInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPMRPSupplyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPMRPSupplyInformationDto);
	}

	public Task<APIValidationInfoDto> SaveMRPSupply(ERPMRPSupplyDto mRPSupply)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM MRPSupply WHERE mrsUniqueID = " + M1Util.ConvertToLinq(mRPSupply.mrsUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["mrsSessionID"] = mRPSupply.mrsSessionID.ToUpper();
				dataRow["mrsLineID"] = mRPSupply.mrsLineID;
				dataRow["mrsSupplyID"] = mRPSupply.mrsSupplyID;
				mRPSupply.mrsUniqueID = ((mRPSupply.mrsUniqueID == Guid.Empty) ? Guid.NewGuid() : mRPSupply.mrsUniqueID);
				dataRow["mrsUniqueID"] = mRPSupply.mrsUniqueID;
				dataRow["mrsCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["mrsCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The MRPSupply could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (mRPSupply.mrsRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the MRPSupply is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["mrsRowVersion"], mRPSupply.mrsRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the MRPSupply has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the MRPSupply again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["mrsCustomerOrganizationID"] = mRPSupply.mrsCustomerOrganizationID;
			DataRow dataRow2 = dataRow;
			DateTime? mrsDueDate = mRPSupply.mrsDueDate;
			dataRow2["mrsDueDate"] = (mrsDueDate.HasValue ? ((object)mrsDueDate.GetValueOrDefault()) : dataRow["mrsDueDate"]);
			dataRow["mrsJobAssemblyID"] = mRPSupply.mrsJobAssemblyID;
			dataRow["mrsJobID"] = mRPSupply.mrsJobID;
			dataRow["mrsPartBinID"] = mRPSupply.mrsPartBinID;
			dataRow["mrsPartID"] = mRPSupply.mrsPartID;
			dataRow["mrsPartRevisionID"] = mRPSupply.mrsPartRevisionID;
			dataRow["mrsPartWarehouseLocationID"] = mRPSupply.mrsPartWarehouseLocationID;
			dataRow["mrsQuantityReceived"] = mRPSupply.mrsQuantityReceived;
			dataRow["mrsQuantityShipped"] = mRPSupply.mrsQuantityShipped;
			dataRow["mrsSource"] = mRPSupply.mrsSource;
			dataRow["mrsType"] = mRPSupply.mrsType;
			if (mRPSupply.CustomFields != null && mRPSupply.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in mRPSupply.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the MRPSupply [{mRPSupply.mrsUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the MRPSupply [{mRPSupply.mrsUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
