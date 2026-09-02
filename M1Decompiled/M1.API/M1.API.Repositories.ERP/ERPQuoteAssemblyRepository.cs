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

public class ERPQuoteAssemblyRepository : APIBaseRepository, IERPQuoteAssemblyRepository, IAPIBaseRepository, IDisposable
{
	public ERPQuoteAssemblyRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesQuoteAssemblyExist(Guid quoteAssemblyId)
	{
		InitializeParameterLists();
		base.filterList.Add("qmaUniqueID|C", quoteAssemblyId);
		base.selectList.Add("qmaUniqueID");
		return Task.FromResult(GetAsObject("QuoteAssemblies", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPQuoteAssemblyInformationDto>> GetAllQuoteAssemblies(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPQuoteAssemblyInformationDto> collection = new List<ERPQuoteAssemblyInformationDto>();
		InitializeParameterLists();
		string[] array = new string[30]
		{
			"qmaAssemblyOverlap", "qmaCreatedBy", "qmaCreatedDate", "qmaDocuments", "qmaUniqueID", "qmaClosed", "qmaPullAllFromStock", "qmaLevel", "qmaOverlapDestinationLink", "qmaOverlapOffsetTime",
			"qmaOverlapOperationID", "qmaOverlapSourceLink", "qmaOverlapSourceOperationID", "qmaOverlapType", "qmaParentAssemblyID", "qmaPartID", "qmaPartLongDescriptionRtf", "qmaPartLongDescriptionText", "qmaPartRevisionID", "qmaPartShortDescription",
			"qmaProductionNotesRTF", "qmaProductionNotesText", "qmaQuantityPerParent", "qmaQuoteID", "qmaQuoteLineID", "qmaRowVersion", "qmaQuoteAssemblyID", "qmaSourceMethodID", "qmaSourceRevisionID", "qmaUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("QuoteAssemblies");
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
		using (DataTable dataTable = GetAsDataTable("QuoteAssemblies", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPQuoteAssemblyInformationDto eRPQuoteAssemblyInformationDto = new ERPQuoteAssemblyInformationDto();
				eRPQuoteAssemblyInformationDto.qmaAssemblyOverlap = dataTable.Rows[i].Field<byte>("qmaAssemblyOverlap");
				eRPQuoteAssemblyInformationDto.qmaCreatedBy = dataTable.Rows[i].Field<string>("qmaCreatedBy");
				eRPQuoteAssemblyInformationDto.qmaCreatedDate = dataTable.Rows[i].Field<DateTime?>("qmaCreatedDate");
				eRPQuoteAssemblyInformationDto.qmaDocuments = dataTable.Rows[i].Field<string>("qmaDocuments");
				eRPQuoteAssemblyInformationDto.qmaUniqueID = dataTable.Rows[i].Field<Guid>("qmaUniqueID");
				eRPQuoteAssemblyInformationDto.qmaClosed = dataTable.Rows[i].Field<bool>("qmaClosed");
				eRPQuoteAssemblyInformationDto.qmaPullAllFromStock = dataTable.Rows[i].Field<bool>("qmaPullAllFromStock");
				eRPQuoteAssemblyInformationDto.qmaLevel = dataTable.Rows[i].Field<short>("qmaLevel");
				eRPQuoteAssemblyInformationDto.qmaOverlapDestinationLink = dataTable.Rows[i].Field<byte>("qmaOverlapDestinationLink");
				eRPQuoteAssemblyInformationDto.qmaOverlapOffsetTime = dataTable.Rows[i].Field<decimal>("qmaOverlapOffsetTime");
				eRPQuoteAssemblyInformationDto.qmaOverlapOperationID = dataTable.Rows[i].Field<int>("qmaOverlapOperationID");
				eRPQuoteAssemblyInformationDto.qmaOverlapSourceLink = dataTable.Rows[i].Field<byte>("qmaOverlapSourceLink");
				eRPQuoteAssemblyInformationDto.qmaOverlapSourceOperationID = dataTable.Rows[i].Field<int>("qmaOverlapSourceOperationID");
				eRPQuoteAssemblyInformationDto.qmaOverlapType = dataTable.Rows[i].Field<byte>("qmaOverlapType");
				eRPQuoteAssemblyInformationDto.qmaParentAssemblyID = dataTable.Rows[i].Field<int>("qmaParentAssemblyID");
				eRPQuoteAssemblyInformationDto.qmaPartID = dataTable.Rows[i].Field<string>("qmaPartID");
				eRPQuoteAssemblyInformationDto.qmaPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("qmaPartLongDescriptionRtf");
				eRPQuoteAssemblyInformationDto.qmaPartLongDescriptionText = dataTable.Rows[i].Field<string>("qmaPartLongDescriptionText");
				eRPQuoteAssemblyInformationDto.qmaPartRevisionID = dataTable.Rows[i].Field<string>("qmaPartRevisionID");
				eRPQuoteAssemblyInformationDto.qmaPartShortDescription = dataTable.Rows[i].Field<string>("qmaPartShortDescription");
				eRPQuoteAssemblyInformationDto.qmaProductionNotesRTF = dataTable.Rows[i].Field<string>("qmaProductionNotesRTF");
				eRPQuoteAssemblyInformationDto.qmaProductionNotesText = dataTable.Rows[i].Field<string>("qmaProductionNotesText");
				eRPQuoteAssemblyInformationDto.qmaQuantityPerParent = dataTable.Rows[i].Field<decimal>("qmaQuantityPerParent");
				eRPQuoteAssemblyInformationDto.qmaQuoteID = dataTable.Rows[i].Field<string>("qmaQuoteID");
				eRPQuoteAssemblyInformationDto.qmaQuoteLineID = dataTable.Rows[i].Field<short>("qmaQuoteLineID");
				eRPQuoteAssemblyInformationDto.qmaRowVersion = dataTable.Rows[i].Field<byte[]>("qmaRowVersion");
				eRPQuoteAssemblyInformationDto.qmaQuoteAssemblyID = dataTable.Rows[i].Field<int>("qmaQuoteAssemblyID");
				eRPQuoteAssemblyInformationDto.qmaSourceMethodID = dataTable.Rows[i].Field<string>("qmaSourceMethodID");
				eRPQuoteAssemblyInformationDto.qmaSourceRevisionID = dataTable.Rows[i].Field<string>("qmaSourceRevisionID");
				eRPQuoteAssemblyInformationDto.qmaUnitOfMeasure = dataTable.Rows[i].Field<string>("qmaUnitOfMeasure");
				eRPQuoteAssemblyInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPQuoteAssemblyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPQuoteAssemblyInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPQuoteAssemblyInformationDto> GetQuoteAssembly(Guid quoteAssemblyId)
	{
		ERPQuoteAssemblyInformationDto eRPQuoteAssemblyInformationDto = new ERPQuoteAssemblyInformationDto();
		InitializeParameterLists();
		string[] collection = new string[30]
		{
			"qmaAssemblyOverlap", "qmaCreatedBy", "qmaCreatedDate", "qmaDocuments", "qmaUniqueID", "qmaClosed", "qmaPullAllFromStock", "qmaLevel", "qmaOverlapDestinationLink", "qmaOverlapOffsetTime",
			"qmaOverlapOperationID", "qmaOverlapSourceLink", "qmaOverlapSourceOperationID", "qmaOverlapType", "qmaParentAssemblyID", "qmaPartID", "qmaPartLongDescriptionRtf", "qmaPartLongDescriptionText", "qmaPartRevisionID", "qmaPartShortDescription",
			"qmaProductionNotesRTF", "qmaProductionNotesText", "qmaQuantityPerParent", "qmaQuoteID", "qmaQuoteLineID", "qmaRowVersion", "qmaQuoteAssemblyID", "qmaSourceMethodID", "qmaSourceRevisionID", "qmaUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("qmaUniqueID|C", quoteAssemblyId);
		AddCustomFieldsToSelectList("QuoteAssemblies");
		using (DataTable dataTable = GetAsDataTable("QuoteAssemblies", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPQuoteAssemblyInformationDto);
			}
			eRPQuoteAssemblyInformationDto.qmaAssemblyOverlap = dataTable.Rows[0].Field<byte>("qmaAssemblyOverlap");
			eRPQuoteAssemblyInformationDto.qmaCreatedBy = dataTable.Rows[0].Field<string>("qmaCreatedBy");
			eRPQuoteAssemblyInformationDto.qmaCreatedDate = dataTable.Rows[0].Field<DateTime?>("qmaCreatedDate");
			eRPQuoteAssemblyInformationDto.qmaDocuments = dataTable.Rows[0].Field<string>("qmaDocuments");
			eRPQuoteAssemblyInformationDto.qmaUniqueID = dataTable.Rows[0].Field<Guid>("qmaUniqueID");
			eRPQuoteAssemblyInformationDto.qmaClosed = dataTable.Rows[0].Field<bool>("qmaClosed");
			eRPQuoteAssemblyInformationDto.qmaPullAllFromStock = dataTable.Rows[0].Field<bool>("qmaPullAllFromStock");
			eRPQuoteAssemblyInformationDto.qmaLevel = dataTable.Rows[0].Field<short>("qmaLevel");
			eRPQuoteAssemblyInformationDto.qmaOverlapDestinationLink = dataTable.Rows[0].Field<byte>("qmaOverlapDestinationLink");
			eRPQuoteAssemblyInformationDto.qmaOverlapOffsetTime = dataTable.Rows[0].Field<decimal>("qmaOverlapOffsetTime");
			eRPQuoteAssemblyInformationDto.qmaOverlapOperationID = dataTable.Rows[0].Field<int>("qmaOverlapOperationID");
			eRPQuoteAssemblyInformationDto.qmaOverlapSourceLink = dataTable.Rows[0].Field<byte>("qmaOverlapSourceLink");
			eRPQuoteAssemblyInformationDto.qmaOverlapSourceOperationID = dataTable.Rows[0].Field<int>("qmaOverlapSourceOperationID");
			eRPQuoteAssemblyInformationDto.qmaOverlapType = dataTable.Rows[0].Field<byte>("qmaOverlapType");
			eRPQuoteAssemblyInformationDto.qmaParentAssemblyID = dataTable.Rows[0].Field<int>("qmaParentAssemblyID");
			eRPQuoteAssemblyInformationDto.qmaPartID = dataTable.Rows[0].Field<string>("qmaPartID");
			eRPQuoteAssemblyInformationDto.qmaPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("qmaPartLongDescriptionRtf");
			eRPQuoteAssemblyInformationDto.qmaPartLongDescriptionText = dataTable.Rows[0].Field<string>("qmaPartLongDescriptionText");
			eRPQuoteAssemblyInformationDto.qmaPartRevisionID = dataTable.Rows[0].Field<string>("qmaPartRevisionID");
			eRPQuoteAssemblyInformationDto.qmaPartShortDescription = dataTable.Rows[0].Field<string>("qmaPartShortDescription");
			eRPQuoteAssemblyInformationDto.qmaProductionNotesRTF = dataTable.Rows[0].Field<string>("qmaProductionNotesRTF");
			eRPQuoteAssemblyInformationDto.qmaProductionNotesText = dataTable.Rows[0].Field<string>("qmaProductionNotesText");
			eRPQuoteAssemblyInformationDto.qmaQuantityPerParent = dataTable.Rows[0].Field<decimal>("qmaQuantityPerParent");
			eRPQuoteAssemblyInformationDto.qmaQuoteID = dataTable.Rows[0].Field<string>("qmaQuoteID");
			eRPQuoteAssemblyInformationDto.qmaQuoteLineID = dataTable.Rows[0].Field<short>("qmaQuoteLineID");
			eRPQuoteAssemblyInformationDto.qmaRowVersion = dataTable.Rows[0].Field<byte[]>("qmaRowVersion");
			eRPQuoteAssemblyInformationDto.qmaQuoteAssemblyID = dataTable.Rows[0].Field<int>("qmaQuoteAssemblyID");
			eRPQuoteAssemblyInformationDto.qmaSourceMethodID = dataTable.Rows[0].Field<string>("qmaSourceMethodID");
			eRPQuoteAssemblyInformationDto.qmaSourceRevisionID = dataTable.Rows[0].Field<string>("qmaSourceRevisionID");
			eRPQuoteAssemblyInformationDto.qmaUnitOfMeasure = dataTable.Rows[0].Field<string>("qmaUnitOfMeasure");
			eRPQuoteAssemblyInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPQuoteAssemblyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPQuoteAssemblyInformationDto);
	}

	public Task<APIValidationInfoDto> SaveQuoteAssembly(ERPQuoteAssemblyDto quoteAssembly)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM QuoteAssemblies WHERE qmaUniqueID = " + M1Util.ConvertToLinq(quoteAssembly.qmaUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qmaQuoteID"] = quoteAssembly.qmaQuoteID.ToUpper();
				dataRow["qmaQuoteLineID"] = quoteAssembly.qmaQuoteLineID;
				dataRow["qmaQuoteAssemblyID"] = quoteAssembly.qmaQuoteAssemblyID;
				quoteAssembly.qmaUniqueID = ((quoteAssembly.qmaUniqueID == Guid.Empty) ? Guid.NewGuid() : quoteAssembly.qmaUniqueID);
				dataRow["qmaUniqueID"] = quoteAssembly.qmaUniqueID;
				dataRow["qmaCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qmaCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The QuoteAssembly could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (quoteAssembly.qmaRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the QuoteAssembly is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["qmaRowVersion"], quoteAssembly.qmaRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the QuoteAssembly has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the QuoteAssembly again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qmaAssemblyOverlap"] = quoteAssembly.qmaAssemblyOverlap;
			dataRow["qmaDocuments"] = quoteAssembly.qmaDocuments ?? dataRow["qmaDocuments"];
			dataRow["qmaClosed"] = quoteAssembly.qmaClosed;
			dataRow["qmaPullAllFromStock"] = quoteAssembly.qmaPullAllFromStock;
			dataRow["qmaLevel"] = quoteAssembly.qmaLevel;
			dataRow["qmaOverlapDestinationLink"] = quoteAssembly.qmaOverlapDestinationLink;
			dataRow["qmaOverlapOffsetTime"] = quoteAssembly.qmaOverlapOffsetTime;
			dataRow["qmaOverlapOperationID"] = quoteAssembly.qmaOverlapOperationID;
			dataRow["qmaOverlapSourceLink"] = quoteAssembly.qmaOverlapSourceLink;
			dataRow["qmaOverlapSourceOperationID"] = quoteAssembly.qmaOverlapSourceOperationID;
			dataRow["qmaOverlapType"] = quoteAssembly.qmaOverlapType;
			dataRow["qmaParentAssemblyID"] = quoteAssembly.qmaParentAssemblyID;
			dataRow["qmaPartID"] = quoteAssembly.qmaPartID;
			dataRow["qmaPartLongDescriptionRtf"] = quoteAssembly.qmaPartLongDescriptionRtf ?? dataRow["qmaPartLongDescriptionRtf"];
			dataRow["qmaPartLongDescriptionText"] = quoteAssembly.qmaPartLongDescriptionText ?? dataRow["qmaPartLongDescriptionText"];
			dataRow["qmaPartRevisionID"] = quoteAssembly.qmaPartRevisionID;
			dataRow["qmaPartShortDescription"] = quoteAssembly.qmaPartShortDescription;
			dataRow["qmaProductionNotesRTF"] = quoteAssembly.qmaProductionNotesRTF ?? dataRow["qmaProductionNotesRTF"];
			dataRow["qmaProductionNotesText"] = quoteAssembly.qmaProductionNotesText ?? dataRow["qmaProductionNotesText"];
			dataRow["qmaQuantityPerParent"] = quoteAssembly.qmaQuantityPerParent;
			dataRow["qmaSourceMethodID"] = quoteAssembly.qmaSourceMethodID;
			dataRow["qmaSourceRevisionID"] = quoteAssembly.qmaSourceRevisionID;
			dataRow["qmaUnitOfMeasure"] = quoteAssembly.qmaUnitOfMeasure;
			if (quoteAssembly.CustomFields != null && quoteAssembly.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in quoteAssembly.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the QuoteAssembly [{quoteAssembly.qmaUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the QuoteAssembly [{quoteAssembly.qmaUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
