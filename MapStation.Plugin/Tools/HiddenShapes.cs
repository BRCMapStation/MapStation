using Reptile;

namespace MapStation.Plugin.Tools;
public static class HiddenShapes {
    private static readonly int[] DebugLayers = [
        Layers.CameraIgnore,
        Layers.TriggerDetectPlayer
    ];

    private static bool visible;
    public static bool Visible {
        get => visible;
        set {
            visible = value;
            Apply();
        }
    }

    private static GameplayCamera camera;
    public static GameplayCamera Camera {
        get => camera;
        set {
            camera = value;
            Apply();
        }
    }

    private static void Apply() {
        if (camera == null) return;
        foreach (var layer in DebugLayers) {
            if (visible) {
                camera.cam.cullingMask |= (1 << layer);
            } else {
                camera.cam.cullingMask &= ~(1 << layer);
            }
        }
    }
}
