using BossIntegration;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using System.Collections.Generic;
using UnityEngine;

namespace BossPackReborn.Bosses;

internal class ETDB : ModBoss
{
    public override string DisplayName => "Bloontonium Expert";
    public override string Icon => "Etdb-Icon";
    public override string ExtraCredits => "Art by Niki7102";
    public override string Description => "After years of research, bloons found a way to extract the core energy of the bloons into a single one !";
    public override Dictionary<int, BossRoundInfo> RoundsInfo => new Dictionary<int, BossRoundInfo>()
    {
        [40] = new BossRoundInfo()
        {
            skullCount = 3,
            interval = 1,
            tier = 1,
            skullDescription = "Spawns 3 MOAB",
        },
        [60] = new BossRoundInfo()
        {
            skullCount = 3,
            interval = 1,
            tier = 2,
            skullDescription = "Spawns a BFB",
        },
        [80] = new BossRoundInfo()
        {
            skullCount = 4,
            interval = 1,
            tier = 3,
            skullDescription = "Spawns 2 DDT",
        },
        [100] = new BossRoundInfo()
        {
            skullCount = 5,
            interval = 1,
            tier = 4,
            skullDescription = "Spawns 2 ZOMG",
        },
        [120] = new BossRoundInfo()
        {
            skullCount = 7,
            interval = 1,
            tier = 5,
            skullDescription = "Spawns a BAD",
        },
    };

    public override void ModifyBaseBloonModel(BloonModel bloonModel)
    {
        base.ModifyBaseBloonModel(bloonModel);
        bloonModel.bloonProperties |= Il2Cpp.BloonProperties.Lead;
    }

    public override BloonModel ModifyForRound(BloonModel bloon, int round)
    {
        if (RoundsInfo.TryGetValue(round, out var roundInfo))
        {
            switch (roundInfo.tier)
            {
                case 1:
                    bloon.maxHealth = 30_000;
                    bloon.speed = 4.5f;
                    break;
                case 2:
                    bloon.maxHealth = 128_000;
                    bloon.speed = 3.93f;
                    break;
                case 3:
                    bloon.maxHealth = 456_000;
                    bloon.speed = 3.375f;
                    break;
                case 4:
                    bloon.maxHealth = 768_000;
                    bloon.speed = 2.75f;
                    break;
                case 5:
                    bloon.maxHealth = 2_304_000;
                    bloon.speed = 2.25f;
                    break;
                default:
                    break;
            }
        }
        return bloon;
    }

    public override void SkullEffect(Bloon boss)
    {
        uint? tier = ModBoss.GetTier(boss);

        switch (tier)
        {
            case 1:
                InGame.instance.SpawnBloons("Moab", 3, 50);
                break;
            case 2:
                InGame.instance.SpawnBloons("Bfb", 1, 0);
                break;
            case 3:
                InGame.instance.SpawnBloons("Ddt", 2, 2000);
                break;
            case 4:
                InGame.instance.SpawnBloons("Zomg", 2, 100);
                break;
            case 5:
                InGame.instance.SpawnBloons("Bad", 1, 0);
                break;
            default:
                break;
        }
        
    }

    public override string TimerDescription => "Regenerates 0.75% of its max health.";

    const float gain = 0.0075f;
    public override void TimerTick(Bloon boss)
    {
        boss.health = boss.health + boss.bloonModel.maxHealth * gain > boss.bloonModel.maxHealth ?
            boss.bloonModel.maxHealth :
            boss.health + boss.bloonModel.maxHealth * gain;
    }

    public override IEnumerable<string> DamageStates => new string[] { };
    public override void OnSpawn(Bloon bloon)
    {
        base.OnSpawn(bloon);
        bloon.trackScale = 3;
    }
    public class EtdbDisplay : ModBloonDisplay<ETDB>
    {
        public override string BaseDisplay => ModDisplay.Generic2dDisplay;
        public EtdbDisplay() { }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "Etdb");
        }
    }
}
