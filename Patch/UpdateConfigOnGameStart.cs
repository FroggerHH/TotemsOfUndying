using Extensions.Valheim;
using HarmonyLib;
using UnityEngine;
using static TotemsOfUndying.Plugin;

namespace TotemsOfUndying.Patch;

[HarmonyPatch]
public class UpdateConfigOnGameStart
{
    [HarmonyPatch(typeof(Game), nameof(Game.Start))]
    [HarmonyPrefix]
    private static void UpdateConfig() => _self.UpdateConfiguration();
}