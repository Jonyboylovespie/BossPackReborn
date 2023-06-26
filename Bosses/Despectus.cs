using BossIntegration;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using System.Collections.Generic;

namespace BossPackReborn.Bosses;

internal class Despectus : ModBoss
{
    public override Dictionary<int, BossRoundInfo> RoundsInfo => new()
    {
        [40] = new()
        {
            tier = 1,
            skullCount = 1,
            skullDescription = "Spawns round 49",
            interval = 3,
            timerDescription = "Spawns 10 Yellow Camo"
        },
        [60] = new()
        {
            tier = 2,
            skullCount = 1,
            skullDescription = "Spawns round 70",
            interval = 5,
            timerDescription = "Spawns 10 Lead Fortified"
        },
        [80] = new()
        {
            tier = 3,
            skullCount = 2,
            skullDescription = "Spawns round 84",
            interval = 10,
            timerDescription = "Spawns 3 DDT"
        },
        [100] = new()
        {
            tier = 4,
            skullCount = 2,
            skullDescription = "Spawns round 95",
            interval = 15,
            timerDescription = "Spawns a Mini Lych T3"
        },
        [120] = new()
        {
            tier = 5,
            skullCount = 3,
            skullDescription = "Spawns round 99",
            interval = 30,
            timerDescription = "Spawns a Mini Lych T5 Elite"
        },
    };

    public override string Icon => "Despectus-Icon";
    public override string Description => "\"I might be weak, but I can always call for backup !\"";
    public override string? ExtraCredits => "Art by Tent2";

    public override void ModifyBaseBloonModel(BloonModel bloonModel)
    {
        base.ModifyBaseBloonModel(bloonModel);
        bloonModel.danger = 1;
        bloonModel.speed = 6;
    }

    public override BloonModel ModifyForRound(BloonModel bloon, int round)
    {
        if (RoundsInfo.TryGetValue(round, out var roundInfo))
        {
            switch (roundInfo.tier)
            {
                case 1:
                    bloon.maxHealth = 22_500;
                    bloon.isCamo = true;
                    break;
                case 2:
                    bloon.maxHealth = 45_000;
                    bloon.bloonProperties |= Il2Cpp.BloonProperties.Lead;
                    break;
                case 3:
                    bloon.maxHealth = 90_000;
                    bloon.bloonProperties |= Il2Cpp.BloonProperties.Lead;
                    break;
                case 4:
                    bloon.maxHealth = 180_000;
                    bloon.bloonProperties |= Il2Cpp.BloonProperties.Purple;
                    break;
                case 5:
                    bloon.maxHealth = 360_000;
                    bloon.bloonProperties |= Il2Cpp.BloonProperties.Purple;
                    break;
                default:
                    break;
            }
        }

        return base.ModifyForRound(bloon, round);
    }

    public override void SkullEffect(Bloon boss)
    {
        uint? tier = ModBoss.GetTier(boss);

        switch (tier)
        {
            case 1:
                InGame.instance.SpawnBloons(49);
                break;
            case 2:
                InGame.instance.SpawnBloons(70);
                break;
            case 3:
                InGame.instance.SpawnBloons(84);
                break;
            case 4:
                InGame.instance.SpawnBloons(95);
                break;
            case 5:
                InGame.instance.SpawnBloons(99);
                break;
            default:
                break;
        }

        
    }

    public override void TimerTick(Bloon boss)
    {
        uint? tier = ModBoss.GetTier(boss);

        switch (tier)
        {
            case 1:
                InGame.instance.SpawnBloons("YellowCamo", 10, 1);
                break;
            case 2:
                InGame.instance.SpawnBloons("LeadFortified", 10, 5);
                break;
            case 3:
                InGame.instance.SpawnBloons("Ddt", 3, 10);
                break;
            case 4:
                InGame.instance.SpawnBloons("MiniLych3", 1, 15);
                break;
            case 5:
                InGame.instance.SpawnBloons("MiniLychElite5", 1, 20);
                break;
            default:
                break;
        }
    }

    public override void OnSpawn(Bloon bloon)
    {
        base.OnSpawn(bloon);
        bloon.trackScale = 3;
    }

    public class DespectusDisplay : ModBloonDisplay<Despectus>
    {
        public override string BaseDisplay => ModDisplay.Generic2dDisplay;
        public DespectusDisplay() { }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "Despectus");
        }
    }
}
