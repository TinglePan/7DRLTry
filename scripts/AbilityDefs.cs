using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace Proj7DRL.scripts;

public static class AbilityDefs
{
    private static System.Collections.Generic.Dictionary<int, AbilityDef> _defs = new()
    {
        {
            0, new AbilityDef
            {
                Id = 0,
                DisplayName = "Test shoot ability",
                Description = "Test shoot ability description",
                IconPath = "res://images/ability_bullet_shooter_front.png",
                Type = "ShootBulletAbility",
                Args = new Dictionary()
                {
                    {"power", "1"},
                    {"cooldown", "1"},
                    {"directions", new Array {(int)FlagConstants.Direction.Up}}
                }
            }
        },
        {
            1, new AbilityDef()
            {
                Id = 1,
                DisplayName = "Test laser ability",
                Description = "Test laser ability description",
                IconPath = "res://images/ability_laser_shooter_front.png",
                Type = "ShootLaserAbility",
                Args = new Dictionary()
                {
                    {"power", "2" },
                    {"dotPower", "1" },
                    {"cooldown", "4" },
                    {"duration", "2" },
                    {"directions", new Array {(int)FlagConstants.Direction.Up}}
                }
            }
        }
    };
        
    public static AbilityDef GetDef(int id)
    {
        return _defs.GetValueOrDefault(id);
    }
}