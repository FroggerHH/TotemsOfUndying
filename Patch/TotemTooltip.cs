using System;
using System.Linq;
using System.Text;
using Extensions;
using Extensions.Valheim;
using HarmonyLib;
using UnityEngine;
using static TotemsOfUndying.Plugin;
using static ItemDrop;
using static ItemDrop.ItemData;

namespace TotemsOfUndying.Patch;

[HarmonyPatch]
public class TotemTooltip
{
    [HarmonyPatch(typeof(ItemData), nameof(ItemData.GetTooltip), new Type[0])]
    [HarmonyPostfix, HarmonyWrapSafe]
    private static void GetTotemTooltip(ItemData __instance, ref string __result)
    {
        var totem = GetTotem(__instance.m_shared.m_name);
        if (totem == null) return;
        var sb = new StringBuilder();

        string tooltipSe = totem.GetSE().GetTooltipString();
        var bestBiome = totem.config.bestBiome.GetLocalizationKey().Localize();
        var badBiome = totem.config.badBiome.GetLocalizationKey().Localize();

        sb.AppendLine($"{totem.config.healthRightBiome} {"$healthAfterBiome".Localize()} {bestBiome}");
        sb.AppendLine($"{totem.config.staminaRightBiome} {"$staminaAfterBiome".Localize()} {bestBiome}");
        sb.AppendLine($"{"$dontWorkInBiome".Localize()} {badBiome}");
        sb.AppendLine($"{"$lessEffectInBiomes".Localize()} "
                      + $"{Math.Round(1 / totem.config.additionalBiomeStatsModifier, 2)} {"$timesInBiomes".Localize()}"
                      + $"{totem.config.aditionalBiomes.Select(x => x.GetLocalizationKey().Localize()).GetString()}");


        sb.AppendLine(tooltipSe);
        __result += sb.ToString();
    }
}