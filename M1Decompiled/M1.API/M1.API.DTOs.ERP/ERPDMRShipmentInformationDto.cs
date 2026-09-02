using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPDMRShipmentInformationDto
{
	public string dspApInvoiceLocationID { get; set; }

	public DateTime? dspClosedDate { get; set; }

	public string dspDmrShipmentID { get; set; }

	public string dspCreatedBy { get; set; }

	public DateTime? dspCreatedDate { get; set; }

	public string dspCurrencyRateID { get; set; }

	public Guid dspUniqueID { get; set; }

	public decimal dspExchangeRate { get; set; }

	public decimal dspFreightCharge { get; set; }

	public decimal dspFreightChargeForeign { get; set; }

	public decimal dspFreightSubtotal { get; set; }

	public decimal dspFreightTotal { get; set; }

	public bool dspClosed { get; set; }

	public bool dspCustomRate { get; set; }

	public bool dspPosted { get; set; }

	public bool dspPrintDmrPackingSlip { get; set; }

	public bool dspPrintLabels { get; set; }

	public bool dspReversalEntry { get; set; }

	public bool dspReversed { get; set; }

	public short dspNumberOfLabels { get; set; }

	public string dspPlantDepartmentID { get; set; }

	public string dspPlantID { get; set; }

	public DateTime? dspPostedDate { get; set; }

	public string dspProjectID { get; set; }

	public byte[] dspRowVersion { get; set; }

	public string dspShipContactID { get; set; }

	public DateTime? dspShipDate { get; set; }

	public string dspShipLocationID { get; set; }

	public string dspShippingCommentsRTF { get; set; }

	public string dspShippingCommentsText { get; set; }

	public string dspShippingMethodID { get; set; }

	public string dspShippingPaymentTypeID { get; set; }

	public string dspStandardMessageID { get; set; }

	public string dspSupplierOrganizationID { get; set; }

	public string dspTrackingNumber { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
