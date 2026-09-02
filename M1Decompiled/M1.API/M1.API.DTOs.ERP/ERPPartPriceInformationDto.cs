using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartPriceInformationDto
{
	public string imiCreatedBy { get; set; }

	public DateTime? imiCreatedDate { get; set; }

	public string imiCurrencyRateID { get; set; }

	public string imiCustomerGroupID { get; set; }

	public DateTime? imiEndDate { get; set; }

	public Guid imiUniqueID { get; set; }

	public bool imiInventoryPrice { get; set; }

	public string imiLocationID { get; set; }

	public string imiOrganizationID { get; set; }

	public string imiPartGroupID { get; set; }

	public string imiPartID { get; set; }

	public string imiPartRevisionID { get; set; }

	public byte imiPriceType { get; set; }

	public string imiQuoteID { get; set; }

	public string imiRfqID { get; set; }

	public byte[] imiRowVersion { get; set; }

	public int imiPartPriceID { get; set; }

	public DateTime? imiStartDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
