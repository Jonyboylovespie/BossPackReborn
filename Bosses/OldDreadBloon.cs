using BossIntegration;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Unity.Display;
using System.Collections.Generic;

namespace BossPackReborn.Bosses;

internal class OldDreadBloon : ModBoss
{
    public override string DisplayName => "Old DreadBloon";
    public override string Icon => "OldDreadBloon-Icon";
    public override string ExtraCredits => "Ninja Kiwi";
    public override string Description => "";
    public override Dictionary<int, BossRoundInfo> RoundsInfo => new Dictionary<int, BossRoundInfo>()
    {
        
    };

    public Dictionary<ObjectId, TowerSet[]> Weaknesses = new();

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

    }

    public override void OnBossesRemoved()
    {
        Weaknesses = new();
    }

    public override void OnSpawn(Bloon bloon)
    {
        base.OnSpawn(bloon);
        bloon.trackScale = 3;
        Weaknesses.Add(bloon.Id, new TowerSet[] { TowerSet.Military });
    }

    public override ModHelperPanel? AddBossPanel(ModHelperPanel holderPanel, Bloon boss, ref BossUI ui)
    {
        ModHelperPanel? panel = base.AddBossPanel(holderPanel, boss, ref ui);

        if(panel != null)
        {
            ModHelperPanel weaknessPanel = panel.AddPanel(new Info("WeaknessPanel", 300, -200, 1500, 150), VanillaSprites.BrownInsertPanel);
            weaknessPanel.AddText(new Info("", 150), "TEXT");
        }
        return panel;
    }

    public class OldDreadBloonDisplay : ModBloonDisplay<OldDreadBloon>
    {
        public override string BaseDisplay => ModDisplay.Generic2dDisplay;

        public OldDreadBloonDisplay() { }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "OldDreadBloon");
        }
    }
}
