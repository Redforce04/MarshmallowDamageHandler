// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         MarshmallowDamageHandler
//    Project:          MarshmallowDamageHandler
//    FileName:         MarshmallowDamageHandler.cs
//    Author:           Redforce04#4091
//    Revision Date:    10/30/2023 1:04 AM
//    Created Date:     10/30/2023 1:04 AM
// -----------------------------------------

using PlayerStatsSystem;

namespace MarshmallowDamageHandler;

public sealed class MarshmallowDamageHandler : UniversalDamageHandler
{
    public MarshmallowDamageHandler(float damage, DeathTranslation deathReason, CassieAnnouncement? cassieAnnouncement = null) : base(damage, deathReason, cassieAnnouncement) { }

    public MarshmallowDamageHandler() : base()
    { }
    
}