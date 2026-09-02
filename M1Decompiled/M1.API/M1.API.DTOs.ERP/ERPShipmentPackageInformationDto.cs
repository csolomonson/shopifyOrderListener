using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPShipmentPackageInformationDto
{
	public string spaCarrier { get; set; }

	public string spaCreatedBy { get; set; }

	public DateTime? spaCreatedDate { get; set; }

	public string spaCustomerPackageID { get; set; }

	public string spaEdi856CustomLabel { get; set; }

	public Guid spaUniqueID { get; set; }

	public string spaFedExPackageTypes { get; set; }

	public bool spaAdditionalHandlingRequired { get; set; }

	public bool spaLargePackage { get; set; }

	public bool spaVerbalConfirmationRequired { get; set; }

	public string spaLabelFilePath { get; set; }

	public string spaPackageDimensionsUom { get; set; }

	public int spaPackageHeight { get; set; }

	public int spaPackageLength { get; set; }

	public decimal spaPackageRate { get; set; }

	public decimal spaPackageRateForeign { get; set; }

	public decimal spaPackageValue { get; set; }

	public decimal spaPackageValueForeign { get; set; }

	public decimal spaPackageWeight { get; set; }

	public string spaPackageWeightUom { get; set; }

	public int spaPackageWidth { get; set; }

	public string spaReference1 { get; set; }

	public string spaReference2 { get; set; }

	public byte[] SPArowVersion { get; set; }

	public int spaShipmentPackageID { get; set; }

	public string spaShipmentID { get; set; }

	public string spaShipmentIDNumber { get; set; }

	public string spaShippingMethodID { get; set; }

	public string spaTrackingNo { get; set; }

	public string spaUpsPackageTypes { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
