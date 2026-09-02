namespace M1.Core;

public sealed class QueryParseError
{
	private int _Column;

	private int _Line;

	private string _Message = string.Empty;

	private int _Number;

	private int _Offset;

	public int Column => _Column;

	public int Line => _Line;

	public string Message => _Message;

	public int Number => _Number;

	public int Offset => _Offset;

	public QueryParseError(int number, int offset, int line, int column, string message)
	{
		_Number = number;
		_Offset = offset;
		_Line = line;
		_Column = column;
		_Message = message;
	}
}
