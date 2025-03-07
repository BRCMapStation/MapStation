using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Reptile;
using UnityEngine.SceneManagement;
using Unity.AI.Navigation;
using Object = UnityEngine.Object;
using System.Drawing;
using MapStation.Common.Runtime;

namespace MapStation.Common.Doctor {
    /// <summary>
    /// Analyze a GameObject hierarchy for potential problems.
    /// </summary>
    public static class Doctor {
        public const string AboutMe = "The Doctor will analyze your map and list any problems, offering suggestions to fix them.";

        public static Analysis Analyze(MapProperties mapProperties) {
            var roots = SceneManager.GetActiveScene().GetRootGameObjects();
            return Analyze(roots, mapProperties);
        }

        public static Analysis Analyze(GameObject root, MapProperties mapProperties) {
            return Analyze(new[] { root }, mapProperties);
        }

        private static List<T> GetComponentsInChildren<T>(this GameObject[] roots) {
            List<T> results = new();
            foreach (var root in roots) {
                results.AddRange(root.GetComponentsInChildren<T>(true));
            }
            return results;
        }

        private static void CheckMapOptions(Analysis a, GameObject[] roots) {
            var activeOnMapOptions = roots.GetComponentsInChildren<ActiveOnMapOption>();
            var mapOptions = roots.GetComponentsInChildren<MapOptions>();
            if (mapOptions.Count > 1) {
                a.Add(Severity.Warning, null, "Multiple Map Options", "Multiple Map Options components are in the map. Only 1 is allowed.");
            }
            foreach(var mapOptionsO in mapOptions) {
                var err = mapOptionsO.GetError();
                if (err != null)
                    a.Add(Severity.Warning, mapOptionsO, err);
            }
            foreach(var mapOption in activeOnMapOptions) {
                var err = mapOption.GetError();
                if (err != ActiveOnMapOption.Errors.NoError)
                    a.Add(Severity.Warning, mapOption, ActiveOnMapOption.GetErrorString(err), ActiveOnMapOption.GetErrorDetails(err));
            }
        }

        private static void CheckChunkStuff(Analysis a, GameObject[] roots) {
            var chunkItems = new List<MonoBehaviour>();
            chunkItems.AddRange(roots.GetComponentsInChildren<Junk>());
            chunkItems.AddRange(roots.GetComponentsInChildren<StreetLifeCluster>());
            foreach (var chunkItem in chunkItems) {
                var chunk = chunkItem.GetComponentInParent<StageChunk>();
                if (chunk == null)
                    a.Add(Severity.Warning, chunkItem, "Component not in Chunk", "This component is not inside of a Stage Chunk. It might not work fully.");
            }
        }

        private static void CheckStreetLife(Analysis a, GameObject[] roots) {
            var peds = roots.GetComponentsInChildren<Pedestrian>();
            foreach (var ped in peds) {
                var cluster = ped.GetComponentInParent<StreetLifeCluster>(true);
                if (cluster == null)
                    a.Add(Severity.Warning, ped, "Pedestrian not in StreetLifeCluster", "Pedestrian is not in a StreetLifeCluster, may not function properly.");
            }
        }

