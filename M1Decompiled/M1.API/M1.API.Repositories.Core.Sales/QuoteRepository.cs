using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;
using M1.API.Utilities;
using M1.Core;
using M1.Extensions;

namespace M1.API.Repositories.Core.Sales;

public class QuoteRepository : APIBaseRepository, IQuoteRepository, IAPIBaseRepository, IDisposable
{
	private readonly string[] quoteFields = new string[16]
	{
		"qmpQuoteID", "qmpCustomerOrganizationID", "qmpPlantID", "qmpQuoterEmployeeID", "qmpQuoteDate", "qmpDueDate", "qmpExpirationDate", "qmpProjectID", "qmpClosed", "qmpClosedDate",
		"qmpPaymentTermID", "qmpShippingMethodID", "qmpCreatedBy", "qmpCreatedDate", "qmpUniqueID", "qmpRowVersion"
	};

	private readonly string GET_QUOTE_LINES_BY_QUOTE_ID = "SELECT qmlQuoteID, qmlQuoteLineID, qmlPartID, qmlPartRevisionID, \r\n                                                qmlUnitofMeasure, qmlPartGroupID, qmlPartShortDescription, qmlOrgPartShortDescription, \r\n                                                qmlResolutionReasonID, qmlQuoteMarkupType, qmlPurchaseToOrder, qmlPurchaseUnitCostForeign, \r\n                                                qmlSupplierOrganizationID, qmlPurchaseLocationID, qmlFirm, qmlProjectID, qmlProjectAreaID, \r\n                                                qmlClosed, qmlCreatedBy, qmlCreatedDate, qmlUniqueID, qmlRowVersion\r\n                                                FROM QuoteLines WHERE qmlQuoteID=@p1";

