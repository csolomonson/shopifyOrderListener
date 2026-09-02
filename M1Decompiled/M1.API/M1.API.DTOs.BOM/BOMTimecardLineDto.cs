using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[DataContract(Namespace = "", Name = "timecardline")]
[XmlRoot(ElementName = "timecardline")]
[XmlType(AnonymousType = true)]
public class BOMTimecardLineDto
{
	[XmlElement(ElementName = "timecardID")]
	[DataMember(Name = "timecardID", Order = 1)]
	[Required(ErrorMessage = "TimecardID is invalid or empty.")]
	public int TimecardID { get; set; }

	[XmlElement(ElementName = "timecardLineID")]
	[DataMember(Name = "timecardLineID", Order = 2)]
	[Required(ErrorMessage = "TimecardLineID is invalid or empty.")]
	public short TimecardLineID { get; set; }

	[XmlElement(ElementName = "employeeID")]
	[DataMember(Name = "employeeID", Order = 3)]
	[Required(ErrorMessage = "EmployeeID is invalid or empty.")]
	public string EmployeeID { get; set; }

	[XmlElement(ElementName = "jobID")]
	[DataMember(Name = "jobID", Order = 4)]
	public string JobID { get; set; }

	[XmlElement(ElementName = "jobAssemblyID")]
	[DataMember(Name = "jobAssemblyID", Order = 5)]
	public int JobAssemblyID { get; set; }

	[XmlElement(ElementName = "jobOperationID")]
	[DataMember(Name = "jobOperationID", Order = 6)]
	public int JobOperationID { get; set; }

	[XmlElement(ElementName = "workCenterID")]
	[DataMember(Name = "workCenterID", Order = 7)]
	[Required(ErrorMessage = "WorkCenterID is invalid or empty.")]
	public string WorkCenterID { get; set; }

	[XmlElement(ElementName = "processID")]
	[DataMember(Name = "processID", Order = 8)]
	public string ProcessID { get; set; }

	[XmlElement(ElementName = "completionType")]
	[DataMember(Name = "completionType", Order = 9)]
	public byte CompletionType { get; set; }

	[XmlElement(ElementName = "workType")]
	[DataMember(Name = "workType", Order = 10)]
	public byte WorkType { get; set; }

	[XmlElement(ElementName = "timecardType")]
	[DataMember(Name = "timecardType", Order = 11)]
	[Required(ErrorMessage = "TimecardType is invalid or empty.")]
	public byte TimecardType { get; set; }

	[XmlElement(ElementName = "goodQuantity")]
	[DataMember(Name = "goodQuantity", Order = 12)]
	public decimal GoodQuantity { get; set; }

	[XmlElement(ElementName = "scrapQuantity")]
	[DataMember(Name = "scrapQuantity", Order = 13)]
	public decimal ScrapQuantity { get; set; }

	[XmlElement(ElementName = "reworkQuantity")]
	[DataMember(Name = "reworkQuantity", Order = 14)]
	public decimal ReworkQuantity { get; set; }

	[XmlElement(ElementName = "actualStartTime")]
	[DataMember(Name = "actualStartTime", Order = 15)]
	public DateTime? ActualStartTime { get; set; }

	[XmlElement(ElementName = "actualEndTime")]
	[DataMember(Name = "actualEndTime", Order = 16)]
	public DateTime? ActualEndTime { get; set; }

	[XmlElement(ElementName = "machineHours")]
	[DataMember(Name = "machineHours", Order = 17)]
	public decimal MachineHours { get; set; }

	[XmlElement(ElementName = "laborHours")]
	[DataMember(Name = "laborHours", Order = 18)]
	public decimal LaborHours { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 19)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 20)]
	public byte[] RowVersion { get; set; }
}
