using BepInEx;
using BepInEx.Logging;
using System.Security;
using System.Security.Permissions;
using UnityEngine;

#pragma warning disable CS0618
[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace RwLogger;

[BepInPlugin("greatgamedota.rwlogger", "RwLogger", "0.1.0")]
public class RwLogger : BaseUnityPlugin
{
    public static Vector2 screenDims;
    public static UI container;
    public static RainWorld game;

    internal static ManualLogSource logger;
    private static bool init;

    public void OnEnable()
    {
        On.RainWorld.PostModsInit += On_RainWorld_PostModsInit;
        logger = Logger;
    }

    public void OnDisable()
    {
        logger = null;
    }

    public static void On_RainWorld_PostModsInit(On.RainWorld.orig_PostModsInit orig, RainWorld self)
    {
        orig(self);
        if (init) return;

        init = true;
        game = self;
        try
        {
            RwLoggerHooks.Apply();
            screenDims = RWCustom.Custom.rainWorld.options.ScreenSize;
        }
        catch (Exception ex)
        {
            logger.LogInfo($"RwLogger failed to load! ${ex.Message}");
        }
    }
}