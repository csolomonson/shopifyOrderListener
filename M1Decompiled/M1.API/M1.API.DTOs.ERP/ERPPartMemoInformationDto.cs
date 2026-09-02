using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartMemoInformationDto
{
	public string imkCreatedBy { get; set; }

	public DateTime? imkCreatedDate { get; set; }

	public Guid imkUniqueID { get; set; }

	public string imkLongDescriptionRtf { get; set; }

	public string imkLongDescriptionText { get; set; }

	public DateTime? imkMemoDate { get; set; }

	public string imkPartID { get; set; }

	public string imkPartRevisionID { get; set; }

	public byte[] imkRowVersion { get; set; }

	public short imkPartMemoID { get; set; }

	public string imkShortDescription { get; set; }

	public bool imkShowInApInvoices { get; set; }

	public bool imkShowInArInvoices { get; set; }

	public bool imkShowInCalls { get; set; }

	public bool imkShowInChangeRequests { get; set; }

	public bool imkShowInDmrClaims { get; set; }

	public bool imkShowInDmrShipments { get; set; }

	public bool imkShowInInspections { get; set; }

	public bool imkShowInJobAssemblies { get; set; }

	public bool imkShowInJobMaterials { get; set; }

	public bool imkShowInJobOperations { get; set; }

	public bool imkShowInJobs { get; set; }

	public bool imkShowInKnowledgebasePages { get; set; }

	public bool imkShowInLeads { get; set; }

	public bool imkShowInNonconformances { get; set; }

	public bool imkShowInPartAssemblies { get; set; }

	public bool imkShowInPartMaterials { get; set; }

	public bool imkShowInPartOperations { get; set; }

	public bool imkShowInPartRevisions { get; set; }

	public bool imkShowInPriceAndAvailability { get; set; }

	public bool imkShowInPurchaseOrders { get; set; }

	public bool imkShowInQuoteAssemblies { get; set; }

	public bool imkShowInQuoteLines { get; set; }

	public bool imkShowInQuoteMaterials { get; set; }

	public bool imkShowInQuoteOperations { get; set; }

	public bool imkShowInReceipts { get; set; }

	public bool imkShowInRfqs { get; set; }

	public bool imkShowInRmaClaims { get; set; }

	public bool imkShowInRmaReceipts { get; set; }

	public bool imkShowInSalesOrders { get; set; }

	public bool imkShowInServiceContracts { get; set; }

	public bool imkShowInShipments { get; set; }

	public bool imkShowInWarehouseReceipts { get; set; }

	public bool imkShowInWarehouseRequisitions { get; set; }

	public bool imkShowInWarehouseTransfers { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
