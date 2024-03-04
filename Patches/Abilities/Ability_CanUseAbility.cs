using BossIntegration;
using BossPackReborn.Bosses;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossPackReborn.Patches.Abilities;

/*[HarmonyPatch(typeof(Ability), nameof(Ability.IsReady))]
internal class Ability_CanUseAbility
{
    [HarmonyPrefix]
    internal static bool Prefix(Ability __instance)
    {
        bool result = true;



        return result;
    }
}*/
