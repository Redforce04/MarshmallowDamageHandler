// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         MarshmallowDamageHandler
//    Project:          MarshmallowDamageHandler
//    FileName:         NWApiPlugin.cs
//    Author:           Redforce04#4091
//    Revision Date:    10/30/2023 1:17 AM
//    Created Date:     10/30/2023 1:17 AM
// -----------------------------------------
#if EXILED
using Exiled.API.Features;
using PluginPriority = Exiled.API.Enums.PluginPriority;
#endif

using PlayerRoles;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Enums;
using PluginAPI.Events;
using Log = PluginAPI.Core.Log;
using Player = PluginAPI.Core.Player;

namespace MarshmallowDamageHandler.Internal;
#if EXILED
public class Plugin : Plugin<Config>
#else
public class Plugin
#endif
{
    public const string PluginVersion = "1.0.0";
    public static Plugin Singleton { get; private set; }
    
#if EXILED
    public override string Name => "MarshmallowDamageFixes";
    public override string Author => "Redforce04";
    public override string Prefix => "marsh";
    public override PluginPriority Priority => PluginPriority.First;
    public override Version Version => System.Version.Parse(PluginVersion);
    public override void OnEnabled()
#else
    [PluginConfig]
    public Config Config; 

    [PluginEntryPoint("MarshmallowDamageFixes", PluginVersion, "Makes marshmallow damage respect server friendly fire settings.", "Redforce04")]
    public void OnEnabled()
#endif
    {
        Log.Info($"MarshmallowDamageFixes is ready.{(Config.Debug ? " [Debug]" : "")}");
        Singleton = this;
        Api.InitDependencies();
        Api.CountCuffed = Config.CountCuffed;
        EventManager.RegisterEvents(this);
    }

    [PluginEvent(ServerEventType.PlayerDamage)]
    internal bool OnPlayerDamage(PlayerDamageEvent ev)
    {
        if (ev is null)
            return true;
        //if (ev.Player is null)
        //    return true;
        if (ev.Target is null)
            return true;
        if (ev.DamageHandler is null)
            return true;
            
        if (Config.Debug) Log.Debug($"damage detected {ev.DamageHandler.GetType().FullName}");

        if(ev.DamageHandler is not MarshmallowDamageHandler marsh)
            return true;
        Player? player = ev.Player ?? marsh.Player;
        if (Config.Debug) 
            Log.Debug($"Marshmallow damage detected. {player.Nickname} [{player.Team}]-> {ev.Target.Nickname}");
        if (player is null)
        {
            if (Config.Debug)
                Log.Debug($"Marshmallow damage detected. Player still null");
            return false;
        }
        if (!Config.MakeMarshmallowRespectFriendlyFire)
            return true;
        
        if(PluginAPI.Core.Server.FriendlyFire) 
            return true; 
        
        
        if (IsFF(player.Role, ev.Target.Role, Api.CountCuffed))
        {
            if(Config.Debug)
                Log.Debug($"Marshmallow damage blocked. {player.Role} -> {ev.Target.Role}");
            return false;
        }
        return true;
    }

    internal static bool IsFF(RoleTypeId role1, RoleTypeId role2, bool countCuffed)
    {
        int team1 = _getTeam(role1);
        int team2 = _getTeam(role2);
        if (team1 == team2)
        {
            //Log.Debug($"FF [True] {role1} [{team1}] -> {role2} [{team2}], team == team");
            return true;
        }

        if (countCuffed && (team1 is 1 or 2 && team2 is 1 or 2))
        {
            //Log.Debug($"FF [True] {role1} [{team1}] -> {role2} [{team2}], cuffed all team");
            return true;
        }

        //Log.Debug($"FF [False] {role1} [{team1}] -> {role2} [{team2}] ");
        return false;
    }

    private static int _getTeam(RoleTypeId id) => id switch
        {
            RoleTypeId.Scientist => 1,
            RoleTypeId.NtfCaptain => 1,
            RoleTypeId.NtfPrivate => 1,
            RoleTypeId.NtfSergeant => 1,
            RoleTypeId.NtfSpecialist => 1,
            RoleTypeId.ClassD => 2,
            RoleTypeId.ChaosConscript => 2,
            RoleTypeId.ChaosMarauder => 2,
            RoleTypeId.ChaosRepressor => 2,
            RoleTypeId.ChaosRifleman => 2,
            RoleTypeId.Tutorial => 3,
            RoleTypeId.Filmmaker => 3,
            RoleTypeId.Spectator => 3,
            RoleTypeId.Overwatch => 3,
            RoleTypeId.Scp049 => 4,
            RoleTypeId.Scp079 => 4,
            RoleTypeId.Scp096 => 4,
            RoleTypeId.Scp106 => 4,
            RoleTypeId.Scp173 => 4,
            RoleTypeId.Scp0492 => 4,
            RoleTypeId.Scp939 => 4,
            RoleTypeId.Scp3114 => 4,
            _ => 0,
        };
    
#if !EXILED
    [PluginUnload]
    public void OnDisabled()
#else
    public override void OnDisabled()
#endif
    {
        EventManager.UnregisterEvents(this);
    }
}