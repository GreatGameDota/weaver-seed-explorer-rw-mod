using Menu;
using Menu.Remix;
using UnityEngine;
using System.Reflection;

namespace RwLogger;

public class UI : RectangularMenuObject
{
    public SimpleButton warp;
    public MenuTabWrapper wrapper;

    // Adjacency list for all portal connections
    private static readonly Dictionary<string, List<string>> portalConnections = new()
    {
        {"WRFA", new List<string>{"WRFA_D08", "WRFA_F01", "WRFA_A21"}},
        {"WSKA", new List<string>{"WSKA_D13", "WSKA_N04"}},
        {"WSKB", new List<string>{"WSKB_C18", "WSKB_C07"}},
        {"WPGA", new List<string>{"WPGA_B08", "WPGA_B10", "WPGA_E01"}},
        {"WARF", new List<string>{"WARF_B11", "WARF_A06", "WARF_B14", "WARF_D30", "WARF_D06"}},
        {"WRRA", new List<string>{"WRRA_B01", "WRRA_A07", "WRRA_A26"}},
        {"WMPA", new List<string>{"WMPA_A09", "WMPA_D09", "WMPA_C07"}},
        {"WBLA", new List<string>{"WBLA_B08"}},
        {"WTDA", new List<string>{"WTDA_B16", "WTDA_A13"}},
        {"WSKD", new List<string>{"WSKD_B38", "WSKD_B01", "WSKD_B12", "WSKD_B42"}},
        {"WARG", new List<string>{"WARG_B31", "WARG_W11", "WARG_D06_FUTURE", "WARG_A06_FUTURE"}},
        {"WARD", new List<string>{"WARD_E12", "WARD_B41", "WARD_E01", "WARD_E33"}},
        {"WVWA", new List<string>{"WVWA_E01", "WVWA_H01"}},
        {"WVWB", new List<string>{"WVWB_E01"}},
        {"WRFB", new List<string>{"WRFB_B12"}},
        {"WTDB", new List<string>{"WTDB_A19", "WTDB_A04"}},
        {"WSKC", new List<string>{"WSKC_A10"}},
        {"WPTA", new List<string>{"WPTA_C07"}},
        {"WARB", new List<string>{"WARB_F18", "WARB_F01"}},
        {"WARC", new List<string>{"WARC_E11", "WARC_B12"}},
        {"WARE", new List<string>{"WARE_H16", "WARE_H21"}},
        {"WARA", new List<string>()}
    };

    // Lookup table: Key format "NATURAL_PORTAL|NATURAL_PORTAL" -> weight/distance
    // this is unfinished, only like 2 examples of echo -> natural warp or natural warp -> natural warp
    // torrid echo dropoff -> heatducts, stormy echo dropoff -> coldstorage, heatducts torrid -> heatducts salination
    // sali heatducts -> sali fetid, pillargrove coral -> pillargrove torrential, torrential aether -> torrential pillar grove
    // private static readonly Dictionary<string, int> naturalPortalWeights = new()
    // {
    //     // WARF - natural portal to natural portal
    //     {"WARF_B11|WARF_A06", 35}, {"WARF_B11|WARF_B14", 25}, {"WARF_B11|WARF_D30", 40}, {"WARF_B11|WARF_D06", 45},
    //     {"WARF_A06|WARF_B11", 40}, {"WARF_A06|WARF_B14", 40}, {"WARF_A06|WARF_D30", 40}, {"WARF_A06|WARF_D06", 40},
    //     {"WARF_B14|WARF_B11", 30}, {"WARF_B14|WARF_A06", 30}, {"WARF_B14|WARF_D30", 30}, {"WARF_B14|WARF_D06", 30},
    //     {"WARF_D30|WARF_B11", 45},
    //     {"WARF_D06|WARF_B11", 50},

    //     {"WARF_A06|WARF_B14", 45}, {"WARF_B14|WARF_A06", 50},
    //     {"WARF_A06|WARF_D30", 45}, {"WARF_D30|WARF_A06", 50},
    //     {"WARF_A06|WARF_D06", 50}, {"WARF_D06|WARF_A06", 45},
    //     {"WARF_B14|WARF_D30", 20}, {"WARF_D30|WARF_B14", 25},
    //     {"WARF_B14|WARF_D06", 30}, {"WARF_D06|WARF_B14", 35},
    //     {"WARF_D30|WARF_D06", 25}, {"WARF_D06|WARF_D30", 30},

    //     // WRFA - natural portal to natural portal
    //     {"WRFA_D08|WRFA_F01", 35}, {"WRFA_F01|WRFA_D08", 40},
    //     {"WRFA_D08|WRFA_A21", 40}, {"WRFA_A21|WRFA_D08", 45},
    //     {"WRFA_F01|WRFA_A21", 25}, {"WRFA_A21|WRFA_F01", 30},

    //     // WSKA - natural portal to natural portal
    //     {"WSKA_D13|WSKA_N04", 15}, {"WSKA_N04|WSKA_D13", 20},

    //     // WSKB - natural portal to natural portal
    //     {"WSKB_C18|WSKB_C07", 20}, {"WSKB_C07|WSKB_C18", 25},

    //     // WPGA - natural portal to natural portal
    //     {"WPGA_B08|WPGA_B10", 10}, {"WPGA_B10|WPGA_B08", 15},
    //     {"WPGA_B08|WPGA_E01", 45}, {"WPGA_E01|WPGA_B08", 50},
    //     {"WPGA_B10|WPGA_E01", 40}, {"WPGA_E01|WPGA_B10", 45},
    //     {"WARG_W11|WARG_D06_FUTURE", 45}, {"WARG_W11|WARG_A06_FUTURE", 50},
    //     {"WARG_D06_FUTURE|WARG_A06_FUTURE", 30},

