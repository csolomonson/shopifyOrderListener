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

public class ERPQuoteLineRepository : APIBaseRepository, IERPQuoteLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPQuoteLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesQuoteLineExist(Guid quoteLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("qmlUniqueID|C", quoteLineId);
		base.selectList.Add("qmlUniqueID");
		return Task.FromResult(GetAsObject("QuoteLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPQuoteLineInformationDto>> GetAllQuoteLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPQuoteLineInformationDto> collection = new List<ERPQuoteLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[41]
		{
			"qmlCreatedBy", "qmlCreatedDate", "qmlDocuments", "qmlUniqueID", "qmlClosed", "qmlCreatedFromMobile", "qmlFirm", "qmlMatrixCalculated", "qmlPurchaseToOrder", "qmlTransferredToOrder",
			"qmlLeadID", "qmlLeadLineID", "qmlNonTaxReasonID", "qmlOrgPartID", "qmlOrgPartShortDescription", "qmlPartGroupID", "qmlPartID", "qmlPartLongDescriptionRtf", "qmlPartLongDescriptionText", "qmlPartRevisionID",
			"qmlPartShortDescription", "qmlProductionNotesRTF", "qmlProductionNotesText", "qmlProjectAreaID", "qmlProjectID", "qmlPurchaseLocationID", "qmlPurchaseUnitCostBase", "qmlPurchaseUnitCostForeign", "qmlQuantityToTotal", "qmlQuoteID",
			"qmlQuoteMarkupType", "qmlResolutionReasonID", "qmlRowVersion", "qmlSecondTaxCodeID", "qmlQuoteLineID", "qmlSourceMethodID", "qmlSourceRevisionID", "qmlSupplierOrganizationID", "qmlTaxCodeID", "qmlTaxDate",
			"qmlUnitOfMeasure"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("QuoteLines");
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
		using (DataTable dataTable = GetAsDataTable("QuoteLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPQuoteLineInformationDto eRPQuoteLineInformationDto = new ERPQuoteLineInformationDto();
				eRPQuoteLineInformationDto.qmlCreatedBy = dataTable.Rows[i].Field<string>("qmlCreatedBy");
				eRPQuoteLineInformationDto.qmlCreatedDate = dataTable.Rows[i].Field<DateTime?>("qmlCreatedDate");
				eRPQuoteLineInformationDto.qmlDocuments = dataTable.Rows[i].Field<string>("qmlDocuments");
				eRPQuoteLineInformationDto.qmlUniqueID = dataTable.Rows[i].Field<Guid>("qmlUniqueID");
				eRPQuoteLineInformationDto.qmlClosed = dataTable.Rows[i].Field<bool>("qmlClosed");
				eRPQuoteLineInformationDto.qmlCreatedFromMobile = dataTable.Rows[i].Field<bool>("qmlCreatedFromMobile");
				eRPQuoteLineInformationDto.qmlFirm = dataTable.Rows[i].Field<bool>("qmlFirm");
				eRPQuoteLineInformationDto.qmlMatrixCalculated = dataTable.Rows[i].Field<bool>("qmlMatrixCalculated");
				eRPQuoteLineInformationDto.qmlPurchaseToOrder = dataTable.Rows[i].Field<bool>("qmlPurchaseToOrder");
				eRPQuoteLineInformationDto.qmlTransferredToOrder = dataTable.Rows[i].Field<bool>("qmlTransferredToOrder");
				eRPQuoteLineInformationDto.qmlLeadID = dataTable.Rows[i].Field<string>("qmlLeadID");
				eRPQuoteLineInformationDto.qmlLeadLineID = dataTable.Rows[i].Field<short>("qmlLeadLineID");
				eRPQuoteLineInformationDto.qmlNonTaxReasonID = dataTable.Rows[i].Field<string>("qmlNonTaxReasonID");
				eRPQuoteLineInformationDto.qmlOrgPartID = dataTable.Rows[i].Field<string>("qmlOrgPartID");
				eRPQuoteLineInformationDto.qmlOrgPartShortDescription = dataTable.Rows[i].Field<string>("qmlOrgPartShortDescription");
				eRPQuoteLineInformationDto.qmlPartGroupID = dataTable.Rows[i].Field<string>("qmlPartGroupID");
				eRPQuoteLineInformationDto.qmlPartID = dataTable.Rows[i].Field<string>("qmlPartID");
				eRPQuoteLineInformationDto.qmlPartLongDescriptionRtf = dataTable.Rows[i].Field<string>("qmlPartLongDescriptionRtf");
				eRPQuoteLineInformationDto.qmlPartLongDescriptionText = dataTable.Rows[i].Field<string>("qmlPartLongDescriptionText");
				eRPQuoteLineInformationDto.qmlPartRevisionID = dataTable.Rows[i].Field<string>("qmlPartRevisionID");
				eRPQuoteLineInformationDto.qmlPartShortDescription = dataTable.Rows[i].Field<string>("qmlPartShortDescription");
				eRPQuoteLineInformationDto.qmlProductionNotesRTF = dataTable.Rows[i].Field<string>("qmlProductionNotesRTF");
				eRPQuoteLineInformationDto.qmlProductionNotesText = dataTable.Rows[i].Field<string>("qmlProductionNotesText");
				eRPQuoteLineInformationDto.qmlProjectAreaID = dataTable.Rows[i].Field<string>("qmlProjectAreaID");
				eRPQuoteLineInformationDto.qmlProjectID = dataTable.Rows[i].Field<string>("qmlProjectID");
				eRPQuoteLineInformationDto.qmlPurchaseLocationID = dataTable.Rows[i].Field<string>("qmlPurchaseLocationID");
				eRPQuoteLineInformationDto.qmlPurchaseUnitCostBase = dataTable.Rows[i].Field<decimal>("qmlPurchaseUnitCostBase");
				eRPQuoteLineInformationDto.qmlPurchaseUnitCostForeign = dataTable.Rows[i].Field<decimal>("qmlPurchaseUnitCostForeign");
				eRPQuoteLineInformationDto.qmlQuantityToTotal = dataTable.Rows[i].Field<byte>("qmlQuantityToTotal");
				eRPQuoteLineInformationDto.qmlQuoteID = dataTable.Rows[i].Field<string>("qmlQuoteID");
				eRPQuoteLineInformationDto.qmlQuoteMarkupType = dataTable.Rows[i].Field<byte>("qmlQuoteMarkupType");
				eRPQuoteLineInformationDto.qmlResolutionReasonID = dataTable.Rows[i].Field<string>("qmlResolutionReasonID");
				eRPQuoteLineInformationDto.qmlRowVersion = dataTable.Rows[i].Field<byte[]>("qmlRowVersion");
				eRPQuoteLineInformationDto.qmlSecondTaxCodeID = dataTable.Rows[i].Field<string>("qmlSecondTaxCodeID");
				eRPQuoteLineInformationDto.qmlQuoteLineID = dataTable.Rows[i].Field<short>("qmlQuoteLineID");
				eRPQuoteLineInformationDto.qmlSourceMethodID = dataTable.Rows[i].Field<string>("qmlSourceMethodID");
				eRPQuoteLineInformationDto.qmlSourceRevisionID = dataTable.Rows[i].Field<string>("qmlSourceRevisionID");
				eRPQuoteLineInformationDto.qmlSupplierOrganizationID = dataTable.Rows[i].Field<string>("qmlSupplierOrganizationID");
				eRPQuoteLineInformationDto.qmlTaxCodeID = dataTable.Rows[i].Field<string>("qmlTaxCodeID");
				eRPQuoteLineInformationDto.qmlTaxDate = dataTable.Rows[i].Field<DateTime?>("qmlTaxDate");
				eRPQuoteLineInformationDto.qmlUnitOfMeasure = dataTable.Rows[i].Field<string>("qmlUnitOfMeasure");
				eRPQuoteLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPQuoteLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPQuoteLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPQuoteLineInformationDto> GetQuoteLine(Guid quoteLineId)
	{
		ERPQuoteLineInformationDto eRPQuoteLineInformationDto = new ERPQuoteLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[41]
		{
			"qmlCreatedBy", "qmlCreatedDate", "qmlDocuments", "qmlUniqueID", "qmlClosed", "qmlCreatedFromMobile", "qmlFirm", "qmlMatrixCalculated", "qmlPurchaseToOrder", "qmlTransferredToOrder",
			"qmlLeadID", "qmlLeadLineID", "qmlNonTaxReasonID", "qmlOrgPartID", "qmlOrgPartShortDescription", "qmlPartGroupID", "qmlPartID", "qmlPartLongDescriptionRtf", "qmlPartLongDescriptionText", "qmlPartRevisionID",
			"qmlPartShortDescription", "qmlProductionNotesRTF", "qmlProductionNotesText", "qmlProjectAreaID", "qmlProjectID", "qmlPurchaseLocationID", "qmlPurchaseUnitCostBase", "qmlPurchaseUnitCostForeign", "qmlQuantityToTotal", "qmlQuoteID",
			"qmlQuoteMarkupType", "qmlResolutionReasonID", "qmlRowVersion", "qmlSecondTaxCodeID", "qmlQuoteLineID", "qmlSourceMethodID", "qmlSourceRevisionID", "qmlSupplierOrganizationID", "qmlTaxCodeID", "qmlTaxDate",
			"qmlUnitOfMeasure"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("qmlUniqueID|C", quoteLineId);
		AddCustomFieldsToSelectList("QuoteLines");
		using (DataTable dataTable = GetAsDataTable("QuoteLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPQuoteLineInformationDto);
			}
			eRPQuoteLineInformationDto.qmlCreatedBy = dataTable.Rows[0].Field<string>("qmlCreatedBy");
			eRPQuoteLineInformationDto.qmlCreatedDate = dataTable.Rows[0].Field<DateTime?>("qmlCreatedDate");
			eRPQuoteLineInformationDto.qmlDocuments = dataTable.Rows[0].Field<string>("qmlDocuments");
			eRPQuoteLineInformationDto.qmlUniqueID = dataTable.Rows[0].Field<Guid>("qmlUniqueID");
			eRPQuoteLineInformationDto.qmlClosed = dataTable.Rows[0].Field<bool>("qmlClosed");
			eRPQuoteLineInformationDto.qmlCreatedFromMobile = dataTable.Rows[0].Field<bool>("qmlCreatedFromMobile");
			eRPQuoteLineInformationDto.qmlFirm = dataTable.Rows[0].Field<bool>("qmlFirm");
			eRPQuoteLineInformationDto.qmlMatrixCalculated = dataTable.Rows[0].Field<bool>("qmlMatrixCalculated");
			eRPQuoteLineInformationDto.qmlPurchaseToOrder = dataTable.Rows[0].Field<bool>("qmlPurchaseToOrder");
			eRPQuoteLineInformationDto.qmlTransferredToOrder = dataTable.Rows[0].Field<bool>("qmlTransferredToOrder");
			eRPQuoteLineInformationDto.qmlLeadID = dataTable.Rows[0].Field<string>("qmlLeadID");
			eRPQuoteLineInformationDto.qmlLeadLineID = dataTable.Rows[0].Field<short>("qmlLeadLineID");
			eRPQuoteLineInformationDto.qmlNonTaxReasonID = dataTable.Rows[0].Field<string>("qmlNonTaxReasonID");
			eRPQuoteLineInformationDto.qmlOrgPartID = dataTable.Rows[0].Field<string>("qmlOrgPartID");
			eRPQuoteLineInformationDto.qmlOrgPartShortDescription = dataTable.Rows[0].Field<string>("qmlOrgPartShortDescription");
			eRPQuoteLineInformationDto.qmlPartGroupID = dataTable.Rows[0].Field<string>("qmlPartGroupID");
			eRPQuoteLineInformationDto.qmlPartID = dataTable.Rows[0].Field<string>("qmlPartID");
			eRPQuoteLineInformationDto.qmlPartLongDescriptionRtf = dataTable.Rows[0].Field<string>("qmlPartLongDescriptionRtf");
			eRPQuoteLineInformationDto.qmlPartLongDescriptionText = dataTable.Rows[0].Field<string>("qmlPartLongDescriptionText");
			eRPQuoteLineInformationDto.qmlPartRevisionID = dataTable.Rows[0].Field<string>("qmlPartRevisionID");
			eRPQuoteLineInformationDto.qmlPartShortDescription = dataTable.Rows[0].Field<string>("qmlPartShortDescription");
			eRPQuoteLineInformationDto.qmlProductionNotesRTF = dataTable.Rows[0].Field<string>("qmlProductionNotesRTF");
			eRPQuoteLineInformationDto.qmlProductionNotesText = dataTable.Rows[0].Field<string>("qmlProductionNotesText");
			eRPQuoteLineInformationDto.qmlProjectAreaID = dataTable.Rows[0].Field<string>("qmlProjectAreaID");
			eRPQuoteLineInformationDto.qmlProjectID = dataTable.Rows[0].Field<string>("qmlProjectID");
			eRPQuoteLineInformationDto.qmlPurchaseLocationID = dataTable.Rows[0].Field<string>("qmlPurchaseLocationID");
			eRPQuoteLineInformationDto.qmlPurchaseUnitCostBase = dataTable.Rows[0].Field<decimal>("qmlPurchaseUnitCostBase");
			eRPQuoteLineInformationDto.qmlPurchaseUnitCostForeign = dataTable.Rows[0].Field<decimal>("qmlPurchaseUnitCostForeign");
			eRPQuoteLineInformationDto.qmlQuantityToTotal = dataTable.Rows[0].Field<byte>("qmlQuantityToTotal");
			eRPQuoteLineInformationDto.qmlQuoteID = dataTable.Rows[0].Field<string>("qmlQuoteID");
			eRPQuoteLineInformationDto.qmlQuoteMarkupType = dataTable.Rows[0].Field<byte>("qmlQuoteMarkupType");
			eRPQuoteLineInformationDto.qmlResolutionReasonID = dataTable.Rows[0].Field<string>("qmlResolutionReasonID");
			eRPQuoteLineInformationDto.qmlRowVersion = dataTable.Rows[0].Field<byte[]>("qmlRowVersion");
			eRPQuoteLineInformationDto.qmlSecondTaxCodeID = dataTable.Rows[0].Field<string>("qmlSecondTaxCodeID");
			eRPQuoteLineInformationDto.qmlQuoteLineID = dataTable.Rows[0].Field<short>("qmlQuoteLineID");
			eRPQuoteLineInformationDto.qmlSourceMethodID = dataTable.Rows[0].Field<string>("qmlSourceMethodID");
			eRPQuoteLineInformationDto.qmlSourceRevisionID = dataTable.Rows[0].Field<string>("qmlSourceRevisionID");
			eRPQuoteLineInformationDto.qmlSupplierOrganizationID = dataTable.Rows[0].Field<string>("qmlSupplierOrganizationID");
			eRPQuoteLineInformationDto.qmlTaxCodeID = dataTable.Rows[0].Field<string>("qmlTaxCodeID");
			eRPQuoteLineInformationDto.qmlTaxDate = dataTable.Rows[0].Field<DateTime?>("qmlTaxDate");
			eRPQuoteLineInformationDto.qmlUnitOfMeasure = dataTable.Rows[0].Field<string>("qmlUnitOfMeasure");
			eRPQuoteLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPQuoteLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPQuoteLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveQuoteLine(ERPQuoteLineDto quoteLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM QuoteLines WHERE qmlUniqueID = " + M1Util.ConvertToLinq(quoteLine.qmlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qmlQuoteID"] = quoteLine.qmlQuoteID.ToUpper();
				dataRow["qmlQuoteLineID"] = quoteLine.qmlQuoteLineID;
				quoteLine.qmlUniqueID = ((quoteLine.qmlUniqueID == Guid.Empty) ? Guid.NewGuid() : quoteLine.qmlUniqueID);
				dataRow["qmlUniqueID"] = quoteLine.qmlUniqueID;
				dataRow["qmlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qmlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The QuoteLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (quoteLine.qmlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the QuoteLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["qmlRowVersion"], quoteLine.qmlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the QuoteLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the QuoteLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qmlDocuments"] = quoteLine.qmlDocuments ?? dataRow["qmlDocuments"];
			dataRow["qmlClosed"] = quoteLine.qmlClosed;
			dataRow["qmlCreatedFromMobile"] = quoteLine.qmlCreatedFromMobile;
			dataRow["qmlFirm"] = quoteLine.qmlFirm;
			dataRow["qmlMatrixCalculated"] = quoteLine.qmlMatrixCalculated;
			dataRow["qmlPurchaseToOrder"] = quoteLine.qmlPurchaseToOrder;
			dataRow["qmlTransferredToOrder"] = quoteLine.qmlTransferredToOrder;
			dataRow["qmlLeadID"] = quoteLine.qmlLeadID;
			dataRow["qmlLeadLineID"] = quoteLine.qmlLeadLineID;
			dataRow["qmlNonTaxReasonID"] = quoteLine.qmlNonTaxReasonID;
			dataRow["qmlOrgPartID"] = quoteLine.qmlOrgPartID;
			dataRow["qmlOrgPartShortDescription"] = quoteLine.qmlOrgPartShortDescription;
			dataRow["qmlPartGroupID"] = quoteLine.qmlPartGroupID;
			dataRow["qmlPartID"] = quoteLine.qmlPartID;
			dataRow["qmlPartLongDescriptionRtf"] = quoteLine.qmlPartLongDescriptionRtf ?? dataRow["qmlPartLongDescriptionRtf"];
			dataRow["qmlPartLongDescriptionText"] = quoteLine.qmlPartLongDescriptionText ?? dataRow["qmlPartLongDescriptionText"];
			dataRow["qmlPartRevisionID"] = quoteLine.qmlPartRevisionID;
			dataRow["qmlPartShortDescription"] = quoteLine.qmlPartShortDescription;
			dataRow["qmlProductionNotesRTF"] = quoteLine.qmlProductionNotesRTF ?? dataRow["qmlProductionNotesRTF"];
			dataRow["qmlProductionNotesText"] = quoteLine.qmlProductionNotesText ?? dataRow["qmlProductionNotesText"];
			dataRow["qmlProjectAreaID"] = quoteLine.qmlProjectAreaID;
			dataRow["qmlProjectID"] = quoteLine.qmlProjectID;
			dataRow["qmlPurchaseLocationID"] = quoteLine.qmlPurchaseLocationID;
			dataRow["qmlPurchaseUnitCostBase"] = quoteLine.qmlPurchaseUnitCostBase;
			dataRow["qmlPurchaseUnitCostForeign"] = quoteLine.qmlPurchaseUnitCostForeign;
			dataRow["qmlQuantityToTotal"] = quoteLine.qmlQuantityToTotal;
			dataRow["qmlQuoteMarkupType"] = quoteLine.qmlQuoteMarkupType;
			dataRow["qmlResolutionReasonID"] = quoteLine.qmlResolutionReasonID;
			dataRow["qmlSecondTaxCodeID"] = quoteLine.qmlSecondTaxCodeID;
			dataRow["qmlSourceMethodID"] = quoteLine.qmlSourceMethodID;
			dataRow["qmlSourceRevisionID"] = quoteLine.qmlSourceRevisionID;
			dataRow["qmlSupplierOrganizationID"] = quoteLine.qmlSupplierOrganizationID;
			dataRow["qmlTaxCodeID"] = quoteLine.qmlTaxCodeID;
			DataRow dataRow2 = dataRow;
			DateTime? qmlTaxDate = quoteLine.qmlTaxDate;
			dataRow2["qmlTaxDate"] = (qmlTaxDate.HasValue ? ((object)qmlTaxDate.GetValueOrDefault()) : dataRow["qmlTaxDate"]);
			dataRow["qmlUnitOfMeasure"] = quoteLine.qmlUnitOfMeasure;
			if (quoteLine.CustomFields != null && quoteLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in quoteLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the QuoteLine [{quoteLine.qmlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the QuoteLine [{quoteLine.qmlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
