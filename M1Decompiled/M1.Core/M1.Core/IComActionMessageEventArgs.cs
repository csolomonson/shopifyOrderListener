namespace M1.Core;

public interface IComActionMessageEventArgs
{
	string MessageID { get; set; }

	object ParametersLength { get; }

	object ParametersExLength { get; }

	object Parameters(object index);

	object ParametersEx(object index);

	object GetParametersAsArray();

	object GetParametersExAsArray();
}
