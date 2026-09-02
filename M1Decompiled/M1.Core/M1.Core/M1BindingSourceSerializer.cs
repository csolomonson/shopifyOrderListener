using System.CodeDom;
using System.ComponentModel.Design.Serialization;
using M1.Extensions;

namespace M1.Core;

public class M1BindingSourceSerializer : M1PassContainerInConstructorSerializer
{
	public override object Serialize(IDesignerSerializationManager manager, object value)
	{
		object obj = base.Serialize(manager, value);
		if (obj is CodeStatementCollection)
		{
			foreach (object item in (CodeStatementCollection)obj)
			{
				if (!(item is CodeExpressionStatement))
				{
					continue;
				}
				CodeExpressionStatement codeExpressionStatement = (CodeExpressionStatement)item;
				if (codeExpressionStatement.Expression is CodeMethodInvokeExpression)
				{
					CodeMethodInvokeExpression codeMethodInvokeExpression = (CodeMethodInvokeExpression)codeExpressionStatement.Expression;
					if (codeMethodInvokeExpression.Method.MethodName.Equals("Add") && codeMethodInvokeExpression.Method.TargetObject is CodePropertyReferenceExpression && ((CodePropertyReferenceExpression)codeMethodInvokeExpression.Method.TargetObject).PropertyName.Equals("DataBindings"))
					{
						codeExpressionStatement.UserData["statement-ordering"] = "end";
					}
				}
			}
		}
		return obj;
	}
}
