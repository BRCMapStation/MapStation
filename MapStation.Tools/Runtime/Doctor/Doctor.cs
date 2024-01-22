using System;
using System.Collections.Generic;
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

        public static Analysis Analyze(GameObject[] roots) {
            var a = new Analysis();

            foreach (var root in roots) {
                foreach (var GraffitiSpot in root.GetComponentsInChildren<GraffitiSpot>()) {
                    if (GraffitiSpot.dynamicRepPickup == null) {
                        a.Add(GraffitiSpot, "Found GraffitiSpot.dynamicRepPickup == null. This will soft-lock when tagged.");
                    }
                    if (GraffitiSpot.tag != Tags.GraffitiSpot) {
                        a.Add(GraffitiSpot, $"Found GraffitiSpot not tagged as 'GraffitiSpot'");
                    }
                    if (GraffitiSpot.gameObject.layer != Layers.TriggerDetectPlayer) {
                        a.Add(GraffitiSpot, $"Found GraffitiSpot not on the 'TriggerDetectPlayer' layer.");
                    }
                }

                foreach (var AProgressable in root.GetComponentsInChildren<AProgressable>()) {
                    const string uidRegexp = @"^[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}$";
                    if (!Regex.Match(AProgressable.uid, uidRegexp, RegexOptions.None).Success) {
                        a.Add(AProgressable, String.Format(
                                         $"Found {AProgressable.GetType().Name}.uid which is not in expected UID format (all lowercase, numbers and letters a-f, correct length, correct hyphens) UID={0}",
                                         AProgressable.uid));
                    }
                }

                foreach (var VendingMachine in root.GetComponentsInChildren<VendingMachine>()) {
                    if (VendingMachine.gameObject.layer != Layers.Enemies) {
                        a.Add(VendingMachine,
                            $"Found VendingMachine that is not on the Enemies layer, this means it cannot be kicked.");
                    }
                    var animation = VendingMachine.GetComponent<Animation>();
                    if (animation == null) {
                        a.Add(VendingMachine, $"Found VendingMachine without an Animation component.");
                    }
                    foreach (var animName in VendingMachineAnimations) {
                        // Editor says `GetState` method doesn't exist
#if BEPINEX
                        if (animation.GetState(animName) == null) {
                            a.Add(VendingMachine,
                                $"Found VendingMachine with Animation component missing animation: {animName}. This will fail with errors when kicked.");
                        }
#endif
                    }
                }

                foreach (var Teleport in root.GetComponentsInChildren<Teleport>()) {
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
                
                // TODO check for missing Sun, suggest adding Sun prefab in Editor
                // Old repair code, DID NOT WORK:
                // // Create a SunFlareGPU if the map doesn't have one
                // if (GameObject.FindObjectOfType<SunFlareGPU>() == null) {
                //     var go = new GameObject();
                //     var sunflareGpu = go.AddComponent<SunFlareGPU>();
                //     var occlusionCamera = new GameObject();
                //     occlusionCamera.transform.SetParent(go.transform);
                //     occlusionCamera.AddComponent<Camera>();
                // }
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
