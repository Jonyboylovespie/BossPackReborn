using BossPackReborn;
using BTD_Mod_Helper;
using MelonLoader;
using System;

[assembly: MelonInfo(typeof(BossPackReborn.BossPack), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BossPackReborn;

public class BossPack : BloonsTD6Mod
{
    public static Random rng = new Random();
}