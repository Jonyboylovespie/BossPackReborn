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

namespace BossPackReborn.Bosses;

internal class Motus : ModBoss
{
    public override string Icon => "Motus-Icon";
    public override Dictionary<int, BossRoundInfo> RoundsInfo => new Dictionary<int, BossRoundInfo>()
    {
        [40] = new BossRoundInfo()
        {
            skullCount = 3,
            tier = 1,
        },
        [60] = new BossRoundInfo()
        {
            skullCount = 1,
            tier = 2,
        },
        [80] = new BossRoundInfo()
        {
            skullCount = 3,
            tier = 3,
        },
        [100] = new BossRoundInfo()
        {
            skullCount = 5,
            tier = 4,
        },
        [120] = new BossRoundInfo()
        {
            skullCount = 7,
            tier = 5,
        },
    };

    public override string Description => "Balance and chaos go hand in hand.";
    public override string? ExtraCredits => "Art by The Evolution Map";
    public override uint? Interval => 3;

    public override void ModifyBaseBloonModel(BloonModel bloonModel)
    {
        base.ModifyBaseBloonModel(bloonModel);
        bloonModel.bloonProperties = Il2Cpp.BloonProperties.Frozen;
        bloonModel.bloonProperties |= Il2Cpp.BloonProperties.Black;
    }

    public override BloonModel ModifyForRound(BloonModel bloon, int round)
    {
        if (RoundsInfo.TryGetValue(round, out var roundInfo))
        {
            switch (roundInfo.tier)
            {
                case 1:
                    bloon.maxHealth = 120_000;
                    bloon.speed = 1.5f;
                    break;
                case 2:
                    bloon.maxHealth = 560_000;
                    bloon.speed = 2f;
                    break;
                case 3:
                    bloon.maxHealth = 1_750_000;
                    bloon.speed = 2.5f;
                    break;
                case 4:
                    bloon.maxHealth = 10_000_000;
                    bloon.speed = 3f;
                    break;
                case 5:
                    bloon.maxHealth = 30_000_000;
                    bloon.speed = 3.5f;
                    break;
                default:
                    break;
            }
        }
        return bloon;
    }

    public override string SkullDescription => "Switch two towers' position.";
    public override void SkullEffect(Bloon boss)
    {
        List<Tower> towers = InGame.instance.GetTowers();

        if (towers.Count < 2)
        {
            return; // Penality
        }

        do
        {
            Tower rdm1 = towers[BossPack.rng.Next(0, towers.Count)];
            towers.Remove(rdm1);

            Tower rdm2 = towers[BossPack.rng.Next(0, towers.Count)];
            towers.Remove(rdm2);

            Il2CppAssets.Scripts.Simulation.SMath.Vector2 rdm1Pos = rdm1.Position.ToVector2();
            rdm1.PositionTower(rdm2.Position.ToVector2());
            rdm2.PositionTower(rdm1Pos);

        } while (towers.Count > 2);
        
    }

    public override string TimerDescription => "Moves around a random tower.";
    public override void TimerTick(Bloon boss)
    {
        List<Tower> towers = InGame.instance.GetTowers();

        if (towers.Count < 1)
            return; // Penality

        uint? tier = ModBoss.GetTier(boss);
        float amount = 1f;

        if (tier != null)
            amount = (float)tier * 5;

        float angle = BossPack.rng.Next(0, 360) * Math.PI / 180;

        towers[BossPack.rng.Next(0, towers.Count)].MoveTower(new Il2CppAssets.Scripts.Simulation.SMath.Vector2(Math.Cos(angle) * amount, Math.Sin(angle) * amount));
    }

    public override IEnumerable<string> DamageStates => new string[] { };
    public override void OnSpawn(Bloon bloon)
    {
        base.OnSpawn(bloon);
        bloon.trackScale = 2;
    }
    public class MotusDisplay : ModBloonDisplay<Motus>
    {
        public override string BaseDisplay => ModDisplay.Generic2dDisplay;
        public MotusDisplay() { }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "Motus");
        }
    }
}
