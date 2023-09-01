using System;
using Extensions.Valheim;
using HarmonyLib;
using static TotemsOfUndying.Plugin;
using static ItemDrop;

namespace TotemsOfUndying.Patch;

[HarmonyPatch]
public class MaxCountInInventory
{
    private static bool CheckForTotemsCore(Inventory __instance, ItemData item)
    {
        if (item == null) return true;
        var totem = GetTotem(item.m_shared.m_name);
        if (totem == null) return true;

        if (__instance.CountItems(item.m_shared.m_name) >= totem.config.maxCountInInventory)
            return false;
        return true;
    }

    [HarmonyPatch(typeof(Inventory), nameof(Inventory.AddItem), new Type[] { typeof(ItemData) })]
    [HarmonyPrefix, HarmonyWrapSafe]
    private static bool CheckForTotems(Inventory __instance, ItemData item) => CheckForTotemsCore(__instance, item);

    [HarmonyPatch(typeof(Inventory), nameof(Inventory.AddItem), new Type[]
    {
        typeof(string), typeof(int), typeof(int),
        typeof(int), typeof(long), typeof(string), typeof(bool)
    })]
    [HarmonyPrefix, HarmonyWrapSafe]
    private static bool CheckForTotems(Inventory __instance, string name)
    {
        var item = ObjectDB.instance.GetItem(name)?.m_itemData;
        return CheckForTotemsCore(__instance, item);
    }
}