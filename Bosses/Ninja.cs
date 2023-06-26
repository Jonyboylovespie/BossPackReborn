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
using static Il2CppAssets.Scripts.Models.Bloons.Behaviors.StunTowersInRadiusActionModel;

namespace BossPackReborn.Bosses;

internal class Ninja : ModBoss
{
    public override string DisplayName => "Goemon\'s Student";
    public override string Icon => "Ninja-Icon";
    public override string ExtraCredits => "Art by thijs";
    public override string Description => "\"Sneakety smackety your defense for DDTs lackety !\"";
    public override Dictionary<int, BossRoundInfo> RoundsInfo => new Dictionary<int, BossRoundInfo>()
    {
        [40] = new BossRoundInfo()
        {
            skullCount = 2,
            interval = 30f,
            tier = 1,
            defeatIfPreviousNotDefeated = true,
            timerDescription = "Spawns 1 Camo DDT",
        },
        [60] = new BossRoundInfo()
        {
            skullCount = 2,
            interval = 26.25f,
            tier = 2,
            defeatIfPreviousNotDefeated = true,
            timerDescription = "Spawns 2 Camo DDT",
        },
        [80] = new BossRoundInfo()
        {
            skullCount = 2,
            interval = 22.5f,
            tier = 3,
            defeatIfPreviousNotDefeated = true,
            timerDescription = "Spawns 3 Camo DDT",
        },
        [100] = new BossRoundInfo()
        {
            skullCount = 2,
            interval = 18.75f,
            tier = 4,
            defeatIfPreviousNotDefeated = true,
            timerDescription = "Spawns 4 Camo DDT",
        },
        [120] = new BossRoundInfo()
        {
            skullCount = 2,
            interval = 15f,
            tier = 5,
            defeatIfPreviousNotDefeated = true,
            timerDescription = "Spawns 5 Camo DDT",
        },
    };

    public override void ModifyBaseBloonModel(BloonModel bloonModel)
    {
        base.ModifyBaseBloonModel(bloonModel);
        bloonModel.bloonProperties &= Il2Cpp.BloonProperties.Black;
        bloonModel.bloonProperties &= Il2Cpp.BloonProperties.White;
        bloonModel.isCamo = true;
        bloonModel.speed = 3;
    }

    public override BloonModel ModifyForRound(BloonModel bloon, int round)
    {
        if (RoundsInfo.TryGetValue(round, out var roundInfo))
        {
            switch (roundInfo.tier)
            {
                case 1:
                    bloon.maxHealth = 18_000;
                    break;
                case 2:
                    bloon.maxHealth = 56_200;
                    break;
                case 3:
                    bloon.maxHealth = 248_000;
                    break;
                case 4:
                    bloon.maxHealth = 556_800;
                    break;
                case 5:
                    bloon.maxHealth = 1_912_000;
                    break;
                default:
                    break;
            }
        }
        return bloon;
    }

    public override string SkullDescription => "Stuns for 2 min (1 min for paragons) the most expensive tower that isn't a farm, a village and isn't already stunned.";
    public override void SkullEffect(Bloon boss)
    {
        TowerFreezeMutator stun = new TowerFreezeMutator(new Il2CppAssets.Scripts.Utils.PrefabReference(), true);
        List<Tower> towers = InGame.instance.GetTowerManager().GetTowers().ToList();

        // If no tower
        if (towers.Count == 0)
            return;

        // Remove farms, villages and stunned towers
        for (int i = towers.Count - 1; i >= 0; i--)
            if (towers[i].towerModel.baseId == "BananaFarm" || towers[i].towerModel.baseId == "MonkeyVillage" || towers[i].IsStunned)
                towers.RemoveAt(i);

        // If no valid tower
        if (towers.Count == 0)
            return;

        float highestWorth = 0;
        int index = 0;
        for (int i = towers.Count - 1; i >= 0; i--)
        {
            if (towers[i].worth > highestWorth)
            {
                highestWorth = towers[i].worth;
                index = i;
            }

            if (towers[i].towerModel.baseId == "NinjaMonkey")
            {
                towers[i].worth = 0;
                towers[i].SellTower();
            }
        }

        if (towers[index].towerModel.isParagon)
            towers[index].AddMutator(stun, 3600);
        else
            towers[index].AddMutator(stun, 7200);

        
    }

    public override void TimerTick(Bloon boss)
    {
        int round = InGame.Bridge.GetCurrentRound() + 1;
        int tier = 1;
        foreach (var item in RoundsInfo)
        {
            if (item.Key < round)
                continue;

            if (item.Value.tier != null)
                tier = (int)item.Value.tier;
            break;
        }

        InGame.instance.SpawnBloons("DdtCamo", tier, 10 * tier);
    }

    public override IEnumerable<string> DamageStates => new string[] { };
    public override void OnSpawn(Bloon bloon)
    {
        base.OnSpawn(bloon);
        bloon.trackScale = 3;
    }
    public class NinjaDisplay : ModBloonDisplay<Ninja>
    {
        public override string BaseDisplay => Generic2dDisplay;
        public NinjaDisplay() { }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "Ninja");
        }
    }
}
