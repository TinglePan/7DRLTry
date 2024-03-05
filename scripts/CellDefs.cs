using System.Collections.Generic;

namespace Proj7DRL.scripts;

public class CellDefs
{

    private static Dictionary<int, CellDef> _cellDefs = new ()
    {
        {0, new CellDef
        {
            Id = 0,
            DisplayName = "Test",
            Description = "Test description",
            IconPath = "res://images/cell_energy.png"
        }}
    }; 
    
    public static CellDef GetCellDef(int id)
    {
        return _cellDefs.GetValueOrDefault(id);
    }
}