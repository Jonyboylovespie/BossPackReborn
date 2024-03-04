using BossIntegration;
using BossPackReborn.Bosses;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Bloons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;

namespace BossPackReborn.Patches.Bloons;

[HarmonyPatch(typeof(Bloon), nameof(Bloon.Damage))]
internal class Bloon_Damage
{
    [HarmonyPrefix]
    internal static bool Prefix(Bloon __instance, float totalAmount, Projectile projectile,
        bool distributeToChildren, bool overrideDistributeBlocker, bool createEffect,
        Tower tower, BloonProperties immuneBloonProperties,
        bool canDestroyProjectile = true, bool ignoreNonTargetable = false,
        bool blockSpawnChildren = false, bool ignoreInvunerable = false)
    {
        bool result = true;

        if (ModBoss.BossesAlive.TryGetValue(__instance.Id, out var bossInfo))
        {
            switch (bossInfo.Boss)
            {
                case OldDreadBloon oldDreadBloon:
                    foreach (var flag in oldDreadBloon.Weaknesses[__instance.Id])
                    {
                        result = !tower.towerModel.towerSet.HasFlag(flag);

                        if (!result)
                            break;
                    }
                    break;
                case Damnaticum damnaticum:
                    __instance.bloonModel.maxHealth += (int)((bossInfo.RoundInfo.tier ?? 1) * totalAmount);
                    __instance.SetHealth(__instance.bloonModel.maxHealth);

                    result = false;
                    break;
                default:
                    break;
            }
        }
        return result;
    }
}