	public QuoteRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public Task<bool> DoesQuoteExistsAsync(string quoteId)
	{
		InitializeParameterLists();
		base.filterList.Add("qmpQuoteID|C", quoteId);
		base.selectList.Add("qmpQuoteID");
		return Task.FromResult(GetAsObject("Quotes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<string> GetQuoteIdFromGuidAsync(Guid guidOut)
	{
		InitializeParameterLists();
		base.filterList.Add("qmpUniqueID|C", guidOut);
		base.selectList.Add("qmpQuoteID");
		return Task.FromResult(GetAsObject("Quotes", base.filterList, base.selectList, null, null)?.ToString());
	}

	public Task<ICollection<BOMQuoteDto>> GetAllQuotesAsync(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<BOMQuoteDto> collection = new List<BOMQuoteDto>();
		InitializeParameterLists();
		base.selectList.AddRange(quoteFields);
		List<string> orderbyList = new List<string> { "qmpQuoteID" };
		using (DataTable dataTable = GetAsDataTable("Quotes", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				BOMQuoteDto bOMQuoteDto = new BOMQuoteDto();
				bOMQuoteDto.QuoteID = dataTable.Rows[i].Field<string>("qmpQuoteID");
				bOMQuoteDto.CustomerOrganizationID = dataTable.Rows[i].Field<string>("qmpCustomerOrganizationID");
				bOMQuoteDto.PlantID = dataTable.Rows[i].Field<string>("qmpPlantID");
				bOMQuoteDto.QuoterEmployeeID = dataTable.Rows[i].Field<string>("qmpQuoterEmployeeID");
				bOMQuoteDto.QuoteDate = dataTable.Rows[i].Field<DateTime?>("qmpQuoteDate");
				bOMQuoteDto.DueDate = dataTable.Rows[i].Field<DateTime?>("qmpDueDate");
				bOMQuoteDto.ExpirationDate = dataTable.Rows[i].Field<DateTime?>("qmpExpirationDate");
				bOMQuoteDto.ProjectID = dataTable.Rows[i].Field<string>("qmpProjectID");
				bOMQuoteDto.Closed = dataTable.Rows[i].Field<bool>("qmpClosed");
				bOMQuoteDto.ClosedDate = dataTable.Rows[i].Field<DateTime?>("qmpClosedDate");
				bOMQuoteDto.PaymentTermID = dataTable.Rows[0].Field<string>("qmpPaymentTermID");
				bOMQuoteDto.ShippingMethodID = dataTable.Rows[i].Field<string>("qmpShippingMethodID");
				bOMQuoteDto.CreatedBy = dataTable.Rows[i].Field<string>("qmpCreatedBy");
				bOMQuoteDto.CreatedDate = ((!dataTable.Rows[i].Field<DateTime?>("qmpCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[i].Field<DateTime?>("qmpCreatedDate"));
				bOMQuoteDto.UniqueID = dataTable.Rows[i].Field<Guid>("qmpUniqueID");
				bOMQuoteDto.RowVersion = dataTable.Rows[0].Field<byte[]>("qmpRowVersion");
				collection.Add(bOMQuoteDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<BOMQuoteDto> GetQuoteAsync(string quoteId)
	{
		BOMQuoteDto bOMQuoteDto = new BOMQuoteDto();
		InitializeParameterLists();
		base.selectList.AddRange(quoteFields);
		base.filterList.Add(Guid.TryParse(quoteId, out var _) ? "qmpUniqueID|C" : "qmpQuoteID|C", quoteId);
		using (DataTable dataTable = GetAsDataTable("Quotes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(bOMQuoteDto);
			}
			bOMQuoteDto.QuoteID = dataTable.Rows[0].Field<string>("qmpQuoteID");
			bOMQuoteDto.CustomerOrganizationID = dataTable.Rows[0].Field<string>("qmpCustomerOrganizationID");
			bOMQuoteDto.PlantID = dataTable.Rows[0].Field<string>("qmpPlantID");
			bOMQuoteDto.QuoterEmployeeID = dataTable.Rows[0].Field<string>("qmpQuoterEmployeeID");
			bOMQuoteDto.QuoteDate = dataTable.Rows[0].Field<DateTime?>("qmpQuoteDate");
			bOMQuoteDto.DueDate = dataTable.Rows[0].Field<DateTime?>("qmpDueDate");
			bOMQuoteDto.ExpirationDate = dataTable.Rows[0].Field<DateTime?>("qmpExpirationDate");
			bOMQuoteDto.ProjectID = dataTable.Rows[0].Field<string>("qmpProjectID");
			bOMQuoteDto.Closed = dataTable.Rows[0].Field<bool>("qmpClosed");
			bOMQuoteDto.ClosedDate = dataTable.Rows[0].Field<DateTime?>("qmpClosedDate");
			bOMQuoteDto.PaymentTermID = dataTable.Rows[0].Field<string>("qmpPaymentTermID");
			bOMQuoteDto.ShippingMethodID = dataTable.Rows[0].Field<string>("qmpShippingMethodID");
			bOMQuoteDto.CreatedBy = dataTable.Rows[0].Field<string>("qmpCreatedBy");
			bOMQuoteDto.CreatedDate = ((!dataTable.Rows[0].Field<DateTime?>("qmpCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[0].Field<DateTime?>("qmpCreatedDate"));
			bOMQuoteDto.UniqueID = dataTable.Rows[0].Field<Guid>("qmpUniqueID");
			bOMQuoteDto.RowVersion = dataTable.Rows[0].Field<byte[]>("qmpRowVersion");
		}
		return Task.FromResult(bOMQuoteDto);
	}

	public Task<IList<BOMQuoteLineDto>> GetQuoteLinesInfoAsync(string quoteId)
	{
		IList<BOMQuoteLineDto> list = new List<BOMQuoteLineDto>();
		InitializeParameterLists();
		base.filterList.Add("@p1", quoteId);
		using (DataTable dataTable = GetAsDataTable(GET_QUOTE_LINES_BY_QUOTE_ID, base.filterList, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(list);
			}
			foreach (DataRow row in dataTable.Rows)
			{
				BOMQuoteLineDto item = new BOMQuoteLineDto
				{
					QuoteID = row["qmlQuoteID"].ToString().Trim(),
					QuoteLineID = short.Parse(row["qmlQuoteLineID"].ToString().Trim()),
					PartID = row["qmlPartID"].ToString().Trim(),
					PartRevisionID = row["qmlPartRevisionID"].ToString().Trim(),
					UnitOfMeasure = row["qmlUnitOfMeasure"].ToString().Trim(),
					PartGroupID = row["qmlPartGroupID"].ToString().Trim(),
					PartShortDescription = row["qmlPartShortDescription"].ToString().Trim(),
					OrgPartShortDescription = row["qmlOrgPartShortDescription"].ToString().Trim(),
					ResolutionReasonID = row["qmlResolutionReasonID"].ToString().Trim(),
					QuoteMarkupType = (byte)row["qmlQuoteMarkupType"],
					PurchaseToOrder = Convert.ToBoolean(Convert.ToInt16(row["qmlPurchaseToOrder"])),
					PurchaseUnitCostForeign = Convert.ToDecimal(row["qmlPurchaseUnitCostForeign"].ToString().Trim()),
					SupplierOrganizationID = row["qmlSupplierOrganizationID"].ToString().Trim(),
					PurchaseLocationID = row["qmlPurchaseLocationID"].ToString().Trim(),
					Firm = Convert.ToBoolean(Convert.ToInt16(row["qmlFirm"])),
					ProjectID = row["qmlProjectID"].ToString().Trim(),
					ProjectAreaID = row["qmlProjectAreaID"].ToString().Trim(),
					Closed = Convert.ToBoolean(Convert.ToInt16(row["qmlClosed"])),
					CreatedBy = row["qmlCreatedBy"].ToString().Trim(),
					CreatedDate = Convert.ToDateTime(string.IsNullOrWhiteSpace(row["qmlCreatedDate"].ToString()) ? DateTime.Parse("01/01/1900") : DateTime.Parse(row["qmlCreatedDate"].ToString())),
					UniqueID = Guid.Parse(row["qmlUniqueID"].ToString().Trim()),
					RowVersion = (byte[])row["qmlRowVersion"]
				};
				list.Add(item);
			}
		}
		return Task.FromResult(list);
	}

	public Task<APIValidationInfoDto> SaveQuoteAsync(BOMCreateQuoteDto quote)
	{
		APIValidationInfoDto result = new APIValidationInfoDto();
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			using M1BindingSource m1BindingSource = new M1BindingSource(base.M1database, null);
			m1BindingSource.ClearCache();
			stringBuilder.Append("qmpQuoteID = " + M1Util.ConvertToLinq(quote.QuoteID));
			m1BindingSource.DataSourceTable = "Quotes";
			m1BindingSource.NavigateTo(stringBuilder.ToString());
			DataRow dataRow;
			if (m1BindingSource.Count == 0)
			{
				dataRow = m1BindingSource.AddNew() as DataRow;
				if (dataRow != null)
				{
					dataRow["qmpQuoteID"] = quote.QuoteID;
				}
			}
			else
			{
				dataRow = m1BindingSource.CurrentAsDataRow;
			}
			dataRow["qmpQuoteID"] = quote.QuoteID ?? dataRow["qmpQuoteID"];
			dataRow["qmpCustomerOrganizationID"] = quote.CustomerOrganizationID ?? dataRow["qmpCustomerOrganizationID"];
			dataRow["qmpShipOrganizationID"] = quote.ShipOrganizationID ?? dataRow["qmpShipOrganizationID"];
			dataRow["qmpQuoterEmployeeID"] = quote.QuoterEmployeeID ?? dataRow["qmpQuoterEmployeeID"];
			dataRow["qmpCurrencyRateID"] = quote.CurrencyRateID ?? dataRow["qmpCurrencyRateID"];
			m1BindingSource.SaveData();
		}
		catch (Exception ex)
		{
			List<string> list = new List<string>();
			list.Add("Error occurred [" + ex.Message + "] while processing the Quote [" + quote.QuoteID + "]");
			result = new APIValidationInfoDto(list, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(result);
	}
}
