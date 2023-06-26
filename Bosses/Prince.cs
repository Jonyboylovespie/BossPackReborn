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

internal class Prince : ModBoss
{
    public override string DisplayName => "The Demon Prince";
    public override string Icon => "Prince-Icon";
    public override string ExtraCredits => "Art by u/fgr12455432487";
    public override string Description => "\"Life leaking leaves you lacking !\"";
    public override Dictionary<int, BossRoundInfo> RoundsInfo => new Dictionary<int, BossRoundInfo>()
    {
        [40] = new BossRoundInfo()
        {
            skullCount = 1,
            interval = 12,
            tier = 1,
            skullDescription = "Removes 50 lives",
        },
        [60] = new BossRoundInfo()
        {
            skullCount = 3,
            interval = 12,
            tier = 2,
            skullDescription = "Removes 50 lives",
        },
        [80] = new BossRoundInfo()
        {
            skullCount = 3,
            interval = 9,
            tier = 3,
            skullDescription = "Removes 100 lives",
        },
        [100] = new BossRoundInfo()
        {
            skullCount = 5,
            interval = 9,
            tier = 4,
            skullDescription = "Removes 100 lives",
        },
        [120] = new BossRoundInfo()
        {
            skullCount = 7,
            interval = 5,
            tier = 5,
            skullDescription = "Removes 150 lives",
        },
    };

    public override void ModifyBaseBloonModel(BloonModel bloonModel)
    {
        base.ModifyBaseBloonModel(bloonModel);
        bloonModel.bloonProperties = Il2Cpp.BloonProperties.Purple;
        bloonModel.isCamo = true;
    }

    public override BloonModel ModifyForRound(BloonModel bloon, int round)
    {
        if (RoundsInfo.TryGetValue(round, out var roundInfo))
        {
            switch (roundInfo.tier)
            {
                case 1:
                    bloon.maxHealth = 30_000;
                    bloon.speed = 1.5f;
                    break;
                case 2:
                    bloon.maxHealth = 100_000;
                    bloon.speed = 1.875f;
                    break;
                case 3:
                    bloon.maxHealth = 500_000;
                    bloon.speed = 2.25f;
                    break;
                case 4:
                    bloon.maxHealth = 1_000_000;
                    bloon.speed = 2.625f;
                    break;
                case 5:
                    bloon.maxHealth = 5_000_000;
                    bloon.speed = 3;
                    break;
                default:
                    break;
            }
        }
        return bloon;
    }

    public override void SkullEffect(Bloon boss)
    {
        InGame.instance.AddHealth(-50 * (InGame.instance.bridge.GetCurrentRound() + 1) / 40);
        
    }

    public override string TimerDescription => "Drains 2 lives.";

    public override void TimerTick(Bloon boss)
    {
        InGame.instance.AddHealth(-2);
    }

    public override IEnumerable<string> DamageStates => new string[] { };
    public override void OnSpawn(Bloon bloon)
    {
        base.OnSpawn(bloon);
        bloon.trackScale = 3;
    }
    public class PrinceDisplay : ModBloonDisplay<Prince>
    {
        public override string BaseDisplay => ModDisplay.Generic2dDisplay;
        public PrinceDisplay() { }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "Prince");
        }
    }
}
