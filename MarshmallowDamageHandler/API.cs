using HarmonyLib;

namespace MarshmallowDamageHandler;

public class Api
{
    internal static bool Initialized { get; set; } = false;
    internal static Harmony Harmony { get; set; }

    /// <summary>
    /// Should the plugin send a crosshair if friendly fire is used, and friendly fire is off.
    /// </summary>
    public static bool SendCrosshairIfFriendlyFire { get; set; } = false;
    /// <summary>
    /// Should killing a cuffed teammate count as tk.
    /// </summary>
    public static bool CountCuffed { get; set; } = true;
    public static void InitDependencies()
    {
        if (Initialized)
            return;
        
        CosturaUtility.Initialize();
        Harmony = new Harmony("me.redforce04.marshmallowdamagehandler");
        Initialized = true;
        Harmony.PatchAll();
    }
}