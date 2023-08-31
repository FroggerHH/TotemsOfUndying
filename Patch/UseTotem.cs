using Extensions.Valheim;
using HarmonyLib;
using UnityEngine;
using static TotemsOfUndying.Plugin;

namespace TotemsOfUndying.Patch;

[HarmonyPatch]
public class UseTotem
{
    [HarmonyPatch(typeof(Character), nameof(Character.CheckDeath))]
    [HarmonyPrefix]
    private static void CharacterDeathPatch(Character __instance)
    {
        bool isFocused = Application.isFocused;
        if (isFocused && __instance.IsPlayer() && !__instance.IsDead() && __instance == Player.m_localPlayer)
        {
            Inventory inventory = Player.m_localPlayer.GetInventory();
            if (Mathf.Floor(__instance.GetHealth()) <= 0f)
                foreach (ItemDrop.ItemData itemData in inventory.GetAllItems())
                    if (itemData.m_shared.m_name.StartsWith("$item_TotemOf"))
                    {
                        Debug("Try to use " + itemData.LocalizeName());
                        _self.UseTotem(itemData, itemData.m_shared.m_name);
                        return;
                    }
        }
    }
}