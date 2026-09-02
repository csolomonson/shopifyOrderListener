using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartInformationDto
{
	public string impPartID { get; set; }

	public short impContractLength { get; set; }

	public string impContractLengthType { get; set; }

	public string impCreatedBy { get; set; }

	public DateTime? impCreatedDate { get; set; }

	public string impCycleCodeID { get; set; }

	public byte impDeliveryType { get; set; }

	public Guid impUniqueID { get; set; }

	public DateTime? impInactiveDate { get; set; }

	public bool impInactive { get; set; }

	public bool impAlwaysNonTaxable { get; set; }

	public bool impBuyForInventory { get; set; }

	public bool impNonPhysicalShipment { get; set; }

	public bool impNonStockedItem { get; set; }

	public bool impPhantomOrKitPart { get; set; }

	public bool impTrackLotNumbers { get; set; }

	public bool impTrackSerialNumbers { get; set; }

	public string impLongDescriptionRtf { get; set; }

	public string impLongDescriptionText { get; set; }

	public string impNextSerialNumberIDFormula { get; set; }

	public string impNonTaxReasonID { get; set; }

	public string impOEMOrganizationID { get; set; }

	public string impPartClassID { get; set; }

	public string impPartGroupID { get; set; }

	public byte impPartType { get; set; }

	public byte impReorderMethod { get; set; }

	public byte[] impRowVersion { get; set; }

	public string impSecondTaxCodeID { get; set; }

	public string impShortDescription { get; set; }

	public string impTaxCodeID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