    //     // WPGA - natural portal to natural portal
    //     {"WPGA_B08|WPGA_B10", 10}, {"WPGA_B08|WPGA_E01", 45}, {"WPGA_B10|WPGA_E01", 40},

    //     // WRRA - natural portal to natural portal
    //     {"WRRA_B01|WRRA_A07", 35}, {"WRRA_B01|WRRA_A26", 45}, {"WRRA_A07|WRRA_A26", 30},

    //     // WMPA - natural portal to natural portal
    //     {"WMPA_A09|WMPA_D09", 45}, {"WMPA_A09|WMPA_C07", 40}, {"WMPA_D09|WMPA_C07", 35},

    //     // WSKD - natural portal to natural portal
    //     {"WSKD_B38|WSKD_B01", 35}, {"WSKD_B38|WSKD_B12", 40}, {"WSKD_B38|WSKD_B42", 45},
    //     {"WSKD_B01|WSKD_B12", 35}, {"WSKD_B01|WSKD_B42", 40},
    //     {"WSKD_B12|WSKD_B42", 15},

    //     // WTDA - natural portal to natural portal
    //     {"WTDA_B16|WTDA_A13", 40},

    //     // WARD - natural portal to natural portal
    //     {"WARD_E12|WARD_B41", 25}, {"WARD_E12|WARD_E01", 30}, {"WARD_E12|WARD_E33", 45},
    //     {"WARD_B41|WARD_E01", 25}, {"WARD_B41|WARD_E33", 45},
    //     {"WARD_E01|WARD_E33", 35},

    //     // WARE - natural portal to natural portal
    //     {"WARE_H16|WARE_H21", 25},

    //     // WTDB - natural portal to natural portal
    //     {"WTDB_A19|WTDB_A04", 40},

    //     // WVWA - natural portal to natural portal
    //     {"WVWA_E01|WVWA_H01", 35},

    //     // WARB - natural portal to natural portal
    //     {"WARB_F18|WARB_F01", 35},

    //     // WARC - natural portal to natural portal
    //     {"WARC_E11|WARC_B12", 40},
    // };

