using System;

namespace M1.API.DTOs.Custom;

public class PartBinDetailInformationDto
{
	public string PartID { get; set; }

	public string PartRevisionID { get; set; }

	public string PartBinID { get; set; }

	public int PartBinDetailID { get; set; }

	public string WarehouseID { get; set; }

	public DateTime? TransactionDate { get; set; }

	public short? QuantityType { get; set; }

	public decimal? OriginalQuantity { get; set; }

	public decimal? RemainingQuantity { get; set; }

	public decimal? UnitCost { get; set; }

	public string SourceTableName { get; set; }

	public string CreatedBy { get; set; }

	public DateTime? CreatedDate { get; set; }

	public Guid UniqueID { get; set; }

	public byte[] RowVersion { get; set; }
}
