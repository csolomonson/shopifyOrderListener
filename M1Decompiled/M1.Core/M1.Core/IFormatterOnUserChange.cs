using System.Data;

namespace M1.Core;

public interface IFormatterOnUserChange
{
	void OnUserChange(FieldDefinition field, DataRow row);
}
