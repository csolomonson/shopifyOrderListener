using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPOrganizationMemoInformationDto
{
	public string cmmContactID { get; set; }

	public string cmmCreatedBy { get; set; }

	public DateTime? cmmCreatedDate { get; set; }

	public Guid cmmUniqueID { get; set; }

	public string cmmLocationID { get; set; }

	public string cmmLongDescriptionRtf { get; set; }

	public string cmmLongDescriptionText { get; set; }

	public DateTime? cmmMemoDate { get; set; }

	public string cmmOrganizationID { get; set; }

	public byte[] cmmRowVersion { get; set; }

	public short cmmOrganizationMemoID { get; set; }

	public string cmmShortDescription { get; set; }

	public bool cmmShowInApInvoices { get; set; }

	public bool cmmShowInApPayments { get; set; }

	public bool cmmShowInArInvoices { get; set; }

	public bool cmmShowInArPayments { get; set; }

	public bool cmmShowInCalls { get; set; }

	public bool cmmShowInDmrClaims { get; set; }

	public bool cmmShowInDmrShipments { get; set; }

	public bool cmmShowInLeads { get; set; }

	public bool cmmShowInOrganizations { get; set; }

	public bool cmmShowInPriceAndAvailability { get; set; }

	public bool cmmShowInPurchaseOrders { get; set; }

	public bool cmmShowInQuotes { get; set; }

	public bool cmmShowInReceipts { get; set; }

	public bool cmmShowInRfqs { get; set; }

	public bool cmmShowInRmaClaims { get; set; }

	public bool cmmShowInRmaReceipts { get; set; }

	public bool cmmShowInSalesOrders { get; set; }

	public bool cmmShowInShipments { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
