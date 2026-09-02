using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Job;

[Serializable]
[DataContract(Namespace = "", Name = "timecard")]
[XmlRoot(ElementName = "timecard")]
[XmlType(AnonymousType = true)]
public class BOMTimecardDto
{
	[XmlElement(ElementName = "timecardID")]
	[DataMember(Name = "timecardID", Order = 1)]
	[Required(ErrorMessage = "TimecardID is invalid or empty.")]
	public int TimecardID { get; set; }

	[XmlElement(ElementName = "employeeID")]
	[DataMember(Name = "employeeID", Order = 2)]
	[Required(ErrorMessage = "EmployeeID is invalid or empty.")]
	public string EmployeeID { get; set; }

	[XmlElement(ElementName = "shiftID")]
	[DataMember(Name = "shiftID", Order = 3)]
	[Required(ErrorMessage = "ShiftID is invalid or empty.")]
	public short ShiftID { get; set; }

	[XmlElement(ElementName = "timecardDate")]
	[DataMember(Name = "timecardDate", Order = 4)]
	[Required(ErrorMessage = "TimecardDate is invalid or empty.")]
	public DateTime? TimecardDate { get; set; }

	[XmlElement(ElementName = "actualStartTime")]
	[DataMember(Name = "actualStartTime", Order = 5)]
	public DateTime? ActualStartTime { get; set; }

	[XmlElement(ElementName = "actualEndTime")]
	[DataMember(Name = "actualEndTime", Order = 6)]
	public DateTime? ActualEndTime { get; set; }

	[XmlElement(ElementName = "lastEndTime")]
	[DataMember(Name = "lastEndTime", Order = 7)]
	public DateTime? LastEndTime { get; set; }

	[XmlElement(ElementName = "plantID")]
	[DataMember(Name = "plantID", Order = 8)]
	public string PlantID { get; set; }

	[XmlElement(ElementName = "plantDepartmentID")]
	[DataMember(Name = "plantDepartmentID", Order = 9)]
	public string PlantDepartmentID { get; set; }

	[XmlElement(ElementName = "postedDate")]
	[DataMember(Name = "postedDate", Order = 10)]
	public DateTime? PostedDate { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 11)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 12)]
	public byte[] RowVersion { get; set; }
}
