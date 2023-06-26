using BossIntegration;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.Rounds;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using System.Collections.Generic;
using System.Linq;

namespace BossPackReborn.Bosses;

internal class OldBloonarius : ModBoss
{
    public override string DisplayName => "Old Bloonarius";
    public override string Icon => "OldBloonarius-Icon";
    public override string ExtraCredits => "Ninja Kiwi";
    public override string Description => "\"Leaving things to fester always makes a bigger problem... especially in the swamp.\"";
    public override Dictionary<int, BossRoundInfo> RoundsInfo => new Dictionary<int, BossRoundInfo>()
    {
        [40] = new BossRoundInfo()
        {
            skullCount = 1,
            tier = 1,
            interval = 30,
            timerDescription = "Spawns a MOAB randomly on the map.",
        },
        [60] = new BossRoundInfo()
        {
            skullCount = 3,
            tier = 2,
            interval = 45,
            timerDescription = "Spawns a BFB randomly on the map.",
        },
        [80] = new BossRoundInfo()
        {
            skullCount = 3,
            tier = 3,
            interval = 60,
            timerDescription = "Spawns a ZOMG randomly on the map.",
        },
        [100] = new BossRoundInfo()
        {
            skullCount = 5,
            tier = 4,
            interval = 75,
            timerDescription = "Spawns a BAD randomly on the map.",
        },
        [120] = new BossRoundInfo()
        {
            skullCount = 7,
            tier = 5,
            interval = 90,
            timerDescription = "Spawns a fortified BAD randomly on the map.",
        },
    };

    public override void ModifyBaseBloonModel(BloonModel bloonModel)
    {
        base.ModifyBaseBloonModel(bloonModel);
        bloonModel.speed = 2.5f;
    }

    public override bool BlockRounds => true;

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
                    bloon.maxHealth = 30_926_000;
                    bloon.speed = 3;
                    break;
                default:
                    break;
            }
        }
        return bloon;
    }

    public override string SkullDescription => "Spawns the biggest threat of the current round where the boss is.";
    public override void SkullEffect(Bloon boss)
    {
        BloonModel highestGroup;
        int round = InGame.Bridge.GetCurrentRound();

        if (round >= 140)
        {
            highestGroup = Game.instance.model.GetBloon("BadFortified");
        }
        else
        {
            IOrderedEnumerable<BloonGroupModel> groups = InGame.instance.GetGameModel().roundSet.rounds[round].groups
                .Where(bloon => !bloon.GetBloonModel().isBoss)
                .OrderByDescending(t => t.GetBloonModel().danger);

            /*foreach (var item in groups)
            {
                TaskScheduler.ScheduleTask(() =>
                {
                    ModHelper.Msg<BossPack>(boss.Id);
                    int spacing = (int)((item.end - item.start) / item.count);
                    for (int i = 0; i < item.count; i++)
                    {
                        TaskScheduler.ScheduleTask(() =>
                        {
                            Bloon bloon = InGame.instance.GetMap().spawner.Emit(Game.instance.model.GetBloon(item.bloon), -1, -1, boss.distanceTraveled / boss.bloonModel.speed);
                            bloon.bloonModel.speedFrames = ModBoss.SpeedToSpeedFrames(bloon.bloonModel.speed);
                        }, ScheduleType.WaitForFrames, spacing * i, new System.Func<bool>(() => InGame.instance.IsInGame()));
                    }

                }, ScheduleType.WaitForFrames, (int)item.start, new System.Func<bool>(() => InGame.instance.IsInGame()));
            }*/

            highestGroup = groups.First().GetBloonModel();
        }

        Bloon bloon = InGame.instance.GetMap().spawner.Emit(highestGroup, -1, -1, boss.distanceTraveled / boss.bloonModel.speed);
        bloon.bloonModel.speedFrames = ModBoss.SpeedToSpeedFrames(bloon.bloonModel.speed);

        //SpawnBloonsAction b = new SpawnBloonsAction();
        //bloon.AddBloonBehavior(b);
        //b.Process(0);

        
    }

    public override uint? Interval => 10;
    const double range = 0.2;
    public override void TimerTick(Bloon boss)
    {
        BloonModel? bloonModel = null;
        uint? tier = ModBoss.GetTier(boss);

        switch (tier)
        {
            case 1:
                bloonModel = Game.instance.model.GetBloon("Moab");
                break;
            case 2:
                bloonModel = Game.instance.model.GetBloon("Bfb");
                break;
            case 3:
                bloonModel = Game.instance.model.GetBloon("Zomg");
                break;
            case 4:
                bloonModel = Game.instance.model.GetBloon("Bad");
                break;
            case 5:
                bloonModel = Game.instance.model.GetBloon("BadFortified");
                break;
            default:
                break;
        }

        if (bloonModel == null)
            return;

        Bloon bloon = InGame.instance.GetMap().spawner.Emit(bloonModel, -1, -1, boss.path.Length * (float)(BossPack.rng.NextDouble() * (1 - range * 2) + range));
        bloon.bloonModel.speedFrames = ModBoss.SpeedToSpeedFrames(bloon.bloonModel.speed);
    }

    public override IEnumerable<string> DamageStates => new string[] { };
    public override void OnSpawn(Bloon bloon)
    {
        base.OnSpawn(bloon);
        bloon.trackScale = 5;
    }
    public class OldBloonariusDisplay : ModBloonDisplay<OldBloonarius>
    {
        public override string BaseDisplay => ModDisplay.Generic2dDisplay;
        public OldBloonariusDisplay() { }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "OldBloonarius");
        }
    }
}