        public static Analysis Analyze(GameObject[] roots, MapProperties mapProperties) {
            var a = new Analysis();

            var cameraOverrides = roots.GetComponentsInChildren<MapStationCameraOverride>();
            if (cameraOverrides.Count > 1) {
                a.Add(Severity.Warning, null, "Multiple Camera Overrides", "There are multiple Camera Overrides. Only one is supported.");
            }

            var playerSpawners = roots.GetComponentsInChildren<PlayerSpawner>();
            var hasDefaultSpawner = false;
            foreach (var spawner in playerSpawners) {
                if (spawner.isDefaultSpawnPoint) {
                    hasDefaultSpawner = true;
                    break;
                }
            }

            if (!hasDefaultSpawner)
                a.Add(Severity.Warning, null, "Missing Default Player Spawn Point", "Map is missing a default player spawn point. Player will spawn at 0,0,0.");

            CheckChunkStuff(a, roots);
            CheckStreetLife(a, roots);
            CheckMapOptions(a, roots);

            var spawners = roots.GetComponentsInChildren<BasicEnemySpawner>();

            var hasHumanoidCops = false;
            var hasCopters = false;
            var hasTankwalkers = false;

            foreach (var spawner in spawners) {
                if (spawner.spawnableEnemies == null) continue;
                foreach (var enemy in spawner.spawnableEnemies) {
                    switch (enemy.name) {
                        case "ShieldCop":
                        case "SniperCop":
                        case "BasicCop":
                            hasHumanoidCops = true;
                            break;
                        case "CopterCop":
                            hasCopters = true;
                            break;
                        case "tankWalker":
                            hasTankwalkers = true;
                            break;
                    }
                }
            }

            var navSurfaces = roots.GetComponentsInChildren<NavMeshSurface>();

            var hasHumanoidNav = false;
            var hasCopterNav = false;
            var hasTankwalkerNav = false;

            foreach (var navSurface in navSurfaces) {
                if (navSurface.navMeshData == null) continue;
                switch (navSurface.agentTypeID) {
                    case 0:
                        hasHumanoidNav = true;
                        break;
                    case -1372625422:
                        hasTankwalkerNav = true;
                        break;
                    case -334000983:
                        hasCopterNav = true;
                        break;
                }
            }

            if (spawners.Count == 0 && !mapProperties.disableCops)
                a.Add(Severity.Warning, null, "Missing Cop Spawners", "Map has cops enabled, but there are no spawners for them.");

            if (hasHumanoidCops && !hasHumanoidNav) {
                a.Add(Severity.Warning, null, "Missing NavMeshSurface for Cops", "Map has cops but no NavMeshSurface for them. They will not be able to navigate.");
            }

            if (hasCopters && !hasCopterNav) {
                a.Add(Severity.Warning, null, "Missing NavMeshSurface for Cop Helicopters", "Map has helicopters but no NavMeshSurface for them. They will not be able to navigate.");
            }

            if (hasTankwalkers && !hasTankwalkerNav) {
                a.Add(Severity.Warning, null, "Missing NavMeshSurface for Tankwalkers", "Map has tankwalkers but no NavMeshSurface for them. They will not be able to navigate.");
            }

            foreach (var GraffitiSpot in roots.GetComponentsInChildren<GraffitiSpot>()) {
#if BEPINEX
                // Commonly attached w/VanillaAssetReference, so test only at runtime.
                if (GraffitiSpot.dynamicRepPickup == null) {
                    a.Add(Severity.Error, GraffitiSpot, "Graffiti missing dynamicRepPickup", "Found GraffitiSpot.dynamicRepPickup == null. This will soft-lock when tagged.");
                }
#endif
                if (GraffitiSpot.tag != Tags.GraffitiSpot) {
                    a.Add(Severity.Warning, GraffitiSpot, "GraffitiSpot has wrong tag", $"Found GraffitiSpot not tagged as 'GraffitiSpot'");
                }
                if (GraffitiSpot.gameObject.layer != Layers.TriggerDetectPlayer) {
                    a.Add(Severity.Warning, GraffitiSpot, "GraffitiSpot has wrong layer", $"Found GraffitiSpot not on the 'TriggerDetectPlayer' layer.");
                }
            }

            var progressables = roots.GetComponentsInChildren<AProgressable>();
            foreach (var progressable in progressables) {
#if BEPINEX
                // BoE HACK: Ignore workaround NPC.
                // Ignore cypher dummy npcs.
                if (progressable.gameObject.name == "DummyNPC" && progressable is NPC)
                    continue;
#endif
                const string uidRegexp = @"^[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}$";
                if (!Regex.Match(progressable.uid, uidRegexp, RegexOptions.None).Success) {
                    a.Add(Severity.Warning, progressable, "Bad Uid",
                          $"Found {progressable.GetType().Name}.uid which is not in expected UID format (all lowercase, numbers and letters a-f, correct length, correct hyphens) UID={progressable.uid}"
                    );
                }
            }
            foreach (var group in progressables.GroupBy(p => p.uid)) {
                if (group.Count() > 1) {
                    // TODO support diagnostics w/multiple targets so all duplicates can be reported as one
                    foreach (var progressable in group) {
                        a.Add(Severity.Error, progressable, "Duplicate Uid",
                            $"Found {progressable.GetType().Name}.uid which is identical to one or more other progressables in this map. This may crash the game when the map loads. Ensure all UIDs are unique. UID={progressable.uid}"
                            );
                    }
                }
            }

            foreach (var VendingMachine in roots.GetComponentsInChildren<VendingMachine>()) {
                if (VendingMachine.gameObject.layer != Layers.Enemies) {
                    a.Add(Severity.Warning, VendingMachine, "VendingMachine has wrong layer",
                        $"Found VendingMachine that is not on the Enemies layer, this means it cannot be kicked.");
                }
                var animation = VendingMachine.GetComponent<Animation>();
                if (animation == null) {
                    a.Add(Severity.Warning, VendingMachine, "VendingMachine missing Animation component", $"Found VendingMachine without an Animation component.");
                }
                foreach (var animName in VendingMachineAnimations) {
#if BEPINEX
                    // Commonly attached w/VanillaAssetReference, so test only at runtime.
                    // Also, editor says `GetState` method doesn't exist.
                    if (animation.GetState(animName) == null) {
                        a.Add(Severity.Warning, VendingMachine, "VendingMachine missing animations",
                            $"Found VendingMachine with Animation component missing animation: {animName}. This will fail with errors when kicked.");
                    }
#endif
                }
            }

            foreach (var Teleport in roots.GetComponentsInChildren<Teleport>()) {
                if (Teleport.teleportTo == null && !Teleport.automaticallyReturnPlayerToLastSafeLocation) {
                    a.Add(Severity.Warning, Teleport, $"Found Teleport missing a `teleportTo` destination.");
                }
                var collider = Teleport.GetComponentInChildren<BoxCollider>();
                if (collider == null) {
                    a.Add(Severity.Warning, Teleport, $"Found Teleport without a Box Collider on a child GameObject.");
                } else {
                    if (collider.tag != Tags.Teleport) {
                        a.Add(Severity.Warning, Teleport, $"Found Teleporter's child collider not tagged as 'Teleport'");
                    }
                    if (collider.gameObject.layer != Layers.TriggerDetectPlayer) {
                        a.Add(Severity.Warning, Teleport, $"Found Teleport's child collider not on the 'TriggerDetectPlayer' layer.");
                    }
                }
            }

            // Should have exactly one sun
            var suns = roots.GetComponentsInChildren<SunFlareGPU>();
            if (suns.Count == 0) {
                a.Add(Severity.Error, null, "Missing Sun", "Map is missing a sun, will crash on startup. Try adding Sun prefab from the right-click menu.");
            }
            if (suns.Count > 1) {
                foreach (var sun in suns) {
                    a.Add(Severity.Warning, sun, "Multiple Suns", "Map has multiple suns, lighting will be too bright. Try deleting all but one.");
                }
            }

            // Detect orphaned or empty Grind components
            var grindNodes = roots.GetComponentsInChildren<GrindNode>();
            foreach (var grindNode in grindNodes) {
                if (grindNode.grindLines.Find(l => l != null) == null) {
                    a.Add(Severity.Warning, grindNode, "Unattached Grind Node", "Grind Node is not attached to any grind lines, should probably be deleted.");
                }
            }
            var grindLines = roots.GetComponentsInChildren<GrindLine>();
            foreach (var grindLine in grindLines) {
                if (grindLine.n0 == null || grindLine.n1 == null) {
                    a.Add(Severity.Warning, grindLine, "Unattached Grind Line", "Grind Line is not attached to two Grind Nodes, should probably be deleted.");
                }
            }

            return a;
        }

