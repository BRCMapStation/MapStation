using System.Collections.Generic;
using Reptile;
using CommonAPI;
using System.IO;

namespace MapStation.Plugin;
public class StageProgresses : CustomSaveData {
    public static StageProgresses Instance;

    public Dictionary<string, StageProgress> stageProgresses = new ();
    private const byte Version = 1;

    public StageProgresses() : base(PluginInfo.PLUGIN_GUID, "{0}.msp") {
    }

    public override void Write(BinaryWriter writer) {
        writer.Write(Version);
        writer.Write(stageProgresses.Count);
        foreach(var stageProgressKeyValues in stageProgresses) {
            var stageProgress = stageProgressKeyValues.Value;
            var internalName = stageProgressKeyValues.Key;
            writer.Write(internalName);
            stageProgress.Write(writer);
            stageProgress.WriteVersionTwo(writer);
            stageProgress.WriteVersionThree(writer);
        }
    }

    public override void Read(BinaryReader reader) {
        var version = reader.ReadByte();
        var stageProgressCount = reader.ReadInt32();
        for(var i = 0; i < stageProgressCount; i++) {

            var prefixedName = reader.ReadString();
            var internalName = StageEnum.RemovePrefixFromInternalName(prefixedName);

            var stageProgress = new StageProgress();
            stageProgress.Read(reader);
            stageProgress.ReadVersionTwo(reader);
            stageProgress.ReadVersionThree(reader);
            stageProgress.stageID = StageEnum.HashMapName(internalName);

            stageProgresses[prefixedName] = stageProgress;
        }
    }

    public override void Initialize() {
        stageProgresses.Clear();
    }

    public StageProgress GetOrCreateForStage(Stage stageId) {
        if(StageEnum.IsValidMapId(stageId)) {
            var internalName = stageId.ToString();
            if(stageProgresses.TryGetValue(internalName, out var stageProgress)) {
                return stageProgress;
            } else {
                stageProgress = new StageProgress() {
                    version = 3,
                    stageID = stageId,
                    policeAllowed = true,
                    mapFound = true,
                    taxiFound = true
                };
                stageProgresses.Add(internalName, stageProgress);
                return stageProgress;
            }
        }
        return null;
    }
}
