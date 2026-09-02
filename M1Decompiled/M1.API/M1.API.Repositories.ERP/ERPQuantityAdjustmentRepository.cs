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

public class ERPQuantityAdjustmentRepository : APIBaseRepository, IERPQuantityAdjustmentRepository, IAPIBaseRepository, IDisposable
{
	public ERPQuantityAdjustmentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesQuantityAdjustmentExist(Guid quantityAdjustmentId)
	{
		InitializeParameterLists();
		base.filterList.Add("inqUniqueID|C", quantityAdjustmentId);
		base.selectList.Add("inqUniqueID");
		return Task.FromResult(GetAsObject("QuantityAdjustments", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPQuantityAdjustmentInformationDto>> GetAllQuantityAdjustments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPQuantityAdjustmentInformationDto> collection = new List<ERPQuantityAdjustmentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[28]
		{
			"inqAdjustmentDate", "inqAdjustmentDescription", "inqAdjustmentType", "inqBinQuantityReceipted", "inqBinQuantityTransferred", "inqChangeQuantity", "inqQuantityAdjustmentID", "inqCountedQuantity", "inqCreatedBy", "inqCreatedDate",
			"inqCurrentQuantity", "inqDestinationPartBinID", "inqDestinationWarehouseID", "inqUniqueID", "inqPosted", "inqNewQuantity", "inqPartBinID", "inqPartID", "inqPartRevisionID", "inqPartShortDescription",
			"inqPartWarehouseLocationID", "inqPlantDepartmentID", "inqPlantID", "inqPostedDate", "inqQuantitySince", "inqRowVersion", "inqTransactionsSince", "inqUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("QuantityAdjustments");
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
		using (DataTable dataTable = GetAsDataTable("QuantityAdjustments", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPQuantityAdjustmentInformationDto eRPQuantityAdjustmentInformationDto = new ERPQuantityAdjustmentInformationDto();
				eRPQuantityAdjustmentInformationDto.inqAdjustmentDate = dataTable.Rows[i].Field<DateTime?>("inqAdjustmentDate");
				eRPQuantityAdjustmentInformationDto.inqAdjustmentDescription = dataTable.Rows[i].Field<string>("inqAdjustmentDescription");
				eRPQuantityAdjustmentInformationDto.inqAdjustmentType = dataTable.Rows[i].Field<byte>("inqAdjustmentType");
				eRPQuantityAdjustmentInformationDto.inqBinQuantityReceipted = dataTable.Rows[i].Field<decimal>("inqBinQuantityReceipted");
				eRPQuantityAdjustmentInformationDto.inqBinQuantityTransferred = dataTable.Rows[i].Field<decimal>("inqBinQuantityTransferred");
				eRPQuantityAdjustmentInformationDto.inqChangeQuantity = dataTable.Rows[i].Field<decimal>("inqChangeQuantity");
				eRPQuantityAdjustmentInformationDto.inqQuantityAdjustmentID = dataTable.Rows[i].Field<string>("inqQuantityAdjustmentID");
				eRPQuantityAdjustmentInformationDto.inqCountedQuantity = dataTable.Rows[i].Field<decimal>("inqCountedQuantity");
				eRPQuantityAdjustmentInformationDto.inqCreatedBy = dataTable.Rows[i].Field<string>("inqCreatedBy");
				eRPQuantityAdjustmentInformationDto.inqCreatedDate = dataTable.Rows[i].Field<DateTime?>("inqCreatedDate");
				eRPQuantityAdjustmentInformationDto.inqCurrentQuantity = dataTable.Rows[i].Field<decimal>("inqCurrentQuantity");
				eRPQuantityAdjustmentInformationDto.inqDestinationPartBinID = dataTable.Rows[i].Field<string>("inqDestinationPartBinID");
				eRPQuantityAdjustmentInformationDto.inqDestinationWarehouseID = dataTable.Rows[i].Field<string>("inqDestinationWarehouseID");
				eRPQuantityAdjustmentInformationDto.inqUniqueID = dataTable.Rows[i].Field<Guid>("inqUniqueID");
				eRPQuantityAdjustmentInformationDto.inqPosted = dataTable.Rows[i].Field<bool>("inqPosted");
				eRPQuantityAdjustmentInformationDto.inqNewQuantity = dataTable.Rows[i].Field<decimal>("inqNewQuantity");
				eRPQuantityAdjustmentInformationDto.inqPartBinID = dataTable.Rows[i].Field<string>("inqPartBinID");
				eRPQuantityAdjustmentInformationDto.inqPartID = dataTable.Rows[i].Field<string>("inqPartID");
				eRPQuantityAdjustmentInformationDto.inqPartRevisionID = dataTable.Rows[i].Field<string>("inqPartRevisionID");
				eRPQuantityAdjustmentInformationDto.inqPartShortDescription = dataTable.Rows[i].Field<string>("inqPartShortDescription");
				eRPQuantityAdjustmentInformationDto.inqPartWarehouseLocationID = dataTable.Rows[i].Field<string>("inqPartWarehouseLocationID");
				eRPQuantityAdjustmentInformationDto.inqPlantDepartmentID = dataTable.Rows[i].Field<string>("inqPlantDepartmentID");
				eRPQuantityAdjustmentInformationDto.inqPlantID = dataTable.Rows[i].Field<string>("inqPlantID");
				eRPQuantityAdjustmentInformationDto.inqPostedDate = dataTable.Rows[i].Field<DateTime?>("inqPostedDate");
				eRPQuantityAdjustmentInformationDto.inqQuantitySince = dataTable.Rows[i].Field<decimal>("inqQuantitySince");
				eRPQuantityAdjustmentInformationDto.inqRowVersion = dataTable.Rows[i].Field<byte[]>("inqRowVersion");
				eRPQuantityAdjustmentInformationDto.inqTransactionsSince = dataTable.Rows[i].Field<short>("inqTransactionsSince");
				eRPQuantityAdjustmentInformationDto.inqUnitOfMeasure = dataTable.Rows[i].Field<string>("inqUnitOfMeasure");
				eRPQuantityAdjustmentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPQuantityAdjustmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPQuantityAdjustmentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPQuantityAdjustmentInformationDto> GetQuantityAdjustment(Guid quantityAdjustmentId)
	{
		ERPQuantityAdjustmentInformationDto eRPQuantityAdjustmentInformationDto = new ERPQuantityAdjustmentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[28]
		{
			"inqAdjustmentDate", "inqAdjustmentDescription", "inqAdjustmentType", "inqBinQuantityReceipted", "inqBinQuantityTransferred", "inqChangeQuantity", "inqQuantityAdjustmentID", "inqCountedQuantity", "inqCreatedBy", "inqCreatedDate",
			"inqCurrentQuantity", "inqDestinationPartBinID", "inqDestinationWarehouseID", "inqUniqueID", "inqPosted", "inqNewQuantity", "inqPartBinID", "inqPartID", "inqPartRevisionID", "inqPartShortDescription",
			"inqPartWarehouseLocationID", "inqPlantDepartmentID", "inqPlantID", "inqPostedDate", "inqQuantitySince", "inqRowVersion", "inqTransactionsSince", "inqUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("inqUniqueID|C", quantityAdjustmentId);
		AddCustomFieldsToSelectList("QuantityAdjustments");
		using (DataTable dataTable = GetAsDataTable("QuantityAdjustments", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPQuantityAdjustmentInformationDto);
			}
			eRPQuantityAdjustmentInformationDto.inqAdjustmentDate = dataTable.Rows[0].Field<DateTime?>("inqAdjustmentDate");
			eRPQuantityAdjustmentInformationDto.inqAdjustmentDescription = dataTable.Rows[0].Field<string>("inqAdjustmentDescription");
			eRPQuantityAdjustmentInformationDto.inqAdjustmentType = dataTable.Rows[0].Field<byte>("inqAdjustmentType");
			eRPQuantityAdjustmentInformationDto.inqBinQuantityReceipted = dataTable.Rows[0].Field<decimal>("inqBinQuantityReceipted");
			eRPQuantityAdjustmentInformationDto.inqBinQuantityTransferred = dataTable.Rows[0].Field<decimal>("inqBinQuantityTransferred");
			eRPQuantityAdjustmentInformationDto.inqChangeQuantity = dataTable.Rows[0].Field<decimal>("inqChangeQuantity");
			eRPQuantityAdjustmentInformationDto.inqQuantityAdjustmentID = dataTable.Rows[0].Field<string>("inqQuantityAdjustmentID");
			eRPQuantityAdjustmentInformationDto.inqCountedQuantity = dataTable.Rows[0].Field<decimal>("inqCountedQuantity");
			eRPQuantityAdjustmentInformationDto.inqCreatedBy = dataTable.Rows[0].Field<string>("inqCreatedBy");
			eRPQuantityAdjustmentInformationDto.inqCreatedDate = dataTable.Rows[0].Field<DateTime?>("inqCreatedDate");
			eRPQuantityAdjustmentInformationDto.inqCurrentQuantity = dataTable.Rows[0].Field<decimal>("inqCurrentQuantity");
			eRPQuantityAdjustmentInformationDto.inqDestinationPartBinID = dataTable.Rows[0].Field<string>("inqDestinationPartBinID");
			eRPQuantityAdjustmentInformationDto.inqDestinationWarehouseID = dataTable.Rows[0].Field<string>("inqDestinationWarehouseID");
			eRPQuantityAdjustmentInformationDto.inqUniqueID = dataTable.Rows[0].Field<Guid>("inqUniqueID");
			eRPQuantityAdjustmentInformationDto.inqPosted = dataTable.Rows[0].Field<bool>("inqPosted");
			eRPQuantityAdjustmentInformationDto.inqNewQuantity = dataTable.Rows[0].Field<decimal>("inqNewQuantity");
			eRPQuantityAdjustmentInformationDto.inqPartBinID = dataTable.Rows[0].Field<string>("inqPartBinID");
			eRPQuantityAdjustmentInformationDto.inqPartID = dataTable.Rows[0].Field<string>("inqPartID");
			eRPQuantityAdjustmentInformationDto.inqPartRevisionID = dataTable.Rows[0].Field<string>("inqPartRevisionID");
			eRPQuantityAdjustmentInformationDto.inqPartShortDescription = dataTable.Rows[0].Field<string>("inqPartShortDescription");
			eRPQuantityAdjustmentInformationDto.inqPartWarehouseLocationID = dataTable.Rows[0].Field<string>("inqPartWarehouseLocationID");
			eRPQuantityAdjustmentInformationDto.inqPlantDepartmentID = dataTable.Rows[0].Field<string>("inqPlantDepartmentID");
			eRPQuantityAdjustmentInformationDto.inqPlantID = dataTable.Rows[0].Field<string>("inqPlantID");
			eRPQuantityAdjustmentInformationDto.inqPostedDate = dataTable.Rows[0].Field<DateTime?>("inqPostedDate");
			eRPQuantityAdjustmentInformationDto.inqQuantitySince = dataTable.Rows[0].Field<decimal>("inqQuantitySince");
			eRPQuantityAdjustmentInformationDto.inqRowVersion = dataTable.Rows[0].Field<byte[]>("inqRowVersion");
			eRPQuantityAdjustmentInformationDto.inqTransactionsSince = dataTable.Rows[0].Field<short>("inqTransactionsSince");
			eRPQuantityAdjustmentInformationDto.inqUnitOfMeasure = dataTable.Rows[0].Field<string>("inqUnitOfMeasure");
			eRPQuantityAdjustmentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPQuantityAdjustmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPQuantityAdjustmentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveQuantityAdjustment(ERPQuantityAdjustmentDto quantityAdjustment)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM QuantityAdjustments WHERE inqUniqueID = " + M1Util.ConvertToLinq(quantityAdjustment.inqUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["inqQuantityAdjustmentID"] = quantityAdjustment.inqQuantityAdjustmentID.ToUpper();
				quantityAdjustment.inqUniqueID = ((quantityAdjustment.inqUniqueID == Guid.Empty) ? Guid.NewGuid() : quantityAdjustment.inqUniqueID);
				dataRow["inqUniqueID"] = quantityAdjustment.inqUniqueID;
				dataRow["inqCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["inqCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The QuantityAdjustment could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (quantityAdjustment.inqRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the QuantityAdjustment is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["inqRowVersion"], quantityAdjustment.inqRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the QuantityAdjustment has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the QuantityAdjustment again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? inqAdjustmentDate = quantityAdjustment.inqAdjustmentDate;
			dataRow2["inqAdjustmentDate"] = (inqAdjustmentDate.HasValue ? ((object)inqAdjustmentDate.GetValueOrDefault()) : dataRow["inqAdjustmentDate"]);
			dataRow["inqAdjustmentDescription"] = quantityAdjustment.inqAdjustmentDescription;
			dataRow["inqAdjustmentType"] = quantityAdjustment.inqAdjustmentType;
			dataRow["inqBinQuantityReceipted"] = quantityAdjustment.inqBinQuantityReceipted;
			dataRow["inqBinQuantityTransferred"] = quantityAdjustment.inqBinQuantityTransferred;
			dataRow["inqChangeQuantity"] = quantityAdjustment.inqChangeQuantity;
			dataRow["inqCountedQuantity"] = quantityAdjustment.inqCountedQuantity;
			dataRow["inqCurrentQuantity"] = quantityAdjustment.inqCurrentQuantity;
			dataRow["inqDestinationPartBinID"] = quantityAdjustment.inqDestinationPartBinID;
			dataRow["inqDestinationWarehouseID"] = quantityAdjustment.inqDestinationWarehouseID;
			dataRow["inqPosted"] = quantityAdjustment.inqPosted;
			dataRow["inqNewQuantity"] = quantityAdjustment.inqNewQuantity;
			dataRow["inqPartBinID"] = quantityAdjustment.inqPartBinID;
			dataRow["inqPartID"] = quantityAdjustment.inqPartID;
			dataRow["inqPartRevisionID"] = quantityAdjustment.inqPartRevisionID;
			dataRow["inqPartShortDescription"] = quantityAdjustment.inqPartShortDescription;
			dataRow["inqPartWarehouseLocationID"] = quantityAdjustment.inqPartWarehouseLocationID;
			dataRow["inqPlantDepartmentID"] = quantityAdjustment.inqPlantDepartmentID;
			dataRow["inqPlantID"] = quantityAdjustment.inqPlantID;
			DataRow dataRow3 = dataRow;
			inqAdjustmentDate = quantityAdjustment.inqPostedDate;
			dataRow3["inqPostedDate"] = (inqAdjustmentDate.HasValue ? ((object)inqAdjustmentDate.GetValueOrDefault()) : dataRow["inqPostedDate"]);
			dataRow["inqQuantitySince"] = quantityAdjustment.inqQuantitySince;
			dataRow["inqTransactionsSince"] = quantityAdjustment.inqTransactionsSince;
			dataRow["inqUnitOfMeasure"] = quantityAdjustment.inqUnitOfMeasure;
			if (quantityAdjustment.CustomFields != null && quantityAdjustment.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in quantityAdjustment.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the QuantityAdjustment [{quantityAdjustment.inqUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the QuantityAdjustment [{quantityAdjustment.inqUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
