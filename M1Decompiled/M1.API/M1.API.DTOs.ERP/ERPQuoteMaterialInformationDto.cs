using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPQuoteMaterialInformationDto
{
	public string qmmCreatedBy { get; set; }

	public DateTime? qmmCreatedDate { get; set; }

	public string qmmDocuments { get; set; }

	public Guid qmmUniqueID { get; set; }

	public decimal qmmEstimatedUnitCost { get; set; }

	public bool qmmBackflush { get; set; }

	public bool qmmClosed { get; set; }

	public bool qmmCostOverride { get; set; }

	public short qmmLeadTime { get; set; }

	public short qmmLeadTime1 { get; set; }

	public short qmmLeadTime2 { get; set; }

	public short qmmLeadTime3 { get; set; }

	public short qmmLeadTime4 { get; set; }

	public short qmmLeadTime5 { get; set; }

	public short qmmLeadTime6 { get; set; }

	public short qmmLeadTime7 { get; set; }

	public short qmmLeadTime8 { get; set; }

	public short qmmLeadTime9 { get; set; }

	public decimal qmmMinimumCharge { get; set; }

	public string qmmPartBinID { get; set; }

	public string qmmPartID { get; set; }

	public string qmmPartLongDescriptionRtf { get; set; }

	public string qmmPartLongDescriptionText { get; set; }

	public string qmmPartRevisionID { get; set; }

	public string qmmPartShortDescription { get; set; }

	public string qmmPartWarehouseLocationID { get; set; }

	public string qmmPurchaseLocationID { get; set; }

	public decimal qmmQuantityBreak1 { get; set; }

	public decimal qmmQuantityBreak2 { get; set; }

	public decimal qmmQuantityBreak3 { get; set; }

	public decimal qmmQuantityBreak4 { get; set; }

	public decimal qmmQuantityBreak5 { get; set; }

	public decimal qmmQuantityBreak6 { get; set; }

	public decimal qmmQuantityBreak7 { get; set; }

	public decimal qmmQuantityBreak8 { get; set; }

	public decimal qmmQuantityBreak9 { get; set; }

	public decimal qmmQuantityPerAssembly { get; set; }

	public int qmmQuoteAssemblyID { get; set; }

	public string qmmQuoteID { get; set; }

	public short qmmQuoteLineID { get; set; }

	public int qmmRelatedQuoteOperationID { get; set; }

	public byte[] qmmRowVersion { get; set; }

	public decimal qmmScrapPercent { get; set; }

	public decimal qmmScrapQuantity { get; set; }

	public int qmmQuoteMaterialID { get; set; }

	public int qmmSourcePriceID { get; set; }

	public string qmmSourceRfqID { get; set; }

	public string qmmSupplierOrganizationID { get; set; }

	public decimal qmmUnitCost1 { get; set; }

	public decimal qmmUnitCost2 { get; set; }

	public decimal qmmUnitCost3 { get; set; }

	public decimal qmmUnitCost4 { get; set; }

	public decimal qmmUnitCost5 { get; set; }

	public decimal qmmUnitCost6 { get; set; }

	public decimal qmmUnitCost7 { get; set; }

	public decimal qmmUnitCost8 { get; set; }

	public decimal qmmUnitCost9 { get; set; }

	public string qmmUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
