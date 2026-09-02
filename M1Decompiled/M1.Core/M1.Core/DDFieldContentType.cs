namespace M1.Core;

public enum DDFieldContentType : short
{
	None = 1,
	Field = 2,
	Table = 4,
	ObjectID = 8,
	GridID = 0x10,
	Expression = 0x20,
	Code = 0x40,
	Filter = 0x80,
	AppExtensionID = 0x100
}
