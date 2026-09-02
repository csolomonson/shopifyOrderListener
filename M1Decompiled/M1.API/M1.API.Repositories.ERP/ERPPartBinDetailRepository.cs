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

public class ERPPartBinDetailRepository : APIBaseRepository, IERPPartBinDetailRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartBinDetailRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartBinDetailExist(Guid partBinDetailId)
	{
		InitializeParameterLists();
		base.filterList.Add("imgUniqueID|C", partBinDetailId);
		base.selectList.Add("imgUniqueID");
		return Task.FromResult(GetAsObject("PartBinDetails", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartBinDetailInformationDto>> GetAllPartBinDetails(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartBinDetailInformationDto> collection = new List<ERPPartBinDetailInformationDto>();
		InitializeParameterLists();
		string[] array = new string[22]
		{
			"imgCreatedBy", "imgCreatedDate", "imgUniqueID", "imgOriginalQuantity", "imgPartBinID", "imgPartID", "imgPartRevisionID", "imgQuantityType", "imgRemainingQuantity", "imgRowVersion",
			"imgPartBinDetailID", "imgSourceTableName", "imgSourceTableUniqueID", "imgTransactionDate", "imgUnitDutyCost", "imgUnitFreightCost", "imgUnitLaborCost", "imgUnitMaterialCost", "imgUnitMiscCost", "imgUnitOverheadCost",
			"imgUnitSubcontractCost", "imgWarehouseID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartBinDetails");
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
		using (DataTable dataTable = GetAsDataTable("PartBinDetails", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartBinDetailInformationDto eRPPartBinDetailInformationDto = new ERPPartBinDetailInformationDto();
				eRPPartBinDetailInformationDto.imgCreatedBy = dataTable.Rows[i].Field<string>("imgCreatedBy");
				eRPPartBinDetailInformationDto.imgCreatedDate = dataTable.Rows[i].Field<DateTime?>("imgCreatedDate");
				eRPPartBinDetailInformationDto.imgUniqueID = dataTable.Rows[i].Field<Guid>("imgUniqueID");
				eRPPartBinDetailInformationDto.imgOriginalQuantity = dataTable.Rows[i].Field<decimal>("imgOriginalQuantity");
				eRPPartBinDetailInformationDto.imgPartBinID = dataTable.Rows[i].Field<string>("imgPartBinID");
				eRPPartBinDetailInformationDto.imgPartID = dataTable.Rows[i].Field<string>("imgPartID");
				eRPPartBinDetailInformationDto.imgPartRevisionID = dataTable.Rows[i].Field<string>("imgPartRevisionID");
				eRPPartBinDetailInformationDto.imgQuantityType = dataTable.Rows[i].Field<byte>("imgQuantityType");
				eRPPartBinDetailInformationDto.imgRemainingQuantity = dataTable.Rows[i].Field<decimal>("imgRemainingQuantity");
				eRPPartBinDetailInformationDto.imgRowVersion = dataTable.Rows[i].Field<byte[]>("imgRowVersion");
				eRPPartBinDetailInformationDto.imgPartBinDetailID = dataTable.Rows[i].Field<int>("imgPartBinDetailID");
				eRPPartBinDetailInformationDto.imgSourceTableName = dataTable.Rows[i].Field<string>("imgSourceTableName");
				eRPPartBinDetailInformationDto.imgSourceTableUniqueID = dataTable.Rows[i].Field<Guid>("imgSourceTableUniqueID");
				eRPPartBinDetailInformationDto.imgTransactionDate = dataTable.Rows[i].Field<DateTime?>("imgTransactionDate");
				eRPPartBinDetailInformationDto.imgUnitDutyCost = dataTable.Rows[i].Field<decimal>("imgUnitDutyCost");
				eRPPartBinDetailInformationDto.imgUnitFreightCost = dataTable.Rows[i].Field<decimal>("imgUnitFreightCost");
				eRPPartBinDetailInformationDto.imgUnitLaborCost = dataTable.Rows[i].Field<decimal>("imgUnitLaborCost");
				eRPPartBinDetailInformationDto.imgUnitMaterialCost = dataTable.Rows[i].Field<decimal>("imgUnitMaterialCost");
				eRPPartBinDetailInformationDto.imgUnitMiscCost = dataTable.Rows[i].Field<decimal>("imgUnitMiscCost");
				eRPPartBinDetailInformationDto.imgUnitOverheadCost = dataTable.Rows[i].Field<decimal>("imgUnitOverheadCost");
				eRPPartBinDetailInformationDto.imgUnitSubcontractCost = dataTable.Rows[i].Field<decimal>("imgUnitSubcontractCost");
				eRPPartBinDetailInformationDto.imgWarehouseID = dataTable.Rows[i].Field<string>("imgWarehouseID");
				eRPPartBinDetailInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartBinDetailInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartBinDetailInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartBinDetailInformationDto> GetPartBinDetail(Guid partBinDetailId)
	{
		ERPPartBinDetailInformationDto eRPPartBinDetailInformationDto = new ERPPartBinDetailInformationDto();
		InitializeParameterLists();
		string[] collection = new string[22]
		{
			"imgCreatedBy", "imgCreatedDate", "imgUniqueID", "imgOriginalQuantity", "imgPartBinID", "imgPartID", "imgPartRevisionID", "imgQuantityType", "imgRemainingQuantity", "imgRowVersion",
			"imgPartBinDetailID", "imgSourceTableName", "imgSourceTableUniqueID", "imgTransactionDate", "imgUnitDutyCost", "imgUnitFreightCost", "imgUnitLaborCost", "imgUnitMaterialCost", "imgUnitMiscCost", "imgUnitOverheadCost",
			"imgUnitSubcontractCost", "imgWarehouseID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imgUniqueID|C", partBinDetailId);
		AddCustomFieldsToSelectList("PartBinDetails");
		using (DataTable dataTable = GetAsDataTable("PartBinDetails", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartBinDetailInformationDto);
			}
			eRPPartBinDetailInformationDto.imgCreatedBy = dataTable.Rows[0].Field<string>("imgCreatedBy");
			eRPPartBinDetailInformationDto.imgCreatedDate = dataTable.Rows[0].Field<DateTime?>("imgCreatedDate");
			eRPPartBinDetailInformationDto.imgUniqueID = dataTable.Rows[0].Field<Guid>("imgUniqueID");
			eRPPartBinDetailInformationDto.imgOriginalQuantity = dataTable.Rows[0].Field<decimal>("imgOriginalQuantity");
			eRPPartBinDetailInformationDto.imgPartBinID = dataTable.Rows[0].Field<string>("imgPartBinID");
			eRPPartBinDetailInformationDto.imgPartID = dataTable.Rows[0].Field<string>("imgPartID");
			eRPPartBinDetailInformationDto.imgPartRevisionID = dataTable.Rows[0].Field<string>("imgPartRevisionID");
			eRPPartBinDetailInformationDto.imgQuantityType = dataTable.Rows[0].Field<byte>("imgQuantityType");
			eRPPartBinDetailInformationDto.imgRemainingQuantity = dataTable.Rows[0].Field<decimal>("imgRemainingQuantity");
			eRPPartBinDetailInformationDto.imgRowVersion = dataTable.Rows[0].Field<byte[]>("imgRowVersion");
			eRPPartBinDetailInformationDto.imgPartBinDetailID = dataTable.Rows[0].Field<int>("imgPartBinDetailID");
			eRPPartBinDetailInformationDto.imgSourceTableName = dataTable.Rows[0].Field<string>("imgSourceTableName");
			eRPPartBinDetailInformationDto.imgSourceTableUniqueID = dataTable.Rows[0].Field<Guid>("imgSourceTableUniqueID");
			eRPPartBinDetailInformationDto.imgTransactionDate = dataTable.Rows[0].Field<DateTime?>("imgTransactionDate");
			eRPPartBinDetailInformationDto.imgUnitDutyCost = dataTable.Rows[0].Field<decimal>("imgUnitDutyCost");
			eRPPartBinDetailInformationDto.imgUnitFreightCost = dataTable.Rows[0].Field<decimal>("imgUnitFreightCost");
			eRPPartBinDetailInformationDto.imgUnitLaborCost = dataTable.Rows[0].Field<decimal>("imgUnitLaborCost");
			eRPPartBinDetailInformationDto.imgUnitMaterialCost = dataTable.Rows[0].Field<decimal>("imgUnitMaterialCost");
			eRPPartBinDetailInformationDto.imgUnitMiscCost = dataTable.Rows[0].Field<decimal>("imgUnitMiscCost");
			eRPPartBinDetailInformationDto.imgUnitOverheadCost = dataTable.Rows[0].Field<decimal>("imgUnitOverheadCost");
			eRPPartBinDetailInformationDto.imgUnitSubcontractCost = dataTable.Rows[0].Field<decimal>("imgUnitSubcontractCost");
			eRPPartBinDetailInformationDto.imgWarehouseID = dataTable.Rows[0].Field<string>("imgWarehouseID");
			eRPPartBinDetailInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartBinDetailInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartBinDetailInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartBinDetail(ERPPartBinDetailDto partBinDetail)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartBinDetails WHERE imgUniqueID = " + M1Util.ConvertToLinq(partBinDetail.imgUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imgPartID"] = partBinDetail.imgPartID.ToUpper();
				dataRow["imgPartRevisionID"] = partBinDetail.imgPartRevisionID.ToUpper();
				dataRow["imgWarehouseID"] = partBinDetail.imgWarehouseID.ToUpper();
				dataRow["imgPartBinID"] = partBinDetail.imgPartBinID.ToUpper();
				dataRow["imgPartBinDetailID"] = partBinDetail.imgPartBinDetailID;
				partBinDetail.imgUniqueID = ((partBinDetail.imgUniqueID == Guid.Empty) ? Guid.NewGuid() : partBinDetail.imgUniqueID);
				dataRow["imgUniqueID"] = partBinDetail.imgUniqueID;
				dataRow["imgCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imgCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartBinDetail could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partBinDetail.imgRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartBinDetail is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imgRowVersion"], partBinDetail.imgRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartBinDetail has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartBinDetail again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imgOriginalQuantity"] = partBinDetail.imgOriginalQuantity;
			dataRow["imgQuantityType"] = partBinDetail.imgQuantityType;
			dataRow["imgRemainingQuantity"] = partBinDetail.imgRemainingQuantity;
			dataRow["imgSourceTableName"] = partBinDetail.imgSourceTableName;
			dataRow["imgSourceTableUniqueID"] = partBinDetail.imgSourceTableUniqueID;
			DataRow dataRow2 = dataRow;
			DateTime? imgTransactionDate = partBinDetail.imgTransactionDate;
			dataRow2["imgTransactionDate"] = (imgTransactionDate.HasValue ? ((object)imgTransactionDate.GetValueOrDefault()) : dataRow["imgTransactionDate"]);
			dataRow["imgUnitDutyCost"] = partBinDetail.imgUnitDutyCost;
			dataRow["imgUnitFreightCost"] = partBinDetail.imgUnitFreightCost;
			dataRow["imgUnitLaborCost"] = partBinDetail.imgUnitLaborCost;
			dataRow["imgUnitMaterialCost"] = partBinDetail.imgUnitMaterialCost;
			dataRow["imgUnitMiscCost"] = partBinDetail.imgUnitMiscCost;
			dataRow["imgUnitOverheadCost"] = partBinDetail.imgUnitOverheadCost;
			dataRow["imgUnitSubcontractCost"] = partBinDetail.imgUnitSubcontractCost;
			if (partBinDetail.CustomFields != null && partBinDetail.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partBinDetail.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartBinDetail [{partBinDetail.imgUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartBinDetail [{partBinDetail.imgUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
