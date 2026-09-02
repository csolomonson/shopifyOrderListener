using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAssetAdjustmentInformationDto
{
	public decimal faaAccumulatedDepreciation { get; set; }

	public DateTime? faaAdjustmentDate { get; set; }

	public string faaAdjustmentType { get; set; }

	public string faaArInvoiceContactID { get; set; }

	public string faaArInvoiceLocationID { get; set; }

	public string faaAssetID { get; set; }

	public string faaAuthorizedByEmployeeID { get; set; }

	public decimal faaClosingPercent { get; set; }

	public decimal faaClosingPeriodDepreciation { get; set; }

	public string faaCreatedBy { get; set; }

	public DateTime? faaCreatedDate { get; set; }

	public string faaCurrencyRateID { get; set; }

	public string faaCustomerOrganizationID { get; set; }

	public decimal faaDepreciationThisYear { get; set; }

	public string faaDestinationPlantID { get; set; }

	public Guid faaUniqueID { get; set; }

	public decimal faaExchangeRate { get; set; }

	public short faaGlFiscalYearID { get; set; }

	public byte faaGlFiscalYearPeriodID { get; set; }

	public bool faaCustomRate { get; set; }

	public bool faaPostedToGl { get; set; }

	public string faaLongDescriptionRtf { get; set; }

	public string faaLongDescriptionText { get; set; }

	public decimal faaNetAssetValue { get; set; }

	public decimal faaOpeningAssetValue { get; set; }

	public DateTime? faaPostedDate { get; set; }

	public decimal faaProfitOrLoss { get; set; }

	public int faaQuantity { get; set; }

	public byte[] faaRowVersion { get; set; }

	public int faaAssetAdjustmentID { get; set; }

	public string faaSourcePlantID { get; set; }

	public decimal faaValue { get; set; }

	public decimal faaValueForeign { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
