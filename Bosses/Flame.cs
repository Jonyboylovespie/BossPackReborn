using BossIntegration;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using System.Collections.Generic;
using UnityEngine;

namespace BossPackReborn.Bosses;

internal class Flame : ModBoss
{
    public override string DisplayName => "Flame of Terror";
    public override string Icon => "Flame-Icon";
    public override string ExtraCredits => "Art by HotAirBalloon";
    public override string Description => "\"Eats cash faster then a fire !\"";
    public override Dictionary<int, BossRoundInfo> RoundsInfo => new Dictionary<int, BossRoundInfo>()
    {
        [40] = new BossRoundInfo()
        {
            skullCount = 1,
            interval = 30,
            tier = 1,
            timerDescription = "Spawns 30 Pinks",
        },
        [60] = new BossRoundInfo()
        {
            skullCount = 2,
            interval = 30,
            tier = 2,
            timerDescription = "Spawns 25 Camo Leads and all the previous timer effect",
        },
        [80] = new BossRoundInfo()
        {
            skullCount = 3,
            interval = 30,
            tier = 3,
            timerDescription = "Spawns 20 Ceramics and all the previous timer effect",
        },
        [100] = new BossRoundInfo()
        {
            skullCount = 5,
            interval = 30,
            tier = 4,
            timerDescription = "Spawns 15 MOABs and all the previous timer effect",
        },
        [120] = new BossRoundInfo()
        {
            skullCount = 7,
            interval = 30,
            tier = 5,
            timerDescription = "Spawns 10 fortified ZOMGs and all the previous timer effect",
        },
    };

    public override void ModifyBaseBloonModel(BloonModel bloonModel)
    {
        base.ModifyBaseBloonModel(bloonModel);
        bloonModel.bloonProperties |= Il2Cpp.BloonProperties.Purple;
        bloonModel.bloonProperties |= Il2Cpp.BloonProperties.Black;
        bloonModel.speed = 6.5f;
    }

    public override BloonModel ModifyForRound(BloonModel bloon, int round)
    {
        if (RoundsInfo.TryGetValue(round, out var roundInfo))
        {
            switch (roundInfo.tier)
            {
                case 1:
                    bloon.maxHealth = 70_000;
                    break;
                case 2:
                    bloon.maxHealth = 130_000;
                    break;
                case 3:
                    bloon.maxHealth = 350_000;
                    break;
                case 4:
                    bloon.maxHealth = 800_000;
                    break;
                case 5:
                    bloon.maxHealth = 1_250_000;
                    break;
                default:
                    break;
            }
        }
        return bloon;
    }

    public override string SkullDescription => "Removes 40% of your cash. If you don't have at least 4K when the skull is triggered, the boss will sell the most expensive tower.";
    public override void SkullEffect(Bloon boss)
    {
        double cash = InGame.instance.bridge.GetCash();

        if (cash < 4_000)
        {
            Tower? currentTower = default;
            double highestCash = double.MinValue;

            List<Tower> towers = InGame.instance.GetTowers();

            for (int i = 0; i < towers.Count; i++)
            {
                if (towers[i].towerModel.cost > highestCash)
                {
                    currentTower = towers[i];
                    highestCash = towers[i].towerModel.cost;
                }
            }

            if (currentTower != null)
            {
                currentTower.SellTower();
            }
        }
        else
        {
            InGame.instance.bridge.SetCash(cash * 0.6);
        }
        
    }

    public override void TimerTick(Bloon boss)
    {
        uint? tier = ModBoss.GetTier(boss);

        switch (tier)
        {
            case 1:
                InGame.instance.SpawnBloons("Pink", 30, 10);
                break;
            case 2:
                InGame.instance.SpawnBloons("Pink", 30, 10);
                InGame.instance.SpawnBloons("LeadCamo", 25, 15);
                break;
            case 3:
                InGame.instance.SpawnBloons("Pink", 30, 10);
                InGame.instance.SpawnBloons("LeadCamo", 25, 15);
                InGame.instance.SpawnBloons("Ceramic", 20, 20);
                break;
            case 4:
                InGame.instance.SpawnBloons("Pink", 30, 10);
                InGame.instance.SpawnBloons("LeadCamo", 25, 15);
                InGame.instance.SpawnBloons("Ceramic", 20, 20);
                InGame.instance.SpawnBloons("Moab", 15, 25);
                break;
            case 5:
                InGame.instance.SpawnBloons("Pink", 30, 10);
                InGame.instance.SpawnBloons("LeadCamo", 25, 15);
                InGame.instance.SpawnBloons("Ceramic", 20, 20);
                InGame.instance.SpawnBloons("Moab", 15, 25);
                InGame.instance.SpawnBloons("ZomgFortified", 10, 30);
                break;
            default:
                break;
        }
    }

    public override IEnumerable<string> DamageStates => new string[] { };
    public override void OnSpawn(Bloon bloon)
    {
        base.OnSpawn(bloon);
        bloon.trackScale = 2;
    }
    public class FlameDisplay : ModBloonDisplay<Flame>
    {
        public override string BaseDisplay => ModDisplay.Generic2dDisplay;
        public FlameDisplay() { }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "Flame");
        }
    }
}
