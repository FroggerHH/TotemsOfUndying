namespace TotemsOfUndying.Patch;

[HarmonyPatch]
public class CraftItemDescriptionSize
{
    private static bool firstCall = true;
    private static float vanilaMinSize;

    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.UpdateRecipe))]
    [HarmonyPostfix] [HarmonyWrapSafe]
    private static void Patch(InventoryGui __instance)
    {
        if (firstCall)
        {
            vanilaMinSize = __instance.m_recipeDecription.fontSizeMin;
            firstCall = false;
        }

        var recipe = __instance.m_selectedRecipe.Key;
        if (recipe == null) return;
        var itemData = recipe.m_item.m_itemData;
        if (itemData == null) return;
        var totem = GetTotem(itemData.m_shared.m_name);

        __instance.m_recipeDecription.fontSizeMin = totem == null ? vanilaMinSize : 1;
    }
}