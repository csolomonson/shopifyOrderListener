namespace M1.Ax.Erp;

internal static class ReductionRate
{
	internal static double GetReductionRate(string state)
	{
		switch (state.ToUpper())
		{
		case "AR":
		case "CA":
		case "CT":
		case "GA":
		case "KY":
		case "MO":
		case "NC":
		case "NY":
		case "OH":
		case "RI":
		case "WI":
			return 0.009;
		case "DE":
			return 0.006;
		case "IN":
		case "VI":
			return 0.012;
		default:
			return 0.0;
		}
	}
}
