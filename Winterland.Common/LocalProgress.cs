using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using UnityEngine;
using CommonAPI;

namespace Winterland.Common {
    /// <summary>
    /// Keeps track of clientside progress for Winterland.
    /// </summary>
    public class LocalProgress : ILocalProgress {
        public WinterObjective Objective { get; set; }
        private const byte Version = 0;
        private string savePath;

        // Default values for a blank save!
        public LocalProgress() {
            Objective = ObjectiveDatabase.StartingObjective;
            savePath = Path.Combine(Paths.ConfigPath, "MilleniumWinterland/localprogress.mwp");
        }

        public void Save() {
            using (var stream = new MemoryStream()) {
                using (var writer = new BinaryWriter(stream)) {
                    Write(writer);
                }
                // Enqueue async file write.
                CustomStorage.Instance.WriteFile(stream.ToArray(), savePath);
            }
        }

        public void Load() {
            if (!File.Exists(savePath)) {
                Debug.Log("No Winterland save to load, starting a new game.");
                return;
            }
            using (var stream = File.Open(savePath, FileMode.Open)) {
                using (var reader = new BinaryReader(stream)) {
                    try {
                        Read(reader);
                    }
                    catch(Exception e) {
                        Debug.LogError($"Failed to load Winterland save!{Environment.NewLine}{e}");
                    }
                }
            }
        }

        private void Write(BinaryWriter writer) {
            //version
            writer.Write(Version);
            writer.Write(Objective.name);
        }

        private void Read(BinaryReader reader) {
            var version = reader.ReadByte();
            if (version > Version) {
                Debug.LogError($"Attemped to read a Winterland save that's too new (version {version}), current version is {Version}.");
                return;
            }
            var objectiveName = reader.ReadString();
            var objective = ObjectiveDatabase.GetObjective(objectiveName);
            if (objective != null)
                Objective = objective;
            else
                Objective = ObjectiveDatabase.StartingObjective;
        }
    }
}
