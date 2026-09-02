namespace M1.Core;

public class ForeignUpdateFieldExtension : FieldExtension
{
	public override void LoadComplete(FieldCollection fields, bool add)
	{
		fields[PartBinField].AttachForeignUpdateBinding(FieldName, RelatedJobField, ReverseSign, RequiredExpression);
	}
}
