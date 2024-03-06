namespace Proj7DRL.scripts;

public interface ICellContainer
{
    public void OnAddCell(Cell cell);
    public void OnRemoveCell(Cell cell, Slot slot);
}