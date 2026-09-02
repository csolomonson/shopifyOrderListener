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

public class ERPPartBinRepository : APIBaseRepository, IERPPartBinRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartBinRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartBinExist(Guid partBinId)
	{
		InitializeParameterLists();
		base.filterList.Add("imbUniqueID|C", partBinId);
		base.selectList.Add("imbUniqueID");
		return Task.FromResult(GetAsObject("PartBins", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartBinInformationDto>> GetAllPartBins(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartBinInformationDto> collection = new List<ERPPartBinInformationDto>();
		InitializeParameterLists();
		string[] array = new string[21]
		{
			"imbBinQuantityOnHand", "imbPartBinID", "imbConversionFactor", "imbCreatedBy", "imbCreatedDate", "imbDescription", "imbUniqueID", "imbInactiveBinDate", "imbInactiveBin", "imbDefaultBin",
			"imbPartID", "imbPartRevisionID", "imbQuantityAllocated", "imbQuantityOnHand", "imbQuantityOnOrderPurchases", "imbQuantityOnOrderSales", "imbQuantityToInspect", "imbQuantityToReturn", "imbQuantityToReturnJob", "imbRowVersion",
			"imbWarehouseID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartBins");
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
		using (DataTable dataTable = GetAsDataTable("PartBins", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartBinInformationDto eRPPartBinInformationDto = new ERPPartBinInformationDto();
				eRPPartBinInformationDto.imbBinQuantityOnHand = dataTable.Rows[i].Field<decimal>("imbBinQuantityOnHand");
				eRPPartBinInformationDto.imbPartBinID = dataTable.Rows[i].Field<string>("imbPartBinID");
				eRPPartBinInformationDto.imbConversionFactor = dataTable.Rows[i].Field<decimal>("imbConversionFactor");
				eRPPartBinInformationDto.imbCreatedBy = dataTable.Rows[i].Field<string>("imbCreatedBy");
				eRPPartBinInformationDto.imbCreatedDate = dataTable.Rows[i].Field<DateTime?>("imbCreatedDate");
				eRPPartBinInformationDto.imbDescription = dataTable.Rows[i].Field<string>("imbDescription");
				eRPPartBinInformationDto.imbUniqueID = dataTable.Rows[i].Field<Guid>("imbUniqueID");
				eRPPartBinInformationDto.imbInactiveBinDate = dataTable.Rows[i].Field<DateTime?>("imbInactiveBinDate");
				eRPPartBinInformationDto.imbInactiveBin = dataTable.Rows[i].Field<bool>("imbInactiveBin");
				eRPPartBinInformationDto.imbDefaultBin = dataTable.Rows[i].Field<bool>("imbDefaultBin");
				eRPPartBinInformationDto.imbPartID = dataTable.Rows[i].Field<string>("imbPartID");
				eRPPartBinInformationDto.imbPartRevisionID = dataTable.Rows[i].Field<string>("imbPartRevisionID");
				eRPPartBinInformationDto.imbQuantityAllocated = dataTable.Rows[i].Field<decimal>("imbQuantityAllocated");
				eRPPartBinInformationDto.imbQuantityOnHand = dataTable.Rows[i].Field<decimal>("imbQuantityOnHand");
				eRPPartBinInformationDto.imbQuantityOnOrderPurchases = dataTable.Rows[i].Field<decimal>("imbQuantityOnOrderPurchases");
				eRPPartBinInformationDto.imbQuantityOnOrderSales = dataTable.Rows[i].Field<decimal>("imbQuantityOnOrderSales");
				eRPPartBinInformationDto.imbQuantityToInspect = dataTable.Rows[i].Field<decimal>("imbQuantityToInspect");
				eRPPartBinInformationDto.imbQuantityToReturn = dataTable.Rows[i].Field<decimal>("imbQuantityToReturn");
				eRPPartBinInformationDto.imbQuantityToReturnJob = dataTable.Rows[i].Field<decimal>("imbQuantityToReturnJob");
				eRPPartBinInformationDto.imbRowVersion = dataTable.Rows[i].Field<byte[]>("imbRowVersion");
				eRPPartBinInformationDto.imbWarehouseID = dataTable.Rows[i].Field<string>("imbWarehouseID");
				eRPPartBinInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartBinInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartBinInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartBinInformationDto> GetPartBin(Guid partBinId)
	{
		ERPPartBinInformationDto eRPPartBinInformationDto = new ERPPartBinInformationDto();
		InitializeParameterLists();
		string[] collection = new string[21]
		{
			"imbBinQuantityOnHand", "imbPartBinID", "imbConversionFactor", "imbCreatedBy", "imbCreatedDate", "imbDescription", "imbUniqueID", "imbInactiveBinDate", "imbInactiveBin", "imbDefaultBin",
			"imbPartID", "imbPartRevisionID", "imbQuantityAllocated", "imbQuantityOnHand", "imbQuantityOnOrderPurchases", "imbQuantityOnOrderSales", "imbQuantityToInspect", "imbQuantityToReturn", "imbQuantityToReturnJob", "imbRowVersion",
			"imbWarehouseID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imbUniqueID|C", partBinId);
		AddCustomFieldsToSelectList("PartBins");
		using (DataTable dataTable = GetAsDataTable("PartBins", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartBinInformationDto);
			}
			eRPPartBinInformationDto.imbBinQuantityOnHand = dataTable.Rows[0].Field<decimal>("imbBinQuantityOnHand");
			eRPPartBinInformationDto.imbPartBinID = dataTable.Rows[0].Field<string>("imbPartBinID");
			eRPPartBinInformationDto.imbConversionFactor = dataTable.Rows[0].Field<decimal>("imbConversionFactor");
			eRPPartBinInformationDto.imbCreatedBy = dataTable.Rows[0].Field<string>("imbCreatedBy");
			eRPPartBinInformationDto.imbCreatedDate = dataTable.Rows[0].Field<DateTime?>("imbCreatedDate");
			eRPPartBinInformationDto.imbDescription = dataTable.Rows[0].Field<string>("imbDescription");
			eRPPartBinInformationDto.imbUniqueID = dataTable.Rows[0].Field<Guid>("imbUniqueID");
			eRPPartBinInformationDto.imbInactiveBinDate = dataTable.Rows[0].Field<DateTime?>("imbInactiveBinDate");
			eRPPartBinInformationDto.imbInactiveBin = dataTable.Rows[0].Field<bool>("imbInactiveBin");
			eRPPartBinInformationDto.imbDefaultBin = dataTable.Rows[0].Field<bool>("imbDefaultBin");
			eRPPartBinInformationDto.imbPartID = dataTable.Rows[0].Field<string>("imbPartID");
			eRPPartBinInformationDto.imbPartRevisionID = dataTable.Rows[0].Field<string>("imbPartRevisionID");
			eRPPartBinInformationDto.imbQuantityAllocated = dataTable.Rows[0].Field<decimal>("imbQuantityAllocated");
			eRPPartBinInformationDto.imbQuantityOnHand = dataTable.Rows[0].Field<decimal>("imbQuantityOnHand");
			eRPPartBinInformationDto.imbQuantityOnOrderPurchases = dataTable.Rows[0].Field<decimal>("imbQuantityOnOrderPurchases");
			eRPPartBinInformationDto.imbQuantityOnOrderSales = dataTable.Rows[0].Field<decimal>("imbQuantityOnOrderSales");
			eRPPartBinInformationDto.imbQuantityToInspect = dataTable.Rows[0].Field<decimal>("imbQuantityToInspect");
			eRPPartBinInformationDto.imbQuantityToReturn = dataTable.Rows[0].Field<decimal>("imbQuantityToReturn");
			eRPPartBinInformationDto.imbQuantityToReturnJob = dataTable.Rows[0].Field<decimal>("imbQuantityToReturnJob");
			eRPPartBinInformationDto.imbRowVersion = dataTable.Rows[0].Field<byte[]>("imbRowVersion");
			eRPPartBinInformationDto.imbWarehouseID = dataTable.Rows[0].Field<string>("imbWarehouseID");
			eRPPartBinInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartBinInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartBinInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartBin(ERPPartBinDto partBin)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartBins WHERE imbUniqueID = " + M1Util.ConvertToLinq(partBin.imbUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imbPartID"] = partBin.imbPartID.ToUpper();
				dataRow["imbPartRevisionID"] = partBin.imbPartRevisionID.ToUpper();
				dataRow["imbWarehouseID"] = partBin.imbWarehouseID.ToUpper();
				dataRow["imbPartBinID"] = partBin.imbPartBinID.ToUpper();
				partBin.imbUniqueID = ((partBin.imbUniqueID == Guid.Empty) ? Guid.NewGuid() : partBin.imbUniqueID);
				dataRow["imbUniqueID"] = partBin.imbUniqueID;
				dataRow["imbCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imbCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartBin could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partBin.imbRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartBin is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imbRowVersion"], partBin.imbRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartBin has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartBin again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imbBinQuantityOnHand"] = partBin.imbBinQuantityOnHand;
			dataRow["imbConversionFactor"] = partBin.imbConversionFactor;
			dataRow["imbDescription"] = partBin.imbDescription;
			DataRow dataRow2 = dataRow;
			DateTime? imbInactiveBinDate = partBin.imbInactiveBinDate;
			dataRow2["imbInactiveBinDate"] = (imbInactiveBinDate.HasValue ? ((object)imbInactiveBinDate.GetValueOrDefault()) : dataRow["imbInactiveBinDate"]);
			dataRow["imbInactiveBin"] = partBin.imbInactiveBin;
			dataRow["imbDefaultBin"] = partBin.imbDefaultBin;
			dataRow["imbQuantityAllocated"] = partBin.imbQuantityAllocated;
			dataRow["imbQuantityOnHand"] = partBin.imbQuantityOnHand;
			dataRow["imbQuantityOnOrderPurchases"] = partBin.imbQuantityOnOrderPurchases;
			dataRow["imbQuantityOnOrderSales"] = partBin.imbQuantityOnOrderSales;
			dataRow["imbQuantityToInspect"] = partBin.imbQuantityToInspect;
			dataRow["imbQuantityToReturn"] = partBin.imbQuantityToReturn;
			dataRow["imbQuantityToReturnJob"] = partBin.imbQuantityToReturnJob;
			if (partBin.CustomFields != null && partBin.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partBin.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartBin [{partBin.imbUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartBin [{partBin.imbUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
