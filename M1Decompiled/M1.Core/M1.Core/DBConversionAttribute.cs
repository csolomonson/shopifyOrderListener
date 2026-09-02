using System;

namespace M1.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class DBConversionAttribute : Attribute
{
	private readonly string _Version;

	private readonly string _Description;

	private readonly string _Date;

	public string Version => _Version;

	public string Description => _Description;

	public string Date => _Date;

	public DBConversionAttribute(string version, string description, string date)
	{
		_Version = version;
		_Description = description;
		_Date = date;
	}
}
