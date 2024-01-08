using System.Collections.Generic;
using Reptile;

namespace MapStation.Plugin;
class StageProgresses {
    public static StageProgresses Instance;

    public Dictionary<string, StageProgress> stageProgresses = new ();

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