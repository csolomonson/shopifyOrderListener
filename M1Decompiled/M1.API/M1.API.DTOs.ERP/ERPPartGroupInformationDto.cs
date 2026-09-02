using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartGroupInformationDto
{
	public string imuArDepositGlAccountID { get; set; }

	public string imuAvalaraTaxCodeID { get; set; }

	public string imuPartGroupID { get; set; }

	public string imuCogsLaborGlAccountID { get; set; }

	public string imuCogsMaterialGlAccountID { get; set; }

	public string imuCogsOverheadGlAccountID { get; set; }

	public string imuCogsSubcontractGlAccountID { get; set; }

	public decimal imuCommissionRate { get; set; }

	public byte imuCommissionType { get; set; }

	public string imuCreatedBy { get; set; }

	public DateTime? imuCreatedDate { get; set; }

	public string imuDescription { get; set; }

	public string imuDiscountGlAccountID { get; set; }

	public Guid imuUniqueID { get; set; }

	public DateTime? imuInactiveDate { get; set; }

	public bool imuInactive { get; set; }

	public string imuNextSerialNumberIDFormula { get; set; }

	public byte imuNextSerialNumberOption { get; set; }

	public string imuNextSerialNumberValue { get; set; }

	public string imuParentPartGroupID { get; set; }

	public string imuPartImageFileName { get; set; }

	public decimal imuQmLaborMarkup { get; set; }

	public byte imuQmMarkupOption { get; set; }

	public decimal imuQmMaterialMarkup { get; set; }

	public decimal imuQmOverHeadMarkup { get; set; }

	public decimal imuQmPurchaseToOrderMarkup { get; set; }

	public byte imuQmQuoteMarkupType { get; set; }

	public decimal imuQmQuotingMarkup { get; set; }

	public decimal imuQmSubcontractMarkup { get; set; }

	public byte[] imuRowVersion { get; set; }

	public string imuSalesGlAccountID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
