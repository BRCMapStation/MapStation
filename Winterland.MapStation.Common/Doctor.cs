using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Reptile;
using UnityEngine;

namespace Winterland.MapStation.Common {
    /// <summary>
    /// Analyze a GameObject hierarchy for potential problems.
    /// </summary>
    public static class Doctor {
        public static List<string> Analyze(GameObject root) {
            var problems = new List<string>();

            foreach(var GraffitiSpot in root.GetComponentsInChildren<GraffitiSpot>()) {
                if(GraffitiSpot.dynamicRepPickup == null) {
                    problems.Add("Found GraffitiSpot.dynamicRepPickup == null. This will soft-lock when tagged.");
                }
                const string uidRegexp = @"^[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}$";
                if(!Regex.Match(GraffitiSpot.uid, uidRegexp, RegexOptions.None).Success) {
                    problems.Add(String.Format("Found GraffitiSpot.uid which is not in expected UID format (all lowercase, numbers and letters a-f, correct length, correct hyphens) UID={0}", GraffitiSpot.uid));
                }
                if(GraffitiSpot.tag != Tags.GraffitiSpot) {
                    problems.Add($"Found GraffitiSpot not tagged as 'GraffitiSpot'");
                }
                if(GraffitiSpot.gameObject.layer != Layers.TriggerDetectPlayer) {
                    problems.Add($"Found GraffitiSpot not on the 'TriggerDetectPlayer' layer.");
                }
            }

            foreach(var VendingMachine in root.GetComponentsInChildren<VendingMachine>()) {
                if(VendingMachine.gameObject.layer != Layers.Enemies) {
                    problems.Add($"Found VendingMachine that is not on the Enemies layer, this means it cannot be kicked.");
                }
                var animation = VendingMachine.GetComponent<Animation>();
                if(animation == null) {
                    problems.Add($"Found VendingMachine without an Animation component.");
                }
                foreach(var animName in VendingMachineAnimations) {
                    if(animation.GetState(animName) == null) {
                        problems.Add($"Found VendingMachine with Animation component missing animation: {animName}. This will fail with errors when kicked.");
                    }
                }
            }

            foreach(var Teleport in root.GetComponentsInChildren<Teleport>()) {
                if(Teleport.teleportTo == null) {
                    problems.Add($"Found Teleport missing a `teleportTo` destination.");
                }
                var collider = Teleport.GetComponentInChildren<BoxCollider>();
                if(collider == null) {
                    problems.Add($"Found Teleport without a Box Collider on a child GameObject.");
                }
                if(collider.tag != Tags.Teleport) {
                    problems.Add($"Found Teleporter's child collider not tagged as 'Teleport'");
                }
                if(collider.gameObject.layer != Layers.TriggerDetectPlayer) {
                    problems.Add($"Found Teleport's child collider not on the 'TriggerDetectPlayer' layer.");
                }
            }

            return problems;
        }

        private static readonly string[] VendingMachineAnimations = ["none", "shake", "emptyShake", "close_0", "drop"];

        public static void AnalyzeAndLog(GameObject root) {
            var results = Analyze(root);
            Debug.Log(String.Format("MapStation: Analysis of {0} found {1} problems.", root.name, results.Count));
            foreach(var result in results) {
                Debug.Log(result);
            }
        }
    }
}