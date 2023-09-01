using HarmonyLib;
using static TotemsOfUndying.Plugin;

namespace TotemsOfUndying.Patch;

[HarmonyPatch]
public class CraftItemDescriptionSize
{
    private static bool firstCall = true;
    private static int vanilaMinSize;

    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.UpdateRecipe))]
    [HarmonyPostfix, HarmonyWrapSafe]
    private static void Patch(InventoryGui __instance)
    {
        if (firstCall)
        {
            vanilaMinSize = __instance.m_recipeDecription.resizeTextMinSize;
            firstCall = false;
        }

        var recipe = __instance.m_selectedRecipe.Key;
        if (recipe == null) return;
        var itemData = recipe.m_item.m_itemData;
        if (itemData == null) return;
        var totem = GetTotem(itemData.m_shared.m_name);

        __instance.m_recipeDecription.resizeTextMinSize = totem == null ? vanilaMinSize : 1;
    }
}