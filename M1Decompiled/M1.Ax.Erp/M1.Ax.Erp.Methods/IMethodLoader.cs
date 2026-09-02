using M1.Core;

namespace M1.Ax.Erp.Methods;

public interface IMethodLoader
{
	Assembly Load(M1Database database, object[] keyValues, int assemblyID);
}
