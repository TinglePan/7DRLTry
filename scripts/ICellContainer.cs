namespace Proj7DRL.scripts;

public interface ICellContainer
{
    public void OnAddCell(Cell cell, Slot slot);
    public void OnRemoveCell(Cell cell, Slot slot);
}