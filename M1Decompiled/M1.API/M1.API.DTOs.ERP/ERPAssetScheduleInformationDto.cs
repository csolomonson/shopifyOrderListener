using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAssetScheduleInformationDto
{
	public int fasActualProductionUnits { get; set; }

	public decimal fasAdditionalAssetAmount { get; set; }

	public string fasAssetID { get; set; }

	public decimal fasClosingAccumBalance { get; set; }

	public decimal fasClosingAssetValue { get; set; }

	public string fasCreatedBy { get; set; }

	public DateTime? fasCreatedDate { get; set; }

	public decimal fasDepreciationAmount { get; set; }

	public Guid fasUniqueID { get; set; }

	public int fasEstimatedProductionUnits { get; set; }

	public short fasGlFiscalYearID { get; set; }

	public byte fasGlFiscalYearPeriodID { get; set; }

	public bool fasPostedToGl { get; set; }

	public decimal fasNetAssetValue { get; set; }

	public decimal fasOpeningAccumBalance { get; set; }

	public decimal fasOpeningAssetValue { get; set; }

	public byte[] fasRowVersion { get; set; }

	public int fasAssetScheduleID { get; set; }

	public decimal fasSubtractAssetAmount { get; set; }

	public string fasType { get; set; }

	public decimal fasWritebackAmount { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
