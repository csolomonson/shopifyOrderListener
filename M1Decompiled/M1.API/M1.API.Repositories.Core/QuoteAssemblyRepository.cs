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

public class QuoteAssemblyRepository : APIBaseRepository, IQuoteAssemblyRepository, IAPIBaseRepository, IDisposable
{
	private readonly string[] quoteAssemblyFields = new string[18]
	{
		"qmaQuoteID", "qmaQuoteLineID", "qmaQuoteAssemblyID", "qmaParentAssemblyID", "qmaLevel", "qmaSourceMethodID", "qmaSourceRevisionID", "qmaPartID", "qmaPartRevisionID", "qmaUnitofMeasure",
		"qmaPartShortDescription", "qmaQuantityPerParent", "qmaClosed", "qmaPullAllFromStock", "qmaCreatedBy", "qmaCreatedDate", "qmaUniqueID", "qmaRowVersion"
	};

	private readonly string GET_QUOTE_ASSEMBLIES_BY_QUOTE_ID = "SELECT qmaQuoteID,qmaQuoteLineID,qmaQuoteAssemblyID,qmaParentAssemblyID,qmaLevel,\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t    qmaSourceMethodID,qmaSourceRevisionID,qmaPartID,qmaPartRevisionID,qmaUnitofMeasure,\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tqmaPartShortDescription,qmaQuantityPerParent,qmaClosed,qmaPullAllFromStock,qmaCreatedBy,\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tqmaCreatedDate,qmaUniqueID,qmaRowVersion\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tFROM QuoteAssemblies\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tWHERE qmaQuoteID = @QuoteID";

