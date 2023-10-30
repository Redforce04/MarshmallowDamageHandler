// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         MarshmallowDamageHandler
//    Project:          MarshmallowDamageHandler
//    FileName:         GetDamageHandlerPatch.cs
//    Author:           Redforce04#4091
//    Revision Date:    10/30/2023 1:09 AM
//    Created Date:     10/30/2023 1:09 AM
// -----------------------------------------

using HarmonyLib;
using InventorySystem.Items.MarshmallowMan;
using PlayerStatsSystem;
using Subtitles;

namespace MarshmallowDamageHandler.Internal.Patches;

[HarmonyPatch(typeof(InventorySystem.Items.MarshmallowMan.MarshmallowItem), nameof(InventorySystem.Items.MarshmallowMan.MarshmallowItem.NewDamageHandler), MethodType.Getter)]
internal static class GetDamageHandlerPatch
{
    internal static bool Prefix(MarshmallowItem __instance, ref UniversalDamageHandler __result)
    {
        __result = new MarshmallowDamageHandler(__instance._attackDamage, DeathTranslations.MarshmallowMan, new DamageHandlerBase.CassieAnnouncement
        {
            Announcement = "TERMINATED BY MARSHMALLOW MAN",
            SubtitleParts = new SubtitlePart[1]
            {
                new SubtitlePart(SubtitleType.TerminatedByMarshmallowMan, (string[])null)
            }
        });

        return false;
    }
}