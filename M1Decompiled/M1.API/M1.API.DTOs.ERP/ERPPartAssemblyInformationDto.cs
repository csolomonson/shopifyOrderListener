using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartAssemblyInformationDto
{
	public byte imaAssemblyOverlap { get; set; }

	public string imaCreatedBy { get; set; }

	public DateTime? imaCreatedDate { get; set; }

	public string imaDocuments { get; set; }

	public Guid imaUniqueID { get; set; }

	public bool imaPullAllFromStock { get; set; }

	public bool imaUseMethod { get; set; }

	public short imaLevel { get; set; }

	public int imaMethodAssemblyID { get; set; }

	public string imaMethodID { get; set; }

	public string imaMethodRevisionID { get; set; }

	public byte imaOverlapDestinationLink { get; set; }

	public decimal imaOverlapOffsetTime { get; set; }

	public int imaOverlapOperationID { get; set; }

	public byte imaOverlapSourceLink { get; set; }

	public int imaOverlapSourceOperationID { get; set; }

	public byte imaOverlapType { get; set; }

	public int imaParentAssemblyID { get; set; }

	public string imaPartID { get; set; }

	public string imaPartLongDescriptionRtf { get; set; }

	public string imaPartLongDescriptionText { get; set; }

	public string imaPartRevisionID { get; set; }

	public string imaPartShortDescription { get; set; }

	public string imaProductionNotesRTF { get; set; }

	public string imaProductionNotesText { get; set; }

	public decimal imaQuantityPerParent { get; set; }

	public byte[] imaRowVersion { get; set; }

	public string imaSourceMethodID { get; set; }

	public string imaSourceRevisionID { get; set; }

	public string imaUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
