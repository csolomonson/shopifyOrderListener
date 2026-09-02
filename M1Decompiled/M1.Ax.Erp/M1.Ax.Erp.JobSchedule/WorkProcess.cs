namespace M1.Ax.Erp.JobSchedule;

public class WorkProcess
{
	public string ProcessID { get; set; }

	public byte TypeID { get; set; }

	public bool IgnoreCalendarQueue { get; set; }

	public bool IgnoreCalendarMove { get; set; }

	public WorkProcess(string processID, bool ignoreQueue, bool ignoreMove, byte typeID)
	{
		ProcessID = processID;
		TypeID = typeID;
		IgnoreCalendarMove = ignoreMove;
		IgnoreCalendarQueue = ignoreQueue;
	}
}
