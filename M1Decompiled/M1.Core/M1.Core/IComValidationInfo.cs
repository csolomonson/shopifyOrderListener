using System.Runtime.InteropServices;

namespace M1.Core;

[ComVisible(true)]
public interface IComValidationInfo
{
	void Clear();

	void AddError(string errorText);

	void AddWarning(string errorText);

	void AddMessage(string errorText);

	string GetRowDescription();
}
