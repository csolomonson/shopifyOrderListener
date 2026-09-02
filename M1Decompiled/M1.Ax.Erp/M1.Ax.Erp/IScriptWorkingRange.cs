namespace M1.Ax.Erp;

public interface IScriptWorkingRange
{
	int GetDaysForPlant(string plantID);

	decimal GetHoursForPlant(string plantID);
}
