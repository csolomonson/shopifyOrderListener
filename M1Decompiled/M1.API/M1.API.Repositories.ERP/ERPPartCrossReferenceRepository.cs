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

public class ERPPartCrossReferenceRepository : APIBaseRepository, IERPPartCrossReferenceRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartCrossReferenceRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartCrossReferenceExist(Guid partCrossReferenceId)
	{
		InitializeParameterLists();
		base.filterList.Add("imxUniqueID|C", partCrossReferenceId);
		base.selectList.Add("imxUniqueID");
		return Task.FromResult(GetAsObject("PartCrossReferences", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartCrossReferenceInformationDto>> GetAllPartCrossReferences(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartCrossReferenceInformationDto> collection = new List<ERPPartCrossReferenceInformationDto>();
		InitializeParameterLists();
		string[] array = new string[18]
		{
			"imxConversionFactor", "imxCreatedBy", "imxCreatedDate", "imxUniqueID", "imxInactive", "imxPurchased", "imxSold", "imxLeadTime", "imxLocationID", "imxLotSize",
			"imxMinimumPurchaseQuantity", "imxOrganizationID", "imxOrgPartID", "imxOrgPartShortDescription", "imxPartID", "imxPartRevisionID", "imxPurchaseUnitOfMeasure", "imxRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartCrossReferences");
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
		using (DataTable dataTable = GetAsDataTable("PartCrossReferences", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartCrossReferenceInformationDto eRPPartCrossReferenceInformationDto = new ERPPartCrossReferenceInformationDto();
				eRPPartCrossReferenceInformationDto.imxConversionFactor = dataTable.Rows[i].Field<decimal>("imxConversionFactor");
				eRPPartCrossReferenceInformationDto.imxCreatedBy = dataTable.Rows[i].Field<string>("imxCreatedBy");
				eRPPartCrossReferenceInformationDto.imxCreatedDate = dataTable.Rows[i].Field<DateTime?>("imxCreatedDate");
				eRPPartCrossReferenceInformationDto.imxUniqueID = dataTable.Rows[i].Field<Guid>("imxUniqueID");
				eRPPartCrossReferenceInformationDto.imxInactive = dataTable.Rows[i].Field<bool>("imxInactive");
				eRPPartCrossReferenceInformationDto.imxPurchased = dataTable.Rows[i].Field<bool>("imxPurchased");
				eRPPartCrossReferenceInformationDto.imxSold = dataTable.Rows[i].Field<bool>("imxSold");
				eRPPartCrossReferenceInformationDto.imxLeadTime = dataTable.Rows[i].Field<short>("imxLeadTime");
				eRPPartCrossReferenceInformationDto.imxLocationID = dataTable.Rows[i].Field<string>("imxLocationID");
				eRPPartCrossReferenceInformationDto.imxLotSize = dataTable.Rows[i].Field<decimal>("imxLotSize");
				eRPPartCrossReferenceInformationDto.imxMinimumPurchaseQuantity = dataTable.Rows[i].Field<decimal>("imxMinimumPurchaseQuantity");
				eRPPartCrossReferenceInformationDto.imxOrganizationID = dataTable.Rows[i].Field<string>("imxOrganizationID");
				eRPPartCrossReferenceInformationDto.imxOrgPartID = dataTable.Rows[i].Field<string>("imxOrgPartID");
				eRPPartCrossReferenceInformationDto.imxOrgPartShortDescription = dataTable.Rows[i].Field<string>("imxOrgPartShortDescription");
				eRPPartCrossReferenceInformationDto.imxPartID = dataTable.Rows[i].Field<string>("imxPartID");
				eRPPartCrossReferenceInformationDto.imxPartRevisionID = dataTable.Rows[i].Field<string>("imxPartRevisionID");
				eRPPartCrossReferenceInformationDto.imxPurchaseUnitOfMeasure = dataTable.Rows[i].Field<string>("imxPurchaseUnitOfMeasure");
				eRPPartCrossReferenceInformationDto.imxRowVersion = dataTable.Rows[i].Field<byte[]>("imxRowVersion");
				eRPPartCrossReferenceInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartCrossReferenceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartCrossReferenceInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartCrossReferenceInformationDto> GetPartCrossReference(Guid partCrossReferenceId)
	{
		ERPPartCrossReferenceInformationDto eRPPartCrossReferenceInformationDto = new ERPPartCrossReferenceInformationDto();
		InitializeParameterLists();
		string[] collection = new string[18]
		{
			"imxConversionFactor", "imxCreatedBy", "imxCreatedDate", "imxUniqueID", "imxInactive", "imxPurchased", "imxSold", "imxLeadTime", "imxLocationID", "imxLotSize",
			"imxMinimumPurchaseQuantity", "imxOrganizationID", "imxOrgPartID", "imxOrgPartShortDescription", "imxPartID", "imxPartRevisionID", "imxPurchaseUnitOfMeasure", "imxRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imxUniqueID|C", partCrossReferenceId);
		AddCustomFieldsToSelectList("PartCrossReferences");
		using (DataTable dataTable = GetAsDataTable("PartCrossReferences", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartCrossReferenceInformationDto);
			}
			eRPPartCrossReferenceInformationDto.imxConversionFactor = dataTable.Rows[0].Field<decimal>("imxConversionFactor");
			eRPPartCrossReferenceInformationDto.imxCreatedBy = dataTable.Rows[0].Field<string>("imxCreatedBy");
			eRPPartCrossReferenceInformationDto.imxCreatedDate = dataTable.Rows[0].Field<DateTime?>("imxCreatedDate");
			eRPPartCrossReferenceInformationDto.imxUniqueID = dataTable.Rows[0].Field<Guid>("imxUniqueID");
			eRPPartCrossReferenceInformationDto.imxInactive = dataTable.Rows[0].Field<bool>("imxInactive");
			eRPPartCrossReferenceInformationDto.imxPurchased = dataTable.Rows[0].Field<bool>("imxPurchased");
			eRPPartCrossReferenceInformationDto.imxSold = dataTable.Rows[0].Field<bool>("imxSold");
			eRPPartCrossReferenceInformationDto.imxLeadTime = dataTable.Rows[0].Field<short>("imxLeadTime");
			eRPPartCrossReferenceInformationDto.imxLocationID = dataTable.Rows[0].Field<string>("imxLocationID");
			eRPPartCrossReferenceInformationDto.imxLotSize = dataTable.Rows[0].Field<decimal>("imxLotSize");
			eRPPartCrossReferenceInformationDto.imxMinimumPurchaseQuantity = dataTable.Rows[0].Field<decimal>("imxMinimumPurchaseQuantity");
			eRPPartCrossReferenceInformationDto.imxOrganizationID = dataTable.Rows[0].Field<string>("imxOrganizationID");
			eRPPartCrossReferenceInformationDto.imxOrgPartID = dataTable.Rows[0].Field<string>("imxOrgPartID");
			eRPPartCrossReferenceInformationDto.imxOrgPartShortDescription = dataTable.Rows[0].Field<string>("imxOrgPartShortDescription");
			eRPPartCrossReferenceInformationDto.imxPartID = dataTable.Rows[0].Field<string>("imxPartID");
			eRPPartCrossReferenceInformationDto.imxPartRevisionID = dataTable.Rows[0].Field<string>("imxPartRevisionID");
			eRPPartCrossReferenceInformationDto.imxPurchaseUnitOfMeasure = dataTable.Rows[0].Field<string>("imxPurchaseUnitOfMeasure");
			eRPPartCrossReferenceInformationDto.imxRowVersion = dataTable.Rows[0].Field<byte[]>("imxRowVersion");
			eRPPartCrossReferenceInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartCrossReferenceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartCrossReferenceInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartCrossReference(ERPPartCrossReferenceDto partCrossReference)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartCrossReferences WHERE imxUniqueID = " + M1Util.ConvertToLinq(partCrossReference.imxUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imxPartID"] = partCrossReference.imxPartID.ToUpper();
				dataRow["imxPartRevisionID"] = partCrossReference.imxPartRevisionID.ToUpper();
				dataRow["imxOrganizationID"] = partCrossReference.imxOrganizationID.ToUpper();
				dataRow["imxLocationID"] = partCrossReference.imxLocationID.ToUpper();
				partCrossReference.imxUniqueID = ((partCrossReference.imxUniqueID == Guid.Empty) ? Guid.NewGuid() : partCrossReference.imxUniqueID);
				dataRow["imxUniqueID"] = partCrossReference.imxUniqueID;
				dataRow["imxCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imxCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartCrossReference could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partCrossReference.imxRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartCrossReference is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imxRowVersion"], partCrossReference.imxRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartCrossReference has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartCrossReference again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imxConversionFactor"] = partCrossReference.imxConversionFactor;
			dataRow["imxInactive"] = partCrossReference.imxInactive;
			dataRow["imxPurchased"] = partCrossReference.imxPurchased;
			dataRow["imxSold"] = partCrossReference.imxSold;
			dataRow["imxLeadTime"] = partCrossReference.imxLeadTime;
			dataRow["imxLotSize"] = partCrossReference.imxLotSize;
			dataRow["imxMinimumPurchaseQuantity"] = partCrossReference.imxMinimumPurchaseQuantity;
			dataRow["imxOrgPartID"] = partCrossReference.imxOrgPartID;
			dataRow["imxOrgPartShortDescription"] = partCrossReference.imxOrgPartShortDescription;
			dataRow["imxPurchaseUnitOfMeasure"] = partCrossReference.imxPurchaseUnitOfMeasure;
			if (partCrossReference.CustomFields != null && partCrossReference.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partCrossReference.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartCrossReference [{partCrossReference.imxUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartCrossReference [{partCrossReference.imxUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
