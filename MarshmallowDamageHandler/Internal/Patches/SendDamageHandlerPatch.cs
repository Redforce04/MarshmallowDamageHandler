// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         MarshmallowDamageHandler
//    Project:          MarshmallowDamageHandler
//    FileName:         PatchSendDamageHandler.cs
//    Author:           Redforce04#4091
//    Revision Date:    10/30/2023 2:02 AM
//    Created Date:     10/30/2023 2:02 AM
// -----------------------------------------

using HarmonyLib;
using Mirror;
using PlayerStatsSystem;
using PluginAPI.Core;
using DamageHandlerBase = PlayerStatsSystem.DamageHandlerBase;

namespace MarshmallowDamageHandler.Internal.Patches;

[HarmonyPatch(typeof(PlayerStatsSystem.DamageHandlerReaderWriter), nameof(PlayerStatsSystem.DamageHandlerReaderWriter.WriteDamageHandler))]
internal static class SendDamageHandlerPatch
{
    internal static bool Prefix(NetworkWriter writer, DamageHandlerBase info)
    {
        if (info is not MarshmallowDamageHandler)
            return true;

        try
        {
            writer.WriteByte(
                DamageHandlers.IdsByTypeHash[typeof(UniversalDamageHandler).FullName.GetStableHashCode()]);
            info.WriteAdditionalData(writer);
        }
        catch (Exception e)
        {
            Log.Error("An error has occured.");
            Log.Debug($"Exception: {e}");
        }

        return false;
    }
}