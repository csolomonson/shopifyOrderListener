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

public class ERPGLJournalLineRepository : APIBaseRepository, IERPGLJournalLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPGLJournalLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesGLJournalLineExist(Guid gLJournalLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("gllUniqueID|C", gLJournalLineId);
		base.selectList.Add("gllUniqueID");
		return Task.FromResult(GetAsObject("GLJournalLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPGLJournalLineInformationDto>> GetAllGLJournalLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPGLJournalLineInformationDto> collection = new List<ERPGLJournalLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[31]
		{
			"gllArPaymentHeaderID", "gllArPaymentSessionID", "gllCreatedBy", "gllCreatedDate", "gllCreditAmount", "gllDebitAmount", "gllDescription", "gllUniqueID", "gllGlAccountID", "gllGlFiscalYearID",
			"gllGlFiscalYearPeriodID", "gllGlJournalID", "gllPosted", "gllJobAssemblyID", "gllJobID", "gllJobMaterialComponentID", "gllJobMaterialID", "gllJobOperationID", "gllLocationID", "gllOrganizationID",
			"gllPartTransactionID", "gllReference", "gllRowVersion", "gllGlJournalLineID", "gllSourceTableName", "gllSourceTableUniqueID", "gllTaxableAmount", "gllTaxCodeID", "gllTransactionAmount", "gllTransactionDate",
			"gllTransactionType"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("GLJournalLines");
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
		using (DataTable dataTable = GetAsDataTable("GLJournalLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPGLJournalLineInformationDto eRPGLJournalLineInformationDto = new ERPGLJournalLineInformationDto();
				eRPGLJournalLineInformationDto.gllArPaymentHeaderID = dataTable.Rows[i].Field<int>("gllArPaymentHeaderID");
				eRPGLJournalLineInformationDto.gllArPaymentSessionID = dataTable.Rows[i].Field<int>("gllArPaymentSessionID");
				eRPGLJournalLineInformationDto.gllCreatedBy = dataTable.Rows[i].Field<string>("gllCreatedBy");
				eRPGLJournalLineInformationDto.gllCreatedDate = dataTable.Rows[i].Field<DateTime?>("gllCreatedDate");
				eRPGLJournalLineInformationDto.gllCreditAmount = dataTable.Rows[i].Field<decimal>("gllCreditAmount");
				eRPGLJournalLineInformationDto.gllDebitAmount = dataTable.Rows[i].Field<decimal>("gllDebitAmount");
				eRPGLJournalLineInformationDto.gllDescription = dataTable.Rows[i].Field<string>("gllDescription");
				eRPGLJournalLineInformationDto.gllUniqueID = dataTable.Rows[i].Field<Guid>("gllUniqueID");
				eRPGLJournalLineInformationDto.gllGlAccountID = dataTable.Rows[i].Field<string>("gllGlAccountID");
				eRPGLJournalLineInformationDto.gllGlFiscalYearID = dataTable.Rows[i].Field<short>("gllGlFiscalYearID");
				eRPGLJournalLineInformationDto.gllGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("gllGlFiscalYearPeriodID");
				eRPGLJournalLineInformationDto.gllGlJournalID = dataTable.Rows[i].Field<int>("gllGlJournalID");
				eRPGLJournalLineInformationDto.gllPosted = dataTable.Rows[i].Field<bool>("gllPosted");
				eRPGLJournalLineInformationDto.gllJobAssemblyID = dataTable.Rows[i].Field<int>("gllJobAssemblyID");
				eRPGLJournalLineInformationDto.gllJobID = dataTable.Rows[i].Field<string>("gllJobID");
				eRPGLJournalLineInformationDto.gllJobMaterialComponentID = dataTable.Rows[i].Field<int>("gllJobMaterialComponentID");
				eRPGLJournalLineInformationDto.gllJobMaterialID = dataTable.Rows[i].Field<int>("gllJobMaterialID");
				eRPGLJournalLineInformationDto.gllJobOperationID = dataTable.Rows[i].Field<int>("gllJobOperationID");
				eRPGLJournalLineInformationDto.gllLocationID = dataTable.Rows[i].Field<string>("gllLocationID");
				eRPGLJournalLineInformationDto.gllOrganizationID = dataTable.Rows[i].Field<string>("gllOrganizationID");
				eRPGLJournalLineInformationDto.gllPartTransactionID = dataTable.Rows[i].Field<int>("gllPartTransactionID");
				eRPGLJournalLineInformationDto.gllReference = dataTable.Rows[i].Field<string>("gllReference");
				eRPGLJournalLineInformationDto.gllRowVersion = dataTable.Rows[i].Field<byte[]>("gllRowVersion");
				eRPGLJournalLineInformationDto.gllGlJournalLineID = dataTable.Rows[i].Field<int>("gllGlJournalLineID");
				eRPGLJournalLineInformationDto.gllSourceTableName = dataTable.Rows[i].Field<string>("gllSourceTableName");
				eRPGLJournalLineInformationDto.gllSourceTableUniqueID = dataTable.Rows[i].Field<Guid>("gllSourceTableUniqueID");
				eRPGLJournalLineInformationDto.gllTaxableAmount = dataTable.Rows[i].Field<decimal>("gllTaxableAmount");
				eRPGLJournalLineInformationDto.gllTaxCodeID = dataTable.Rows[i].Field<string>("gllTaxCodeID");
				eRPGLJournalLineInformationDto.gllTransactionAmount = dataTable.Rows[i].Field<decimal>("gllTransactionAmount");
				eRPGLJournalLineInformationDto.gllTransactionDate = dataTable.Rows[i].Field<DateTime?>("gllTransactionDate");
				eRPGLJournalLineInformationDto.gllTransactionType = dataTable.Rows[i].Field<byte>("gllTransactionType");
				eRPGLJournalLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPGLJournalLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPGLJournalLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPGLJournalLineInformationDto> GetGLJournalLine(Guid gLJournalLineId)
	{
		ERPGLJournalLineInformationDto eRPGLJournalLineInformationDto = new ERPGLJournalLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[31]
		{
			"gllArPaymentHeaderID", "gllArPaymentSessionID", "gllCreatedBy", "gllCreatedDate", "gllCreditAmount", "gllDebitAmount", "gllDescription", "gllUniqueID", "gllGlAccountID", "gllGlFiscalYearID",
			"gllGlFiscalYearPeriodID", "gllGlJournalID", "gllPosted", "gllJobAssemblyID", "gllJobID", "gllJobMaterialComponentID", "gllJobMaterialID", "gllJobOperationID", "gllLocationID", "gllOrganizationID",
			"gllPartTransactionID", "gllReference", "gllRowVersion", "gllGlJournalLineID", "gllSourceTableName", "gllSourceTableUniqueID", "gllTaxableAmount", "gllTaxCodeID", "gllTransactionAmount", "gllTransactionDate",
			"gllTransactionType"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("gllUniqueID|C", gLJournalLineId);
		AddCustomFieldsToSelectList("GLJournalLines");
		using (DataTable dataTable = GetAsDataTable("GLJournalLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPGLJournalLineInformationDto);
			}
			eRPGLJournalLineInformationDto.gllArPaymentHeaderID = dataTable.Rows[0].Field<int>("gllArPaymentHeaderID");
			eRPGLJournalLineInformationDto.gllArPaymentSessionID = dataTable.Rows[0].Field<int>("gllArPaymentSessionID");
			eRPGLJournalLineInformationDto.gllCreatedBy = dataTable.Rows[0].Field<string>("gllCreatedBy");
			eRPGLJournalLineInformationDto.gllCreatedDate = dataTable.Rows[0].Field<DateTime?>("gllCreatedDate");
			eRPGLJournalLineInformationDto.gllCreditAmount = dataTable.Rows[0].Field<decimal>("gllCreditAmount");
			eRPGLJournalLineInformationDto.gllDebitAmount = dataTable.Rows[0].Field<decimal>("gllDebitAmount");
			eRPGLJournalLineInformationDto.gllDescription = dataTable.Rows[0].Field<string>("gllDescription");
			eRPGLJournalLineInformationDto.gllUniqueID = dataTable.Rows[0].Field<Guid>("gllUniqueID");
			eRPGLJournalLineInformationDto.gllGlAccountID = dataTable.Rows[0].Field<string>("gllGlAccountID");
			eRPGLJournalLineInformationDto.gllGlFiscalYearID = dataTable.Rows[0].Field<short>("gllGlFiscalYearID");
			eRPGLJournalLineInformationDto.gllGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("gllGlFiscalYearPeriodID");
			eRPGLJournalLineInformationDto.gllGlJournalID = dataTable.Rows[0].Field<int>("gllGlJournalID");
			eRPGLJournalLineInformationDto.gllPosted = dataTable.Rows[0].Field<bool>("gllPosted");
			eRPGLJournalLineInformationDto.gllJobAssemblyID = dataTable.Rows[0].Field<int>("gllJobAssemblyID");
			eRPGLJournalLineInformationDto.gllJobID = dataTable.Rows[0].Field<string>("gllJobID");
			eRPGLJournalLineInformationDto.gllJobMaterialComponentID = dataTable.Rows[0].Field<int>("gllJobMaterialComponentID");
			eRPGLJournalLineInformationDto.gllJobMaterialID = dataTable.Rows[0].Field<int>("gllJobMaterialID");
			eRPGLJournalLineInformationDto.gllJobOperationID = dataTable.Rows[0].Field<int>("gllJobOperationID");
			eRPGLJournalLineInformationDto.gllLocationID = dataTable.Rows[0].Field<string>("gllLocationID");
			eRPGLJournalLineInformationDto.gllOrganizationID = dataTable.Rows[0].Field<string>("gllOrganizationID");
			eRPGLJournalLineInformationDto.gllPartTransactionID = dataTable.Rows[0].Field<int>("gllPartTransactionID");
			eRPGLJournalLineInformationDto.gllReference = dataTable.Rows[0].Field<string>("gllReference");
			eRPGLJournalLineInformationDto.gllRowVersion = dataTable.Rows[0].Field<byte[]>("gllRowVersion");
			eRPGLJournalLineInformationDto.gllGlJournalLineID = dataTable.Rows[0].Field<int>("gllGlJournalLineID");
			eRPGLJournalLineInformationDto.gllSourceTableName = dataTable.Rows[0].Field<string>("gllSourceTableName");
			eRPGLJournalLineInformationDto.gllSourceTableUniqueID = dataTable.Rows[0].Field<Guid>("gllSourceTableUniqueID");
			eRPGLJournalLineInformationDto.gllTaxableAmount = dataTable.Rows[0].Field<decimal>("gllTaxableAmount");
			eRPGLJournalLineInformationDto.gllTaxCodeID = dataTable.Rows[0].Field<string>("gllTaxCodeID");
			eRPGLJournalLineInformationDto.gllTransactionAmount = dataTable.Rows[0].Field<decimal>("gllTransactionAmount");
			eRPGLJournalLineInformationDto.gllTransactionDate = dataTable.Rows[0].Field<DateTime?>("gllTransactionDate");
			eRPGLJournalLineInformationDto.gllTransactionType = dataTable.Rows[0].Field<byte>("gllTransactionType");
			eRPGLJournalLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPGLJournalLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPGLJournalLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveGLJournalLine(ERPGLJournalLineDto gLJournalLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM GLJournalLines WHERE gllUniqueID = " + M1Util.ConvertToLinq(gLJournalLine.gllUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["gllGlJournalID"] = gLJournalLine.gllGlJournalID;
				dataRow["gllGlJournalLineID"] = gLJournalLine.gllGlJournalLineID;
				gLJournalLine.gllUniqueID = ((gLJournalLine.gllUniqueID == Guid.Empty) ? Guid.NewGuid() : gLJournalLine.gllUniqueID);
				dataRow["gllUniqueID"] = gLJournalLine.gllUniqueID;
				dataRow["gllCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["gllCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The GLJournalLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (gLJournalLine.gllRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the GLJournalLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["gllRowVersion"], gLJournalLine.gllRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the GLJournalLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the GLJournalLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["gllArPaymentHeaderID"] = gLJournalLine.gllArPaymentHeaderID;
			dataRow["gllArPaymentSessionID"] = gLJournalLine.gllArPaymentSessionID;
			dataRow["gllCreditAmount"] = gLJournalLine.gllCreditAmount;
			dataRow["gllDebitAmount"] = gLJournalLine.gllDebitAmount;
			dataRow["gllDescription"] = gLJournalLine.gllDescription;
			dataRow["gllGlAccountID"] = gLJournalLine.gllGlAccountID;
			dataRow["gllGlFiscalYearID"] = gLJournalLine.gllGlFiscalYearID;
			dataRow["gllGlFiscalYearPeriodID"] = gLJournalLine.gllGlFiscalYearPeriodID;
			dataRow["gllPosted"] = gLJournalLine.gllPosted;
			dataRow["gllJobAssemblyID"] = gLJournalLine.gllJobAssemblyID;
			dataRow["gllJobID"] = gLJournalLine.gllJobID;
			dataRow["gllJobMaterialComponentID"] = gLJournalLine.gllJobMaterialComponentID;
			dataRow["gllJobMaterialID"] = gLJournalLine.gllJobMaterialID;
			dataRow["gllJobOperationID"] = gLJournalLine.gllJobOperationID;
			dataRow["gllLocationID"] = gLJournalLine.gllLocationID;
			dataRow["gllOrganizationID"] = gLJournalLine.gllOrganizationID;
			dataRow["gllPartTransactionID"] = gLJournalLine.gllPartTransactionID;
			dataRow["gllReference"] = gLJournalLine.gllReference;
			dataRow["gllSourceTableName"] = gLJournalLine.gllSourceTableName;
			dataRow["gllSourceTableUniqueID"] = gLJournalLine.gllSourceTableUniqueID;
			dataRow["gllTaxableAmount"] = gLJournalLine.gllTaxableAmount;
			dataRow["gllTaxCodeID"] = gLJournalLine.gllTaxCodeID;
			dataRow["gllTransactionAmount"] = gLJournalLine.gllTransactionAmount;
			DataRow dataRow2 = dataRow;
			DateTime? gllTransactionDate = gLJournalLine.gllTransactionDate;
			dataRow2["gllTransactionDate"] = (gllTransactionDate.HasValue ? ((object)gllTransactionDate.GetValueOrDefault()) : dataRow["gllTransactionDate"]);
			dataRow["gllTransactionType"] = gLJournalLine.gllTransactionType;
			if (gLJournalLine.CustomFields != null && gLJournalLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in gLJournalLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the GLJournalLine [{gLJournalLine.gllUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the GLJournalLine [{gLJournalLine.gllUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
