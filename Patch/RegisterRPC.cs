﻿using HarmonyLib;

namespace TotemsOfUndying.Patch;

[HarmonyPatch(typeof(ZNetScene))]
public static class RegisterRPC
{
    [HarmonyPatch(nameof(ZNetScene.Awake))]
    private static void Postfix()
    {
        try
        {
            ZRoutedRpc.instance.Register("RelocateMerchant", Relocator.RPC_RelocateMerchant);
        }
        catch
        {
        }
    }
}