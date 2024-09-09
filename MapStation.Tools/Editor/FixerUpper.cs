using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MapStation.Tools.Editor {

    /// <summary>
    /// Upgrades project settings and other assets that we can't update via packages.
    /// </summary>
    public static class FixerUpper {
        private const int CurrentVersion = 0;
        private const string CachePath = "fixupcache.dat";

        [InitializeOnLoadMethod]
        public static void CheckFixUp() {
            if (!File.Exists(CachePath))
                FixUp(-1);
            else {
                var textfile = File.ReadAllText(CachePath);
                if (int.TryParse(textfile, out var version))
                    FixUp(version);
                else
                    FixUp(-1);
            }
            File.WriteAllText(CachePath, CurrentVersion.ToString());
        }

        private static void FixUp(int fromVersion) {
            if (fromVersion >= CurrentVersion) return;
            if (fromVersion < 0)
                FixUpVersion0();
            Debug.Log($"[{nameof(FixerUpper)}] MapStation assets have been upgraded. {fromVersion} -> {CurrentVersion}");
            AssetDatabase.Refresh();
        }

        private static void FixUpVersion0() {
            // THIS IS ASSSSSSS but hey we can change it anytime.
            File.WriteAllText("ProjectSettings/NavMeshAreas.asset", "%YAML 1.1\n%TAG !u! tag:unity3d.com,2011:\n--- !u!126 &1\nNavMeshProjectSettings:\n  serializedVersion: 2\n  m_ObjectHideFlags: 0\n  areas:\n  - name: Walkable\n    cost: 1\n  - name: Not Walkable\n    cost: 1\n  - name: Jump\n    cost: 5\n  - name: Tankwalker\n    cost: 1\n  - name: Copter\n    cost: 1\n  - name: Boss\n    cost: 1\n  - name: SniperLedge\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  - name:\n    cost: 1\n  m_LastAgentTypeID: -902729914\n  m_Settings:\n  - serializedVersion: 2\n    agentTypeID: 0\n    agentRadius: 0.4\n    agentHeight: 2\n    agentSlope: 45\n    agentClimb: 0.3\n    ledgeDropHeight: 0\n    maxJumpAcrossDistance: 0\n    minRegionArea: 2\n    manualCellSize: 0\n    cellSize: 0.16666667\n    manualTileSize: 0\n    tileSize: 256\n    accuratePlacement: 0\n    maxJobWorkers: 0\n    preserveTilesOutsideBounds: 0\n    debug:\n      m_Flags: 0\n  - serializedVersion: 2\n    agentTypeID: -1372625422\n    agentRadius: 5\n    agentHeight: 14\n    agentSlope: 25.8\n    agentClimb: 1\n    ledgeDropHeight: 0\n    maxJumpAcrossDistance: 0\n    minRegionArea: 2\n    manualCellSize: 0\n    cellSize: 0.16666667\n    manualTileSize: 0\n    tileSize: 256\n    accuratePlacement: 0\n    maxJobWorkers: 0\n    preserveTilesOutsideBounds: 0\n    debug:\n      m_Flags: 0\n  - serializedVersion: 2\n    agentTypeID: -334000983\n    agentRadius: 3\n    agentHeight: 5\n    agentSlope: 46\n    agentClimb: 2.2\n    ledgeDropHeight: 0\n    maxJumpAcrossDistance: 0\n    minRegionArea: 2\n    manualCellSize: 0\n    cellSize: 0.16666667\n    manualTileSize: 0\n    tileSize: 256\n    accuratePlacement: 0\n    maxJobWorkers: 0\n    preserveTilesOutsideBounds: 0\n    debug:\n      m_Flags: 0\n  - serializedVersion: 2\n    agentTypeID: 1479372276\n    agentRadius: 1\n    agentHeight: 2\n    agentSlope: 53.5\n    agentClimb: 1.65\n    ledgeDropHeight: 0\n    maxJumpAcrossDistance: 0\n    minRegionArea: 2\n    manualCellSize: 0\n    cellSize: 0.16666667\n    manualTileSize: 0\n    tileSize: 256\n    accuratePlacement: 0\n    maxJobWorkers: 0\n    preserveTilesOutsideBounds: 0\n    debug:\n      m_Flags: 0\n  - serializedVersion: 2\n    agentTypeID: -1923039037\n    agentRadius: 1.5\n    agentHeight: 3\n    agentSlope: 35\n    agentClimb: 1\n    ledgeDropHeight: 0\n    maxJumpAcrossDistance: 0\n    minRegionArea: 2\n    manualCellSize: 0\n    cellSize: 0.16666667\n    manualTileSize: 0\n    tileSize: 256\n    accuratePlacement: 0\n    maxJobWorkers: 0\n    preserveTilesOutsideBounds: 0\n    debug:\n      m_Flags: 0\n  - serializedVersion: 2\n    agentTypeID: -902729914\n    agentRadius: 0.5\n    agentHeight: 2\n    agentSlope: 10.92\n    agentClimb: 0.4\n    ledgeDropHeight: 0\n    maxJumpAcrossDistance: 0\n    minRegionArea: 2\n    manualCellSize: 0\n    cellSize: 0.16666667\n    manualTileSize: 0\n    tileSize: 256\n    accuratePlacement: 0\n    maxJobWorkers: 0\n    preserveTilesOutsideBounds: 0\n    debug:\n      m_Flags: 0\n  m_SettingNames:\n  - Humanoid\n  - TankWalker\n  - Copter\n  - Boss\n  - DJBoss\n  - HumanoidNoStairs\n");
        }
    }
}
