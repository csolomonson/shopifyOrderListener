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

public class ERPPartOrgReferenceRepository : APIBaseRepository, IERPPartOrgReferenceRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartOrgReferenceRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartOrgReferenceExist(Guid partOrgReferenceId)
	{
		InitializeParameterLists();
		base.filterList.Add("imzUniqueID|C", partOrgReferenceId);
		base.selectList.Add("imzUniqueID");
		return Task.FromResult(GetAsObject("PartOrgReferences", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartOrgReferenceInformationDto>> GetAllPartOrgReferences(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartOrgReferenceInformationDto> collection = new List<ERPPartOrgReferenceInformationDto>();
		InitializeParameterLists();
		string[] array = new string[17]
		{
			"imzConversionFactor", "imzCreatedBy", "imzCreatedDate", "imzUniqueID", "imzInactive", "imzPurchased", "imzSold", "imzLeadTime", "imzLotSize", "imzMinimumPurchaseQuantity",
			"imzOrganizationID", "imzOrgPartID", "imzOrgPartShortDescription", "imzPartID", "imzPartRevisionID", "imzPurchaseUnitOfMeasure", "imzRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartOrgReferences");
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
		using (DataTable dataTable = GetAsDataTable("PartOrgReferences", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartOrgReferenceInformationDto eRPPartOrgReferenceInformationDto = new ERPPartOrgReferenceInformationDto();
				eRPPartOrgReferenceInformationDto.imzConversionFactor = dataTable.Rows[i].Field<decimal>("imzConversionFactor");
				eRPPartOrgReferenceInformationDto.imzCreatedBy = dataTable.Rows[i].Field<string>("imzCreatedBy");
				eRPPartOrgReferenceInformationDto.imzCreatedDate = dataTable.Rows[i].Field<DateTime?>("imzCreatedDate");
				eRPPartOrgReferenceInformationDto.imzUniqueID = dataTable.Rows[i].Field<Guid>("imzUniqueID");
				eRPPartOrgReferenceInformationDto.imzInactive = dataTable.Rows[i].Field<bool>("imzInactive");
				eRPPartOrgReferenceInformationDto.imzPurchased = dataTable.Rows[i].Field<bool>("imzPurchased");
				eRPPartOrgReferenceInformationDto.imzSold = dataTable.Rows[i].Field<bool>("imzSold");
				eRPPartOrgReferenceInformationDto.imzLeadTime = dataTable.Rows[i].Field<short>("imzLeadTime");
				eRPPartOrgReferenceInformationDto.imzLotSize = dataTable.Rows[i].Field<decimal>("imzLotSize");
				eRPPartOrgReferenceInformationDto.imzMinimumPurchaseQuantity = dataTable.Rows[i].Field<decimal>("imzMinimumPurchaseQuantity");
				eRPPartOrgReferenceInformationDto.imzOrganizationID = dataTable.Rows[i].Field<string>("imzOrganizationID");
				eRPPartOrgReferenceInformationDto.imzOrgPartID = dataTable.Rows[i].Field<string>("imzOrgPartID");
				eRPPartOrgReferenceInformationDto.imzOrgPartShortDescription = dataTable.Rows[i].Field<string>("imzOrgPartShortDescription");
				eRPPartOrgReferenceInformationDto.imzPartID = dataTable.Rows[i].Field<string>("imzPartID");
				eRPPartOrgReferenceInformationDto.imzPartRevisionID = dataTable.Rows[i].Field<string>("imzPartRevisionID");
				eRPPartOrgReferenceInformationDto.imzPurchaseUnitOfMeasure = dataTable.Rows[i].Field<string>("imzPurchaseUnitOfMeasure");
				eRPPartOrgReferenceInformationDto.imzRowVersion = dataTable.Rows[i].Field<byte[]>("imzRowVersion");
				eRPPartOrgReferenceInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartOrgReferenceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartOrgReferenceInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartOrgReferenceInformationDto> GetPartOrgReference(Guid partOrgReferenceId)
	{
		ERPPartOrgReferenceInformationDto eRPPartOrgReferenceInformationDto = new ERPPartOrgReferenceInformationDto();
		InitializeParameterLists();
		string[] collection = new string[17]
		{
			"imzConversionFactor", "imzCreatedBy", "imzCreatedDate", "imzUniqueID", "imzInactive", "imzPurchased", "imzSold", "imzLeadTime", "imzLotSize", "imzMinimumPurchaseQuantity",
			"imzOrganizationID", "imzOrgPartID", "imzOrgPartShortDescription", "imzPartID", "imzPartRevisionID", "imzPurchaseUnitOfMeasure", "imzRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imzUniqueID|C", partOrgReferenceId);
		AddCustomFieldsToSelectList("PartOrgReferences");
		using (DataTable dataTable = GetAsDataTable("PartOrgReferences", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartOrgReferenceInformationDto);
			}
			eRPPartOrgReferenceInformationDto.imzConversionFactor = dataTable.Rows[0].Field<decimal>("imzConversionFactor");
			eRPPartOrgReferenceInformationDto.imzCreatedBy = dataTable.Rows[0].Field<string>("imzCreatedBy");
			eRPPartOrgReferenceInformationDto.imzCreatedDate = dataTable.Rows[0].Field<DateTime?>("imzCreatedDate");
			eRPPartOrgReferenceInformationDto.imzUniqueID = dataTable.Rows[0].Field<Guid>("imzUniqueID");
			eRPPartOrgReferenceInformationDto.imzInactive = dataTable.Rows[0].Field<bool>("imzInactive");
			eRPPartOrgReferenceInformationDto.imzPurchased = dataTable.Rows[0].Field<bool>("imzPurchased");
			eRPPartOrgReferenceInformationDto.imzSold = dataTable.Rows[0].Field<bool>("imzSold");
			eRPPartOrgReferenceInformationDto.imzLeadTime = dataTable.Rows[0].Field<short>("imzLeadTime");
			eRPPartOrgReferenceInformationDto.imzLotSize = dataTable.Rows[0].Field<decimal>("imzLotSize");
			eRPPartOrgReferenceInformationDto.imzMinimumPurchaseQuantity = dataTable.Rows[0].Field<decimal>("imzMinimumPurchaseQuantity");
			eRPPartOrgReferenceInformationDto.imzOrganizationID = dataTable.Rows[0].Field<string>("imzOrganizationID");
			eRPPartOrgReferenceInformationDto.imzOrgPartID = dataTable.Rows[0].Field<string>("imzOrgPartID");
			eRPPartOrgReferenceInformationDto.imzOrgPartShortDescription = dataTable.Rows[0].Field<string>("imzOrgPartShortDescription");
			eRPPartOrgReferenceInformationDto.imzPartID = dataTable.Rows[0].Field<string>("imzPartID");
			eRPPartOrgReferenceInformationDto.imzPartRevisionID = dataTable.Rows[0].Field<string>("imzPartRevisionID");
			eRPPartOrgReferenceInformationDto.imzPurchaseUnitOfMeasure = dataTable.Rows[0].Field<string>("imzPurchaseUnitOfMeasure");
			eRPPartOrgReferenceInformationDto.imzRowVersion = dataTable.Rows[0].Field<byte[]>("imzRowVersion");
			eRPPartOrgReferenceInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartOrgReferenceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartOrgReferenceInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartOrgReference(ERPPartOrgReferenceDto partOrgReference)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartOrgReferences WHERE imzUniqueID = " + M1Util.ConvertToLinq(partOrgReference.imzUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imzPartID"] = partOrgReference.imzPartID.ToUpper();
				dataRow["imzPartRevisionID"] = partOrgReference.imzPartRevisionID.ToUpper();
				dataRow["imzOrganizationID"] = partOrgReference.imzOrganizationID.ToUpper();
				partOrgReference.imzUniqueID = ((partOrgReference.imzUniqueID == Guid.Empty) ? Guid.NewGuid() : partOrgReference.imzUniqueID);
				dataRow["imzUniqueID"] = partOrgReference.imzUniqueID;
				dataRow["imzCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imzCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartOrgReference could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partOrgReference.imzRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartOrgReference is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imzRowVersion"], partOrgReference.imzRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartOrgReference has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartOrgReference again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imzConversionFactor"] = partOrgReference.imzConversionFactor;
			dataRow["imzInactive"] = partOrgReference.imzInactive;
			dataRow["imzPurchased"] = partOrgReference.imzPurchased;
			dataRow["imzSold"] = partOrgReference.imzSold;
			dataRow["imzLeadTime"] = partOrgReference.imzLeadTime;
			dataRow["imzLotSize"] = partOrgReference.imzLotSize;
			dataRow["imzMinimumPurchaseQuantity"] = partOrgReference.imzMinimumPurchaseQuantity;
			dataRow["imzOrgPartID"] = partOrgReference.imzOrgPartID;
			dataRow["imzOrgPartShortDescription"] = partOrgReference.imzOrgPartShortDescription;
			dataRow["imzPurchaseUnitOfMeasure"] = partOrgReference.imzPurchaseUnitOfMeasure;
			if (partOrgReference.CustomFields != null && partOrgReference.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partOrgReference.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartOrgReference [{partOrgReference.imzUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartOrgReference [{partOrgReference.imzUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
