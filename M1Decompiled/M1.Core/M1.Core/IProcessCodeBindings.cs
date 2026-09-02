using System.Text;

namespace M1.Core;

public interface IProcessCodeBindings
{
	void ProcessCodeBindings(string eventName, StringBuilder code);
}
