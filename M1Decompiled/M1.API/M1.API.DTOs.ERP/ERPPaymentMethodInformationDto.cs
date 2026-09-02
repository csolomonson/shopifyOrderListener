using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPaymentMethodInformationDto
{
	public byte xahArPaymentSessionRule { get; set; }

	public string xahBankAccountID { get; set; }

	public string xahPaymentMethodID { get; set; }

	public string xahCreatedBy { get; set; }

	public DateTime? xahCreatedDate { get; set; }

	public string xahDescription { get; set; }

	public Guid xahUniqueID { get; set; }

	public DateTime? xahInactiveDate { get; set; }

	public bool xahInactive { get; set; }

	public bool xahDoNotOpenCashDrawer { get; set; }

	public bool xahPmAmex { get; set; }

	public bool xahPmCash { get; set; }

	public bool xahPmCheck { get; set; }

	public bool xahPmDiners { get; set; }

	public bool xahPmDiscover { get; set; }

	public bool xahPmEnroute { get; set; }

	public bool xahPmJAL { get; set; }

	public bool xahPmJCB { get; set; }

	public bool xahPmMasterCard { get; set; }

	public bool xahPmPurchaseOrder { get; set; }

	public bool xahPmStoreCredit { get; set; }

	public bool xahPmVisa { get; set; }

	public byte xahRefundPriority { get; set; }

	public byte[] xahRowVersion { get; set; }

	public decimal xahSettlementTime { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
