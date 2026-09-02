using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPQuoteAssemblyInformationDto
{
	public byte qmaAssemblyOverlap { get; set; }

	public string qmaCreatedBy { get; set; }

	public DateTime? qmaCreatedDate { get; set; }

	public string qmaDocuments { get; set; }

	public Guid qmaUniqueID { get; set; }

	public bool qmaClosed { get; set; }

	public bool qmaPullAllFromStock { get; set; }

	public short qmaLevel { get; set; }

	public byte qmaOverlapDestinationLink { get; set; }

	public decimal qmaOverlapOffsetTime { get; set; }

	public int qmaOverlapOperationID { get; set; }

	public byte qmaOverlapSourceLink { get; set; }

	public int qmaOverlapSourceOperationID { get; set; }

	public byte qmaOverlapType { get; set; }

	public int qmaParentAssemblyID { get; set; }

	public string qmaPartID { get; set; }

	public string qmaPartLongDescriptionRtf { get; set; }

	public string qmaPartLongDescriptionText { get; set; }

	public string qmaPartRevisionID { get; set; }

	public string qmaPartShortDescription { get; set; }

	public string qmaProductionNotesRTF { get; set; }

	public string qmaProductionNotesText { get; set; }

	public decimal qmaQuantityPerParent { get; set; }

	public string qmaQuoteID { get; set; }

	public short qmaQuoteLineID { get; set; }

	public byte[] qmaRowVersion { get; set; }

	public int qmaQuoteAssemblyID { get; set; }

	public string qmaSourceMethodID { get; set; }

	public string qmaSourceRevisionID { get; set; }

	public string qmaUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