        private static readonly string[] VendingMachineAnimations = new[] { "none", "shake", "emptyShake", "close", "drop" };

        public static string GetGameObjectPath(GameObject obj) {
            string path = "/" + obj.name;
            while (obj.transform.parent != null) {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }
    }

    public class Analysis {
        public readonly Dictionary<GameObject, List<Diagnostic>> gameObjects = new();
        public readonly List<Diagnostic> diagnostics = new();
        public readonly List<Diagnostic> diagnosticsWithoutTarget = new();
        public readonly Dictionary<Severity, int> countBySeverity = new();

        public Analysis() {
            foreach (var s in Enum.GetValues(typeof(Severity))) {
                countBySeverity.Add((Severity) s, 0);
            }
        }

        public void Add(Severity severity, Object target, string message, string details = null) {
            var diagnostic = new Diagnostic(severity, target, message, details);
            diagnostics.Add(diagnostic);
            countBySeverity[diagnostic.Severity]++;
            if (diagnostic.GameObject != null) {
                gameObjects.TryGetValue(diagnostic.GameObject, out var goList);
                if (goList == null) {
                    goList = new();
                    gameObjects.Add(diagnostic.GameObject, goList);
                }
                goList.Add(diagnostic);
            } else {
                diagnosticsWithoutTarget.Add(diagnostic);
            }
        }

        public void Log() {
            Debug.Log($"MapStation: Analysis found {diagnostics.Count} problems.");
            foreach (var diagnostic in diagnostics) {
                var path = diagnostic.TargetPath;
                var suffix = "";
                if (path != null) suffix = $" (at {path})";
                Debug.Log(diagnostic.Message + suffix);
            }
        }
    }

    public class Diagnostic {
        public Diagnostic(Severity severity, Object target, string message, string details = null) {
            GameObject go = target as GameObject;
            Component c = target as Component;
            if (c != null) go = c.gameObject;
            // if (c == null && go == null) throw new Exception("Not supported: target must be gameobject or component on a gameobject");
            Message = message;
            Details = details;
            Target = target;
            GameObject = go;
            Severity = severity;
            Component = c;
            TargetPath = GameObject == null ? null : Doctor.GetGameObjectPath(GameObject);
        }

        /// <summary>
        /// Error message briefly explaining the problem.
        /// </summary>
        public readonly string Message;
        /// <summary>
        /// Additional description, shown in UI only when you expand a dropdown or similar.
        /// </summary>
        public readonly string Details;
        /// <summary>
        /// GameObject or Component exhibiting the problem. May be clickable in UI to navigate to the object.
        /// </summary>
        public readonly Object Target;
        /// <summary>
        /// Component exhibiting the problem. May be same as Target, or null if Target is not a Component.
        /// </summary>
        public readonly Component Component;
        /// <summary>
        /// GameObject exhibiting the problem. May be same as Target, or may be target's GameObject.
        /// </summary>
        public readonly GameObject GameObject;
        /// <summary>
        /// Diagnostic severity.
        /// </summary>
        public readonly Severity Severity;

        public string TargetPath;

    }
    public enum Severity {
        /// <summary>
        /// Errors prevent the author from building their map, so these should be severe enough to cause a crash.
        /// </summary>
        Error,
        Warning
    }
}
