using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Reptile;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace MapStation.Common.Doctor {
    /// <summary>
    /// Analyze a GameObject hierarchy for potential problems.
    /// </summary>
    public static class Doctor {
        public const string AboutMe = "The Doctor will analyze your map and list any problems, offering suggestions to fix them.";
        
        public static Analysis Analyze() {
            var roots = SceneManager.GetActiveScene().GetRootGameObjects();
            return Analyze(roots);
        }

        public static Analysis Analyze(GameObject root) {
            return Analyze(new[] {root});
        }

            private static List<T> GetComponentsInChildren<T>(this GameObject[] roots) {
                List<T> results = new ();
                foreach (var root in roots) {
                    results.AddRange(root.GetComponentsInChildren<T>());
                }
                return results;
            }

        public static Analysis Analyze(GameObject[] roots) {
            var a = new Analysis();

            foreach (var GraffitiSpot in roots.GetComponentsInChildren<GraffitiSpot>()) {
#if BEPINEX
                // Commonly attached w/VanillaAssetReference, so test only at runtime.
                if (GraffitiSpot.dynamicRepPickup == null) {
                    a.Add(GraffitiSpot, "Graffiti missing dynamicRepPickup", "Found GraffitiSpot.dynamicRepPickup == null. This will soft-lock when tagged.");
                }
#endif
                if (GraffitiSpot.tag != Tags.GraffitiSpot) {
                    a.Add(GraffitiSpot, "GraffitiSpot has wrong tag", $"Found GraffitiSpot not tagged as 'GraffitiSpot'");
                }
                if (GraffitiSpot.gameObject.layer != Layers.TriggerDetectPlayer) {
                    a.Add(GraffitiSpot, "GraffitiSpot has wrong layer", $"Found GraffitiSpot not on the 'TriggerDetectPlayer' layer.");
                }
            }

            foreach (var AProgressable in roots.GetComponentsInChildren<AProgressable>()) {
                const string uidRegexp = @"^[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}$";
                if (!Regex.Match(AProgressable.uid, uidRegexp, RegexOptions.None).Success) {
                    a.Add(AProgressable, "Bad Uid", String.Format(
                                     $"Found {AProgressable.GetType().Name}.uid which is not in expected UID format (all lowercase, numbers and letters a-f, correct length, correct hyphens) UID={0}",
                                     AProgressable.uid));
                }
            }

            foreach (var VendingMachine in roots.GetComponentsInChildren<VendingMachine>()) {
                if (VendingMachine.gameObject.layer != Layers.Enemies) {
                    a.Add(VendingMachine, "VendingMachine has wrong layer",
                        $"Found VendingMachine that is not on the Enemies layer, this means it cannot be kicked.");
                }
                var animation = VendingMachine.GetComponent<Animation>();
                if (animation == null) {
                    a.Add(VendingMachine, "VendingMachine missing Animation component", $"Found VendingMachine without an Animation component.");
                }
                foreach (var animName in VendingMachineAnimations) {
#if BEPINEX
                    // Commonly attached w/VanillaAssetReference, so test only at runtime.
                    // Also, editor says `GetState` method doesn't exist.
                    if (animation.GetState(animName) == null) {
                        a.Add(VendingMachine, "VendingMachine missing animations",
                            $"Found VendingMachine with Animation component missing animation: {animName}. This will fail with errors when kicked.");
                    }
#endif
                }
            }

            foreach (var Teleport in roots.GetComponentsInChildren<Teleport>()) {
                if (Teleport.teleportTo == null) {
                    a.Add(Teleport, $"Found Teleport missing a `teleportTo` destination.");
                }
                var collider = Teleport.GetComponentInChildren<BoxCollider>();
                if (collider == null) {
                    a.Add(Teleport, $"Found Teleport without a Box Collider on a child GameObject.");
                }
                if (collider.tag != Tags.Teleport) {
                    a.Add(Teleport, $"Found Teleporter's child collider not tagged as 'Teleport'");
                }
                if (collider.gameObject.layer != Layers.TriggerDetectPlayer) {
                    a.Add(Teleport, $"Found Teleport's child collider not on the 'TriggerDetectPlayer' layer.");
                }
            }
            
            // Should have exactly one sun
            var suns = roots.GetComponentsInChildren<SunFlareGPU>();
            if (suns.Count == 0) {
                a.Add(null, "Missing Sun", "Map is missing a sun, will crash on startup. Try adding Sun prefab from the right-click menu.");
            }
            if (suns.Count > 1) {
                foreach (var sun in suns) {
                    a.Add(sun, "Multiple Suns", "Map has multiple suns, lighting will be too bright. Try deleting all but one.");
                }
            }
            
            return a;
        }

        private static readonly string[] VendingMachineAnimations = new[] {"none", "shake", "emptyShake", "close", "drop"};
        
        public static string GetGameObjectPath(GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }
    }

    public class Analysis {
        public readonly Dictionary<GameObject, List<Diagnostic>> gameObjects = new ();
        public readonly List<Diagnostic> diagnostics = new ();
        public readonly List<Diagnostic> diagnosticsWithoutTarget = new ();

        public void Add(Object target, string message, string details = null) {
            var diagnostic = new Diagnostic(target, message, details);
            diagnostics.Add(diagnostic);
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
            Debug.Log(String.Format("MapStation: Analysis found {0} problems.", diagnostics.Count));
            foreach(var diagnostic in diagnostics) {
                var path = diagnostic.TargetPath;
                var suffix = "";
                if (path != null) suffix = $" (at {path})";
                Debug.Log(diagnostic.Message + suffix);
            }
        }
        
    }

    public class Diagnostic {
        public Diagnostic(Object target, string message, string details = null) {
            GameObject go = target as GameObject;
            Component c = target as Component;
            if (c != null) go = c.gameObject;
            // if (c == null && go == null) throw new Exception("Not supported: target must be gameobject or component on a gameobject");
            Message = message;
            Details = details;
            Target = target;
            GameObject = go;
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

        public string TargetPath;
    }
}
