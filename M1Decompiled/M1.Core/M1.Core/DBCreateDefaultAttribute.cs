using System;

namespace M1.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class DBCreateDefaultAttribute : Attribute
{
	private readonly string _Description;

	public string Description => _Description;

	public DBCreateDefaultAttribute(string description)
	{
		_Description = description;
	}
}
