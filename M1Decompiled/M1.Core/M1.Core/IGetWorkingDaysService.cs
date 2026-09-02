namespace M1.Core;

public interface IGetWorkingDaysService
{
	IGetWorkingDays GetWorkingDaysService(M1Database database, string plantID);
}