	public QuoteAssemblyRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public Task<bool> DoesQuoteAssemblyExist(string quoteAssemblyId)
	{
		InitializeParameterLists();
		base.filterList.Add("qmaQuoteAssemblyID|C", quoteAssemblyId);
		base.selectList.Add("qmaQuoteAssemblyID");
		return Task.FromResult(GetAsObject("QuoteAssemblies", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesQuoteAssemblyExist(string quoteId, string quoteLineId, string quoteAssemblyId)
	{
		InitializeParameterLists();
		base.filterList.Add("qmaQuoteID|C", quoteId);
		base.filterList.Add("qmaQuoteLineID|C", quoteLineId);
		base.filterList.Add("qmaQuoteAssemblyID|C", quoteAssemblyId);
		base.selectList.Add("qmaQuoteAssemblyID");
		return Task.FromResult(GetAsObject("QuoteAssemblies", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<BOMQuoteAssemblyDto>> GetAllQuoteAssemblies(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<BOMQuoteAssemblyDto> collection = new List<BOMQuoteAssemblyDto>();
		InitializeParameterLists();
		base.selectList.AddRange(quoteAssemblyFields);
		List<string> orderbyList = new List<string> { "qmaQuoteAssemblyID" };
		using (DataTable dataTable = GetAsDataTable("QuoteAssemblies", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				BOMQuoteAssemblyDto bOMQuoteAssemblyDto = new BOMQuoteAssemblyDto();
				bOMQuoteAssemblyDto.QuoteID = dataTable.Rows[i].Field<string>("qmaQuoteID");
				bOMQuoteAssemblyDto.QuoteLineID = dataTable.Rows[i].Field<short>("qmaQuoteLineID");
				bOMQuoteAssemblyDto.QuoteAssemblyID = dataTable.Rows[i].Field<int>("qmaQuoteAssemblyID");
				bOMQuoteAssemblyDto.ParentAssemblyID = dataTable.Rows[i].Field<int>("qmaParentAssemblyID");
				bOMQuoteAssemblyDto.Level = dataTable.Rows[i].Field<short>("qmaLevel");
				bOMQuoteAssemblyDto.SourceMethodID = dataTable.Rows[i].Field<string>("qmaSourceMethodID");
				bOMQuoteAssemblyDto.SourceRevisionID = dataTable.Rows[i].Field<string>("qmaSourceRevisionID");
				bOMQuoteAssemblyDto.PartID = dataTable.Rows[i].Field<string>("qmaPartID");
				bOMQuoteAssemblyDto.PartRevisionID = dataTable.Rows[i].Field<string>("qmaPartRevisionID");
				bOMQuoteAssemblyDto.UnitOfMeasure = dataTable.Rows[i].Field<string>("qmaUnitOfMeasure");
				bOMQuoteAssemblyDto.PartShortDescription = dataTable.Rows[i].Field<string>("qmaPartShortDescription");
				bOMQuoteAssemblyDto.QuantityPerParent = dataTable.Rows[i].Field<decimal>("qmaQuantityPerParent");
				bOMQuoteAssemblyDto.Closed = dataTable.Rows[i].Field<bool>("qmaClosed");
				bOMQuoteAssemblyDto.PullAllFromStock = dataTable.Rows[i].Field<bool>("qmaPullAllFromStock");
				bOMQuoteAssemblyDto.CreatedBy = dataTable.Rows[i].Field<string>("qmaCreatedBy");
				bOMQuoteAssemblyDto.CreatedDate = ((!dataTable.Rows[i].Field<DateTime?>("qmaCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[i].Field<DateTime?>("qmaCreatedDate"));
				bOMQuoteAssemblyDto.UniqueID = dataTable.Rows[i].Field<Guid>("qmaUniqueID");
				bOMQuoteAssemblyDto.RowVersion = dataTable.Rows[0].Field<byte[]>("qmaRowVersion");
				collection.Add(bOMQuoteAssemblyDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<IList<BOMQuoteAssemblyDto>> GetQuoteAssemblies(string quoteId, string quoteLineId)
	{
		IList<BOMQuoteAssemblyDto> list = new List<BOMQuoteAssemblyDto>();
		InitializeParameterLists();
		base.filterList.Add("@QuoteID", quoteId);
		bool flag = !string.IsNullOrEmpty(quoteLineId);
		if (flag)
		{
			base.filterList.Add("@QuoteLineID", quoteLineId);
		}
		using (DataTable dataTable = GetAsDataTable(GetQuoteAssembliesQuery(flag), base.filterList, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(list);
			}
			foreach (DataRow row in dataTable.Rows)
			{
				BOMQuoteAssemblyDto item = new BOMQuoteAssemblyDto
				{
					QuoteID = row.Field<string>("qmaQuoteID"),
					QuoteLineID = row.Field<short>("qmaQuoteLineID"),
					QuoteAssemblyID = row.Field<int>("qmaQuoteAssemblyID"),
					ParentAssemblyID = row.Field<int>("qmaParentAssemblyID"),
					Level = row.Field<short>("qmaLevel"),
					SourceMethodID = row.Field<string>("qmaSourceMethodID"),
					SourceRevisionID = row.Field<string>("qmaSourceRevisionID"),
					PartID = row.Field<string>("qmaPartID"),
					PartRevisionID = row.Field<string>("qmaPartRevisionID"),
					UnitOfMeasure = row.Field<string>("qmaUnitOfMeasure"),
					PartShortDescription = row.Field<string>("qmaPartShortDescription"),
					QuantityPerParent = row.Field<decimal>("qmaQuantityPerParent"),
					Closed = row.Field<bool>("qmaClosed"),
					PullAllFromStock = row.Field<bool>("qmaPullAllFromStock"),
					CreatedBy = row.Field<string>("qmaCreatedBy"),
					CreatedDate = ((!row.Field<DateTime?>("qmaCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : row.Field<DateTime?>("qmaCreatedDate")),
					UniqueID = row.Field<Guid>("qmaUniqueID"),
					RowVersion = row.Field<byte[]>("qmaRowVersion")
				};
				list.Add(item);
			}
		}
		return Task.FromResult(list);
	}

	private string GetQuoteAssembliesQuery(bool includeAdditionalCondition)
	{
		string text = "SELECT qmaQuoteID,qmaQuoteLineID,qmaQuoteAssemblyID,qmaParentAssemblyID,qmaLevel,\r\n\t\t\t\t\t\t\t\t\t\tqmaSourceMethodID,qmaSourceRevisionID,qmaPartID,qmaPartRevisionID,qmaUnitofMeasure,\r\n\t\t\t\t\t\t\t\t\t\tqmaPartShortDescription,qmaQuantityPerParent,qmaClosed,qmaPullAllFromStock,qmaCreatedBy,\r\n\t\t\t\t\t\t\t\t\t\tqmaCreatedDate,qmaUniqueID,qmaRowVersion\r\n\t\t\t\t\t\t\t\t\t\tFROM QuoteAssemblies\r\n\t\t\t\t\t\t\t\t\t\tWHERE qmaQuoteID = @QuoteID";
		if (includeAdditionalCondition)
		{
			text += " AND qmaQuoteLineID = @QuoteLineID;";
		}
		return text;
	}

	public async Task<APIValidationInfoDto> SaveQuoteAssemblyAsync(BOMCreateQuoteAssemblyDto quoteAssembly)
	{
		APIValidationInfoDto result = new APIValidationInfoDto();
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			using M1BindingSource m1BindingSource = new M1BindingSource(base.M1database, null);
			m1BindingSource.ClearCache();
			stringBuilder.Append("qmaQuoteID = " + M1Util.ConvertToLinq(quoteAssembly.QuoteID) + $"And qmaQuoteLineID = {quoteAssembly.QuoteLineID}" + $"And qmaQuoteAssemblyID = {quoteAssembly.QuoteAssemblyID}");
			m1BindingSource.DataSourceTable = "QuoteAssemblies";
			m1BindingSource.NavigateTo(stringBuilder.ToString());
			DataRow dataRow;
			if (m1BindingSource.Count == 0)
			{
				dataRow = m1BindingSource.AddNew() as DataRow;
				dataRow["qmaQuoteID"] = quoteAssembly.QuoteID;
				dataRow["qmaQuoteLineID"] = quoteAssembly.QuoteLineID;
				dataRow["qmaQuoteAssemblyID"] = quoteAssembly.QuoteAssemblyID;
			}
			else
			{
				dataRow = m1BindingSource.CurrentAsDataRow;
			}
			dataRow["qmaPartID"] = quoteAssembly.PartID ?? dataRow["qmaPartID"];
			dataRow["qmaPartRevisionID"] = quoteAssembly.PartRevisionID ?? dataRow["qmaPartRevisionID"];
			dataRow["qmaUnitOfMeasure"] = quoteAssembly.UnitOfMeasure ?? dataRow["qmaUnitOfMeasure"];
			dataRow["qmaQuantityPerParent"] = quoteAssembly.QuantityPerParent;
			if (!quoteAssembly.PullAllFromStock)
			{
				dataRow["qmaPullAllFromStock"] = quoteAssembly.PullAllFromStock;
			}
			m1BindingSource.SaveData();
		}
		catch (Exception ex)
		{
			result = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex.Message}] while processing the QuoteAssembly [{quoteAssembly.QuoteAssemblyID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return await Task.FromResult(result);
	}
}