    // Lookup table: Key format "DYNAMIC_WARP|NATURAL_PORTAL" -> weight/distance
    // These numbers dont take into account how large the destination room is (will take longer between warps) (all except WARF_D29-WARF_D06)
    private static readonly Dictionary<string, int> warpToPortalWeights = new()
    {
        // WARF - dynamic warps to natural portals
        {"WARF_A01|WARF_B11", 45}, {"WARF_A01|WARF_A06", 2}, {"WARF_A01|WARF_B14", 50}, {"WARF_A01|WARF_D30", 45}, {"WARF_A01|WARF_D06", 50},
        {"WARF_B07|WARF_B11", 25}, {"WARF_B07|WARF_A06", 35}, {"WARF_B07|WARF_B14", 45}, {"WARF_B07|WARF_D30", 45}, {"WARF_B07|WARF_D06", 45},
        {"WARF_B23|WARF_B11", 30}, {"WARF_B23|WARF_A06", 45}, {"WARF_B23|WARF_B14", 10}, {"WARF_B23|WARF_D30", 25}, {"WARF_B23|WARF_D06", 35},
        {"WARF_D29|WARF_B11", 45}, {"WARF_D29|WARF_A06", 50}, {"WARF_D29|WARF_B14", 15}, {"WARF_D29|WARF_D30", 20}, {"WARF_D29|WARF_D06", 4},
        
        // WRFA - dynamic warps to natural portals
        {"WRFA_A05|WRFA_D08", 50}, {"WRFA_A05|WRFA_F01", 25}, {"WRFA_A05|WRFA_A21", 45},
        {"WRFA_A07|WRFA_D08", 50}, {"WRFA_A07|WRFA_F01", 15}, {"WRFA_A07|WRFA_A21", 35},
        {"WRFA_A08|WRFA_D08", 50}, {"WRFA_A08|WRFA_F01", 25}, {"WRFA_A08|WRFA_A21", 45},
        {"WRFA_SK02|WRFA_D08", 50}, {"WRFA_SK02|WRFA_F01", 16}, {"WRFA_SK02|WRFA_A21", 36},
        {"WRFA_A12|WRFA_D08", 50}, {"WRFA_A12|WRFA_F01", 3}, {"WRFA_A12|WRFA_A21", 6},
        
        // WSKA - dynamic warps to natural portals
        {"WSKA_D01|WSKA_D13", 4}, {"WSKA_D01|WSKA_N04", 3},
        {"WSKA_D06|WSKA_D13", 2}, {"WSKA_D06|WSKA_N04", 6},
        {"WSKA_D10|WSKA_D13", 2}, {"WSKA_D10|WSKA_N04", 4},
        {"WSKA_D18|WSKA_D13", 2}, {"WSKA_D18|WSKA_N04", 10},
        {"WSKA_D27|WSKA_D13", 12}, {"WSKA_D27|WSKA_N04", 15},
        
        // WSKB - dynamic warps to natural portals
        {"WSKB_C03|WSKB_C18", 12}, {"WSKB_C03|WSKB_C07", 2},
        {"WSKB_C15|WSKB_C18", 25}, {"WSKB_C15|WSKB_C07", 6},
        {"WSKB_N12|WSKB_C18", 2}, {"WSKB_N12|WSKB_C07", 25},
        {"WSKB_N16|WSKB_C18", 2}, {"WSKB_N16|WSKB_C07", 25},
        
        // WARG - dynamic warps to natural portals
        {"WARG_G05|WARG_B31", 50}, {"WARG_G05|WARG_W11", 50}, {"WARG_G05|WARG_D06_FUTURE", 50}, {"WARG_G05|WARG_A06_FUTURE", 50},
        {"WARG_G21|WARG_B31", 3}, {"WARG_G21|WARG_W11", 50}, {"WARG_G21|WARG_D06_FUTURE", 27}, {"WARG_G21|WARG_A06_FUTURE", 50},
        {"WARG_G28|WARG_B31", 50}, {"WARG_G28|WARG_W11", 30}, {"WARG_G28|WARG_D06_FUTURE", 50}, {"WARG_G28|WARG_A06_FUTURE", 50},
        {"WARG_G30|WARG_B31", 50}, {"WARG_G30|WARG_W11", 40}, {"WARG_G30|WARG_D06_FUTURE", 50}, {"WARG_G30|WARG_A06_FUTURE", 50},
        {"WARG_W02|WARG_B31", 50}, {"WARG_W02|WARG_W11", 30}, {"WARG_W02|WARG_D06_FUTURE", 50}, {"WARG_W02|WARG_A06_FUTURE", 50},
        {"WARG_O06_FUTURE|WARG_B31", 10}, {"WARG_O06_FUTURE|WARG_W11", 50}, {"WARG_O06_FUTURE|WARG_D06_FUTURE", 25}, {"WARG_O06_FUTURE|WARG_A06_FUTURE", 45},
        
        // WBLA - dynamic warps to natural portals
        {"WBLA_C02|WBLA_B08", 10},
        {"WBLA_C05|WBLA_B08", 11},
        {"WBLA_D02|WBLA_B08", 7},
        {"WBLA_F04|WBLA_B08", 7},
        
        // WPGA - dynamic warps to natural portals
        {"WPGA_B09|WPGA_B08", 5}, {"WPGA_B09|WPGA_B10", 5}, {"WPGA_B09|WPGA_E01", 45},
        {"WPGA_B12|WPGA_B08", 9}, {"WPGA_B12|WPGA_B10", 6}, {"WPGA_B12|WPGA_E01", 45},
        
        // WRRA - dynamic warps to natural portals
        {"WRRA_B06|WRRA_B01", 50}, {"WRRA_B06|WRRA_A07", 40}, {"WRRA_B06|WRRA_A26", 50},
        {"WRRA_D03|WRRA_B01", 50}, {"WRRA_D03|WRRA_A07", 35}, {"WRRA_D03|WRRA_A26", 5},
        {"WRRA_D04|WRRA_B01", 50}, {"WRRA_D04|WRRA_A07", 40}, {"WRRA_D04|WRRA_A26", 6},
        {"WRRA_D05|WRRA_B01", 50}, {"WRRA_D05|WRRA_A07", 41}, {"WRRA_D05|WRRA_A26", 5},
        
        // WMPA - dynamic warps to natural portals
        {"WMPA_A07|WMPA_A09", 7}, {"WMPA_A07|WMPA_D09", 50}, {"WMPA_A07|WMPA_C07", 50},
        {"WMPA_D03|WMPA_A09", 50}, {"WMPA_D03|WMPA_D09", 4}, {"WMPA_D03|WMPA_C07", 50},
        
        // WRFB - dynamic warps to natural portals
        {"WRFB_B05|WRFB_B12", 50},
        {"WRFB_B08|WRFB_B12", 50},
        {"WRFB_B11|WRFB_B12", 50},
        {"WRFB_D02|WRFB_B12", 50},
        
        // WSKD - dynamic warps to natural portals
        {"WSKD_B05|WSKD_B38", 50}, {"WSKD_B05|WSKD_B01", 2}, {"WSKD_B05|WSKD_B12", 50}, {"WSKD_B05|WSKD_B42", 10},
        {"WSKD_B18|WSKD_B38", 50}, {"WSKD_B18|WSKD_B01", 50}, {"WSKD_B18|WSKD_B12", 3}, {"WSKD_B18|WSKD_B42", 3},
        {"WSKD_B31|WSKD_B38", 15}, {"WSKD_B31|WSKD_B01", 7}, {"WSKD_B31|WSKD_B12", 16}, {"WSKD_B31|WSKD_B42", 15},
        {"WSKD_B35|WSKD_B38", 5}, {"WSKD_B35|WSKD_B01", 12}, {"WSKD_B35|WSKD_B12", 50}, {"WSKD_B35|WSKD_B42", 50},
        
        // WTDA - dynamic warps to natural portals
        {"WTDA_B01|WTDA_B16", 50}, {"WTDA_B01|WTDA_A13", 1},
        {"WTDA_Z04|WTDA_B16", 50}, {"WTDA_Z04|WTDA_A13", 50},
        {"WTDA_Z08|WTDA_B16", 50}, {"WTDA_Z08|WTDA_A13", 50},
        
        // WARD - dynamic warps to natural portals
        {"WARD_D27|WARD_E12", 11}, {"WARD_D27|WARD_B41", 3}, {"WARD_D27|WARD_E01", 16}, {"WARD_D27|WARD_E33", 50}, {"WARD_D27|WARD_R10", 999},
        {"WARD_E02|WARD_E12", 11}, {"WARD_E02|WARD_B41", 5}, {"WARD_E02|WARD_E01", 15}, {"WARD_E02|WARD_E33", 50}, {"WARD_E02|WARD_R10", 999},
        {"WARD_E03|WARD_E12", 11}, {"WARD_E03|WARD_B41", 5}, {"WARD_E03|WARD_E01", 15}, {"WARD_E03|WARD_E33", 50}, {"WARD_E03|WARD_R10", 999},
        {"WARD_R07|WARD_E12", 50}, {"WARD_R07|WARD_B41", 50}, {"WARD_R07|WARD_E01", 35}, {"WARD_R07|WARD_E33", 50}, {"WARD_R07|WARD_R10", 999},
        {"WARD_R16|WARD_E12", 50}, {"WARD_R16|WARD_B41", 50}, {"WARD_R16|WARD_E01", 50}, {"WARD_R16|WARD_E33", 50}, {"WARD_R16|WARD_R10", 999},
        
        // WARE - dynamic warps to natural portals
        {"WARE_H01|WARE_H16", 10}, {"WARE_H01|WARE_H21", 15},
        {"WARE_H02|WARE_H16", 3}, {"WARE_H02|WARE_H21", 5},
        
        // WTDB - dynamic warps to natural portals
        {"WTDB_A06|WTDB_A19", 50}, {"WTDB_A06|WTDB_A04", 2},
        {"WTDB_A10|WTDB_A19", 50}, {"WTDB_A10|WTDB_A04", 3},
        {"WTDB_A13|WTDB_A19", 5}, {"WTDB_A13|WTDB_A04", 10},
        {"WTDB_A15|WTDB_A19", 2}, {"WTDB_A15|WTDB_A04", 50},
        {"WTDB_A17|WTDB_A19", 1}, {"WTDB_A17|WTDB_A04", 45},
        
        // WSKC - dynamic warps to natural portals
        {"WSKC_A05|WSKC_A10", 12},
        {"WSKC_A19|WSKC_A10", 17},
        
        // WVWA - dynamic warps to natural portals
        {"WVWA_D01|WVWA_E01", 40}, {"WVWA_D01|WVWA_H01", 25},
        {"WVWA_F02|WVWA_E01", 50}, {"WVWA_F02|WVWA_H01", 10},
        
        // WVWB - dynamic warps to natural portals
        {"WVWB_H01|WVWB_E01", 20},
        
        // WARB - dynamic warps to natural portals
        {"WARB_F03|WARB_F18", 40}, {"WARB_F03|WARB_F01", 1},
        {"WARB_F11|WARB_F18", 45}, {"WARB_F11|WARB_F01", 50},
        {"WARB_J07|WARB_F18", 50}, {"WARB_J07|WARB_F01", 50},
        
        // WPTA - dynamic warps to natural portals
        {"WPTA_D03|WPTA_C07", 50},
        {"WPTA_G01|WPTA_C07", 50},
        {"WPTA_B06|WPTA_C07", 4},
        {"WPTA_F01|WPTA_C07", 25},
        
        // WARC - dynamic warps to natural portals
        {"WARC_A02|WARC_E11", 50}, {"WARC_A02|WARC_B12", 15},
        {"WARC_A04|WARC_E11", 50}, {"WARC_A04|WARC_B12", 50},
        {"WARC_B08|WARC_E11", 50}, {"WARC_B08|WARC_B12", 50},
        {"WARC_C06|WARC_E11", 50}, {"WARC_C06|WARC_B12", 50}
    };

