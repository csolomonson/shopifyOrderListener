using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPQuoteLineInformationDto
{
	public string qmlCreatedBy { get; set; }

	public DateTime? qmlCreatedDate { get; set; }

	public string qmlDocuments { get; set; }

	public Guid qmlUniqueID { get; set; }

	public bool qmlClosed { get; set; }

	public bool qmlCreatedFromMobile { get; set; }

	public bool qmlFirm { get; set; }

	public bool qmlMatrixCalculated { get; set; }

	public bool qmlPurchaseToOrder { get; set; }

	public bool qmlTransferredToOrder { get; set; }

	public string qmlLeadID { get; set; }

	public short qmlLeadLineID { get; set; }

	public string qmlNonTaxReasonID { get; set; }

	public string qmlOrgPartID { get; set; }

	public string qmlOrgPartShortDescription { get; set; }

	public string qmlPartGroupID { get; set; }

	public string qmlPartID { get; set; }

	public string qmlPartLongDescriptionRtf { get; set; }

	public string qmlPartLongDescriptionText { get; set; }

	public string qmlPartRevisionID { get; set; }

	public string qmlPartShortDescription { get; set; }

	public string qmlProductionNotesRTF { get; set; }

	public string qmlProductionNotesText { get; set; }

	public string qmlProjectAreaID { get; set; }

	public string qmlProjectID { get; set; }

	public string qmlPurchaseLocationID { get; set; }

	public decimal qmlPurchaseUnitCostBase { get; set; }

	public decimal qmlPurchaseUnitCostForeign { get; set; }

	public byte qmlQuantityToTotal { get; set; }

	public string qmlQuoteID { get; set; }

	public byte qmlQuoteMarkupType { get; set; }

	public string qmlResolutionReasonID { get; set; }

	public byte[] qmlRowVersion { get; set; }

	public string qmlSecondTaxCodeID { get; set; }

	public short qmlQuoteLineID { get; set; }

	public string qmlSourceMethodID { get; set; }

	public string qmlSourceRevisionID { get; set; }

	public string qmlSupplierOrganizationID { get; set; }

	public string qmlTaxCodeID { get; set; }

	public DateTime? qmlTaxDate { get; set; }

	public string qmlUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
