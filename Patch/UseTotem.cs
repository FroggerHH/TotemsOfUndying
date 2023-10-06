namespace TotemsOfUndying.Patch;

[HarmonyPatch]
public class UseTotem
{
    [HarmonyPatch(typeof(Character), nameof(Character.CheckDeath))]
    [HarmonyPrefix]
    private static void CharacterDeathPatch(Character __instance)
    {
        var isFocused = Application.isFocused;
        if (isFocused && __instance.IsPlayer() && !__instance.IsDead() && __instance == Player.m_localPlayer)
        {
            var inventory = Player.m_localPlayer.GetInventory();
            if (Mathf.Floor(__instance.GetHealth()) <= 0f)
                foreach (var itemData in inventory.GetAllItems())
                    if (itemData.m_shared.m_name.StartsWith("$item_TotemOf"))
                    {
                        Debug("Try to use " + itemData.LocalizeName());
                        (plugin as Plugin).UseTotem(itemData, itemData.m_shared.m_name);
                        return;
                    }
        }
    }
}