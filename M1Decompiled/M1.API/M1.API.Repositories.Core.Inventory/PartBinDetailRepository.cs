using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using M1.API.DTOs.Custom;
using M1.API.Utilities;
using M1.Core;

namespace M1.API.Repositories.Core.Inventory;

public class PartBinDetailRepository : APIBaseRepository, IPartBinDetailRepository, IAPIBaseRepository, IDisposable
{
	public PartBinDetailRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public PartBinDetailRepository(M1Database database)
	{
		base.M1database = database;
	}

	public Task<IList<PartBinDetailInformationDto>> GetPartBinDetailsInfo(string partId)
	{
		IList<PartBinDetailInformationDto> list = new List<PartBinDetailInformationDto>();
		InitializeParameterLists();
		base.filterList.Add("@p1", partId);
		using (DataTable dataTable = GetAsDataTable(GetActivePartBinDetailsQuery(includeWhereCondition: true), base.filterList, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(list);
			}
			foreach (DataRow row in dataTable.Rows)
			{
				DateTime result;
				DateTime value = (DateTime.TryParse(row["imgTransactionDate"]?.ToString(), out result) ? result : DateTime.MinValue);
				DateTime result2;
				DateTime value2 = (DateTime.TryParse(row["imgCreatedDate"]?.ToString(), out result2) ? result2 : DateTime.MinValue);
				PartBinDetailInformationDto item = new PartBinDetailInformationDto
				{
					PartID = partId,
					PartRevisionID = row["imgPartRevisionID"].ToString().Trim(),
					PartBinID = row["imgPartBinID"].ToString().Trim(),
					PartBinDetailID = int.Parse(row["imgPartBinDetailID"].ToString().Trim()),
					WarehouseID = row["imgWarehouseID"].ToString().Trim(),
					TransactionDate = value,
					QuantityType = short.Parse(row["imgQuantityType"].ToString().Trim()),
					OriginalQuantity = Convert.ToDecimal(row["imgOriginalQuantity"].ToString().Trim()),
					RemainingQuantity = Convert.ToDecimal(row["imgRemainingQuantity"].ToString().Trim()),
					UnitCost = Convert.ToDecimal(row["unitCost"].ToString().Trim()),
					SourceTableName = row["imgSourceTableName"].ToString().Trim(),
					CreatedBy = row["imgCreatedBy"].ToString().Trim(),
					CreatedDate = value2,
					UniqueID = Guid.Parse(row["imgUniqueID"].ToString().Trim()),
					RowVersion = (byte[])row["imgRowVersion"]
				};
				list.Add(item);
			}
		}
		return Task.FromResult(list);
	}

	public Task<ICollection<PartBinDetailInformationDto>> GetAllPartBinDetails(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<PartBinDetailInformationDto> collection = new List<PartBinDetailInformationDto>();
		InitializeParameterLists();
		using (DataTable dataTable = GetAsDataTable(GetActivePartBinDetailsQuery(), base.filterList, null))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				PartBinDetailInformationDto partBinDetailInformationDto = new PartBinDetailInformationDto();
				partBinDetailInformationDto.PartID = dataTable.Rows[i].Field<string>("imgPartID");
				partBinDetailInformationDto.PartRevisionID = dataTable.Rows[i].Field<string>("imgPartRevisionID");
				partBinDetailInformationDto.PartBinID = dataTable.Rows[i].Field<string>("imgPartBinID");
				partBinDetailInformationDto.PartBinDetailID = dataTable.Rows[i].Field<int>("imgPartBinDetailID");
				partBinDetailInformationDto.WarehouseID = dataTable.Rows[i].Field<string>("imgWarehouseID");
				partBinDetailInformationDto.TransactionDate = Convert.ToDateTime(string.IsNullOrWhiteSpace(dataTable.Rows[i].Field<DateTime?>("imgTransactionDate").ToString()) ? DateTime.Parse("01/01/1900") : DateTime.Parse(dataTable.Rows[i].Field<DateTime?>("imgTransactionDate").ToString()));
				partBinDetailInformationDto.QuantityType = dataTable.Rows[i].Field<byte>("imgQuantityType");
				partBinDetailInformationDto.OriginalQuantity = dataTable.Rows[i].Field<decimal>("imgOriginalQuantity");
				partBinDetailInformationDto.RemainingQuantity = dataTable.Rows[i].Field<decimal>("imgRemainingQuantity");
				partBinDetailInformationDto.UnitCost = dataTable.Rows[i].Field<decimal>("unitCost");
				partBinDetailInformationDto.SourceTableName = dataTable.Rows[i].Field<string>("imgSourceTableName");
				partBinDetailInformationDto.CreatedBy = dataTable.Rows[i].Field<string>("imgCreatedBy");
				partBinDetailInformationDto.CreatedDate = ((!dataTable.Rows[i].Field<DateTime?>("imgCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[i].Field<DateTime?>("imgCreatedDate"));
				partBinDetailInformationDto.UniqueID = dataTable.Rows[i].Field<Guid>("imgUniqueID");
				partBinDetailInformationDto.RowVersion = dataTable.Rows[i].Field<byte[]>("imgRowVersion");
				collection.Add(partBinDetailInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	private string GetActivePartBinDetailsQuery(bool includeWhereCondition = false)
	{
		string text = "SELECT imgPartID,imgPartRevisionID,imgPartBinID,imgPartBinDetailID,imgTransactionDate,imgQuantityType,imgWarehouseID\r\n                            ,imgOriginalQuantity,imgRemainingQuantity,imgUnitLaborCost,imgUnitOverheadCost,imgUnitMaterialCost,imgUnitSubcontractCost\r\n                            ,imgUnitDutyCost,imgUnitFreightCost,imgUnitMiscCost\r\n                            ,ISNULL(SUM(imgUnitLaborCost + imgUnitOverheadCost + imgUnitMaterialCost + imgUnitSubcontractCost + imgUnitDutyCost + imgUnitFreightCost + imgUnitMiscCost), 0) AS unitCost\r\n                            ,imgSourceTableName,imgCreatedBy,imgCreatedDate,imgUniqueID,imgRowVersion\r\n                        FROM PartBinDetails\r\n                        WHERE (ISNULL(imgTransactionDate, Convert(DATETIME, '01/01/2999', 103)) < GETDATE())";
		if (includeWhereCondition)
		{
			text += " AND imgPartID = @p1";
		}
		return text + " GROUP BY imgPartID,imgPartRevisionID,imgPartBinID,imgPartBinDetailID,imgTransactionDate,imgQuantityType,imgWarehouseID\r\n                        ,imgOriginalQuantity,imgRemainingQuantity,imgUnitLaborCost,imgUnitOverheadCost,imgUnitMaterialCost,imgUnitSubcontractCost\r\n                        ,imgUnitDutyCost,imgUnitFreightCost,imgUnitMiscCost\r\n                        ,imgSourceTableName,imgCreatedBy,imgCreatedDate,imgUniqueID,imgRowVersion";
	}
}
