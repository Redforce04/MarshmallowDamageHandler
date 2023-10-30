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

using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Enums;
using PluginAPI.Events;

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
            Log.Debug($"Marshmallow damage detected. {player.Nickname} -> {ev.Target.Nickname}");
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
        
        
        if (player.Team == ev.Target.Team);
        {
            if(Config.Debug)
                Log.Debug($"Marshmallow damage blocked.");
            return false;
        }
        return true;
    } 

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