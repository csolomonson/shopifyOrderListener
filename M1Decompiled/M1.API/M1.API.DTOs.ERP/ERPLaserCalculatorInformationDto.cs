using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPLaserCalculatorInformationDto
{
	public Guid ccpLaserCalculatorID { get; set; }

	public string ccpCreatedBy { get; set; }

	public DateTime? ccpCreatedDate { get; set; }

	public string ccpdescription { get; set; }

	public Guid ccpUniqueID { get; set; }

	public decimal ccpExternalFeed { get; set; }

	public decimal ccpHoleCutTime { get; set; }

	public bool ccpObround { get; set; }

	public bool ccpOther { get; set; }

	public bool ccpRectangle { get; set; }

	public bool ccpRound { get; set; }

	public bool ccpSquare { get; set; }

	public string ccpLaserMaterialTypeID { get; set; }

	public decimal ccpLeadInOut { get; set; }

	public decimal ccpLeadInOutFeed { get; set; }

	public decimal ccpLeadInOutTime { get; set; }

	public decimal ccplength { get; set; }

	public string ccpMeasurementType { get; set; }

	public int ccpNumberOfHoles { get; set; }

	public decimal ccpPartPerimeter { get; set; }

	public decimal ccpPerimeterCutTime { get; set; }

	public decimal ccpPiercedHoles { get; set; }

	public decimal ccpPierceTime { get; set; }

	public decimal ccpQuantity { get; set; }

	public decimal ccpRate { get; set; }

	public byte[] ccpRowVersion { get; set; }

	public decimal ccpThickness { get; set; }

	public decimal ccpTotalCutTime { get; set; }

	public decimal ccpTotalLeadInOutTime { get; set; }

	public decimal ccpTotalPierceTime { get; set; }

	public decimal ccpWidth { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
