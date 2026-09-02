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

public class ERPRFQLineRepository : APIBaseRepository, IERPRFQLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPRFQLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesRFQLineExist(Guid rFQLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("rqlUniqueID|C", rFQLineId);
		base.selectList.Add("rqlUniqueID");
		return Task.FromResult(GetAsObject("RFQLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPRFQLineInformationDto>> GetAllRFQLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPRFQLineInformationDto> collection = new List<ERPRFQLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[29]
		{
			"rqlCreatedBy", "rqlCreatedDate", "rqlDocuments", "rqlUniqueID", "rqlInventoryUnitOfMeasure", "rqlAlternatePart", "rqlClosed", "rqlJobAssemblyID", "rqlJobEstimatedQty", "rqlJobID",
			"rqlJobMaterialID", "rqlJobOperationID", "rqlPartID", "rqlPartLongDescriptionRtf", "rqlPartLongDescriptionText", "rqlPartRevisionID", "rqlPartShortDescription", "rqlProjectAreaID", "rqlProjectID", "rqlPurchaseUnitOfMeasure",
			"rqlQuoteAssemblyID", "rqlQuoteID", "rqlQuoteLineID", "rqlQuoteMaterialID", "rqlQuoteOperationID", "rqlRfqID", "rqlRfqType", "rqlRowVersion", "rqlRfqLineID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("RFQLines");
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
		using (DataTable dataTable = GetAsDataTable("RFQLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPRFQLineInformationDto eRPRFQLineInformationDto = new ERPRFQLineInformationDto();
				eRPRFQLineInformationDto.rqlCreatedBy = dataTable.Rows[i].Field<string>("rqlCreatedBy");
				eRPRFQLineInformationDto.rqlCreatedDate = dataTable.Rows[i].Field<DateTime?>("rqlCreatedDate");
				eRPRFQLineInformationDto.rqlDocuments = dataTable.Rows[i].Field<string>("rqlDocuments");
				eRPRFQLineInformationDto.rqlUniqueID = dataTable.Rows[i].Field<Guid>("rqlUniqueID");
				eRPRFQLineInformationDto.rqlInventoryUnitOfMeasure = dataTable.Rows[i].Field<string>("rqlInventoryUnitOfMeasure");
				eRPRFQLineInformationDto.rqlAlternatePart = dataTable.Rows[i].Field<bool>("rqlAlternatePart");
				eRPRFQLineInformationDto.rqlClosed = dataTable.Rows[i].Field<bool>("rqlClosed");
				eRPRFQLineInformationDto.rqlJobAssemblyID = dataTable.Rows[i].Field<int>("rqlJobAssemblyID");
				eRPRFQLineInformationDto.rqlJobEstimatedQty = dataTable.Rows[i].Field<decimal>("rqlJobEstimatedQty");
				eRPRFQLineInformationDto.rqlJobID = dataTable.Rows[i].Field<string>("rqlJobID");
				eRPRFQLineInformationDto.rqlJobMaterialID = dataTable.Rows[i].Field<int>("rqlJobMaterialID");
				eRPRFQLineInformationDto.rqlJobOperationID = dataTable.Rows[i].Field<int>("rqlJobOperationID");
				eRPRFQLineInformationDto.rqlPartID = dataTable.Rows[i].Field<string>("rqlPartID");
				eRPRFQLineInformationDto.rqlPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("rqlPartLongDescriptionRtf");
				eRPRFQLineInformationDto.rqlPartLongDescriptionText = dataTable.Rows[i].Field<string>("rqlPartLongDescriptionText");
				eRPRFQLineInformationDto.rqlPartRevisionID = dataTable.Rows[i].Field<string>("rqlPartRevisionID");
				eRPRFQLineInformationDto.rqlPartShortDescription = dataTable.Rows[i].Field<string>("rqlPartShortDescription");
				eRPRFQLineInformationDto.rqlProjectAreaID = dataTable.Rows[i].Field<string>("rqlProjectAreaID");
				eRPRFQLineInformationDto.rqlProjectID = dataTable.Rows[i].Field<string>("rqlProjectID");
				eRPRFQLineInformationDto.rqlPurchaseUnitOfMeasure = dataTable.Rows[i].Field<string>("rqlPurchaseUnitOfMeasure");
				eRPRFQLineInformationDto.rqlQuoteAssemblyID = dataTable.Rows[i].Field<int>("rqlQuoteAssemblyID");
				eRPRFQLineInformationDto.rqlQuoteID = dataTable.Rows[i].Field<string>("rqlQuoteID");
				eRPRFQLineInformationDto.rqlQuoteLineID = dataTable.Rows[i].Field<short>("rqlQuoteLineID");
				eRPRFQLineInformationDto.rqlQuoteMaterialID = dataTable.Rows[i].Field<int>("rqlQuoteMaterialID");
				eRPRFQLineInformationDto.rqlQuoteOperationID = dataTable.Rows[i].Field<int>("rqlQuoteOperationID");
				eRPRFQLineInformationDto.rqlRfqID = dataTable.Rows[i].Field<string>("rqlRfqID");
				eRPRFQLineInformationDto.rqlRfqType = dataTable.Rows[i].Field<byte>("rqlRfqType");
				eRPRFQLineInformationDto.rqlRowVersion = dataTable.Rows[i].Field<byte[]>("rqlRowVersion");
				eRPRFQLineInformationDto.rqlRfqLineID = dataTable.Rows[i].Field<short>("rqlRfqLineID");
				eRPRFQLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPRFQLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPRFQLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPRFQLineInformationDto> GetRFQLine(Guid rFQLineId)
	{
		ERPRFQLineInformationDto eRPRFQLineInformationDto = new ERPRFQLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[29]
		{
			"rqlCreatedBy", "rqlCreatedDate", "rqlDocuments", "rqlUniqueID", "rqlInventoryUnitOfMeasure", "rqlAlternatePart", "rqlClosed", "rqlJobAssemblyID", "rqlJobEstimatedQty", "rqlJobID",
			"rqlJobMaterialID", "rqlJobOperationID", "rqlPartID", "rqlPartLongDescriptionRtf", "rqlPartLongDescriptionText", "rqlPartRevisionID", "rqlPartShortDescription", "rqlProjectAreaID", "rqlProjectID", "rqlPurchaseUnitOfMeasure",
			"rqlQuoteAssemblyID", "rqlQuoteID", "rqlQuoteLineID", "rqlQuoteMaterialID", "rqlQuoteOperationID", "rqlRfqID", "rqlRfqType", "rqlRowVersion", "rqlRfqLineID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rqlUniqueID|C", rFQLineId);
		AddCustomFieldsToSelectList("RFQLines");
		using (DataTable dataTable = GetAsDataTable("RFQLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPRFQLineInformationDto);
			}
			eRPRFQLineInformationDto.rqlCreatedBy = dataTable.Rows[0].Field<string>("rqlCreatedBy");
			eRPRFQLineInformationDto.rqlCreatedDate = dataTable.Rows[0].Field<DateTime?>("rqlCreatedDate");
			eRPRFQLineInformationDto.rqlDocuments = dataTable.Rows[0].Field<string>("rqlDocuments");
			eRPRFQLineInformationDto.rqlUniqueID = dataTable.Rows[0].Field<Guid>("rqlUniqueID");
			eRPRFQLineInformationDto.rqlInventoryUnitOfMeasure = dataTable.Rows[0].Field<string>("rqlInventoryUnitOfMeasure");
			eRPRFQLineInformationDto.rqlAlternatePart = dataTable.Rows[0].Field<bool>("rqlAlternatePart");
			eRPRFQLineInformationDto.rqlClosed = dataTable.Rows[0].Field<bool>("rqlClosed");
			eRPRFQLineInformationDto.rqlJobAssemblyID = dataTable.Rows[0].Field<int>("rqlJobAssemblyID");
			eRPRFQLineInformationDto.rqlJobEstimatedQty = dataTable.Rows[0].Field<decimal>("rqlJobEstimatedQty");
			eRPRFQLineInformationDto.rqlJobID = dataTable.Rows[0].Field<string>("rqlJobID");
			eRPRFQLineInformationDto.rqlJobMaterialID = dataTable.Rows[0].Field<int>("rqlJobMaterialID");
			eRPRFQLineInformationDto.rqlJobOperationID = dataTable.Rows[0].Field<int>("rqlJobOperationID");
			eRPRFQLineInformationDto.rqlPartID = dataTable.Rows[0].Field<string>("rqlPartID");
			eRPRFQLineInformationDto.rqlPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("rqlPartLongDescriptionRtf");
			eRPRFQLineInformationDto.rqlPartLongDescriptionText = dataTable.Rows[0].Field<string>("rqlPartLongDescriptionText");
			eRPRFQLineInformationDto.rqlPartRevisionID = dataTable.Rows[0].Field<string>("rqlPartRevisionID");
			eRPRFQLineInformationDto.rqlPartShortDescription = dataTable.Rows[0].Field<string>("rqlPartShortDescription");
			eRPRFQLineInformationDto.rqlProjectAreaID = dataTable.Rows[0].Field<string>("rqlProjectAreaID");
			eRPRFQLineInformationDto.rqlProjectID = dataTable.Rows[0].Field<string>("rqlProjectID");
			eRPRFQLineInformationDto.rqlPurchaseUnitOfMeasure = dataTable.Rows[0].Field<string>("rqlPurchaseUnitOfMeasure");
			eRPRFQLineInformationDto.rqlQuoteAssemblyID = dataTable.Rows[0].Field<int>("rqlQuoteAssemblyID");
			eRPRFQLineInformationDto.rqlQuoteID = dataTable.Rows[0].Field<string>("rqlQuoteID");
			eRPRFQLineInformationDto.rqlQuoteLineID = dataTable.Rows[0].Field<short>("rqlQuoteLineID");
			eRPRFQLineInformationDto.rqlQuoteMaterialID = dataTable.Rows[0].Field<int>("rqlQuoteMaterialID");
			eRPRFQLineInformationDto.rqlQuoteOperationID = dataTable.Rows[0].Field<int>("rqlQuoteOperationID");
			eRPRFQLineInformationDto.rqlRfqID = dataTable.Rows[0].Field<string>("rqlRfqID");
			eRPRFQLineInformationDto.rqlRfqType = dataTable.Rows[0].Field<byte>("rqlRfqType");
			eRPRFQLineInformationDto.rqlRowVersion = dataTable.Rows[0].Field<byte[]>("rqlRowVersion");
			eRPRFQLineInformationDto.rqlRfqLineID = dataTable.Rows[0].Field<short>("rqlRfqLineID");
			eRPRFQLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPRFQLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPRFQLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveRFQLine(ERPRFQLineDto rFQLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM RFQLines WHERE rqlUniqueID = " + M1Util.ConvertToLinq(rFQLine.rqlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rqlRfqID"] = rFQLine.rqlRfqID.ToUpper();
				dataRow["rqlRfqLineID"] = rFQLine.rqlRfqLineID;
				rFQLine.rqlUniqueID = ((rFQLine.rqlUniqueID == Guid.Empty) ? Guid.NewGuid() : rFQLine.rqlUniqueID);
				dataRow["rqlUniqueID"] = rFQLine.rqlUniqueID;
				dataRow["rqlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rqlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The RFQLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (rFQLine.rqlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the RFQLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rqlRowVersion"], rFQLine.rqlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the RFQLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the RFQLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rqlDocuments"] = rFQLine.rqlDocuments ?? dataRow["rqlDocuments"];
			dataRow["rqlInventoryUnitOfMeasure"] = rFQLine.rqlInventoryUnitOfMeasure;
			dataRow["rqlAlternatePart"] = rFQLine.rqlAlternatePart;
			dataRow["rqlClosed"] = rFQLine.rqlClosed;
			dataRow["rqlJobAssemblyID"] = rFQLine.rqlJobAssemblyID;
			dataRow["rqlJobEstimatedQty"] = rFQLine.rqlJobEstimatedQty;
			dataRow["rqlJobID"] = rFQLine.rqlJobID;
			dataRow["rqlJobMaterialID"] = rFQLine.rqlJobMaterialID;
			dataRow["rqlJobOperationID"] = rFQLine.rqlJobOperationID;
			dataRow["rqlPartID"] = rFQLine.rqlPartID;
			dataRow["rqlPartLongDescriptionRtf"] = rFQLine.rqlPartLongDescriptionRtf ?? dataRow["rqlPartLongDescriptionRtf"];
			dataRow["rqlPartLongDescriptionText"] = rFQLine.rqlPartLongDescriptionText ?? dataRow["rqlPartLongDescriptionText"];
			dataRow["rqlPartRevisionID"] = rFQLine.rqlPartRevisionID;
			dataRow["rqlPartShortDescription"] = rFQLine.rqlPartShortDescription;
			dataRow["rqlProjectAreaID"] = rFQLine.rqlProjectAreaID;
			dataRow["rqlProjectID"] = rFQLine.rqlProjectID;
			dataRow["rqlPurchaseUnitOfMeasure"] = rFQLine.rqlPurchaseUnitOfMeasure;
			dataRow["rqlQuoteAssemblyID"] = rFQLine.rqlQuoteAssemblyID;
			dataRow["rqlQuoteID"] = rFQLine.rqlQuoteID;
			dataRow["rqlQuoteLineID"] = rFQLine.rqlQuoteLineID;
			dataRow["rqlQuoteMaterialID"] = rFQLine.rqlQuoteMaterialID;
			dataRow["rqlQuoteOperationID"] = rFQLine.rqlQuoteOperationID;
			dataRow["rqlRfqType"] = rFQLine.rqlRfqType;
			if (rFQLine.CustomFields != null && rFQLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in rFQLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the RFQLine [{rFQLine.rqlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the RFQLine [{rFQLine.rqlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
