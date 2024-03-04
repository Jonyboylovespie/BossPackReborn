using BossIntegration;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using System.Collections.Generic;
using static Il2CppAssets.Scripts.Models.Bloons.Behaviors.StunTowersInRadiusActionModel;

namespace BossPackReborn.Bosses;

internal class OldVortex : ModBoss
{
    public override string DisplayName => "Old Vortex";
    public override string Icon => "OldVortex-Icon";
    public override string ExtraCredits => "Ninja Kiwi";
    public override string Description => "\"Air can break down stone itself. It is only a matter of rounds ...\"";
    public override Dictionary<int, BossRoundInfo> RoundsInfo => new Dictionary<int, BossRoundInfo>()
    {
        [40] = new BossRoundInfo()
        {
            tier = 1,
            skullCount = 5,
            skullDescription = "Stuns 5% of your towers for 1 round",
        },
        [60] = new BossRoundInfo()
        {
            tier = 2,
            skullCount = 4,
            skullDescription = "Stuns 25% of your towers for 2 rounds",
        },
        [80] = new BossRoundInfo()
        {
            tier = 3,
            skullCount = 3,
            skullDescription = "Stuns 50% of your towers for 3 rounds",
        },
        [100] = new BossRoundInfo()
        {
            tier = 4,
            skullCount = 3,
            defeatIfPreviousNotDefeated = true,
            skullDescription = "Stuns 75% of your towers for 4 rounds",
        },
        [120] = new BossRoundInfo()
        {
            tier = 5,
            skullCount = 3,
            defeatIfPreviousNotDefeated = true,
            skullDescription = "Stuns 95% of your towers for 1 round",
        },
    };

    public override void ModifyBaseBloonModel(BloonModel bloonModel)
    {
        base.ModifyBaseBloonModel(bloonModel);
        bloonModel.speed = 6.25f;
    }

    public override BloonModel ModifyForRound(BloonModel bloon, int round)
    {
        if (RoundsInfo.TryGetValue(round, out var roundInfo))
        {
            switch (roundInfo.tier)
            {
                case 1:
                    bloon.maxHealth = 55_000;
                    break;
                case 2:
                    bloon.maxHealth = 650_000;
                    break;
                case 3:
                    bloon.maxHealth = 4_240_240;
                    break;
                case 4:
                    bloon.maxHealth = 12_450_069;
                    break;
                case 5:
                    bloon.maxHealth = 50_222_000;
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
        float percentage = 0.05f;
        int rounds = tier != null ? (int)tier : 1;

        switch (tier)
        {
            case 2:
                percentage = 0.25f;
                break;
            case 3:
                percentage = 0.5f;
                break;
            case 4:
                percentage = 0.8f;
                break;
            case 5:
                percentage = 0.95f;
                rounds = 1;
                break;
            case 1:
            default:
                break;
        }

        List<Tower> towers = InGame.instance.GetTowers();

        int countRemaining = Math.CeilToInt(towers.Count * percentage);

        for (int i = 0; i < countRemaining; i++)
        {
            if (towers.Count == 0)
                break;

            TowerFreezeMutator stun = new TowerFreezeMutator("Freeze", new Il2CppAssets.Scripts.Utils.PrefabReference("289f511b736a06a4c993b9e0e73d2b8a"), true);
            int rdmIndex = BossPack.rng.Next(0, towers.Count);

            towers[rdmIndex].AddMutator(stun, roundsRemaining: rounds);

            towers.RemoveAt(rdmIndex);
        }
    }

    public override void OnSpawn(Bloon bloon)
    {
        base.OnSpawn(bloon);
        bloon.trackScale = 3;
    }

    public override IEnumerable<string> DamageStates => new string[] { "OldVortex-1", "OldVortex-2", "OldVortex-3" };

    public class OldVortexDisplay : ModBloonDisplay<OldVortex>
    {
        public override string BaseDisplay => ModDisplay.Generic2dDisplay;

        public OldVortexDisplay() { }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "OldVortex-0");
        }
    }
}
