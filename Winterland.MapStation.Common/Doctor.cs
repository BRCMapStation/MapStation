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
            }

            return problems;
        }

        public static void AnalyzeAndLog(GameObject root) {
            var results = Analyze(root);
            Debug.Log(String.Format("MapStation: Analysis of {0} found {1} problems.", root.name, results.Count));
            foreach(var result in results) {
                Debug.Log(result);
            }
        }
    }
}