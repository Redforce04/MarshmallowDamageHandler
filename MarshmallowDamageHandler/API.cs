using HarmonyLib;

namespace MarshmallowDamageHandler;

public class Api
{
    internal static bool Initialized { get; set; } = false;
    internal static Harmony Harmony { get; set; }
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