    public UI(Menu.Menu menu, MenuObject owner, Vector2 pos, Vector2 size) : base(menu, owner, pos, size)
    {
        wrapper = new MenuTabWrapper(menu, this);
        subObjects.Add(wrapper);

        warp = new SimpleButton(menu, this, "WARP", "warp", new Vector2(145f, RwLogger.screenDims.y - 135f), new Vector2(110f, 30f));
        subObjects.Add(warp);
    }

    public override void Singal(MenuObject sender, string message)
    {
        base.Singal(sender, message);
        if (message == "warp")
        {
            Task.Run(() =>
            {
                RainWorldGame _game = (RainWorldGame)RwLogger.game.processManager.currentMainLoop;
                var room = _game.Players[0].Room;
                var world = room.world;
                var worldName = world.name;
                int warps = world.game.GetStorySession.saveState.miscWorldSaveData.numberOfWarpPointsGenerated;
                var discoveredPoints = new Dictionary<string, string>(world.game.GetStorySession.saveState.miscWorldSaveData.discoveredWarpPoints);
                List<string> roomsSealedByVoidWeaverCopy = [.. world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByVoidWeaver];
                List<string> roomsSealedByWeaverAbilityCopy = [.. world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByWeaverAbility];

                var chooseDynamicWarpTarget = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .SelectMany(x => x.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                    .Where(x => x.Name == "ChooseDynamicWarpTarget").FirstOrDefault();

                if (chooseDynamicWarpTarget == null)
                {
                    RwLogger.logger.LogInfo("ChooseDynamicWarpTarget method not found via reflection.");
                    return;
                }

                // int testIterations = 3;
                for (int _testIterations = 2; _testIterations < 7; _testIterations++)
                {
                    int testIterations = _testIterations;
                    int rippleLvl = 2;
                    int iterations = 100;

                    string text = world.name;
                    RwLogger.logger.LogInfo($"Generating warps starting in room {text}");

                    // neededList.Clear(); // temp
                    // neededList2.Clear(); // temp
                    // iterations = 10;

                    // for (int j = 1; j <= 10000; j++)
                    // {
                    //     RWCustom.Custom.rainWorld.progression.miscProgressionData.watcherCampaignSeed = j;
                    var neededList = new List<string>() {
                        "WPTA_F01", "WTDB_A17", "WTDB_A15", "WTDB_A13", "WARB_J07", "WARF_B23", "WARF_D29", "WTDA_Z08", "WTDA_Z04", "WRRA_D03", "WRRA_D05", "WSKC_A19",
                        "WVWA_F02",
                        //"WARC_C06",
                        //"WRFB_B05", "WRFB_D02",
                    };
                    var neededList3 = new List<string>() { // Fetid allowed at 8 or more ripple (7 internally here)
                        "WARC_C06",
                    };
                    var neededList2 = new List<string>() {
                        "WBLA_C02", "WBLA_F04",
                        //"WARC_C06",
                        //"WVWA_F02",
                    };
                    var warpsToBeClosed = new List<string>()
                    {
                        "WARF_B33", "WBLA_D03", "WTDB_A26", "WARC_F01", "WVWB_A04", "WARE_I14", "WARB_J01", "WPTA_F03", "WSKC_A23", "WTDA_Z14", "WRFB_A22", "WVWA_F03"
                    };
                    var laterWarps = new List<string>()
                    {
                        "WBLA_D03", "WVWB_A04",
                        //"WARC_F01",
                        //"WVWA_F03",
                    };
                    var laterWarps2 = new List<string>()
                    {
                        //"WBLA_D03", "WVWB_A04",
                        "WARC_F01",
                        //"WVWA_F03",
                    };

                    // Track closed warps
                    HashSet<string> closedEchoWarps = [];
                    HashSet<string> closedNaturalWarps = [];
                    int totalWarpsNeeded = 19; // 12 echo + 7 natural
                    string finalPath = "";
                    int totalWeight = 0;
                    int totalDynamicWarps = 0;

                    for (int i = 0; i < iterations; i++)
                    {
                        int dynamicWarpStreak = 0; // Count consecutive dynamic warps without finding needed rooms
                        var discoveredPointsBeforeSearch = new Dictionary<string, string>(world.game.GetStorySession.saveState.miscWorldSaveData.discoveredWarpPoints);

                        // Get unsealed natural warps
                        List<string> unsealedWarps = world.game.GetStorySession.saveState.RoomsWithWarpsRemainingToBeSealed(false, "");
                        List<string> regionNaturalWarps = [];
                        string cachedText = text;
                        int cachedWorldWarpsGen = world.game.GetStorySession.saveState.miscWorldSaveData.numberOfWarpPointsGenerated;
                        string cachedWorldName = world.name;
                        string searchPath = $"{text} -> ";

                        // Check region's natural warps
                        foreach (var warp in unsealedWarps)
                        {
                            string warpRegion = warp.ToUpperInvariant().Split('_')[0];
                            if (warpRegion == text.Split('_')[0])
                            {
                                regionNaturalWarps.Add(warp);
                            }
                        }

                        var naturalWeightsForRegion = new Dictionary<string, int>();
                        foreach (var natWarp in regionNaturalWarps)
                        {
                            var natUpper = natWarp.ToUpperInvariant();
                            if (natUpper.Split('_')[0] != text.Split('_')[0].ToUpperInvariant()) continue;

                            string key = $"{text.ToUpperInvariant()}|{natUpper}";
                            // RwLogger.logger.LogInfo($"Checking natural warp weight for region key {key}");
                            if (warpToPortalWeights.TryGetValue(key, out int weight))
                            {
                                naturalWeightsForRegion[key] = weight;
                            }
                            else if (i == 0) // For now just fallback for the first warp from weaver spot
                            {
                                // If "text" dynamic warp location is after an echo or the initial weaver spot location
                                KeyValuePair<string, int> dynamicWarpLocation = warpToPortalWeights.Where(x => x.Key.StartsWith(natUpper.Split('_')[0])).FirstOrDefault();
                                if (dynamicWarpLocation.Key != null && dynamicWarpLocation.Key != "")
                                {
                                    key = $"{dynamicWarpLocation.Key.Split('|')[0]}|{natUpper}";
                                    // RwLogger.logger.LogInfo($"Checking natural warp weight for region key {key} --");
                                    if (warpToPortalWeights.TryGetValue(key, out int weight2))
                                    {
                                        naturalWeightsForRegion["-" + key] = weight2;
                                    }
                                }
                            }
                        }

                        if ((closedNaturalWarps.Count >= 3 && closedEchoWarps.Count != 12) || closedEchoWarps.Count == 12)
                        {
                            testIterations = Math.Max(5, _testIterations);
                        }
                        // else if (closedNaturalWarps.Count < 3 || closedEchoWarps.Count == 12)
                        // {
                        //     testIterations = _testIterations;
                        // }
                        for (int attempt = 0; attempt < testIterations; attempt++)
                        {
                            string chosen = (string)chooseDynamicWarpTarget.Invoke(null, [world, text, null, false, false, true]);
                            text = chosen.ToUpperInvariant();
                            world.game.GetStorySession.saveState.miscWorldSaveData.numberOfWarpPointsGenerated++;
                            world.name = text.Split('_')[0];
                            world.game.GetStorySession.saveState.miscWorldSaveData.discoveredWarpPoints[text] = "";
                            totalDynamicWarps++;

                            // Check region's natural warps
                            foreach (var warp in unsealedWarps)
                            {
                                string warpRegion = warp.ToUpperInvariant().Split('_')[0];
                                if (warpRegion == text.Split('_')[0])
                                {
                                    regionNaturalWarps.Add(warp);
                                }
                            }
                            foreach (var natWarp in regionNaturalWarps)
                            {
                                var natUpper = natWarp.ToUpperInvariant();
                                if (natUpper.Split('_')[0] != text.Split('_')[0].ToUpperInvariant()) continue;

                                string key = $"{text.ToUpperInvariant()}|{natUpper}";
                                if (warpToPortalWeights.TryGetValue(key, out int weight))
                                {
                                    naturalWeightsForRegion[key] = weight + attempt + 1;
                                }
                                else
                                {
                                    // If "text" dynamic warp location is after an echo or the initial weaver spot location
                                    // this will always be a dynamic warp location though?? so prob not needed
                                    RwLogger.logger.LogInfo($"Checking natural warp weight for region key {key} -- SOMETHING WENT WRONG");
                                    KeyValuePair<string, int> dynamicWarpLocation = warpToPortalWeights.Where(x => x.Key.StartsWith(natUpper.Split('_')[0])).FirstOrDefault();
                                    if (dynamicWarpLocation.Key != null && dynamicWarpLocation.Key != "")
                                    {
                                        key = $"{dynamicWarpLocation.Key.Split('|')[0]}|{natUpper}";
                                        RwLogger.logger.LogInfo($"Checking natural warp weight for region key {key} --");
                                        if (warpToPortalWeights.TryGetValue(key, out int weight2))
                                        {
                                            naturalWeightsForRegion[key] = weight2 + attempt + 1;
                                        }
                                    }
                                }
                            }

                            if (neededList.Contains(text) || (neededList2.Contains(text) && rippleLvl >= 8) || (neededList3.Contains(text) && rippleLvl >= 7))
                            {
                                RwLogger.logger.LogInfo($"Warp {attempt + 1}: {text} - NEEDED");

                                string warpToClose = null;
                                while (warpsToBeClosed.Any(x => text.Split('_')[0].Equals(x.Split('_')[0])
                                    && (!laterWarps.Contains(x) || (laterWarps.Contains(x) && rippleLvl >= 8))
                                    && (!laterWarps2.Contains(x) || (laterWarps2.Contains(x) && rippleLvl >= 7)))
                                    || text.StartsWith("WRRA"))
                                {
                                    if (text.StartsWith("WPTA") && !text.Equals("WPTA_F01"))
                                    {
                                        break; // Hard code not crossing all of signal for echo
                                    }
                                    warpToClose = warpsToBeClosed.Where(x => x.StartsWith(text.Split('_')[0])).FirstOrDefault();
                                    if (warpToClose == null || warpToClose == "")
                                    {
                                        warpToClose = "WRRA_A26"; // Hard code desolate natural portal
                                        closedNaturalWarps.Add(warpToClose);
                                    }
                                    else
                                    {
                                        warpsToBeClosed.Remove(warpToClose);
                                        closedEchoWarps.Add(warpToClose);
                                    }
                                    finalPath += searchPath + $"{warpToClose} echo - ";
                                    searchPath = "";
                                    neededList.RemoveAll(x => x.StartsWith(text.Split('_')[0]));
                                    neededList2.RemoveAll(x => x.StartsWith(text.Split('_')[0]));
                                    neededList3.RemoveAll(x => x.StartsWith(text.Split('_')[0]));
                                    if (text.StartsWith("WTDB"))
                                    {
                                        neededList.RemoveAll(x => x.StartsWith("WRRA")); // Remove unecessary WRRA warps if WTDB found
                                    }
                                    CloseWarps(world, warpToClose.ToLowerInvariant());
                                    world.game.GetStorySession.saveState.miscWorldSaveData.discoveredWarpPoints[warpToClose] = "";

                                    text = GetDest(warpToClose);
                                    if (text == null)
                                    {
                                        RwLogger.logger.LogInfo($"Error getting destination for closed warp {warpToClose}");
                                        return;
                                    }
                                    world.name = text.Split('_')[0];
                                    if (!warpToClose.StartsWith("WTDA") && !warpToClose.StartsWith("WARF") && !warpToClose.StartsWith("WRRA"))
                                    {
                                        rippleLvl++;
                                    }
                                }

                                if (warpsToBeClosed.Count == 0)
                                {
                                    RwLogger.logger.LogInfo($"All echo warps closed. Echo: {closedEchoWarps.Count}, Natural: {closedNaturalWarps.Count}");

                                    // Need to close remaining natural warps
                                    int naturalWarpsNeeded = 7 - closedNaturalWarps.Count;
                                    if (naturalWarpsNeeded > 0)
                                    {
                                        RwLogger.logger.LogInfo($"Need to close {naturalWarpsNeeded} more natural warps");
                                    }
                                    else
                                    {
                                        RwLogger.logger.LogInfo($"All warps closed! Total: {closedEchoWarps.Count + closedNaturalWarps.Count}");
                                        break;
                                    }
                                }

                                // if (warpToClose != null && warpToClose.Split('_')[0] == "WARB" && i > 20)
                                // {
                                //     world.game.GetStorySession.saveState.miscWorldSaveData.discoveredWarpPoints = new Dictionary<string, string>(discoveredPoints);
                                //     RwLogger.logger.LogInfo($"Resetting discovered warp points to clear cache.");
                                // }
                                RwLogger.logger.LogInfo($"Generating warps from {text}");
                                break;
                            }
                            else
                            {
                                dynamicWarpStreak++;
                                RwLogger.logger.LogInfo($"Warp {attempt + 1}: {text}");
                                searchPath += $"{text} -> ";
                            }
                        }

                        // If no good dynamic warps found for testIterations iterations, try natural warps
                        if (dynamicWarpStreak >= testIterations)
                        {
                            RwLogger.logger.LogInfo($"No needed warps found in {dynamicWarpStreak} iterations. Checking natural warps...");

                            if (regionNaturalWarps.Count > 0)
                            {
                                // Determine if we should close a natural warp
                                int naturalWarpsStillNeeded = 7 - closedNaturalWarps.Count;

                                // Only close natural warp if:
                                // 1. We need more natural warps closed
                                // 2. It's not the last natural warp (save that for the finale)
                                if (naturalWarpsStillNeeded > 1 || (naturalWarpsStillNeeded == 1 && warpsToBeClosed.Count == 0))
                                {
                                    KeyValuePair<string, int> natWarp = naturalWeightsForRegion.OrderBy(x => x.Value).FirstOrDefault();
                                    string naturalWarpToClose = natWarp.Key.Split('|')[1].ToUpperInvariant();
                                    // Dynamic warp to the natural warp region
                                    text = cachedText;
                                    world.game.GetStorySession.saveState.miscWorldSaveData.numberOfWarpPointsGenerated = cachedWorldWarpsGen;
                                    world.name = cachedWorldName;
                                    world.game.GetStorySession.saveState.miscWorldSaveData.discoveredWarpPoints = new Dictionary<string, string>(discoveredPointsBeforeSearch);
                                    searchPath = $"{text} -> ";
                                    totalDynamicWarps -= testIterations;
                                    RwLogger.logger.LogInfo($"Natural warps found in region. Warping to region {text.Split('_')[0]} to close warp.");
                                    // if (naturalWarpToClose.Split('_')[0] == text.Split('_')[0])
                                    // {
                                    //     RwLogger.logger.LogInfo($"#Something went wrong.#");
                                    // }
                                    while (!text.Equals(natWarp.Key.Split('|')[0]) && !natWarp.Key.Split('|')[0].StartsWith("-"))
                                    {
                                        string chosen = (string)chooseDynamicWarpTarget.Invoke(null, [world, text, null, false, false, true]);
                                        text = chosen.ToUpperInvariant();
                                        world.game.GetStorySession.saveState.miscWorldSaveData.numberOfWarpPointsGenerated++;
                                        world.name = text.Split('_')[0];
                                        world.game.GetStorySession.saveState.miscWorldSaveData.discoveredWarpPoints[text] = "";
                                        RwLogger.logger.LogInfo($"Warping to {text}...");
                                        searchPath += $"{text} -> ";
                                        totalDynamicWarps++;
                                    }
                                    RwLogger.logger.LogInfo($"Using natural warp: {naturalWarpToClose} with weight {natWarp.Value}");

                                    closedNaturalWarps.Add(naturalWarpToClose);
                                    CloseWarps(world, naturalWarpToClose.ToLowerInvariant());
                                    world.game.GetStorySession.saveState.miscWorldSaveData.discoveredWarpPoints[naturalWarpToClose] = "";

                                    text = GetDest(naturalWarpToClose);
                                    if (text != null)
                                    {
                                        world.name = text.Split('_')[0];
                                        RwLogger.logger.LogInfo($"Natural warp closed. Moving to {text}. Echo: {closedEchoWarps.Count}, Natural: {closedNaturalWarps.Count}");
                                        finalPath += searchPath.Substring(0, searchPath.Length - 3) + "- " + $"{naturalWarpToClose}|{text}({natWarp.Value}) - ";
                                        totalWeight += natWarp.Value;
                                        dynamicWarpStreak = 0;
                                    }
                                }
                                else
                                {
                                    RwLogger.logger.LogInfo($"Skipping natural warp closure (saving for finale or already completed)");
                                    dynamicWarpStreak = 0; // Reset to continue with dynamic warps

                                    // Exclude last dynamic warp (it is added again at beginning of this loop)
                                    string[] warpStrings = searchPath.Split([" -> "], StringSplitOptions.RemoveEmptyEntries);
                                    string strippedSearchPath = "";
                                    for (int k = 0; k < warpStrings.Length - 1; k++)
                                    {
                                        strippedSearchPath += warpStrings[k] + " -> ";
                                    }
                                    finalPath += strippedSearchPath;
                                }
                            }
                            else
                            {
                                RwLogger.logger.LogInfo($"No natural warps found in regions, ending search.");
                                break;
                            }
                        }

                        // Check completion
                        if (closedEchoWarps.Count + closedNaturalWarps.Count >= totalWarpsNeeded)
                        {
                            RwLogger.logger.LogInfo($"SUCCESS! All {totalWarpsNeeded} warps closed at iteration {i + 1}");
                            RwLogger.logger.LogInfo($"Echo warps: {closedEchoWarps.Count}, Natural warps: {closedNaturalWarps.Count}");
                            RwLogger.logger.LogInfo($"Final path: {finalPath}");
                            RwLogger.logger.LogInfo($"Total dynamic warps: {totalDynamicWarps}, Total nat warp weight: {totalWeight}");
                            RwLogger.logger.LogInfo($"Total route score: {totalDynamicWarps + totalWeight}");
                            break;
                        }
                    }
                    // if (warpsToBeClosed.Count != 0)
                    // {
                    //     RwLogger.logger.LogInfo($"{j} Failed to find all warps");
                    // }

                    //     world.game.GetStorySession.saveState.miscWorldSaveData.numberOfWarpPointsGenerated = warps;
                    //     world.name = worldName;
                    //     world.game.GetStorySession.saveState.miscWorldSaveData.discoveredWarpPoints = new Dictionary<string, string>(discoveredPoints);
                    //     world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByVoidWeaver = [.. roomsSealedByVoidWeaverCopy];
                    //     world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByWeaverAbility = [.. roomsSealedByWeaverAbilityCopy];
                    // }

                    // var neededList = new List<string>() {
                    //         "WPTA_F01", "WTDB_A17", "WTDB_A15", "WTDB_A13", "WARB_J07", "WARF_B23", "WARF_D29", "WTDA_Z08", "WTDA_Z04", // "WRRA_D03", "WRRA_D05"
                    //     };
                    // var neededList2 = new List<string>() {
                    //         "WBLA_C02", "WBLA_C04", "WARC_C06", "WVWA_F02"
                    //     };
                    // var warpsToBeClosed = new List<string>()
                    //     {
                    //         "WARF_B33", "WBLA_D03", "WTDB_A26", "WARC_F01", "WVWB_A04", "WARE_I14", "WARB_J01", "WPTA_F03", "WSKC_A23", "WTDA_Z14", "WRFB_A22", "WVWA_F03"
                    //     };
                    // var laterWarps = new List<string>()
                    //     {
                    //         "WBLA_D03", "WARC_F01", "WVWB_A04", "WVWA_F03"
                    //     };
                    // for (int i = 0; i < iterations; i++)
                    // {
                    //     string chosen = (string)chooseDynamicWarpTarget.Invoke(null, [world, text, null, false, false, true]);
                    //     text = chosen.ToUpperInvariant();
                    //     world.game.GetStorySession.saveState.miscWorldSaveData.numberOfWarpPointsGenerated++;
                    //     world.name = text.Split('_')[0];
                    //     world.game.GetStorySession.saveState.miscWorldSaveData.discoveredWarpPoints[text] = "";
                    //     RwLogger.logger.LogInfo($"Warp {i + 1}: {text}");
                    // }

                    // Reset world state cus var is a reference
                    world.game.GetStorySession.saveState.miscWorldSaveData.numberOfWarpPointsGenerated = warps;
                    world.name = worldName;
                    world.game.GetStorySession.saveState.miscWorldSaveData.discoveredWarpPoints = new Dictionary<string, string>(discoveredPoints);
                    world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByVoidWeaver = [.. roomsSealedByVoidWeaverCopy];
                    world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByWeaverAbility = [.. roomsSealedByWeaverAbilityCopy];
                    RwLogger.logger.LogInfo("---------------------------------------------------------------------------------------------------------------------");
                }
            });
        }
    }

    // I hope this is pass by reference (c#....)
    public void CloseWarps(World world, string room)
    {
        // Watcher seals
        RwLogger.logger.LogInfo($"Watcher closing warps: ");
        if (!world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByWeaverAbility.Contains(room))
        {
            world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByWeaverAbility.Add(room);
            RwLogger.logger.LogInfo($"{room}");
        }
        string destRoom = GetDest(room);
        if (destRoom != null && !world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByWeaverAbility.Contains(destRoom.ToLowerInvariant()))
        {
            world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByWeaverAbility.Add(destRoom.ToLowerInvariant());
            RwLogger.logger.LogInfo($"{destRoom.ToLowerInvariant()}");
        }

        // Weaver seals
        List<string> list = world.game.GetStorySession.saveState.RoomsWithWarpsRemainingToBeSealed(RWCustom.Custom.rainWorld.progression.miscProgressionData.beaten_Watcher_SpinningTop, "");
        if (list.Count > 2)
        {
            list.Sort();
            UnityEngine.Random.State state = UnityEngine.Random.state;
            UnityEngine.Random.InitState(RWCustom.Custom.rainWorld.progression.miscProgressionData.watcherCampaignSeed + list.Count);
            string closeWarp = list[UnityEngine.Random.Range(0, list.Count)].ToLowerInvariant();
            UnityEngine.Random.state = state;
            // Search discovered/spawned warps?
            string closeWarpDest = GetDest(closeWarp);
            RwLogger.logger.LogInfo($"Weaver closing warps: ");
            if (!world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByVoidWeaver.Contains(closeWarp))
            {
                world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByVoidWeaver.Add(closeWarp);
                RwLogger.logger.LogInfo($"{closeWarp}");
            }
            // Always assume closeWarpDest is not null?
            if (!world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByVoidWeaver.Contains(closeWarpDest.ToLowerInvariant()))
            {
                world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByVoidWeaver.Add(closeWarpDest.ToLowerInvariant());
                RwLogger.logger.LogInfo($"{closeWarpDest.ToLowerInvariant()}");
            }
            string[] array3 = ["ward_r10", "wssr_cramped", "wssr_lab6"];
            for (int m = 0; m < array3.Length; m++)
            {
                if (!world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByVoidWeaver.Contains(array3[m]) && !world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByWeaverAbility.Contains(array3[m]))
                {
                    world.game.GetStorySession.saveState.miscWorldSaveData.roomsSealedByVoidWeaver.Add(array3[m]);
                    RwLogger.logger.LogInfo($"{array3[m]}");
                }
            }
        }
    }

    // Won't look in discovered/spawned warps (does that matter?)
    // Returns room in UPPERCASE
    public string GetDest(string room)
    {
        string text2 = null;
        string text = room.ToLowerInvariant();
        foreach (KeyValuePair<string, List<string>> keyValuePair3 in RWCustom.Custom.rainWorld.regionWarpRooms)
        {
            for (int k = 0; k < keyValuePair3.Value.Count; k++)
            {
                string[] array = keyValuePair3.Value[k].Split(':');
                if (array[0].ToLowerInvariant() == text && array.Length > 3)
                {
                    text2 = array[3];
                    break;
                }
            }
            if (text2 != null)
            {
                break;
            }
        }
        if (text2 == null) // Search ST warps if not found
        {
            foreach (KeyValuePair<string, List<string>> keyValuePair4 in RWCustom.Custom.rainWorld.regionSpinningTopRooms)
            {
                for (int l = 0; l < keyValuePair4.Value.Count; l++)
                {
                    string[] array2 = keyValuePair4.Value[l].Split(':');
                    if (array2[0].ToLowerInvariant() == text && array2.Length > 2)
                    {
                        text2 = array2[2];
                        break;
                    }
                }
                if (text2 != null)
                {
                    break;
                }
            }
        }
        return text2?.ToUpperInvariant();
    }
}