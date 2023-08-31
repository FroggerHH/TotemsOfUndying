using System;
using System.Collections.Generic;
using Extensions;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering;
using static TotemsOfUndying.Plugin;
using static Terminal;

namespace TotemsOfUndying;

public static class TerminalCommands
{
    private static bool isServer => SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;
    private static string modName => ModName;

    [HarmonyPatch(typeof(Terminal), nameof(InitTerminal))]
    [HarmonyWrapSafe]
    internal class AddChatCommands
    {
        private static void Postfix()
        {
            new ConsoleCommand("-",
                "-",
                args =>
                {
                    AddCommand(args =>
                    {
                        #region Errors

                        if (!ZoneSystem.instance) throw new Exception("Command cannot be executed in game menu");
                        if (args.Args.Length < 2 || !int.TryParse(args.Args[1], out var count))
                            throw new Exception("First argument must be a number");
                        if (args.Args.Length < 3)
                            throw new Exception("First argument must be a location name (string)");

                        #endregion

                        // code

                        args.Context.AddString($"Done.");
                    }, args);
                }, true);
        }
    }

    internal static void AddCommand(Action<ConsoleEventArgs> action, ConsoleEventArgs args)
    {
        try
        {
            action.Invoke(args);
        }
        catch (Exception e)
        {
            args.Context.AddString("<color=red>Error: " + e.Message + "</color>");
        }
    }
}