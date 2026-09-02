using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPFreightPackageRateInformationDto
{
	public string fprCreatedBy { get; set; }

	public DateTime? fprCreatedDate { get; set; }

	public Guid fprUniqueID { get; set; }

	public decimal fprFdxBaseCharge { get; set; }

	public string fprFdxCurrency { get; set; }

	public DateTime? fprFdxDeliveryDate { get; set; }

	public string fprFdxDeliveryDay { get; set; }

	public string fprFdxDestinationStationID { get; set; }

	public decimal fprFdxPackageBaseCharge { get; set; }

	public decimal fprFdxPackageBillingWeight { get; set; }

	public decimal fprFdxPackageDimWeight { get; set; }

	public decimal fprFdxPackageFreightDiscount { get; set; }

	public decimal fprFdxPackageNetCharge { get; set; }

	public decimal fprFdxPackageNetFreight { get; set; }

	public decimal fprFdxPackageSurcharges { get; set; }

	public string fprFdxPackaging { get; set; }

	public string fprFdxService { get; set; }

	public short fprFdxTimeInTransit { get; set; }

	public decimal fprFdxTotalBillingWeight { get; set; }

	public decimal fprFdxTotalCustomerCharge { get; set; }

	public decimal fprFdxTotalDimWeight { get; set; }

	public decimal fprFdxTotalFreightDiscount { get; set; }

	public decimal fprFdxTotalNetCharge { get; set; }

	public decimal fprFdxTotalNetFreightCharge { get; set; }

	public decimal fprFdxTotalSurcharges { get; set; }

	public string fprFdxUnits { get; set; }

	public decimal fprFdxVariableHandlingCharge { get; set; }

	public short fprFreightPackageID { get; set; }

	public string fprFreightShipmentID { get; set; }

	public string fprRCTI { get; set; }

	public byte[] fprRowVersion { get; set; }

	public short fprFreightPackageRateID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
