using System.Collections.Generic;

namespace MapStation.Plugin;

class SceneNameMapper {
    public static SceneNameMapper Instance;

    public Dictionary<string, string> Names = new Dictionary<string, string>();
    public Dictionary<string, string> Paths = new Dictionary<string, string>();
}
