using BossIntegration;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Bloons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using System.Collections.Generic;

namespace BossPackReborn.Bosses;

internal class Cever : ModBoss
{
    public override string DisplayName => "Queen of Jaws";
    public override string Icon => "Cever-Icon";
    public override string ExtraCredits => "Art by Hypno1337";
    public override string Description => "The queen heard the screams and is back for blood! After finding a metal pool in the depths and taking a quick dip, she is ready to rampage!";
    public override Dictionary<int, BossRoundInfo> RoundsInfo => new Dictionary<int, BossRoundInfo>()
    {
        [40] = new BossRoundInfo()
        {
            skullCount = 1,
            interval = 6,
            tier = 1,
        },
        [60] = new BossRoundInfo()
        {
            skullCount = 2,
            interval = 6,
            tier = 2,
        },
        [80] = new BossRoundInfo()
        {
            skullCount = 3,
            interval = 6,
            tier = 3,
        },
        [100] = new BossRoundInfo()
        {
            skullCount = 4,
            interval = 6,
            tier = 4,
        },
        [120] = new BossRoundInfo()
        {
            skullCount = 5,
            interval = 6,
            tier = 5,
        },
    };

    public override void ModifyBaseBloonModel(BloonModel bloonModel)
    {
        base.ModifyBaseBloonModel(bloonModel);
        bloonModel.bloonProperties |= Il2Cpp.BloonProperties.Lead;
        bloonModel.bloonProperties |= Il2Cpp.BloonProperties.Purple;
    }

    public override BloonModel ModifyForRound(BloonModel bloon, int round)
    {
        if (RoundsInfo.TryGetValue(round, out var roundInfo))
        {
            switch (roundInfo.tier)
            {
                case 1:
                    bloon.maxHealth = 7_500;
                    bloon.speed = 40;
                    break;
                case 2:
                    bloon.maxHealth = 15_000;
                    bloon.speed = 44;
                    break;
                case 3:
                    bloon.maxHealth = 30_000;
                    bloon.speed = 48;
                    break;
                case 4:
                    bloon.maxHealth = 60_000;
                    bloon.speed = 52;
                    break;
                case 5:
                    bloon.maxHealth = 120_000;
                    bloon.speed = 56;
                    break;
                default:
                    break;
            }
        }
        return bloon;
    }

    public override string SkullDescription => "Goes at x10 speed for 9 seconds.";
    public override void SkullEffect(Bloon boss)
    {
        boss.AddMutator(new SpeedUpMutator("CeverBash", 10f), 9);
    }

    public override string TimerDescription => "Chomps a random tower and removes a certain amount of lives depending if the tower chomped is a mechanical tower or not.";

    public override void TimerTick(Bloon boss)
    {
        if (InGame.instance.GetTowers().Count == 0)
        {
            boss.AddMutator(new SpeedUpMutator("CeverBash", 10f), 9);
            return;
        }

        Tower tower = InGame.instance.GetTowers()[BossPack.rng.Next(0, InGame.instance.GetTowers().Count)];

        if (tower != null)
        {
            bool isMechanical = false;

            if (!MechanicalTowers.ContainsKey(tower.towerModel.baseId))
            {
                InGame.instance.AddHealth(-30);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (tower.towerModel.tiers[i] >= MechanicalTowers[tower.towerModel.baseId][i])
                    {
                        isMechanical = true;
                    }
                }

                if (isMechanical)
                    InGame.instance.AddHealth(-5);
                else
                    InGame.instance.AddHealth(-30);
            }

            if (!isMechanical)
            {
                tower.worth /= 2;

                if (tower != null)
                    InGame.instance.SellTower(tower);
            }
        }
    }

    private static readonly Dictionary<string, int[]> MechanicalTowers = new Dictionary<string, int[]>()
        {
            { "DartMonkey", new int[]{ 4, 6, 6 } },
            { "BombShooter", new int[]{ 0, 0, 0 } },
            { "TackShooter", new int[]{ 0, 0, 0 } },
            { "MonkeySub", new int[]{ 0, 0, 0 } },
            { "MonkeyBuccaneer", new int[]{ 2, 4, 4 } },
            { "MonkeyAce", new int[]{ 0, 0, 0 } },
            { "HeliPilot", new int[]{ 0, 0, 0 } },
            { "DartlingGunner", new int[]{ 5, 5, 4 } },
            { "SuperMonkey", new int[]{ 3, 3, 5 } },
            { "BananaFarm", new int[]{ 3, 3, 3 } },
            { "SpikeFactory", new int[]{ 0, 0, 0 } },
            { "MonkeyVillage", new int[]{ 6, 3, 6 } }
        };

    public override IEnumerable<string> DamageStates => new string[] { };
    public override void OnSpawn(Bloon bloon)
    {
        base.OnSpawn(bloon);
        bloon.trackScale = 3;
    }
    public class CeverDisplay : ModBloonDisplay<Cever>
    {
        public override string BaseDisplay => ModDisplay.Generic2dDisplay;
        public CeverDisplay() { }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "Cever");
        }
    }
}
