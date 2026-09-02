using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Job;

[Serializable]
[DataContract(Namespace = "", Name = "job")]
[XmlRoot]
[XmlType(AnonymousType = true)]
public class BOMJobDto
{
	[XmlElement(ElementName = "jobID")]
	[DataMember(Name = "jobID", Order = 1)]
	public string JobID { get; set; }

	[XmlElement(ElementName = "plantID")]
	[DataMember(Name = "plantID", Order = 2)]
	public string PlantID { get; set; }

	[XmlElement(ElementName = "productionDueDate")]
	[DataMember(Name = "productionDueDate", Order = 3)]
	public DateTime? ProductionDueDate { get; set; }

	[XmlElement(ElementName = "customerOrganizationID")]
	[DataMember(Name = "customerOrganizationID", Order = 4)]
	public string CustomerOrganizationID { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 5)]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 6)]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "partWareHouseLocationID")]
	[DataMember(Name = "partWareHouseLocationID", Order = 7)]
	public string PartWareHouseLocationID { get; set; }

	[XmlElement(ElementName = "partBinID")]
	[DataMember(Name = "partBinID", Order = 8)]
	public string PartBinID { get; set; }

	[XmlElement(ElementName = "jobPriorityID")]
	[DataMember(Name = "jobPriorityID", Order = 9)]
	public short JobPriorityID { get; set; }

	[XmlElement(ElementName = "unitOfMeasure")]
	[DataMember(Name = "unitOfMeasure", Order = 10)]
	public string UnitOfMeasure { get; set; }

	[XmlElement(ElementName = "orderQuantity")]
	[DataMember(Name = "orderQuantity", Order = 11)]
	public decimal OrderQuantity { get; set; }

	[XmlElement(ElementName = "inventoryQuantity")]
	[DataMember(Name = "inventoryQuantity", Order = 12)]
	public decimal InventoryQuantity { get; set; }

	[XmlElement(ElementName = "scrapQuantity")]
	[DataMember(Name = "scrapQuantity", Order = 13)]
	public decimal ScrapQuantity { get; set; }

	[XmlElement(ElementName = "scrapQuantityCompleted")]
	[DataMember(Name = "scrapQuantityCompleted", Order = 14)]
	public decimal ScrapQuantityCompleted { get; set; }

	[XmlElement(ElementName = "reworkQuantity")]
	[DataMember(Name = "reworkQuantity", Order = 15)]
	public decimal ReworkQuantity { get; set; }

	[XmlElement(ElementName = "reworkDate")]
	[DataMember(Name = "reworkDate", Order = 16)]
	public DateTime? ReworkDate { get; set; }

	[XmlElement(ElementName = "productionQuantity")]
	[DataMember(Name = "productionQuantity", Order = 17)]
	public decimal ProductionQuantity { get; set; }

	[XmlElement(ElementName = "planningComplete")]
	[DataMember(Name = "planningComplete", Order = 18)]
	public bool PlanningComplete { get; set; }

	[XmlElement(ElementName = "scheduleComplete")]
	[DataMember(Name = "scheduleComplete", Order = 19)]
	public bool ScheduleComplete { get; set; }

	[XmlElement(ElementName = "productionComplete")]
	[DataMember(Name = "productionComplete", Order = 20)]
	public bool ProductionComplete { get; set; }

	[XmlElement(ElementName = "releasedToFloor")]
	[DataMember(Name = "releasedToFloor", Order = 21)]
	public bool ReleasedToFloor { get; set; }

	[XmlElement(ElementName = "closed")]
	[DataMember(Name = "closed", Order = 22)]
	public bool Closed { get; set; }

	[XmlElement(ElementName = "projectAreaID")]
	[DataMember(Name = "projectAreaID", Order = 23)]
	public string ProjectAreaID { get; set; }

	[XmlElement(ElementName = "projectID")]
	[DataMember(Name = "projectID", Order = 24)]
	public string ProjectID { get; set; }

	[XmlElement(ElementName = "partForecastPeriodID")]
	[DataMember(Name = "partForecastPeriodID", Order = 25)]
	public short PartForecastPeriodID { get; set; }

	[XmlElement(ElementName = "partForecastYearID")]
	[DataMember(Name = "partForecastYearID", Order = 26)]
	public short PartForecastYearID { get; set; }

	[XmlElement(ElementName = "firm")]
	[DataMember(Name = "firm", Order = 27)]
	public bool Firm { get; set; }

	[XmlElement(ElementName = "quantityShipped")]
	[DataMember(Name = "quantityShipped", Order = 28)]
	public decimal QuantityShipped { get; set; }

	[XmlElement(ElementName = "quantityReceivedToInventory")]
	[DataMember(Name = "quantityReceivedToInventory", Order = 29)]
	public decimal QuantityReceivedToInventory { get; set; }

	[XmlElement(ElementName = "nestlinkProcessed")]
	[DataMember(Name = "nestlinkProcessed", Order = 30)]
	public bool NestlinkProcessed { get; set; }

	[XmlElement(ElementName = "scheduledDueDate")]
	[DataMember(Name = "scheduledDueDate", Order = 31)]
	public DateTime? DueDate { get; set; }
}
