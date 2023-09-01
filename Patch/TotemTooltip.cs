using System;
using System.Linq;
using System.Text;
using Extensions;
using Extensions.Valheim;
using HarmonyLib;
using UnityEngine;
using static System.Enum;
using static TotemsOfUndying.Plugin;
using static ItemDrop;
using static ItemDrop.ItemData;

namespace TotemsOfUndying.Patch;

[HarmonyPatch]
public class TotemTooltip
{
    [HarmonyPatch(typeof(ItemData), nameof(ItemData.GetTooltip),
        new Type[4] { typeof(ItemData), typeof(int), typeof(bool), typeof(float) })]
    [HarmonyPostfix, HarmonyWrapSafe]
    private static void GetTotemTooltip(ItemData item,
        int qualityLevel, bool crafting, float worldLevel, ref string __result)
    {
        var totem = GetTotem(item.m_shared.m_name);
        if (totem == null) return;
        var sb = new StringBuilder();
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            sb.AppendLine();
            sb.AppendLine("$holdShift".Localize());
            __result += sb.ToString();
            return;
        }

        string tooltipSe = totem.GetSE().GetTooltipString();
        var bestBiome = $"<color=orange>{(totem.config.allBiomes
            ? "$allBiomes".Localize()
            : totem.config.bestBiome.GetLocalizationKey().Localize())}</color>";
        var badBiome = totem.config.badBiome.GetLocalizationKey().Localize();

        sb.AppendLine();

        sb.AppendLine(
            $"<color=#ff8080ff>{totem.config.healthRightBiome}</color> {"$healthAfterBiome".Localize()} {bestBiome}");
        sb.AppendLine(
            $"<color=yellow>{totem.config.staminaRightBiome}</color> {"$staminaAfterBiome".Localize()} {bestBiome}");
        if (totem.config.allBiomes == false)
        {
            var lessEffect = Math.Round(1 / totem.config.additionalBiomeStatsModifier, 2);
            if (lessEffect != 1)
            {
                var lessEffectStr = $"{"$lessEffectInBiomes".Localize()} "
                                    + $"<color=yellow>{lessEffect}</color> {"$timesInBiomes".Localize()} "
                                    + $"<color=orange>"
                                    + $"{totem.config.aditionalBiomes.Where(x => x.GetLocalizationKey() != "$biome_none").Select(x => x.GetLocalizationKey()
                                            .Localize())
                                        .GetString("</color>, <color=orange>")}"
                                    + $"</color>";
                sb.AppendLine(lessEffectStr);
            }

            var wrongBiomes = TotemConfig.AllBiomesStrings.Where(x =>
                    x != "None" && x != totem.config.bestBiome.ToString() &&
                    !totem
                        .config
                        .aditionalBiomes.Select(x => x.ToString()).Contains(x))
                .Select(x => ((Heightmap.Biome)Parse(typeof(Heightmap.Biome), x)).GetLocalizationKey().Localize())
                .ToList();
            if (wrongBiomes.Count > 0)
            {
                var wrongBiomesStr = wrongBiomes.GetString("</color>, <color=orange>");
                sb.AppendLine(
                    $"<color=#ff8080ff>{totem.config.healthWrongBiome}</color> {"$healthAfterBiome".Localize()} "
                              + $"<color=orange>"
                              + $"{wrongBiomesStr}"
                              + $"</color>");
                sb.AppendLine(
                    $"<color=yellow>{totem.config.staminaWrongBiome}</color> {"$staminaAfterBiome".Localize()} "
                    + $"<color=orange>"
                    + $"{wrongBiomesStr}"
                    + $"</color>");
            }
            
            if (badBiome != "[biome_none]")
                sb.AppendLine($"{"$dontWorkInBiome".Localize()} <color=orange>{badBiome}</color> ");

            sb.AppendLine(
                $"{"$chanceToActivateBuffInAdditionalBiome".Localize()} {bestBiome} "
                + $"<color=yellow>{totem.config.chanceToActivateBufInAdditionalBiome}%</color>");
        }

        if (totem.config.buffs.Count > 0)
        {
            sb.AppendLine($"{"$buffs".Localize()}");
            sb.AppendLine(
                $"<color=orange>{totem.config.buffs.Select(x => ObjectDB.instance.GetStatusEffect(x
                    .GetStableHashCode()).m_name.Localize()).GetString("</color>, <color=orange>")}</color> ");
        }

        //sb.AppendLine();
        sb.AppendLine($"{"$bossSE_effects".Localize()} {bestBiome}");
        sb.AppendLine(tooltipSe);
        __result += sb.ToString();
    }
}