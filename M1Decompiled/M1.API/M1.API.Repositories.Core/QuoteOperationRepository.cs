using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.Utilities;

namespace M1.API.Repositories.Core;

public class QuoteOperationRepository : APIBaseRepository, IQuoteOperationRepository, IAPIBaseRepository, IDisposable
{
	private readonly string[] quoteOperationFields = new string[29]
	{
		"qmoQuoteID", "qmoQuoteLineID", "qmoQuoteAssemblyID", "qmoQuoteOperationID", "qmoOperationType", "qmoWorkcenterID", "qmoProcessID", "qmoProcessShortDescription", "qmoProcessLongDescriptionRTF", "qmoProcessLongDescriptionText",
		"qmoQuantityPerAssembly", "qmoQueueTime", "qmoSetupHours", "qmoMoveTime", "qmoQuotingRate", "qmoSetupRate", "qmoProductionRate", "qmoOverheadRate", "qmoPartID", "qmoPartRevisionID",
		"qmoUnitofMeasure", "qmoSupplierOrganizationID", "qmoStandardFactor", "qmoProductionStandard", "qmoClosed", "qmoCreatedBy", "qmoCreatedDate", "qmoUniqueID", "qmoRowVersion"
	};

	public QuoteOperationRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public Task<bool> DoesQuoteOperationExists(string quoteOperationId)
	{
		InitializeParameterLists();
		base.filterList.Add("qmoQuoteOperationID|C", quoteOperationId);
		base.selectList.Add("qmoQuoteOperationID");
		return Task.FromResult(GetAsObject("QuoteOperations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<BOMQuoteOperationDto>> GetAllQuoteOperations(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<BOMQuoteOperationDto> collection = new List<BOMQuoteOperationDto>();
		InitializeParameterLists();
		base.selectList.AddRange(quoteOperationFields);
		List<string> orderbyList = new List<string> { "qmoQuoteOperationID" };
		using (DataTable dataTable = GetAsDataTable("QuoteOperations", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				BOMQuoteOperationDto bOMQuoteOperationDto = new BOMQuoteOperationDto();
				bOMQuoteOperationDto.QuoteID = dataTable.Rows[i].Field<string>("qmoQuoteID");
				bOMQuoteOperationDto.QuoteLineID = dataTable.Rows[i].Field<short>("qmoQuoteLineID");
				bOMQuoteOperationDto.QuoteAssemblyID = dataTable.Rows[i].Field<int>("qmoQuoteAssemblyID");
				bOMQuoteOperationDto.QuoteOperationID = dataTable.Rows[i].Field<int>("qmoQuoteOperationID");
				bOMQuoteOperationDto.OperationType = dataTable.Rows[i].Field<byte>("qmoOperationType");
				bOMQuoteOperationDto.WorkCenterID = dataTable.Rows[i].Field<string>("qmoWorkCenterID");
				bOMQuoteOperationDto.ProcessID = dataTable.Rows[i].Field<string>("qmoProcessID");
				bOMQuoteOperationDto.ProcessShortDescription = dataTable.Rows[i].Field<string>("qmoProcessShortDescription");
				bOMQuoteOperationDto.ProcessLongDescriptionRtf = dataTable.Rows[i].Field<string>("qmoProcessLongDescriptionRtf");
				bOMQuoteOperationDto.ProcessLongDescriptionText = dataTable.Rows[i].Field<string>("qmoProcessLongDescriptionText");
				bOMQuoteOperationDto.QuantityPerAssembly = dataTable.Rows[i].Field<decimal>("qmoQuantityPerAssembly");
				bOMQuoteOperationDto.QueueTime = dataTable.Rows[i].Field<decimal>("qmoQueueTime");
				bOMQuoteOperationDto.SetupHours = dataTable.Rows[i].Field<decimal>("qmoSetupHours");
				bOMQuoteOperationDto.MoveTime = dataTable.Rows[i].Field<decimal>("qmoMoveTime");
				bOMQuoteOperationDto.QuotingRate = dataTable.Rows[i].Field<decimal>("qmoQuotingRate");
				bOMQuoteOperationDto.SetupRate = dataTable.Rows[i].Field<decimal>("qmoSetupRate");
				bOMQuoteOperationDto.ProductionRate = dataTable.Rows[i].Field<decimal>("qmoProductionRate");
				bOMQuoteOperationDto.OverheadRate = dataTable.Rows[i].Field<decimal>("qmoOverheadRate");
				bOMQuoteOperationDto.PartID = dataTable.Rows[i].Field<string>("qmoPartID");
				bOMQuoteOperationDto.PartRevisionID = dataTable.Rows[i].Field<string>("qmoPartRevisionID");
				bOMQuoteOperationDto.UnitOfMeasure = dataTable.Rows[i].Field<string>("qmoUnitOfMeasure");
				bOMQuoteOperationDto.SupplierOrganizationID = dataTable.Rows[i].Field<string>("qmoSupplierOrganizationID");
				bOMQuoteOperationDto.StandardFactor = dataTable.Rows[i].Field<string>("qmoStandardFactor");
				bOMQuoteOperationDto.ProductionStandard = dataTable.Rows[i].Field<decimal>("qmoProductionStandard");
				bOMQuoteOperationDto.UniqueID = dataTable.Rows[i].Field<Guid>("qmoUniqueID");
				bOMQuoteOperationDto.Closed = dataTable.Rows[i].Field<bool>("qmoClosed");
				bOMQuoteOperationDto.CreatedBy = dataTable.Rows[i].Field<string>("qmoCreatedBy");
				bOMQuoteOperationDto.CreatedDate = ((!dataTable.Rows[i].Field<DateTime?>("qmoCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[i].Field<DateTime?>("qmoCreatedDate"));
				bOMQuoteOperationDto.RowVersion = dataTable.Rows[0].Field<byte[]>("qmoRowVersion");
				collection.Add(bOMQuoteOperationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ICollection<BOMQuoteOperationDto>> GetQuoteOperationsAsync(string quoteId, string quoteLineId, string quoteAssemblyId)
	{
		ICollection<BOMQuoteOperationDto> collection = new List<BOMQuoteOperationDto>();
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
		using (DataTable dataTable = GetAsDataTable(GetSelectQuoteOperationsQuery(flag, flag2), base.filterList, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(collection);
			}
			foreach (DataRow row in dataTable.Rows)
			{
				BOMQuoteOperationDto item = new BOMQuoteOperationDto
				{
					QuoteID = row.Field<string>("qmoQuoteID"),
					QuoteLineID = row.Field<short>("qmoQuoteLineID"),
					QuoteAssemblyID = row.Field<int>("qmoQuoteAssemblyID"),
					QuoteOperationID = row.Field<int>("qmoQuoteOperationID"),
					OperationType = row.Field<byte>("qmoOperationType"),
					WorkCenterID = row.Field<string>("qmoWorkCenterID"),
					ProcessID = row.Field<string>("qmoProcessID"),
					ProcessShortDescription = row.Field<string>("qmoProcessShortDescription"),
					ProcessLongDescriptionRtf = row.Field<string>("qmoProcessLongDescriptionRtf"),
					ProcessLongDescriptionText = row.Field<string>("qmoProcessLongDescriptionText"),
					QuantityPerAssembly = row.Field<decimal>("qmoQuantityPerAssembly"),
					QueueTime = row.Field<decimal>("qmoQueueTime"),
					SetupHours = row.Field<decimal>("qmoSetupHours"),
					MoveTime = row.Field<decimal>("qmoMoveTime"),
					QuotingRate = row.Field<decimal>("qmoQuotingRate"),
					SetupRate = row.Field<decimal>("qmoSetupRate"),
					ProductionRate = row.Field<decimal>("qmoProductionRate"),
					OverheadRate = row.Field<decimal>("qmoOverheadRate"),
					PartID = row.Field<string>("qmoPartID"),
					PartRevisionID = row.Field<string>("qmoPartRevisionID"),
					UnitOfMeasure = row.Field<string>("qmoUnitOfMeasure"),
					SupplierOrganizationID = row.Field<string>("qmoSupplierOrganizationID"),
					StandardFactor = row.Field<string>("qmoStandardFactor"),
					ProductionStandard = row.Field<decimal>("qmoProductionStandard"),
					UniqueID = row.Field<Guid>("qmoUniqueID"),
					Closed = row.Field<bool>("qmoClosed"),
					CreatedBy = row.Field<string>("qmoCreatedBy"),
					CreatedDate = ((!row.Field<DateTime?>("qmoCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : row.Field<DateTime?>("qmoCreatedDate")),
					RowVersion = row.Field<byte[]>("qmoRowVersion")
				};
				collection.Add(item);
			}
		}
		return Task.FromResult(collection);
	}

	private string GetSelectQuoteOperationsQuery(bool includeQuoteLineId, bool includeQuoteAssemblyId)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("SELECT qmoQuoteID, qmoQuoteLineID, qmoQuoteAssemblyID, qmoQuoteOperationID, qmoOperationType,\r\n                          qmoWorkcenterID, qmoProcessID, qmoProcessShortDescription, qmoProcessLongDescriptionRTF,\r\n                          qmoProcessLongDescriptionText, qmoQuantityPerAssembly, qmoQueueTime, qmoSetupHours, qmoMoveTime,\r\n                          qmoQuotingRate, qmoSetupRate, qmoProductionRate, qmoOverheadRate, qmoPartID, qmoPartRevisionID,\r\n                          qmoUnitofMeasure, qmoSupplierOrganizationID, qmoStandardFactor, qmoProductionStandard,\r\n                          qmoClosed, qmoCreatedBy, qmoCreatedDate, qmoUniqueID, qmoRowVersion\r\n                  FROM QuoteOperations\r\n                  WHERE qmoQuoteID = @QuoteID");
		if (includeQuoteLineId)
		{
			stringBuilder.Append(" AND qmoQuoteLineID = @QuoteLineID");
		}
		if (includeQuoteAssemblyId)
		{
			stringBuilder.Append(" AND qmoQuoteAssemblyID = @QuoteAssemblyID");
		}
		stringBuilder.Append(";");
		return stringBuilder.ToString();
	}
}
