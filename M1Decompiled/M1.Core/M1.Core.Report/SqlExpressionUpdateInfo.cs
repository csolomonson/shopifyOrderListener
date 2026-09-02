using System;

namespace M1.Core.Report;

public class SqlExpressionUpdateInfo : IDisposable
{
	public object FormulaFieldController;

	public int FormulaIndex;

	public object FormulaField;

	public string FormulaText;

	public SqlExpressionUpdateInfo(object formulaFieldController, int formulaIndex, object formulaField, string formulaText)
	{
		FormulaFieldController = formulaFieldController;
		FormulaIndex = formulaIndex;
		FormulaField = formulaField;
		FormulaText = formulaText;
	}

	public void Dispose()
	{
		FormulaFieldController = null;
		FormulaField = null;
	}
}
