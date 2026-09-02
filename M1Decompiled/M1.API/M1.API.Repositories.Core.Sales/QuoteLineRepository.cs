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

public class QuoteLineRepository : APIBaseRepository, IQuoteLineRepository, IAPIBaseRepository, IDisposable
{
	private readonly IQuoteAssemblyRepository _quoteAssemblyRepository;

	private readonly string[] quoteLineFields = new string[22]
	{
		"qmlQuoteID", "qmlQuoteLineID", "qmlPartID", "qmlPartRevisionID", "qmlUnitofMeasure", "qmlPartGroupID", "qmlPartShortDescription", "qmlOrgPartShortDescription", "qmlResolutionReasonID", "qmlQuoteMarkupType",
		"qmlPurchaseToOrder", "qmlPurchaseUnitCostForeign", "qmlSupplierOrganizationID", "qmlPurchaseLocationID", "qmlFirm", "qmlProjectID", "qmlProjectAreaID", "qmlClosed", "qmlCreatedBy", "qmlCreatedDate",
		"qmlUniqueID", "qmlRowVersion"
	};

	public QuoteLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		_quoteAssemblyRepository = new QuoteAssemblyRepository(clientContext);
	}

	public Task<bool> DoesQuoteLineExists(string quoteId, string quoteLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("qmlQuoteID|C", quoteId);
		base.filterList.Add("qmlQuoteLineID|C", quoteLineId);
		base.selectList.Add("qmlQuoteLineID");
		return Task.FromResult(GetAsObject("QuoteLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<BOMQuoteLineDto>> GetAllQuoteLines(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<BOMQuoteLineDto> collection = new List<BOMQuoteLineDto>();
		InitializeParameterLists();
		base.selectList.AddRange(quoteLineFields);
		List<string> orderbyList = new List<string> { "qmlQuoteLineID" };
		using (DataTable dataTable = GetAsDataTable("QuoteLines", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				BOMQuoteLineDto bOMQuoteLineDto = new BOMQuoteLineDto();
				bOMQuoteLineDto.QuoteID = dataTable.Rows[i].Field<string>("qmlQuoteID");
				bOMQuoteLineDto.QuoteLineID = dataTable.Rows[i].Field<short>("qmlQuoteLineID");
				bOMQuoteLineDto.PartID = dataTable.Rows[i].Field<string>("qmlPartID");
				bOMQuoteLineDto.PartRevisionID = dataTable.Rows[i].Field<string>("qmlPartRevisionID");
				bOMQuoteLineDto.UnitOfMeasure = dataTable.Rows[i].Field<string>("qmlUnitOfMeasure");
				bOMQuoteLineDto.PartGroupID = dataTable.Rows[i].Field<string>("qmlPartGroupID");
				bOMQuoteLineDto.PartShortDescription = dataTable.Rows[i].Field<string>("qmlPartShortDescription");
				bOMQuoteLineDto.OrgPartShortDescription = dataTable.Rows[i].Field<string>("qmlOrgPartShortDescription");
				bOMQuoteLineDto.ResolutionReasonID = dataTable.Rows[i].Field<string>("qmlResolutionReasonID");
				bOMQuoteLineDto.QuoteMarkupType = dataTable.Rows[i].Field<byte>("qmlQuoteMarkupType");
				bOMQuoteLineDto.PurchaseToOrder = dataTable.Rows[i].Field<bool>("qmlPurchaseToOrder");
				bOMQuoteLineDto.PurchaseUnitCostForeign = dataTable.Rows[i].Field<decimal>("qmlPurchaseUnitCostForeign");
				bOMQuoteLineDto.SupplierOrganizationID = dataTable.Rows[i].Field<string>("qmlSupplierOrganizationID");
				bOMQuoteLineDto.PurchaseLocationID = dataTable.Rows[i].Field<string>("qmlPurchaseLocationID");
				bOMQuoteLineDto.Firm = dataTable.Rows[i].Field<bool>("qmlFirm");
				bOMQuoteLineDto.ProjectID = dataTable.Rows[i].Field<string>("qmlProjectID");
				bOMQuoteLineDto.ProjectAreaID = dataTable.Rows[i].Field<string>("qmlProjectAreaID");
				bOMQuoteLineDto.Closed = dataTable.Rows[i].Field<bool>("qmlClosed");
				bOMQuoteLineDto.CreatedBy = dataTable.Rows[i].Field<string>("qmlCreatedBy");
				bOMQuoteLineDto.CreatedDate = ((!dataTable.Rows[i].Field<DateTime?>("qmlCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[i].Field<DateTime?>("qmlCreatedDate"));
				bOMQuoteLineDto.UniqueID = dataTable.Rows[i].Field<Guid>("qmlUniqueID");
				bOMQuoteLineDto.RowVersion = dataTable.Rows[0].Field<byte[]>("qmlRowVersion");
				collection.Add(bOMQuoteLineDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<BOMQuoteLineDto> GetQuoteLine(string quoteId, string quoteLineId)
	{
		BOMQuoteLineDto bOMQuoteLineDto = new BOMQuoteLineDto();
		InitializeParameterLists();
		base.selectList.AddRange(quoteLineFields);
		base.filterList.Add(Guid.TryParse(quoteLineId, out var _) ? "qmlUniqueID|C" : "qmlQuoteLineID|C", quoteLineId);
		base.filterList.Add("qmlQuoteID", quoteId);
		using (DataTable dataTable = GetAsDataTable("QuoteLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(bOMQuoteLineDto);
			}
			bOMQuoteLineDto.CreatedBy = dataTable.Rows[0].Field<string>("qmlCreatedBy");
			bOMQuoteLineDto.CreatedDate = ((!dataTable.Rows[0].Field<DateTime?>("qmlCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[0].Field<DateTime?>("qmlCreatedDate"));
			bOMQuoteLineDto.UniqueID = dataTable.Rows[0].Field<Guid>("qmlUniqueID");
			bOMQuoteLineDto.Closed = dataTable.Rows[0].Field<bool>("qmlClosed");
			bOMQuoteLineDto.Firm = dataTable.Rows[0].Field<bool>("qmlFirm");
			bOMQuoteLineDto.PurchaseToOrder = dataTable.Rows[0].Field<bool>("qmlPurchaseToOrder");
			bOMQuoteLineDto.OrgPartShortDescription = dataTable.Rows[0].Field<string>("qmlOrgPartShortDescription");
			bOMQuoteLineDto.PartGroupID = dataTable.Rows[0].Field<string>("qmlPartGroupID");
			bOMQuoteLineDto.PartID = dataTable.Rows[0].Field<string>("qmlPartID");
			bOMQuoteLineDto.PartRevisionID = dataTable.Rows[0].Field<string>("qmlPartRevisionID");
			bOMQuoteLineDto.PartShortDescription = dataTable.Rows[0].Field<string>("qmlPartShortDescription");
			bOMQuoteLineDto.ProjectAreaID = dataTable.Rows[0].Field<string>("qmlProjectAreaID");
			bOMQuoteLineDto.ProjectID = dataTable.Rows[0].Field<string>("qmlProjectID");
			bOMQuoteLineDto.PurchaseLocationID = dataTable.Rows[0].Field<string>("qmlPurchaseLocationID");
			bOMQuoteLineDto.PurchaseUnitCostForeign = dataTable.Rows[0].Field<decimal>("qmlPurchaseUnitCostForeign");
			bOMQuoteLineDto.QuoteID = dataTable.Rows[0].Field<string>("qmlQuoteID");
			bOMQuoteLineDto.QuoteMarkupType = dataTable.Rows[0].Field<byte>("qmlQuoteMarkupType");
			bOMQuoteLineDto.ResolutionReasonID = dataTable.Rows[0].Field<string>("qmlResolutionReasonID");
			bOMQuoteLineDto.QuoteLineID = dataTable.Rows[0].Field<short>("qmlQuoteLineID");
			bOMQuoteLineDto.SupplierOrganizationID = dataTable.Rows[0].Field<string>("qmlSupplierOrganizationID");
			bOMQuoteLineDto.UnitOfMeasure = dataTable.Rows[0].Field<string>("qmlUnitOfMeasure");
			bOMQuoteLineDto.RowVersion = dataTable.Rows[0].Field<byte[]>("qmlRowVersion");
		}
		return Task.FromResult(bOMQuoteLineDto);
	}

	public async Task<APIValidationInfoDto> SaveQuoteLineAsync(BOMCreateQuoteLineDto quoteLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningList = new List<string>();
		APIValidationInfoDto apiValidationInfoDto = new APIValidationInfoDto();
		StringBuilder stringBuilder = new StringBuilder();
		string unitOfMeasure = quoteLine.UnitOfMeasure;
		BOMCreateQuoteAssemblyDto quoteAssembly = new BOMCreateQuoteAssemblyDto
		{
			QuoteID = quoteLine.QuoteID,
			QuoteLineID = quoteLine.QuoteLineID,
			QuoteAssemblyID = 0,
			ParentAssemblyID = 0,
			PartID = quoteLine.PartID,
			PartRevisionID = quoteLine.PartRevisionID,
			UnitOfMeasure = unitOfMeasure,
			QuantityPerParent = 1m,
			Level = 1
		};
		try
		{
			using M1BindingSource quoteLineBindingSource = new M1BindingSource(base.M1database, null);
			quoteLineBindingSource.ClearCache();
			stringBuilder.Clear();
			stringBuilder.Append("qmlQuoteID = " + M1Util.ConvertToLinq(quoteLine.QuoteID) + "And qmlQuoteLineID = " + M1Util.ConvertToLinq(quoteLine.QuoteLineID));
			quoteLineBindingSource.DataSourceTable = "QuoteLines";
			quoteLineBindingSource.NavigateTo(stringBuilder.ToString());
			DataRow dataRow;
			if (quoteLineBindingSource.Count == 0)
			{
				dataRow = quoteLineBindingSource.AddNew() as DataRow;
				dataRow["qmlQuoteID"] = quoteLine.QuoteID;
				dataRow["qmlQuoteLineID"] = quoteLine.QuoteLineID;
			}
			else
			{
				dataRow = quoteLineBindingSource.CurrentAsDataRow;
			}
			dataRow["qmlPartID"] = quoteLine.PartID ?? dataRow["qmlPartID"];
			dataRow["qmlPartRevisionID"] = quoteLine.PartRevisionID ?? dataRow["qmlPartRevisionID"];
			dataRow["qmlUnitOfMeasure"] = unitOfMeasure ?? dataRow["qmlUnitOfMeasure"];
			dataRow["qmlSupplierOrganizationID"] = quoteLine.SupplierOrganizationID ?? dataRow["qmlSupplierOrganizationID"];
			dataRow["qmlPurchaseLocationID"] = quoteLine.PurchaseLocationID ?? dataRow["qmlPurchaseLocationID"];
			dataRow["qmlPurchaseUnitCostBase"] = quoteLine.PurchaseUnitCostBase;
			DataRow dataRow2 = dataRow;
			bool? firm = quoteLine.Firm;
			dataRow2["qmlFirm"] = (firm.HasValue ? ((object)(firm == true)) : dataRow["qmlFirm"]);
			quoteLineBindingSource.SaveData();
			APIValidationInfoDto aPIValidationInfoDto = await _quoteAssemblyRepository.SaveQuoteAssemblyAsync(quoteAssembly);
			errorsList.AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
			warningList.AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
			if (errorsList == null || errorsList.Count <= 0)
			{
			}
		}
		catch (Exception ex)
		{
			HttpStatusCode httpValidationStatusCode = HttpStatusCode.InternalServerError;
			errorsList.Add($"Error occurred [{ex.Message}] while processing the QuoteLine with QuoteID [{quoteLine.QuoteID}] and QuoteLine ID [{quoteLine.QuoteLineID}]");
			apiValidationInfoDto = new APIValidationInfoDto(errorsList, null, httpValidationStatusCode);
		}
		return await Task.FromResult(apiValidationInfoDto);
	}
}
