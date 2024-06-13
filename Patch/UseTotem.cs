namespace TotemsOfUndying.Patch;

[HarmonyPatch]
public class UseTotem
{
    [HarmonyPatch(typeof(Character), nameof(Character.CheckDeath))]
    [HarmonyPrefix]
    private static void CharacterDeathPatch(Character __instance)
    {
        var isFocused = Application.isFocused;
        if (!isFocused || !__instance.IsPlayer() || __instance.IsDead() || __instance != Player.m_localPlayer) return;
        var inventory = Player.m_localPlayer.GetInventory();
        if (!(Mathf.Floor(__instance.GetHealth()) <= 0f)) return;
        foreach (var itemData in inventory.GetAllItems()
                     .Where(itemData => itemData.m_shared.m_name.StartsWith("$item_TotemOf")).ToList())
        {
            Debug("Try to use " + itemData.LocalizeName());
            UseTotem(itemData, itemData.m_shared.m_name);
            return;
        }
    }
}