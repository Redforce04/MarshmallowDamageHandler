// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         MarshmallowDamageHandler
//    Project:          MarshmallowDamageHandler
//    FileName:         Config.cs
//    Author:           Redforce04#4091
//    Revision Date:    10/30/2023 1:18 AM
//    Created Date:     10/30/2023 1:18 AM
// -----------------------------------------

using System.ComponentModel;
using Exiled.API.Interfaces;

namespace MarshmallowDamageHandler.Internal;
#if EXILED
public class Config : IConfig
#else
public class Config
#endif
{
    [Description("Should this plugin be enabled. This will not stop plugins from using the included api, as long as they initialize it.")]
    public bool IsEnabled { get; set; }
    [Description("Should debug logs be shown.")]
    public bool Debug { get; set; }
    [Description("Should the marshmallow effect, follow the server friendlyfire settings.")]
    public bool MakeMarshmallowRespectFriendlyFire { get; set; }
}