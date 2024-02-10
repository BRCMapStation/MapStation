using Reptile;
using UnityEngine;

namespace MapStation.Plugin;

public class CrashDetector {
    private const string PlayerPrefName = "MapStation.CrashDetector.IsLoadingStage";
    public static bool ShouldAvoidReCrashing { get; private set; }
    
    public static void InitOnGameStart() {
        var wasLoadingStage = PlayerPrefs.GetInt(PlayerPrefName);
        ShouldAvoidReCrashing = wasLoadingStage > 0;
        StageManager.OnStagePostInitialization += AfterLoadStage;
    }
    
    public static void BeforeLoadStage() {
        PlayerPrefs.SetInt(PlayerPrefName, 1);
    }
    
    public static void AfterLoadStage() {
        PlayerPrefs.SetInt(PlayerPrefName, 0);
        ShouldAvoidReCrashing = false;
    }   
}
