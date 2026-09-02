namespace M1.Ax.Erp.ChangeID;

internal class OperationParams
{
	public int AssemblyId { get; set; }

	public int OperationId { get; set; }

	public bool ShouldBeDeleted { get; set; }

	public bool ShouldBeUpdated { get; set; }
}
