// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         MarshmallowDamageHandler
//    Project:          MarshmallowDamageHandler
//    FileName:         ProcessMarshmallowDamage.cs
//    Author:           Redforce04#4091
//    Revision Date:    10/30/2023 2:30 AM
//    Created Date:     10/30/2023 2:30 AM
// -----------------------------------------

using CustomPlayerEffects;
using HarmonyLib;
using InventorySystem.Items.MarshmallowMan;
using Mirror;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core;
using Subtitles;

namespace MarshmallowDamageHandler.Internal.Patches;

[HarmonyPatch(typeof(InventorySystem.Items.MarshmallowMan.MarshmallowItem), nameof(InventorySystem.Items.MarshmallowMan.MarshmallowItem.ServerAttack))]
internal static class ProcessMarshmallowDamage
{
    internal static bool Prefix(MarshmallowItem __instance, ReferenceHub syncTarget)
    {
        try
        {

            foreach (IDestructible item in __instance.DetectDestructibles())
            {
                if ((!(item is HitboxIdentity hitboxIdentity) || !(hitboxIdentity.TargetHub != syncTarget)) &&
                    item.Damage(__instance._attackDamage,
                        _getDamageHandler(__instance.Owner, __instance._attackDamage),
                        item.CenterOfMass))
                {
                    bool hitbox = true;
                    if (item is HitboxIdentity hitboxIdentity2 && !hitboxIdentity2.TargetHub.IsAlive())
                    {
                        __instance.Owner.playerEffectsController.GetEffect<SugarCrave>().OnKill();
                        hitbox = (Server.FriendlyFire || !Plugin.IsFF(__instance.Owner.GetRoleId(),
                            hitboxIdentity2.TargetHub.GetRoleId(), Api.CountCuffed));
                    }

                    if (!Api.SendCrosshairIfFriendlyFire || hitbox)
                        Hitmarker.SendHitmarkerDirectly(__instance.Owner, 1f);

                    __instance.ServerSendPublicRpc(delegate(NetworkWriter writer) { writer.WriteByte(1); });
                    break;
                }
            }

        }
        catch (Exception e)
        {
            Log.Error("A critical error has occured while processing marshmallow damage.");
            Log.Debug($"Exception: \n{e}");
        }

        return false;
    }

    private static DamageHandlerBase _getDamageHandler(ReferenceHub attacker, float damage)
    {
        DamageHandlerBase damageHandler = new ScpDamageHandler(attacker, damage, DeathTranslations.MarshmallowMan);
        damageHandler.CassieDeathAnnouncement.Announcement = "TERMINATED BY MARSHMALLOW MAN";
        damageHandler.CassieDeathAnnouncement.SubtitleParts = new SubtitlePart[1]
        {
            new SubtitlePart(SubtitleType.TerminatedByMarshmallowMan)
        };
        return damageHandler;
    }
}
