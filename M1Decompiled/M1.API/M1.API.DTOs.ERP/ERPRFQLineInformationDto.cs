using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPRFQLineInformationDto
{
	public string rqlCreatedBy { get; set; }

	public DateTime? rqlCreatedDate { get; set; }

	public string rqlDocuments { get; set; }

	public Guid rqlUniqueID { get; set; }

	public string rqlInventoryUnitOfMeasure { get; set; }

	public bool rqlAlternatePart { get; set; }

	public bool rqlClosed { get; set; }

	public int rqlJobAssemblyID { get; set; }

	public decimal rqlJobEstimatedQty { get; set; }

	public string rqlJobID { get; set; }

	public int rqlJobMaterialID { get; set; }

	public int rqlJobOperationID { get; set; }

	public string rqlPartID { get; set; }

	public string rqlPartLongDescriptionRtf { get; set; }

	public string rqlPartLongDescriptionText { get; set; }

	public string rqlPartRevisionID { get; set; }

	public string rqlPartShortDescription { get; set; }

	public string rqlProjectAreaID { get; set; }

	public string rqlProjectID { get; set; }

	public string rqlPurchaseUnitOfMeasure { get; set; }

	public int rqlQuoteAssemblyID { get; set; }

	public string rqlQuoteID { get; set; }

	public short rqlQuoteLineID { get; set; }

	public int rqlQuoteMaterialID { get; set; }

	public int rqlQuoteOperationID { get; set; }

	public string rqlRfqID { get; set; }

	public byte rqlRfqType { get; set; }

	public byte[] rqlRowVersion { get; set; }

	public short rqlRfqLineID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
