// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         MarshmallowDamageHandler
//    Project:          MarshmallowDamageHandler
//    FileName:         Extensions.cs
//    Author:           Redforce04#4091
//    Revision Date:    11/04/2023 3:38 PM
//    Created Date:     11/04/2023 3:38 PM
// -----------------------------------------

using PlayerStatsSystem;
using Subtitles;

namespace MarshmallowDamageHandler;

public static class Extensions
{
    public static bool IsMarshmallowDamageHandler(this DamageHandlerBase damageHandlerBase)
    {
        if (damageHandlerBase is not ScpDamageHandler scp)
            return false;
        return scp.CassieDeathAnnouncement.Announcement == "TERMINATED BY MARSHMALLOW MAN" ||
            scp.CassieDeathAnnouncement.SubtitleParts.Any(x => x.Subtitle == SubtitleType.TerminatedByMarshmallowMan);
    }
}