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

public class ERPPartAssemblyRepository : APIBaseRepository, IERPPartAssemblyRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartAssemblyRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartAssemblyExist(Guid partAssemblyId)
	{
		InitializeParameterLists();
		base.filterList.Add("imaUniqueID|C", partAssemblyId);
		base.selectList.Add("imaUniqueID");
		return Task.FromResult(GetAsObject("PartAssemblies", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartAssemblyInformationDto>> GetAllPartAssemblies(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartAssemblyInformationDto> collection = new List<ERPPartAssemblyInformationDto>();
		InitializeParameterLists();
		string[] array = new string[30]
		{
			"imaAssemblyOverlap", "imaCreatedBy", "imaCreatedDate", "imaDocuments", "imaUniqueID", "imaPullAllFromStock", "imaUseMethod", "imaLevel", "imaMethodAssemblyID", "imaMethodID",
			"imaMethodRevisionID", "imaOverlapDestinationLink", "imaOverlapOffsetTime", "imaOverlapOperationID", "imaOverlapSourceLink", "imaOverlapSourceOperationID", "imaOverlapType", "imaParentAssemblyID", "imaPartID", "imaPartLongDescriptionRtf",
			"imaPartLongDescriptionText", "imaPartRevisionID", "imaPartShortDescription", "imaProductionNotesRTF", "imaProductionNotesText", "imaQuantityPerParent", "imaRowVersion", "imaSourceMethodID", "imaSourceRevisionID", "imaUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartAssemblies");
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
		using (DataTable dataTable = GetAsDataTable("PartAssemblies", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartAssemblyInformationDto eRPPartAssemblyInformationDto = new ERPPartAssemblyInformationDto();
				eRPPartAssemblyInformationDto.imaAssemblyOverlap = dataTable.Rows[i].Field<byte>("imaAssemblyOverlap");
				eRPPartAssemblyInformationDto.imaCreatedBy = dataTable.Rows[i].Field<string>("imaCreatedBy");
				eRPPartAssemblyInformationDto.imaCreatedDate = dataTable.Rows[i].Field<DateTime?>("imaCreatedDate");
				eRPPartAssemblyInformationDto.imaDocuments = dataTable.Rows[i].Field<string>("imaDocuments");
				eRPPartAssemblyInformationDto.imaUniqueID = dataTable.Rows[i].Field<Guid>("imaUniqueID");
				eRPPartAssemblyInformationDto.imaPullAllFromStock = dataTable.Rows[i].Field<bool>("imaPullAllFromStock");
				eRPPartAssemblyInformationDto.imaUseMethod = dataTable.Rows[i].Field<bool>("imaUseMethod");
				eRPPartAssemblyInformationDto.imaLevel = dataTable.Rows[i].Field<short>("imaLevel");
				eRPPartAssemblyInformationDto.imaMethodAssemblyID = dataTable.Rows[i].Field<int>("imaMethodAssemblyID");
				eRPPartAssemblyInformationDto.imaMethodID = dataTable.Rows[i].Field<string>("imaMethodID");
				eRPPartAssemblyInformationDto.imaMethodRevisionID = dataTable.Rows[i].Field<string>("imaMethodRevisionID");
				eRPPartAssemblyInformationDto.imaOverlapDestinationLink = dataTable.Rows[i].Field<byte>("imaOverlapDestinationLink");
				eRPPartAssemblyInformationDto.imaOverlapOffsetTime = dataTable.Rows[i].Field<decimal>("imaOverlapOffsetTime");
				eRPPartAssemblyInformationDto.imaOverlapOperationID = dataTable.Rows[i].Field<int>("imaOverlapOperationID");
				eRPPartAssemblyInformationDto.imaOverlapSourceLink = dataTable.Rows[i].Field<byte>("imaOverlapSourceLink");
				eRPPartAssemblyInformationDto.imaOverlapSourceOperationID = dataTable.Rows[i].Field<int>("imaOverlapSourceOperationID");
				eRPPartAssemblyInformationDto.imaOverlapType = dataTable.Rows[i].Field<byte>("imaOverlapType");
				eRPPartAssemblyInformationDto.imaParentAssemblyID = dataTable.Rows[i].Field<int>("imaParentAssemblyID");
				eRPPartAssemblyInformationDto.imaPartID = dataTable.Rows[i].Field<string>("imaPartID");
				eRPPartAssemblyInformationDto.imaPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("imaPartLongDescriptionRtf");
				eRPPartAssemblyInformationDto.imaPartLongDescriptionText = dataTable.Rows[i].Field<string>("imaPartLongDescriptionText");
				eRPPartAssemblyInformationDto.imaPartRevisionID = dataTable.Rows[i].Field<string>("imaPartRevisionID");
				eRPPartAssemblyInformationDto.imaPartShortDescription = dataTable.Rows[i].Field<string>("imaPartShortDescription");
				eRPPartAssemblyInformationDto.imaProductionNotesRTF = dataTable.Rows[i].Field<string>("imaProductionNotesRTF");
				eRPPartAssemblyInformationDto.imaProductionNotesText = dataTable.Rows[i].Field<string>("imaProductionNotesText");
				eRPPartAssemblyInformationDto.imaQuantityPerParent = dataTable.Rows[i].Field<decimal>("imaQuantityPerParent");
				eRPPartAssemblyInformationDto.imaRowVersion = dataTable.Rows[i].Field<byte[]>("imaRowVersion");
				eRPPartAssemblyInformationDto.imaSourceMethodID = dataTable.Rows[i].Field<string>("imaSourceMethodID");
				eRPPartAssemblyInformationDto.imaSourceRevisionID = dataTable.Rows[i].Field<string>("imaSourceRevisionID");
				eRPPartAssemblyInformationDto.imaUnitOfMeasure = dataTable.Rows[i].Field<string>("imaUnitOfMeasure");
				eRPPartAssemblyInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartAssemblyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartAssemblyInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartAssemblyInformationDto> GetPartAssembly(Guid partAssemblyId)
	{
		ERPPartAssemblyInformationDto eRPPartAssemblyInformationDto = new ERPPartAssemblyInformationDto();
		InitializeParameterLists();
		string[] collection = new string[30]
		{
			"imaAssemblyOverlap", "imaCreatedBy", "imaCreatedDate", "imaDocuments", "imaUniqueID", "imaPullAllFromStock", "imaUseMethod", "imaLevel", "imaMethodAssemblyID", "imaMethodID",
			"imaMethodRevisionID", "imaOverlapDestinationLink", "imaOverlapOffsetTime", "imaOverlapOperationID", "imaOverlapSourceLink", "imaOverlapSourceOperationID", "imaOverlapType", "imaParentAssemblyID", "imaPartID", "imaPartLongDescriptionRtf",
			"imaPartLongDescriptionText", "imaPartRevisionID", "imaPartShortDescription", "imaProductionNotesRTF", "imaProductionNotesText", "imaQuantityPerParent", "imaRowVersion", "imaSourceMethodID", "imaSourceRevisionID", "imaUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imaUniqueID|C", partAssemblyId);
		AddCustomFieldsToSelectList("PartAssemblies");
		using (DataTable dataTable = GetAsDataTable("PartAssemblies", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartAssemblyInformationDto);
			}
			eRPPartAssemblyInformationDto.imaAssemblyOverlap = dataTable.Rows[0].Field<byte>("imaAssemblyOverlap");
			eRPPartAssemblyInformationDto.imaCreatedBy = dataTable.Rows[0].Field<string>("imaCreatedBy");
			eRPPartAssemblyInformationDto.imaCreatedDate = dataTable.Rows[0].Field<DateTime?>("imaCreatedDate");
			eRPPartAssemblyInformationDto.imaDocuments = dataTable.Rows[0].Field<string>("imaDocuments");
			eRPPartAssemblyInformationDto.imaUniqueID = dataTable.Rows[0].Field<Guid>("imaUniqueID");
			eRPPartAssemblyInformationDto.imaPullAllFromStock = dataTable.Rows[0].Field<bool>("imaPullAllFromStock");
			eRPPartAssemblyInformationDto.imaUseMethod = dataTable.Rows[0].Field<bool>("imaUseMethod");
			eRPPartAssemblyInformationDto.imaLevel = dataTable.Rows[0].Field<short>("imaLevel");
			eRPPartAssemblyInformationDto.imaMethodAssemblyID = dataTable.Rows[0].Field<int>("imaMethodAssemblyID");
			eRPPartAssemblyInformationDto.imaMethodID = dataTable.Rows[0].Field<string>("imaMethodID");
			eRPPartAssemblyInformationDto.imaMethodRevisionID = dataTable.Rows[0].Field<string>("imaMethodRevisionID");
			eRPPartAssemblyInformationDto.imaOverlapDestinationLink = dataTable.Rows[0].Field<byte>("imaOverlapDestinationLink");
			eRPPartAssemblyInformationDto.imaOverlapOffsetTime = dataTable.Rows[0].Field<decimal>("imaOverlapOffsetTime");
			eRPPartAssemblyInformationDto.imaOverlapOperationID = dataTable.Rows[0].Field<int>("imaOverlapOperationID");
			eRPPartAssemblyInformationDto.imaOverlapSourceLink = dataTable.Rows[0].Field<byte>("imaOverlapSourceLink");
			eRPPartAssemblyInformationDto.imaOverlapSourceOperationID = dataTable.Rows[0].Field<int>("imaOverlapSourceOperationID");
			eRPPartAssemblyInformationDto.imaOverlapType = dataTable.Rows[0].Field<byte>("imaOverlapType");
			eRPPartAssemblyInformationDto.imaParentAssemblyID = dataTable.Rows[0].Field<int>("imaParentAssemblyID");
			eRPPartAssemblyInformationDto.imaPartID = dataTable.Rows[0].Field<string>("imaPartID");
			eRPPartAssemblyInformationDto.imaPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("imaPartLongDescriptionRtf");
			eRPPartAssemblyInformationDto.imaPartLongDescriptionText = dataTable.Rows[0].Field<string>("imaPartLongDescriptionText");
			eRPPartAssemblyInformationDto.imaPartRevisionID = dataTable.Rows[0].Field<string>("imaPartRevisionID");
			eRPPartAssemblyInformationDto.imaPartShortDescription = dataTable.Rows[0].Field<string>("imaPartShortDescription");
			eRPPartAssemblyInformationDto.imaProductionNotesRTF = dataTable.Rows[0].Field<string>("imaProductionNotesRTF");
			eRPPartAssemblyInformationDto.imaProductionNotesText = dataTable.Rows[0].Field<string>("imaProductionNotesText");
			eRPPartAssemblyInformationDto.imaQuantityPerParent = dataTable.Rows[0].Field<decimal>("imaQuantityPerParent");
			eRPPartAssemblyInformationDto.imaRowVersion = dataTable.Rows[0].Field<byte[]>("imaRowVersion");
			eRPPartAssemblyInformationDto.imaSourceMethodID = dataTable.Rows[0].Field<string>("imaSourceMethodID");
			eRPPartAssemblyInformationDto.imaSourceRevisionID = dataTable.Rows[0].Field<string>("imaSourceRevisionID");
			eRPPartAssemblyInformationDto.imaUnitOfMeasure = dataTable.Rows[0].Field<string>("imaUnitOfMeasure");
			eRPPartAssemblyInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartAssemblyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartAssemblyInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartAssembly(ERPPartAssemblyDto partAssembly)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartAssemblies WHERE imaUniqueID = " + M1Util.ConvertToLinq(partAssembly.imaUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imaMethodID"] = partAssembly.imaMethodID.ToUpper();
				dataRow["imaMethodRevisionID"] = partAssembly.imaMethodRevisionID.ToUpper();
				dataRow["imaMethodAssemblyID"] = partAssembly.imaMethodAssemblyID;
				partAssembly.imaUniqueID = ((partAssembly.imaUniqueID == Guid.Empty) ? Guid.NewGuid() : partAssembly.imaUniqueID);
				dataRow["imaUniqueID"] = partAssembly.imaUniqueID;
				dataRow["imaCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imaCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartAssembly could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partAssembly.imaRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartAssembly is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imaRowVersion"], partAssembly.imaRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartAssembly has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartAssembly again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imaAssemblyOverlap"] = partAssembly.imaAssemblyOverlap;
			dataRow["imaDocuments"] = partAssembly.imaDocuments ?? dataRow["imaDocuments"];
			dataRow["imaPullAllFromStock"] = partAssembly.imaPullAllFromStock;
			dataRow["imaUseMethod"] = partAssembly.imaUseMethod;
			dataRow["imaLevel"] = partAssembly.imaLevel;
			dataRow["imaOverlapDestinationLink"] = partAssembly.imaOverlapDestinationLink;
			dataRow["imaOverlapOffsetTime"] = partAssembly.imaOverlapOffsetTime;
			dataRow["imaOverlapOperationID"] = partAssembly.imaOverlapOperationID;
			dataRow["imaOverlapSourceLink"] = partAssembly.imaOverlapSourceLink;
			dataRow["imaOverlapSourceOperationID"] = partAssembly.imaOverlapSourceOperationID;
			dataRow["imaOverlapType"] = partAssembly.imaOverlapType;
			dataRow["imaParentAssemblyID"] = partAssembly.imaParentAssemblyID;
			dataRow["imaPartID"] = partAssembly.imaPartID;
			dataRow["imaPartLongDescriptionRtf"] = partAssembly.imaPartLongDescriptionRtf ?? dataRow["imaPartLongDescriptionRtf"];
			dataRow["imaPartLongDescriptionText"] = partAssembly.imaPartLongDescriptionText ?? dataRow["imaPartLongDescriptionText"];
			dataRow["imaPartRevisionID"] = partAssembly.imaPartRevisionID;
			dataRow["imaPartShortDescription"] = partAssembly.imaPartShortDescription;
			dataRow["imaProductionNotesRTF"] = partAssembly.imaProductionNotesRTF ?? dataRow["imaProductionNotesRTF"];
			dataRow["imaProductionNotesText"] = partAssembly.imaProductionNotesText ?? dataRow["imaProductionNotesText"];
			dataRow["imaQuantityPerParent"] = partAssembly.imaQuantityPerParent;
			dataRow["imaSourceMethodID"] = partAssembly.imaSourceMethodID;
			dataRow["imaSourceRevisionID"] = partAssembly.imaSourceRevisionID;
			dataRow["imaUnitOfMeasure"] = partAssembly.imaUnitOfMeasure;
			if (partAssembly.CustomFields != null && partAssembly.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partAssembly.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartAssembly [{partAssembly.imaUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartAssembly [{partAssembly.imaUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
