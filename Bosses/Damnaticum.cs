using BossIntegration;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Unity.Display;
using System.Collections.Generic;

namespace BossPackReborn.Bosses;

internal class Damnaticum : ModBoss
{
    public override Dictionary<int, BossRoundInfo> RoundsInfo => new()
    {
        [40] = new()
        {
            tier = 1,
        },
        [60] = new()
        {
            tier = 2,
        },
        [80] = new()
        {
            tier = 3,
        },
        [100] = new()
        {
            tier = 4
        },
        [120] = new()
        {
            tier = 5
        },
    };

    public override string Icon => "Damnaticum-Icon";
    public override string Description => " \"Play with THIS bloon and you will get burned !\"";

    public override bool AlwaysDefeatOnLeak => false;

    public override void ModifyBaseBloonModel(BloonModel bloonModel)
    {
        base.ModifyBaseBloonModel(bloonModel);
        bloonModel.maxHealth = 1;
        bloonModel.speed = 9;
    }

    public override void OnLeak(Bloon bloon)
    {
        bloon.bloonModel.leakDamage = bloon.health;

        base.OnLeak(bloon);
    }

    public override void OnSpawn(Bloon bloon)
    {
        base.OnSpawn(bloon);
        bloon.trackScale = 3;
    }
    public class DamnaticumDisplay : ModBloonDisplay<Damnaticum>
    {
        public override string BaseDisplay => ModDisplay.Generic2dDisplay;
        public DamnaticumDisplay() { }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "Damnaticum");
        }
    }
}
