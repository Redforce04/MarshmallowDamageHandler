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

namespace MarshmallowDamageHandler.Internal.Patches;

[HarmonyPatch(typeof(InventorySystem.Items.MarshmallowMan.MarshmallowItem), nameof(InventorySystem.Items.MarshmallowMan.MarshmallowItem.ServerAttack))]
internal static class ProcessMarshmallowDamage
{
    internal static bool Prefix(MarshmallowItem __instance, ReferenceHub syncTarget)
    {
        foreach (IDestructible item in __instance.DetectDestructibles())
        {
            if ((!(item is HitboxIdentity hitboxIdentity) || !(hitboxIdentity.TargetHub != syncTarget)) && item.Damage(__instance._attackDamage, _getDamageHandler(__instance.NewDamageHandler, Player.Get(__instance.Owner)), item.CenterOfMass))
            {
                bool hitbox = true;
                if (item is HitboxIdentity hitboxIdentity2 && !hitboxIdentity2.TargetHub.IsAlive())
                {
                    __instance.Owner.playerEffectsController.GetEffect<SugarCrave>().OnKill();
                    hitbox = (Server.FriendlyFire || !Plugin.IsFF(__instance.Owner.GetRoleId(), hitboxIdentity2.TargetHub.GetRoleId(), Api.CountCuffed)) ;
                }
                if(!Api.SendCrosshairIfFriendlyFire || hitbox)
                    Hitmarker.SendHitmarkerDirectly(__instance.Owner, 1f);
                
                __instance.ServerSendPublicRpc(delegate(NetworkWriter writer)
                {
                    writer.WriteByte(1);
                });
                break;
            }
        }

        return false;
    }

    private static UniversalDamageHandler _getDamageHandler(UniversalDamageHandler handler, Player ply)
    {
        if (handler is MarshmallowDamageHandler marsh)
        {
            marsh.Player = ply;
            return marsh;
        }

        return handler;
    }
}
