using System.Collections.Generic;
using Godot.Collections;

namespace Proj7DRL.scripts;

public struct AbilityDef
{
    public int Id;
    public string DisplayName;
    public string Description;
    public string IconPath;
    public string Type;
    public Dictionary Args;
}