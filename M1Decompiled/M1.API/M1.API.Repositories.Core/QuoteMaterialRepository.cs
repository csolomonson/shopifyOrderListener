using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;
using M1.API.Utilities;
using M1.Core;
using M1.Extensions;

namespace M1.API.Repositories.Core;

public class QuoteMaterialRepository : APIBaseRepository, IQuoteMaterialRepository, IAPIBaseRepository, IDisposable
{
	private readonly string[] quoteMaterialFields = new string[23]
	{
		"qmmQuoteID", "qmmQuoteLineID", "qmmQuoteAssemblyID", "qmmQuoteMaterialID", "qmmPartID", "qmmPartRevisionID", "qmmPartWarehouseLocationID", "qmmPartBinID", "qmmUnitofMeasure", "qmmPartShortDescription",
		"qmmQuantityPerAssembly", "qmmScrapPercent", "qmmScrapQuantity", "qmmEstimatedUnitCost", "qmmSupplierOrganizationID", "qmmPurchaseLocationID", "qmmLeadTime", "qmmMinimumCharge", "qmmCreatedBy", "qmmCreatedDate",
		"qmmClosed", "qmmUniqueID", "qmmRowVersion"
	};

	public QuoteMaterialRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public Task<bool> DoesQuoteMaterialExists(string quoteMaterialId)
	{
		InitializeParameterLists();
		base.filterList.Add("qmmQuoteMaterialID|C", quoteMaterialId);
		base.selectList.Add("qmmQuoteMaterialID");
		return Task.FromResult(GetAsObject("QuoteMaterials", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<BOMQuoteMaterialDto>> GetAllQuoteMaterials(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<BOMQuoteMaterialDto> collection = new List<BOMQuoteMaterialDto>();
		InitializeParameterLists();
		base.selectList.AddRange(quoteMaterialFields);
		List<string> orderbyList = new List<string> { "qmmQuoteMaterialID" };
		using (DataTable dataTable = GetAsDataTable("QuoteMaterials", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				BOMQuoteMaterialDto bOMQuoteMaterialDto = new BOMQuoteMaterialDto();
				bOMQuoteMaterialDto.CreatedBy = dataTable.Rows[i].Field<string>("qmmCreatedBy");
				bOMQuoteMaterialDto.CreatedDate = ((!dataTable.Rows[i].Field<DateTime?>("qmmCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[i].Field<DateTime?>("qmmCreatedDate"));
				bOMQuoteMaterialDto.UniqueID = dataTable.Rows[i].Field<Guid>("qmmUniqueID");
				bOMQuoteMaterialDto.EstimatedUnitCost = dataTable.Rows[i].Field<decimal>("qmmEstimatedUnitCost");
				bOMQuoteMaterialDto.Closed = dataTable.Rows[i].Field<bool>("qmmClosed");
				bOMQuoteMaterialDto.LeadTime = dataTable.Rows[i].Field<short>("qmmLeadTime");
				bOMQuoteMaterialDto.MinimumCharge = dataTable.Rows[i].Field<decimal>("qmmMinimumCharge");
				bOMQuoteMaterialDto.PartBinID = dataTable.Rows[i].Field<string>("qmmPartBinID");
				bOMQuoteMaterialDto.PartID = dataTable.Rows[i].Field<string>("qmmPartID");
				bOMQuoteMaterialDto.PartRevisionID = dataTable.Rows[i].Field<string>("qmmPartRevisionID");
				bOMQuoteMaterialDto.PartShortDescription = dataTable.Rows[i].Field<string>("qmmPartShortDescription");
				bOMQuoteMaterialDto.PartWarehouseLocationID = dataTable.Rows[i].Field<string>("qmmPartWarehouseLocationID");
				bOMQuoteMaterialDto.PurchaseLocationID = dataTable.Rows[i].Field<string>("qmmPurchaseLocationID");
				bOMQuoteMaterialDto.QuantityPerAssembly = dataTable.Rows[i].Field<decimal>("qmmQuantityPerAssembly");
				bOMQuoteMaterialDto.QuoteAssemblyID = dataTable.Rows[i].Field<int>("qmmQuoteAssemblyID");
				bOMQuoteMaterialDto.QuoteID = dataTable.Rows[i].Field<string>("qmmQuoteID");
				bOMQuoteMaterialDto.QuoteLineID = dataTable.Rows[i].Field<short>("qmmQuoteLineID");
				bOMQuoteMaterialDto.ScrapPercent = dataTable.Rows[i].Field<decimal>("qmmScrapPercent");
				bOMQuoteMaterialDto.ScrapQuantity = dataTable.Rows[i].Field<decimal>("qmmScrapQuantity");
				bOMQuoteMaterialDto.QuoteMaterialID = dataTable.Rows[i].Field<int>("qmmQuoteMaterialID");
				bOMQuoteMaterialDto.SupplierOrganizationID = dataTable.Rows[i].Field<string>("qmmSupplierOrganizationID");
				bOMQuoteMaterialDto.UnitOfMeasure = dataTable.Rows[i].Field<string>("qmmUnitOfMeasure");
				bOMQuoteMaterialDto.RowVersion = dataTable.Rows[i].Field<byte[]>("qmmRowVersion");
				collection.Add(bOMQuoteMaterialDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ICollection<BOMQuoteMaterialDto>> GetQuoteMaterialsAsync(string quoteId, string quoteLineId, string quoteAssemblyId)
	{
		ICollection<BOMQuoteMaterialDto> collection = new List<BOMQuoteMaterialDto>();
		InitializeParameterLists();
		base.filterList.Add("@QuoteID", quoteId);
		bool flag = !string.IsNullOrEmpty(quoteLineId);
		if (flag)
		{
			base.filterList.Add("@QuoteLineID", quoteLineId);
		}
		bool flag2 = !string.IsNullOrEmpty(quoteAssemblyId);
		if (flag2)
		{
			base.filterList.Add("@QuoteAssemblyID", quoteAssemblyId);
		}
		using (DataTable dataTable = GetAsDataTable(GetSelectQuoteMaterialsQuery(flag, flag2), base.filterList, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(collection);
			}
			foreach (DataRow row in dataTable.Rows)
			{
				BOMQuoteMaterialDto item = new BOMQuoteMaterialDto
				{
					CreatedBy = row.Field<string>("qmmCreatedBy"),
					CreatedDate = ((!row.Field<DateTime?>("qmmCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : row.Field<DateTime?>("qmmCreatedDate")),
					UniqueID = row.Field<Guid>("qmmUniqueID"),
					EstimatedUnitCost = row.Field<decimal>("qmmEstimatedUnitCost"),
					Closed = row.Field<bool>("qmmClosed"),
					LeadTime = row.Field<short>("qmmLeadTime"),
					MinimumCharge = row.Field<decimal>("qmmMinimumCharge"),
					PartBinID = row.Field<string>("qmmPartBinID"),
					PartID = row.Field<string>("qmmPartID"),
					PartRevisionID = row.Field<string>("qmmPartRevisionID"),
					PartShortDescription = row.Field<string>("qmmPartShortDescription"),
					PartWarehouseLocationID = row.Field<string>("qmmPartWarehouseLocationID"),
					PurchaseLocationID = row.Field<string>("qmmPurchaseLocationID"),
					QuantityPerAssembly = row.Field<decimal>("qmmQuantityPerAssembly"),
					QuoteAssemblyID = row.Field<int>("qmmQuoteAssemblyID"),
					QuoteID = row.Field<string>("qmmQuoteID"),
					QuoteLineID = row.Field<short>("qmmQuoteLineID"),
					ScrapPercent = row.Field<decimal>("qmmScrapPercent"),
					ScrapQuantity = row.Field<decimal>("qmmScrapQuantity"),
					QuoteMaterialID = row.Field<int>("qmmQuoteMaterialID"),
					SupplierOrganizationID = row.Field<string>("qmmSupplierOrganizationID"),
					UnitOfMeasure = row.Field<string>("qmmUnitOfMeasure"),
					RowVersion = row.Field<byte[]>("qmmRowVersion")
				};
				collection.Add(item);
			}
		}
		return Task.FromResult(collection);
	}

	private string GetSelectQuoteMaterialsQuery(bool includeQuoteLineId, bool includeQuoteAssemblyId)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("SELECT qmmQuoteID, qmmQuoteLineID, qmmQuoteAssemblyID, qmmQuoteMaterialID, qmmPartID,\r\n                          qmmPartRevisionID, qmmPartWarehouseLocationID, qmmPartBinID, qmmUnitofMeasure, \r\n                          qmmPartShortDescription, qmmQuantityPerAssembly, qmmScrapPercent, qmmScrapQuantity,\r\n                          qmmEstimatedUnitCost, qmmSupplierOrganizationID, qmmPurchaseLocationID, qmmLeadTime,\r\n                          qmmMinimumCharge, qmmCreatedBy, qmmCreatedDate, qmmClosed, qmmUniqueID, qmmRowVersion\r\n                  FROM QuoteMaterials\r\n                  WHERE qmmQuoteID = @QuoteID");
		if (includeQuoteLineId)
		{
			stringBuilder.Append(" AND qmmQuoteLineID = @QuoteLineID");
		}
		if (includeQuoteAssemblyId)
		{
			stringBuilder.Append(" AND qmmQuoteAssemblyID = @QuoteAssemblyID");
		}
		stringBuilder.Append(";");
		return stringBuilder.ToString();
	}

	public async Task<APIValidationInfoDto> SaveQuoteMaterialAsync(BOMCreateQuoteMaterialDto quoteMaterial)
	{
		List<string> list = new List<string>();
		new List<string>();
		APIValidationInfoDto result = new APIValidationInfoDto();
		StringBuilder stringBuilder = new StringBuilder();
		string unitOfMeasure = quoteMaterial.UnitOfMeasure;
		try
		{
			using M1BindingSource m1BindingSource = new M1BindingSource(base.M1database, null);
			m1BindingSource.ClearCache();
			stringBuilder.Clear();
			stringBuilder.Append("qmmQuoteID = " + M1Util.ConvertToLinq(quoteMaterial.QuoteID) + $"And qmmQuoteLineID = {quoteMaterial.QuoteLineID}" + $"And qmmQuoteAssemblyID = {quoteMaterial.QuoteAssemblyID}" + $"And qmmQuoteMaterialID = {quoteMaterial.QuoteMaterialID}");
			m1BindingSource.DataSourceTable = "QuoteMaterials";
			m1BindingSource.NavigateTo(stringBuilder.ToString());
			DataRow dataRow;
			if (m1BindingSource.Count == 0)
			{
				dataRow = m1BindingSource.AddNew() as DataRow;
				dataRow["qmmQuoteID"] = quoteMaterial.QuoteID;
				dataRow["qmmQuoteLineID"] = quoteMaterial.QuoteLineID;
				dataRow["qmmQuoteAssemblyID"] = quoteMaterial.QuoteAssemblyID;
				dataRow["qmmQuoteMaterialID"] = quoteMaterial.QuoteMaterialID;
			}
			else
			{
				dataRow = m1BindingSource.CurrentAsDataRow;
			}
			dataRow["qmmPartID"] = quoteMaterial.PartID ?? dataRow["qmmPartID"];
			dataRow["qmmPartRevisionID"] = quoteMaterial.PartRevisionID ?? dataRow["qmmPartRevisionID"];
			dataRow["qmmUnitOfMeasure"] = unitOfMeasure ?? dataRow["qmmUnitOfMeasure"];
			dataRow["qmmQuantityPerAssembly"] = quoteMaterial.QuantityPerAssembly;
			dataRow["qmmSupplierOrganizationID"] = quoteMaterial.SupplierOrganizationID ?? dataRow["qmmSupplierOrganizationID"];
			dataRow["qmmPurchaseLocationID"] = quoteMaterial.PurchaseLocationID ?? dataRow["qmmPurchaseLocationID"];
			m1BindingSource.SaveData();
		}
		catch (Exception ex)
		{
			HttpStatusCode httpValidationStatusCode = HttpStatusCode.InternalServerError;
			list.Add($"Error occurred [{ex.Message}] while processing the QuoteMaterial with QuoteID [{quoteMaterial.QuoteID}], QuoteLine ID [{quoteMaterial.QuoteLineID}] and QuoteLine ID [{quoteMaterial.QuoteLineID}] and QuoteAssembly ID [{quoteMaterial.QuoteAssemblyID}]");
			result = new APIValidationInfoDto(list, null, httpValidationStatusCode);
		}
		return await Task.FromResult(result);
	}
}
