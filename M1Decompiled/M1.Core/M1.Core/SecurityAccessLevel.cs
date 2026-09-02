namespace M1.Core;

public enum SecurityAccessLevel : byte
{
	Default = 0,
	None = 1,
	View = 2,
	Edit = 4,
	Add = 8,
	Delete = 0x10,
	ChangeID = 0x20
}
