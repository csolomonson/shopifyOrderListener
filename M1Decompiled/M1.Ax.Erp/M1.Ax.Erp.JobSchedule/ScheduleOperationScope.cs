namespace M1.Ax.Erp.JobSchedule;

public enum ScheduleOperationScope : byte
{
	PreviousOperationsThisAssembly = 4,
	SubsequentOperationsThisAssembly = 2,
	CurrentOperation = 1
}
