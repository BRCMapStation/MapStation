using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Plugin {
    public static class MapOverrides {
        public static void OnStagePreInitialization(Stage newStage, Stage previousStage) {
            OverrideQualitySettings(newStage, Core.Instance.BaseModule.user.VideoSettingsManager.cachedVideoSettings);
        }

        public static void OverrideQualitySettings(Stage stage, VideoSettings videoSettings) {
            if (MapDatabase.Instance.maps.TryGetValue(stage, out var mapDatabaseEntry)) {
                if (mapDatabaseEntry.Properties.overrideShadowDistance) {
                    QualitySettings.shadowDistance = mapDatabaseEntry.Properties.shadowDistance;
                    return;
                }
            }
            QualitySettings.shadowDistance = videoSettings.shadowDistance;
        }
    }
}
