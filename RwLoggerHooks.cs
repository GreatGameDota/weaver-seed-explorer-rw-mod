using MonoMod.RuntimeDetour;
using System.Reflection;
using UnityEngine;
using Watcher;

namespace RwLogger;

public static class RwLoggerHooks
{
    public static void Apply()
    {
        On.Menu.PauseMenu.ctor += PauseMenu_ctor;
        // new Hook(typeof(BingoMode.BingoSteamworks.SteamFinal).GetMethod("BroadcastCurrentBoardState", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static), On_BroadcastCurrentBoardState);
        new Hook(typeof(SaveState).GetMethod("RoomsWithWarpsRemainingToBeSealed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static), On_RoomsWithWarpsRemainingToBeSealed);
        new Hook(typeof(WarpPoint).GetMethod("ExpireWarpByWeaver", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static), On_ExpireWarpByWeaver);
        new Hook(typeof(WarpPoint).GetMethod("ChooseDynamicWarpTarget", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static), On_ChooseDynamicWarpTarget);
        new Hook(typeof(WarpPoint).GetMethod("GetAvailableDynamicWarpTargets", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, [typeof(World), typeof(string), typeof(string), typeof(bool)], null), On_GetAvailableDynamicWarpTargets);

        // BingoMode.BingoSteamworks.InnerWorkings is an internal class
        // var InnerWorkings = AppDomain.CurrentDomain.GetAssemblies()
        //     .SelectMany(x => x.GetTypes())
        //     .Where(x => x.IsClass && x.Namespace == "BingoMode.BingoSteamworks")
        //     .FirstOrDefault(x => x.Name == "InnerWorkings");
        // if (InnerWorkings != null)
        //     new Hook(InnerWorkings.GetMethod("SendMessage", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static), On_InnerWorkings_SendMessage);
    }

    private static void PauseMenu_ctor(On.Menu.PauseMenu.orig_ctor orig, Menu.PauseMenu self, ProcessManager manager, RainWorldGame game)
    {
        orig(self, manager, game);
        RwLogger.container = new UI(self, self.pages[0], self.pages[0].pos, new Vector2());
        self.pages[0].subObjects.Add(RwLogger.container);
    }

    public static void On_ExpireWarpByWeaver(Action<WarpPoint> orig, WarpPoint self)
    {
        RwLogger.logger.LogInfo($"ExpireWarpByWeaver called for WarpPoint in room {self.room.abstractRoom.name}");
        var list = new List<string>();
        for (int i = 0; i < self.room.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByVoidWeaver.Count; i++)
        {
            list.Add(self.room.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByVoidWeaver[i]);
        }
        // for (int i = 0; i < list.Count; i++)
        // {
        //     RwLogger.logger.LogInfo($"ExpireWarpByWeaver  Warp Room: {list[i]}");
        // }
        orig(self);
        var listAfter = self.room.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByVoidWeaver;
        // for (int i = 0; i < listAfter.Count; i++)
        // {
        //     RwLogger.logger.LogInfo($"ExpireWarpByWeaver  Warp Room: {listAfter[i]}");
        // }
        for (int i = list.Count; i < listAfter.Count; i++)
        {
            RwLogger.logger.LogInfo($"ExpireWarpByWeaver2  Warp Room: {listAfter[i]}");
        }
    }

    public static List<string> On_RoomsWithWarpsRemainingToBeSealed(Func<SaveState, bool, string, List<string>> orig, SaveState self, bool includeSpinningTopWarps, string filterByRegion = "")
    {
        // RwLogger.logger.LogInfo($"RoomsWithWarpsRemainingToBeSealed called with includeSpinningTopWarps={includeSpinningTopWarps}, filterByRegion={filterByRegion}");
        List<string> list = orig(self, includeSpinningTopWarps, filterByRegion);
        // for (int i = 0; i < list.Count; i++)
        // {
        //     RwLogger.logger.LogInfo($"RoomsWithWarpsRemainingToBeSealed  Warp Room: {list[i]}");
        // }
        // if (list.Count != 0)
        // {
        //     list.Sort();
        //     UnityEngine.Random.State state = UnityEngine.Random.state;
        //     UnityEngine.Random.InitState(RWCustom.Custom.rainWorld.progression.miscProgressionData.watcherCampaignSeed + list.Count);
        //     string text = list[UnityEngine.Random.Range(0, list.Count)].ToLowerInvariant();
        //     UnityEngine.Random.state = state;
        //     RwLogger.logger.LogInfo($"RoomsWithWarpsRemainingToBeSealed roomToBeClosed={text}");
        // }
        return list;
    }

    public static string On_ChooseDynamicWarpTarget(Func<World, string, string, bool, bool, bool, string> orig, World world, string oldRoom, string targetRegion, bool badWarp, bool spreadingRot, bool playerCreated)
    {
        // RwLogger.logger.LogInfo($"ChooseDynamicWarpTarget called for WarpPoint with oldRoom={oldRoom}, targetRegion={targetRegion}, badWarp={badWarp}, spreadingRot={spreadingRot}, playerCreated={playerCreated}");

        // var discoveredPoints = new Dictionary<string, string>(world.game.GetStorySession.saveState.miscWorldSaveData.discoveredWarpPoints);
        // var spawnedPoints = new Dictionary<string, string>(world.game.GetStorySession.saveState.deathPersistentSaveData.spawnedWarpPoints);
        // RwLogger.logger.LogInfo($"ChooseDynamicWarpTarget  Discovered Warp Points:");
        // foreach (var kvp in discoveredPoints)
        // {
        //     RwLogger.logger.LogInfo($"    {kvp.Key} => {kvp.Value}");
        // }
        // RwLogger.logger.LogInfo($"ChooseDynamicWarpTarget  Spawned Warp Points:");
        // foreach (var kvp in spawnedPoints)
        // {
        //     RwLogger.logger.LogInfo($"    {kvp.Key} => {kvp.Value}");
        // }

        string text = orig(world, oldRoom, targetRegion, badWarp, spreadingRot, playerCreated);
        // RwLogger.logger.LogInfo($"ChooseDynamicWarpTarget returned target room {text}");

        // RwLogger.logger.LogInfo($"ChooseDynamicWarpTarget  Discovered Warp Points:");
        // foreach (var kvp in world.game.GetStorySession.saveState.miscWorldSaveData.discoveredWarpPoints)
        // {
        //     RwLogger.logger.LogInfo($"    {kvp.Key} => {kvp.Value}");
        // }
        // RwLogger.logger.LogInfo($"ChooseDynamicWarpTarget  Spawned Warp Points:");
        // foreach (var kvp in world.game.GetStorySession.saveState.deathPersistentSaveData.spawnedWarpPoints)
        // {
        //     RwLogger.logger.LogInfo($"    {kvp.Key} => {kvp.Value}");
        // }
        return text;
    }

    public static List<string> On_GetAvailableDynamicWarpTargets(Func<World, string, string, bool, List<string>> orig, World world, string oldRoom, string targetRegion, bool spreadingRot)
    {
        // RwLogger.logger.LogInfo($"GetAvailableDynamicWarpTargets called for WarpPoint with oldRoom={oldRoom}, targetRegion={targetRegion}, spreadingRot={spreadingRot}");
        List<string> text = orig(world, oldRoom, targetRegion, spreadingRot);
        // for (int i = 0; i < text.Count; i++)
        // {
        //     RwLogger.logger.LogInfo($"GetAvailableDynamicWarpTargets  Warp Room: {text[i]}");
        // }
        return text;
    }
}
